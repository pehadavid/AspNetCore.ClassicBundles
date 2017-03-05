using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace AspNetCore.ClassicBundles.TagHelpers
{
    [ViewComponent(Name = "StyleRenderer")]
    public class StyleRenderer : ViewComponent
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