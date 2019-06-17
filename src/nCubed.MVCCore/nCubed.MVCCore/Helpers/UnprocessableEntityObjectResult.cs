using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.MVCCore.Helpers
{
    public class UnprocessableEntityObjectResult : ObjectResult
    {
        public UnprocessableEntityObjectResult(ModelStateDictionary model)
            : base(new SerializableError(model))
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            StatusCode = 422;
        }
    }
}
