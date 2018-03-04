namespace CodeGenerator.Web.ObjectModel.Abstract
{
	using System.Collections.Generic;
	using System.Linq;
	using Core.ObjectModel.Abstract.Rules;
	using Helpers;
	using HtmlAgilityPack;
	using JdiConverters.ObjectModel.Enums;

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

			return firstRule?.TargetType ?? JdiElementTypes.Element;
		}

        public IEnumerable<IRule<HtmlElementTypes>> Rules { get; set; }
        public IEnumerable<string> ExcludeList { get; set; }
        public IRule<HtmlElementTypes> RuleThatWon { get; set; }
        public int Priority { get; set; }
    }
}