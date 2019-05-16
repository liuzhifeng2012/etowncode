using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class ApiService
    {
        private int id;
        private string servicername = String.Empty;
        private string password = String.Empty;
        private string deskey = String.Empty;
        private string remarks = String.Empty;
        private bool isrun = true;
        private string organization = String.Empty;

        public ApiService() { }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Organization
        {
            get { return organization; }
            set { organization = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Deskey
        {
            get { return deskey; }
            set { deskey = value; }
        }
        public string Servicername
        {
            get { return servicername; }
            set { servicername = value; }
        }
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }
        public bool Isrun
        {
            get { return isrun; }
            set { isrun = value; }
        }
    }
}
