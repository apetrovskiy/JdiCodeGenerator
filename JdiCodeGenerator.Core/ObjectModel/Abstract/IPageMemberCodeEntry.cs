namespace CodeGenerator.Core.ObjectModel.Abstract
{
	using System.Collections.Generic;
	using Enums;
	using JdiConverters.ObjectModel.Enums;

	public interface IPageMemberCodeEntry // : ICodeEntry
        : IPieceOfPackage
    {
        List<LocatorDefinition> Locators { get; set; }
        string MemberName { get; set; }
        JdiElementTypes JdiMemberType { get; set; }
        string MemberType { get; set; }
        string EnumerationTypeName { get; set; }

        string AnalyzerThatWon { get; set; }
        string RuleThatWon { get; set; }

        bool ProcessChildren { get; }

        /*
@JDropdown(root = @FindBy(css = "dropdown"), value = @FindBy(id = "dropdownMenu1"), list = @FindBy(tagName = "li"))
IDropDown<JobCategories> category;
        */
        LocatorDefinition Root { get; set; }
        LocatorDefinition Value { get; set; }
        LocatorDefinition List { get; set; }
        List<string> ListMemberNames { get; set; }
        PageMemberCodeEntryTypes Type { get; }

        SourceMemberTypeHolder SourceMemberType { get; set; }
    }
}