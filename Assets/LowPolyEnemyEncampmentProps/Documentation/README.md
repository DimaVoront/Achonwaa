# Low Poly Enemy Encampment Props

Version 1.0.0 · Unity 6000.4.5f1 or newer · Universal Render Pipeline (URP 17.4.0)

Low Poly Enemy Encampment Props is the camp where the enemies of Low Poly Enemy Melee Weapons live: 50 stylized, flat-shaded low-poly props for raider, orc and goblin camps. It uses the same art direction as Low Poly Enemy Melee Weapons, so the props and the weapons can share a scene.

## Contents

50 prefabs in 7 categories:

- **Fortifications (8):** Wooden Barricade, Spiked Barricade, Scrap Barricade, Stake Fence, Defensive Stakes, Small Palisade Section, Reinforced Palisade Section, Spiked Road Block
- **Camp Structures (8):** Small Enemy Tent, Large Enemy Tent, Bedroll, Campfire, Cooking Tripod, Cooking Pot, Rough Stool, Rough Camp Table
- **Storage & Supplies (8):** Wooden Crate, Reinforced Crate, Broken Crate, Wooden Barrel, Broken Barrel, Supply Sack, Woven Basket, Supply Cart
- **Enemy Utility (8):** Weapon Rack, Wall Weapon Rack, Grinding Stone, Chopping Block, Training Dummy, Small Cage, Prisoner Cage, Butcher Table
- **Lighting & Identity (6):** Torch Stand, Wall Torch, Brazier, Hanging Lantern, Enemy Banner, Warning Sign
- **Bone & Ritual (6):** Bone Pile, Skull Pile, Bone Totem, Skull Totem, Ritual Altar, Monster Nest
- **Clutter & Debris (6):** Firewood Pile, Scrap Metal Pile, Rope Coil, Broken Cart Wheel, Supply Pile, Broken Wood Debris

| Folder | Content |
| --- | --- |
| `Prefabs/<Category>/PF_<Name>.prefab` | 50 ready-to-use prefabs (use these, not the raw FBX files) |
| `Meshes/<Category>/SM_<Name>.fbx` | 50 source meshes |
| `Textures/T_EEP_<Material>_<Map>.png` | 49 tileable 1024x1024 PNG textures |
| `Materials/MAT_EEP_<Material>.mat` | 23 materials shared by all prefabs (21 URP Lit, 2 with the flame shader) |
| `Shaders/` | `EEP_FlameWave.shader` (`EEP/FlameWave`): the animated flame shader |
| `Animations/` | 18 Animator Controllers and 18 AnimationClips that make the fire lights flicker |
| `Demo/` | demo scene `LowPolyEnemyEncampmentProps_Demo.unity`, its lighting data and the showcase-only materials (`MAT_Demo_Ground`, `MAT_Demo_Wall`), which are not part of the prop set |
| `Documentation/` | this file |

The props are static meshes: nothing is rigged and there is no character animation. The only motion is the fire: the flames move through a vertex shader and the fire lights flicker through AnimationClips (see Fire props). Not included: characters, character rigs and animations, gameplay scripts, particle effects and the weapons themselves (those are in Low Poly Enemy Melee Weapons).

## Render pipeline and version

- Made with Unity 6000.4.5f1 and URP 17.4.0. 21 materials use the `Universal Render Pipeline/Lit` shader (metallic workflow). `MAT_EEP_Flame` and `MAT_EEP_FlameCore` use `EEP/FlameWave`, a URP shader included in the pack (SRP Batcher compatible) that animates the flame and otherwise shades like URP Lit with the same maps.
- Built-in and HDRP are not set up. The packed mask texture (`_URPMask`: R = metallic, G = ambient occlusion, A = smoothness) already has the channel layout of the HDRP Mask Map and of the Built-in Standard shader's Metallic (R, smoothness in A) and Occlusion (G) slots, so a material can be converted by switching the shader and assigning `_BC` and `_URPMask`. The flame shader is URP only: in another pipeline the flame materials render pink, and switching them to a Lit/Standard shader gives a still flame.

## How to use

