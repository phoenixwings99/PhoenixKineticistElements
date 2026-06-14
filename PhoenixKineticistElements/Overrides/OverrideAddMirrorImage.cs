using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.View;
using Kingmaker.Visual.Particles;
using Kingmaker.Visual.Particles.FxSpawnSystem;
using PhoenixKineticistElements.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using static Kingmaker.Visual.Particles.MirrorImageFX;
using static LayoutRedirectElement;

namespace PhoenixKineticistElements.Overrides
{
    public static class OverrideAddMirrorImage
    {

        [HarmonyPatch(typeof(AddMirrorImage), nameof(AddMirrorImage.OnActivate))]
        class OverrideOnActivate
        {
            static bool Prefix(AddMirrorImage __instance)
            {
                try
                {
                    __instance.OnTurnOn();
                    int imagesCount = Math.Min(__instance.MaxCount, __instance.Count.Calculate(__instance.Context));
                    Main.Log.Log($"AddMirrorImage.OnActivate prefix firing from {__instance.Buff.Blueprint.name} on {__instance.Context.MainTarget.Unit.CharacterName} for images {imagesCount}");
                    __instance.Owner.Ensure<UnitPartMirrorImage>().Init(imagesCount, __instance.Buff);

                }
                catch (Exception ex)
                {
                    Main.Log.Error(ex.Message);
                    Main.Log.Error(ex.StackTrace);
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(AddMirrorImage), nameof(AddMirrorImage.OnDeactivate))]
        class OverrideOnDeactivate
        {
            static bool Prefix(AddMirrorImage __instance)
            {

                __instance.OnTurnOff();
                Main.Log.Log($"AddMirrorImage.OnDeactivate prefix firing from {__instance.Buff.Blueprint.name} on {__instance.Context.MainTarget.Unit.CharacterName}");
                __instance.Owner.Ensure<UnitPartMirrorImage>().RemoveImagesFromSource(__instance.Buff);
                if (__instance.Owner.Ensure<UnitPartMirrorImageMultisource>().ConsiderRemove())
                {
                    __instance.Owner.Remove<UnitPartMirrorImage>();
                    __instance.Owner.Remove<UnitPartMirrorImageMultisource>();
                }

                return false;
            }
        }

        [HarmonyPatch(typeof(UnitPartMirrorImage), nameof(UnitPartMirrorImage.Init))]
        class OverrideInit
        {
            static bool Prefix(UnitPartMirrorImage __instance, int imagesCount, Buff source)
            {
                Main.Log.Log($"UnitPartMirrorImage.Init prefix firing on {__instance.Owner.CharacterName} with {imagesCount} images");
                Main.Log.Log($"MirrorImageFix is {(__instance.Fx is not null ? " not " : "")} null before init");
                UnitPartMirrorImageMultisource lib = __instance.Owner.Ensure<UnitPartMirrorImageMultisource>();

                for (int i = 1; i <= imagesCount; i++)
                {
                    
                    int target = lib.Images.Count == 0 ? 1 : lib.Images.Select(x => x.indexThingy).Max() + 1;
                    Main.Log.Log($"UnitPartMirrorImage.Init: adding image at val {target}");
                    __instance.VisualImages.Add(target);
                    __instance.MechanicsImages.Add(target);
                    lib.AddImageFromSource(target, source, false);
                }
                Main.Log.Log($"UnitPartMirrorImage.Init end of call - visualImages {__instance.VisualImages.Count} / mechanicsimages {__instance.MechanicsImages.Count()}");

                return false;

            }

            public static void Postfix(UnitPartMirrorImage __instance, int imagesCount, Buff source)
            {
                Main.Log.Log($"MirrorImageFx is {(__instance.Fx is not null ? " not " : "")} null after init");
            }
        }


