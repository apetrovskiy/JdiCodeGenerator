namespace JdiCodeGenerator.Tests.Helpers
{
    using Xunit;
    using Core.Helpers;

    public class NameGenerationTests
    {
        string _sourceName;

        public NameGenerationTests()
        {
            _sourceName = string.Empty;
        }

        [Theory]
        [InlineData("", "NoName")]
        [InlineData("abc", "Abc")]
        [InlineData("ab cd", "AbCd")]
        [InlineData("aaa bbb,ccc", "AaaBbbCcc")]
        [InlineData("#text", "Text")]
        [InlineData(@"\\server\share", "ServerShare")]
        [InlineData("/link", "Link")]
        [InlineData("/goto/link", "GotoLink")]
        [InlineData("/path/to/link", "PathToLink")]
        [InlineData("/link_to/file.txt", "LinkToFileTxt")]

        [InlineData(".a", "A")]
        [InlineData(".bbb", "Bbb")]
        [InlineData(".ul.li", "UlLi")]
        [InlineData(".ul .li .div", "UlLiDiv")]

        [InlineData("-aaa", "Aaa")]
        [InlineData("aaa-bbb-ccc", "AaaBbbCcc")]
        [InlineData("-", "NoName")]
        [InlineData("--", "NoName")]
        [InlineData("aaa--bb.*c", "AaaBbC")]

        [InlineData("a/", "A")]
        [InlineData("aaa/", "Aaa")]
        [InlineData("b#", "B")]
        [InlineData("bbb#c#", "BbbC")]
        [InlineData("#bbb#c#", "BbbC")]
        [InlineData("/bbb#c#", "BbbC")]

        [InlineData("@a", "A")]
        [InlineData("a@", "A")]
        [InlineData("a@a", "AA")]
        [InlineData("http://www.test.com", "HttpWwwTestCom")]

        [InlineData("file(0)", "File0")]
        [InlineData("file[0]", "File0")]
        [InlineData("http://site:111/page.htm?param1=2&param2=word", "HttpSite111PageHtmParam12Param2Word")]
        [InlineData("http://site:111/page.htm?param1=2+param2=word", "HttpSite111PageHtmParam12Param2Word")]
        [InlineData("http://site:111/page.htm%20?param1=2%param2=word", "HttpSite111PageHtm20Param12Param2Word")]
        [InlineData(@"http://site:111/page.htm
                    %20?param1=2%param2=word", "HttpSite111PageHtm20Param12Param2Word")]

        [InlineData("http://site:111/page.htm%20?param1=2;param2=word", "HttpSite111PageHtm20Param12Param2Word")]
        [InlineData("http://site:111/page.htm%20?param1=2!param2=word", "HttpSite111PageHtm20Param12Param2Word")]
        [InlineData("http://site:111/page.htm%20?param1=2™param2=word", "HttpSite111PageHtm20Param12Param2Word")]
        [InlineData("http://site:111/page.htm%20?param1=2<param2=word", "HttpSite111PageHtm20Param12Param2Word")]
        [InlineData("http://site:111/page.htm%20?param1=2>param2=word", "HttpSite111PageHtm20Param12Param2Word")]
        [InlineData("http://site:111/page.htm%20?param1=2©param2=word", "HttpSite111PageHtm20Param12Param2Word")]

        [InlineData("http://site:111/page.htm%20?param1=2‹param2=word", "HttpSite111PageHtm20Param12Param2Word")]
        [InlineData("http://site:111/page.htm%20?param1=2™param2=word", "HttpSite111PageHtm20Param12Param2Word")]
        [InlineData("http://site:111/page.htm%20?param1=2\"param2=word", "HttpSite111PageHtm20Param12Param2Word")]
        [InlineData("http://site:111/page.htm%20?param1=2 param2=word", "HttpSite111PageHtm20Param12Param2Word")]
        [InlineData("http://site:111/page.htm%20?param1=2№param2=word", "HttpSite111PageHtm20Param12Param2Word")]

        [InlineData("http://site:111/page.htm%20?param1=2—param2=word", "HttpSite111PageHtm20Param12Param2Word")]
        [InlineData("http://site:111/page.htm%20?param1=2¬param2=word", "HttpSite111PageHtm20Param12Param2Word")]
        [InlineData("http://site:111/page.htm%20?param1=2±param2=word", "HttpSite111PageHtm20Param12Param2Word")]
        [InlineData("http://site:111/page.htm%20?param1=2§param2=word", "HttpSite111PageHtm20Param12Param2Word")]

        [InlineData("file!(0)", "File0")]
        [InlineData("file™(0)", "File0")]
        [InlineData("file<(0)", "File0")]
        [InlineData("file>(0)", "File0")]
        [InlineData("file©(0)", "File0")]
        [InlineData("file‹(0)", "File0")]
        [InlineData("file\'(0)", "File0")]
        [InlineData("file'(0)", "File0")]
        [InlineData("file™(0)", "File0")]
        [InlineData("file\"(0)", "File0")]
        [InlineData(@"file""""(0)", "File0")]
		[InlineData("file (0)", "File0")]
        [InlineData("file№(0)", "File0")]
        [InlineData("file—(0)", "File0")]
        [InlineData("file¬(0)", "File0")]
        [InlineData("file±(0)", "File0")]
        [InlineData("file§(0)", "File0")]

        [Trait("Category", "NameGeneration")]
        public void ConvertStringToPascalCase(string input, string expected)
        {
            GivenSourceName(input);
            WhenPreparingName();
            ThenThereIsResult(expected);
        }

        void GivenSourceName(string name)
        {
            _sourceName = name;
        }

        void WhenPreparingName()
        {
            _sourceName = _sourceName.ToPascalCase();
        }

        void ThenThereIsResult(string expected)
        {
            Assert.Equal(expected, _sourceName);
        }
    }
}