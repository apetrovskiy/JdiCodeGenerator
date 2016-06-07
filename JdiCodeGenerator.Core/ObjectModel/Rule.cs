namespace JdiCodeGenerator.Core.ObjectModel
{
    using System.Collections.Generic;
    using System.Linq;
    using Abstract;
    using Helpers;
    using HtmlAgilityPack;
    using Plugins;

    public class Rule : IRule
    {
        public IEnumerable<IRuleCondition> OrConditions { get; set; }
        public IEnumerable<IRuleCondition> AndConditions { get; set; }
        public string Description { get; set; }
        public List<HtmlElementTypes> SourceTypes { get; set; }
        public JdiElementTypes TargetType { get; set; }

        public Rule()
        {
            SourceTypes = new List<HtmlElementTypes>();
            OrConditions = new List<IRuleCondition>();
            AndConditions = new List<IRuleCondition>();
            TargetType = JdiElementTypes.Element;
        }

        public bool IsMatch(HtmlNode node)
        {
            var elementType = new General().Analyze(node.OriginalName);
            if (!SourceTypes.Contains(elementType))
                return false;

            return ResolveRuleToJdiType(node);
        }

        bool ResolveRuleToJdiType(HtmlNode node)
        {
            // experimental
            // return OrConditions.Any(condition => CheckCondition(node, condition));
            // (!orList.Any() || orList.Any(item => item)) && (!andList.Any() || andList.All(item => item))
            return (!OrConditions.Any() || OrConditions.Any(condition => CheckCondition(node, condition))) &&
            (!AndConditions.Any() || AndConditions.All(condition => CheckCondition(node, condition)));
        }

        bool CheckCondition(HtmlNode node, IRuleCondition condition)
        {
            var nodeForCondition = GetNodeThatMatchesTheCondition(node, condition.Relationship, condition.Marker);
            if (null == nodeForCondition)
                return false;
            return NodeMatchesTheCondition(nodeForCondition, condition.Marker, condition.MarkerValues);
        }

        HtmlNode GetNodeThatMatchesTheCondition(HtmlNode node, NodeRelationships relationship, Markers marker)
        {
            // TODO: refactor this!
            switch (relationship)
            {
                case NodeRelationships.Self:
                    return node.HasAttribute(marker) ? node : null;
                case NodeRelationships.Sibling:
                    // TODO: write better code!
                    return null;
                case NodeRelationships.Parent:
                    return node.ParentNode.HasAttribute(marker) ? node.ParentNode : null;
                case NodeRelationships.Ancestor:
                    return node.Ancestors().Any(ancestor => ancestor.HasAttribute(marker))
                        ? node.Ancestors().FirstOrDefault(ancestor => ancestor.HasAttribute(marker))
                        : null;
                case NodeRelationships.Child:
                    return node.ChildNodes.Any(childNode => childNode.HasAttribute(marker))
                        ? node.ChildNodes.FirstOrDefault(childNode => childNode.HasAttribute(marker))
                        : null;
                case NodeRelationships.Descendant:
                    return node.Descendants().Any(descendant => descendant.HasAttribute(marker))
                        ? node.Descendants().FirstOrDefault(childNode => childNode.HasAttribute(marker))
                        : null;
                default:
                    return null;
            }
        }

        bool NodeMatchesTheCondition(HtmlNode nodeForCondition, Markers marker, List<string> markerValues)
        {
            var attributeValue = nodeForCondition.GetAttributeValue(marker);
            return markerValues.Any(markerValue => attributeValue.Contains(markerValue));
        }
    }
}
