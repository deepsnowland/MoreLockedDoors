using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace MoreLockedDoors.Utils
{
    internal class Items
    {

        public static GameObject prybar = Addressables.LoadAssetAsync<GameObject>("GEAR_Prybar").WaitForCompletion();
        public static GameObject boltcutters = Addressables.LoadAssetAsync<GameObject>("GEAR_Boltcutters").WaitForCompletion();
        public static GameObject hacksaw = Addressables.LoadAssetAsync<GameObject>("GEAR_Hacksaw").WaitForCompletion();

        //Custom items
        public static GameObject farmKey = Addressables.LoadAssetAsync<GameObject>("GEAR_MoreLockedDoors_PV_FarmKey").WaitForCompletion();
        public static GameObject communityHallKey = Addressables.LoadAssetAsync<GameObject>("GEAR_MoreLockedDoors_PV_CommunityHallKey").WaitForCompletion();
        public static GameObject campOfficeKey = Addressables.LoadAssetAsync<GameObject>("GEAR_MoreLockedDoors_ML_CampOfficeKey").WaitForCompletion();
        public static GameObject anglersDenKey = Addressables.LoadAssetAsync<GameObject>("GEAR_MoreLockedDoors_AC_AnglersDenKey").WaitForCompletion();
        public static GameObject fishingCabinKey = Addressables.LoadAssetAsync<GameObject>("GEAR_MoreLockedDoors_CH_FishingCabinKey").WaitForCompletion();

    }
}
