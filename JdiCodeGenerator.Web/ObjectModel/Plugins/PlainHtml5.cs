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
                new Rule<HtmlElementTypes>
                {
                    Description = "Button",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> { Types = new List<HtmlElementTypes> { HtmlElementTypes.Button } } },
                    TargetType = JdiElementTypes.Button,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "button" } } }
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
                new Rule<HtmlElementTypes>
                {
                    Description = "DropDownList",
                    SourceTypes = new List<SourceElementTypeCollection<HtmlElementTypes>> { new SourceElementTypeCollection<HtmlElementTypes> {  Types = new List<HtmlElementTypes> { HtmlElementTypes.Select } } },
                    TargetType = JdiElementTypes.DropDown,
                    OrConditions = new List<IRuleCondition>
                    { new RuleCondition { Relationship = NodeRelationships.Self, Marker = Markers.Tag, MarkerValues = new List<string> { "select" } } }
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
                }
            };
        }
    }
}