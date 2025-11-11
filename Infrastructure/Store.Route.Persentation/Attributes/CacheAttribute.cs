using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Store.Route.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Persentation.Attributes
{
    public class CacheAttribute(int durationInSec) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
          var CacheService=  context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var CacheKey = GenerateCacheKey(context.HttpContext.Request);
            var Result = await CacheService.GetCacheValueAsync(CacheKey);

            if(!string.IsNullOrEmpty(Result))
            {
                context.Result = new ContentResult()
                {
                    ContentType = "Application/json",
                    StatusCode = StatusCodes.Status200OK,
                    Content = Result
                };
                //Return response
                return;
            }
            //Execute the end point
           var ContextResult= await next.Invoke();

            if(ContextResult.Result is OkObjectResult okObject)
            {
                await CacheService.SetCacheValueAsync(CacheKey,okObject.Value,TimeSpan.FromSeconds(durationInSec));

            }
        
        }
   
    
     private string GenerateCacheKey(HttpRequest httpRequest)
        {
            var key = new StringBuilder();
            key.Append(httpRequest.Path);

            foreach (var item in httpRequest.Query.OrderBy(q=>q.Key))
            {
                key.Append($"|{item.Key}-{item.Value}");
            }
            return key.ToString();
        }
    }
}
