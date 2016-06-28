namespace JdiCodeGenerator.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using HtmlAgilityPack;
    using Core;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;
    using ObjectModel.Abstract;
    using ObjectModel.Plugins;

    public static class ExtensionMethodsForNodes
    {
        public static bool HasAttribute(this HtmlNode node, string attributeName)
        {
            if (WebNames.AttributeNameTag == attributeName.ToLower())
                return true;
            return node.Attributes.Any(attribute => attribute.Name.ToLower() == attributeName);
        }

        public static bool HasAttribute(this HtmlNode node, Markers marker)
        {
            return HasAttribute(node, ConvertMarkerToStringNameOfAttribute(marker));
        }

        static string ConvertMarkerToStringNameOfAttribute(this Markers marker)
        {
            return marker.ToString().ToLower();
        }

        public static string GetAttributeValue(this HtmlNode node, string attributeName)
        {
            if (null == node)
                return string.Empty;
            if (WebNames.AttributeNameTag == attributeName.ToLower())
                return node.OriginalName;
            return NodeWithAttributes(node) ? node.Attributes.First(attribute => attribute.Name.ToLower() == attributeName).Value : string.Empty;
        }

        public static string GetAttributeValue(this HtmlNode node, Markers marker)
        {
            if (null == node)
                return string.Empty;
            if (Markers.Tag == marker)
                return node.OriginalName;
            return NodeWithAttributes(node) ? node.Attributes.First(attribute => attribute.Name.ToLower() == ConvertMarkerToStringNameOfAttribute(marker)).Value : string.Empty;
        }

        static bool NodeWithAttributes(HtmlNode node)
        {
            return null != node && null != node.Attributes && node.Attributes.Any();
        }

        public static bool HasAttributeValue(this HtmlNode node, string attributeName, string attributeValue)
        {
            return node.Attributes.First(attribute => attribute.Name.ToLower() == attributeName).Value == attributeValue;
        }

        public static bool HasAttributeValue(this HtmlNode node, Markers marker, string attributeValue)
        {
            return node.Attributes.First(attribute => attribute.Name.ToLower() == ConvertMarkerToStringNameOfAttribute(marker)).Value == attributeValue;
        }

        // experimental
        public static IFrameworkAlingmentAnalysisPlugin AnalyzerThatWon { get; set; }

        public static JdiElementTypes ApplyApplicableAnalyzers(this HtmlNode node)
        {
            var result = JdiElementTypes.Element;

            // TODO: write code here
            // // apply General

            // apply all applicable
            // TODO: use the selection the user provided
            // refactoring
            // 20160629
            // var typesOfAnalyzers = AppDomain.CurrentDomain.GetAssemblies().Where(assm => assm.FullName.Contains("JdiCodeGenerator.Core")).SelectMany(assm => assm.GetTypes()).Where(type => type.GetInterfaces().Contains(typeof(IFrameworkAlingmentAnalysisPlugin)));
            // var typesOfAnalyzers = AppDomain.CurrentDomain.GetAssemblies().Where(assm => assm.FullName.Contains("JdiCodeGenerator.Web")).SelectMany(assm => assm.GetTypes()).Where(type => type.GetInterfaces().Contains(typeof(IFrameworkAlingmentAnalysisPlugin)));
            var currentAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            var typesOfAnalyzers = AppDomain.CurrentDomain.GetAssemblies().Where(assm => assm.FullName.Contains(currentAssemblyName)).SelectMany(assm => assm.GetTypes()).Where(type => type.GetInterfaces().Contains(typeof(IFrameworkAlingmentAnalysisPlugin)));

            typesOfAnalyzers.ToList().ForEach(type => { result = (JdiElementTypes) type.GetMethod("Analyze").Invoke(Activator.CreateInstance(type), new object[] {node}); });

            return result;
        }

        public static string GetOriginalNameOfElement(this HtmlNode node)
        {
            return !string.IsNullOrEmpty(node.OriginalName) ? node.OriginalName : "*";
        }

        public static LocatorDefinition CreateDomLocatorByAttribute(this HtmlNode node, string attributeName, SearchTypePreferences searchTypePreference)
        {
            if (!node.Attributes.Contains(attributeName))
                return null;
            var attributeValue = node.Attributes[attributeName].Value;
            // fixing multi-line strings
            if (Enumerable.Contains(attributeValue, '\r'))
                attributeValue = attributeValue.Replace("\r", string.Empty);
            if (Enumerable.Contains(attributeValue, '\n'))
                attributeValue = attributeValue.Replace("\n", string.Empty);

            return new LocatorDefinition
            {
                Attribute = FindTypes.FindBy, SearchTypePreference = searchTypePreference, SearchString = attributeValue
            };
        }

        public static LocatorDefinition CreateCssLocator(this HtmlNode node)
        {
            var searchString = ExtensionMethodsForHtmlElements.GenerateElementCss(node);
            if (string.IsNullOrEmpty(searchString))
                return null;
            return new LocatorDefinition
            {
                Attribute = FindTypes.FindBy, SearchTypePreference = SearchTypePreferences.css, SearchString = searchString
            };
        }

        public static LocatorDefinition CreateXpathLocator(this HtmlNode node)
        {
            var searchString = GenerateElementXpath(node);
            return new LocatorDefinition
            {
                Attribute = FindTypes.FindBy, SearchTypePreference = SearchTypePreferences.xpath, SearchString = searchString
            };
        }

        static string GenerateElementXpath(this HtmlNode node)
        {
            var originalName = GetOriginalNameOfElement(node);
            if (WebNames.ElementTypeBody == originalName)
                return "/";

            var result = !string.IsNullOrEmpty(node.Id) ? string.Format(@"/{0}[@id='{1}']", originalName, node.Id) : HasAttribute(node, (string) WebNames.AttributeNameName) ? string.Format(@"/{0}[@name='{1}']", originalName, GetAttributeValue(node, (string) WebNames.AttributeNameName)) : HasAttribute(node, (string) WebNames.AttributeNameClass) && !GetAttributeValue(node, (string) WebNames.AttributeNameClass).Contains(" ") ? string.Format(@"/{0}[@class='{1}']", originalName, GetAttributeValue(node, (string) WebNames.AttributeNameClass))
                : string.Format(@"/{0}", originalName);
            return NodeIsAppropriateForXpathBuilding(node.ParentNode) ? GenerateElementXpath(node.ParentNode) + result : result;
        }

        static bool NodeIsAppropriateForXpathBuilding(this HtmlNode node)
        {
            if (null == node)
                return false;
            if (HtmlNodeType.Comment == node.NodeType || HtmlNodeType.Document == node.NodeType || HtmlNodeType.Text == node.NodeType)
                return false;
            return true;
        }

        public static IEnumerable<HtmlNode> GetNodesThatMatchTheCondition(this HtmlNode node, NodeRelationships relationship, Markers marker)
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

        public static bool CheckCondition(this HtmlNode node, IRuleCondition condition)
        {
            // refactoring
            // 20160628
            var nodesForCondition = GetNodesThatMatchTheCondition(node, condition.Relationship, condition.Marker);
            var forCondition = nodesForCondition as HtmlNode[] ?? nodesForCondition.ToArray();
            if (!forCondition.Any())
                return false;
            return forCondition.Any(probeNode => NodeMatchesTheCondition(probeNode, condition.Marker, condition.MarkerValues));
        }

        // refactoring
        // 20160628
        //public static bool ResolveRuleToJdiType(this HtmlNode node)
        //{
        //    return (!OrConditions.Any() || OrConditions.Any(condition => CheckCondition(node, condition))) &&
        //    (!AndConditions.Any() || AndConditions.All(condition => CheckCondition(node, condition)));
        //}
        public static bool ResolveRuleToJdiType(this IRule rule, HtmlNode node)
        {
            return (!rule.OrConditions.Any() || rule.OrConditions.Any(condition => CheckCondition(node, condition))) &&
            (!rule.AndConditions.Any() || rule.AndConditions.All(condition => CheckCondition(node, condition)));
        }

        public static bool NodeMatchesTheCondition(this HtmlNode nodeForCondition, Markers marker, List<string> markerValues)
        {
            var attributeValue = nodeForCondition.GetAttributeValue(marker);
            return markerValues.Any(markerValue => attributeValue.Contains(markerValue));
        }

        // refactoring
        // 20160628
        //public static bool IsMatch(this HtmlNode node)
        //{
        //    var elementType = new General().Analyze(node.OriginalName);
        //    if (!SourceTypes.Contains(elementType))
        //        return false;

        //    return ResolveRuleToJdiType(node);
        //}
        public static bool IsMatch(this IRule rule, HtmlNode node)
        {
            var elementType = new General().Analyze(node.OriginalName);
            if (!rule.SourceTypes.Contains(elementType))
                return false;

            return rule.ResolveRuleToJdiType(node);
        }
    }
}
