using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ETS.Framework
{
    public class TxtHelper
    {
        #region 记录操作日志,这个方法路径还得需要填写，比较麻烦，推荐用LogHelper.cs中的方法
        public static void WriteFile(string pathWrite, string content)
        {
            try
            {
                pathWrite = pathWrite.Trim();
                //pathWrite 需要形式如 D:\\XXX..\\..\\XXX\\XXX.txt 或者 D:\\XXX..\\..\\XXX\\XXX.log  
                string prePath = pathWrite.Substring(0, pathWrite.LastIndexOf("\\"));
                string filename = pathWrite.Substring(pathWrite.LastIndexOf("\\") + 2);
                if (filename.IndexOf(".txt") == -1 && filename.IndexOf(".log") == -1)
                {
                    //日志文件格式不正确
                }
                else
                {
                    if (!Directory.Exists(prePath))//如果日志目录不存在就创建
                    {
                        Directory.CreateDirectory(prePath);
                    }

                    //创建或打开日志文件，向日志文件末尾追加记录
                    StreamWriter mySw = File.AppendText(pathWrite);

                    //向日志文件写入内容
                    string write_content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + content;
                    mySw.WriteLine(write_content);

                    //关闭日志文件
                    mySw.Close();
                }



                //File.AppendAllText(pathWrite, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + content + "\r\n----------------------------------------\r\n",
                //    Encoding.GetEncoding("utf-8"));
            }
            catch { }
        }
        #endregion
        //public static void CheckLog(string LogFile, string Log)
        //{
        //    try
        //    {
        //        if (File.Exists(LogFile))
        //        {
        //            WriteLog(Log, LogFile);
        //        }
        //        else
        //        {
        //            CreateLog(LogFile);
        //            WriteLog(Log, LogFile);
        //        }
        //    }
        //    catch { }
        //}
        //private static void CreateLog(string LogFile)
        //{
        //    StreamWriter SW;
        //    SW = File.CreateText(LogFile);
        //    SW.WriteLine("Log created at: " +
        //                         DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
        //    SW.Close();
        //}
        //private static void WriteLog(string Log, string LogFile)
        //{
        //    using (StreamWriter SW = File.AppendText(LogFile))
        //    {
        //        SW.WriteLine(Log);
        //        SW.Close();
        //    }
        //}
    }
}
