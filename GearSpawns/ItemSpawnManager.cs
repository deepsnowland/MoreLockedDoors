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

namespace MoreLockedDoors.GearSpawns
{
    internal class ItemSpawnManager
    {
        public static void InitializeCustomHandler()
        {
            SpawnTagManager.AddHandler("morelockeddoors_randomunlockitems", new OneGuaranteedItemSpawnHandler());
            SpawnTagManager.AddHandler("morelockeddoors_randomkeys", new KeySpawnHandler());
            //SpawnTagManager.AddHandler("morelockeddoors_farmhousekey", new OneGuaranteedItemSpawnHandler());
        }

        public static void ChooseSpawnLocationForKey(string keyName)
        {

            SaveDataManager sdm = Implementation.sdm;

            if (sdm.LoadKeyData(keyName) != null) return;

            Random random = new Random();
            string sceneName = "";

            string rawSpawnTextData = GearSpawnerOverrides.ReadRawDataFromGearSpawnFile("keys");
            string[] rawLines = GearSpawnerOverrides.ParseInformationToLines(rawSpawnTextData);

            KeySpawnSaveDataProxy sdp = null;

            if (keyName == "GEAR_MoreLockedDoors_ML_CampOfficeKey")
            {
                //int indoorOutdoorChoice = random.Next(3);

                int indoorOutdoorChoice = 0;

                if (indoorOutdoorChoice == 0)
                {
                    sceneName = "LakeRegion";
                }
                else if (indoorOutdoorChoice == 1)
                {
                    sceneName = ""; //lone lake cabin
                }
                else if (indoorOutdoorChoice == 2)
                {
                    sceneName = ""; //lake cabin whatever
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
                    sceneName = ""; //lone lake cabin
                }
                else if (indoorOutdoorChoice == 2)
                {
                    sceneName = ""; //lake cabin whatever
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
                    sceneName = ""; //lone lake cabin
                }
                else if (indoorOutdoorChoice == 2)
                {
                    sceneName = ""; //lake cabin whatever
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
                    sceneName = ""; //lone lake cabin
                }
                else if (indoorOutdoorChoice == 2)
                {
                    sceneName = ""; //lake cabin whatever
                }
            }

            List<GearSpawnInfo> list = GearSpawnerOverrides.GetListOfGearSpawnInfos(rawLines, sceneName);
            int spawnChoice = random.Next(0, list.Count - 1);
            sdp = new KeySpawnSaveDataProxy(sceneName, list[spawnChoice]);
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
