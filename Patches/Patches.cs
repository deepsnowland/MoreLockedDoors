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


