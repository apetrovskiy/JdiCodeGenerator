namespace JdiCodeGenerator.Tests.ObjectModel
{
    using System.Collections.Generic;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;
    using HtmlAgilityPack;
    using Plugins;
    using Web.ObjectModel.Plugins.Plain;
    using Xunit;

    public class PiecesOfCodeTests
    {
        //20160718
        // PageMemberCodeEntry<HtmlElementTypes> _entry;
        PageMemberCodeEntry _entry;
        HtmlDocument _doc;
        //20160718
        // List<IPieceOfCode<HtmlElementTypes>> _entries;
        List<IPieceOfCode> _entries;

        public PiecesOfCodeTests()
        {
            _entry = null;
            _doc = null;
            //20160718
            // _entries = new List<IPieceOfCode<HtmlElementTypes>>();
            _entries = new List<IPieceOfCode>();
        }

        [Theory]
        [InlineData(@"..\Data\Jdi\Simple\TextField.txt", "ITextField")]
        [InlineData(@"..\Data\Jdi\Simple\TimePicker.txt", "ITimePicker")]
        [InlineData(@"..\Data\Jdi\Simple\Button.txt", "IButton")]

        [Trait("Category", "pieces of code, single element")]

        public void ParsePlainHtml5ForSingleElement(string input, string expectedType)
        {
            GivenHtmlFromFile(input);
            WhenParsing(expectedType);
            ThenThereIsElementOfTypeAndPageCodeUnit(expectedType);
        }

        [Theory]
        [InlineData(@"..\Data\Jdi\Simple\DatePicker.txt", "IDatePicker", 1)]
        [Trait("Category", "pieces of code, single element, positional")]

        public void ParsePlainHtml5ForSingleElementWithPosition(string input, string expectedType, int elementPosition)
        {
            GivenHtmlFromFile(input);
            WhenParsing(elementPosition);
            ThenThereIsElementOfTypeAndPageCodeUnit(expectedType);
        }

        //[Theory]
        //[InlineData(@"..\Data\Jdi\Complex\Pagination.txt", "IPagination<", "uui-pagination", "uui-pagination", "li")]

        //[Trait("Category", "JDI, collection")]

        //public void ParsePlainHtml5ForCollection(string input, string expectedType, string rootSearchString, string valueSearchString, string listSearchString)
        //{
        //    GivenHtmlFromFile(input);
        //    WhenParsing(expectedType);
        //    ThenThereIsCollectionOfElementsOfType(expectedType, rootSearchString, valueSearchString, listSearchString);
        //}

        void GivenHtmlFromFile(string path)
        {
            _doc = new HtmlDocument();
            _doc.LoadHtml(TestFactory.Instance.GetPlainHtml5Page(path));
        }

        void WhenParsing(string expectedType)
        {
            // _entry = TestFactory.Instance.GetEntryExpected(_doc, new[] { typeof(Jdi) }, expectedType);
            _entries = TestFactory.Instance.GetPiecesOfCodeCollection(_doc, new[] {typeof(Jdi)});
        }

        void WhenParsing(int elementPosition)
        {
            // _entry = TestFactory.Instance.GetEntryExpected(_doc, new[] { typeof(Jdi) }, elementPosition);
            _entries = TestFactory.Instance.GetPiecesOfCodeCollection(_doc, new[] {typeof(Jdi)});
        }

        void ThenThereIsElementOfTypeAndPageCodeUnit(string expectedType)
        {
            // Assert.True(_entry.GenerateCode(SupportedLanguages.Java).Contains(expectedType));
            Assert.Equal(typeof(PageMemberCodeEntry), _entries[0].GetType());
        }
    }
}