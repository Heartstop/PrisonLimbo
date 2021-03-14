using System;
using System.Collections.Generic;
using System.Linq;

namespace PrisonLimbo.Scripts.Extensions
{
    public static class CollectionExtensions
    {
        public static TA GetRandom<TA>(this ICollection<TA> source, Random randomSource)
        {
            var selectedIndex = randomSource.Next(0, source.Count);
            return source.Skip(selectedIndex).First();
        }
    }
}