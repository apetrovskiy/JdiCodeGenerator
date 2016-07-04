namespace JdiCodeGenerator.Core.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstract;
    using Helpers;

    public class CodeEntry<T> : ICodeEntry<T>
    {
        public Guid Id { get; set; }
        public List<LocatorDefinition> Locators { get; set; }
        public string MemberName { get; set; }
        public SourceElementTypeCollection<T> SourceMemberType { get; set; }
        public JdiElementTypes JdiMemberType { get; set; }
        // for debugging purposes
        // public string MemberType { get; set; }
        string _memberType;
        public string MemberType
        {
            get { return _memberType;  }
            set { _memberType = value.CleanUpFromWrongCharacters(); }
        }

        // temporarily!
        //public IFrameworkAlingmentAnalysisPlugin AnalyzerThatWon { get; set; }
        //public IRule RuleThatWon { get; set; }
        public string RuleThatWon { get; set; }
        public string Type { get; set; }

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

        SupportedLanguages _language;

        public CodeEntry()
        {
            Id = Guid.NewGuid();
            Locators = new List<LocatorDefinition>();
        }

        public string GenerateCodeForEntry(SupportedLanguages language)
        {
            var result = string.Empty;

            // TODO: for the future use
            _language = language;

            // FilterOutWrongLocators();

            result = GenerateCodeEntryWithBestLocator();

            return result;
        }

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
                result += GenerateAnnotationForComplexType();

            var overallResult = string.Empty;

            if (SupportedLanguages.Java == _language || SupportedLanguages.CSharp == _language)
                // overallResult = string.IsNullOrEmpty(result) ? result : result + string.Format("\r\npublic {0} {1};", JdiMemberType.ConvertToTypeString(), MemberName);
                overallResult = string.IsNullOrEmpty(result) ? result : result + string.Format("\r\npublic {0} {1};", JdiMemberType.ConvertToTypeString(EnumerationTypeName), MemberName);

            return overallResult;
        }

        string GenerateAnnotationForComplexType()
        {
            EnumerationTypeName = GenerateEnumerationTypeName();
            return string.Format(@"@J{0}(root = {1}, value = {2}, list = {3})", GetNormalizedLocatorName(), GetDropDownRootLocator(), GetDropDownValueLocator(), GetDropDownListLocator());
        }

        string GetNormalizedLocatorName()
        {
            return JdiMemberType.ToString().Substring(0, 1).ToUpper() + JdiMemberType.ToString().Substring(1).ToLower();
        }

        string GetDropDownRootLocator()
        {
            return string.Empty;
        }

        string GetDropDownValueLocator()
        {
            return string.Empty;
        }

        string GetDropDownListLocator()
        {
            return string.Empty;
        }

        string GenerateEnumerationTypeName()
        {
            // TODO: write code
            return "SomeEnum";
        }
    }
}