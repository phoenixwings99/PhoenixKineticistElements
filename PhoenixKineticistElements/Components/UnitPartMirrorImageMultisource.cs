using JetBrains.Annotations;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using Kingmaker.Visual.Particles;
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
        public List<Tuple<int, Buff>> ImagesFromSource = new List<Tuple<int, Buff>>();

        public bool ConsiderRemove()
        {
            return ImagesFromSource.Count == 0;

        }

        internal void RemoveImagesFromSource(Buff source)
        {
            ImagesFromSource.RemoveAll(x =>
            {
                return x.Item2 == source;
            });
        }






        internal void AddImageFromSource(int imageNum, Buff source)
        {
            Main.Log.Log($"UnitPartMirrorImageMultisource.Init: adding image at val {imageNum} from {source.Name}");
            ImagesFromSource.Add(new(imageNum, source));
        }

        internal void RemoveImage(int v)
        {
            var maybe = ImagesFromSource.First(x =>
            {
                return x.Item1 == v;
            });
            var maybeBuff = maybe.Item2;
            if (maybe != null)
            {
                Main.Log.Log($"UnitPartMirrorImageMultisource.RemoveImage: adding image at val {v} from {maybeBuff.Name}");
                
                ImagesFromSource.Remove(maybe);
            }
            if (!ImagesFromSource.Any(x=> x.Item2 == maybeBuff))
            {
                Main.Log.Log($"UnitPartMirrorImageMultisource.RemoveImage: removing buff {maybeBuff.Name}");
                maybeBuff.Remove();
            }

        }

        


        
    }
}
