using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle.Enum;
using FileUpload.FileUpload.Entities.Enum;
using ETS2.Common.Business;
using FileUpload.FileUpload.Data;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.UI.PMUI
{
    public partial class ProductAdd : System.Web.UI.Page
    {
        public string nowdate = "";//现在日期
        public string monthdate = "";//现在日期+一个月


        public int proid = 0;//产品id
        public int industryid = 0;

        public int projectid = 0;

        protected string headPortraitImgSrc = "/images/defaultThumb.png";

        public int servertype = 1;//服务类型,默认1(电子凭证)

        public int comid = 0;//公司编号

        //产品的导入产品id；产品的已销售数量和可销售数量(如果是导入产品则查询原始产品的已销售数量和可销售数量)
        public int bindingid = 0;
        public int limitbuytotalnum = 0;
        public int buynum = 0;
        public int Source_type = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (UserHelper.ValidateLogin())
            {
                comid = UserHelper.CurrentCompany.ID;
            }

            projectid = Request["projectid"].ConvertTo<int>(0);

            nowdate = DateTime.Now.ToString("yyyy-MM-dd");
            monthdate = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");

            proid = Request["proid"].ConvertTo<int>(0);

            servertype = Request["servertype"].ConvertTo<int>(1);

            if (proid > 0)
            {
                int bindingid = new B2bComProData().GetbindingidbyProid(proid);
                if (bindingid > 0)
                {
                    new B2bComProData().Getsalenum(bindingid, out limitbuytotalnum, out buynum);
                }
                else
                {
                    new B2bComProData().Getsalenum(proid, out limitbuytotalnum, out buynum);
                }
            }

            //产品主图片
            BindHeadPortrait(proid);
            ShowImgBind(proid);

            //产品子级图片(多图片)上传
            BindChildImg(proid);


            //获取所属行业
            B2b_company_manageuser user = UserHelper.CurrentUser();
            B2b_company company = UserHelper.CurrentCompany;
            var comdata = B2bCompanyData.GetCompany(company.ID);
            if (comdata != null)
            {
                industryid = comdata.Com_type;
            }

            //判断公司是否含有项目，不含有的话，添加默认项目
            int count = new B2b_com_projectData().GetProjectCountByComId(company.ID);
            if (count == 0)
            {
                B2b_company_info companyinfo = new B2bCompanyInfoData().GetCompanyInfo(company.ID);
                B2b_company_saleset saleset = B2bCompanySaleSetData.GetDirectSellByComid(company.ID.ToString());

                B2b_com_project model = new B2b_com_project()
                {
                    Id = 0,
                    Projectname = company.Com_name,
                    Projectimg = saleset.Logo.ConvertTo<int>(0),
                    Province = companyinfo.Province.ConvertTo<string>(""),
                    City = companyinfo.City,
                    Industryid = company.Com_type,
                    Briefintroduce = companyinfo.Scenic_intro,
                    Address = companyinfo.Scenic_address,
                    Mobile = companyinfo.Tel,
                    Coordinate = "",
                    Serviceintroduce = companyinfo.Serviceinfo,
                    Onlinestate = "1",
                    Comid = company.ID,
                    Createtime = DateTime.Now,
                    Createuserid = 0
                };
                int result = new B2b_com_projectData().EditProject(model);

                //设置公司下产品的项目id都改为默认项目id
                int result2 = new B2bComProData().UpProjectId(company.ID.ToString(), result);
            }

        }

        private void BindChildImg(int proid)
        {
            childImg.uploadFileInfo.ObjId = proid;
            childImg.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            childImg.IsMultiUpload = true;
            childImg.ViewImgFlag = "GetProChildImg";
        }

        private void BindHeadPortrait(int proid)
        {
            headPortrait.uploadFileInfo.ObjId = proid;
            headPortrait.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            headPortrait.displayImgId = "headPortraitImg";
        }
        private void ShowImgBind(int proid)
        {


            //根据产品id得到 产品信息

            B2b_com_pro proo = new B2bComProData().GetProById(proid.ToString());

            if (proo != null)
            {
                Source_type = proo.Source_type;
                var identityFileUpload = new FileUploadData().GetFileById(proo.Imgurl.ToString().ConvertTo<int>(0));
                if (identityFileUpload != null)
                {
                    headPortraitImgSrc = identityFileUpload.Relativepath;
                }


            }
        }
    }
}