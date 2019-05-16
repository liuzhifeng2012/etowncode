using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Business;
using Newtonsoft.Json;
using System.Web;
using System.IO;
using System.Collections;
using ETS.Framework;

namespace ETS.JsonFactory
{
    public class UserJosnData : JsonBase
    {

        private string columns;

        public UserJosnData(HttpRequest request)
            : base(request)
        {
            this.columns = request["sColumns"];
        }

        public override string OutputData()
        {
            ParamAnalyser analyser = new ParamAnalyser(this.request.Url.AbsolutePath, '/');
            //绝对路径{""}/json/user/{处理数据标识}
            string action = analyser.Get<string>(3);

            //在路径中获取不同的数据，如：list为分页，edit:为编辑

            switch (action)
            {
                //case "list":
                //    return PageListData();
                //case "edit":
                //    return UserEdit();
                //case "pwd":
                //    return UserPwdEdit();
                //case "editPassWord":
                //    return EditPassWord();
                //case "editPwd":
                //    return EditPwdByUserId();
                //case "exp":
                //    return UserExtendEdit();
                //case "roles":
                //    return GetRoleList();
                //case "insertcontact":
                //    return InsertUserContact();
                //case "deletecontact":
                //    return DeleteUserContact();
                //case "contactlist":
                //    return GetUserContactList();
                //case "homelist":
                //    return HomeListData();
                ////case "query" :
                ////    return QueryUser();
                //case "provider":
                //    return GetProviderList();
                //case "operatorlist":
                //    return OperatorList();
                //case "UpdateUserType":
                //    return UpdateUserType();
                //case "allUserData":
                //    return GetUserAllData();
                //case "accounts":
                //    return GetUserAccounts();
                //case "editemail":
                //    return EditEmail();
                //case "editmobile":
                //    return EditMobilePhone();
                //case "examine":
                //    return ExamineBuyer();
                //case "ValidateLogin":
                //    return ValidateIsLogin();
                //case "getTopData":
                //    return GetTopData();
                //case "getUserBasicInfo":
                //    return GetUserBasicInfo();
                //case "super":
                //    return SuperLogin();
                default:
                    return null;
            }
        }

        //private string SuperLogin()
        //{
        //    string message;
        //    int userId = 0;

        //    var userEmail = request["email"].ConvertTo("");
        //    var userMobile = request["mobile"].ConvertTo("");

        //    bool verify = VerifyUserNoPwd(userEmail, userMobile, out message, out userId);
        //    if (!verify)
        //    {

        //        return JsonConvert.SerializeObject(new { type = 1, msg = "【" + message + "】" });
        //    }

        //    var company = CompanyBusiness.GetCompanyByUserId(userId);

        //    UserHelper.SetCookie(userId);
        //    UserOnlineBusiness.InsertUserDuration(userId);


        //    return JsonConvert.SerializeObject(new { type = 100, msg = message, role = (int)company.Type });
        //}

        //public bool VerifyUserNoPwd(string email, string mobile, out string message, out int userId)
        //{
        //    int organizationStatus;
        //    userId = 0;
        //    var user = UserBusiness.VerifyUserNoPwd(email, mobile, out organizationStatus);
        //    if (user == null)
        //    {
        //        message = "邮件或者手机号码错误";
        //        return false;
        //    }
        //    if (user.Status != CRMService.Module.Enum.UserStatus.Normal || user.AuditStatus == (int)UserAuditStatus.PurchaserRefused)
        //    {
        //        message = "用户状态不可用";
        //        return false;
        //    }

        //    userId = user.Id;

        //    if (user.UserType == TypeUser.Register)
        //    {
        //        var ext = UserBusiness.GetUserRegisterExt(user.Id);

        //        if (ext != null && ext.State == UserRegisterState.UserJoined)
        //        {
        //            var company = CompanyBusiness.GetCompany(user.CompanyId);

        //            if (company.CompanyServiceInfo != null)
        //            {
        //                if (company.CompanyServiceInfo.ServiceState != (int)BusinessStatus.InBusiness ||
        //                    company.EditStatus != AuditStatus.Passed)
        //                {
        //                    message = "机构未开通，无法登录！";
        //                    return false;
        //                }
        //            }
        //            else
        //            {
        //                if (company.BusinessStatus != BusinessStatus.InBusiness ||
        //                    company.EditStatus != AuditStatus.Passed)
        //                {
        //                    message = "机构未开通，无法登录！";
        //                    return false;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Company company = CompanyBusiness.GetCompany(user.CompanyId);

        //        if (company.Type == (int)OrganType.Dept)
        //        {
        //            if (company.Hidden)
        //            {
        //                message = "您所在的部门已经删除，无法登录！";
        //                return false;
        //            }

        //            var operatorCompany = CompanyBusiness.GetParent(company.Id, OrganType.Operator);

        //            if (operatorCompany != null) company = CompanyBusiness.GetCompany(operatorCompany.Id);
        //        }

        //        if (company.CompanyServiceInfo != null)
        //        {
        //            if (company.CompanyServiceInfo.ServiceState != (int)BusinessStatus.InBusiness || company.EditStatus != AuditStatus.Passed)
        //            {
        //                message = "机构未开通，无法登录！";
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            if (company.BusinessStatus != BusinessStatus.InBusiness || company.EditStatus != AuditStatus.Passed)
        //            {
        //                message = "机构未开通，无法登录！";
        //                return false;
        //            }
        //        }

