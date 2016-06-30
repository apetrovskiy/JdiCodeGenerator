namespace JdiCodeGenerator.Web.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using HtmlAgilityPack;
    using Core;
    using Core.Helpers;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;
    using ObjectModel;
    using ObjectModel.Abstract;
    using ObjectModel.Plugins;

    public class HtmlElementToCodeEntryConvertor<T>
    {
        public ICodeEntry<T> ConvertToCodeEntry<T>(HtmlNode node)
        {
            // refactoring
            // 20160630
            // var codeEntry = new CodeEntry<T> { HtmlMemberType = new General().Analyze(node.OriginalName) };
            var codeEntry = new CodeEntry<T> { SourceMemberType = new SourceElementTypeCollection<T> { Types = new List<T> { new General().Analyze(node.OriginalName) } } };

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
                // refactoring
                // 20160630
                // codeEntry.JdiMemberType = codeEntry.HtmlMemberType.ConvertHtmlTypeToJdiType();
                codeEntry.JdiMemberType = codeEntry.SourceMemberType.Types[0].ConvertHtmlTypeToJdiType();

            // temporarily!
            codeEntry.Type = node.GetOriginalNameOfElement().CleanUpFromWrongCharacters();

            // temporarily!
            codeEntry.MemberType = node.GetOriginalNameOfElement().CleanUpFromWrongCharacters();

            return codeEntry;
        }

        public IEnumerable<ICodeEntry<HtmlElementTypes>> ConvertToCodeEntries(HtmlNode rootNode)
        {
            //var codeEntry = ConvertToCodeEntry(rootNode);
            //return codeEntry.ProcessChildren ? rootNode.ChildNodes
            //    .Where(node => node.NodeType == HtmlNodeType.Element)
            //    .SelectMany(ConvertToCodeEntries).ToList() :
            //new List<ICodeEntry> {codeEntry};

            var processChildren = rootNode.OriginalName == WebNames.ElementTypeBody
                ? true
                : ConvertToCodeEntry<HtmlElementTypes>(rootNode).ProcessChildren;

            var resultList = new List<ICodeEntry<HtmlElementTypes>>();
            if (rootNode.OriginalName != WebNames.ElementTypeBody)
                resultList.Add(ConvertToCodeEntry<HtmlElementTypes>(rootNode));

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