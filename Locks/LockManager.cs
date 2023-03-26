using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Il2Cpp;


namespace MoreLockedDoors.Locks
{
    internal class LockManager : MonoBehaviour
    {
        public string woodDoorLockedAudio = "Play_SndMechDoorWoodLocked01";
        public string metalChainDoorLockedAudio = "Play_SndMechDoorChainLocked01";
        public string metalDoorLockedAudio = "Play_SndMechDoorMetalLocked01";
        public bool InitializeLock(GameObject targetObj, int lockChance, string audio, string companionGUID, GearItem tool)
        {

            if(targetObj == null)
            {
                MelonLogger.Msg("Object is null");
                return false;
            }
            else
            {
                
                /*if (targetObj.GetComponent<Lock>())
                {
                    MelonLogger.Msg("Object already has component. No need to add or modify.");
                    return true;
                } */

                targetObj.AddComponent<Lock>();
                Lock addedLock = targetObj.GetComponent<Lock>();

                addedLock.m_ChanceLocked = lockChance;

                if (tool != null)
                {
                    addedLock.m_GearPrefabToForceLock = tool;
                }
                else
                {
                    MelonLogger.Msg("Tool is null!");
                }

                addedLock.m_LockedAudio = audio;
                addedLock.m_ModeFilter = GameModeFilter.Sandbox;

                addedLock.m_CompanionLockGuid = companionGUID;

                addedLock.m_ObjectGuid = targetObj.GetComponent<ObjectGuid>();

                addedLock.m_LockStateRolled = false;
                addedLock.RollLockedState();
                addedLock.m_AttemptedToOpen = false;
                addedLock.MaybeGetHoverIconsToShow();
              //SetupHoverIconsOverride(addedLock, targetObj);
                addedLock.AssignBindingOverrides();

                addedLock.MaybeUnlockDueToCompanionBeingUnlocked();
                return true;
            }
        }

        //not currently used
        public bool InitializeCustomLock(GameObject targetObj, int lockChance, string audio, string companionGUID, GearItem tool)
        {

            if (targetObj == null)
            {
                MelonLogger.Msg("Object is null");
                return false;
            }
            else
            {

                targetObj.AddComponent<CustomLock>();
                MelonLogger.Msg("Added custom lock component");
                CustomLock addedLock = targetObj.GetComponent<CustomLock>();

                addedLock.m_ChanceLocked = lockChance;

                if (tool != null)
                {
                    addedLock.m_GearPrefabToForceLock = tool;
                }
                else
                {
                    MelonLogger.Msg("Tool is null!");
                }

                addedLock.m_LockedAudio = audio;
                addedLock.m_ModeFilter = GameModeFilter.Sandbox;

                addedLock.m_CompanionLockGuid = companionGUID;
                return true;
            }
        }
        public bool RemoveLocks(GameObject targetObj)
        {

            if (targetObj == null)
            {
                MelonLogger.Msg("Object is null");
                return false;
            }
            else
            {
                foreach(var component in targetObj.GetComponents<Lock>())
                {

                    if(component == null)
                    {
                        MelonLogger.Msg("No lock component on object to remove.");
                    }
                    else
                    {
                        Destroy(component);
                        MelonLogger.Msg("Destroyed one lock component: {0}", component.m_GearPrefabToForceLock.name);
                    }
                }
                return true;
            }
        }

        //not currently used
        public void SetupHoverIconsOverride(Lock lck, GameObject obj)
        {
            lck.m_HoverIcons = obj.GetComponent<HoverIconsToShow>();
            MelonLogger.Msg("Hover Icons: {0}", lck.m_HoverIcons);
            if (lck.m_HoverIcons == null)
            {
                MelonLogger.Msg("Icons are null, working...");
                lck.m_HoverIcons = obj.AddComponent<HoverIconsToShow>();
                MelonLogger.Msg("Adding component to object and setting lock icons variable");
                lck.m_HoverIcons.m_HoverIcons = new HoverIconsToShow.HoverIcons[1] { HoverIconsToShow.HoverIcons.Locked };
                MelonLogger.Msg("Setting locked icon");
                return;
            }
            List<HoverIconsToShow.HoverIcons> list = new List<HoverIconsToShow.HoverIcons>(lck.m_HoverIcons.m_HoverIcons);
            if (!list.Contains(HoverIconsToShow.HoverIcons.Locked))
            {
                list.Add(HoverIconsToShow.HoverIcons.Locked);
            }
            lck.m_HoverIcons.m_HoverIcons = list.ToArray();
            MelonLogger.Msg("Setting lock icons variable to the array of icons.");


        }

    }
}
