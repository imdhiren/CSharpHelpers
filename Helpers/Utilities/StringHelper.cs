using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Helpers.Utilities
{
    public static class StringHelper
    {
        private const string _chars = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";

        /// <summary>
        /// Remove all non-numeric characters from a string
        /// </summary>
        /// <param name="dirtyString">Alphanumeric string</param>
        /// <returns>Number string</returns>
        public static string RemoveNonNumericCharacters(this string dirtyString)
        {
            var rx = new Regex("[^0123456789.]");
            return rx.Replace(dirtyString, "");
        }

        /// <summary>
        /// Returns an alphanimeric guid without dashes and brackets
        /// </summary>
        public static string GetCleanGuid
        {
            get { return Guid.NewGuid().ToString().Replace("{", "").Replace("}", "").Replace("-", "").ToLower(); }
        }

        /// <summary>
        /// Returns a list of email addresses from a string that is delimited with , or ; or a space
        /// </summary>
        /// <param name="emailField">String of email addresses</param>
        /// <returns>Array of email addresses</returns>
        public static string[] GetEmailsFromString(this string emailField)
        {
            var delimiters = new[] { ',', ';', ' ' };
            return emailField.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Validate string is a valid email address
        /// </summary>
        /// <param name="email">Email address to validate</param>
        /// <returns>True or False</returns>
        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            email = email.Trim().ToLower();
            const string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                   + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                   + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            return Regex.IsMatch(email, pattern);
        }

        /// <summary>
        /// Returns a formatted phone number from a numeric phone number
        /// </summary>
        /// <param name="phone">Numeric phone number</param>
        /// <returns>Formatted phone number</returns>
        public static string FormatPhoneNumber(this string phone)
        {
            var phoneNumber = phone.RemoveNonNumericCharacters();
            if (phoneNumber.Length == 10 || phoneNumber.Length == 7 || phoneNumber.Length == 11 ||
                phoneNumber.Length == 12)
            {
                switch (phoneNumber.Length)
                {
                    case 11:
                        phoneNumber = String.Format("{0:+0# (###) ###-####}", decimal.Parse(phoneNumber));
                        break;
                    case 12:
                        phoneNumber = String.Format("{0:+## (###) ###-####}", decimal.Parse(phoneNumber));
                        break;
                    case 10:
                        phoneNumber = String.Format("{0:(###) ###-####}", decimal.Parse(phoneNumber));
                        break;
                    case 7:
                        phoneNumber = String.Format("{0:###-####}", decimal.Parse(phoneNumber));
                        break;
                }
            }
            else
            {
                throw new ArgumentException("The phone number must either be 7, 10, 11 or 12 digits long");
            }
            return phoneNumber;
        }

        /// <summary>
        /// Generate a random string
        /// </summary>
        /// <param name="size">Length of string</param>
        /// <returns>Random string</returns>
        public static string RandomString(int size)
        {
            return RandomString(size, 1, _chars)[0];
        }

        /// <summary>
        /// Generate a list of random strings
        /// </summary>
        /// <param name="size">Length of each string</param>
        /// <param name="quantity">Number of random strings to return</param>
        /// <returns>List of randomized strings</returns>
        public static List<string> RandomString(int size, int quantity)
        {
            return RandomString(size, quantity, _chars);
        }

        /// <summary>
        /// Generate a list of random strings
        /// </summary>
        /// <param name="size">Length of each string</param>
        /// <param name="quantity">Number of random strings to return</param>
        /// <param name="characters">Characters to randomize</param>
        /// <returns>List of randomized strings</returns>
        public static List<string> RandomString(int size, int quantity, string characters)
        {
            var ret = new List<string>();
            var rng = new Random();
            while (ret.Count < quantity)
            {
                var buffer = new char[size];
                for (var i = 0; i < size; i++)
                {
                    buffer[i] = characters[rng.Next(characters.Length)];
                }
                var item = new string(buffer);
                if (!ret.Contains(item))
                    ret.Add(item);
            }
            return ret;
        }

        /// <summary>
        /// Convert a string to a byte array
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <returns>Byte array</returns>
        public static byte[] ToByteArray(this string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// Split a string based on delimiters
        /// </summary>
        /// <param name="data">String to split</param>
        /// <param name="delimiters">Delimiters to check for</param>
        /// <returns>Ienumerable of split string</returns>
        public static IEnumerable<string> SplitString(this string data, string[] delimiters)
        {
            var temp = data.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            return temp.Select(item => item.Trim()).ToList();
        }

        /// <summary>
        /// Date format conversion yyyy-MM-dd specified format
        /// </summary>
        /// <param name="sourceObj"></param>
        /// <returns></returns>
        public static string ToDate(this object sourceObj)
        {
            return ToDate(sourceObj, "yyyy-MM-dd");
        }

        /// <summary>
        /// Date format conversion
        /// </summary>
        /// <param name="sourceObj">source data</param>
        /// <param name="dataFormat">Formatted string</param>
        /// <returns></returns>
        public static string ToDate(this object sourceObj, string dataFormat)
        {
            DateTime temp = DateTime.Now;

            DateTime.TryParse(sourceObj.ToString(), out temp);

            return temp.ToString(dataFormat);
        }

        /// <summary>
        /// Date to Gujarati
        /// </summary>
        /// <param name="dow"></param>
        /// <returns></returns>
        public static string TransDayOfWeekToGujarati(this DayOfWeek dow)
        {
            switch (dow)
            {
                case DayOfWeek.Friday:
                    return "શુક્રવાર્";
                case DayOfWeek.Monday:
                    return "સોમવાર";
                case DayOfWeek.Saturday:
                    return "શનિવાર્";
                case DayOfWeek.Sunday:
                    return "રવિવાર્";
                case DayOfWeek.Thursday:
                    return "ગુરુવાર્";
                case DayOfWeek.Tuesday:
                    return "મન્ગલ્વાર્";
                case DayOfWeek.Wednesday:
                    return "બુત્વાર્";
                default:
                    return " ";
            }
        }

        /// <summary>
        /// Date changed to English short
        /// </summary>
        /// <param name="dow"></param>
        /// <returns></returns>
        public static string TransDayOfWeekShot(this DayOfWeek dow)
        {
            switch (dow)
            {
                case DayOfWeek.Friday:
                    return "Fri";
                case DayOfWeek.Monday:
                    return "Mon";
                case DayOfWeek.Saturday:
                    return "Sat";
                case DayOfWeek.Sunday:
                    return "Sun";
                case DayOfWeek.Thursday:
                    return "Thur";
                case DayOfWeek.Tuesday:
                    return "Tues";
                case DayOfWeek.Wednesday:
                    return "Wed";
                default:
                    return " ";
            }
        }

        /// <summary>
        /// Text to numeric
        /// </summary>
        /// <param name="sourceObj"></param>
        /// <returns></returns>
        public static int ToInt(this string sourceObj)
        {
            int temp = 0;

            int.TryParse(sourceObj, out temp);

            return temp;
        }

        /// <summary>
        /// Truncate string
        /// </summary>
        /// <param name="sourceObj"></param>
        /// <returns></returns>
        public static string Cut(this object sourceObj)
        {
            return Cut(sourceObj.ToString(), 18);
        }

        /// <summary>
        /// Truncate string
        /// </summary>
        /// <param name="sourceObj"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Cut(this object sourceObj, int length)
        {
            return Cut(sourceObj.ToString(), length);
        }

        /// <summary>
        /// Truncate text with html characters and return plain text
        /// </summary>
        /// <param name="sourceObj"></param>
        /// <returns></returns>
        public static string CutHtml(this object sourceObj)
        {
            var _tmpStr = sourceObj.FilterOutHtml();

            return Cut(_tmpStr, 30);
        }

        /// <summary>
        /// Filter html string
        /// </summary>
        /// <param name="Htmlstring">html string</param>
        /// <returns></returns>
        public static string FilterOutHtml(this object Htmlstring)
        {
            var _tmpStr = Htmlstring.ToString().Trim();

            if (_tmpStr.Length > 0)
            {
                //Remove HTML tags
                _tmpStr = System.Text.RegularExpressions.Regex.Replace(_tmpStr, "<[^>]+>", "");

                //Remove special symbols
                _tmpStr = System.Text.RegularExpressions.Regex.Replace(_tmpStr, "&[^;]+;", "");
            }
            return _tmpStr.Trim().Replace("'", "’");
        }
    }
}
