using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;
        private List<Shirt> _matchingShirts;
        private readonly List<SizeCount> _sizeCounts;
        private readonly List<ColorCount> _colorCounts;
        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;

            _matchingShirts = new List<Shirt>();

            _sizeCounts = Size.All.Select(size => new SizeCount
            {
                Size = size,
                Count = 0
            }).ToList();

            _colorCounts = Color.All.Select(color => new ColorCount
            {
                Color = color,
                Count = 0
            }).ToList();
        }


        public SearchResults Search(SearchOptions options)
        {
            var optionSizeIds = options.Sizes.Select(size => size.Id).ToList();
            var optionsColorIds = options.Colors.Select(color => color.Id).ToList();

            if (optionSizeIds.Any() && optionsColorIds.Any())
            {
                _matchingShirts = _shirts.Where(shirt => optionSizeIds.Contains(shirt.Size.Id) && optionsColorIds.Contains(shirt.Color.Id)).ToList();
            }
            else if (!optionSizeIds.Any() && optionsColorIds.Any())
            {
                _matchingShirts = _shirts.Where(shirt => optionsColorIds.Contains(shirt.Color.Id)).ToList();
            }
            else if (optionSizeIds.Any() && !optionsColorIds.Any())
            {
                _matchingShirts = _shirts.Where(shirt => optionSizeIds.Contains(shirt.Size.Id)).ToList();
            }
            else
            {
                _matchingShirts = _shirts;
            }

            foreach (var size in options.Sizes)
            {
                var sizeCountToChange = _sizeCounts.Single(sizeCount => sizeCount.Size.Equals(size));

                sizeCountToChange.Count = _matchingShirts.Count(s => s.Size.Id == size.Id && (!options.Colors.Any() || optionsColorIds.Contains(s.Color.Id)));
            }

            foreach (var color in options.Colors)
            {
                var colorCountToChange = _colorCounts.Single(colorCount => colorCount.Color.Equals(color));

                colorCountToChange.Count = _matchingShirts.Count(c => c.Color.Id == color.Id && (!options.Sizes.Any() || optionSizeIds.Contains(c.Size.Id)));
            }

            return new SearchResults
            {
                Shirts = _matchingShirts,
                SizeCounts = _sizeCounts,
                ColorCounts = _colorCounts
            };
        }
    }
}