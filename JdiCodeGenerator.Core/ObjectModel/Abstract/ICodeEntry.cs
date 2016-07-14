namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    using System;

    public interface ICodeEntry : IPieceOfCode
    {
        Guid ParentId { get; set; }
    }
}