using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using VikingWeapons.Configuration;
using VikingWeapons.Profiles;

namespace VikingWeapons
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    [BepInDependency("com.digitalvikings.vikinglib", BepInDependency.DependencyFlags.HardDependency)]
    public class VikingWeaponsPlugin : BaseUnityPlugin
    {
        public const string ModGUID = "com.digitalvikings.vikingweapons";
        public const string ModName = "VikingWeapons";
        public const string ModVersion = "0.1.0";

        public static VikingWeaponsPlugin Instance { get; private set; } = null!;
        public static ManualLogSource Log { get; private set; } = null!;

        private Harmony _harmony = null!;

        private void Awake()
        {
            Instance = this;
            Log = Logger;

            ModConfig.Init(Config);

            _harmony = new Harmony(ModGUID);
            _harmony.PatchAll();

            WeaponProfiles.RegisterAll();

            Log.LogInfo($"{ModName} v{ModVersion} initialized successfully.");
        }

        private void OnDestroy()
        {
            _harmony.UnpatchSelf();
        }
    }
}
