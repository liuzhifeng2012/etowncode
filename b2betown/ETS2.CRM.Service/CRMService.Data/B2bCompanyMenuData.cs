using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data.InternalData;
using ETS.Data.SqlHelper;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class B2bCompanyMenuData
    {


        public int InsertOrUpdate(B2b_company_menu imageset)
        {
            using (var helper = new SqlHelper())
            {

                var id = new InternalB2bCompanyMenu(helper).InsertOrUpdate(imageset);

                return id;
            }
        }

        public int ButtonInsertOrUpdate(B2b_company_Button imageset)
        {
            using (var helper = new SqlHelper())
            {

                var id = new InternalB2bCompanyMenu(helper).ButtonInsertOrUpdate(imageset);

                return id;
            }
        }

        public int DeleteButton(int comid, int id)
        {
            using (var helper = new SqlHelper())
            {

                var ret = new InternalB2bCompanyMenu(helper).DeleteButton(comid, id);

                return ret;
            }
        }


        public B2b_company_Button GetButtonByComid(int comid,int id)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyMenu(helper);
                    var salesetinfo = internalData.GetButtonByComid(comid, id);

                    return salesetinfo;
                }
                catch
                {
                    return null;
                }
            }
        }


        public int Insertmenu_pro(int comid, int proid,int id)
        {
            using (var helper = new SqlHelper())
            {

                var ret = new InternalB2bCompanyMenu(helper).Insertmenu_pro(comid, proid,id);

                return ret;
            }
        }

        public int InsertConsultant_pro(int comid, int proid, int id)
        {
            using (var helper = new SqlHelper())
            {

                var ret = new InternalB2bCompanyMenu(helper).InsertConsultant_pro(comid, proid, id);

                return ret;
            }
        }

        public int deletemenu_pro(int comid, int id)
        {
            using (var helper = new SqlHelper())
            {

                var ret = new InternalB2bCompanyMenu(helper).deletemenu_pro(comid, id);

                return ret;
            }
        }

        public int deleteConsultant_pro(int comid, int id)
        {
            using (var helper = new SqlHelper())
            {

                var ret = new InternalB2bCompanyMenu(helper).deleteConsultant_pro(comid, id);

                return ret;
            }
        }

        public int selectoucountmenu_pro(int comid, int id)
        {
            using (var helper = new SqlHelper())
            {

                var ret = new InternalB2bCompanyMenu(helper).selectoucountmenu_pro(comid, id);

                return ret;
            }
        }
        public int selectConsultant_projectid(int comid, int id)
        {
            using (var helper = new SqlHelper())
            {

                var ret = new InternalB2bCompanyMenu(helper).selectConsultant_projectid(comid, id);

                return ret;
            }
        }
        public int selectprojceidbychannelid(int comid, int id)
        {
            using (var helper = new SqlHelper())
            {

                var ret = new InternalB2bCompanyMenu(helper).selectprojceidbychannelid(comid, id);

                return ret;
            }
        }
        

        public int Deletemenu(int comid, int id)
        {
            using (var helper = new SqlHelper())
            {

                var ret = new InternalB2bCompanyMenu(helper).Deletemenu(comid, id);

                return ret;
            }
        }

        public static B2b_company_menu GetMenuByComid(int comid, int id)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    var salesetinfo = new InternalB2bCompanyMenu(helper).GetMenuByComid(comid, id);

                    return salesetinfo;
                }
                catch
                {
                    return null;
                }
            }
        }



        public List<B2b_company_menu> GetMenuList(int comid, int pageindex, int pagesize, out int totalcount, int usetype=0,int menuindex=0)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyMenu(helper);
                    var salesetinfo = internalData.GetMenuList(comid, pageindex, pagesize, out totalcount, usetype, menuindex);

                    return salesetinfo;
                }
                catch
                {
                    totalcount = 0;
                    return null;
                }
            }
        }

        public List<B2b_company_Button> GetButtonlist(int comid, int pageindex, int pagesize,int linktype, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyMenu(helper);
                    var salesetinfo = internalData.GetButtonlist(comid, pageindex, pagesize,linktype, out totalcount);

                    return salesetinfo;
                }
                catch
                {
                    totalcount = 0;
                    return null;
                }
            }
        }


        public int SortMenu(string id, int sortid)
        {
            using (var helper = new SqlHelper())
            {
                var ret = new InternalB2bCompanyMenu(helper).SortMenu(id, sortid);
                return ret;
            }
        }


        public int getConsultantidbychannelid(int channaleid)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalB2bCompanyMenu(helper).getConsultantidbychannelid(channaleid);
                return id;
            }
        }


        public int ConsultantInsertOrUpdate(B2b_company_menu imageset)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalB2bCompanyMenu(helper).ConsultantInsertOrUpdate(imageset);
                return id;
            }
        }

        public int DeleteConsultant(int comid, int id)
        {
            using (var helper = new SqlHelper())
            {
                var ret = new InternalB2bCompanyMenu(helper).DeleteConsultant(comid, id);
                return ret;
            }
        }

        public static B2b_company_menu GetConsultantByComid(int comid, int id)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    var salesetinfo = new InternalB2bCompanyMenu(helper).GetConsultantByComid(comid, id);
                    return salesetinfo;
                }
                catch
                {
                    return null;
                }
            }
        }


        public List<B2b_company_menu> GetconsultantList(int comid, int pageindex, int pagesize, out int totalcount,int channelid=0)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyMenu(helper);
                    var salesetinfo = internalData.GetconsultantList(comid, pageindex, pagesize, out totalcount, channelid);
                    return salesetinfo;
                }
                catch
                {
                    totalcount = 0;
                    return null;
                }
            }
        }

        public int SortConsultant(string id, int sortid)
        {
            using (var helper = new SqlHelper())
            {
                var ret = new InternalB2bCompanyMenu(helper).SortConsultant(id, sortid);
                return ret;
            }
        }


    }
}
