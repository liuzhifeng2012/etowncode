﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.PermissionUI
{
    public partial class sys_subnavdetail : System.Web.UI.Page
    {
        public int subnavid = 0;//子分栏id

        public int actionid = 0;
        public int columnid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            subnavid = Request["subnavid"].ConvertTo<int>(0);
            actionid = Context.Request["actionid"].ConvertTo<int>(0);
            columnid = Context.Request["columnid"].ConvertTo<int>(0);
        } 
    }
}