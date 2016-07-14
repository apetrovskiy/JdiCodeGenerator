namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    using System;
    using System.Collections.Generic;
    using Enums;

    public interface IElementMemberCodeEntry<T> : ICodeEntry // IPieceOfCode
    {
        // List<LocatorDefinition> Locators { get; set; }
        // string MemberName { get; set; }
        string MemberLogicalName { get; set; }
        List<T> SourceMemberType { get; set; } // ??
        JdiElementTypes JdiMemberType { get; set; } // ??
        // string MemberType { get; set; }
        ElementMemberCodeEntryTypes MemberType { get; set; }
        // string GenerateCodeForEntry(SupportedLanguages language);
        // string EnumerationTypeName { get; set; }

        // string AnalyzerThatWon { get; set; }
        // string RuleThatWon { get; set; }

        // temporarily!
        // string Type { get; set; }
        // bool ProcessChildren { get; }

        /*
@JDropdown(root = @FindBy(css = "dropdown"), value = @FindBy(id = "dropdownMenu1"), list = @FindBy(tagName = "li"))
IDropDown<JobCategories> category;
        */
        //LocatorDefinition Root { get; set; }
        //LocatorDefinition Value { get; set; }
        //LocatorDefinition List { get; set; }
        //List<string> ListMemberNames { get; set; }
        //PageMemberCodeEntryTypes Type { get; }

        // Guid ParentId { get; set; }
        Guid UsesId { get; set; }
    }
}