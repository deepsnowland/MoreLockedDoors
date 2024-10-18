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
using Il2CppTLD.IntBackedUnit;

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
                AddForceItemLockComponents();
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



        [HarmonyPatch(typeof(LoadScene), nameof(LoadScene.PerformHold))]

        public class ForceLockOnSceneLoadTrigger
        {

            public static bool Prefix(LoadScene __instance)
            {

                if (__instance.gameObject.GetComponent<CustomLock>())
                {
                    if (__instance.gameObject.GetComponent<CustomLock>().IsLocked())
                    {
                        __instance.gameObject.GetComponent<CustomLock>().UnlockBegin();
                        return false;
                    }
                    else return true;
                }
                else return true;
            }

        }

        [HarmonyPatch(typeof(OpenClose), nameof(OpenClose.PerformHold))]

        public class ForceLockOnHinge
        {
            public static bool Prefix(OpenClose __instance)
            {

                if (__instance.gameObject.GetComponent<CustomLock>())
                {
                    if (__instance.gameObject.GetComponent<CustomLock>().IsLocked())
                    {
                        __instance.gameObject.GetComponent<CustomLock>().UnlockBegin();
                        return false;
                    }
                    else return true;
                }
                else return true;
            }
        }


        [HarmonyPatch(typeof(Panel_HUD), nameof(Panel_HUD.SetHoverText))]

        public class AddLockedHoverText
        {

            public static void Prefix(ref string hoverText, ref GameObject itemUnderCrosshairs, ref HoverTextState textState, Panel_HUD __instance)
            {

                if (GameManager.IsMainMenuActive()) return;
                if (hoverText == null || itemUnderCrosshairs == null) return;

                CustomLock cl = itemUnderCrosshairs.GetComponent<CustomLock>();
                if (cl != null)
                {
                    if (cl.IsLocked())
                    {
                        __instance.m_Label_SubText.color = Color.red;
                        hoverText += "\n Locked";
                    }
                }

            }

        }

        [HarmonyPatch(typeof(GearItem), nameof(GearItem.Awake))]

        public class RemoveCollectibleFromBoltcutters : MonoBehaviour
        {

            public static void Postfix(GearItem __instance)
            {
                if (__instance.DisplayName.ToLowerInvariant().Contains("bolt"))
                {
                    Destroy(__instance.GetComponent<NarrativeCollectibleItem>());
                    __instance.m_GearItemData.m_BaseWeight = new ItemWeight(1500000000);
                }


            }
        }
    }
}



