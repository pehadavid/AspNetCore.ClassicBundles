using System;
using Microsoft.AspNetCore.Html;

namespace AspNetCore.ClassicBundles
{
    public sealed class BundleRenderer
    {
        private static readonly Lazy<BundleRenderer> lazy =
            new Lazy<BundleRenderer>(() => new BundleRenderer());

        public static BundleRenderer Instance => lazy.Value;



        private BundleRenderer()
        {

        }
        public HtmlString Render(string bundlePath)
        {
            var bundle = BundleCollection.Instance.GetBundle(bundlePath);
            ExpManager.ThrowIfNull(bundle);
            //bundle management : dev -> multipletags, prod -> one bundle
            return BundleCollection.Instance.IsMinMode ? MinRender(bundle) : FlatRender(bundle);
        }
        private HtmlString FlatRender(Bundle bundle)
        {
            return bundle.FlatRender();
        }

        private HtmlString MinRender(Bundle bundle)
        {
            return bundle.MinRender();
        }

    }
}