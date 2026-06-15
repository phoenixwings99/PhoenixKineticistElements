using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Class.Kineticist;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using PhoenixKineticistElements.ElementLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PhoenixKineticistElements
{
    public static class ElementFactories
    {

        static BlueprintFeature kinBlade = BlueprintTool.Get<BlueprintFeature>("9ff81732daddb174aa8138ad1297c787");
        static BlueprintBuff kinBladeBuff = BlueprintTool.Get<BlueprintBuff>("426a9c079ee7ac34aa8e0054f2218074");
        static Sprite KinBladeInventoryIcon = BlueprintTool.Get<BlueprintItemWeapon>("43ff67143efb86d4f894b10577329050").Icon;


        public static void ConfigureXBlast(BlastElement element)
        {

            var genericBlastFeature = Main.LocalPKEModContext.Blueprints.GetDerivedMaster("XBlastFeature");
            var genericBlastBaseAbility = Main.LocalPKEModContext.Blueprints.GetDerivedMaster("XBlastBase");
            var genericBlast = Main.LocalPKEModContext.Blueprints.GetDerivedMaster("XBlastAbility");
            var ERBlast = Main.LocalPKEModContext.Blueprints.GetDerivedMaster("ExtendedRangeXBlastAbility");
            var bladerootfeature = Main.LocalPKEModContext.Blueprints.GetDerivedMaster("XKineticBladeFeature");
            var kbxblastability = Main.LocalPKEModContext.Blueprints.GetDerivedMaster("KineticBladeXBlastAbility");
            var kbxblastbuff = Main.LocalPKEModContext.Blueprints.GetDerivedMaster("KineticBladeXBlastBuff");
            var kbxblastweapon = Main.LocalPKEModContext.Blueprints.GetDerivedMaster("XKineticBladeWeapon");
            var kbxblastburnability = Main.LocalPKEModContext.Blueprints.GetDerivedMaster("KineticBladeXBlastBurnAbility");
            var kbxblastdamage = Main.LocalPKEModContext.Blueprints.GetDerivedMaster("XBlastBladeDamage");
            var kbxenchantment = Main.LocalPKEModContext.Blueprints.GetDerivedMaster("XKineticBladeEnchantment");


            FeatureConfigurator.New($"{element.Name}BlastFeature", element.FeatureGuid)
                .SetDisplayName($"{element.Name}BlastAbility.Name")
                .SetDescription($"{element.Name}BlastAbility.Desc")
                .Configure();
            AbilityConfigurator.New($"{element.Name}BlastBase", Main.LocalPKEModContext.Blueprints.GetDerivedGUID($"{element.Name}BlastBase", genericBlastBaseAbility, Main.LocalPKEModContext.Blueprints.GetGUID($"{element.Name}BlastFeature")).m_Guid.ToString())
                .SetDisplayName($"{element.Name}BlastAbility.Name")
                .SetDescription($"{element.Name}BlastAbility.Desc")
                .Configure();
            AbilityConfigurator.New($"{element.Name}BlastAbility", Main.LocalPKEModContext.Blueprints.GetDerivedGUID($"{element.Name}BlastAbility", genericBlast, Main.LocalPKEModContext.Blueprints.GetGUID($"{element.Name}BlastFeature")).m_Guid.ToString())
                .SetDisplayName($"{element.Name}BlastAbility.Name")
                .SetDescription($"{element.Name}BlastAbility.Desc")
                .Configure();
            AbilityConfigurator.New($"ExtendedRange{element.Name}BlastAbility", Main.LocalPKEModContext.Blueprints.GetDerivedGUID($"ExtendedRange{element.Name}BlastAbility", ERBlast, Main.LocalPKEModContext.Blueprints.GetGUID($"{element.Name}BlastFeature")).m_Guid.ToString())
                .SetDisplayName(BlueprintTool.Get<BlueprintFeature>("cb2d9e6355dd33940b2bef49e544b0bf").m_DisplayName)
                .SetDescription(BlueprintTool.Get<BlueprintFeature>("cb2d9e6355dd33940b2bef49e544b0bf").m_Description)
                .Configure();
            FeatureConfigurator.New($"{element.Name}KineticBladeFeature", Main.LocalPKEModContext.Blueprints.GetDerivedGUID($"{element.Name}KineticBladeFeature", bladerootfeature, Main.LocalPKEModContext.Blueprints.GetGUID($"{element.Name}BlastFeature")).m_Guid.ToString())
                .SetHideInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .Configure();
            ActivatableAbilityConfigurator.New($"KineticBlade{element.Name}BlastAbility", Main.LocalPKEModContext.Blueprints.GetDerivedGUID($"KineticBlade{element.Name}BlastAbility", kbxblastability, Main.LocalPKEModContext.Blueprints.GetGUID($"{element.Name}BlastFeature")).m_Guid.ToString())
                .SetDisplayName($"{element.Name}KineticBlade.Name")
                .SetDescription(kinBlade.m_Description)
                .Configure();
            BuffConfigurator.New($"KineticBlade{element.Name}BlastBuff", Main.LocalPKEModContext.Blueprints.GetDerivedGUID($"KineticBlade{element.Name}BlastBuff", kbxblastbuff, Main.LocalPKEModContext.Blueprints.GetGUID($"{element.Name}BlastFeature")).m_Guid.ToString())
               .SetFlags(BlueprintBuff.Flags.HiddenInUi, BlueprintBuff.Flags.StayOnDeath)
               .Configure();
            ItemWeaponConfigurator.New($"{element.Name}KineticBladeWeapon", Main.LocalPKEModContext.Blueprints.GetDerivedGUID($"{element.Name}KineticBladeWeapon", kbxblastweapon, Main.LocalPKEModContext.Blueprints.GetGUID($"{element.Name}BlastFeature")).m_Guid.ToString())
                .Configure();

            AbilityConfigurator.New($"KineticBlade{element.Name}BlastBurnAbility", Main.LocalPKEModContext.Blueprints.GetDerivedGUID($"KineticBlade{element.Name}BlastBurnAbility", kbxblastburnability, Main.LocalPKEModContext.Blueprints.GetGUID($"{element.Name}BlastFeature")).m_Guid.ToString())
                .Configure();

            AbilityConfigurator.New($"{element.Name}BlastBladeDamage", Main.LocalPKEModContext.Blueprints.GetDerivedGUID($"{element.Name}BlastBladeDamage", kbxblastdamage, Main.LocalPKEModContext.Blueprints.GetGUID($"{element.Name}BlastFeature")).m_Guid.ToString())
                .Configure();

            WeaponEnchantmentConfigurator.New($"{element.Name}KineticBladeEnchantment", Main.LocalPKEModContext.Blueprints.GetDerivedGUID($"{element.Name}KineticBladeEnchantment", kbxenchantment, Main.LocalPKEModContext.Blueprints.GetGUID($"{element.Name}BlastFeature")).m_Guid.ToString())
                .SetEnchantName($"{element.Name}KineticBlade.Name")
                .SetPrefix($"{element.Name}Blast.Name")
                .Configure();

            //TODO INSERT SETTINGS HERE



            ElementFactories.CreateXBlastFeature(element);

            ElementFactories.CreateXBlastBaseAbility(element);

            ElementFactories.CreateXBlastVariant_Base(element);

            ElementFactories.CreateXBlastVariant_ExtendedRange(element);

            ElementFactories.CreateXKineticBladeFeature(element);

            ElementFactories.CreateKineticBladeXBlastAbility(element);

            ElementFactories.CreateKineticBladeXBuffAbility(element);

            ElementFactories.MakeXKineticBladeWeapon(element);

            ElementFactories.MakeKineticBladeXBlastBurnAbility(element);

            ElementFactories.CreateXBlastBladeDamage(element);

            ElementFactories.XKineticBladeEnchantment(element);

            //KineticWhirlwind
            AbilityConfigurator.For("80f10dc9181a0f64f97a9f7ac9f47d65").EditComponent<AbilityCasterHasFacts>(x =>
            {
                x.m_Facts = x.m_Facts.AddItem(BlueprintTool.GetRef<BlueprintUnitFactReference>($"KineticBlade{element.Name}BlastBuff")).ToArray();

            }).Configure();


            


            static void AddToUniversalBlastBuffOptions(string buffId, BlastElement element)
            {
                BuffConfigurator.For(buffId).EditComponent<AddKineticistBurnModifier>(x =>
                {
                    x.m_AppliableTo = x.m_AppliableTo.AddItem(BlueprintTool.GetRef<BlueprintAbilityReference>($"{element.Name}BlastBase")).ToArray();

                }).Configure();
            }
            AddToUniversalBlastBuffOptions("a4018afcb4a84c0aad23f16448cdbbe1", element);//Armor Piercing Blast
            AddToUniversalBlastBuffOptions("f583e43e4c904c1e854c479a280ab657", element);//Chain Arrows
            AddToUniversalBlastBuffOptions("132f24437854435eb903f11d47062b1a", element);//Cluster Arrows


        }

        public static void ConfigureXBlastLate(BlastElement element)
        {
            if (Main.IsDarkCodexInstalled())
            {

            }
        }


        public static void CreateKineticBladeXBlastAbility(BlastElement element)
        {
            ActivatableAbilityConfigurator.For($"KineticBlade{element.Name}BlastAbility")
                .AddRestrictionCanUseKineticBlade()
                .SetDescription(kinBlade.m_Description)
                .SetGroup(Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilityGroup.FormInfusion)
                .SetIcon(element.DefaultIconMelee)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfOwnerUnconscious(true)
                .SetActivationType(Kingmaker.UnitLogic.ActivatableAbilities.AbilityActivationType.Immediately)
                .SetActivateWithUnitCommand(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetActivateOnUnitAction(Kingmaker.UnitLogic.ActivatableAbilities.AbilityActivateOnUnitActionType.Attack)
                .SetBuff($"KineticBlade{element.Name}BlastBuff")
                .Configure();
        }

        public static void CreateKineticBladeXBuffAbility(BlastElement element)
        {
            BuffConfigurator.For($"KineticBlade{element.Name}BlastBuff")
                 .AddSpellDescriptorComponent(element.spellDescriptors)
                .AddKineticistBlade(blade: $"{element.Name}KineticBladeWeapon")
                .SetFlags(BlueprintBuff.Flags.HiddenInUi, BlueprintBuff.Flags.StayOnDeath)
                .SetIsClassFeature(true)
                .Configure();
        }

        public static void MakeXKineticBladeWeapon(BlastElement element)
        {
            ItemWeaponConfigurator.For($"{element.Name}KineticBladeWeapon")
                .AddWeaponKineticBlade(activationAbility: $"KineticBlade{element.Name}BlastBurnAbility", blast: $"{element.Name}BlastBladeDamage")
                .SetIcon(KinBladeInventoryIcon)
                .SetForceStackable(false)
                .SetDestructible(false)
                .SetVisualParameters(new WeaponVisualParameters()
                {
                    m_WeaponAnimationStyle = Kingmaker.View.Animation.WeaponAnimationStyle.SlashingOneHanded,
                    m_WeaponModel = new Kingmaker.ResourceLinks.PrefabLink() { AssetId = "7c05296dbc70bf6479e66df7d9719d1e" },
                    m_WeaponSheathModelOverride = new Kingmaker.ResourceLinks.PrefabLink() { AssetId = "f777a23c850d099428c33807f83cd3d6" }


                })
                .SetType("b05a206f6c1133a469b2f7e30dc970ef")
                .SetEnchantments($"{element.Name}KineticBladeEnchantment")
                .SetOverrideDamageDice(true)
                .Configure();

        }

        public static void MakeKineticBladeXBlastBurnAbility(BlastElement element)
        {
            AbilityConfigurator.For($"KineticBlade{element.Name}BlastBurnAbility")
                .AddAbilityKineticist(amount: 1, infusionBurnCost: 1, blastBurnCost: element.Burn)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(buff: kinBladeBuff, durationValue: new()
                {
                    Rate = DurationRate.Rounds,
                    BonusValue = new()
                    {
                        Value = 2
                    }
                }))
                .AddAbilityKineticBlade()
                 .AddSpellDescriptorComponent(element.spellDescriptors)
                .SetType(AbilityType.Special)
                .SetRange(AbilityRange.Personal)
                .SetCanTargetSelf(true)
                .SetShouldTurnToTarget(true)
                .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetHidden(true)
                .SetAvailableMetamagic(Metamagic.Extend, Metamagic.Heighten)
                .Configure();

        }

        public static void CreateXBlastBladeDamage(BlastElement element)
        {
            ContextDiceValue damageDiceValue = element.Composite ? (element.Physical ? PhysicalCompositeBlastDice() : EnergyCompositeBlastDice()) : element.Physical ? PhysicalSimpleBlastDice() : EnergySimpleBlastDice();

            AbilityConfigurator.For($"{element.Name}BlastBladeDamage")
                 .AddSpellDescriptorComponent(element.spellDescriptors)
                .SetDisplayName($"{element.Name}KineticBlade.Name")
                .AddAbilityEffectRunAction(BuildChainableDamageAction(element))
                .AddAbilityShowIfCasterHasFact(unitFact: "4d39ccef7b5b2e9458e8599eae3c3be0", not: false)
                .AddAbilityDeliveredByWeapon()
                .AddContextCalculateSharedValue(modifier: 1.0, value: new ContextDiceValue()
                {
                    DiceType = Kingmaker.RuleSystem.DiceType.One,
                    DiceCountValue = new()
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = Kingmaker.Enums.AbilityRankType.DamageDice,
                        ValueShared = AbilitySharedValue.Damage,
                    },
                    BonusValue = new()
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = Kingmaker.Enums.AbilityRankType.DamageBonus,
                        ValueShared = AbilitySharedValue.Damage
                    },
                }, valueType: AbilitySharedValue.Damage)
                .AddContextRankConfig(KinStatProperty(!element.Physical))
                .AddAbilityKineticist(amount: 1, blastBurnCost: element.Burn,
                    cachedDamageInfo: GetTeasers(element).ToList())
                .AddContextRankConfig(KineticBlastFeatureCountConfig())
                .AddContextCalculateAbilityParamsBasedOnClass(characterClass: "42a455d9ec1ad924d889272429eb8391", statType: Kingmaker.EntitySystem.Stats.StatType.Dexterity)
                .SetParent($"{element.Name}BlastBase")
                .AddKBRicochet(element.ProjectileGuid)
                .AddAbilityDeliverProjectile(projectiles: [element.ProjectileGuid],
                    needAttackRoll: true,
                    weapon: element.Physical ? "65951e1195848844b8ab8f46d942f6e8" : "4d3265a5b9302ee4cab9c07adddb253f")
                .SetType(AbilityType.Special)
                .SetCanTargetEnemies(true)
                .SetShouldTurnToTarget(true)
                .SetSpellResistance(!element.Physical)
                .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Kineticist)
                .SetTargetMapObjects(true)
                .AddToAvailableMetamagic(Metamagic.Empower, Metamagic.Maximize, Metamagic.Quicken, Metamagic.Heighten, Metamagic.Reach)
                .Configure();

        }

        public static void XKineticBladeEnchantment(BlastElement element)
        {
            WeaponEnchantmentConfigurator.For($"{element.Name}KineticBladeEnchantment")
                .AddContextCalculateSharedValue(modifier: 1.0, value: new ContextDiceValue()
                {
                    DiceType = Kingmaker.RuleSystem.DiceType.Zero,
                    DiceCountValue = new ContextValue()
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0,
                        ValueRank = AbilityRankType.DamageDice,
                        ValueShared = AbilitySharedValue.Damage

                    },
                    BonusValue = new()
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice,
                        ValueShared = AbilitySharedValue.Damage
                    }
                })
                .AddContextRankConfig(KineticBlastFeatureCountConfig())
                .AddContextRankConfig(KinStatProperty(!element.Physical))
                .SetWeaponFxPrefab(element.weaponfx)
                .Configure();


        }

        public static void CreateXKineticBladeFeature(BlastElement element)
        {
            FeatureConfigurator.For($"{element.Name}KineticBladeFeature")
                .AddFeatureIfHasFact(checkedFact: $"KineticBlade{element.Name}BlastAbility", feature: $"KineticBlade{element.Name}BlastAbility", not: true)
                .AddFeatureIfHasFact(checkedFact: $"KineticBlade{element.Name}BlastBurnAbility", feature: $"KineticBlade{element.Name}BlastBurnAbility", not: true)
                .SetHideInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .SetIsClassFeature(true)
                .Configure();

            FeatureConfigurator.For("9ff81732daddb174aa8138ad1297c787").AddFeatureIfHasFact(checkedFact: $"{element.Name}BlastBase", feature: $"{element.Name}KineticBladeFeature").Configure();
        }


        public static void CreateXBlastFeature(BlastElement element)
        {
            FeatureConfigurator.For($"{element.Name}BlastFeature")
                .AddFacts(facts: ["cb30a291c75def84090430fbf2b5c05e"])//Composite blast buff
                .AddFeatureIfHasFact($"{element.Name}BlastBase", feature: $"{element.Name}BlastBase", not: true)
                .AddToGroups(FeatureGroup.KineticBlast)
                .SetIcon(element.DefaultIcon)
                .SetRanks(20)
                .SetIsClassFeature(true)
                .Configure();
        }

        public static void AddToSubstanceInfusion(BlastElement blastElement, string featureID, string buffID)
        {
            FeatureConfigurator.For(featureID).EditComponent<PrerequisiteFeaturesFromList>(x =>
            {
                x.m_Features = x.m_Features.AddItem(BlueprintTool.GetRef<BlueprintFeatureReference>($"{blastElement.Name}BlastFeature")).ToArray();
            }).Configure();

            BuffConfigurator.For(buffID).EditComponent<AddKineticistInfusionDamageTrigger>(x =>
            {
                x.m_AbilityList = x.m_AbilityList.AddItem(BlueprintTool.GetRef<BlueprintAbilityReference>($"{blastElement.Name}BlastBase")).ToArray();

            }).EditComponent<AddKineticistBurnModifier>(x =>
            {
                x.m_AppliableTo = x.m_AppliableTo.AddItem(BlueprintTool.GetRef<BlueprintAbilityReference>($"{blastElement.Name}BlastBase")).ToArray();

            }).Configure();
        }

        public static void CreateXBlastBaseAbility(BlastElement element)
        {
            AbilityConfigurator.For($"{element.Name}BlastBase")
                .AddAbilityVariants([$"{element.Name}BlastAbility", $"ExtendedRange{element.Name}BlastAbility"])
                .AddAbilityShowIfCasterHasFact(unitFact: "1f3a15a3ae8a5524ab8b97f469bf4e3d")//Focus Selection
                .AddAbilityKineticist(amount: 1)
                .AddSpellDescriptorComponent(element.spellDescriptors)
                .SetIcon(element.DefaultIcon)
                .SetType(AbilityType.Special)
                .SetRange(AbilityRange.Close)
                .SetCanTargetPoint(true)
                .SetCanTargetFriends(true)
                .SetCanTargetSelf(true)
                .SetCanTargetEnemies(true)
                .SetSpellResistance(true)
                .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetAvailableMetamagic(Metamagic.Empower, Metamagic.Maximize, Metamagic.Quicken, Metamagic.Extend, Metamagic.Heighten, Metamagic.Reach)
                .Configure();

        }



        public static void CreateXBlastVariant_Base(BlastElement element)
        {
            var blast = AbilityConfigurator.For($"{element.Name}BlastAbility")
                .SetRange(AbilityRange.Close)
                .SetParent($"{element.Name}BlastBase")
                .ApplyShotProperties(element, 0)
                .Configure();
        }

        public static void CreateXBlastVariant_ExtendedRange(BlastElement element)
        {
            var blast = AbilityConfigurator.For($"ExtendedRange{element.Name}BlastAbility")
                .AddAbilityShowIfCasterHasFact(unitFact: "cb2d9e6355dd33940b2bef49e544b0bf")
                .SetRange(AbilityRange.Long)
                .SetParent($"{element.Name}BlastBase")
                .ApplyShotProperties(element, 1)
                .Configure();
        }

        public static AbilityConfigurator ApplyShotProperties(this AbilityConfigurator abilityConfigurator, BlastElement element, int shotcost)
        {
            ContextDiceValue damageDiceValue = element.Composite ? (element.Physical ? PhysicalCompositeBlastDice() : EnergyCompositeBlastDice()) : element.Physical ? PhysicalSimpleBlastDice() : EnergySimpleBlastDice();


            return abilityConfigurator.SetIcon(element.DefaultIcon)
                 .AddSpellDescriptorComponent(element.spellDescriptors)
                .SetType(AbilityType.Special)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetAvailableMetamagic(availableMetamagic: new[] { Metamagic.Empower, Metamagic.Maximize, Metamagic.Quicken,
                    Metamagic.Heighten})
                .SetCanTargetEnemies(true)
                .SetIcon(element.DefaultIcon)
                .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Kineticist)
                 .AddContextRankConfig(KineticBlastFeatureCountConfig())
                .AddContextRankConfig(KinStatProperty(!element.Physical))
                .AddKBRicochet(element.ProjectileGuid)
                .AddAbilityDeliverProjectile(
                    projectiles: [element.ProjectileGuid],
                    needAttackRoll: true,
                    weapon: element.Physical ? "65951e1195848844b8ab8f46d942f6e8" : "4d3265a5b9302ee4cab9c07adddb253f")

                .AddAbilityKineticist(amount: 1,
                    blastBurnCost: element.Burn,
                    infusionBurnCost: shotcost,
                    cachedDamageInfo: GetTeasers(element).ToList()
                    )
                .AddAbilityEffectRunAction(BuildChainableDamageAction(element))
                .AddContextCalculateSharedValue(modifier: 1.0, value: new ContextDiceValue()
                {
                    DiceType = Kingmaker.RuleSystem.DiceType.One,
                    DiceCountValue = new()
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = Kingmaker.Enums.AbilityRankType.DamageDice,
                        ValueShared = AbilitySharedValue.Damage,
                    },
                    BonusValue = new()
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = Kingmaker.Enums.AbilityRankType.DamageBonus,
                        ValueShared = AbilitySharedValue.Damage
                    },
                }, valueType: AbilitySharedValue.Damage)

                .AddAbilitySpawnFx(anchor: Kingmaker.UnitLogic.Abilities.Components.Base.AbilitySpawnFxAnchor.Caster,
                    time: Kingmaker.UnitLogic.Abilities.Components.Base.AbilitySpawnFxTime.OnPrecastStart,
                    orientationMode: Kingmaker.UnitLogic.Abilities.Components.Base.AbilitySpawnFxOrientation.Copy,
                    prefabLink: "69a83b56c1265464f8626a2ab414364a"
                )
                .AddAbilitySpawnFx(anchor: Kingmaker.UnitLogic.Abilities.Components.Base.AbilitySpawnFxAnchor.Caster,
                    time: Kingmaker.UnitLogic.Abilities.Components.Base.AbilitySpawnFxTime.OnStart,
                    orientationMode: Kingmaker.UnitLogic.Abilities.Components.Base.AbilitySpawnFxOrientation.Copy,
                    prefabLink: "852b687aad7863e438c61339dd35d85d"
                )
                ;

        }

        public static ContextDiceValue PhysicalCompositeBlastDice()
        {
            return new ContextDiceValue()
            {
                DiceType = Kingmaker.RuleSystem.DiceType.D6,
                DiceCountValue = new ContextValue()
                {
                    ValueType = ContextValueType.Rank,
                    ValueRank = Kingmaker.Enums.AbilityRankType.DamageDice
                },
                BonusValue = new ContextValue()
                {
                    ValueType = ContextValueType.Shared,
                    ValueRank = AbilityRankType.DamageBonus,
                    ValueShared = AbilitySharedValue.Damage
                }
            };
        }

        public static ContextDiceValue PhysicalSimpleBlastDice()
        {
            return new ContextDiceValue()
            {
                DiceType = Kingmaker.RuleSystem.DiceType.D6,
                DiceCountValue = new ContextValue()
                {
                    ValueType = ContextValueType.Rank,
                    ValueRank = Kingmaker.Enums.AbilityRankType.DamageDice
                },
                BonusValue = new ContextValue()
                {
                    ValueType = ContextValueType.Shared,
                    ValueRank = AbilityRankType.Default,
                    ValueShared = AbilitySharedValue.Damage
                }
            };
        }

        public static ContextDiceValue EnergySimpleBlastDice()
        {
            return new ContextDiceValue()
            {
                DiceType = Kingmaker.RuleSystem.DiceType.D6,
                DiceCountValue = new ContextValue()
                {
                    ValueType = ContextValueType.Rank,
                    ValueRank = Kingmaker.Enums.AbilityRankType.DamageDice
                },
                BonusValue = new ContextValue()
                {
                    ValueType = ContextValueType.Rank,
                    ValueRank = AbilityRankType.DamageBonus,
                    ValueShared = AbilitySharedValue.DamageBonus
                }
            };
        }

        public static ContextDiceValue EnergyCompositeBlastDice()
        {
            return new ContextDiceValue()
            {
                DiceType = Kingmaker.RuleSystem.DiceType.D6,
                DiceCountValue = new ContextValue()
                {
                    ValueType = ContextValueType.Rank,
                    ValueRank = Kingmaker.Enums.AbilityRankType.DamageDice
                },
                BonusValue = new ContextValue()
                {
                    ValueType = ContextValueType.Rank,
                    ValueRank = AbilityRankType.DamageBonus,
                    ValueShared = AbilitySharedValue.Damage
                }
            };
        }

        public static ContextRankConfig KinStatProperty(bool div2)
        {
            return new Kingmaker.UnitLogic.Mechanics.Components.ContextRankConfig()
            {
                m_Type = Kingmaker.Enums.AbilityRankType.DamageBonus,
                m_BaseValueType = ContextRankBaseValueType.CustomProperty,
                m_Stat = Kingmaker.EntitySystem.Stats.StatType.Constitution,
                m_Max = 20,
                m_Progression = div2 ? ContextRankProgression.Div2 : ContextRankProgression.AsIs,
                m_CustomProperty = BlueprintTool.GetRef<BlueprintUnitPropertyReference>("f897845bbbc008d4f9c1c4a03e22357a")
            };
        }

        public static ContextRankConfig CompositeEnergyBlastDamageConfigDiceCount(BlastElement element)
        {
            return new ContextRankConfig()
            {
                m_Type = AbilityRankType.DamageDice,
                m_BaseValueType = ContextRankBaseValueType.FeatureListRanks,

                m_Feature = BlueprintTool.GetRef<BlueprintFeatureReference>("93efbde2764b5504e98e6824cab3d27c"),
                m_Progression = ContextRankProgression.MultiplyByModifier,
                m_BuffRankMultiplier = 1,
                m_StartLevel = 0,
                m_StepLevel = 2,

            };
        }








        public static ContextRankConfig KineticBlastFeatureCountConfig()
        {
            return (new Kingmaker.UnitLogic.Mechanics.Components.ContextRankConfig()
            {
                m_Type = Kingmaker.Enums.AbilityRankType.DamageDice,
                m_BaseValueType = ContextRankBaseValueType.FeatureRank,
                m_Feature = BlueprintTool.GetRef<BlueprintFeatureReference>("93efbde2764b5504e98e6824cab3d27c"),
                m_Max = 20
            });
        }





        public static ActionsBuilder BuildChainableDamageAction(BlastElement element)
        {
            ContextDiceValue damageDiceValue = element.Composite ? (element.Physical ? PhysicalCompositeBlastDice() : EnergyCompositeBlastDice()) : element.Physical ? PhysicalSimpleBlastDice() : EnergySimpleBlastDice();

            ActionsBuilder ifTrue = ActionsBuilder.New();
            if (element.damageType.Length == 2)
            {
                ifTrue.DealDamage(element.damageType[0]
                   , value: damageDiceValue
                   , useWeaponDamageModifiers: true,
                       half: true
                   )
                .DealDamage(element.damageType[1]
                , value: damageDiceValue
                , useWeaponDamageModifiers: true,
                    half: true
                );
            }
            else
            {
                ifTrue.DealDamage(element.damageType[0]
                   , value: damageDiceValue
                   , useWeaponDamageModifiers: true,
                       half: false
                   );
            }
            ActionsBuilder ifFalse = ActionsBuilder.New()
                .DealDamage(element.damageType[0]
                   , value: damageDiceValue
                   , useWeaponDamageModifiers: true,
                       half: true
                   );

            return ActionsBuilder.New().Conditional(ConditionsBuilder.New().AddIsEqual(new DeliverEffectLayer(), new IntConstant() { Value = 0 }), ifTrue: ifTrue, ifFalse: ifFalse);
        }

        public static DamageTypeDescription BPSDamage()
        {
            return new()
            {
                Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Physical,
                Physical = new()
                {
                    Form = Kingmaker.Enums.Damage.PhysicalDamageForm.Bludgeoning | Kingmaker.Enums.Damage.PhysicalDamageForm.Piercing |
                               Kingmaker.Enums.Damage.PhysicalDamageForm.Slashing,
                }
            };
        }

        #region damage helpers

        public static DamageTypeDescription ColdDamage()
        {
            return new()
            {
                Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                Energy = Kingmaker.Enums.Damage.DamageEnergyType.Cold
            };
        }

        internal static DamageTypeDescription LDamage()
        {
            return new()
            {
                Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                Energy = Kingmaker.Enums.Damage.DamageEnergyType.Electricity
            };
        }

        internal static DamageTypeDescription FDamage()
        {
            return new()
            {
                Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                Energy = Kingmaker.Enums.Damage.DamageEnergyType.Fire
            };
        }

        public static DamageTypeDescription BDamage()
        {
            return new()
            {
                Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Physical,
                Physical = new()
                {
                    Form = Kingmaker.Enums.Damage.PhysicalDamageForm.Bludgeoning,
                }
            };
        }

        public static DamageTypeDescription BSDamage()
        {
            return new()
            {
                Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Physical,
                Physical = new()
                {
                    Form = Kingmaker.Enums.Damage.PhysicalDamageForm.Bludgeoning |
                               Kingmaker.Enums.Damage.PhysicalDamageForm.Slashing,
                }
            };
        }

        #endregion

        private static AbilityKineticist.DamageInfo[] GetTeasers(BlastElement element)
        {
            if (element.damageType.Length == 2)
            {
                return new[] { new Kingmaker.UnitLogic.Class.Kineticist.AbilityKineticist.DamageInfo()
                {
                    Value = new()
                    {
                        DiceType = Kingmaker.RuleSystem.DiceType.D6,
                        DiceCountValue = new ContextValue()
                        {
                            ValueType = ContextValueType.Rank,
                            ValueRank = Kingmaker.Enums.AbilityRankType.DamageDice
                        },
                        BonusValue = new ContextValue()
                        {
                            ValueType = ContextValueType.Shared,
                        },



                    },
                    Half = true,
                    Type = element.damageType[0],
                    UseWeaponDamageModifiers = true

                },
                new Kingmaker.UnitLogic.Class.Kineticist.AbilityKineticist.DamageInfo()
                {
                    Value = new()
                    {
                        DiceType = Kingmaker.RuleSystem.DiceType.D6,
                        DiceCountValue = new ContextValue()
                        {
                            ValueType = ContextValueType.Rank,
                            ValueRank = Kingmaker.Enums.AbilityRankType.DamageDice
                        },
                        BonusValue = new ContextValue()
                        {
                            ValueType = ContextValueType.Shared,
                        },



                    },
                    Half = true,
                    Type = element.damageType[1],
                    UseWeaponDamageModifiers = true
                }
            };



            }
            else
            {
                return new[] {
                    new Kingmaker.UnitLogic.Class.Kineticist.AbilityKineticist.DamageInfo()
                    {
                        Value = new()
                        {
                            DiceType = Kingmaker.RuleSystem.DiceType.D6,
                            DiceCountValue = new ContextValue()
                            {
                                ValueType = ContextValueType.Rank,
                                ValueRank = Kingmaker.Enums.AbilityRankType.DamageDice
                            },
                            BonusValue = new ContextValue()
                            {
                                ValueType = ContextValueType.Shared,
                            },



                        },

                        Type = element.damageType[0],
                        UseWeaponDamageModifiers = true

                    }
                };
            }
        }





        public static FeatureConfigurator AddFireFocusPrerequisite(this FeatureConfigurator featureConfigurator)
        {
            return featureConfigurator.AddPrerequisiteFeaturesFromList(features: new() { "13bdf8d542811ac4ca228a53aa108145", "caa7edca64af1914d9e14785beb6a143", "56e2fc3abed8f2247a621ac37e75f303", "d4a2a75d01d1e77489ff692636a538bf" }, amount: 1);
        }

        public static FeatureConfigurator AddAirFocusPrerequisite(this FeatureConfigurator featureConfigurator)
        {
            return featureConfigurator.AddPrerequisiteFeaturesFromList(features: new() { "2bd0d44953a536f489082534c48f8e31", "93bd14dd916cfd1429c11ad66adf5e2b", "659c39542b728c04b83e969c834782a9", "651570c873e22b84f893f146ce2de502" }, amount: 1);
        }

        public static FeatureConfigurator AddWaterFocusPrerequisite(this FeatureConfigurator featureConfigurator)
        {
            return featureConfigurator.AddPrerequisiteFeaturesFromList(features: new() { "7ab8947ce2e19c44a9edcf5fd1466686", "5e839c743c6da6649a43cdeb70b6018f", "faa5f1233600d864fa998bc0afe351ab", "86eff374d040404438ad97fedd7218bc" }, amount: 1);
        }

        public static FeatureConfigurator AddEarthFocusPrerequisite(this FeatureConfigurator featureConfigurator)
        {
            return featureConfigurator.AddPrerequisiteFeaturesFromList(features: new() { "c6816ad80a3df9c4ea7d3b012b06bacd", "d2a93ab18fcff8c419b03a2c3d573606", "956b65effbf37e5419c13100ab4385a3", "c43d9c2d23e56fb428a4eb60da9ba1cb" }, amount: 1);
        }

        public static FeatureConfigurator AddLightFocusPrerequisite(this FeatureConfigurator featureConfigurator)
        {
            return featureConfigurator.AddPrerequisiteFeaturesFromList(features: new() { "ElementalFocusLight", "KineticKnightElementalFocusLight", "SecondaryElementLight", "ThirdElementLight" }, amount: 1);
        }

        private static void CreateDeeperUniversalBlasts(BlastElement element)
        {

            MakeSpindleXBlastAbility(element);
        }

        #region vanilla form infusion helpers

        public static void MakeSpindleXBlastAbility(BlastElement element)
        {
            ActionsBuilder damageAction = ActionsBuilder.New();
            if (element.damageType.Length == 1 && element.Physical)
            {
                damageAction.DealDamage(damageType: element.damageType[0], value: (element.Composite ? PhysicalCompositeBlastDice() : PhysicalSimpleBlastDice()), half: true);//IsALreadyHalved missing? //TODO
            }
            else if (element.damageType.Length == 2 && element.Physical)
            {

            }


            var spindle = BlueprintTool.Get<BlueprintAbility>("a28e54e4e5fafd1449dd9e926be85160");
            var guid = Main.LocalPKEModContext.Blueprints.GetDerivedGUID($"Spindle{element.Name}BlastAbility", spindle.AssetGuid, BlueprintTool.Get<BlueprintFeature>(element.FeatureGuid).AssetGuid);

            AbilityConfigurator.New($"Spindle{element.Name}BlastAbility", guid.m_Guid.ToString())
                .SetDisplayName(spindle.m_DisplayName)
                .SetDescription(spindle.m_Description)
            .Configure();
        }

        public static void MakeExplodingArrowsXBlastAbility(BlastElement element) { }

        public static void MakeXTorrentBlastAbility(BlastElement element)
        {

        }

        public static void MakeXCloud(BlastElement element)
        {


            static void MakeCloudXBlastAbility(BlastElement element)
            {

            }

            static void MakeCloudXCBlastArea(BlastElement element)
            {

            }
        }

        public static void CycloneXBlastAbility(BlastElement element)
        {

        }

        public static void MakeXDeadlyEarth(BlastElement element)
        {
            static void MakeDeadlyEarthXBlastAbility(BlastElement element)
            {

            }
            static void MakeDeadlyEarthXBlastArea(BlastElement element)
            {

            }
        }

        public static void MakeEruptionXBlastAbility(BlastElement element)
        {

        }

        public static void MakeFanOfFlamesXBlastAbility(BlastElement element)
        {

        }

        public static void MakeFragmentationXBlastAbility(BlastElement element)
        {

        }

        public static void MakeSprayXBlastAbility(BlastElement element)
        {

        }

        public static void MakeXWall(BlastElement element)
        {

            static void MakeWallXBlastAbility(BlastElement element) { }
            ;
            static void MakeWallXBlastArea(BlastElement element) { }
            ;

        }

        #endregion

        #region DarkCodex Form Helpers

        public static void MakeChainXBlastAbility(BlastElement element) { }
        public static void MakeImpaleXBlastAbility(BlastElement element) { }

        


        #endregion


    }

}