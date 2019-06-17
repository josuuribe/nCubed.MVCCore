using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.MVCCore.Services.PropertyMappings
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; private set; }
        public bool Revert { get; set; }

        public PropertyMappingValue(params string[] destinationProperties)
        {
            DestinationProperties = destinationProperties;
        }
    }
}
