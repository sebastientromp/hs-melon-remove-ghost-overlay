using MelonLoader;
using HarmonyLib;
using System.Linq;
using System;
using UnityEngine;

namespace RemoveGhostOverlayWhenExaminingCard
{
    public class RemoveGhostOverlayWhenExaminingCardMod : MelonMod
    {
        public static MelonLogger.Instance SharedLogger;

        public static string currentBobSkin = null;

        public override void OnInitializeMelon()
        {
            RemoveGhostOverlayWhenExaminingCardMod.SharedLogger = LoggerInstance;
            var harmony = this.HarmonyInstance;
            harmony.PatchAll(typeof(LoadGhostActorIfNecessaryPatcher));
        }
    } 

    public static class LoadGhostActorIfNecessaryPatcher
    {
        [HarmonyPatch(typeof(CraftingManager), "GetAndPositionNewActor")]
        [HarmonyPrefix]
        public static bool GetAndPositionNewActorPrefix(CraftingManager __instance, Actor oldActor, int numCopies, ref Actor __result)
        {
            RemoveGhostOverlayWhenExaminingCardMod.SharedLogger.Msg($"Showing non-ghost version of the card");
            var nonGhostActorMethod = AccessTools.Method(typeof(CraftingManager), "GetNonGhostActor");
            Actor actor = nonGhostActorMethod.Invoke(__instance, new object[] { oldActor }) as Actor;
            if (actor != null)
            {
                actor.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            __result = actor;
            return false;
        }
    }
}
