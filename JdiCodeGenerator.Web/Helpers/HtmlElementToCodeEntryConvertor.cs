namespace JdiCodeGenerator.Web.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using HtmlAgilityPack;
    using Core;
    using Core.Helpers;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;
    using ObjectModel.Abstract;
    using ObjectModel.Plugins;

    public class HtmlElementToCodeEntryConvertor//<T>
    {
        public ICodeEntry<HtmlElementTypes> ConvertToCodeEntry(HtmlNode node)
        {
            var codeEntry = new CodeEntry<HtmlElementTypes> { SourceMemberType = new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { (new General()).Analyze(node.OriginalName) } } };

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
            codeEntry.Locators.RemoveAll(locator => null == locator || locator.SearchString == string.Empty);

            // TODO: write the code behind // ??
            codeEntry.JdiMemberType = node.ApplyApplicableAnalyzers();

            // experimental
            codeEntry.AnalyzerThatWon = null != ExtensionMethodsForNodes.AnalyzerThatWon ? ExtensionMethodsForNodes.AnalyzerThatWon.GetType().Name : string.Empty;
            codeEntry.RuleThatWon = null != ExtensionMethodsForNodes.AnalyzerThatWon && null != ExtensionMethodsForNodes.AnalyzerThatWon.RuleThatWon ? ExtensionMethodsForNodes.AnalyzerThatWon.RuleThatWon.GetType().Name : string.Empty;

            if (JdiElementTypes.Element == codeEntry.JdiMemberType)
                codeEntry.JdiMemberType = codeEntry.SourceMemberType.Types[0].ConvertHtmlTypeToJdiType();

            // temporarily!
            codeEntry.Type = node.GetOriginalNameOfElement().CleanUpFromWrongCharacters();

            // temporarily!
            codeEntry.MemberType = node.GetOriginalNameOfElement().CleanUpFromWrongCharacters();

            return codeEntry;
        }

        public IEnumerable<ICodeEntry<HtmlElementTypes>> ConvertToCodeEntries(HtmlNode rootNode)
        {
            var processChildren = rootNode.OriginalName == WebNames.ElementTypeBody || ConvertToCodeEntry(rootNode).ProcessChildren;

            var resultList = new List<ICodeEntry<HtmlElementTypes>>();
            if (rootNode.OriginalName != WebNames.ElementTypeBody)
                resultList.Add(ConvertToCodeEntry(rootNode));

            if (processChildren)
                resultList.AddRange(
                    rootNode.ChildNodes
                    .Where(node => node.NodeType == HtmlNodeType.Element)
                    .SelectMany(ConvertToCodeEntries).ToList()
                    );

            return resultList;
        }
    }
}