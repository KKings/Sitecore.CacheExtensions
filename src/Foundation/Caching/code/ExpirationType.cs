
namespace KKings.Foundation.Caching
{
    using System.ComponentModel;

    public enum ExpirationType
    {
        /// <summary>
        /// Cache Entries will expire with the given time
        /// </summary>
        [Description("absolute")]
        Absolute,

        /// <summary>
        /// Cache Entries will expire with the given time
        /// after the last cache hit
        /// </summary>
        [Description("sliding")]
        Sliding,

        /// <summary>
        /// Cache Entries will stick until cleared
        /// </summary>
        [Description("sticky")]
        Sticky
    }
}