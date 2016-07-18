namespace JdiCodeGenerator.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using ObjectModel.Abstract;
    using ObjectModel.Enums;

    public static class ElementMemberCodeEntriesExtensions
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

        static readonly Regex UppercaseTheNextCharacter = new Regex("[^0-9a-zA-Z]+(?<letter>[a-z])", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        static readonly Regex JustRemoveWrongCharacter = new Regex("[^0-9a-zA-Z]+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        // internal static string ToPascalCase(this string wronglyFormattedString)
        public static string ToPascalCase(this string wronglyFormattedString)
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

        // 20160718
        //[Obsolete("Currently this is set in the code entry itself and there's a question about this method")]
        //public static IEnumerable<IPageMemberCodeEntry> SetBestChoice(this IEnumerable<IPageMemberCodeEntry> codeEntries)
        //{
        //    // 20160718
        //    // var entries = codeEntries as IPageMemberCodeEntry[] ?? codeEntries.ToArray();
        //    // var entries = codeEntries.Cast<IPieceOfCode>() ?? codeEntries.ToArray();
        //    var entries = codeEntries as IPieceOfCode[] ?? codeEntries.ToArray();
        //    // 20160718
        //    // entries.ToList().ForEach(codeEntry => codeEntry.Locators.ForEach(locator => locator.IsBestChoice = false));
        //    entries.Where(entry => PiecesOfCodeClasses.PageMember == entry.CodeClass).Cast<IPageMemberCodeEntry>().ToList().ForEach(codeEntry => codeEntry.Locators.ForEach(locator => locator.IsBestChoice = false));
        //    // 20160718
        //    // entries.ToList().ForEach(codeEntry => codeEntry.Locators.OrderBy(locator => (int)locator.SearchTypePreference).First().IsBestChoice = true);
        //    entries.Where(entry => PiecesOfCodeClasses.PageMember == entry.CodeClass).Cast<IPageMemberCodeEntry>()ToList().ForEach(codeEntry => codeEntry.Locators.OrderBy(locator => (int)locator.SearchTypePreference).First().IsBestChoice = true);
        //    return entries;
        //}

        public static IEnumerable<IPageMemberCodeEntry> SetDistinguishNamesForMembers(this IEnumerable<IPageMemberCodeEntry> codeEntries)
        {
            // 20160718
            // var distinguishNamesForMembers = codeEntries as IPageMemberCodeEntry[] ?? codeEntries.ToArray();
            var distinguishNamesForMembers = codeEntries.Where(entry => PiecesOfCodeClasses.PageMember == entry.CodeClass).Cast<IPageMemberCodeEntry>().ToArray();
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
        internal static string GenerateNameBasedOnNamingPreferences(this IPageMemberCodeEntry pageMemberCodeEntry)
        {
            var prefix = pageMemberCodeEntry.JdiMemberType.ToString().Substring(0, 1).ToLower() + pageMemberCodeEntry.JdiMemberType.ToString().Substring(1);
            if (!pageMemberCodeEntry.Locators.Any())
                return prefix + NoName;
            if (pageMemberCodeEntry.Locators.Any(locator => locator.SearchTypePreference == SearchTypePreferences.name))
                return prefix + pageMemberCodeEntry.Locators.First(locator => locator.SearchTypePreference == SearchTypePreferences.name).SearchString.ToPascalCase();

            if (pageMemberCodeEntry.Locators.Any(locator => locator.SearchTypePreference == SearchTypePreferences.id))
                return prefix + pageMemberCodeEntry.Locators.First(locator => locator.SearchTypePreference == SearchTypePreferences.id).SearchString.ToPascalCase();

            if (pageMemberCodeEntry.Locators.Any(locator => locator.SearchTypePreference == SearchTypePreferences.tagName))
                return prefix + pageMemberCodeEntry.Locators.First(locator => locator.SearchTypePreference == SearchTypePreferences.tagName).SearchString.ToPascalCase();

            if (pageMemberCodeEntry.Locators.Any(locator => locator.SearchTypePreference == SearchTypePreferences.linkText))
                return prefix + pageMemberCodeEntry.Locators.First(locator => locator.SearchTypePreference == SearchTypePreferences.linkText).SearchString.ToPascalCase();

            if (pageMemberCodeEntry.Locators.Any(locator => locator.SearchTypePreference == SearchTypePreferences.className))
                return prefix + pageMemberCodeEntry.Locators.First(locator => locator.SearchTypePreference == SearchTypePreferences.className).SearchString.ToPascalCase();

            return prefix + NoName;
        }
    }
}