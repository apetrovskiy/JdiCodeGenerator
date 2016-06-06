namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    using System.Collections.Generic;

    public interface IRule
    {
        HtmlElementTypes SourceType { get; set; }
        JdiElementTypes TargetType { get; set; }
        IEnumerable<IRuleCondition> OrConditions { get; set; }
        string Description { get; set; }
    }
}
