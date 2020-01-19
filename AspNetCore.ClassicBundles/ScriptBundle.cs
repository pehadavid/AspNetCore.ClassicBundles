using System;
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
            foreach (var item in Pathes)
            {
                result += $"<script  type=\"text/javascript\" src=\"{item.Key}\"></script>";
            }

            return new HtmlString(result);
        }

        public override HtmlString MinRender()
        {
           return new HtmlString($"<script  type=\"text/javascript\" src=\"{this.BundlePath.Replace("~/","/")}?v={this.VersionIdentifier}\"></script>");
        }

    

        public override string GetResponseContentType()
        {
            return "application/javascript";
        }

        public override string GetContentText()
        {
            throw new NotImplementedException();
        }
    }
}