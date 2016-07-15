namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    public interface IPredefinedCodeUnit<T> : IPieceOfCode<T>
    {
        string Value { get; set; }
    }
}