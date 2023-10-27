
using System.Linq;
using System.Reflection;
using Domain.Common;
using Infrastructure.Shared.Extensions.Filters;


namespace Infrastructure.Shared.Extensions
{
    public static class EFCoreDynamicFilterExtension
    {
        public static System.Linq.IQueryable<T> GetFilteredList<T>(this IQueryable<T> collection, string filterString) where T : AuditableBaseEntity
        {
            Filter filteredResult = new Filter();
            if (!string.IsNullOrEmpty(filterString))
            {
                if (filterString.Contains(","))
                {
                    filteredResult = GetFilterFromFilterString(filterString.Split(",")[0]);
                    filterString.Split(",")
                    .Skip(1).ToList()
                    .ForEach(e => filteredResult.Filters.Add(GetFilterFromFilterString(e)));
                }
                else filteredResult = GetFilterFromFilterString(filterString);
            }
            return collection.Filter(filteredResult);
        }

        private static Filter GetFilterFromFilterString(string filterString)
        {
            if (filterString.Contains(':') && filterString.Split(':').Length == 3)
            {
                return new Filter()
                {
                    Field = filterString.Split(':')[0],
                    Operator = filterString.Split(':')[1],
                    Value = filterString.Split(':')[2],
                    Logic = "and"
                };
            }
            return new Filter();
        }
    }

}