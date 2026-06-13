using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixKineticistElements.Components
{
    internal class AddIllusoryDuplicates : UnitFactComponentDelegate, IUnitLevelUpHandler, ISubscriber, IGlobalSubscriber
    {
        

        public void HandleUnitAfterLevelUp(UnitEntityData unit, LevelUpController controller)
        {
            Main.Log.Log("AddIllusoryDuplicates leveluphandler firing");
            base.Owner.Ensure<UnitPartIllusoryDuplicates>();
            //base.Owner.Ensure<UnitPartIllusoryDuplicates>().DeployAllowedDuplicates();
            
        }

        public void HandleUnitBeforeLevelUp(UnitEntityData unit)
        {
            
        }
    }
}
