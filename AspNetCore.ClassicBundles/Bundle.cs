using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using NUglify;
using NUglify.JavaScript;

namespace AspNetCore.ClassicBundles
{
    public abstract class Bundle
    {
        public string BundlePath { get; protected set; }

        public Dictionary<string, string> Pathes { get; set; }
        
        public string VersionIdentifier { get; protected set; }
        protected Func<string, NUglify.UglifyResult> MinFunc;

        protected UglifyResult minResult;

        protected Bundle(string bundlePath)
        {
            this.BundlePath = bundlePath;
            this.Pathes = new Dictionary<string, string>();
     
        }


        public virtual Bundle Include(params string[] filePath)
        {

            foreach (var fp in filePath)
            {
                var simplePath = fp.Replace("~/", String.Empty);
                var fullPath = Path.Combine(BundleCollection.Instance.RootPath, simplePath);
 
                this.Pathes.Add($"/{simplePath}", fullPath);

                

            }
           
            return this;
        }

       

      

     
        public virtual Bundle IncludeDirectory(string directoryPath, string pattern)
        {
            directoryPath = directoryPath.Replace("~/", string.Empty);
            directoryPath = Path.Combine(BundleCollection.Instance.RootPath, directoryPath);
            ExpManager.ThrowIfFalse(Directory.Exists(directoryPath));
            var files = Directory.GetFiles(directoryPath, pattern);
            return Include(files);
        }

        protected void SetIdentifier(UglifyResult min)
        {
            SHA256 sha256 = SHA256.Create();
            var hashResult = sha256.ComputeHash(Encoding.Unicode.GetBytes(min.Code));
            this.VersionIdentifier = Convert.ToBase64String(hashResult);
        }

        public abstract HtmlString FlatRender();
        public abstract HtmlString MinRender();

        public void Prepare()
        {

            var fullContent = string.Empty;
            foreach (var item in this.Pathes)
            {
                var fContent = File.ReadAllText(item.Value);
                fContent = Relocate(fContent, item.Key);
                fullContent += fContent;
            }

            this.minResult = MinFunc.Invoke(fullContent);
            var outPath = Path.Combine(BundleCollection.Instance.RootPath, this.BundlePath.Replace("~/", string.Empty));
            File.WriteAllText(outPath, minResult.Code);
            SetIdentifier(minResult);
        }

        /// <summary>
        /// relocate url('../ to absolute path
        /// </summary>
        /// <param name="fContent"></param>
        /// <param name="originPath"></param>
        /// <returns></returns>
        private string Relocate(string fContent, string originPath)
        {
            string pathString = GetRelDir(originPath);

            var pUrl = @"url\((\s*)'../";
            string regexReplace = $"url('{pathString}/";

            return Regex.Replace(fContent, pUrl, regexReplace);
        }

        private string GetRelDir(string originPath, char dirSep = '/')
        {
            var splitted = originPath.Split(dirSep);
            return string.Join($"{dirSep}", splitted.Take(splitted.Length - 2));
        }

        public Task<Bundle> PrepareAsync()
        {
            Prepare();
            return Task.FromResult(this);
        }



        public virtual Stream GetResponseBody()
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(this.minResult.Code));
        }

        public abstract string GetResponseContentType();

        public abstract string GetContentText();
    }
}