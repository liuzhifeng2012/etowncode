﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.MemberUI
{
    public partial class PublishDetail : System.Web.UI.Page
    {
        public string issueid = "0";//发行id
        protected void Page_Load(object sender, EventArgs e)
        {
            issueid = Request["issueid"].ConvertTo<string>("0");
        }
    }
}