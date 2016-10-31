
namespace KKings.Foundation.Caching.Pipelines.Initialize
{
    using Caches;
    using Configuration;
    using Sitecore.Pipelines;

    /// <summary>
    /// Initialize the caches by finding the nodes in the 
    /// config file under /sitecore/cacheManager/caches
    /// </summary>
    public class InitializeCaches
    {
        /// <summary>
        /// Implementation of reading the cache
        /// </summary>
        private readonly IConfigurationReader configurationReader;
        
        /// <summary>
        /// Implementation of Cache Manager
        /// </summary>
        private readonly ICacheManager cacheManager;

        public InitializeCaches(IConfigurationReader configurationReader,
            ICacheManager cacheManager)
        {
            this.configurationReader = configurationReader;
            this.cacheManager = cacheManager;
        }

        public void Process(PipelineArgs args)
        {
            foreach (var cache in this.configurationReader.Read())
            {
                this.cacheManager.Add(cache);
            }
        }
    }
}
