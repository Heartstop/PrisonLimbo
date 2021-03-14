using System.Diagnostics.Contracts;

namespace PrisonLimbo.Scripts.Extensions
{
    public static class IntExtensions
    {
        [Pure]
        public static ulong SquaredUl(this int value)
        {
            checked
            {
                var unsigned = value < 0 ? (ulong) (value * -1L) : (ulong) value;
                return unsigned * unsigned;
            }
        }
    }
}