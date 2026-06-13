using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Units;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.UnitLogic.Parts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Designers.EventConditionActionSystem.Actions.TransferSharedVendorTable;

namespace PhoenixKineticistElements.Components
{
    public class UnitPartIllusoryDuplicates : OldStyleUnitPart, IUnitRestHandler, IGlobalSubscriber, ISubscriber, IUnitLevelUpHandler, ITickEachRound
    {
        BlueprintBuffReference countBuff;
        BlueprintBuffReference imageBuff;
        BlueprintUnitProperty count;

        
        int replaceDuplicateTime = 50;

        [JsonProperty]
        int Ticks { get; set; } = 0;

        public UnitPartIllusoryDuplicates()
        {
            countBuff = BlueprintTool.GetRef<BlueprintBuffReference>("IllusoryDuplicatesUpgradeBuff");
            imageBuff = BlueprintTool.GetRef<BlueprintBuffReference>("IllusoryDuplicatesEffectBuff");
            count = BlueprintTool.Get<BlueprintUnitProperty>("IllusoryDuplicatesCurrentUpgrades");

        }

        private void AddIllusion()
        {
            Main.Log.Log($"AddIllusion firing on UnitPartIllusoryDuplicates");
            Owner.AddBuff(blueprint: imageBuff, Owner);
        }

        public int GetDeployedIllusions()
        {
            int illusionCount = Owner.Buffs.Enumerable.Count(x => x.Blueprint.ToReference<BlueprintBuffReference>().Equals(countBuff));
            Main.Log.Log($"GetDeployedIllusions called for {Owner.CharacterName}, Illusions actove is {illusionCount}");
            return illusionCount;
        }

        public int GetIllusionLimit()
        {

            int limit = count.GetInt(Owner) + 1;
            Main.Log.Log($"GetIllusionLimit called for {Owner.CharacterName}, Illusion Limit is {limit}");
            return limit;
        }

        public void HandleUnitRest(UnitEntityData unit)
        {
            DeployAllowedDuplicates();
        }

        internal void DeployAllowedDuplicates()
        {
            int count = GetIllusionLimit() - GetDeployedIllusions();
            Main.Log.Log($"DeployAllowedDuplicates called for {Owner.CharacterName}, Deploying {count}");
            for (int i = 0; i < count; i++)
            {
                AddIllusion();
            }
        }

        public void HandleUnitBeforeLevelUp(UnitEntityData unit)
        {

        }

        public void HandleUnitAfterLevelUp(UnitEntityData unit, LevelUpController controller)
        {

        }

        public void OnNewRound()
        {
            Ticks++;
            if (Ticks >= replaceDuplicateTime && GetIllusionLimit() > GetDeployedIllusions())
            {
                Ticks = 0;
                AddIllusion();

            }
            else
            {
                Ticks = replaceDuplicateTime / 2;
            }
        }
    }
}
