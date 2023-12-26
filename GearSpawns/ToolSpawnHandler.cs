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
    internal class ToolSpawnHandler : GearSpawnHandler
    {

        public override bool ShouldSpawn(DifficultyLevel difficultyLevel, FirearmAvailability firearmAvailability, GearSpawnInfo gearSpawnInfo)
        {
            if (gearSpawnInfo.PrefabName.ToLowerInvariant().Contains("boltcutters"))
            {
                //this should only be called once 
                ItemSpawnManager.ChooseSpawnLocationsForBoltcutters();

                string scene = GameManager.m_ActiveScene;

                SaveDataManager sdm = Implementation.sdm;

                List<SpawnSaveDataProxy> list = sdm.LoadBoltcuttersList();

                if (list != null)
                {
                    foreach(SpawnSaveDataProxy sp in list)
                    {
                        if (sp.sceneName != scene) continue;

                        CustomGearSpawnInfo cgsi = sp.gsi;
                        Vector3 pos = new Vector3(cgsi.Position.x, cgsi.Position.y, cgsi.Position.z);
                        Quaternion rot = new Quaternion(cgsi.Rotation.x, cgsi.Rotation.y, cgsi.Rotation.z, cgsi.Rotation.w);
                        GearSpawnInfo gsi = new GearSpawnInfo(cgsi.Tag, pos, cgsi.PrefabName, rot, cgsi.SpawnChance);

                        if (ItemSpawnManager.AreGearSpawnInfosEqual(gsi, gearSpawnInfo))
                        {
                            return true;
                        }
                    }

                    //if you have not returned true at this point, then the item spawn has not been found in the list which means it shouldn't spawn
                    return false;
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

