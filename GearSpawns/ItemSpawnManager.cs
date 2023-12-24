using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using GearSpawner;
using Il2CppNodeCanvas.Tasks.Actions;
using MoreLockedDoors.Utils;
using MoreLockedDoors.GearSpawns.CustomHelperClasses;

namespace MoreLockedDoors.GearSpawns
{
    internal class ItemSpawnManager
    {
        public static void InitializeCustomHandler()
        {
            SpawnTagManager.AddHandler("morelockeddoors_randomunlockitems", new OneGuaranteedItemSpawnHandler());
            SpawnTagManager.AddHandler("morelockeddoors_randomkeys", new KeySpawnHandler());
        }

        public static void ChooseSpawnLocationForKey(string keyName)
        {

            SaveDataManager sdm = Implementation.sdm;

            if (sdm.LoadKeyData(keyName) != null)
            {
                return;
            }

            Random random = new Random();
            string sceneName = "";

            string rawSpawnTextData = GearSpawnerOverrides.ReadRawDataFromGearSpawnFile("keys");
            string[] rawLines = GearSpawnerOverrides.ParseInformationToLines(rawSpawnTextData);


            if (keyName == "GEAR_MoreLockedDoors_ML_CampOfficeKey")
            {
                int campOfficeKeySpawnChoice = random.Next(3);

                if (campOfficeKeySpawnChoice == 0)
                {
                    sceneName = "LakeRegion";
                }
                else if (campOfficeKeySpawnChoice == 1)
                {
                    sceneName = "LakeCabinB";
                }
                else if (campOfficeKeySpawnChoice == 2)
                {
                    sceneName = "LakeCabinE";
                }
            }
            else if (keyName == "GEAR_MoreLockedDoors_PV_CommunityHallKey")
            {
                int communityHallKeySpawnChoice = random.Next(2);

                if (communityHallKeySpawnChoice == 0)
                {
                    sceneName = "RuralRegion";
                }
                else if (communityHallKeySpawnChoice == 1)
                {
                    sceneName = "ChurchC";
                }
            }
            else if (keyName == "GEAR_MoreLockedDoors_PV_FarmKey")
            {
                int farmhouseKeySpawnChoice = random.Next(3);

                if (farmhouseKeySpawnChoice == 0)
                {
                    sceneName = "RuralRegion";
                }
                else if (farmhouseKeySpawnChoice == 1)
                {
                    sceneName = "BarnHouseB";
                }
                else if (farmhouseKeySpawnChoice == 2)
                {
                    sceneName = "FarmHouseABasement";
                }
            }
            else if (keyName == "GEAR_MoreLockedDoors_CH_FishingCabinKey")
            {
                int fishingCabinKeySpawnChoice = random.Next(2);

                if (fishingCabinKeySpawnChoice == 0)
                {
                    sceneName = "CoastalRegion";
                }
                else if (fishingCabinKeySpawnChoice == 1)
                {
                    sceneName = "FishingCabinC";
                }
            }
            else if (keyName == "GEAR_MoreLockedDoors_AC_AnglersDenKey")
            {
                sceneName = "AshCanyonRegion";
            }

            List<GearSpawnInfo> list = GearSpawnerOverrides.GetListOfGearSpawnInfos(rawLines, sceneName);
            int spawnChoice = random.Next(0, list.Count - 1);

            MelonLogger.Msg("Chosen scene: {0}", sceneName);

            GearSpawnInfo chosen = list[spawnChoice];
            Vector3Ser pos = new Vector3Ser(chosen.Position);
            QuaternionSer rot = new QuaternionSer(chosen.Rotation);
            CustomGearSpawnInfo cgsi = new CustomGearSpawnInfo(chosen.Tag, pos, chosen.PrefabName, rot, chosen.SpawnChance);
            KeySpawnSaveDataProxy sdp = new KeySpawnSaveDataProxy(sceneName, cgsi);
            sdm.SaveKeyData(sdp, keyName);

        }

        public static bool AreGearSpawnInfosEqual(GearSpawnInfo gsi1, GearSpawnInfo gsi2)
        {

            if (gsi1 == null || gsi2 == null || gsi1.GetType() != gsi2.GetType())
            {
                return false;
            }

            FieldInfo[] fields = gsi1.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                // Compare each field's value
                object value1 = field.GetValue(gsi1);
                object value2 = field.GetValue(gsi2);

                if (!object.Equals(value1, value2))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
