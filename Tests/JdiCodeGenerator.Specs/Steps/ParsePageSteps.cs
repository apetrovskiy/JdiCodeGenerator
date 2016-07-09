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
    using Web.ObjectModel.Plugins;
    using Web.ObjectModel.Plugins.BootstrapAndCompetitors;

    [Binding]
    public class ParsePageSteps
    {
        string _html;
        HtmlDocument _doc;
        CodeEntry<HtmlElementTypes> _entry;
        readonly List<ICodeEntry<HtmlElementTypes>> _entries;

        public ParsePageSteps()
        {
            _entries = new List<ICodeEntry<HtmlElementTypes>>();
        }

        [Given(@"I have a Bootstrap web page ""(.*)""")]
        public void GivenIHaveABootstrapPage(string path)
        {
            var fullHtml = TestFactory.GetBootstrap3Page(path);
            _doc = new HtmlDocument();
            _doc.LoadHtml(fullHtml);
        }

        [When(@"I start the parser app")]
        public void WhenIStartTheParserApp()
        {
            var pageLoader = new PageLoader();
            var applicableAnalyzers = new[] { typeof(Bootstrap3), typeof(PlainHtml5) };
            _entries.AddRange(pageLoader.GetCodeEntriesFromNode<HtmlElementTypes>(_doc.DocumentNode, TestFactory.ExcludeList, applicableAnalyzers));
            _entry = _entries.Cast<CodeEntry<HtmlElementTypes>>().ToArray()[0];
        }
        
        [Then(@"the result should be an element of type ""(.*)""")]
        public void ThenTheResultShouldBeAnElementOfType(string p0)
        {
            Xunit.Assert.True(_entry.GenerateCodeForEntry(SupportedLanguages.Java).Contains(p0));
        }
    }
}
