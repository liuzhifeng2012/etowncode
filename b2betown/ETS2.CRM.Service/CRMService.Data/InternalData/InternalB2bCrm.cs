using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using System.Data;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model;
using ETS.Framework;
using System.Text.RegularExpressions;
using ETS2.CRM.Service.CRMService.Modle.Enum;


namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class InternalB2bCrm
    {
        private SqlHelper sqlHelper;
        public InternalB2bCrm(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        #region 添加或者编辑公司基本信息

        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateB2bCrm";

        public int InsertOrUpdate(B2b_crm model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Com_id", model.Com_id);
            cmd.AddParam("@Name", model.Name);
            cmd.AddParam("@Sex", model.Sex);
            cmd.AddParam("@Phone", model.Phone);
            cmd.AddParam("@Email", model.Email);
            cmd.AddParam("@Weixin", model.Weixin);
            cmd.AddParam("@Laiyuan", model.Laiyuan);
            cmd.AddParam("@Regidate", model.Regidate);
            cmd.AddParam("@Age", model.Age);
            cmd.AddParam("@IDCard", model.Idcard);
            cmd.AddParam("@crmlevel", model.CrmLevel);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

        #region 用户修改密码
        public string UpMemberpass(int com_id, int id, string pass)
        {
            const string sqlTxt = @"Update [EtownDB].[dbo].[b2b_crm] set password1=@password1 where id=@id and com_id=@Comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@id", id);
            cmd.AddParam("@Comid", com_id);
            cmd.AddParam("@password1", pass);
            cmd.ExecuteNonQuery();
            return "OK";
        }
        #endregion

        #region 用户修改手机
        public string UpMemberphone(int com_id, int id, string phone)
        {
            const string sqlTxt = @"Update [EtownDB].[dbo].[b2b_crm] set phone=@phone where id=@id and com_id=@Comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@id", id);
            cmd.AddParam("@Comid", com_id);
            cmd.AddParam("@phone", phone);
            cmd.ExecuteNonQuery();
            return "OK";
        }
        #endregion

        #region 用户修改邮箱
        public string UpMembermail(int com_id, int id, string mail)
        {
            const string sqlTxt = @"Update [EtownDB].[dbo].[b2b_crm] set email=@email where id=@id and com_id=@Comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@id", id);
            cmd.AddParam("@Comid", com_id);
            cmd.AddParam("@email", mail);
            cmd.ExecuteNonQuery();
            return "OK";
        }
        #endregion

        public string UpMembercard(int com_id, string phone, decimal card)
        {
            string err_msg = "";
            sqlHelper.BeginTrancation();
            try
            {
                string sqlTxt = "Update [EtownDB].[dbo].[b2b_crm] set IDcard='" + card + "' where phone='" + phone + "' and com_id=" + com_id;
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.ExecuteNonQuery();

                var sqltext = " Update Member_Card set  OPENstate=1  where Cardcode='" + card + "' and com_id=" + com_id;//0是未使用，1是已使用
                var cmm = sqlHelper.PrepareTextSqlCommand(sqltext);
                cmm.ExecuteNonQuery();


                string sql6 = "select cardcode from member_channel where com_id=" + com_id + " and mobile='" + phone + "'";
                cmd = sqlHelper.PrepareTextSqlCommand(sql6);
                object o = cmd.ExecuteScalar();


                if (o != null)//如果没有渠道 不要操作
                {
                    if (o.ToString() == "0")
                    {
                        string sql7 = "update member_channel set cardcode='" + card + "' where com_id=" + com_id + " and mobile='" + phone + "'";
                        cmd = sqlHelper.PrepareTextSqlCommand(sql7);
                        cmd.ExecuteNonQuery();
                    }
                }


                sqlHelper.Commit();
            }
            catch (Exception e)
            {
                err_msg = e.Message;
                sqlHelper.Rollback();
            }
            finally
            {
                sqlHelper.Dispose();
            }
            if (err_msg == "")
            {
                return "OK";
            }
            else
            {
                return err_msg;
            }
        }

        #region 用户修改姓名
        public string UpMembername(int com_id, int id, string name)
        {
            const string sqlTxt = @"Update [EtownDB].[dbo].[b2b_crm] set name=@name where id=@id and com_id=@Comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@id", id);
            cmd.AddParam("@Comid", com_id);
            cmd.AddParam("@name", name);
            cmd.ExecuteNonQuery();
            return "OK";
        }
        #endregion

        #region 根据卡号判断有效性
        public string GetCard(string card, int comid)
        {

            const string sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[Crid]
      ,[Cardcode]
      ,[OPENstate]
      ,[OPENsubdate]
      ,[IssueCard]
  FROM [EtownDB].[dbo].[Member_Card] where Cardcode=@card and com_id=@comid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@card", card);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    if (reader.GetValue<int>("Openstate") == 0)
                    {
                        return "OK";
                    }
                    else
                    {
                        return "此卡已经使用";
                    }

                }
                else
                {
                    return "没有此卡号";
                }
            }
        }
        #endregion


        #region 根据渠道卡号判断有效性
        public string GetChannelCard(string card, int comid)
        {

            const string sqlTxt = @"select * from Member_Channel where cardcode=@card and com_id=@comid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@card", card);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return "OK";
                }
                else
                {
                    return "推荐人卡号错误";
                }
            }
        }
        #endregion

        #region 判断邮箱是否注册过
        public string GetEmail(string email, int comid)
        {

            const string sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[email]
  FROM [EtownDB].[dbo].[b2b_crm] where email=@email and com_id=@comid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@email", email);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    return "此邮箱已经注册使用了";
                }
                else
                {

                    return "OK";
                }
            }
        }
        #endregion

        #region  key判断
        public string Getkey(string key)
        {
            string sqltxt = "";
            sqltxt = @"SELECT [id]
      ,[sms_key]
      ,[title]
      ,[Remark]
      ,[openstate]
      ,[subdate]
      ,[ip]
  FROM [EtownDB].[dbo].[member_sms] where sms_key=@sms_key";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@sms_key", key);
            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    return "Key重复错误";
                }
                else
                {

                    return "OK";
                }
            }

        }
        #endregion

        #region 判断邮箱是否注册过
        public string GetPhone(string phone, int comid)
        {

            const string sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[phone]
  FROM [EtownDB].[dbo].[b2b_crm] where phone=@phone and com_id=@comid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@phone", phone);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    return "此手机已经注册";
                }
                else
                {

                    return "OK";
                }
            }
        }
        #endregion


        #region 判断微信号用过
        public bool GetWeixin(string openid, int comid)
        {

            const string sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[phone]
  FROM [EtownDB].[dbo].[b2b_crm] where weixin=@openid and com_id=@comid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@openid", openid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    return true;
                }
                else
                {

                    return false;
                }
            }
        }
        #endregion

        #region 登陆
        public string Login(string account, int accounttype, string pwd, int comid, out B2b_crm userinfo)
        {

            string sqlTxt = "";

            //如果数字格式则不需要查询邮箱，如果包含非数字格式则不能查询卡号
            if (accounttype == 1)
            {
                sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[phone]
,[password1]
,[name]
,[email]
,[regidate]
,[sex]
,[weixin]
,[Idcard]
  FROM [EtownDB].[dbo].[b2b_crm] where com_id=@comid and (phone=@account or idcard=@account) ";
            }
            else
            {
                sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[phone]
,[password1]
,[name]
,[email]
,[regidate]
,[sex]
,[weixin]
,[Idcard]
  FROM [EtownDB].[dbo].[b2b_crm] where com_id=@comid and (phone=@account or email=@account) ";
            }




            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@account", account);
            cmd.AddParam("@password", pwd);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {

                    if (pwd == reader.GetValue<string>("password1"))
                    {
                        userinfo = new B2b_crm
                        {
                            Id = reader.GetValue<int>("Id"),
                            Com_id = reader.GetValue<int>("Com_id"),
                            Password1 = reader.GetValue<string>("Password1"),
                            Email = reader.GetValue<string>("Email"),
                            Name = reader.GetValue<string>("name"),
                            Phone = reader.GetValue<string>("phone"),
                            Regidate = reader.GetValue<DateTime>("regidate"),
                            Sex = reader.GetValue<string>("sex"),
                            Weixin = reader.GetValue<string>("weixin"),
                            Idcard = reader.GetValue<decimal>("Idcard"),
                        };

                        return "OK";
                    }
                    else
                    {
                        userinfo = null;
                        return "账户密码匹配错误!";

                    }
                }
                else
                {
                    userinfo = null;
                    return "没有此账户!";
                }
            }
        }
        #endregion

        #region 微信通过密码登陆，并同时绑定
        public string WeixinPassLogin(string phone, string weixin, string pwd, int comid, out B2b_crm userinfo)
        {

            string sqlTxt = "";
            sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[phone]
,[password1]
,[name]
,[email]
,[regidate]
,[sex]
,[weixin]
,[Idcard]
  FROM [EtownDB].[dbo].[b2b_crm] where com_id=@comid and phone=@phone ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@phone", phone);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (pwd == reader.GetValue<string>("password1"))
                    {
                        userinfo = new B2b_crm
                        {
                            Id = reader.GetValue<int>("Id"),
                            Com_id = reader.GetValue<int>("Com_id"),
                            Password1 = reader.GetValue<string>("Password1"),
                            Email = reader.GetValue<string>("Email"),
                            Name = reader.GetValue<string>("name"),
                            Phone = reader.GetValue<string>("phone"),
                            Regidate = reader.GetValue<DateTime>("regidate"),
                            Sex = reader.GetValue<string>("sex"),
                            Weixin = reader.GetValue<string>("weixin"),
                            Idcard = reader.GetValue<decimal>("Idcard"),

                        };

                        reader.Close();

                        if (userinfo.Weixin == "" || userinfo.Weixin == null)
                        {
                            string sqltext = @"update b2b_crm set weixin='' where weixin=@Weixin";
                            cmd = sqlHelper.PrepareTextSqlCommand(sqltext);
                            cmd.AddParam("@Weixin", weixin);
                            cmd.ExecuteNonQuery();


                            sqltext = @"update b2b_crm set weixin=@Weixin where id=@Id";
                            cmd = sqlHelper.PrepareTextSqlCommand(sqltext);
                            cmd.AddParam("@Id", userinfo.Id);
                            cmd.AddParam("@Weixin", weixin);
                            cmd.ExecuteNonQuery();
                        }

                        return "OK";
                    }
                    else
                    {
                        userinfo = null;
                        return "账户密码匹配错误!";
                    }
                }
                else
                {
                    userinfo = null;
                    return "没有此账户!";
                }
            }
        }
        #endregion


        #region 微信通过coocki登陆，并同时绑定
        public string WeixinCookieLogin(string accountid, string accountmd5, int comid, out B2b_crm userinfo)
        {

            string sqlTxt = "";
            sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[phone]
,[password1]
,[name]
,[email]
,[regidate]
,[sex]
,[weixin]
,[Idcard]
  FROM [EtownDB].[dbo].[b2b_crm] where com_id=@comid and id=@accountid ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@accountid", accountid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    //验证加密信息
                    var md5key = EncryptionHelper.ToMD5(reader.GetValue<decimal>("Idcard").ToString() + reader.GetValue<int>("Id").ToString(), "UTF-8");

                    if (accountmd5 == md5key)
                    {
                        userinfo = new B2b_crm
                        {
                            Id = reader.GetValue<int>("Id"),
                            Com_id = reader.GetValue<int>("Com_id"),
                            Password1 = reader.GetValue<string>("Password1") == null ? "" : reader.GetValue<string>("Password1"),
                            Email = reader.GetValue<string>("Email") == null ? "" : reader.GetValue<string>("Email"),
                            Name = reader.GetValue<string>("name") == null ? "" : reader.GetValue<string>("name"),
                            Phone = reader.GetValue<string>("phone") == null ? "" : reader.GetValue<string>("phone"),
                            Regidate = reader.GetValue<DateTime>("regidate"),
                            Sex = reader.GetValue<string>("sex") == null ? "" : reader.GetValue<string>("sex"),
                            Weixin = reader.GetValue<string>("weixin"),
                            Idcard = reader.GetValue<decimal>("Idcard"),

                        };

                        return "OK";
                    }
                    else
                    {
                        userinfo = null;
                        return "账户密码匹配错误!";
                    }
                }
                else
                {
                    userinfo = null;
                    return "没有此账户!";
                }
            }
        }
        #endregion


        #region 微信直接登陆
        public string WeixinLogin(string openid, string weixinpass1, int comid, out B2b_crm userinfo)
        {

            string sqlTxt = "";
            sqlTxt = @"SELECT *  FROM [b2b_crm] where com_id=@comid and weixin=@openid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@openid", openid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string weinpass_temp = reader.GetValue<string>("weixinpass").Trim();
                    if (weixinpass1.Trim() == weinpass_temp)
                    {
                        userinfo = new B2b_crm
                         {
                             Id = reader.GetValue<int>("Id"),
                             Com_id = reader.GetValue<int>("Com_id"),
                             Password1 = reader.GetValue<string>("Password1"),
                             Email = reader.GetValue<string>("Email"),
                             Name = reader.GetValue<string>("name"),
                             Phone = reader.GetValue<string>("phone"),
                             Regidate = reader.GetValue<DateTime>("regidate"),
                             Sex = reader.GetValue<string>("sex"),
                             Weixin = reader.GetValue<string>("weixin"),
                             Idcard = reader.GetValue<decimal>("Idcard"),
                             Integral = reader.GetValue<decimal>("Integral"),
                             Imprest = reader.GetValue<decimal>("imprest")
                         };
                        return "OK";
                    }
                    else
                    {
                        userinfo = null;
                        return "err2";
                    }
                }
                else
                {
                    userinfo = null;
                    return "没有此账户!";
                }
            }
        }
        #endregion

        #region h5 通过 openid查询用户信息
        public B2b_crm b2b_crmH5(string openid, int comid)
        {

            string sqlTxt = "";
            sqlTxt = @"SELECT *  FROM [EtownDB].[dbo].[b2b_crm] where com_id=@comid and weixin=@openid and not weixin=''";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@openid", openid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_crm u = null;

                while (reader.Read())
                {
                    u = new B2b_crm
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Email = reader.GetValue<string>("Email"),
                        Name = reader.GetValue<string>("name"),
                        Phone = reader.GetValue<string>("phone"),
                        Regidate = reader.GetValue<DateTime>("regidate"),
                        Sex = reader.GetValue<string>("sex"),
                        Age = reader.GetValue<int>("age"),
                        Weixin = reader.GetValue<string>("weixin"),
                        Idcard = reader.GetValue<decimal>("Idcard"),
                        //Cardid = reader.GetValue<int>("Cardid"),
                        //Servercard = reader.GetValue<decimal>("Servercard"),
                        Imprest = reader.GetValue<decimal>("imprest"),
                        Integral = reader.GetValue<decimal>("integral"),
                        Password1 = reader.GetValue<string>("password1"),
                        Birthday = reader.GetValue<DateTime>("Birthday")
                    };
                }
                return u;

            }
        }
        #endregion

        #region 读取用户信息
        internal B2b_crm Readuser(int accountid, int comid)
        {

            const string sqlTxt = @"SELECT a.[id]
      ,a.[com_id]
      ,[phone]
,[password1]
,[name]
,[email]
,[regidate]
,a.[sex]
,[age]
,[weixin]
,[Idcard]
,[imprest]
,[integral]
,Birthday
,b.[Servercard]
,b.id as cardid
,c.headimgurl
,c.nickname
,c.province
,c.city
,c.Sex as wxsex
,a.dengjifen
  FROM  [dbo].[b2b_crm] as a left join Member_Card as b on a.com_id=b.Com_id and a.IDcard=b.Cardcode left join wxmemberbasic as c on a.weixin=c.openid where a.com_id=@comid and a.id=@accountid ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@accountid", accountid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_crm u = null;

                while (reader.Read())
                {
                    u = new B2b_crm
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Email = reader.GetValue<string>("Email"),
                        Name = reader.GetValue<string>("name"),
                        Phone = reader.GetValue<string>("phone"),
                        Regidate = reader.GetValue<DateTime>("regidate"),
                        Sex = reader.GetValue<string>("sex"),
                        Age = reader.GetValue<int>("age"),
                        Weixin = reader.GetValue<string>("weixin"),
                        Idcard = reader.GetValue<decimal>("Idcard"),
                        Cardid = reader.GetValue<int>("Cardid"),
                        Servercard = reader.GetValue<decimal>("Servercard"),
                        Imprest = reader.GetValue<decimal>("imprest"),
                        Integral = reader.GetValue<decimal>("integral"),
                        Password1 = reader.GetValue<string>("password1"),
                        Birthday = reader.GetValue<DateTime>("Birthday"),

                        WxHeadimgurl = reader.GetValue<string>("headimgurl").ConvertTo<string>("/images/defaultThumb.png"),
                        WxNickname = reader.GetValue<string>("nickname").ConvertTo<string>(""),
                        WxProvince = reader.GetValue<string>("province").ConvertTo<string>(""),
                        WxCity = reader.GetValue<string>("city").ConvertTo<string>(""),
                        WxSex = reader.GetValue<int>("wxsex").ToString().ConvertTo<int>(0),

                        Dengjifen = reader.GetValue<decimal>("dengjifen")
                    };
                }
                return u;

            }
        }
        #endregion

        #region 初始用户密码
        public int initialuser(B2b_crm b2binfo)
        {
            const string sqlTxt = @"Update [EtownDB].[dbo].[b2b_crm] set password1=@password1 where id=@id and com_id=@Comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@Id", b2binfo.Id);
            cmd.AddParam("@Comid", b2binfo.Com_id);
            cmd.AddParam("@password1", b2binfo.Password1);
            cmd.ExecuteNonQuery();
            return b2binfo.Id;
        }
        #endregion

        #region 卡号注册于绑定

        private static readonly string SQLInsertOrUpdate1 = "usp_RegCardUpdateB2bCrm";

        public int RegCard(string card, string email, string password1, string name, string phone, int comid, string sex)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate1);
            cmd.AddParam("@Card", card);
            cmd.AddParam("@Email", email);
            cmd.AddParam("@Password1", password1);
            cmd.AddParam("@Name", name);
            cmd.AddParam("@Phone", phone);
            cmd.AddParam("@Comid", comid);
            cmd.AddParam("@Sex", sex);
            cmd.AddParam("@Subdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

        #region 用户注册
        public int UsereRegCard(string cardid, string email, string password1, string name, string phone, int comid, string sex, int channelid)
        {

            //修改发行渠道如果是用户自己注册则标记为注册
            string sqltxt = @"update member_card set issuecard=@channelid where cardcode=@IDcard and com_id=@com_id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@com_id", comid);
            cmd.AddParam("@IDcard", cardid);
            cmd.AddParam("@channelid", channelid);
            cmd.ExecuteNonQuery();


            sqltxt = @"INSERT INTO b2b_crm(com_id,password1,name,sex,phone,email,regidate,IDcard)
                  VALUES(@com_id,@password1,@name,@sex,@phone,@email,@regidate,@IDcard);select @@identity;";
            cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@com_id", comid);
            cmd.AddParam("@password1", password1);
            cmd.AddParam("@name", name);
            cmd.AddParam("@sex", sex);
            cmd.AddParam("@phone", phone);
            cmd.AddParam("@email", email);
            cmd.AddParam("@regidate", DateTime.Now);
            cmd.AddParam("@IDcard", cardid);
            object o = cmd.ExecuteScalar();
            int newId = o == null ? 0 : int.Parse(o.ToString());
            return newId;
            //return cmd.ExecuteNonQuery();

        }
        #endregion

        #region 修改会员信息

        public int UpMember(B2b_crm b2binfo)
        {
            const string sqlTxt = @"Update [EtownDB].[dbo].[b2b_crm] set email=@Email,name=@Name,phone=@Phone,Sex=@Sex,Age=@Age where id=@id and com_id=@Comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@Id", b2binfo.Id);
            cmd.AddParam("@Email", b2binfo.Email);
            cmd.AddParam("@Name", b2binfo.Name);
            cmd.AddParam("@Phone", b2binfo.Phone);
            cmd.AddParam("@Comid", b2binfo.Com_id);
            cmd.AddParam("@Sex", b2binfo.Sex);
            cmd.AddParam("@Age", b2binfo.Age);
            cmd.ExecuteNonQuery();
            return b2binfo.Id;
        }
        #endregion

        #region 微信编辑会员信息

        public int weiUpMember(B2b_crm b2binfo)
        {
            const string sqlTxt = @"Update [EtownDB].[dbo].[b2b_crm] set name=@Name,phone=@Phone,Sex=@Sex,Birthday=@Birthday where IDcard=@IDcard and com_id=@Comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@IDcard", b2binfo.Idcard);
            cmd.AddParam("@Name", b2binfo.Name);
            cmd.AddParam("@Phone", b2binfo.Phone);
            cmd.AddParam("@Comid", b2binfo.Com_id);
            cmd.AddParam("@Sex", b2binfo.Sex);
            cmd.AddParam("@Birthday", b2binfo.Birthday);

            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region 解绑手机

        public int UserUnlockPhone(string openid, int comid)
        {
            const string sqlTxt = @"Update [EtownDB].[dbo].[b2b_crm] set phone='' where weixin=@openid and com_id=@Comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@openid", openid);
            cmd.AddParam("@Comid", comid);


            return cmd.ExecuteNonQuery();
        }
        #endregion
        #region 微信绑卡
        private static readonly string SQLInsertOrUpdate2 = "usp_RegWeixinCardUpdateB2bCrm";
        public int WeixinRegCard(string card, string openid, string password1, string name, string phone, int comid, int channelid)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate2);
            cmd.AddParam("@Card", card);
            cmd.AddParam("@Openid", openid);
            cmd.AddParam("@Password1", password1);
            cmd.AddParam("@Name", name);
            cmd.AddParam("@Phone", phone);
            cmd.AddParam("@Comid", comid);
            cmd.AddParam("@Channelid", channelid);
            cmd.AddParam("@Subdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

        #region 微信密码设定
        private static readonly string SQLInsertOrUpdate4 = "usp_WeixinSetPassUpdateB2bCrm";
        public int WeixinSetPass(string openid, string password1, int comid)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate4);
            cmd.AddParam("@Openid", openid);
            cmd.AddParam("@Comid", comid);
            cmd.AddParam("@Password1", password1);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

        #region 得到微信密码
        public string WeixinGetPass(string openid, int comid)
        {
            const string sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[name]
      ,[sex]
      ,[phone]
      ,[email]
      ,[weixin]
      ,[weixinpass]
      ,[laiyuan]
      ,[regidate]
      ,[age]
  FROM [EtownDB].[dbo].[b2b_crm] where weixin=@openid and com_id=@comid and not weixin=''";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@openid", openid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<string>("weixinpass");
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        #region 注销微信密码
        public string WeixinConPass(string openid, int comid)
        {
            const string sqlTxt = @"update [b2b_crm] set weixinpass='' where weixin=@openid and com_id=@comid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@openid", openid);
            cmd.AddParam("@comid", comid);
            cmd.ExecuteReader();
            return "";

        }
        #endregion

        #region 手机注册账户
        private static readonly string SQLInsertOrUpdate3 = "usp_RegAccountUpdateB2bCrm";
        public int RegAccount(string card, string phone, string name, string password1, int comid, int channelid)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate3);
            cmd.AddParam("@Card", card);
            cmd.AddParam("@Password1", password1);
            cmd.AddParam("@Name", name);
            cmd.AddParam("@Phone", phone);
            cmd.AddParam("@Comid", comid);
            cmd.AddParam("@Channelid", channelid);
            cmd.AddParam("@Subdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

        internal List<B2b_crm> SJKeHuPageList(string comid, int pageindex, int pagesize, out int totalcount)
        {



            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            var condition = "com_id=" + comid;


            cmd.PagingCommand1("wxmemberbasic a right join b2b_crm b on a.openid=b.weixin", "b.*,a.headimgurl,a.nickname,a.province,a.city,a.sex as wxsex", "b.id desc", "", pagesize, pageindex, "", condition);


            List<B2b_crm> list = new List<B2b_crm>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_crm
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Age = reader.GetValue<int>("Age").ToString().ConvertTo<int>(0),
                        Email = reader.GetValue<string>("Email").ConvertTo<string>(""),
                        Laiyuan = reader.GetValue<string>("Laiyuan").ConvertTo<string>(""),
                        Name = reader.GetValue<string>("Name"),
                        Phone = reader.GetValue<string>("Phone"),
                        Regidate = reader.GetValue<DateTime>("Regidate"),
                        Sex = reader.GetValue<string>("Sex"),
                        Weixin = reader.GetValue<string>("Weixin").ConvertTo<string>(""),
                        Imprest = reader.GetValue<decimal>("Imprest"),
                        Integral = reader.GetValue<decimal>("Integral"),
                        Idcard = reader.GetValue<decimal>("Idcard"),


                        WxHeadimgurl = reader.GetValue<string>("headimgurl").ConvertTo<string>("/images/defaultThumb.png"),
                        WxNickname = reader.GetValue<string>("nickname").ConvertTo<string>(""),
                        WxProvince = reader.GetValue<string>("province").ConvertTo<string>(""),
                        WxCity = reader.GetValue<string>("city").ConvertTo<string>(""),
                        WxSex = reader.GetValue<int>("wxsex").ToString().ConvertTo<int>(0),


                        Whetherwxfocus = reader.GetValue<bool>("whetherwxfocus"),
                        Whetheractivate = reader.GetValue<bool>("whetheractivate")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }
        internal List<B2b_crm> SJKeHuPageList(string comid, int pageindex, int pagesize, B2b_company_manageuser user, out int totalcount)
        {


            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            var condition = "b.idcard in (select cardcode from member_card where issuecard in (select id from member_channel where companyid  =" + user.Channelcompanyid + "))";



            cmd.PagingCommand1("wxmemberbasic a right join b2b_crm b on a.openid=b.weixin", "b.*,a.headimgurl,a.nickname,a.province,a.city,a.sex as wxsex", "b.id desc", "", pagesize, pageindex, "", condition);

            //var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            //var tblName = "b2b_crm";
            //var strGetFields = "*";
            //var sortKey = "id";
            //var sortMode = "1";
            //var condition = "idcard in (select cardcode from member_card where issuecard in (select id from member_channel where companyid  =" + user.Channelcompanyid + "))";


            //cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_crm> list = new List<B2b_crm>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_crm
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Age = reader.GetValue<int>("Age").ToString().ConvertTo<int>(0),
                        Email = reader.GetValue<string>("Email").ConvertTo<string>(""),
                        Laiyuan = reader.GetValue<string>("Laiyuan").ConvertTo<string>(""),
                        Name = reader.GetValue<string>("Name"),
                        Phone = reader.GetValue<string>("Phone"),
                        Regidate = reader.GetValue<DateTime>("Regidate").ToString("yyyy-MM-dd HH:mm:ss").ConvertTo<DateTime>(),
                        Sex = reader.GetValue<string>("Sex"),
                        Weixin = reader.GetValue<string>("Weixin").ConvertTo<string>(""),
                        Imprest = reader.GetValue<decimal>("Imprest"),
                        Integral = reader.GetValue<decimal>("Integral"),
                        Idcard = reader.GetValue<decimal>("Idcard"),

                        WxHeadimgurl = reader.GetValue<string>("headimgurl").ConvertTo<string>("/images/defaultThumb.png"),
                        WxNickname = reader.GetValue<string>("nickname").ConvertTo<string>(""),
                        WxProvince = reader.GetValue<string>("province").ConvertTo<string>(""),
                        WxCity = reader.GetValue<string>("city").ConvertTo<string>(""),
                        WxSex = reader.GetValue<int>("wxsex").ToString().ConvertTo<int>(0),


                        Whetherwxfocus = reader.GetValue<bool>("whetherwxfocus"),
                        Whetheractivate = reader.GetValue<bool>("whetheractivate")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }
        internal List<B2b_crm> fuwuPageList(string comid, int pageindex, int pagesize, int uesr, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_crm left join Member_Card  on b2b_crm.IDcard =Member_Card.Cardcode";
            var strGetFields = "b2b_crm.*,Member_Card.*";
            var sortKey = "IDcard";
            var sortMode = "1";
            var condition = "b2b_crm.com_id=" + comid + "  and Member_Card.ServerCard=" + uesr;
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);

            List<B2b_crm> list = new List<B2b_crm>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_crm
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Age = reader.GetValue<int>("Age"),
                        Email = reader.GetValue<string>("Email"),
                        Laiyuan = reader.GetValue<string>("Laiyuan"),
                        Name = reader.GetValue<string>("Name"),
                        Phone = reader.GetValue<string>("Phone"),
                        Regidate = reader.GetValue<DateTime>("Regidate"),
                        Sex = reader.GetValue<string>("Sex"),
                        Weixin = reader.GetValue<string>("Weixin"),
                        Imprest = reader.GetValue<decimal>("Imprest"),
                        Integral = reader.GetValue<decimal>("Integral"),
                        Idcard = reader.GetValue<decimal>("Idcard"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }

        internal List<B2b_crm> SearchPageList(string comid, int pageindex, int pagesize, string key, bool isNum, out int totalcount)
        {
            string condition = "";

            string channelcompanylimit = "b.com_id=" + comid;

            if (isNum)
            {
                condition += channelcompanylimit + " and (b.phone='" + key + "'  or b.email='" + key + "' or b.name='" + key + "' or b.idcard='" + key + "')";
            }
            else
            {
                condition += channelcompanylimit + " and (b.phone='" + key + "'  or b.email='" + key + "' or b.name='" + key + "')";
            }

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");




            cmd.PagingCommand1("wxmemberbasic a right join b2b_crm b on a.openid=b.weixin", "b.*,a.headimgurl,a.nickname,a.province,a.city,a.sex as wxsex", "b.id desc", "", pagesize, pageindex, "", condition);

            //var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            //var tblName = "b2b_crm";
            //var strGetFields = "*";
            //var sortKey = "id";
            //var sortMode = "1";

            //cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_crm> list = new List<B2b_crm>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_crm
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        //Age = reader.GetValue<int>("age"),
                        Email = reader.GetValue<string>("Email"),
                        // Laiyuan = reader.GetValue<string>("laiyuan"),
                        Name = reader.GetValue<string>("Name"),
                        Phone = reader.GetValue<string>("Phone"),
                        Regidate = reader.GetValue<DateTime>("Regidate"),
                        Sex = reader.GetValue<string>("Sex"),
                        //Weixin = reader.GetValue<string>("weixin"),
                        Imprest = reader.GetValue<decimal>("Imprest"),
                        Integral = reader.GetValue<decimal>("Integral"),
                        Idcard = reader.GetValue<decimal>("Idcard"),

                        WxHeadimgurl = reader.GetValue<string>("headimgurl").ConvertTo<string>("/images/defaultThumb.png"),
                        WxNickname = reader.GetValue<string>("nickname").ConvertTo<string>(""),
                        WxProvince = reader.GetValue<string>("province").ConvertTo<string>(""),
                        WxCity = reader.GetValue<string>("city").ConvertTo<string>(""),
                        WxSex = reader.GetValue<int>("wxsex").ToString().ConvertTo<int>(0),

                        Whetherwxfocus = reader.GetValue<bool>("whetherwxfocus"),
                        Whetheractivate = reader.GetValue<bool>("whetheractivate")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }

        internal List<B2b_crm> SearchPageList(string comid, int pageindex, int pagesize, string key, out int totalcount, string isactivate = "1", string iswxfocus = "0,1", string ishasweixin = "0,1", string ishasphone = "0,1")
        {
            string condition = "b.com_id=" + comid + " and b.whetheractivate in(" + isactivate + ")";

            if (iswxfocus != "0,1")
            {
                condition += "  and b.whetherwxfocus in (" + iswxfocus + ")";
            }
            if (ishasweixin != "0,1")
            {
                if (ishasweixin == "0")//没有微信
                {
                    condition += " and b.weixin=''";
                }
                else //含有微信
                {
                    condition += " and b.weixin!=''";
                }
            }

            if (ishasphone != "0,1")
            {
                if (ishasphone == "0")//没有
                {
                    condition += " and b.phone=''";
                }
                else //含有
                {
                    condition += " and b.phone!=''";
                }
            }

            //卡号类型为decimal ，不可用like查询，需要判断一下
            Regex regex = new Regex("^[0-9]*$");
            bool isNum = regex.IsMatch(key.Trim());


            if (key != "")
            {
                key = key.Trim();
                if (isNum == false)
                {
                    condition += " and (b.phone like  '%" + key + "%'  or b.email like '%" + key + "%' or b.name like '%" + key + "%' or a.nickname like '%" + key + "%') ";

                }
                else
                {
                    condition += " and (b.phone like  '%" + key + "%'  or b.email like '%" + key + "%' or b.name like '%" + key + "%' or a.nickname like '%" + key + "%' or b.idcard like '%" + key + "%')";
                }
            }
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");


            cmd.PagingCommand1("wxmemberbasic a right join b2b_crm b on a.openid=b.weixin", "b.*,a.headimgurl,a.nickname,a.province,a.city,a.sex as wxsex", "b.id desc", "", pagesize, pageindex, "", condition);



            List<B2b_crm> list = new List<B2b_crm>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_crm
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        //Age = reader.GetValue<int>("age"),
                        Email = reader.GetValue<string>("Email"),
                        // Laiyuan = reader.GetValue<string>("laiyuan"),
                        Name = reader.GetValue<string>("Name"),
                        Phone = reader.GetValue<string>("Phone"),
                        Regidate = reader.GetValue<DateTime>("Regidate"),
                        Sex = reader.GetValue<string>("Sex"),
                        Weixin = reader.GetValue<string>("weixin"),
                        Imprest = reader.GetValue<decimal>("Imprest"),
                        Integral = reader.GetValue<decimal>("Integral"),
                        Idcard = reader.GetValue<decimal>("Idcard"),

                        WxHeadimgurl = reader.GetValue<string>("headimgurl").ConvertTo<string>("/images/defaultThumb.png"),
                        WxNickname = reader.GetValue<string>("nickname").ConvertTo<string>(""),
                        WxProvince = reader.GetValue<string>("province").ConvertTo<string>(""),
                        WxCity = reader.GetValue<string>("city").ConvertTo<string>(""),
                        WxSex = reader.GetValue<int>("wxsex").ToString().ConvertTo<int>(0),

                        Whetherwxfocus = reader.GetValue<bool>("whetherwxfocus"),
                        Whetheractivate = reader.GetValue<bool>("whetheractivate"),
                        Groupid = reader.GetValue<int>("groupid"),
                        Wxlastinteracttime = reader.GetValue<DateTime>("wxlastinteracttime")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }



        internal string Searchb2bcrmbyopenid(string openid)
        {

            string condition = "select * from wxmemberbasic a right join b2b_crm b on a.openid=b.weixin where b.weixin='" + openid+"'" ;
            var cmd = sqlHelper.PrepareTextSqlCommand(condition);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<string>("headimgurl").ConvertTo<string>("/images/defaultThumb.png");
                }
            }
            return "";
        }


        internal List<B2b_crm> SearchPageList(string comid, int pageindex, int pagesize, string key, bool isNum, B2b_company_manageuser user, out int totalcount)
        {
            string condition = "";

            var channelcompanylimit = "b.idcard in (select cardcode from member_card where issuecard in (select id from member_channel where companyid  =" + user.Channelcompanyid + "))";

            if (isNum)
            {
                condition += channelcompanylimit + " and (b.phone='" + key + "'  or b.email='" + key + "' or b.name='" + key + "' or b.idcard='" + key + "')";
            }
            else
            {
                condition += channelcompanylimit + " and (b.phone='" + key + "'  or b.email='" + key + "' or b.name='" + key + "')";
            }
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            cmd.PagingCommand1("wxmemberbasic a right join b2b_crm b on a.openid=b.weixin", "b.*,a.headimgurl,a.nickname,a.province,a.city,a.sex as wxsex", "b.id desc", "", pagesize, pageindex, "", condition);


            List<B2b_crm> list = new List<B2b_crm>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_crm
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        //Age = reader.GetValue<int>("age"),
                        Email = reader.GetValue<string>("Email"),
                        // Laiyuan = reader.GetValue<string>("laiyuan"),
                        Name = reader.GetValue<string>("Name"),
                        Phone = reader.GetValue<string>("Phone"),
                        Regidate = reader.GetValue<DateTime>("Regidate"),
                        Sex = reader.GetValue<string>("Sex"),
                        //Weixin = reader.GetValue<string>("weixin"),
                        Imprest = reader.GetValue<decimal>("Imprest"),
                        Integral = reader.GetValue<decimal>("Integral"),
                        Idcard = reader.GetValue<decimal>("Idcard"),

                        WxHeadimgurl = reader.GetValue<string>("headimgurl").ConvertTo<string>("/images/defaultThumb.png"),
                        WxNickname = reader.GetValue<string>("nickname").ConvertTo<string>(""),
                        WxProvince = reader.GetValue<string>("province").ConvertTo<string>(""),
                        WxCity = reader.GetValue<string>("city").ConvertTo<string>(""),
                        WxSex = reader.GetValue<int>("wxsex").ToString().ConvertTo<int>(0),

                        Whetherwxfocus = reader.GetValue<bool>("whetherwxfocus"),
                        Whetheractivate = reader.GetValue<bool>("whetheractivate")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }
        internal List<B2b_crm> SearchPageList(string comid, int pageindex, int pagesize, string key, B2b_company_manageuser user, out int totalcount, string isactivate = "1", string iswxfocus = "0,1", string ishasweixin = "0,1")
        {
            string condition = "b.idcard in (select cardcode from member_card where issuecard in (select id from member_channel where companyid  =" + user.Channelcompanyid + ")) and b.whetheractivate in (" + isactivate + ")";

            if (iswxfocus != "0,1")
            {
                condition += " and b.whetherwxfocus in (" + iswxfocus + ")";
            }
            if (ishasweixin != "0,1")
            {
                if (ishasweixin == "0")
                {
                    condition += " and b.weixin=''";
                }
                else
                {
                    condition += " and b.weixin!=''";
                }
            }


            //卡号类型为decimal ，不可用like查询，需要判断一下
            Regex regex = new Regex("^[0-9]*$");
            bool isNum = regex.IsMatch(key.Trim());

            if (key != "")
            {
                if (isNum)
                {
                    condition += " and (b.phone like '%" + key + "%'  or b.email like  '%" + key + "%' or b.name like  '%" + key + "%' or b.idcard like '%" + key + "%' or a.nickname like '%" + key + "%')";
                }
                else
                {
                    condition += " and (b.phone like '%" + key + "%'  or b.email like  '%" + key + "%' or b.name like  '%" + key + "%'   or a.nickname like '%" + key + "%')";
                }
            }
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            cmd.PagingCommand1("wxmemberbasic a right join b2b_crm b on a.openid=b.weixin", "b.*,a.headimgurl,a.nickname,a.province,a.city,a.sex as wxsex", "b.id desc", "", pagesize, pageindex, "", condition);


            List<B2b_crm> list = new List<B2b_crm>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_crm
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        //Age = reader.GetValue<int>("age"),
                        Email = reader.GetValue<string>("Email"),
                        // Laiyuan = reader.GetValue<string>("laiyuan"),
                        Name = reader.GetValue<string>("Name"),
                        Phone = reader.GetValue<string>("Phone"),
                        Regidate = reader.GetValue<DateTime>("Regidate"),
                        Sex = reader.GetValue<string>("Sex"),
                        Weixin = reader.GetValue<string>("weixin"),
                        Imprest = reader.GetValue<decimal>("Imprest"),
                        Integral = reader.GetValue<decimal>("Integral"),
                        Idcard = reader.GetValue<decimal>("Idcard"),

                        WxHeadimgurl = reader.GetValue<string>("headimgurl").ConvertTo<string>("/images/defaultThumb.png"),
                        WxNickname = reader.GetValue<string>("nickname").ConvertTo<string>(""),
                        WxProvince = reader.GetValue<string>("province").ConvertTo<string>(""),
                        WxCity = reader.GetValue<string>("city").ConvertTo<string>(""),
                        WxSex = reader.GetValue<int>("wxsex").ToString().ConvertTo<int>(0),

                        Whetherwxfocus = reader.GetValue<bool>("whetherwxfocus"),
                        Whetheractivate = reader.GetValue<bool>("whetheractivate")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }
        internal List<B2b_crm> SearchPageList1(string comid, int pageindex, int pagesize, string key, int channelcompanyid, out int totalcount, string isactivate = "1", string iswxfocus = "0,1", string ishasweixin = "0,1", string channelcompanytype = "0", bool crmIsAccurateToPerson = false, int userid = 0, string ishasphone = "0,1")
        {
            string condition = "1=1 ";
            if (crmIsAccurateToPerson == true)
            {
                if (userid == 0)
                {
                    totalcount = 0;
                    return null;
                }
                else
                {
                    condition = "b.idcard in (select cardcode from member_card where issuecard in (select id from member_channel where mobile in (select tel from b2b_company_manageuser where id=" + userid + "))) ";
                }
            }
            else
            {
                if(channelcompanyid>0)
                {
                    condition = "b.idcard in (select cardcode from member_card where issuecard in (select id from member_channel where companyid  =" + channelcompanyid + ")) ";
                    #region 以下代码已经注释掉，由上面代码替代
                    ////渠道公司类型：0内部门店
                    //if (channelcompanytype == "0")
                    //{
                    //    condition = "b.idcard in (select cardcode from member_card where issuecard in (select id from member_channel where companyid  =" + channelcompanyid + ")) ";
                    //}
                    ////渠道公司类型：1合作公司
                    //if (channelcompanytype == "1")
                    //{
                    //    condition = "b.weixin in (select openid from WxSubscribeDetail where subscribesourceid in (select id from WxSubscribeSource where channelcompanyid =" + channelcompanyid + "))";
                    //}
                    #endregion
                }
            }

            if (isactivate != "0,1")
            {
                condition += " and b.whetheractivate in (" + isactivate + ")";
            }

            if (iswxfocus != "0,1")
            {
                condition += " and b.whetherwxfocus in (" + iswxfocus + ")";
            }
            if (ishasweixin != "0,1")
            {
                if (ishasweixin == "0")
                {
                    condition += " and b.weixin=''";
                }
                else
                {
                    condition += " and b.weixin!=''";
                }
            }
            if (ishasphone != "0,1")
            {
                if (ishasphone == "0")
                {
                    condition += " and b.phone=''";
                }
                else
                {
                    condition += " and b.phone!=''";
                }
            }


            //卡号类型为decimal ，不可用like查询，需要判断一下
            Regex regex = new Regex("^[0-9]*$");
            bool isNum = regex.IsMatch(key.Trim());

            if (key != "")
            {
                if (isNum)
                {
                    condition += " and (b.phone like '%" + key + "%'  or b.email like  '%" + key + "%' or b.name like  '%" + key + "%' or b.idcard like '%" + key + "%' or a.nickname like '%" + key + "%')";
                }
                else
                {
                    condition += " and (b.phone like '%" + key + "%'  or b.email like  '%" + key + "%' or b.name like  '%" + key + "%'   or a.nickname like '%" + key + "%')";
                }
            }


            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            cmd.PagingCommand1("wxmemberbasic a right join b2b_crm b on a.openid=b.weixin", "b.*,a.headimgurl,a.nickname,a.province,a.city,a.sex as wxsex", "b.id desc", "", pagesize, pageindex, "", condition);


            List<B2b_crm> list = new List<B2b_crm>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_crm
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        //Age = reader.GetValue<int>("age"),
                        Email = reader.GetValue<string>("Email"),
                        // Laiyuan = reader.GetValue<string>("laiyuan"),
                        Name = reader.GetValue<string>("Name"),
                        Phone = reader.GetValue<string>("Phone"),
                        Regidate = reader.GetValue<DateTime>("Regidate"),
                        Sex = reader.GetValue<string>("Sex"),
                        Weixin = reader.GetValue<string>("weixin"),
                        Imprest = reader.GetValue<decimal>("Imprest"),
                        Integral = reader.GetValue<decimal>("Integral"),
                        Idcard = reader.GetValue<decimal>("Idcard"),

                        WxHeadimgurl = reader.GetValue<string>("headimgurl").ConvertTo<string>("/images/defaultThumb.png"),
                        WxNickname = reader.GetValue<string>("nickname").ConvertTo<string>(""),
                        WxProvince = reader.GetValue<string>("province").ConvertTo<string>(""),
                        WxCity = reader.GetValue<string>("city").ConvertTo<string>(""),
                        WxSex = reader.GetValue<int>("wxsex").ToString().ConvertTo<int>(0),

                        Whetherwxfocus = reader.GetValue<bool>("whetherwxfocus"),
                        Whetheractivate = reader.GetValue<bool>("whetheractivate"),
                        Groupid = reader.GetValue<int>("groupid"),
                        Wxlastinteracttime = reader.GetValue<DateTime>("wxlastinteracttime")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }

        internal B2b_crm GetSjKeHu(string phone, int comid)
        {
            const string sqlTxt = @"SELECT * FROM [EtownDB].[dbo].[b2b_crm] where phone=@phone and com_id=@comid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@phone", phone);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_crm u = null;

                while (reader.Read())
                {
                    u = new B2b_crm
                    {
                        Com_id = reader.GetValue<int>("com_id"),
                        Age = reader.GetValue<int>("age"),
                        Email = reader.GetValue<string>("email"),
                        Id = reader.GetValue<int>("id"),
                        Laiyuan = reader.GetValue<string>("laiyuan"),
                        Name = reader.GetValue<string>("name"),
                        Phone = reader.GetValue<string>("phone"),
                        Regidate = reader.GetValue<DateTime>("regidate"),
                        Sex = reader.GetValue<string>("sex"),
                        Weixin = reader.GetValue<string>("weixin"),
                        Password1 = reader.GetValue<string>("password1"),
                        Idcard = reader.GetValue<decimal>("IDcard").ToString().ConvertTo<decimal>(0)
                    };

                }
                return u;
            }
        }

        internal B2b_crm GetB2bCrmByCardcode(decimal cardcode)
        {
            const string sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[IDcard]
      ,[password1]
      ,[name]
      ,[sex]
      ,[phone]
      ,[email]
      ,[weixin]
      ,[laiyuan]
      ,[regidate]
      ,[age]
      ,[imprest]
      ,[Integral]
      ,crmlevel
      ,dengjifen
  FROM [EtownDB].[dbo].[b2b_crm] where   IDcard=@cardcode";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@cardcode", cardcode);


            using (var reader = cmd.ExecuteReader())
            {
                B2b_crm u = null;

                while (reader.Read())
                {
                    u = new B2b_crm
                    {
                        Com_id = reader.GetValue<int>("com_id"),
                        Age = reader.GetValue<int?>("age") == null ? 0 : reader.GetValue<int>("age"),
                        Email = reader.GetValue<string>("email") == null ? "" : reader.GetValue<string>("email"),
                        Id = reader.GetValue<int>("id"),
                        Laiyuan = reader.GetValue<string>("laiyuan") == null ? "" : reader.GetValue<string>("laiyuan"),
                        Name = reader.GetValue<string>("name"),
                        Phone = reader.GetValue<string>("phone"),
                        Regidate = reader.GetValue<DateTime>("regidate"),
                        Sex = reader.GetValue<string>("sex"),
                        Weixin = reader.GetValue<string>("weixin") == null ? "" : reader.GetValue<string>("weixin"),
                        Imprest = reader.GetValue<decimal>("Imprest"),
                        Integral = reader.GetValue<decimal>("Integral"),
                        Idcard = reader.GetValue<decimal?>("Idcard") == null ? 0 : reader.GetValue<decimal>("Idcard"),
                        CrmLevel = reader.GetValue<string>("crmlevel"),
                        Dengjifen = reader.GetValue<decimal>("dengjifen")
                    };

                }
                return u;
            }
        }

        internal B2b_crm GetB2bCrmByPhone(string phone)
        {
            const string sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[IDcard]
      ,[password1]
      ,[name]
      ,[sex]
      ,[phone]
      ,[email]
      ,[weixin]
      ,[laiyuan]
      ,[regidate]
      ,[age]
      ,[imprest]
      ,[Integral]
      ,crmlevel
      ,dengjifen
  FROM [EtownDB].[dbo].[b2b_crm] where   phone=@phone";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@phone", phone);


            using (var reader = cmd.ExecuteReader())
            {
                B2b_crm u = null;

                while (reader.Read())
                {
                    u = new B2b_crm
                    {
                        Com_id = reader.GetValue<int>("com_id"),
                        Age = reader.GetValue<int>("age"),
                        Email = reader.GetValue<string>("email"),
                        Id = reader.GetValue<int>("id"),
                        Laiyuan = reader.GetValue<string>("laiyuan"),
                        Name = reader.GetValue<string>("name"),
                        Phone = reader.GetValue<string>("phone"),
                        Regidate = reader.GetValue<DateTime>("regidate"),
                        Sex = reader.GetValue<string>("sex"),
                        Weixin = reader.GetValue<string>("weixin"),
                        Idcard = reader.GetValue<decimal>("IDcard"),
                        CrmLevel = reader.GetValue<string>("crmlevel"),
                        Dengjifen = reader.GetValue<decimal>("dengjifen")
                    };

                }
                return u;
            }
        }
        internal B2b_crm GetB2bCrmByPhone(string phone, int comid)
        {
            const string sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[IDcard]
      ,[password1]
      ,[name]
      ,[sex]
      ,[phone]
      ,[email]
      ,[weixin]
      ,[laiyuan]
      ,[regidate]
      ,[age]
      ,[imprest]
      ,[Integral]
      ,crmlevel
      ,dengjifen
  FROM [EtownDB].[dbo].[b2b_crm] where   phone=@phone and com_id=@comid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@phone", phone);
            cmd.AddParam("@comid", comid);


            using (var reader = cmd.ExecuteReader())
            {
                B2b_crm u = null;

                while (reader.Read())
                {
                    u = new B2b_crm
                    {
                        Com_id = reader.GetValue<int>("com_id"),
                        Age = reader.GetValue<int>("age"),
                        Email = reader.GetValue<string>("email"),
                        Id = reader.GetValue<int>("id"),
                        Laiyuan = reader.GetValue<string>("laiyuan"),
                        Name = reader.GetValue<string>("name"),
                        Phone = reader.GetValue<string>("phone"),
                        Regidate = reader.GetValue<DateTime>("regidate"),
                        Sex = reader.GetValue<string>("sex"),
                        Weixin = reader.GetValue<string>("weixin"),
                        Idcard = reader.GetValue<decimal>("IDcard"),
                        Imprest = reader.GetValue<decimal>("imprest"),
                        Integral = reader.GetValue<decimal>("Integral"),
                        CrmLevel = reader.GetValue<string>("crmlevel"),
                        Dengjifen = reader.GetValue<decimal>("dengjifen")

                    };

                }
                return u;
            }
        }
        internal B2b_crm GetB2bCrmByWeiXin(string weixinno)
        {
            const string sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[IDcard]
      ,[password1]
      ,[name]
      ,[sex]
      ,[phone]
      ,[email]
      ,[weixin]
      ,[laiyuan]
      ,[regidate]
      ,[age]
      ,[imprest]
      ,[Integral]
      ,wxlastinteracttime
      ,whetherwxfocus
      ,crmlevel
      ,dengjifen
  FROM [EtownDB].[dbo].[b2b_crm] where   weixin=@weixin";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@weixin", weixinno);


            using (var reader = cmd.ExecuteReader())
            {
                B2b_crm u = null;

                while (reader.Read())
                {
                    u = new B2b_crm
                    {
                        Com_id = reader.GetValue<int>("com_id"),
                        Age = reader.GetValue<int>("age"),
                        Email = reader.GetValue<string>("email"),
                        Id = reader.GetValue<int>("id"),
                        Laiyuan = reader.GetValue<string>("laiyuan"),
                        Name = reader.GetValue<string>("name"),
                        Phone = reader.GetValue<string>("phone"),
                        Regidate = reader.GetValue<DateTime>("regidate"),
                        Sex = reader.GetValue<string>("sex"),
                        Weixin = reader.GetValue<string>("weixin"),
                        Idcard = reader.GetValue<decimal>("IDcard"),
                        Cardid = GetMemberCardID(reader.GetValue<decimal>("IDcard")),
                        Imprest = reader.GetValue<decimal>("imprest"),
                        Integral = reader.GetValue<decimal>("Integral"),
                        Wxlastinteracttime = reader.GetValue<DateTime>("wxlastinteracttime"),
                        Whetherwxfocus = reader.GetValue<bool>("whetherwxfocus"),
                        CrmLevel = reader.GetValue<string>("crmlevel"),
                        Dengjifen = reader.GetValue<decimal>("dengjifen")
                    };

                }
                return u;
            }
        }

        internal B2b_crm GetB2bCrmByWeiXin(string weixinno, int comid)
        {
            const string sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[IDcard]
      ,[password1]
      ,[name]
      ,[sex]
      ,[phone]
      ,[email]
      ,[weixin]
      ,[laiyuan]
      ,[regidate]
      ,[age]
      ,[imprest]
      ,[Integral]
      ,crmlevel
      ,dengjifen
  FROM [EtownDB].[dbo].[b2b_crm] where   weixin=@weixin and com_id=@comid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@weixin", weixinno);
            cmd.AddParam("@comid", comid);


            using (var reader = cmd.ExecuteReader())
            {
                B2b_crm u = null;

                if (reader.Read())
                {
                    u = new B2b_crm
                    {
                        Com_id = reader.GetValue<int>("com_id"),
                        Age = reader.GetValue<int>("age").ToString().ConvertTo<int>(0),
                        Email = reader.GetValue<string>("email"),
                        Id = reader.GetValue<int>("id"),
                        Laiyuan = reader.GetValue<string>("laiyuan").ConvertTo<string>(""),
                        Name = reader.GetValue<string>("name"),
                        Phone = reader.GetValue<string>("phone"),
                        Regidate = reader.GetValue<DateTime>("regidate"),
                        Sex = reader.GetValue<string>("sex"),
                        Weixin = reader.GetValue<string>("weixin").ConvertTo<string>(""),
                        Idcard = reader.GetValue<decimal>("IDcard"),
                        Cardid = GetMemberCardID(reader.GetValue<decimal>("IDcard")),
                        Imprest = reader.GetValue<decimal>("imprest"),
                        Integral = reader.GetValue<decimal>("Integral"),
                        CrmLevel = reader.GetValue<string>("crmlevel"),
                        Dengjifen = reader.GetValue<decimal>("dengjifen")
                    };

                }
                return u;
            }
        }

        private int GetMemberCardID(decimal idcard)
        {
            Member_Card card = new MemberCardData().GetCardByCardNumber(idcard);
            if (card == null)
            {
                return 0;
            }
            else
            {
                return card.Id;
            }

        }

        #region 活动使用日志
        //and a.ServerName=@ServerName 
        internal List<Member_Activity_Log> SearchActivityList(string comid, int pageindex, int pagesize, string key, string ServerName, bool isNum, out int totalcount)
        {
            string sqlTxt = "";


            string channelcompanylimit = " comid=" + comid;

            int countnum = 0;
            if (isNum)
            {
                sqlTxt = @"SELECT top 10 [ID]
      ,[CardID]
      ,[ACTID]
      ,[Pno]
      ,[Usenum]
      ,[OrderId]
      ,[ServerName]
      ,[Num_people]
      ,[Per_capita_money]
      ,[sales_admin]
      ,[Member_return_money]
      ,[return_money_state]
      ,[return_money_admin]
      ,[Usesubdate]
  FROM [EtownDB].[dbo].[Member_Activity_Log] as a where " + channelcompanylimit + "  and ( a.OrderId=@key or CardID in (select id from Member_Card where Cardcode=@key ) ) order by Usesubdate desc";
            }
            else
            {
                sqlTxt = @"SELECT top 10 [ID]
      ,[CardID]
      ,[ACTID]
      ,[Pno]
      ,[Usenum]
      ,[OrderId]
      ,[ServerName]
      ,[Num_people]
      ,[Per_capita_money]
      ,[sales_admin]
      ,[Member_return_money]
      ,[return_money_state]
      ,[return_money_admin]
      ,[Usesubdate]
  FROM [EtownDB].[dbo].[Member_Activity_Log] as a where " + channelcompanylimit + " and a.sales_admin=@key order by Usesubdate desc";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@key", key);

            //cmd.AddParam("@ServerName", ServerName);

            List<Member_Activity_Log> list = new List<Member_Activity_Log>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Activity_Log
                    {
                        ID = reader.GetValue<int>("ID"),
                        CardID = reader.GetValue<int>("CardID"),
                        ACTID = reader.GetValue<int>("ACTID"),
                        Pno = reader.GetValue<string>("Pno"),
                        Usenum = reader.GetValue<int>("Usenum"),
                        OrderId = reader.GetValue<decimal>("OrderId"),
                        ServerName = reader.GetValue<string>("ServerName"),
                        Sales_admin = reader.GetValue<string>("sales_admin"),
                        Num_people = reader.GetValue<int>("Num_people"),
                        Per_capita_money = reader.GetValue<decimal>("Per_capita_money"),
                        Member_return_money = reader.GetValue<decimal>("Member_return_money"),
                        Return_money_state = reader.GetValue<int>("Return_money_state"),
                        Return_money_admin = reader.GetValue<string>("return_money_admin"),
                        Usesubdate = reader.GetValue<DateTime>("Usesubdate"),

                    });
                    countnum = countnum + 1;
                }
            }
            totalcount = countnum;

            return list;

        }


        #endregion
        internal List<Member_Activity_Log> SearchActivityList(string comid, int pageindex, int pagesize, string key, string ServerName, bool isNum, int channelcompanyid, out int totalcount)
        {
            string sqlTxt = "";



            var channelcompanylimit = " cardid in (select id from member_card where issuecard in (select id from member_channel where companyid =" + channelcompanyid + "))";

            int countnum = 0;
            if (isNum)
            {
                sqlTxt = @"SELECT top 10 [ID]
      ,[CardID]
      ,[ACTID]
      ,[Pno]
      ,[Usenum]
      ,[OrderId]
      ,[ServerName]
      ,[Num_people]
      ,[Per_capita_money]
      ,[sales_admin]
      ,[Member_return_money]
      ,[return_money_state]
      ,[return_money_admin]
      ,[Usesubdate]
  FROM [EtownDB].[dbo].[Member_Activity_Log] as a where " + channelcompanylimit + "  and ( a.OrderId=@key or CardID in (select id from Member_Card where Cardcode=@key ) ) order by Usesubdate desc";
            }
            else
            {
                sqlTxt = @"SELECT top 10 [ID]
      ,[CardID]
      ,[ACTID]
      ,[Pno]
      ,[Usenum]
      ,[OrderId]
      ,[ServerName]
      ,[Num_people]
      ,[Per_capita_money]
      ,[sales_admin]
      ,[Member_return_money]
      ,[return_money_state]
      ,[return_money_admin]
      ,[Usesubdate]
  FROM [EtownDB].[dbo].[Member_Activity_Log] as a where " + channelcompanylimit + " and a.sales_admin=@key order by Usesubdate desc";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@key", key);

            //cmd.AddParam("@ServerName", ServerName);

            List<Member_Activity_Log> list = new List<Member_Activity_Log>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Activity_Log
                    {
                        ID = reader.GetValue<int>("ID"),
                        CardID = reader.GetValue<int>("CardID"),
                        ACTID = reader.GetValue<int>("ACTID"),
                        Pno = reader.GetValue<string>("Pno"),
                        Usenum = reader.GetValue<int>("Usenum"),
                        OrderId = reader.GetValue<decimal>("OrderId"),
                        ServerName = reader.GetValue<string>("ServerName"),
                        Sales_admin = reader.GetValue<string>("sales_admin"),
                        Num_people = reader.GetValue<int>("Num_people"),
                        Per_capita_money = reader.GetValue<decimal>("Per_capita_money"),
                        Member_return_money = reader.GetValue<decimal>("Member_return_money"),
                        Return_money_state = reader.GetValue<int>("Return_money_state"),
                        Return_money_admin = reader.GetValue<string>("return_money_admin"),
                        Usesubdate = reader.GetValue<DateTime>("Usesubdate"),

                    });
                    countnum = countnum + 1;
                }
            }
            totalcount = countnum;

            return list;

        }

        #region 活动加载明细列表
        internal List<Member_Activity_Log> LoadingList(string comid, int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "Member_Activity_Log";
            var strGetFields = "*";
            var sortKey = "ID";
            var sortMode = "1";

            var condition = " comid=" + comid;
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);

            List<Member_Activity_Log> list = new List<Member_Activity_Log>();

            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Activity_Log
                    {
                        ID = reader.GetValue<int>("ID"),
                        CardID = reader.GetValue<int>("CardID"),
                        ACTID = reader.GetValue<int>("ACTID"),
                        Pno = reader.GetValue<string>("Pno"),
                        Usenum = reader.GetValue<int>("Usenum"),
                        OrderId = reader.GetValue<decimal>("OrderId"),
                        ServerName = reader.GetValue<string>("ServerName"),
                        Sales_admin = reader.GetValue<string>("sales_admin"),
                        Num_people = reader.GetValue<int>("Num_people"),
                        Per_capita_money = reader.GetValue<decimal>("Per_capita_money"),
                        Member_return_money = reader.GetValue<decimal>("Member_return_money"),
                        Return_money_state = reader.GetValue<int>("Return_money_state"),
                        Return_money_admin = reader.GetValue<string>("return_money_admin"),
                        Usesubdate = reader.GetValue<DateTime>("Usesubdate"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        #endregion
        internal List<Member_Activity_Log> LoadingList(string comid, int pageindex, int pagesize, int channelcompanyid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "Member_Activity_Log";
            var strGetFields = "*";
            var sortKey = "ID";
            var sortMode = "1";

            var condition = " cardid in (select id from member_card where issuecard in (select id from member_channel where companyid =" + channelcompanyid + "))";
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);

            List<Member_Activity_Log> list = new List<Member_Activity_Log>();

            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Activity_Log
                    {
                        ID = reader.GetValue<int>("ID"),
                        CardID = reader.GetValue<int>("CardID"),
                        ACTID = reader.GetValue<int>("ACTID"),
                        Pno = reader.GetValue<string>("Pno"),
                        Usenum = reader.GetValue<int>("Usenum"),
                        OrderId = reader.GetValue<decimal>("OrderId"),
                        ServerName = reader.GetValue<string>("ServerName"),
                        Sales_admin = reader.GetValue<string>("sales_admin"),
                        Num_people = reader.GetValue<int>("Num_people"),
                        Per_capita_money = reader.GetValue<decimal>("Per_capita_money"),
                        Member_return_money = reader.GetValue<decimal>("Member_return_money"),
                        Return_money_state = reader.GetValue<int>("Return_money_state"),
                        Return_money_admin = reader.GetValue<string>("return_money_admin"),
                        Usesubdate = reader.GetValue<DateTime>("Usesubdate"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }

        #region  crm活动日志详细页面
        internal Member_Activity_Log Logdetails(int id, int comid)
        {
            string sqlTxt = "";
            sqlTxt = @"SELECT [ID]
      ,[CardID]
      ,[ACTID]
      ,[Pno]
      ,[Usenum]
      ,[OrderId]
      ,[ServerName]
      ,[Num_people]
      ,[Per_capita_money]
      ,[sales_admin]
      ,[Member_return_money]
      ,[return_money_state]
      ,[return_money_admin]
      ,[Usesubdate]
      ,[comid]
  FROM [EtownDB].[dbo].[Member_Activity_Log] as a where a.comid=@comid and a.ID=@ID ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@ID", id);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                Member_Activity_Log list = null;

                while (reader.Read())
                {
                    list = new Member_Activity_Log
                    {
                        ID = reader.GetValue<int>("ID"),
                        CardID = reader.GetValue<int>("CardID"),
                        ACTID = reader.GetValue<int>("ACTID"),
                        OrderId = reader.GetValue<int>("OrderId"),
                        ServerName = reader.GetValue<string>("ServerName"),
                        Sales_admin = reader.GetValue<string>("sales_admin"),
                        Num_people = reader.GetValue<int>("Num_people"),
                        Usesubdate = reader.GetValue<DateTime>("Usesubdate"),

                    };
                }
                return list;

            }
        }
        #endregion



        #region  获得单个活动，单个人使用的活动日志
        internal List<Member_Activity_Log> Logcardact(int actid, int cardid, int comid, out int totalcount)
        {
            int countnum = 0;
            string sqlTxt = "";
            sqlTxt = @"SELECT [ID]
      ,[CardID]
      ,[ACTID]
      ,[Pno]
      ,[Usenum]
      ,[OrderId]
      ,[ServerName]
      ,[Num_people]
      ,[Per_capita_money]
      ,[sales_admin]
      ,[Member_return_money]
      ,[return_money_state]
      ,[return_money_admin]
      ,[Usesubdate]
      ,[comid]
  FROM [EtownDB].[dbo].[Member_Activity_Log] as a where a.comid=@comid  ";

            if (actid > 0)
            {
                sqlTxt += " and a.actid=@actid ";
            }
            if (cardid > 0)
            {
                sqlTxt += " and a.cardid=@cardid";
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            if (actid > 0)
            {
                cmd.AddParam("@actid", actid);
            }
            if (cardid > 0)
            {
                cmd.AddParam("@cardid", cardid);
            }
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                List<Member_Activity_Log> list = new List<Member_Activity_Log>();

                while (reader.Read())
                {
                    list.Add(new Member_Activity_Log
                   {
                       ID = reader.GetValue<int>("ID"),
                       CardID = reader.GetValue<int>("CardID"),
                       ACTID = reader.GetValue<int>("ACTID"),
                       OrderId = reader.GetValue<decimal>("OrderId"),
                       ServerName = reader.GetValue<string>("ServerName"),
                       Sales_admin = reader.GetValue<string>("sales_admin"),
                       Num_people = reader.GetValue<int>("Num_people"),
                       Usesubdate = reader.GetValue<DateTime>("Usesubdate"),

                   });
                    countnum = countnum + 1;
                }
                totalcount = countnum;
                return list;
            }
        }
        #endregion


        #region 微信首次关注时 绑定账号
        internal int InsB2bCrm(int comid, string cardcode, string weixinNo, string weixinpass, string registertime, int whetherwxfocus = 0, string crmlevel = "0")
        {

            const string sqlTxt = @"insert [EtownDB].[dbo].[b2b_crm]([com_id],[IDcard],[weixin],[weixinpass],[regidate],whetherwxfocus,crmlevel) values(@comid,@idcard,@weixin,@weixinpass,@registerdate,@whetherwxfocus,@crmlevel);SELECT @@IDENTITY;";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@idcard", cardcode);
            cmd.AddParam("@weixin", weixinNo);
            cmd.AddParam("@weixinpass", weixinpass);
            cmd.AddParam("@registerdate", registertime);
            cmd.AddParam("@whetherwxfocus", whetherwxfocus);
            cmd.AddParam("@crmlevel", crmlevel);
            object o = cmd.ExecuteScalar();
            int newId = o == null ? 0 : int.Parse(o.ToString());
            return newId;

        }
        #endregion

        #region 发展会员统计
        internal DataTable GetCrmStatistics(int comid, int channelcompanyid)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_crm_statistics");


            cmd.AddOutParam("@allcardopennum", SqlDbType.Int, 32);
            cmd.AddOutParam("@Yesterdayallcardopennum", SqlDbType.Int, 32);
            cmd.AddOutParam("@monthallcardopennum", SqlDbType.Int, 32);
            cmd.AddOutParam("@entitycardopennum", SqlDbType.Int, 32);
            cmd.AddOutParam("@Yesterdayentitycardopennum", SqlDbType.Int, 32);
            cmd.AddOutParam("@monthentitycardopennum", SqlDbType.Int, 32);

            cmd.AddOutParam("@webcardopennum", SqlDbType.Int, 32);
            cmd.AddOutParam("@Yesterdaywebcardopennum", SqlDbType.Int, 32);
            cmd.AddOutParam("@monthwebcardopennum", SqlDbType.Int, 32);

            cmd.AddOutParam("@salewebcardopennum", SqlDbType.Int, 32);
            cmd.AddOutParam("@saleyesterdaywebcardopennum", SqlDbType.Int, 32);
            cmd.AddOutParam("@salemonthwebcardopennum", SqlDbType.Int, 32);

            cmd.AddOutParam("@weixinnum", SqlDbType.Int, 32);
            cmd.AddOutParam("@Yesterdayweixinnum", SqlDbType.Int, 32);
            cmd.AddOutParam("@monthweixinnum", SqlDbType.Int, 32);
            cmd.AddOutParam("@weixinphonenum", SqlDbType.Int, 32);

            cmd.AddOutParam("@crmconsultnum", SqlDbType.Int, 32);
            cmd.AddOutParam("@Yesterdaycrmconsultnum", SqlDbType.Int, 32);
            cmd.AddOutParam("@monthcrmconsultnum", SqlDbType.Int, 32);

            cmd.AddOutParam("@crmordernum", SqlDbType.Int, 32);
            cmd.AddOutParam("@Yesterdaycrmordernum", SqlDbType.Int, 32);
            cmd.AddOutParam("@monthcrmordernum", SqlDbType.Int, 32);

            cmd.AddOutParam("@cardvalidatenum", SqlDbType.Int, 32);
            cmd.AddOutParam("@Yesterdaycardvalidatenum", SqlDbType.Int, 32);
            cmd.AddOutParam("@monthcardvalidatenum", SqlDbType.Int, 32);

            cmd.AddOutParam("@carddealMoney", SqlDbType.Decimal, 2);
            cmd.AddOutParam("@YesterdaycarddealMoney", SqlDbType.Decimal, 2);
            cmd.AddOutParam("@monthcarddealMoney", SqlDbType.Decimal, 2);

            cmd.AddOutParam("@cardavgdealmoney", SqlDbType.Decimal, 2);
            cmd.AddOutParam("@cardeveryoneavgdealmoney", SqlDbType.Decimal, 2);
            cmd.AddParam("@yesterday", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            cmd.AddParam("@lastdayoflastmonth", DateTime.Now.AddDays(1 - DateTime.Now.Day).AddDays(-1).ToString("yyyy-MM-dd"));//上个月最后一天
            cmd.AddParam("@firstdayofnextmonth", DateTime.Now.AddMonths(1).AddDays(1 - DateTime.Now.AddMonths(1).Day).ToString("yyyy-MM-dd"));//下个月第一天

            cmd.AddParam("@comid", comid);
            cmd.AddParam("@channelcompanyid", channelcompanyid);

            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("allcardopennum", typeof(int));
            dt.Columns.Add("Yesterdayallcardopennum", typeof(int));
            dt.Columns.Add("monthallcardopennum", typeof(int));
            dt.Columns.Add("entitycardopennum", typeof(int));
            dt.Columns.Add("Yesterdayentitycardopennum", typeof(int));
            dt.Columns.Add("monthentitycardopennum", typeof(int));
            dt.Columns.Add("webcardopennum", typeof(int));
            dt.Columns.Add("Yesterdaywebcardopennum", typeof(int));
            dt.Columns.Add("monthwebcardopennum", typeof(int));

            dt.Columns.Add("salewebcardopennum", typeof(int));
            dt.Columns.Add("saleyesterdaywebcardopennum", typeof(int));
            dt.Columns.Add("salemonthwebcardopennum", typeof(int));

            dt.Columns.Add("weixinnum", typeof(int));
            dt.Columns.Add("Yesterdayweixinnum", typeof(int));
            dt.Columns.Add("monthweixinnum", typeof(int));
            dt.Columns.Add("weixinphonenum", typeof(int));

            dt.Columns.Add("crmconsultnum", typeof(int));
            dt.Columns.Add("Yesterdaycrmconsultnum", typeof(int));
            dt.Columns.Add("monthcrmconsultnum", typeof(int));

            dt.Columns.Add("crmordernum", typeof(int));
            dt.Columns.Add("Yesterdaycrmordernum", typeof(int));
            dt.Columns.Add("monthcrmordernum", typeof(int));

            dt.Columns.Add("cardvalidatenum", typeof(int));
            dt.Columns.Add("Yesterdaycardvalidatenum", typeof(int));
            dt.Columns.Add("monthcardvalidatenum", typeof(int));

            dt.Columns.Add("carddealMoney", typeof(decimal));
            dt.Columns.Add("YesterdaycarddealMoney", typeof(decimal));
            dt.Columns.Add("monthcarddealMoney", typeof(decimal));

            dt.Columns.Add("cardavgdealmoney", typeof(decimal));
            dt.Columns.Add("cardeveryoneavgdealmoney", typeof(decimal));

            DataRow dr;
            dr = dt.NewRow();//新行
            dr[0] = int.Parse(cmd.Parameters[0].Value.ToString());
            dr[1] = int.Parse(cmd.Parameters[1].Value.ToString());
            dr[2] = int.Parse(cmd.Parameters[2].Value.ToString());
            dr[3] = int.Parse(cmd.Parameters[3].Value.ToString());
            dr[4] = int.Parse(cmd.Parameters[4].Value.ToString());
            dr[5] = int.Parse(cmd.Parameters[5].Value.ToString());
            dr[6] = int.Parse(cmd.Parameters[6].Value.ToString());
            dr[7] = int.Parse(cmd.Parameters[7].Value.ToString());
            dr[8] = int.Parse(cmd.Parameters[8].Value.ToString());


            dr[9] = int.Parse(cmd.Parameters[9].Value.ToString());
            dr[10] = int.Parse(cmd.Parameters[10].Value.ToString());
            dr[11] = int.Parse(cmd.Parameters[11].Value.ToString());
            dr[12] = int.Parse(cmd.Parameters[12].Value.ToString());
            dr[13] = int.Parse(cmd.Parameters[13].Value.ToString());
            dr[14] = int.Parse(cmd.Parameters[14].Value.ToString());


            dr[15] = decimal.Parse(cmd.Parameters[15].Value.ToString());
            dr[16] = decimal.Parse(cmd.Parameters[16].Value.ToString());
            dr[17] = decimal.Parse(cmd.Parameters[17].Value.ToString());
            dr[18] = decimal.Parse(cmd.Parameters[18].Value.ToString());

            dr[19] = decimal.Parse(cmd.Parameters[19].Value.ToString());
            dr[20] = decimal.Parse(cmd.Parameters[20].Value.ToString());
            dr[21] = decimal.Parse(cmd.Parameters[21].Value.ToString());
            dr[22] = decimal.Parse(cmd.Parameters[22].Value.ToString());
            dr[23] = decimal.Parse(cmd.Parameters[23].Value.ToString());
            dr[24] = decimal.Parse(cmd.Parameters[24].Value.ToString());
            dr[25] = decimal.Parse(cmd.Parameters[25].Value.ToString());
            dr[26] = decimal.Parse(cmd.Parameters[26].Value.ToString());
            dr[27] = decimal.Parse(cmd.Parameters[27].Value.ToString());
            dr[28] = decimal.Parse(cmd.Parameters[28].Value.ToString());
            dr[29] = decimal.Parse(cmd.Parameters[29].Value.ToString());


            dt.Rows.Add(dr);

            return dt;

        }
        #endregion

        internal string GetB2bCrm(string openid, int comid, out B2b_crm userinfo)
        {
            string sqlTxt = "";
            sqlTxt = @"SELECT *  FROM [EtownDB].[dbo].[b2b_crm] where com_id=@comid and weixin=@openid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@openid", openid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {

                    userinfo = new B2b_crm
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Password1 = reader.GetValue<string>("Password1"),
                        Email = reader.GetValue<string>("Email"),
                        Name = reader.GetValue<string>("name"),
                        Phone = reader.GetValue<string>("phone"),
                        Regidate = reader.GetValue<DateTime>("regidate"),
                        Sex = reader.GetValue<string>("sex"),
                        Weixin = reader.GetValue<string>("weixin"),
                        Idcard = reader.GetValue<decimal>("Idcard"),
                        Integral = reader.GetValue<decimal>("Integral"),
                        Imprest = reader.GetValue<decimal>("imprest")
                    };
                    return "OK";

                }
                else
                {
                    userinfo = null;
                    return "没有此账户!";
                }
            }
        }

        internal int GetMemberNums(int comid)
        {
            string sql = "select  count(1) from b2b_crm where com_id=@comid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            object o = cmd.ExecuteScalar();
            return o == null ? 0 : int.Parse(o.ToString());
        }

        internal int GetWeiXinAttentionNum(int comid)
        {
            string sql = "select  count(1) from b2b_crm where com_id=@comid and id not in (select id from b2b_crm where weixin='' or weixin is null)";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            object o = cmd.ExecuteScalar();
            return o == null ? 0 : int.Parse(o.ToString());
        }

        internal int ModifyIDCard(string cardcode, int crmid)
        {
            string sql = "update b2b_crm set idcard=@idcard where id=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@idcard", cardcode);
            cmd.AddParam("@id", crmid);

            return cmd.ExecuteNonQuery();
        }

        internal B2b_crm GetB2bCrm(string openid, int comid)
        {
            string sqlTxt = "";
            sqlTxt = @"SELECT top 1 *  FROM [EtownDB].[dbo].[b2b_crm] where com_id=@comid and weixin=@openid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@openid", openid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_crm
                   {
                       Id = reader.GetValue<int>("Id"),
                       Com_id = reader.GetValue<int>("Com_id"),
                       Password1 = reader.GetValue<string>("Password1"),
                       Email = reader.GetValue<string>("Email"),
                       Name = reader.GetValue<string>("name"),
                       Phone = reader.GetValue<string>("phone"),
                       Regidate = reader.GetValue<DateTime>("regidate"),
                       Sex = reader.GetValue<string>("sex"),
                       Weixin = reader.GetValue<string>("weixin"),
                       Idcard = reader.GetValue<decimal>("Idcard"),
                       Integral = reader.GetValue<decimal>("Integral"),
                       Imprest = reader.GetValue<decimal>("imprest")
                   };

                }
                else
                {

                    return null;
                }
            }
        }
        /// <summary>
        /// excel导入其他商家的会员
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="cardcode"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="weixin"></param>
        /// <param name="whetherwxfocus"></param>
        /// <param name="whetheractivate"></param>
        /// <param name="registertime"></param>
        /// <returns></returns>
        internal int ExcelInsB2bCrm(int comid, string cardcode, string name, string phone, string weixin, int whetherwxfocus, int whetheractivate, string registertime, string email, decimal imprest, decimal integral, string country, string province, string city, string address, string agegroup, string crmlevel)
        {
            string sql = "INSERT INTO [EtownDB].[dbo].[b2b_crm]" +
            "([com_id],[IDcard],[password1],[name],[sex],[phone],[email],[weixin],[weixinpass],[laiyuan],[regidate],[age],[imprest],[Integral],[Birthday] ,[whetherwxfocus] ,[whetheractivate],crmlevel, wxcountry,  wxprovince,  wxcity,  address,  agegroup)" +
            "VALUES(" + comid + ",'" + cardcode + "','','" + name + "','','" + phone + "','" + email + "','" + weixin + "','','','" + registertime + "','0','" + imprest + "','" + integral + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "'," + whetherwxfocus + "," + whetheractivate + ",'" + crmlevel + "','" + country + "','" + province + "','" + city + "','" + address + "','" + agegroup + "');SELECT @@IDENTITY;";


            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            int newId = o == null ? 0 : int.Parse(o.ToString());
            return newId;
        }

        internal int ModifyCrmActivate(string weixin, int comid, int activatestate)
        {
            string sql = "update b2b_crm set whetheractivate=" + activatestate + " where weixin='" + weixin + "' and com_id=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }


        internal B2b_crm GetB2bCrmByPhone(int comid, string Phone)
        {
            const string sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[IDcard]
      ,[password1]
      ,[name]
      ,[sex]
      ,[phone]
      ,[email]
      ,[weixin]
      ,[laiyuan]
      ,[regidate]
      ,[age]
      ,[imprest]
      ,[Integral]
  FROM [EtownDB].[dbo].[b2b_crm] where   com_id=@comid and phone=@phone";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@phone", Phone);


            using (var reader = cmd.ExecuteReader())
            {
                B2b_crm u = null;

                while (reader.Read())
                {
                    u = new B2b_crm
                    {
                        Com_id = reader.GetValue<int>("com_id"),
                        Age = reader.GetValue<int>("age").ToString().ConvertTo<int>(0),
                        Email = reader.GetValue<string>("email"),
                        Id = reader.GetValue<int>("id"),
                        Laiyuan = reader.GetValue<string>("laiyuan").ConvertTo<string>(""),
                        Name = reader.GetValue<string>("name"),
                        Phone = reader.GetValue<string>("phone"),
                        Regidate = reader.GetValue<DateTime>("regidate"),
                        Sex = reader.GetValue<string>("sex"),
                        Weixin = reader.GetValue<string>("weixin").ConvertTo<string>(""),
                        Idcard = reader.GetValue<decimal>("IDcard").ToString().ConvertTo<decimal>(0),
                        Cardid = new MemberCardData().GetCardByCardNumber(reader.GetValue<decimal>("IDcard")).Id,
                        Imprest = reader.GetValue<decimal>("imprest"),
                        Integral = reader.GetValue<decimal>("Integral")
                    };

                }
                return u;
            }
        }

        internal bool IsHasCrmWeiXin(int comid, string weixin)
        {
            string sql = "select count(1) from b2b_crm where com_id=" + comid + " and weixin='" + weixin + "'";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                if (int.Parse(o.ToString()) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal bool IsHasCrmPhone(int comid, string phone)
        {
            string sql = "select count(1) from b2b_crm where com_id=" + comid + " and phone='" + phone + "'";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                if (int.Parse(o.ToString()) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                sqlHelper.Dispose();
                return false;
            }
        }

        internal B2b_crm GetB2bCrmById(int crmid)
        {
            const string sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[IDcard]
      ,[password1]
      ,[name]
      ,[sex]
      ,[phone]
      ,[email]
      ,[weixin]
      ,[laiyuan]
      ,[regidate]
      ,[age]
      ,[imprest]
      ,[Integral]
  FROM [EtownDB].[dbo].[b2b_crm] where  id=@crmid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@crmid", crmid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_crm u = null;

                while (reader.Read())
                {
                    u = new B2b_crm
                    {
                        Com_id = reader.GetValue<int>("com_id"),
                        Age = reader.GetValue<int>("age").ToString().ConvertTo<int>(0),
                        Email = reader.GetValue<string>("email"),
                        Id = reader.GetValue<int>("id"),
                        Laiyuan = reader.GetValue<string>("laiyuan").ConvertTo<string>(""),
                        Name = reader.GetValue<string>("name"),
                        Phone = reader.GetValue<string>("phone"),
                        Regidate = reader.GetValue<DateTime>("regidate"),
                        Sex = reader.GetValue<string>("sex"),
                        Weixin = reader.GetValue<string>("weixin").ConvertTo<string>(""),
                        Idcard = reader.GetValue<decimal>("IDcard").ToString().ConvertTo<decimal>(0),
                        Cardid = new MemberCardData().GetCardByCardNumber(reader.GetValue<decimal>("IDcard")).Id,
                        Imprest = reader.GetValue<decimal>("imprest"),
                        Integral = reader.GetValue<decimal>("Integral")
                    };

                }
                return u;
            }
        }

        internal List<B2b_crm> GetB2bCrmWeixinByComid(int comid, string country = "", string province = "", string city = "", string sex = "", int tagtype = 0, int tag = 0, string groupid = "全部")
        {
            string sqlTxt = "SELECT   [weixin]   FROM [EtownDB].[dbo].[b2b_crm] where com_id=" + comid + " and whetherwxfocus=1";

            if (country == "国家" || country == "全部")
            {
            }
            else
            {
                if (province == "省市")
                {
                    //按国家查询
                    sqlTxt += " and   wxcountry='" + country + "' ";
                }
                else
                {
                    if (province == "城市")
                    {
                        //按省市查询
                        sqlTxt += " and   wxprovince='" + province + "'  ";
                    }
                    else
                    {
                        //按城市查询
                        sqlTxt += " and   wxcity='" + city + "' ";
                    }
                }
            }

            if (sex != "")
            {
                if (sex != "0")
                {
                    sqlTxt += " and weixin in (select openid from WxMemberBasic where sex='" + sex + "')";
                }
            }


            if (tag != 0)//精确到兴趣标签
            {
                sqlTxt += " and id in (select crmid from b2b_crm_interesttag where tagid=" + tag + ")";
            }
            else //精确到兴趣类型
            {
                if (tagtype != 0)
                {
                    sqlTxt += " and id in (select crmid from b2b_crm_interesttag where tagid in (select id from b2b_interesttag where tagtypeid=" + tagtype + ") )";
                }
            }

            if (groupid != "全部")
            {
                sqlTxt += " and groupid =" + groupid;
            }

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;


                using (var reader = cmd.ExecuteReader())
                {
                    List<B2b_crm> list = new List<B2b_crm>();

                    while (reader.Read())
                    {
                        list.Add(new B2b_crm
                        {

                            Weixin = reader.GetValue<string>("weixin").ConvertTo<string>("")

                        });

                    }
                    return list;
                }
            }
            catch
            {
                return null;
            }
        }

        internal List<B2b_crm> GetB2bCrmWeixinByMenshi(int menshiid, string country = "", string province = "", string city = "", string sex = "", int tagtype = 0, int tag = 0)
        {
            string sqlTxt = @"SELECT   [weixin]  FROM [EtownDB].[dbo].[b2b_crm] where  idcard in (select cardcode from member_card where issuecard in (select id from member_channel where companyid =@menshiid)) ";
            try
            {
                if (country == "国家" || country == "全部")
                {
                }
                else
                {
                    if (province == "省市")
                    {
                        //按国家查询
                        sqlTxt += " and   wxcountry='" + country + "'  ";
                    }
                    else
                    {
                        if (province == "城市")
                        {
                            //按省市查询
                            sqlTxt += " and   wxprovince='" + province + "'  ";
                        }
                        else
                        {
                            //按城市查询
                            sqlTxt += " and   wxcity='" + city + "' ";
                        }
                    }
                }

                if (sex != "")
                {
                    if (sex != "0")
                    {
                        sqlTxt += " and weixin in (select openid from WxMemberBasic where sex='" + sex + "')";
                    }
                }

                if (tag != 0)//精确到兴趣标签
                {
                    sqlTxt += " and id in (select crmid from b2b_crm_interesttag where tagid=" + tag + ")";
                }
                else //精确到兴趣类型
                {
                    if (tagtype != 0)
                    {
                        sqlTxt += " and id in (select crmid from b2b_crm_interesttag where tagid in (select id from b2b_interesttag where tagtypeid=" + tagtype + ") )";
                    }
                }

                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@menshiid", menshiid);

                using (var reader = cmd.ExecuteReader())
                {
                    List<B2b_crm> list = new List<B2b_crm>();

                    while (reader.Read())
                    {
                        list.Add(new B2b_crm
                        {

                            Weixin = reader.GetValue<string>("weixin").ConvertTo<string>("")

                        });

                    }
                    return list;
                }
            }
            catch
            {
                return null;
            }
        }

        internal int CoverRegion(string openid, string Country, string Province, string City)
        {
            string sql = "update b2b_crm set wxcountry='" + Country + "',wxprovince='" + Province + "',wxcity='" + City + "' where weixin='" + openid + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal List<string> Getqunfaphone(int comid, out int total, string qunfanum = "-1")
        {
            string sql = "";
            if (qunfanum == "-1")
            {
                sql = "select phone from b2b_crm where weixin='' and phone!='' and com_id=" + comid;
            }
            else
            {
                if (qunfanum == "0")
                {
                    sql = "select phone from b2b_crm where weixin='' and phone!='' and com_id=" + comid + " and phone not in (select phone from b2b_invitecodesendlog)";
                }
                if (qunfanum == "1")
                {
                    sql = "select Phone from b2b_invitecodesendlog where phone in (Select phone From b2b_invitecodesendlog Group By phone Having Count(1)=1)  group by phone";

                }
                if (qunfanum == "2")
                {
                    sql = "select Phone from b2b_invitecodesendlog where phone in (Select phone From b2b_invitecodesendlog Group By phone Having Count(1)=2)  group by phone";

                }
            }
            try
            {
                if (sql == "")
                {
                    total = 0;
                    return null;
                }
                else
                {
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                    List<string> list = new List<string>();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetValue<string>("phone"));
                        }
                    }
                    total = list.Count;
                    return list;
                }
            }
            catch
            {
                total = 0;
                return null;
            }
        }

        internal bool IsCompanyCrm(string openid, int comid)
        {
            string sql = "select count(1) from b2b_crm where weixin='" + openid + "' and IDcard in ( " +
       " select Cardcode from Member_Card  where   IssueCard=(select id from Member_Channel where Issuetype=4 and Com_id=" + comid + ") ) ";

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                int count = int.Parse(o.ToString());
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return true;
            }
        }


        //通过用户ID ，获取渠道公司名称
        internal string GetMemberChannelcompanyByB2bCrmId(int crmid)
        {
            const string sqlTxt = @"select * from member_channel_company where id in (select companyid from  member_channel where id in(select issuecard from member_card where cardcode in (select idcard from b2b_crm where id=@crmid)))";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@crmid", crmid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<string>("companyname");
                }
                return "";

            }
        }

        //通过用户ID ，获取渠道公司名称
        internal int GetMemberChannelcompanyIdByB2bCrmId(int crmid)
        {
            const string sqlTxt = @"select * from member_channel_company where id in (select companyid from  member_channel where id in(select issuecard from member_card where cardcode in (select idcard from b2b_crm where id=@crmid)))";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@crmid", crmid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("id");
                }
                return 0;

            }
        }
        ////门市坐标
        internal string GetB2bchannelDistanceByid(int channelcomid)
        {
            const string sqlTxt = @"select * from member_channel_company where id =@channelcomid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@channelcomid", channelcomid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var companylocate = reader.GetValue<string>("companylocate");
                    if (companylocate != null)
                    {
                        return companylocate;
                    }
                    return "";

                }
                return "";
            }
        }


        ////用户坐标
        internal string GetB2bCrmDistanceByid(string openid)
        {
            const string sqlTxt = @"select * from b2b_crm_location where weixin=@openid and weixin !='' ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@openid", openid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<string>("longitude") + "," + reader.GetValue<string>("latitude");
                }
                return "";
            }
        }
        ////项目坐标
        internal string GetProjDistanceByid(int projid)
        {
            const string sqlTxt = @"select * from b2b_com_project where id=@projid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@projid", projid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<string>("coordinate");
                }
                return "";
            }
        }

        ////门市坐标
        internal string GetChannelDistanceByid(int channelid)
        {
            const string sqlTxt = @"select companylocate from Member_Channel_company where id =@channelid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@channelid", channelid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<string>("companylocate");
                }
                return "";
            }
        }

        ////计算距离,返回公里数
        internal double CalculateTheCoordinates(string n1, string e1, string n2, string e2)
        {

            try
            {
                var dinates = CommonFunc.Distance(double.Parse(n1), double.Parse(e1), double.Parse(n2), double.Parse(e2));

                dinates = Math.Round(dinates, 0);
                return dinates;
            }
            catch
            {
                return 0;
            }

        }



        internal string Getweixinbycrmid(int crmid)
        {
            string sql = " select weixin from b2b_crm  where id=" + crmid;
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o.ToString();
            }
            catch
            {
                sqlHelper.Dispose();
                return "";
            }
        }

        internal int RecordInteracttime(int comid, string weixin, DateTime dateTime)
        {
            string sql = "update b2b_crm set wxlastinteracttime='" + dateTime + "' where weixin='" + weixin + "' and com_id=" + comid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int UpWxsubstate(int comid, string weixin, int whetherwxfocus, int whetheractivate)
        {
            string sql = "update b2b_crm set whetherwxfocus='" + whetherwxfocus + "',whetheractivate =" + whetheractivate + " where weixin='" + weixin + "' and com_id=" + comid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int Adjust_dengjifen(B2bcrm_dengjifenlog djflog, int id, int comid, decimal dengjifen)
        {
            try
            {
                int is_correct = 1;

                sqlHelper.BeginTrancation();
                //判读商户是否设置了 用户级别
                int crmlevelscount = new B2bcrmlevelsData().Getcrmlevelscount(comid);
                if (crmlevelscount == 0)
                {
                    is_correct = 0;
                }
                else
                {
                    //记入等积分变动日志
                    if (djflog.id == 0)
                    {
                        string sql = @"INSERT INTO [EtownDB].[dbo].[b2bcrm_dengjifenlog]
           ([crmid]
           ,[dengjifen]
           ,[ptype]
           ,[opertor]
           ,[opertime]
           ,[orderid]
           ,[ordername]
           ,[remark])
     VALUES
           (@crmid
           ,@dengjifen
           ,@ptype
           ,@opertor
           ,@opertime
           ,@orderid
           ,@ordername
           ,@remark);select @@identity;";
                        var cmd0 = sqlHelper.PrepareTextSqlCommand(sql);
                        cmd0.AddParam("@crmid", djflog.crmid);
                        cmd0.AddParam("@dengjifen", djflog.dengjifen);
                        cmd0.AddParam("@ptype", djflog.ptype);
                        cmd0.AddParam("@opertor", djflog.opertor);
                        cmd0.AddParam("@opertime", djflog.opertime);
                        cmd0.AddParam("@orderid", djflog.orderid);
                        cmd0.AddParam("@ordername", djflog.ordername);
                        cmd0.AddParam("@remark", djflog.remark);

                        cmd0.ExecuteNonQuery();
                    }

                    string sql1 = "select dengjifen from b2b_crm where id=" + id;
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql1);
                    object o = cmd.ExecuteScalar();
                    decimal djf = o == null ? 0 : decimal.Parse(o.ToString());

                    decimal djf_now = djf + decimal.Parse(dengjifen.ToString());
                    if (djf_now < 0)
                    {
                        djf_now = 0;
                    }

                    string sql2 = "update b2b_crm set dengjifen=" + djf_now + "  where id=" + id;
                    cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                    cmd.ExecuteNonQuery();

                    if (dengjifen > 0)
                    {
                        //得到等积分变化前对应的级别id
                        string sql3 = "select id from B2bcrmlevels where crmlevel=(select crmlevel from b2b_crm where id=" + id + ") and com_id=" + comid;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql3);
                        o = cmd.ExecuteScalar();
                        int last_id = o == null ? 0 : int.Parse(o.ToString());

                        //判断等积分变化后对应的级别id
                        string sql4 = "select id from B2bcrmlevels where dengjifen_begin<=" + djf_now + " and dengjifen_end>=" + djf_now + "  and com_id=" + comid;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql4);
                        o = cmd.ExecuteScalar();
                        int now_id = o == null ? 0 : int.Parse(o.ToString());

                        //如果等积分变化后 会员级别升高了
                        if (now_id > last_id)
                        {
                            //修改会员级别
                            string sql5 = "update b2b_crm set crmlevel=(select crmlevel from B2bcrmlevels where id=" + now_id + ") where id=" + id;
                            cmd = sqlHelper.PrepareTextSqlCommand(sql5);
                            cmd.ExecuteNonQuery();

                            //得到会员变化后的级别
                            string sql6 = "select crmlevel from B2bcrmlevels where dengjifen_begin<=" + djf_now + " and dengjifen_end>=" + djf_now + "  and com_id=" + comid;
                            cmd = sqlHelper.PrepareTextSqlCommand(sql6);
                            o = cmd.ExecuteScalar();
                            string now_crmlevel = o == null ? "" : o.ToString();

                            //记录会员级别 变动情况
                            string sql7 = "insert into b2bcrm_levelchangelog (crmid,crmlevel,changetime,comid,remark) values(" + id + ",'" + now_crmlevel + "','" + DateTime.Now + "'," + comid + ",'" + djflog.remark + "')";
                            cmd = sqlHelper.PrepareTextSqlCommand(sql7);
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
                sqlHelper.Commit();
                sqlHelper.Dispose();
                return is_correct;
            }
            catch
            {
                sqlHelper.Rollback();
                sqlHelper.Dispose();
                return 0;
            }

        }

        internal B2b_crm_location GetGuwenLocationByVipweixin(string openid, int comid)
        {
            if (openid == "" || openid == null)
            {
                return null;
            }
            try
            {
                string sql = "select * from B2b_crm_location where weixin=( select weixin from b2b_crm  where com_id=" + comid + " and phone!='' and phone=(select mobile from member_channel where  com_id=" + comid + " and  id =(select issuecard from member_card where com_id=" + comid + " and  cardcode=(select idcard from b2b_crm where weixin='" + openid + "' and weixin!=''))))";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                B2b_crm_location m = null;
                using (var read = cmd.ExecuteReader())
                {
                    if (read.Read())
                    {
                        m = new B2b_crm_location
                        {
                            Id = read.GetValue<int>("id"),
                            Comid = read.GetValue<int>("Comid"),
                            Weixin = read.GetValue<string>("weixin"),
                            Latitude = read.GetValue<string>("Latitude"),
                            Longitude = read.GetValue<string>("Longitude"),
                            Createtime = read.GetValue<string>("Createtime"),
                            Createtimeformat = read.GetValue<DateTime>("Createtimeformat"),
                        };
                    }
                    return m;
                }
            }
            catch
            {
                return null;
            }
        }

        internal B2b_crm_location GetB2bcrmlocationByopenid(string openid)
        {
            string sql = "select * from B2b_crm_location where weixin= '" + openid + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            B2b_crm_location m = null;
            using (var read = cmd.ExecuteReader())
            {
                if (read.Read())
                {
                    m = new B2b_crm_location
                    {
                        Id = read.GetValue<int>("id"),
                        Comid = read.GetValue<int>("Comid"),
                        Weixin = read.GetValue<string>("weixin"),
                        Latitude = read.GetValue<string>("Latitude"),
                        Longitude = read.GetValue<string>("Longitude"),
                        Createtime = read.GetValue<string>("Createtime"),
                        Createtimeformat = read.GetValue<DateTime>("Createtimeformat"),
                    };
                }
                return m;
            }
        }

        internal int UpFreelandingAccount(string openid, string freelandingaccount)
        {
            string sql = "update b2b_crm set freelandingagentaccount='" + freelandingaccount + "' where weixin='" + openid + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int GetComidByOpenid(string openid)
        {
            string sql = "select com_id from b2b_crm where weixin='" + openid + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int comid = 0;
                if (reader.Read())
                {
                    comid = reader.GetValue<int>("com_id");
                }
                return comid;
            }
        }

        //type=1 图片 ，type=2 别名
        internal string GetNameorImgByid(int id,int type)
        {
            string sql = "select * from wxmemberbasic as a right join b2b_crm as b on a.openid=b.weixin where b.id=" + id + "";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                string retscr = "";
                if (reader.Read())
                {
                    if (type == 1) {
                        retscr = reader.GetValue<string>("headimgurl");
                        
                    }
                    if (type == 2)
                    {
                        retscr = reader.GetValue<string>("nickname");
                    }
                }
                return retscr;
            }
        }
       

    }
}
