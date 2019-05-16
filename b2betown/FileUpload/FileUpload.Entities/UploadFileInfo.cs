using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileUpload.FileUpload.Entities
{
    public class UploadFileInfo
    {
        private int objId;
        private int objType;
        public int ObjId
        {
            get { return this.objId; }
            set { this.objId = value; }
        }
        public int ObjType
        {
            get { return this.objType; }
            set { this.objType = value; }
        }
    }
}
