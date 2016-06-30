namespace JdiCodeGenerator.Core.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using ObjectModel.Abstract;

    public static class ExtensionMethodsForCodeEntries
    {
        static readonly List<char> WrongCharactersSmallList = new List<char> {
            // ' ',
            '-',
            '.',
            // '/',
            '#',
            ',',
            // '\\',
            '*',
            ':',
            '@',
            // '(',
            // ')',
            // '[',
            // ']',
            '?',
            '=',
            '&',
            '+',
            '%',
            // '\r',
            // '\n'
            ';'
            };
        public static string CleanUpFromWrongCharacters(this string originalString)
        {
            // ??
            if (string.IsNullOrEmpty(originalString))
                return string.Empty;

            // 20160625
            // this worked but was non-standard
            //WrongCharactersSmallList.ForEach(character =>
            //{
            //    if (originalString.Contains(character))
            //        originalString = originalString.Replace(character.ToString(), string.Empty);
            //});
            originalString = JustRemoveWrongCharacter.Replace(originalString, string.Empty);
            return originalString;
        }

        static Regex UppercaseTheNextCharacter = new Regex("[^0-9a-zA-Z]+(?<letter>[a-z])", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        static Regex JustRemoveWrongCharacter = new Regex("[^0-9a-zA-Z]+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        internal static string ToPascalCase(this string wronglyFormattedString)
        {
            if (string.IsNullOrEmpty(wronglyFormattedString))
                return NoName;

            var firstCharacter = (int)wronglyFormattedString[0];
            wronglyFormattedString = ((char)firstCharacter).ToString().ToUpper() + wronglyFormattedString.Substring(1);

            wronglyFormattedString = UppercaseTheNextCharacter.Replace(wronglyFormattedString, m => m.ToString().ToUpper());
            wronglyFormattedString = JustRemoveWrongCharacter.Replace(wronglyFormattedString, string.Empty);

            if (string.IsNullOrEmpty(wronglyFormattedString))
                return NoName;

            return wronglyFormattedString;
        }

        public static IEnumerable<ICodeEntry<T>> SetBestChoice<T>(this IEnumerable<ICodeEntry<T>> codeEntries)
        {
            var entries = codeEntries as ICodeEntry<T>[] ?? codeEntries.ToArray();
            entries.ToList().ForEach(codeEntry => codeEntry.Locators.ForEach(locator => locator.IsBestChoice = false));
            entries.ToList().ForEach(codeEntry => codeEntry.Locators.OrderBy(locator => (int)locator.SearchTypePreference).First().IsBestChoice = true);
            return entries;
        }

        public static IEnumerable<ICodeEntry<T>> SetDistinguishNamesForMembers<T>(this IEnumerable<ICodeEntry<T>> codeEntries)
        {
            var distinguishNamesForMembers = codeEntries as ICodeEntry<T>[] ?? codeEntries.ToArray();
            distinguishNamesForMembers.ToList().ForEach(codeEntry => codeEntry.MemberName = codeEntry.GenerateNameBasedOnNamingPreferences());
            distinguishNamesForMembers
                .GroupBy(codeEntryName => codeEntryName.MemberName)
                .Select(grouping =>
                {
                    var i = 0;
                    grouping.ToList().Skip(1).ToList().ForEach(item => { item.MemberName += ++i; });
                    return grouping;
                }).SelectMany(grouping => grouping.Select(item => item))
                .ToList();
            return distinguishNamesForMembers;
        }

        /*
        title,
        name,
        id,
        tagName,
        linkText,
        className,
        */

        internal const string NoName = "NoName";
        internal static string GenerateNameBasedOnNamingPreferences<T>(this ICodeEntry<T> codeEntry)
        {
            var prefix = codeEntry.JdiMemberType.ToString().Substring(0, 1).ToLower() + codeEntry.JdiMemberType.ToString().Substring(1);
            if (!codeEntry.Locators.Any())
                return prefix + NoName;
            if (codeEntry.Locators.Any(locator => locator.SearchTypePreference == SearchTypePreferences.name))
                return prefix + codeEntry.Locators.First(locator => locator.SearchTypePreference == SearchTypePreferences.name).SearchString.ToPascalCase();

            if (codeEntry.Locators.Any(locator => locator.SearchTypePreference == SearchTypePreferences.id))
                return prefix + codeEntry.Locators.First(locator => locator.SearchTypePreference == SearchTypePreferences.id).SearchString.ToPascalCase();

            if (codeEntry.Locators.Any(locator => locator.SearchTypePreference == SearchTypePreferences.tagName))
                return prefix + codeEntry.Locators.First(locator => locator.SearchTypePreference == SearchTypePreferences.tagName).SearchString.ToPascalCase();

            if (codeEntry.Locators.Any(locator => locator.SearchTypePreference == SearchTypePreferences.linkText))
                return prefix + codeEntry.Locators.First(locator => locator.SearchTypePreference == SearchTypePreferences.linkText).SearchString.ToPascalCase();

            if (codeEntry.Locators.Any(locator => locator.SearchTypePreference == SearchTypePreferences.className))
                return prefix + codeEntry.Locators.First(locator => locator.SearchTypePreference == SearchTypePreferences.className).SearchString.ToPascalCase();

            return prefix + NoName;
        }
    }
}