1. Import the package into a URP project (Unity 6000.4.5f1 or newer).
2. Open `LowPolyEnemyEncampmentProps/Demo/LowPolyEnemyEncampmentProps_Demo.unity` to see the props.
3. Drag any `PF_` prefab from `Prefabs/<Category>/` into your scene. Material, collider and, on the fire props, the flame and its light are already set up.
4. Move, rotate and scale the prefab root. Leave the `SM_` child as it is (identity transform).

## Conventions

- **Scale:** 1 Unity unit = 1 meter, real-world size, checked against a 1.8 m human during production.
- **Orientation:** +Y is up and **+Z is the front** of every prop. Prefab roots and the `SM_` children have zero rotation and scale 1.
- **Hierarchy:** `PF_<Name>` (root, colliders) → `SM_<Name>` (mesh) [→ `SM_<Name>_Flame`], plus `FireLight` (Light + Animator) on the fire props.
- **Pivots:**
  - Bottom center, on the ground: 44 props. The pivot is at the center of the base, on the ground (Y = 0).
  - Wall line: Stake Fence, Small Palisade Section and Reinforced Palisade Section. X runs from -1.000 to +1.000, Y = 0 is the ground and Z = 0 is the axis of the wall, so sections placed on the same line meet without steps (see Modular walls).
  - Back center: Wall Weapon Rack and Wall Torch. The back face sits at Z = 0 and the prop extends toward +Z; X and Y are centered. Place the pivot on the wall surface.
  - Top center: Hanging Lantern. The top (hanging point) is at Y = 0 and the prop hangs down to Y = -0.488 m.
- **Closed from below:** every prop is modeled with its base closed, so it stays solid when seen from below. All 50 meshes pass a check that casts rays upward and at 35° from under the prop (at most 0.5% of hits may land on back faces; thin decals such as nail heads and blood are exempt).
- **Naming:** `PF_` prefab · `SM_` mesh · `MAT_EEP_` material · `T_EEP_<Material>_<Map>` texture · `AC_`/`AN_` Animator Controller / AnimationClip.

## Demo scene

Open `LowPolyEnemyEncampmentProps/Demo/LowPolyEnemyEncampmentProps_Demo.unity`. It needs no scripts or extra packages and is not added to your Build Settings.

- **Showcase_AllProps:** all 50 prefabs at real-world scale, grouped by category, facing the Main Camera. The Supply Cart in this grid is turned 90° to fit its row. The last row shows the modular sections snapped side by side, the Wall Weapon Rack and Wall Torch mounted on a demo wall, and the Hanging Lantern on a demo post.
- **Vignette_EnemyCamp:** a small enemy camp built from the same prefabs. It includes a campfire with the Cooking Pot hanging from the Cooking Tripod, tents, a palisade backdrop of snapped sections, torches, storage, bone props and a second tripod holding a Hanging Lantern.
- **Environment:** a directional Sun, a large dark ground plane, the default skybox for ambient light and reflections, a baked Reflection Probe covering the showcase and the camp, and a Global Volume (bloom and neutral tonemapping). The ground and wall materials in `Demo/Materials` are for the showcase only.

| Camera | Use |
| --- | --- |
| `Main Camera` | active; overview of the whole grid |
| `Cam_Vignette_Wide` | disabled; wide view of the camp |
| `Cam_Vignette_Close` | disabled; closer view into the camp |

To look through one of the vignette cameras, enable it and disable the Main Camera (or set its Depth higher).

The demo's baked lighting data (reflection probe and skybox reflection) is stored in `Demo/LowPolyEnemyEncampmentProps_Demo/`.

## Lighting note: metal needs reflections

The iron is a dark blue-grey metal (metallic 0.80, average base colour 0.25 / 0.28 / 0.30 sRGB), and the rusted iron mixes that metal with non-metallic rust (average metallic 0.49). Metals get almost all of their look from reflected light, so **use a skybox with Environment Reflections, or a Reflection Probe covering the area where the props stand** (the demo scene uses both). Without reflections the iron parts render almost black. The ambient occlusion channel of the textures is flat (1.0): the tileable textures carry no baked occlusion, so baked lighting or SSAO in your scene adds contact shading.

