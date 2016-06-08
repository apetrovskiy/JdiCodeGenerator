namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    using System.Collections.Generic;
    using HtmlAgilityPack;

    public interface IRule
    {
        List<HtmlElementTypes> SourceTypes { get; set; }
        JdiElementTypes TargetType { get; set; }
        IEnumerable<IRuleCondition> OrConditions { get; set; }
        IEnumerable<IRuleCondition> AndConditions { get; set; }
        string Description { get; set; }
        Dictionary<string, IRule> InternalElements { get; set; }
        bool IsMatch(HtmlNode node);
    }
}
