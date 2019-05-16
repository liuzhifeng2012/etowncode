using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ETS2.CRM.Business;
using Newtonsoft.Json;

namespace ETS.JsonFactory
{
    public abstract class JsonBase
    {

        protected HttpRequest request;


        public JsonBase(HttpRequest request)
        {
            this.request = request;
        }

        public abstract string OutputData();

        //protected virtual bool GetPurviewScopeId(out string aryCompanyId, out string aryUserId, string keyWord = "", PurviewScopeType scopeType = PurviewScopeType.All, int companyType = 0, int companyId = 0, string companyAbbr = "", int signType = 0, string url = "", FuncOperation operation = FuncOperation.View, int businessType = 0)
        //{
        //    bool isScope = FuncValidate.GetPurviewScopeId(out aryCompanyId, out aryUserId, keyWord, scopeType, companyType, companyId, companyAbbr, signType: signType, url: this.request.UrlReferrer.PathAndQuery, operation: operation, businessType: businessType);

        //    return isScope;
        //}

        //#region 验证是否有权限操作实体

        ///// <summary>
        ///// 验证是否有操作此用户的权限
        ///// </summary>
        ///// <param name="userId">要验证的用户Id</param>
        ///// <returns>True或False</returns>
        //protected bool ValidateUserModel(int userId)
        //{
        //    return FuncValidate.ValidateUserModel(userId, ViewPurviewScope);
        //}

        ///// <summary>
        ///// 验证是否有操作公司的权限
        ///// </summary>
        ///// <param name="companyId">公司Id</param>
        ///// <returns>True或False</returns>
        //protected bool ValidateCompanyModel(int companyId)
        //{
        //    return FuncValidate.ValidateCompanyModel(companyId, ViewPurviewScope);
        //}

        ///// <summary>
        ///// 验证是否有操作些账户的权限
        ///// </summary>
        ///// <param name="accountId">账户ID</param>
        ///// <returns>True或False</returns>
        //protected bool ValidateAccountModel(int accountId)
        //{
        //    return FuncValidate.ValidateAccountModel(accountId, ViewPurviewScope);
        //}


        //#endregion

        //protected virtual bool GetScopeAccountId(out string aryAccountId, string keyWord = "", VAccountType vaccountType = VAccountType.All,
        //    int companyType = 0, int companyId = 0, string companyAbbr = "", int signType = 0)
        //{
        //    PurviewScopeType scopeType = PurviewScopeType.All;

        //    switch (vaccountType)
        //    {
        //        case VAccountType.Organization:
        //            scopeType = PurviewScopeType.Organization;
        //            break;
        //        case VAccountType.Person:
        //        case VAccountType.Consultant:
        //        case VAccountType.NormalSales:
        //            scopeType = PurviewScopeType.Person;
        //            break;
        //        default:
        //            scopeType = PurviewScopeType.All;
        //            break;
        //    }

        //    aryAccountId = string.Empty;

        //    string aryCompanyId = string.Empty;
        //    string aryUserId = string.Empty;

        //    bool isScope = GetPurviewScopeId(out aryCompanyId, out aryUserId, keyWord, scopeType, companyType, companyId, companyAbbr, signType: signType);

        //    if (isScope == false)
        //        return false;

        //    List<int> accountId = new List<int>();

        //    switch (vaccountType)
        //    {
        //        case VAccountType.Organization:
        //            if (ViewPurviewScope != PurviewScope.ScopeAll && string.IsNullOrEmpty(aryCompanyId))
        //            {
        //                return false;
        //            }
        //            aryAccountId = AccountBusiness.GetScopeAccountId(aryCompanyId, "").TrimEnd(',');
        //            if (aryAccountId.Length == 0)
        //                return false;
        //            break;
        //        case VAccountType.Person:
        //            if (ViewPurviewScope != PurviewScope.ScopeAll && string.IsNullOrEmpty(aryUserId))
        //            {
        //                return false;
        //            }
        //            aryAccountId = AccountBusiness.GetScopeAccountId("", aryUserId).TrimEnd(',');
        //            if (aryAccountId.Length == 0)
        //                return false;
        //            break;
        //        case VAccountType.Consultant:
        //            if (ViewPurviewScope != PurviewScope.ScopeAll && string.IsNullOrEmpty(aryUserId))
        //            {
        //                return false;
        //            }
        //            aryAccountId = AccountBusiness.GetScopeAccountId("", aryUserId, VAccountType.Consultant).TrimEnd(',');
        //            if (aryAccountId.Length == 0)
        //                return false;
        //            break;
        //        case VAccountType.NormalSales:
        //            if (ViewPurviewScope != PurviewScope.ScopeAll && string.IsNullOrEmpty(aryUserId))
        //            {
        //                return false;
        //            }
        //            aryAccountId = AccountBusiness.GetScopeAccountId("", aryUserId, VAccountType.NormalSales).TrimEnd(',');
        //            if (aryAccountId.Length == 0)
        //                return false;
        //            break;
        //        case VAccountType.All:
        //            if (!string.IsNullOrEmpty(aryCompanyId) || !string.IsNullOrEmpty(aryUserId))
        //            {
        //                aryAccountId = AccountBusiness.GetScopeAccountId(aryCompanyId, aryUserId).TrimEnd(',');
        //                if (aryAccountId.Length == 0)
        //                    return false;
        //            }
        //            break;
        //    }

        //    //aryAccountId = string.Join(",", accountId.ToArray());

        //    //aryAccountId = string.Concat(aryAccountId, ",", string.Join(",", aryAdvisorId.ToArray())).TrimEnd(',').TrimStart(',');

        //    return isScope;
        //}

        //private PurviewScope viewPurviewScope;
        //protected PurviewScope ViewPurviewScope
        //{
        //    get
        //    {
        //        if ((int)viewPurviewScope == 0)
        //        {
        //            List<RoleFunc> roleFuncList = FuncValidate.AllFunc(this.request.UrlReferrer.PathAndQuery);

        //            var funcList = from f in roleFuncList
        //                           where f.FuncCode == FuncOperation.View
        //                           select f;

        //            viewPurviewScope = funcList.Min(f => f.PurviewScope);
        //        }
        //        return viewPurviewScope;
        //    }
        //}

        //protected virtual IEnumerable<FuncOperation> GetPageFunc()
        //{
        //    List<RoleFunc> roleFuncList = FuncValidate.AllFunc(this.request.UrlReferrer.PathAndQuery);

        //    return from f in roleFuncList
        //           where f.FuncCode != FuncOperation.View
        //           select f.FuncCode;
        //}


        //protected string ValidateUrl
        //{
        //    get
        //    {
        //        return this.request.UrlReferrer.PathAndQuery;
        //    }
        //}

        //protected virtual bool HasPermission(FuncOperation operation)
        //{
        //    return this.GetPageFunc().Any(f => f == operation);
        //}

        protected string Error
        {
            get
            {
                return JsonConvert.SerializeObject(new { message = 0 });
            }
        }
    }
}
