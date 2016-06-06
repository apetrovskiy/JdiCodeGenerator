namespace JdiCodeGenerator.Core.ObjectModel
{
    using System.Collections.Generic;
    using Abstract;

    public class Rule : IRule
    {
        public IEnumerable<IRuleCondition> OrConditions { get; set; }
        public string Description { get; set; }
        public HtmlElementTypes SourceType { get; set; }
        public JdiElementTypes TargetType { get; set; }

        public Rule()
        {
            OrConditions = new List<IRuleCondition>();
        }
    }
}
