
namespace KKings.Foundation.Caching.Tests
{
    using System;
    using System.Xml;
    using Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Sitecore.Abstractions;

    [TestClass]
    public class ConfigurationReaderTests
    {
        private BaseLog BaseLog { get; set; }
        
        [TestInitialize]
        public void Setup()
        {       
            var mockConfigurationFactory = new Mock<BaseFactory>();

            var mockBaseLogger = new Mock<BaseLog>();

            mockBaseLogger
                .Setup(m => m.Debug(It.IsAny<string>(), It.IsAny<object>()));
            mockBaseLogger
                .Setup(m => m.Warn(It.IsAny<string>(), It.IsAny<object>()));
            mockBaseLogger
                .Setup(m => m.Error(It.IsAny<string>(), It.IsAny<object>()));

            this.BaseLog = mockBaseLogger.Object;

        }

        [TestMethod]
        public void Reader_Read_ReturnsAllCaches()
        {
            var mockConfigurationFactory = new Mock<BaseFactory>();

            var cacheConfiguration = 
                        "<caches>" +
                        "<cache name=\"sliding-cache\" maxSize=\"2MB\" lifespan=\"60\" expirationType=\"sliding\" />" +
                        "<cache name=\"absolute-cache\" maxSize=\"2MB\" lifespan=\"60\" expirationType=\"absolute\"/>" +
                        "</caches>";

            mockConfigurationFactory
                .Setup(m => m.GetConfigNode("/sitecore/caches"))
                .Returns(this.ParseXmlNode(cacheConfiguration));

            var sut = new ConfigurationReader(mockConfigurationFactory.Object, this.BaseLog);

            // Act
            var caches = sut.Read();

            // Assert
            Assert.AreEqual(2, caches.Count);
        }

        [TestMethod]
        public void Reader_Read_NoConfigurationShouldNotThrowException()
        {
            var mockConfigurationFactory = new Mock<BaseFactory>();

            mockConfigurationFactory
                .Setup(m => m.GetConfigNode("/sitecore/caches"))
                .Returns(It.Is<XmlNode>(null));

            var sut = new ConfigurationReader(mockConfigurationFactory.Object, this.BaseLog);

            // Act
            var caches = sut.Read();

            // Assert
            Assert.AreEqual(0, caches.Count);
        }

        [TestMethod]
        public void Reader_Read_NoCachesShouldNotThrowException()
        {
            var mockConfigurationFactory = new Mock<BaseFactory>();

            var cacheConfiguration = "<caches></caches>";

            mockConfigurationFactory
                .Setup(m => m.GetConfigNode("/sitecore/caches"))
                .Returns(this.ParseXmlNode(cacheConfiguration));

            var sut = new ConfigurationReader(mockConfigurationFactory.Object, this.BaseLog);

            // Act
            var caches = sut.Read();

            // Assert
            Assert.AreEqual(0, caches.Count);
        }


        [TestMethod]
        public void Reader_ParseExpiration_Sliding()
        {
            // Arrange
            var mockConfigurationFactory = new Mock<BaseFactory>();
            var sut = new ConfigurationReader(mockConfigurationFactory.Object, this.BaseLog);
            var @type = "sliding";

            // Act
            var expirationType = sut.ParseExpirationType(@type);

            // Assert
            Assert.AreEqual((Object)ExpirationType.Sliding, expirationType);
        }

        [TestMethod]
        public void Reader_ParseExpiration_SlidingCaseInsensitive()
        {
            // Arrange
            var mockConfigurationFactory = new Mock<BaseFactory>();
            var sut = new ConfigurationReader(mockConfigurationFactory.Object, this.BaseLog);
            var @type = "SlIdiNg";

            // Act
            var expirationType = sut.ParseExpirationType(@type);

            // Assert
            Assert.AreEqual((Object)ExpirationType.Sliding, expirationType);
        }

        [TestMethod]
        public void Reader_ParseExpiration_Absolute()
        {
            // Arrange
            var mockConfigurationFactory = new Mock<BaseFactory>();
            var sut = new ConfigurationReader(mockConfigurationFactory.Object, this.BaseLog);
            var @type = "absolute";

            // Act
            var expirationType = sut.ParseExpirationType(@type);

            // Assert
            Assert.AreEqual((Object)ExpirationType.Absolute, expirationType);
        }


        [TestMethod]
        public void Reader_ParseExpiration_AbsoluteCaseInsensitive()
        {
            // Arrange
            var mockConfigurationFactory = new Mock<BaseFactory>();
            var sut = new ConfigurationReader(mockConfigurationFactory.Object, this.BaseLog);
            var @type = "absOLute";

            // Act
            var expirationType = sut.ParseExpirationType(@type);

            // Assert
            Assert.AreEqual((Object)ExpirationType.Absolute, expirationType);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Reader_ParseExpiration_NotConfiguredThrowException()
        {
            // Arrange
            var mockConfigurationFactory = new Mock<BaseFactory>();
            var sut = new ConfigurationReader(mockConfigurationFactory.Object, this.BaseLog);
            var @type = "not-configured-type";

            // Act
            sut.ParseExpirationType(@type);

            // Assert
            // Should Throw Error
        }

        private XmlElement ParseXmlNode(string xml)
        {
            var doc = new XmlDocument();

            doc.LoadXml(xml);

            return doc.DocumentElement;
        }
    }
}
