using SearchAlgorithms.Interfaces;
using SearchAlgorithms.Models;

namespace SearchAlgorithms.Implementations
{
    public class InterpolationSearch : ISearch
    {
        public SearchResult Search(int[] source, int key)
        {
            int left = 0;
            int right = source.Length - 1;
            int iterationCount = 0;
            int comparisonCount = 0;

            while (left <= right && source[left] <= key && key <= source[right])
            {
                iterationCount++;

                comparisonCount++;
                if (left == right || source[right] == source[left])
                {
                    return new SearchResult { Index = left, IterationCount = iterationCount, ComparisonCount = comparisonCount };
                }

                int mid = (int)(left + ((double)(key - source[left]) * (right - left) / (source[right] - source[left])));

                comparisonCount++;
                if (source[mid] == key)
                {
                    return new SearchResult { Index = mid, IterationCount = iterationCount, ComparisonCount = comparisonCount };
                }

                comparisonCount++;
                if (source[mid] < key)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }
            return new SearchResult { Index = left, IterationCount = iterationCount, ComparisonCount = comparisonCount };
        }

    }
}

