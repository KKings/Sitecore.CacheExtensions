
namespace KKings.Foundation.Caching.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Caches;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class TransientCacheTests
    {
        internal class TestEntry
        {
            public string Name { get; set; }

            public int Value => -5;
        }

        /// <summary>
        /// Mocked HttpContext
        /// </summary>
        public Mock<HttpContextBase> HttpContext { get; set; }

        /// <summary>
        /// Transient Cache (SUT)
        /// </summary>
        public TransientCache TransientCache { get; set; }

        [TestInitialize]
        public void Setup()
        {
            this.HttpContext = new Mock<HttpContextBase>();

            var request     = new Mock<HttpRequestBase>();
            var response    = new Mock<HttpResponseBase>();
            var session     = new Mock<HttpSessionStateBase>();
            var server      = new Mock<HttpServerUtilityBase>();

            this.HttpContext.Setup(ctx => ctx.Request).Returns(request.Object);
            this.HttpContext.Setup(ctx => ctx.Response).Returns(response.Object);
            this.HttpContext.Setup(ctx => ctx.Session).Returns(session.Object);
            this.HttpContext.Setup(ctx => ctx.Server).Returns(server.Object);
            this.HttpContext.Setup(ctx => ctx.Items).Returns(new Dictionary<string, object>());

            var defaultCollection = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>( "default-1", new TestEntry { Name = "Test Entry 1" }),
                new KeyValuePair<string, object>( "default-2", new TestEntry { Name = "Test Entry 2" }),
                new KeyValuePair<string, object>( "default-3", new TestEntry { Name = "Test Entry 3" })
            };

            this.TransientCache = new TransientCache(this.HttpContext.Object, defaultCollection);
        }

        [TestMethod]
        public void TransientCache_Set_ExpectsValue()
        {
            // Arrange
            var value = new TestEntry { Name = "test" };
            var key = Guid.NewGuid().ToString();
            var sut = this.TransientCache;

            // Act
            sut.Set(key, value);

            // Assert
            Assert.IsTrue(sut.Contains(key));
        }

        [TestMethod]
        public void TransientCache_Set_DoubleSetShouldContainLastValue()
        {
            // Arrange
            var entry1 = new TestEntry { Name = "test" };
            var entry2 = new TestEntry { Name = "test" };
            var key = Guid.NewGuid().ToString();
            var sut = this.TransientCache;
            
            // Act
            sut.Set(key, entry1);
            sut.Set(key, entry2);

            var cachedEntry = this.TransientCache.Get<TestEntry>(key);

            // Assert
            Assert.IsTrue(sut.Contains(key));
            Assert.IsNotNull(cachedEntry);
            Assert.AreEqual(entry2.Name, cachedEntry.Name);
        }

        [TestMethod]
        public void TransientCache_Get_ExpectsValue()
        {
            // Arrange
            var key = "default-1";

            var sut = this.TransientCache;

            // Act
            var entry = sut.Get<TestEntry>(key);

            // Assert
            Assert.IsTrue(sut.Contains(key));
            Assert.IsNotNull(entry);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TransientCache_Get_NullKeyShouldThrowException()
        {
            // Arrange
            var sut = this.TransientCache;

            // Act
            var entry = sut.Get<TestEntry>(null);

            // Assert
            // Should throw exception
        }

        [TestMethod]
        public void TransientCache_Get_HandleNullValue()
        {
            // Arrange
            var key = Guid.NewGuid().ToString();
            TestEntry value = null;

            var sut = this.TransientCache;
            sut.Set(key, value);

            // Act
            var entry = sut.Get<TestEntry>(key);

            // Assert
            Assert.IsTrue(sut.Contains(key));
            Assert.IsNull(entry);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TransientCache_Set_NullKeyShouldThrowException()
        {
            // Arrange
            var value = new TestEntry { Name = "Test" };
            var sut = this.TransientCache;

            // Act
            sut.Set(null, value);

            // Assert
            // Should throw error
        }
    }
}
