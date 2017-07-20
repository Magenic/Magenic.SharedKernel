using System;
using System.Collections.Generic;
using System.Linq;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// Conversion related helper functions.
    /// </summary>
    public static class Conversion
    {
        #region Public Methods

        /// <summary>
        /// Converts string value to enum value, returning default if not defined.
        /// </summary>
        /// <typeparam name="TEnum">Type of enum to create.</typeparam>
        /// <param name="value">String value (can be name or numeric string value).</param>
        /// <param name="defaultValue">Default value if invalid or null.</param>
        /// <returns>Enum value.</returns>
        public static TEnum ToEnum<TEnum>(string value, TEnum defaultValue)
        {
            return Enum.IsDefined(typeof(TEnum), value)
                ? (TEnum)Enum.Parse(typeof(TEnum), value)
                : defaultValue;
        }

        /// <summary>
        /// Converts string value to enum value. Throws KeyNotFoundException if invalid 
        /// value is provided.
        /// </summary>
        /// <typeparam name="TEnum">Type of enum to create.</typeparam>
        /// <param name="value">String value (can be name or numeric string value).</param>
        /// <returns>Enum value.</returns>
        public static TEnum ToEnum<TEnum>(string value)
        {
            if (Enum.IsDefined(typeof(TEnum), value))
            {
                return (TEnum)Enum.Parse(typeof(TEnum), value);
            }
            else
            {
                throw new KeyNotFoundException(
                    $"Value {value} is not valid for enum type {typeof(TEnum)}.");
            }
        }

        /// <summary>
        /// Converts int value to enum value, throwing exception if invalid value.
        /// </summary>
        /// <typeparam name="TEnum">Ttype of enum to create.</typeparam>
        /// <param name="value">String value (can be name or numeric string value).</param>
        /// <returns>Enum value.</returns>
        public static TEnum ToEnum<TEnum>(int value)
            where TEnum: struct
        {
            if (Enum.IsDefined(typeof(TEnum), value))
            {
                return (TEnum)(object)value;
            }
            else
            {
                throw new KeyNotFoundException(
                    $"Value {value} is not valid for enum type {typeof(TEnum)}.");
            }
        }

        /// <summary>
        /// Converts array of string values to a list of enum values.
        /// </summary>
        /// <typeparam name="TEnum">Type of enum.</typeparam>
        /// <param name="textValues">Raw string array of values (names or numeric).</param>
        /// <returns>Enum list.</returns>
        public static IList<TEnum> ToEnumList<TEnum>(string[] textValues)
        {
            return textValues
                .Map(textValue => (TEnum)Enum.Parse(typeof(TEnum), textValue))
                .ToList();
        }

        /// <summary>
        /// Takes a string separated by specified separator and converts to a list of enums. 
        /// </summary>
        /// <typeparam name="TEnum">Type of enum to create.</typeparam>
        /// <param name="enumNames">Delimited string of enum values (names or numbers).</param>
        /// <param name="separator">Separator to split on.  Defaults to comma.</param>
        /// <returns>List of enum values.</returns>
        public static IList<TEnum> ToEnumList<TEnum>(
            string enumNames,
            string separator = ",")
        {
            if (enumNames != null)
            {
                return ToEnumList<TEnum>(
                    enumNames.Split(
                        separator.ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                throw new ArgumentNullException(nameof(enumNames));
            }
        }

        #endregion
    }
}
