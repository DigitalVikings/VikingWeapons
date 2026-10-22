using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using VikingLib.Combat.Attacks;
using VikingWeapons.Configuration;

namespace VikingWeapons.Patches
{
    /// <summary>
    /// Component managing the dual visual orientations of 1H spears:
    /// - Flipped forward (180° pitch inversion) during idle, movement, and forward underhand thrusts.
    /// - Native orientation during overhand throw windups (spear_throw) so the spearhead points forward towards the target.
    /// </summary>
    public class SpearVisualController : MonoBehaviour
    {
        private Quaternion _nativeRotation;
        private Quaternion _flippedRotation;
        private Vector3 _nativePosition;
        private Vector3 _flippedPosition;
        private bool _isFlipped;

        public void Initialize(Vector3 rotEulerOffset, Vector3 posOffset)
        {
            _nativeRotation = transform.localRotation;
            _flippedRotation = _nativeRotation * Quaternion.Euler(rotEulerOffset);
            _nativePosition = transform.localPosition;
            _flippedPosition = _nativePosition + posOffset;

            SetFlipped(true);
        }

        public void SetFlipped(bool flipped)
        {
            _isFlipped = flipped;
            transform.localRotation = flipped ? _flippedRotation : _nativeRotation;
            transform.localPosition = flipped ? _flippedPosition : _nativePosition;
        }

        public bool IsFlipped => _isFlipped;
    }

    /// <summary>
    /// Corrects the visual orientation of 1H spears held in the character's right hand.
    /// Native Valheim spears have an inverted pitch (-90° X) designed for the vanilla overhand javelin stab.
    /// This patch flips the spear forward for underhand thrusts and movement, but dynamically un-flips
    /// it back to native orientation during overhand throw windups.
    /// </summary>
    [HarmonyPatch]
    public static class SpearModelPatch
    {
        private static readonly AccessTools.FieldRef<VisEquipment, GameObject> RightItemInstanceRef =
            AccessTools.FieldRefAccess<VisEquipment, GameObject>("m_rightItemInstance");

        private static readonly Dictionary<int, bool> SpearHashCache = new();

        [HarmonyPatch(typeof(VisEquipment), "SetRightHandEquipped")]
        [HarmonyPostfix]
        public static void PostfixSetRightHandEquipped(VisEquipment __instance, int hash, bool __result)
        {
            if (!ModConfig.ModEnabled.Value || !ModConfig.SpearModelFlipEnabled.Value) return;
            if (!__result || hash == 0 || __instance == null) return;
            if (__instance.m_isArmorStand && !ModConfig.SpearModelFlipArmorStand.Value) return;

            if (IsSpear(hash))
            {
                GameObject instance = RightItemInstanceRef(__instance);
                if (instance != null)
                {
                    var controller = instance.GetComponent<SpearVisualController>();
                    if (controller == null)
                    {
                        controller = instance.AddComponent<SpearVisualController>();
                    }

                    Vector3 rotEuler = new Vector3(
                        ModConfig.SpearModelFlipAngleX.Value,
                        ModConfig.SpearModelFlipAngleY.Value,
                        ModConfig.SpearModelFlipAngleZ.Value);

                    Vector3 posOffset = new Vector3(
                        ModConfig.SpearModelPositionOffsetX.Value,
                        ModConfig.SpearModelPositionOffsetY.Value,
                        ModConfig.SpearModelPositionOffsetZ.Value);

                    controller.Initialize(rotEuler, posOffset);
                }
            }
        }

        [HarmonyPatch(typeof(Attack), nameof(Attack.Start))]
        [HarmonyPrefix]
        public static void PrefixAttackStart(Attack __instance, Humanoid character)
        {
            if (!ModConfig.ModEnabled.Value || !ModConfig.SpearModelFlipEnabled.Value) return;
            if (__instance == null || character == null) return;

            if (__instance.m_attackAnimation == "spear_throw")
            {
                SetSpearFlipped(character, false);
            }
        }

        [HarmonyPatch(typeof(Attack), nameof(Attack.Stop))]
        [HarmonyPostfix]
        public static void PostfixAttackStop(Attack __instance)
        {
            if (!ModConfig.ModEnabled.Value || !ModConfig.SpearModelFlipEnabled.Value) return;
            if (__instance == null) return;

            if (__instance.m_attackAnimation == "spear_throw")
            {
                var character = HumanoidAttackHelper.GetCharacter(__instance);
                if (character != null)
                {
                    SetSpearFlipped(character, true);
                }
            }
        }

        private static void SetSpearFlipped(Humanoid character, bool flipped)
        {
            var vis = character.GetVisEquipment();
            if (vis == null) return;

            var rightItem = RightItemInstanceRef(vis);
            if (rightItem != null)
            {
                var controller = rightItem.GetComponent<SpearVisualController>();
                if (controller != null)
                {
                    controller.SetFlipped(flipped);
                }
            }
        }

        public static bool IsSpear(int hash)
        {
            if (SpearHashCache.TryGetValue(hash, out bool cached))
            {
                return cached;
            }

            if (ObjectDB.instance == null) return false;
            var prefab = ObjectDB.instance.GetItemPrefab(hash);
            if (prefab == null) return false;

            var itemDrop = prefab.GetComponent<ItemDrop>();
            bool isSpear = itemDrop?.m_itemData?.m_shared != null &&
                           itemDrop.m_itemData.m_shared.m_skillType == Skills.SkillType.Spears &&
                           itemDrop.m_itemData.m_shared.m_itemType == ItemDrop.ItemData.ItemType.OneHandedWeapon;

            SpearHashCache[hash] = isSpear;
            return isSpear;
        }
    }
}
