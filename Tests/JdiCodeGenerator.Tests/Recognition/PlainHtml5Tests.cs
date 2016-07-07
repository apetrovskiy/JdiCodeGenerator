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

    public class PlainHtml5Tests
    {
        CodeEntry<HtmlElementTypes> _entry;
        HtmlDocument _doc;
        readonly List<ICodeEntry<HtmlElementTypes>> _entries;

        public PlainHtml5Tests()
        {
            _entry = null;
            _doc = null;
            _entries = new List<ICodeEntry<HtmlElementTypes>>();
        }

        [Theory]
        [InlineData(@"..\Data\PlainHtml5\Simple\TextField.txt", "ITextField", 1)]
        [InlineData(@"..\Data\PlainHtml5\Simple\Button.txt", "IButton", 0)]

        [InlineData(@"..\Data\PlainHtml5\Simple\TextArea.txt", "ITextArea", 0)]
        [InlineData(@"..\Data\PlainHtml5\Simple\Link.txt", "ILink", 0)]
        [InlineData(@"..\Data\PlainHtml5\Simple\Label.txt", "ILabel", 1)]
        [InlineData(@"..\Data\PlainHtml5\Simple\Hyperlink.txt", "ILink", 0)]

        [InlineData(@"..\Data\Complex\MetalsColors.txt", "IButton", 33)]

        [Trait("Category", "HTML 5, single element")]

        public void ParsePlainHtml5ForSingleElement(string input, string expected, int elementPosition)
        {
            GivenHtmlFromFile(input);
            WhenParsing(elementPosition);
            ThenThereIsElementOfType(expected);
        }

        [Theory]
        //[InlineData(@"..\Data\PlainHtml5\Complex\DropDownList.txt", "IDropDown<SomeEnum>", 0)]
        //[InlineData(@"..\Data\PlainHtml5\Complex\Form.txt", "IForm<SomeEnum>", 0)]
        // [InlineData(@"..\Data\PlainHtml5\Complex\DropDownList.txt", "IDropDown<", 0)]
        [InlineData(@"..\Data\PlainHtml5\Complex\ComboBox.txt", "IComboBox<", "//select", "//select", "//select", 0)]
        [InlineData(@"..\Data\PlainHtml5\Complex\Form.txt", "IForm<", "", "", "", 0)]
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
        [InlineData(@"..\Data\PlainHtml5\Complex\FifaDropDown.txt", "IComboBox<", "//select", "//select", "//select", 0)]

        /*
    @FindBy(css = "#elements-checklist label")
    public ICheckList<Nature> nature;

    @FindBy(xpath = "//*[@id='elements-checklist']//*[label[text()='%s']]/label")
    public ICheckList<Nature> natureTemplate;
        */
        [InlineData(@"..\Data\PlainHtml5\Complex\CheckList.txt", "ICheckList<", "", "", "//label", 0)]


        [InlineData(@"..\Data\Complex\MetalsColors.txt", "IRadioButtons<", "", "", "//label", 1)]
        [InlineData(@"..\Data\Complex\MetalsColors.txt", "ICheckList<", "", "", "//label", 4)] // 26)]
        

        [Trait("Category", "HTML 5, collection")]

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
            var applicableAnalyzers = new[] { typeof(PlainHtml5) };
            _entries.AddRange(pageLoader.GetCodeEntriesFromNode<HtmlElementTypes>(_doc.DocumentNode, TestFactory.ExcludeList, applicableAnalyzers));
            _entry = _entries.Cast<CodeEntry<HtmlElementTypes>>().ToArray()[elementPosition];
        }

        void ThenThereIsElementOfType(string expected)
        {
            //        	try {
            Assert.True(_entry.GenerateCodeForEntry(SupportedLanguages.Java).Contains(expected));
            //        	}
            //        	catch {
            //        		int i = 1;
            //        	}
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