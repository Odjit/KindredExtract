using HarmonyLib;
using ProjectM;
using Stunlock.Network;
using Unity.Scenes;


namespace KindredExtract.Patches;

[HarmonyPatch(typeof(SceneSectionStreamingSystem), nameof(SceneSectionStreamingSystem.ShutdownAsynchrnonousStreamingSupport))]
public static class InitializationPatch
{
	[HarmonyPostfix]
	public static void OneShot_AfterLoad_InitializationPatch()
	{
		Core.InitializeAfterLoaded();
		Plugin.Harmony.Unpatch(typeof(SceneSectionStreamingSystem).GetMethod("ShutdownAsynchrnonousStreamingSupport"), typeof(InitializationPatch).GetMethod("OneShot_AfterLoad_InitializationPatch"));
	}
}
