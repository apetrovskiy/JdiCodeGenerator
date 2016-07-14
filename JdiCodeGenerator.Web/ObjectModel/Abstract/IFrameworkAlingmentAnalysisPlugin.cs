namespace JdiCodeGenerator.Web.ObjectModel.Abstract
{
    using System.Collections.Generic;
    using HtmlAgilityPack;
    using Core.ObjectModel.Abstract;
    using Core.ObjectModel.Enums;

    public interface IFrameworkAlingmentAnalysisPlugin<T>
    {
        JdiElementTypes Analyze(HtmlNode node);
        IEnumerable<IRule<T>> Rules { get; set; }
        IEnumerable<string> ExcludeList { get; set; }
        IRule<T> RuleThatWon { get; set; }
        int Priority { get; set; }
    }
}