using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using GearSpawner;
using Il2CppNodeCanvas.Tasks.Actions;

namespace MoreLockedDoors.GearSpawns
{
    internal class ItemSpawnManager
    {
        public static void InitializeCustomHandler()
        {
            SpawnTagManager.AddHandler("morelockeddoors_randomunlockitems", new OneGuaranteedItemSpawnHandler());
            SpawnTagManager.AddHandler("morelockeddoors_randomkeys", new OneGuaranteedItemSpawnHandler());
            SpawnTagManager.AddHandler("morelockeddoors_farmhousekey", new OneGuaranteedItemSpawnHandler());

        }
    }
}
