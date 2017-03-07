using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using NUglify;
using NUglify.JavaScript;

namespace AspNetCore.ClassicBundles
{
    public abstract class Bundle
    {
        public string BundlePath { get; protected set; }
        protected List<string> filesPath;
        protected List<string> WebsPathList;
        public string VersionIdentifier { get; protected set; }
        protected Func<string, UgliflyResult> MinFunc;

        protected Bundle(string bundlePath)
        {
            this.BundlePath = bundlePath;
            this.filesPath = new List<string>();
            this.WebsPathList = new List<string>();
        }


        public virtual Bundle Include(params string[] filePath)
        {

            foreach (var fp in filePath)
            {
                var simplePath = fp.Replace("~/", String.Empty);
                var fullPath = Path.Combine(BundleCollection.Instance.RootPath, simplePath);
                this.filesPath.Add(fullPath);
                this.WebsPathList.Add($"/{simplePath}");
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

        protected void SetIdentifier(UgliflyResult min)
        {
            SHA256 sha256 = SHA256.Create();
            var hashResult = sha256.ComputeHash(Encoding.Unicode.GetBytes(min.Code));
            this.VersionIdentifier = Convert.ToBase64String(hashResult);
        }
     
        public abstract HtmlString FlatRender();
        public abstract HtmlString MinRender();

        public virtual void Prepare()
        {

            var fullContent = string.Empty;
            foreach (string fp in this.filesPath)
            {
                fullContent += File.ReadAllText(fp);
            }

            var min = MinFunc.Invoke(fullContent);
            var outPath = Path.Combine(BundleCollection.Instance.RootPath, this.BundlePath.Replace("~/", string.Empty));
            File.WriteAllText(outPath, min.Code);
            SetIdentifier(min);
        }

        public  Task<Bundle> PrepareAsync()
        {
            Prepare();
            return Task.FromResult(this);
        }

      

    }
}