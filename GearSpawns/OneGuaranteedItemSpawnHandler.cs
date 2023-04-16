using GearSpawner;
using Il2Cpp;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreLockedDoors.GearSpawns
{
    internal class OneGuaranteedItemSpawnHandler : GearSpawnHandler
    {

        bool spawnedItem = false;
        int determinedItems = 0;
        int TotalSpawnPoints = 1;

        private void Reset()
        {
            spawnedItem = false;
            determinedItems = 0;
        }

        public OneGuaranteedItemSpawnHandler()
        {
            SpawnManager.OnStartSpawning += (_) => Reset();
        }

        public override bool ShouldSpawn(DifficultyLevel difficultyLevel, FirearmAvailability firearmAvailability, GearSpawnInfo gearSpawnInfo)
        {

            if (gearSpawnInfo.PrefabName.Contains("BoltCutters"))
            {
                GetTotalNumOfCutters(GameManager.m_ActiveScene);
            }
            else
            {
                GetTotalNumOfKeys(GameManager.m_ActiveScene, gearSpawnInfo);
            }


            if (spawnedItem) return false;
            else if (determinedItems == TotalSpawnPoints - 1)
            {
                spawnedItem = true;
                return true;
            }
            else
            {
                spawnedItem = RollChance(((1f + determinedItems) / TotalSpawnPoints) * 100);
                determinedItems++;
                return spawnedItem;
            }
        }

        private void GetTotalNumOfCutters(string scene)
        {
            switch (scene)
            {
                case "LakeRegion": TotalSpawnPoints = 3;
                    break;
                case "RuralRegion": TotalSpawnPoints = 2;
                    break;
                case "TracksRegion": TotalSpawnPoints = 4;
                    break;
                case "MarshRegion": TotalSpawnPoints = 2;
                    break;
                case "CoastalRegion": TotalSpawnPoints = 3;
                    break;
                case "WhalingStationRegion": TotalSpawnPoints = 5;
                    break;
                case "WhalingWarehouseA": TotalSpawnPoints = 2;
                    break;
                default: TotalSpawnPoints = 1;
                    break;
            }
        }

        private void GetTotalNumOfKeys(string scene, GearSpawnInfo gsi)
        {
            switch (scene)
            {
                case "LakeRegion":
                    TotalSpawnPoints = 5;
                    break;
                case "RuralRegion":
                    TotalSpawnPoints = CheckForMultipleKeysPerRegion(gsi);
                    break;
                case "CoastalRegion":
                    TotalSpawnPoints = 4;
                    break;
                case "AshCanyonRegion":
                    TotalSpawnPoints = 3;
                    break;
                default:
                    TotalSpawnPoints = 1;
                    break;
            }
        }

        private int CheckForMultipleKeysPerRegion(GearSpawnInfo gsi)
        {
            if (gsi.PrefabName == "GEAR_MoreLockedDoors_PV_FarmKey") return 4;
            else return 3;
        }

    }
}
