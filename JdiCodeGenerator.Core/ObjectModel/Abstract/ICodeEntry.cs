namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    public interface ICodeEntry : IPieceOfCode
    {
        // Guid ParentId { get; set; }
        SourceMemberTypeHolder SourceMemberType { get; set; }
    }
}