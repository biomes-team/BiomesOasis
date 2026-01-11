using System;
using BiomesCore.Planet;
using BiomesCore.WorldMap;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace BiomesOasis.Planet
{
	/// <summary>
	/// Chromatic oasis is placed on world tiles that contain desert or extreme desert, and match the criteria in the
	/// WorldGenInfo_OasisPresence instance.
	/// </summary>
	public class BiomeWorker_Oasis : BiomeWorker
	{
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			//basic filters
			if (tile.WaterCovered)
			{
				return 0f;
			}
			if (tile.elevation < 55f)
			{
				return 0f;
			}
			//if (tile.hilliness >= Hilliness.Mountainous)
			//{
			//	return 0f;
			//}

			//this should make it only spawn on desert tiles, I think
			float desertScore = Math.Max(BiomeWorkerUtil.DesertScore(tile), BiomeWorkerUtil.ExtremeDesertScore(tile));
			if (desertScore <= 0f)
			{
				return 0f;
			}

			if(Rand.Value  > 0.003f)
			{
				return 0f;
			}

			return 100f;

		}
	}
}