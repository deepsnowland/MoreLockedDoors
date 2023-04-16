using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using MoreLockedDoors.Locks;
using Random = System.Random;
using LoadScene = Il2Cpp.LoadScene;
using MoreLockedDoors.Utils;
using Object = System.Object;

namespace MoreLockedDoors
{
    internal class Patches : MelonMod
    {
        [HarmonyPatch(typeof(QualitySettingsManager), nameof(QualitySettingsManager.ApplyCurrentQualitySettings))]

        internal class LockEnabler
        {
            private static void Postfix()
            {
                LockManager lockManager = new();

                AddMysteryLakeLocks(lockManager);
                AddMountainTownLocks(lockManager);
                AddBrokenRailroadLocks(lockManager);
                AddDesolationPointLocks(lockManager);
                AddCoastalHighwayLocks(lockManager);
                AddCinderHillsCoalMineLocks(lockManager);
                AddPleasantValleyLocks(lockManager);
                AddBlackrockLocks(lockManager);
                AddAshCanyonLocks(lockManager);
            }
        }

        public static void AddMysteryLakeLocks(LockManager lockManager)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LakeRegion")
            {

                var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "InteriorLoadTrigger");

                //lock Camp Office exterior doors
                GameObject campOfficeFrontDoor = null; //5662730c-dd4b-401d-b63e-3e5379a2ea05
                GameObject campOfficeBackDoor = null; //19e12caa-9b4a-48ba-a73a-c5473633a8ac

                foreach (var obj in objects)
                {
                    if (obj.GetComponent<ObjectGuid>().PDID == "5662730c-dd4b-401d-b63e-3e5379a2ea05")
                    {
                        campOfficeFrontDoor = obj;
                    }
                    else if (obj.GetComponent<ObjectGuid>().PDID == "19e12caa-9b4a-48ba-a73a-c5473633a8ac")
                    {
                        campOfficeBackDoor = obj;
                    }
                }

                if (campOfficeFrontDoor != null && campOfficeBackDoor != null)
                {

                    MelonLogger.Msg("Camp office exterior doors found");

                    lockManager.InitializeLock(campOfficeFrontDoor, 80, lockManager.woodDoorLockedAudio, "1b38bf5f-fcf1-4414-85a5-2287a083561c", "CampOfficeFrontDoorExt", "CampOfficeFrontDoorInt", Items.campOfficeKey.GetComponent<GearItem>());
                    lockManager.InitializeLock(campOfficeBackDoor, 80, lockManager.woodDoorLockedAudio, "7ae0e7e1-c485-488e-86e5-3cc35c5a8f6b", "CampOfficeBackDoorExt", "CampOfficeBackDoorInt", Items.campOfficeKey.GetComponent<GearItem>());
                }
                
