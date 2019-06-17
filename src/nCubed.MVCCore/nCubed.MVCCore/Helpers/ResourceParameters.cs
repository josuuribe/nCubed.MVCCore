using nCubed.MVCCore.Services.PropertyMappings;
using System;

namespace nCubed.MVCCore.Helpers
{
    internal class ResourceParameters
    {
        const int maxPageSize = 20;
        private int pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = Math.Min(maxPageSize, value);
            }
        }

        public string Filter { get; set; }

        public string SearchQuery { get; set; }

        public string OrderBy { get; set; }

        public string Fields { get; set; }

        internal string OrderByLinq<TModel, TEntity>()
        {
            return PropertyMappingService.DynamicSort<TModel, TEntity>(this.OrderBy);
        }
    }
}
