using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.MVCCore.Services.PropertyMappings
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}
