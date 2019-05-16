﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace ETS2.WeiXin.Service.WinXinService.BLL.WeiXinUploadDown
{

    public class AmrConvertToMP3
    {
        private static object lockobj = new object();
        /// <summary>
        /// 将amr音频转成mp3手机音频
        /// </summary>
        /// <param name="applicationPath">ffmeg.exe文件路径</param>
        /// <param name="fileName">amr文件的路径(带文件名)</param>
        /// <param name="targetFilName">生成目前mp3文件路径（带文件名）</param>
        public string ConvertToMp3(string fileName, string targetFilName)
        {
            //lock (lockobj)
            //{
            //    if (Path.GetExtension(targetFilName).ToLower() != ".mp3")
            //    {
            //        return "";
            //    }
            //    if (File.Exists(targetFilName))
            //    {
            //        File.Delete(targetFilName);
            //    }

            //    string c = @"D:/site/b2betown/ETS2.WebApp/ffmpeg/ffmpeg.exe   -i " + fileName + "   " + targetFilName;

            //    System.Diagnostics.Process process = new System.Diagnostics.Process();
            //    process.StartInfo.FileName = "cmd.exe";
            //    process.StartInfo.Arguments = "/C " + c;//“/C”表示执行完命令后马上退出  
            //    process.StartInfo.UseShellExecute = false;//不使用系统外壳程序启动   
            //    process.StartInfo.RedirectStandardInput = false;//不重定向输入  
            //    process.StartInfo.RedirectStandardOutput = true;//重定向输出  
            //    process.StartInfo.CreateNoWindow = true;//不创建窗口 

            //    string outStr = "";
            //    try
            //    {
            //        process.Start();

            //        process.WaitForExit(100);//等待进程结束，等待时间为指定的毫秒(如果设定为0 或者 不设定，则无限等待)
            //        //outStr = process.StandardOutput.ReadToEnd();
            //    }
            //    catch (Exception ex)
            //    {
            //        //outStr = ex.Message;
            //    }
            //    finally
            //    {
            //        if (process != null)
            //            process.Close();
            //    }
            //    return outStr;
            //}
            return "";
        }

        ///// <summary>
        ///// 获取文件的byte[]
        ///// </summary>
        ///// <param name="fileName"></param>
        ///// <returns></returns>
        //public byte[] GetFileByte(string fileName)
        //{
        //    FileStream pFileStream = null;
        //    byte[] pReadByte = new byte[0];
        //    try
        //    {
        //        pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //        BinaryReader r = new BinaryReader(pFileStream);
        //        r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
        //        pReadByte = r.ReadBytes((int)r.BaseStream.Length);
        //        return pReadByte;
        //    }
        //    catch
        //    {
        //        return pReadByte;
        //    }
        //    finally
        //    {
        //        if (pFileStream != null)
        //            pFileStream.Close();
        //    }
        //}

        ///// <summary>
        ///// 将文件的byte[]生成文件
        ///// </summary>
        ///// <param name="pReadByte"></param>
        ///// <param name="fileName"></param>
        ///// <returns></returns>
        //public bool writeFile(byte[] pReadByte, string fileName)
        //{
        //    FileStream pFileStream = null;
        //    try
        //    {
        //        pFileStream = new FileStream(fileName, FileMode.OpenOrCreate);
        //        pFileStream.Write(pReadByte, 0, pReadByte.Length);
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    finally
        //    {
        //        if (pFileStream != null)
        //            pFileStream.Close();
        //    }
        //    return true;

        //}
    }
}
