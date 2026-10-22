using BepInEx.Configuration;

namespace VikingWeapons.Configuration
{
    public static class ModConfig
    {
        public static ConfigEntry<bool> ModEnabled = null!;

        // 1. Axes
        public static ConfigEntry<bool> AxeSecondarySpeedEnabled = null!;
        public static ConfigEntry<float> AxeSecondarySpeedMultiplier = null!;

        // 2. Battleaxes
        public static ConfigEntry<bool> BattleaxeOverhaulEnabled = null!;
        public static ConfigEntry<float> BattleaxePrimaryCleave = null!;
        public static ConfigEntry<float> BattleaxeDamageMultiplier = null!;
        public static ConfigEntry<float> BattleaxeFinisherMultiplier = null!;
        public static ConfigEntry<bool> BattleaxeSecondaryDamageTypeBlunt = null!;
        public static ConfigEntry<float> BattleaxeSecondaryDamageMultiplier = null!;

        // 3. Swords
        public static ConfigEntry<bool> SwordOverhaulEnabled = null!;
        public static ConfigEntry<float> SwordPrimaryCleave = null!;
        public static ConfigEntry<float> SwordDamageMultiplier = null!;
        public static ConfigEntry<float> SwordFinisherMultiplier = null!;
        public static ConfigEntry<bool> SwordSecondaryPierce = null!;
        public static ConfigEntry<float> SwordAttackSpeedMultiplier = null!;

        // 4. Clubs / Maces
        public static ConfigEntry<bool> ClubSecondarySpeedEnabled = null!;
        public static ConfigEntry<float> ClubSecondarySpeedMultiplier = null!;

        // 5. Spears
        public static ConfigEntry<bool> SpearOverhaulEnabled = null!;
        public static ConfigEntry<string> SpearPrimaryAnimation = null!;
        public static ConfigEntry<float> SpearPokeSpeedMultiplier = null!;
        public static ConfigEntry<float> SpearPrimaryReach = null!;
        public static ConfigEntry<float> SpearPrimaryHeight = null!;
        public static ConfigEntry<bool> SpearModelFlipEnabled = null!;
        public static ConfigEntry<float> SpearModelFlipAngleX = null!;
        public static ConfigEntry<float> SpearModelFlipAngleY = null!;
        public static ConfigEntry<float> SpearModelFlipAngleZ = null!;
        public static ConfigEntry<float> SpearModelPositionOffsetX = null!;
        public static ConfigEntry<float> SpearModelPositionOffsetY = null!;
        public static ConfigEntry<float> SpearModelPositionOffsetZ = null!;
        public static ConfigEntry<bool> SpearModelFlipArmorStand = null!;
        public static ConfigEntry<bool> SpearSecondaryHamstringEnabled = null!;
        public static ConfigEntry<float> SpearSecondaryHamstringDuration = null!;
        public static ConfigEntry<float> SpearSecondaryHamstringPotency = null!;

        // 6. Polearms (Atgeirs)
        public static ConfigEntry<bool> PolearmOverhaulEnabled = null!;
        public static ConfigEntry<float> PolearmSecondaryCleave = null!;
        public static ConfigEntry<bool> PolearmSecondarySlashDamage = null!;
        public static ConfigEntry<float> PolearmSecondaryStaggerMultiplier = null!;

