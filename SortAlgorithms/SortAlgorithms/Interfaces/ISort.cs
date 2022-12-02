using System.Collections.Generic;

namespace SortAlgorithms.Interfaces
{
    public interface ISort
    {
        public IEnumerable<T> Sort<T>(T source);
    }
}
