namespace JdiCodeGenerator.Web.ObjectModel.Plugins
{
    using System.Collections.Generic;
    using Abstract;
    using Core;
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
                /*
<button class="uui-button dark-blue" type="submit" id="submit-button">Submit</button>
                */
                new Rule<HtmlElementTypes>
                {
                    Description = "Button",
                    // 20160708
                    // experimental
                    // for Sharepoint
                    // SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Button } } },
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Button, HtmlElementTypes.Input } } },
                    TargetType = JdiElementTypes.Button,
                    OrConditions = new List<IRuleCondition>
                    // { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "button" } } }
                    {
                        new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "button" } },
                        new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Type, MarkerValues = new List<string> { "submit" }}
                    }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Link",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Link } } },
                    TargetType = JdiElementTypes.Link,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "link" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "TextArea",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Textarea } } },
                    TargetType = JdiElementTypes.TextArea,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "textarea" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Hyperlink",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.A } } },
                    TargetType = JdiElementTypes.Link,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "a" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "Label",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Label } } },
                    TargetType = JdiElementTypes.Label,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "label" } } }
                },
                new Rule<HtmlElementTypes>
                {
                    Description = "something non-useful",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Body } } },
                    TargetType = JdiElementTypes.Button,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "hzhz" } } }
                },
                /*
<select>
  <option value="volvo">Volvo</option>
  <option value="saab">Saab</option>
  <option value="mercedes">Mercedes</option>
  <option value="audi">Audi</option>
</select>
                */
                //new Rule<HtmlElementTypes>
                //{
                //    Description = "DropDown",
                //    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> {  Types = new List<HtmlElementTypes> { HtmlElementTypes.Select } } },
                //    TargetType = JdiElementTypes.DropDown,
                //    OrConditions = new List<IRuleCondition>
                //    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "select" } } },
                //},
                new Rule<HtmlElementTypes>
                {
                    Description = "ComboBox",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> {  Types = new List<HtmlElementTypes> { HtmlElementTypes.Select } } },
                    TargetType = JdiElementTypes.ComboBox,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "select" } } },
                    InternalElements = new Dictionary<string, IRule<HtmlElementTypes>>
                    {
                        //{ Resources.Jdi_DropDown_root, new Rule<HtmlElementTypes> { SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Select } } }, TargetType = JdiElementTypes.DropDown, OrConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "select" } } } } },
                        //{ Resources.Jdi_DropDown_value, new Rule<HtmlElementTypes> { SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Select } } }, TargetType = JdiElementTypes.DropDown, OrConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "select" } } } } },
                        //{ Resources.Jdi_DropDown_list, new Rule<HtmlElementTypes> { SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Option } } }, TargetType = JdiElementTypes.ListItem, OrConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Tag, MarkerValues = new List<string> { "option" } } } } }
                        { Resources.Jdi_DropDown_root, new Rule<HtmlElementTypes> { SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Select } } }, TargetType = JdiElementTypes.DropDown, AndConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "select" } } } } },
                        { Resources.Jdi_DropDown_value, new Rule<HtmlElementTypes> { SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Select } } }, TargetType = JdiElementTypes.DropDown, AndConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "select" } } } } },
                        // { Resources.Jdi_DropDown_list, new Rule<HtmlElementTypes> { SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Option } } }, TargetType = JdiElementTypes.ListItem, AndConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Tag, MarkerValues = new List<string> { "option" } } } } }
                        { Resources.Jdi_DropDown_list, new Rule<HtmlElementTypes> { SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Select } } }, TargetType = JdiElementTypes.DropDown, AndConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "select" } } } } }
                    }
                },
                /*
<form action="demo_form.asp" method="get">
  First name: <input type="text" name="fname"><br>
  Last name: <input type="text" name="lname"><br>
  <input type="submit" value="Submit">
</form>
                */
                new Rule<HtmlElementTypes>
                {
                    Description = "Form",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> {  Types = new List<HtmlElementTypes> { HtmlElementTypes.Form } } },
                    TargetType = JdiElementTypes.Form,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition {Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "form" } } }
                },
