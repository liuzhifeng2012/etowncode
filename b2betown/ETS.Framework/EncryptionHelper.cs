using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Web;

namespace ETS.Framework
{
    public class EncryptionHelper
    {
        public static string ToMD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用UTF-8编码方式把字符串转化为字节数组．
            inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }

        public static string ToMD5ToLower(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用UTF-8编码方式把字符串转化为字节数组．
            inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToLower();
            return retStr;
        }

        public static string ToBase64(string str, string charset)
        {
            byte[] bs = Encoding.GetEncoding(charset).GetBytes(str);
            return Convert.ToBase64String(bs);
        }
        #region des加解密1
        public static string Encrypt(string toEncrypt, string key/*, bool useHashing*/)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray;
                inputByteArray = Encoding.Default.GetBytes(toEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").Substring(0, 8));
                des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").Substring(0, 8));
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                return ret.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static string Decrypt(string toDecrypt, string key/*, bool useHashing*/)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                int len;
                len = toDecrypt.Length / 2;
                byte[] inputByteArray = new byte[len];
                int x, i;
                for (x = 0; x < len; x++)
                {
                    i = Convert.ToInt32(toDecrypt.Substring(x * 2, 2), 16);
                    inputByteArray[x] = (byte)i;
                }
                des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").Substring(0, 8));
                des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").Substring(0, 8));
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.Default.GetString(ms.ToArray());
            }
            catch 
            {
                return "";
            }
        }
        #endregion

        #region des加解密2
        public static string DESEnCode(string pToEncrypt, string sKey)
        {
            try
            {
                pToEncrypt = HttpContext.Current.Server.UrlEncode(pToEncrypt);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.GetEncoding("UTF-8").GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.PKCS7;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:x2}", b);
                }

                return ret.ToString();
            }
            catch
            {
                return "";
            }
        }
        public static string DESDeCode(string pToDecrypt, string sKey)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {

                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.PKCS7;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                return HttpContext.Current.Server.UrlDecode(System.Text.Encoding.GetEncoding("UTF-8").GetString(ms.ToArray()));
            }
            catch
            {
                return "";
            }
        }
        #endregion


        #region 对电子票加密解密, destype=0加密,destype=1解密
        public static string EticketPnoDES(string pno, int destype)
        {
            string key = "KJiI45Hf";//默认加解密秘钥
            if (pno != null && pno != "")
            {
                if (destype == 0)
                {
                    return DESEnCode(pno, key);
                }
                else
                {
                    if (pno.Length > 20)
                    {
                        return DESDeCode(pno, key);
                    }
                }
            }

            return pno;
        }

        #endregion


    }
}