                //lock Carter Hydro Dam staging fence gate
                GameObject damFrontGateObj = GameObject.Find(Utils.Paths.damFrontGate);
                lockManager.InitializeLock(damFrontGateObj, 70, lockManager.metalChainDoorLockedAudio, "", "HydroDamGate", "", Utils.Items.boltcutters.GetComponent<GearItem>());
            }
            else if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "CampOffice")
            {
                GameObject campOfficeFrontDoor = GameObject.Find(Paths.campOfficeFrontDoorInt);
                GameObject campOfficeBackDoor = GameObject.Find(Paths.campOfficeBackDoorInt);

                lockManager.InitializeLock(campOfficeFrontDoor, 80, lockManager.woodDoorLockedAudio, "5662730c-dd4b-401d-b63e-3e5379a2ea05", "CampOfficeFrontDoorInt", "CampOfficeFrontDoorExt", Items.campOfficeKey.GetComponent<GearItem>());
                lockManager.InitializeLock(campOfficeBackDoor, 80, lockManager.woodDoorLockedAudio, "19e12caa-9b4a-48ba-a73a-c5473633a8ac", "CampOfficeBackDoorInt", "CampOfficeBackDoorExt", Items.campOfficeKey.GetComponent<GearItem>());
            }
        }
        public static void AddMountainTownLocks(LockManager lockManager)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MountainTownRegion")
            {
                //lock Milton Credit Union
                GameObject bankFrontDoorObj = GameObject.Find(Utils.Paths.bankFrontDoor);
                lockManager.InitializeLock(bankFrontDoorObj, 50, lockManager.woodDoorLockedAudio, "", "MiltonBankDoor", "",Utils.Items.prybar.GetComponent<GearItem>());

            }
        }
        public static void AddBrokenRailroadLocks(LockManager lockManager)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "TracksRegion")
            {
                //lock Hunting Lodge fence gate
                GameObject lodgeGateObj = GameObject.Find(Utils.Paths.huntingLodgeGate);
                lockManager.InitializeLock(lodgeGateObj, 60, lockManager.metalChainDoorLockedAudio, "", "HuntingLodgeGate", "", Utils.Items.boltcutters.GetComponent<GearItem>());

                //lock Maintenance shed exterior doors
                GameObject maintenanceShedDoorAobj = GameObject.Find(Utils.Paths.maintenanceShedDoorA);
                GameObject maintenanceShedDoorBobj = GameObject.Find(Utils.Paths.maintenanceShedDoorB);
                GameObject maintenanceShedDoorCobj = GameObject.Find(Utils.Paths.maintenanceShedDoorC);

                string[] GUIDs = { "bbb36907-bb53-41ec-a5cb-e6d362580663", "2c22cb32-5b14-4a1c-9fef-3d6a9c33ee5d", "ea186e44-18ac-4117-9c46-348158fb0e25s" };

                GameObject[] doors = { maintenanceShedDoorAobj, maintenanceShedDoorBobj, maintenanceShedDoorCobj };
                string[] doorNames = { "MaintenanceShedDoorAExt", "MaintenanceShedDoorBExt", "MaintenanceShedDoorCExt" };
                string[] interiorDoorNames = { "MaintenanceShedDoorAInt", "MaintenanceShedDoorBInt", "MaintenanceShedDoorCInt" };
                var i = 0;
                foreach (var door in doors)
                {
                    lockManager.InitializeLock(door, 80, lockManager.metalDoorLockedAudio, GUIDs[i], doorNames[i], interiorDoorNames[i], Utils.Items.prybar.GetComponent<GearItem>());
                    i++;
                }
            }
            else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MaintenanceShedA")
            {
                //lock Maintenance shed interior doors

                GameObject maintenanceShedDoorAobj = GameObject.Find(Utils.Paths.maintenanceShedDoorAInterior);
                GameObject maintenanceShedDoorBobj = GameObject.Find(Utils.Paths.maintenanceShedDoorBInterior);
                GameObject maintenanceShedDoorCobj = GameObject.Find(Utils.Paths.maintenanceShedDoorCInterior);

                string[] GUIDs = { "575f6158-c639-4bc0-86db-ba39e5f108ce", "2a871b6f-8d5d-4d2b-812f-adbcf2b3d2dc", "6d63dd98-a94c-4b71-80e3-99aeb3da354b" };

                GameObject[] doors = { maintenanceShedDoorAobj, maintenanceShedDoorBobj, maintenanceShedDoorCobj };
                string[] doorNames = { "MaintenanceShedDoorAInt", "MaintenanceShedDoorBInt", "MaintenanceShedDoorCInt" };
                string[] exteriorDoorNames = { "MaintenanceShedDoorAExt", "MaintenanceShedDoorBExt", "MaintenanceShedDoorCExt" };

                var i = 0;
                foreach (var door in doors)
                {
                    lockManager.InitializeLock(door, 80, lockManager.metalDoorLockedAudio, GUIDs[i], doorNames[i], exteriorDoorNames[i], Utils.Items.prybar.GetComponent<GearItem>());
                    i++;
                }

            }
        }
        public static void AddDesolationPointLocks(LockManager lockManager)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "WhalingStationRegion")
            {

                var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "TransitionContact");

                //Lock Quonset Gas Station Exterior
                GameObject mine5door1 = null;
                GameObject mine5door2 = null;

                foreach (var obj in objects)
                {
                    if (obj.GetComponent<ObjectGuid>().PDID == "d61e4ad1-6e4a-47bf-a5ed-696070a88399")
                    {
                        mine5door1 = obj;
                    }
                    else if (obj.GetComponent<ObjectGuid>().PDID == "1d06bcf8-fc72-4c47-ad3d-ed2f5455d5b2")
                    {
                        mine5door2 = obj;
                    }
                }

                if (mine5door1 != null && mine5door2 != null)
                {
                    lockManager.InitializeLock(mine5door1, 40, lockManager.metalChainDoorLockedAudio, "27abfb4e-d322-4b64-b1a8-d382cee053af", "MineNo5Door1Ext", "MineNo5Door1Int", Utils.Items.hacksaw.GetComponent<GearItem>());
                    lockManager.InitializeLock(mine5door2, 40, lockManager.metalChainDoorLockedAudio, "a358f1a0-1af4-4faa-9f00-0b77fa1ff114", "MineNo5Door2Ext", "MineNo5Door2Int", Utils.Items.hacksaw.GetComponent<GearItem>());
                }
                
                //Lock Hibernia processing
                GameObject hiberniaFrontDoor = GameObject.Find(Utils.Paths.hiberniaFrontDoor);
                GameObject hiberniaBackDoor = GameObject.Find(Utils.Paths.hiberniaBackDoor);

                lockManager.InitializeLock(hiberniaFrontDoor, 60, lockManager.metalDoorLockedAudio, "5805f636-2b63-4bda-a468-750bef9b0ac6", "HiberniaFrontDoorExt", "HiberniaFrontDoorInt", Utils.Items.prybar.GetComponent<GearItem>());
                lockManager.InitializeLock(hiberniaBackDoor, 60, lockManager.metalDoorLockedAudio, "61299ab5-b6ac-420a-bbdc-4e4787c7b461", "HiberniaBackDoorExt", "HiberniaBackDoorInt", Utils.Items.prybar.GetComponent<GearItem>());

            }
            else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "WhalingWarehouseA")
            {

                //Lock interior Hibernia doors
                GameObject hiberniaFrontDoor = GameObject.Find(Utils.Paths.hiberniaFrontDoorInt);
                GameObject hiberniaBackDoor = GameObject.Find(Utils.Paths.hiberniaBackDoorInt);

                lockManager.InitializeLock(hiberniaFrontDoor, 60, lockManager.metalDoorLockedAudio, "8497788f-28d1-4a17-baab-f3a6b98100f2", "HiberniaFrontDoorInt", "HiberniaFrontDoorExt", Utils.Items.prybar.GetComponent<GearItem>());
                lockManager.InitializeLock(hiberniaBackDoor, 60, lockManager.metalDoorLockedAudio, "ed87b39e-addb-457a-8abf-81d7bfa4a97e",  "HiberniaBackDoorInt", "HiberniaBackDoorExt", Utils.Items.prybar.GetComponent<GearItem>());

            }
            else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "WhalingMine")
            {

                //Lock Mine 5 interior doors
                GameObject mine5door1Int = GameObject.Find(Utils.Paths.mine5door1Interior);
                GameObject mine5door2Int = GameObject.Find(Utils.Paths.mine5door2Interior);

                lockManager.InitializeLock(mine5door1Int, 40, lockManager.metalChainDoorLockedAudio, "d61e4ad1-6e4a-47bf-a5ed-696070a88399",  "MineNo5Door1Int", "MineNo5Door1Ext", Utils.Items.hacksaw.GetComponent<GearItem>());
                lockManager.InitializeLock(mine5door2Int, 40, lockManager.metalChainDoorLockedAudio, "1d06bcf8-fc72-4c47-ad3d-ed2f5455d5b2",  "MineNo5Door2Int", "MineNo5Door2Ext", Utils.Items.hacksaw.GetComponent<GearItem>());

            }
        }
        public static void AddCoastalHighwayLocks(LockManager lockManager)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "CoastalRegion")
            {

                //Get door objects
                var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "InteriorLoadTrigger");

                GameObject quonsetFrontDoor = null;
                GameObject quonsetBackDoor = null;
                GameObject fishingCabinDoorExt = null;

                foreach (var obj in objects)
                {
                    if (obj.GetComponent<ObjectGuid>().PDID == "a40c7e13-86b4-4a04-9a50-16f78bd14d8e")
                    {
                        quonsetFrontDoor = obj;
                    }
                    else if (obj.GetComponent<ObjectGuid>().PDID == "e6910ab2-75ec-4df5-adb2-8f3a4c76ecaa")
                    {
                        quonsetBackDoor = obj;
                    }
                    else if(obj.GetComponent<ObjectGuid>().PDID == "d6667072-a207-41d8-8f4c-f6b1804baaf2")
                    {
                        fishingCabinDoorExt = obj;
                    }
                }

                //Lock Quonset Gas Station Exterior

                if (quonsetFrontDoor != null && quonsetBackDoor != null)
                {
                    lockManager.InitializeLock(quonsetFrontDoor, 70, lockManager.metalDoorLockedAudio, "8939b09a-3b9b-4b2c-9ced-cde4bc8ad171", "QuonsetFrontDoorExt", "QuonsetFrontDoorInt", Utils.Items.prybar.GetComponent<GearItem>());
                    lockManager.InitializeLock(quonsetBackDoor, 70, lockManager.metalDoorLockedAudio, "9991f9bb-0c1a-48b3-81a8-8ce883ccd389",  "QuonsetBackDoorExt", "QuonsetBackDoorInt", Utils.Items.prybar.GetComponent<GearItem>());
                }
                
                //Lock Cinder Hills Coal Mine Exterior CH
                GameObject cinderHillsCoalMineDoor = GameObject.Find(Paths.cinderHillsCoalMineExteriorDoor);
                lockManager.InitializeLock(cinderHillsCoalMineDoor, 90, lockManager.metalChainDoorLockedAudio, "27abfb4e-d322-4b64-b1a8-d382cee053af", "CinderHillsMineDoorExt", "CinderHillsCoalMineDoorInt", Utils.Items.hacksaw.GetComponent<GearItem>());

                //Lock Fishing Cabin

                if(fishingCabinDoorExt != null)
                {
                    lockManager.InitializeLock(fishingCabinDoorExt, 70, lockManager.woodDoorLockedAudio, "", "FishingCabinDoorExt", "", Items.fishingCabinKey.GetComponent<GearItem>());
                }

            }
            else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "QuonsetGasStation")
            {
                //Lock quonset interior door
                GameObject quonsetFrontDoor = GameObject.Find(Paths.quonsetGasStationInteriorFrontDoor);
                GameObject quonsetBackDoor = GameObject.Find(Paths.quonsetGasStationInteriorBackDoor);

                lockManager.InitializeLock(quonsetFrontDoor, 70, lockManager.metalDoorLockedAudio, "a40c7e13-86b4-4a04-9a50-16f78bd14d8e", "QuonsetFrontDoorInt", "QuonsetFrontDoorExt", Utils.Items.prybar.GetComponent<GearItem>());
                lockManager.InitializeLock(quonsetBackDoor, 70, lockManager.metalDoorLockedAudio, "e6910ab2-75ec-4df5-adb2-8f3a4c76ecaa",  "QuonsetBackDoorInt", "QuonsetBackDoorExt", Utils.Items.prybar.GetComponent<GearItem>());
            }
        }
        public static void AddCinderHillsCoalMineLocks(LockManager lockManager)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MineTransitionZone")
            {

                //Lock mine exits to CH and PV
                GameObject cinderHillsCoalMineDoorCH = GameObject.Find(Paths.cinderHillsCoalMineInteriorDoorCH);

                lockManager.InitializeLock(cinderHillsCoalMineDoorCH, 90, lockManager.metalChainDoorLockedAudio, "adb77a85-783e-4fc6-bd50-07bd264efe44", "CinderHillsMineDoorInt", "CinderHillsMineDoorExt", Utils.Items.hacksaw.GetComponent<GearItem>());

            }
        }
        public static void AddPleasantValleyLocks(LockManager lockManager)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "RuralRegion")
            {

                var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "InteriorLoadTrigger");

                //Lock Farmhouse exterior

                //9769a1de-8021-4585-9648-bc439aa334fd
                GameObject farmhouseDoor1 = null;
                //57607823-0142-409a-aa92-893f35dc5b8a
                GameObject farmhouseDoor2 = null;
                //40b60936-4e53-4b76-b127-43ffd123fcb0
                GameObject farmhouseDoor3 = null;
                //ee96b557-caa7-4d96-89d3-79ebc406b8b5
                GameObject farmhouseCellarDoor = null;

                foreach (var obj in objects)
                {
                    if (obj.GetComponent<ObjectGuid>().PDID == "9769a1de-8021-4585-9648-bc439aa334fd")
                    {
                        farmhouseDoor1 = obj;
                    }
                    else if (obj.GetComponent<ObjectGuid>().PDID == "57607823-0142-409a-aa92-893f35dc5b8a")
                    {
                        farmhouseDoor2 = obj;
                    }
                    else if (obj.GetComponent<ObjectGuid>().PDID == "40b60936-4e53-4b76-b127-43ffd123fcb0")
                    {
                        farmhouseDoor3 = obj;
                    }
                    else if (obj.GetComponent<ObjectGuid>().PDID == "ee96b557-caa7-4d96-89d3-79ebc406b8b5")
                    {
                        farmhouseCellarDoor = obj;
                    }
                }

                if(farmhouseDoor1 != null && farmhouseDoor2 != null && farmhouseDoor3 != null && farmhouseCellarDoor != null)
                {
                    lockManager.InitializeLock(farmhouseDoor1, 80, lockManager.woodDoorLockedAudio, "fb7c5b5d-e88d-489e-a1c2-ccbdc76e5da2",  "FarmhouseDoor1Ext", "FarmhouseDoor1Int", Items.farmKey.GetComponent<GearItem>());
                    lockManager.InitializeLock(farmhouseDoor2, 80, lockManager.woodDoorLockedAudio, "0b9a4999-4125-4c11-bb13-d48ed81546c9",  "FarmhouseDoor2Ext", "FarmhouseDoor2Int", Items.farmKey.GetComponent<GearItem>());
                    lockManager.InitializeLock(farmhouseDoor3, 80, lockManager.woodDoorLockedAudio, "2edd5f53-8533-4780-b0dd-eeec30a744d5",  "FarmhouseDoor3Ext", "FarmhouseDoor3Int", Items.farmKey.GetComponent<GearItem>());
                    lockManager.InitializeLock(farmhouseCellarDoor, 80, lockManager.metalDoorLockedAudio, "e62f85fd-edc9-4aaf-ae59-7b893b06abe9", "FarmhouseCellarDoorExt", "FarmhouseCellarDoorInt", Items.prybar.GetComponent<GearItem>());
                }

                //Lock Community Hall exterior

                GameObject communityHallFrontDoor = null;
                GameObject communityHallBackDoor = null;

                foreach (var obj in objects)
                {
                    if (obj.GetComponent<ObjectGuid>().PDID == "a30c0c66-7117-4359-bcad-f7b75ea1ccaf")
                    {
                        communityHallFrontDoor = obj;
                    }
                    else if (obj.GetComponent<ObjectGuid>().PDID == "87d6326a-f32b-4e87-968e-39b48d9e1a3f")
                    {
                        communityHallBackDoor = obj;
                    }
                }

                if(communityHallFrontDoor != null && communityHallBackDoor != null)
                {
                    lockManager.InitializeLock(communityHallFrontDoor, 70, lockManager.woodDoorLockedAudio, "2d1702f8-5626-444f-b36e-f9542899d606",  "CommunityHallFrontDoorExt", "CommunityHallFrontDoorInt", Items.communityHallKey.GetComponent<GearItem>());
                    lockManager.InitializeLock(communityHallBackDoor, 70, lockManager.woodDoorLockedAudio, "7fa2c843-3a37-497f-bb16-3083be100f96",   "CommunityHallBackDoorExt", "CommunityHallBackDoorInt", Items.communityHallKey.GetComponent<GearItem>());
                }

                //Lock Rural Store exterior

                GameObject ruralStoreFrontDoor = null;
                GameObject ruralStoreBackDoor = null;

                foreach (var obj in objects)
                {
                    if (obj.GetComponent<ObjectGuid>().PDID == "e5cb7bfa-5c37-4fef-a57e-7fac7174aaa3")
                    {
                        ruralStoreFrontDoor = obj;
                    }
                    else if (obj.GetComponent<ObjectGuid>().PDID == "30ce35d1-f08b-43e4-abdd-c8f226352afc")
                    {
                        ruralStoreBackDoor = obj;
                    }
                }

                if (ruralStoreFrontDoor != null && ruralStoreBackDoor != null)
                {
                    lockManager.InitializeLock(ruralStoreFrontDoor, 70, lockManager.woodDoorLockedAudio, "c17a1359-75ee-43fa-ace4-b76819fdaa9b",  "RuralStoreFrontDoorExt", "RuralStoreFrontDoorInt", Utils.Items.prybar.GetComponent<GearItem>());
                    lockManager.InitializeLock(ruralStoreBackDoor, 70, lockManager.woodDoorLockedAudio, "138894f7-7fd0-4c97-be36-1f350cb93499",  "RuralStoreBackDoorExt", "RuralStoreBackDoorInt", Utils.Items.prybar.GetComponent<GearItem>());
                }
                
                //Lock Radio Control Hut
                GameObject signalHillDoor = GameObject.Find(Paths.signalHillDoor);
                lockManager.InitializeLock(signalHillDoor, 100, lockManager.metalDoorLockedAudio, "", "SignalHillDoorExt", "", Utils.Items.prybar.GetComponent<GearItem>());
            }
            else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "RuralStoreA")
            {

                //Lock Rural store interior
                GameObject ruralStoreFrontDoor = GameObject.Find(Utils.Paths.ruralStoreFrontDoorInt);
                GameObject ruralStoreBackDoor = GameObject.Find(Utils.Paths.ruralStoreBackDoorInt);

                lockManager.InitializeLock(ruralStoreFrontDoor, 70, lockManager.woodDoorLockedAudio, "e5cb7bfa-5c37-4fef-a57e-7fac7174aaa3", "RuralStoreFrontDoorInt", "RuralStoreFrontDoorExt", Utils.Items.prybar.GetComponent<GearItem>());
                lockManager.InitializeLock(ruralStoreBackDoor, 70, lockManager.woodDoorLockedAudio, "30ce35d1-f08b-43e4-abdd-c8f226352afc", "RuralStoreBackDoorInt", "RuralStoreBackDoorExt", Utils.Items.prybar.GetComponent<GearItem>());

            }
            else if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "CommunityHallA")
            {

                GameObject communityHallFrontDoorInt = GameObject.Find(Utils.Paths.communityHallFrontDoorInt);
                GameObject communityHallBackDoorInt = GameObject.Find(Utils.Paths.communityHallBackDoorInt);

                lockManager.InitializeLock(communityHallFrontDoorInt, 70, lockManager.woodDoorLockedAudio, "a30c0c66-7117-4359-bcad-f7b75ea1ccaf", "CommunityHallFrontDoorInt", "CommunityHallFrontDoorExt", Utils.Items.communityHallKey.GetComponent<GearItem>());
                lockManager.InitializeLock(communityHallBackDoorInt, 70, lockManager.woodDoorLockedAudio, "87d6326a-f32b-4e87-968e-39b48d9e1a3f",  "CommunityHallBackDoorInt", "CommunityHallBackDoorExt", Utils.Items.communityHallKey.GetComponent<GearItem>());
            }
            else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "FarmhouseA")
            {

                var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "ExteriorLoadTrigger");

                GameObject farmhouseBasementDoorInt = GameObject.Find(Paths.farmhouseBasementDoorHouse);

                GameObject farmhouseDoorInt1 = null;
                GameObject farmhouseDoorInt2 = null;
                GameObject farmhouseDoorInt3 = null;


                foreach (var obj in objects)
                {
                    if (obj.GetComponent<ObjectGuid>().PDID == "fb7c5b5d-e88d-489e-a1c2-ccbdc76e5da2")
                    {
                        farmhouseDoorInt1 = obj;
                    }
                    else if (obj.GetComponent<ObjectGuid>().PDID == "0b9a4999-4125-4c11-bb13-d48ed81546c9")
                    {
                        farmhouseDoorInt2 = obj;
                    }
                    else if (obj.GetComponent<ObjectGuid>().PDID == "2edd5f53-8533-4780-b0dd-eeec30a744d5")
                    {
                        farmhouseDoorInt3 = obj;
                    }
                }

                if(farmhouseDoorInt1 != null && farmhouseDoorInt2 != null && farmhouseDoorInt3 != null && farmhouseBasementDoorInt != null)
                {
                    lockManager.InitializeLock(farmhouseDoorInt1, 80, lockManager.woodDoorLockedAudio, "9769a1de-8021-4585-9648-bc439aa334fd", "FarmhouseDoor1Int", "FarmhouseDoor1Ext", Items.farmKey.GetComponent<GearItem>());
                    lockManager.InitializeLock(farmhouseDoorInt2, 80, lockManager.woodDoorLockedAudio, "57607823-0142-409a-aa92-893f35dc5b8a", "FarmhouseDoor2Int", "FarmhouseDoor2Ext", Items.farmKey.GetComponent<GearItem>());
                    lockManager.InitializeLock(farmhouseDoorInt3, 80, lockManager.woodDoorLockedAudio, "40b60936-4e53-4b76-b127-43ffd123fcb0", "FarmhouseDoor3Int", "FarmhouseDoor3Ext", Items.farmKey.GetComponent<GearItem>());
                    lockManager.InitializeLock(farmhouseBasementDoorInt, 70, lockManager.woodDoorLockedAudio, "c42cd462-fa07-4ccb-9bea-e611224260ee", "FarmhouseBasementDoorInt", "FarmhouseBasementDoorExt", Items.farmKey.GetComponent<GearItem>());
                } 

            }
            else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "FarmhouseABasement")
            {

                GameObject basementDoorBasement = GameObject.Find(Utils.Paths.farmhouseBasementDoorBasement);
                GameObject farmhouseCellarDoorInt = GameObject.Find(Utils.Paths.farmhouseCellarDoorInt);

                lockManager.InitializeLock(basementDoorBasement, 70, lockManager.woodDoorLockedAudio, "396c8857-9d29-4c6d-aa7b-ba7e839c6cec", "FarmhouseBasementDoorExt", "FarmhouseBasementDoorInt", Items.farmKey.GetComponent<GearItem>());
                lockManager.InitializeLock(farmhouseCellarDoorInt, 80, lockManager.metalDoorLockedAudio, "ee96b557-caa7-4d96-89d3-79ebc406b8b5", "FarmhouseCellarDoorInt", "FarmhouseCellarDoorExt", Items.prybar.GetComponent<GearItem>());

            }
        }
        public static void AddBlackrockLocks(LockManager lockManager)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "BlackrockRegion")
            {
                GameObject substationDoor = GameObject.Find(Utils.Paths.huntingLodgeGate);
                lockManager.InitializeLock(substationDoor, 60, lockManager.metalDoorLockedAudio, "f4bc3103-7108-4d80-9b0d-2e83864983c7", "SubstationDoorExt", "SubstationDoorInt", Utils.Items.prybar.GetComponent<GearItem>());
            }
            else if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "RadioControlHutC")
            {
                GameObject substationDoor = GameObject.Find(Utils.Paths.substationDoorInt);
                lockManager.InitializeLock(substationDoor, 60, lockManager.metalDoorLockedAudio, "a0fb16ec-8fb0-48e1-aa38-8e4760b24410", "SubstationDoorInt", "SubstationDoorExt", Utils.Items.prybar.GetComponent<GearItem>());
            }
        }
        public static void AddAshCanyonLocks(LockManager lockManager)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "AshCanyonRegion")
            {
                GameObject anglersDenDoorExt = GameObject.Find(Utils.Paths.anglersDenDoorExt);
                lockManager.InitializeLock(anglersDenDoorExt, 60, lockManager.woodDoorLockedAudio, "7a33c7df-02cc-49a3-8766-9fa022979c50", "AnglersDenDoorExt", "", Items.anglersDenKey.GetComponent<GearItem>());
            }
        }
        
        [HarmonyPatch(typeof(Lock), nameof(Lock.FinishForceLock))]
        internal class LockStateSaver
        {

            private static void Postfix(Lock __instance)
            {

                if(__instance.tag != "Untagged")
                {
                    if (__instance.m_LockState == LockState.Broken) Implementation.sdm.Save("Broken", __instance.tag);
                }

            }

        }

        //These patches fix issues with the generic Lock component
        [HarmonyPatch(typeof(LoadScene), nameof(LoadScene.Update))]

        internal class LoadScene_Update
        {
           /* private static void Postfix(LoadScene __instance)
            {
                Lock lck = null;

                GameObject obj = __instance.gameObject;
                string[] paths = Utils.Paths.paths;

                if (paths.Any(Utils.Paths.GetObjectPath(obj).Contains))
                {
                    //MelonLogger.Msg("Found mod lock object: {0}", Paths.GetObjectPath(obj));
                    lck = obj.GetComponent<Lock>();
                    lck.MaybeUnlockDueToCompanionBeingUnlocked();

                    if (lck.m_LockState == LockState.Unlocked) lck.UnlockCompanionLock(); 
                }
                else
                {
                    return;
                }

            } */
        }

        [HarmonyPatch(typeof(Lock), nameof(Lock.Awake))]

        internal class Lock_Awake
        {

            //Override the Awake method for the lock component if it is added with this mod. This prevents the Awake method from executing before modifications to the component are complete.
            public static bool Prefix(Lock __instance)
            {

                GameObject obj = __instance.gameObject;
                string[] paths = Utils.Paths.paths;

                
                if (paths.Any(Utils.Paths.GetObjectPath(obj).Contains))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }

    }
}


