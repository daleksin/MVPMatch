using System;

namespace MVPMatch.Utils
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCaseSafe(this string valueA, string valueB)
        {
            return valueA?.Equals(valueB, StringComparison.InvariantCultureIgnoreCase)
                   ?? valueB is null;
        }
    }
}
