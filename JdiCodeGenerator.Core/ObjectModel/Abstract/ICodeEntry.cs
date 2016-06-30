namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    using System;
    using System.Collections.Generic;

    public interface ICodeEntry<T>
    {
        Guid Id { get; set; }
        List<LocatorDefinition> Locators { get; set; }
        string MemberName { get; set; }
        // refactoring
        // 20160630
        // HtmlElementTypes HtmlMemberType { get; set; }
        JdiElementTypes JdiMemberType { get; set; }
        string MemberType { get; set; }
        string GenerateCodeForEntry(SupportedLanguages language);
        string EnumerationTypeName { get; set; }

        // experimental
        //IFrameworkAlingmentAnalysisPlugin AnalyzerThatWon { get; set; }
        //IRule RuleThatWon { get; set; }
        string AnalyzerThatWon { get; set; }
        string RuleThatWon { get; set; }

        // temporarily!
        string Type { get; set; }
        // bool ProcessChildren { get; set; }
        bool ProcessChildren { get; }
    }
}