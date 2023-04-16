using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModData;

namespace MoreLockedDoors.Utils
{
    internal class SaveDataManager
    {

        ModDataManager dm = new ModDataManager("More Locked Doors", false);
        public void Save(string data, string suffix)
        {
            dm.Save(data, suffix);
        }

        public string LoadLockData(string suffix)
        {
            string? lockState = dm.Load(suffix);

            return lockState;
        }

    }
}
