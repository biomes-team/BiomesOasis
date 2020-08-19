using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using Verse.Noise;
using RimWorld.Planet;
using UnityEngine;
using BiomesCore.DefModExtensions;

namespace BiomesOasis.GenSteps
{
    public class GenStep_OasisB : GenStep
    {
        public override int SeedPart
        {
            get
            {
                return 1449027791;
            }
        }

        float oasisSize = Rand.Range(30f, 70f);
        float beachSize = Rand.Range(30f, 50f);
        float distanceVariance = Rand.Range(1.0f, 1.5f);
        float perlinVariance = 5f;
        bool isIsland;
        List<int> tmpNeighbors = new List<int>();
        public override void Generate(Map map, GenStepParams parms)
        {
            if (!map.Biome.HasModExtension<BiomesMap>())
            {
                return;
            }
            if (!map.Biome.GetModExtension<BiomesMap>().isOasis)
            {
                return;
            }
            MapGenFloatGrid oasisGrid = MapGenerator.FloatGridNamed("OasisGrid");
            IntVec3 oasisCenter = map.Center;
            ModuleBase moduleBase = new Perlin(Rand.Range(0.015f, 0.028f), 2.0, 0.5, 6, Rand.Range(0, 2147483647), QualityMode.Medium);
            if (oasisSize >= 50f)
            {
                perlinVariance = 6f;
            }
            WorldGrid grid = Find.World.grid;
            int tileID = map.Tile;
            grid.GetTileNeighbors(tileID, tmpNeighbors);
            for (int i = 0; i < tmpNeighbors.Count; i++)
            {
                if (grid[tmpNeighbors[i]].biome != BiomeDefOf.Ocean)
                {
                    isIsland = false;
                }
            }
            Rot4 beachDirection = Find.World.CoastDirectionAt(map.Tile);
            if (isIsland == true)
            {
                oasisSize = Rand.Range(20f, 30f);
                perlinVariance = 4f;
            }
            else
            {
                if(beachDirection != null)
                {
                    // If it has a beach, the oasis is smaller with reduced variance
                    // Move the center of the oasis away from the beach
                    oasisSize = Rand.Range(20f, 30f);
                    perlinVariance = 4f;
                    if (beachDirection == Rot4.North)
                    {
                        oasisCenter.z -= 10;
                    }
                    else if (beachDirection == Rot4.South)
                    {
                        oasisCenter.z += 10;
                    }
                    else if (beachDirection == Rot4.East)
                    {
                        oasisCenter.x -= 10;
                    }
                    else if (beachDirection == Rot4.West)
                    {
                        oasisCenter.x += 10;
                    }
                }
            }
            
            MapGenFloatGrid elevation = MapGenerator.Elevation;
            MapGenFloatGrid fertility = MapGenerator.Fertility;

            foreach (IntVec3 current in map.AllCells)
            {
                elevation[current] = elevation[current] * 0.75f;
                float distance = DistanceBetweenPoints(current, oasisCenter) / 1.5f;
                fertility[current] = ((moduleBase.GetValue(current) * perlinVariance) + 0.1f * ((oasisSize) - (distance * distanceVariance))) - (elevation[current] * 11);
            }


        }


        private float DistanceBetweenPoints(IntVec3 point1, IntVec3 point2)
        {
            float dist = 0;
            double xDist = Math.Pow(point1.x - point2.x, 2);
            double zDist = Math.Pow(point1.z - point2.z, 2);
            dist = (float)Math.Sqrt(xDist + zDist);

            return dist;
        }
    }
}