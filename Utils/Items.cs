using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine;
using Il2Cpp;
using ModComponent;
using MelonLoader;

namespace MoreLockedDoors.Utils
{
    internal class Items
    {

        public static string prybar = "GEAR_Prybar";
        public static string boltcutters = "GEAR_BoltCutters";
        public static string hacksaw = "GEAR_Hacksaw";
        public static string hatchet = "GEAR_Hatchet";

        //Custom items
        public static string farmKey = "GEAR_MoreLockedDoors_PV_FarmKey";
        public static string communityHallKey = "GEAR_MoreLockedDoors_PV_CommunityHallKey";
        public static string campOfficeKey = "GEAR_MoreLockedDoors_ML_CampOfficeKey";
        public static string anglersDenKey = "GEAR_MoreLockedDoors_AC_AnglersDenKey";
        public static string fishingCabinKey = "GEAR_MoreLockedDoors_CH_FishingCabinKey";

        //unsued
        public static GearItem GetGearItem(string prefabName)
        {

            GameObject prefab = Addressables.LoadAssetAsync<GameObject>(prefabName).WaitForCompletion();
            return prefab.GetComponent<GearItem>();
        }

    }
}
