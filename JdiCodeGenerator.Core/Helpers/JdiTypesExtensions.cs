namespace JdiCodeGenerator.Core.Helpers
{
    using ObjectModel.Abstract;
    using ObjectModel.Enums;

    public static class JdiTypesExtensions
    {
        public static string ConvertToTypeString(this JdiElementTypes jdiType, string typeName = "")
        {
            switch (jdiType)
            {
                case JdiElementTypes.Element:
                case JdiElementTypes.Button:
                case JdiElementTypes.CheckBox:
                case JdiElementTypes.DatePicker:
                case JdiElementTypes.TimePicker:
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

        public static bool IsComplexControl(this JdiElementTypes jdiElementType)
        {
            switch (jdiElementType)
            {
                case JdiElementTypes.Element:
                case JdiElementTypes.Button:
                    return false;
                case JdiElementTypes.CheckBox:
                    break;
                case JdiElementTypes.DatePicker:
                case JdiElementTypes.TimePicker:
                case JdiElementTypes.FileInput:
                case JdiElementTypes.Image:
                case JdiElementTypes.Label:
                case JdiElementTypes.Link:
                case JdiElementTypes.Text:
                case JdiElementTypes.TextArea:
                case JdiElementTypes.TextField:
                    return false;
                case JdiElementTypes.MenuItem:
                    break;
                case JdiElementTypes.TabItem:
                    break;
                case JdiElementTypes.NavBar:
                    break;
                case JdiElementTypes.Pager:
                    break;
                case JdiElementTypes.Progress:
                    break;
                case JdiElementTypes.List:
                    break;
                case JdiElementTypes.ListItem:
                    break;
                case JdiElementTypes.Popover:
                    break;
                case JdiElementTypes.Carousel:
                    break;
                case JdiElementTypes.CheckList:
                case JdiElementTypes.ComboBox:
                case JdiElementTypes.DropDown:
                case JdiElementTypes.DropList:
                case JdiElementTypes.Form:
                    return true;
                case JdiElementTypes.Group:
                    break;
                case JdiElementTypes.Menu:
                    break;
                case JdiElementTypes.Page:
                    break;
                case JdiElementTypes.Pagination:
                    break;
                case JdiElementTypes.Popup:
                    break;
                case JdiElementTypes.RadioButtons:
                    return true;
                case JdiElementTypes.Search:
                    break;
                case JdiElementTypes.Selector:
                    break;
                case JdiElementTypes.Tabs:
                    break;
                case JdiElementTypes.TextList:
                    break;
                case JdiElementTypes.Table:
                    break;
                case JdiElementTypes.Cell:
                    break;
                case JdiElementTypes.Column:
                    break;
                case JdiElementTypes.Coulmns:
                    break;
                case JdiElementTypes.DynamicTable:
                    break;
                case JdiElementTypes.ElementIndexType:
                    break;
                case JdiElementTypes.Row:
                    break;
                case JdiElementTypes.RowColumn:
                    break;
                case JdiElementTypes.Rows:
                    break;
                case JdiElementTypes.TableLine:
                    break;
                default:
                    return false;
            }
            return false;
        }
    }
}