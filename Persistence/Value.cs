using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Persistence
{
    public static class Value
    {
        public const string RegExPhone = @"^\d?(?:(?:[\+]?(?:[\d]{1,3}(?:[ ]+|[\-.])))?[(]?(?:[\d]{3})[\-/)]?(?:[ ]+)?)?(?:[a-zA-Z2-9][a-zA-Z0-9 \-.]{6,})(?:(?:[ ]+|[xX]|(i:ext[\.]?)){1,2}(?:[\d]{1,5}))?$";
        public const string RegExZipCode = @"^(\d{5}-\d{4}|\d{5}|\d{9})$|^([a-zA-Z]\d[a-zA-Z] \d[a-zA-Z]\d)$";
        public const string RegExEmail = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                        @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                        @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
		public const string RegExPassword = @"^[-a-zA-Z0-9@]{1,25}$";

		public static bool IsValidPassword(this string s)
		{
			return Regex.IsMatch(s, RegExPassword);
		}

        public static bool IsPhone(this string s)
        {
            return Regex.IsMatch(s, RegExPhone);
        }

        public static bool IsZipCode(this string s)
        {
            return Regex.IsMatch(s, RegExZipCode);
        }

        public static bool IsEmail(this string s)
        {
            return Regex.IsMatch(s, RegExEmail);
        }

        public static bool IsNumeric(this string s)
        {
            Decimal result;
            return Decimal.TryParse(s, out result);
        }

        public static string ToNumericOnly(this string s)
        {
            string ret = "";
            foreach (char c in s.ToArray())
                if (Char.IsNumber(c) || c == '.')
                    ret += c;
            return ret;
        }

        public static string ToNumbersOnly(this string s)
        {
            string ret = "";
            foreach (char c in s.ToArray())
                if (char.IsNumber(c))
                    ret += c;
            return ret;
        }

        public static bool IsNull(this object value)
        {
            if (value == null)
                return true;
            else if (value is Nullable)
                return false;
            else if (value is String && Value.IsEmpty(((string)value)))
                return false;
            else if (value is Int32 && Convert.ToInt32(value) == 0)
                return true;
            else if (value is DateTime && ((DateTime)value) < new DateTime(1753, 1, 1))
                return true;
            else if (value is Decimal && Convert.ToDecimal(value) == 0)
                return true;
            else if (value is Single && Convert.ToSingle(value) == 0)
                return true;
            else if (value is Double && Convert.ToDouble(value) == 0)
                return true;
            else if (value is Int16 && Convert.ToInt16(value) == 0)
                return true;
            else if (value is Int64 && Convert.ToInt64(value) == 0)
                return true;
            else if (value is Guid && (Guid)value == new Guid())
                return true;
            else if (value is DateTimeOffset && (DateTimeOffset)value == new DateTimeOffset())
                return true;
            else
                return false;
        }

        public static string ToPhoneFormat(this string s)
        {
            if (s.IsEmpty())
                s = "";

            s = s.ToNumbersOnly();

            if (s.Length == 10)
                return String.Format("({0}) {1}-{2}", s.Substring(0, 3), s.Substring(3, 3), s.Substring(6));
            else if (s.Length == 11)
                return String.Format("+{0} ({1}) {2}-{3}", s.Substring(0, 1), s.Substring(1, 3), s.Substring(4, 3), s.Substring(7));
            else
                return s;
        }

        public static bool ToBoolean(this string value)
        {
            if (value.ToLower().StartsWith("y"))
                return true;
            else if (value.ToLower().StartsWith("t"))
                return true;
            else if (value == "1")
                return true;
            return false;
        }

        public static string ToYesNo(this bool value)
        {
            return (value) ? "Yes" : "No";
        }

        public static string ToString(this DateTime date)
        {
            return date.ToString("");
        }

        public static string ToString(this DateTime date, string format)
        {
            if (date.IsNull())
                return "";
            else
                return date.ToString(format);
        }

        public static bool IsEmpty(this string s)
        {
            if (s == null)
                return true;
            else
                return String.IsNullOrEmpty(s.Trim());
        }

        public static bool IsNotEmpty(this string s)
        {
            return !s.IsEmpty();
        }

        public static bool IsEmpty(this int i)
        {
            return (i == 0);
        }

        public static bool IsNotEmpty(this int i)
        {
            return (i > 0);
        }

        public static bool IsEmpty(this object o)
        {
            return (o.IsNull());
        }

        public static bool IsNotEmpty(this object o)
        {
            return (! o.IsNull());
        }

        public static bool IsPersistable(this object o)
        {
            foreach (Attribute a in o.GetType().GetCustomAttributes(true))
                if (a is PersistableAttribute)
                    return true;
            return false;
        }

        public static DateTime ToDateTime(this string s)
        {
            if (s.IsEmpty())
                return new DateTime();
            else
                return Convert.ToDateTime(s);
        }

        public static DateTime ToDateTime(this string s, DateTime defaultValue)
        {
            if (s.IsEmpty())
                return defaultValue;
            else
                return Convert.ToDateTime(s);
        }

        public static int ToInt32(this string s)
        {
            if (s.IsEmpty())
                return 0;
            else
                return Convert.ToInt32(s);
        }

        public static int ToInt32(this object o)
        {
            if (o.IsEmpty())
                return 0;
            else
                return Convert.ToInt32(o);
        }

        public static double ToDouble(this string s)
        {
            if (s.IsEmpty())
                return 0;
            else
                return Convert.ToDouble(s);
        }

        public static string TruncateLast(this string s, int characters)
        {
            return s.Substring(0, s.Length - characters);
        }

        public static string TruncateTo(this string s, int characters)
        {
            if (s.IsEmpty())
                return "";
            else if (s.Length < characters)
                return s;
            else
                return s.Substring(0, characters);
        }

        public static byte[] ToBytes(this Stream s)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read = 0;
                while ((read = s.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