//                ,
//                /*
//<div class="ma-data-wrap">
//	<select class="ma-ddl">
//		<option value="afg">Afghanistan</option>
//                */
//                new Rule<HtmlElementTypes>
//                {
//                    Description = "DropDown with options",
//                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> {  Types = new List<HtmlElementTypes> { HtmlElementTypes.Select } } },
//                    TargetType = JdiElementTypes.DropDown,
//                    OrConditions = new List<IRuleCondition>
//                    { new RuleCondition {Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "form" } } }
//                }
                /*
<section class="vertical-group" id="elements-checklist">
	<p class="checkbox">
		<input type="checkbox" id="g1" class="uui-form-element blue">
		<label for="g1">Water</label>
	</p>
</section>
                */
                new Rule<HtmlElementTypes>
                {
                    Description = "CheckList",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Section } } },
                    TargetType = JdiElementTypes.CheckList,
                    AndConditions = new List<IRuleCondition>
                    {
                        new RuleCondition {  Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "vertical-group" } },
                        new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Tag, MarkerValues = new List<string> { "input" } },
                        new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Type, MarkerValues = new List<string> { "checkbox" } }
                    },
                    InternalElements = new Dictionary<string, IRule<HtmlElementTypes>>
                    {
                        // { Resources.Jdi_DropDown_list, new Rule<HtmlElementTypes> { SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Input } } }, TargetType = JdiElementTypes.CheckBox, AndConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Tag, MarkerValues = new List<string> { "input" } }, new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Type, MarkerValues = new List<string> { "checkbox" } } } } }
                        // { Resources.Jdi_DropDown_list, new Rule<HtmlElementTypes> { SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Label } } }, TargetType = JdiElementTypes.CheckBox, AndConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Tag, MarkerValues = new List<string> { "label" } } } } }
                        { Resources.Jdi_DropDown_list, new Rule<HtmlElementTypes> { SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Label } } }, TargetType = JdiElementTypes.CheckBox } }
                    }
                },
                /*
<section class="horizontal-group" id="odds-selector">
	<p class="radio">
		<input type="radio" name="custom_radio_odd" id="p1" checked="" class="uui-form-element">
		<label for="p1">1</label>
	</p>
</section>
                */
                new Rule<HtmlElementTypes>
                {
                    Description = "RadioButtons",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Section } } },
                    TargetType = JdiElementTypes.RadioButtons,
                    AndConditions = new List<IRuleCondition>
                    {
                        new RuleCondition {  Relationship = NodeRelationships.Self, Marker = Markers.Class, MarkerValues = new List<string> { "horizontal-group" } },
                        new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Tag, MarkerValues = new List<string> { "input" } },
                        new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Type, MarkerValues = new List<string> { "radio" } }
                    },
                    InternalElements = new Dictionary<string, IRule<HtmlElementTypes>>
                    {
                        // { Resources.Jdi_DropDown_list, new Rule<HtmlElementTypes> { SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Input } } }, TargetType = JdiElementTypes.CheckBox, AndConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Tag, MarkerValues = new List<string> { "input" } }, new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Type, MarkerValues = new List<string> { "checkbox" } } } } }
                        // { Resources.Jdi_DropDown_list, new Rule<HtmlElementTypes> { SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Label } } }, TargetType = JdiElementTypes.CheckBox, AndConditions = new List<IRuleCondition> { new RuleCondition { Relationship = NodeRelationships.Descendant, Marker = Markers.Tag, MarkerValues = new List<string> { "label" } } } } }
                        { Resources.Jdi_DropDown_list, new Rule<HtmlElementTypes> { SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Label } } }, TargetType = JdiElementTypes.RadioButtons } }
                    }
                }

            };
        }
    }
}