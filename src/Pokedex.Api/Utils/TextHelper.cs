using System.Text.RegularExpressions;

namespace Pokedex.Api.Utils
{
    public class TextHelper
    {
        /// <summary>
        ///     Replaces new line characters with given replacement. Defaults to single whitespace.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string ReplaceNewLinesWith(string content, string replacement = " ")
            => Regex.Replace(content, @"\t|\n|\r", replacement);
    }
}