For baked lighting, enable **Generate Lightmap UVs** in the import settings of the FBX files you bake: the meshes have one UV channel, used by the tileable textures.

## Fire props

Campfire, Torch Stand, Wall Torch, Brazier, Hanging Lantern and Ritual Altar have an animated low-poly flame and a flickering light. No C# script is involved: the flame moves in its shader and the light is driven by an Animator.

- **Flame:** a separate mesh, `SM_<Name>_Flame`, child of `SM_<Name>`. It uses `MAT_EEP_Flame` and `MAT_EEP_FlameCore` (shader `EEP/FlameWave`), casts no shadows and is not part of the collider. The tongues sway and rise while their base stays in place. Each copy of a prefab gets its own phase from its position, so neighbouring fires do not move in step.
- **Light:** `FireLight` is a warm point light without shadows on the prefab root. Its Animator plays a 3 s looping clip that varies the intensity by up to ±10% around the base value (Culling Mode: Always Animate).

| Prefab | Tris (body + flame) | Flame height (m) | Light range (m) | Base intensity | Animated intensity | Glowing embers on the body |
| --- | --- | --- | --- | --- | --- | --- |
| Campfire | 660 + 144 | 0.59 | 5.28 | 2.93 | 2.65 to 3.14 | yes |
| Torch Stand | 580 + 144 | 0.29 | 2.59 | 1.44 | 1.31 to 1.55 | yes |
| Wall Torch | 238 + 144 | 0.25 | 2.50 | 1.24 | 1.12 to 1.33 | yes |
| Brazier | 532 + 182 | 0.45 | 4.04 | 2.24 | 2.03 to 2.41 | yes |
| Hanging Lantern | 416 + 76 | 0.15 | 2.50 | 1.20 | 1.09 to 1.29 | no |
| Ritual Altar | 952 + 240 | 0.24 | 2.82 | 1.20 | 1.09 to 1.29 | no |

### Turning a fire on and off

Disable the flame object and the light together, in the editor or from code. Enable both again to relight the fire.

```csharp
var t = campfire.transform;                        // instance of PF_Campfire
t.Find("SM_Campfire/SM_Campfire_Flame").gameObject.SetActive(false);
t.Find("FireLight").gameObject.SetActive(false);   // also stops its Animator
```

Embers on the body (`MAT_EEP_Ember`) keep glowing. For a completely cold prop, assign a non-emissive material such as `MAT_EEP_Ash` to that slot of the `SM_` renderer.

### Adjusting the flame motion

The motion is set on the flame materials, which all fire props share. Change the values on `MAT_EEP_Flame` and `MAT_EEP_FlameCore` to change every fire. To change one fire only, duplicate both materials and assign the copies to that prefab's `SM_<Name>_Flame` renderer. Values as shipped:

| Property | Value | Effect |
| --- | --- | --- |
| Amplitude | 0.09 | sideways sway, as a fraction of each tongue's height |
| Speed | 1 | animation speed; the motion repeats every 3 / Speed seconds |
| Frequency | 0.9 | number of waves travelling up each tongue |
| Rise Scale | 0.1 | vertical stretch, as a fraction of the tongue's height |
| Base Lock | 1.6 | how firmly the base stays in place (higher = more of the tongue stays still) |
| Instance Phase | 1 | 1 = each copy gets its own phase and sway axis from its position; 0 = all copies move together |

Amplitude 0 and Rise Scale 0 give a still flame that looks like URP Lit with the same maps. The shader reads the animation data from the flame meshes' vertex colors (R = height along the tongue, G = phase of the tongue, B = tongue height in meters). Use the flame materials only on the `_Flame` meshes of this pack: on a mesh without that data the whole object sways. To preview the motion in Edit Mode, turn on *Always Refresh* in the Scene view's effects menu.

### Rows of the same fire prop: controllers A, B and C

Each fire prop has 3 light controllers in `Animations/`: `AC_<Name>_FireLight` (A, used by the prefab), `AC_<Name>_FireLight_B` (shifted by one third of the loop) and `AC_<Name>_FireLight_C` (shifted by two thirds). When you place several copies of the same prefab close together, select the `FireLight` of each copy and set its Animator's *Controller* to A, B, C, A... in turn. This is an instance override; the prefab is not changed. The intensity curves of B and C have a correlation of -0.5 with A, so neighbouring lights do not brighten together. The demo camp does this for its repeated fires.

