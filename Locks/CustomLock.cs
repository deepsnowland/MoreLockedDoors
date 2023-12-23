using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Il2Cpp;
using Object = UnityEngine.Object;
using MoreLockedDoors.Utils;
using Random = UnityEngine.Random;
using System.Text.Json;
using MelonLoader;

namespace MoreLockedDoors.Locks
{
    [RegisterTypeInIl2Cpp]
    internal class CustomLock : MonoBehaviour
    {
        private ObjectGuid m_GUID;

        private LockState m_LockState;

        private bool m_CheckedForPair;
        private string m_Pair;

        private bool m_AttemptedToOpen;

        private List<string> m_ItemsUsedToForceLock;
        private GearItem m_GearItemUsedToUnlock;

        private float m_ForceLockDurationSecondsMin = 3f;
        private float m_ForceLockDurationSecondsMax = 4f;

        public string m_LockedAudio;
        public string m_UnlockAudio;

        private uint m_ForceLockAudioID;

        private bool m_IsBeingInteractedWith;
        private float m_InteractionTimer;

        private float m_RandomFailureTime;
        private bool m_BreakOnUse;

        private bool m_UseHoverTextColour;

        private HoverIconsToShow m_HoverIcons;

        private PlayerControlMode m_RestoreControlMode;

        public void Awake()
        {
            m_GUID = base.GetComponent<ObjectGuid>();
            MelonLogger.Msg("Awakening... GUID is: {0}", m_GUID.PDID);
            LoadData();
            m_AttemptedToOpen = false;
            MaybeGetHoverIcons();
        }
        public void Start()
        {
            this.m_CheckedForPair = false;
        }

        public void Update()
        {
            if (GameManager.m_IsPaused)
            {
                return;
            }

            if (!this.m_CheckedForPair)
            {
                this.MaybeUnlockDueToPairBeingUnlocked();
                this.m_CheckedForPair = true;
            }

            if (this.m_HoverIcons != null && !this.IsLocked())
            {
                Object.Destroy(this.m_HoverIcons);
                this.m_HoverIcons = null;
            }

            if (m_IsBeingInteractedWith)
            {
                if (GameManager.GetPlayerManagerComponent().GetControlMode() != PlayerControlMode.Locked) GameManager.GetPlayerManagerComponent().SetControlMode(PlayerControlMode.Locked);

                m_InteractionTimer -= Time.deltaTime;
                if (m_InteractionTimer <= 0f)
                {
                    Cancel();
                    ForceLockComplete(true, false, 1f);
                    return;
                }
                else
                {
                    //check if player is not holding down LMB
                    if (!Input.GetMouseButton(0)) Cancel();
                }


                if (GameManager.GetPlayerManagerComponent().GetControlMode() == PlayerControlMode.Struggle)
                {
                    Cancel();
                }
            }

        }

        public void LoadData()
        {
            SaveDataManager sdm = Implementation.sdm;

            CustomLockSaveDataProxy sdp = sdm.LoadLockData(m_GUID.PDID);

            m_LockState = sdp.m_LockState;
            m_ItemsUsedToForceLock = sdp.m_ItemsUsedToForceLock;
            m_LockedAudio = sdp.m_LockedAudio;
            m_Pair = sdp.m_Pair;

            //there will always be data!
            MaybeGetHoverIcons();
        }

        public void SaveData()
        {

            SaveDataManager sdm = Implementation.sdm;

            CustomLockSaveDataProxy sdp = new CustomLockSaveDataProxy(m_LockState, m_ItemsUsedToForceLock, m_LockedAudio, m_Pair);

            string dataToSave = JsonSerializer.Serialize(sdp);
            sdm.Save(dataToSave, m_GUID.PDID);
        }

        public static LockState RollLockedState(int chance)
        {
            if (Il2Cpp.Utils.RollChance(chance))
            {
                return LockState.Locked;
            }
            else
            {
                return LockState.Unlocked;
            }
        }

        public void LockOrUnlock(LockState locked)
        {
            m_LockState = locked;
        }

        public bool IsLocked()
        {
            return m_LockState == LockState.Locked;
        }

