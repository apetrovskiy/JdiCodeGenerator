namespace JdiCodeGenerator.Tests.ObjectModel
{
    using System.Collections.Generic;
    using Core.Helpers;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;
    using Core.ObjectModel.Enums;
    using HtmlAgilityPack;
    using Plugins;
    using Web.ObjectModel.Abstract;
    using Web.ObjectModel.Plugins.Plain;
    using Xunit;

    public class PiecesOfCodeTests
    {
        PageMemberCodeEntry<HtmlElementTypes> _entry;
        HtmlDocument _doc;
        List<IPieceOfCode<HtmlElementTypes>> _entries;

        public PiecesOfCodeTests()
        {
            _entry = null;
            _doc = null;
            _entries = new List<IPieceOfCode<HtmlElementTypes>>();
        }

        [Theory]
        [InlineData(@"..\Data\Jdi\Simple\TextField.txt", "ITextField")]
        [InlineData(@"..\Data\Jdi\Simple\TimePicker.txt", "ITimePicker")]
        [InlineData(@"..\Data\Jdi\Simple\Button.txt", "IButton")]

        [Trait("Category", "JDI, single element")]

        public void ParsePlainHtml5ForSingleElement(string input, string expectedType)
        {
            GivenHtmlFromFile(input);
            WhenParsing(expectedType);
            ThenThereIsElementOfTypeAndPageCodeUnit(expectedType);
        }

        [Theory]
        [InlineData(@"..\Data\Jdi\Simple\DatePicker.txt", "IDatePicker", 1)]
        [Trait("Category", "JDI, single element, positional")]

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
            Assert.Equal(typeof(ICodeUnit<HtmlElementTypes>), _entries[0].GetType());
        }

        //void ThenThereIsCollectionOfElementsOfType(string expectedType, string rootSearchString, string valueSearchString, string listSearchString)
        //{
        //    var generatedCodeEntry = _entry.GenerateCode(SupportedLanguages.Java);
        //    Assert.True(generatedCodeEntry.Contains(expectedType));
        //    if (_entry.JdiMemberType.IsComplexControl())
        //    {
        //        if (!string.IsNullOrEmpty(rootSearchString))
        //            Assert.False(string.IsNullOrEmpty(_entry.Root.SearchString));
        //        if (!string.IsNullOrEmpty(valueSearchString))
        //            Assert.False(string.IsNullOrEmpty(_entry.Value.SearchString));
        //        if (!string.IsNullOrEmpty(listSearchString))
        //            Assert.False(string.IsNullOrEmpty(_entry.List.SearchString));
        //    }
        //}
    }
}