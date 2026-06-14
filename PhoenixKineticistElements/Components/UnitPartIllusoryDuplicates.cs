using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Units;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.UnitLogic.Parts;
using Newtonsoft.Json;
using PhoenixKineticistElements.Overrides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.UMMTools;
using static Kingmaker.Designers.EventConditionActionSystem.Actions.TransferSharedVendorTable;

namespace PhoenixKineticistElements.Components
{
    public class UnitPartIllusoryDuplicates : OldStyleUnitPart, IUnitRestHandler, IGlobalSubscriber, ISubscriber, IUnitLevelUpHandler, ITickEachRound
    {
        [JsonProperty]
        BlueprintBuffReference UpgradeBuff { get; set; }
        [JsonProperty]
        BlueprintBuffReference ImagesBuff { get; set; }
        [JsonProperty]
        BlueprintUnitProperty IllusionUpgradeCountProperty;

        [JsonProperty]
        List<int> active =new List<int>();
        [JsonProperty]
        Dictionary<int, int> CooldownBySlot = new();

        [JsonProperty]
        int cooldownTime = 300;

        

        public UnitPartIllusoryDuplicates()
        {
            ImagesBuff = BlueprintTool.GetRef<BlueprintBuffReference>("IllusoryDuplicatesEffectBuff");
            UpgradeBuff = BlueprintTool.GetRef<BlueprintBuffReference>("IllusoryDuplicatesUpgradeBuff");
            
            IllusionUpgradeCountProperty = BlueprintTool.Get<BlueprintUnitProperty>("IllusoryDuplicatesCurrentUpgrades");

        }

        public void AddIllusion()
        {
            Main.Log.Log($"AddIllusion firing on UnitPartIllusoryDuplicates");
            int slot = Owner.Get<UnitPartMirrorImage>().AddIllusoryDuplicate(Owner.Buffs.GetBuff(ImagesBuff.Get()));
            active.Add(slot);
            
        }

        public int GetDeployedIllusions()
        {
            int illusionCount = Owner.Ensure<UnitPartMirrorImageMultisource>().Images.Count(x=>x.buff == Owner.Buffs.GetBuff(ImagesBuff) && x.active);
            Main.Log.Log($"GetDeployedIllusions called for {Owner.CharacterName}, Illusions actove is {illusionCount}");
            return illusionCount;
        }

        public int GetIllusionLimit()
        {

            int limit = IllusionUpgradeCountProperty.GetInt(Owner) + 1;
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
            foreach(var v in CooldownBySlot)
            {
                CooldownBySlot[v.Key] = v.Value + 1;
                if (CooldownBySlot[v.Key] >= cooldownTime)
                {
                    Owner.Get<UnitPartMirrorImage>().ReactivateRenewableImage(Owner.Buffs.GetBuff(ImagesBuff), v.Key);
                    active.Add(v.Key);
                    CooldownBySlot.Remove(v.Key);
                }

            }
            
        }

        internal void ExpendImage(int slot)
        {
            active.Remove(slot);
            CooldownBySlot[slot]= 0;
        }
    }
}
