using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineTests : SearchEngineTestsBase
    {
        [Test, Combinatorial]
        public void Search_GivenSizeColorOptions_ShouldReturnCorrectResult([ValueSource("GenerateSizeOptions")] IEnumerable<Size> sizeOptions, [ValueSource("GenerateColorOptions")] IEnumerable<Color> colorOptions)
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            Console.WriteLine(string.Join(",", colorOptions.Select(c => c.Name)));
            Console.WriteLine(string.Join(",", sizeOptions.Select(s => s.Name)));

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = colorOptions.ToList(),
                Sizes = sizeOptions.ToList()
            };

            var results = searchEngine.Search(searchOptions);

            Console.WriteLine(string.Join(",", results.Shirts.Select(s => s.Name)));

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        private static IEnumerable<IEnumerable<Size>> GenerateSizeOptions()
        {
            return Permutations(Size.All);
        }
        private static IEnumerable<IEnumerable<Color>> GenerateColorOptions()
        {
            return Permutations(Color.All);
        }

        //https://stackoverflow.com/questions/7802822/all-possible-combinations-of-a-list-of-values
        private static IEnumerable<T[]> Permutations<T>(IEnumerable<T> source)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));

            T[] data = source.ToArray();

            return Enumerable
                   .Range(0, 1 << (data.Length))
                   .Select(index => data
                                    .Where((v, i) => (index & (1 << i)) != 0)
                                    .ToArray());
        }
    }
}
