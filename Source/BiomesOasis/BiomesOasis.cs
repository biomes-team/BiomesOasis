using HarmonyLib;
using Verse;

namespace BiomesOasis
{
    [StaticConstructorOnStartup]
    static class BiomesOasis
    {
        static BiomesOasis()
        {
            new Harmony("biomesoasis").PatchAll();
        }
    }
}
