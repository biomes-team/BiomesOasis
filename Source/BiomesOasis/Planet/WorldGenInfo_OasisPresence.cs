using BiomesCore.Planet;
using BiomesCore.WorldMap;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace BiomesOasis.Planet
{
	/// <summary>
	/// Random value for each world map tile. The perlin noise generating the value is distributed to have solitary
	/// peaks always surrounded by valleys. Peaks tend to appear in clusters. This value is either 0 or 1.
	/// </summary>
	public class WorldGenInfo_OasisPresence : WorldGenInfo
	{
		/// <summary>
		/// Responsible for clustering oases together.
		/// </summary>
		private ModuleBase _clusterNoise;

		/// <summary>
		/// Determines positions of oases.
		/// </summary>
		private ModuleBase _presenceNoise;

		protected override void Setup()
		{
			int worldSeed = WorldGenInfoHandler.WorldSeed;
			_clusterNoise = new Perlin(0.55, 2.0, 0.4, 6, Gen.HashCombineInt(worldSeed, 1588320896), QualityMode.High);
			NoiseDebugUI.StorePlanetNoise(_clusterNoise, "BMT_Oasis_ClusterNoise");
			_presenceNoise = new Perlin(0.55, 2.0, 0.4, 6, Gen.HashCombineInt(worldSeed, -1780198374), QualityMode.High);
			NoiseDebugUI.StorePlanetNoise(_presenceNoise, "BMT_Oasis_PresenceNoise");
		}

		private bool CanHaveOasis(Tile tile, int tileID, Vector3 tileCenter)
		{
			// Determines how many oases end up being present on the map.
			const float oasisFrequency = 0.35F;
			// Minimum elevation for oases.
			const float elevationThreshold = 55.0F;
			// Oasis position threshold.
			const float presenceThreshold = 0.5F;
			// Makes oases cluster together. Also affects oases frequency.
			const float clusterThreshold = 0.7F;

			return !tile.WaterCovered && tile.hilliness <= Hilliness.SmallHills &&
			       WorldGenInfoHandler.NoiseElevation.GetValue(tileCenter) > elevationThreshold &&
			       _presenceNoise.GetValue(tileCenter) > presenceThreshold &&
			       _clusterNoise.GetValue(tileCenter) > clusterThreshold &&
			       Rand.ChanceSeeded(oasisFrequency, Gen.HashCombineInt(WorldGenInfoHandler.WorldSeed, tileID));
		}

		protected override float GenerateTileData(Tile tile, int tileID, Vector3 tileCenter)
		{
			return CanHaveOasis(tile, tileID, tileCenter)
				? 1.0F
				: 0.0F;
		}
	}
}