﻿namespace JdiCodeGenerator.Core.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using Enums;
    using Helpers;

    public class CodeUnit : ICodeUnit
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }

        CodeUnit(CodeUnitTypes unitType)
        {
            Id = Guid.NewGuid();
            Dependencies = new List<string>();
            Type = unitType;
            CodeClass = PiecesOfCodeClasses.CodeUnit;
            switch (unitType)
            {
                case CodeUnitTypes.ClassForSite:
                    // 20160715
                    // TODO: site default dependencies
                    break;
                case CodeUnitTypes.ClassForPage:
                    // 20160715
                    // TODO: page default dependencies
                    break;
                case CodeUnitTypes.EnumForMembers:
                    // 20160715
                    // TODO: enum default dependencies
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unitType), unitType, null);
            }
        }

        public static CodeUnit NewSite(string siteName)
        {
            // 20160715
            // TODO: generate name from the project name
            // 20160715
            // TODO: use resources or alike for "Site"
            var projectName = siteName.ToPascalCase() + "Site";
            return new CodeUnit(CodeUnitTypes.ClassForSite) { Name = projectName };
        }

        public static CodeUnit NewPage(string pageName)
        {
            // 20160715
            // TODO: use resources or alike for "Page"
            return new CodeUnit(CodeUnitTypes.ClassForPage) { Name = pageName.ToPascalCase() + "Page" };
        }

        public static CodeUnit NewEnum(string enumName)
        {
            return new CodeUnit(CodeUnitTypes.EnumForMembers) { Name = enumName.ToPascalCase() };
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

        public CodeUnitTypes Type { get; set; }
    }
}