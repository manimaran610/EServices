
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Common;
using FluentValidation;

namespace Infrastructure.Shared.Extensions
{

    public static class DynamicSortExtension
    {
        private const BindingFlags DefaultBindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
        public static  IEnumerable<T> GetDynamicSortedList<T>(this IEnumerable<T> collection, string sort) where T :AuditableBaseEntity
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
        private static IEnumerable<T> SortDynamically<T>(IEnumerable<T> collection, IList<SortOptions> sorts) where T :AuditableBaseEntity
        {
            if (sorts.Count == 0)
            {
                return collection;
            }
            var firstSort = sorts.OrderBy(x => x.SortOrder).First();
            var sortedPersonsAlt = firstSort.Direction.ToLower()
            switch
            {
                "asc" => collection.OrderBy(x =>x!.GetPropertyValue(firstSort.PropertyName), StringComparer.Ordinal),
                "desc" => collection.OrderByDescending(x => x!.GetPropertyValue(firstSort.PropertyName), StringComparer.Ordinal),
                _ => throw new ValidationException("Sort Direction must be asc or desc")
            };
            foreach (var sort in sorts.OrderBy(x => x.SortOrder).Skip(1))
            {
                sortedPersonsAlt = sort.Direction.ToLower()
                switch
                {
                    "asc" => sortedPersonsAlt.ThenBy(x => x!.GetPropertyValue(sort.PropertyName), StringComparer.Ordinal),
                    "desc" => sortedPersonsAlt.ThenByDescending(x =>  x!.GetPropertyValue(sort.PropertyName), StringComparer.Ordinal),
                    _ => throw new ValidationException("sort Direction must be asc or desc")
                };
            }
            return sortedPersonsAlt;
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

          private static string GetPropertyValue(this object inputObject, string propertyName)
        {
            return inputObject.GetType().GetProperty(propertyName, DefaultBindingFlags)?.GetValue(inputObject, null)?.ToString()!;
        }
    }


}