        public void MaybeUnlockDueToPairBeingUnlocked()
        {
            if (m_GUID == null || m_LockState == LockState.Broken) return;

            SaveDataManager sdm = Implementation.sdm;

            //check for pair lock here
            CustomLockSaveDataProxy? pair = sdm.LoadLockData(m_Pair);

            //this ensures that the lock's pair gets saved if it hasn't been loaded yet so when it does activate for the first time, it's loaded accurately
            if (pair == null)
            {
                pair = new CustomLockSaveDataProxy(m_LockState, m_ItemsUsedToForceLock, m_LockedAudio, m_GUID.PDID);
                string dataToSave = JsonSerializer.Serialize(pair);
                sdm.Save(dataToSave, m_Pair);
                return;
            }

            if (pair != null && pair.m_LockState == LockState.Unlocked || pair.m_LockState == LockState.Broken)
            {
                LockOrUnlock(LockState.Unlocked);
                SaveData();
            }
        }

        //for when a lock is unlocked by the player
        public void UnlockPair()
        {
            if (m_Pair == null) return;

            SaveDataManager sdm = Implementation.sdm;

            CustomLockSaveDataProxy pair = sdm.LoadLockData(m_Pair) != null ? sdm.LoadLockData(m_Pair) : new CustomLockSaveDataProxy(LockState.Broken, m_ItemsUsedToForceLock, m_LockedAudio, m_GUID.PDID); 

            pair.m_LockState = LockState.Broken;
            string dataToSave = JsonSerializer.Serialize(pair);
            sdm.Save(dataToSave, m_Pair);
        }

        private void ForceLockComplete(bool success, bool cancel, float progress)
        {
            if (success)
            {
                this.LockOrUnlock(LockState.Broken);
                //do delegate whatever that is
                this.UnlockPair();
            }
            if (this.m_GearItemUsedToUnlock.m_DegradeOnUse)
            {
                this.m_GearItemUsedToUnlock.Degrade(this.m_GearItemUsedToUnlock.m_DegradeOnUse.m_DegradeHP);
            }
            if (success && Mathf.Approximately(progress, 1f))
            {
                this.PlayUnlockAudio();
            }
        }
        public void Cancel()
        {
            if (this.m_ForceLockAudioID != 0U)
            {
                AkSoundEngine.StopPlayingID(this.m_ForceLockAudioID, Systems.GetEngineSystems().GetComponent<GameAudioManager>().m_StopAudioFadeOutMicroseconds);
                this.m_ForceLockAudioID = 0U;
            }
            
            this.m_IsBeingInteractedWith = false;
            this.m_InteractionTimer = 0f;
            if (GameManager.GetPlayerManagerComponent().GetControlMode() == PlayerControlMode.Locked)
            {
                GameManager.GetPlayerManagerComponent().SetControlMode(m_RestoreControlMode);
            }
            InterfaceManager.GetPanel<Panel_HUD>().CancelItemProgressBar();
        }

        public void UnlockBegin()
        {

            this.m_GearItemUsedToUnlock = this.ChooseGearItemToUnlock();
            SetDurationToUnlockBasedOnItemUsed();

            if (!this.m_GearItemUsedToUnlock)
            {
                this.PlayLockedAudio();
                HUDMessage.AddMessage("DEBUG: Don't have item to unlock door!");
                return;
            }
            if (this.m_GearItemUsedToUnlock.m_ForceLockItem == null)
            {
                this.PlayLockedAudio();
                HUDMessage.AddMessage("DEBUG: Item used to unlock doesn't have force unlock item component on it!");
                return;
            }
            this.StartInteract();
        }

        private void PlayUnlockAudio()
        {
            if (string.IsNullOrEmpty(this.m_UnlockAudio))
            {
                return;
            }
            GameAudioManager.PlaySound(this.m_UnlockAudio, Systems.GetEngineSystems());
        }

        private void PlayLockedAudio()
        {
            if (string.IsNullOrEmpty(this.m_LockedAudio))
            {
                GameAudioManager.PlayGUIError();
                return;
            }
            GameAudioManager.PlaySound(this.m_LockedAudio, Systems.GetEngineSystems());
        }

