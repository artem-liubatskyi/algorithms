using SearchAlgorithms.Models;

namespace SearchAlgorithms.Interfaces
{
    public interface ISearch
    {
        public SearchResult Search(int[] source, int key);
    }
}
