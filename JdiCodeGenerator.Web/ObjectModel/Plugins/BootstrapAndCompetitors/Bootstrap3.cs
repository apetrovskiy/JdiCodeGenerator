﻿namespace JdiCodeGenerator.Web.ObjectModel.Plugins.BootstrapAndCompetitors
{
    using System.Collections.Generic;
    using System.Linq;
    using HtmlAgilityPack;
    using Core;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;
    using Helpers;
    using Abstract;

    public class Bootstrap3 : IFrameworkAlingmentAnalysisPlugin<HtmlElementTypes>
    {
        public IEnumerable<IRule<HtmlElementTypes>> Rules { get; set; }
        public IEnumerable<string> ExcludeList { get; set; }
        public IRule<HtmlElementTypes> RuleThatWon { get; set; }

        public Bootstrap3()
        {
            ExcludeList = new List<string> { "aaa", "bbb", "ccc" };
            Rules = new List<IRule<HtmlElementTypes>>
            {
                new Rule<HtmlElementTypes>
                {
                    Description = "Form input",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Input } } },
                    TargetType = JdiElementTypes.TextField,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "form-control" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Button",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Button, HtmlElementTypes.A, HtmlElementTypes.Input } } },
                    TargetType = JdiElementTypes.Button,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "btn" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Dropdown",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Div } } },
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
                    InternalElements = new Dictionary<string, IRule<HtmlElementTypes>>
                    {
                        { Resources.Bootstrap3_DropDown_root, new Rule<HtmlElementTypes> { SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Div } } }, TargetType = JdiElementTypes.DropDown, OrConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown", "dropup" } } } } },
                        { Resources.Bootstrap3_DropDown_value, new Rule<HtmlElementTypes> { SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Button } } }, TargetType = JdiElementTypes.Button, OrConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown-toggle" } } } } },
                        { Resources.Bootstrap3_DropDown_list, new Rule<HtmlElementTypes> { SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Li } } }, TargetType = JdiElementTypes.ListItem, OrConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Tag, MarkerValues = new List<string> { "li" } } } } }
                    }
                },
                new Rule<HtmlElementTypes>
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
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Div } } },
                    TargetType = JdiElementTypes.DropDown,
                    AndConditions = new List<IRuleCondition>
                    {
                        new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "btn-group" } },
                        // new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Role, MarkerValues = new List<string> { "group" } },
                        new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown-toggle" } },
                        new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown-menu" } }
                    }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Menu item",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Ul } } },
                    TargetType = JdiElementTypes.MenuItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "dropdown-menu" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "CheckBox",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Input } } },
                    TargetType = JdiElementTypes.CheckBox,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Type, MarkerValues = new List<string> { "checkbox" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Tabs",
                    // see also http://getbootstrap.com/javascript/#markup
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Ul } } },
                    TargetType = JdiElementTypes.Tabs,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "nav-tabs", "nav-pills" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "TabItem",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Li } } },
                    TargetType = JdiElementTypes.TabItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Role, MarkerValues = new List<string> { "presentation" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "NavBar",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Nav } } },
                    TargetType = JdiElementTypes.NavBar,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "navbar" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "NavBar Form",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Form } } },
                    TargetType = JdiElementTypes.Form,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "navbar-form" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Pagination",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Ul } } },
                    TargetType = JdiElementTypes.Pagination,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "pagination" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Pager",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Ul } } },
                    TargetType = JdiElementTypes.Pager,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "pager" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Label",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Label } } },
                    TargetType = JdiElementTypes.Label,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Child, Marker = Markers.Class, MarkerValues = new List<string> { "label" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Progress",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Div } } },
                    TargetType = JdiElementTypes.Progress,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "progress" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "List",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Ul } } },
                    TargetType = JdiElementTypes.List,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "list-group" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "ListItem",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Li } } },
                    TargetType = JdiElementTypes.ListItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "list-group-item" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "ListItem alt",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.A } } },
                    TargetType = JdiElementTypes.ListItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "list-group-item" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "ListItem alt 2",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Button } } },
                    TargetType = JdiElementTypes.ListItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "list-group-item" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Table",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Table } } },
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
                new Rule<HtmlElementTypes>
                {
                    Description = "Form inline, group",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Form } } },
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
                new Rule<HtmlElementTypes>
                {
                    Description = "Table alt",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>>(),
                    TargetType = JdiElementTypes.Table,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "table" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Modal",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Div } } },
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
                new Rule<HtmlElementTypes>
                {
                    Description = "Carousel",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Div } } },
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
                new Rule<HtmlElementTypes>
                {
                    Description = "Label",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Span } } },
                    TargetType = JdiElementTypes.Label,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "label" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Jumbo, alert, well",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Div } } },
                    TargetType = JdiElementTypes.Text,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "jumbotron", "page-header", "alert", "well" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "navbar-text",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.P } } },
                    TargetType = JdiElementTypes.Text,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "navbar-text" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "RadioButtons",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Input } } },
                    TargetType = JdiElementTypes.RadioButtons,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Type, MarkerValues = new List<string> { "radio" } } }
                },
                // experimental rules
                new Rule<HtmlElementTypes>
                {
                    Description = "experimental: Menu",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Ul } } },
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