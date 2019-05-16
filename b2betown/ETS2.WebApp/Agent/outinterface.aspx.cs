using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS.Framework;

namespace ETS2.WebApp.Agent
{
    public partial class outinterface : System.Web.UI.Page
    {
        public int Agentid = 0;
        public int agent_no = 0;//机构号
        public string deskey = "";//秘钥
        public string agent_updateurl = "";//分销商验证通知url
        public string outinterurl = "http://shop.etown.cn/ticketservice/HttpService.asmx?wsdl";//webservice引用地址:
        public string agentbind_ip = "";//分销商绑定的ip 
        public string inter_sendmethod = "post";//分销商验证通知发送方式
        protected void Page_Load(object sender, EventArgs e)
        {
            Agentid = Request["agentid"].ConvertTo<int>(0);
            //根据机构号获得机构(分销商)信息
            Agent_company agentcompany = new AgentCompanyData().GetAgentCompany(Agentid);
            if (agentcompany != null)
            {
                agent_no = agentcompany.Id + 1000000000;
                deskey = agentcompany.Inter_deskey;
                if (deskey == "")
                {
                    //随机生成6位随机秘钥
                    deskey = CommonFunc.RandCode(8).ToUpper();
                    //把新生成的秘钥录入数据库
                    int r = new AgentCompanyData().UpAgentip(agentcompany.Id,deskey);
                }
                agent_updateurl = agentcompany.Agent_updateurl;
                agentbind_ip = new AgentCompanyData().GetAgentBind_ip(Agentid);
                inter_sendmethod = agentcompany.inter_sendmethod;
            }
        }
    }
}