using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.M
{
    public partial class h5_b2bcrmtequan : System.Web.UI.Page
    {
        public string title = "";

        public string tequan = "";
        public string tishi = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            int comid = Request["comid"].ConvertTo<int>();
            string crmlevel = Request["crmlevel"].ConvertTo<string>();
            string openid = Request["openid"].ConvertTo<string>("");


            if (crmlevel != null)
            {
                B2bcrmlevels mcrmlevel = new B2bcrmlevelsData().Getb2bcrmlevel(comid, crmlevel);
                if (mcrmlevel != null)
                {
                    title = mcrmlevel.levelname;
                    tequan = mcrmlevel.tequan;
 

                    decimal djf_end = mcrmlevel.dengjifen_end;
                    if(openid!="")
                    {
                        B2b_crm mcrm = new B2bCrmData().GetB2bCrmByWeiXin(openid);
                        if(mcrm!=null){
                            decimal mdjf = mcrm.Dengjifen;
                            //得到会员下一级别
                            B2bcrmlevels next_mcrmlevel = new B2bcrmlevelsData().Getb2bcrmlevel(comid, djf_end + 1);
                            if (next_mcrmlevel != null)
                            {
                                if (next_mcrmlevel.isavailable == 0)
                                {
                                    //当前会员级别为最高级别
                                    tishi = "您现在等积分:" + mdjf.ToString("F0") + "分;";
                                }
                                else
                                {
                                    //判断是否是最高级别会员
                                    decimal diff = djf_end - mdjf + 1;
                                    if (diff > 0)
                                    {
                                        tishi = "您现在等积分:" + mdjf.ToString("F0") + "分,还需" + diff.ToString("F0") + "分可以升级为" + next_mcrmlevel.levelname;
                                    }
                                }
                            }
                            else 
                            {
                                //当前会员级别为最高级别
                                tishi = "您现在等积分:" + mdjf.ToString("F0") + "分;";
                            }

                          
                        }


                    }
                }
            }

        }
    }
}