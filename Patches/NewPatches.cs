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

namespace MoreLockedDoors.Patches
{
    internal class NewPatches
    {

        [HarmonyPatch(typeof(LoadScene), nameof(LoadScene.PerformHold))]

        public class ForceLock
        {

            public static bool Prefix(LoadScene __instance)
            {

                bool canOpen = false;

                if (__instance.gameObject.GetComponent<CustomLock>())
                {
                    __instance.gameObject.GetComponent<CustomLock>().UnlockBegin(ref canOpen);

                    if (!canOpen) return false;
                    else return true;
                }
                return true;
            }

        }

        [HarmonyPatch(typeof(GearItem), nameof(GearItem.Awake))]

        public class AddHatchetUnlockAudio
        {

            public static void Postfix(GearItem __instance)
            {

                if (GameManager.IsMainMenuActive() || GameManager.IsBootSceneActive() || GameManager.IsEmptySceneActive() || GameManager.m_ActiveScene == null) return;
                
                if (__instance.name.ToLowerInvariant().Contains("hatchet"))
                {
                    if (__instance.GetComponent<ForceLockItem>())
                    {
                        __instance.GetComponent<ForceLockItem>().m_ForceLockAudio = "";
                    }
                    
                } 

            }

        }


    }
}
