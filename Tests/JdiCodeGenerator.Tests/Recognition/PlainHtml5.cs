namespace JdiCodeGenerator.Tests.Recognition
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;
    using HtmlAgilityPack;
    using Internals;
    using NUnit.Framework;
    using Web.Helpers;
    using Web.ObjectModel.Abstract;
    using Xunit;

    public class PlainHtml5
    {
        CodeEntry<HtmlElementTypes> _entry;
        HtmlDocument _doc;
        readonly List<ICodeEntry<HtmlElementTypes>> _entries;

        public PlainHtml5()
        {
            _entry = null;
            _doc = null;
            _entries = new List<ICodeEntry<HtmlElementTypes>>();
        }

        [Xunit.Theory]
        [InlineData(@"..\Data\PlainHtml5\Simple\TextField.txt", "ITextField", 1)]
        [InlineData(@"..\Data\PlainHtml5\Simple\Button.txt", "IButton", 0)]

        [InlineData(@"..\Data\PlainHtml5\Simple\TextArea.txt", "ITextArea", 0)]
        [InlineData(@"..\Data\PlainHtml5\Simple\Link.txt", "ILink", 0)]
        [InlineData(@"..\Data\PlainHtml5\Simple\Label.txt", "ILabel", 1)]
        [InlineData(@"..\Data\PlainHtml5\Simple\Hyperlink.txt", "ILink", 0)]

        [Trait("Category", "HTML 5, single element")]

        public void ParsePlainHtml5ForSingleElement(string input, string expected, int elementPosition)
        {
            GivenHtmlFromFile(input);
            WhenParsing(elementPosition);
            ThenThereIsElementOfType(expected);
        }

        [Xunit.Theory]
        [InlineData(@"..\Data\PlainHtml5\Complex\DropDownList.txt", "IDropDown<SomeEnum>", 0)]
        [InlineData(@"..\Data\PlainHtml5\Complex\Form.txt", "IForm<SomeEnum>", 0)]
        // [InlineData(@"..\Data\PlainHtml5\Complex\Menu.txt", "IMenu<SomeEnum>", 0)]
        // [InlineData(@"..\Data\PlainHtml5\Complex\MenuItem.txt", "IMenu<SomeEnum>", 0)]
        // [InlineData(@"..\Data\PlainHtml5\Complex\Table.txt", "ITable<SomeEnum>", 0)]
        
        [Trait("Category", "Bootstrap 3, collection")]


        public void ParsePlainHtml5ForCollection(string input, string expected, int elementPosition)
        {
            GivenHtmlFromFile(input);
            WhenParsing(elementPosition);
            ThenThereIsCollectionOfElementsOfType(expected);
        }

        void GivenHtmlFromFile(string path)
        {
            _doc = new HtmlDocument();
            _doc.LoadHtml(TestFactory.GetPlainHtml5Page(path));
        }

        void WhenParsing(int elementPosition)
        {
            var pageLoader = new PageLoader();
            _entries.AddRange(pageLoader.GetCodeEntriesFromNode<HtmlElementTypes>(_doc.DocumentNode, TestFactory.ExcludeList));
            _entry = _entries.Cast<CodeEntry<HtmlElementTypes>>().ToArray()[elementPosition];
        }

        void ThenThereIsElementOfType(string expected)
        {
            //        	try {
            Xunit.Assert.True(_entry.GenerateCodeForEntry(SupportedLanguages.Java).Contains(expected));
            //        	}
            //        	catch {
            //        		int i = 1;
            //        	}
        }

        void ThenThereIsCollectionOfElementsOfType(string expected)
        {
            //Console.WriteLine("================================================================================================");
            //Console.WriteLine(_entry.GenerateCodeForEntry(SupportedLanguages.Java));

            Xunit.Assert.True(_entry.GenerateCodeForEntry(SupportedLanguages.Java).Contains(expected));
        }
    }
}