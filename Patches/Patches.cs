using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Unity;
using UnityEngine;
using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using MoreLockedDoors.Locks;
using LoadScene = Il2Cpp.LoadScene;
using MoreLockedDoors.Utils;
using Il2CppNodeCanvas.Tasks.Actions;

namespace MoreLockedDoors.Patches
{
    internal class Patches : MelonMod
    {
        static LockManager lockManager = new();

        [HarmonyPatch(typeof(QualitySettingsManager), nameof(QualitySettingsManager.ApplyCurrentQualitySettings))]

        internal class LockEnabler
        {
            private static void Postfix()
            {
                AddMysteryLakeLocks();
                AddMountainTownLocks();
                AddCoastalHighwayLocks();
                AddDesolationPointLocks();
                AddCinderHillsCoalMineLocks();
                AddPleasantValleyLocks();
                AddAshCanyonLocks();
                AddBrokenRailroadLocks();
                AddBlackrockLocks();

                AddForceItemLockComponents();


            }
        }


        public static void AddMysteryLakeLocks()
        {
            if (GameManager.m_ActiveScene == "LakeRegion")
            {

                var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "InteriorLoadTrigger");

                List<string> campOfficetools = new List<string>() { Items.campOfficeKey, Items.hatchet };

                //lock Camp Office exterior doors
                GameObject campOfficeFrontDoor = null; //5662730c-dd4b-401d-b63e-3e5379a2ea05
                GameObject campOfficeBackDoor = null; //19e12caa-9b4a-48ba-a73a-c5473633a8ac

                int chance = 80;

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
                    lockManager.InitializeCustomLock(campOfficeFrontDoor, ref chance, lockManager.woodDoorLockedAudio, "1b38bf5f-fcf1-4414-85a5-2287a083561c", campOfficetools);
                    lockManager.InitializeCustomLock(campOfficeBackDoor, ref chance, lockManager.woodDoorLockedAudio, "7ae0e7e1-c485-488e-86e5-3cc35c5a8f6b", campOfficetools);
                }

