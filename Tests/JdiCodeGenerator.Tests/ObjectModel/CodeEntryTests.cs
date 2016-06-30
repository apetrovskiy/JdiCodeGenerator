namespace JdiCodeGenerator.Tests.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Helpers;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;
    using HtmlAgilityPack;
    using Mocks;
    using NSubstitute;
    using NUnit.Framework;
    using Web.Helpers;
    using Web.ObjectModel.Abstract;
    using Xunit;

    public class CodeEntryTests
    {
        ICodeEntry<HtmlElementTypes> _entry;
        string _code;

        public CodeEntryTests()
        {
            _entry = null;
            _code = string.Empty;
        }

        /*
        Guid Id { get; set; }
        List<LocatorDefinition> Locators { get; set; }
        string MemberName { get; set; }
        HtmlElementTypes HtmlMemberType { get; set; }
        string MemberType { get; set; }

        // temporarily!
        string Type { get; set; }
        */

        [Xunit.Theory]
        [InlineData(new[] { "id", "" }, "", "element", "elementNoName")]
        [InlineData(new[] { "id", "" }, "input", "textfield", "textFieldNoName")]
        [InlineData(new[] { "id", "id" }, "input", "textfield", "textFieldId")]
        [InlineData(new[] { "name", "some name" }, "input", "textfield", "textFieldSomeName")]
        [InlineData(new[] { "css", ".a .ul" }, "input", "textfield", "textFieldNoName")] // ??
        [InlineData(new[] { "css", "[name='a']" }, "input", "textfield", "textFieldNoName")] // ??
        [InlineData(new[] { "className", ".a .ul" }, "input", "textfield", "textFieldAUl")]
        [InlineData(new[] { "xpath", "//*[class=test]" }, "input", "textfield", "textFieldNoName")] // ??
        [InlineData(new[] { "linkText", "/aa/bb/cc" }, "input", "textfield", "textFieldAaBbCc")]
        [InlineData(new[] { "tagName", "ttt" }, "input", "textfield", "textFieldTtt")]
        [InlineData(new[] { "tagName", "ttt" }, "?xml", "element", "elementTtt")]
        [Trait("Category", "EntryTitle")]

        [TestCase(new[] { "id", "" }, "", "element", "elementNoName")]
        [TestCase(new[] { "id", "" }, "input", "textfield", "textFieldNoName")]
        [TestCase(new[] { "id", "id" }, "input", "textfield", "textFieldId")]
        [TestCase(new[] { "name", "some name" }, "input", "textfield", "textFieldSomeName")]
        [TestCase(new[] { "css", ".a .ul" }, "input", "textfield", "textFieldNoName")] // ??
        [TestCase(new[] { "css", "[name='a']" }, "input", "textfield", "textFieldNoName")] // ??
        [TestCase(new[] { "className", ".a .ul" }, "input", "textfield", "textFieldAUl")]
        [TestCase(new[] { "xpath", "//*[class=test]" }, "input", "textfield", "textFieldNoName")] // ??
        [TestCase(new[] { "linkText", "/aa/bb/cc" }, "input", "textfield", "textFieldAaBbCc")]
        [TestCase(new[] { "tagName", "ttt" }, "input", "textfield", "textFieldTtt")]
        [TestCase(new[] { "tagName", "ttt" }, "?xml", "element", "elementTtt")]
        public void GeneratesEntryTitle(string[] stringLocatorDefinitions, string memberType, string jdiMemberType, string expectedTitle)
        {
            var locatorDefinitions = ConvertStringArrayToLocatorDefinitions(stringLocatorDefinitions);
            GivenCodeEntry(locatorDefinitions, jdiMemberType, memberType);
            WhenGeneratingTitle();
            ThenTitleIs(expectedTitle);
        }

        [Xunit.Theory]
        [InlineData(new[] { "id", "id" }, "", "element", "IElement")]
        [InlineData(new[] { "id", "id" }, "input", "textfield", "ITextField")]
        [InlineData(new[] { "id", "id" }, "label", "label", "ILabel")]
        [InlineData(new[] { "id", "id" }, "button", "button", "IButton")]
        [InlineData(new[] { "id", "id" }, "select", "checkbox", "ICheckBox")]
        [InlineData(new[] { "id", "id" }, "a", "link", "ILink")]
        [InlineData(new[] { "id", "id" }, "img", "image", "IImage")]
        // [InlineData(new[] { "id", "id" }, "textarea", "textarea", "ITextArea")]
        // [InlineData(new[] { "id", "id" }, "label", "label", "ILabel")]
        [Trait("Category", "EntryJdiType")]

        [TestCase(new[] { "id", "id" }, "", "element", "IElement")]
        [TestCase(new[] { "id", "id" }, "input", "textfield", "ITextField")]
        [TestCase(new[] { "id", "id" }, "label", "label", "ILabel")]
        [TestCase(new[] { "id", "id" }, "button", "button", "IButton")]
        [TestCase(new[] { "id", "id" }, "select", "checkbox", "ICheckBox")]
        [TestCase(new[] { "id", "id" }, "a", "link", "ILink")]
        [TestCase(new[] { "id", "id" }, "img", "image", "IImage")]
        public void GenerateCodeEntryWithBestLocator(string[] stringLocatorDefinitions, string memberType, string jdiMemberType, string expectedJdiType)
        {
            var locatorDefinitions = ConvertStringArrayToLocatorDefinitions(stringLocatorDefinitions);
            GivenCodeEntry(locatorDefinitions, jdiMemberType, memberType);
            WhenGeneratingCode();
            ThenCodeContains(expectedJdiType);
        }

        void GivenCodeEntry(IEnumerable<LocatorDefinition> locatorDefinitions, string jdiMemberType, string memberType)
        {
            _entry = new CodeEntry<HtmlElementTypes>
            {
                Locators = locatorDefinitions.ToList(),
                // HtmlMemberType = Enum.GetValues(typeof(HtmlElementTypes)).Cast<HtmlElementTypes>().FirstOrDefault(val => 0 == string.Compare(val.ToString().ToLower(), memberType, StringComparison.Ordinal)),
                SourceMemberType = new SourceElementTypeCollection<HtmlElementTypes> {  Types = new List<HtmlElementTypes> { Enum.GetValues(typeof(HtmlElementTypes)).Cast<HtmlElementTypes>().FirstOrDefault(val => 0 == string.Compare(val.ToString().ToLower(), memberType, StringComparison.Ordinal)) } },
                JdiMemberType = Enum.GetValues(typeof(JdiElementTypes)).Cast<JdiElementTypes>().FirstOrDefault(val => 0 == string.Compare(val.ToString().ToLower(), jdiMemberType, StringComparison.Ordinal)),
                MemberType = memberType
            };
        }

        void WhenGeneratingTitle()
        {
            _entry.MemberName = _entry.GenerateNameBasedOnNamingPreferences();
        }

        void WhenGeneratingCode()
        {
            var node = Substitute.For<HtmlNodeMock>(HtmlNodeType.Element, new HtmlDocument(), 1);
            node.OriginalName.Returns(_entry.MemberType);

            // var bootstrapAnalyzer = new Bootstrap3();
            // _entry.JdiMemberType = _entry.HtmlMemberType.ConvertHtmlTypeToJdiType();
            _entry.JdiMemberType = _entry.SourceMemberType.Types[0].ConvertHtmlTypeToJdiType();

            _code = _entry.GenerateCodeForEntry(SupportedLanguages.Java);
        }

        void ThenTitleIs(string expectedTitle)
        {
            Xunit.Assert.Equal(expectedTitle, _entry.MemberName);
        }

        void ThenCodeContains(string expectedJdiType)
        {
            Xunit.Assert.True(_code.Contains(expectedJdiType));
        }

        SearchTypePreferences ConvertStringToSearchTypePreference(string stringTypePreference)
        {
            switch (stringTypePreference)
            {
                case "id":
                    return SearchTypePreferences.id;
                case "name":
                    return SearchTypePreferences.name;
                case "css":
                    return SearchTypePreferences.css;
                case "className":
                    return SearchTypePreferences.className;
                case "xpath":
                    return SearchTypePreferences.xpath;
                case "linkText":
                    return SearchTypePreferences.linkText;
                case "tagName":
                    return SearchTypePreferences.tagName;
                default:
                    return SearchTypePreferences.id;
            }
        }

        List<LocatorDefinition> ConvertStringArrayToLocatorDefinitions(string[] stringLocatorDefinitions)
        {
            return new List<LocatorDefinition> { new LocatorDefinition { Attribute = FindTypes.FindBy, IsBestChoice = true, SearchTypePreference = ConvertStringToSearchTypePreference(stringLocatorDefinitions[0]), SearchString = stringLocatorDefinitions[1] } };
        }
    }
}