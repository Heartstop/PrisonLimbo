using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        [Pure]
        public static ImmutableQueue<T> ToImmutableQueue<T>(this IEnumerable<T> source)
            => ImmutableQueue.CreateRange(source);

        [Pure]
        public static Queue<T> ToQueue<T>(this IEnumerable<T> source) => new Queue<T>(source);
    }
}