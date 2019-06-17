using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.MVCCore.Services.TypeHelperService
{
    interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields);
    }
}