        //    }

        //    message = "";
        //    return true;
        //}
        ///// <summary>
        ///// 获取用户及公司信息
        ///// </summary>
        ///// <returns></returns>
        //private string GetUserBasicInfo()
        //{
        //    try
        //    {
        //        User userInfo = UserHelper.CurrentUser();
        //        Company companyInfo = UserHelper.CurrentCompany;

        //        ETS2.Common.Module.Enum.OrganType drptype = (ETS2.Common.Module.Enum.OrganType)companyInfo.Type;
        //        //companyFullName = companyInfo.FullName;            
        //        //集团名+ 机构类型companyInfo.Name + "/" + drptype.GetDescription() + "/" 
        //        return JsonConvert.SerializeObject(new { msg = "0", companyType = (int)drptype, companyFullName = companyInfo.Name, userName = userInfo.Name });
        //    }
        //    catch (Exception ex)
        //    {
        //        return JsonConvert.SerializeObject(new { msg = "0" });
        //    }
        //}
        ///// <summary>
        ///// //获取头部的数据，包括旅游产品数目，供应商数目，采购商数目
        ///// </summary>
        ///// <returns></returns>
        //private string GetTopData()
        //{
        //    try
        //    {
        //        var Num1 = UserBusiness.GetSystemUserCountByUserType(1);//买家数量
        //        var Num2 = UserBusiness.GetSystemUserCountByUserType(2);//卖家数量
        //        var Num3 = ProductListBusiness.GetProductCountByStatus(0, 0, ProductStatusEnum.Editing, "", "") + ProductListBusiness.GetProductCountByStatus(0, 0, ProductStatusEnum.ExpiredShelves, "", "") + ProductListBusiness.GetProductCountByStatus(0, 0, ProductStatusEnum.OffShelf, "", "") + ProductListBusiness.GetProductCountByStatus(0, 0, ProductStatusEnum.Sales, "", "") + ProductListBusiness.GetProductCountByStatus(0, 0, ProductStatusEnum.Soldshelf, "", "");//商品数量（除“无”，“违规下架”,"删除"外其他所有状态的所有商品数量）

        //        return JsonConvert.SerializeObject(new { msg = "1", purchaserNum = Num1, sellerNum = Num2, productNum = Num3 });
        //    }
        //    catch (Exception ex)
        //    {
        //        return JsonConvert.SerializeObject(new { msg = "0" });
        //    }
        //}
        ///// <summary>
        ///// 验证当前用户是否已经通过验证
        ///// </summary>
        ///// <returns></returns>
        //private string ValidateIsLogin()
        //{
        //    bool isvalidate = UserHelper.ValidateLogin();
        //    if (isvalidate)
        //    {
        //        return JsonConvert.SerializeObject(new { msg = "1" });
        //    }
        //    else
        //    {
        //        return JsonConvert.SerializeObject(new { msg = "0" });
        //    }
        //}

        ////买家审核 author:chenggang
        //private string ExamineBuyer()
        //{
        //    var userId = request["userId"].ConvertTo(0);
        //    var remark = request["remark"].ConvertTo("");
        //    var status = request["status"].ConvertTo("");
        //    var accountId = request["accountId"].ConvertTo("");
        //    var message = "";


        //    var operation = "";
        //    var optContent = "";
        //    var result = 0;
        //    var currentStatus = UserBusiness.GetUser(userId).AuditStatus;
        //    if (status == "pass")
        //    {
        //        if (accountId != "")
        //        {
        //            accountId = accountId.TrimEnd('-');
        //            string[] str = accountId.Split('-');
        //            foreach (var item in str)
        //            {
        //                try
        //                {
        //                    BankBusiness.UpdateAccountStatusById(int.Parse(item), AccountStatus.normal);
        //                }
        //                catch (Exception ex)
        //                {
        //                    message = "更新银行状态失败！" + ex.Message;
        //                }

        //            }

        //        }

        //        if (currentStatus == (int)UserAuditStatus.PurchaserAuditing || currentStatus == (int)UserAuditStatus.PurchaserPassed || currentStatus == (int)UserAuditStatus.PurchaserRefused)
        //        {
        //            operation = ((int)UserAuditStatus.PurchaserPassed).ToString();
        //            optContent = UserAuditStatus.PurchaserPassed.GetDescription();
        //            UserLogs userLogs = GetEntity(userId, remark, operation, optContent);

        //            try
        //            {
        //                result = UserBusiness.UpdateAuditStatusByUserId(userId, UserAuditStatus.PurchaserPassed);
        //                if (result > 0)
        //                {
        //                    UserLogsBusiness.Insert(userLogs);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                message = "更新用户审核状态失败！" + ex.Message;
        //            }



