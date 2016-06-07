namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    using System.Collections.Generic;
    using HtmlAgilityPack;

    public interface IRule
    {
        // HtmlElementTypes SourceTypes { get; set; }
        List<HtmlElementTypes> SourceTypes { get; set; }
        JdiElementTypes TargetType { get; set; }
        IEnumerable<IRuleCondition> OrConditions { get; set; }
        IEnumerable<IRuleCondition> AndConditions { get; set; }
        string Description { get; set; }
        bool IsMatch(HtmlNode node);
    }
}
