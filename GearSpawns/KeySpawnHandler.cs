using GearSpawner;
using Il2Cpp;
using MelonLoader;
using MoreLockedDoors.GearSpawns.CustomHelperClasses;
using MoreLockedDoors.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreLockedDoors.GearSpawns
{
    internal class KeySpawnHandler : GearSpawnHandler
    {

        public override bool ShouldSpawn(DifficultyLevel difficultyLevel, FirearmAvailability firearmAvailability, GearSpawnInfo gearSpawnInfo)
        {
            if (gearSpawnInfo.PrefabName.ToLowerInvariant().Contains("key"))
            {
                //this should only be called once per scene, since if it finds data is saved it returns. Otherwise it saves data.
                ItemSpawnManager.ChooseSpawnLocationForKey(gearSpawnInfo.PrefabName);

                string scene = GameManager.m_ActiveScene;

                SaveDataManager sdm = Implementation.sdm;

                SpawnSaveDataProxy sdp = sdm.LoadKeyData(gearSpawnInfo.PrefabName);

                if (sdp != null)
                {
                    if (sdp.sceneName == scene)
                    {

                        CustomGearSpawnInfo cgsi = sdp.gsi;
                        Vector3 pos = new Vector3(cgsi.Position.x, cgsi.Position.y, cgsi.Position.z);
                        Quaternion rot = new Quaternion(cgsi.Rotation.x, cgsi.Rotation.y, cgsi.Rotation.z, cgsi.Rotation.w);
                        GearSpawnInfo gsi = new GearSpawnInfo(cgsi.Tag, pos, cgsi.PrefabName, rot, cgsi.SpawnChance);

                        if (ItemSpawnManager.AreGearSpawnInfosEqual(gsi, gearSpawnInfo))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else return false;
                }
                else
                {
                    return false;
                }


            }
            else return false;
        }
    }

}