        //        }
        //        else if (currentStatus == (int)UserAuditStatus.BuyerAndSellerAuditing)
        //        {
        //            operation = ((int)UserAuditStatus.BuyerPassAndSellerAuditing).ToString();
        //            optContent = UserAuditStatus.BuyerPassAndSellerAuditing.GetDescription();
        //            UserLogs userLogs = GetEntity(userId, remark, operation, optContent);
        //            try
        //            {
        //                result = UserBusiness.UpdateAuditStatusByUserId(userId, UserAuditStatus.BuyerPassAndSellerAuditing);
        //                if (result > 0)
        //                {
        //                    UserLogsBusiness.Insert(userLogs);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                message = "更新用户审核状态失败！" + ex.Message;
        //            }
        //        }
        //        else
        //        {
        //            message = "当前状态不可以审核！";
        //        }
        //    }
        //    if (status == "refused")
        //    {
        //        if (currentStatus == (int)UserAuditStatus.PurchaserAuditing || currentStatus == (int)UserAuditStatus.PurchaserPassed || currentStatus == (int)UserAuditStatus.PurchaserRefused || currentStatus == (int)UserAuditStatus.BuyerAndSellerAuditing)
        //        {
        //            operation = ((int)UserAuditStatus.PurchaserRefused).ToString();
        //            optContent = UserAuditStatus.PurchaserRefused.GetDescription();
        //            UserLogs userLogs = GetEntity(userId, remark, operation, optContent);

        //            try
        //            {
        //                result = UserBusiness.UpdateAuditStatusByUserId(userId, UserAuditStatus.PurchaserRefused);
        //                if (result > 0)
        //                {
        //                    UserLogsBusiness.Insert(userLogs);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                message = "更新用户审核状态失败！" + ex.Message;
        //            }

        //        }
        //        else
        //        {
        //            message = "当前状态不可以审核！";
        //        }

        //        if (currentStatus == (int)UserAuditStatus.BuyerAndSellerAuditing)
        //        {
        //            var companyId = UserBusiness.GetUser(userId).CompanyId;
        //            try
        //            {
        //                CompanyBusiness.UpdateOrganType(companyId, OrganType.Sales);
        //            }
        //            catch (Exception ex)
        //            {
        //                message = "更新机构类型失败！" + ex.Message;
        //            }

        //        }

        //    }
        //    UserBusiness.ClearAllCash(userId, UserBusiness.GetUser(userId).CompanyId);
        //    return JsonConvert.SerializeObject(new { rt = result, msg = message });
        //}


        //private UserLogs GetEntity(int userId, string remark, string operation, string optContent)
        //{
        //    var userLogs = new UserLogs
        //    {
        //        ObjId = 0,
        //        ObjType = UserLogsType.Users,
        //        Operation = operation,
        //        OptContent = optContent,
        //        Remark = remark,
        //        UserId = userId,
        //        UserIp = CommonFunc.GetRealIP()
        //    };
        //    return userLogs;
        //}



        //private string EditPwdByUserId()
        //{
        //    int id = this.request["id"].ConvertTo<int>(0);
        //    string pwd = this.request["pwd"];
        //    string oldPwd = this.request["oldPwd"];
        //    int result = 0;
        //    if (!string.IsNullOrWhiteSpace(oldPwd))
        //    {
        //        result = UserBusiness.UpdateUserPassword(id, oldPwd.MakeMD5(), pwd.MakeMD5());
        //    }
        //    return JsonConvert.SerializeObject(new { message = result });
        //}

        ////获取所在地全称
        //public static string GetWholeAreas(int areaId)
        //{
        //    List<DictionaryArea> list = DictionaryAreaProxy.GetParentAreaById(areaId);
        //    var wholeArea = "";
        //    foreach (var item in list)
        //    {
        //        wholeArea += item.Name + "-";
        //    }
        //    return wholeArea.TrimEnd('-');
        //}
        //private string GetUserAccounts()
        //{
        //    var userId = request["userId"].ConvertTo(0);
        //    var companyId = 0;
        //    if (userId != 0)
        //    {
        //        companyId = CompanyBusiness.GetCompanyByUserId(userId).Id;
        //    }
        //    else
        //    {
        //        companyId = UserHelper.CurrentCompany.Id;
        //    }

        //    var list = BankBusiness.GetAllAccountByObjId(companyId);
        //    IEnumerable accountModel = null;
        //    if (list != null)
        //    {
        //        accountModel = list.Select(p => new
        //        {
        //            id = p.Id,
        //            bankName = BankBusiness.GetBankById(p.BankId).Name,
        //            accountNumber = p.AccountNumber,
        //            status = p.Status,
        //            isDefault = p.IsDefault
        //        });
        //    }

        //    return JsonConvert.SerializeObject(new { AccountData = accountModel });
        //}

        ///// <summary>
        ///// 获取用户所有信息 author:chenggang
        ///// </summary>
        ///// <returns>用户JSON数据</returns>
        //private string GetUserAllData()
        //{
        //    var userId = request["userId"].ConvertTo(0);
        //    if (userId == 0)
        //    {
        //        userId = UserHelper.CurrentUserId();
        //    }
        //    var user = UserBusiness.GetUserAllData(userId);
        //    var areaId = user.Company.AreaId;
        //    var wholeAreaName = GetWholeAreas(areaId);
        //    var areaModel = DictionaryAreaProxy.GetDictionaryAreaById(areaId);
        //    var AllUserData = new
        //    {
        //        Id = user.Id,
        //        PhotoPath = user.UsersExt.Photo,
        //        Name = user.Name,
        //        Sex = user.UsersExt.Sex,
        //        Department = user.UsersExt.Department,
        //        JobName = user.UsersExt.JobName,
        //        QQ = user.UsersExt.QQ,
        //        Email = user.Email,
        //        MobilePhone = user.Mobile,
        //        CompanyName = user.Company.FullName,
        //        Adress = user.Company.Address,
        //        Zipcode = user.Company.Zipcode,
        //        TelPhone = user.Company.Telphone,
        //        Fax = user.Company.Fax,
        //        AreaId = areaId,
        //        AreaName = areaModel.Name,
        //        WholeAreaName = wholeAreaName,
        //        AuditStatus = ((UserAuditStatus)user.AuditStatus).GetDescription(),
        //        CompanyType = user.Company.Type
        //    };
        //    return JsonConvert.SerializeObject(AllUserData);
        //}

