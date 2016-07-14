namespace JdiCodeGenerator.Web.Helpers
{
    using System;
    using System.Linq;
    using HtmlAgilityPack;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;
    using Core.ObjectModel.Enums;
    using ObjectModel.Abstract;

    public static class HtmlElementsExtensions
    {
        public static string GenerateElementCss(this HtmlNode node)
        {
            var originalName = node.GetOriginalNameOfElement();

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
            if (node.HasAttribute((string) WebNames.AttributeNameName))
                result += string.Format(@"[name='{0}']", node.GetAttributeValue((string) WebNames.AttributeNameName));

            // [title*='Hello beautiful']
            // [title='Hello beautiful']
            if (node.HasAttribute("title"))
                result += string.Format(@"[title='{0}']",
                    node.GetAttributeValue("title"));

            return result;
        }

        public static LocatorDefinition CreateClassLocator(this HtmlNode node)
        {
            return node.CreateDomLocatorByAttribute(WebNames.AttributeNameClass, SearchTypePreferences.className);
        }

        public static LocatorDefinition CreateTagLocator(this HtmlNode node)
        {
            return node.CreateDomLocatorByAttribute(WebNames.AttributeNameTag, SearchTypePreferences.tagName);
        }

        public static LocatorDefinition CreateIdLocator(this HtmlNode node)
        {
            return node.CreateDomLocatorByAttribute(WebNames.AttributeNameId, SearchTypePreferences.id);
        }

        public static LocatorDefinition CreateNameLocator(this HtmlNode node)
        {
            return node.CreateDomLocatorByAttribute(WebNames.AttributeNameName, SearchTypePreferences.name);
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
                SearchTypePreference = fullLinkTextParts.Any() ? SearchTypePreferences.partialLinkText : SearchTypePreferences.linkText,
                SearchString = fullLinkTextParts.Any() ? fullLinkTextParts.OrderByDescending(part => part.Length).First().Trim() : fullLinkText
            };
        }

        // // TODO: rewrite this method to use rules
        // internal static JdiElementTypes ConvertHtmlTypeToJdiType(this HtmlElementTypes htmlElementType)
        public static JdiElementTypes ConvertHtmlTypeToJdiType(this HtmlElementTypes htmlElementType)
        {
            var result = JdiElementTypes.Element;
            switch (htmlElementType)
            {
                case HtmlElementTypes.Unknown:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.A:
                    result = JdiElementTypes.Link;
                    break;
                case HtmlElementTypes.Abbr:
                case HtmlElementTypes.Acronym:
                case HtmlElementTypes.Address:
                case HtmlElementTypes.Applet:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Area:
                    result = JdiElementTypes.TextArea;
                    break;
                case HtmlElementTypes.Article:
                    result = JdiElementTypes.TextArea;
                    break;
                case HtmlElementTypes.Aside:
                case HtmlElementTypes.Audio:
                case HtmlElementTypes.B:
                case HtmlElementTypes.Base:
                case HtmlElementTypes.Basefont:
                case HtmlElementTypes.Bdi:
                case HtmlElementTypes.Bdo:
                case HtmlElementTypes.Big:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Blockquote:
                    break;
                case HtmlElementTypes.Body:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Br:
                    break;
                case HtmlElementTypes.Button:
                    result = JdiElementTypes.Button;
                    break;
                case HtmlElementTypes.Canvas:
                    break;
                case HtmlElementTypes.Caption:
                    break;
                case HtmlElementTypes.Center:
                    break;
                case HtmlElementTypes.Cite:
                    break;
                case HtmlElementTypes.Code:
                    break;
                case HtmlElementTypes.Col:
                    break;
                case HtmlElementTypes.Colgroup:
                    break;
                case HtmlElementTypes.Datalist:
                case HtmlElementTypes.Dd:
                case HtmlElementTypes.Del:
                case HtmlElementTypes.Details:
                case HtmlElementTypes.Dfn:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Dialog:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Dir:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Div:
                    // result = texta
                    break;
                case HtmlElementTypes.Dl:
                case HtmlElementTypes.Dt:
                case HtmlElementTypes.Em:
                case HtmlElementTypes.Embed:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Fieldset:
                    break;
                case HtmlElementTypes.Figcaption:
                    break;
                case HtmlElementTypes.Figure:
                    break;
                case HtmlElementTypes.Font:
                    break;
                case HtmlElementTypes.Footer:
                    break;
                case HtmlElementTypes.Form:
                    break;
                case HtmlElementTypes.Frame:
                    break;
                case HtmlElementTypes.Frameset:
                    break;
                case HtmlElementTypes.H1:
                case HtmlElementTypes.H2:
                case HtmlElementTypes.H3:
                case HtmlElementTypes.H4:
                case HtmlElementTypes.H5:
                case HtmlElementTypes.H6:
                    result = JdiElementTypes.Label;
                    break;
                case HtmlElementTypes.Head:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Header:
                    break;
                case HtmlElementTypes.Hr:
                case HtmlElementTypes.Html:
                case HtmlElementTypes.I:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Iframe:
                    break;
                case HtmlElementTypes.Img:
                    result = JdiElementTypes.Image;
                    break;
                case HtmlElementTypes.Input:
                    result = JdiElementTypes.TextField; // ??
                    break;
                case HtmlElementTypes.Ins:
                case HtmlElementTypes.Kbd:
                case HtmlElementTypes.Keygen:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Label:
                    result = JdiElementTypes.Label;
                    break;
                case HtmlElementTypes.Legend:
                    break;
                case HtmlElementTypes.Li:
                    break;
                case HtmlElementTypes.Link:
                    result = JdiElementTypes.Link; // ??
                    break;
                case HtmlElementTypes.Main:
                case HtmlElementTypes.Map:
                case HtmlElementTypes.Mark:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Menu:
                    break;
                case HtmlElementTypes.Menuitem:
                    break;
                case HtmlElementTypes.Meta:
                case HtmlElementTypes.Meter:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Nav:
                    break;
                case HtmlElementTypes.Noframes:
                case HtmlElementTypes.Noscript:
                case HtmlElementTypes.Object:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Ol:
                    break;
                case HtmlElementTypes.Optgroup:
                    break;
                case HtmlElementTypes.Option:
                    // result = jdi
                    break;
                case HtmlElementTypes.Output:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.P:
                    result = JdiElementTypes.TextArea;
                    break;
                case HtmlElementTypes.Param:
                case HtmlElementTypes.Pre:
                case HtmlElementTypes.Progress:
                case HtmlElementTypes.Q:
                case HtmlElementTypes.Rp:
                case HtmlElementTypes.Rt:
                case HtmlElementTypes.Ruby:
                case HtmlElementTypes.S:
                case HtmlElementTypes.Samp:
                case HtmlElementTypes.Script:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Section:
                    break;
                case HtmlElementTypes.Select:
                    result = JdiElementTypes.CheckBox;
                    break;
                case HtmlElementTypes.Small:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Source:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Span:
                    break;
                case HtmlElementTypes.Strike:
                case HtmlElementTypes.Strong:
                case HtmlElementTypes.Style:
                case HtmlElementTypes.Sub:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Summary:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Sup:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Table:
                    break;
                case HtmlElementTypes.Tbody:
                    break;
                case HtmlElementTypes.Td:
                    break;
                case HtmlElementTypes.Textarea:
                    break;
                case HtmlElementTypes.Tfoot:
                    break;
                case HtmlElementTypes.Th:
                    break;
                case HtmlElementTypes.Thead:
                    break;
                case HtmlElementTypes.Time:
                case HtmlElementTypes.Title:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Tr:
                    break;
                case HtmlElementTypes.Track:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Tt:
                    break;
                case HtmlElementTypes.U:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Ul:
                    break;
                case HtmlElementTypes.Var:
                case HtmlElementTypes.Video:
                    result = JdiElementTypes.Element;
                    break;
                case HtmlElementTypes.Wbr:
                    result = JdiElementTypes.Element;
                    break;
                default:
                    result = JdiElementTypes.Element;
                    break;
            }
            return result;
        }

        public static HtmlElementTypes ConvertOriginalHtmlElementNameIntoHtmlElementType(this string originalName)
        {
            var result = HtmlElementTypes.Unknown;

            Enum.GetValues(typeof(HtmlElementTypes))
                .Cast<HtmlElementTypes>()
                .ToList()
                .ForEach(htmlElementType =>
                {
                    // if (null != node.OriginalName && string.CompareOrdinal(htmlElementType.ToString(), node.OriginalName) != 0)
                    if (!string.IsNullOrEmpty(originalName) && string.CompareOrdinal(htmlElementType.ToString().ToLower(), originalName) == 0)
                    {
                        result = htmlElementType;
                        return;
                    }
                });

            return result;
        }
    }
}