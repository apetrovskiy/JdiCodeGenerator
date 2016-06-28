namespace JdiCodeGenerator.Web.Helpers
{
    using System.Linq;
    using HtmlAgilityPack;
    using Core;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;

    public static class ExtensionMethodsForHtmlElements
    {
        public static string GenerateElementCss(this HtmlNode node)
        {
            var originalName = ExtensionMethodsForNodes.GetOriginalNameOfElement(node);

            if (Enumerable.Contains(new[]
            {
                WebNames.ElementTypeHtml,
                WebNames.ElementTypeHead,
                WebNames.ElementTypeBody,
                WebNames.ElementTypeComment,
                WebNames.ElementTypeText
            }, originalName))
                return string.Empty;

            var result = string.Empty;

            // [id='createUserId']
            if (!string.IsNullOrEmpty(node.Id))
                result += string.Format((string) @"[id='{0}']", (object) node.Id);
            // [name='createUser']
            if (ExtensionMethodsForNodes.HasAttribute(node, (string) WebNames.AttributeNameName))
                result += string.Format(@"[name='{0}']", ExtensionMethodsForNodes.GetAttributeValue(node, (string) WebNames.AttributeNameName));

            // [title*='Hello beautiful']
            // [title='Hello beautiful']
            if (ExtensionMethodsForNodes.HasAttribute(node, "title"))
                result += string.Format(@"[title='{0}']",
                    ExtensionMethodsForNodes.GetAttributeValue(node, "title"));

            return result;
        }

        public static LocatorDefinition CreateClassLocator(this HtmlNode node)
        {
            return ExtensionMethodsForNodes.CreateDomLocatorByAttribute(node, WebNames.AttributeNameClass, SearchTypePreferences.className);
        }

        public static LocatorDefinition CreateTagLocator(this HtmlNode node)
        {
            return ExtensionMethodsForNodes.CreateDomLocatorByAttribute(node, WebNames.AttributeNameTag, SearchTypePreferences.tagName);
        }

        public static LocatorDefinition CreateIdLocator(this HtmlNode node)
        {
            return ExtensionMethodsForNodes.CreateDomLocatorByAttribute(node, WebNames.AttributeNameId, SearchTypePreferences.id);
        }

        public static LocatorDefinition CreateNameLocator(this HtmlNode node)
        {
            return ExtensionMethodsForNodes.CreateDomLocatorByAttribute(node, WebNames.AttributeNameName, SearchTypePreferences.name);
        }

        public static LocatorDefinition CreateLinkTextLocator(this HtmlNode node)
        {
            // return node.CreateDomLocatorByAttribute(WebNames.AttributeNameHref, SearchTypePreferences.linkText);
            var fullLinkText = node.InnerText;

            // experimental
            fullLinkText = fullLinkText.Replace("\"", "\\\"");

            string[] fullLinkTextParts = {};
            if (fullLinkText.Contains("\r"))
                fullLinkTextParts = fullLinkText.Split('\r');
            if (fullLinkText.Contains("\n"))
                fullLinkTextParts = fullLinkText.Split('\n');
            return new LocatorDefinition
            {
                Attribute = FindTypes.FindBy,
                // SearchTypePreference = SearchTypePreferences.linkText,
                SearchTypePreference = fullLinkTextParts.Any() ? SearchTypePreferences.partialLinkText : SearchTypePreferences.linkText,
                // SearchString = node.InnerText
                SearchString = fullLinkTextParts.Any() ? fullLinkTextParts.OrderByDescending(part => part.Length).First() : fullLinkText
            };
        }
    }
}