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
            _shirts = shirts;

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

            var matchingShirts = GetMatchingShirts(optionSizeIds, optionsColorIds);

            ChangeSizeCounts(optionSizeIds, matchingShirts);
            ChangeColorCounts(optionsColorIds, matchingShirts);

            return new SearchResults
            {
                Shirts = matchingShirts,
                SizeCounts = _sizeCounts,
                ColorCounts = _colorCounts
            };
        }

        private List<Shirt> GetMatchingShirts(List<Guid> optionSizeIds, List<Guid> optionColorIds)
        {
            var anySizeOptions = optionSizeIds.Any();
            var anyColorOptions = optionColorIds.Any();

            if (anySizeOptions && anyColorOptions)
            {
                return _shirts.Where(shirt => optionSizeIds.Contains(shirt.Size.Id) && optionColorIds.Contains(shirt.Color.Id)).ToList();
            }

            if (!anySizeOptions && anyColorOptions)
            {
                return _shirts.Where(shirt => optionColorIds.Contains(shirt.Color.Id)).ToList();
            }

            if (anySizeOptions && !anyColorOptions)
            {
                return _shirts.Where(shirt => optionSizeIds.Contains(shirt.Size.Id)).ToList();
            }

            return _shirts;
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