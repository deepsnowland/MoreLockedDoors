using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine;
using Il2Cpp;

namespace MoreLockedDoors.Utils
{
    internal class Items
    {

        public static GearItem prybar = Addressables.LoadAssetAsync<GameObject>("GEAR_Prybar").WaitForCompletion().GetComponent<GearItem>();
        public static GearItem boltcutters = Addressables.LoadAssetAsync<GameObject>("GEAR_Boltcutters").WaitForCompletion().GetComponent<GearItem>();
        public static GearItem hacksaw = Addressables.LoadAssetAsync<GameObject>("GEAR_Hacksaw").WaitForCompletion().GetComponent<GearItem>();
        public static GearItem hatchet = Addressables.LoadAssetAsync<GameObject>("GEAR_Hatchet").WaitForCompletion().GetComponent<GearItem>();

        //Custom items
        public static GearItem farmKey = Addressables.LoadAssetAsync<GameObject>("GEAR_MoreLockedDoors_PV_FarmKey").WaitForCompletion().GetComponent<GearItem>();
        public static GearItem communityHallKey = Addressables.LoadAssetAsync<GameObject>("GEAR_MoreLockedDoors_PV_CommunityHallKey").WaitForCompletion().GetComponent<GearItem>();
        public static GearItem campOfficeKey = Addressables.LoadAssetAsync<GameObject>("GEAR_MoreLockedDoors_ML_CampOfficeKey").WaitForCompletion().GetComponent<GearItem>();
        public static GearItem anglersDenKey = Addressables.LoadAssetAsync<GameObject>("GEAR_MoreLockedDoors_AC_AnglersDenKey").WaitForCompletion().GetComponent<GearItem>();
        public static GearItem fishingCabinKey = Addressables.LoadAssetAsync<GameObject>("GEAR_MoreLockedDoors_CH_FishingCabinKey").WaitForCompletion().GetComponent<GearItem>();

    }
}
