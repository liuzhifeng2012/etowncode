using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileUpload.FileUpload.Entities;
using FileUpload.FileUpload.Data;
using Newtonsoft.Json;

namespace ETS.JsonFactory
{
    public class FileUploadJsonData
    {

        public static string GetProChildImg(int objId)
        {
            try{
                IList<FileUploadModel> list = new FileUploadData().GetProChildImg(objId);
                return JsonConvert.SerializeObject(new { type = 100,   msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        } 
    }
}