        public static int AddIllusoryDuplicate(this UnitPartMirrorImage mirrorImage, Buff source)
        {
            try
            {
                Main.Log.Log($"AddIllusoryDuplicate firing on UnitPartMirrorImage, buff is {(source != null ? source.Blueprint.NameSafe() : "null" )}");
                Main.Log.Log($"AddIllusoryDuplicate firing on UnitPartMirrorImage, Owner is  {(mirrorImage.Owner != null ? mirrorImage.Owner.CharacterName : "null")}");

                UnitPartMirrorImageMultisource lib = mirrorImage.Owner.Ensure<UnitPartMirrorImageMultisource>();
                int target = lib.Images.Count == 0 ? 1 : lib.Images.Select(x => x.indexThingy).Max() + 1;
                Main.Log.Log($"UnitPartMirrorImage.AddIllusoryDuplicate: adding image at val {target}");
                mirrorImage.VisualImages.Add(target);
                mirrorImage.MechanicsImages.Add(target);
                lib.AddImageFromSource(target, source, true);
                if (mirrorImage.Fx != null)
                {
                    mirrorImage.Fx.DeployImage(mirrorImage.Owner.Unit.View, mirrorImage, target);
                }
                else
                {
                    Main.Log.Log($"MirrorImageFx.Update2 should be called but MirrorImage.fx is void");
                }
                return target;
            }
            catch (Exception e)
            {
                Main.Log.Log($"Error on UnitPartMirrorImage.AddIllusoryDuplicate: {e.Message}\n {e.InnerException} \n {e.StackTrace} ");
                return -1;
            }

                    

        }
        public static void TestInit(this MirrorImageFX mirrorImageFX, UnitEntityView unit)
        {
                mirrorImageFX.Init(unit);
        }

        
        public static void DeployImage(this MirrorImageFX mirrorImageFX, UnitEntityView unit, UnitPartMirrorImage unitPartMirrorImage, int slot)
        {

           
            //Above lines setup UnitPart to FX relationship

           
            Dictionary<int, MirrorImageFX.MirrorImageEntry> dict = new();
            for (int i = 1; i <= mirrorImageFX.m_Entries.Count; i++)
            {
                if (!unitPartMirrorImage.VisualImages.Contains(i))
                {
                    dict.Add(i, mirrorImageFX.m_Entries[i - 1]);
                }
            }
            foreach (var imageIndex in dict)
            {
                if (dict.Count <= 0)
                {
                    break;
                }
                int index = UnityEngine.Random.Range(0, dict.Count - 1);
                MirrorImageFX.MirrorImageEntry mirrorImageEntry = dict[index];//Selects a random part of the holder list from m_entries
                mirrorImageEntry.ImageIndex = imageIndex.Key;
                mirrorImageFX.m_ActiveEntries.Add(imageIndex.Value);
                dict.Remove(index);
                mirrorImageEntry.MainFxInstance = FxHelper.SpawnFxOnUnit(mirrorImageEntry.MainFxPrefab, unit, null, null, default(Vector3), FxPriority.Important);
            }
            
        }
        

        public static int ReactivateRenewableImage(this UnitPartMirrorImage mirrorimage, Buff source)
        {
            UnitPartMirrorImageMultisource lib = mirrorimage.Owner.Ensure<UnitPartMirrorImageMultisource>();
            int target = lib.Images.First(x => (x.buff == source && x.active == false))?.indexThingy ?? -1 ;
            if (target != -1)
            {
                ReactivateRenewableImage(mirrorimage, source, target);
            }
            return target;
        }

        public static void ReactivateRenewableImage(this UnitPartMirrorImage mirrorImage, Buff source, int indexThingy)
        {
            UnitPartMirrorImageMultisource lib = mirrorImage.Owner.Ensure<UnitPartMirrorImageMultisource>();
            mirrorImage.VisualImages.Add(indexThingy);
            mirrorImage.MechanicsImages.Add(indexThingy);
            lib.Images.FirstOrDefault(x=>x.indexThingy == indexThingy).active = true;
            if (mirrorImage.Fx != null)
            {
                mirrorImage.Fx.Update2();
            }
            else
            {
                Main.Log.Log($"MirrorImageFx.Update2 called");
            }


        }

        


