using System;
using System.Collections.Generic;
using Range = Logic.Range;

namespace Helpers
{
    public static class RangeExtensions
    {
        public static Range GetHitRange(this IEnumerable<Range> ranges, float value)
        {
            foreach (Range range in ranges)
            {
                if (range.ValueIsIn(value))
                    return range;
            }

            throw new Exception("Unreachable");
        }
    }
}