        ////更改手机号码 author:chenggang
        //private string EditMobilePhone()
        //{
        //    var userId = request["userId"].ConvertTo(0);
        //    var mobile = request["mobile"].ConvertTo("");
        //    var result = UserBusiness.EditMobilePhoneByUserId(userId, mobile);
        //    return result.ToString();

        //}

        ////更换邮箱地址 author:chenggang
        //private string EditEmail()
        //{
        //    var userId = 0;
        //    var oldEmail = request["oldEmail"].ConvertTo("");
        //    var newEmail = request["newEmail"].ConvertTo("");
        //    userId = UserBusiness.GetUserIdByEmail(oldEmail);
        //    if (userId > 0)
        //    {
        //        var result = UserBusiness.EditEmailByUserId(userId, newEmail);
        //        return result.ToString();
        //    }
        //    else
        //    {
        //        return "-1";
        //    }
        //}

        //private string UpdateUserType()
        //{
        //    int userId = request["userId"].ConvertTo(0);
        //    int type = request["type"].ConvertTo(0);

        //    if (userId == 0)
        //    {
        //        return "用户id为空！";
        //    }
        //    else
        //    {
        //        var result = UserBusiness.UpdateUserType(userId, type);
        //        return result.ToString();
        //    }
        //}

        //private string OperatorList()
        //{
        //    //operatorId
        //    int operatorId = this.request["operatorId"].ConvertTo<int>(0);
        //    string path = AppSettings.CommonSetting.GetValue("OperatorFile/Path");
        //    string name = getCompany().Id + AppSettings.CommonSetting.GetValue("OperatorFile/NameExt");
        //    if (operatorId > 0)
        //    {
        //        name = operatorId + AppSettings.CommonSetting.GetValue("OperatorFile/NameExt");
        //    }
        //    string defualtName = AppSettings.CommonSetting.GetValue("OperatorFile/DefaultName");
        //    string relativePath = Path.Combine(path, name);
        //    StreamReader sr;
        //    string html = "";
        //    try
        //    {
        //        if (File.Exists(relativePath))
        //        {
        //            sr = new StreamReader(relativePath, Encoding.UTF8);
        //            html = sr.ReadToEnd();
        //        }
        //        else
        //        {
        //            sr = new StreamReader(Path.Combine(path, defualtName), Encoding.UTF8);
        //            html = sr.ReadToEnd();
        //        }
        //        sr.Dispose();
        //        sr.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return JsonConvert.SerializeObject(html);
        //}

        //private Company getCompany()
        //{
        //    var company = UserHelper.CurrentCompany;
        //    if (company.Type == (int)OrganType.Dept)
        //    {
        //        company = CompanyBusiness.GetParent(company.Id, OrganType.Operator);
        //    }
        //    else
        //    {
        //        company = CompanyBusiness.GetParentCompany(company.LeftValue, company.RightValue, OrganType.Operator);
        //    }
        //    return company;
        //}

    
        //private string UserEdit()
        //{
        //    //!2012年4月13日 19:08:44   by:熊德超
        //    // throw new NotImplementedException("在同一方法内验证旧密码");
        //    int id = this.request["id"].ConvertTo<int>(0);
        //    string name = this.request["name"];
        //    string email = this.request["email"].ConvertTo("").Trim();
        //    string mobile = this.request["mobile"];
        //    string pwd = this.request["pwd"];
        //    string officePhone = this.request["officePhone"];
        //    string fax = this.request["fax"];
        //    string ip = this.request["ip"];
        //    int Operator = this.request["Operator"].ConvertTo<int>(0);
        //    int companyId = this.request["companyId"].ConvertTo<int>(0);
        //    string roleIds = this.request["roleIds"];
        //    int office = this.request["office"].ConvertTo<int>(0);
        //    int status = this.request["status"].ConvertTo<int>(0);
        //    string jobNumber = this.request["jobNumber"];
        //    //string oldPwd = this.request["oldPwd"];
        //    //  string region = this.request["region"];

        //    //areaIds areaNames
        //    var areas = new List<UserBusinessArea>();
        //    var areaIds = request["areaIds"].ConvertTo("").Split(',');
        //    var areaNames = request["areaNames"].ConvertTo("").Split(','); ;

        //    for (var i = 0; i < areaIds.Count(); i++)
        //    {
        //        areas.Add(new UserBusinessArea
        //        {
        //            AreaId = areaIds[i].ConvertTo(0),
        //            AreaPath = areaNames[i].ConvertTo("")
        //        });
        //    }


