using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using NUglify;

namespace AspNetCore.ClassicBundles
{
    public class ScriptBundle : Bundle
    {
       
        public ScriptBundle(string bundlePath) : base(bundlePath)
        {
            this.MinFunc = s=>Uglify.Js(s);
        
        }




        public override HtmlString FlatRender()
        {
            string result = string.Empty;
            foreach (string fp in WebsPathList)
            {
                result += $"<script  type=\"text/javascript\" src=\"{fp}\"></script>";
            }

            return new HtmlString(result);
        }

        public override HtmlString MinRender()
        {
           return new HtmlString($"<script  type=\"text/javascript\" src=\"{this.BundlePath.Replace("~/","/")}?v={this.VersionIdentifier}\"></script>");
        }

   

    }
}