using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ModData;
using MoreLockedDoors.Locks;

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

            CustomLockSaveDataProxy sdp = JsonSerializer.Deserialize<CustomLockSaveDataProxy>(data);

            return sdp;
        }

    }
}
