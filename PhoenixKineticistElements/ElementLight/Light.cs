using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI.MVVM._ConsoleView.InGame;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Class.Kineticist;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Owlcat.Runtime.UI.Utility;
using System;
using TabletopTweaks.Core.Config;
using TabletopTweaks.Core.Utilities;
using Unity.Mathematics;
using UnityEngine;
using static Kingmaker.Kingdom.Settlements.SettlementGridTopology;


namespace PhoenixKineticistElements.ElementLight
{
    internal static class Light
    {




        private static Sprite baseIcon;

        private static BlueprintFeature kinBlade;
        private static BlueprintBuff kinBladeBuff;
        private static Sprite KinBladeInventoryIcon;

        //Base elements
        public static BlastElement LightBlast;


        //Composite Elements
        public static BlastElement AuroraBlast;
        public static BlastElement BioluminescentBlast;
        public static BlastElement CrystalBlast;
        public static BlastElement GloriousBlast;
        public static BlastElement LightningBlast;
        public static BlastElement RainbowBlast;
        public static BlastElement SolarBlast;




        public static void Configure()
        {

            baseIcon = BlueprintTool.Get<BlueprintAbility>("bf0accce250381a44b857d4af6c8e10d").Icon;//Searing Light
            string holyeffect = BlueprintTool.Get<BlueprintWeaponEnchantment>("28a9964d81fedae44bae3ca45710c140").WeaponFxPrefab.AssetId;
            kinBlade = BlueprintTool.Get<BlueprintFeature>("9ff81732daddb174aa8138ad1297c787");
            kinBladeBuff = BlueprintTool.Get<BlueprintBuff>("426a9c079ee7ac34aa8e0054f2218074");
            KinBladeInventoryIcon = BlueprintTool.Get<BlueprintItemWeapon>("43ff67143efb86d4f894b10577329050").Icon;

            LightBlast = new()
            {
                Name = "Light",
                Composite = false,
                Physical = true,
                DefaultIcon = baseIcon,
                FeatureGuid = "A06EC6C0-92DB-4E40-B438-2932CB138CD7",
                damageType = new[] { BPSDamage() },
                ProjectileGuid = "c4b0d8b4786a1244d9fbc4b424931b83",
                weaponfx = holyeffect

            };

            AuroraBlast = new()
            {
                Name = "Aurora",
                Composite = false,
                Physical = true,
                DefaultIcon = baseIcon,
                FeatureGuid = "D8E0DE4B-66AC-44D0-B1B6-68A611F9DCF9",
                damageType = new[] { BDamage(), ColdDamage() },
                ProjectileGuid = "c4b0d8b4786a1244d9fbc4b424931b83",
                weaponfx = holyeffect

            };

            BioluminescentBlast = new()
            {
                Name = "Bioluminescent",
                Composite = true,
                Physical = true,
                DefaultIcon = baseIcon,
                FeatureGuid = "519F7A00-9C7C-4FBC-BA0E-C50CA4B29B1B",
                damageType = new[] { BSDamage() },
                ProjectileGuid = "c4b0d8b4786a1244d9fbc4b424931b83",
                weaponfx = holyeffect

            };

            GloriousBlast = new()
            {
                Name = "Glorious",
                Composite = true,
                Physical = true,
                DefaultIcon = baseIcon,
                FeatureGuid = "56FF6626-A7B1-465B-9F16-E6F146DD2A23",
                damageType = new[] { BDamage() },
                ProjectileGuid = "c4b0d8b4786a1244d9fbc4b424931b83",
                weaponfx = holyeffect

            };

            ConfigureBlast(LightBlast);

            ConfigureBlast(GloriousBlast);

            //CreateLightBlastVariant_Base();

            //CreateLightBlastVariant_Extended();

            //CreateLightBlastBaseAbility();

            //CreateLightBlastFeature();

            //CreateIllusoryDuplicates();

            CreateLightBlastProgression();

            CreateLightClassSkills();

            CreateLightFocuses();

            FeatureSelectionConfigurator.For("1f3a15a3ae8a5524ab8b97f469bf4e3d").AddToAllFeatures("ElementalFocusLight").Configure();

            //Dazzling Infusion
            AddToSubstanceInfusion(LightBlast, "037460f7ae3e21943b237007f2b1a5d5", "ee8d9f5631c53684d8d627d715eb635c");
            AddToSubstanceInfusion(GloriousBlast, "037460f7ae3e21943b237007f2b1a5d5", "ee8d9f5631c53684d8d627d715eb635c");

            //Flash Infusion
        }