        //    //if (!string.IsNullOrWhiteSpace(oldPwd))
        //    //{
        //    //    //string md5Pwd = UserBusiness.GetP(id);
        //    //    //if (md5Pwd != oldPwd.MakeMD5())
        //    //    //    return "-5";
        //    //}


        //    User model = new User()
        //    {
        //        Id = id,
        //        Name = name,
        //        Email = email,
        //        Mobile = mobile,
        //        Password = pwd != "" ? pwd.MakeMD5() : "",
        //        OfficePhone = officePhone,
        //        Fax = fax,
        //        Integral = 0,
        //        CreateIp = ip,
        //        Creator = Operator,
        //        CompanyId = companyId,
        //        RoleIDs = roleIds.ConvertTo<int>(','), //roleIds != "" ? roleIds.TrimEnd(new char[] { ',' }).Split(new char[] { ',' }).Select(m => Convert.ToInt32(m)).ToArray() : new int[] { },
        //        Office = (OfficeType)office,
        //        Status = (CRMService.Module.Enum.UserStatus)status,
        //        JobNumber = jobNumber,
        //        BusinessAreas = areas,
        //    };
        //    int result = 0;
        //    int accountResult = 1;
        //    try
        //    {
        //        result = UserBusiness.InsertOrUpdateUser(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Write(ex.Message, "CreateUser");
        //    }

        //    if (result > 0)
        //    {
        //        //添加日志
        //        UserLogs logs = new UserLogs();
        //        logs.ObjType = UserLogsType.Users;
        //        logs.ObjId = result;
        //        logs.Operation = (id == 0 ? "添加" : "修改") + "用户";
        //        logs.OptContent = logs.Operation;
        //        logs.Remark = logs.Operation;
        //        logs.UserId = Operator;
        //        logs.UserIp = ip;
        //        UserLogsBusiness.Insert(logs);

        //        //int userId = model.Id > 0 ? model.Id : result;//存储过程里更新返回的是执行行数
        //        ////TODO:这是什么写法？2012年4月15日，增加创建虚拟用户失败的提示和日志 17:57:57   by:熊德超


        //        //不再创建个人虚拟账户，注释下面的代码
        //        //try
        //        //{
        //        //    accountResult = AccountBusiness.InsertOrUpdateAccount(CreateUserAccount(name, userId, companyId, pwd != "" ? pwd.MakeMD5() : ""));

        //        //    if (accountResult <= 0)
        //        //    {
        //        //        LogHelper.Write("创建虚拟用户出错", "CreateVASAccount");
        //        //    }
        //        //    else
        //        //    {
        //        //        //添加日志
        //        //        UserLogs logs = new UserLogs();
        //        //        logs.ObjType = UserLogsType.Users;
        //        //        logs.ObjId = result;
        //        //        logs.Operation = (id == 0 ? "添加" : "修改") + "用户";
        //        //        logs.OptContent = logs.Operation;
        //        //        logs.Remark = logs.Operation;
        //        //        logs.UserId = Operator;
        //        //        logs.UserIp = ip;
        //        //        UserLogsBusiness.Insert(logs);
        //        //    }

        //        //    //if (model.Id <= 0)//添加用户
        //        //    //{
        //        //    //    EQBusiness.AddUser(id, companyId, name, email, pwd.MakeMD5());
        //        //    //}
        //        //    //else
        //        //    //{
        //        //    //    if (!string.IsNullOrEmpty(pwd))//修改密码
        //        //    //    {
        //        //    //        EQBusiness.UpdatePassword(id, pwd, email);
        //        //    //    }
        //        //    //}
        //        //}
        //        //catch (Exception ex)
        //        //{
        //        //    LogHelper.Write(ex.Message, "CreateVASAccount");
        //        //}
        //    }
        //    else
        //    {
        //        LogHelper.Write("创建用户出错", "CreateUser");
        //    }

        //    return JsonConvert.SerializeObject(new { id = result, accountId = accountResult });
        //}

        //#region 返回用户财务帐户
        //private AccountUser CreateUserAccount(string userName, int userId, int companyId, string pwd)
        //{
        //    Company company = CompanyBusiness.GetCompany(companyId);
        //    var user = UserBusiness.GetUser(userId);

        //    AccountUser userAccount = new AccountUser
        //    {
        //        Type = user.UserType == TypeUser.NormalSales ? VASService.Model.Enums.UserType.NormalSales : VASService.Model.Enums.UserType.Person,
        //        Cascading = companyId.ToString(),
        //        Name = userName,
        //        UserName = string.Format(AppSettings.CommonSetting.GetValue("VAccountType/" + VAccountType.Person.ToString()), userId),
        //        ObjectId = userId,
        //        Operator = UserHelper.CurrentUserId(),
        //        OperatorIp = request.UserHostAddress,
        //        OrganNum = company.CompanyAbbr,
        //        OrganType = company.Type == (int)OrganType.Dept ? (int)OrganType.Operator : company.Type,
        //        Pwd = pwd,
        //        AccountGroup = new AccountGroup
        //        {
        //            GroupId = companyId,
        //            IsMaster = false,
        //        }
        //    };

        //    return userAccount;
        //}
        //#endregion

        //private string EditPassWord()
        //{
        //    int userId = request["userId"].ConvertTo(0);
        //    string pwd = request["passWord"].ConvertTo("");

        //    if (string.IsNullOrEmpty(pwd))
        //        return "密码不能为空";

