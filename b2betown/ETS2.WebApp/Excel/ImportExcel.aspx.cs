using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using System.IO;
using System.Data;
using NPOI.SS.UserModel;
using System.Text;
using NPOI.HSSF.UserModel;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;
using ETS.Data.SqlHelper;
using System.Data.SqlClient;
using ETS2.Member.Service.MemberService.Data;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle.Enum;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.Excel
{
    public partial class ImportExcel : System.Web.UI.Page
    {
        public int comid = UserHelper.CurrentCompany.ID;//公司id
        public string comname = UserHelper.CurrentCompany.Com_name;//公司名称
        public bool IsParentCompanyUser = true; //判断操作账户类型(总公司账户;门市账户)
        protected void Page_Load(object sender, EventArgs e)
        {
            int userid = UserHelper.CurrentUserId();
            IsParentCompanyUser = new B2bCompanyManagerUserData().IsParentCompanyUser(userid);
        }
        #region "页面加载中效果"
        /// <summary>
        /// 页面加载中效果
        /// </summary>
        public static void initJavascript()
        {
            HttpContext.Current.Response.Write(" <script language=JavaScript type=text/javascript>");
            HttpContext.Current.Response.Write("var t_id = setInterval(animate,20);");
            HttpContext.Current.Response.Write("var pos=0;var dir=2;var len=0;");
            HttpContext.Current.Response.Write("function animate(){");
            HttpContext.Current.Response.Write("var elem = document.getElementById('progress');");
            HttpContext.Current.Response.Write("if(elem != null) {");
            HttpContext.Current.Response.Write("if (pos==0) len += dir;");
            HttpContext.Current.Response.Write("if (len>32 || pos>79) pos += dir;");
            HttpContext.Current.Response.Write("if (pos>79) len -= dir;");
            HttpContext.Current.Response.Write(" if (pos>79 && len==0) pos=0;");
            HttpContext.Current.Response.Write("elem.style.left = pos;");
            HttpContext.Current.Response.Write("elem.style.width = len;");
            HttpContext.Current.Response.Write("}}");
            HttpContext.Current.Response.Write("function remove_loading() {");
            HttpContext.Current.Response.Write(" this.clearInterval(t_id);");
            HttpContext.Current.Response.Write("var targelem = document.getElementById('loader_container');");
            HttpContext.Current.Response.Write("targelem.style.display='none';");
            HttpContext.Current.Response.Write("targelem.style.visibility='hidden';");
            HttpContext.Current.Response.Write("}");
            HttpContext.Current.Response.Write("</script>");
            HttpContext.Current.Response.Write("<style>");
            HttpContext.Current.Response.Write("#loader_container {text-align:center; position:absolute; top:40%; width:100%; left: 0;}");
            HttpContext.Current.Response.Write("#loader {font-family:Tahoma, Helvetica, sans; font-size:11.5px; color:#000000; background-color:#FFFFFF; padding:10px 0 16px 0; margin:0 auto; display:block; width:130px; border:1px solid #5a667b; text-align:left; z-index:2;}");
            HttpContext.Current.Response.Write("#progress {height:5px; font-size:1px; width:1px; position:relative; top:1px; left:0px; background-color:#8894a8;}");
            HttpContext.Current.Response.Write("#loader_bg {background-color:#e4e7eb; position:relative; top:8px; left:8px; height:7px; width:113px; font-size:1px;}");
            HttpContext.Current.Response.Write("</style>");
            HttpContext.Current.Response.Write("<div id=loader_container>");
            HttpContext.Current.Response.Write("<div id=loader>");
            HttpContext.Current.Response.Write("<div align=center>数据正在导入,请勿关闭页面... </div>");
            HttpContext.Current.Response.Write("<div id=loader_bg><div id=progress> </div></div>");
            HttpContext.Current.Response.Write("</div></div>");
            HttpContext.Current.Response.Flush();
        }
        public static void UnloadJavascript()
        {
            HttpContext.Current.Response.Write(" <script language=JavaScript type=text/javascript>");
            HttpContext.Current.Response.Write("remove_loading();");
            HttpContext.Current.Response.Write(" </script>");
        }
        #endregion
        //private void BindRunCompanyList()
        //{
        //    int totalcount = 0;
        //    string comstate = "1";//公司状态1:正常运行
        //    List<B2b_company> list = new B2bCompanyData().GetAllCompanys(comstate, out totalcount);
        //    ddlcompany.DataSource = list;
        //    ddlcompany.DataValueField = "ID";
        //    ddlcompany.DataTextField = "Com_name";
        //    ddlcompany.DataBind();

        //}
        void BindGrid(int comid, int importnum)
        {
            GridView1.DataSource = ExcelSqlHelper.ExecuteReader("select * from excelimportlog where comid=" + comid + " and importnum=" + importnum);
            GridView1.DataBind();
        }

        public delegate int DBAction(string sql, params IDataParameter[] parameters);
        /// <summary>
        /// 上传文件并导入到数据库
        /// </summary>
        protected void Button1_Click(object sender, EventArgs e)
        {
            initJavascript();
            //comid = int.Parse(ddlcompany.SelectedValue.ToString());

            string dd = "";//页面提示内容

            //根据公司得到会员录入的次数
            int MaxImportNum = new ExcelImportLogData().GetMaxImportNum(comid);

            if (FileUpload1.HasFile)
            {
                byte[] fileBytes = FileUpload1.FileBytes;
                if (ExcelRender.HasData(new MemoryStream(fileBytes)))
                {
                    Stream excelFileStream = new MemoryStream(fileBytes);
                    int sheetIndex = 0;
                    int headerRowIndex = 0;

                    int rowAffected = 0;//录入正确的条数
                    int rowerr = 0;//录入错误的条数
                    using (excelFileStream)
                    {
                        using (IWorkbook workbook = new HSSFWorkbook(excelFileStream))
                        {
                            using (ISheet sheet = workbook.GetSheetAt(sheetIndex))
                            {
                                StringBuilder builder = new StringBuilder();

                                IRow headerRow = sheet.GetRow(headerRowIndex);
                                int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
                                int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1
                                if (rowCount > 1000)
                                {
                                    dd += "<strong>每次导入限制1000条数据</strong>";
                                }
                                else
                                {

                                    for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                                    {
                                        IRow row = sheet.GetRow(i);
                                        if (row != null)
                                        {

                                            int whetherwxfocus = 0;//微信关注状态标注为1(未关注)
                                            int whetheractivate = 0;//用户激活状态标注为0(未激活)

                                            string name = GetCellValue(row.GetCell(0));//姓名
                                            string phone = GetCellValue(row.GetCell(1));//手机号     
                                            string email = GetCellValue(row.GetCell(2));//email
                                            string weixin = GetCellValue(row.GetCell(3));//微信号
                                            if (weixin != "")
                                            {
                                                whetherwxfocus = 1;
                                                whetheractivate = 1;
                                            }
                                            string country = "中国";//国家，都为“中国”
                                            string province = GetCellValue(row.GetCell(4)).ConvertTo<string>("");//城市
                                            string city = GetCellValue(row.GetCell(5)).ConvertTo<string>("");//区县
                                            string address = GetCellValue(row.GetCell(6)).ConvertTo<string>("");//地址
                                            string agegroup = GetCellValue(row.GetCell(7)).ConvertTo<string>("");//年龄段
                                            string crmlevel = GetCellValue(row.GetCell(8)).ConvertTo<string>("A");//会员级别

                                            decimal imprest = GetCellValue(row.GetCell(9)).ConvertTo<decimal>(0);//预付款
                                            decimal integral = GetCellValue(row.GetCell(10)).ConvertTo<decimal>(0);//积分 





                                            int importstate = 1;//录入状态默认1：成功;0:出错
                                            string ErrReason = "";//录入错误原因

                                            bool ishascrmphone = false;
                                            bool ishascrmweixin = false;
                                            if (phone != "")
                                            {
                                                ishascrmphone = new B2bCrmData().IsHasCrmPhone(comid, phone);//判断当前公司会员是否已经绑定当前手机 
                                                if (ishascrmphone == true)
                                                {
                                                    ErrReason = "当前公司已有会员绑定过手机" + phone;

                                                }
                                            }
                                            if (weixin != "")
                                            {
                                                ishascrmweixin = new B2bCrmData().IsHasCrmWeiXin(comid, weixin);//判断当前公司会员是否已经绑定当前微信
                                                if (ishascrmweixin == true)
                                                {
                                                    ErrReason = "当前公司已有会员绑定过微信" + weixin;

                                                }
                                            }
                                            if (ishascrmphone == false && ishascrmweixin == false)
                                            {
                                                if (phone == "" && weixin == "")
                                                {
                                                    ErrReason = "导入会员的手机，微信必须至少有其中一项";
                                                    //会员通过excel录入日志
                                                    importstate = 0;
                                                    int importnum = MaxImportNum + 1;
                                                    string importtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                    int insertlog = new ExcelImportLogData().InsExcelImportLog(importstate, 0, comid, "0", name, phone, weixin, whetherwxfocus, whetheractivate, importtime, importnum, email, ErrReason);
                                                    rowerr++;
                                                }
                                                else
                                                {
                                                    //创建卡号并插入活动
                                                    string cardcode = MemberCardData.CreateECard(2, comid);
                                                    //插入会员表
                                                    int insb2bcrm = new B2bCrmData().ExcelInsB2bCrm(comid, cardcode, name, phone, weixin, whetherwxfocus, whetheractivate, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), email, imprest, integral, country, province, city, address, agegroup, crmlevel);
                                                    int jifen_temp = 0;
                                                    //插入关注赠送优惠券
                                                    var InputMoney = MemberCardData.AutoInputMoeny(insb2bcrm, 4, comid, out jifen_temp);

                                                    //会员通过excel录入日志
                                                    int importnum = MaxImportNum + 1;
                                                    string importtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                    int insertlog = new ExcelImportLogData().InsExcelImportLog(importstate, insb2bcrm, comid, cardcode, name, phone, weixin, whetherwxfocus, whetheractivate, importtime, importnum, email, ErrReason);

                                                    rowAffected++;
                                                }
                                            }
                                            else
                                            {
                                                //会员通过excel录入日志
                                                importstate = 0;
                                                int importnum = MaxImportNum + 1;
                                                string importtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                int insertlog = new ExcelImportLogData().InsExcelImportLog(importstate, 0, comid, "0", name, phone, weixin, whetherwxfocus, whetheractivate, importtime, importnum, email, ErrReason);
                                                rowerr++;
                                            }

                                        }

                                    }
                                }
                            }
                        }
                    }
                    UnloadJavascript();
                    dd += "<strong>成功导入数据共：" + rowAffected.ToString() + "条;</strong>";
                    if (rowerr > 0)
                    {
                        dd += "<strong>错误导入数据共：" + rowerr.ToString() + "条;</strong>";
                    }
                    Literal1.Text = dd;
                }
                else
                {
                    UnloadJavascript();
                    Literal1.Text = "没有数据可用于导入";
                }
                fileBytes = null;
            }
            BindGrid(comid, MaxImportNum + 1);

        }


        /// <summary>
        /// 根据Excel列类型获取列的值
        /// </summary>
        /// <param name="cell">Excel列</param>
        /// <returns></returns>
        private static string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.BLANK:
                    return string.Empty;
                case CellType.BOOLEAN:
                    return cell.BooleanCellValue.ToString();
                case CellType.ERROR:
                    return cell.ErrorCellValue.ToString();
                case CellType.NUMERIC:
                case CellType.Unknown:
                default:
                    return cell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
                case CellType.STRING:
                    return cell.StringCellValue;
                case CellType.FORMULA:
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }
        //导入项目列表，不用后删除或者隐藏
        protected void Button2_Click(object sender, EventArgs e)
        {

            if (FileUpload1.HasFile)
            {
                byte[] fileBytes = FileUpload1.FileBytes;
                if (ExcelRender.HasData(new MemoryStream(fileBytes)))
                {
                    Stream excelFileStream = new MemoryStream(fileBytes);
                    int sheetIndex = 0;
                    int headerRowIndex = 0;

                    using (excelFileStream)
                    {
                        using (IWorkbook workbook = new HSSFWorkbook(excelFileStream))
                        {
                            using (ISheet sheet = workbook.GetSheetAt(sheetIndex))
                            {
                                StringBuilder builder = new StringBuilder();

                                IRow headerRow = sheet.GetRow(headerRowIndex);
                                int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
                                int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

                                for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                                {
                                    IRow row = sheet.GetRow(i);
                                    if (row != null)
                                    {
                                        string name = GetCellValue(row.GetCell(0));//项目名称

                                        //判断项目中是否有同名的项目，有的话不在录入
                                        object o = ExcelSqlHelper.ExecuteScalar("select count(1) from b2b_com_project where projectname='" + name+"'");
                                        int c = o == null ? 0 : int.Parse(o.ToString());
                                        if (c > 0)
                                        {
                                        }
                                        else
                                        {
                                            B2b_com_project model = new B2b_com_project
                                            {
                                                Id = 0,
                                                Projectname = name,
                                                Projectimg = 0,
                                                Province = "",
                                                City = "",
                                                Industryid = 0,
                                                Briefintroduce = "",
                                                Address = "",
                                                Mobile = "",
                                                Coordinate = "",
                                                Serviceintroduce = "",
                                                Onlinestate = "0",
                                                Comid = 106,
                                                Createuserid = UserHelper.CurrentUser().Id,
                                                Createtime = DateTime.Now
                                            };

                                            int d = new B2b_com_projectData().EditProject(model);

                                        }
                                    }
                                    if (i == rowCount)
                                    {
                                        Literal1.Text = "导入完成";
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }
    }
}