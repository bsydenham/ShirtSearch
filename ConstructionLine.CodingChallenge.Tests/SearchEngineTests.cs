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
        public void Search_GivenSizeColorOptions_ShouldReturnCorrectResult([ValueSource("GenerateSizeOptions")] IEnumerable<Size> sizeOptions, 
                                                                           [ValueSource("GenerateColorOptions")] IEnumerable<Color> colorOptions,
                                                                           [ValueSource("GetTestShirts")] IEnumerable<Shirt> testShirts)
        {
            var shirts = testShirts.ToList();

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = colorOptions.ToList(),
                Sizes = sizeOptions.ToList()
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        private static IEnumerable<IEnumerable<Shirt>> GetTestShirts()
        {
            var testShirts = new List<List<Shirt>>();

            testShirts.Add(new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            });

            testShirts.Add(new List<Shirt>
            {
            });

            testShirts.Add(new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Red - Medium", Size.Medium, Color.Red),
                new Shirt(Guid.NewGuid(), "Red - Large", Size.Large, Color.Red),
            });


            testShirts.Add(new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Small", Size.Small, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Small", Size.Small, Color.Blue),
            });

            testShirts.Add(new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
            });

            return testShirts;
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
