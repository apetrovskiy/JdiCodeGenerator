namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    using System.Collections.Generic;

    public interface ISourceElementTypeCollection<T>
    {
        List<T> Types { get; set; }
    }
}