                //lock Carter Hydro Dam staging fence gate
                GameObject damFrontGateObj = GameObject.Find(Paths.damFrontGate);
                chance = 70; //this might cause issues
                lockManager.InitializeCustomLock(damFrontGateObj, ref chance, lockManager.metalChainDoorLockedAudio, "", new List<string> { Items.boltcutters });
            }
            else if (GameManager.m_ActiveScene == "CampOffice")
            {
                GameObject campOfficeFrontDoor = GameObject.Find(Paths.campOfficeFrontDoorInt);
                GameObject campOfficeBackDoor = GameObject.Find(Paths.campOfficeBackDoorInt);
                List<string> campOfficetools = new List<string>() { Items.campOfficeKey, Items.hatchet };

                int chance = 80;

                lockManager.InitializeCustomLock(campOfficeFrontDoor, ref chance, lockManager.woodDoorLockedAudio, "5662730c-dd4b-401d-b63e-3e5379a2ea05", campOfficetools);
                lockManager.InitializeCustomLock(campOfficeBackDoor, ref chance, lockManager.woodDoorLockedAudio, "19e12caa-9b4a-48ba-a73a-c5473633a8ac", campOfficetools);
            }
        }
        public static void AddMountainTownLocks()
        {
            if (GameManager.m_ActiveScene == "MountainTownRegion")
            {
                //lock Milton Credit Union
                GameObject bankFrontDoorObj = GameObject.Find(Paths.bankFrontDoor);
                int chance = 50;
                lockManager.InitializeCustomLock(bankFrontDoorObj, ref chance, lockManager.woodDoorLockedAudio, "", new List<string> { Items.prybar });

            }
        }
        public static void AddBrokenRailroadLocks()
        {
            if (GameManager.m_ActiveScene == "TracksRegion")
            {
                //lock Hunting Lodge fence gate
                GameObject lodgeGateObj = GameObject.Find(Paths.huntingLodgeGate);
                int gateChance = 70;
                lockManager.InitializeCustomLock(lodgeGateObj, ref gateChance, lockManager.metalChainDoorLockedAudio, "", new List<string> { Items.boltcutters });

                //lock Maintenance shed exterior doors
                GameObject maintenanceShedDoorAobj = GameObject.Find(Paths.maintenanceShedDoorA);
                GameObject maintenanceShedDoorBobj = GameObject.Find(Paths.maintenanceShedDoorB);
                GameObject maintenanceShedDoorCobj = GameObject.Find(Paths.maintenanceShedDoorC);

                int shedChance = 80;

                string[] GUIDs = { "bbb36907-bb53-41ec-a5cb-e6d362580663", "2c22cb32-5b14-4a1c-9fef-3d6a9c33ee5d", "ea186e44-18ac-4117-9c46-348158fb0e25s" };

                GameObject[] doors = { maintenanceShedDoorAobj, maintenanceShedDoorBobj, maintenanceShedDoorCobj };
                var i = 0;
                foreach (var door in doors)
                {
                    lockManager.InitializeCustomLock(door, ref shedChance, lockManager.metalDoorLockedAudio, GUIDs[i], new List<string> { Items.prybar });
                    i++;
                }
            }
            else if (GameManager.m_ActiveScene == "MaintenanceShedA")
            {
                //lock Maintenance shed interior doors

                GameObject maintenanceShedDoorAobj = GameObject.Find(Paths.maintenanceShedDoorAInterior);
                GameObject maintenanceShedDoorBobj = GameObject.Find(Paths.maintenanceShedDoorBInterior);
                GameObject maintenanceShedDoorCobj = GameObject.Find(Paths.maintenanceShedDoorCInterior);

                int shedChance = 80;

                string[] GUIDs = { "575f6158-c639-4bc0-86db-ba39e5f108ce", "2a871b6f-8d5d-4d2b-812f-adbcf2b3d2dc", "6d63dd98-a94c-4b71-80e3-99aeb3da354b" };

                GameObject[] doors = { maintenanceShedDoorAobj, maintenanceShedDoorBobj, maintenanceShedDoorCobj };
               
                var i = 0;
                foreach (var door in doors)
                {
                    lockManager.InitializeCustomLock(door, ref shedChance, lockManager.metalDoorLockedAudio, GUIDs[i], new List<string> {Items.prybar});
                    i++;
                }

            }
        }
        public static void AddDesolationPointLocks()
        {
            if (GameManager.m_ActiveScene == "WhalingStationRegion")
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
                    int mineGateChance = 40;

                    lockManager.InitializeCustomLock(mine5door1, ref mineGateChance, lockManager.metalChainDoorLockedAudio, "27abfb4e-d322-4b64-b1a8-d382cee053af", new List<string> { Items.hacksaw });
                    lockManager.InitializeCustomLock(mine5door2, ref mineGateChance, lockManager.metalChainDoorLockedAudio, "a358f1a0-1af4-4faa-9f00-0b77fa1ff114", new List<string> { Items.hacksaw });
                }

                //Lock Hibernia processing
                GameObject hiberniaFrontDoor = GameObject.Find(Paths.hiberniaFrontDoor);
                GameObject hiberniaBackDoor = GameObject.Find(Paths.hiberniaBackDoor);

                int hiberniaChance = 60;

                lockManager.InitializeCustomLock(hiberniaFrontDoor, ref hiberniaChance, lockManager.metalDoorLockedAudio, "5805f636-2b63-4bda-a468-750bef9b0ac6", new List<string> { Items.prybar });
                lockManager.InitializeCustomLock(hiberniaBackDoor, ref hiberniaChance, lockManager.metalDoorLockedAudio, "61299ab5-b6ac-420a-bbdc-4e4787c7b461", new List<string> { Items.prybar });

            }
            else if (GameManager.m_ActiveScene == "WhalingWarehouseA")
            {

                //Lock interior Hibernia doors
                GameObject hiberniaFrontDoor = GameObject.Find(Paths.hiberniaFrontDoorInt);
                GameObject hiberniaBackDoor = GameObject.Find(Paths.hiberniaBackDoorInt);

                int hiberniaChance = 60;

                lockManager.InitializeCustomLock(hiberniaFrontDoor, ref hiberniaChance, lockManager.metalDoorLockedAudio, "8497788f-28d1-4a17-baab-f3a6b98100f2", new List<string> { Items.prybar });
                lockManager.InitializeCustomLock(hiberniaBackDoor, ref hiberniaChance, lockManager.metalDoorLockedAudio, "ed87b39e-addb-457a-8abf-81d7bfa4a97e", new List<string> { Items.prybar });

            }
            else if (GameManager.m_ActiveScene == "WhalingMine")
            {

                //Lock Mine 5 interior doors
                GameObject mine5door1Int = GameObject.Find(Paths.mine5door1Interior);
                GameObject mine5door2Int = GameObject.Find(Paths.mine5door2Interior);

                int mineGateChance = 40;

                lockManager.InitializeCustomLock(mine5door1Int, ref mineGateChance, lockManager.metalChainDoorLockedAudio, "d61e4ad1-6e4a-47bf-a5ed-696070a88399", new List<string> { Items.hacksaw });
                lockManager.InitializeCustomLock(mine5door2Int, ref mineGateChance, lockManager.metalChainDoorLockedAudio, "1d06bcf8-fc72-4c47-ad3d-ed2f5455d5b2", new List<string> { Items.hacksaw });

            }
        }
        public static void AddCoastalHighwayLocks()
        {
            if (GameManager.m_ActiveScene == "CoastalRegion")
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
                    else if (obj.GetComponent<ObjectGuid>().PDID == "d6667072-a207-41d8-8f4c-f6b1804baaf2")
                    {
                        fishingCabinDoorExt = obj;
                    }
                }

                //Lock Quonset Gas Station Exterior

                if (quonsetFrontDoor != null && quonsetBackDoor != null)
                {
                    int quonsetChance = 75; 

                    lockManager.InitializeCustomLock(quonsetFrontDoor, ref quonsetChance, lockManager.metalDoorLockedAudio, "8939b09a-3b9b-4b2c-9ced-cde4bc8ad171", new List<string> { Items.prybar });
                    lockManager.InitializeCustomLock(quonsetBackDoor, ref quonsetChance, lockManager.metalDoorLockedAudio, "9991f9bb-0c1a-48b3-81a8-8ce883ccd389", new List<string> { Items.prybar });
                }

                //Lock Cinder Hills Coal Mine Exterior CH
                GameObject cinderHillsCoalMineDoor = GameObject.Find(Paths.cinderHillsCoalMineExteriorDoor);
                int cinderHillsChance = 90;
                lockManager.InitializeCustomLock(cinderHillsCoalMineDoor, ref cinderHillsChance, lockManager.metalChainDoorLockedAudio, "27abfb4e-d322-4b64-b1a8-d382cee053af", new List<string> { Items.hacksaw });

                //Lock Fishing Cabin

                if (fishingCabinDoorExt != null)
                {
                    int fishingCabinChance = 70;
                    lockManager.InitializeCustomLock(fishingCabinDoorExt, ref fishingCabinChance, lockManager.woodDoorLockedAudio, "", new List<string> { Items.fishingCabinKey, Items.hatchet });
                }

            }
            else if (GameManager.m_ActiveScene == "QuonsetGasStation")
            {
                //Lock quonset interior door
                GameObject quonsetFrontDoor = GameObject.Find(Paths.quonsetGasStationInteriorFrontDoor);
                GameObject quonsetBackDoor = GameObject.Find(Paths.quonsetGasStationInteriorBackDoor);

                int quonsetChance = 75;

                lockManager.InitializeCustomLock(quonsetFrontDoor, ref quonsetChance, lockManager.metalDoorLockedAudio, "a40c7e13-86b4-4a04-9a50-16f78bd14d8e", new List<string> { Items.prybar });
                lockManager.InitializeCustomLock(quonsetBackDoor, ref quonsetChance, lockManager.metalDoorLockedAudio, "e6910ab2-75ec-4df5-adb2-8f3a4c76ecaa", new List<string> { Items.prybar });
            }
        }
        public static void AddCinderHillsCoalMineLocks()
        {
            if (GameManager.m_ActiveScene == "MineTransitionZone")
            {
                //Lock mine exits to CH and PV
                GameObject cinderHillsCoalMineDoorCH = GameObject.Find(Paths.cinderHillsCoalMineInteriorDoorCH);
                int cinderHillsCoalMineChance = 90;
                lockManager.InitializeCustomLock(cinderHillsCoalMineDoorCH, ref cinderHillsCoalMineChance, lockManager.metalChainDoorLockedAudio, "adb77a85-783e-4fc6-bd50-07bd264efe44", new List<string> { Items.hacksaw });
            }
        }
        public static void AddPleasantValleyLocks()
        {
            if (GameManager.m_ActiveScene == "RuralRegion")
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

                int farmhouseChance = 80;
                int farmhouseCellarChance = 60;

                if (farmhouseDoor1 != null && farmhouseDoor2 != null && farmhouseDoor3 != null && farmhouseCellarDoor != null)
                {
                    lockManager.InitializeCustomLock(farmhouseDoor1, ref farmhouseChance, lockManager.woodDoorLockedAudio, "fb7c5b5d-e88d-489e-a1c2-ccbdc76e5da2",  new List<string> { Items.farmKey, Items.hatchet });
                    lockManager.InitializeCustomLock(farmhouseDoor2, ref farmhouseChance, lockManager.woodDoorLockedAudio, "0b9a4999-4125-4c11-bb13-d48ed81546c9", new List<string> { Items.farmKey, Items.hatchet });
                    lockManager.InitializeCustomLock(farmhouseDoor3, ref farmhouseChance, lockManager.woodDoorLockedAudio, "2edd5f53-8533-4780-b0dd-eeec30a744d5", new List<string> { Items.farmKey, Items.hatchet });
                    lockManager.InitializeCustomLock(farmhouseCellarDoor, ref farmhouseCellarChance, lockManager.metalDoorLockedAudio, "e62f85fd-edc9-4aaf-ae59-7b893b06abe9", new List<string> { Items.prybar });
                }

                //Lock Community Hall exterior

                GameObject communityHallFrontDoor = null;
                GameObject communityHallBackDoor = null;

                int communityHallChance = 70;

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

                if (communityHallFrontDoor != null && communityHallBackDoor != null)
                {
                    lockManager.InitializeCustomLock(communityHallFrontDoor, ref communityHallChance, lockManager.woodDoorLockedAudio, "2d1702f8-5626-444f-b36e-f9542899d606",  new List<string> { Items.communityHallKey });
                    lockManager.InitializeCustomLock(communityHallBackDoor, ref communityHallChance, lockManager.woodDoorLockedAudio, "7fa2c843-3a37-497f-bb16-3083be100f96", new List<string> { Items.communityHallKey });
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
                    int ruralStoreChance = 80;

                    lockManager.InitializeCustomLock(ruralStoreFrontDoor, ref ruralStoreChance, lockManager.woodDoorLockedAudio, "c17a1359-75ee-43fa-ace4-b76819fdaa9b",  new List<string> { Items.prybar });
                    lockManager.InitializeCustomLock(ruralStoreBackDoor, ref ruralStoreChance, lockManager.woodDoorLockedAudio, "138894f7-7fd0-4c97-be36-1f350cb93499", new List<string> { Items.prybar });
                }

                //Lock Radio Control Hut
                GameObject signalHillDoor = GameObject.Find(Paths.signalHillDoor);
                int signalHillChance = 80;
                lockManager.InitializeCustomLock(signalHillDoor, ref signalHillChance, lockManager.metalDoorLockedAudio, "", new List<string> { Items.prybar });

                //lock Barnhouse
                GameObject barnhouseDoor = GameObject.Find(Paths.barnhouseDoorExt);
                int barnhouseLockChance = 40;
                lockManager.InitializeCustomLock(barnhouseDoor, ref barnhouseLockChance, lockManager.woodDoorLockedAudio, "", new List<string> { Items.hatchet });
            }
            else if (GameManager.m_ActiveScene == "RuralStoreA")
            {

                //Lock Rural store interior
                GameObject ruralStoreFrontDoor = GameObject.Find(Paths.ruralStoreFrontDoorInt);
                GameObject ruralStoreBackDoor = GameObject.Find(Paths.ruralStoreBackDoorInt);

                int chance = 70;

                lockManager.InitializeCustomLock(ruralStoreFrontDoor, ref chance, lockManager.woodDoorLockedAudio, "e5cb7bfa-5c37-4fef-a57e-7fac7174aaa3", new List<string> { Items.prybar });
                lockManager.InitializeCustomLock(ruralStoreBackDoor, ref chance, lockManager.woodDoorLockedAudio, "30ce35d1-f08b-43e4-abdd-c8f226352afc", new List<string> { Items.prybar });

            }
            else if (GameManager.m_ActiveScene == "CommunityHallA")
            {

                GameObject communityHallFrontDoorInt = GameObject.Find(Paths.communityHallFrontDoorInt);
                GameObject communityHallBackDoorInt = GameObject.Find(Paths.communityHallBackDoorInt);

                int chance = 70;

                lockManager.InitializeCustomLock(communityHallFrontDoorInt, ref chance, lockManager.woodDoorLockedAudio, "a30c0c66-7117-4359-bcad-f7b75ea1ccaf", new List<string> { Items.communityHallKey });
                lockManager.InitializeCustomLock(communityHallBackDoorInt, ref chance, lockManager.woodDoorLockedAudio, "87d6326a-f32b-4e87-968e-39b48d9e1a3f", new List<string> { Items.communityHallKey });
            }
            else if (GameManager.m_ActiveScene == "FarmHouseA")
            {

                var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "ExteriorLoadTrigger");

                GameObject farmhouseBasementDoorInt = GameObject.Find(Paths.farmhouseBasementDoorHouse);

                GameObject farmhouseDoorInt1 = null;
                GameObject farmhouseDoorInt2 = null;
                GameObject farmhouseDoorInt3 = null;

                int farmhouseChance = 80;

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

                if (farmhouseDoorInt1 != null && farmhouseDoorInt2 != null && farmhouseDoorInt3 != null && farmhouseBasementDoorInt != null)
                {
                    lockManager.InitializeCustomLock(farmhouseDoorInt1, ref farmhouseChance, lockManager.woodDoorLockedAudio, "9769a1de-8021-4585-9648-bc439aa334fd", new List<string> { Items.farmKey, Items.hatchet });
                    lockManager.InitializeCustomLock(farmhouseDoorInt2, ref farmhouseChance, lockManager.woodDoorLockedAudio, "57607823-0142-409a-aa92-893f35dc5b8a", new List<string> { Items.farmKey, Items.hatchet });
                    lockManager.InitializeCustomLock(farmhouseDoorInt3, ref farmhouseChance, lockManager.woodDoorLockedAudio, "40b60936-4e53-4b76-b127-43ffd123fcb0", new List<string> { Items.farmKey, Items.hatchet });
                    lockManager.InitializeCustomLock(farmhouseBasementDoorInt, ref farmhouseChance, lockManager.woodDoorLockedAudio, "c42cd462-fa07-4ccb-9bea-e611224260ee", new List<string> { Items.farmKey });
                }
                else
                {
                    MelonLogger.Msg("Farmhouse interior door objects not found.");
                }

            }
            else if (GameManager.m_ActiveScene == "FarmHouseABasement")
            {

                GameObject basementDoorBasement = GameObject.Find(Paths.farmhouseBasementDoorBasement);
                GameObject farmhouseCellarDoorInt = GameObject.Find(Paths.farmhouseCellarDoorInt);

                int cellarChance = 60;
                int doorChance = 80;

                lockManager.InitializeCustomLock(basementDoorBasement, ref doorChance, lockManager.woodDoorLockedAudio, "396c8857-9d29-4c6d-aa7b-ba7e839c6cec", new List<string> { Items.farmKey });
                lockManager.InitializeCustomLock(farmhouseCellarDoorInt, ref cellarChance, lockManager.metalDoorLockedAudio, "ee96b557-caa7-4d96-89d3-79ebc406b8b5", new List<string> { Items.prybar });

            }
            else if(GameManager.m_ActiveScene == "BarnHouseB")
            {
                GameObject barnhouseDoorInt = GameObject.Find(Paths.barnhouseDoorInt);
                int chance = 40;
                lockManager.InitializeCustomLock(barnhouseDoorInt, ref chance, lockManager.woodDoorLockedAudio, "", new List<string> { Items.hatchet });
            }
        }
        public static void AddBlackrockLocks()
        {
            if (GameManager.m_ActiveScene == "BlackrockRegion")
            {

                int chance = 60;

                GameObject substationDoor = GameObject.Find(Paths.huntingLodgeGate);
                lockManager.InitializeCustomLock(substationDoor, ref chance, lockManager.metalDoorLockedAudio, "f4bc3103-7108-4d80-9b0d-2e83864983c7", new List<string> { Items.prybar });
            }
            else if (GameManager.m_ActiveScene == "RadioControlHutC")
            {
                int chance = 0;

                GameObject substationDoor = GameObject.Find(Paths.substationDoorInt);
                lockManager.InitializeCustomLock(substationDoor, ref chance, lockManager.metalDoorLockedAudio, "a0fb16ec-8fb0-48e1-aa38-8e4760b24410", new List<string> { Items.prybar });
            }
        } 
        public static void AddAshCanyonLocks()
        {
            if (GameManager.m_ActiveScene == "AshCanyonRegion")
            {
                GameObject anglersDenDoorExt = GameObject.Find(Paths.anglersDenDoorExt);
                int chance = 60;
                lockManager.InitializeCustomLock(anglersDenDoorExt, ref chance, lockManager.woodDoorLockedAudio, "7a33c7df-02cc-49a3-8766-9fa022979c50", new List<string> { Items.anglersDenKey });
            }
        } 
        public static void AddForceItemLockComponents()
        {

            var customObjects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name.Contains("MoreLockedDoors"));
            var vanillaObjects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name.Contains("Hatchet"));

            LocalizedString progressText = new();
            progressText.m_LocalizationID = "GAMEPLAY_Unlocking";

            if (customObjects == null || customObjects.Count() < 1)
            {
                return;
            }

            foreach (var obj in customObjects)
            {
                obj.AddComponent<ForceLockItem>();
                ForceLockItem comp = obj.GetComponent<ForceLockItem>();
                comp.m_ForceLockAudio = "Play_WoodDoorKey";
                comp.m_LocalizedProgressText = progressText;
            }

            if (vanillaObjects == null || vanillaObjects.Count() < 1)
            {
                return;
            }

            foreach (var obj in vanillaObjects)
            {
                obj.AddComponent<ForceLockItem>();
                ForceLockItem comp = obj.GetComponent<ForceLockItem>();

                comp.m_ForceLockAudio = "Play_WoodDoorKey";
                comp.m_LocalizedProgressText = progressText;

                if (obj.name.Contains("Hatchet"))
                {
                    progressText.m_LocalizationID = "Chopping...";

                    comp.m_ForceLockAudio = "PLAY_HARVESTINGWOODRECLAIMED";
                    comp.m_LocalizedProgressText = progressText;
                }
            }

        }
        public static void ForceUnlockAllVanillaLocks(GameObject sourceObj)
        {

            if (sourceObj.GetComponent<Lock>())
            {
                sourceObj.GetComponent<Lock>().m_LockState = LockState.Unlocked;
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
    }
}