        private static void ConfigureBlast(BlastElement element)
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

           

            CreateXBlastFeature(element);

            CreateXBlastBaseAbility(element);

            CreateXBlastVariant_Base(element);

            CreateXBlastVariant_ExtendedRange(element);

            CreateXKineticBladeFeature(element);

            CreateKineticBladeXBlastAbility(element);

            CreateKineticBladeXBuffAbility(element);

            MakeXKineticBladeWeapon(element);

            MakeKineticBladeXBlastBurnAbility(element);

            CreateXBlastBladeDamage(element);

            XKineticBladeEnchantment(element);
        }


        #region foci

        private static void CreateLightFocuses()
        {
            var primary = ProgressionConfigurator.New("ElementalFocusLight", "C9E2A78E05A9401B9DEEF8A0BB02238E")
                .AddPrerequisiteNoArchetype("365b50dba54efb74fa24c07e9b7a838c", "42a455d9ec1ad924d889272429eb8391")
                .SetDisplayName("LightFocus.Name")
                .SetDescription("LightFocus.Desc")
                .SetIcon(baseIcon)
                .AddToGroups(FeatureGroup.KineticElementalFocus)
                .AddToClasses("42a455d9ec1ad924d889272429eb8391")
                .AddToLevelEntry(1, "LightClassSkills", "LightBlastProgression")
                .AddToLevelEntry(2, "E11D1E1A-936C-4C66-8DB2-2589A40C43E3")
                .Configure();



            var knight = ProgressionConfigurator.New("KineticKnightElementalFocusLight", "BE3EDD1C-8657-4F00-8A1A-3FE5E808F676")
               .SetDisplayName("LightFocus.Name")
               .SetDescription("LightFocus.Desc")
               .SetIcon(baseIcon)
               .AddToGroups(FeatureGroup.KineticElementalFocus)
               .AddToClasses("42a455d9ec1ad924d889272429eb8391")
               .AddToLevelEntry(1, "LightClassSkills", "LightBlastProgression")
               .AddToLevelEntry(4, "E11D1E1A-936C-4C66-8DB2-2589A40C43E3")
               .Configure();

            ProgressionConfigurator.New("SecondaryElementLight", "B36683B6-AB79-4029-9E04-E73E89070579")
                .AddActivateTrigger(conditions: ConditionsBuilder.New().HasFact(unit: new FactOwner(), fact: "ElementalFocusLight")
                .HasFact(unit: new FactOwner(), fact: "KineticKnightElementalFocusLight"),
                    actions: ActionsBuilder.New().AddFact(unit: new FactOwner(), fact: "GloriousBlastFeature"))
                .AddFacts(facts: new() { "cb30a291c75def84090430fbf2b5c05e" })
                .AddFeatureIfHasFact(checkedFact: "LightBlastFeature", feature: "LightBlastFeature", not: true)
                .SetDisplayName("LightFocus.Name")
                .SetDescription("LightFocus.Desc")
                .SetIcon(baseIcon)
                .SetHideInUI(true)
                .AddToGroups(FeatureGroup.KineticElementalFocus)
                .AddToClasses("42a455d9ec1ad924d889272429eb8391")
                .AddToLevelEntry(7, "LightBlastProgression")
                .Configure();

            ProgressionConfigurator.New("ThirdElementLight", "0EF27AAC-B72F-484F-9C83-51FE3B448E30")
                .AddActivateTrigger(conditions: ConditionsBuilder.New().HasFact(unit: new FactOwner(), fact: "ElementalFocusLight")
                .HasFact(unit: new FactOwner(), fact: "KineticKnightElementalFocusLight"),
                    actions: ActionsBuilder.New().AddFact(unit: new FactOwner(), fact: "GloriousBlastFeature"))
                .AddFacts(facts: new() { "cb30a291c75def84090430fbf2b5c05e" })
                .AddFeatureIfHasFact(checkedFact: "LightBlastFeature", feature: "LightBlastFeature", not: true)
                .SetDisplayName("LightFocus.Name")
                .SetDescription("LightFocus.Desc")
                .SetIcon(baseIcon)
                .SetHideInUI(true)
                .AddToGroups(FeatureGroup.KineticElementalFocus)
                .AddToClasses("42a455d9ec1ad924d889272429eb8391")
                .AddToLevelEntry(15, "LightBlastProgression")
                .Configure();

            ProgressionConfigurator.For("SecondaryElementLight").AddPrerequisiteNoFeature("ThirdElementLight").Configure();
            ProgressionConfigurator.For("ThirdElementLight").AddPrerequisiteNoFeature("SecondaryElementLight").Configure();
            FeatureSelectionConfigurator.For("4204bc10b3d5db440b1f52f0c375848b").AddToAllFeatures("SecondaryElementLight").Configure();
            FeatureSelectionConfigurator.For("e2c1718828fc843479f18ab4d75ded86").AddToAllFeatures("ThirdElementLight").Configure();
        }

        
        private static Kingmaker.Blueprints.Classes.BlueprintFeature CreateLightClassSkills()
        {
            return FeatureConfigurator.New("LightClassSkills", "2AE1D4977B294F9084FA42B5F8190D7F")
                .SetDisplayName("LightClassSkills.Name").SetDescription("LightClassSkills.Desc")
                .AddClassSkill(Kingmaker.EntitySystem.Stats.StatType.SkillLoreNature)
                .AddClassSkill(Kingmaker.EntitySystem.Stats.StatType.SkillStealth)

                .Configure();

        }
        #endregion

