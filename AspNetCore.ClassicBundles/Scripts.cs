using Microsoft.AspNetCore.Html;

namespace AspNetCore.ClassicBundles
{
    public static class Scripts
    {

        public static HtmlString Render(string bundlePath) => BundleRenderer.Instance.Render(bundlePath);
    }
}