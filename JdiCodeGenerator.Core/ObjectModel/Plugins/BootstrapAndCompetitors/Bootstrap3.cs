namespace JdiCodeGenerator.Core.ObjectModel.Plugins.BootstrapAndCompetitors
{
    using System.Collections.Generic;
    using System.Linq;
    using Abstract;
    using Helpers;
    using HtmlAgilityPack;

    public class Bootstrap3 : IFrameworkAlingmentAnalysisPlugin
    {
        public IEnumerable<IRule> Rules { get; set; }
        public IEnumerable<string> ExcludeList { get; set; }
        public IRule RuleThatWon { get; set; }

        public Bootstrap3()
        {
            ExcludeList = new List<string> { "aaa", "bbb", "ccc" };
            Rules = new List<IRule>
            {
                new Rule
                {
                    Description = "Form input",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Input },
                    TargetType = JdiElementTypes.TextField,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "form-control" } } }
                },
                new Rule
                {
                    Description = "Button",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Button, HtmlElementTypes.A, HtmlElementTypes.Input },
                    TargetType = JdiElementTypes.Button,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "btn" } } }
                },
                new Rule
                {
                    Description = "Dropdown",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div },
                    TargetType = JdiElementTypes.DropDown,
                    // OrConditions = new List<IRuleCondition>
                    AndConditions = new List<IRuleCondition>
                    {
                        new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown", "dropup" } },
                        new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown-toggle" } },
                        new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown-menu" } }
                    },
                    /*
@JDropdown(root = @FindBy(css = "dropdown"), value = @FindBy(id = "dropdownMenu1"), list = @FindBy(tagName = "li"))
IDropDown<JobCategories> category;
                    */
                    InternalElements = new Dictionary<string, IRule>
                    {
                        { Resources.Bootstrap3_DropDown_root, new Rule { SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div }, TargetType = JdiElementTypes.DropDown, OrConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown", "dropup" } } } } },
                        { Resources.Bootstrap3_DropDown_value, new Rule { SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Button }, TargetType = JdiElementTypes.Button, OrConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown-toggle" } } } } },
                        { Resources.Bootstrap3_DropDown_list, new Rule { SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Li }, TargetType = JdiElementTypes.ListItem, OrConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Tag, MarkerValues = new List<string> { "li" } } } } }
                    }
                },
                new Rule
                {
                    #region example
                    /*
<div class="btn-group" role="group" aria-label="...">
  <button type="button" class="btn btn-default">1</button>
  <button type="button" class="btn btn-default">2</button>

  <div class="btn-group" role="group">
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
      Dropdown
      <span class="caret"></span>
    </button>
    <ul class="dropdown-menu">
      <li><a href="#">Dropdown link</a></li>
      <li><a href="#">Dropdown link</a></li>
    </ul>
  </div>
</div>
                    */
                    #endregion
                    Description = "Dropdown in a button of the button group",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div },
                    TargetType = JdiElementTypes.DropDown,
                    AndConditions = new List<IRuleCondition>
                    {
                        new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "btn-group" } },
                        // new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Role, MarkerValues = new List<string> { "group" } },
                        new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown-toggle" } },
                        new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown-menu" } }
                    }
                },
                new Rule
                {
                    Description = "Menu item",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul },
                    TargetType = JdiElementTypes.MenuItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown-menu" } } }
                },
                new Rule
                {
                    Description = "CheckBox",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Input },
                    TargetType = JdiElementTypes.CheckBox,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Type, MarkerValues = new List<string> { "checkbox" } } }
                },
                new Rule
                {
                    Description = "Tabs",
                    // see also http://getbootstrap.com/javascript/#markup
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul },
                    TargetType = JdiElementTypes.Tabs,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "nav-tabs", "nav-pills" } } }
                },
                new Rule
                {
                    Description = "TabItem",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Li },
                    TargetType = JdiElementTypes.TabItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Role, MarkerValues = new List<string> { "presentation" } } }
                },
                new Rule
                {
                    Description = "NavBar",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Nav },
                    TargetType = JdiElementTypes.NavBar,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "navbar" } } }
                },
                new Rule
                {
                    Description = "NavBar Form",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Form },
                    TargetType = JdiElementTypes.Form,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "navbar-form" } } }
                },
                new Rule
                {
                    Description = "Pagination",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul },
                    TargetType = JdiElementTypes.Pagination,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "pagination" } } }
                },
                new Rule
                {
                    Description = "Pager",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul },
                    TargetType = JdiElementTypes.Pager,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "pager" } } }
                },
                new Rule
                {
                    Description = "Label",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Label },
                    TargetType = JdiElementTypes.Label,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Child, Marker = Markers.Class, MarkerValues = new List<string> { "label" } } }
                },
                new Rule
                {
                    Description = "Progress",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div },
                    TargetType = JdiElementTypes.Progress,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "progress" } } }
                },
                new Rule
                {
                    Description = "List",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul },
                    TargetType = JdiElementTypes.List,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "list-group" } } }
                },
                new Rule
                {
                    Description = "ListItem",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Li },
                    TargetType = JdiElementTypes.ListItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "list-group-item" } } }
                },
                new Rule
                {
                    Description = "ListItem alt",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.A },
                    TargetType = JdiElementTypes.ListItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "list-group-item" } } }
                },
                new Rule
                {
                    Description = "ListItem alt 2",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Button },
                    TargetType = JdiElementTypes.ListItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "list-group-item" } } }
                },
                new Rule
                {
                    Description = "Table",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Table },
                    TargetType = JdiElementTypes.Table,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "table" } } }
                },
                //new Rule
                //{
                //    Description = "Form inline, group",
                //    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Form },
                //    TargetType = JdiElementTypes.Form,
                //    OrConditions = new List<IRuleCondition>
                //    {
                //        new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "form-inline", "form-horizontal" } },
                //        new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "form-group", "form-control" } }
                //    }
                //    //,
                //    //AndConditions = new List<IRuleCondition>
                //    //{
                //    //    // new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "form-group" } },
                //    //    new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> {  } }
                //    //}
                //},
                new Rule
                {
                    Description = "Form inline, group",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Form },
                    TargetType = JdiElementTypes.Form,
                    OrConditions = new List<IRuleCondition>
                    {
                        // new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "form-inline", "form-horizontal" } },
                        new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "form" } },
                        new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "form" } }
                    }
                    ,
                    AndConditions = new List<IRuleCondition>
                    {
                        new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "form-group", "form-control" } }
                    }
                },
                new Rule
                {
                    Description = "Table alt",
                    SourceTypes = new List<HtmlElementTypes>(),
                    TargetType = JdiElementTypes.Table,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "table" } } }
                },
                new Rule
                {
                    Description = "Modal",
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
                    Description = "Carousel",
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
                new Rule
                {
                    Description = "Label",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Span },
                    TargetType = JdiElementTypes.Label,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "label" } } }
                },
                new Rule
                {
                    Description = "Jumbo, alert, well",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div },
                    TargetType = JdiElementTypes.Text,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "jumbotron", "page-header", "alert", "well" } } }
                },
                new Rule
                {
                    Description = "navbar-text",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.P },
                    TargetType = JdiElementTypes.Text,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "navbar-text" } } }
                },
                // experimental rules
                new Rule
                {
                    Description = "experimental: Menu",
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
            
            // experimental
            RuleThatWon = firstRule;
            ExtensionMethodsForNodes.AnalyzerThatWon = this;

            // return firstRule?.TargetType ?? JdiElementTypes.Element;
            return null == firstRule ? JdiElementTypes.Element : firstRule.TargetType;
        }
    }
}