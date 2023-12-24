using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GearSpawner;
using Il2Cpp;
using UnityEngine;
using MelonLoader;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Reflection;


namespace MoreLockedDoors.Utils
{
    internal class GearSpawnerOverrides
    {

        // language=regex
        private const string NUMBER = @"-?\d+(?:\.\d+)?";
        // language=regex
        private const string VECTOR = NUMBER + @"\s*,\s*" + NUMBER + @"\s*,\s*" + NUMBER;

        private static readonly Regex LOOTTABLE_ENTRY_REGEX = new Regex(@"^item\s*=\s*(\w+)" + @"\W+w\s*=\s*(" + NUMBER + ")$", RegexOptions.Compiled);
        private static readonly Regex LOOTTABLE_REGEX = new Regex(@"^loottable\s*=\s*(\w+)$", RegexOptions.Compiled);
        private static readonly Regex SCENE_REGEX = new Regex(@"^scene\s*=\s*(\w+)$", RegexOptions.Compiled);
        private static readonly Regex TAG_REGEX = new Regex(@"^tag\s*=\s*(\w+)$", RegexOptions.Compiled);

        private static readonly Regex SPAWN_REGEX = new Regex(
            @"^item\s*=\s*(\w+)" +
            @"(?:\W+p\s*=\s*(" + VECTOR + "))?" +
            @"(?:\W+r\s*=\s*(" + VECTOR + "))?" +
            @"(?:\W+\s*c\s*=\s*(" + NUMBER + "))?$", RegexOptions.Compiled);

        public static string ReadRawDataFromGearSpawnFile(string fileName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "MoreLockedDoors.morelockeddoors.gear_spawns." + fileName + ".txt";
            string fileContents;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    Console.WriteLine($"Resource '{resourceName}' not found.");
                    return "";
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    // Read the contents of the file
                    fileContents = reader.ReadToEnd();
                }
            }


           

            return fileContents;
        }

        public static string[] ParseInformationToLines(string text)
        {
            string[] lines = Regex.Split(text, "\r\n|\r|\n");
            return lines;
        }

        public static List<GearSpawnInfo> GetListOfGearSpawnInfos(string[] lines, string sceneName)
        {

            string? scene = null;
            string tag = "none";

            List<GearSpawnInfo> listToReturn = new List<GearSpawnInfo>();

            foreach (string eachLine in lines)
            {
                string trimmedLine = eachLine.Trim();
                if (trimmedLine.Length == 0 || trimmedLine.StartsWith("#"))
                {
                    continue;
                }

                var match = SCENE_REGEX.Match(trimmedLine);
                if (match.Success)
                {
                    scene = match.Groups[1].Value;
                    continue;
                }

                match = TAG_REGEX.Match(trimmedLine);
                if (match.Success)
                {
                    tag = match.Groups[1].Value;
                    continue;
                }

                match = SPAWN_REGEX.Match(trimmedLine);
                if (match.Success)
                {
                    if (string.IsNullOrEmpty(scene))
                    {
                        MelonLogger.Error($"No scene name defined before line '{eachLine}'. Did you forget a 'scene = <SceneName>'?");
                    }

                    GearSpawnInfo info = new GearSpawnInfo
                    {
                        PrefabName = match.Groups[1].Value,
                        SpawnChance = ParseFloat(match.Groups[4].Value, 100, eachLine),
                        Position = ParseVector(match.Groups[2].Value, eachLine),
                        Rotation = Quaternion.Euler(ParseVector(match.Groups[3].Value, eachLine)),
                        Tag = tag
                    };

                    if (scene == sceneName) 
                    {
                        listToReturn.Add(info);
                    }

                    continue;
                }
            }

            return listToReturn;
        }



        private static float ParseFloat(string value, float defaultValue, string line)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            try
            {
                return float.Parse(value, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new ArgumentException($"Could not parse '{value}' as numeric value in line {line}.");
            }
        }

        private static int ParseInt(string value, int defaultValue, string line)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            try
            {
                return int.Parse(value);
            }
            catch (Exception)
            {
                throw new ArgumentException($"Could not parse '{value}' as numeric value in line {line}.");
            }
        }


        //stuff i copied from GearSpawner
        private static Vector3 ParseVector(string value, string line)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Vector3.zero;
            }

            string[] components = value.Split(',');
            if (components.Length != 3)
            {
                throw new ArgumentException($"A vector requires 3 components, but found {components.Length} in line '{line}'.");
            }

            return new Vector3(
                ParseFloat(components[0].Trim(), 0, line),
                ParseFloat(components[1].Trim(), 0, line),
                ParseFloat(components[2].Trim(), 0, line));
        }




    }
}