        //    var user = UserBusiness.GetUser(userId);

        //    if (user == null || !ValidateUserModel(user.Id))
        //        return "您没有修改改用户密码的权限！";

        //    var result = 0;
        //    if (user != null)
        //    {
        //        result = UserBusiness.UpdateUserPassword(userId, pwd.MakeMD5());

        //        // EQBusiness.UpdatePassword(userId, pwd.MakeMD5(), user.Email);
        //    }
        //    return result.ToString();
        //}

        //private string UserPwdEdit()
        //{
        //    int id = this.request["id"].ConvertTo<int>(0);

        //    var user = UserBusiness.GetUser(id);

        //    if (user == null || !ValidateUserModel(user.Id))
        //        return Error;

        //    string pwd = this.request["pwd"];
        //    string oldPwd = this.request["oldPwd"];
        //    int result = 0;
        //    if (!string.IsNullOrWhiteSpace(oldPwd))
        //    {
        //        //TODO:Update的时候同时验证旧密码 2012年4月13日 19:02:17   by:熊德超
        //        //throw new NotImplementedException("TODO:Update的时候同时验证旧密码");
        //        //string md5Pwd = UserBusiness.GetP(id);
        //        //if (md5Pwd != oldPwd.MakeMD5())
        //        //    return JsonConvert.SerializeObject(new { message = "-5" });
        //        //else
        //        //{
        //        result = UserBusiness.UpdateUserPassword(id, oldPwd.MakeMD5(), pwd.MakeMD5());


        //        // EQBusiness.UpdatePassword(id, pwd.MakeMD5(), user.Email);

        //        //}
        //    }
        //    return JsonConvert.SerializeObject(new { message = result });
        //}

        //private string UserExtendEdit()
        //{
        //    int userId = this.request["userId"].ConvertTo<int>(0);
        //    int sex = this.request["sex"].ConvertTo<int>(0);
        //    int nationality = this.request["nationality"].ConvertTo<int>(0);
        //    string birthday = this.request["birthday"].ConvertTo("");
        //    int certificateType = this.request["certificateType"].ConvertTo<int>(0);
        //    string certificateCode = this.request["certificateCode"].ConvertTo("");
        //    string certificateFile = this.request["certificateFile"].ConvertTo("");
        //    int politics = this.request["politics"].ConvertTo<int>(0);
        //    int education = this.request["education"].ConvertTo<int>(0);
        //    string domicile = this.request["domicile"].ConvertTo("");
        //    string address = this.request["address"].ConvertTo("");
        //    string post = this.request["post"].ConvertTo("");
        //    string department = this.request["department"].ConvertTo("");
        //    string jobname = this.request["jobname"].ConvertTo("");
        //    string resume = this.request["resume"].ConvertTo("");
        //    string photo = this.request["photo"].ConvertTo("");
        //    string labourContractFile = this.request["labourContractFile"].ConvertTo("");
        //    string labourContractFileIndate = this.request["labourContractFileIndate"];
        //    string labourContractFileRemark = this.request["labourContractFileRemark"].ConvertTo("");
        //    string otherMobile = this.request["otherMobile"].ConvertTo("");
        //    string otherPhone = this.request["otherPhone"].ConvertTo("");
        //    string officeAddress = this.request["officeAddress"].ConvertTo("");
        //    string officePost = this.request["officePost"].ConvertTo("");
        //    string QQ = this.request["QQ"].ConvertTo("");
        //    string MSN = this.request["MSN"].ConvertTo("");
        //    int result = 0;
        //    UsersExt model = new UsersExt()
        //    {
        //        UserID = userId,
        //        Sex = sex,
        //        Nationality = (NationalityType)nationality,
        //        Birthday = birthday != "" ? Convert.ToDateTime(birthday) : Convert.ToDateTime("1900-01-01"),
        //        CertificateType = certificateType,
        //        CertificateCode = certificateCode,
        //        CertificateFile = certificateFile,
        //        CertificateBackFile = "",
        //        Politics = (PoliticsType)politics,
        //        Education = (EducationType)education,
        //        Domicile = domicile,
        //        Address = address,
        //        Post = post,
        //        Department = department,
        //        JobName = jobname,
        //        Resume = resume,
        //        Photo = photo,
        //        LabourContractFile = labourContractFile,
        //        LabourContractFileIndate = labourContractFileIndate != "" ? Convert.ToDateTime(labourContractFileIndate) : Convert.ToDateTime("1900-01-01"),
        //        LabourContractFileRemark = labourContractFileRemark,
        //        OtherMobile = otherMobile,
        //        OtherPhone = otherPhone,
        //        OfficeAddress = officeAddress,
        //        OfficePost = officePost,
        //        QQ = QQ,
        //        MSN = MSN,
        //        IsBindMobile = (int)IsBindMobileStatus.Bind,
        //        IsBindEmail = (int)IsBindEmailStatus.Bind
        //    };

        //    //TODO:把方法从从FileBusiness中移出 2012年4月13日 19:03:02   by:熊德超
        //    //throw new NotImplementedException("把方法从FileBusiness中移出,在UserBusiness中实现");
        //    try
        //    {
        //        result = UserBusiness.UpdateUserExt(model);//FileBusiness.SaveUserWithFile(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Write(ex.Message, "UpdateUserExt");
        //    }
        //    return JsonConvert.SerializeObject(new { message = result });
        //}

