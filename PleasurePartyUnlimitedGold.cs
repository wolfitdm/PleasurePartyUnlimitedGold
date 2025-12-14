using Assets.FantasyInventory.Scripts.Data;
using Assets.FantasyInventory.Scripts.Enums;
using Assets.FantasyInventory.Scripts.Interface;
using Assets.FantasyInventory.Scripts.Interface.Elements;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using HarmonyLib;
using SemanticVersioning;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
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

        public static void PatchHarmonyMethod(string originalClassName, string originalMethodName, string patchedMethodName, bool usePrefix, bool usePostfix)
        {
            // Create a new Harmony instance with a unique ID
            var harmony = new Harmony("com.wolfitdm.PleasurePartyUnlimitedGold");

            Type originalClass = null;

            originalClass = MyGetType(originalClassName);

            if (originalClass == null)
            {
                Logger.LogInfo($"GetType {originalClassName} == null");
                return;
            }

            // Or apply patches manually
            MethodInfo original = AccessTools.Method(originalClass, originalMethodName);

            if (original == null)
            {
                Logger.LogInfo($"AccessTool.Method original {originalClassName} == null");
                return;
            }

            MethodInfo patched = AccessTools.Method(typeof(PleasurePartyUnlimitedGold), patchedMethodName);

            if (patched == null)
            {
                Logger.LogInfo($"AccessTool.Method patched {patchedMethodName} == null");
                return;

            }

            HarmonyMethod patchedMethod = new HarmonyMethod(patched);
            var prefixMethod = usePrefix ? patchedMethod : null;
            var postfixMethod = usePostfix ? patchedMethod : null;

            harmony.Patch(original,
                prefix: prefixMethod,
                postfix: postfixMethod);
        }

        public static bool testIsP2()
        {
            return MyGetType("CharacterManager_P2") != null;
        }

        private static bool isP2 = testIsP2();

        private void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;
            Harmony.CreateAndPatchAll(typeof(PleasurePartyUnlimitedGold));

            if (!isP2)
            {
                Logger.LogInfo($"Plugin PleasurePartyUnlimitedGold for p1 is loaded!");
                return;
            }

            // P2 shit

            //PatchHarmonyMethod("Assets.FantasyInventory.Scripts.Interface.Shop", "Awake", "AddMoneyForP2", true, false);
            //PatchHarmonyMethod("Assets.FantasyInventory.Scripts.Interface.Shop", "Start", "AddMoneyForP2", true, false);
            //PatchHarmonyMethod("Assets.FantasyInventory.Scripts.Interface.Shop", "loadInventory", "loadInventory", true, false);
            PatchHarmonyMethod("CharacterManager_P2", "SaveCharacterData", "_AddMoneyForP2", false, true);
            //PatchHarmonyMethod("MenuManager", "startGame", "exitGame", false, true);
            Logger.LogInfo($"Plugin PleasurePartyUnlimitedGold for p2 is loaded!");
        }

        public static int moneyP2_cashVar = 900000;

        public static void AddMoneyForP2_SP_P2_money(int money)
        {
            Type pleasureParty2 = MyGetType("Classes_P2");

            if (pleasureParty2 == null)
            {
                Logger.LogInfo("No support for pleasure party 2 found: return here");
                return;
            }

            Type pleasureParty2_SP_P2 = MyGetType("SP_P2");

            if (pleasureParty2_SP_P2 == null)
            {
                Logger.LogInfo("class SP_P2 for pleasure party 2 not found: return here");
                return;
            }

            FieldInfo pleasureParty2_SP_P2_money = pleasureParty2_SP_P2.GetField("money", BindingFlags.Public | BindingFlags.Static);
            if (pleasureParty2_SP_P2_money == null)
            {
                Logger.LogInfo("field SP_P2.money for pleasure party 2 not found: return here");
                return;
            }

            try
            {
                int oldValue = (int)pleasureParty2_SP_P2_money.GetValue(null);
                Logger.LogInfo("SP_P2.money: Old money value: " + oldValue.ToString() + " $");
                oldValue += money;
                moneyP2_cashVar = oldValue;
                Logger.LogInfo("SP_P2.money: New money value: " + oldValue.ToString() + " $");
                pleasureParty2_SP_P2_money.SetValue(null, oldValue);
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Can not set field SP_P2.money for pleasure party 2: return here");
                Logger.LogInfo(ex.ToString());
                return;
            }
        }

        public static void AddMoneyForP2_SP_P2_myGameData_Money(int money)
        {
            Type pleasureParty2 = MyGetType("Classes_P2");

            if (pleasureParty2 == null)
            {
                Logger.LogInfo("No support for pleasure party 2 found: return here");
                return;
            }

            Type pleasureParty2_SP_P2 = MyGetType("SP_P2");

            if (pleasureParty2_SP_P2 == null)
            {
                Logger.LogInfo("class SP_P2 for pleasure party 2 not found: return here");
                return;
            }

            FieldInfo pleasureParty2_SP_P2_myGameData = pleasureParty2_SP_P2.GetField("myGameData", BindingFlags.Public | BindingFlags.Static);
            if (pleasureParty2_SP_P2_myGameData == null)
            {
                Logger.LogInfo("field SP_P2.myGameData for pleasure party 2 not found: return here");
                return;
            }

            Type innerType = null;

            if (innerType == null)
            {
                innerType = pleasureParty2_SP_P2_myGameData.FieldType;
            }

            if (innerType == null)
            {
                Type[] nestedTypes = pleasureParty2.GetNestedTypes();

                foreach (Type nestedType in nestedTypes)
                {
                    if (nestedType.Name == "gameData2")
                    {
                        innerType = nestedType;
                        Logger.LogInfo("class Classes_P2.gameData2 for pleasure party 2 found here: yeahhh!");
                        break;
                    }
                }
            }

            if (innerType == null)
            {
                // Step 2: Get the nested (inner) class Type
                innerType = pleasureParty2.GetNestedType("gameData2", BindingFlags.Public);
            }

            if (innerType == null)
            {
                innerType = MyGetType("Classes_P2.gameData2");
            }

            if (innerType == null)
            {
                Logger.LogInfo("class Classes_P2.gameData2 for pleasure party 2 not found: return here");
                return;
            }

            // --- Modify public field "Age" ---
            FieldInfo moneyField = innerType.GetField("Money", BindingFlags.Public | BindingFlags.Instance);
            if (moneyField == null)
            {
                Logger.LogInfo("field Classes_P2.gameData2.Money for pleasure party 2 not found: return here");
                return;
            }

            object myGameData = null;

            try
            {
                myGameData = pleasureParty2_SP_P2_myGameData.GetValue(null);
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Can not get field SP_P2.myGameData for pleasure party 2: try SP2_P2.myGameData");
                Logger.LogInfo(ex.ToString());
                myGameData = null;
            }

            if (myGameData == null)
            {
                try
                {
                    myGameData = SP_P2.myGameData;
                }
                catch (Exception ex)
                {
                    Logger.LogInfo("Can not get field SP_P2.myGameData for pleasure party 2: return here");
                    Logger.LogInfo(ex.ToString());
                    return;
                }
            }

            money += moneyP2_cashVar;

            try
            {
                int oldValue = (int)moneyField.GetValue(myGameData);
                Logger.LogInfo("SP_P2.myGameData.Money: Old money value: " + oldValue.ToString() + " $");
                oldValue += money;
                moneyP2_cashVar = oldValue;
                Logger.LogInfo("SP_P2.myGameData.Money: New money value: " + oldValue.ToString() + " $");
                moneyField.SetValue(myGameData, oldValue);
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Can not set field Classes_P2.gameData2.Money for pleasure party 2: return here");
                Logger.LogInfo(ex.ToString());
            }
        }

        public static void AddMoneyForP2_ScenePersistent_myGameData_Money(int money)
        {
            Type pleasureParty2 = MyGetType("Classes_P2");

            if (pleasureParty2 == null)
            {
                Logger.LogInfo("No support for pleasure party 2 found: return here");
                return;
            }

            Type pleasureParty2_ScenePersistent = MyGetType("ScenePersistent");

            if (pleasureParty2_ScenePersistent == null)
            {
                Logger.LogInfo("class ScenePersistent for pleasure party 2 not found: return here");
                return;
            }

            FieldInfo pleasureParty2_ScenePersistent_myGameData = pleasureParty2_ScenePersistent.GetField("myGameData", BindingFlags.Public | BindingFlags.Static);
            if (pleasureParty2_ScenePersistent_myGameData == null)
            {
                Logger.LogInfo("field ScenePersistent.myGameData for pleasure party 2 not found: return here");
                return;
            }

            Type innerType_scenePersistent = null;

            if (innerType_scenePersistent == null)
            {
                innerType_scenePersistent = pleasureParty2_ScenePersistent_myGameData.FieldType;
            }

            if (innerType_scenePersistent == null)
            {
                Type[] nestedTypes_scenePersistent = pleasureParty2.GetNestedTypes();

                foreach (Type nestedType in nestedTypes_scenePersistent)
                {
                    if (nestedType.Name == "gameData2")
                    {
                        innerType_scenePersistent = nestedType;
                        Logger.LogInfo("class Classes_P2.gameData2 for pleasure party 2 found here: yeahhh!");
                        break;
                    }
                }
            }

            if (innerType_scenePersistent == null)
            {
                // Step 2: Get the nested (inner) class Type
                innerType_scenePersistent = pleasureParty2.GetNestedType("gameData2", BindingFlags.Public);
            }

            if (innerType_scenePersistent == null)
            {
                innerType_scenePersistent = MyGetType("Classes_P2.gameData2");
            }

            if (innerType_scenePersistent == null)
            {
                Logger.LogInfo("class Classes_P2.gameData2 for pleasure party 2 not found: return here");
                return;
            }

            // --- Modify public field "Age" ---
            FieldInfo moneyField_scenePersistent = innerType_scenePersistent.GetField("Money", BindingFlags.Public | BindingFlags.Instance);
            if (moneyField_scenePersistent == null)
            {
                Logger.LogInfo("field Classes_P2.gameData2.Money for pleasure party 2 not found: return here");
                return;
            }

            object myGameData_scenePersistent = null;

            try
            {
                myGameData_scenePersistent = pleasureParty2_ScenePersistent_myGameData.GetValue(null);
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Can not get field ScenePersistent.myGameData for pleasure party 2: try ScenePersistent.myGameData");
                Logger.LogInfo(ex.ToString());
                myGameData_scenePersistent = null;
            }

            if (myGameData_scenePersistent == null)
            {
                try
                {
                    myGameData_scenePersistent = ScenePersistent.myGameData;
                }
                catch (Exception ex)
                {
                    Logger.LogInfo("Can not get field ScenePersistent.myGameData for pleasure party 2: return here");
                    Logger.LogInfo(ex.ToString());
                    return;
                }
            }

            money += moneyP2_cashVar;

            try
            {
                int oldValue = (int)moneyField_scenePersistent.GetValue(myGameData_scenePersistent);
                Logger.LogInfo("ScenePersistent.myGameData.Money: Old money value: " + oldValue.ToString() + " $");
                oldValue += money;
                moneyP2_cashVar = oldValue;
                Logger.LogInfo("ScenePersistent.myGameData.Money: New money value: " + oldValue.ToString() + " $");
                moneyField_scenePersistent.SetValue(myGameData_scenePersistent, oldValue);
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Can not set field Classes_P2.gameData2.Money for pleasure party 2: return here");
                Logger.LogInfo(ex.ToString());
                return;
            }
        }

       /* public static void exitGame(object __instance)
        {
            MenuManager _this = (MenuManager)__instance;
            Type type = typeof(MenuManager);
            FieldInfo fieldInfo = type.GetField("gameDataFile");
            string gameDataFile = (string)fieldInfo.GetValue(_this);
            Logger.LogInfo(gameDataFile);
            ScenePersistent.stopSaving = true;
            if (!Directory.Exists(Application.dataPath + "/HFTData"))
            {
                Debug.Log((object)"nor found data path, creating");
                Directory.CreateDirectory(Application.dataPath + "/HFTData");
            }
            
            Bayat.SaveSystem.SaveSystemAPI.SaveAsync(gameDataFile, (object)new ScenePersistent.gameData()
            {
                Money = 100000,
                Level = 1,
                HighestOverallPoints = 0.0f,
                ManViewCamNotices = 0,
                FreeCamNotices = 0,
                SteamOn = false,
                arenasOwned = new bool[12],
                peopleOwned = new bool[30],
                lastPlayedDateTime = "",
                completedTutorial = false,
                completedConclusion = false
            });
        }*/

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

        /*public static void AddMoneyShitForP2_Ex(Assets.FantasyInventory.Scripts.Interface.Shop obj, int value, ItemId currencyId) {

            ItemContainer _bag = (ItemContainer)obj.Bag;

            bool foundMoney = false;

            try
            {
                _bag.Items.ForEach(item => {
                    if (item != null && item.Id == ItemId.Gold)
                    {
                        item.Count = moneyP2_cashVar;
                        foundMoney = true;
                    }
                });
                if (!foundMoney)
                {
                    _bag.Items.Insert(0, new Item(ItemId.Gold, moneyP2_cashVar));
                }
                Logger.LogInfo("Add money for p2 really fuck off");
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Can not add money for p2 really fuck off");
                Logger.LogInfo(ex.ToString());
            }
        }*/


        static void AddMoneyShitForP2_All(int value)
        {
            AddMoneyForP2_SP_P2_money(value);
            AddMoneyForP2_SP_P2_myGameData_Money(value);
            AddMoneyForP2_ScenePersistent_myGameData_Money(value);
        }

        static void _AddMoneyForP2()
        {
            AddMoneyShitForP2_All(5000);
        }

        public static void AddMoneyShitForP2(Assets.FantasyInventory.Scripts.Interface.Shop obj, int value, ItemId currencyId)
        {
            // P1 so cool

            Logger.LogInfo("Add Money to character: " + value.ToString() + " $");

            ItemContainer _bag = (ItemContainer)obj.Bag;

            AddMoney(obj, _bag, value, ItemId.Gold);

            // P2 shit

            Type pleasureParty2 = MyGetType("Classes_P2");

            Logger.LogInfo("Test support for pleasure party 2");

            if (!isP2)
            {
                Logger.LogInfo("Really no support found for pleasure party 2");
                return;
            }

            if (pleasureParty2 == null)
            {
                Logger.LogInfo("Really no support found for pleasure party 2");
                return;
            }

            AddMoneyShitForP2_All(value);

            //AddMoneyShitForP2_Ex(obj, value, currencyId);
        }
        static bool AddMoneyForP2 (object __instance)
        {
            Logger.LogInfo("Add money for p2 is called");
            Assets.FantasyInventory.Scripts.Interface.Shop _this = (Assets.FantasyInventory.Scripts.Interface.Shop)__instance;
            AddMoneyShitForP2(_this, 5000, ItemId.Gold);
            //Assets.FantasyInventory.Scripts.Interface.Shop _this = (Assets.FantasyInventory.Scripts.Interface.Shop)__instance;
            //_this.inventory.Add(new Assets.FantasyInventory.Scripts.Data.Item(Assets.FantasyInventory.Scripts.Enums.ItemId.FruityDrink, 100));
            return true;
        }

        [HarmonyPatch(typeof(Assets.FantasyInventory.Scripts.Interface.Shop), "Buy")] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]                              // There are different patch types. Prefix code runs before original code
        static bool Buy(object __instance)
        {
            Assets.FantasyInventory.Scripts.Interface.Shop _this = (Assets.FantasyInventory.Scripts.Interface.Shop)__instance;
            AddMoneyShitForP2(_this, 5000, ItemId.Gold);
            //Assets.FantasyInventory.Scripts.Interface.Shop _this = (Assets.FantasyInventory.Scripts.Interface.Shop)__instance;
            //_this.inventory.Add(new Assets.FantasyInventory.Scripts.Data.Item(Assets.FantasyInventory.Scripts.Enums.ItemId.FruityDrink, 100));
            return true;
        }

        [HarmonyPatch(typeof(Assets.FantasyInventory.Scripts.Interface.Shop), "Sell")] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]                              // There are different patch types. Prefix code runs before original code
        static bool Sell(object __instance)
        {
            Assets.FantasyInventory.Scripts.Interface.Shop _this = (Assets.FantasyInventory.Scripts.Interface.Shop)__instance;
            AddMoneyShitForP2(_this, 5000, ItemId.Gold);
            //Assets.FantasyInventory.Scripts.Interface.Shop _this = (Assets.FantasyInventory.Scripts.Interface.Shop)__instance;
            //_this.inventory.Add(new Assets.FantasyInventory.Scripts.Data.Item(Assets.FantasyInventory.Scripts.Enums.ItemId.FruityDrink, 100));
            return true;
        }

        /*static bool loadInventory(object __instance)
        {
            try
            {
                Assets.FantasyInventory.Scripts.Interface.Shop _this = (Assets.FantasyInventory.Scripts.Interface.Shop)__instance;
                if (ScenePersistent.inTutorial)
                {
                    if (ScenePersistent.gaveTutFruityDrink)
                        return false;
                    _this.inventory.Add(new Item(ItemId.FruityDrink, 1));
                    ScenePersistent.gaveTutFruityDrink = true;
                }
                else
                {
                    if (PlayerPrefs.GetInt("INVCount") == 0)
                        return false;
                    _this.inventory.Clear();
                    int num = PlayerPrefs.GetInt("INVCount");
                    if (num <= 0)
                        return false;
                    for (int index = 0; index < num; ++index)
                    {
                        if (PlayerPrefs.GetInt("INVID_" + index.ToString()) == 3)
                            _this.inventory.Add(new Item(ItemId.Gold, moneyP2_cashVar));
                        else
                            _this.inventory.Add(new Item((ItemId)PlayerPrefs.GetInt("INVID_" + index.ToString()), PlayerPrefs.GetInt("INVNum_" + index.ToString())));
                    }
                }
                return false;
            } catch (Exception ex)
            {
                return true;
            }
        }*/
    }
}