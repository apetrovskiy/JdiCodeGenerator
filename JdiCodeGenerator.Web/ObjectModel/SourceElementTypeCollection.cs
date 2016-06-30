namespace JdiCodeGenerator.Web.ObjectModel
{
    using System.Collections.Generic;
    using Abstract;
    using Core.ObjectModel.Abstract;

    public class SourceElementTypeCollection<T> : ISourceElementTypeCollection<T>
    {
        public List<T> Types { get; set; }
    }
}