using HarmonyLib;
using System.Text;
using System.Reflection;
using UnityModManagerNet;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.JsonSystem;
using PhoenixKineticistElements.ElementLight;
using TabletopTweaks.Core.Config;

namespace PhoenixKineticistElements;

public static class Main {
    internal static Harmony HarmonyInstance;
    internal static UnityModManager.ModEntry.ModLogger Log;
    
    public static PKEModContext LocalPKEModContext;
    public static bool Load(UnityModManager.ModEntry modEntry) {
        Log = modEntry.Logger;
        modEntry.OnGUI = OnGUI;
        HarmonyInstance = new Harmony(modEntry.Info.Id);
        try {
            LocalPKEModContext = new PKEModContext(modEntry);
            LocalPKEModContext.ModEntry.OnSaveGUI = OnSaveGUI;
            LocalPKEModContext.ModEntry.OnGUI = OnGUI;
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
           
        } catch {
            HarmonyInstance.UnpatchAll(HarmonyInstance.Id);
            throw;
        }
        return true;
    }

    private static void OnSaveGUI(UnityModManager.ModEntry entry)
    {
        
    }

    public static void OnGUI(UnityModManager.ModEntry modEntry) {

    }

    [HarmonyPatch(typeof(BlueprintsCache))]
    public static class BlueprintsCaches_Patch {
        private static bool Initialized = false;

        [HarmonyPriority(Priority.First)]
        [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
        public static void Init_Postfix() {
            try {
                if (Initialized) {
                    Log.Log("Already initialized blueprints cache.");
                    return;
                }
                Initialized = true;
                LocalizationTool.LoadEmbeddedLocalizationPacks(
                  "PhoenixKineticistElements.Localization.Kinetics.json",
                  "PhoenixKineticistElements.Kinetics.json"
                  );
                Log.Log("Patching blueprints.");
                // Insert your mod's patching methods here
                // Example
                // SuperAwesomeFeat.Configure()
                Light.Configure();
                BasicKinesis.MakeMain();
            } catch (Exception e) {
                Log.Log(string.Concat("Failed to initialize.", e));
            }
        }
    }

    [HarmonyPatch(typeof(BlueprintsCache))]
    public static class BlueprintsCaches_Patch2
    {
        private static bool Initialized = false;

        [HarmonyPriority(Priority.Last)]
        [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
        public static void Init_Postfix()
        {
            try
            {
                BasicKinesis.Patch();
            }
            catch (Exception e)
            {
                Log.Log(string.Concat("Failed to initialize Late Patch.", e));
            }
        }
    }

    private static bool? DarkCodexInstalled;
    public static bool IsDarkCodexInstalled()
    {
        return DarkCodexInstalled ??= UnityModManager.modEntries.Exists(x => x.Info.Id.Equals("DarkCodex"));
    }

    private static bool? KineticistElementsExpandedInstalled;
    public static bool IsKineticistElementsExpandedInstalled()
    {
        return KineticistElementsExpandedInstalled ??= UnityModManager.modEntries.Exists(x => x.Info.Id.Equals("KineticistElementsExpanded"));
    }

    private static bool? KineticistArchetypesInstalled;
    public static bool IsKineticistArchetypesInstalled()
    {
        return KineticistArchetypesInstalled ??= UnityModManager.modEntries.Exists(x => x.Info.Id.Equals("KineticArchetypes"));
    }
}
