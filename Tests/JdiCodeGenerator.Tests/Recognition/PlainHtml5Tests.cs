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

    public class PlainHtml5Tests
    {
        CodeEntry<HtmlElementTypes> _entry;
        HtmlDocument _doc;

        public PlainHtml5Tests()
        {
            _entry = null;
            _doc = null;
        }

        [Theory]
        [InlineData(@"..\Data\PlainHtml5\Simple\Button.txt", "IButton")]

        [InlineData(@"..\Data\PlainHtml5\Simple\TextArea.txt", "ITextArea")]
        [InlineData(@"..\Data\PlainHtml5\Simple\Link.txt", "ILink")]
        [InlineData(@"..\Data\PlainHtml5\Simple\Hyperlink.txt", "ILink")]

        [Trait("Category", "HTML 5, single element")]

        public void ParsePlainHtml5ForSingleElement(string input, string expectedType)
        {
            GivenHtmlFromFile(input);
            WhenParsing(expectedType);
            ThenThereIsElementOfType(expectedType);
        }


        [Theory]
        [InlineData(@"..\Data\PlainHtml5\Simple\TextField.txt", "ITextField", 1)]
        [InlineData(@"..\Data\PlainHtml5\Simple\Label.txt", "ILabel", 1)]
        [InlineData(@"..\Data\Complex\MetalsColors.txt", "IButton", 33)]

        [Trait("Category", "HTML 5, single element, positional")]
        public void ParsePlainHtml5ForSingleElementWithPosition(string input, string expectedType, int elementPosition)
        {
            GivenHtmlFromFile(input);
            WhenParsing(elementPosition);
            ThenThereIsElementOfType(expectedType);
        }

        [Theory]
        //[InlineData(@"..\Data\PlainHtml5\Complex\DropDownList.txt", "IDropDown<SomeEnum>", 0)]
        //[InlineData(@"..\Data\PlainHtml5\Complex\Form.txt", "IForm<SomeEnum>", 0)]
        // [InlineData(@"..\Data\PlainHtml5\Complex\DropDownList.txt", "IDropDown<", 0)]
        [InlineData(@"..\Data\PlainHtml5\Complex\ComboBox.txt", "IComboBox<", "//select", "//select", "//select")]
        [InlineData(@"..\Data\PlainHtml5\Complex\Form.txt", "IForm<", "", "", "")]
        // [InlineData(@"..\Data\PlainHtml5\Complex\Menu.txt", "IMenu<SomeEnum>", 0)]
        // [InlineData(@"..\Data\PlainHtml5\Complex\MenuItem.txt", "IMenu<SomeEnum>", 0)]
        // [InlineData(@"..\Data\PlainHtml5\Complex\Table.txt", "ITable<SomeEnum>", 0)]

        /*
    public IComboBox<Metals> comboBox =
            new ComboBox<Metals>(By.cssSelector(".metals .caret"), By.cssSelector(".metals li span"), By.cssSelector(".metals input")) {
                @Override
                protected String getTextAction() {
                    return new Text(By.cssSelector(".metals .filter-option")).getText();
                }
            };
        */
        [InlineData(@"..\Data\PlainHtml5\Complex\FifaDropDown.txt", "IComboBox<", "//select", "//select", "//select")]

        /*
    @FindBy(css = "#elements-checklist label")
    public ICheckList<Nature> nature;

    @FindBy(xpath = "//*[@id='elements-checklist']//*[label[text()='%s']]/label")
    public ICheckList<Nature> natureTemplate;
        */
        [InlineData(@"..\Data\PlainHtml5\Complex\CheckList.txt", "ICheckList<", "", "", "//label")]

        [Trait("Category", "HTML 5, collection")]

        public void ParsePlainHtml5ForCollection(string input, string expectedType, string rootSearchString, string valueSearchString, string listSearchString)
        {
            GivenHtmlFromFile(input);
            WhenParsing(expectedType);
            ThenThereIsCollectionOfElementsOfType(expectedType, rootSearchString, valueSearchString, listSearchString);
        }


        [Theory]
        [InlineData(@"..\Data\Complex\MetalsColors.txt", "IRadioButtons<", "", "", "//label", 1)]
        [InlineData(@"..\Data\Complex\MetalsColors.txt", "ICheckList<", "", "", "//label", 4)]
        [Trait("Category", "HTML 5, collection, positional")]

        public void ParsePlainHtml5ForCollectionWithPosition(string input, string expectedType, string rootSearchString, string valueSearchString, string listSearchString, int elementPosition)
        {
            GivenHtmlFromFile(input);
            WhenParsing(elementPosition);
            ThenThereIsCollectionOfElementsOfType(expectedType, rootSearchString, valueSearchString, listSearchString);
        }

        void GivenHtmlFromFile(string path)
        {
            _doc = new HtmlDocument();
            _doc.LoadHtml(TestFactory.Instance.GetPlainHtml5Page(path));
        }

        void WhenParsing(string expectedType)
        {
            _entry = TestFactory.Instance.GetEntryExpected(_doc, new[] { typeof(PlainHtml5) }, expectedType);
        }

        void WhenParsing(int elementPosition)
        {
            _entry = TestFactory.Instance.GetEntryExpected(_doc, new[] { typeof(PlainHtml5) }, elementPosition);
        }

        void ThenThereIsElementOfType(string expected)
        {
            Assert.True(_entry.GenerateCodeForEntry(SupportedLanguages.Java).Contains(expected));
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