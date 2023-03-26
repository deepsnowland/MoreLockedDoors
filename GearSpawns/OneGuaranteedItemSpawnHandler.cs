using GearSpawner;
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
        const int TotalSpawnPoints = 2; //temporary value for testing

        private void Reset()
        {
            MelonLogger.Msg("Resetting...");
            spawnedItem = false;
            determinedItems = 0;
        }

        public OneGuaranteedItemSpawnHandler()
        {
            SpawnManager.OnStartSpawning += (_) => Reset();
        }

        public override bool ShouldSpawn(DifficultyLevel difficultyLevel, FirearmAvailability firearmAvailability, GearSpawnInfo gearSpawnInfo)
        {

            MelonLogger.Msg("Determining if item should spawn: {0}", spawnedItem);

            if (spawnedItem) return false;
            else if (determinedItems == TotalSpawnPoints - 1)
            {
                spawnedItem = true;
                MelonLogger.Msg("Item should spawn now");
                return true;
            }
            else
            {
                spawnedItem = RollChance((1f + determinedItems) / TotalSpawnPoints);
                MelonLogger.Msg("Item should spawn: {0}", spawnedItem);
                determinedItems++;
                return spawnedItem;
            }
        }

    }
}
