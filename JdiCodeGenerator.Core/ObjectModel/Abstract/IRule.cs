namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    using System.Collections.Generic;

    public interface IRule<T>
    {
        List<SourceElementTypeCollection<T>> SourceTypes { get; set; }
        JdiElementTypes TargetType { get; set; }
        IEnumerable<IRuleCondition> OrConditions { get; set; }
        IEnumerable<IRuleCondition> AndConditions { get; set; }
        string Description { get; set; }
        Dictionary<string, IRule<T>> InternalElements { get; set; }
    }
}
