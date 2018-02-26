namespace JdiCodeGenerator.Core.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using Enums;
    using Helpers;

    public class CodeFile : ICodeFile
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }

        CodeFile(CodeFileTypes fileType)
        {
            Id = Guid.NewGuid();
            Dependencies = new List<string>();
            Type = fileType;
            CodeClass = PiecesOfCodeClasses.CodeFile;
            switch (fileType)
            {
                case CodeFileTypes.ClassForSite:
                    // 20160715
                    // TODO: site default dependencies
                    break;
                case CodeFileTypes.ClassForPage:
                    // 20160715
                    // TODO: page default dependencies
                    break;
                case CodeFileTypes.EnumForMembers:
                    // 20160715
                    // TODO: enum default dependencies
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null);
            }
        }

        public static CodeFile NewSite(string siteName)
        {
            // 20160715
            // TODO: generate name from the project name
            // 20160715
            // TODO: use resources or alike for "Site"
            var projectName = siteName.ToPascalCase() + "Site";
            return new CodeFile(CodeFileTypes.ClassForSite) { Name = projectName };
        }

        public static CodeFile NewPage(string pageName)
        {
            // 20160715
            // TODO: use resources or alike for "Page"
            return new CodeFile(CodeFileTypes.ClassForPage) { Name = pageName.ToPascalCase() + "Page" };
        }

        public static CodeFile NewEnum(string enumName)
        {
            return new CodeFile(CodeFileTypes.EnumForMembers) { Name = enumName.ToPascalCase() };
        }

        public string GenerateCode(SupportedLanguages language)
        {
            // TODO: write code
            return string.Empty;
        }

        public PiecesOfCodeClasses CodeClass { get; set; }

        public Guid DependsOnId { get; set; }
        public List<string> Dependencies { get; set; }
        public string MemberName { get; set; }
        public string MemberType { get; set; }
        public string Name { get; set; }
        public string RelativePathInProject { get; set; }
        // public string Type { get; set; }

        //CodeUnitTypes ICodeUnit<T>.Type
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        public CodeFileTypes Type { get; set; }
    }
}