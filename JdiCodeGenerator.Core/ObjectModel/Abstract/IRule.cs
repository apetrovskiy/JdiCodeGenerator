namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    using System.Collections.Generic;

    // refactoring
    // 20160630
    public interface IRule<T>
    {
        // refactoring
        // 20160630
        // List<HtmlElementTypes> SourceTypes { get; set; }
        List<ISourceElementTypeCollection<T>> SourceTypes { get; set; }
        JdiElementTypes TargetType { get; set; }
        IEnumerable<IRuleCondition> OrConditions { get; set; }
        IEnumerable<IRuleCondition> AndConditions { get; set; }
        string Description { get; set; }
        // refactoring
        // 20160630
        // Dictionary<string, IRule> InternalElements { get; set; }
        Dictionary<string, IRule<T>> InternalElements { get; set; }
    }
}
