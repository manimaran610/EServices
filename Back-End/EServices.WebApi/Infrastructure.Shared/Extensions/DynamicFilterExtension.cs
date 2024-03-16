using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Domain.Common;

namespace Infrastructure.Shared.Extensions
{
    public static class DynamicFilterExtension
    {
        private const BindingFlags DefaultBindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

        public static List<T> GetDynamicFilteredList<T>(this IEnumerable<T> collection, string propertyName, string filterString) where T:AuditableBaseEntity
        {
            var filteredResult = new List<T>();
            var filterName = string.Empty;
            var filterNameValue = string.Empty;
            if (filterString.Contains(':'))
            {
                filterName = filterString.Split(':')[0];
                filterNameValue = filterString.Split(':')[1];
            }
            else
            {
                filterName = "default";
                filterNameValue = filterString;
            }
            switch (filterName.ToLower())
            {
                case "eq":
                    filteredResult.AddRange(collection.Where(x => GetPropertyValue(propertyName, x!) == filterNameValue));
                    break;
                case "ieq":
                    filteredResult.AddRange(collection.Where(x => string.Equals(GetPropertyValue(propertyName, x!), filterNameValue, StringComparison.OrdinalIgnoreCase)));
                    break;
                case "in":
                    {
                        var filterNameArray = filterNameValue.Split(',');
                        var filteredUser = from item in collection
                                           where filterNameArray.Contains(GetPropertyValue(propertyName, item))
                                           select item;
                        filteredResult.AddRange(filteredUser);
                    }
                    break;
                case "iin":
                    {
                        var filterNameArray = filterNameValue.Split(',');
                        filterNameArray = filterNameArray.Select(s => s.ToLowerInvariant()).ToArray();
                        var filteredUser = from item in collection
                                           where filterNameArray.Contains(GetPropertyValue(propertyName, item!).ToLower())
                                           select item;

                        filteredResult.AddRange(filteredUser);
                    }
                    break;
                case "con":
                    filteredResult.AddRange(collection.Where(x => GetPropertyValue(propertyName, x!).Contains(filterNameValue)));
                    break;
                case "sw":
                    filteredResult.AddRange(collection.Where(x => GetPropertyValue(propertyName, x!).StartsWith(filterNameValue)));
                    break;
                case "ew":
                    filteredResult.AddRange(collection.Where(x => GetPropertyValue(propertyName, x!).EndsWith(filterNameValue)));
                    break;
                case "neq":
                    filteredResult.AddRange(collection.Where(x => GetPropertyValue(propertyName, x!) != filterNameValue));
                    break;
                case "ineq":
                    filteredResult.AddRange(collection.Where(x => !string.Equals(GetPropertyValue(propertyName, x!), filterNameValue, StringComparison.OrdinalIgnoreCase)));
                    break;
                case "nin":
                    {
                        var filterNameArray = filterNameValue.Split(',');

                        var query = collection
                            .Select(e => e)
                            .Where(e => !filterNameArray.Contains(GetPropertyValue(propertyName, e!))).ToList();
                        filteredResult.AddRange(query);
                    }
                    break;
                case "inin":
                    {
                        var filterNameArray = filterNameValue.Split(',');
                        filterNameArray = filterNameArray.Select(s => s.ToLowerInvariant()).ToArray();
                        var query = collection
                            .Select(e => e)
                            .Where(e => !filterNameArray.Contains(GetPropertyValue(propertyName, e!).ToLower())).ToList();
                        filteredResult.AddRange(query);
                    }
                    break;
                case "ncon":
                    filteredResult.AddRange(collection.Where(x => !GetPropertyValue(propertyName, x!).Contains(filterNameValue)));
                    break;
                case "nsw":
                    filteredResult.AddRange(collection.Where(x => !GetPropertyValue(propertyName, x!).StartsWith(filterNameValue)));
                    break;
                case "new":
                    filteredResult.AddRange(collection.Where(x => !GetPropertyValue(propertyName, x!).EndsWith(filterNameValue)));
                    break;
                case "icon":
                    filteredResult.AddRange(collection.Where(x => GetPropertyValue(propertyName, x!).ToLower().Contains(filterNameValue, StringComparison.OrdinalIgnoreCase)));
                    break;
                case "isw":
                    filteredResult.AddRange(collection.Where(x => GetPropertyValue(propertyName, x!).ToLower().StartsWith(filterNameValue.ToLower(), StringComparison.OrdinalIgnoreCase)));
                    break;
                case "iew":
                    filteredResult.AddRange(collection.Where(x => GetPropertyValue(propertyName, x!).ToLower().EndsWith(filterNameValue.ToLower(), StringComparison.OrdinalIgnoreCase)));
                    break;
                case "incon":
                    filteredResult.AddRange(collection.Where(x => !GetPropertyValue(propertyName, x!).ToLower().Contains(filterNameValue.ToLower(), StringComparison.OrdinalIgnoreCase)));
                    break;
                case "insw":
                    filteredResult.AddRange(collection.Where(x => !GetPropertyValue(propertyName, x!).ToLower().StartsWith(filterNameValue.ToLower(), StringComparison.OrdinalIgnoreCase)));
                    break;
                case "inew":
                    filteredResult.AddRange(collection.Where(x => !GetPropertyValue(propertyName, x!).ToLower().EndsWith(filterNameValue.ToLower(), StringComparison.OrdinalIgnoreCase)));
                    break;
                case "default":
                    filteredResult.AddRange(collection.Where(x => GetPropertyValue(propertyName, x!).ToLower() == (filterNameValue.ToLower())));
                    break;
            }

            return filteredResult;
        }

