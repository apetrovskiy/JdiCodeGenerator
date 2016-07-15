namespace JdiCodeGenerator.Core.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using Enums;

    public class ElementMemberCodeEntry<T> : IElementMemberCodeEntry<T>
    {
        public Guid Id { get; set; }

        // public Guid DependsOn { get; set; }
        public string MemberLogicalName { get; set; }
        public List<T> SourceMemberType { get; set; }
        public JdiElementTypes JdiMemberType { get; set; }
        public ElementMemberCodeEntryTypes MemberType { get; set; }
        public Guid ParentId { get; set; }
        // public Guid UsesId { get; set; }

        public ElementMemberCodeEntry()
        {
            Id = Guid.NewGuid();
        }

        public string GenerateCode(SupportedLanguages language)
        {
            // TODO: write code
            return string.Empty;
        }
    }
}