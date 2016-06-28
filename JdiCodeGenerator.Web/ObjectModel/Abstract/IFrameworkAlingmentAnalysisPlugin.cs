namespace JdiCodeGenerator.Web.ObjectModel.Abstract
{
    using System.Collections.Generic;
    using HtmlAgilityPack;
    using Core.ObjectModel.Abstract;

    public interface IFrameworkAlingmentAnalysisPlugin
    {
        JdiElementTypes Analyze(HtmlNode node);
        IEnumerable<IRule> Rules { get; set; }
        IEnumerable<string> ExcludeList { get; set; }
        IRule RuleThatWon { get; set; }
    }
}