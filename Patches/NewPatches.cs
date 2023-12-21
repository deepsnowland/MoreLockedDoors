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

            public static void Prefix(LoadScene __instance)
            {
                MelonLogger.Msg("Performing hold action...");
                __instance.gameObject.GetComponent<CustomLock>().StartInteract();
            }

        }

        [HarmonyPatch(typeof(ForceLockItem), nameof(ForceLockItem.Start))]

        public class AddHatchetUnlockAudio
        {

            public static void Postfix(ForceLockItem __instance)
            {

                if (__instance.GetComponent<GearItem>().name.ToLowerInvariant().Contains("hatchet"))
                {
                    //set this to hatchet wood chopping audio
                    __instance.m_ForceLockAudio = "";
                }

            }

        }


    }
}
