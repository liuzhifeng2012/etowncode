using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using System.Data;
using ETS2.Member.Service.MemberService.Model;
using ETS.Framework;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class InternalB2bCompanyInfo
    {
        private SqlHelper sqlHelper;
        public InternalB2bCompanyInfo(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        #region 添加或者编辑公司扩展信息

        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateB2bCompanyInfo";
        public int InsertOrUpdate(B2b_company_info model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Com_id", model.Com_id);
            cmd.AddParam("@Com_city", model.Com_city);
            cmd.AddParam("@Province", model.Province);
            cmd.AddParam("@Com_add", model.Com_add);
            cmd.AddParam("@Com_class", model.Com_class);
            cmd.AddParam("@Com_code", model.Com_code);
            cmd.AddParam("@Com_sitecode", model.Com_sitecode);
            cmd.AddParam("@Com_license", model.Com_license);
            cmd.AddParam("@Sale_Agreement", model.Sale_Agreement);
            cmd.AddParam("@Agent_Agreement", model.Agent_Agreement);
            cmd.AddParam("@Scenic_address", model.Scenic_address);
            cmd.AddParam("@Scenic_intro", model.Scenic_intro);
            cmd.AddParam("@Scenic_Takebus", model.Scenic_Takebus);
            cmd.AddParam("@Scenic_Drivingcar", model.Scenic_Drivingcar);
            cmd.AddParam("@Contact", model.Contact);
            cmd.AddParam("@Tel", model.Tel);
            cmd.AddParam("@Phone", model.Phone);
            cmd.AddParam("@Qq", model.Qq);
            cmd.AddParam("@Email", model.Email);
            cmd.AddParam("@Defaultprint", model.Defaultprint);

            cmd.AddParam("@Serviceinfo", model.Serviceinfo);
            cmd.AddParam("@Coordinate", model.Coordinate);
            cmd.AddParam("@Coordinatesize", model.Coordinatesize);
            cmd.AddParam("@Domainname", model.Domainname);
            cmd.AddParam("@Admindomain", model.Admindomain);

            cmd.AddParam("@Merchant_intro", model.Merchant_intro);

            //微信
            cmd.AddParam("@weixinimg", model.Weixinimg);
            cmd.AddParam("@weixinname", model.Weixinname);
            cmd.AddParam("@hasinnerchannel", model.HasInnerChannel);

            cmd.AddParam("@wl_PartnerId", model.wl_PartnerId);
            cmd.AddParam("@wl_userkey", model.wl_userkey);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion
        #region 得到公司扩展信息
        internal B2b_company_info GetCompanyInfo(int comid)
        {
            const string sqltext = @"SELECT [id]
      ,[com_id]
      ,[com_city]
      ,[com_add]
      ,[com_class]
      ,[com_code]
      ,[com_sitecode]
      ,[com_license]
      ,[sale_Agreement]
      ,[agent_Agreement]
      ,[Scenic_address]
      ,[Scenic_intro]
      ,[Scenic_Takebus]
      ,[Scenic_Drivingcar]
      ,[Contact]
      ,[tel]
      ,[phone]
      ,[qq]
      ,[email]
      ,[Defaultprint]
      ,[serviceinfo]
      ,[coordinate]
      ,[coordinatesize]
      ,[domainname]
      ,admindomain
      ,[merchant_intro],
      weixinimg,
      weixinname,
      hasinnerchannel,
      com_province,
      wxfocus_author,
wxfocus_url,
wl_PartnerId,
wl_userkey,
istransfer_customer_service
  FROM  [dbo].[b2b_company_info] where com_id=@com_id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltext);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@com_id", comid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_info cominfo = null;
                while (reader.Read())
                {
                    cominfo = new B2b_company_info
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Com_add = reader.GetValue<string>("com_add"),
                        Com_city = reader.GetValue<string>("com_city"),
                        Province = reader.GetValue<string>("com_Province"),
                        Com_class = reader.GetValue<int>("com_class"),
                        Com_code = reader.GetValue<string>("com_code"),
                        Com_license = reader.GetValue<string>("com_license"),
                        Com_sitecode = reader.GetValue<string>("com_sitecode"),
                        Contact = reader.GetValue<string>("contact"),
                        Agent_Agreement = reader.GetValue<string>("agent_agreement"),
                        Defaultprint = reader.GetValue<string>("defaultprint"),
                        Email = reader.GetValue<string>("email"),
                        Phone = reader.GetValue<string>("phone"),
                        Qq = reader.GetValue<string>("qq"),
                        Sale_Agreement = reader.GetValue<string>("sale_agreement"),
                        Scenic_address = reader.GetValue<string>("scenic_address"),
                        Scenic_Drivingcar = reader.GetValue<string>("scenic_drivingcar"),
                        Scenic_intro = reader.GetValue<string>("scenic_intro"),
                        Scenic_Takebus = reader.GetValue<string>("scenic_takebus"),
                        Tel = reader.GetValue<string>("tel"),
                        Serviceinfo = reader.GetValue<string>("serviceinfo"),
                        Coordinate = reader.GetValue<string>("coordinate"),
                        Coordinatesize = reader.GetValue<int>("coordinatesize"),
                        Domainname = reader.GetValue<string>("domainname"),
                        Admindomain = reader.GetValue<string>("admindomain"),
                        Merchant_intro = reader.GetValue<string>("merchant_intro"),
                        Weixinimg = reader.GetValue<string>("weixinimg"),
                        Weixinname = reader.GetValue<string>("weixinname"),
                        HasInnerChannel = reader.GetValue<bool>("hasinnerchannel").ToString().ConvertTo<bool>(true),
                        Wxfocus_author = reader.GetValue<string>("wxfocus_author"),
                        Wxfocus_url = reader.GetValue<string>("wxfocus_url"),
                        wl_PartnerId = reader.GetValue<string>("wl_PartnerId"),
                        wl_userkey = reader.GetValue<string>("wl_userkey"),
                        Istransfer_customer_service = reader.GetValue<int>("istransfer_customer_service")
                    };

                }

                sqlHelper.Dispose();
                return cominfo;
            }

        }
        #endregion


        #region 得到公司产品的数量
        internal int GetCompanyProCount(int comid)
        {
            const string sqltext = @"SELECT count(id) as num FROM  [dbo].[b2b_com_pro] where com_id=@com_id and pro_state=1";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltext);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@com_id", comid);
            using (var reader = cmd.ExecuteReader())
            {
                int cominfo = 0;
                if (reader.Read())
                {

                    return reader.GetValue<int>("num");

                }
                return cominfo;
            }

        }
        #endregion

        #region 得到公司成交笔数
        internal int GetCompanyChengjiaoCount(int comid)
        {
            const string sqltext = @"SELECT count(id) as num FROM  [dbo].[b2b_order] where comid=@com_id ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltext);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@com_id", comid);
            using (var reader = cmd.ExecuteReader())
            {
                int cominfo = 0;
                if (reader.Read())
                {

                    return reader.GetValue<int>("num");

                }
                return cominfo;
            }

        }
        #endregion


        #region 得到公司本周成交笔数
        internal int GetCompanyWeekChengjiaoCount(int comid)
        {
            string week7day = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");

            const string sqltext = @"SELECT count(id) as num FROM  [dbo].[b2b_order] where comid=@com_id and u_subdate>=@week7day";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltext);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@com_id", comid);
            cmd.AddParam("@week7day", week7day);
            using (var reader = cmd.ExecuteReader())
            {
                int cominfo = 0;
                if (reader.Read())
                {

                    return reader.GetValue<int>("num");

                }
                return cominfo;
            }

        }
        #endregion
        #region 修改商家资质信息
        internal int Modifyzizhi(int comextid, string comcode, string comsitecode, string comlicence, string scenic_intro = "", string domainname = "")
        {
            string sqlTxt = @"update b2b_company_info set com_license=@comlicense,scenic_intro=@scenic_intro,domainname=@domainname,weixinname=@comsitecode,com_code=@comcode where  com_id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@id", comextid);
            cmd.AddParam("@scenic_intro", scenic_intro);
            cmd.AddParam("@domainname", domainname);
            cmd.AddParam("@comlicense", comlicence);
            cmd.AddParam("@comsitecode", comsitecode);
            cmd.AddParam("@comcode", comcode);


            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region 修改商家资质信息
        internal int UpdateB2bCompanyName(int comid, string comname)
        {
            string sqlTxt = @"update b2b_company set Com_name=@comname where  id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@id", comid);
            cmd.AddParam("@comname", comname);

            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region 修改商家资质信息
        internal int UpdateB2bCompanySearchset(int comid, int setsearch)
        {
            string sqlTxt = @"update b2b_company set setsearch=@setsearch where  id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@id", comid);
            cmd.AddParam("@setsearch", setsearch);

            return cmd.ExecuteNonQuery();
        }
        #endregion



        #region 修改公司授权和协议信息
        internal int ModifyComShouquan(int comextid, string sale_Agreement, string agent_Agreement)
        {
            string sqlTxt = @"update b2b_company_info set sale_Agreement=@saleagreement , agent_Agreement=@agentagreement where id=@id ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@id", comextid);
            cmd.AddParam("@saleagreement", sale_Agreement);
            cmd.AddParam("@agentagreement", agent_Agreement);


            return cmd.ExecuteNonQuery();
        }
        #endregion
        #region 修改绑定打印机信息
        internal int ModifyBangPrint(int comextid, string Defaultprint)
        {
            string sqlTxt = @"update b2b_company_info set Defaultprint=@Defaultprint where id=@id ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@id", comextid);
            cmd.AddParam("@Defaultprint", Defaultprint);
            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region 获取特定公司的pos列表
        internal List<B2b_company_info> PosList(string comid, int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_company_pos";
            var strGetFields = "*";
            var sortKey = "com_id";
            var sortMode = "0";
            var condition = "com_id=" + comid;
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_company_info> list = new List<B2b_company_info>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company_info
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Posid = reader.GetValue<int>("Posid"),
                        Poscompany = reader.GetValue<string>("poscompany"),
                        BindingTime = reader.GetValue<DateTime>("bindingTime"),
                        Admin = reader.GetValue<string>("admin"),
                        Remark = reader.GetValue<string>("remark"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;

        }
        #endregion
        internal List<B2b_company_info> PosList(int pageindex, int pagesize, out int totalcount, string key = "")
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            var condition = "";
            if (key != "")
            {
                try
                {
                    int tkey = int.Parse(key);
                    condition += " poscompany like '%" + key + "%' or  posid='" + key + "'";
                }
                catch 
                {
                    condition += " poscompany like '%" + key + "%'";
                }
               
            }
            cmd.PagingCommand1("b2b_company_pos", "*", "id desc", "", pagesize, pageindex, "", condition);


            List<B2b_company_info> list = new List<B2b_company_info>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company_info
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Posid = reader.GetValue<int>("Posid"),
                        Poscompany = reader.GetValue<string>("poscompany"),
                        BindingTime = reader.GetValue<DateTime>("bindingTime"),
                        Admin = reader.GetValue<string>("admin"),
                        Remark = reader.GetValue<string>("remark"),
                        Projectid = reader.GetValue<int>("Projectid"),
                        md5key = reader.GetValue<string>("md5key"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;

        }
        #region 新绑定POS信息
        internal int ModifyBangPos(int posid, string poscompany, int com_id, int userid, string remark, int pos_id, string md5key, int projectid = 0)
        {
            string sqlTxt = "";
            if (pos_id == 0)
            {
                sqlTxt = @"insert b2b_company_pos (com_id,poscompany,posid,admin,remark,projectid,md5key) values(@com_id,@poscompany,@posid,@admin,@remark,@projectid,@md5key) ";
            }
            else
            {
                sqlTxt = @"update b2b_company_pos set com_id=@com_id,poscompany=@poscompany,posid=@posid,admin=@admin,remark=@remark,projectid=@projectid,md5key=@md5key where id=@id";
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@id", pos_id);
            cmd.AddParam("@com_id", com_id);
            cmd.AddParam("@poscompany", poscompany);
            cmd.AddParam("@posid", posid);
            cmd.AddParam("@admin", userid);
            cmd.AddParam("@remark", remark);
            cmd.AddParam("@projectid", projectid);
            cmd.AddParam("@md5key", md5key);
            return cmd.ExecuteNonQuery();
        }
        #endregion
        #region 获取特定公司的pos详细信息
        internal List<B2b_company_info> PosInfo(int pos_id)
        {
            string sqlTxt = @"select * from b2b_company_pos where id=@id order by id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@id", pos_id);

            List<B2b_company_info> list = new List<B2b_company_info>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_company_info
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Posid = reader.GetValue<int>("Posid"),
                        Poscompany = reader.GetValue<string>("poscompany"),
                        BindingTime = reader.GetValue<DateTime>("bindingTime"),
                        Admin = reader.GetValue<string>("admin"),
                        Remark = reader.GetValue<string>("remark"),
                        Projectid = reader.GetValue<int>("Projectid"),
                        md5key = reader.GetValue<string>("md5key")
                    });

                }
            }

            return list;

        }
        #endregion


        #region 获取特定公司的pos详细信息
        internal B2b_company_info PosInfobyposid(int pos_id)
        {
            string sqlTxt = @"select * from b2b_company_pos where Posid=@pos_id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@pos_id", pos_id);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return new B2b_company_info
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Posid = reader.GetValue<int>("Posid"),
                        Poscompany = reader.GetValue<string>("poscompany"),
                        BindingTime = reader.GetValue<DateTime>("bindingTime"),
                        Admin = reader.GetValue<string>("admin"),
                        Remark = reader.GetValue<string>("remark"),
                        Projectid = reader.GetValue<int>("Projectid"),

                    };

                }
            }

            return null;

        }
        #endregion

        #region 编辑短信
        internal int Insertnote(string key, string content, string title, bool radio, int note_id)
        {
            string sqltxt = "";
            if (note_id == 0)
            {
                sqltxt = @"insert into member_sms(sms_key,title,Remark,subdate,ip,openstate)values(@key,@title,@remark,@subdate,@ip,@openstate)";
            }
            else
            {
                sqltxt = @"update member_sms set sms_key=@key,title=@title,Remark=@remark,openstate=@openstate,subdate=@subdate,ip=@ip
                           where id=@id";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@key", key);
            cmd.AddParam("@remark", content);
            cmd.AddParam("@title", title);
            cmd.AddParam("@subdate", DateTime.Now);
            cmd.AddParam("@ip", System.Web.HttpContext.Current.Request.UserHostAddress);
            cmd.AddParam("@openstate", radio);
            cmd.AddParam("@id", note_id);

            return cmd.ExecuteNonQuery();
        }

        //显示列表
        internal List<Member_sms> notList(string comid, int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "Member_sms";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "";
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, "");


            List<Member_sms> list = new List<Member_sms>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_sms
                    {
                        Id = reader.GetValue<int>("id"),
                        Sms_key = reader.GetValue<string>("sms_key"),
                        Title = reader.GetValue<string>("title"),
                        Remark = reader.GetValue<string>("remark"),
                        Openstate = reader.GetValue<bool>("openstate"),
                        Subdate = reader.GetValue<DateTime>("subdate"),
                        Ip = reader.GetValue<string>("ip")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;

        }

        internal List<Member_sms> noteInfo(int note_id)
        {
            string sqlTxt = "";
            sqlTxt = @"select * from Member_sms where id=@id order by id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@id", note_id);

            List<Member_sms> list = new List<Member_sms>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Member_sms
                    {
                        Id = reader.GetValue<int>("id"),
                        Sms_key = reader.GetValue<string>("sms_key"),
                        Title = reader.GetValue<string>("title"),
                        Remark = reader.GetValue<string>("remark"),
                        Openstate = reader.GetValue<bool>("openstate"),
                        Subdate = reader.GetValue<DateTime>("subdate"),
                        Ip = reader.GetValue<string>("ip")
                    });

                }
            }

            return list;

        }
        #endregion

        #region  删除短信
        internal int Delnote(int id, string key)
        {
            string sqltxt = "";
            sqltxt = @"DELETE FROM [EtownDB].[dbo].[member_sms] WHERE id=@id and sms_key=@sms_key";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", id);
            cmd.AddParam("@sms_key", key);

            return cmd.ExecuteNonQuery();
        }
        #endregion

        internal B2b_company_info GetB2bCompanyInfoByDomainname(string Domainname)
        {
            const string sqltext = @"SELECT [id]
      ,[com_id]
      ,[com_city]
      ,[com_add]
      ,[com_class]
      ,[com_code]
      ,[com_sitecode]
      ,[com_license]
      ,[sale_Agreement]
      ,[agent_Agreement]
      ,[Scenic_address]
      ,[Scenic_intro]
      ,[Scenic_Takebus]
      ,[Scenic_Drivingcar]
      ,[Contact]
      ,[tel]
      ,[phone]
      ,[qq]
      ,[email]
      ,[Defaultprint]
      ,[serviceinfo]
      ,[coordinate]
      ,[coordinatesize]
      ,[domainname]
      ,admindomain
,com_Province
  FROM [dbo].[b2b_company_info] where domainname=@domainname";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltext);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@domainname", Domainname);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_info cominfo = null;
                while (reader.Read())
                {
                    cominfo = new B2b_company_info
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Com_add = reader.GetValue<string>("com_add"),
                        Com_city = reader.GetValue<string>("com_city"),
                        Province = reader.GetValue<string>("com_Province"),
                        Com_class = reader.GetValue<int>("com_class"),
                        Com_code = reader.GetValue<string>("com_code"),
                        Com_license = reader.GetValue<string>("com_license"),
                        Com_sitecode = reader.GetValue<string>("com_sitecode"),
                        Contact = reader.GetValue<string>("contact"),
                        Agent_Agreement = reader.GetValue<string>("agent_agreement"),
                        Defaultprint = reader.GetValue<string>("defaultprint"),
                        Email = reader.GetValue<string>("email"),
                        Phone = reader.GetValue<string>("phone"),
                        Qq = reader.GetValue<string>("qq"),
                        Sale_Agreement = reader.GetValue<string>("sale_agreement"),
                        Scenic_address = reader.GetValue<string>("scenic_address"),
                        Scenic_Drivingcar = reader.GetValue<string>("scenic_drivingcar"),
                        Scenic_intro = reader.GetValue<string>("scenic_intro"),
                        Scenic_Takebus = reader.GetValue<string>("scenic_takebus"),
                        Tel = reader.GetValue<string>("tel"),
                        Serviceinfo = reader.GetValue<string>("serviceinfo"),
                        Coordinate = reader.GetValue<string>("coordinate"),
                        Coordinatesize = reader.GetValue<int>("coordinatesize"),
                        Domainname = reader.GetValue<string>("domainname"),
                        Admindomain = reader.GetValue<string>("admindomain")
                    };

                }
                return cominfo;
            }
        }

        //后台显示屏蔽 列表
        internal List<B2b_company_info> ComList(int pageindex, int pagesize, int comstate, out int totalcount, string key = "")
        {

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = " 1=1 ";

            if (key != "")
            {
                condition += " and  a.com_id in (select id from b2b_company where (com_name like '%" + key + "%') or (scenic_name like '%" + key + "%'))";
            }
            else
            {
                if (comstate != 0)
                {
                    condition += " and a.com_id  in (select id from b2b_company where com_state=" + comstate + ")";
                }
            }
            cmd.PagingCommand1("b2b_company_info a " +
  " left join (select sum(abs(money)) as SjZxsk,com_id   from b2b_finance where payment_type='直销收款' group by com_id) as b" +
  " on a.com_id=b.com_id" +
  " left join (select sum(abs(money)) as SjZxtp,com_id   from b2b_finance where payment_type='直销退票' group by com_id) as c" +
  " on a.com_id=c.com_id" +
  " left join (select sum(abs(money)) as SjTx,com_id   from b2b_finance where payment_type='商家提现' group by com_id) as d" +
  " on a.com_id=d.com_id" +
   " left join (select sum(abs(money)) as SjZsjalipay,com_id   from b2b_finance where payment_type='转商家支付宝' group by com_id) as e" +
  " on a.com_id=e.com_id" +
   " left join (select sum(abs(money)) as SjSxf,com_id   from b2b_finance where payment_type='手续费' group by com_id) as f" +
  " on a.com_id=f.com_id", "a.*,b.SjZxsk,c.SjZxtp,d.SjTx,e.SjZsjalipay,f.SjSxf", "a.sortid", "", pagesize, pageindex, "", condition);

            List<B2b_company_info> list = new List<B2b_company_info>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company_info
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Com_add = reader.GetValue<string>("com_add"),
                        Com_city = reader.GetValue<string>("com_city"),
                        Province = reader.GetValue<string>("com_Province"),
                        Com_class = reader.GetValue<int>("com_class"),
                        Com_code = reader.GetValue<string>("com_code"),
                        Com_license = reader.GetValue<string>("com_license"),
                        Com_sitecode = reader.GetValue<string>("com_sitecode"),
                        Contact = reader.GetValue<string>("contact"),
                        Agent_Agreement = reader.GetValue<string>("agent_agreement"),
                        Defaultprint = reader.GetValue<string>("defaultprint"),
                        Email = reader.GetValue<string>("email"),
                        Phone = reader.GetValue<string>("phone"),
                        Qq = reader.GetValue<string>("qq"),
                        Sale_Agreement = reader.GetValue<string>("sale_agreement"),
                        Scenic_address = reader.GetValue<string>("scenic_address"),
                        Scenic_Drivingcar = reader.GetValue<string>("scenic_drivingcar"),
                        Scenic_intro = reader.GetValue<string>("scenic_intro"),
                        Scenic_Takebus = reader.GetValue<string>("scenic_takebus"),
                        Tel = reader.GetValue<string>("tel"),
                        Serviceinfo = reader.GetValue<string>("serviceinfo"),
                        Coordinate = reader.GetValue<string>("coordinate"),
                        Coordinatesize = reader.GetValue<int>("coordinatesize"),
                        Domainname = reader.GetValue<string>("domainname"),
                        Admindomain = reader.GetValue<string>("admindomain"),
                        Info_state = reader.GetValue<int>("info_state"),
                        HasInnerChannel = reader.GetValue<bool>("hasinnerchannel").ToString().ConvertTo<bool>(false),

                        SjZxsk = reader.GetValue<decimal>("SjZxsk").ToString().ConvertTo<decimal>(0),
                        SjZxtp = reader.GetValue<decimal>("SjZxtp").ToString().ConvertTo<decimal>(0),
                        SjTx = reader.GetValue<decimal>("SjTx").ToString().ConvertTo<decimal>(0),
                        SjZsjalipay = reader.GetValue<decimal>("SjZsjalipay").ToString().ConvertTo<decimal>(0),
                        SjSxf = reader.GetValue<decimal>("SjSxf").ToString().ConvertTo<decimal>(0)

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }

        //后台显示屏蔽 修改
        internal string UpCom(int id, string state)
        {
            int statebool = 0;
            if (state == "已屏蔽")
            {
                statebool = 1;
            }
            if (state == "已显示")
            {
                statebool = 0;
            }


            var sqlTxt = @"update b2b_company_info set info_state=@info_state where com_id=@com_id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@info_state", statebool);
            cmd.AddParam("@com_id", id);

            cmd.ExecuteNonQuery();

            return "OK";
        }

        #region  商家详细列表
        internal List<B2b_company_info> ComPageList(int pageindex, int pagesize, int prostate, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_company_info";
            var strGetFields = "*";
            var sortKey = "sortid";
            var sortMode = "0";
            var condition = "info_state=" + prostate;
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_company_info> list = new List<B2b_company_info>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company_info
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Com_add = reader.GetValue<string>("com_add"),
                        Com_city = reader.GetValue<string>("com_city"),
                        Province = reader.GetValue<string>("com_Province"),
                        Com_class = reader.GetValue<int>("com_class"),
                        Com_code = reader.GetValue<string>("com_code"),
                        Com_license = reader.GetValue<string>("com_license"),
                        Com_sitecode = reader.GetValue<string>("com_sitecode"),
                        Contact = reader.GetValue<string>("contact"),
                        Agent_Agreement = reader.GetValue<string>("agent_agreement"),
                        Defaultprint = reader.GetValue<string>("defaultprint"),
                        Email = reader.GetValue<string>("email"),
                        Phone = reader.GetValue<string>("phone"),
                        Qq = reader.GetValue<string>("qq"),
                        Sale_Agreement = reader.GetValue<string>("sale_agreement"),
                        Scenic_address = reader.GetValue<string>("scenic_address"),
                        Scenic_Drivingcar = reader.GetValue<string>("scenic_drivingcar"),
                        Scenic_intro = reader.GetValue<string>("scenic_intro"),
                        Scenic_Takebus = reader.GetValue<string>("scenic_takebus"),
                        Tel = reader.GetValue<string>("tel"),
                        Serviceinfo = reader.GetValue<string>("serviceinfo"),
                        Coordinate = reader.GetValue<string>("coordinate"),
                        Coordinatesize = reader.GetValue<int>("coordinatesize"),
                        Domainname = reader.GetValue<string>("domainname"),
                        Admindomain = reader.GetValue<string>("admindomain"),
                        Info_state = reader.GetValue<int>("info_state")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        #endregion

        #region  商家详细列表
        internal List<B2b_company_info> ComSelectpagelist(int prostate, int pageindex, int pagesize, string key, out int totalcount, int proclass = 0)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "View_b2b_cmopany_select";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "0";
            var condition = "info_state=" + prostate;

            if (key != "")
            {
                condition += " and (com_name like '%" + key + "%' or com_id in (select com_id from b2b_company_info where com_city='" + key + "' or com_Province='" + key + "'))";
            }


            if (proclass != 0)
            {//按类目查询
                condition += " and com_id in (select com_id from b2b_com_pro where id in (select proid from b2b_com_pro_class where classid=" + proclass + " ))";
            }


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_company_info> list = new List<B2b_company_info>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company_info
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Com_add = reader.GetValue<string>("com_add"),
                        Com_city = reader.GetValue<string>("com_city"),
                        Com_class = reader.GetValue<int>("com_class"),
                        Com_code = reader.GetValue<string>("com_code"),
                        Com_license = reader.GetValue<string>("com_license"),
                        Com_sitecode = reader.GetValue<string>("com_sitecode"),
                        Contact = reader.GetValue<string>("contact"),
                        Agent_Agreement = reader.GetValue<string>("agent_agreement"),
                        Defaultprint = reader.GetValue<string>("defaultprint"),
                        Email = reader.GetValue<string>("email"),
                        Phone = reader.GetValue<string>("phone"),
                        Qq = reader.GetValue<string>("qq"),
                        Sale_Agreement = reader.GetValue<string>("sale_agreement"),
                        Scenic_address = reader.GetValue<string>("scenic_address"),
                        Scenic_Drivingcar = reader.GetValue<string>("scenic_drivingcar"),
                        Scenic_intro = reader.GetValue<string>("scenic_intro"),
                        Scenic_Takebus = reader.GetValue<string>("scenic_takebus"),
                        Tel = reader.GetValue<string>("tel"),
                        Serviceinfo = reader.GetValue<string>("serviceinfo"),
                        Coordinate = reader.GetValue<string>("coordinate"),
                        Coordinatesize = reader.GetValue<int>("coordinatesize"),
                        Domainname = reader.GetValue<string>("domainname"),
                        //Admindomain = reader.GetValue<string>("admindomain"),
                        Info_state = reader.GetValue<int>("info_state")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        #endregion

        internal string UpComstate(int id, string state)
        {
            int statebool = 0;
            if (state == "已开通")
            {
                statebool = 2;
            }
            if (state == "已暂停")
            {
                statebool = 1;
            }


            var sqlTxt = @"update b2b_company set com_state=@com_state where id=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@com_state", statebool);
            cmd.AddParam("@id", id);

            cmd.ExecuteNonQuery();

            return "OK";
        }

        internal int SortCom(string comid, int sortid)
        {
            string sql = "update b2b_company_info set sortid=@sortid where com_id =@comid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@sortid", sortid);

            return cmd.ExecuteNonQuery();
        }

        internal int AdjustHasInnerChannel(int companyinfoid, string hasinnerchannel)
        {
            string sql = "update b2b_company_info set hasinnerchannel=@hasinnerchannel  where com_id =@infoid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@hasinnerchannel", hasinnerchannel);
            cmd.AddParam("@infoid", companyinfoid);

            return cmd.ExecuteNonQuery();
        }

        internal string Getmd5keybyposid(string pos_id)
        {
            try
            {
                string sql = "select md5key from b2b_company_pos where posid='" + pos_id + "'";
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

        internal int Editjoinpolicy(int comid, string joinpolicy)
        {
            string sql = "update b2b_company_info set agent_joinpolicy='" + joinpolicy + "' where com_id=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            return cmd.ExecuteNonQuery();
        }
        internal string Getjoinpolicy(int comid)
        {
            try
            {
                string sql = "select agent_joinpolicy from b2b_company_info where com_id=" + comid;
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
    }
}