        //private string PageListData()
        //{
        //    var organType = this.request["OrganType"].ConvertTo<int>(0);
        //    var keyWord = this.request["keyWord"].ConvertTo("");
        //    var status = this.request["status"].ConvertTo(0);
        //    var operatorId = this.request["operatorId"].ConvertTo(0);
        //    var businessType = this.request["businessType"].ConvertTo(0);

        //    int itemCount;
        //    var users = this.GetUser(out itemCount, organType, keyWord, status, operatorId, businessType);

        //    IEnumerable result = "";
        //    if (users != null)
        //    {
        //        result = from u in users
        //                 select new
        //                 {
        //                     Id = u.Id,
        //                     Name = u.Name,
        //                     Email = u.Email,
        //                     Mobile = u.Mobile,
        //                     OfficePhone = u.OfficePhone,
        //                     CreateTime = u.CreateTime == DateTime.MinValue ? "" : u.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
        //                     UserName = u.UserName ?? string.Empty,
        //                     CompanyName = u.CompanyName ?? string.Empty,
        //                     JobNumber = u.JobNumber ?? string.Empty,
        //                     CompanyId = CompanyBusiness.GetCompany(u.CompanyId).Name,
        //                     Status = u.Status.GetDescription(), //ETS2.CRM.Business.CommonBusiness.GetItemDescription(u.Status),
        //                     Organiztion = new Company { Id = u.CompanyId },
        //                     Operation = "",
        //                 };
        //    }
        //    var jsonData = new JDataTableProperty();

        //    jsonData.sEcho = this.request["sEcho"].ConvertTo<int>();
        //    jsonData.sColumns = this.request["sColumns"];
        //    // jsonData.iTotalRecords = 100;
        //    jsonData.iTotalDisplayRecords = itemCount;
        //    jsonData.aaData = result;
        //    jsonData.iTotalRecords = itemCount;

        //    //jsonData.aaData = "";

        //    return JsonConvert.SerializeObject(jsonData);
        //}

        //private string GetRoleList()
        //{
        //    int cType = this.request["cType"].ConvertTo<int>(0);
        //    IList<Role> roleList = RoleFunBusiness.GetRoleList();
        //    //var selectRole = roleList.Where(m => ((int)m.Type & cType) == (int)m.Type || ((int)m.Type & cType) == cType).ToList();
        //    var selectRole = roleList.Where(m => m.Type == cType).ToList();
        //    return JsonConvert.SerializeObject(selectRole);
        //}


        ////private string SerializerProperty(PropertyInfo property, object obj)
        ////{
        ////    object value = property.GetValue(obj, null);
        ////    return String.Format("\"{0}\":\"{1}\"", property.Name, (value == null || String.IsNullOrEmpty(value.ToString())) ? "null" : value.ToString());
        ////}

        ////private IEnumerable<IEnumerable<KeyValuePair<string,object>>> OutputData(string columns)
        ////{
        ////    var users = this.GetUser();
        ////    foreach (var item in users)
        ////    {
        ////        yield return OutProperty(columns, item);
        ////    }
        ////}


        //private IEnumerable<PropertyInfo> OutProperties(string columns)
        //{
        //    var aryColumns = columns.Split(',');
        //    return from p in typeof(User).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty)
        //           where aryColumns.Contains(p.Name)
        //           select p;
        //}


        //private IList<User> GetUser(out int itemCount, int organType, string keyWord, int status, int operatorId, int businessType)
        //{
        //    var aryCompanyId = "";
        //    var aryUserId = "";

        //    var isScope = true;
        //    if (ViewPurviewScope != PurviewScope.ScopeAll)
        //    {
        //        isScope = GetPurviewScopeId(out aryCompanyId, out aryUserId, "", PurviewScopeType.Person, organType, operatorId, businessType: businessType);

        //    }

        //    if (isScope)
        //    {
        //        var tableUtils = new JTableUtils(this.request);
        //        var listUser = UserBusiness.GetUserPaging(tableUtils.PageIndex,
        //            tableUtils.PageSize,
        //            UserHelper.CurrentUserId(),
        //            tableUtils.OrderType,
        //            tableUtils.OrderField,
        //            keyWord,
        //            0,
        //            aryCompanyId,
        //            (OrganType)organType,
        //            aryUserId,
        //            status,
        //            out itemCount);
        //        return listUser;
        //    }

        //    itemCount = 0;
        //    return null;
        //}

        //private string GetProviderList()
        //{
        //    var keyWord = this.request["keyWord"].ConvertTo("");
        //    var operatorId = this.request["operatorId"].ConvertTo(0);

        //    int itemCount;
        //    var users = this.GetUser(out itemCount, (int)OrganType.Provider, keyWord, (int)CRMService.Module.Enum.UserStatus.Normal, operatorId, 0);
        //    if (users == null)
        //        return "";

        //    var result = from u in users
        //                 select new
        //                 {
        //                     Id = u.Id,
        //                     Name = u.Name,
        //                     Mobile = u.Mobile,
        //                     RegionName = CompanyBusiness.GetParent(u.CompanyId).Name,
        //                     Company = CompanyBusiness.GetCompany(u.CompanyId),
        //                     CityName = AreaBusiness.GetAreaById(CompanyBusiness.GetCompany(u.CompanyId).AreaId),
        //                     BusinessAreaName = GetBusinessAreaName(u.CompanyId),
        //                     Operation = "",
        //                 };

