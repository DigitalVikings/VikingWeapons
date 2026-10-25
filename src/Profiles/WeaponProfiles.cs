using VikingLib.Combat;
using VikingLib.Combat.Attacks;
using VikingLib.StatusEffects;
using VikingWeapons.Configuration;

namespace VikingWeapons.Profiles
{
    public static class WeaponProfiles
    {
        public static void RegisterAll()
        {
            if (!ModConfig.ModEnabled.Value) return;

            // 1. Swords Overhaul: 25% cleave, +10% damage, Pierce secondary
            if (ModConfig.SwordOverhaulEnabled.Value)
            {
                var swordProfile = new WeaponAttackProfile();
                float cleave = ModConfig.SwordPrimaryCleave.Value;
                swordProfile.AddStep("primary_1", ModConfig.SwordDamageMultiplier.Value, cleave);
                swordProfile.AddStep("primary_2", ModConfig.SwordDamageMultiplier.Value, cleave);
                swordProfile.AddStep("primary_3", ModConfig.SwordFinisherMultiplier.Value, cleave);

                float secCleave = ModConfig.SwordSecondaryCleave.Value;
                var swordSec = new AttackStepModifiers(defaultMultiplier: ModConfig.SwordSecondaryDamageMultiplier.Value, cleave: secCleave);
                if (ModConfig.SwordSecondaryPierce.Value)
                {
                    swordSec.OverrideDamageType = DamageType.Pierce;
                }
                swordProfile.Steps["secondary"] = swordSec;
                WeaponAttackRegistry.Register("category_sword", swordProfile);
            }

            // 2. Battleaxes Overhaul: 40% cleave, +10% damage, Blunt secondary poke
            if (ModConfig.BattleaxeOverhaulEnabled.Value)
            {
                var battleaxeProfile = new WeaponAttackProfile();
                float cleave = ModConfig.BattleaxePrimaryCleave.Value;
                battleaxeProfile.AddStep("primary_1", ModConfig.BattleaxeDamageMultiplier.Value, cleave);
                battleaxeProfile.AddStep("primary_2", ModConfig.BattleaxeDamageMultiplier.Value, cleave);
                battleaxeProfile.AddStep("primary_3", ModConfig.BattleaxeFinisherMultiplier.Value, cleave);

                var battleaxeSec = new AttackStepModifiers(defaultMultiplier: ModConfig.BattleaxeSecondaryDamageMultiplier.Value, cleave: 0.375f);
                if (ModConfig.BattleaxeSecondaryDamageTypeBlunt.Value)
                {
                    battleaxeSec.OverrideDamageType = DamageType.Blunt;
                }
                battleaxeProfile.Steps["secondary"] = battleaxeSec;
                WeaponAttackRegistry.Register("category_battleaxe", battleaxeProfile);
            }

            // 3. Polearms (Atgeirs) Overhaul: 40% cleave on spin, Slash damage conversion
            if (ModConfig.PolearmOverhaulEnabled.Value)
            {
                var atgeirProfile = new WeaponAttackProfile();
                atgeirProfile.AddStep("primary_1", 1.0f, 0.375f);
                atgeirProfile.AddStep("primary_2", 1.0f, 0.375f);
                atgeirProfile.AddStep("primary_3", 2.0f, 0.375f);

                var atgeirSec = new AttackStepModifiers(defaultMultiplier: 1.0f, cleave: ModConfig.PolearmSecondaryCleave.Value);
                if (ModConfig.PolearmSecondarySlashDamage.Value)
                {
                    atgeirSec.OverrideDamageType = DamageType.Slash;
                }
                atgeirProfile.Steps["secondary"] = atgeirSec;
                WeaponAttackRegistry.Register("category_atgeir", atgeirProfile);
            }

            // 4. Spears Overhaul: Thrown spear inflicts Cripple / Hamstring
            if (ModConfig.SpearOverhaulEnabled.Value)
            {
                var spearProfile = new WeaponAttackProfile();
                spearProfile.AddStep("primary_1", 1.0f, 0.375f);

                var spearSec = new AttackStepModifiers(defaultMultiplier: 1.5f, cleave: 0f);
                if (ModConfig.SpearSecondaryHamstringEnabled.Value)
                {
                    spearSec.OnHitStatusEffect = CanonicalStatusEffects.CrippleId;
                    spearSec.StatusEffectDuration = ModConfig.SpearSecondaryHamstringDuration.Value;
                    spearSec.StatusEffectValue = ModConfig.SpearSecondaryHamstringPotency.Value;
                }
                spearProfile.Steps["secondary"] = spearSec;
                WeaponAttackRegistry.Register("category_spear", spearProfile);
            }

            // 5. Knives Overhaul: Slash on primary_1 & 2, Pierce on primary_3 & secondary
            if (ModConfig.KnifeOverhaulEnabled.Value)
            {
                var knifeProfile = new WeaponAttackProfile();
                float cleave = ModConfig.KnifePrimaryCleave.Value;

                var p1 = new AttackStepModifiers(defaultMultiplier: ModConfig.KnifeDamageMultiplier.Value, cleave: cleave);
                if (ModConfig.KnifePrimarySlashDamage.Value)
                {
                    p1.OverrideDamageType = DamageType.Slash;
                }
                knifeProfile.Steps["primary_1"] = p1;

                var p2 = new AttackStepModifiers(defaultMultiplier: ModConfig.KnifeDamageMultiplier.Value, cleave: cleave);
                if (ModConfig.KnifePrimarySlashDamage.Value)
                {
                    p2.OverrideDamageType = DamageType.Slash;
                }
                knifeProfile.Steps["primary_2"] = p2;

                var p3 = new AttackStepModifiers(defaultMultiplier: ModConfig.KnifeFinisherMultiplier.Value, cleave: cleave);
                if (ModConfig.KnifeFinisherPierceDamage.Value)
                {
                    p3.OverrideDamageType = DamageType.Pierce;
                }
                knifeProfile.Steps["primary_3"] = p3;

                float secCleave = ModConfig.KnifeSecondaryCleave.Value;
                var knifeSec = new AttackStepModifiers(defaultMultiplier: ModConfig.KnifeSecondaryDamageMultiplier.Value, cleave: secCleave);
                if (ModConfig.KnifeSecondaryPierceDamage.Value)
                {
                    knifeSec.OverrideDamageType = DamageType.Pierce;
                }
                knifeProfile.Steps["secondary"] = knifeSec;

                WeaponAttackRegistry.Register("category_knife", knifeProfile);
            }
        }
    }
}
