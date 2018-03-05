namespace CodeGenerator.Web.ObjectModel.Plugins.Plain
{
    using System.Collections.Generic;
    using Abstract;
    using Core;
    using Core.ObjectModel.Abstract.Rules;
    using Core.ObjectModel.Enums;
    using Core.ObjectModel.Rules;
    using JdiConverters.ObjectModel.Enums;

    public class Jdi : FrameworkAlignmentAnalysisPlugin
    {
        public Jdi()
        {
            Priority = -1;

            Rules = new List<IRule<HtmlElementTypes>>
            {
                new Rule<HtmlElementTypes>
                {
                    Description = "Pagination",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Ul },
                    TargetType = JdiElementTypes.Pagination,
                    AndConditions = new List<IRuleCondition> { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.OtherAttribute, MarkerValues = new List<string> { "IPagination" } } },
                    InternalElements = new Dictionary<string, IRule<HtmlElementTypes>>
                    {
                        // root?
                        // value?
                        { Resources.Jdi_DropDown_list, new Rule<HtmlElementTypes> { SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Li }, TargetType = JdiElementTypes.ListItem, AndConditions = new List<IRuleCondition> { new RuleCondition { NodeRelationship = NodeRelationships.Descendant, MarkerAttribute = MarkerAttributes.Tag, MarkerValues = new List<string> { "li" } } } } }
                    }
                },
                // <input id="Name" type="text" class="uui-form-element" jdi-type="ITextField" jdi-name="Name" jdi-parent="contactForm">
                new Rule<HtmlElementTypes>
                {
                    Description = "TextField",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Input },
                    TargetType = JdiElementTypes.TextField,
                    AndConditions = new List<IRuleCondition> { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.OtherAttribute, MarkerValues = new List<string> { "ITextField" } } }
                },
                /*
    <div id="datepicker" class="date uui-datepicker date-button small">
        <input type="text" class="uui-form-element small" jdi-type="IDatePicker" jdi-parent="contactForm" jdi-name="date">
        <span class="input-group-addon uui-button small">
            <i class="fa fa-calendar"></i>
        </span>
    </div>
                */
                new Rule<HtmlElementTypes>
                {
                    Description = "DatePicker",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Input },
                    TargetType = JdiElementTypes.DatePicker,
                    AndConditions = new List<IRuleCondition> { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.OtherAttribute, MarkerValues = new List<string> { "IDatePicker" } } }
                },
                /*
<div class="input-append bootstrap-timepicker uui-timepicker time-button">
    <input id="timepicker" type="text" class="uui-form-element small" jdi-type="ITimePicker" jdi-parent="contactForm" jdi-name="time">
    <span class="add-on uui-button small">
        <i class="fa fa-clock-o"></i>
    </span>
</div>
                */
                new Rule<HtmlElementTypes>
                {
                    Description = "TimePicker",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Input },
                    TargetType = JdiElementTypes.TimePicker,
                    AndConditions = new List<IRuleCondition> { new RuleCondition { NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.OtherAttribute, MarkerValues = new List<string> { "ITimePicker" } } }
                },
                // <button class="uui-button dark-blue m-t35" type="submit" jdi-type="IButton" jdi-parent="contactForm" jdi-name="submit">Submit</button>
                new Rule<HtmlElementTypes>
                {
                    Description = "Button",
                    SourceTypes = new List<HtmlElementTypes> { HtmlElementTypes.Button },
                    TargetType = JdiElementTypes.Button,
                    AndConditions = new List<IRuleCondition> { new RuleCondition {  NodeRelationship = NodeRelationships.Self, MarkerAttribute = MarkerAttributes.OtherAttribute, MarkerValues = new List<string> { "IButton" } } }
                }

            };
        }
    }
}