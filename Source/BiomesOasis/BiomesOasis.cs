using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using System;
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
