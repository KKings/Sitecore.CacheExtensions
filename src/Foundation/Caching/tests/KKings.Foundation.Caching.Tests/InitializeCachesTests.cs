namespace KKings.Foundation.Caching.Tests
{
    using Caches;
    using Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Pipelines.Initialize;
    using Sitecore.Pipelines;

    [TestClass]
    public class InitializeCachesTests
    {
        [TestMethod]
        public void Process_Verify_ReaderAndManagerCalled()
        {
            // Arrange
            var reader = new Mock<IConfigurationReader>();
            reader.Setup(m => m.Read())
                  .Returns(new[] { It.IsAny<CacheConfig>(), It.IsAny<CacheConfig>() });

            var manager = new Mock<ICacheManager>();
            manager.Setup(m => m.Add(It.IsAny<CacheConfig>()));

            var processor = new InitializeCaches(reader.Object, manager.Object);
            var args = new PipelineArgs();

            // Act
            processor.Process(args);

            // Assert
            reader.Verify(m => m.Read(), Times.Once);
            manager.Verify(m => m.Add(It.IsAny<CacheConfig>()), Times.Exactly(2));
        }
    }
}
