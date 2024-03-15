using RimWorld;
using Verse;

namespace BiomesOasis
{
	public class CompProperties_MultiHatcher : CompProperties_Hatcher
	{
		public IntRange numToHatch = new IntRange(3, 5);

		public CompProperties_MultiHatcher()
		{
			compClass = typeof(CompMultiHatcher);
		}
	}
}