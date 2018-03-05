namespace CodeGenerator.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Core.ObjectModel.Abstract.Results;
    using Core.ObjectModel.Abstract.Rules;
    using Core.ObjectModel.Enums;
    using Core.ObjectModel.Results;
    using HtmlAgilityPack;
    using JdiConverters.ObjectModel.Enums;
    using ObjectModel.Abstract;

    public static class HtmlNodesExtensions
    {
        public static bool HasAttribute(this HtmlNode node, string attributeName)
        {
            if (WebNames.AttributeNameTag == attributeName.ToLower())
                return true;
            return node.Attributes.Any(attribute => attribute.Name.ToLower() == attributeName);
        }

        public static bool HasAttribute(this HtmlNode node, MarkerAttributes markerAttribute)
        {
            return MarkerAttributes.OtherAttribute == markerAttribute ? node.HasAttributes : HasAttribute(node, ConvertMarkerToStringNameOfAttribute(markerAttribute));
        }

        static string ConvertMarkerToStringNameOfAttribute(this MarkerAttributes markerAttribute)
        {
            return markerAttribute.ToString().ToLower();
        }

        public static string GetAttributeValue(this HtmlNode node, string attributeName)
        {
            if (null == node)
                return string.Empty;
            if (WebNames.AttributeNameTag == attributeName.ToLower())
                return node.OriginalName;
            return NodeWithAttributes(node) ? node.Attributes.First(attribute => attribute.Name.ToLower() == attributeName).Value : string.Empty;
        }

        public static string GetAttributeValue(this HtmlNode node, MarkerAttributes markerAttribute)
        {
            if (null == node)
                return string.Empty;
            if (MarkerAttributes.Tag == markerAttribute)
                return node.OriginalName;
            return NodeWithAttributes(node) ? node.Attributes.First(attribute => attribute.Name.ToLower() == ConvertMarkerToStringNameOfAttribute(markerAttribute)).Value : string.Empty;
        }

        public static string GetAttributeValue(this HtmlNode node, MarkerAttributes markerAttribute, string attributeValue)
        {
            if (null == node)
                return string.Empty;
            if (MarkerAttributes.Tag == markerAttribute)
                return node.OriginalName;
            // return NodeWithAttributes(node) ? node.Attributes.First(attribute => attribute.Name.ToLower() == ConvertMarkerToStringNameOfAttribute(markerAttribute)).Value : string.Empty;
            if (MarkerAttributes.OtherAttribute != markerAttribute)
                return string.Empty;
            return NodeWithAttributes(node) ? node.Attributes.First(attribute => attributeValue == attribute.Value).Value : string.Empty;
        }

        static bool NodeWithAttributes(HtmlNode node)
        {
            return null != node?.Attributes && node.Attributes.Any();
        }

        public static bool HasAttributeValue(this HtmlNode node, string attributeName, string attributeValue)
        {
            return node.Attributes.First(attribute => attribute.Name.ToLower() == attributeName).Value == attributeValue;
        }

        // experimental
        public static IFrameworkAlingmentAnalysisPlugin<HtmlElementTypes> AnalyzerThatWon { get; set; }

        public static JdiElementTypes ApplyApplicableAnalyzers(this HtmlNode node, Type[] analyzers)
        {
            var result = JdiElementTypes.Element;

            // TODO: write code here
            // // apply General

            // apply all applicable
            // TODO: use the selection the user provided
            // refactoring
            // 20160706
            // var currentAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            // var typesOfAnalyzers = AppDomain.CurrentDomain.GetAssemblies().Where(assm => assm.FullName.Contains(currentAssemblyName)).SelectMany(assm => assm.GetTypes()).Where(type => type.GetInterfaces().Contains(typeof(IFrameworkAlingmentAnalysisPlugin<HtmlElementTypes>)) && !type.IsAbstract);
            // typesOfAnalyzers.OrderByDescending(type => (int)type.GetProperty(Resources.RuleMember_Priority).GetValue(Activator.CreateInstance(type)))
            analyzers.OrderByDescending(type => (int)type.GetProperty(Resources.RuleMember_Priority).GetValue(Activator.CreateInstance(type)))
                .ToList()
                .ForEach(type =>
                {
                    var preliminaryResult = (JdiElementTypes)type.GetMethod(Resources.RuleMember_Analyze).Invoke(Activator.CreateInstance(type), new object[] { node });
                    if (JdiElementTypes.Element != preliminaryResult)
                        result = preliminaryResult;
                });

            return result;
        }

        public static string GetOriginalNameOfElement(this HtmlNode node)
        {
            return !string.IsNullOrEmpty(node.OriginalName) ? node.OriginalName : "*";
        }

        public static LocatorDefinition CreateDomLocatorByAttribute(this HtmlNode node, string attributeName, ElementSearchTypePreferences elementSearchTypePreference)
        {
            if (!node.Attributes.Contains(attributeName))
                return null;
            var attributeValue = node.Attributes[attributeName].Value;
            // fixing multi-line strings
            if (attributeValue.Contains('\r'))
                attributeValue = attributeValue.Replace("\r", string.Empty);
            if (attributeValue.Contains('\n'))
                attributeValue = attributeValue.Replace("\n", string.Empty);

            return new LocatorDefinition
            {
                Attribute = FindAnnotationTypes.FindBy, ElementSearchTypePreference = elementSearchTypePreference, SearchString = attributeValue
            };
        }

        public static LocatorDefinition CreateCssLocator(this HtmlNode node)
        {
            var searchString = node.GenerateElementCss();
            if (string.IsNullOrEmpty(searchString))
                return null;
            return new LocatorDefinition
            {
                Attribute = FindAnnotationTypes.FindBy, ElementSearchTypePreference = ElementSearchTypePreferences.css, SearchString = searchString
            };
        }

        public static LocatorDefinition CreateXpathLocator(this HtmlNode node)
        {
            var searchString = GenerateElementXpath(node);
            return new LocatorDefinition
            {
                Attribute = FindAnnotationTypes.FindBy, ElementSearchTypePreference = ElementSearchTypePreferences.xpath, SearchString = searchString
            };
        }

        static string GenerateElementXpath(this HtmlNode node)
        {
            var originalName = GetOriginalNameOfElement(node);
            if (WebNames.ElementTypeBody == originalName)
                return "/";
            var result = !string.IsNullOrEmpty(node.Id)
                ? $"/{originalName}[@id='{node.Id}']"
                : HasAttribute(node, (string) WebNames.AttributeNameName)
                    ? $"/{originalName}[@name='{GetAttributeValue(node, (string) WebNames.AttributeNameName)}']"
                    : HasAttribute(node, (string) WebNames.AttributeNameClass) &&
                      !GetAttributeValue(node, (string) WebNames.AttributeNameClass).Contains(" ")
                        ? $"/{originalName}[@class='{GetAttributeValue(node, (string) WebNames.AttributeNameClass)}']"
                        : $@"/{originalName}";
    
            return NodeIsAppropriateForXpathBuilding(node.ParentNode) ? GenerateElementXpath(node.ParentNode) + result : result;
        }

        static bool NodeIsAppropriateForXpathBuilding(this HtmlNode node)
        {
            if (null == node) return false;
            return HtmlNodeType.Comment != node.NodeType && HtmlNodeType.Document != node.NodeType && HtmlNodeType.Text != node.NodeType;
        }

        public static IEnumerable<HtmlNode> GetNodesThatMatchTheCondition(this HtmlNode node, NodeRelationships relationship, MarkerAttributes markerAttribute)
        {
            // TODO: refactor this!
            switch (relationship)
            {
                case NodeRelationships.Self:
                    var result = node.HasAttribute(markerAttribute) ? new List<HtmlNode> { node } : new List<HtmlNode> { null };
                    return result;
                case NodeRelationships.Sibling:
                    // TODO: write better code!
                    return new List<HtmlNode> { null };
                case NodeRelationships.Parent:
                    return node.ParentNode.HasAttribute(markerAttribute) ? new List<HtmlNode> { node.ParentNode } : new List<HtmlNode> { null };
                case NodeRelationships.Ancestor:
                    return node.Ancestors().Any(ancestor => ancestor.HasAttribute(markerAttribute))
                        ? node.Ancestors().Where(ancestor => ancestor.HasAttribute(markerAttribute)).ToList()
                        : new List<HtmlNode> { null };
                case NodeRelationships.Child:
                    return node.ChildNodes.Any(childNode => childNode.HasAttribute(markerAttribute))
                        ? node.ChildNodes.Where(childNode => childNode.HasAttribute(markerAttribute)).ToList()
                        : new List<HtmlNode> { null };
                //return node.SelectNodes("*").Any(childNode => childNode.HasAttribute(markerAttribute))
                //    ? node.SelectNodes("*").Where(childNode => childNode.HasAttribute(markerAttribute)).ToList()
                //    : new List<HtmlNode> { null };
                case NodeRelationships.Descendant:
                    //return node.Descendants().Any(descendant => descendant.HasAttribute(markerAttribute))
                    //    ? node.Descendants().Where(childNode => childNode.HasAttribute(markerAttribute)).ToList()
                    //    : new List<HtmlNode> { null };
                    var result2 = node.Descendants().Any(descendant => descendant.HasAttribute(markerAttribute))
                        ? node.Descendants().Where(childNode => childNode.HasAttribute(markerAttribute)).ToList()
                        : new List<HtmlNode> { null };
                    //var result2 = node.SelectNodes("*").Any(descendant => descendant.HasAttribute(markerAttribute))
                    //    ? node.SelectNodes("*").Where(childNode => childNode.HasAttribute(markerAttribute)).ToList()
                    //    : new List<HtmlNode> { null };
                    return result2;
                default:
                    return new List<HtmlNode> { null };
            }
        }

        public static bool CheckCondition(this HtmlNode node, IRuleCondition condition)
        {
            var nodesForCondition = GetNodesThatMatchTheCondition(node, condition.NodeRelationship, condition.MarkerAttribute);
            var forCondition = nodesForCondition as HtmlNode[] ?? nodesForCondition.ToArray();
            if (!forCondition.Any())
                return false;
            return forCondition.Any(probeNode => NodeMatchesTheCondition(probeNode, condition.MarkerAttribute, condition.MarkerValues));
        }

        public static bool IsRuleResolvableToJdiType(this IRule<HtmlElementTypes> rule, HtmlNode node)
        {
            return (!rule.OrConditions.Any() || rule.OrConditions.Any(condition => CheckCondition(node, condition))) &&
            (!rule.AndConditions.Any() || rule.AndConditions.All(condition => CheckCondition(node, condition)));
        }

        public static bool NodeMatchesTheCondition(this HtmlNode nodeForCondition, MarkerAttributes markerAttribute, List<string> markerValues)
        {
            if (null == nodeForCondition)
                return false;

            if (MarkerAttributes.OtherAttribute == markerAttribute)
                return markerValues.Any(markerValue => nodeForCondition.Attributes.Any(attribute => markerValue == attribute.Value));

            var attributeValue = nodeForCondition.GetAttributeValue(markerAttribute);
            return markerValues.Any(markerValue => attributeValue.Contains(markerValue));
        }

        public static bool IsMatch(this IRule<HtmlElementTypes> rule, HtmlNode node)
        {
            var elementType = node.OriginalName.ConvertOriginalHtmlElementNameIntoHtmlElementType();
            if (!rule.SourceTypes.Contains(elementType))
                return false;

            return rule.IsRuleResolvableToJdiType(node);
        }

        public static IPageMemberCodeEntry ConvertToCodeEntry(this HtmlNode node)
        {
            var codeEntry = new PageMemberCodeEntry();
            codeEntry.SourceMemberType.Set(new List<HtmlElementTypes> { node.OriginalName.ConvertOriginalHtmlElementNameIntoHtmlElementType() });
            //....ToString()// .Set(new List<HtmlElementTypes> { node.OriginalName.ConvertOriginalHtmlElementNameIntoHtmlElementType() });

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

            codeEntry.Locators.ForEach(locator => locator.IsBestChoice = false);
            codeEntry.Locators.OrderBy(locator => (int)locator.ElementSearchTypePreference).First().IsBestChoice = true;

            return codeEntry;
        }
    }
}
