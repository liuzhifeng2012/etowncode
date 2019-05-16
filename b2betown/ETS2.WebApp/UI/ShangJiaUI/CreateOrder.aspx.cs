using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.UI.ShangJiaUI
{
    public partial class CreateOrder : System.Web.UI.Page
    {
        public string nowdate = "";//购买人出游时间
        public string enddate = "";//购买人默认离开事件


        public decimal childreduce = 0;//儿童减免费用
        public int pro_servertype = 1;//产品服务类型1.票务2.跟团游 8当地游  9酒店客房  10.旅游大巴

        public string pickuppoint = "";//上车地点
        public string dropoffpoint = "";//下车地点

        public int manyspeci = 0;
        public List<B2b_com_pro_Speci> gglist = null;//规格列表

        public Decimal price = 0;
        public decimal face_price = 0;
        public int viewtop = 1; //头部及左侧相关显示控制
        public int iscanbook = 1;//产品是否可以预订
        public int Wrentserver = 0;
        public int issetidcard = 0;//是否 需要填写身份证
        public int isSetVisitDate = 0;//是否需要日期
        public DateTime pro_end;
        public DateTime pro_start;

        protected void Page_Load(object sender, EventArgs e)
        {
            int proid = Request["proid"].ConvertTo<int>(0);
            B2b_com_pro pro = new B2bComProData().GetProById(proid.ToString());
            if (pro != null)
            {
                pro_servertype = pro.Server_type;
                childreduce = pro.Childreduce;
                issetidcard= pro.issetidcard;
                pickuppoint = pro.pickuppoint;
                dropoffpoint = pro.dropoffpoint;
                isSetVisitDate=pro.isSetVisitDate;
                manyspeci = pro.Manyspeci;
                Wrentserver = pro.Wrentserver;

                pro_start = pro.Pro_start;
                pro_end = pro.Pro_end;

                //如果是 云顶旅游大巴的 不显示头部
                //绿野 不显示头部
                if (pro.Com_id == 2553)
                {
                    viewtop = 0;
                }


                //如果多规格读取规格
                if (manyspeci == 1)
                {
                    gglist = new B2b_com_pro_SpeciData().Getgglist(pro.Id);

                }

                price = pro.Advise_price;
                face_price = pro.Face_price;

                //如果含有规格读取规格价格中最低价
                if (manyspeci == 1)
                {
                    if (gglist != null)
                    {
                        price = 0;
                        face_price = 0;
                        for (int i = 0; i < gglist.Count(); i++)
                        {

                            if (price == 0 || price > gglist[i].speci_advise_price)
                            {
                                price = gglist[i].speci_advise_price;
                                face_price = gglist[i].speci_face_price;
                            }
                        }
                    }
                }



                //作废超时未支付订单，完成回滚操作
                int rs = new B2bComProData().CancelOvertimeOrder( pro);
                 
            }

           nowdate = DateTime.Now.ToString("yyyy-MM-dd");
           enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

        }
    }
}