using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PrisonLimbo.Scripts.Extensions
{
    public static class EnumerableExtensions
    {
        [Pure]
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random random)
        {
            var buffer = source.ToArray();
            for (var i = 0; i < buffer.Length - 2; i++)
            {
                var r = random.Next(i, buffer.Length);
                var v = buffer[i];
                buffer[i] = buffer[r];
                buffer[r] = v;
            }

            return buffer;
        }
    }
}