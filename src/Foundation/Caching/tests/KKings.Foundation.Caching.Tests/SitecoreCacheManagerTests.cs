
namespace KKings.Foundation.Caching.Tests
{
    using System;
    using System.Collections.Generic;
    using Caches;
    using Factories;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Sitecore.Abstractions;

    [TestClass]
    public class SitecoreCacheManagerTests
    {
        public SitecoreCacheManager CacheManager { get; set; }

        [TestInitialize]
        public void Setup()
        {
            var mockBaseLogger = new Mock<BaseLog>();

            mockBaseLogger
                .Setup(m => m.Debug(It.IsAny<string>(), It.IsAny<object>()));
            mockBaseLogger
                .Setup(m => m.Warn(It.IsAny<string>(), It.IsAny<object>()));
            mockBaseLogger
                .Setup(m => m.Error(It.IsAny<string>(), It.IsAny<object>()));

            var mockFactory = new Mock<ISitecoreCacheFactory>();

            var mockCache = new Mock<ICache>();
            mockCache.SetupAllProperties();
            
            mockFactory.Setup(m => m.Create(It.IsAny<CacheConfig>(), mockBaseLogger.Object))
                       .Returns(mockCache.Object);

            this.CacheManager = new SitecoreCacheManager(mockBaseLogger.Object, mockFactory.Object);
        }

        [TestMethod]
        public void Manager_Add_CacheAdded()
        {
            // Assert
            var config = new CacheConfig("test-cache", 0, 100, ExpirationType.Sliding);

            // Act
            this.CacheManager.Add(config);

            // Arrange
            Assert.IsTrue(this.CacheManager.Exists(config.Name));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Manager_Add_CacheAddedTwiceThrowsError()
        {
            // Assert
            var name = "cache-added-twice-throws-error";
            var config = new CacheConfig(name, 0, 100, ExpirationType.Sliding);

            // Act
            this.CacheManager.Add(config);
            this.CacheManager.Add(config);

            // Arrange
            // Expected Error
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Manager_Add_CacheAddedWithNoNameThrowsError()
        {
            // Assert
            var name = String.Empty;
            var config = new CacheConfig(name, 0, 100, ExpirationType.Sliding);

            // Act
            this.CacheManager.Add(config);

            // Arrange
            // Expected Error
        }

        [TestMethod]
        public void Manager_Get_CacheExists()
        {
            // Assert
            var name = "cache-exists";
            var config = new CacheConfig(name, 0, 100, ExpirationType.Sliding);

            // Act
            this.CacheManager.Add(config);
            var cache = this.CacheManager.Get(name, false);

            // Arrange
            Assert.IsNotNull(cache, "Cache != null");
            Assert.IsInstanceOfType(cache, typeof(ICache));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Manager_Get_DoesNotExistThrowErrorIfNotSilent()
        {
            // Assert
            var name = "cache-does-not-exist";

            // Act
            var cache = this.CacheManager.Get(name, false);

            // Arrange
            // Expected Error
        }

        [TestMethod]
        public void Manager_Get_DoesNotExistDoesNotThrowErrorIfSilent()
        {
            // Assert
            var name = "cache-does-not-exist";

            // Act
            var cache = this.CacheManager.Get(name, true);

            // Arrange
            Assert.IsNull(cache, "cache != null");
        }
    }
}
