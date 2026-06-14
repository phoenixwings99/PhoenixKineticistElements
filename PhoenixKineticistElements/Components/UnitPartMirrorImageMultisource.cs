using JetBrains.Annotations;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using Kingmaker.Visual.Particles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LayoutRedirectElement;

namespace PhoenixKineticistElements.Components
{
    public class UnitPartMirrorImageMultisource : OldStyleUnitPart
    {

        [JsonProperty]
        public List<MirrorImageInstance> Images = new List<MirrorImageInstance>();
        

        public bool ConsiderRemove()
        {
            return Images.Count == 0;

        }

        internal void RemoveImagesFromSource(Buff source)
        {
            Images.RemoveAll(x =>
            {
                return x.buff  == source;
            });
        }






        internal void AddImageFromSource(int imageNum, Buff source, bool persistant)
        {
            Main.Log.Log($"UnitPartMirrorImageMultisource.AddImageFromSource: adding image at val {imageNum} from {source.Name}");
            Images.Add(new(imageNum, persistant, source, true));
        }

        internal void ExpendImage(int v)
        {
            var maybe = Images.First(x =>
            {
                return x.indexThingy == v;
            });
            var maybeBuff = maybe.buff;
            if (maybe != null)
            {
                Main.Log.Log($"UnitPartMirrorImageMultisource.RemoveImage: adding image at val {v} from {maybeBuff.Name}");
                
                if (maybe.persistant)
                {
                    maybe.active = false;
                }
                else
                {
                    Images.Remove(maybe);
                    if (!Images.Any(x => x.buff == maybeBuff))
                    {
                        Main.Log.Log($"UnitPartMirrorImageMultisource.RemoveImage: removing buff {maybeBuff.Name}");
                        maybeBuff.Remove();
                    }
                }            
            }
            

        }



        public class MirrorImageInstance
        {
            [JsonProperty]
            public int indexThingy;
            [JsonProperty]
            public bool persistant;
            [JsonProperty]
            public Buff buff;
            [JsonProperty]
            public bool active;

            public MirrorImageInstance(int indexThingy, bool persistant, Buff buff, bool active)
            {
                this.indexThingy = indexThingy;
                this.persistant = persistant;
                this.buff = buff;
                this.active = active;
            }
        }


    }
}
