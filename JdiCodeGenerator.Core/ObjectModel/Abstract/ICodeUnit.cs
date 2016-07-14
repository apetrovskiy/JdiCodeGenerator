namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    using System;
    using Enums;

    public interface ICodeUnit<T> : IPieceOfCode
    {
        string MemberName { get; set; }
        string MemberType { get; set; }

        // temporarily!
        // string Type { get; set; }

        CodeUnitTypes Type { get; set; }
        Guid DependsOnId { get; set; }
    }
}