        private static void AddToSubstanceInfusion(BlastElement blastElement, string featureID, string buffID)
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

        #region blast backed

       

        private static void CreateXBlastFeature(BlastElement element)
        {
            FeatureConfigurator.For($"{element.Name}BlastFeature")
                .AddFacts(facts: ["cb30a291c75def84090430fbf2b5c05e"])//Composite blast buff
                .AddFeatureIfHasFact($"{element.Name}BlastBase", feature: $"{element.Name}BlastBase", not: true)
                .AddToGroups(FeatureGroup.KineticBlast)
                .SetIcon(baseIcon)
                .SetRanks(20)
                .SetIsClassFeature(true)
                .Configure();
        }


        private static void CreateLightBlastProgression()
        {
            var lightBlastProgression = ProgressionConfigurator.New("LightBlastProgression", "C83F4371-B2F2-4A88-9D05-85781723A754")
                .SetDisplayName("LightBlastAbility.Name")
                .SetDescription("LightBlastAbility.Desc")
                .SetIcon(baseIcon)
                .SetIsClassFeature(true)
                .AddToClasses("42a455d9ec1ad924d889272429eb8391")
                .AddToLevelEntries(1, "LightBlastFeature")
                .SetHideInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .Configure();

        }

        



        #endregion

        #region Light Blast

