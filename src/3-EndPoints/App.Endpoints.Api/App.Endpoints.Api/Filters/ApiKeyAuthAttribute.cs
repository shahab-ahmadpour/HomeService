using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace App.Endpoints.Api.Filters
{
    public class ApiKeyAuthAttribute : Attribute, IActionFilter
    {
        private const string ApiKeyName = "ApiKey";

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyName, out var extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("کلید API ارائه نشده است");
                return;
            }

            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = configuration["ApiKey"];

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("کلید API نامعتبر است");
                return;
            }

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
