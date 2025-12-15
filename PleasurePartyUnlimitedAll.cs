using Assets.FantasyInventory.Scripts.Enums;
using Assets.FantasyInventory.Scripts.Interface.Elements;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace PleasurePartyUnlimitedAll
{
    [BepInPlugin("com.wolfitdm.PleasurePartyUnlimitedAll", "PleasurePartyUnlimitedAll Plugin", "1.0.0.0")]
    public class PleasurePartyUnlimitedAll : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;

        private ConfigEntry<bool> configUnlimitedGold;
        private ConfigEntry<bool> configUnlimitedLevel;
        private ConfigEntry<bool> configUnlimitedLibido;
        private ConfigEntry<bool> configUnlimitedSkillpoints;
        private ConfigEntry<bool> configUnlimitedPeoples;
        public PleasurePartyUnlimitedAll()
        {
        }

        public static Type MyGetType(string originalClassName)
        {
            return Type.GetType(originalClassName + ",Assembly-CSharp");
        }

        public static void PatchHarmonyMethod(string originalClassName, string originalMethodName, string patchedMethodName, bool usePrefix, bool usePostfix)
        {
            // Create a new Harmony instance with a unique ID
            var harmony = new Harmony("com.wolfitdm.PleasurePartyUnlimitedAll");

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

            MethodInfo patched = AccessTools.Method(typeof(PleasurePartyUnlimitedAll), patchedMethodName);

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
        private static string pluginKey = "General.Toggles";

        public static int moneyP2_cashVar = 900000;
        public static float highestOverallPoints = 900000;

        public static bool unlimitedGold = false;
        public static bool unlimitedLevel = false;
        public static bool unlimitedLibido = false;
        public static bool unlimitedSkillpoints = false;
        public static bool unlimitedPeoples = false;

        private void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;

            configUnlimitedGold = Config.Bind(pluginKey,
                                              "UnlimitedMoney",
                                              true,
                                             "Whether or not you want unlimited money (default true also yes, you want it, and false = no)");
            
            configUnlimitedLevel = Config.Bind(pluginKey,
                                               "UnlimitedLevel",
                                               true,
                                               "Whether or not you want unlimited level (default true also yes, you want it, and false = no)");


            configUnlimitedLibido = Config.Bind(pluginKey,
                                              "UnlimitedLibido",
                                              true,
                                             "Whether or not you want unlimited libido (default true also yes, you want it, and false = no)");

            configUnlimitedSkillpoints = Config.Bind(pluginKey,
                                               "UnlimitedSkillpoints",
                                               true,
                                               "Whether or not you want unlimited skillpoints (default true also yes, you want it, and false = no)");

            configUnlimitedPeoples = Config.Bind(pluginKey,
                                   "UnlimitedPeoples",
                                   true,
                                   "Whether or not you want unlimited peoples (default true also yes, you want it, and false = no)");

            unlimitedGold = configUnlimitedGold.Value;
            unlimitedLevel = configUnlimitedLevel.Value;
            unlimitedLibido = configUnlimitedLibido.Value;
            unlimitedSkillpoints = configUnlimitedSkillpoints.Value;
            unlimitedPeoples = configUnlimitedPeoples.Value;

            Harmony.CreateAndPatchAll(typeof(PleasurePartyUnlimitedAll));

            if (!isP2)
            {
                PatchHarmonyMethod("CharacterProperties", "Update", "CharacterProperties_Update", true, false);
                Logger.LogInfo($"Plugin PleasurePartyUnlimitedAll for p1 is loaded!");
                return;
            }

            // P2 shit

            //PatchHarmonyMethod("Assets.FantasyInventory.Scripts.Interface.Shop", "Awake", "AddMoneyForP2", true, false);
            //PatchHarmonyMethod("Assets.FantasyInventory.Scripts.Interface.Shop", "Start", "AddMoneyForP2", true, false);
            //PatchHarmonyMethod("Assets.FantasyInventory.Scripts.Interface.Shop", "loadInventory", "loadInventory", true, false);
            PatchHarmonyMethod("CharacterManager_P2", "SaveCharacterData", "_AddMoneyForP2", false, true);
            //PatchHarmonyMethod("MenuManager", "startGame", "exitGame", false, true);
            Logger.LogInfo($"Plugin PleasurePartyUnlimitedAll for p2 is loaded!");
        }


        public static bool CharacterProperties_Update(object __instance)
        {
            if (!unlimitedLibido && !unlimitedSkillpoints && !unlimitedLevel)
            {
                return true;
            }

            CharacterProperties _this = (CharacterProperties)__instance;

            _this.libido = unlimitedLibido ? 150 : _this.libido;

            if (unlimitedSkillpoints)
            {
                _this.personalityType = 30;
                _this.intelligence = 30;
                _this.looks = 30;
                _this.turnOn = 30;
                _this.penisSize = 30;
                _this.quirks = 30;
                _this.sexWithSelf = true;
                _this.sexWithMen = true;
                _this.sexWithWomen = true;
                _this.sexWithMultiple = true;
                _this.likesBJ = true;
                _this.likesOral = true;
                _this.likesTop = true;
                _this.likesBottom = true;
                _this.likesLaying = true;
                _this.likesSitting = true;
                _this.likesStanding = true;
                _this.likesBehind = true;
                _this.likesAnal = true;
                _this.standardSex = true;
                _this.lesbianSex = true;
                _this.soloSex = true;
                _this.analSex = true;
                _this.myPersonalityType = 30;
                _this.myIntelligence = 30;
                _this.myLooks = 30;
                _this.myTurnOn = 30;
                _this.myPenisSize = 30;
                _this.myAge = 30;
                _this.mySexualSkill = 100;
                _this.mySexualExperience = 100;
                _this.myDesireToPlease = 100;
                _this.myStamina = 100;
                _this.myRecoveryTime = 0;
                _this.compatibilityScore = 100f;
            }

            if (unlimitedLevel)
            {
                ScenePersistent.generalOutputMultiplier = 10;
                ScenePersistent.climaxBonusMultiplier = 10;
                ScenePersistent.libidoFactor = 10;
                _this.orgasmMultiplier = 10;
                _this.climaxFactor = 10;
            }
            return true;
        }

        public static void AddMoneyForP2_SP_P2_money(int money)
        {
            if (!unlimitedGold)
            {
                return;
            }

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

            moneyP2_cashVar += money;

            if (money > 90000000)
            {
                moneyP2_cashVar = money = 90000000;
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
            if (!unlimitedGold)
            {
                return;
            }

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

            if (money > 90000000)
            {
                moneyP2_cashVar = money = 90000000;
            }

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
            if (!unlimitedGold)
            {
                return;
            }

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

            if (money > 90000000)
            {
                moneyP2_cashVar = money = 90000000;
            }

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

        public static void AddLevel_ScenePersistent_myGameData_HighestOverallPoints(float hp)
        {
            if (!unlimitedLevel)
            {
                return;
            }

            Type pleasureParty2_ScenePersistent = MyGetType("ScenePersistent");

            if (pleasureParty2_ScenePersistent == null)
            {
                Logger.LogInfo("class ScenePersistent for pleasure party not found: return here");
                return;
            }

            FieldInfo pleasureParty2_ScenePersistent_myGameData = pleasureParty2_ScenePersistent.GetField("myGameData", BindingFlags.Public | BindingFlags.Static);
            if (pleasureParty2_ScenePersistent_myGameData == null)
            {
                Logger.LogInfo("field ScenePersistent.myGameData for pleasure party not found: return here");
                return;
            }

            Type innerType_scenePersistent = null;

            if (innerType_scenePersistent == null)
            {
                innerType_scenePersistent = pleasureParty2_ScenePersistent_myGameData.FieldType;
            }

            if (innerType_scenePersistent == null)
            {
                Logger.LogInfo("class ScenePersistent.myGameData for pleasure party not found: return here");
                return;
            }

            FieldInfo hpField = innerType_scenePersistent.GetField("HighestOverallPoints", BindingFlags.Public | BindingFlags.Instance);

            if (hpField == null)
            {
                Logger.LogInfo("Can not get field ScenePersistent.myGameData.HighestOverallPoints for pleasure party: try ScenePersistent.myGameData.HighestOverallPoints");
                try
                {
                    ScenePersistent.myGameData.HighestOverallPoints += (float)(hp + highestOverallPoints);
                }
                catch (Exception ex)
                {
                }
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

            hp += highestOverallPoints;

            if (hp > 90000000)
            {
                highestOverallPoints = hp = 90000000;
            }

            try
            {
                float oldValue = (float)hpField.GetValue(myGameData_scenePersistent);
                Logger.LogInfo("ScenePersistent.myGameData.HighestOverallPoints: Old hp value: " + oldValue.ToString() + " $");
                oldValue += hp;
                highestOverallPoints = oldValue;
                Logger.LogInfo("ScenePersistent.myGameData.HighestOverallPoints: New hp value: " + oldValue.ToString() + " $");
                hpField.SetValue(myGameData_scenePersistent, oldValue);
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
            if (!unlimitedGold)
            {
                return; 
            }

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
            if (!unlimitedGold)
            {
                return;
            }

            AddMoneyForP2_SP_P2_money(value);
            AddMoneyForP2_SP_P2_myGameData_Money(value);
            AddMoneyForP2_ScenePersistent_myGameData_Money(value);
        }

        static void _AddMoneyForP2(object __instance)
        {
            if (!unlimitedLibido && !unlimitedSkillpoints)
            {
                return;
            }

            AddMoneyShitForP2_All(5000);
            AddLevel_ScenePersistent_myGameData_HighestOverallPoints(5000);

            CharacterManager_P2 _this = (CharacterManager_P2)__instance;    
            if (unlimitedLibido)
            {
                if (_this.libido < 100)
                    _this.libido = 100;
            }

            if (unlimitedSkillpoints)
            {
                if (_this.skillPoints < 1000)
                    _this.skillPoints = 1000;
            }
        }

        public static void AddMoneyShitForP2(Assets.FantasyInventory.Scripts.Interface.Shop obj, int value, ItemId currencyId)
        {
            if (!unlimitedGold)
            {
                return;
            }

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
            if (!unlimitedGold)
            {
                return true;
            }

            Logger.LogInfo("Add money for p2 is called");
            Assets.FantasyInventory.Scripts.Interface.Shop _this = (Assets.FantasyInventory.Scripts.Interface.Shop)__instance;
            AddMoneyShitForP2(_this, 5000, ItemId.Gold);
            //Assets.FantasyInventory.Scripts.Interface.Shop _this = (Assets.FantasyInventory.Scripts.Interface.Shop)__instance;
            //_this.inventory.Add(new Assets.FantasyInventory.Scripts.Data.Item(Assets.FantasyInventory.Scripts.Enums.ItemId.FruityDrink, 100));
            return true;
        }

        [HarmonyPatch(typeof(LevelManager), "Update")] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]                              // There are different patch types. Prefix code runs before original code
        static bool Update(object __instance)
        {
            LevelManager _this = (LevelManager)__instance;
            bool increaseIndex = false;
            
            if (_this.levelRequirements.Length <= 30)
            {
                increaseIndex = true;
            } else
            {
                return true;
            }

            /*
             * 
            Type thisType = _this.GetType();
            FieldInfo oldLevelField = thisType.GetField("oldLevel", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo myLevelField = thisType.GetField("myLevel", BindingFlags.NonPublic | BindingFlags.Instance);
            int oldLevel = 0;
            int myLevel = 0;

            if (oldLevelField == null)
            {
                oldLevel = 29;
            }
            else
            {
                try
                {
                    oldLevel = (int)oldLevelField.GetValue(thisType);
                }
                catch (Exception e)
                {
                    oldLevel = 29;
                }
            }

            if (myLevelField == null)
            {
                myLevel = 29;
            }
            else
            {
                try
                {
                    myLevel = (int)myLevelField.GetValue(thisType);
                }
                catch (Exception e)
                {
                    myLevel = 29;
                }
            }

            if (_this.levelRequirements.Length <= myLevel)
            {
                increaseIndex = true;
            }

            if (_this.levelRequirements.Length <= oldLevel)
            {
                increaseIndex = true;
            }*/

            if (increaseIndex)
            {
                int[] newLevelRequirements = new int[_this.levelRequirements.Length + 30];
                for (int i = 0; i < _this.levelRequirements.Length; i++)
                {
                    newLevelRequirements[i] = _this.levelRequirements[i];
                }
                for (int i = _this.levelRequirements.Length; i < newLevelRequirements.Length; i++)
                {
                    newLevelRequirements[i] = 0;
                }
                _this.levelRequirements = newLevelRequirements;
            }
            return true;
        }

        [HarmonyPatch(typeof(SelectionProcess), "selectArena")]
        [HarmonyPrefix]
        static bool selectArena(GameObject whichArea, object __instance)
        {
            if (!unlimitedPeoples)
            {
                return true;
            }

            SelectionProcess _this = (SelectionProcess)__instance;
            arenaProperties myProps = whichArea.GetComponent<arenaProperties>();
            myProps.numberOfPeople += 14;
            myProps.specialGirlsOnly = false;
            Type _thisType = (Type)_this.GetType();
            FieldInfo myPropsField = _thisType.GetField("myProps", BindingFlags.NonPublic | BindingFlags.Instance);
            if (myPropsField != null)
            {
                Logger.LogInfo("set arena myprops value");
                myPropsField.SetValue(__instance, myProps);
            }
            return true;
        }

        [HarmonyPatch(typeof(LevelManager), "calculateLevel")] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]                              // There are different patch types. Prefix code runs before original code
        static bool _calculateLevel(object __instance)
        {
            if (!unlimitedLevel)
            {
                return true;
            }
            
            try
            {
                if (ScenePersistent.myGameData.Level >= 29)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
            }

            AddLevel_ScenePersistent_myGameData_HighestOverallPoints(5000);
            /*try
            {
                try
                {
                    ScenePersistent.myGameData.Level = 30;
                } catch (Exception ex)
                {
                }

                try
                {
                    ScenePersistent.nextLevelRequirement = 0;
                }
                catch (Exception ex)
                {
                }

                try
                {
                    ScenePersistent.myGameData.HighestOverallPoints = 90000000;
                }
                catch (Exception ex)
                {
                }*/

            /*Logger.LogInfo("my level is now: 30");
            LevelManager _this = (LevelManager)__instance;
            Type _thisType = _this.GetType();
            FieldInfo oldLevelField = _thisType.GetField("oldLevel");
            FieldInfo myLevelField = _thisType.GetField("myLevel");
            int oldLevel = (int)oldLevelField.GetValue(_this);
            int myLevel = (int)myLevelField.GetValue(_this);

            oldLevel = myLevel;

            int levelRequirementsLength = _this.levelRequirements == null ? 0 : _this.levelRequirements.Length;

            oldLevel = myLevel = levelRequirementsLength - 1;

            if (oldLevel < 0)
            {
                oldLevel = myLevel = 0;
            }
            ScenePersistent.myGameData.HighestOverallPoints = -1;
            for (int index = 0; index < levelRequirementsLength; ++index)
            {
                if (_this.levelRequirements[index] > 0 && _this.levelRequirements[index] > ScenePersistent.myGameData.HighestOverallPoints)
                {
                    ScenePersistent.myGameData.HighestOverallPoints = _this.levelRequirements[index] + 1;
                }
                _this.levelRequirements[index] = 0;
                ScenePersistent.nextLevelRequirement = 0;
            }
            oldLevel = myLevel;
            oldLevelField.SetValue(_this, oldLevel);
            myLevelField.SetValue(_this, myLevel);
            Logger.LogInfo("my level is now: " + myLevel.ToString());
            if (myLevel > 17 && !ScenePersistent.myGameData.completedConclusion && !ScenePersistent.inConclusion)
                ScenePersistent.inConclusion = true;
            ScenePersistent.myGameData.Level = myLevel;
            ScenePersistent.nextLevelRequirement = 0;*/
            /*}
                catch (Exception e)
                {
                    Logger.LogInfo("Errror error in calculateLevel");
                    Logger.LogInfo(e.ToString());
                }*/
            return true;
        }

        [HarmonyPatch(typeof(Assets.FantasyInventory.Scripts.Interface.Shop), "Buy")] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]                              // There are different patch types. Prefix code runs before original code
        static bool Buy(object __instance)
        {
            if (!unlimitedGold)
            {
                return true;
            }

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
            if (!unlimitedGold)
            {
                return true;
            }

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