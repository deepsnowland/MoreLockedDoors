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
                MelonLogger.Msg("Found key data. Keeping it.");
                return;
            }

            Random random = new Random();
            string sceneName = "";

            string rawSpawnTextData = GearSpawnerOverrides.ReadRawDataFromGearSpawnFile("keys");
            string[] rawLines = GearSpawnerOverrides.ParseInformationToLines(rawSpawnTextData);

            KeySpawnSaveDataProxy sdp = null;

            if (keyName == "GEAR_MoreLockedDoors_ML_CampOfficeKey")
            {
                int indoorOutdoorChoice = random.Next(3);

                MelonLogger.Msg("Choice: {0}", indoorOutdoorChoice);

                if (indoorOutdoorChoice == 0)
                {
                    sceneName = "LakeRegion";
                }
                else if (indoorOutdoorChoice == 1)
                {
                    sceneName = "LakeCabinB"; 
                }
                else if (indoorOutdoorChoice == 2)
                {
                    sceneName = "LakeCabinE"; 
                }
            }
            else if(keyName == "GEAR_MoreLockedDoors_PV_CommunityHallKey" || keyName == "GEAR_MoreLockedDoors_PV_FarmKey")
            {
                //int indoorOutdoorChoice = random.Next(3);

                int indoorOutdoorChoice = 0;

                if (indoorOutdoorChoice == 0)
                {
                    sceneName = "RuralRegion";
                }
                else if (indoorOutdoorChoice == 1)
                {
                    sceneName = ""; 
                }
                else if (indoorOutdoorChoice == 2)
                {
                    sceneName = ""; 
                }
            }
            else if (keyName == "GEAR_MoreLockedDoors_CH_FishingCabinKey")
            {
                //int indoorOutdoorChoice = random.Next(3);

                int indoorOutdoorChoice = 0;

                if (indoorOutdoorChoice == 0)
                {
                    sceneName = "CoastalRegion";
                }
                else if (indoorOutdoorChoice == 1)
                {
                    sceneName = ""; 
                }
                else if (indoorOutdoorChoice == 2)
                {
                    sceneName = "";
                }
            }
            else if (keyName == "GEAR_MoreLockedDoors_AC_AnglersDenKey")
            {
                //int indoorOutdoorChoice = random.Next(3);

                int indoorOutdoorChoice = 0;

                if (indoorOutdoorChoice == 0)
                {
                    sceneName = "AshCanyonRegion";
                }
                else if (indoorOutdoorChoice == 1)
                {
                    sceneName = ""; 
                }
                else if (indoorOutdoorChoice == 2)
                {
                    sceneName = ""; 
                }
            }

            List<GearSpawnInfo> list = GearSpawnerOverrides.GetListOfGearSpawnInfos(rawLines, sceneName);
            int spawnChoice = random.Next(0, list.Count - 1);

            GearSpawnInfo chosen = list[spawnChoice];
            Vector3Ser pos = new Vector3Ser(chosen.Position);
            QuaternionSer rot = new QuaternionSer(chosen.Rotation);
            CustomGearSpawnInfo cgsi = new CustomGearSpawnInfo(chosen.Tag, pos, chosen.PrefabName, rot, chosen.SpawnChance);
            sdp = new KeySpawnSaveDataProxy(sceneName, cgsi);
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
