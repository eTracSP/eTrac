using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace WorkOrderEMS.Data.RepositaryContext
{
    /// <summary>EntityHelper
    /// Created BY:     Nagendra Upwanshi
    /// Created On:     Aug-21-2014 
    /// Created For:    EntityHelper 
    /// </summary>
    public static class EntityHelper<T> where T : class
    {
        public static IEnumerable<T> SortAndFilterRecords<K>(IEnumerable<T> entities, Expression<Func<T, K>> orderBy, out int totalRecords, SortDirection sortDirection, int pageNumber, int pageSize)
        {
            return SortAndFilterRecords(entities.AsQueryable(), orderBy, out totalRecords, sortDirection, pageNumber, pageSize)
                .ToList();
        }

        public static IQueryable<T> SortAndFilterRecords<K>(IQueryable<T> entities, Expression<Func<T, K>> orderBy, out int totalRecords, SortDirection sortDirection, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                pageNumber = 1;

            if (pageSize <= 0)
                pageSize = 100; //TODO: this value should come from db SysSetting table

            var skipRecords = (pageNumber - 1) * pageSize;

            if (orderBy != null)
                entities = sortDirection == SortDirection.Ascending ? entities.OrderBy(orderBy) : entities.OrderByDescending(orderBy);

            totalRecords = entities.Count();

            entities = entities.Skip(skipRecords)
                .Take(pageSize);

            return entities;
        }

        public static IQueryable<T> SortAndFilterRecords<K>(IQueryable<T> entities, Expression<Func<T, K>> orderBy, SortDirection sortDirection)
        {
            if (orderBy != null)
                entities = sortDirection == SortDirection.Ascending ? entities.OrderBy(orderBy) : entities.OrderByDescending(orderBy);

            return entities;
        }

        public static bool IsPropertyNameValid(T entity, string propertyName)
        {
            return TypeDescriptor.GetProperties(entity)[propertyName] != null;
        }

        public static Expression<Func<T, object>> GetPropertyExpression(string propertyName)
        {
            var parameterExpression = Expression.Parameter(typeof(T), "type");
            var propertyExpression = Expression.Property(parameterExpression, propertyName);

            return Expression.Lambda<Func<T, object>>(Expression.Convert(propertyExpression, propertyExpression.Type), parameterExpression);
        }
    }
}
