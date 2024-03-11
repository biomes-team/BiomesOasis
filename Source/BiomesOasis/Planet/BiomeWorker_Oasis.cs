using RimWorld;
using RimWorld.Planet;

namespace BiomesOasis.Planet
{
	public class BiomeWorker_Oasis : BiomeWorker
	{
		public override float GetScore(Tile tile, int tileID)
		{
			if (tile.WaterCovered)
			{
				return 0.0F;
			}

			return 1.0F;
		}
	}
}