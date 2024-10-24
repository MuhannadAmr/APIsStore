using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.DEMO.Core.Services.Contract;
using System.Text;

namespace Store.DEMO.APIs.Attributes
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTime;

        public CachedAttribute(int expireTime)
        {
            _expireTime = expireTime;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cachedService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cachKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cacheResponse = await cachedService.GetCacheKeyAsync(cachKey);
            if (!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

            var executedContext = await next();

            if(executedContext.Result is OkObjectResult result)
            {
                await cachedService.SetCacheKeyAsnc(cachKey, result.Value , TimeSpan.FromSeconds(_expireTime));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var cachKey = new StringBuilder();
            cachKey.Append($"{request.Path}");

            foreach(var (key, value) in request.Query.OrderBy(P => P.Key))
            {
                cachKey.Append($"|{key}-{value}");
            }
            return cachKey.ToString();
        }
    }
}
