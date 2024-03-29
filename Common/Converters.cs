﻿using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace FactCheck.Common;

public static class Converters
{
    private static readonly Regex AllCapsRegex = new(@"^\p{Lu}+$", RegexOptions.Compiled);
    private static readonly Regex SplitTextRegex = new(@"([\p{L}\p{Nl}\p{Mn}\p{Mc}\p{Cf}]+|\p{Nd}+)", RegexOptions.Compiled);
    private static readonly Regex ValidIdentifierRegex = new(@"^[\p{L}\p{Nl}_][\p{L}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Nd}]*$", RegexOptions.Compiled);
    private static readonly Regex SeparatorRegex = new(@"(\W|\p{Pc})+", RegexOptions.Compiled);

    public static string CodeToText(string code)
        => string.Join(" ",
            SplitCode(code)
                .Select((chunk, index) => AllCapsRegex.IsMatch(chunk) ? chunk :
                    index == 0 ? CultureInfo.InvariantCulture.TextInfo.ToTitleCase(chunk) :
                    CultureInfo.InvariantCulture.TextInfo.ToLower(chunk))
        );

    public static string? TextToCode(string text)
    {
        var code = string.Join(
            string.Empty,
            SplitText(text)
                .Select(chunk => char.ToUpperInvariant(chunk[0]) + chunk.Substring(1))
        );
        return ValidIdentifierRegex.IsMatch(code) ? code : null;
    }

    public static bool TextEqualsCode(string text, string code)
        => TextToCode(text)?.Equals(SeparatorRegex.Replace(code, string.Empty), StringComparison.InvariantCultureIgnoreCase) ?? false;

    public static IEnumerable<string> SplitText(string text)
        => SplitTextRegex.Matches(text).Cast<Match>().Select(match => match.Value);

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
            position < word.Length - 1 ? GetCharClass(word[position + 1]) : CharClass.Other);
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
}