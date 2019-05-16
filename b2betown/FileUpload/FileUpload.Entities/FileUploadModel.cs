using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileUpload.FileUpload.Entities
{
    [Serializable()]
    public class FileUploadModel
    {

        private int id;
        private string origenalName = String.Empty;
        private string relativepath = String.Empty;
        private string savepath = String.Empty;
        private string extentionname = String.Empty;
        private int type;
        private int objid;
        private int objtype;
        private string creator = String.Empty;
        private string creationip = String.Empty;



        public FileUploadModel() { }




        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }





        public string OrigenalName
        {
            get { return this.origenalName; }
            set { this.origenalName = value; }
        }


        public string Relativepath
        {
            get { return this.relativepath; }
            set { this.relativepath = value; }
        }


        public string Savepath
        {
            get { return this.savepath; }
            set { this.savepath = value; }
        }


        public string Extentionname
        {
            get
            {
                if (String.IsNullOrEmpty(extentionname))
                {
                    this.extentionname = Path.GetExtension(this.OrigenalName);
                }
                return this.extentionname;
            }
            set { this.extentionname = value; }
        }


        public int Type
        {
            get { return this.type; }
            set { this.type = value; }
        }


        public int Objid
        {
            get { return this.objid; }
            set { this.objid = value; }
        }


        public int Objtype
        {
            get { return this.objtype; }
            set { this.objtype = value; }
        }


        public string Creator
        {
            get { return this.creator; }
            set { this.creator = value; }
        }


        public string Creationip
        {
            get { return this.creationip; }
            set { this.creationip = value; }
        }

    }
}
