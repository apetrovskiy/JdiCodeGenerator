namespace CodeGenerator.Web.ObjectModel.Plugins.BootstrapAndCompetitors
{
	using System.Collections.Generic;
	using Abstract;
	using Core;
	using Core.ObjectModel.Abstract.Rules;
	using Core.ObjectModel.Enums;
	using Core.ObjectModel.Rules;
	using JdiConverters.ObjectModel.Enums;

	public class Bootstrap3 : FrameworkAlignmentAnalysisPlugin
    {
        public Bootstrap3()
        {
            // TODO: set priority depending on the user's choice
            Priority = 100;

            ExcludeList = new List<string> { "aaa", "bbb", "ccc" };
            Rules = new List<IRule<HtmlElementTypes>>
            {
                new Rule<HtmlElementTypes>
                {
                    Description = "Form input",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Input },
                    TargetType = JdiElementTypes.TextField,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "form-control" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Button",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Button, HtmlElementTypes.A, HtmlElementTypes.Input },
                    TargetType = JdiElementTypes.Button,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "btn" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Dropdown",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div },
                    TargetType = JdiElementTypes.DropDown,
                    // OrConditions = new List<IRuleCondition>
                    AndConditions = new List<IRuleCondition>
                    {
                        new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "dropdown", "dropup" } },
                        new RuleCondition { NodeRelationship = NodeRelationships.Descendant, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "dropdown-toggle" } },
                        new RuleCondition { NodeRelationship = NodeRelationships.Descendant, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "dropdown-menu" } }
                    },
                    /*
@JDropdown(root = @FindBy(css = "dropdown"), value = @FindBy(id = "dropdownMenu1"), list = @FindBy(tagName = "li"))
IDropDown<JobCategories> category;
                    */
                    InternalElements = new Dictionary<string, IRule<HtmlElementTypes>>
                    {
                        { Resources.Jdi_DropDown_root, new Rule<HtmlElementTypes> { SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div }, TargetType = JdiElementTypes.DropDown, OrConditions = new List<IRuleCondition> { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "dropdown", "dropup" } } } } },
                        { Resources.Jdi_DropDown_value, new Rule<HtmlElementTypes> { SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Button }, TargetType = JdiElementTypes.Button, OrConditions = new List<IRuleCondition> { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "dropdown-toggle" } } } } },
                        // ??
                        // { Resources.Jdi_DropDown_list, new Rule<HtmlElementTypes> { SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Li } } }, TargetType = JdiElementTypes.ListItem, OrConditions = new List<IRuleCondition> { new RuleCondition { NodeRelationship = NodeRelationships.Descendant, MarkerAttribute = MarkerAttributes.Tag, MarkerValues = new List<string> { "li" } } } } }
                        { Resources.Jdi_DropDown_list, new Rule<HtmlElementTypes> { SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul }, TargetType = JdiElementTypes.ListItem, OrConditions = new List<IRuleCondition> { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "dropdown-menu" } } } } }
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
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div },
                    TargetType = JdiElementTypes.DropDown,
                    AndConditions = new List<IRuleCondition>
                    {
                        new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "btn-group" } },
                        // new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Role, MarkerValues = new List<string> { "group" } },
                        new RuleCondition { NodeRelationship = NodeRelationships.Descendant, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "dropdown-toggle" } },
                        new RuleCondition { NodeRelationship = NodeRelationships.Descendant, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "dropdown-menu" } }
                    },
                    // 20160706
                    InternalElements = new Dictionary<string, IRule<HtmlElementTypes>>
                    {
                        { Resources.Jdi_DropDown_root, new Rule<HtmlElementTypes> { SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div }, TargetType = JdiElementTypes.DropDown, AndConditions = new List<IRuleCondition> { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "btn-group" } } } } },
                        { Resources.Jdi_DropDown_value, new Rule<HtmlElementTypes> { SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Button, HtmlElementTypes.A }, TargetType = JdiElementTypes.Button, AndConditions = new List<IRuleCondition> { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "dropdown-toggle" } } } } },
                        { Resources.Jdi_DropDown_list, new Rule<HtmlElementTypes> { SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul }, TargetType = JdiElementTypes.ListItem, OrConditions = new List<IRuleCondition> { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "dropdown-menu" } } } } }
                    }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Menu item",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul },
                    TargetType = JdiElementTypes.MenuItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "dropdown-menu" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "CheckBox",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Input },
                    TargetType = JdiElementTypes.CheckBox,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Type, MarkerValues = new List<string> { "checkbox" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Tabs",
                    // see also http://getbootstrap.com/javascript/#markup
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul },
                    TargetType = JdiElementTypes.Tabs,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "nav-tabs", "nav-pills" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "TabItem",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Li },
                    TargetType = JdiElementTypes.TabItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Role, MarkerValues = new List<string> { "presentation" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "NavBar",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Nav },
                    TargetType = JdiElementTypes.NavBar,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "navbar" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "NavBar Form",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Form },
                    TargetType = JdiElementTypes.Form,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "navbar-form" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Pagination",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul },
                    TargetType = JdiElementTypes.Pagination,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "pagination" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Pager",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul },
                    TargetType = JdiElementTypes.Pager,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "pager" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Label",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Label },
                    TargetType = JdiElementTypes.Label,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Child, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "label" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Progress",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div },
                    TargetType = JdiElementTypes.Progress,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "progress" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "List",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul },
                    TargetType = JdiElementTypes.List,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "list-group" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "ListItem",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Li },
                    TargetType = JdiElementTypes.ListItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "list-group-item" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "ListItem alt",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.A },
                    TargetType = JdiElementTypes.ListItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "list-group-item" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "ListItem alt 2",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Button },
                    TargetType = JdiElementTypes.ListItem,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "list-group-item" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Table",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Table },
                    TargetType = JdiElementTypes.Table,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "table" } } }
                },
                //new Rule
                //{
                //    Description = "Form inline, group",
                //    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Form },
                //    TargetType = JdiElementTypes.Form,
                //    OrConditions = new List<IRuleCondition>
                //    {
                //        new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "form-inline", "form-horizontal" } },
                //        new RuleCondition { NodeRelationship = NodeRelationships.Descendant, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "form-group", "form-control" } }
                //    }
                //    //,
                //    //AndConditions = new List<IRuleCondition>
                //    //{
                //    //    // new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "form-group" } },
                //    //    new RuleCondition { NodeRelationship = NodeRelationships.Descendant, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> {  } }
                //    //}
                //},
                new Rule<HtmlElementTypes>
                {
                    Description = "Form inline, group",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Form },
                    TargetType = JdiElementTypes.Form,
                    OrConditions = new List<IRuleCondition>
                    {
                        // new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "form-inline", "form-horizontal" } },
                        new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "form" } },
                        new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Tag, MarkerValues = new List<string> { "form" } }
                    }
                    ,
                    AndConditions = new List<IRuleCondition>
                    {
                        new RuleCondition { NodeRelationship = NodeRelationships.Descendant, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "form-group", "form-control" } }
                    }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Table alt",
                    SourceTypes = new List<HtmlElementTypes>(),
                    TargetType = JdiElementTypes.Table,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "table" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Modal",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div },
                    TargetType = JdiElementTypes.Popup,
                    AndConditions = new List<IRuleCondition>
                    {
                        new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "modal fade", "modal" } }, // ??
                        new RuleCondition { NodeRelationship = NodeRelationships.Descendant, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "modal-dialog" } },
                        new RuleCondition { NodeRelationship = NodeRelationships.Descendant, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "modal-content" } }
                    }
                },
                // popover see http://getbootstrap.com/javascript/#four-directions-1
                // carousel see http://getbootstrap.com/javascript/#carousel-examples
                new Rule<HtmlElementTypes>
                {
                    Description = "Carousel",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div },
                    TargetType = JdiElementTypes.Carousel,
                    AndConditions = new List<IRuleCondition>
                    {
                        new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "carousel slide" } },
                        // new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "carousel" } },
                        new RuleCondition { NodeRelationship = NodeRelationships.Descendant, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "carousel-indicators" } }
                        // ,
                        // new RuleCondition { NodeRelationship = NodeRelationships.Descendant, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "carousel-control" } }
                    }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Label",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Span },
                    TargetType = JdiElementTypes.Label,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "label" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Jumbo, alert, well",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Div },
                    TargetType = JdiElementTypes.Text,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "jumbotron", "page-header", "alert", "well" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "navbar-text",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.P },
                    TargetType = JdiElementTypes.Text,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "navbar-text" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "RadioButtons",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Input },
                    TargetType = JdiElementTypes.RadioButtons,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Type, MarkerValues = new List<string> { "radio" } } }
                },
                // experimental rules
                new Rule<HtmlElementTypes>
                {
                    Description = "experimental: Menu",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul },
                    TargetType = JdiElementTypes.Menu,
                    AndConditions = new List<IRuleCondition>
                    {
                        new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "nav" } },
                        //new RuleCondition { NodeRelationship = NodeRelationships.Descendant, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "dropdown-toggle" } },
                        //new RuleCondition { NodeRelationship = NodeRelationships.Descendant, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "dropdown-menu" } }
                        new RuleCondition { NodeRelationship = NodeRelationships.Descendant, MarkerAttribute = MarkerAttributes.Class, MarkerValues = new List<string> { "dropdown-toggle", "dropdown-menu" } }
                    }
                }
            };
        }
    }
}