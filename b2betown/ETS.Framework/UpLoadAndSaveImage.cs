using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ETS.Framework
{
    public class UpLoadAndSaveImage
    {
        public string UpLoadAndSave(byte[] data, ref string virPath, string fext)
        {
            string physicPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~"), @"UploadFile\");

            // 返回文件物理地址，修改虚拟地址 
            if (data == null || virPath == null || fext == null || physicPath == "")
            {
                throw new Exception(" 非法参数");
            }
            string rtnValue = SaveToServer(data, fext, physicPath, data.Length);
            virPath += rtnValue;
            physicPath += rtnValue;
            return physicPath;
        }
        private string CreateFilePath(string fext)
        {
            string filePath = "";
            Random rd = new Random();
            filePath += DateTime.Now.Year.ToString("0000");
            filePath += DateTime.Now.Month.ToString("00");
            filePath += DateTime.Now.Date.ToString("00");
            filePath += DateTime.Now.Hour.ToString("00");
            filePath += DateTime.Now.Minute.ToString("00");
            filePath += DateTime.Now.Second.ToString("00");
            filePath += DateTime.Now.Millisecond.ToString("00");
            filePath += rd.Next(99).ToString("00");
            filePath += fext;
            return filePath;
        }
        private string SaveToServer(byte[] data, string fext, string physicPath, int fileLen)
        {
            string filePath = CreateFilePath(fext);
            string rtnValue = filePath;
            filePath = filePath.Insert(0, @physicPath);
            if (File.Exists(filePath))
            {
                filePath = CreateFilePath(fext);
                rtnValue = filePath;
            }

            FileStream fs = new FileStream(filePath, FileMode.CreateNew);
            fs.Write(data, 0, fileLen);
            fs.Close();
            return rtnValue;
        }

    }
}
