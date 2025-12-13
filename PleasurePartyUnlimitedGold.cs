using Assets.FantasyInventory.Scripts.Data;
using Assets.FantasyInventory.Scripts.Enums;
using Assets.FantasyInventory.Scripts.Interface;
using Assets.FantasyInventory.Scripts.Interface.Elements;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using HarmonyLib;
using System;
using System.Reflection;
using System.Xml.Linq;
using UnityEngine;

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
        public static void AddMoney(Assets.FantasyInventory.Scripts.Interface.Shop obj, ItemContainer inventory, int value, ItemId currencyId)
        {
            // Retrieve the private method info
            MethodInfo methodInfo = typeof(Assets.FantasyInventory.Scripts.Interface.Shop).GetMethod(
                "AddMoney",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            if (methodInfo == null)
            {
                Logger.LogInfo($"Private method AddMoney not found.");
                return;
            }

            // Prepare parameters for the method
            object[] parameters = { inventory, value, currencyId };

            // Invoke the private method
            methodInfo.Invoke(obj, parameters);
        }

        [HarmonyPatch(typeof(Assets.FantasyInventory.Scripts.Interface.Shop), "Buy")] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]                              // There are different patch types. Prefix code runs before original code
        static bool Buy(object __instance)
        {
            Assets.FantasyInventory.Scripts.Interface.Shop _this = (Assets.FantasyInventory.Scripts.Interface.Shop)__instance;
            ItemContainer inv = (ItemContainer)_this.Bag;
            AddMoney(_this, inv, 5000, ItemId.Gold);
            //Assets.FantasyInventory.Scripts.Interface.Shop _this = (Assets.FantasyInventory.Scripts.Interface.Shop)__instance;
            //_this.inventory.Add(new Assets.FantasyInventory.Scripts.Data.Item(Assets.FantasyInventory.Scripts.Enums.ItemId.FruityDrink, 100));
            return true;
        }

        [HarmonyPatch(typeof(Assets.FantasyInventory.Scripts.Interface.Shop), "Sell")] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]                              // There are different patch types. Prefix code runs before original code
        static bool Sell(object __instance)
        {
            Assets.FantasyInventory.Scripts.Interface.Shop _this = (Assets.FantasyInventory.Scripts.Interface.Shop)__instance;
            ItemContainer inv = (ItemContainer)_this.Bag;
            AddMoney(_this, inv, 5000, ItemId.Gold);
            //Assets.FantasyInventory.Scripts.Interface.Shop _this = (Assets.FantasyInventory.Scripts.Interface.Shop)__instance;
            //_this.inventory.Add(new Assets.FantasyInventory.Scripts.Data.Item(Assets.FantasyInventory.Scripts.Enums.ItemId.FruityDrink, 100));
            return true;
        }
    }
}