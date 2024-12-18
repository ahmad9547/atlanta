using System.Text.RegularExpressions;

namespace Core.Helpers
{
    public static class StringHelpers
    {
        /// <summary>
        /// Returns string splited with spaces by capital letters
        /// </summary>
        /// <param name="originalString"></param>
        /// <returns></returns>
        public static string GetSpaceSplitedString(string originalString)
        {
            return Regex.Replace(originalString, @"\B[A-Z]", match => " " + match);
        }

        public static string GetDashSplitedString(string originalString)
        {
            return originalString.Replace('_', ' ');
        }
    }
}