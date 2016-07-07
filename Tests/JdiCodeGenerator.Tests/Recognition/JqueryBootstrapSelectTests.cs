namespace JdiCodeGenerator.Tests.Recognition
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Helpers;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;
    using HtmlAgilityPack;
    using Internals;
    using Web.Helpers;
    using Web.ObjectModel.Abstract;
    using Web.ObjectModel.Plugins;
    using Web.ObjectModel.Plugins.BootstrapAndCompetitors;
    using Xunit;

    public class JqueryBootstrapSelectTests
    {
        CodeEntry<HtmlElementTypes> _entry;
        HtmlDocument _doc;
        readonly List<ICodeEntry<HtmlElementTypes>> _entries;

        public JqueryBootstrapSelectTests()
        {
            _entry = null;
            _doc = null;
            _entries = new List<ICodeEntry<HtmlElementTypes>>();
        }

        [Theory]
        [InlineData(@"..\Data\JqueryBootstrapSelect\Select.txt", "IComboBox<", "bootstrap-select", "dropdown-toggle", "dropdown-menu", 0)]
        [Trait("Category", "jQuery Bootstrap-select, collection")]
        public void ParseBootstrap3ForCollection(string input, string expected, string rootSearchString, string valueSearchString, string listSearchString, int elementPosition)
        {
            GivenHtmlFromFile(input);
            WhenParsing(elementPosition);
            ThenThereIsCollectionOfElementsOfType(expected, rootSearchString, valueSearchString, listSearchString);
        }

        void GivenHtmlFromFile(string path)
        {
            _doc = new HtmlDocument();
            _doc.LoadHtml(TestFactory.GetBootstrap3Page(path));
        }

        void WhenParsing(int elementPosition)
        {
            var pageLoader = new PageLoader();
            var applicableAnalyzers = new[] { typeof(JqueryBootstrapSelect) };
            _entries.AddRange(pageLoader.GetCodeEntriesFromNode<HtmlElementTypes>(_doc.DocumentNode, TestFactory.ExcludeList, applicableAnalyzers));
            _entry = _entries.Cast<CodeEntry<HtmlElementTypes>>().ToArray()[elementPosition];
        }

        void ThenThereIsCollectionOfElementsOfType(string expected, string rootSearchString, string valueSearchString, string listSearchString)
        {
            var generatedCodeEntry = _entry.GenerateCodeForEntry(SupportedLanguages.Java);
            Assert.True(generatedCodeEntry.Contains(expected));
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