namespace JdiCodeGenerator.Core.ObjectModel
{
    using System;
    using Abstract;

    public class CodeUnit<T> : ICodeUnit<T>
    {
        public Guid Id { get; set; }
        public string GenerateCodeForEntry(SupportedLanguages language)
        {
            // TODO: write code
            return string.Empty;
        }

        public Guid DependsOn { get; set; }
        public string MemberName { get; set; }
        public string MemberType { get; set; }
        // public string Type { get; set; }

        //CodeUnitTypes ICodeUnit<T>.Type
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        public CodeUnitTypes Type { get; set; }
    }
}