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
        public string Description { get; set; }
        public HtmlElementTypes SourceType { get; set; }
        public JdiElementTypes TargetType { get; set; }

        public Rule()
        {
            OrConditions = new List<IRuleCondition>();
            TargetType = JdiElementTypes.Element;
        }

        public bool IsMatch(HtmlNode node)
        {
            return ResolveRuleToJdiType(node);
        }

        bool ResolveRuleToJdiType(HtmlNode node)
        {
            return OrConditions.Any(condition => CheckCondition(node, condition));
        }

        bool CheckCondition(HtmlNode node, IRuleCondition condition)
        {
            var nodeForCondition = GetNodeThatMatchesTheCondition(node, condition.Relationship, condition.Marker);
            if (null == nodeForCondition)
                return false;
            return NodeMatchesTheCondition(nodeForCondition, condition.Marker, condition.MarkerValue);
        }

        HtmlNode GetNodeThatMatchesTheCondition(HtmlNode node, NodeRelationships relationship, Markers marker)
        {
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

        bool NodeMatchesTheCondition(HtmlNode nodeForCondition, Markers marker, string markerValue)
        {
            var attributeValue = nodeForCondition.GetAttributeValue(marker);
            return attributeValue.Contains(markerValue);
        }
    }
}
