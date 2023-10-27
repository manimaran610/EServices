
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Domain.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Extensions
{

    public static class DynamicSortExtension
    {
        private const BindingFlags DefaultBindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
        public static  DbSet<T> GetSortedList<T>(this DbSet<T> collection, string sort) where T :AuditableBaseEntity
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
            var result = SortDynamically<T>(collection, listOfsortOptions);
            return result; 
        }
        private static DbSet<T> SortDynamically<T>(DbSet<T> collection, IList<SortOptions> sorts) where T :AuditableBaseEntity
        {
            if (sorts.Count == 0)
            {
                return collection;
            }
            var firstSort = sorts.OrderBy(x => x.SortOrder).First();
            var sortedPersonsAlt = firstSort.Direction.ToLower()
            switch
            {
                "asc" => collection.OrderBy(x =>x!.GetType().GetProperty(firstSort.PropertyName, DefaultBindingFlags).GetValue(x, null).ToString(), StringComparer.Ordinal),
                "desc" => collection.OrderByDescending(x => x!.GetType().GetProperty(firstSort.PropertyName, DefaultBindingFlags).GetValue(x, null).ToString(), StringComparer.Ordinal),
                _ => throw new ValidationException("Sort Direction must be asc or desc")
            };
            foreach (var sort in sorts.OrderBy(x => x.SortOrder).Skip(1))
            {
                sortedPersonsAlt = sort.Direction.ToLower()
                switch
                {
                    "asc" => sortedPersonsAlt.ThenBy(x => x!.GetType().GetProperty(sort.PropertyName, DefaultBindingFlags).GetValue(x, null).ToString(), StringComparer.Ordinal),
                    "desc" => sortedPersonsAlt.ThenByDescending(x => x!.GetType().GetProperty(sort.PropertyName, DefaultBindingFlags).GetValue(x, null).ToString(), StringComparer.Ordinal),
                    _ => throw new ValidationException("sort Direction must be asc or desc")
                };
            }
            return collection;
        }
        private class SortOptions
        {
            public string PropertyName { get; set; } = default!;
            public int SortOrder { get; set; }
            public string Direction { get; set; } = default!;
        }

        public static string GetFieldName<T>(this T input, string propertyName)
        {
            string result = string.Empty;
            if (input != null)
            {
                var prop = input!.GetType().GetProperty(propertyName, DefaultBindingFlags);
                if (prop != null)
                {
                    result = prop.GetValue(input, null)?.ToString();
                }
            }
            return result;
        }
    }


}