        public static void Init(ConfigFile config)
        {
            ModEnabled = config.Bind("0. General", "ModEnabled", true, "Master toggle for VikingWeapons.");

            // 1. Axes
            AxeSecondarySpeedEnabled = config.Bind("1. Axes", "AxeSecondarySpeedEnabled", true, "Speeds up the 1H Axe secondary uppercut attack.");
            AxeSecondarySpeedMultiplier = config.Bind("1. Axes", "AxeSecondarySpeedMultiplier", 1.30f, "Playback speed multiplier for Axe secondary attack.");

            // 2. Battleaxes
            BattleaxeOverhaulEnabled = config.Bind("2. Battleaxes", "BattleaxeOverhaulEnabled", true, "Enables the Battleaxe archetype overhaul.");
            BattleaxePrimaryCleave = config.Bind("2. Battleaxes", "BattleaxePrimaryCleave", 0.40f, "Cleave efficiency factor for Battleaxe primary attacks (default 40%).");
            BattleaxeDamageMultiplier = config.Bind("2. Battleaxes", "BattleaxeDamageMultiplier", 1.10f, "Base damage step multiplier for Battleaxe primary swings (1.10x).");
            BattleaxeFinisherMultiplier = config.Bind("2. Battleaxes", "BattleaxeFinisherMultiplier", 2.20f, "Base damage step multiplier for Battleaxe combo finisher (2.20x).");
            BattleaxeSecondaryDamageTypeBlunt = config.Bind("2. Battleaxes", "BattleaxeSecondaryDamageTypeBlunt", true, "Converts Battleaxe secondary attack damage to Blunt.");
            BattleaxeSecondaryDamageMultiplier = config.Bind("2. Battleaxes", "BattleaxeSecondaryDamageMultiplier", 0.60f, "Damage multiplier for Battleaxe secondary attack poke.");

            // 3. Swords
            SwordOverhaulEnabled = config.Bind("3. Swords", "SwordOverhaulEnabled", true, "Enables the Sword archetype overhaul.");
            SwordPrimaryCleave = config.Bind("3. Swords", "SwordPrimaryCleave", 0.25f, "Cleave efficiency factor for Sword primary attacks (reduced from native 100% to 25%).");
            SwordDamageMultiplier = config.Bind("3. Swords", "SwordDamageMultiplier", 1.10f, "Compensatory base damage step multiplier for Sword primary swings (1.10x).");
            SwordFinisherMultiplier = config.Bind("3. Swords", "SwordFinisherMultiplier", 2.20f, "Compensatory base damage step multiplier for Sword combo finisher (2.20x).");
            SwordSecondaryPierce = config.Bind("3. Swords", "SwordSecondaryPierce", true, "Converts Sword secondary thrust attack damage to Pierce.");
            SwordAttackSpeedMultiplier = config.Bind("3. Swords", "SwordAttackSpeedMultiplier", 1.10f, "Compensatory attack animation speed multiplier for swords.");

            // 4. Clubs
            ClubSecondarySpeedEnabled = config.Bind("4. Clubs", "ClubSecondarySpeedEnabled", true, "Speeds up Club/Mace secondary attack to reduce enemy interruption.");
            ClubSecondarySpeedMultiplier = config.Bind("4. Clubs", "ClubSecondarySpeedMultiplier", 1.30f, "Playback speed multiplier for Club/Mace secondary attack.");

            // 5. Spears
            SpearOverhaulEnabled = config.Bind("5. Spears", "SpearOverhaulEnabled", true, "Enables the Spear archetype overhaul.");
            SpearPrimaryAnimation = config.Bind("5. Spears", "SpearPrimaryAnimation", "sword_secondary", "Primary attack animation trigger ('sword_secondary' for forward poke, 'knife_stab', or 'spear_poke').");
            SpearPokeSpeedMultiplier = config.Bind("5. Spears", "SpearPokeSpeedMultiplier", 0.45f, "Playback speed multiplier for the forward spear poke animation (default 0.45x provides a balanced, natural thrust cadence).");
            SpearPrimaryReach = config.Bind("5. Spears", "SpearPrimaryReach", 1.9f, "Attack range (reach in meters) for spear primary poke (native spear reach is 1.9m).");
            SpearPrimaryHeight = config.Bind("5. Spears", "SpearPrimaryHeight", 1.0f, "Vertical attack height in meters for spear primary poke (1.0m matches waist/chest underhand thrust; native overhand was 1.5m).");
            SpearModelFlipEnabled = config.Bind("5. Spears", "SpearModelFlipEnabled", true, "Flips equipped 1H spears 180 degrees in the hand so the spearhead points forward during thrusts.");
            SpearModelFlipAngleX = config.Bind("5. Spears", "SpearModelFlipAngleX", 180f, "Euler X rotation offset applied to equipped spears.");
            SpearModelFlipAngleY = config.Bind("5. Spears", "SpearModelFlipAngleY", 0f, "Euler Y rotation offset applied to equipped spears.");
            SpearModelFlipAngleZ = config.Bind("5. Spears", "SpearModelFlipAngleZ", 0f, "Euler Z rotation offset applied to equipped spears.");
            SpearModelPositionOffsetX = config.Bind("5. Spears", "SpearModelPositionOffsetX", 0f, "Local X position offset applied to equipped spears.");
            SpearModelPositionOffsetY = config.Bind("5. Spears", "SpearModelPositionOffsetY", 0f, "Local Y position offset applied to equipped spears.");
            SpearModelPositionOffsetZ = config.Bind("5. Spears", "SpearModelPositionOffsetZ", 0f, "Local Z position offset applied to equipped spears.");
            SpearModelFlipArmorStand = config.Bind("5. Spears", "SpearModelFlipArmorStand", false, "Whether to apply the spear flip to armor stands (default false keeps spears upright on stands).");
            SpearSecondaryHamstringEnabled = config.Bind("5. Spears", "SpearSecondaryHamstringEnabled", true, "Thrown spears inflict Hamstring/Cripple on struck targets.");
            SpearSecondaryHamstringDuration = config.Bind("5. Spears", "SpearSecondaryHamstringDuration", 6.0f, "Duration in seconds for the spear hamstring/cripple effect.");
            SpearSecondaryHamstringPotency = config.Bind("5. Spears", "SpearSecondaryHamstringPotency", 0.50f, "Potency slice value for the cripple slow (0.50 = 33% slow).");

            // 6. Polearms
            PolearmOverhaulEnabled = config.Bind("6. Polearms", "PolearmOverhaulEnabled", true, "Enables the Polearm (Atgeir) archetype overhaul.");
            PolearmSecondaryCleave = config.Bind("6. Polearms", "PolearmSecondaryCleave", 0.40f, "Cleave efficiency factor for Atgeir secondary 360-spin (reduced from native 100% to 40%).");
            PolearmSecondarySlashDamage = config.Bind("6. Polearms", "PolearmSecondarySlashDamage", true, "Converts Atgeir secondary spin attack damage to Slash.");
            PolearmSecondaryStaggerMultiplier = config.Bind("6. Polearms", "PolearmSecondaryStaggerMultiplier", 1.0f, "Secondary spin stagger multiplier (down from native 6.0x, relies on 5.0x knockback).");
        }
    }
}
