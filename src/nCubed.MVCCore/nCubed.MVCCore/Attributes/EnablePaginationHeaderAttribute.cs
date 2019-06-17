using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using nCubed.MVCCore.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace nCubed.MVCCore.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class EnablePaginationHeaderAttribute : ActionFilterAttribute
    {
        public string PageSize = nameof(PageSize);
        public string PageNumber = nameof(PageNumber);


        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext == null)
            {
                throw new ArgumentNullException(nameof(actionExecutedContext));
            }

            HttpRequest request = actionExecutedContext.HttpContext.Request;
            if (request == null)
            {
                throw new ArgumentNullException(nameof(actionExecutedContext), "Action executed context must have a request.");
            }

            ActionDescriptor actionDescriptor = actionExecutedContext.ActionDescriptor;
            if (actionDescriptor == null)
            {
                throw new ArgumentNullException(nameof(actionExecutedContext), "Action context must have a descriptor.");
            }

            HttpResponse response = actionExecutedContext.HttpContext.Response;
            // Check is the response is set and successful.
            if (response != null && IsSuccessStatusCode(response.StatusCode) && actionExecutedContext.Result != null)
            {
                // actionExecutedContext.Result might also indicate a status code that has not yet
                // been applied to the result; make sure it's also successful.
                StatusCodeResult statusCodeResult = actionExecutedContext.Result as StatusCodeResult;
                if (statusCodeResult == null || IsSuccessStatusCode(statusCodeResult.StatusCode))
                {
                    ObjectResult responseContent = actionExecutedContext.Result as ObjectResult;
                    if (responseContent != null)
                    {
                        var query = responseContent.Value as IEnumerable<dynamic>;
                        ResourceParameters parameters = new ResourceParameters();
                        foreach (var kvp in request.Query)
                        {
                            switch (kvp.Key)
                            {
                                case String s when s == PageSize:
                                    if (int.TryParse(kvp.Value, out int pageSize))
                                    {
                                        parameters.PageSize = pageSize;
                                    }
                                    else
                                    {
                                        throw new ArgumentException($"{PageSize} wrong value '{pageSize}'.");
                                    }
                                    break;
                                case String s when s == PageNumber:
                                    if (int.TryParse(kvp.Value, out int pageNumber))
                                    {
                                        parameters.PageNumber = pageNumber;
                                    }
                                    else
                                    {
                                        throw new ArgumentException($"{PageNumber} wrong value '{pageNumber}'.");
                                    }
                                    break;
                            }
                        }
                        AddPaginationHeader((actionExecutedContext.Controller as Controller), query.Count(), parameters);
                    }
                }
            }
        }

        /// <summary>
        /// Determine if the status code indicates success.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>True if the response has a success status code; false otherwise.</returns>
        private bool IsSuccessStatusCode(int statusCode)
        {
            return statusCode >= 200 && statusCode < 300;
        }

        private void AddPaginationHeader(Controller controller, int count, ResourceParameters resourceParameters)
        {
            var linkGenerator = controller.HttpContext.RequestServices.GetRequiredService<LinkGenerator>();

            var previousLink = string.Empty;
            var nextLink = string.Empty;
            int totalPages = 0;

            totalPages = (int)Math.Ceiling(count / (double)resourceParameters.PageSize);// 1 is minimun value

            if (totalPages > 1)
            {
                object previousLinkValue = null;
                object nextLinkValue = null;

                switch (resourceParameters.PageNumber)
                {
                    case 1:
                        nextLinkValue = new
                        {
                            pageNumber = 2,
                        };
                        break;
                    case int page when resourceParameters.PageNumber < 0:
                        nextLinkValue = new
                        {
                            pageNumber = Math.Min(2, totalPages),
                        };
                        break;
                    case int page when resourceParameters.PageNumber > totalPages:
                        previousLinkValue = new
                        {
                            pageNumber = totalPages - 1,
                        };
                        break;
                    case int page when resourceParameters.PageNumber == totalPages:
                        previousLinkValue = new
                        {
                            pageNumber = totalPages - 1,
                        };
                        break;
                    case int page when resourceParameters.PageNumber < totalPages:
                        previousLinkValue = new
                        {
                            pageNumber = resourceParameters.PageNumber - 1,
                        };
                        nextLinkValue = new
                        {
                            pageNumber = resourceParameters.PageNumber + 1,
                        };
                        break;
                }

                previousLink = previousLinkValue == null ? string.Empty : linkGenerator.GetPathByAction(action: controller.ControllerContext.ActionDescriptor.ActionName, controller: controller.ControllerContext.ActionDescriptor.ControllerName, values: previousLinkValue);
                nextLink = nextLinkValue == null ? string.Empty : linkGenerator.GetPathByAction(action: controller.ControllerContext.ActionDescriptor.ActionName, controller: controller.ControllerContext.ActionDescriptor.ControllerName, values: nextLinkValue);
            }

            var paginationMetadata = new
            {
                totalCount = count,
                pageSize = resourceParameters.PageSize,
                currentPage = resourceParameters.PageNumber,
                totalPage = count == 0 ? 0 : totalPages,
                previousPageLink = previousLink,
                nextPageLink = nextLink
            };
            controller.HttpContext.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
        }
    }
}
