using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.MVCCore.Filters
{
    public class HeaderFilter : ActionFilterAttribute
    {
        public string HeaderName { get; set; }
        public bool IsRequired => false;
        public bool CanBeEmpty => true;
        public string FieldName => string.Empty;

        public HeaderFilter()
        {

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var request = context.HttpContext.Request;

            var exists = request.Headers.ContainsKey(HeaderName);

            if(IsRequired && !exists)
            {
                context.Result = new BadRequestObjectResult($"Header {HeaderName} not found.");
                return;
            }

            string headerValue = request.Headers[HeaderName];

            if(!CanBeEmpty && String.IsNullOrEmpty(headerValue))
            {
                context.Result = new BadRequestObjectResult($"Header {HeaderName} can not be empty.");
                return;
            }

            context.HttpContext.Items.Add(FieldName, headerValue);
        }
    }
}
