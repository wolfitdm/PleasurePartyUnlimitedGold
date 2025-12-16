using Assets.FantasyInventory.Scripts.Enums;
using Assets.FantasyInventory.Scripts.Interface.Elements;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using FIMSpace.FLook;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

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
        private ConfigEntry<bool> configUnlimitedPleasureOutput;
        private ConfigEntry<bool> configAllPartyParticipantsAreNaked;
        private ConfigEntry<bool> configPleasureParty1Only_LooksAndLikesAnyPersonalty;
        private ConfigEntry<bool> configPleasureParty1only_LikesEverySexPosition;
        private ConfigEntry<bool> configPleasureParty2only_useCmRootUpdateFix;
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

        public static int maxValue = 90000000;
        public static float maxHpValue = 90000000;

        public static int moneyP2_cashVar = maxValue;
        public static float highestOverallPoints = maxHpValue;
        

        public static bool unlimitedGold = false;
        public static bool unlimitedLevel = false;
        public static bool unlimitedLibido = false;
        public static bool unlimitedSkillpoints = false;
        public static bool unlimitedPeoples = false;
        public static bool unlimitedPleasureOutput = false;
        public static bool allPartyParticipantsAreNaked = false;
        public static bool pleasureParty1only_looksAndLikesAnyPersonalty = false;
        public static bool pleasureParty1only_likesEverySexPosition = false;
        public static bool pleasureParty2only_useCmRootUpdateFix = false;

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
                                               "Whether or not you want unlimited skillpoints or if you play pleasure party 1 all party participants are full skilled (default true also yes, you want it, and false = no)");

            configUnlimitedPeoples = Config.Bind(pluginKey,
                                   "UnlimitedPeoples",
                                   true,
                                   "Whether or not you want unlimited peoples (default true also yes, you want it, and false = no)");

            configUnlimitedPleasureOutput = Config.Bind(pluginKey,
                       "UnlimitedPleasureOutput",
                       true,
                       "Whether or not you want unlimited pleasure output (default true also yes, you want it, and false = no)");

            configAllPartyParticipantsAreNaked = Config.Bind(pluginKey,
                                                           "AllPartyParticipantsAreNaked",
                                                            true,
                                                            "Whether or not you want that all party participants are naked (default true also yes, you want it, and false = no)");

            configPleasureParty1Only_LooksAndLikesAnyPersonalty = Config.Bind(pluginKey,
                                               "PleasureParty1Only_LooksAndLikesAnyPersonalty",
                                                true,
                                                "Whether or not you want that all party participants likes and loves together  (default true also yes, you want it, that all set to any and false = no)");

            configPleasureParty1only_LikesEverySexPosition = Config.Bind(pluginKey,
                                               "PleasureParty1only_LikesEverySexPosition",
                                                true,
                                                "Whether or not you want that all party participants likes and loves every sex position (default true also yes, you want it, and false = no)");

            configPleasureParty2only_useCmRootUpdateFix = Config.Bind(pluginKey,
                                               "PleasureParty2only_UseCmRootUpdateFix",
                                                false,
                                                "Whether or not you want use my cm root update fix in order to prevent a nullpointerreference exception in the bepinex log (default false you do not want use my cm root update fix, you want use the original shit!)"); ;

            unlimitedGold = configUnlimitedGold.Value;
            unlimitedLevel = configUnlimitedLevel.Value;
            unlimitedLibido = configUnlimitedLibido.Value;
            unlimitedSkillpoints = configUnlimitedSkillpoints.Value;
            unlimitedPeoples = configUnlimitedPeoples.Value;
            unlimitedPleasureOutput = configUnlimitedPleasureOutput.Value;
            allPartyParticipantsAreNaked = configAllPartyParticipantsAreNaked.Value;
            pleasureParty1only_looksAndLikesAnyPersonalty = configPleasureParty1Only_LooksAndLikesAnyPersonalty.Value;
            pleasureParty1only_likesEverySexPosition = configPleasureParty1only_LikesEverySexPosition.Value;
            pleasureParty2only_useCmRootUpdateFix = configPleasureParty2only_useCmRootUpdateFix.Value;

            Harmony.CreateAndPatchAll(typeof(PleasurePartyUnlimitedAll));

            PatchHarmonyMethod("SexOutputDisplayManager", "Update", "MoneyOverflow_Update", false, true);
            PatchHarmonyMethod("SP_P2", "Update", "MoneyOverflow_Update", false, true);

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
            PatchHarmonyMethod("CMRoot", "Update", "CMRoot_Update", true, false);
            //PatchHarmonyMethod("MenuManager", "startGame", "exitGame", false, true);
            Logger.LogInfo($"Plugin PleasurePartyUnlimitedAll for p2 is loaded!");
        }


        public static bool CharacterProperties_Update(object __instance)
        {
            if (!unlimitedLibido && !unlimitedSkillpoints && !pleasureParty1only_looksAndLikesAnyPersonalty && !pleasureParty1only_likesEverySexPosition && !allPartyParticipantsAreNaked)
            {
                return true;
            }

            CharacterProperties _this = (CharacterProperties)__instance;

            _this.libido = unlimitedLibido ? 150 : _this.libido;

            if (pleasureParty1only_looksAndLikesAnyPersonalty)
            {
                _this.personalityType = 0;
                _this.intelligence = 0;
                _this.looks = 0;
                _this.turnOn = 0;
                _this.penisSize = 0;
                _this.quirks = 0;
                _this.age = 0;
                _this.myPersonalityType = 0;
                _this.myIntelligence = 0;
                _this.myLooks = 0;
                _this.myTurnOn = 0;
                _this.myPenisSize = 0;
                _this.myAge = 0;
            }

            if (pleasureParty1only_likesEverySexPosition)
            {
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
            }

            if (unlimitedSkillpoints)
            {
                _this.mySexualSkill = 100;
                _this.mySexualExperience = 100;
                _this.myDesireToPlease = 100;
                _this.myStamina = 100;
                _this.myRecoveryTime = 0;
                _this.compatibilityScore = 100f;
                _this.bonusSpectators += 10;
                _this.bonusGirlsInProximity += 10;
                _this.bonusConnections += 10;
                _this.bonusNumberOfPartners += 10;
                _this.bonusFavoritePosition = true;
                _this.bonusSpecialPosition = true;
                _this.bonusFavoriteType = true;
                _this.bonusPerfectMatch = true;
                _this.bonusJustChanged = true;
                _this.haveBonusPosition = true;
            }

            if (unlimitedPleasureOutput)
            {
                ScenePersistent.generalOutputMultiplier = 10;
                ScenePersistent.climaxBonusMultiplier = 10;
                ScenePersistent.libidoFactor = 10;
                _this.orgasmMultiplier = 10;
                _this.climaxFactor = 10;
            }

            if (allPartyParticipantsAreNaked)
            {
                _this.outfit = "nude";
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

            if (money > maxValue || money <= 0)
            {
                moneyP2_cashVar = money = maxValue;
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

            if (money > maxValue || money <= 0)
            {
                moneyP2_cashVar = money = maxValue;
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

            if (money > maxValue || money <= 0)
            {
                moneyP2_cashVar = money = maxValue;
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

            if (hp > maxHpValue || hp <= 0)
            {
                highestOverallPoints = hp = maxHpValue;
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

            AddMoneyShitForP2_Ex(obj, maxValue, ItemId.Gold);
        }

        public static void AddMoneyShitForP2_Ex(Assets.FantasyInventory.Scripts.Interface.Shop obj, int value, ItemId currencyId) {

            ItemContainer _bag = (ItemContainer)obj.Bag;

            bool foundMoney = false;

            try
            {
                _bag.Items.ForEach(item => {
                    if (item != null && item.Id == ItemId.Gold)
                    {
                        if (item.Count > maxValue || item.Count <= 0)
                        {
                            item.Count = value;
                        }
                        foundMoney = true;
                    }
                });
                foundMoney = true;
                if (!foundMoney)
                {
                    _bag.Items.Insert(0, new Assets.FantasyInventory.Scripts.Data.Item(ItemId.Gold, value));
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Can not add money for p2 really fuck off");
                Logger.LogInfo(ex.ToString());
            }
        }


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

        static FieldInfo getFloatFeld(string name, Type type)
        {
            return type.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
        }

        static float getFloatValue(FieldInfo field, object __instance = null)
        {
            if (field != null)
            {
                try
                {
                    return (float)field.GetValue(__instance);
                }
                catch (Exception e)
                {
                    return 0;
                }
            } else
            {
                return 0;
            }
        }

        static void setFloatField(FieldInfo field, float value, object __instance = null)
        {
            if (field != null)
            {
                try
                {
                    field.SetValue(__instance, value);
                }
                catch (Exception e)
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        static void _AddMoneyForP2(object __instance)
        {
            if (!unlimitedLibido && !unlimitedSkillpoints && !unlimitedPleasureOutput && !allPartyParticipantsAreNaked)
            {
                return;
            }

            AddMoneyShitForP2_All(5000);
            
            CharacterManager_P2 _this = (CharacterManager_P2)__instance;
            Type thisType = _this.GetType();

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

            if (unlimitedPleasureOutput)
            {
                _this.climaxFactor = 10;
                _this.climaxBonus = 10;
                FieldInfo libidoFactorField = getFloatFeld("libidoFactor", thisType);
                FieldInfo maleMultiplierField = getFloatFeld("maleMultiplier", thisType);
                FieldInfo skillMultiplierField = getFloatFeld("skillMultiplier", thisType);
                FieldInfo skillIncreaseFactorField = getFloatFeld("skillIncreaseFactor", thisType);
                FieldInfo skillBonusAddonField = getFloatFeld("skillBonusAddon", thisType);
                float libidoFactor = getFloatValue(libidoFactorField, _this);
                float maleMultiplier = getFloatValue(maleMultiplierField, _this);
                float skillMultiplier = getFloatValue(skillMultiplierField, _this);
                float skillIncreaseFactor = getFloatValue(skillIncreaseFactorField, _this);
                float skillBonusAddon = getFloatValue(skillBonusAddonField, _this);
                libidoFactor = 10;
                maleMultiplier = 10;
                skillMultiplier = 10;
                skillIncreaseFactor = 10;
                skillBonusAddon = 10;
                setFloatField(libidoFactorField, libidoFactor, _this);
                setFloatField(maleMultiplierField, maleMultiplier, _this);
                setFloatField(skillMultiplierField, skillMultiplier, _this);
                setFloatField(skillIncreaseFactorField, skillIncreaseFactor, _this);
                setFloatField(skillBonusAddonField, skillBonusAddon, _this);
            }

            if (allPartyParticipantsAreNaked)
            {
                try
                {
                    for (int index = 0; index < _this.shirt.Length; ++index)
                        _this.shirt[index].SetActive(false);

                    for (int index = 0; index < _this.pants.Length; ++index)
                        _this.pants[index].SetActive(false);

                    for (int index = 0; index < _this.shoes.Length; ++index)
                        _this.shoes[index].SetActive(false);

                    _this.shirtOn = false;
                    _this.pantsOn = false;
                    _this.shoesOn = false;

                    int length = _this.selectedClothingArray != null ? _this.selectedClothingArray.Length : 0;

                    if (length > 7)
                    {
                        _this.selectedClothingArray[7] = -1;
                        _this.selectedClothingArray[5] = -1;
                        _this.selectedClothingArray[4] = -1;
                    }
                    else if (length > 5)
                    {
                        _this.selectedClothingArray[5] = -1;
                        _this.selectedClothingArray[4] = -1;
                    }
                    else if (length > 4)
                    {
                        _this.selectedClothingArray[4] = -1;
                    }

                    if (!_this.isNew)
                    {
                        FieldInfo myCharDataField = thisType.GetField("myCharData", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (myCharDataField != null)
                        {
                            try
                            {
                                Classes_P2.characterDataP2 myCharData = (Classes_P2.characterDataP2)myCharDataField.GetValue(_this);
                                myCharData.shirtOn = false;
                                myCharData.pantsOn = false;
                                myCharData.shoesOn = false;
                                myCharDataField.SetValue(_this, myCharData);
                            }
                            catch (Exception ex)
                            {
                                Logger.LogInfo("Can not set myCharData field");
                                Logger.LogInfo(ex.ToString());
                            }
                        }
                    } 
                } catch (Exception ex)
                {
                    Logger.LogInfo("Can not set all participants naked");
                    Logger.LogInfo(ex.ToString());
                }
            }
        }

        public static bool CMRoot_Update(object __instance)
        {
            if (!pleasureParty2only_useCmRootUpdateFix)
            {
                return true;
            }

            CMRoot _this = (CMRoot)__instance;
            Type thisType = _this.GetType();
            FieldInfo setTimeField = thisType.GetField("setTime", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo lastGoodOneField = thisType.GetField("lastGoodOne", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo doCorrectField = thisType.GetField("doCorrect", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo correctedOneField = thisType.GetField("correctedOne", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo longestDistanceField = thisType.GetField("longestDistance", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo cmTimerField = thisType.GetField("cmTimer", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo myLifeSpanField = thisType.GetField("myLifeSpan", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo lifeTimerField = thisType.GetField("lifeTimer", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo allAdjustedField = thisType.GetField("allAdjusted", BindingFlags.NonPublic | BindingFlags.Instance);
            float setTime = setTimeField != null ? (float)setTimeField.GetValue(_this) : 0;
            GameObject lastGoodOne = lastGoodOneField != null ? (GameObject)lastGoodOneField.GetValue(_this) : null;
            float lifeTimer = lifeTimerField != null ? (float)lifeTimerField.GetValue(_this) : 0;
            float myLifeSpan = myLifeSpanField != null ? (float)myLifeSpanField.GetValue(_this) : 0;
            bool allAdjusted = allAdjustedField != null ? (bool)allAdjustedField.GetValue(_this) : false;
            float longestDistance = longestDistanceField != null ? (float)longestDistanceField.GetValue(_this) : 0;
            bool doCorrect = doCorrectField != null ? (bool)doCorrectField.GetValue(_this) : false;
            GameObject correctedOne = correctedOneField != null ? (GameObject)correctedOneField.GetValue(_this) : null;
            float cmTimer = cmTimerField != null ? (float)cmTimerField.GetValue(_this) : 0;
            if (_this.imOn && !_this.allStopped)
            {
                setTime += Time.deltaTime;
                if ((double)setTime > 0.20000000298023224)
                {
                    for (int index = 0; index < _this.myBones.Length; ++index)
                    {
                        if ((bool)(Object)_this.myBones[index] && (bool)(Object)_this.myBones[index].gameObject.GetComponent<Rigidbody>())
                        {
                            if (!_this.myBones[index].gameObject.GetComponent<Rigidbody>().isKinematic)
                            {
                                Object.Destroy((Object)_this.myBones[index].gameObject.GetComponent<Rigidbody>());
                                _this.myBones[index].gameObject.GetComponent<CMSticker>().iWasStopped = true;
                                if ((bool)(Object)lastGoodOne)
                                {
                                    _this.myBones[index].gameObject.transform.position = lastGoodOne.transform.position;
                                    _this.myBones[index].gameObject.transform.rotation = lastGoodOne.transform.rotation;
                                    _this.myBones[index].gameObject.transform.parent = lastGoodOne.transform.parent;
                                    _this.myBones[index].gameObject.GetComponent<CMSticker>().iWasPlaced = true;
                                }
                                else
                                    _this.myBones[index].transform.parent = _this.firstBone.transform;
                            }
                            else
                                lastGoodOne = _this.myBones[index].gameObject;
                        }
                    }
                    for (int index = _this.myBones.Length - 1; index > -1; --index)
                    {
                        if (_this.myBones[index].gameObject.GetComponent<CMSticker>().iWasStopped && !_this.myBones[index].gameObject.GetComponent<CMSticker>().iWasPlaced)
                        {
                            if ((bool)(Object)lastGoodOne)
                            {
                                _this.myBones[index].gameObject.transform.position = lastGoodOne.transform.position;
                                _this.myBones[index].gameObject.transform.rotation = lastGoodOne.transform.rotation;
                                _this.myBones[index].gameObject.transform.parent = lastGoodOne.transform.parent;
                                _this.myBones[index].gameObject.GetComponent<CMSticker>().iWasPlaced = true;
                            }
                        }
                        else
                            lastGoodOne = _this.myBones[index].gameObject;
                    }
                    doCorrect = false;
                    correctedOne = (GameObject)null;
                    for (int index = _this.myBones.Length - 1; index > -1; --index)
                    {
                        if (doCorrect)
                        {
                            _this.myBones[index].gameObject.transform.position = correctedOne.transform.position;
                            _this.myBones[index].gameObject.transform.rotation = correctedOne.transform.rotation;
                            _this.myBones[index].gameObject.transform.parent = correctedOne.transform.parent;
                            Debug.Log((object)$"MOVING: {index.ToString()} - {_this.myBones[index].name}");
                        }
                        if (index > 0)
                        {
                            float num = Vector3.Distance(_this.myBones[index].gameObject.transform.position, _this.myBones[index - 1].gameObject.transform.position);
                            if ((double)num > (double)longestDistance)
                                longestDistance = num;
                            if ((double)num > 0.11999999731779099)
                            {
                                doCorrect = true;
                                correctedOne = _this.myBones[index];
                            }
                        }
                    }
                    Debug.Log((object)$"LONGEST DISTANCE: {longestDistance.ToString()} - {doCorrect.ToString()}");
                    _this.allStopped = true;
                }
            }

            cmTimer += Time.deltaTime;
            if ((double)cmTimer > 4.0 && !_this.imOn)
                Object.Destroy((Object)_this.gameObject);

            bool haveRet = false;
            
            if ((double)myLifeSpan <= 0.0)
            {
                haveRet = true;
            }

            if (!haveRet)
            {
                lifeTimer += Time.deltaTime;
                if ((double)lifeTimer <= (double)myLifeSpan)
                {
                    haveRet = true;
                }
            }

            if (!haveRet)
            {
                Object.Destroy((Object)_this.gameObject);
                for (int index = 0; index < _this.myBones.Length; ++index)
                    Object.Destroy((Object)_this.myBones[index]);
                haveRet = true;
            }

            if (haveRet)
            {
                if (setTimeField != null)
                    setTimeField.SetValue(_this, setTime);
                
                if (lastGoodOneField != null)
                    lastGoodOneField.SetValue(_this, lastGoodOne);
                
                if (doCorrectField != null)
                    doCorrectField.SetValue(_this, doCorrect);
                
                if (correctedOneField != null)
                    correctedOneField.SetValue(_this, correctedOne);

                if (longestDistanceField != null)
                   longestDistanceField.SetValue(_this, longestDistance);

                if (cmTimerField != null)
                    cmTimerField.SetValue(_this, cmTimer);

                if (myLifeSpanField != null)
                    myLifeSpanField.SetValue(_this, myLifeSpan);

                if (lifeTimerField != null)
                    lifeTimerField.SetValue(_this, lifeTimer);

                if (allAdjustedField != null)
                    allAdjustedField.SetValue(_this, allAdjusted);
            }

            return false;
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

        static void MoneyOverflow_Update(object __instance)
        {
            if (ScenePersistent.myGameData.Money > maxValue)
            {
                ScenePersistent.myGameData.Money = maxValue;
            }

            if (ScenePersistent.myGameData.Money <= 0)
            {
                ScenePersistent.myGameData.Money = 0;
            }

            if (isP2)
            {
                if (SP_P2.money > maxValue)
                {
                    SP_P2.money = maxValue;
                }

                if (SP_P2.money <= 0)
                {
                    SP_P2.money = 0;
                }
            }
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
            if (!unlimitedPeoples && !allPartyParticipantsAreNaked)
            {
                return true;
            }

            SelectionProcess _this = (SelectionProcess)__instance;
            arenaProperties myProps = whichArea.GetComponent<arenaProperties>();
            myProps.numberOfPeople += 14;
            myProps.specialGirlsOnly = false;
            if (allPartyParticipantsAreNaked)
            {
                myProps.specialNude = true;
            }
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
                    ScenePersistent.myGameData.HighestOverallPoints = maxHpValue;
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