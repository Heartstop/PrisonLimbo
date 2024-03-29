using System;
using System.Runtime.CompilerServices;

namespace PrisonLimbo.Scripts.Extensions
{
    public static class RandomExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NextBool(this Random random) => random.NextDouble() < 0.5d;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NextBool(this Random random, double trueChance) => random.NextDouble() < trueChance;
    }
}