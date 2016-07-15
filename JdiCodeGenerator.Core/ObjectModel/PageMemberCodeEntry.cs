namespace JdiCodeGenerator.Core.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstract;
    using Enums;
    using Helpers;

    public class PageMemberCodeEntry<T> : IPageMemberCodeEntry<T>
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public List<LocatorDefinition> Locators { get; set; }
        public string MemberName { get; set; }
        public List<T> SourceMemberType { get; set; }
        public JdiElementTypes JdiMemberType { get; set; }
        // for debugging purposes
        // public string MemberType { get; set; }
        string _memberType;
        public string MemberType
        {
            get { return _memberType;  }
            set { _memberType = value.CleanUpFromWrongCharacters(); }
        }

        public string RuleThatWon { get; set; }
        // public string Type { get; set; }
        public PageMemberCodeEntryTypes Type
        {
            get
            {
                switch (JdiMemberType)
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
                    case JdiElementTypes.MenuItem:
                    case JdiElementTypes.TabItem:
                        return PageMemberCodeEntryTypes.Simple;
                    case JdiElementTypes.NavBar:
                    case JdiElementTypes.Pager:
                    case JdiElementTypes.Progress:
                    case JdiElementTypes.List:
                    case JdiElementTypes.ListItem:
                    case JdiElementTypes.Popover:
                    case JdiElementTypes.Carousel:
                    case JdiElementTypes.CheckList:
                        return PageMemberCodeEntryTypes.Unknown;
                    case JdiElementTypes.ComboBox:
                        return PageMemberCodeEntryTypes.ComplexWithConstructor;
                    case JdiElementTypes.DropDown:
                        return PageMemberCodeEntryTypes.ComplexWithAnnotations;
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
                        return PageMemberCodeEntryTypes.ComplexWithConstructor;
                    case JdiElementTypes.Table:
                        return PageMemberCodeEntryTypes.ComplexWithConstructor;
                    case JdiElementTypes.Cell:
                    case JdiElementTypes.Column:
                    case JdiElementTypes.Coulmns:
                    case JdiElementTypes.DynamicTable:
                    case JdiElementTypes.ElementIndexType:
                    case JdiElementTypes.Row:
                    case JdiElementTypes.RowColumn:
                    case JdiElementTypes.Rows:
                    case JdiElementTypes.TableLine:
                        return PageMemberCodeEntryTypes.Unknown;
                    //case JdiElementTypes.StopProcessing:
                    //    break;
                    default:
                        return PageMemberCodeEntryTypes.Simple;
                }
            }
        }

        public bool ProcessChildren
        {
            get
            {
                switch (JdiMemberType)
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
                        return true;
                    case JdiElementTypes.MenuItem:
                    case JdiElementTypes.TabItem:
                    case JdiElementTypes.NavBar:
                    case JdiElementTypes.Pager:
                    case JdiElementTypes.Progress:
                    case JdiElementTypes.List:
                    case JdiElementTypes.ListItem:
                    case JdiElementTypes.Popover:
                    case JdiElementTypes.Carousel:
                        return true;
                    case JdiElementTypes.CheckList:
                    case JdiElementTypes.ComboBox:
                    case JdiElementTypes.DropDown:
                    case JdiElementTypes.DropList:
                        return false;
                    case JdiElementTypes.Form:
                        // TODO: generate elements during form processing
                        return true;
                    case JdiElementTypes.Group:
                        return false;
                    case JdiElementTypes.Menu:
                        return false;
                    case JdiElementTypes.Page:
                        return true;
                    case JdiElementTypes.Pagination:
                        return false;
                    case JdiElementTypes.Popup:
                        // TODO: generate elements during form processing
                        return true;
                    case JdiElementTypes.RadioButtons:
                        return false;
                    case JdiElementTypes.Search:
                        return true;
                    case JdiElementTypes.Selector:
                        return true;
                    case JdiElementTypes.Tabs:
                        return false;
                    case JdiElementTypes.TextList:
                        return false;
                    case JdiElementTypes.Table:
                    case JdiElementTypes.Cell:
                    case JdiElementTypes.Column:
                    case JdiElementTypes.Coulmns:
                    case JdiElementTypes.DynamicTable:
                    case JdiElementTypes.ElementIndexType:
                    case JdiElementTypes.Row:
                    case JdiElementTypes.RowColumn:
                    case JdiElementTypes.Rows:
                    case JdiElementTypes.TableLine:
                        // TODO: generate elements during form processing
                        return false;
                    default:
                        return true;
                }
            }
        }

        public LocatorDefinition Root { get; set; }
        public LocatorDefinition Value { get; set; }
        public LocatorDefinition List { get; set; }
        public List<string> ListMemberNames { get; set; }
        // public Guid DependsOn { get; set; }

        SupportedLanguages _language;

        public PageMemberCodeEntry()
        {
            Id = Guid.NewGuid();
            Locators = new List<LocatorDefinition>();
            ListMemberNames = new List<string>();
            CodeClass = PiecesOfCodeClasses.PageMember;
        }

        public string GenerateCode(SupportedLanguages language)
        {
            var result = string.Empty;

            // TODO: for the future use
            _language = language;

            // FilterOutWrongLocators();

            result = GenerateCodeEntryWithBestLocator();

            return result;
        }

        public PiecesOfCodeClasses CodeClass { get; set; }

        public string EnumerationTypeName { get; set; }
        public string AnalyzerThatWon { get; set; }

        //void FilterOutWrongLocators()
        //{
        //    // TODO: test with Selenium
        //    Locators.ForEach(locator =>
        //    {
        //        // if () outside the screen
        //    });
        //}

        internal string GenerateCodeEntryWithBestLocator()
        {
            if (!Locators.Any())
                return string.Empty;

            var bestLocator = Locators.First(locator => locator.IsBestChoice);
            var result = string.Empty;
            if (SupportedLanguages.Java == _language)
                result = string.Format("\r\n@{0}({1}=\"{2}\")", bestLocator.Attribute, bestLocator.SearchTypePreference, bestLocator.SearchString);
            if (SupportedLanguages.CSharp == _language)
                result = string.Format("\r\n[{0}({1}=\"{2}\")]", bestLocator.Attribute, bestLocator.SearchTypePreference, bestLocator.SearchString);

            /*
            @JDropdown(root = @FindBy(css = "dropdown"), value = @FindBy(id = "dropdownMenu1"), list = @FindBy(tagName = "li"))
            IDropDown<JobCategories> category;
            */
            if (JdiMemberType.IsComplexControl())
                result += GenerateAnnotationForComplexType(_language);

            var overallResult = string.Empty;

            if (SupportedLanguages.Java == _language || SupportedLanguages.CSharp == _language)
                // overallResult = string.IsNullOrEmpty(result) ? result : result + string.Format("\r\npublic {0} {1};", JdiMemberType.ConvertToTypeString(), MemberName);
                overallResult = string.IsNullOrEmpty(result) ? result : result + string.Format("\r\npublic {0} {1};", JdiMemberType.ConvertToTypeString(EnumerationTypeName), MemberName);

            return overallResult;
        }

        // TODO: get enumeration type name via Id that is bound, probably
        string GenerateAnnotationForComplexType(SupportedLanguages supportedLanguage)
        {
            EnumerationTypeName = GenerateEnumerationTypeName();
            return string.Format("\r\n@J{0}({1}, {2}, {3})", GetNormalizedLocatorName(), GetDropDownRootLocator(supportedLanguage), GetDropDownValueLocator(supportedLanguage), GetDropDownListLocator(supportedLanguage));
        }

        string GetNormalizedLocatorName()
        {
            return JdiMemberType.ToString().Substring(0, 1).ToUpper() + JdiMemberType.ToString().Substring(1).ToLower();
        }

        string GetDropDownRootLocator(SupportedLanguages supportedLanguage)
        {
            // return null != Root ? Root.SearchString : string.Empty;
            return null != Root ? GetLocatorText(Root, "root", supportedLanguage) : string.Empty;
        }

        string GetDropDownValueLocator(SupportedLanguages supportedLanguage)
        {
            return null != Value ? GetLocatorText(Value, "value", supportedLanguage) : string.Empty;
        }

        string GetDropDownListLocator(SupportedLanguages supportedLanguage)
        {
            return null != List ? GetLocatorText(List, "list", supportedLanguage) : string.Empty;
        }

        /*
    @JDropdown(
        root = @FindBy(className = "country-selection"),
        value = @FindBy(css = ".country-wrapper .arrow"),
        elementByName = @FindBy(xpath = "*root*/ /*[contains(@id,'select-box-applicantCountry')]//li[.='%s']"))
    IDropDown country;

    @JDropdown(
            root = @FindBy(className = "city-selection"),
            expand = @FindBy(css = ".city-wrapper .arrow"),
            list = @FindBy(xpath = "*root*/ /*[contains(@id,'select-box-applicantCity')]//li")
    )
    IDropDown city;
        */

        string GetLocatorText(LocatorDefinition locator, string locatorName, SupportedLanguages supportedLanguage)
        {
            if (SupportedLanguages.Java == supportedLanguage)
                return string.Format("\r\n{0} = @{1}({2}=\"{3}\")", locatorName, locator.Attribute, locator.SearchTypePreference, locator.SearchString);
            if (SupportedLanguages.CSharp == supportedLanguage)
                return string.Format("\r\n{0} = [{1}({2}=\"{3}\")]", locatorName, locator.Attribute, locator.SearchTypePreference, locator.SearchString);
            return string.Empty;
        }

        string GenerateEnumerationTypeName()
        {
            // TODO: write code
            // return "SomeEnum";
            return MemberName.Substring(0, 1).ToUpper() + MemberName.Substring(1);
        }
    }
}