### Static batching

Do not mark `SM_<Name>_Flame` as *Batching Static*. With static batching every flame in the batch uses the batch's origin, so all copies sway in step again. They still move, but they lose their individual phase. The rest of the prefab can be static, and the light's Animator is not affected.

The light range and base intensity were set per prefab from the size of the flame; adjust them to your scene's exposure. If you change the base intensity, the clip still animates the original values, so edit the clip's curve or remove the Animator.

## Modular walls

Stake Fence, Small Palisade Section and Reinforced Palisade Section are modular: each section is exactly 2.000 m wide (X from -1.000 to +1.000) with the pivot on the wall line. Place sections 2.000 m apart along their local X axis, with the same rotation, and they meet edge to edge.

| Section | Height (m) | Depth (m) | Front (+Z) of the wall line (m) | Back (−Z) of the wall line (m) |
| --- | --- | --- | --- | --- |
| Stake Fence | 1.58 | 0.24 | 0.14 | 0.10 |
| Small Palisade Section | 2.40 | 0.57 | 0.26 | 0.30 |
| Reinforced Palisade Section | 3.00 | 1.10 | 0.27 | 0.83 |

## Colliders

Every prefab has BoxColliders. Most use one box that matches the body (the flame is never included). These props use shaped colliders instead:

- **Broken Crate** (stepped, 3 boxes): boxes follow the height profile of the smashed crate, so there is no invisible block over its low side.
- **Supply Cart** (stepped, 7 boxes): boxes follow the height profile of the cart along its length.
- **Wall Weapon Rack** (box, panel only): one box around the board panel only; the hooks, pegs and skull stick out of it.
- **Prisoner Cage** (composite, 9 boxes incl. the open door): the door is modeled open; the boxes cover the base, roof, bars and door leaf and keep the interior and the doorway free, so a character can stand inside.
- **Butcher Table** (box, without the puddle): one box around the table; the blood puddle on the ground is left out.
- **Broken Wood Debris** (9 boxes, one per plank): one rotated box per plank; small splinters have no collider.

Turn a collider into a trigger, replace it or remove it as your game needs.

## Asset list

Size is the prop's bounding box in meters: X (width) × Y (height) × Z (depth), flame excluded. Tris include the flame where there is one.

