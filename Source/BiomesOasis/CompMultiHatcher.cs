using RimWorld;
using RimWorld.Planet;
using Verse;

namespace BiomesOasis
{
	public class CompMultiHatcher : ThingComp
	{
		private float gestateProgress;

		public Pawn hatcheeParent;

		public Pawn otherParent;

		public Faction hatcheeFaction;

		public CompProperties_MultiHatcher Props => (CompProperties_MultiHatcher) props;

		private CompTemperatureRuinable FreezerComp => parent.GetComp<CompTemperatureRuinable>();

		public bool TemperatureDamaged
		{
			get
			{
				if (FreezerComp != null)
				{
					return FreezerComp.Ruined;
				}

				return false;
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look(ref gestateProgress, "gestateProgress");
			Scribe_References.Look(ref hatcheeParent, "hatcheeParent");
			Scribe_References.Look(ref otherParent, "otherParent");
			Scribe_References.Look(ref hatcheeFaction, "hatcheeFaction");
		}

		public override void CompTick()
		{
			if (!TemperatureDamaged)
			{
				float num = 1f / (Props.hatcherDaystoHatch * 60000f);
				gestateProgress += num;
				if (gestateProgress > 1f)
				{
					Hatch();
				}
			}
		}

		public void Hatch()
		{
			try
			{
				PawnGenerationRequest request = new PawnGenerationRequest(Props.hatcherPawn, hatcheeFaction,
					PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: false,
					developmentalStages: DevelopmentalStage.Newborn);
				int numToHatch = Props.numToHatch.RandomInRange;


				for (int i = 0; i < numToHatch; i++)
				{
					Pawn pawn = PawnGenerator.GeneratePawn(request);
					if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, parent))
					{
						if (pawn != null)
						{
							if (hatcheeParent != null)
							{
								if (pawn.playerSettings != null && hatcheeParent.playerSettings != null &&
								    hatcheeParent.Faction == hatcheeFaction)
								{
									pawn.playerSettings.AreaRestrictionInPawnCurrentMap =
										hatcheeParent.playerSettings.AreaRestrictionInPawnCurrentMap;
								}

								if (pawn.RaceProps.IsFlesh)
								{
									pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, hatcheeParent);
								}
							}

							if (otherParent != null && (hatcheeParent == null || hatcheeParent.gender != otherParent.gender) &&
							    pawn.RaceProps.IsFlesh)
							{
								pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, otherParent);
							}
						}

						if (parent.Spawned)
						{
							FilthMaker.TryMakeFilth(parent.Position, parent.Map, ThingDefOf.Filth_AmnioticFluid);
						}
					}
					else
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
				}
			}
			finally
			{
				parent.Destroy();
			}
		}

		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = count / (float) (parent.stackCount + count);
			float b = ((ThingWithComps) otherStack).GetComp<CompMultiHatcher>().gestateProgress;
			gestateProgress = UnityEngine.Mathf.Lerp(gestateProgress, b, t);
		}

		public override void PostSplitOff(Thing piece)
		{
			CompMultiHatcher comp = ((ThingWithComps) piece).GetComp<CompMultiHatcher>();
			comp.gestateProgress = gestateProgress;
			comp.hatcheeParent = hatcheeParent;
			comp.otherParent = otherParent;
			comp.hatcheeFaction = hatcheeFaction;
		}

		public override void PrePreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			base.PrePreTraded(action, playerNegotiator, trader);
			switch (action)
			{
				case TradeAction.PlayerBuys:
					hatcheeFaction = Faction.OfPlayer;
					break;
				case TradeAction.PlayerSells:
					hatcheeFaction = trader.Faction;
					break;
			}
		}

		public override void PostPostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
		{
			base.PostPostGeneratedForTrader(trader, forTile, forFaction);
			hatcheeFaction = forFaction;
		}

		public override string CompInspectStringExtra()
		{
			if (!TemperatureDamaged)
			{
				return "EggProgress".Translate() + ": " + gestateProgress.ToStringPercent();
			}

			return null;
		}
	}
}