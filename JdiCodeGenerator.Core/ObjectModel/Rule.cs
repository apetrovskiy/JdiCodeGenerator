namespace JdiCodeGenerator.Core.ObjectModel
{
    using System.Collections.Generic;
    using Abstract;

    // refactoring
    // 20160630
    public class Rule<T> : IRule<T>
    {
        public IEnumerable<IRuleCondition> OrConditions { get; set; }
        public IEnumerable<IRuleCondition> AndConditions { get; set; }
        public string Description { get; set; }
        // refactoring
        // 20160630
        // public Dictionary<string, IRule> InternalElements { get; set; }
        public Dictionary<string, IRule<T>> InternalElements { get; set; }
        // refactoring
        // 20160630
        // public List<HtmlElementTypes> SourceTypes { get; set; }
        public List<ISourceElementTypeCollection<T>> SourceTypes { get; set; }
        public JdiElementTypes TargetType { get; set; }

        public Rule()
        {
            // refactoring
            // 20160630
            // SourceTypes = new List<HtmlElementTypes>();
            SourceTypes = new List<ISourceElementTypeCollection<T>>();
            OrConditions = new List<IRuleCondition>();
            AndConditions = new List<IRuleCondition>();
            // refactoring
            // 20160630
            // InternalElements = new Dictionary<string, IRule>();
            InternalElements = new Dictionary<string, IRule<T>>();
            TargetType = JdiElementTypes.Element;
        }
    }
}
