
namespace KKings.Foundation.Caching.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Extensions;
    using Sitecore;
    using Sitecore.Abstractions;
    using Sitecore.Xml;

    public sealed class ConfigurationReader : IConfigurationReader
    {
        /// <summary>
        /// Top Level Configuration Node Path for Caches
        /// </summary>
        private const string Path = "/sitecore/caches";

        /// <summary>
        /// Configuration Node Path for each Cache
        /// </summary>
        private const string NodePath = "cache";

        /// <summary>
        /// Implementation of Base Factory
        /// </summary>
        private readonly BaseFactory baseFactory;

        /// <summary>
        /// Implementation of Base Log
        /// </summary>
        private readonly BaseLog baseLog;

        public ConfigurationReader(BaseFactory configFactory, BaseLog logger)
        {
            this.baseFactory = configFactory;
            this.baseLog = logger;
        }

        /// <summary>
        /// Reads all configured Sitecore Caches
        /// </summary>
        /// <returns>List of Sitecore Cache Configurations</returns>
        public IList<CacheConfig> Read()
        {
            var root = this.baseFactory.GetConfigNode(ConfigurationReader.Path);

            if (root == null)
            {
                this.baseLog.Debug($"Unable to find cache configuration at {ConfigurationReader.Path}", this);
                return new CacheConfig[0];
            }

            var nodes = root.SelectNodes(ConfigurationReader.NodePath);

            if (nodes == null || nodes.Count == 0)
            {
                this.baseLog.Debug($"No caches have been configured at {ConfigurationReader.NodePath}", this);
                return new CacheConfig[0];
            }

            return (from XmlNode node in nodes
                    let name = XmlUtil.GetAttribute("name", node)
                    let maxSize = StringUtil.ParseSizeString(XmlUtil.GetAttribute("maxSize", node))
                    let lifespan = Int32.Parse(XmlUtil.GetAttribute("lifespan", node))
                    let expirationType = this.ParseExpirationType(XmlUtil.GetAttribute("expirationType", node))
                    select new CacheConfig(name, maxSize, lifespan, expirationType))
                .ToList();
        }

        /// <summary>
        /// Parses the Expiration Type from the Configuration
        /// </summary>
        /// <param name="type">Configured Type</param>
        /// <returns><c>Expiration Type</c></returns>
        public ExpirationType ParseExpirationType(string type)
        {
            if (String.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            var absolute    = ExpirationType.Absolute.Description();
            var sliding     = ExpirationType.Sliding.Description();
            var sticky      = ExpirationType.Sticky.Description();

            if (type.Equals(absolute, StringComparison.InvariantCultureIgnoreCase))
            {
                return ExpirationType.Absolute;
            }

            if (type.Equals(sliding, StringComparison.InvariantCultureIgnoreCase))
            {
                return ExpirationType.Sliding;
            }

            if (type.Equals(sticky, StringComparison.InvariantCultureIgnoreCase))
            {
                return ExpirationType.Sticky;
            }

            throw new ArgumentException($"Unknown expiration type found, {type}");
        }
    }
}