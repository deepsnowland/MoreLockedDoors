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
using MoreLockedDoors.Utils;
using Il2CppTLD.IntBackedUnit;

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

        [HarmonyPatch(typeof(Weather), nameof(Weather.CalculateCurrentTemperature))]
        internal class DropTempIndoorsIfDoorsAreBrokenOpen
        {
            static void Prefix(Weather __instance)
            {

                if (Systems.IsAssemblyPresent("CoolHome") || !__instance.IsIndoorEnvironment()) return;

                CustomLock[] locks = GameObject.FindObjectsOfType<CustomLock>();

                foreach (var lck in locks)
                {
                    if (lck.IsBrokenOpen())
                    {
                        __instance.m_IndoorTemperatureCelsius = __instance.GetBaseTemperature();
                        break;
                    }
                }

            }
        }

    }
}
