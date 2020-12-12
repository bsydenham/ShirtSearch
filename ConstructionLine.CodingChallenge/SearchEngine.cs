using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;
        private readonly List<SizeCount> _sizeCounts;
        private readonly List<ColorCount> _colorCounts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts ?? throw new ArgumentNullException(nameof(shirts));

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
            var optionColorIds = options.Colors.Select(color => color.Id).ToList();

            var matchingShirts = _shirts.Where(shirt => (!options.Sizes.Any() || options.Sizes.Any(size => size == shirt.Size)) 
                                                    && (!options.Colors.Any() || options.Colors.Any(color => color == shirt.Color))).ToList();

            ChangeSizeCounts(optionSizeIds, matchingShirts);
            ChangeColorCounts(optionColorIds, matchingShirts);

            return new SearchResults
            {
                Shirts = matchingShirts,
                SizeCounts = _sizeCounts,
                ColorCounts = _colorCounts
            };
        }

        private void ChangeSizeCounts(List<Guid> optionSizeIds, List<Shirt> matchingShirts)
        {
            foreach (var sizeId in optionSizeIds)
            {
                var sizeCountToChange = _sizeCounts.Single(sizeCount => sizeCount.Size.Id.Equals(sizeId));

                sizeCountToChange.Count = matchingShirts.Count(s => s.Size.Id == sizeId);
            }
        }

        private void ChangeColorCounts(List<Guid> optionColorIds, List<Shirt> matchingShirts)
        {
            foreach (var colorId in optionColorIds)
            {
                var colorCountToChange = _colorCounts.Single(colorCount => colorCount.Color.Id.Equals(colorId));

                colorCountToChange.Count = matchingShirts.Count(c => c.Color.Id == colorId);
            }
        }
    }
}