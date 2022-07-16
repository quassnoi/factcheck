using System.Text;
using System.Text.RegularExpressions;

namespace FactCheck.Common
{
    public static class Converters
    {
        private static readonly Regex AllCapsRegex = new(@"^\p{Lu}+$", RegexOptions.Compiled);
        public static string CodeToText(string code)
            => string.Join(" ",
                SplitCode(code)
                .Select((chunk, index) => AllCapsRegex.IsMatch(chunk) ?
                        chunk :
                        index == 0 ?
                            System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(chunk) :
                            System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToLower(chunk))
                );

        public static IEnumerable<string> SplitCode(string code)
        {
            StringBuilder? builder = null;

            for (var i = 0; i < code.Length; i++)
            {
                var oldBuilder = builder;
                builder = ProcessPosition(code, i, oldBuilder);
                if (oldBuilder != builder && oldBuilder != null)
                {
                    yield return oldBuilder.ToString();
                }
            }
            if (builder != null)
            {
                yield return builder.ToString();
            }
        }

        private static StringBuilder? ProcessPosition(string code, int position, StringBuilder? builder)
            => (GetWordState(code, position) switch
            {
                WordState.Outside => null,
                WordState.Initial => new StringBuilder(),
                WordState.Inside => builder,
                _ => throw new NotImplementedException()
            })?.Append(code[position]);

        private enum CharClass
        {
            Other,
            Lower,
            Upper,
            Digit
        }

        private enum WordState
        {
            Outside,
            Initial,
            Inside
        }

        private static CharClass GetCharClass(char character)
        {
            if (char.IsUpper(character))
            {
                return CharClass.Upper;
            }

            if (char.IsLower(character))
            {
                return CharClass.Lower;
            }

            if (char.IsDigit(character))
            {
                return CharClass.Digit;
            }

            return CharClass.Other;
        }

        private static WordState GetWordState(string word, int position)
        {
            var slice = (position > 0 ? GetCharClass(word[position - 1]) : CharClass.Other,
                        GetCharClass(word[position]),
                        position < (word.Length - 1) ? GetCharClass(word[position + 1]) : CharClass.Other);
            return slice switch
            {
                (CharClass.Upper, CharClass.Upper, CharClass.Lower) => WordState.Initial,
                (CharClass.Upper, CharClass.Upper, _) => WordState.Inside,
                (CharClass.Upper, CharClass.Lower, _) => WordState.Inside,
                (CharClass.Lower, CharClass.Lower, _) => WordState.Inside,
                (CharClass.Digit, CharClass.Digit, _) => WordState.Inside,
                (_, CharClass.Upper, _) => WordState.Initial,
                (_, CharClass.Lower, _) => WordState.Initial,
                (_, CharClass.Digit, _) => WordState.Initial,
                _ => WordState.Outside
            };
        }
    }
}
