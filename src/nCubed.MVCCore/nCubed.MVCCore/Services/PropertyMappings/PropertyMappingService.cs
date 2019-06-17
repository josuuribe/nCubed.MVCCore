using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nCubed.MVCCore.Services.PropertyMappings
{
    public static class PropertyMappingService //: IPropertyMappingService
    {
        //private Dictionary<string, PropertyMappingValue> propertyMapping =
        //    new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        //    {
        //        {"Id",new PropertyMappingValue(new List<string>(){"Id" }) },
        //        {"Name",new PropertyMappingValue(new List<string>(){"FirstName, LastName" }) },
        //    };

        private static IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

        public static void Add<TModel, TEntity>(string property, PropertyMappingValue value)
        {
            propertyMappings.Add(new PropertyMapping<TModel, TEntity>(
                new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
                {
                    { property, value }
                }));
        }

        //public PropertyMappingService(Dictionary<string, PropertyMappingValue> propertyMapping)
        //{
        //    propertyMappings.Add(new PropertyMapping<TModel, TEntity>(propertyMapping));
        //}

        private static Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {

            var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();
            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First().MappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)}>");


        }

        public static bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var fieldsAfterSplit = fields.Split(',');

            foreach (var field in fieldsAfterSplit)
            {
                var trimmedField = field.Trim();

                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }

        public static string DynamicSort<TModel, TEntity>(string orderBy)
        {
            string sort = string.Empty;

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return sort;
            }

            Dictionary<string, PropertyMappingValue> mappingDictionary = GetPropertyMapping<TModel, TEntity>();

            var orderByAfterSplit = orderBy.Split(",");

            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                var trimmedOrderByClause = orderByClause.Trim();
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");
                var indexOffFirstSpace = trimmedOrderByClause.IndexOf(" ");
                var propertyName = indexOffFirstSpace == -1 ? trimmedOrderByClause : trimmedOrderByClause.Remove(indexOffFirstSpace);

                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing.");
                }

                var propertyMappingValue = mappingDictionary[propertyName];
                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException(nameof(propertyMappingValue));
                }

                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    if (propertyMappingValue.Revert)
                    {
                        orderDescending = !orderDescending;
                    }
                    sort += destinationProperty + (orderDescending ? "descending" : "ascending");
                }
            }
            return sort;
        }
    }
}
