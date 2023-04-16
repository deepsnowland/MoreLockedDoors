using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreLockedDoors.Utils
{
    internal class Paths
    {

        //Mountain Town
        public const string bankFrontDoor = "Art/Structures/Town/Houses_AlwaysHere/STR_BankA_Prefab (1)/Tech/InteriorLoadTrigger";

        //Mystery Lake
        public const string damFrontDoor = "Art/Structures/STRSPAWN_DamA_Prefab/Tech/InteriorLoadTrigger";
        public const string damFrontGate = "Art/DamObjectGroup/DamFenceGate-intact/OBJ_fenceSecurityChainGateA_Prefab/Hinge/OBJ_fenceSecurityChainGateB_Prefab";

        public const string campOfficeExteriorDoors = "Art/Structures/STRSPAWN_CampOffice_Prefab/Tech/InteriorLoadTrigger";

        //1b38bf5f-fcf1-4414-85a5-2287a083561c
        public const string campOfficeFrontDoorInt = "Root/Design/Interactive/INTERACTIVE_CampOfficeInteriorDoorFront_Prefab";
        //7ae0e7e1-c485-488e-86e5-3cc35c5a8f6b
        public const string campOfficeBackDoorInt = "Root/Design/Interactive/INTERACTIVE_CampOfficeInteriorDoorBack_Prefab";

        //Broken Railroad

        public const string huntingLodgeGate = "Art/Structures/MainenanceYard/gate_area/fences/LodgeGate/OBJ_fenceSecurityChainGateA_Prefab (1)/Hinge/OBJ_fenceSecurityChainGateB_Prefab";
        public const string maintenanceShedDoorA = "Design/Transitions/MaintenenceShed/TRIGGER_MainteneceShedA";
        public const string maintenanceShedDoorB = "Design/Transitions/MaintenenceShed/TRIGGER_MainteneceShedB";
        public const string maintenanceShedDoorC = "Design/Transitions/MaintenenceShed/TRIGGER_MainteneceShedC";
        public const string maintenanceShedDoorAInterior = "Root/Design/Scripting/MainteneceShedA_trigger";
        public const string maintenanceShedDoorBInterior = "Root/Design/Scripting/MainteneceShedB_trigger";
        public const string maintenanceShedDoorCInterior = "Root/Design/Scripting/MainteneceShedC_trigger";

        //Coastal Highway

        public const string quonsetGasStationExteriorDoors = "Art/Structure_Group/STRSPAWN_QuonsetGasStation_Prefab/Tech/InteriorLoadTrigger";
        public const string quonsetGasStationInteriorFrontDoor = "Root/Design/Scripting/FrontDoorInteriorExit";
        public const string quonsetGasStationInteriorBackDoor = "Root/Design/Scripting/BackDoorInteriorExit";
        public const string cinderHillsCoalMineExteriorDoor = "Design/Scripting/Transitions/Mine/TransitionContact";

        //Cinder Hills Coal Mine
        public const string cinderHillsCoalMineInteriorDoorCH = "Design/Transitions/Coastal/TransitionContact";

        //Pleasant Valley

        public const string signalHillDoor = "Art/Structures_Group/RadioTowerArea/STRSPAWN_RadioControlHut_Prefab/Tech/InteriorLoadTrigger";

        public const string ruralStoreExteriorDoors = "Art/Structures_Group/Town/STRSPAWN_ThompsonStoreA_Prefab/InteriorLoadTrigger";

        //c17a1359-75ee-43fa-ace4-b76819fdaa9b
        public const string ruralStoreFrontDoorInt = "Root/Design/Scripting/FrontDoorInteriorExit";
        //138894f7-7fd0-4c97-be36-1f350cb93499
        public const string ruralStoreBackDoorInt = "Root/Design/Scripting/BackDoorInteriorExit";

        public const string communityHallExteriorDoors = "Art/STR_CommunityHallA_Exterior_Prefab (1)/Tech/InteriorLoadTrigger";

        //2d1702f8-5626-444f-b36e-f9542899d606
        public const string communityHallFrontDoorInt = "Root/Design/Scripting/FrontDoorInteriorExit";
        //7fa2c843-3a37-497f-bb16-3083be100f96
        public const string communityHallBackDoorInt = "Root/Design/Scripting/BackDoorInteriorExit";

        public const string farmhouseExteriorDoors = "Art/Structures_Group/SouthFarmLands/FarmHouse/STRSPAWN_FarmHouseA_Prefab/Tech/InteriorLoadTrigger";
        //396c8857-9d29-4c6d-aa7b-ba7e839c6cec
        public const string farmhouseBasementDoorHouse = "Root/Design/Scripting/BasementLoadTrigger";
        //c42cd462-fa07-4ccb-9bea-e611224260ee
        public const string farmhouseBasementDoorBasement = "Root/Design/Scripting/FarmHouseABasementHouseExit";
        //e62f85fd-edc9-4aaf-ae59-7b893b06abe9
        public const string farmhouseCellarDoorInt = "Root/Design/Scripting/FarmHouseABasementInteriorExit";

        //Desolation Point

        //8497788f-28d1-4a17-baab-f3a6b98100f2
        public const string hiberniaFrontDoor = "Design/Transitions/WhalingWarehouse/TRIGGER_WhalingRegionB";
        //5805f636-2b63-4bda-a468-750bef9b0ac6
        public const string hiberniaFrontDoorInt = "Root/Design/Scripting/TRIGGER_WhalingRegionB";
        //ed87b39e-addb-457a-8abf-81d7bfa4a97e
        public const string hiberniaBackDoor = "Design/Transitions/WhalingWarehouse/TRIGGER_WhalingRegion";
        //61299ab5-b6ac-420a-bbdc-4e4787c7b461
        public const string hiberniaBackDoorInt = "Root/Design/Scripting/TRIGGER_WhalingRegion";

        //27abfb4e-d322-4b64-b1a8-d382cee053af
        public const string mine5door1Interior = "Design/Transitions/Mine1Entrance/TransitionContact";
        //a358f1a0-1af4-4faa-9f00-0b77fa1ff114
        public const string mine5door2Interior = "Design/Transitions/Mine2Entrance/TransitionContact";

        //Blackrock

        //f4bc3103-7108-4d80-9b0d-2e83864983c7
        public const string substationDoor = "Design/Scripting/Transitions/SubStation/InteriorLoadTrigger";
        //a0fb16ec-8fb0-48e1-aa38-8e4760b24410
        public const string substationDoorInt = "Root/Design/Scripting/RadioControlHutInteriorExit";

        //Ash Canyon

        //c76eb6c3-60ab-46f6-8a06-5dd151b79a9a
        public const string anglersDenDoorExt = "Greybox/structures/STRSPAWN_LakeCabinF_Prefab/Tech/InteriorLoadTrigger";

        public static string[] paths = {bankFrontDoor, damFrontGate, campOfficeExteriorDoors, campOfficeFrontDoorInt, campOfficeBackDoorInt, huntingLodgeGate, maintenanceShedDoorA, maintenanceShedDoorB, maintenanceShedDoorC, maintenanceShedDoorAInterior, maintenanceShedDoorBInterior, maintenanceShedDoorCInterior, cinderHillsCoalMineExteriorDoor, cinderHillsCoalMineInteriorDoorCH, quonsetGasStationExteriorDoors, quonsetGasStationInteriorFrontDoor, quonsetGasStationInteriorBackDoor, signalHillDoor, ruralStoreExteriorDoors, ruralStoreBackDoorInt, ruralStoreFrontDoorInt, communityHallExteriorDoors, communityHallBackDoorInt, communityHallFrontDoorInt, farmhouseExteriorDoors, farmhouseCellarDoorInt, farmhouseBasementDoorHouse, farmhouseBasementDoorBasement, substationDoor, substationDoorInt, hiberniaBackDoor, hiberniaBackDoorInt, hiberniaFrontDoor, hiberniaFrontDoorInt, mine5door1Interior, mine5door2Interior, anglersDenDoorExt};

        public static string GetObjectPath(GameObject obj)
        {

            string name = obj.name;

            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                name = obj.name + "/" + name;
            }
            return name;
        }

    }
}
