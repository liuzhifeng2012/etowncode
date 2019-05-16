using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ETS.Framework
{
    public class Mjld_TCodeServiceCrypt
    {
        #region TripleDES加密解密
        /// <summary>
        /// TripleDES加密
        /// </summary>
        /// <param name="strEncrypt"></param>
        /// <param name="strKey">strKey 密钥,必须为24位</param>
        /// <returns></returns>
        public static string Encrypt3DESToBase64(string strEncrypt, string strKey)
        {
            return ToBase64(Encrypt3DES(Encoding.UTF8.GetBytes(strEncrypt), strKey));
        }

        /// <summary>
        /// TripleDES解密
        /// </summary>
        /// <param name="strDecrypt"></param>
        /// <param name="strKey">strKey 密钥,必须为24位</param>
        /// <returns></returns>
        public static string Decrypt3DESFromBase64(string strDecrypt, string strKey)
        {
            return Encoding.UTF8.GetString(Decrypt3DES(FromBase64(strDecrypt), strKey));
        }

        #endregion

        #region 私有
        private static byte[] Encrypt3DES(string strEncrypt, string strKey)
        {
            return Encrypt3DES(Encoding.UTF8.GetBytes(strEncrypt), strKey);
        }

        private static byte[] Decrypt3DES(string strDecrypt, string strKey)
        {
            return Decrypt3DES(Encoding.UTF8.GetBytes(strDecrypt), strKey);
        }

        private static byte[] Encrypt3DES(byte[] arrEncrypt, string strKey)
        {
            ICryptoTransform DESEncrypt = null;
            try
            {
                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

                DES.Key = ASCIIEncoding.ASCII.GetBytes(strKey);
                DES.Mode = CipherMode.ECB;

                DESEncrypt = DES.CreateEncryptor();
            }
            catch (System.Exception e)
            {
                return null;
            }

            return DESEncrypt.TransformFinalBlock(arrEncrypt, 0, arrEncrypt.Length);
        }

        private static byte[] Decrypt3DES(byte[] arrDecrypt, string strKey)
        {
            ICryptoTransform DESDecrypt = null;
            try
            {
                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

                DES.Key = ASCIIEncoding.ASCII.GetBytes(strKey);
                DES.Mode = CipherMode.ECB;
                DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

                DESDecrypt = DES.CreateDecryptor();
            }
            catch (System.Exception e)
            {
                return null;
            }

            return DESDecrypt.TransformFinalBlock(arrDecrypt, 0, arrDecrypt.Length);
        }

        //private static byte[] MD5(string str)
        //{
        //    return MD5(Encoding.UTF8.GetBytes(str));
        //}

        //private static byte[] MD5(byte[] str)
        //{
        //    MD5 m = new MD5CryptoServiceProvider();
        //    /*byte[] s = m.ComputeHash(str);
        //    string md5 = BitConverter.ToString(s);   
        //    md5 = md5.Replace("-", "");
        //    md5 = md5.Trim();
        //    return md5;*/
        //    return m.ComputeHash(str);
        //}

        //private static string ToBase64(string str)
        //{
        //    byte[] s = Encoding.UTF8.GetBytes(str);
        //    return Convert.ToBase64String(s);
        //}

        private static string ToBase64(byte[] str)
        {
            return Convert.ToBase64String(str);
        }

        private static byte[] FromBase64(string str)
        {
            return Convert.FromBase64String(str);
        }

        //public static string Utf8ToUnicode(byte[] str)
        //{
        //    byte[] s = Encoding.Convert(Encoding.UTF8, Encoding.Default, str, 0, str.Length);
        //    return Encoding.Default.GetString(s, 0, s.Length);
        //}

        #endregion

    }
}
