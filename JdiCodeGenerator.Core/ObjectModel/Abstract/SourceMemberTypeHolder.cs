namespace CodeGenerator.Core.ObjectModel.Abstract
{
    using System.Collections.Generic;
    using System.Linq;

    public class SourceMemberTypeHolder
    {
        List<object> _list;

        public SourceMemberTypeHolder()
        {
            _list = new List<object>();
        }

        public void Set<T>(List<T> list)
        {
            _list = new List<object>();
            _list.AddRange(list.Cast<object>());
        }

        public List<T> Get<T>()
        {
            return _list.Cast<T>().ToList();
        }
    }
}