        public GearItem GetRequiredGearToUnlock(string selection = "")
        {
            //check inventory for any of the gear items and returns true if found

            GearItem highestConditionGearThatMatchesName = null;

            foreach (var giStr in m_ItemsUsedToForceLock)
            {
                highestConditionGearThatMatchesName = GameManager.GetInventoryComponent().GetHighestConditionGearThatMatchesName(giStr);
                if (highestConditionGearThatMatchesName == null) continue;
                if (m_ItemsUsedToForceLock.Count > 1 && (highestConditionGearThatMatchesName.name.ToLowerInvariant().Contains("key") || highestConditionGearThatMatchesName.name.ToLowerInvariant().Contains(selection))) break;
            }
            return highestConditionGearThatMatchesName;
        }
        private GearItem ChooseGearItemToUnlock()
        {
            //if lock only needs 1 item and player has it, choose that one
            if (m_ItemsUsedToForceLock.Count == 1) return GetRequiredGearToUnlock();

            //otherwise open selection UI or something else

            return GetRequiredGearToUnlock();
        }

        public void StartInteract()
        {

            if (this.m_GearItemUsedToUnlock.m_ForceLockItem.m_ForceLockAudio.Length > 0)
            {
                this.m_ForceLockAudioID = GameAudioManager.PlaySound(this.m_GearItemUsedToUnlock.m_ForceLockItem.m_ForceLockAudio, base.gameObject);
            }
            this.m_RestoreControlMode = GameManager.GetPlayerManagerComponent().GetControlMode();
            this.m_IsBeingInteractedWith = true;
            float num = Random.Range(this.m_ForceLockDurationSecondsMin, this.m_ForceLockDurationSecondsMax);
            this.m_InteractionTimer = num;
            GameManager.GetPlayerManagerComponent().SetControlMode(PlayerControlMode.Locked);
            this.m_RandomFailureTime = 0f;
            if (this.m_GearItemUsedToUnlock.CheckForBreakOnUse())
            {
                this.m_RandomFailureTime = Random.Range(0.1f, 0.8f) * num;
                this.m_BreakOnUse = true;
            }
            else
            {
                this.m_BreakOnUse = false;
            }
            string progressLabel = "";
            if (!string.IsNullOrEmpty(this.m_GearItemUsedToUnlock.m_ForceLockItem.m_LocalizedProgressText.m_LocalizationID))
            {
                progressLabel = this.m_GearItemUsedToUnlock.m_ForceLockItem.m_LocalizedProgressText.Text();
            }

            InterfaceManager.GetPanel<Panel_HUD>().StartItemProgressBar(this.m_InteractionTimer, progressLabel, null, new Action(this.ProgressBarStarted));
            return;

        }

        public void ProgressBarStarted()
        {
        }
        private void SetDurationToUnlockBasedOnItemUsed()
        {
            if (m_GearItemUsedToUnlock == null) return;
            else if (m_GearItemUsedToUnlock.name.ToLowerInvariant().Contains("key"))
            {
                m_ForceLockDurationSecondsMin = 2f;
                m_ForceLockDurationSecondsMax = 3f;
            }
            else if (m_GearItemUsedToUnlock.name.ToLowerInvariant().Contains("hatchet"))
            {
                m_ForceLockDurationSecondsMin = 5f;
                m_ForceLockDurationSecondsMax = 7f;
            }
            else if (m_GearItemUsedToUnlock.name.ToLowerInvariant().Contains("hacksaw"))
            {
                m_ForceLockDurationSecondsMin = 8f;
                m_ForceLockDurationSecondsMax = 9f;
            }
        }
        private void MaybeGetHoverIcons()
        {
            if (this.m_HoverIcons)
            {
                return;
            }

            base.TryGetComponent<HoverIconsToShow>(out this.m_HoverIcons);
            if (this.m_HoverIcons == null)
            {
                this.m_HoverIcons = base.gameObject.AddComponent<HoverIconsToShow>();
                this.m_HoverIcons.m_HoverIcons = new HoverIconsToShow.HoverIcons[]
                {
                HoverIconsToShow.HoverIcons.Locked
                };
                return;
            }
            List<HoverIconsToShow.HoverIcons> list = new List<HoverIconsToShow.HoverIcons>(this.m_HoverIcons.m_HoverIcons);
            if (!list.Contains(HoverIconsToShow.HoverIcons.Locked))
            {
                list.Add(HoverIconsToShow.HoverIcons.Locked);
            }
            this.m_HoverIcons.m_HoverIcons = list.ToArray();
        }


    }
}
