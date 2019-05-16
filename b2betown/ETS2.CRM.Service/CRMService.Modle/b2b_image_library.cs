using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    [Serializable()]
    public class b2b_image_library
    {
        private int id;
        private int usetype;
        private int imgurl;
        private int height;
        private int width;
        private string imgurl_address = String.Empty;
        private int modelid;
        private string fonticon;

        public b2b_image_library() { }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public int Modelid
        {
            get { return this.modelid; }
            set { this.modelid = value; }
        }
        public int Usetype
        {
            get { return this.usetype; }
            set { this.usetype = value; }
        }

        public int Imgurl
        {
            get { return this.imgurl; }
            set { this.imgurl = value; }
        }

        public int Height
        {
            get { return this.height; }
            set { this.height = value; }
        }

        public int Width
        {
            get { return this.width; }
            set { this.width = value; }
        }
        public string Imgurl_address
        {
            get { return this.imgurl_address; }
            set { this.imgurl_address = value; }
        }

        public string Fonticon
        {
            get { return this.fonticon; }
            set { this.fonticon = value; }
        }

    }
}
