using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using KindredExtract.Models;
using ProjectM;
using VampireCommandFramework;

namespace KindredExtract;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("gg.deca.VampireCommandFramework")]
public class Plugin : BasePlugin
{
    public static Harmony Harmony => _harmony;
    static Harmony _harmony;
    public static ManualLogSource LogInstance { get; private set; }
    public static Database Settings { get; private set; }

    public override void Load()
    {
        // Plugin startup logic
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} is loaded!");
        LogInstance = Log;
        Settings = new(Config);
        Settings.InitConfig();


        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        // Harmony patching
        _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        Log.LogInfo("Harmony Patching");
        _harmony.PatchAll(assembly);

        // Register all commands in the assembly with VCF
        Log.LogInfo("Registering commands");
        CommandRegistry.RegisterAll(assembly);
    }

    public override bool Unload()
    {
        CommandRegistry.UnregisterAssembly();
        _harmony?.UnpatchSelf();
        return true;
    }
    public void OnGameInitialized()
    {

        if (!HasLoaded())
        {
            Log.LogDebug("Attempt to initialize before everything has loaded.");
            return;
        }

        Core.InitializeAfterLoaded();
    }
    private static bool HasLoaded()
    {
        // Hack, check to make sure that entities loaded enough because this function
        // will be called when the plugin is first loaded, when this will return 0
        // but also during reload when there is data to initialize with.
        var collectionSystem = Core.Server.GetExistingSystemManaged<PrefabCollectionSystem>();
        return collectionSystem?.SpawnableNameToPrefabGuidDictionary.Count > 0;
    }
    // // Uncomment for example commmand or delete

    // /// <summary> 
    // /// Example VCF command that demonstrated default values and primitive types
    // /// Visit https://github.com/decaprime/VampireCommandFramework for more info 
    // /// </summary>
    // /// <remarks>
    // /// How you could call this command from chat:
    // ///
    // /// .kindredextract-example "some quoted string" 1 1.5
    // /// .kindredextract-example boop 21232
    // /// .kindredextract-example boop-boop
    // ///</remarks>
    // [Command("kindredextract-example", description: "Example command from kindredextract", adminOnly: true)]
    // public void ExampleCommand(ICommandContext ctx, string someString, int num = 5, float num2 = 1.5f)
    // { 
    //     ctx.Reply($"You passed in {someString} and {num} and {num2}");
    // }
}
