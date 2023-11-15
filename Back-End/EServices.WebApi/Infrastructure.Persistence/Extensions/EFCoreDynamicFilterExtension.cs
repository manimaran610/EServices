
using System.Linq;
using System.Reflection;
using Domain.Common;
using Infrastructure.Shared.Extensions.Filters;


namespace Infrastructure.Shared.Extensions
{
    public static class EFCoreDynamicFilterExtension
    {
        public static System.Linq.IQueryable<T> GetFilteredList<T>(this IQueryable<T> collection, string filterString, string logic = null) where T : AuditableBaseEntity
        {
            Filter filteredResult = new Filter();
            if (!string.IsNullOrEmpty(filterString))
            {
                if (filterString.Contains(","))
                {
                    filteredResult.Logic = "&&";
                    filterString.Split(",").ToList()
                    .ForEach(e => filteredResult.Filters.Add(GetFilterFromFilterString(e)));
                }
                else filteredResult = GetFilterFromFilterString(filterString, logic);
            }
            return collection.Filter(filteredResult);
        }

        private static Filter GetFilterFromFilterString(string filterString, string logic = null)
        {
            if (filterString.Contains(':') && filterString.Split(':').Length == 3)
            {
                return new Filter()
                {
                    Field = filterString.Split(':')[0],
                    Operator = filterString.Split(':')[1],
                    Value = filterString.Split(':')[2],
                    Logic = logic == null ? "&&" : logic
                };
            }
            return new Filter();
        }
    }

}