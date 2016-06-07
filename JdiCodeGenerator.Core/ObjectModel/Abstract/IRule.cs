namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    using System.Collections.Generic;
    using HtmlAgilityPack;

    public interface IRule
    {
        HtmlElementTypes SourceType { get; set; }
        JdiElementTypes TargetType { get; set; }
        IEnumerable<IRuleCondition> OrConditions { get; set; }
        string Description { get; set; }
        bool IsMatch(HtmlNode node);
    }
}
