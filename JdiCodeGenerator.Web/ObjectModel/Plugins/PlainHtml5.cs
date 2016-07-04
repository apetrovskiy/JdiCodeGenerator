namespace JdiCodeGenerator.Web.ObjectModel.Plugins
{
    using System.Collections.Generic;
    using Abstract;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;

    public class PlainHtml5 : FrameworkAlignmentAnalysisPlugin
    {
        public PlainHtml5()
        {
            Priority = 0;

            Rules = new List<IRule<HtmlElementTypes>>
            {
                new Rule<HtmlElementTypes>
                {
                    Description = "Text field",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Input } } },
                    TargetType = JdiElementTypes.TextField,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Type, MarkerValues = new List<string> { "text", "email" } } }
                },
                //new Rule<HtmlElementTypes>
                //{
                //    Description = "Button",
                //    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Button, HtmlElementTypes.A, HtmlElementTypes.Input } } },
                //    TargetType = JdiElementTypes.Button,
                //    OrConditions = new List<IRuleCondition>
                //    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "button" } } }
                //}
                new Rule<HtmlElementTypes>
                {
                    Description = "something non-useful",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Body } } },
                    TargetType = JdiElementTypes.Button,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "hzhz" } } }
                }
            };
        }
    }
}