        //    //int itemCount;
        //    var jsonData = new JDataTableProperty();

        //    jsonData.sEcho = this.request["sEcho"].ConvertTo<int>();
        //    jsonData.sColumns = this.request["sColumns"];
        //    // jsonData.iTotalRecords = 100;
        //    jsonData.iTotalDisplayRecords = itemCount;
        //    jsonData.aaData = result;
        //    jsonData.iTotalRecords = itemCount;

        //    //jsonData.aaData = "";

        //    return JsonConvert.SerializeObject(jsonData);
        //}

        //private string GetBusinessAreaName(int companyId)
        //{
        //    var list = AreaBusiness.GetBusinessAreasByCompany(companyId);
        //    var result = "";
        //    foreach (var item in list)
        //    {
        //        result += item.Name + ",";
        //    }
        //    return result;
        //}

        //private string InsertUserContact()
        //{
        //    string ids = this.request["ids"].ConvertTo<string>("");
        //    int result = 0;
        //    foreach (var item in ids.Split(','))
        //    {
        //        if (!string.IsNullOrEmpty(item))
        //        {
        //            UserContact model = new UserContact()
        //            {
        //                UserId = item.ConvertTo<int>(0),
        //                Creator = UserHelper.CurrentUser().Id
        //            };

        //            try
        //            {
        //                result = UserBusiness.InsertOrUpdateUserContact(model);
        //            }
        //            catch (Exception ex)
        //            {
        //                LogHelper.Write(ex.Message, "InsertUserContact");
        //            }
        //        }
        //    }
        //    return JsonConvert.SerializeObject(new { message = result });
        //}

        //private string DeleteUserContact()
        //{
        //    int userId = this.request["userId"].ConvertTo<int>(0);
        //    int result = 0;

        //    try
        //    {
        //        result = UserBusiness.DeleteUserContact(userId, UserHelper.CurrentUser().Id);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Write(ex.Message, "DeleteUserContact");
        //    }
        //    return JsonConvert.SerializeObject(new { message = result });
        //}

        //private string GetUserContactList()
        //{
        //    int status = this.request["status"].ConvertTo<int>(0);
        //    string keyWord = this.request["keyWord"].ConvertTo<string>("");

        //    //string aryCompanyId = "";
        //    string aryUserId = "";

        //    //PurviewScopeBusiness.GetScopeId(out aryCompanyId, out aryUserId, keyWord, PurviewScopeType.Person , (int)OrganType.Operator, 0, 0);

        //    var listUser = UserBusiness.GetUserContactList(aryUserId, UserHelper.CurrentUser().Id);

        //    var result = from u in listUser
        //                 select new
        //                 {
        //                     Id = u.UserId,
        //                     User = UserBusiness.GetUser(u.UserId),
        //                     Company = GetCompany(u.UserId)
        //                 };
        //    return JsonConvert.SerializeObject(result);
        //}

        //private Company GetCompany(int userId)
        //{
        //    return CompanyBusiness.GetCompanyByUserId(userId);
        //}

        //private string HomeListData()
        //{
        //    var keyWord = this.request["keyWord"].ConvertTo("");

        //    int itemCount;
        //    var users = this.GetUser(out itemCount, keyWord);
        //    if (users == null)
        //        return "";

        //    var result = from u in users
        //                 select new
        //                 {
        //                     Id = u.Id,
        //                     Name = u.Name,
        //                     Email = u.Email,
        //                     Mobile = u.Mobile,
        //                     OfficePhone = u.OfficePhone,
        //                     CompanyId = CompanyBusiness.GetCompany(u.CompanyId).Name,
        //                     Status = u.Status.GetDescription(),
        //                     Organiztion = new Company { Id = u.CompanyId },
        //                     Operation = "",
        //                 };

        //    var jsonData = new JDataTableProperty();

        //    jsonData.sEcho = this.request["sEcho"].ConvertTo<int>();
        //    jsonData.sColumns = this.request["sColumns"];
        //    jsonData.iTotalDisplayRecords = itemCount;
        //    jsonData.aaData = result;
        //    jsonData.iTotalRecords = itemCount;

        //    return JsonConvert.SerializeObject(jsonData);
        //}
        //private IList<User> GetUser(out int itemCount, string keyWord)
        //{
        //    var aryCompanyId = "";
        //    var aryUserId = "";

        //    PurviewScopeBusiness.GetScopeId(out aryCompanyId, out aryUserId, keyWord, PurviewScopeType.Person, 1, 0, 0, signType: (int)OrganType.Null);

        //    var tableUtils = new JTableUtils(this.request);
        //    var listUser = UserBusiness.GetUserPaging(tableUtils.PageIndex,
        //        tableUtils.PageSize,
        //        UserHelper.CurrentUserId(),
        //        tableUtils.OrderType,
        //        tableUtils.OrderField,
        //        keyWord,
        //        0,
        //        aryCompanyId,
        //        OrganType.Dept,
        //        aryUserId,
        //        (int)CRMService.Module.Enum.UserStatus.Normal,
        //        out itemCount);
        //    return listUser;

        //}
    }
}
