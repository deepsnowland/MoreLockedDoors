using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using MoreLockedDoors.Locks;
using Unity.VisualScripting;

namespace MoreLockedDoors.Patches
{
    internal class NewPatches
    {

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

                if (GameManager.m_ActiveScene.ToLowerInvariant().Contains("menu") || GameManager.m_ActiveScene.ToLowerInvariant().Contains("boot")) return;
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

        public class AddHatchetUnlockItem
        {

            public static void Postfix(GearItem __instance)
            {

                if (GameManager.IsMainMenuActive() || GameManager.IsBootSceneActive() || GameManager.IsEmptySceneActive() || GameManager.m_ActiveScene == null) return;

                if (__instance.name.ToLowerInvariant().Contains("hatchet"))
                {
                    if (!__instance.gameObject.GetComponent<ForceLockItem>())
                    {
                        MelonLogger.Msg("hatchet doesn't have force lock item, adding...");
                        __instance.gameObject.AddComponent<ForceLockItem>();
                        __instance.gameObject.GetComponent<ForceLockItem>().m_LocalizedProgressText = new LocalizedString();
                        __instance.gameObject.GetComponent<ForceLockItem>().m_LocalizedProgressText.m_LocalizationID = "Chopping...";
                        __instance.gameObject.GetComponent<ForceLockItem>().m_ForceLockAudio = "PLAY_HARVESTINGWOODRECLAIMED"; //no audio yet

                        __instance.m_ForceLockItem = __instance.gameObject.GetComponent<ForceLockItem>();
                    }

                }

            }

        }


        //testing patches



    }
}
