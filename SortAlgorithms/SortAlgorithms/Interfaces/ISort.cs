using System;
using System.Collections.Generic;

namespace SortAlgorithms.Interfaces
{
    public interface ISort
    {
        public IEnumerable<int> Sort(IEnumerable<int> source);
    }
}
