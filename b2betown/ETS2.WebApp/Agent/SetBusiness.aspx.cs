using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS.Framework;
using ETS.Data.SqlHelper;
using Newtonsoft.Json;
using ETS2.Common.Business;

namespace ETS2.WebApp.Agent
{
    public partial class SetBusiness : System.Web.UI.Page
    {


        public int Agentid = 0;
        public int loginstate=0;//是否绑定商户
        public int phonestate = 0;//如果未
        public string phone = "";
        public string comname = "";

        protected void Page_Load(object sender, EventArgs e)
        {


            if (Session["Agentid"] != null)
            {
                //账户信息
                Agentid = Int32.Parse(Session["Agentid"].ToString());

                if (Agentid != 0)
                {
                    var com = B2bCompanyData.GetAllComMsgbyAgentid(Agentid);
                    if (com != null)
                    {
                        //返回登陆成功
                        loginstate = 1;
                    }
                    else {

                        var agentinfo = AgentCompanyData.GetAgentByid(Agentid);
                        if (agentinfo != null) {
                            var mobile = agentinfo.Mobile;
                            phone = agentinfo.Mobile;
                            if (mobile != "")
                            {
                                var com_id = B2bCompanyData.GetAllComMsgbyphone(mobile);
                                if (com_id > 0)
                                {
                                    phonestate = com_id;
                                    var cominfo = B2bCompanyData.GetAllComMsg(com_id);
                                    if (cominfo != null) {
                                        comname = cominfo.Com_name;
                                    }

                                }
                                else if (com_id < 0)
                                {
                                    phonestate = com_id;
                                }




                            }
                        }
                    
                    }
                }




            }


        }
    }
}