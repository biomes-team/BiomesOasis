using BiomesCore.Planet;
using BiomesOasis.Planet;
using HarmonyLib;
using Verse;

namespace BiomesOasis
{
	[StaticConstructorOnStartup]
	public static class BiomesOasis
	{
		static BiomesOasis()
		{
			new Harmony("biomesoasis").PatchAll();
			// Register WorldGenInfos in Biomes! Core.
			WorldGenInfoHandler.Register<WorldGenInfo_OasisPresence>();
		}
	}
}