namespace JdiCodeGenerator.Core.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
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
            codeEntry.Locators.RemoveAll(locator => null == locator || locator.SearchString == string.Empty);

            // TODO: write the code behind // ??
            codeEntry.JdiMemberType = node.ApplyApplicableAnalyzers();

            // experimental
            codeEntry.AnalyzerThatWon = null != ExtensionMethodsForNodes.AnalyzerThatWon ? ExtensionMethodsForNodes.AnalyzerThatWon.GetType().Name : string.Empty;
            codeEntry.RuleThatWon = null != ExtensionMethodsForNodes.AnalyzerThatWon && null != ExtensionMethodsForNodes.AnalyzerThatWon.RuleThatWon ? ExtensionMethodsForNodes.AnalyzerThatWon.RuleThatWon.GetType().Name : string.Empty;

            if (JdiElementTypes.Element == codeEntry.JdiMemberType)
                codeEntry.JdiMemberType = codeEntry.HtmlMemberType.ConvertHtmlTypeToJdiType();

            // temporarily!
            codeEntry.Type = node.GetOriginalNameOfElement().CleanUpFromWrongCharacters();

            // temporarily!
            codeEntry.MemberType = node.GetOriginalNameOfElement().CleanUpFromWrongCharacters();

            return codeEntry;
        }

        public IEnumerable<ICodeEntry> ConvertToCodeEntries(HtmlNode rootNode)
        {
            //var codeEntry = ConvertToCodeEntry(rootNode);
            //return codeEntry.ProcessChildren ? rootNode.ChildNodes
            //    .Where(node => node.NodeType == HtmlNodeType.Element)
            //    .SelectMany(ConvertToCodeEntries).ToList() :
            //new List<ICodeEntry> {codeEntry};

            var processChildren = rootNode.OriginalName == WebNames.ElementTypeBody
                ? true
                : ConvertToCodeEntry(rootNode).ProcessChildren;

            var resultList = new List<ICodeEntry>();
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