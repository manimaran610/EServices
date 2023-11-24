
using System.Reflection;
using Domain.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;



namespace Infrastructure.Persistence.Extensions
{
    public static class EFCoreDynamicSortExtension
    {

        public static IQueryable<T> GetSortedList<T>(this IQueryable<T> collection, string sort)
        {
            if (string.IsNullOrEmpty(sort)) return collection;
            var array = sort.Contains(',') ? sort.Split(",") : new string[] { sort };
            List<SortOptions> listOfsortOptions = new(); int count = 1;
            foreach (string item in array)
            {
                if (item.Contains(':'))
                {
                    listOfsortOptions.Add(new SortOptions() { PropertyName = item.Split(":")[0], Direction = item.Split(":")[1], SortOrder = count });
                    count++;
                }
            }
            return collection.SortDynamically(listOfsortOptions);

        }

        private static IQueryable<T> SortDynamically<T>(this IQueryable<T> queryable, IEnumerable<SortOptions> sort)
        {
            if (sort != null && sort.Any())
            {
                var ordering = string.Join(",", sort.OrderBy(e => e.SortOrder).Select(s => $"{s.PropertyName} {s.Direction}"));
                return queryable.OrderBy(ordering);
            }
            return queryable;
        }

        private class SortOptions
        {
            public string PropertyName { get; set; } = default!;
            public int SortOrder { get; set; }
            public string Direction { get; set; } = default!;
        }

    }


}