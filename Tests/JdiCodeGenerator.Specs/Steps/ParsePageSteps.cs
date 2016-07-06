namespace JdiCodeGenerator.Specs.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;
    using HtmlAgilityPack;
    using TechTalk.SpecFlow;
    using Tests.Recognition.Internals;
    using Web.Helpers;
    using Web.ObjectModel.Abstract;

    [Binding]
    public class ParsePageSteps
    {
        string _html;
        HtmlDocument _doc;
        CodeEntry<HtmlElementTypes> _entry;
        readonly List<ICodeEntry<HtmlElementTypes>> _entries;
        const string HtmlFirstPart = @"
<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""utf-8"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title>Bootstrap 101 Template</title>

    <!-- Bootstrap -->
    <link href=""css/bootstrap.min.css"" rel=""stylesheet"">

    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src=""https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js""></script>
      <script src=""https://oss.maxcdn.com/respond/1.4.2/respond.min.js""></script>
    <![endif]-->
  </head>
  <body>
";
        const string HtmlLastPart = @"
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src=""https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js""></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src=""js/bootstrap.min.js""></script>
  </body>
</html>
";

        public ParsePageSteps()
        {
            _entries = new List<ICodeEntry<HtmlElementTypes>>();
        }

        [Given(@"I have a web page with a button")]
        public void GivenIHaveAWebPageWithAButton()
        {
            // ScenarioContext.Current.Pending();
            // [Xunit.Theory]
            // [InlineData(@"
            var _html = @"
<button type=""button"" class=""btn btn-default"" aria-label=""Left Align"">
  <span class=""glyphicon glyphicon-align-left"" aria-hidden=""true""></span>
</button>
";
            // , "IButton", 0)]
            var fullHtml = HtmlFirstPart + _html + HtmlLastPart;
            _doc = new HtmlDocument();
            _doc.LoadHtml(fullHtml);
        }
        
        [When(@"I start the parser app")]
        public void WhenIStartTheParserApp()
        {
            // ScenarioContext.Current.Pending();
            var pageLoader = new PageLoader();
            var applicableAnalyzers = new[] { typeof(Web.ObjectModel.Plugins.PlainHtml5) };
            _entries.AddRange(pageLoader.GetCodeEntriesFromNode<HtmlElementTypes>(_doc.DocumentNode, TestFactory.ExcludeList, applicableAnalyzers));
            _entry = _entries.Cast<CodeEntry<HtmlElementTypes>>().ToArray()[0];
        }
        
        [Then(@"the result should be an element of type ""(.*)""")]
        public void ThenTheResultShouldBeAnElementOfType(string p0)
        {
            // ScenarioContext.Current.Pending();
            Xunit.Assert.True(_entry.GenerateCodeForEntry(SupportedLanguages.Java).Contains(p0));
        }
    }
}
