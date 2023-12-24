﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MelonLoader;
using ModData;
using MoreLockedDoors.Locks;
using MoreLockedDoors.GearSpawns;
using GearSpawner;

namespace MoreLockedDoors.Utils
{
    internal class SaveDataManager
    {

        ModDataManager dm = new ModDataManager("More Locked Doors", false);
        public void Save(string data, string suffix)
        {
            dm.Save(data, suffix);
        }

        public CustomLockSaveDataProxy LoadLockData(string suffix)
        {
            string? data = dm.Load(suffix);

            if (data is null) return null;
            
            CustomLockSaveDataProxy sdp = JsonSerializer.Deserialize<CustomLockSaveDataProxy>(data);

            return sdp;
        }

        public void SaveKeyData(KeySpawnSaveDataProxy data, string keyName)
        {

            if(data != null)
            {
                string dataToSave = JsonSerializer.Serialize<KeySpawnSaveDataProxy>(data);
                dm.Save(dataToSave, keyName);
            }

        }

        public KeySpawnSaveDataProxy LoadKeyData(string keyName)
        {

            string? data = dm.Load(keyName);

            if (data is null) return null;
           
            KeySpawnSaveDataProxy sdp = JsonSerializer.Deserialize<KeySpawnSaveDataProxy>(data);

            return sdp;
        }

        

    }
}
