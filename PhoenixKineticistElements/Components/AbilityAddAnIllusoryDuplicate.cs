using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using PhoenixKineticistElements.Overrides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace PhoenixKineticistElements.Components
{
    internal class AbilityAddAnIllusoryDuplicate : AbilityApplyEffect
    {
        public override void Apply(AbilityExecutionContext context, TargetWrapper target)
        {
            target.Unit.Ensure<UnitPartIllusoryDuplicates>();
            target.Unit.Ensure<UnitPartMirrorImage>();
            target.Unit.Get<UnitPartIllusoryDuplicates>().AddIllusion();
        }
    }
}
