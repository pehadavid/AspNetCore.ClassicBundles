using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.ClassicBundles
{
    public sealed class BundleCollection
    {
        private static readonly Lazy<BundleCollection> lazy =
            new Lazy<BundleCollection>(() => new BundleCollection());

        public static BundleCollection Instance => lazy.Value;
        public bool IsMinMode { get; set; }
        public bool DontThrowOnMissingFile { get; set; }
        public string RootPath { get; set; }

        private ConcurrentDictionary<string, Bundle> bundlesDictionary;
        private List<FileSystemWatcher> Watchers { get; set; }
        private List<string> FileNames { get; set; }
        private BundleCollection()
        {
            this.bundlesDictionary = new ConcurrentDictionary<string, Bundle>();
            this.Watchers = new List<FileSystemWatcher>();
            this.FileNames = new List<string>();
            this.DontThrowOnMissingFile = false;
        }

        //public Bundle GetBundle(string bundlePath)
        //{
        //    return bundlesDictionary[bundlePath];
        //}

        public Bundle Add(Bundle bundle)
        {
            this.bundlesDictionary.TryAdd(bundle.BundlePath, bundle);
            bundle.Prepare();
            return bundle;
        }

        public Task<Bundle> AddAsync(Bundle bundle)
        {
            this.bundlesDictionary.TryAdd(bundle.BundlePath, bundle);
            return bundle.PrepareAsync();
        }

        public Bundle Find(PathString path)
        {
            return Find(path.Value);

        }
        public Bundle Find(string path)
        {
            KeyValuePair<string, Bundle> b = this.bundlesDictionary.FirstOrDefault(x => x.Key == path);
            return b.Value;

        }

        private void ConfigureWatchers(List<KeyValuePair<string, string>> Pathes)
        {
            foreach (var path in Pathes)
            {
                var dirPath = Path.GetDirectoryName(path.Value);
                var watcher = Watchers.FirstOrDefault(x => x.Path == dirPath);
                if (watcher == null)
                {
                    AddWatcher(path.Value);
                }

                InsertFileName(Path.GetFileName(path.Value));
            }
        }

        private void InsertFileName(string fileName)
        {
            this.FileNames.Add(fileName);
            this.FileNames = this.FileNames.Distinct().ToList();
        }

        private void AddWatcher(string fullPath)
        {
            var directory = Path.GetDirectoryName(fullPath);
            var watcher = new FileSystemWatcher(directory)
            {
                Filter = "*.*",
                //   NotifyFilter = NotifyFilters.LastWrite,
                EnableRaisingEvents = true
            };
            watcher.Changed += Watcher_Changed;
            watcher.Renamed += Watcher_Changed;
            this.Watchers.Add(watcher);
        }
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            var bundle = Instance.FindByFile(CleanPath(e.FullPath));
            bundle?.Prepare();
        }

        private string CleanPath(string eFullPath)
        {
            var uri = new Uri($"file:///{eFullPath}");
            var directory = Path.GetDirectoryName(uri.LocalPath);
            foreach (var fullLocalFileName in Directory.GetFiles(directory))
            {
                var fname = Path.GetFileName(fullLocalFileName);
                if (Path.GetFileName(uri.LocalPath).Contains(fname))
                    return fullLocalFileName;
            }

            return eFullPath;
        }

        private Bundle FindByFile(string eName)
        {
            return this.bundlesDictionary
                .FirstOrDefault(x => x.Value.Pathes.FirstOrDefault(y => y.Value == eName).Value != null).Value;
        }

        public void ApplyMonitoring()
        {
            var instance = Instance;
            var pathes = instance.bundlesDictionary.SelectMany(pair => pair.Value.Pathes)
                .ToList();
            ConfigureWatchers(pathes);
        }
    }
}