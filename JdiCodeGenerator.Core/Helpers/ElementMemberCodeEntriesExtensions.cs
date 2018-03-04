namespace CodeGenerator.Core.Helpers
{
	using System.Collections.Generic;
	using System.Linq;
	using ObjectModel.Abstract.Results;
	using ObjectModel.Enums;

	public static class ElementMemberCodeEntriesExtensions
    {
        public static IEnumerable<IPageMemberCodeEntry> SetDistinguishNamesForMembers(this IEnumerable<IPageMemberCodeEntry> codeEntries)
        {
			var distinguishNamesForMembers = codeEntries.Where(entry => PageObjectParts.CodeOfMember == entry.CodeClass).ToArray();
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
            if (pageMemberCodeEntry.Locators.Any(locator => locator.ElementSearchTypePreference == ElementSearchTypePreferences.name))
                return prefix + pageMemberCodeEntry.Locators.First(locator => locator.ElementSearchTypePreference == ElementSearchTypePreferences.name).SearchString.ToPascalCase();

            if (pageMemberCodeEntry.Locators.Any(locator => locator.ElementSearchTypePreference == ElementSearchTypePreferences.id))
                return prefix + pageMemberCodeEntry.Locators.First(locator => locator.ElementSearchTypePreference == ElementSearchTypePreferences.id).SearchString.ToPascalCase();

            if (pageMemberCodeEntry.Locators.Any(locator => locator.ElementSearchTypePreference == ElementSearchTypePreferences.tagName))
                return prefix + pageMemberCodeEntry.Locators.First(locator => locator.ElementSearchTypePreference == ElementSearchTypePreferences.tagName).SearchString.ToPascalCase();

            if (pageMemberCodeEntry.Locators.Any(locator => locator.ElementSearchTypePreference == ElementSearchTypePreferences.linkText))
                return prefix + pageMemberCodeEntry.Locators.First(locator => locator.ElementSearchTypePreference == ElementSearchTypePreferences.linkText).SearchString.ToPascalCase();

            if (pageMemberCodeEntry.Locators.Any(locator => locator.ElementSearchTypePreference == ElementSearchTypePreferences.className))
                return prefix + pageMemberCodeEntry.Locators.First(locator => locator.ElementSearchTypePreference == ElementSearchTypePreferences.className).SearchString.ToPascalCase();

            return prefix + NoName;
        }
    }
}