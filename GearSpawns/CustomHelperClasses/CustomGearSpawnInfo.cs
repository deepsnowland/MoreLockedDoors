using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreLockedDoors.GearSpawns.CustomHelperClasses
{
    internal class CustomGearSpawnInfo
    {

        public string Tag { get; set; }

        public Vector3Ser Position { get; set; }

        public string PrefabName { get; set; }

        public QuaternionSer Rotation { get; set; }

        public float SpawnChance { get; set; }

        public CustomGearSpawnInfo(string tag, Vector3Ser position, string prefabName, QuaternionSer rotation, float spawnChance)
        {
            Tag = tag;
            Position = position;
            PrefabName = prefabName;
            Rotation = rotation;
            SpawnChance = spawnChance;
        }

        public CustomGearSpawnInfo()
        {
        }

    }
}
