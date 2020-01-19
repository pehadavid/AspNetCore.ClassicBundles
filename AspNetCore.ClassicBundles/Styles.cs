using Microsoft.AspNetCore.Html;

namespace AspNetCore.ClassicBundles
{
    public static class Styles
    {
        public static HtmlString Render(string bundlePath) => BundleRenderer.Instance.Render(bundlePath);
    }
}
