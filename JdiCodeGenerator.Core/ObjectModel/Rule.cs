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
        public Dictionary<string, IRule> InternalElements { get; set; }
        public List<HtmlElementTypes> SourceTypes { get; set; }
        public JdiElementTypes TargetType { get; set; }

        public Rule()
        {
            SourceTypes = new List<HtmlElementTypes>();
            OrConditions = new List<IRuleCondition>();
            AndConditions = new List<IRuleCondition>();
            InternalElements = new Dictionary<string, IRule>();
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
            return (!OrConditions.Any() || OrConditions.Any(condition => CheckCondition(node, condition))) &&
            (!AndConditions.Any() || AndConditions.All(condition => CheckCondition(node, condition)));
        }

        bool CheckCondition(HtmlNode node, IRuleCondition condition)
        {
            var nodesForCondition = GetNodesThatMatchTheCondition(node, condition.Relationship, condition.Marker);
            var forCondition = nodesForCondition as HtmlNode[] ?? nodesForCondition.ToArray();
            if (!forCondition.Any())
                return false;
            return forCondition.Any(probeNode => NodeMatchesTheCondition(probeNode, condition.Marker, condition.MarkerValues));
        }

        IEnumerable<HtmlNode> GetNodesThatMatchTheCondition(HtmlNode node, NodeRelationships relationship, Markers marker)
        {
            // TODO: refactor this!
            switch (relationship)
            {
                case NodeRelationships.Self:
                    // return node.HasAttribute(marker) ? new List<HtmlNode> {node} : new List<HtmlNode> {null};
                    var result = node.HasAttribute(marker) ? new List<HtmlNode> { node } : new List<HtmlNode> { null };
                    return result;
                case NodeRelationships.Sibling:
                    // TODO: write better code!
                    return new List<HtmlNode> { null };
                case NodeRelationships.Parent:
                    return node.ParentNode.HasAttribute(marker) ? new List<HtmlNode> { node.ParentNode } : new List<HtmlNode> { null };
                case NodeRelationships.Ancestor:
                    return node.Ancestors().Any(ancestor => ancestor.HasAttribute(marker))
                        ? node.Ancestors().Where(ancestor => ancestor.HasAttribute(marker)).ToList()
                        : new List<HtmlNode> { null };
                case NodeRelationships.Child:
                    return node.ChildNodes.Any(childNode => childNode.HasAttribute(marker))
                        ? node.ChildNodes.Where(childNode => childNode.HasAttribute(marker)).ToList()
                        : new List<HtmlNode> { null };
                //return node.SelectNodes("*").Any(childNode => childNode.HasAttribute(marker))
                //    ? node.SelectNodes("*").Where(childNode => childNode.HasAttribute(marker)).ToList()
                //    : new List<HtmlNode> { null };
                case NodeRelationships.Descendant:
                    //return node.Descendants().Any(descendant => descendant.HasAttribute(marker))
                    //    ? node.Descendants().Where(childNode => childNode.HasAttribute(marker)).ToList()
                    //    : new List<HtmlNode> { null };
                    var result2 = node.Descendants().Any(descendant => descendant.HasAttribute(marker))
                        ? node.Descendants().Where(childNode => childNode.HasAttribute(marker)).ToList()
                        : new List<HtmlNode> { null };
                    //var result2 = node.SelectNodes("*").Any(descendant => descendant.HasAttribute(marker))
                    //    ? node.SelectNodes("*").Where(childNode => childNode.HasAttribute(marker)).ToList()
                    //    : new List<HtmlNode> { null };
                    return result2;
                default:
                    return new List<HtmlNode> { null };
            }
        }

        bool NodeMatchesTheCondition(HtmlNode nodeForCondition, Markers marker, List<string> markerValues)
        {
            var attributeValue = nodeForCondition.GetAttributeValue(marker);
            return markerValues.Any(markerValue => attributeValue.Contains(markerValue));
        }
    }
}
