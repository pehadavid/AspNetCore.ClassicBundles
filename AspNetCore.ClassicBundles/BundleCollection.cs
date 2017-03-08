using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace AspNetCore.ClassicBundles
{
    public sealed class BundleCollection
    {
        private static readonly Lazy<BundleCollection> lazy =
            new Lazy<BundleCollection>(() => new BundleCollection());

        public static BundleCollection Instance => lazy.Value;
        public bool IsMinMode { get; set; }
        public string RootPath { get; set; }

        private ConcurrentDictionary<string, Bundle> bundlesDictionary;
        private BundleCollection()
        {
            this.bundlesDictionary = new ConcurrentDictionary<string, Bundle>();
        }

        public Bundle GetBundle(string bundlePath)
        {
            return bundlesDictionary[bundlePath];
        }

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

    }
}