using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.ModLogic;
using Unity.Burst.Intrinsics;
using UnityModManagerNet;

namespace PhoenixKineticistElements
{
    public class PKEModContext : ModContextBase
    {
        public PKEModContext(UnityModManager.ModEntry modEntry) : base(modEntry)
        {
#if DEBUG
            Debug = true;
#endif
            LoadAllSettings();

        }

        public override void LoadAllSettings()
        {
            LoadBlueprints("PhoenixKineticistElements.Config", this); 
        }
    }
}
