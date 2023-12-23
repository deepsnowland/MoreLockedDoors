using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using UnityEngine;
using Il2Cpp;
using MoreLockedDoors.Utils;
using Il2CppNewtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MoreLockedDoors.Locks
{
    internal class LockManager : MonoBehaviour
    {
        public string woodDoorLockedAudio = "Play_SndMechDoorWoodLocked01";
        public string metalChainDoorLockedAudio = "Play_SndMechDoorChainLocked01";
        public string metalDoorLockedAudio = "Play_SndMechDoorMetalLocked01";

        SaveDataManager sdm = Implementation.sdm;

        public void InitializeCustomLock(GameObject targetObj, ref int lockChance, string audio, string pair, List<string> tools)
        {

            MelonLogger.Msg("Items to unlock count: {0}", tools.Count);

            if (targetObj == null)
            {
                MelonLogger.Msg("Object is null");
                return;
            }
            else
            {

                LockState lockState = CustomLock.RollLockedState(lockChance);
                if (lockState == LockState.Locked) lockChance = 100;
                else if (lockState == LockState.Unlocked) lockChance = 0;

                ObjectGuid guid = targetObj.GetComponent<ObjectGuid>();

                //if data has not been saved for this lock yet, save a new copy so when the lock awakens it loads the data. Otherwise, save the existing copy because why not, it will still load it on awake.
                CustomLockSaveDataProxy sdp = sdm.LoadLockData(guid.PDID) != null ? sdm.LoadLockData(guid.PDID) : new CustomLockSaveDataProxy(lockState, tools, audio, pair);

                string dataToSave = JsonSerializer.Serialize(sdp);
                sdm.Save(dataToSave, guid.PDID);

                targetObj.AddComponent<CustomLock>();
            }
        }
        
        //not currently used
        public bool RemoveLocks(GameObject targetObj)
        {

            if (targetObj == null)
            {
                MelonLogger.Msg("Object is null");
                return false;
            }
            else
            {
                foreach (var component in targetObj.GetComponents<Lock>())
                {

                    if (component == null)
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


    }
}
