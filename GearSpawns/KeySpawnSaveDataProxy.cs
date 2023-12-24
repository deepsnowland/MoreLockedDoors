using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2Cpp;
using GearSpawner;
using MoreLockedDoors.GearSpawns.CustomHelperClasses;


namespace MoreLockedDoors.GearSpawns
{
    internal class KeySpawnSaveDataProxy
    {

        public string sceneName { get; set; }

        public CustomGearSpawnInfo gsi { get; set; }

        public KeySpawnSaveDataProxy(string sceneName, CustomGearSpawnInfo gsi)
        {
            this.sceneName = sceneName;
            this.gsi = gsi;
        }

        public KeySpawnSaveDataProxy()
        {
        }
    }
}