| Prop | Category | Prefab | Tris | Size X × Y × Z (m) | Collider | Notes |
| --- | --- | --- | --- | --- | --- | --- |
| Wooden Barricade | Fortifications | `PF_WoodenBarricade.prefab` | 464 | 2.00 × 1.25 × 0.88 | Box | — |
| Spiked Barricade | Fortifications | `PF_SpikedBarricade.prefab` | 890 | 2.24 × 1.04 × 1.22 | Box | — |
| Scrap Barricade | Fortifications | `PF_ScrapBarricade.prefab` | 980 | 1.97 × 1.40 × 0.81 | Box | — |
| Stake Fence | Fortifications | `PF_StakeFence.prefab` | 644 | 2.00 × 1.58 × 0.24 | Box | pivot: on the wall line (ground level, centered in X); modular, snaps every 2.000 m |
| Defensive Stakes | Fortifications | `PF_DefensiveStakes.prefab` | 654 | 1.63 × 1.02 × 1.10 | Box | — |
| Small Palisade Section | Fortifications | `PF_SmallPalisadeSection.prefab` | 810 | 2.00 × 2.40 × 0.57 | Box | pivot: on the wall line (ground level, centered in X); modular, snaps every 2.000 m |
| Reinforced Palisade Section | Fortifications | `PF_ReinforcedPalisadeSection.prefab` | 1,094 | 2.00 × 3.00 × 1.10 | Box | pivot: on the wall line (ground level, centered in X); modular, snaps every 2.000 m |
| Spiked Road Block | Fortifications | `PF_SpikedRoadBlock.prefab` | 846 | 2.02 × 1.05 × 0.88 | Box | — |
| Small Enemy Tent | Camp Structures | `PF_SmallEnemyTent.prefab` | 930 | 1.80 × 1.50 × 2.36 | Box | — |
| Large Enemy Tent | Camp Structures | `PF_LargeEnemyTent.prefab` | 2,318 | 3.98 × 3.78 × 3.59 | Box | — |
| Bedroll | Camp Structures | `PF_Bedroll.prefab` | 536 | 0.80 × 0.23 × 1.90 | Box | — |
| Campfire | Camp Structures | `PF_Campfire.prefab` | 804 | 1.09 × 0.38 × 1.08 | Box | flame child (144 tris, included in the count) + FireLight |
| Cooking Tripod | Camp Structures | `PF_CookingTripod.prefab` | 498 | 1.30 × 1.80 × 1.30 | Box | the Cooking Pot hangs from its hook over the Campfire (see the demo camp) |
| Cooking Pot | Camp Structures | `PF_CookingPot.prefab` | 466 | 0.58 × 0.62 × 0.53 | Box | — |
| Rough Stool | Camp Structures | `PF_RoughStool.prefab` | 229 | 0.42 × 0.46 × 0.42 | Box | — |
| Rough Camp Table | Camp Structures | `PF_RoughCampTable.prefab` | 504 | 1.85 × 0.82 × 0.91 | Box | — |
| Wooden Crate | Storage & Supplies | `PF_WoodenCrate.prefab` | 454 | 0.69 × 0.69 × 0.69 | Box | — |
| Reinforced Crate | Storage & Supplies | `PF_ReinforcedCrate.prefab` | 746 | 0.95 × 0.58 × 0.64 | Box | — |
| Broken Crate | Storage & Supplies | `PF_BrokenCrate.prefab` | 608 | 1.23 × 0.55 × 0.84 | Stepped, 3 boxes | shaped collider, see Colliders |
| Wooden Barrel | Storage & Supplies | `PF_WoodenBarrel.prefab` | 639 | 0.62 × 0.89 × 0.59 | Box | — |
| Broken Barrel | Storage & Supplies | `PF_BrokenBarrel.prefab` | 702 | 0.97 × 0.65 × 0.82 | Box | — |
| Supply Sack | Storage & Supplies | `PF_SupplySack.prefab` | 406 | 0.48 × 0.72 × 0.41 | Box | — |
| Woven Basket | Storage & Supplies | `PF_WovenBasket.prefab` | 688 | 0.53 × 0.47 × 0.51 | Box | — |
| Supply Cart | Storage & Supplies | `PF_SupplyCart.prefab` | 2,118 | 1.59 × 1.20 × 2.88 | Stepped, 7 boxes | shaped collider, see Colliders |
| Weapon Rack | Enemy Utility | `PF_WeaponRack.prefab` | 604 | 1.60 × 1.50 × 0.70 | Box | made for pole weapons at least 1.25 m long |
| Wall Weapon Rack | Enemy Utility | `PF_WallWeaponRack.prefab` | 412 | 1.40 × 0.69 × 0.24 | Box, panel only | pivot: center of the back face (mounts on a wall); shaped collider, see Colliders |
| Grinding Stone | Enemy Utility | `PF_GrindingStone.prefab` | 660 | 1.05 × 0.98 × 0.61 | Box | — |
| Chopping Block | Enemy Utility | `PF_ChoppingBlock.prefab` | 300 | 0.82 × 0.86 × 0.62 | Box | — |
| Training Dummy | Enemy Utility | `PF_TrainingDummy.prefab` | 898 | 0.99 × 1.81 × 0.61 | Box | — |
| Small Cage | Enemy Utility | `PF_SmallCage.prefab` | 1,096 | 0.96 × 0.95 × 0.96 | Box | — |
| Prisoner Cage | Enemy Utility | `PF_PrisonerCage.prefab` | 1,754 | 1.66 × 2.20 × 1.89 | Composite, 9 boxes incl. the open door | shaped collider, see Colliders |
| Butcher Table | Enemy Utility | `PF_ButcherTable.prefab` | 978 | 1.46 × 1.49 × 0.78 | Box, without the puddle | shaped collider, see Colliders |
| Torch Stand | Lighting & Identity | `PF_TorchStand.prefab` | 724 | 0.50 × 2.00 × 0.45 | Box | flame child (144 tris, included in the count) + FireLight |
| Wall Torch | Lighting & Identity | `PF_WallTorch.prefab` | 382 | 0.15 × 0.51 × 0.31 | Box | pivot: center of the back face (mounts on a wall); flame child (144 tris, included in the count) + FireLight |
| Brazier | Lighting & Identity | `PF_Brazier.prefab` | 714 | 0.72 × 0.97 × 0.72 | Box | flame child (182 tris, included in the count) + FireLight |
| Hanging Lantern | Lighting & Identity | `PF_HangingLantern.prefab` | 492 | 0.24 × 0.49 × 0.21 | Box | pivot: top center (hanging point); flame child (76 tris, included in the count) + FireLight |
| Enemy Banner | Lighting & Identity | `PF_EnemyBanner.prefab` | 900 | 1.11 × 2.97 × 0.26 | Box | — |
| Warning Sign | Lighting & Identity | `PF_WarningSign.prefab` | 603 | 0.81 × 1.77 × 0.21 | Box | — |
| Bone Pile | Bone & Ritual | `PF_BonePile.prefab` | 1,160 | 1.02 × 0.38 × 0.82 | Box | — |
| Skull Pile | Bone & Ritual | `PF_SkullPile.prefab` | 1,452 | 0.75 × 0.59 × 0.69 | Box | — |
| Bone Totem | Bone & Ritual | `PF_BoneTotem.prefab` | 1,062 | 0.82 × 2.38 × 0.55 | Box | — |
| Skull Totem | Bone & Ritual | `PF_SkullTotem.prefab` | 1,254 | 1.06 × 2.56 × 0.47 | Box | — |
| Ritual Altar | Bone & Ritual | `PF_RitualAltar.prefab` | 1,192 | 1.52 × 0.96 × 0.90 | Box | flame child (240 tris, included in the count) + FireLight |
| Monster Nest | Bone & Ritual | `PF_MonsterNest.prefab` | 1,648 | 1.75 × 0.53 × 1.71 | Box | — |
| Firewood Pile | Clutter & Debris | `PF_FirewoodPile.prefab` | 540 | 1.32 × 0.54 × 0.70 | Box | — |
| Scrap Metal Pile | Clutter & Debris | `PF_ScrapMetalPile.prefab` | 1,154 | 1.15 × 0.44 × 1.03 | Box | — |
| Rope Coil | Clutter & Debris | `PF_RopeCoil.prefab` | 656 | 0.64 × 0.11 × 0.59 | Box | — |
| Broken Cart Wheel | Clutter & Debris | `PF_BrokenCartWheel.prefab` | 576 | 0.98 × 0.19 × 1.02 | Box | — |
| Supply Pile | Clutter & Debris | `PF_SupplyPile.prefab` | 1,660 | 1.18 × 0.71 × 0.97 | Box | — |
| Broken Wood Debris | Clutter & Debris | `PF_BrokenWoodDebris.prefab` | 522 | 1.52 × 0.22 × 1.22 | 9 boxes, one per plank | shaped collider, see Colliders |

