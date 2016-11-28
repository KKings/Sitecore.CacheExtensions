using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KKings.Foundation.Caching.Tests
{
    using System.Collections.Generic;
    using System.Web;
    using Caches;
    using Moq;

    [TestClass]
    public class SessionCacheTests
    {
        internal class TestEntry
        {
            public string Name { get; set; }

            public int Value => -5;
        }

        /// <summary>
        /// A Class to allow simulation of SessionObject
        /// </summary>
        internal class MockHttpSession : HttpSessionStateBase
        {
            readonly Dictionary<string, object> session = new Dictionary<string, object>();

            public override object this[string name]
            {
                get { return this.session[name]; }
                set { this.session[name] = value; }
            }
        }

        /// <summary>
        /// Mocked HttpContext
        /// </summary>
        public Mock<HttpContextBase> HttpContext { get; set; }

        /// <summary>
        /// Transient Cache (SUT)
        /// </summary>
        public SessionCache SessionCache { get; set; }

        [TestInitialize]
        public void Setup()
        {
            this.HttpContext = new Mock<HttpContextBase>();

            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new MockHttpSession();
            var server = new Mock<HttpServerUtilityBase>();

            this.HttpContext.Setup(ctx => ctx.Request).Returns(request.Object);
            this.HttpContext.Setup(ctx => ctx.Response).Returns(response.Object);
            this.HttpContext.Setup(ctx => ctx.Session).Returns(session);
            this.HttpContext.Setup(ctx => ctx.Server).Returns(server.Object);
            this.HttpContext.Setup(ctx => ctx.Items).Returns(new Dictionary<string, object>());

            var defaultCollection = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>( "default-1", new TestEntry { Name = "Test Entry 1" }),
                new KeyValuePair<string, object>( "default-2", new TestEntry { Name = "Test Entry 2" }),
                new KeyValuePair<string, object>( "default-3", new TestEntry { Name = "Test Entry 3" })
            };

            this.SessionCache = new SessionCache(this.HttpContext.Object, defaultCollection);
        }

        [TestMethod]
        public void SessionCache_Set_ExpectsValue()
        {
            // Arrange
            var value = new TestEntry { Name = "test" };
            var key = Guid.NewGuid().ToString();
            var sut = this.SessionCache;

            // Act
            sut.Set(key, value);

            // Assert
            Assert.IsTrue(sut.Contains(key));
        }

        [TestMethod]
        public void SessionCache_Set_DoubleSetShouldContainLastValue()
        {
            // Arrange
            var entry1 = new TestEntry { Name = "test" };
            var entry2 = new TestEntry { Name = "test" };
            var key = Guid.NewGuid().ToString();
            var sut = this.SessionCache;

            // Act
            sut.Set(key, entry1);
            sut.Set(key, entry2);

            var cachedEntry = this.SessionCache.Get<TestEntry>(key);

            // Assert
            Assert.IsTrue(sut.Contains(key));
            Assert.IsNotNull(cachedEntry);
            Assert.AreEqual(entry2.Name, cachedEntry.Name);
        }

        [TestMethod]
        public void SessionCache_Get_ExpectsValue()
        {
            // Arrange
            var key = "default-1";

            var sut = this.SessionCache;

            // Act
            var entry = sut.Get<TestEntry>(key);

            // Assert
            Assert.IsTrue(sut.Contains(key));
            Assert.IsNotNull(entry);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void SessionCache_Get_NullKeyShouldThrowException()
        {
            // Arrange
            var sut = this.SessionCache;

            // Act
            var entry = sut.Get<TestEntry>(null);

            // Assert
            // Should throw exception
        }

        [TestMethod]
        public void SessionCache_Get_HandleNullValue()
        {
            // Arrange
            var key = Guid.NewGuid().ToString();
            TestEntry value = null;

            var sut = this.SessionCache;
            sut.Set(key, value);

            // Act
            var entry = sut.Get<TestEntry>(key);

            // Assert
            Assert.IsTrue(sut.Contains(key));
            Assert.IsNull(entry);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void SessionCache_Set_NullKeyShouldThrowException()
        {
            // Arrange
            var value = new TestEntry { Name = "Test" };
            var sut = this.SessionCache;

            // Act
            sut.Set(null, value);

            // Assert
            // Should throw error
        }
    }
}
