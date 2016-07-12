namespace JdiCodeGenerator.Web.ObjectModel.Plugins.JavaScript
{
    using System.Collections.Generic;
    using Abstract;
    using Core;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;

    public class JqueryBootstrapSelect : FrameworkAlignmentAnalysisPlugin
    {
        public JqueryBootstrapSelect()
        {
            Priority = 50;

            // https://silviomoreto.github.io/bootstrap-select/
            // https://silviomoreto.github.io/bootstrap-select/examples/
            Rules = new List<IRule<HtmlElementTypes>>
            {
                new Rule<HtmlElementTypes>
                {
                    Description = "ComboBox",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div },
                    TargetType = JdiElementTypes.ComboBox,
                    AndConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "bootstrap-select" } } },
                    InternalElements = new Dictionary<string, IRule<HtmlElementTypes>>
                    {
                        { Resources.Jdi_DropDown_root, new Rule<HtmlElementTypes> { SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div }, TargetType = JdiElementTypes.DropDown, OrConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "bootstrap-select" } } } } },
                        { Resources.Jdi_DropDown_value, new Rule<HtmlElementTypes> { SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Button }, TargetType = JdiElementTypes.Button, OrConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown-toggle" } } } } },
                        { Resources.Jdi_DropDown_list, new Rule<HtmlElementTypes> { SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul }, TargetType = JdiElementTypes.ListItem, OrConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown-menu" } } } } }
                    }
                }
            };
        }
    }
}