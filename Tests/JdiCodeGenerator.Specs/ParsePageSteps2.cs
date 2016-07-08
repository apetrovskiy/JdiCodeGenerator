using System;
using TechTalk.SpecFlow;

namespace JdiCodeGenerator.Specs
{
    [Binding]
    public class ParsePageSteps2
    {
        [Then(@"the result should be a ""(.*)""")]
        public void ThenTheResultShouldBeA(string p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
