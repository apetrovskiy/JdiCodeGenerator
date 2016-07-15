namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    using System;
    using System.Collections.Generic;
    using Enums;

    public interface ICodeUnit<T> : IPieceOfCode<T>
    {
        string MemberName { get; set; }
        string MemberType { get; set; }
        string Name { get; set; }
        string RelativePathInProject { get; set; }

        // temporarily!
        // string Type { get; set; }

        CodeUnitTypes Type { get; set; }
        Guid DependsOnId { get; set; }
        List<string> Dependencies { get; set; }
    }
}