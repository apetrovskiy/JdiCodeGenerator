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
			var distinguishedNamesForMembers = codeEntries.Where(entry => PageObjectParts.CodeOfMember == entry.CodeClass).ToArray();
	        distinguishedNamesForMembers.GenerateNamesInCollection();
	        distinguishedNamesForMembers.NumerateNamesInCollection();
            return distinguishedNamesForMembers;
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

			// TODO: refactor this for the simplicity sake
	        if (pageMemberCodeEntry.ContainsLocatorWithSearchTypePreference(ElementSearchTypePreferences.name))
		        return prefix + pageMemberCodeEntry.GenerateNameWithoutPrefix(ElementSearchTypePreferences.name);

	        if (pageMemberCodeEntry.ContainsLocatorWithSearchTypePreference(ElementSearchTypePreferences.id))
		        return prefix + pageMemberCodeEntry.GenerateNameWithoutPrefix(ElementSearchTypePreferences.id);

	        if (pageMemberCodeEntry.ContainsLocatorWithSearchTypePreference(ElementSearchTypePreferences.tagName))
		        return prefix + pageMemberCodeEntry.GenerateNameWithoutPrefix(ElementSearchTypePreferences.tagName);

	        if (pageMemberCodeEntry.ContainsLocatorWithSearchTypePreference(ElementSearchTypePreferences.linkText))
		        return prefix + pageMemberCodeEntry.GenerateNameWithoutPrefix(ElementSearchTypePreferences.linkText);

	        if (pageMemberCodeEntry.ContainsLocatorWithSearchTypePreference(ElementSearchTypePreferences.className))
		        return prefix + pageMemberCodeEntry.GenerateNameWithoutPrefix(ElementSearchTypePreferences.className);

			return prefix + NoName;
        }

	    private static bool ContainsLocatorWithSearchTypePreference(this IPageMemberCodeEntry pageMemberCodeEntry, ElementSearchTypePreferences preference)
	    {
			return pageMemberCodeEntry.Locators.Any(locator => preference == locator.ElementSearchTypePreference);
		}

	    private static string GenerateNameWithoutPrefix(this IPageMemberCodeEntry pageMemberCodeEntry, ElementSearchTypePreferences preference)
	    {
			return pageMemberCodeEntry.Locators.First(locator => preference == locator.ElementSearchTypePreference).SearchString.ToPascalCase();
		}

	    internal static void GenerateNamesInCollection(this IPageMemberCodeEntry[] collectionCodeEntries)
		{
			collectionCodeEntries.ToList().ForEach(codeEntry => codeEntry.MemberName = codeEntry.GenerateNameBasedOnNamingPreferences());
		}

	    internal static void NumerateNamesInCollection(this IPageMemberCodeEntry[] collectionCodeEntries)
	    {
			collectionCodeEntries
				.GroupBy(codeEntryName => codeEntryName.MemberName)
				.Select(grouping =>
				{
					var i = 0;
					grouping.ToList().Skip(1).ToList().ForEach(item => { item.MemberName += ++i; });
					return grouping;
				}).SelectMany(grouping => grouping.Select(item => item))
				.ToList();
		}
    }
}