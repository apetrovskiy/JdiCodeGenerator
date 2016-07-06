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
    using Xunit;

    public class JdiTests
    {
        CodeEntry<HtmlElementTypes> _entry;
        HtmlDocument _doc;
        readonly List<ICodeEntry<HtmlElementTypes>> _entries;

        public JdiTests()
        {
            _entry = null;
            _doc = null;
            _entries = new List<ICodeEntry<HtmlElementTypes>>();
        }

        [Theory]
        [InlineData(@"..\Data\Jdi\Simple\TextField.txt", "ITextField", 0)]
        [InlineData(@"..\Data\Jdi\Simple\DatePicker.txt", "IDatePicker", 1)]
        [InlineData(@"..\Data\Jdi\Simple\TimePicker.txt", "ITimePicker", 0)]
        [InlineData(@"..\Data\Jdi\Simple\Button.txt", "IButton", 0)]

        //[InlineData(@"..\Data\Jdi\Simple\TextArea.txt", "ITextArea", 0)]
        //[InlineData(@"..\Data\Jdi\Simple\Link.txt", "ILink", 0)]
        //[InlineData(@"..\Data\Jdi\Simple\Label.txt", "ILabel", 1)]
        //[InlineData(@"..\Data\Jdi\Simple\Hyperlink.txt", "ILink", 0)]

        [Trait("Category", "JDI, single element")]

        public void ParsePlainHtml5ForSingleElement(string input, string expected, int elementPosition)
        {
            GivenHtmlFromFile(input);
            WhenParsing(elementPosition);
            ThenThereIsElementOfType(expected);
        }

        [Theory]
        //[InlineData(@"..\Data\Jdi\Complex\DropDownList.txt", "IDropDown<SomeEnum>", 0)]
        //[InlineData(@"..\Data\Jdi\Complex\Form.txt", "IForm<SomeEnum>", 0)]
        // [InlineData(@"..\Data\Jdi\Complex\DropDownList.txt", "IDropDown<", 0)]
        // [InlineData(@"..\Data\Jdi\Complex\ComboBox.txt", "IComboBox<", "//select", "//select", "//option", 0)]
        // [InlineData(@"..\Data\Jdi\Complex\Form.txt", "IForm<", "", "", "", 0)]
        // [InlineData(@"..\Data\Jdi\Complex\Menu.txt", "IMenu<SomeEnum>", 0)]
        // [InlineData(@"..\Data\Jdi\Complex\MenuItem.txt", "IMenu<SomeEnum>", 0)]
        // [InlineData(@"..\Data\Jdi\Complex\Table.txt", "ITable<SomeEnum>", 0)]

        // [InlineData(@"..\Data\Jdi\Complex\FifaDropDown.txt", "IComboBox<", "//select", "//select", "//option", 0)]
        [InlineData(@"..\Data\Jdi\Complex\Pagination.txt", "IPagination<", "uui-pagination", "uui-pagination", "li", 0)]

        [Trait("Category", "JDI, collection")]

        public void ParsePlainHtml5ForCollection(string input, string expected, string rootSearchString, string valueSearchString, string listSearchString, int elementPosition)
        {
            GivenHtmlFromFile(input);
            WhenParsing(elementPosition);
            ThenThereIsCollectionOfElementsOfType(expected, rootSearchString, valueSearchString, listSearchString);
        }

        void GivenHtmlFromFile(string path)
        {
            _doc = new HtmlDocument();
            _doc.LoadHtml(TestFactory.GetPlainHtml5Page(path));
        }

        void WhenParsing(int elementPosition)
        {
            var pageLoader = new PageLoader();
            var applicableAnalyzers = new[] { typeof(Jdi) };
            _entries.AddRange(pageLoader.GetCodeEntriesFromNode<HtmlElementTypes>(_doc.DocumentNode, TestFactory.ExcludeList, applicableAnalyzers));
            _entry = _entries.Cast<CodeEntry<HtmlElementTypes>>().ToArray()[elementPosition];
        }

        void ThenThereIsElementOfType(string expected)
        {
            Assert.True(_entry.GenerateCodeForEntry(SupportedLanguages.Java).Contains(expected));
        }

        void ThenThereIsCollectionOfElementsOfType(string expected, string rootSearchString, string valueSearchString, string listSearchString)
        {
            // Assert.True(_entry.GenerateCodeForEntry(SupportedLanguages.Java).Contains(expected));
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