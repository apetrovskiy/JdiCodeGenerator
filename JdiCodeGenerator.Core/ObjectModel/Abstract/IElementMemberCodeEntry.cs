namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    using System;
    using System.Collections.Generic;
    using Enums;

    public interface IElementMemberCodeEntry<T> : ICodeEntry<T> // IPieceOfCode
    {
        // List<LocatorDefinition> Locators { get; set; }
        // string MemberName { get; set; }
        string MemberLogicalName { get; set; }
        List<T> SourceMemberType { get; set; } // ??
        JdiElementTypes JdiMemberType { get; set; } // ??
        // string MemberType { get; set; }
        ElementMemberCodeEntryTypes MemberType { get; set; }
        // Guid UsesId { get; set; }
    }
}