        [HarmonyPatch(typeof(UnitPartMirrorImage), nameof(UnitPartMirrorImage.TryAbsorbHit))]
        class OverrideTryAbsorbHit
        {
            static bool Prefix(ref int __result, UnitPartMirrorImage __instance, bool force)
            {
                if (__instance.MechanicsImages.Count <= 0)
                {
                    __result = 0;
                    return false;
                }
                int num = force ? __instance.MechanicsImages.Count : UnityEngine.Random.Range(0, __instance.MechanicsImages.Count + 1);
                if (num <= 0)
                {
                    __result = 0;
                    return false;
                }
                __result = __instance.MechanicsImages[num - 1];
                __instance.MechanicsImages.RemoveAt(__result);
                __instance.Owner.Ensure<UnitPartMirrorImageMultisource>().ExpendImage(__result);
                __instance.Owner.Get<UnitPartIllusoryDuplicates>().ExpendImage(__result);
                return false;
            }
        }

        [HarmonyPatch(typeof(UnitPartMirrorImage), nameof(UnitPartMirrorImage.SpendReservedImage))]
        class OverrideSpendReservedImage
        {
            static bool Prefix(UnitPartMirrorImage __instance, int imageIndex)
            {
                if (__instance.Fx != null)
                {
                    __instance.Fx.DestroyImage(imageIndex);
                }
                __instance.VisualImages.Remove(imageIndex);

                return false;
            }
        }



        [HarmonyPatch(typeof(UnitPartMirrorImage), nameof(UnitPartMirrorImage.OnDispose))]
        class OverrideOnDispose
        {
            static bool Prefix(UnitPartMirrorImage __instance)
            {
                __instance.Owner.Ensure<UnitPartMirrorImageMultisource>().Dispose();
                return true;
            }
        }
        
        public static void RemoveImagesFromSource(this UnitPartMirrorImage imagePart, Buff source)
        {
            imagePart.Owner.Ensure<UnitPartMirrorImageMultisource>().RemoveImagesFromSource(source);
        }

        public static void Update2(this MirrorImageFX fx)
        {


            foreach (MirrorImageFX.MirrorImageEntry mirrorImageEntry in fx.m_ActiveEntries)
            {
                if (mirrorImageEntry.MainFxInstance == null)
                {
                    mirrorImageEntry.MainFxInstance = FxHelper.SpawnFxOnUnit(mirrorImageEntry.MainFxPrefab, fx.m_Unit, null, null, default(Vector3), FxPriority.EventuallyImportant);
                }
                if (mirrorImageEntry.MainFxInstance.IsSpawned)
                {
                    if (!mirrorImageEntry.Inited)
                    {
                        mirrorImageEntry.Init();
                    }
                    mirrorImageEntry.Update(fx.m_Unit);
                }
            }
            if (fx.m_MirrorImage == null || fx.m_MirrorImage.VisualImages.Count <= 0)
            {
                FxHelper.Destroy(fx.gameObject, false);
            }
        }

        [HarmonyPatch(typeof(MirrorImageFX), nameof(MirrorImageFX.Init))]
        class OverrideMirrorImageFX
        {
            static void Postfix(MirrorImageFX __instance, UnitEntityView unit)
            {
                Main.Log.Log($"MirrorImageFx.init called on {unit.name}, mirror image pointer {((__instance.m_MirrorImage is not null) ? "found" : "not found")}");
                Main.Log.Log($"{__instance.m_Entries.Count} entries in MirrorImageFx.m_entries");
                Main.Log.Log($"{__instance.m_ActiveEntries.Count} entries in MirrorImageFx.m_ActiveEntries");
               
            }
        }
    }
}
