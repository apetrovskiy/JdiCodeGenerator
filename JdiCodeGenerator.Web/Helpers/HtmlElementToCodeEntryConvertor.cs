namespace JdiCodeGenerator.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using HtmlAgilityPack;
    using Core.Helpers;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;
    using ObjectModel.Abstract;

    public class HtmlElementToCodeEntryConvertor // <T>
    {
        Type[] _analyzers;
        ICodeEntry<HtmlElementTypes> _codeEntry;

        public ICodeEntry<HtmlElementTypes> ConvertToCodeEntry(HtmlNode node, Type[] analyzers)
        {
            _codeEntry = node.ConvertToCodeEntry();
            _codeEntry.JdiMemberType = node.ApplyApplicableAnalyzers(analyzers);

            // experimental
            _codeEntry.AnalyzerThatWon = null != ExtensionMethodsForNodes.AnalyzerThatWon ? ExtensionMethodsForNodes.AnalyzerThatWon.GetType().Name : string.Empty;
            _codeEntry.RuleThatWon = null != ExtensionMethodsForNodes.AnalyzerThatWon && null != ExtensionMethodsForNodes.AnalyzerThatWon.RuleThatWon ? ExtensionMethodsForNodes.AnalyzerThatWon.RuleThatWon.Description : string.Empty;

            // if there're rules for internal elements, get the internal
            // children collection for complex elements
            if (!string.IsNullOrEmpty(_codeEntry.RuleThatWon))
                // if (null != ExtensionMethodsForNodes.AnalyzerThatWon.RuleThatWon.InternalElements && ExtensionMethodsForNodes.AnalyzerThatWon.RuleThatWon.InternalElements.Any())
                // WorkOutInternalElements(ExtensionMethodsForNodes.AnalyzerThatWon.RuleThatWon, _codeEntry, node);
                WorkOutInternalElements(ExtensionMethodsForNodes.AnalyzerThatWon.RuleThatWon, node);

            if (JdiElementTypes.Element == _codeEntry.JdiMemberType)
                _codeEntry.JdiMemberType = _codeEntry.SourceMemberType.Types[0].ConvertHtmlTypeToJdiType();

            // temporarily!
            _codeEntry.Type = node.GetOriginalNameOfElement().CleanUpFromWrongCharacters();

            // temporarily!
            _codeEntry.MemberType = node.GetOriginalNameOfElement().CleanUpFromWrongCharacters();

            return _codeEntry;
        }

        public IEnumerable<ICodeEntry<HtmlElementTypes>> ConvertToCodeEntries(HtmlNode rootNode, Type[] analyzers)
        {
            _analyzers = analyzers;

            var processChildren = rootNode.OriginalName == WebNames.ElementTypeBody || ConvertToCodeEntry(rootNode, _analyzers).ProcessChildren;

            var resultList = new List<ICodeEntry<HtmlElementTypes>>();
            if (rootNode.OriginalName != WebNames.ElementTypeBody)
                resultList.Add(ConvertToCodeEntry(rootNode, _analyzers));

            if (processChildren)
                resultList.AddRange(
                    rootNode.ChildNodes
                    .Where(node => node.NodeType == HtmlNodeType.Element)
                    .SelectMany(node => ConvertToCodeEntries(node, _analyzers)).ToList()
                    );

            return resultList;
        }

        void WorkOutInternalElements(IRule<HtmlElementTypes> rule, HtmlNode node)
        {
            if (null == rule.InternalElements || !rule.InternalElements.Any())
                return;
            // the root element
            //if (rule.InternalElements.Any(subRule => Resources.Jdi_DropDown_root == subRule.Key))
            //{
            //    var rootNode = GetNodeByRule(rule.InternalElements.First(subRule => Resources.Jdi_DropDown_root == subRule.Key).Value, node);
            //    if (null != rootNode)
            //    {
            //        // _codeEntry.Root = rootNode.ConvertToCodeEntry().Locators.First(locator => locator.IsBestChoice);
            //        var entryRoot = rootNode.ConvertToCodeEntry();
            //        var locatorsForRoot = entryRoot.Locators;
            //        _codeEntry.Root = locatorsForRoot.First(locator => locator.IsBestChoice);
            //    }
            //}
            _codeEntry.Root = GetLocatorByRule(rule, node, Resources.Jdi_DropDown_root);
            // the value element
            //if (rule.InternalElements.Any(subRule => Resources.Jdi_DropDown_value == subRule.Key))
            //{
            //    var valueNode = GetNodeByRule(rule.InternalElements.First(subRule => Resources.Jdi_DropDown_value == subRule.Key).Value, node);
            //    if (null != valueNode)
            //    {
            //        // _codeEntry.Value = valueNode.ConvertToCodeEntry().Locators.First(locator => locator.IsBestChoice);
            //        var entryValue = valueNode.ConvertToCodeEntry();
            //        var locatorsForValue = entryValue.Locators;
            //        _codeEntry.Value = locatorsForValue.First(locator => locator.IsBestChoice);
            //    }
            //}
            _codeEntry.Value = GetLocatorByRule(rule, node, Resources.Jdi_DropDown_value);
            // the list collection
            if (rule.InternalElements.Any(subRule => Resources.Jdi_DropDown_list == subRule.Key))
            {
                var listNodes = GetNodeListByRule(rule.InternalElements.First(subRule => Resources.Jdi_DropDown_list == subRule.Key).Value, node);
                //if (null != listNodes && listNodes.Any())
                //{
                //    _codeEntry.List =
                //        listNodes.First().ConvertToCodeEntry(_analyzers).Locators.First(locator => locator.IsBestChoice);
                //    _codeEntry.ListMemberNames.AddRange(listNodes.Select(listNode => listNode.InnerText.ToPascalCase()));
                //}

                var listNodesPreEnumerated = listNodes as IList<HtmlNode> ?? listNodes.ToList();

                if (!listNodesPreEnumerated.Any())
                {
                    _codeEntry.List = new LocatorDefinition
                    {
                        Attribute = FindTypes.FindBy,
                        SearchTypePreference = SearchTypePreferences.xpath,
                        SearchString = string.Empty,
                        IsBestChoice = true
                    };
                    return;
                }

                if (null != listNodes && listNodesPreEnumerated.Any())
                {
                    // 20160707
                    // a dubious fix
                    _codeEntry.List = listNodesPreEnumerated.First().ConvertToCodeEntry().Locators.First(locator => locator.IsBestChoice);
                    if (listNodesPreEnumerated.Any())
                        _codeEntry.ListMemberNames.AddRange(listNodesPreEnumerated.Select(listNode => listNode.InnerText.ToPascalCase()));
                }
            }
        }

        LocatorDefinition GetLocatorByRule(IRule<HtmlElementTypes> rule, HtmlNode node, string ruleKey)
        {
            // if (rule.InternalElements.Any(subRule => ruleKey == subRule.Key))
            // ReSharper tricks
            //if (rule.InternalElements.Any(subRule => ruleKey == subRule.Key))
            //{
            //    var rootNode = GetNodeByRule(rule.InternalElements.First(subRule => ruleKey == subRule.Key).Value, node);
            //    if (null == rootNode) return null;
            //    // _codeEntry.Root = rootNode.ConvertToCodeEntry().Locators.First(locator => locator.IsBestChoice);
            //    var entryRoot = rootNode.ConvertToCodeEntry();
            //    var locatorsForRoot = entryRoot.Locators;
            //    return locatorsForRoot.First(locator => locator.IsBestChoice);
            //}
            if (rule.InternalElements.All(subRule => ruleKey != subRule.Key)) return null;
            var rootNode = GetNodeByRule(rule.InternalElements.First(subRule => ruleKey == subRule.Key).Value, node);
            if (null == rootNode) return null;
            // _codeEntry.Root = rootNode.ConvertToCodeEntry().Locators.First(locator => locator.IsBestChoice);
            var entryRoot = rootNode.ConvertToCodeEntry();
            var locatorsForRoot = entryRoot.Locators;
            return locatorsForRoot.First(locator => locator.IsBestChoice);
        }

        HtmlNode GetNodeByRule(IRule<HtmlElementTypes> rule, HtmlNode upperNode)
        {
            //HtmlNode nodeInQuestion = null;
            //upperNode.DescendantsAndSelf()
            //    .ToList()
            //    .ForEach(potentialNodeInQuestion =>
            //    {
            //        if (rule.InternalElements.First(ruleByKey => ruleKey == ruleByKey.Key).Value.IsMatch(potentialNodeInQuestion))
            //            nodeInQuestion = potentialNodeInQuestion;
            //    });
            //return nodeInQuestion;
            return upperNode.DescendantsAndSelf()
                .ToList()
                .First(rule.IsMatch);
        }

        IEnumerable<HtmlNode> GetNodeListByRule(IRule<HtmlElementTypes> rule, HtmlNode upperNode)
        {
            //var resultList = new List<HtmlNode>();
            //if (!rule.InternalElements.Any())
            //    return resultList;
            //resultList.AddRange(upperNode.Descendants()
            //    .Where(someNode => rule.InternalElements.First(listRule => Resources.Jdi_DropDown_list == listRule.Key).Value.IsMatch(someNode))
            //    .ToList());
            //return resultList;
            var resultList = new List<HtmlNode>();
            //resultList.AddRange(upperNode.Descendants()
            //    .Where(rule.IsMatch)
            //    .ToList());
            var descendants = upperNode.Descendants();
            var descendantsThatMatch = descendants.Where(child => rule.IsMatch(child));
            if (null != descendantsThatMatch && descendantsThatMatch.Any())
                resultList.AddRange(descendantsThatMatch);
            return resultList;
        }
    }
}