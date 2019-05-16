using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;
using Newtonsoft.Json;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle.Enum;
namespace ETS.JsonFactory
{
    public class RegisterUserJsonData
    {
        #region 编辑注册信息

        /// <summary>
        /// 编辑注册信息 by:xiaoliu
        /// </summary>
        /// <param name="b2b_company"></param>
        /// <param name="B2b_Company_Info"></param>
        /// <param name="manageuser"></param>
        /// <returns></returns>
        public static string InsertOrUpdateRegister(B2b_company b2b_company, B2b_company_info B2b_Company_Info, B2b_company_manageuser manageuser)
        {
            using (var sql = new SqlHelper())
            {

                try
                {
                    //判断登录账户是否存在
                    B2b_company_manageuser model2 = B2bCompanyManagerUserData.GetManageUserByAccount(manageuser.Accounts);
                    if (model2 != null)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "账户已经被注册" });
                    }

                    //判断商家注册公司名称是否存在
                    B2b_company model1 = B2bCompanyManagerUserData.GetB2bCompanyByCompanyName(b2b_company.Com_name);
                    if (model1 != null)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "商家公司已经被注册" });
                    }

                    int result = new InternalB2bCompany(sql).EditB2bCompanyInfo(b2b_company, B2b_Company_Info, manageuser);//编辑注册公司全部信息
                    if (result > 0)
                    {
                        //当注册成功 后，对绑定分销判断,如果有绑定分销直接进行绑定
                        if (b2b_company.Bindingagent != 0) {
                            var bangdinginfo = AgentCompanyData.BindingAgent(result, b2b_company.Bindingagent);//商户和分销绑定

                            var prodata = new B2bCompanyInfoData();
                            var kaitong = prodata.UpComstate(result, "已暂停");//对暂停的 自动开通

                            //分配权限   
                            int createmasterid = 0;
                            string createmastername = "分销开商户自动分配微信负责人(1024)";
                            DateTime createdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            //获取
                            string masterid = B2bCompanyManagerUserData.GetFirstIDUser(result).ToString();


                            string data = PermissionJsonData.EditMasterGroup(masterid, manageuser.Accounts, "1024", createmasterid, createmastername, createdate);
                            
                        }

                        return JsonConvert.SerializeObject(new { type = 100, msg = "商家注册成功",result=result });

                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = "商家注册出现错误" });
                    }

                }
                catch
                {
                    //sql.Rollback();
                    return JsonConvert.SerializeObject(new { type = 1, msg = "商家注册出现意外错误" });
                    throw;
                }
            }

        }
        #endregion
        #region 商家登录
        public static string Login(string username, string pwd)
        {
            string message;
            int userid = 0;

            bool verify = UserHelper.Entry(username, pwd, out message, out userid);
            if (!verify)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "【" + message + "】" });
            }
            UserHelper.SetCookie(userid);
            return JsonConvert.SerializeObject(new { type = 100, msg = "OK" });
        }
        #endregion

        #region 商家登录
        public static string AgentLoginCom(int userid,int comid)
        {



            string message = "";

            B2b_company company = B2bCompanyData.GetCompany(comid);//根据公司id得到公司信息
            if (company == null)
            {
                message = "商家null";
                return JsonConvert.SerializeObject(new { type = 1, msg = message });
            }
            else
            {
                if (company.Com_state == (int)CompanyStatus.InBusiness)
                {

                }
                else
                {
                    message = "商家状态不正确";
                    return JsonConvert.SerializeObject(new { type = 1, msg = message });
                }
            }

            UserHelper.SetCookie(userid);
            return JsonConvert.SerializeObject(new { type = 100, msg = "" });
        }
        #endregion


        #region 忘记密码
        public static string FindPass(string account, string phone, string findway)
        {
            string message;
            string content = "您账户密码重置成功,新密码：$pass$，请登陆后更改密码！";
            int userid = 0;
            if (findway == ""){
                findway="sms";
            }

            //判断登录账户是否存在
            B2b_company_manageuser model2 = B2bCompanyManagerUserData.GetManageUserByAccount(account);
            if (model2 == null) {
                return JsonConvert.SerializeObject(new { type = 1, msg = "账户与手机匹配错误！" });
            }
            if (phone.Trim()==""){
                return JsonConvert.SerializeObject(new { type = 1, msg = "手机信息错误！" });
            }
            if (phone.Trim() != model2.Tel.Trim()) {
                return JsonConvert.SerializeObject(new { type = 1, msg = "账户与手机匹配错误！" });
            }

            if (findway == "sms") { 
                //短信重置密码
                 Random ra = new Random();
                string newPass= ra.Next(26844521, 98946546).ToString();

                var uppass = B2bCompanyManagerUserData.ChangePwd(model2.Id,model2.Passwords,newPass,out message);

                if (uppass == 0) {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "重置密码错误！" });
                }

                content = content.Replace("$pass$", newPass);
                var backContent = SendSmsHelper.SendSms(phone, content, model2.Com_id, out message);
                if (backContent<0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "重置短信发送失败，请重新重置密码！" });
                }

            }

            if (findway == "email")
            {
                //邮件重置密码连接
                //尚未做


            }



            return JsonConvert.SerializeObject(new { type = 100, msg = "密码重置成功" });
        }
        #endregion

    }
}
