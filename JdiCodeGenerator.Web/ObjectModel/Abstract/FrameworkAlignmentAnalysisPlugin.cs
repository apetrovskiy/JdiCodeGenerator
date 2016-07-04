namespace JdiCodeGenerator.Web.ObjectModel.Abstract
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.ObjectModel.Abstract;
    using Helpers;
    using HtmlAgilityPack;

    public abstract class FrameworkAlignmentAnalysisPlugin : IFrameworkAlingmentAnalysisPlugin<HtmlElementTypes>
    {
        public FrameworkAlignmentAnalysisPlugin()
        {
            Rules = new List<IRule<HtmlElementTypes>>();
        }

        public JdiElementTypes Analyze(HtmlNode node)
        {
            if (null == node)
                return JdiElementTypes.StopProcessing;

            // 20160704
            // return GetJdiTypeOfElementByUsingRules(node);
            var jdiType = GetJdiTypeOfElementByUsingRules(node);
            if (JdiElementTypes.Element != jdiType)
                node = null;

            return jdiType;
        }

        public IEnumerable<IRule<HtmlElementTypes>> Rules { get; set; }
        public IEnumerable<string> ExcludeList { get; set; }
        public IRule<HtmlElementTypes> RuleThatWon { get; set; }
        public int Priority { get; set; }

        JdiElementTypes GetJdiTypeOfElementByUsingRules(HtmlNode node)
        {
            var firstRule = Rules.FirstOrDefault(rule => rule.IsMatch(node));

            // experimental
            RuleThatWon = firstRule;
            ExtensionMethodsForNodes.AnalyzerThatWon = this;

            // return firstRule?.TargetType ?? JdiElementTypes.Element;
            return null == firstRule ? JdiElementTypes.Element : firstRule.TargetType;
        }
    }
}