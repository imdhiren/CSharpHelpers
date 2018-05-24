using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Utilities
{
    public static class DecimalUtility
    {
        public static decimal? Round(this decimal? value, int decimals)
        {
            if (value.HasValue)
            {
                return Decimal.Round(value.Value, 2);
            }
            return null;
        }

        /// <summary>
        ///Cut off decimal places
        /// </summary>
        /// <param name="source">Source number</param>
        /// <param name="len">Decimal digits</param>
        /// <returns></returns>
        public static string TruncateDecimal(this decimal source, int len)
        {

            string destination = string.Empty;
            int i = source.ToString().IndexOf(".");
            if (i < 0)
            {
                destination = source.ToString();
            }
            else
            {
                if (source.ToString().Length >= (i + len + 1))
                    destination = source.ToString().Substring(0, i + len + 1);
                else
                    destination = source.ToString();
            }
            return destination;

        }

        /// <summary>
        /// Cut off decimal places
        /// </summary>
        /// <param name="source">Source number</param>
        /// <param name="len">Decimal digits</param>
        /// <returns></returns>
        public static string TruncateDecimal(this decimal? source, int len)
        {
            if (!source.HasValue)
            {
                return string.Empty;
            }
            return TruncateDecimal(source.Value, len);

        }

        /// <summary>
		/// Determines whether all the texts in the provided collection represent a valid decimal.
		/// </summary>
		/// <param name="texts">A collection of texts.</param>
		public static bool AreDecimals(IEnumerable<string> texts)
        {
            return texts.All(t => IsDecimal(t));
        }

        /// <summary>
        /// Gets only the decimal part of this decimal.
        /// <para>If the value is negative, the returned value will also be negative.</para>
        /// </summary>
        /// <param name="value">The decimal value.</param>
        public static decimal GetDecimals(this decimal value)
        {
            return value - decimal.Truncate(value);
        }

        /// <summary>
        /// Gets the specified number of decimals from the decimal part of this decimal number as an integer.
        /// <para>If the value is negative, the returned value will also be negative.</para>
        /// </summary>
        /// <param name="value">The decimal value.</param>
        /// <param name="decimalCount">Number of decimals to get.</param>
        public static int GetDecimalPart(this decimal value, int decimalCount)
        {
            decimal decimals = GetDecimals(value);
            return (int)(decimals * (int)(Math.Pow(10, decimalCount)));
        }

        /// <summary>
        /// Gets the whole decimal part of this decimal number as an integer.
        /// <para>If the value is negative, the returned value will also be negative.</para>
        /// </summary>
        /// <param name="value">The decimal value.</param>
        public static int GetDecimalPart(this decimal value)
        {
            decimal decimals = GetDecimals(value);
            int decimalPart = 0;
            int add;
            while (decimals != 0)
            {
                decimals *= 10;
                decimalPart *= 10;
                add = (int)decimals;
                decimalPart += add;
                decimals -= add;
            }
            return decimalPart;
        }

        /// <summary>
        /// Gets the whole part of this decimal number.
        /// </summary>
        /// <param name="value">The decimal value.</param>
        public static int GetWholePart(this decimal value)
        {
            return (int)decimal.Truncate(value);
        }

        /// <summary>
        /// Determines whether the provided text represents a valid decimal.
        /// </summary>
        /// <param name="text">The text.</param>
        public static bool IsDecimal(string text)
        {
            return decimal.TryParse(text, out decimal value);
        }

        /// <summary>
        /// Converts a collection of string representations of decimals to actual decimals.
        /// </summary>
        /// <param name="texts">A collection of texts to parse.</param>
        public static IEnumerable<decimal> Parse(IEnumerable<string> texts)
        {
            return texts.Select(t => decimal.Parse(t));
        }

        /// <summary>
        /// Converts a collection of string representations of decimals to actual decimals. Sets zero if a text is invalid.
        /// </summary>
        /// <param name="texts">A collection of texts to parse.</param>
        public static IEnumerable<decimal> Parse0(IEnumerable<string> texts)
        {
            return texts.Select(t => Parse0(t));
        }

        /// <summary>
        /// Parses the provided text to a decimal. Returns zero if it is invalid.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        public static decimal Parse0(string text)
        {
            return ParseNullable(text) ?? 0;
        }

        /// <summary>
        /// Converts a collection of string representations of decimals to actual decimals. Sets null if a text is invalid.
        /// </summary>
        /// <param name="texts">A collection of texts to parse.</param>
        public static IEnumerable<decimal?> ParseNullable(IEnumerable<string> texts)
        {
            return texts.Select(t => ParseNullable(t));
        }

        /// <summary>
        /// Parses the provided text to a decimal. Returns null if it is invalid.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        public static decimal? ParseNullable(string text)
        {
            if (decimal.TryParse(text, out decimal value))
            {
                return value;
            }
            return null;
        }
    }
}