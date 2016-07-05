namespace JdiCodeGenerator.Web.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using HtmlAgilityPack;
    using Core.Helpers;
    using Core.ObjectModel.Abstract;
    using ObjectModel.Abstract;

    public class HtmlElementToCodeEntryConvertor // <T>
    {
        public ICodeEntry<HtmlElementTypes> ConvertToCodeEntry(HtmlNode node)
        {
            var codeEntry = node.ConvertToCodeEntry();

            // if there're rules for internal elements, get the internal
            // children collection for complex elements
            if (!string.IsNullOrEmpty(codeEntry.RuleThatWon))
                // if (null != ExtensionMethodsForNodes.AnalyzerThatWon.RuleThatWon.InternalElements && ExtensionMethodsForNodes.AnalyzerThatWon.RuleThatWon.InternalElements.Any())
                WorkOutInternalElements(ExtensionMethodsForNodes.AnalyzerThatWon.RuleThatWon, codeEntry, node);

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

        // TODO: children collection of complex elements
        void WorkOutInternalElements(IRule<HtmlElementTypes> rule, ICodeEntry<HtmlElementTypes> codeEntry, HtmlNode node)
        {
            if (null == rule.InternalElements || !rule.InternalElements.Any())
                return;
            //// TODO: the root element
            //codeEntry.Root = GetNodeByRule(node, Resources.Jdi_DropDown_root).;
            // codeEntry.Root = GetNodeByRule(node, Resources.Jdi_DropDown_root).ConvertToCodeEntry().Locators.First(locator => locator.IsBestChoice);
            if (rule.InternalElements.Any(subRule => Resources.Jdi_DropDown_root == subRule.Key))
            {
                var rootNode = GetNodeByRule(rule, node, Resources.Jdi_DropDown_root);
                if (null != rootNode)
                    codeEntry.Root = rootNode.ConvertToCodeEntry().Locators.First(locator => locator.IsBestChoice);
            }
            //// TODO: the value element
            //codeEntry.Value = GetNodeByRule(node, Resources.Jdi_DropDown_value);
            // codeEntry.Value = GetNodeByRule(node, Resources.Jdi_DropDown_value).ConvertToCodeEntry().Locators.First(locator => locator.IsBestChoice);
            if (rule.InternalElements.Any(subRule => Resources.Jdi_DropDown_value == subRule.Key))
            {
                var valueNode = GetNodeByRule(rule, node, Resources.Jdi_DropDown_value);
                if (null != valueNode)
                    codeEntry.Value = valueNode.ConvertToCodeEntry().Locators.First(locator => locator.IsBestChoice);
            }
            //// TODO: the list collection
            //codeEntry.List = GetNodeListByRule(node);
            // codeEntry.List = GetNodeListByRule(node).First().ConvertToCodeEntry().Locators.First(locator => locator.IsBestChoice);
            // codeEntry.ListMemberNames.AddRange(GetNodeListByRule(node).Select(listNode => listNode.InnerText.ToPascalCase()));
            if (rule.InternalElements.Any(subRule => Resources.Jdi_DropDown_list == subRule.Key))
            {
                var listNodes = GetNodeListByRule(rule, node);
                if (null != listNodes && listNodes.Any())
                {
                    codeEntry.List =
                        listNodes.First().ConvertToCodeEntry().Locators.First(locator => locator.IsBestChoice);
                    codeEntry.ListMemberNames.AddRange(listNodes.Select(listNode => listNode.InnerText.ToPascalCase()));
                }
            }
        }

        HtmlNode GetNodeByRule(IRule<HtmlElementTypes> rule, HtmlNode upperNode, string ruleKey)
        {
            HtmlNode nodeInQuestion = null;
            upperNode.DescendantsAndSelf()
                .ToList()
                .ForEach(potentialNodeInQuestion =>
                {
                    if (rule.InternalElements.First(ruleByKey => ruleKey == ruleByKey.Key).Value.IsMatch(potentialNodeInQuestion))
                        nodeInQuestion = potentialNodeInQuestion;
                });
            return nodeInQuestion;
        }

        IEnumerable<HtmlNode> GetNodeListByRule(IRule<HtmlElementTypes> rule, HtmlNode upperNode)
        {
            var resultList = new List<HtmlNode>();
            if (!rule.InternalElements.Any())
                return resultList;
            resultList.AddRange(upperNode.Descendants()
                .Where(someNode => rule.InternalElements.First(listRule => Resources.Jdi_DropDown_list == listRule.Key).Value.IsMatch(someNode))
                .ToList());
            return resultList;
        }
    }
}