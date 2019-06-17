using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.MVCCore.Services.PropertyMappings
{
    public class PropertyMapping<TSource, TDestination> : IPropertyMapping
    {
        public Dictionary<string, PropertyMappingValue> MappingDictionary { get; private set; }

        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            this.MappingDictionary = mappingDictionary;
        }
    }
}