Total: 42,421 triangles for the 50 prefabs (229 to 2,318 per prop, 848 on average).

## Materials and textures

All props share one library of 23 materials (`MAT_EEP_<Material>`, metallic workflow: 21 on URP Lit, the two flame materials on `EEP/FlameWave`). Each material has tileable 1024x1024 PNG textures:

| Map | Import | Use |
| --- | --- | --- |
| `_BC` | sRGB | Base Color (`Base Map`) |
| `_URPMask` | linear | packed mask in the `Metallic Map` slot: R = metallic, G = ambient occlusion (flat 1.0), A = smoothness |
| `_E` | sRGB | Emission map, only on `MAT_EEP_Ember`, `MAT_EEP_Flame` and `MAT_EEP_FlameCore` (HDR emission) |

**Mask packing.** Roughness, metallic and ambient occlusion were packed by script into the single `_URPMask` texture, which is how URP Lit reads them from its `Metallic Map` slot (with *Smoothness Source* set to *Metallic Alpha*):

- **R = metallic:** 0 on non-metal materials and 0.8 on iron. Rust mixes iron and rust, so its value varies across the texture.
- **G = ambient occlusion:** flat 1.0, because tileable textures carry no baked occlusion.
- **B:** unused.
- **A = smoothness** (1 − roughness).

