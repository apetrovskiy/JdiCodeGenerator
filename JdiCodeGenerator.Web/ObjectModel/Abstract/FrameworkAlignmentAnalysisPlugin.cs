namespace JdiCodeGenerator.Web.ObjectModel.Abstract
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.ObjectModel.Abstract;
    using Core.ObjectModel.Enums;
    using Helpers;
    using HtmlAgilityPack;

    public abstract class FrameworkAlignmentAnalysisPlugin : IFrameworkAlingmentAnalysisPlugin<HtmlElementTypes>
    {
        protected FrameworkAlignmentAnalysisPlugin()
        {
            Rules = new List<IRule<HtmlElementTypes>>();
        }

        public JdiElementTypes Analyze(HtmlNode node)
        {
            var firstRule = Rules.FirstOrDefault(rule => rule.IsMatch(node));

            // experimental
            RuleThatWon = firstRule;
            HtmlNodesExtensions.AnalyzerThatWon = this;

            // return firstRule?.TargetType ?? JdiElementTypes.Element;
            return null == firstRule ? JdiElementTypes.Element : firstRule.TargetType;
        }

        public IEnumerable<IRule<HtmlElementTypes>> Rules { get; set; }
        public IEnumerable<string> ExcludeList { get; set; }
        public IRule<HtmlElementTypes> RuleThatWon { get; set; }
        public int Priority { get; set; }
    }
}