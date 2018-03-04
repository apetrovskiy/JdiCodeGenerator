namespace CodeGenerator.Core.Helpers
{
	using System.Collections.Generic;
	using System.Linq;
	using ObjectModel.Abstract;
	using ObjectModel.Enums;

	public static class ElementMemberCodeEntriesExtensions
    {
        public static IEnumerable<IPageMemberCodeEntry> SetDistinguishNamesForMembers(this IEnumerable<IPageMemberCodeEntry> codeEntries)
        {
			var distinguishNamesForMembers = codeEntries.Where(entry => PiecesOfCodeClasses.PageMember == entry.CodeClass).ToArray();
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