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

        //c17a1359-75ee-43fa-ace4-b76819fdaa9b
        public const string ruralStoreFrontDoorInt = "Root/Design/Scripting/FrontDoorInteriorExit";
        //138894f7-7fd0-4c97-be36-1f350cb93499
        public const string ruralStoreBackDoorInt = "Root/Design/Scripting/BackDoorInteriorExit";

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

        public static string[] paths = {bankFrontDoor, damFrontGate, huntingLodgeGate, maintenanceShedDoorA, maintenanceShedDoorB, maintenanceShedDoorC, maintenanceShedDoorAInterior, maintenanceShedDoorBInterior, maintenanceShedDoorCInterior, quonsetGasStationExteriorDoors, cinderHillsCoalMineExteriorDoor, cinderHillsCoalMineInteriorDoorCH, signalHillDoor, hiberniaBackDoor, hiberniaBackDoorInt, hiberniaFrontDoor, hiberniaFrontDoorInt, mine5door1Interior, mine5door2Interior };

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
