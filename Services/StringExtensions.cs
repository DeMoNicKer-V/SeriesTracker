using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeNamespace
{
    internal static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static bool ContainsCaseInsensitive(this string source, string substring)
        {
            return source.Contains(substring, StringComparison.OrdinalIgnoreCase);
        }
    }
}
