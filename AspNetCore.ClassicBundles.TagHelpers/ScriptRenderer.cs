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
        public async Task<IViewComponentResult> InvokeAsync(string bundlePath)
        {
           
            var bundle = BundleCollection.Instance.GetBundle(bundlePath);
            if (BundleCollection.Instance.IsMinMode)
            {
                return new HtmlContentViewComponentResult(bundle.MinRender());
            }
            return new HtmlContentViewComponentResult(bundle.FlatRender());
        }

    }
}
