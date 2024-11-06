using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Extensions
{
    public static class DictionaryExtension
    {
        public static void ValidateEnumDictionary<TE, TV>(this Dictionary<TE, TV> dictionary) where TE : Enum
        {
            Type enumType = typeof(TE);
            if (!Enum.GetValues(enumType).OfType<TE>().Except(dictionary.Keys).Count().Equals(0))
            {
                throw new KeyNotFoundException($"Not all {enumType} values are used in {dictionary} dictionary");
            }
        }
    }
}