using System;
using HarmonyLib;
using System.Reflection;
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
