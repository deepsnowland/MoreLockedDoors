using System;
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
using UnityEngine;

namespace MoreLockedDoors.Utils
{
    internal class SaveDataManager
    {

        ModDataManager dm = new ModDataManager("More Locked Doors", true);
        public void Save(string data, string suffix)
        {
            if(suffix != null || suffix != "") MelonLogger.Msg("Saving data using suffix: {0}", suffix);
            dm.Save(data, suffix);
        }

        public CustomLockSaveDataProxy LoadLockData(string suffix, bool displayDebug = false)
        {
            string? data = dm.Load(suffix);

            if (data is null)
            {
                if(displayDebug) MelonLogger.Error("Unable to load data from mod data using suffix {0}", suffix);
                return null;
            }
            
            CustomLockSaveDataProxy sdp = JsonSerializer.Deserialize<CustomLockSaveDataProxy>(data);

            return sdp;
        }

        public void SaveKeyData(SpawnSaveDataProxy data, string keyName)
        {

            if(data != null)
            {
                string dataToSave = JsonSerializer.Serialize<SpawnSaveDataProxy>(data);
                dm.Save(dataToSave, keyName);
            }

        }

        public SpawnSaveDataProxy LoadKeyData(string keyName)
        {

            string? data = dm.Load(keyName);

            if (data is null) return null;
           
            SpawnSaveDataProxy sdp = JsonSerializer.Deserialize<SpawnSaveDataProxy>(data);

            return sdp;
        }

        public void SaveBoltcuttersList(List<SpawnSaveDataProxy> data)
        {
            if (data != null)
            {
                MelonLogger.Msg("Saving list");
                string dataToSave = JsonSerializer.Serialize<List<SpawnSaveDataProxy>>(data);
                dm.Save(dataToSave, "boltcutters");
            }
        }

        public List<SpawnSaveDataProxy> LoadBoltcuttersList()
        {
            string? data = dm.Load("boltcutters");

            if (data is null) return null;

            List<SpawnSaveDataProxy> list = JsonSerializer.Deserialize<List<SpawnSaveDataProxy>>(data);

            return list;
        }


    }
}
