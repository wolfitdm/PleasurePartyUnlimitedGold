using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using HarmonyLib;
using BepInEx;
using System.Reflection;

namespace AlwaysBeNaked {

   [BepInPlugin("com.wolfitdm.AlwaysBeNaked", "AlwaysBeNaked Plugin", "1.0.0.0")]
    public class AlwaysBeNaked : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;

        public AlwaysBeNaked()
        {
        }

        private void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;
            Harmony.CreateAndPatchAll(typeof(AlwaysBeNaked));
            Logger.LogInfo($"Plugin AlwaysBeNaked is loaded!");
        }


        [HarmonyPatch(typeof(Character_Clothes_UW), "CheckAllClothes_Player")] // Specify target method with HarmonyPatch attribute
        [HarmonyPostfix]                              // There are different patch types. Prefix code runs before original code
        static void CheckAllClothes_Player(bool updateBreasts, bool updateCurs, object __instance)
        {
            Character_Clothes_UW _this = (Character_Clothes_UW)__instance;
            if (_this.isPlayer)
            {
                _this.hasPantsNow = true;
                _this.hasShirtNow = true;
            }
        }

        [HarmonyPatch(typeof(Character_Clothes_UW), "StartScripts")] // Specify target method with HarmonyPatch attribute
        [HarmonyPostfix]                                            // There are different patch types. Prefix code runs before original code
        static void StartScripts(object __instance)
        {
            Character_Clothes_UW _this = (Character_Clothes_UW)__instance;
            if (_this.isPlayer)
            {
                _this.hasPantsNow = true;
                _this.hasShirtNow = true;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object other)
        {
            return base.Equals(other);
        }
    }
}