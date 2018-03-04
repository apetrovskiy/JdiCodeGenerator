namespace CodeGenerator.Core.ObjectModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Abstract;
	using Enums;
	using Helpers;
	using JdiConverters.Helpers;
	using JdiConverters.ObjectModel.Enums;

	public class PageMemberCodeEntry : IPageMemberCodeEntry
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public List<LocatorDefinition> Locators { get; set; }
        public string MemberName { get; set; }
        public SourceMemberTypeHolder SourceMemberType { get; set; }
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
        public JdiPageMemberCodeEntryTypes Type
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
                        return JdiPageMemberCodeEntryTypes.Simple;
                    case JdiElementTypes.NavBar:
                    case JdiElementTypes.Pager:
                    case JdiElementTypes.Progress:
                    case JdiElementTypes.List:
                    case JdiElementTypes.ListItem:
                    case JdiElementTypes.Popover:
                    case JdiElementTypes.Carousel:
                    case JdiElementTypes.CheckList:
                        return JdiPageMemberCodeEntryTypes.Unknown;
                    case JdiElementTypes.ComboBox:
                        return JdiPageMemberCodeEntryTypes.ComplexWithConstructor;
                    case JdiElementTypes.DropDown:
                        return JdiPageMemberCodeEntryTypes.ComplexWithAnnotations;
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
                        return JdiPageMemberCodeEntryTypes.ComplexWithConstructor;
                    case JdiElementTypes.Table:
                        return JdiPageMemberCodeEntryTypes.ComplexWithConstructor;
                    case JdiElementTypes.Cell:
                    case JdiElementTypes.Column:
                    case JdiElementTypes.Coulmns:
                    case JdiElementTypes.DynamicTable:
                    case JdiElementTypes.ElementIndexType:
                    case JdiElementTypes.Row:
                    case JdiElementTypes.RowColumn:
                    case JdiElementTypes.Rows:
                    case JdiElementTypes.TableLine:
                        return JdiPageMemberCodeEntryTypes.Unknown;
                    //case JdiElementTypes.StopProcessing:
                    //    break;
                    default:
                        return JdiPageMemberCodeEntryTypes.Simple;
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

        SupportedTargetLanguages _targetLanguage;

        public PageMemberCodeEntry()
        {
            Id = Guid.NewGuid();
            Locators = new List<LocatorDefinition>();
            ListMemberNames = new List<string>();
            CodeClass = PageObjectParts.CodeOfMember;
            // SourceMemberType = SourceMemberTypeHolder.GetInstance<>();
            SourceMemberType = new SourceMemberTypeHolder();
        }

        public string GenerateCode(SupportedTargetLanguages targetLanguage)
        {
            var result = string.Empty;

            // TODO: for the future use
            _targetLanguage = targetLanguage;

            // FilterOutWrongLocators();

            result = GenerateCodeEntryWithBestLocator();

            return result;
        }

        public PageObjectParts CodeClass { get; set; }

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
            if (SupportedTargetLanguages.Java == _targetLanguage)
                result = $"\r\n@{bestLocator.Attribute}({bestLocator.ElementSearchTypePreference}=\"{bestLocator.SearchString}\")";
            if (SupportedTargetLanguages.CSharp == _targetLanguage)
                result = $"\r\n[{bestLocator.Attribute}({bestLocator.ElementSearchTypePreference}=\"{bestLocator.SearchString}\")]";

            /*
            @JDropdown(root = @FindBy(css = "dropdown"), value = @FindBy(id = "dropdownMenu1"), list = @FindBy(tagName = "li"))
            IDropDown<JobCategories> category;
            */
            if (JdiMemberType.IsComplexControl())
                result += GenerateAnnotationForComplexType(_targetLanguage);

            var overallResult = string.Empty;

            if (SupportedTargetLanguages.Java == _targetLanguage || SupportedTargetLanguages.CSharp == _targetLanguage)
                overallResult = string.IsNullOrEmpty(result) ? result : $"{result}\r\npublic {JdiMemberType.ConvertToTypeString(EnumerationTypeName)} {MemberName};";

            return overallResult;
        }

        // TODO: get enumeration type name via Id that is bound, probably
        string GenerateAnnotationForComplexType(SupportedTargetLanguages supportedTargetLanguage)
        {
            EnumerationTypeName = GenerateEnumerationTypeName();
            return $"\r\n@J{GetNormalizedLocatorName()}({GetDropDownRootLocator(supportedTargetLanguage)}, {GetDropDownValueLocator(supportedTargetLanguage)}, {GetDropDownListLocator(supportedTargetLanguage)})";
        }

        string GetNormalizedLocatorName()
        {
            return JdiMemberType.ToString().Substring(0, 1).ToUpper() + JdiMemberType.ToString().Substring(1).ToLower();
        }

        string GetDropDownRootLocator(SupportedTargetLanguages supportedTargetLanguage)
        {
            // return null != Root ? Root.SearchString : string.Empty;
            return null != Root ? GetLocatorText(Root, "root", supportedTargetLanguage) : string.Empty;
        }

        string GetDropDownValueLocator(SupportedTargetLanguages supportedTargetLanguage)
        {
            return null != Value ? GetLocatorText(Value, "value", supportedTargetLanguage) : string.Empty;
        }

        string GetDropDownListLocator(SupportedTargetLanguages supportedTargetLanguage)
        {
            return null != List ? GetLocatorText(List, "list", supportedTargetLanguage) : string.Empty;
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

        string GetLocatorText(LocatorDefinition locator, string locatorName, SupportedTargetLanguages supportedTargetLanguage)
        {
            if (SupportedTargetLanguages.Java == supportedTargetLanguage)
                return $"\r\n{locatorName} = @{locator.Attribute}({locator.ElementSearchTypePreference}=\"{locator.SearchString}\")";
            if (SupportedTargetLanguages.CSharp == supportedTargetLanguage)
                return $"\r\n{locatorName} = [{locator.Attribute}({locator.ElementSearchTypePreference}=\"{locator.SearchString}\")]";
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