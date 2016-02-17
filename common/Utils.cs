using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace common
{
    public static class Utils
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            if (list == null) return;
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box); while (!(box[0] < n * (uint.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static int FromString(string x)
        {
            if (x.StartsWith("0x"))
                return int.Parse(x.Substring(2), System.Globalization.NumberStyles.HexNumber);
            else
                return int.Parse(x);
        }

        public static string To4Hex(this ushort x)
        {
            return "0x" + x.ToString("x4");
        }

        public static string ToCommaSepString<T>(this T[] arr)
        {
            var ret = new StringBuilder();
            for (var i = 0; i < arr.Length; i++)
            {
                if (i != 0) ret.Append(", ");
                ret.Append(arr[i].ToString());
            }
            return ret.ToString();
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            if (value == null)
                return true;
            int index = 0;
            while (index < value.Length)
            {
                if (char.IsWhiteSpace(value[index]))
                    index++;
                else
                    return false;
            }
            return true;
        }

        public static int[] FromCommaSepString32(string x)
        {
            if (IsNullOrWhiteSpace(x)) return new int[0];
            return x.Split(',').Select(_ => FromString(_.Trim())).ToArray();
        }

        public static short[] FromCommaSepString16(string x)
        {
            if (IsNullOrWhiteSpace(x)) return new short[0];
            return x.Split(',').Select(_ => (short)FromString(_.Trim())).ToArray();
        }

        public static T[] CommaToArray<T>(this string x)
        {
            if (typeof(T) == typeof(ushort))
                return x.Split(',').Select(_ => (T)(object)(ushort)FromString(_.Trim())).ToArray();
            else if (typeof(T) == typeof(string))
                return x.Split(',').Select(_ => (T)(object)_).ToArray();
            else  //assume int
                return x.Split(',').Select(_ => (T)(object)FromString(_.Trim())).ToArray();
        }

        public static byte[] SHA1(string val)
        {
            var sha1 = new SHA1Managed();
            return sha1.ComputeHash(Encoding.UTF8.GetBytes(val));
        }

        public static int ToUnixTimestamp(this DateTime dateTime)
        {
            return (int)(dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }

        public static T Exec<T>(this Task<T> task)
        {
            task.Wait();
            return task.Result;
        }
    }

    public static class StringUtils
    {
        public static bool ContainsIgnoreCase(this string self, string val)
        {
            return self.IndexOf(val, StringComparison.InvariantCultureIgnoreCase) != -1;
        }

        public static bool EqualsIgnoreCase(this string self, string val)
        {
            return self.Equals(val, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
