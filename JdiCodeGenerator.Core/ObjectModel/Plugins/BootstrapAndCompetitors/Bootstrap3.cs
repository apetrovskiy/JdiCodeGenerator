namespace JdiCodeGenerator.Core.ObjectModel.Plugins.BootstrapAndCompetitors
{
    using System.Collections.Generic;
    using System.Linq;
    using Abstract;
    using HtmlAgilityPack;

    public class Bootstrap3 : IFrameworkAlingmentAnalysisPlugin
    {
        public IEnumerable<IRule> Rules { get; set; }

        public Bootstrap3()
        {
            Rules = new List<IRule>
            {
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Input },
                    TargetType = JdiElementTypes.TextField,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "form-control" } } }
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Button, HtmlElementTypes.A, HtmlElementTypes.Input },
                    TargetType = JdiElementTypes.Button,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "btn" } } }
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div },
                    TargetType = JdiElementTypes.DropDown,
                    // OrConditions = new List<IRuleCondition>
                    AndConditions = new List<IRuleCondition>
                    {
                        new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown", "dropup" } },
                        // new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown-toggle" } },
                        new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown-menu" }}
                    }
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul },
                    TargetType = JdiElementTypes.MenuItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown-menu" } } }
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Input },
                    TargetType = JdiElementTypes.CheckBox,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Type, MarkerValues = new List<string> { "checkbox" } } }
                },
                new Rule
                {
                    // see also http://getbootstrap.com/javascript/#markup
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul },
                    TargetType = JdiElementTypes.Tabs,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "nav-tabs", "nav-pills" } } }
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Li },
                    TargetType = JdiElementTypes.TabItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Role, MarkerValues = new List<string> { "presentation" } } }
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Nav },
                    TargetType = JdiElementTypes.NavBar,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "navbar" } } }
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Form },
                    TargetType = JdiElementTypes.Form,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "navbar-form" } } }
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul },
                    TargetType = JdiElementTypes.Pagination,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "pagination" } } }
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul },
                    TargetType = JdiElementTypes.Pager,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "pager" } } }
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Label },
                    TargetType = JdiElementTypes.Label,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Child, Marker = Markers.Class, MarkerValues = new List<string> { "label" } } }
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div },
                    TargetType = JdiElementTypes.Progress,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "progress" } } }
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul },
                    TargetType = JdiElementTypes.List,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "list-group" } } }
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Li },
                    TargetType = JdiElementTypes.ListItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "list-group-item" } } }
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.A },
                    TargetType = JdiElementTypes.ListItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "list-group-item" } } }
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Button },
                    TargetType = JdiElementTypes.ListItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "list-group-item" } } }
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Table },
                    TargetType = JdiElementTypes.Table,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "table" } } }
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Form },
                    TargetType = JdiElementTypes.Form,
                    OrConditions = new List<IRuleCondition>
                    {
                        new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "form-inline", "form-horizontal" } },
                        new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "form-group", "form-control" } }
                    }
                    //,
                    //AndConditions = new List<IRuleCondition>
                    //{
                    //    // new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "form-group" } },
                    //    new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> {  } }
                    //}
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes>(),
                    TargetType = JdiElementTypes.Table,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "table" } } }
                },
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div },
                    TargetType = JdiElementTypes.Popup,
                    AndConditions = new List<IRuleCondition>
                    {
                        new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "modal fade", "modal" } }, // ??
                        new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "modal-dialog" } },
                        new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "modal-content" } }
                    }
                },
                // popover see http://getbootstrap.com/javascript/#four-directions-1
                // carousel see http://getbootstrap.com/javascript/#carousel-examples
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div },
                    TargetType = JdiElementTypes.Carousel,
                    AndConditions = new List<IRuleCondition>
                    {
                        new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "carousel slide" } },
                        // new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "carousel" } },
                        new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "carousel-indicators" } }
                        // ,
                        // new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "carousel-control" } }
                    }
                },
                // experimental rules
                new Rule
                {
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul },
                    TargetType = JdiElementTypes.Menu,
                    AndConditions = new List<IRuleCondition>
                    {
                        new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "nav" } },
                        //new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown-toggle" } },
                        //new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown-menu" } }
                        new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown-toggle", "dropdown-menu" } }
                    }
                }
            };
        }

        public JdiElementTypes Analyze(HtmlNode node)
        {
            return GetJdiTypeOfElementByUsingRules(node);
        }

        JdiElementTypes GetJdiTypeOfElementByUsingRules(HtmlNode node)
        {
            var firstRule = Rules.FirstOrDefault(rule => rule.IsMatch(node));
            return firstRule?.TargetType ?? JdiElementTypes.Element;
        }
    }
}