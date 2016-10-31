
namespace KKings.Foundation.Caching.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;

    public static class EnumExtensions
    {
        /// <summary>
        /// Using Reflection, get the Description from the Enum Attribute
        /// </summary>
        /// <param name="value">Enum Value to get the Description</param>
        /// <returns>If available, the description of the Enum. Defaults the name of the Enum.</returns>
        public static string Description<T>(this T value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"{typeof(T)} must be an enumerated type.");
            }

            var fieldInfo = value
                .GetType()
                .GetField(value.ToString(CultureInfo.InvariantCulture));

            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Any()
                ? attributes[0].Description
                : value.ToString(CultureInfo.InvariantCulture);
        }
    }
}