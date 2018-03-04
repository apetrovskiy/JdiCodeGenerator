namespace CodeGenerator.Tests.ObjectModel
{
	using System.Collections.Generic;
	using System.Linq;
	using Core.ObjectModel.Abstract.Results;
	using Core.ObjectModel.Enums;
	using Core.ObjectModel.Results;
	using HtmlAgilityPack;
	using Plugins;
	using Web.ObjectModel.Plugins.Plain;
	using Xunit;

	public class PiecesOfCodeTests
    {
        PageMemberCodeEntry _entry;
        HtmlDocument _doc;
        List<IPieceOfPackage> _entries;

        public PiecesOfCodeTests()
        {
            _entry = null;
            _doc = null;
            _entries = new List<IPieceOfPackage>();
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
            ThenThereIsPageCodeFile(expectedType);
        }

        [Theory]
        [InlineData(@"..\Data\Jdi\Simple\DatePicker.txt", "IDatePicker", 1)]
        [Trait("Category", "pieces of code, single element, positional")]

        public void ParsePlainHtml5ForSingleElementWithPosition(string input, string expectedType, int elementPosition)
        {
            GivenHtmlFromFile(input);
            WhenParsing(elementPosition);
            ThenThereIsPageCodeFile(expectedType);
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
            _entries = TestFactory.Instance.GetPiecesOfCodeCollection(_doc, new[] {typeof(Jdi)});
        }

        void ThenThereIsPageCodeFile(string expectedType)
        {
            Assert.True(_entries.Any(entry => PageObjectParts.ClassFile == entry.CodeClass));
            Assert.True(_entries.Any(entry => PageObjectParts.CodeOfMember == entry.CodeClass));
            Assert.True( _entries.Any(entry => entry is CodeFile));
            Assert.True(_entries.Any(entry => entry is PageMemberCodeEntry));
        }
    }
}