using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Html;
using NUglify;

namespace AspNetCore.ClassicBundles
{
    public class StyleBundle : Bundle
    {
        public StyleBundle(string bundlePath) : base(bundlePath)
        {
            this.MinFunc = s=>Uglify.Css(s);
        }



        public override HtmlString FlatRender()
        {
            string result = string.Empty;
            foreach (string fp in WebsPathList)
            {
                result += $"<link rel=\"stylesheet\" type=\"text/css\" href=\"{fp}\"  />";
            }

            return new HtmlString(result);
        }

        public override HtmlString MinRender()
        {
            return new HtmlString($"<link rel=\"stylesheet\" type=\"text/css\" href=\"{this.BundlePath.Replace("~/", "/")}?v={this.VersionIdentifier}\"  />");
        }


    }
}