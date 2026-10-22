using HarmonyLib;
using UnityEngine;
using VikingLib.Combat.Attacks;
using VikingWeapons.Configuration;

namespace VikingWeapons.Patches
{
    /// <summary>
    /// Dynamically scales attack animation playback speed for specific weapon steps
    /// (e.g. faster Axe & Club secondary attacks, sword speed compensation, and snappy spear pokes).
    /// Intercepts both Attack.Start and CharacterAnimEvent.Speed to prevent embedded animation
    /// clip speed events (such as sword_secondary's slow-motion 0.2x curve) from overriding speed.
    /// </summary>
    [HarmonyPatch]
    public static class WeaponAttackSpeedPatch
    {
        private static readonly AccessTools.FieldRef<ZSyncAnimation, Animator> AnimatorRef =
            AccessTools.FieldRefAccess<ZSyncAnimation, Animator>("m_animator");

        private static readonly AccessTools.FieldRef<CharacterAnimEvent, Character> CharacterAnimRef =
            AccessTools.FieldRefAccess<CharacterAnimEvent, Character>("m_character");

        [HarmonyPatch(typeof(Attack), nameof(Attack.Start))]
        [HarmonyPostfix]
        [HarmonyPriority(Priority.Low)] // Executes after VikingLib baseline stat scaling
        public static void PostfixAttackStart(Attack __instance, bool __result, Humanoid character, ZSyncAnimation zanim)
        {
            if (!ModConfig.ModEnabled.Value || !__result || character == null || zanim == null) return;

            var weapon = __instance.GetWeapon() ?? character.GetCurrentWeapon();
            if (weapon?.m_shared == null) return;

            bool isSecondary = weapon.m_shared.m_secondaryAttack != null &&
                               __instance.m_attackAnimation == weapon.m_shared.m_secondaryAttack.m_attackAnimation;

            if (isSecondary)
            {
                if (weapon.m_shared.m_skillType == Skills.SkillType.Axes && ModConfig.AxeSecondarySpeedEnabled.Value)
                {
                    MultiplyAnimSpeed(zanim, ModConfig.AxeSecondarySpeedMultiplier.Value);
                }
                else if (weapon.m_shared.m_skillType == Skills.SkillType.Clubs && ModConfig.ClubSecondarySpeedEnabled.Value)
                {
                    MultiplyAnimSpeed(zanim, ModConfig.ClubSecondarySpeedMultiplier.Value);
                }
            }
            else
            {
                if (weapon.m_shared.m_skillType == Skills.SkillType.Swords && ModConfig.SwordOverhaulEnabled.Value)
                {
                    MultiplyAnimSpeed(zanim, ModConfig.SwordAttackSpeedMultiplier.Value);
                }
                else if (weapon.m_shared.m_skillType == Skills.SkillType.Spears && ModConfig.SpearOverhaulEnabled.Value)
                {
                    if (ModConfig.SpearPrimaryAnimation.Value == "sword_secondary" && ModConfig.SpearPokeSpeedMultiplier.Value > 0f)
                    {
                        MultiplyAnimSpeed(zanim, ModConfig.SpearPokeSpeedMultiplier.Value);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(CharacterAnimEvent), nameof(CharacterAnimEvent.Speed))]
        [HarmonyPrefix]
        public static void PrefixCharacterAnimEventSpeed(CharacterAnimEvent __instance, ref float speedScale)
        {
            if (!ModConfig.ModEnabled.Value || __instance == null) return;
            var character = CharacterAnimRef(__instance);
            var humanoid = character as Humanoid;
            if (humanoid == null) return;

            var weapon = humanoid.GetCurrentWeapon();
            if (weapon?.m_shared == null) return;

            var currentAttack = HumanoidAttackHelper.GetCurrentAttack(humanoid);

            // 1. Spear Primary Attack (sword_secondary animation)
            if (weapon.m_shared.m_skillType == Skills.SkillType.Spears && ModConfig.SpearOverhaulEnabled.Value)
            {
                string currentAnim = currentAttack?.m_attackAnimation ?? weapon.m_shared.m_attack?.m_attackAnimation ?? "";
                if (currentAnim == "sword_secondary")
                {
                    // In Sword-Attack-R4, vanilla events are Speed(0.2) [windup], Speed(0.6) [thrust], Speed(0.2) [recovery].
                    // Those 0.2 values make the sword lunge take 1.75s!
                    // For a fast, snappy spear poke, map:
                    // <= 0.25f (windup/recovery) -> 1.0f
                    // > 0.25f (thrust forward) -> 1.5f
                    float baseSpeed = speedScale <= 0.25f ? 1.0f : 1.5f;
                    speedScale = baseSpeed * ModConfig.SpearPokeSpeedMultiplier.Value;
                    return;
                }
            }

            // 2. Axe Secondary Attack
            if (weapon.m_shared.m_skillType == Skills.SkillType.Axes && ModConfig.AxeSecondarySpeedEnabled.Value)
            {
                string currentAnim = currentAttack?.m_attackAnimation ?? weapon.m_shared.m_secondaryAttack?.m_attackAnimation ?? "";
                if (currentAnim == "axe_secondary")
                {
                    speedScale *= ModConfig.AxeSecondarySpeedMultiplier.Value;
                    return;
                }
            }

            // 3. Club Secondary Attack
            if (weapon.m_shared.m_skillType == Skills.SkillType.Clubs && ModConfig.ClubSecondarySpeedEnabled.Value)
            {
                string currentAnim = currentAttack?.m_attackAnimation ?? weapon.m_shared.m_secondaryAttack?.m_attackAnimation ?? "";
                if (currentAnim == "club_secondary")
                {
                    speedScale *= ModConfig.ClubSecondarySpeedMultiplier.Value;
                    return;
                }
            }

            // 4. Sword Speed
            if (weapon.m_shared.m_skillType == Skills.SkillType.Swords && ModConfig.SwordOverhaulEnabled.Value)
            {
                speedScale *= ModConfig.SwordAttackSpeedMultiplier.Value;
                return;
            }
        }

        private static void MultiplyAnimSpeed(ZSyncAnimation zanim, float factor)
        {
            if (zanim == null || Mathf.Approximately(factor, 1.0f)) return;
            var animator = AnimatorRef(zanim);
            if (animator == null) return;
            float currentSpeed = animator.speed;
            zanim.SetSpeed(currentSpeed * factor);
        }
    }
}
