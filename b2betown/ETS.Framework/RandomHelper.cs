using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Framework
{
    public static class RandomHelper
    {
        #region 数字随机数
        /// <summary> 
        /// 数字随机数 
        /// </summary> 
        /// <param name="n">生成长度</param> 
        /// <returns></returns> 
        public static string RandNum(int n)
        {
            char[] arrChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, 9)].ToString());
            }
            return num.ToString();
        }
        #endregion
        #region 数字和字母随机数
        /// <summary> 
        /// 数字和字母随机数 
        /// </summary> 
        /// <param name="n">生成长度</param> 
        /// <returns></returns> 
        public static string RandCode(int n)
        {
            char[] arrChar = new char[]{ 
'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x', 
'0','1','2','3','4','5','6','7','8','9', 
'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z' 
};
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }
        #endregion
        #region 字母随机数
        /// <summary> 
        /// 字母随机数 
        /// </summary> 
        /// <param name="n">生成长度</param> 
        /// <returns></returns> 
        public static string RandLetter(int n)
        {
            char[] arrChar = new char[]{ 
'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x', 
'_', 
'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z' 
};
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }
        #endregion
        #region 日期随机函数
        /// <summary> 
        /// 日期随机函数 
        /// </summary> 
        /// <param name="ra">长度</param> 
        /// <returns></returns> 
        public static string DateRndName(Random ra)
        {
            DateTime d = DateTime.Now;
            string s = null, y, m, dd, h, mm, ss;
            y = d.Year.ToString();
            m = d.Month.ToString();
            if (m.Length < 2) m = "0" + m;
            dd = d.Day.ToString();
            if (dd.Length < 2) dd = "0" + dd;
            h = d.Hour.ToString();
            if (h.Length < 2) h = "0" + h;
            mm = d.Minute.ToString();
            if (mm.Length < 2) mm = "0" + mm;
            ss = d.Second.ToString();
            if (ss.Length < 2) ss = "0" + ss;
            s += y + m + dd + h + mm + ss;
            s += ra.Next(100, 999).ToString();
            return s;
        }
        #endregion
        #region 生成GUID
        /// <summary> 
        /// 生成GUID 
        /// </summary> 
        /// <returns></returns> 
        public static string GetGuid()
        {
            System.Guid g = System.Guid.NewGuid();
            return g.ToString();
        }
        #endregion

         #region 判断是否为单数
        /// <summary> 
        /// 判断是否为单数YesOdd
        /// </summary> 
        /// <returns></returns> 
        public static int YesOdd(int num, int maxnum)
        {
            if (num % 2 == 0)
            {
                return 0;
            }
            else {

                if (maxnum == 0)// 不做判断
                {
                    return 1;
                }
                else {
                    //如果传入最大值，必须是奇数并且的等于最大数才返回 1 否则返回0
                    if (maxnum == num)
                    {
                        return 1;
                    }
                    else {
                        return 0;
                    }
                }


            }
        }
        #endregion



       
    }
}
