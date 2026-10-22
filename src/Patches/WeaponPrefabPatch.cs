using HarmonyLib;
using UnityEngine;
using VikingWeapons.Configuration;

namespace VikingWeapons.Patches
{
    /// <summary>
    /// Patches ObjectDB to update registered weapon prefabs with custom animations and stagger tunings.
    /// Operates exclusively on registered prefab templates in ObjectDB (Rule 1).
    /// </summary>
    [HarmonyPatch]
    public static class WeaponPrefabPatch
    {
        [HarmonyPatch(typeof(ObjectDB), "Awake")]
        [HarmonyPostfix]
        public static void PostfixObjectDBAwake(ObjectDB __instance)
        {
            ApplyPrefabModifications(__instance);
        }

        [HarmonyPatch(typeof(ObjectDB), nameof(ObjectDB.CopyOtherDB))]
        [HarmonyPostfix]
        public static void PostfixObjectDBCopyOtherDB(ObjectDB __instance)
        {
            ApplyPrefabModifications(__instance);
        }

        public static void ApplyPrefabModifications(ObjectDB? db)
        {
            if (!ModConfig.ModEnabled.Value || db == null || db.m_items == null || db.m_items.Count == 0) return;

            foreach (var go in db.m_items)
            {
                if (go == null) continue;
                var itemDrop = go.GetComponent<ItemDrop>();
                if (itemDrop?.m_itemData?.m_shared == null) continue;

                var shared = itemDrop.m_itemData.m_shared;

                // 1. Spear Primary Animation & Reach Calibration
                if (ModConfig.SpearOverhaulEnabled.Value && shared.m_skillType == Skills.SkillType.Spears &&
                    shared.m_itemType == ItemDrop.ItemData.ItemType.OneHandedWeapon)
                {
                    if (shared.m_attack != null)
                    {
                        string desiredAnim = ModConfig.SpearPrimaryAnimation.Value;
                        if (!string.IsNullOrEmpty(desiredAnim))
                        {
                            shared.m_attack.m_attackAnimation = desiredAnim;
                        }

                        if (ModConfig.SpearPrimaryReach.Value > 0f)
                        {
                            shared.m_attack.m_attackRange = ModConfig.SpearPrimaryReach.Value;
                        }

                        if (ModConfig.SpearPrimaryHeight.Value > 0f)
                        {
                            shared.m_attack.m_attackHeight = ModConfig.SpearPrimaryHeight.Value;
                        }
                    }
                }

                // 2. Polearm (Atgeir) Secondary Stagger Multiplier
                if (ModConfig.PolearmOverhaulEnabled.Value && shared.m_skillType == Skills.SkillType.Polearms)
                {
                    if (shared.m_secondaryAttack != null)
                    {
                        shared.m_secondaryAttack.m_staggerMultiplier = ModConfig.PolearmSecondaryStaggerMultiplier.Value;
                    }
                }
            }
        }
    }
}
