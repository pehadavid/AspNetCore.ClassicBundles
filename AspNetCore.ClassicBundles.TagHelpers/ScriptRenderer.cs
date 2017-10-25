using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspNetCore.ClassicBundles.TagHelpers
{
    [ViewComponent(Name = "ScriptRenderer")]
    public class ScriptRenderer : ViewComponent
    {       
        public  Task<IViewComponentResult> InvokeAsync(string bundlePath)
        {
           
            var bundle = BundleCollection.Instance.Find(bundlePath);
            if (bundle == null)
            {
               return  EmptyRenderer.GetAsync(bundlePath);
            }
            var result = BundleCollection.Instance.IsMinMode ? new HtmlContentViewComponentResult(bundle.MinRender()) : new HtmlContentViewComponentResult(bundle.FlatRender());
            return Task.FromResult<IViewComponentResult>(result);
        }

    }
}
