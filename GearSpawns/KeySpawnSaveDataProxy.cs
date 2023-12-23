using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2Cpp;
using GearSpawner;


namespace MoreLockedDoors.GearSpawns
{
    internal class KeySpawnSaveDataProxy
    {

        public string sceneName { get; set; }
        public GearSpawnInfo gsi { get; set; }

        public KeySpawnSaveDataProxy(string sceneName, GearSpawnInfo gsi)
        {
            this.sceneName = sceneName;
            this.gsi = gsi;
        }

        public KeySpawnSaveDataProxy()
        {
        }
    }
}
