using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.Configurators.UnitLogic.Properties;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI.MVVM._ConsoleView.InGame;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Class.Kineticist;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Owlcat.Runtime.UI.Utility;
using PhoenixKineticistElements.Components;
using System;
using System.Xml.Linq;
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
        private static Sprite baseIconMelee;

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
            baseIconMelee = BlueprintTool.Get<BlueprintActivatableAbility>("24ee96e7c589333468da975fd3892a8d").Icon;//Searing Light
            
            string holyeffect = BlueprintTool.Get<BlueprintWeaponEnchantment>("28a9964d81fedae44bae3ca45710c140").WeaponFxPrefab.AssetId;
            kinBlade = BlueprintTool.Get<BlueprintFeature>("9ff81732daddb174aa8138ad1297c787");
            kinBladeBuff = BlueprintTool.Get<BlueprintBuff>("426a9c079ee7ac34aa8e0054f2218074");
            KinBladeInventoryIcon = BlueprintTool.Get<BlueprintItemWeapon>("43ff67143efb86d4f894b10577329050").Icon;


            //c4b0d8b4786a1244d9fbc4b424931b83 sunbeam for solar blast
            //c7734162c01abdc478418bfb286ed7a5 lightning bolt for lightning blast
            LightBlast = new()
            {
                Name = "Light",
                Composite = false,
                Physical = true,
                DefaultIcon = baseIcon,
                DefaultIconMelee = baseIconMelee,
                FeatureGuid = "A06EC6C0-92DB-4E40-B438-2932CB138CD7",
                damageType = new[] { ElementFactories.BPSDamage() },
                ProjectileGuid = "2511627d593387d4d89004bec111ba31", //Searing Light
                weaponfx = holyeffect

            };

            AuroraBlast = new()
            {
                Name = "Aurora",
                Composite = false,
                Physical = true,
                DefaultIcon = baseIcon,
                DefaultIconMelee = baseIconMelee,
                FeatureGuid = "D8E0DE4B-66AC-44D0-B1B6-68A611F9DCF9",
                damageType = new[] { ElementFactories.BDamage(), ElementFactories.ColdDamage() },
                ProjectileGuid = "5769363a427374f428490092c57820a7", //Prismatic spray
                weaponfx = holyeffect,
                spellDescriptors = SpellDescriptor.Cold,
                Burn =2

            };

            BioluminescentBlast = new()
            {
                Name = "Bioluminescent",
                Composite = true,
                Physical = true,
                DefaultIcon = baseIcon,
                DefaultIconMelee = baseIconMelee,
                FeatureGuid = "519F7A00-9C7C-4FBC-BA0E-C50CA4B29B1B",
                damageType = new[] { ElementFactories.BSDamage() },
                ProjectileGuid = "c4b0d8b4786a1244d9fbc4b424931b83",
                weaponfx = holyeffect,
                Burn = 2

            };

            CrystalBlast = new()
            {
                Name = "Crystal",
                Composite = true,
                Physical = true,
                DefaultIcon = baseIcon,
                DefaultIconMelee = baseIconMelee,
                FeatureGuid = "948BD692-A37A-4CB6-A129-0BBDBF2B2F9F",
                damageType = new[] { ElementFactories.BSDamage() },
                ProjectileGuid = "c4b0d8b4786a1244d9fbc4b424931b83",
                weaponfx = holyeffect,
                Burn = 2

            };

            GloriousBlast = new()
            {
                Name = "Glorious",
                Composite = true,
                Physical = true,
                DefaultIcon = baseIcon,
                DefaultIconMelee = baseIconMelee,
                FeatureGuid = "56FF6626-A7B1-465B-9F16-E6F146DD2A23",
                damageType = new[] { ElementFactories.BDamage() },
                ProjectileGuid = "c4b0d8b4786a1244d9fbc4b424931b83",
                weaponfx = holyeffect,
                Burn = 2

            };

            RainbowBlast = new()
            {
                Name = "Rainbow",
                Composite = true,
                Physical = true,
                DefaultIcon = baseIcon,
                DefaultIconMelee = baseIconMelee,
                FeatureGuid = "6732B69D-44CB-41D7-B500-1D4F513641AD",
                damageType = new[] { ElementFactories.BDamage() },
                ProjectileGuid = "54a9377a7b01f7d44a56e7713f939db6", //Azata Rainbow Arrows
                weaponfx = holyeffect,
                Burn = 2

            };


            ConfigureXBlast(LightBlast);

            ConfigureXBlast(AuroraBlast);

            ConfigureXBlast(RainbowBlast);

            ConfigureXBlast(GloriousBlast);



            CreateIllusoryDuplicates();

            CreateLightBlastProgression();

            CreateLightClassSkills();

            CreateLightFocuses();

            FeatureSelectionConfigurator.For("1f3a15a3ae8a5524ab8b97f469bf4e3d").AddToAllFeatures("ElementalFocusLight").Configure();

            //Dazzling Infusion
            ElementFactories.AddToSubstanceInfusion(LightBlast, "037460f7ae3e21943b237007f2b1a5d5", "ee8d9f5631c53684d8d627d715eb635c");
            ElementFactories.AddToSubstanceInfusion(GloriousBlast, "037460f7ae3e21943b237007f2b1a5d5", "ee8d9f5631c53684d8d627d715eb635c");
            ElementFactories.AddToSubstanceInfusion(RainbowBlast, "037460f7ae3e21943b237007f2b1a5d5", "ee8d9f5631c53684d8d627d715eb635c");
            ElementFactories.AddToSubstanceInfusion(AuroraBlast, "037460f7ae3e21943b237007f2b1a5d5", "ee8d9f5631c53684d8d627d715eb635c");

            //Flash Infusion

            ElementFactories.AddToSubstanceInfusion(LightBlast, "37f3cfca29073e142a80c3b8e7c54b05", "50cf40b1cb3115546a3e9b44d7687384");
            ElementFactories.AddToSubstanceInfusion(GloriousBlast, "37f3cfca29073e142a80c3b8e7c54b05", "50cf40b1cb3115546a3e9b44d7687384");
            ElementFactories.AddToSubstanceInfusion(RainbowBlast, "37f3cfca29073e142a80c3b8e7c54b05", "50cf40b1cb3115546a3e9b44d7687384");
            ElementFactories.AddToSubstanceInfusion(AuroraBlast, "37f3cfca29073e142a80c3b8e7c54b05", "50cf40b1cb3115546a3e9b44d7687384");

            MakeSkilledKineticistXBuff("Light", StatType.SkillLoreNature);
        }

        private static void ConfigureXBlast(BlastElement element)
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

           

            AbilityConfigurator.For("80f10dc9181a0f64f97a9f7ac9f47d65").EditComponent<AbilityCasterHasFacts>(x =>
            {

                x.m_Facts = x.m_Facts.AddItem<BlueprintUnitFactReference>(BlueprintTool.GetRef<BlueprintUnitFactReference>($"KineticBlade{element.Name}BlastBuff")).ToArray();
            }).Configure();
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
                .AddActivateTrigger(conditions: ConditionsBuilder.New()
                    .HasFact(unit: new FactOwner(), fact: "ElementalFocusLight")
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
                .AddActivateTrigger(conditions: ConditionsBuilder.New()
                    .HasFact(unit: new FactOwner(), fact: "ElementalFocusLight")
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

                .Configure();

        }
        #endregion

        

        #region blast backed

       

        

        private static void CreateLightBlastProgression()
        {
            var lightBlastProgression = ProgressionConfigurator.New("LightBlastProgression", "C83F4371-B2F2-4A88-9D05-85781723A754")
                .SetDisplayName("LightBlastAbility.Name")
                .SetDescription("LightBlastAbility.Desc")
                .SetIcon(baseIcon)
                .SetIsClassFeature(true)
                .AddToClasses("42a455d9ec1ad924d889272429eb8391")
                .AddFeatureIfHasFact($"LightBlastBase", feature: $"LightBlastBase", not: true)
                .AddFeatureIfHasFact(checkedFact: $"9ff81732daddb174aa8138ad1297c787", feature: $"LightKineticBladeFeature")
                .AddToLevelEntries(1, "LightBlastFeature")
                .SetHideInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .Configure();
            
        }

        



        #endregion

        #region Light Blast

        

      
        


       
        #endregion

        

        







        

        #region Blast Helpers

        

        






        private static void CreateIllusoryDuplicates()
        {
            AbilityResourceConfigurator.New("IllusoryDuplicatesResource", "11F0E81B-180C-4FBD-8C7E-EEEE5E3DF410")
                .SetLocalizedName("IllusoryDuplicates.Name")
                .SetLocalizedDescription("IllusoryDuplicates.Desc")
                .SetMaxAmount(ResourceAmountBuilder.New(0).IncreaseByLevelStartPlusDivStep(classes: ["42a455d9ec1ad924d889272429eb8391"], startingLevel: 1, startingBonus: 1, levelsPerStep: 4, bonusPerStep: 1))
                .Configure();


            BuffConfigurator.New("IllusoryDuplicatesEffectBuff", "9EE017FF-9415-4968-9FE0-87F9813D7677")
                .SetIsClassFeature(true)
                .SetFlags(BlueprintBuff.Flags.HiddenInUi, BlueprintBuff.Flags.StayOnDeath, BlueprintBuff.Flags.RemoveOnRest)
                .SetStacking(StackingType.Replace)
                .SetIsClassFeature(true)
                .SetFxOnStart("bde04297a8dc74b4fa22480f4e40133b")
                .AddComponent<AddMirrorImageRenewable>(x=>{
                })
                
                .SetRanks(0)
                .Configure();

            FeatureConfigurator.New("IllusoryDuplicatesUpgradeFeature", "7C1C0BA2-0371-482B-AE3C-E7B92302995D")
                .Configure();

            BuffConfigurator.New("IllusoryDuplicatesUpgradeBuff", "7B51854E-94FF-476A-8439-421A4F04F57E")
                .SetIsClassFeature(true)
                .SetFlags(BlueprintBuff.Flags.HiddenInUi, BlueprintBuff.Flags.StayOnDeath, BlueprintBuff.Flags.RemoveOnRest)
                .SetStacking(StackingType.Stack)
                .SetRanks(50)
                .AddNotDispelable()
                .Configure();

           

            UnitPropertyConfigurator.New("IllusoryDuplicatesCurrentUpgrades", "15553116-1CF8-4DED-BE2F-5C962F332D2D")
                .AddFactRankGetter("IllusoryDuplicatesUpgradeBuff")                
                .Configure();

            BuffConfigurator.For("IllusoryDuplicatesEffectBuff").AddContextRankConfig(ContextRankConfigs.CustomProperty("IllusoryDuplicatesCurrentUpgrades", AbilityRankType.StatBonus)).Configure();

            AbilityConfigurator.New("IllusoryDuplicatesAbility", "67E50DF0-B17A-4A17-848D-1899D1BF0374")
                .SetDisplayName("IllusoryDuplicates.Name")
                .SetDescription("IllusoryDuplicates.Desc")
                .AddAbilityEffectRunAction(
                ActionsBuilder.New()
                        .ApplyBuffPermanent("IllusoryDuplicatesUpgradeBuff", isNotDispelable: true, isFromSpell: false, asChild: true)
                    //                    .RemoveBuff("IllusoryDuplicatesEffectBuff")
                    //                    .ApplyBuffPermanent("IllusoryDuplicatesEffectBuff", isNotDispelable: true, isFromSpell: false, asChild: true)
                    )
                .AddComponent<AbilityAddAnIllusoryDuplicate>()
                .SetCanTargetSelf(true)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free) 
                .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
                .SetRange(AbilityRange.Personal)
                .SetIcon(BlueprintTool.Get<BlueprintAbility>("3e4ab69ada402d145a5e0ad3ad4b8564").Icon)
                .AddAbilitySpawnFx(anchor: Kingmaker.UnitLogic.Abilities.Components.Base.AbilitySpawnFxAnchor.Caster, prefabLink: "790eb82d267bf0749943fba92b7953c2", time: Kingmaker.UnitLogic.Abilities.Components.Base.AbilitySpawnFxTime.OnApplyEffect)
                .AddAbilityKineticist(amount: 1, wildTalentBurnCost: 1).AddComponent<AbilityRestrictionWildTalentCastCapper>(x =>
                {

                    x.m_facts = new List<BlueprintUnitFactReference>()
                    {
                        BlueprintTool.GetRef<BlueprintUnitFactReference>("IllusoryDuplicatesUpgradeBuff")

                    };
                    x.useCapResource = true;
                    x.m_CapResource = BlueprintTool.GetRef<BlueprintAbilityResourceReference>("IllusoryDuplicatesResource");
                    x.IsDefense = true;
                    //x.m_MythicKineticDefense = BlueprintTool.GetRef<BlueprintFeatureReference>("MythicKineticDefenses");
                })
                .Configure();

                    BuffConfigurator.New("IllusoryDuplicatesBuff", "4E958E74-4B24-42DF-B72C-6073BFEDAA73")
                .SetDisplayName("IllusoryDuplicates.Name")
                .SetDescription("IllusoryDuplicates.Desc")


                .Configure();

            string guid = "E11D1E1A-936C-4C66-8DB2-2589A40C43E3";
            FeatureConfigurator.New("IllusoryDuplicatesFeature", guid)
                .SetDisplayName("IllusoryDuplicates.Name")
                .SetDescription("IllusoryDuplicates.Desc")
                .AddAbilityResources(amount: 0, resource: "IllusoryDuplicatesResource", restoreAmount: true)
                .AddComponent<EnableIllusoryDuplicates>()
                .AddFacts(facts: new() { "IllusoryDuplicatesAbility" })
                .Configure();

            

        }

        public static void MakeSkilledKineticistXBuff(string elementName, StatType skill)
        {
            var guid = Main.LocalPKEModContext.Blueprints.GetDerivedGUID($"SkilledKineticist{elementName}", BlueprintTool.Get<BlueprintFeature>("fd13e9efee08db448974fe0263eb96c8").AssetGuid, BlueprintTool.Get<BlueprintProgression>($"ElementalFocus{elementName}").AssetGuid);

            var buff = BuffConfigurator.New($"SkilledKineticist{elementName}Buff", guid.m_Guid.ToString())
                .AddContextRankConfig(new ContextRankConfig()
                {
                    m_BaseValueType = ContextRankBaseValueType.ClassLevel,
                    m_Progression = ContextRankProgression.Div2,
                    m_Class = [BlueprintTool.GetRef<BlueprintCharacterClassReference>("42a455d9ec1ad924d889272429eb8391")]
                })
                .AddContextStatBonus(skill, value: new ContextValue() { ValueType = ContextValueType.Rank }, descriptor: ModifierDescriptor.UntypedStackable, multiplier: 1)
                .SetDisplayName(BlueprintTool.Get<BlueprintFeature>("fd13e9efee08db448974fe0263eb96c8").m_DisplayName)
                .SetIsClassFeature(true)
                .SetFlags(BlueprintBuff.Flags.HiddenInUi, BlueprintBuff.Flags.StayOnDeath)
                .Configure();

            BuffConfigurator.For("56b70109d78b0444cb3ad04be3b1ee9e")
                .AddFeatureIfHasFact($"ElementalFocus{elementName}", buff)
                .AddFeatureIfHasFact($"KineticKnightElementalFocus{elementName}", buff)
                .Configure();
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



        
        #endregion
        

    }
}