There are no normal maps: the faceted look comes from the flat-shaded geometry.

**UV mapping.** The meshes have one UV channel in world scale: each material repeats at a fixed size in meters (table below), so a material has the same texel density on every prop and props of any size can share it. UVs overlap and extend outside the 0–1 range on purpose (all textures tile and use Repeat wrap mode). This means the textures cannot be painted per prop, and the UVs cannot be used for lightmaps (see the lighting note).

| Material | Repeats every (m) | Metallic (avg.) | Smoothness (avg.) | Used by (prefabs) |
| --- | --- | --- | --- | --- |
| `MAT_EEP_Ash` | 0.80 | 0.00 | 0.08 | 1 |
| `MAT_EEP_Bark` | 1.00 | 0.00 | 0.08 | 26 |
| `MAT_EEP_Blood` | 0.80 | 0.00 | 0.37 | 6 |
| `MAT_EEP_Bone` | 0.80 | 0.00 | 0.23 | 13 |
| `MAT_EEP_Canvas` | 1.50 | 0.00 | 0.10 | 9 |
| `MAT_EEP_ClothRed` | 1.20 | 0.00 | 0.09 | 3 |
| `MAT_EEP_Ember` | 0.50 | 0.00 | 0.11 | 4 |
| `MAT_EEP_Flame` | 1.00 | 0.00 | 0.09 | 6 |
| `MAT_EEP_FlameCore` | 1.00 | 0.00 | 0.09 | 6 |
| `MAT_EEP_Fur` | 0.80 | 0.00 | 0.10 | 2 |
| `MAT_EEP_Horn` | 0.80 | 0.00 | 0.25 | 10 |
| `MAT_EEP_Iron` | 0.75 | 0.80 | 0.27 | 28 |
| `MAT_EEP_Leather` | 0.60 | 0.00 | 0.10 | 7 |
| `MAT_EEP_Meat` | 0.40 | 0.00 | 0.45 | 2 |
| `MAT_EEP_Rope` | 0.30 | 0.00 | 0.08 | 23 |
| `MAT_EEP_Rust` | 0.75 | 0.49 | 0.23 | 7 |
| `MAT_EEP_Stone` | 1.00 | 0.00 | 0.12 | 4 |
| `MAT_EEP_Straw` | 0.60 | 0.00 | 0.12 | 2 |
| `MAT_EEP_Wicker` | 0.40 | 0.00 | 0.13 | 1 |
| `MAT_EEP_Wood` | 1.00 | 0.00 | 0.15 | 25 |
| `MAT_EEP_WoodCharred` | 1.00 | 0.00 | 0.09 | 2 |
| `MAT_EEP_WoodCut` | 0.50 | 0.00 | 0.14 | 38 |
| `MAT_EEP_WoodDark` | 1.00 | 0.00 | 0.12 | 13 |

## Technical summary

- Prefabs: 50 (50 FBX meshes), no variants
- Triangles: 229 to 2,318 per prop, 42,421 in total
- Materials: 23 shared (21 URP Lit, 2 on `EEP/FlameWave`) · Textures: 49 PNG, 1024x1024
- UVs: 1 channel, world-scale tiling (overlapping by design) · Normal maps: no · LODs: no
- Rig: no · Character animation: no · Animated content: the flames (vertex shader `EEP/FlameWave`) and the fire lights (18 AnimationClips, 18 Animator Controllers) · C# scripts: none
- Closed from below: 50/50 meshes
- Validation: all 50/50 prefabs passed the production checks in Unity (materials, shader, triangle budget, size against the source model, orientation, pivot, collider, fire light and its animation, flame vertex data)

## Support

Sug's LowPoly · gulopes.augusto@gmail.com · https://suglowpoly.com

Questions or problems: write to the address above, or use the publisher contact on the Asset Store page of this package.

Companion pack: **Low Poly Enemy Melee Weapons**, the weapons of the enemies who live in this camp.
