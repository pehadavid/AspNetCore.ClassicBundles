using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace AspNetCore.ClassicBundles.TagHelpers
{
    [ViewComponent(Name = "StyleRenderer")]
    public class StyleRenderer : ViewComponent
    {
        public  Task<IViewComponentResult> InvokeAsync(string bundlePath)
        {

            var bundle = BundleCollection.Instance.Find(bundlePath);
            if (bundle == null)
            {
                return EmptyRenderer.GetAsync(bundlePath);
            }
            
         
            var result =  BundleCollection.Instance.IsMinMode ? new HtmlContentViewComponentResult(bundle.MinRender()) : new HtmlContentViewComponentResult(bundle.FlatRender());
            return Task.FromResult<IViewComponentResult>(result);
        }

    }
}