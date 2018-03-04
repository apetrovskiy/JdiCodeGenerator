namespace CodeGenerator.Tests.Plugins.JavaScript
{
	using Core.ObjectModel;
	using Core.ObjectModel.Enums;
	using HtmlAgilityPack;
	using JdiConverters.Helpers;
	using Web.ObjectModel.Plugins.JavaScript;
	using Xunit;

	public class JqueryBootstrapSelectTests
    {
        PageMemberCodeEntry _entry;
        HtmlDocument _doc;

        public JqueryBootstrapSelectTests()
        {
            _entry = null;
            _doc = null;
        }

        [Theory]
        [InlineData(@"..\Data\JqueryBootstrapSelect\Select.txt", "IComboBox<", "bootstrap-select", "dropdown-toggle", "dropdown-menu")]
        [Trait("Category", "jQuery Bootstrap-select, collection")]
        public void ParseBootstrap3ForCollection(string input, string expectedType, string rootSearchString, string valueSearchString, string listSearchString)
        {
            GivenHtmlFromFile(input);
            WhenParsing(expectedType);
            ThenThereIsCollectionOfElementsOfType(expectedType, rootSearchString, valueSearchString, listSearchString);
        }

        //[Theory]
        //[InlineData(@"..\Data\JqueryBootstrapSelect\Select.txt", "IComboBox<", "bootstrap-select", "dropdown-toggle", "dropdown-menu", 0)]
        //[Trait("Category", "jQuery Bootstrap-select, collection")]
        //public void ParseBootstrap3ForCollection(string input, string expected, string rootSearchString, string valueSearchString, string listSearchString, int elementPosition)
        //{
        //    GivenHtmlFromFile(input);
        //    WhenParsing(elementPosition);
        //    ThenThereIsCollectionOfElementsOfType(expected, rootSearchString, valueSearchString, listSearchString);
        //}

        void GivenHtmlFromFile(string path)
        {
            _doc = new HtmlDocument();
            _doc.LoadHtml(TestFactory.Instance.GetBootstrap3Page(path));
        }

        void WhenParsing(string expectedType)
        {
            _entry = TestFactory.Instance.GetEntryExpected(_doc, new[] { typeof(JqueryBootstrapSelect) }, expectedType);
        }

        void WhenParsing(int elementPosition)
        {
            _entry = TestFactory.Instance.GetEntryExpected(_doc, new[] {typeof(JqueryBootstrapSelect)}, elementPosition);
        }

        void ThenThereIsCollectionOfElementsOfType(string expectedType, string rootSearchString, string valueSearchString, string listSearchString)
        {
            var generatedCodeEntry = _entry.GenerateCode(SupportedTargetLanguages.Java);
            Assert.True(generatedCodeEntry.Contains(expectedType));
            if (_entry.JdiMemberType.IsComplexControl())
            {
                if (!string.IsNullOrEmpty(rootSearchString))
                    Assert.False(string.IsNullOrEmpty(_entry.Root.SearchString));
                if (!string.IsNullOrEmpty(valueSearchString))
                    Assert.False(string.IsNullOrEmpty(_entry.Value.SearchString));
                if (!string.IsNullOrEmpty(listSearchString))
                    Assert.False(string.IsNullOrEmpty(_entry.List.SearchString));
            }
        }
    }
}