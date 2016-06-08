namespace JdiCodeGenerator.Tests.Recognition
{
    using System.Reflection;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;
    using HtmlAgilityPack;
    using Xunit;

    public class PlainHtml5
    {
        /*
        CodeEntry _entry;
        HtmlDocument _doc;

        public PlainHtml5()
        {
            _entry = null;
            _doc = null;
        }

        [Theory]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [Trait("Category", "Plain HTML5, single element")]
        public void ParsePlainHtml5ForSingleElement(string input, string expected)
        {
            GivenHtml(input);
            WhenParsing();
            ThenThereIsElementOfType(expected);
        }

        [Theory]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [InlineData(@"

", "")]
        [Trait("Category", "Plain HTML5, collection")]
        public void ParsePlainHtml5ForCollection(string input, string expected)
        {
            GivenHtml(input);
            WhenParsing();
            ThenThereIsCollectionOfElementsOfType(expected);
        }

        void GivenHtml(string input)
        {
            var fullHtml = @"<html><head></head><body>" + input + "</body></html>";
            _doc = new HtmlDocument();
            _doc.LoadHtml(fullHtml);
        }

        void WhenParsing()
        {

        }

        void ThenThereIsElementOfType(string expected)
        {
            Assert.True(_entry.GenerateCodeForEntry(SupportedLanguages.Java).Contains(expected));
        }

        void ThenThereIsCollectionOfElementsOfType(string expected)
        {
            Assert.True(_entry.GenerateCodeForEntry(SupportedLanguages.Java).Contains(expected));
        }
        */
    }
}