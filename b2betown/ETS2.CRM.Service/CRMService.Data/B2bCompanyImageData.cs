using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data.InternalData;
using ETS.Data.SqlHelper;


namespace ETS2.CRM.Service.CRMService.Data
{
    public class B2bCompanyImageData
    {


        public int InsertOrUpdate(B2b_company_image imageset)
        {
            using (var helper = new SqlHelper())
            {

                var id = new InternalB2bCompanyImage(helper).InsertOrUpdate(imageset);

                return id;
            }
        }

        public int UpDownState(int comid, int id,int state)
        {
            using (var helper = new SqlHelper())
            {

                var ret = new InternalB2bCompanyImage(helper).UpDownState(comid, id,state);

                return ret;
            }
        }
        public int UpAllDownState(int comid)
        {
            using (var helper = new SqlHelper())
            {

                var ret = new InternalB2bCompanyImage(helper).UpAllDownState(comid);

                return ret;
            }
        }
        

        public int Deleteimage(int comid, int id, int channelcompanyid=0)
        {
            using (var helper = new SqlHelper())
            {

                var ret = new InternalB2bCompanyImage(helper).Deleteimage(comid, id, channelcompanyid);

                return ret;
            }
        }

        public static B2b_company_image GetimageByComid(int comid, int id, int channelcompanyid=0)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    var salesetinfo = new InternalB2bCompanyImage(helper).GetimageByComid(comid, id, channelcompanyid);

                    return salesetinfo;
                }
                catch
                {
                    return null;
                }
            }
        }



        public List<B2b_company_image> GetimageList(int comid, int typeid, int pageindex, int pagesize, out int totalcount, int channelcompanyid = 0)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyImage(helper);
                    var salesetinfo = internalData.GetimageList(comid, typeid, pageindex, pagesize, out totalcount, channelcompanyid);

                    return salesetinfo;
                }
                catch
                {
                    totalcount = 0;
                    return null;
                }
            }
        }


        public List<B2b_company_image> PageGetimageList(int comid, int typeid,out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyImage(helper);
                    var salesetinfo = internalData.PageGetimageList(comid, typeid, out totalcount);

                    return salesetinfo;
                }
                catch
                {
                    totalcount = 0;
                    return null;
                }
            }
        }


        public List<B2b_company_image> PageChannelGetimageList(int comid, int channelcompanyid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyImage(helper);
                    var salesetinfo = internalData.PageChannelGetimageList(comid, channelcompanyid, out totalcount);

                    return salesetinfo;
                }
                catch
                {
                    totalcount = 0;
                    return null;
                }
            }
        }


        //图库列表
        public List<b2b_image_library> GetimageLibraryList(int usertype, int pageindex, int pagesize, out int totalcount,int modelid=0)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyImage(helper);
                    var salesetinfo = internalData.GetimageLibraryList(usertype, pageindex, pagesize, out totalcount,modelid);

                    return salesetinfo;
                }
                catch
                {
                    totalcount = 0;
                    return null;
                }
            }
        }

        //图标库列表
        public List<b2b_image_library> GetfontLibraryList(int usertype, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyImage(helper);
                    var salesetinfo = internalData.GetfontLibraryList(usertype, pageindex, pagesize, out totalcount);

                    return salesetinfo;
                }
                catch
                {
                    totalcount = 0;
                    return null;
                }
            }
        }

        //根据模板id获取图库列表
        public List<b2b_image_library> GettypemodelidimageLibraryList(int usertype,int modelid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyImage(helper);
                    var salesetinfo = internalData.GettypemodelidimageLibraryList(usertype, modelid, pageindex, pagesize, out totalcount);

                    return salesetinfo;
                }
                catch
                {
                    totalcount = 0;
                    return null;
                }
            }
        }
        
        //获取一张图
        public static b2b_image_library GetimageLibraryByid(int id)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    var salesetinfo = new InternalB2bCompanyImage(helper).GetimageLibraryByid(id);

                    return salesetinfo;
                }
                catch
                {
                    return null;
                }
            }
        }


        public int DeleteLibraryimage(int id)
        {
            using (var helper = new SqlHelper())
            {

                var ret = new InternalB2bCompanyImage(helper).DeleteLibraryimage(id);

                return ret;
            }
        }

        public int LibraryInsertOrUpdate(b2b_image_library imageset)
        {
            using (var helper = new SqlHelper())
            {

                var id = new InternalB2bCompanyImage(helper).LibraryInsertOrUpdate(imageset);

                return id;
            }
        }

        
    }
}
