namespace CodeGenerator.Core.ObjectModel.Abstract.Results
{
	using System;
	using System.Collections.Generic;
	using Enums;

	public interface ICodeFile : IPieceOfPackage
    {
        string MemberName { get; set; }
        string MemberType { get; set; }
        string Name { get; set; }
        string RelativePathInProject { get; set; }
        CodeFileTypes Type { get; set; }
        Guid DependsOnId { get; set; }
        List<string> Dependencies { get; set; }
    }
}