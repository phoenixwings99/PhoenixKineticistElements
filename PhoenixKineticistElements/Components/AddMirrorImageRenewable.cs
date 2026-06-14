using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixKineticistElements.Components
{
    public class AddMirrorImageRenewable : UnitBuffComponentDelegate
    {
        public override void OnActivate()
        {
            Main.Log.Log($"AddMirrorImageRenewable.OnActivate prefix firing from {Buff.Blueprint.name} on {Context.MainTarget.Unit.CharacterName}");
            base.OnActivate();
            base.Owner.Ensure<UnitPartMirrorImage>().Init(1, base.Buff);
        }
    }
}
