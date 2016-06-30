namespace JdiCodeGenerator.Core.Helpers
{
    using ObjectModel.Abstract;

    public static class ExtensionMethodsForJdiTypes
    {
        public static string ConvertToTypeString(this JdiElementTypes jdiType, string typeName = "")
        {
            switch (jdiType)
            {
                case JdiElementTypes.Element:
                case JdiElementTypes.Button:
                case JdiElementTypes.CheckBox:
                case JdiElementTypes.DatePicker:
                case JdiElementTypes.FileInput:
                case JdiElementTypes.Image:
                case JdiElementTypes.Label:
                case JdiElementTypes.Link:
                case JdiElementTypes.Text:
                case JdiElementTypes.TextArea:
                case JdiElementTypes.TextField:
                    return "I" + jdiType;
                case JdiElementTypes.MenuItem:
                case JdiElementTypes.TabItem:
                case JdiElementTypes.NavBar:
                case JdiElementTypes.Pager:
                case JdiElementTypes.Progress:
                case JdiElementTypes.List:
                case JdiElementTypes.ListItem:
                case JdiElementTypes.Popover:
                case JdiElementTypes.Carousel:
                // case JdiElementTypes.Table:
                    return "non_existing_I" + jdiType;
                case JdiElementTypes.CheckList:
                case JdiElementTypes.ComboBox:
                case JdiElementTypes.DropDown:
                case JdiElementTypes.DropList:
                case JdiElementTypes.Form:
                case JdiElementTypes.Group:
                case JdiElementTypes.Menu:
                case JdiElementTypes.Page:
                case JdiElementTypes.Pagination:
                case JdiElementTypes.Popup:
                case JdiElementTypes.RadioButtons:
                case JdiElementTypes.Search:
                case JdiElementTypes.Selector:
                case JdiElementTypes.Tabs:
                case JdiElementTypes.TextList:
                    return "I" + jdiType + string.Format(@"<{0}>", typeName);
                case JdiElementTypes.Table:
                    return "I" + jdiType + string.Format(@"<{0}>", typeName);
                default:
                    return "IElement";
            }
        }

        //// // TODO: rewrite this method to use rules
        //// internal static JdiElementTypes ConvertHtmlTypeToJdiType(this HtmlElementTypes htmlElementType)
        //public static JdiElementTypes ConvertHtmlTypeToJdiType(this HtmlElementTypes htmlElementType)
        //{
        //    var result = JdiElementTypes.Element;
        //    switch (htmlElementType)
        //    {
        //        case HtmlElementTypes.Unknown:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.A:
        //            result = JdiElementTypes.Link;
        //            break;
        //        case HtmlElementTypes.Abbr:
        //        case HtmlElementTypes.Acronym:
        //        case HtmlElementTypes.Address:
        //        case HtmlElementTypes.Applet:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Area:
        //            result = JdiElementTypes.TextArea;
        //            break;
        //        case HtmlElementTypes.Article:
        //            result = JdiElementTypes.TextArea;
        //            break;
        //        case HtmlElementTypes.Aside:
        //        case HtmlElementTypes.Audio:
        //        case HtmlElementTypes.B:
        //        case HtmlElementTypes.Base:
        //        case HtmlElementTypes.Basefont:
        //        case HtmlElementTypes.Bdi:
        //        case HtmlElementTypes.Bdo:
        //        case HtmlElementTypes.Big:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Blockquote:
        //            break;
        //        case HtmlElementTypes.Body:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Br:
        //            break;
        //        case HtmlElementTypes.Button:
        //            result = JdiElementTypes.Button;
        //            break;
        //        case HtmlElementTypes.Canvas:
        //            break;
        //        case HtmlElementTypes.Caption:
        //            break;
        //        case HtmlElementTypes.Center:
        //            break;
        //        case HtmlElementTypes.Cite:
        //            break;
        //        case HtmlElementTypes.Code:
        //            break;
        //        case HtmlElementTypes.Col:
        //            break;
        //        case HtmlElementTypes.Colgroup:
        //            break;
        //        case HtmlElementTypes.Datalist:
        //        case HtmlElementTypes.Dd:
        //        case HtmlElementTypes.Del:
        //        case HtmlElementTypes.Details:
        //        case HtmlElementTypes.Dfn:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Dialog:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Dir:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Div:
        //            // result = texta
        //            break;
        //        case HtmlElementTypes.Dl:
        //        case HtmlElementTypes.Dt:
        //        case HtmlElementTypes.Em:
        //        case HtmlElementTypes.Embed:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Fieldset:
        //            break;
        //        case HtmlElementTypes.Figcaption:
        //            break;
        //        case HtmlElementTypes.Figure:
        //            break;
        //        case HtmlElementTypes.Font:
        //            break;
        //        case HtmlElementTypes.Footer:
        //            break;
        //        case HtmlElementTypes.Form:
        //            break;
        //        case HtmlElementTypes.Frame:
        //            break;
        //        case HtmlElementTypes.Frameset:
        //            break;
        //        case HtmlElementTypes.H1:
        //        case HtmlElementTypes.H2:
        //        case HtmlElementTypes.H3:
        //        case HtmlElementTypes.H4:
        //        case HtmlElementTypes.H5:
        //        case HtmlElementTypes.H6:
        //            result = JdiElementTypes.Label;
        //            break;
        //        case HtmlElementTypes.Head:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Header:
        //            break;
        //        case HtmlElementTypes.Hr:
        //        case HtmlElementTypes.Html:
        //        case HtmlElementTypes.I:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Iframe:
        //            break;
        //        case HtmlElementTypes.Img:
        //            result = JdiElementTypes.Image;
        //            break;
        //        case HtmlElementTypes.Input:
        //            result = JdiElementTypes.TextField; // ??
        //            break;
        //        case HtmlElementTypes.Ins:
        //        case HtmlElementTypes.Kbd:
        //        case HtmlElementTypes.Keygen:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Label:
        //            result = JdiElementTypes.Label;
        //            break;
        //        case HtmlElementTypes.Legend:
        //            break;
        //        case HtmlElementTypes.Li:
        //            break;
        //        case HtmlElementTypes.Link:
        //            result = JdiElementTypes.Link; // ??
        //            break;
        //        case HtmlElementTypes.Main:
        //        case HtmlElementTypes.Map:
        //        case HtmlElementTypes.Mark:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Menu:
        //            break;
        //        case HtmlElementTypes.Menuitem:
        //            break;
        //        case HtmlElementTypes.Meta:
        //        case HtmlElementTypes.Meter:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Nav:
        //            break;
        //        case HtmlElementTypes.Noframes:
        //        case HtmlElementTypes.Noscript:
        //        case HtmlElementTypes.Object:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Ol:
        //            break;
        //        case HtmlElementTypes.Optgroup:
        //            break;
        //        case HtmlElementTypes.Option:
        //            // result = jdi
        //            break;
        //        case HtmlElementTypes.Output:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.P:
        //            result = JdiElementTypes.TextArea;
        //            break;
        //        case HtmlElementTypes.Param:
        //        case HtmlElementTypes.Pre:
        //        case HtmlElementTypes.Progress:
        //        case HtmlElementTypes.Q:
        //        case HtmlElementTypes.Rp:
        //        case HtmlElementTypes.Rt:
        //        case HtmlElementTypes.Ruby:
        //        case HtmlElementTypes.S:
        //        case HtmlElementTypes.Samp:
        //        case HtmlElementTypes.Script:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Section:
        //            break;
        //        case HtmlElementTypes.Select:
        //            result = JdiElementTypes.CheckBox;
        //            break;
        //        case HtmlElementTypes.Small:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Source:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Span:
        //            break;
        //        case HtmlElementTypes.Strike:
        //        case HtmlElementTypes.Strong:
        //        case HtmlElementTypes.Style:
        //        case HtmlElementTypes.Sub:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Summary:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Sup:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Table:
        //            break;
        //        case HtmlElementTypes.Tbody:
        //            break;
        //        case HtmlElementTypes.Td:
        //            break;
        //        case HtmlElementTypes.Textarea:
        //            break;
        //        case HtmlElementTypes.Tfoot:
        //            break;
        //        case HtmlElementTypes.Th:
        //            break;
        //        case HtmlElementTypes.Thead:
        //            break;
        //        case HtmlElementTypes.Time:
        //        case HtmlElementTypes.Title:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Tr:
        //            break;
        //        case HtmlElementTypes.Track:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Tt:
        //            break;
        //        case HtmlElementTypes.U:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Ul:
        //            break;
        //        case HtmlElementTypes.Var:
        //        case HtmlElementTypes.Video:
        //            result = JdiElementTypes.Element;
        //            break;
        //        case HtmlElementTypes.Wbr:
        //            result = JdiElementTypes.Element;
        //            break;
        //        default:
        //            result = JdiElementTypes.Element;
        //            break;
        //    }
        //    return result;
        //}
    }
}