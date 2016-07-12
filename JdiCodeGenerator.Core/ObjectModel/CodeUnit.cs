namespace JdiCodeGenerator.Core.ObjectModel
{
    using System;
    using Abstract;
    public class CodeUnit<T> : ICodeUnit<T>
    {
        public Guid Id { get; set; }
        public string GenerateCodeForEntry(SupportedLanguages language)
        {
            // throw new NotImplementedException();
            return string.Empty;
        }

        public Guid DependsOn { get; set; }
        public string MemberName { get; set; }
        public string MemberType { get; set; }
        public string Type { get; set; }
    }
}