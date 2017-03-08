using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using AspNetCore.ClassicBundles;

namespace AspNetCore.ClassicBundles.xUnit
{
    public class JavascriptBundleTests : IDisposable
    {
        private string tempPath;
        private List<string> jsFiles;
        public JavascriptBundleTests()
        {
            this.jsFiles = new List<string>();
            PrepareAssets();
        }

        private void PrepareAssets()
        {
            tempPath = Path.GetTempPath();
            var JsFile1Content = @"document.addEventListener(""DOMActivate"",
                        function(e) {

                            console.log(""dummy thing"");
                            var myverylongvar = 0;
                            for (var i = 0; i < 100; i++) {
                                alert(i);


                                myverylongvar = myverylongvar + i;
                            }
                        });";
            var fp1 = Path.Combine(tempPath, "file-1.js");
            File.WriteAllText(fp1, JsFile1Content);

            var jsFile2Content = @"var divs = document.querySelectorAll(""div"");
alert(""a total of "" + divs.length + "" divs in this page."");";
            var fp2 = Path.Combine(tempPath, "file-2.js");
            File.WriteAllText(fp2, jsFile2Content);

            jsFiles.Add("file-1.js");
            jsFiles.Add("file-2.js");

            if (!Directory.Exists(Path.Combine(tempPath, "utdir")))
            {
                Directory.CreateDirectory(Path.Combine(tempPath, "utdir"));
            }

            File.Copy(fp1, Path.Combine(tempPath,"utdir","file-1.js"), true);
            File.Copy(fp2, Path.Combine(tempPath, "utdir", "file-2.js"),true);

        }
        [Fact]
        public void TestJsBundleSimpleFiles()
        {
            var instance = BundleCollection.Instance;
            var assetsPath = tempPath;
            instance.RootPath = assetsPath;
            var jsBundle = new ScriptBundle("~/my-test-bundle.js").Include(jsFiles.ToArray());

            instance.Add(jsBundle);
            Assert.True(File.Exists(Path.Combine(tempPath, "my-test-bundle.js")));
        }

        [Fact]
        public void TestJsIncludeDirectory()
        {
            var instance = BundleCollection.Instance;
            var assetsPath = tempPath;
            instance.RootPath = assetsPath;
            var jsBundle = new ScriptBundle("~/my-test-dir-bundle.js").IncludeDirectory("~/utdir", "file-*.js");
            instance.Add(jsBundle);
            Assert.True(File.Exists(Path.Combine(tempPath, "my-test-dir-bundle.js")));
        }
        public void Dispose()
        {
            
        }
    }
}
