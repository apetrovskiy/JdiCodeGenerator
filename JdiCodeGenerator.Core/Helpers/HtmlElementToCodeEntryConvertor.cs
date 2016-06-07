namespace JdiCodeGenerator.Core.Helpers
{
    using System.Collections.Generic;
    using HtmlAgilityPack;
    using ObjectModel;
    using ObjectModel.Abstract;
    using ObjectModel.Plugins;

    public class HtmlElementToCodeEntryConvertor
    {
        public ICodeEntry ConvertToCodeEntry(HtmlNode node)
        {
            var codeEntry = new CodeEntry { HtmlMemberType = new General().Analyze(node.OriginalName) };

            codeEntry.Locators.AddRange(
                new List<LocatorDefinition>
                {
                    node.CreateIdLocator(),
                    node.CreateNameLocator(),
                    node.CreateClassLocator(),
                    node.CreateTagLocator(),
                    node.CreateLinkTextLocator(),
                    node.CreateCssLocator(),
                    node.CreateXpathLocator()
                });
            // experimental
            // codeEntry.Locators.RemoveAll(locator => null == locator);
            codeEntry.Locators.RemoveAll(locator => null == locator || locator.SearchString == string.Empty);

            // TODO: write the code behind // ??
            codeEntry.JdiMemberType = node.ApplyApplicableAnalyzers();
            if (JdiElementTypes.Element == codeEntry.JdiMemberType)
                codeEntry.JdiMemberType = codeEntry.HtmlMemberType.ConvertHtmlTypeToJdiType();

            // temporarily!
            codeEntry.Type = node.GetOriginalNameOfElement().CleanUpFromWrongCharacters();

            // temporarily!
            codeEntry.MemberType = node.GetOriginalNameOfElement().CleanUpFromWrongCharacters();

            return codeEntry;
        }
    }
}