

using System.Collections.Generic;

namespace Infrastructure.Shared.Extensions.Filters
{
     public class Filter
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
        public string Logic { get; set; }
        public IList<Filter> Filters { get; set; }
    }
      public class FilterDTO
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public IEnumerable<Sort> Sort { get; set; }
        public Filter Filter { get; set; }
    }
        public class Sort
    {
        public string Field { get; set; }
        public string Dir { get; set; }
    }
}