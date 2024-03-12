using System;
using BiomesCore.Planet;
using BiomesCore.WorldMap;
using RimWorld;
using RimWorld.Planet;

namespace BiomesOasis.Planet
{
	/// <summary>
	/// Chromatic oasis is placed on world tiles that contain desert or extreme desert, and match the criteria in the
	/// WorldGenInfo_OasisPresence instance.
	/// </summary>
	public class BiomeWorker_Oasis : BiomeWorker
	{
		public override float GetScore(Tile tile, int tileID)
		{
			if (tile.WaterCovered)
			{
				return 0.0F;
			}

			float desertScore = Math.Max(BiomeWorkerUtil.DesertScore(tile), BiomeWorkerUtil.ExtremeDesertScore(tile));
			return desertScore > 0.0F
				? desertScore + WorldGenInfoHandler.Get<WorldGenInfo_OasisPresence>().GetValue(tileID)
				: 0.0F;
		}
	}
}