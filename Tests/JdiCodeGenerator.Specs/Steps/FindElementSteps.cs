﻿namespace CodeGenerator.Specs.Steps
{
    using Core.ObjectModel.Enums;
    using Core.ObjectModel.Results;
    using HtmlAgilityPack;
    using TechTalk.SpecFlow;
    using Tests.Plugins;
    using Web.ObjectModel.Plugins.BootstrapAndCompetitors;
    using Web.ObjectModel.Plugins.Plain;

    [Binding]
    public class FindElementSteps
    {
        HtmlDocument _doc;
        PageMemberCodeEntry _entry;

        [Given(@"I have a Bootstrap web page ""(.*)""")]
        public void GivenIHaveABootstrapPage(string path)
        {
            var fullHtml = TestFactory.Instance.GetBootstrap3Page(path);
            _doc = new HtmlDocument();
            _doc.LoadHtml(fullHtml);
        }

        [When(@"I start the parser app")]
        public void WhenIStartTheParserApp()
        {
            _entry = TestFactory.Instance.GetEntryExpected(_doc, new[] {typeof(Bootstrap3), typeof(PlainHtml5)}, 0);
        }
        
        [Then(@"the result should be an element of type ""(.*)""")]
        public void ThenTheResultShouldBeAnElementOfType(string p0)
        {
            Xunit.Assert.True(_entry.GenerateCode(SupportedTargetLanguages.Java).Contains(p0));
        }

        [Then(@"the result should be a ""(.*)""")]
        public void ThenTheResultShouldBeA(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the result should be a collection of elements")]
        public void ThenTheResultShouldBeACollectionOfElements()
        {
            Xunit.Assert.True(!string.IsNullOrEmpty(_entry.GenerateCode(SupportedTargetLanguages.Java)));
        }
    }
}