        private static void CreateXBlastBaseAbility(BlastElement element)
        {
            AbilityConfigurator.For($"{element.Name}BlastBase")
                .AddAbilityVariants([$"{element.Name}BlastAbility", $"ExtendedRange{element.Name}BlastAbility"])
                .AddAbilityShowIfCasterHasFact(unitFact: "1f3a15a3ae8a5524ab8b97f469bf4e3d")//Focus Selection
                .AddAbilityKineticist(amount: 1)
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



        private static void CreateXBlastVariant_Base(BlastElement element)
        {
            var blast = AbilityConfigurator.For($"{element.Name}BlastAbility")
                .SetRange(AbilityRange.Close)
                .SetParent($"{element.Name}BlastBase")
                .ApplyShotProperties(element, 0)
                .Configure();
        }

        private static void CreateXBlastVariant_ExtendedRange(BlastElement element)
        {
            var blast = AbilityConfigurator.For($"ExtendedRange{element.Name}BlastAbility")
                .AddAbilityShowIfCasterHasFact(unitFact: "cb2d9e6355dd33940b2bef49e544b0bf")
                .SetRange(AbilityRange.Long)
                .SetParent($"{element.Name}BlastBase")
                .ApplyShotProperties(element, 1)
                .Configure();
        }

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


        private static AbilityConfigurator ApplyShotProperties(this AbilityConfigurator abilityConfigurator, BlastElement element, int shotcost)
        {
            ContextDiceValue damageDiceValue = element.Composite ? (element.Physical ? PhysicalCompositeBlastDice() : EnergyCompositeBlastDice()) : element.Physical ? PhysicalSimpleBlastDice() : EnergySimpleBlastDice();


            return abilityConfigurator.SetIcon(baseIcon)
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




        private static void CreateKineticBladeXBlastAbility(BlastElement element)
        {
            ActivatableAbilityConfigurator.For($"KineticBlade{element.Name}BlastAbility")
                .AddRestrictionCanUseKineticBlade()
                .SetDescription(kinBlade.m_Description)
                .SetGroup(Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilityGroup.FormInfusion)
                .SetIcon(element.DefaultIcon)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfOwnerUnconscious(true)
                .SetActivationType(Kingmaker.UnitLogic.ActivatableAbilities.AbilityActivationType.Immediately)
                .SetActivateWithUnitCommand(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetActivateOnUnitAction(Kingmaker.UnitLogic.ActivatableAbilities.AbilityActivateOnUnitActionType.Attack)
                .SetBuff($"KineticBlade{element.Name}BlastBuff")
                .Configure();
        }

        private static void CreateKineticBladeXBuffAbility(BlastElement element)
        {
            BuffConfigurator.For($"KineticBlade{element.Name}BlastBuff")
                .AddKineticistBlade(blade: $"{element.Name}KineticBladeWeapon")
                .SetFlags(BlueprintBuff.Flags.HiddenInUi, BlueprintBuff.Flags.StayOnDeath)
                .SetIsClassFeature(true)
                .Configure();
        }

        private static void MakeXKineticBladeWeapon(BlastElement element)
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

        private static void MakeKineticBladeXBlastBurnAbility(BlastElement element)
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

        private static void CreateXBlastBladeDamage(BlastElement element)
        {
            ContextDiceValue damageDiceValue = element.Composite ? (element.Physical ? PhysicalCompositeBlastDice() : EnergyCompositeBlastDice()) : element.Physical ? PhysicalSimpleBlastDice() : EnergySimpleBlastDice();

            AbilityConfigurator.For($"{element.Name}BlastBladeDamage")
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

        private static void XKineticBladeEnchantment(BlastElement element)
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

        private static void CreateXKineticBladeFeature(BlastElement element)
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

        #endregion

        #region Light Blast Helpers

        private static ContextDiceValue PhysicalSimpleBlastDice()
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
                }
            };
        }

        private static ContextDiceValue EnergySimpleBlastDice()
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

        private static ContextDiceValue EnergyCompositeBlastDice()
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

        private static ContextDiceValue PhysicalCompositeBlastDice()
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







        #endregion

        #region Blast Helpers

        private static ActionsBuilder BuildChainableDamageAction(BlastElement element)
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

        private static DamageTypeDescription BPSDamage()
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

        private static DamageTypeDescription BColdDamage()
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

        private static DamageTypeDescription ColdDamage()
        {
            return new()
            {
                Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                Energy = Kingmaker.Enums.Damage.DamageEnergyType.Cold
            };
        }

        private static DamageTypeDescription BDamage()
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

        private static DamageTypeDescription BSDamage()
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

        private static ContextRankConfig KinStatProperty(bool div2)
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



        #endregion
        private static void CreateLightBlast()
        {


            string progGuid = "C83F4371-B2F2-4A88-9D05-85781723A754";
            string lightblastfeatureguid = "EB5AD780-4AD4-43A4-BD77-5F8B83C53F30";
            string lightbaseabilityguid = "9A8E6020-DF82-43C5-83B5-45A02E26470C";
            string lightbladefeatureguid = "00DB5B41-56CE-4D66-802E-94D3B205C65D";
            string lightbladeacticate = "0B735BB3-8D2C-4B1C-BD46-8BFC17F3CD3A";
            string lightbladeweaponguid = "F1ED7C1C-B0BA-4D9E-B4B0-1C49FC9DD7E5";
            string lightbladedamageguid = "32BEC956-1014-4BDB-9BEE-F201438E1A5E";
            string lightbladeburnguid = "32BEC956-1014-4BDB-9BEE-F201438E1A5E";
            throw new NotImplementedException();
        }







        private static ContextRankConfig KineticBlastFeatureCountConfig()
        {
            return (new Kingmaker.UnitLogic.Mechanics.Components.ContextRankConfig()
            {
                m_Type = Kingmaker.Enums.AbilityRankType.DamageDice,
                m_BaseValueType = ContextRankBaseValueType.FeatureRank,
                m_Feature = BlueprintTool.GetRef<BlueprintFeatureReference>("93efbde2764b5504e98e6824cab3d27c"),
                m_Max = 20
            });
        }






        private static void CreateIllusoryDuplicates()
        {
            string guid = "E11D1E1A-936C-4C66-8DB2-2589A40C43E3";
            FeatureConfigurator.New("IllusoryDuplicatesFeature", guid)
                .SetDisplayName("IllusoryDuplicates.Name")
                .SetDescription("IllusoryDuplicates.Desc")


                .Configure();

        }


        private static void CreateCompositeBlasts()
        {

            CreateAurora();
            CreateBioluminescent();
            CreateCrystal();
            CreateGlorious();
            CreateLightning();
            CreateRainbow();
            CreateSolar();

            throw new NotImplementedException();
        }

        private static void CreateSolar()
        {

            throw new NotImplementedException();
        }

        private static void CreateRainbow()
        {
            throw new NotImplementedException();
        }

        private static void CreateLightning()
        {
            throw new NotImplementedException();
        }

        private static void CreateGlorious()
        {
            throw new NotImplementedException();
        }

        private static void CreateCrystal()
        {
            throw new NotImplementedException();
        }

        private static void CreateBioluminescent()
        {
            //{76D7AB9D-2DF3-475E-90E6-129632CF4825}
            throw new NotImplementedException();
        }

        private static void CreateAurora()
        {
            string blastfeature = "96DB4404-63D2-4BEF-9B5E-D10F36A90DEC";
            string baseability = "9FDCE3C8-087B-4440-8F96-6B7432DED9B7";
            string bladefeatureguid = "7F6ACE56-5CF6-4627-86F1-71443EAD82D4";
            string bladeactiveguid = "C2CC58BD-B4D7-4939-8C6A-A2028F3EF45C";
            string bladeweaponguid = "D28D200F-E9FF-41E6-83D4-BDCDBF3D755E";
            string bladedamageguid = "A8186198-B162-4486-AF31-F879833023D9";
            string bladeburnguid = "08A7F135-2535-489C-9AFD-8E98BFBD0627";
            throw new NotImplementedException();
        }



        public static AbilityConfigurator AddKBRicochet(this AbilityConfigurator input, string projectile)
        {
            return input.AddAbilityDeliverRicochet(layer: 1, beforeCondition: ConditionsBuilder.New().UseOr()
                .Add(new ContextConditionHasBuff() { m_Buff = BlueprintTool.GetRef<BlueprintBuffReference>("5f7d567ae4054cc291e42fc43ef5a046") })
                .Add(new ContextConditionHasBuff() { m_Buff = BlueprintTool.GetRef<BlueprintBuffReference>("f583e43e4c904c1e854c479a280ab657") }),
                   targetsCount: new ContextValue()
                   {
                       ValueType = ContextValueType.CasterCustomProperty,
                       m_CustomProperty = BlueprintTool.GetRef<BlueprintUnitPropertyReference>("4a18040254d040f78c298f10649eab71")

                   },
                   radius: new Kingmaker.Utility.Feet(10f),
                   targetType: Kingmaker.UnitLogic.Abilities.Components.TargetType.Enemy,
                   projectile: projectile);
        }



        #region infusion

        private static void CreateInfusions()
        {
            CreateIlluminatingInfusion();
            CreatePhotokineticInfusion();
            CreateBeaconInfusion();
            CreateObfuscatingInfusion();
            CreateOverloadInfusion();
            CreateColorburstInfusion();
        }

        private static void CreateColorburstInfusion()
        {
            throw new NotImplementedException();
        }

        private static void CreateOverloadInfusion()
        {
            throw new NotImplementedException();
        }

        private static void CreateObfuscatingInfusion()
        {
            throw new NotImplementedException();
        }

        private static void CreatePhotokineticInfusion()
        {
            throw new NotImplementedException();
        }

        private static void CreateBeaconInfusion()
        {
            throw new NotImplementedException();
        }

        private static void CreateIlluminatingInfusion()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
