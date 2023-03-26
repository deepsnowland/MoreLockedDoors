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
        public static void InitializeItemSpawns()
        {

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MoreLockedDoors.GearSpawns.GearSpawns.txt";
            string data = "";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                data = reader.ReadToEnd();
            }


            MelonLogger.Msg("Gear Spawn data: {0}", data);

            SpawnManager.ParseSpawnInformation(data);

        }
        public static void InitializeCustomHandler()
        {
            SpawnTagManager.AddHandler("morelockeddoors_randomunlockitems", new OneGuaranteedItemSpawnHandler());
        }
    }
}
