using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;
using ETS.Data.SqlHelper;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
     public class Weixin_templateData
    {
         public  Weixin_template GetWeixinTmpl(int comid, string infotype)
         {
             using (var helper = new SqlHelper())
             {
                 Weixin_template m = new InternalWeixin_template(helper).GetWeixinTmpl(comid, infotype);
                 return m;
             }
         }

         public Weixin_template Templatemodelinfo(int id)
         {
             using (var helper = new SqlHelper())
             {
                 Weixin_template basic = new InternalWeixin_template(helper).Templatemodelinfo(id);
                 return basic;
             }
         }

         public int Templatemodeledit(int id, int infotype, string template_name, string first_DATA, string remark_DATA)
         {
             using (var helper = new SqlHelper())
             {
                 int basic = new InternalWeixin_template(helper).Templatemodeledit(id, infotype, template_name, first_DATA, remark_DATA);
                 return basic;
             }
         }

         public int Templateedit(int comid,string id, string template_id)
         {
             using (var helper = new SqlHelper())
             {
                 int basic = new InternalWeixin_template(helper).Templateedit(comid, id, template_id);
                 return basic;
             }
         }

         
         public  List<Weixin_template> Templatemodelpagelist(int pageindex,int pagesize,out int totalcount)
         {
             using (var helper = new SqlHelper())
             {
                 var basic = new InternalWeixin_template(helper).Templatemodelpagelist(pageindex, pagesize, out totalcount);
                 return basic;
             }
         }

         public int Templatecominsert(int comid)
         {
             using (var helper = new SqlHelper())
             {
                 int basic = new InternalWeixin_template(helper).Templatecominsert(comid);
                 return basic;
             }
         }


         public List<Weixin_template> Templatecompagelist(int comid, out int totalcount)
         {
             using (var helper = new SqlHelper())
             {
                 var basic = new InternalWeixin_template(helper).Templatecompagelist(comid, out totalcount);
                 return basic;
             }
         }
         



    }
}
