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
    using Core.ObjectModel.Enums;
    using ObjectModel.Abstract;

    public class HtmlElementToElementMemberCodeEntryConvertor
    {
        Type[] _analyzers;
        IPageMemberCodeEntry<HtmlElementTypes> _pageMemberCodeEntry;
        // 20160715
        Guid _parentPageGuid;

        // 20160715
        public HtmlElementToElementMemberCodeEntryConvertor(Guid parentPageGuid)
        {
            _parentPageGuid = parentPageGuid;
        }

        public IPageMemberCodeEntry<HtmlElementTypes> ConvertToCodeEntry(HtmlNode node, Type[] analyzers)
        {
            _pageMemberCodeEntry = node.ConvertToCodeEntry();
            _pageMemberCodeEntry.JdiMemberType = node.ApplyApplicableAnalyzers(analyzers);

            // experimental
            _pageMemberCodeEntry.AnalyzerThatWon = null != HtmlNodesExtensions.AnalyzerThatWon ? HtmlNodesExtensions.AnalyzerThatWon.GetType().Name : string.Empty;
            _pageMemberCodeEntry.RuleThatWon = null != HtmlNodesExtensions.AnalyzerThatWon && null != HtmlNodesExtensions.AnalyzerThatWon.RuleThatWon ? HtmlNodesExtensions.AnalyzerThatWon.RuleThatWon.Description : string.Empty;

            // if there're rules for internal elements, get the internal
            // children collection for complex elements
            if (!string.IsNullOrEmpty(_pageMemberCodeEntry.RuleThatWon))
                WorkOutInternalElements(HtmlNodesExtensions.AnalyzerThatWon.RuleThatWon, node);

            if (JdiElementTypes.Element == _pageMemberCodeEntry.JdiMemberType)
                _pageMemberCodeEntry.JdiMemberType = _pageMemberCodeEntry.SourceMemberType[0].ConvertHtmlTypeToJdiType();

            // temporarily!
            // _pageMemberCodeEntry.Type = node.GetOriginalNameOfElement().CleanUpFromWrongCharacters();

            // temporarily!
            _pageMemberCodeEntry.MemberType = node.GetOriginalNameOfElement().CleanUpFromWrongCharacters();

            // 20160715
            _pageMemberCodeEntry.ParentId = _parentPageGuid;

            return _pageMemberCodeEntry;
        }

        // 20160715
        // TODO: use more wider type for the list
        public IEnumerable<IPageMemberCodeEntry<HtmlElementTypes>> ConvertToCodeEntries(HtmlNode rootNode, Type[] analyzers)
        {
            _analyzers = analyzers;

            var processChildren = rootNode.OriginalName == WebNames.ElementTypeBody || ConvertToCodeEntry(rootNode, _analyzers).ProcessChildren;

            // 20160715
            // TODO: use more wider type for the list
            var resultList = new List<IPageMemberCodeEntry<HtmlElementTypes>>();
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
            _pageMemberCodeEntry.Root = GetLocatorByRule(rule, node, Resources.Jdi_DropDown_root);
            _pageMemberCodeEntry.Value = GetLocatorByRule(rule, node, Resources.Jdi_DropDown_value);
            // the list collection
            if (rule.InternalElements.Any(subRule => Resources.Jdi_DropDown_list == subRule.Key))
            {
                var listNodes = GetNodeListByRule(rule.InternalElements.First(subRule => Resources.Jdi_DropDown_list == subRule.Key).Value, node);
                var listNodesPreEnumerated = listNodes as IList<HtmlNode> ?? listNodes.ToList();

                if (!listNodesPreEnumerated.Any())
                {
                    _pageMemberCodeEntry.List = new LocatorDefinition
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
                    _pageMemberCodeEntry.List = listNodesPreEnumerated.First().ConvertToCodeEntry().Locators.First(locator => locator.IsBestChoice);
                    if (listNodesPreEnumerated.Any())
                        _pageMemberCodeEntry.ListMemberNames.AddRange(listNodesPreEnumerated.Select(listNode => listNode.InnerText.ToPascalCase()));
                }
            }
        }

        LocatorDefinition GetLocatorByRule(IRule<HtmlElementTypes> rule, HtmlNode node, string ruleKey)
        {
            if (rule.InternalElements.All(subRule => ruleKey != subRule.Key)) return null;
            var rootNode = GetNodeByRule(rule.InternalElements.First(subRule => ruleKey == subRule.Key).Value, node);
            if (null == rootNode) return null;
            var entryRoot = rootNode.ConvertToCodeEntry();
            var locatorsForRoot = entryRoot.Locators;
            return locatorsForRoot.First(locator => locator.IsBestChoice);
        }

        HtmlNode GetNodeByRule(IRule<HtmlElementTypes> rule, HtmlNode upperNode)
        {
            return upperNode.DescendantsAndSelf()
                .ToList()
                .First(rule.IsMatch);
        }

        IEnumerable<HtmlNode> GetNodeListByRule(IRule<HtmlElementTypes> rule, HtmlNode upperNode)
        {
            var resultList = new List<HtmlNode>();
            var descendants = upperNode.DescendantsAndSelf();
            // var descendantsThatMatch = descendants.Where(child => rule.IsMatch(child));
            var descendantsThatMatch = descendants.Where(rule.IsMatch);
            if (null != descendantsThatMatch && descendantsThatMatch.Any())
                resultList.AddRange(descendantsThatMatch);
            return resultList;
        }
    }
}