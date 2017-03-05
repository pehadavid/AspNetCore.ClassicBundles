using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspNetCore.ClassicBundles.TagHelpers
{
    [HtmlTargetElement("script-bundle")]
    public class BundleScriptTagHelper : TagHelper
    {
        public string BundlePath { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var bundle = BundleCollection.Instance.GetBundle(BundlePath);
            if (!(bundle is ScriptBundle)) return;
            if (BundleCollection.Instance.IsMinMode)
                ProcessMin(context, output, bundle);
            else
                ProcessFlat(context, output, bundle);
        }

        private void ProcessFlat(TagHelperContext context, TagHelperOutput output, Bundle bundle)
        {
            output.SuppressOutput();
        }

        private void ProcessMin(TagHelperContext context, TagHelperOutput output, Bundle bundle)
        {
            output.TagName = "script";
            output.Attributes.Add("type","text/javascript");
            output.Attributes.Add("src", bundle.BundlePath);
        }
    }
}
