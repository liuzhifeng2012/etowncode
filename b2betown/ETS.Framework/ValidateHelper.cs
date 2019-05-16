using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ETS.Framework
{
    public class ValidateHelper
    {
        public static bool IsEmail(string val)
        {
            if (String.IsNullOrWhiteSpace(val))
            {
                return false;
            }
            return Regex.IsMatch(val, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        public static int Len(string val)
        {
            if (String.IsNullOrEmpty(val))
            {
                return 0;
            }
            var chars = val.ToCharArray();
            int len = 0;
            foreach (var item in chars)
            {
                if (Convert.ToInt64(item) > 0XFF0)
                {
                    len += 2;
                }
                else
                {
                    len++;
                }
            }
            return len;
        }

        public static bool ValidateNum(string num)
        {
            if (string.IsNullOrEmpty(num))
                return false;

            return Regex.IsMatch(num, @"^\d*\$");

        }
    }
}
