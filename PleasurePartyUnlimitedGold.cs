using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using HarmonyLib;
using System;

namespace PleasurePartyUnlimitedGold
{
    [BepInPlugin("com.wolfitdm.PleasurePartyUnlimitedGold", "PleasurePartyUnlimitedGold Plugin", "1.0.0.0")]
    public class PleasurePartyUnlimitedGold : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;
        public PleasurePartyUnlimitedGold()
        {
        }

        public static Type MyGetType(string originalClassName)
        {
            return Type.GetType(originalClassName + ",Assembly-CSharp");
        }
        private void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;
            Harmony.CreateAndPatchAll(typeof(PleasurePartyUnlimitedGold));
            Logger.LogInfo($"Plugin PleasurePartyUnlimitedGold is loaded!");
        }

        [HarmonyPatch(typeof(Assets.FantasyInventory.Scripts.Interface.Shop), "loadInventory")] // Specify target method with HarmonyPatch attribute
        [HarmonyPostfix]                              // There are different patch types. Prefix code runs before original code
        static void loadIventory(object __instance)
        {
            Assets.FantasyInventory.Scripts.Interface.Shop _this = (Assets.FantasyInventory.Scripts.Interface.Shop)__instance;
            _this.inventory.Add(new Assets.FantasyInventory.Scripts.Data.Item(Assets.FantasyInventory.Scripts.Enums.ItemId.Gold, 100000));
        }
    }
}