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
    public partial class ImportMemberCard : System.Web.UI.Page
    {
        public int comid = UserHelper.CurrentCompany.ID;//公司id
        public int imtor = UserHelper.CurrentUserId();


        public int surplusNum = 0;//剩余可用卡号
        public int upNum = 0;//上传次数
        protected void Page_Load(object sender, EventArgs e)
        {
            BindGrid();
        }

        void BindGrid()
        {
            GridView1.DataSource = ExcelSqlHelper.ExecuteReader("select * from Out_MemberCardCodeImLog where comid=" + comid + " order by id desc");
            GridView1.DataBind();

            object o1 = ExcelSqlHelper.ExecuteScalar("select count(1) from Out_MemberCardCodeImLog where comid=" + comid);
            upNum = int.Parse(o1.ToString());

            object o2 = ExcelSqlHelper.ExecuteScalar("select count(1) from Out_MemberCardCode where comid=" + comid + " and isused=0");
            surplusNum = int.Parse(o2.ToString());
        }


        /// <summary>
        /// 上传文件并导入到数据库
        /// </summary>
        protected void Button1_Click(object sender, EventArgs e)
        {

            if (FileUpload1.HasFile)
            {
                int imlogid = 0;
                bool b = Upload(FileUpload1, out imlogid);

                if (b)
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
                                    if (rowCount > 5000)
                                    {

                                        Literal1.Text = "每次导入限制5000条数据";
                                    }
                                    else
                                    {
                                        this.Button1.Enabled = false;
                                        this.Button1.Text = "导入中..";

                                        for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                                        {
                                            IRow row = sheet.GetRow(i);
                                            if (row != null)
                                            {
                                                string outcardcode = GetCellValue(row.GetCell(0));//外部卡号

                                                if (outcardcode.Trim() != "")
                                                {
                                                    //判断卡号是否存在此商户下
                                                    int ishas = new MemberCardData().IsHasOutCardcode(comid, outcardcode);
                                                    if (ishas == 0)
                                                    {
                                                        if (CommonFunc.IsNumber(outcardcode))
                                                        {
                                                            int r = new MemberCardData().InsOutMemberCardcode(outcardcode, 0, comid, imlogid);
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                        this.Button1.Enabled = true;
                                        this.Button1.Text = "导入Excel到数据库";
                                        Literal1.Text = "导入会员卡号成功";
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                    
                        Literal1.Text = "没有数据可用于导入";
                    }
                    fileBytes = null;
                }
                else
                {
                    //错误描述在方法 Upload 已经出现 
                }
                BindGrid();
            }
            else 
            {
                Literal1.Text = "请选择导入文件";
            }
          

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

        #region  已经注释
        ////导入项目列表，不用后删除或者隐藏
        //protected void Button2_Click(object sender, EventArgs e)
        //{

        //    if (FileUpload1.HasFile)
        //    {
        //        byte[] fileBytes = FileUpload1.FileBytes;
        //        if (ExcelRender.HasData(new MemoryStream(fileBytes)))
        //        {
        //            Stream excelFileStream = new MemoryStream(fileBytes);
        //            int sheetIndex = 0;
        //            int headerRowIndex = 0;

        //            using (excelFileStream)
        //            {
        //                using (IWorkbook workbook = new HSSFWorkbook(excelFileStream))
        //                {
        //                    using (ISheet sheet = workbook.GetSheetAt(sheetIndex))
        //                    {
        //                        StringBuilder builder = new StringBuilder();

        //                        IRow headerRow = sheet.GetRow(headerRowIndex);
        //                        int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
        //                        int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

        //                        for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
        //                        {
        //                            IRow row = sheet.GetRow(i);
        //                            if (row != null)
        //                            {
        //                                string name = GetCellValue(row.GetCell(0));//项目名称

        //                                //判断项目中是否有同名的项目，有的话不在录入
        //                                object o = ExcelSqlHelper.ExecuteScalar("select count(1) from b2b_com_project where projectname='" + name + "'");
        //                                int c = o == null ? 0 : int.Parse(o.ToString());
        //                                if (c > 0)
        //                                {
        //                                }
        //                                else
        //                                {
        //                                    B2b_com_project model = new B2b_com_project
        //                                    {
        //                                        Id = 0,
        //                                        Projectname = name,
        //                                        Projectimg = 0,
        //                                        Province = "",
        //                                        City = "",
        //                                        Industryid = 0,
        //                                        Briefintroduce = "",
        //                                        Address = "",
        //                                        Mobile = "",
        //                                        Coordinate = "",
        //                                        Serviceintroduce = "",
        //                                        Onlinestate = "0",
        //                                        Comid = 106,
        //                                        Createuserid = UserHelper.CurrentUser().Id,
        //                                        Createtime = DateTime.Now
        //                                    };

        //                                    int d = new B2b_com_projectData().EditProject(model);

        //                                }
        //                            }
        //                            if (i == rowCount)
        //                            {
        //                                Literal1.Text = "导入完成";
        //                            }

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion

        //上传文件方法
        private bool Upload(System.Web.UI.WebControls.FileUpload myFileUpload, out int imlogid)
        {
            bool flag = false; //是否允许上载 
            bool fileAllow = false; //设定允许上载的扩展文件名类型 
            string[] allowExtensions = { ".xls" }; //取得网站根目录路径
            string path = HttpContext.Current.Request.MapPath("~/upload_outMemberCardcode/");
            if (myFileUpload.HasFile)
            {
                string fileExtension = System.IO.Path.GetExtension(myFileUpload.FileName).ToLower();
                for (int i = 0; i < allowExtensions.Length; i++)
                {
                    if (fileExtension == allowExtensions[i])
                    {
                        fileAllow = true;
                    }
                }
                if (fileAllow)
                {
                    try
                    {
                        //string filename=DateTime.Now.ToString("HHmmssfff")+ myFileUpload.FileName;

                        //string filePath = path + filename; 

                        ////存储文件到文件夹 
                        //myFileUpload.SaveAs(filePath);

                        HttpPostedFile file = myFileUpload.PostedFile;

                        byte[] data = new byte[file.ContentLength];

                        file.InputStream.Read(data, 0, file.ContentLength);

                        string filename = DateTime.Now.ToString("HHmmssfff") + "-" + myFileUpload.FileName;

                        string filePath = path + filename;

                        string relativepath = "/upload_outMemberCardcode/" + filename;

                        FileStream fs = new FileStream(filePath, FileMode.CreateNew);
                        fs.Write(data, 0, data.Length);
                        fs.Close();

                        Literal1.Text = "文件导入成功";
                        flag = true;
                        imlogid = new MemberCardData().Insoutcardcodeimlog(myFileUpload.FileName, relativepath, imtor, comid, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    }
                    catch (Exception ex)
                    {
                        Literal1.Text += ex.Message;
                        flag = false;
                        imlogid = 0;
                    }
                }
                else
                {
                    Literal1.Text = "不允许上载：" + myFileUpload.PostedFile.FileName + "，只能上传xls的文件，请检查！";
                    flag = false;
                    imlogid = 0;
                }
            }
            else
            {
                Literal1.Text = "请选择要导入的excel文件!";
                flag = false;
                imlogid = 0;
            }
            return flag;
        }

    }
}