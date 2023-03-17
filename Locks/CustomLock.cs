using Il2Cpp;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;
using RangeAttribute = System.ComponentModel.DataAnnotations.RangeAttribute;
using ModData;

namespace MoreLockedDoors.Locks
{
    [RegisterTypeInIl2Cpp]
    internal class CustomLock : MonoBehaviour
    {

        public delegate void OnLockStateBroken();

        public GameModeFilter m_ModeFilter;

        [Range(0f, 100f)]
        public float m_ChanceLocked;

        [Range(0f, 10f)]
        public float m_ForceLockDurationSecondsMin = 3f;

        [Range(0f, 10f)]
        public float m_ForceLockDurationSecondsMax = 4f;

        [Range(0f, 100f)]
        public float m_ChanceContainerRuinedWhenToolBreaks = 50f;

        public GearItem m_GearPrefabToForceLock;

        public string m_CompanionLockGuid;

        public string m_LockedAudio;

        public string m_UnlockAudio;

        //"Use the custom Hover Text Lock/Unlock colors"
        public bool m_UseHoverTextColor;

        //"Hide Tool Required To Force Open Hint Text"
        public bool m_HideToolRequiredToForceOpenHintText;

        //"Animation"
        public AnimatedInteraction m_AnimatedInteraction;

        public Animator m_AnimatedObjectAnimator;

        public string m_AnimatedObjectTrackName;

        public static List<string> m_UnlockedCompanionGuids = new List<string>();

        private GearItem m_GearUsedToForceLock;

        private LockState m_LockState;

        private bool m_AttemptedToOpen;

        private bool m_LockStateRolled;

        private bool m_BreakOnUse;

        private bool m_CheckedForUnlockedCompanion;

        private ObjectGuid m_ObjectGuid;

        private HoverIconsToShow m_HoverIcons;

        private OnLockStateBroken m_OnLockStateBroken;

        private bool m_IsBeingInteractedWith;

        private float m_InteractTimer;

        private uint m_ForceLockAudioID;

        private PlayerControlMode m_RestoreControlMode;

        private float m_RandomFailureTime;

        private bool m_WasEverLocked;

        private static CustomLockSaveDataProxy m_LockSaveDataProxy = new CustomLockSaveDataProxy();

        private static ModDataManager m_UnlockedCompanionSaveDataManager = new ModDataManager("MoreLockedDoors");

        private static ModDataManager m_LockSaveDataManager = new ModDataManager("MoreLockedDoors");
        public CustomLock(IntPtr ptr) : base(ptr) { }

        public void Awake()
        {
            AssignBindingOverrides();
        }