        public static List<T> GetDynamicChildFilteredList<T>(IEnumerable<T> collection, string propertyName, string filterString)
        {
            var filteredResult = new List<T>();
            var filterName = string.Empty;
            var filterNameValue = string.Empty;
            if (filterString.Contains(':'))
            {
                filterName = filterString.Split(':')[0];
                filterNameValue = filterString.Split(':')[1];
            }
            else
            {
                filterName = "default";
                filterNameValue = filterString;
            }
            switch (filterName.ToLower())
            {
                case "eq":
                    filteredResult.AddRange(collection.Where(x => GetNestedObjectList(x!).Any(e => GetPropertyValue(propertyName, e!) == filterNameValue)));
                    break;
                case "ieq":
                    filteredResult.AddRange(collection.Where(x => GetNestedObjectList(x!).Any(e => string.Equals(GetPropertyValue(propertyName, e!), filterNameValue, StringComparison.OrdinalIgnoreCase))));
                    break;
                case "in":
                    {
                        var filterNameArray = filterNameValue.Split(',');
                        var filteredUser = from item in collection
                                           where GetNestedObjectList(item).Any(e => filterNameArray.Contains(GetPropertyValue(propertyName, e!)))
                                           select item;
                        filteredResult.AddRange(filteredUser);
                    }
                    break;
                case "iin":
                    {
                        var filterNameArray = filterNameValue.Split(',');
                        filterNameArray = filterNameArray.Select(s => s.ToLowerInvariant()).ToArray();
                        var filteredUser = from item in collection
                                           where GetNestedObjectList(item).Any(e => filterNameArray.Contains(GetPropertyValue(propertyName, e!).ToLower()))
                                           select item;

                        filteredResult.AddRange(filteredUser);
                    }
                    break;
                case "con":
                    filteredResult.AddRange(collection.Where(x => GetNestedObjectList(x!).Any(e => GetPropertyValue(propertyName, e!).Contains(filterNameValue))));
                    break;
                case "sw":
                    filteredResult.AddRange(collection.Where(x => GetNestedObjectList(x!).Any(e => GetPropertyValue(propertyName, e!).StartsWith(filterNameValue))));
                    break;
                case "ew":
                    filteredResult.AddRange(collection.Where(x => GetNestedObjectList(x!).Any(e => GetPropertyValue(propertyName, e!).EndsWith(filterNameValue))));
                    break;
                case "neq":
                    filteredResult.AddRange(collection.Where(x => GetNestedObjectList(x!).Any(e => GetPropertyValue(propertyName, e!) != filterNameValue)));
                    break;
                case "ineq":
                    filteredResult.AddRange(collection.Where(x => GetNestedObjectList(x!).Any(e => !string.Equals(GetPropertyValue(propertyName, e!), filterNameValue, StringComparison.OrdinalIgnoreCase))));
                    break;
                case "nin":
                    {
                        var filterNameArray = filterNameValue.Split(',');

                        var query = collection
                            .Select(e => e)
                            .Where(x => GetNestedObjectList(x!).Any(e => !filterNameArray.Contains(GetPropertyValue(propertyName, e!)))).ToList();
                        filteredResult.AddRange(query);
                    }
                    break;
                case "inin":
                    {
                        var filterNameArray = filterNameValue.Split(',');
                        filterNameArray = filterNameArray.Select(s => s.ToLowerInvariant()).ToArray();
                        var query = collection
                            .Select(e => e)
                            .Where(x => GetNestedObjectList(x!).Any(e => !filterNameArray.Contains(GetPropertyValue(propertyName, e!).ToLower()))).ToList();
                        filteredResult.AddRange(query);
                    }
                    break;
                case "ncon":
                    filteredResult.AddRange(collection.Where(x => GetNestedObjectList(x!).Any(e => !GetPropertyValue(propertyName, e!).Contains(filterNameValue))));
                    break;
                case "nsw":
                    filteredResult.AddRange(collection.Where(x => GetNestedObjectList(x!).Any(e => !GetPropertyValue(propertyName, e!).StartsWith(filterNameValue))));
                    break;
                case "new":
                    filteredResult.AddRange(collection.Where(x => GetNestedObjectList(x!).Any(e => !GetPropertyValue(propertyName, e!).EndsWith(filterNameValue))));
                    break;
                case "icon":
                    filteredResult.AddRange(collection.Where(x => GetNestedObjectList(x!).Any(e => GetPropertyValue(propertyName, e!).ToLower().Contains(filterNameValue, StringComparison.OrdinalIgnoreCase))));
                    break;
                case "isw":
                    filteredResult.AddRange(collection.Where(x => GetNestedObjectList(x!).Any(e => GetPropertyValue(propertyName, e!).ToLower().StartsWith(filterNameValue.ToLower(), StringComparison.OrdinalIgnoreCase))));
                    break;
                case "iew":
                    filteredResult.AddRange(collection.Where(x => GetNestedObjectList(x!).Any(e => GetPropertyValue(propertyName, e!).ToLower().EndsWith(filterNameValue.ToLower(), StringComparison.OrdinalIgnoreCase))));
                    break;
                case "incon":
                    filteredResult.AddRange(collection.Where(x => GetNestedObjectList(x!).Any(e => !GetPropertyValue(propertyName, e!).ToLower().Contains(filterNameValue.ToLower(), StringComparison.OrdinalIgnoreCase))));
                    break;
                case "insw":
                    filteredResult.AddRange(collection.Where(x => GetNestedObjectList(x!).Any(e => !GetPropertyValue(propertyName, e!).ToLower().StartsWith(filterNameValue.ToLower(), StringComparison.OrdinalIgnoreCase))));
                    break;
                case "inew":
                    filteredResult.AddRange(collection.Where(x => GetNestedObjectList(x!).Any(e => !GetPropertyValue(propertyName, e!).ToLower().EndsWith(filterNameValue.ToLower(), StringComparison.OrdinalIgnoreCase))));
                    break;
                case "default":
                    filteredResult.AddRange(collection.Where(x => GetNestedObjectList(x!).Any(e => GetPropertyValue(propertyName, e!).ToLower() == (filterNameValue.ToLower()))));
                    break;
            }

            return filteredResult;
        }

        private static string GetPropertyValue(this string propertyName, object inputObject)
        {
            return inputObject.GetType().GetProperty(propertyName, DefaultBindingFlags)?.GetValue(inputObject, null)?.ToString()!;
        }
        private static List<object> GetNestedObjectList(object inputObject)
        {
            var properties = inputObject.GetType().GetProperties();
            List<object> result = new();
            foreach (var item in properties)
            {
                var value = item.GetValue(inputObject);
                if (value != null) result.Add(value);
            }
            return result;
        }

    }

}