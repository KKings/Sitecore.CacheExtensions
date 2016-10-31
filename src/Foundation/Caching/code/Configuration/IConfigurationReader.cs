
namespace KKings.Foundation.Caching.Configuration
{
    using System.Collections.Generic;

    public interface IConfigurationReader
    {
        /// <summary>
        /// Gets all configured sitecore caches
        /// </summary>
        IList<CacheConfig> Read();

        /// <summary>
        /// Parses the Expiration Type
        /// </summary>
        /// <param name="type">Configuration Type</param>
        /// <returns><c>ExpirationType</c></returns>
        ExpirationType ParseExpirationType(string type);
    }
}