        public void Start()
        {
            RollLockedState();
            m_AttemptedToOpen = false;
            MaybeGetHoverIconsToShow();
            m_ObjectGuid = GetComponent<ObjectGuid>();
            m_CheckedForUnlockedCompanion = false;
        }
        public void Update()
        {
            if (GameManager.m_IsPaused)
            {
                return;
            }
            if (m_LockState == LockState.Locked)
            {
                m_WasEverLocked = true;
            }
            if (!m_CheckedForUnlockedCompanion)
            {
                MaybeUnlockDueToCompanionBeingUnlocked();
                m_CheckedForUnlockedCompanion = true;
            }
            if (m_HoverIcons != null && !IsLocked())
            {
                Destroy(m_HoverIcons);
                m_HoverIcons = null;
            }
            if (m_IsBeingInteractedWith)
            {
                m_InteractTimer -= Time.deltaTime;
                if (m_BreakOnUse && m_InteractTimer < m_RandomFailureTime)
                {
                    Cancel();
                    OnForceLockComplete(success: false, playerCancel: false, 0f);
                }
                else if (m_InteractTimer <= 0f)
                {
                    Cancel();
                    OnForceLockComplete(success: true, playerCancel: false, 1f);
                }
                else if (GameManager.GetPlayerManagerComponent().GetControlMode() == PlayerControlMode.Struggle)
                {
                    Cancel();
                }
            }
        }
        public void Cancel()
        {
            if (m_ForceLockAudioID != 0)
            {
                AkSoundEngine.StopPlayingID(m_ForceLockAudioID, GameAudioManager.Instance.m_StopAudioFadeOutMicroseconds);
                m_ForceLockAudioID = 0u;
            }
            if (GameManager.GetPlayerManagerComponent().GetControlMode() == PlayerControlMode.Locked)
            {
                GameManager.GetPlayerManagerComponent().SetControlMode(m_RestoreControlMode);
            }
            m_IsBeingInteractedWith = false;
            m_InteractTimer = 0f;
            //GameManager.GetPlayerManagerComponent().m_ForceLockInProgress = null;
            InterfaceManager.GetPanel<Panel_HUD>().CancelItemProgressBar();
        }
        public void RollLockedState()
        {
            if (!m_LockStateRolled)
            {
                if (Il2Cpp.Utils.RollChance(m_ChanceLocked))
                {
                    m_LockState = LockState.Locked;
                }
                else
                {
                    m_LockState = LockState.Unlocked;
                }
                m_LockStateRolled = true;
            }
        }
        public bool IsLocked()
        {
            if (m_LockState == LockState.Locked)
            {
                return true;
            }
            return false;
        }
        public void SetLockState(LockState state)
        {
            m_LockState = state;
        }
        public bool WasEverLocked()
        {
            return m_WasEverLocked;
        }
        public bool IsBroken()
        {
            if (m_LockState == LockState.Broken)
            {
                return true;
            }
            return false;
        }
        public bool AttemptedToOpen()
        {
            return m_AttemptedToOpen;
        }
        public bool RequiresToolToUnlock()
        {
            return m_GearPrefabToForceLock != null;
        }
        public bool PlayerHasRequiredToolToUnlock()
        {
            return GetGearItemToForceLock() != null;
        }
        public GearItem GetGearItemToForceLock()
        {
            if (m_GearPrefabToForceLock == null)
            {
                return null;
            }
            GearItem highestConditionGearThatMatchesName = GameManager.GetInventoryComponent().GetHighestConditionGearThatMatchesName((m_GearPrefabToForceLock).name);
            if (!highestConditionGearThatMatchesName)
            {
                AlternateTools component = ((Component)m_GearPrefabToForceLock).GetComponent<AlternateTools>();
                if (component)
                {
                    for (int i = 0; i < component.m_AlternateToolsList.Length; i++)
                    {
                        if (component.m_AlternateToolsList[i])
                        {
                            highestConditionGearThatMatchesName = GameManager.GetInventoryComponent().GetHighestConditionGearThatMatchesName((component.m_AlternateToolsList[i]).name);
                            if (highestConditionGearThatMatchesName != null)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return highestConditionGearThatMatchesName;
        }
        public void ForceLockBegin(OnLockStateBroken onLockStateBroken)
        {
            if (m_GearPrefabToForceLock == null)
            {
                PlayLockedAudio();
                return;
            }
            m_OnLockStateBroken = onLockStateBroken;
            m_GearUsedToForceLock = GetGearItemToForceLock();
            if (!m_GearUsedToForceLock)
            {
                PlayLockedAudio();
                if (!m_HideToolRequiredToForceOpenHintText)
                {
                    HUDMessage.AddMessage(Localization.Get("GAMEPLAY_ToolRequiredToForceOpen").Replace("{item-name}", m_GearPrefabToForceLock.DisplayName));
                }
            }
            else if (m_GearUsedToForceLock.m_ForceLockItem == null)
            {
                PlayLockedAudio();
            }
            else
            {
                StartInteract();
            }
        }
        private void OnForceLockComplete(bool success, bool playerCancel, float progress)
        {
            Container component = this.GetComponent<Container>();
            if (success)
            {
                SetLockState(LockState.Broken);
                if (m_OnLockStateBroken != null)
                {
                    m_OnLockStateBroken();
                }
                UnlockCompanionLock();
            }
            if (m_BreakOnUse)
            {
                m_GearUsedToForceLock.BreakOnUse();
                if (component && Il2Cpp.Utils.RollChance(50f))
                {
                    component.MakeCanNeverBeOpened();
                }
            }
            else if (m_GearUsedToForceLock.m_DegradeOnUse)
            {
                m_GearUsedToForceLock.Degrade(m_GearUsedToForceLock.m_DegradeOnUse.m_DegradeHP);
            }
            if (success && Mathf.Approximately(progress, 1f))
            {
                PlayUnlockAudio();
            }
        }
        public void StartInteract()
        {
            if (m_AnimatedInteraction)
            {
                InterfaceManager.GetPanel<Panel_HUD>().HideHudElements(hide: true);
                InterfaceManager.GetPanel<Panel_HUD>().HideReticle(hide: true);
                m_AnimatedInteraction.StartAnimatedInteraction((Il2CppSystem.Action)StartInteractInternal, forceInteraction: true, shouldStowItemInHands: true);
            }
            else
            {
                StartInteractInternal();
            }
        }
        public void StartInteractInternal()
        {
            if (m_GearUsedToForceLock.m_ForceLockItem.m_ForceLockAudio.Length > 0)
            {
                m_ForceLockAudioID = GameAudioManager.PlaySound(m_GearUsedToForceLock.m_ForceLockItem.m_ForceLockAudio, base.gameObject);
            }
            m_RestoreControlMode = GameManager.GetPlayerManagerComponent().GetControlMode();
            m_IsBeingInteractedWith = true;
            float num = (m_InteractTimer = Random.Range(m_ForceLockDurationSecondsMin, m_ForceLockDurationSecondsMax));
            //GameManager.GetPlayerManagerComponent().m_ForceLockInProgress = this;
            GameManager.GetPlayerManagerComponent().SetControlMode(PlayerControlMode.Locked);
            m_RandomFailureTime = 0f;
            if (m_GearUsedToForceLock.CheckForBreakOnUse())
            {
                m_RandomFailureTime = Random.Range(0.1f, 0.8f) * num;
                m_BreakOnUse = true;
            }
            else
            {
                m_BreakOnUse = false;
            }
            string progressLabel = "";
            if (!string.IsNullOrEmpty(m_GearUsedToForceLock.m_ForceLockItem.m_LocalizedProgressText.m_LocalizationID))
            {
                progressLabel = m_GearUsedToForceLock.m_ForceLockItem.m_LocalizedProgressText.Text();
            }
            if (!m_AnimatedInteraction)
            {
                InterfaceManager.GetPanel<Panel_HUD>().StartItemProgressBar(m_InteractTimer, progressLabel, null, (Il2CppSystem.Action)ProgressBarStarted);
                return;
            }
            InterfaceManager.GetPanel<Panel_HUD>().HideHudElements(hide: false);
            InterfaceManager.GetPanel<Panel_HUD>().HideReticle(hide: false);
            ProgressBarStarted();
         //   Cancel();
            OnForceLockComplete(success: true, playerCancel: false, 1f);
        }
        public void PlayLockedAudio()
        {
            if (string.IsNullOrEmpty(m_LockedAudio))
            {
                GameAudioManager.PlayGUIError();
            }
            else
            {
                GameAudioManager.PlaySound(m_LockedAudio, GameAudioManager.Instance.gameObject);
            }
        }
        public void PlayUnlockAudio()
        {
            if (!string.IsNullOrEmpty(m_UnlockAudio))
            {
                GameAudioManager.PlaySound(m_UnlockAudio, GameAudioManager.Instance.gameObject);
            }
        }
        public void UnlockCompanionLock()
        {
            if (!string.IsNullOrEmpty(m_CompanionLockGuid) && !m_UnlockedCompanionGuids.Contains(m_CompanionLockGuid))
            {
                m_UnlockedCompanionGuids.Add(m_CompanionLockGuid);
            }
        }
        private void MaybeUnlockDueToCompanionBeingUnlocked()
        {
            if (!(m_ObjectGuid == null) && m_UnlockedCompanionGuids.Contains(m_ObjectGuid.Get()))
            {
                SetLockState(LockState.Unlocked);
                m_UnlockedCompanionGuids.Remove(m_ObjectGuid.Get());
            }
        }
        private void AssignBindingOverrides()
        {
            if ((bool)m_AnimatedInteraction && (bool)m_AnimatedObjectAnimator && !string.IsNullOrEmpty(m_AnimatedObjectTrackName))
            {
                TLD_TimelineDirector.BindingInfo bindingInfo = new TLD_TimelineDirector.BindingInfo();
                bindingInfo.m_OverrideType = TLD_TimelineDirector.BindingInfo.OverrideType.TrackBinding;
                m_AnimatedInteraction.m_TimelineBindingOverrides.Add(bindingInfo);
                bindingInfo.m_Name = m_AnimatedObjectTrackName;
                bindingInfo.m_BindingType = TLD_TimelineDirector.BindingInfo.BindingInfoType.GameObject;
                bindingInfo.m_GameObject = m_AnimatedObjectAnimator.gameObject;
            }
        }
        private void MaybeGetHoverIconsToShow()
        {
            if ((bool)m_HoverIcons)
            {
                return;
            }
            TryGetComponent<HoverIconsToShow>(out m_HoverIcons);
            if (m_HoverIcons == null)
            {
                m_HoverIcons = base.gameObject.AddComponent<HoverIconsToShow>();
                m_HoverIcons.m_HoverIcons = new HoverIconsToShow.HoverIcons[1] { HoverIconsToShow.HoverIcons.Locked };
                return;
            }
            List<HoverIconsToShow.HoverIcons> list = new List<HoverIconsToShow.HoverIcons>(m_HoverIcons.m_HoverIcons);
            if (!list.Contains(HoverIconsToShow.HoverIcons.Locked))
            {
                list.Add(HoverIconsToShow.HoverIcons.Locked);
            }
            m_HoverIcons.m_HoverIcons = list.ToArray();
        }
        public void Serialize()
        {
            m_LockSaveDataProxy.m_LockStateProxy = m_LockState;
            m_LockSaveDataProxy.m_AttemptedToOpen = m_AttemptedToOpen;

            //unique identifier for which lock it applies to
            string suffix = m_ObjectGuid.PDID;

            string data = JsonSerializer.Serialize(m_LockSaveDataProxy);

            m_LockSaveDataManager.Save(data, m_ObjectGuid.PDID);

        }
        public void Deserialize(string guid)
        {
            if (guid != null)
            {

                string? data = m_LockSaveDataManager.Load(guid);

                CustomLockSaveDataProxy? lockSaveDataProxy = JsonSerializer.Deserialize<CustomLockSaveDataProxy>(data);

                if (lockSaveDataProxy == null)
                {
                    MelonLogger.Msg("Deserialized data is null");
                    return;
                }

                m_LockState = lockSaveDataProxy.m_LockStateProxy;
                m_AttemptedToOpen = lockSaveDataProxy.m_AttemptedToOpen;
                MaybeGetHoverIconsToShow();
            }
        }
        public static void SerializeUnlockedCompanionGuids()
        {
            if (m_UnlockedCompanionGuids.Count == 0)
            {
                return;
            }

            m_UnlockedCompanionSaveDataManager.Save(string.Join(",", m_UnlockedCompanionGuids));

        }
        public static void DeserializeUnlockedCompanionGuids()
        {
            m_UnlockedCompanionGuids.Clear();

            string? data = m_UnlockedCompanionSaveDataManager.Load();

            if(data == null)
            {
                MelonLogger.Msg("Deserialized data is null");
                return;
            }

            m_UnlockedCompanionGuids = data.Split(",").ToList();
            MelonLogger.Msg("Deserialized data is {0}", m_UnlockedCompanionGuids.ToArray().ToString());

        }

        public void ProgressBarStarted()
        {
        }

    }

   



}
