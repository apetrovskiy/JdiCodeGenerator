namespace JdiCodeGenerator.Tests.Recognition
{
    using Core.Helpers;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;
    using HtmlAgilityPack;
    using Internals;
    using Web.ObjectModel.Abstract;
    using Web.ObjectModel.Plugins;
    using Xunit;

    public class JdiTests
    {
        CodeEntry<HtmlElementTypes> _entry;
        HtmlDocument _doc;

        public JdiTests()
        {
            _entry = null;
            _doc = null;
        }

        [Theory]
        [InlineData(@"..\Data\Jdi\Simple\TextField.txt", "ITextField")]
        [InlineData(@"..\Data\Jdi\Simple\TimePicker.txt", "ITimePicker")]
        [InlineData(@"..\Data\Jdi\Simple\Button.txt", "IButton")]

        //[InlineData(@"..\Data\Jdi\Simple\TextArea.txt", "ITextArea", 0)]
        //[InlineData(@"..\Data\Jdi\Simple\Link.txt", "ILink", 0)]
        //[InlineData(@"..\Data\Jdi\Simple\Label.txt", "ILabel", 1)]
        //[InlineData(@"..\Data\Jdi\Simple\Hyperlink.txt", "ILink", 0)]

        [Trait("Category", "JDI, single element")]

        public void ParsePlainHtml5ForSingleElement(string input, string expectedType)
        {
            GivenHtmlFromFile(input);
            WhenParsing(expectedType);
            ThenThereIsElementOfType(expectedType);
        }

        [Theory]
        [InlineData(@"..\Data\Jdi\Simple\DatePicker.txt", "IDatePicker", 1)]
        [Trait("Category", "JDI, single element, positional")]

        public void ParsePlainHtml5ForSingleElementWithPosition(string input, string expectedType, int elementPosition)
        {
            GivenHtmlFromFile(input);
            WhenParsing(elementPosition);
            ThenThereIsElementOfType(expectedType);
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
        [InlineData(@"..\Data\Jdi\Complex\Pagination.txt", "IPagination<", "uui-pagination", "uui-pagination", "li")]

        [Trait("Category", "JDI, collection")]

        public void ParsePlainHtml5ForCollection(string input, string expectedType, string rootSearchString, string valueSearchString, string listSearchString)
        {
            GivenHtmlFromFile(input);
            WhenParsing(expectedType);
            ThenThereIsCollectionOfElementsOfType(expectedType, rootSearchString, valueSearchString, listSearchString);
        }

        //[Theory]
        
        //[Trait("Category", "JDI, collection, positional")]

        //public void ParsePlainHtml5ForCollectionWithPosition(string input, string expectedType, string rootSearchString, string valueSearchString, string listSearchString, int elementPosition)
        //{
        //    GivenHtmlFromFile(input);
        //    WhenParsing(elementPosition);
        //    ThenThereIsCollectionOfElementsOfType(expectedType, rootSearchString, valueSearchString, listSearchString);
        //}

        void GivenHtmlFromFile(string path)
        {
            _doc = new HtmlDocument();
            _doc.LoadHtml(TestFactory.Instance.GetPlainHtml5Page(path));
        }

        void WhenParsing(string expectedType)
        {
            _entry = TestFactory.Instance.GetEntryExpected(_doc, new[] { typeof(Jdi) }, expectedType);
        }

        void WhenParsing(int elementPosition)
        {
            _entry = TestFactory.Instance.GetEntryExpected(_doc, new[] { typeof(Jdi) }, elementPosition);
        }

        void ThenThereIsElementOfType(string expectedType)
        {
            Assert.True(_entry.GenerateCodeForEntry(SupportedLanguages.Java).Contains(expectedType));
        }

        void ThenThereIsCollectionOfElementsOfType(string expectedType, string rootSearchString, string valueSearchString, string listSearchString)
        {
            var generatedCodeEntry = _entry.GenerateCodeForEntry(SupportedLanguages.Java);
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