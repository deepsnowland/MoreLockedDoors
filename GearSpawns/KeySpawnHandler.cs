using GearSpawner;
using Il2Cpp;
using MelonLoader;
using MoreLockedDoors.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreLockedDoors.GearSpawns
{
    internal class KeySpawnHandler : GearSpawnHandler
    {

        public override bool ShouldSpawn(DifficultyLevel difficultyLevel, FirearmAvailability firearmAvailability, GearSpawnInfo gearSpawnInfo)
        {

            if (gearSpawnInfo.PrefabName.ToLowerInvariant().Contains("key"))
            {
                ItemSpawnManager.ChooseSpawnLocationForKey(gearSpawnInfo.PrefabName);

                string scene = GameManager.m_ActiveScene;

                SaveDataManager sdm = Implementation.sdm;

                KeySpawnSaveDataProxy sdp = sdm.LoadKeyData(gearSpawnInfo.PrefabName);

                if (sdp != null)
                {
                    if (sdp.sceneName == scene)
                    {
                        if (ItemSpawnManager.AreGearSpawnInfosEqual(sdp.gsi, gearSpawnInfo))
                        {
                            return true;
                        }
                        else return false;
                    }
                    else return false;
                }
                else
                {
                    MelonLogger.Error("No data found for key");
                    return false;
                }


            }
            else return false;
        }
    }

}

