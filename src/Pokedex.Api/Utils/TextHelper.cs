using System.Text.RegularExpressions;

namespace Pokedex.Api.Utils
{
    public class TextHelper
    {
        /// <summary>
        ///     Replaces character escapes with given replacement. Defaults to single whitespace.
        ///     Character Escapes: \t, \n, \r, \f, \e, \v, \a
        /// </summary>
        /// <param name="content"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string ReplaceCharacterEscapesWith(string content, string replacement = " ")
            => Regex.Replace(content, @"\t|\n|\r|\f|\\e|\v|\a", replacement);
    }
}