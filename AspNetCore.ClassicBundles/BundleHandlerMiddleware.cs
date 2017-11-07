using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.ClassicBundles
{
    public class BundleHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public BundleHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            Bundle bundle = BundleCollection.Instance.Find(path);
            if (bundle == null)
                // Call the next delegate/middleware in the pipeline
                return this._next(context);
            else
            {
                
                context.Response.ContentType = bundle.GetResponseContentType();
                context.Response.Body = bundle.GetResponseBody();
                return Task.CompletedTask;

            }
        }
    }

    public static class BundleHandlerMiddlewareExtensions
    {


        public static IApplicationBuilder UseBundles(this IApplicationBuilder builder, IHostingEnvironment env, Action<BundleCollection> setCollectionAction)
        {
            var bundles = BundleCollection.Instance;
            bundles.IsMinMode = !env.IsDevelopment();
            bundles.RootPath = env.WebRootPath;
            setCollectionAction(BundleCollection.Instance);
            bundles.ApplyMonitoring();
            return builder.UseMiddleware<BundleHandlerMiddleware>();
           
        }
    }
}

