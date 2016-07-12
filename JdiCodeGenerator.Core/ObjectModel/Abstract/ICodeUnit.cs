namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    public interface ICodeUnit<T> : IPieceOfCode
    {
        string MemberName { get; set; }
        string MemberType { get; set; }

        // temporarily!
        string Type { get; set; }
    }
}