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
            var firstRule = Rules.FirstOrDefault(rule => rule.IsMatch(node));

            // experimental
            RuleThatWon = firstRule;
            ExtensionMethodsForNodes.AnalyzerThatWon = this;

            // children collection for complex elements
            if (null != firstRule && null != firstRule.InternalElements && firstRule.InternalElements.Any())
                WorkOutInternalElements(node);

            // return firstRule?.TargetType ?? JdiElementTypes.Element;
            return null == firstRule ? JdiElementTypes.Element : firstRule.TargetType;
        }

        public IEnumerable<IRule<HtmlElementTypes>> Rules { get; set; }
        public IEnumerable<string> ExcludeList { get; set; }
        public IRule<HtmlElementTypes> RuleThatWon { get; set; }
        public int Priority { get; set; }

        // TODO: children collection of complex elements
        void WorkOutInternalElements(HtmlNode node)
        {
            // TODO: the root element

            // TODO: the value element

            // TODO: the list collection

        }
    }
}