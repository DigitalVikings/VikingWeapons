# VikingWeapons

**VikingWeapons** reimagines Valheim's melee combat archetypes with tactile secondary attacks, underhand spear mechanics, multi-target cleave profiles, and animation speed calibrations built on VikingLib.

![Spear Pointed Forward](https://raw.githubusercontent.com/DigitalVikings/VikingWeapons/main/spear_pointed_forward.png)

> ⚠️ **Work in Progress (Pre-Release v0.1.0)**: This mod is in active development and has not been tested extensively. Features, balance, and mechanics may change, and unintended bugs or edge cases may occur. Backup your character and world saves before testing.

> ⚡ **Powered by VikingLib**: This mod requires [VikingLib](https://valheim.thunderstore.io/package/DigitalVikings/VikingLib/). 
> VikingLib replaces Valheim's native combat calculation and stat engines. Not all mods may be compatible / work as expected with this.

---

## How It Works

Each weapon family gains a distinct tactical role, distinct damage types, and responsive animation pacing:

- **1H Spear Overhaul & Visual Flip**: Corrects Valheim's awkward inverted spear orientation. Spears are gripped pointing forward for underhand thrusts and movement, dynamically flipping back to native overhand posture during throw windups. Forward reach is extended and vertical hitboxes are lowered to consistently hit low targets (necks and boars). Secondary throws inflict a **Hamstring** movement slow.
- **Tactical Secondary Conversions**: Secondary attacks serve distinct functional roles: Sword secondary attacks deal **Piercing** thrust damage; Battleaxe secondary attacks deal **Blunt** pommel-poke damage with heavy stagger; Atgeir 360° spin attacks convert to wide **Slashing** cleaves.
- **Combat Cleave Profiles**: Primary weapon swings utilize VikingLib's cleave engine to damage multiple foes in tight arcs without split damage penalties (25% cleave for swords, 40% cleave for battleaxes).
- **Responsive Attack Cadence**: Heavy secondary windups on 1H Axes and Clubs/Maces are accelerated to feel snappy and competitive in fast-paced combat.

<details>
<summary><b>Click to expand Weapon Archetype & Attack Calibrations</b></summary>

| Weapon Family | Primary Changes | Secondary Changes | Special Mechanics |
| :--- | :--- | :--- | :--- |
| **Spears (1H)** | Forward underhand thrusts, +0.6m reach, lowered attack origin | Vanilla throw | Forward visual flip; throws inflict **Hamstring** (-30% speed for 6s) |
| **Swords (1H)** | +10% base damage, 25% primary cleave across combo | Converted to **Piercing** damage | 2.5× finisher multiplier on 3rd combo strike |
| **Battleaxes (2H)**| +10% base damage, 40% primary cleave across combo | Converted to **Blunt** damage | Pommel poke deals heavy stagger damage; 2.5× finisher multiplier |
| **Polearms (Atgeirs)**| 37.5% cleave on primary attacks | Converted to **Slashing** damage | 40% cleave on 360° spin; 6.0× stagger multiplier |
| **Axes (1H)** | Vanilla | Accelerated swing speed (+30%) | Faster secondary overhead chop |
| **Clubs / Maces** | Vanilla | Accelerated swing speed (+25%) | Faster secondary uppercut |

</details>
