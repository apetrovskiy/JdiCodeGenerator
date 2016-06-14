namespace JdiCodeGenerator.Core.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
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

            WrongCharactersSmallList.ForEach(character =>
            {
                if (originalString.Contains(character))
                    originalString = originalString.Replace(character.ToString(), string.Empty);
            });
            return originalString;
        }

        static readonly List<char> WrongCharacters = new List<char> {
            ' ',
            '-',
            '.',
            '/',
            '#',
            ',',
            '\\',
            '*',
            ':',
            '@',
            '(',
            ')',
            '[',
            ']',
            '?',
            '=',
            '&',
            '+',
            '%',
            '\r',
            '\n',
            ';',
            '!',
            '™',
            '<',
            '>',
            '©',
            '‹',
            '\'',
            '™'
            };
        internal static string ToPascalCase(this string wronglyFormattedString)
        {
            if (string.IsNullOrEmpty(wronglyFormattedString))
                return NoName;

            const int lowerA = 97;
            const int lowerZ = 122;
            const int differenceBetweenUpperAndLower = 32;

            WrongCharacters.ForEach(character =>
            {
                if (0 == wronglyFormattedString.Length) return;
                var firstCharacter = (int) wronglyFormattedString[0];
                if (firstCharacter >= lowerA && firstCharacter <= lowerZ)
                    wronglyFormattedString = ((char) firstCharacter).ToString().ToUpper() +
                                             wronglyFormattedString.Substring(1);

                var charPosition = wronglyFormattedString.IndexOf(character);

                if (charPosition == wronglyFormattedString.Length - 1)
                    wronglyFormattedString = wronglyFormattedString.Replace(character.ToString(), string.Empty);

                while (charPosition >= 0)
                {
                    if (charPosition < wronglyFormattedString.Length - 1)
                    {
                        var charToCapitalize = (int) wronglyFormattedString.ElementAt(charPosition + 1);
                        if (charToCapitalize >= lowerA && charToCapitalize <= lowerZ)
                            charToCapitalize -= differenceBetweenUpperAndLower;
                        wronglyFormattedString = wronglyFormattedString.Substring(0, charPosition) +
                                                 (char) charToCapitalize +
                                                 wronglyFormattedString.Substring(charPosition + 2);
                    }
                    charPosition = wronglyFormattedString.IndexOf(character);
                    if (charPosition == wronglyFormattedString.Length - 1)
                    {
                        wronglyFormattedString = wronglyFormattedString.Replace(character.ToString(), string.Empty);
                    }

                    charPosition = wronglyFormattedString.IndexOf(character);
                }
            });

            if (string.IsNullOrEmpty(wronglyFormattedString))
                return NoName;

            return wronglyFormattedString;
        }

        public static IEnumerable<ICodeEntry> SetBestChoice(this IEnumerable<ICodeEntry> codeEntries)
        {
            var entries = codeEntries as ICodeEntry[] ?? codeEntries.ToArray();
            entries.ToList().ForEach(codeEntry => codeEntry.Locators.ForEach(locator => locator.IsBestChoice = false));
            entries.ToList().ForEach(codeEntry => codeEntry.Locators.OrderBy(locator => (int)locator.SearchTypePreference).First().IsBestChoice = true);
            return entries;
        }

        public static IEnumerable<ICodeEntry> SetDistinguishNamesForMembers(this IEnumerable<ICodeEntry> codeEntries)
        {
            var distinguishNamesForMembers = codeEntries as ICodeEntry[] ?? codeEntries.ToArray();
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
        internal static string GenerateNameBasedOnNamingPreferences(this ICodeEntry codeEntry)
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