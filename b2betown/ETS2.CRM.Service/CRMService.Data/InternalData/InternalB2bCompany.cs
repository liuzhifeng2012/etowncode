using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using System.Data;
using ETS.Framework;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{

    public class InternalB2bCompany
    {
        private SqlHelper sqlHelper;
        public InternalB2bCompany(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        #region 添加或者编辑公司基本信息

        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateB2bCompany";

        public int InsertOrUpdate(B2b_company model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
            cmd.AddParam("@Id", model.ID);
            cmd.AddParam("@Com_name", model.Com_name);
            cmd.AddParam("@Scenic_name", model.Scenic_name);
            cmd.AddParam("@Com_type", model.Com_type);
            cmd.AddParam("@Com_state", model.Com_state);
            cmd.AddParam("@Imprest", model.Imprest);//修改不修改此参数

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

        #region 根据商家标识列得到商家基本信息
        /// <summary>
        ///  根据商家标识列得到商家基本信息
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        internal B2b_company GetCompany(int companyid)
        {
            const string sqlTxt = @"SELECT  *
              FROM  [dbo].[b2b_company] a where a.ID=@ID";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@ID", companyid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company u = null;

                if (reader.Read())
                {
                    u = new B2b_company
                    {
                        ID = reader.GetValue<int>("id"),
                        Com_name = reader.GetValue<string>("com_name"),
                        Com_state = reader.GetValue<int>("com_state"),
                        Com_type = reader.GetValue<int>("com_type"),
                        Scenic_name = reader.GetValue<string>("Scenic_name"),
                        Imprest = reader.GetValue<decimal>("imprest"),
                        Fee = reader.GetValue<decimal>("fee"),
                        ServiceFee = reader.GetValue<decimal>("Servicefees"),
                        OpenDate = reader.GetValue<DateTime>("OpenDate"),
                        EndDate = reader.GetValue<DateTime>("EndDate"),
                        Agentopenstate = reader.GetValue<int>("Agentopenstate"),
                        B2bcompanyinfo = new B2bCompanyInfoData().GetCompanyInfo(companyid),
                        Bindingagent = reader.GetValue<int>("Bindingagent"),
                        Lp_agentlevel = reader.GetValue<int>("Lp_agentlevel"),
                        Lp = reader.GetValue<int>("Lp"),
                        Setsearch = reader.GetValue<int>("Setsearch"),
                    };

                }
                return u;
            }
        }
        #endregion

        #region 根据分销商绑定的商家标识列得到商家基本信息
        /// <summary>
        ///  根据分销商绑定的商家标识列得到商家基本信息
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        internal B2b_company GetCompanybyAgentid(int agentid)
        {
            const string sqlTxt = @"SELECT  *
              FROM  [dbo].[b2b_company] a where a.bindingagent=@ID";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@ID", agentid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company u = null;

                while (reader.Read())
                {
                    u = new B2b_company
                    {
                        ID = reader.GetValue<int>("id"),
                        Com_name = reader.GetValue<string>("com_name"),
                        Com_state = reader.GetValue<int>("com_state"),
                        Com_type = reader.GetValue<int>("com_type"),
                        Scenic_name = reader.GetValue<string>("Scenic_name"),
                        Imprest = reader.GetValue<decimal>("imprest"),
                        Fee = reader.GetValue<decimal>("fee"),
                        ServiceFee = reader.GetValue<decimal>("Servicefees"),
                        OpenDate = reader.GetValue<DateTime>("OpenDate"),
                        EndDate = reader.GetValue<DateTime>("EndDate"),
                        Agentopenstate = reader.GetValue<int>("Agentopenstate"),
                        Bindingagent = reader.GetValue<int>("Bindingagent"),
                    };

                }
                return u;
            }
        }
        #endregion


        #region 通过公司绑定的手机查询是否有此手机的商户
        /// <summary>
        ///  通过公司绑定的手机查询是否有此手机的商户(必须是未绑定的商户)
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        internal int GetAllComMsgbyphone(string phone)
        {
            const string sqlTxt = @"SELECT  a.id,a.bindingagent
              FROM  b2b_company as a left join  [b2b_company_info] as b on a.id=b.com_id where b.phone=@phone";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@phone", phone);

            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    if (reader.GetValue<int>("bindingagent") == 0)
                    {
                        return reader.GetValue<int>("id");
                    }
                    else
                    {
                        return -1;
                    }

                }
                return 0;
            }
        }
        #endregion


        #region 通过关键词查询未开通的公司信息
        /// <summary>
        ///  通过关键词查询未开通的公司信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal B2b_company SearchUnopenCom(string key)
        {
            const string sqlTxt = @"SELECT top 1 [ID]
                  ,[com_name]
                  ,[Scenic_name]
                  ,[com_type]
                  ,[com_state]
                  ,[imprest]
                  ,[Fee]
                  ,[Servicefees]
              FROM  [dbo].[b2b_company] a where a.com_state=2 and (a.com_name=@key or id in (select com_id from b2b_company_manageuser where accounts=@key or tel=@key )) ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@key", key);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company u = null;

                if (reader.Read())
                {
                    u = new B2b_company
                    {
                        ID = reader.GetValue<int>("id"),
                        Com_name = reader.GetValue<string>("com_name"),
                        Com_state = reader.GetValue<int>("com_state"),
                        Com_type = reader.GetValue<int>("com_type"),
                        Scenic_name = reader.GetValue<string>("Scenic_name"),
                        Imprest = reader.GetValue<decimal>("imprest"),
                        Fee = reader.GetValue<decimal>("fee"),
                        ServiceFee = reader.GetValue<decimal>("Servicefees"),
                        B2bcompanyinfo = new B2bCompanyInfoData().GetCompanyInfo(reader.GetValue<int>("id"))
                    };

                }
                return u;
            }
        }
        #endregion

        #region 根据域名得到商家ID
        /// <summary>
        ///  根据商家标识列得到商家基本信息
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        internal B2b_company_info DomainGetComId(string domain)
        {
            const string sqlTxt = @"SELECT  [ID]
                  ,[com_id]
              FROM  [dbo].[b2b_company_info] where domainname=@domain ";


            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@domain", domain);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_info u = null;

                while (reader.Read())
                {
                    u = new B2b_company_info
                    {
                        Com_id = reader.GetValue<int>("com_id")
                    };

                }
                sqlHelper.Dispose();
                return u;
            }
        }
        #endregion

        #region 根据域名得到商家ID
        /// <summary>
        ///  根据商家标识列得到商家基本信息
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        internal B2b_company_info GetComIdByAdmindomain(string domain)
        {
            const string sqlTxt = @"SELECT  [ID]
                  ,[com_id]
              FROM  [dbo].[b2b_company_info] where admindomain=@domain ";


            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@domain", domain);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_info u = null;

                while (reader.Read())
                {
                    u = new B2b_company_info
                    {
                        Com_id = reader.GetValue<int>("com_id")
                    };

                }
                sqlHelper.Dispose();
                return u;
            }
        }
        #endregion


        #region 根据管理域名域名得到商家ID
        /// <summary>
        ///  根据商家标识列得到商家基本信息
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        internal B2b_company_info AdminDomainGetComId(string domain)
        {
            const string sqlTxt = @"SELECT  [ID]
                  ,[com_id]
              FROM  [dbo].[b2b_company_info] a where a.admindomain=@domain";


            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@domain", domain);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_info u = null;

                while (reader.Read())
                {
                    u = new B2b_company_info
                    {
                        Com_id = reader.GetValue<int>("com_id")
                    };

                }
                return u;
            }
        }
        #endregion



        #region 根据用户id 得到商家基本信息

        internal B2b_company GetCompanyByUid(int userid)
        {
            const string sqlTxt = @"SELECT  [ID]
                  ,[com_name]
                  ,[Scenic_name]
                  ,[com_type]
                  ,[com_state]
                  ,[imprest]
,BindingAgent
              FROM  [dbo].[b2b_company] a where a.ID in (select com_id from b2b_company_manageuser where id =@userid)";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@userid", userid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company u = null;

                while (reader.Read())
                {
                    u = new B2b_company
                    {
                        ID = reader.GetValue<int>("id"),
                        Com_name = reader.GetValue<string>("com_name"),
                        Com_state = reader.GetValue<int>("com_state"),
                        Com_type = reader.GetValue<int>("com_type"),
                        Scenic_name = reader.GetValue<string>("Scenic_name"),
                        Imprest = reader.GetValue<decimal>("imprest"),
                        Bindingagent = reader.GetValue<int>("BindingAgent")
                    };

                }
                return u;
            }
        }
        #endregion



        public int EditB2bCompanyInfo(B2b_company model, B2b_company_info model1, B2b_company_manageuser model2)
        {
            string procsql = "usp_EditComapnyInfo";

            var cmd = sqlHelper.PrepareStoredSqlCommand(procsql);
            //注册公司基本信息
            cmd.AddParam("@CompanyId", model.ID);
            cmd.AddParam("@Com_name", model.Com_name);
            cmd.AddParam("@Scenic_name", model.Scenic_name);
            cmd.AddParam("@Com_type", model.Com_type);
            cmd.AddParam("@Com_state", model.Com_state);
            cmd.AddParam("@Imprest", model.Imprest);
            cmd.AddParam("@Agentid", model.Agentid);
            //注册公司附加信息
            cmd.AddParam("@CompanyInfoId", model1.Id);
            cmd.AddParam("@CompanyInfoCom_id", model1.Com_id);
            cmd.AddParam("@Com_city", model1.Com_city);
            cmd.AddParam("@Com_add", model1.Com_add);
            cmd.AddParam("@Com_class", model1.Com_class);
            cmd.AddParam("@Com_code", model1.Com_code);
            cmd.AddParam("@Com_sitecode", model1.Com_sitecode);
            cmd.AddParam("@Com_license", model1.Com_license);
            cmd.AddParam("@Sale_Agreement", model1.Sale_Agreement);
            cmd.AddParam("@Agent_Agreement", model1.Agent_Agreement);
            cmd.AddParam("@Scenic_address", model1.Scenic_address);
            cmd.AddParam("@Scenic_intro", model1.Scenic_intro);
            cmd.AddParam("@Scenic_Takebus", model1.Scenic_Takebus);
            cmd.AddParam("@Scenic_Drivingcar", model1.Scenic_Drivingcar);
            cmd.AddParam("@Contact", model1.Contact);
            cmd.AddParam("@CompanyInfoTel", model1.Tel);
            cmd.AddParam("@Phone", model1.Phone);
            cmd.AddParam("@Qq", model1.Qq);
            cmd.AddParam("@Email", model1.Email);
            cmd.AddParam("@Defaultprint", model1.Defaultprint);
            cmd.AddParam("@Province", model1.Province);

            cmd.AddParam("@Serviceinfo", model1.Serviceinfo);
            cmd.AddParam("@Coordinate", model1.Coordinate);
            cmd.AddParam("@Coordinatesize", model1.Coordinatesize);
            cmd.AddParam("@Domainname", model1.Domainname);
            //注册公司员工信息
            cmd.AddParam("@CompanyUserId", model2.Id);
            cmd.AddParam("@CompanyUserCom_id", model2.Com_id);
            cmd.AddParam("@Accounts", model2.Accounts);
            cmd.AddParam("@Passwords", model2.Passwords);
            cmd.AddParam("@Atype", model2.Atype);
            cmd.AddParam("@Employeename", model2.Employeename);
            cmd.AddParam("@Job", model2.Job);
            cmd.AddParam("@CompanyUserTel", model2.Tel);
            cmd.AddParam("@Employeestate", model2.Employeestate);
            cmd.AddParam("@Createuserid", model2.Createuserid);
            cmd.AddParam("@channelcompanyid", model2.Channelcompanyid);
            cmd.AddParam("@channelsource", model2.Channelsource);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;


        }


        internal List<B2b_company> ComList(int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_company";
            var strGetFields = "*";
            var sortKey = "ID";
            var sortMode = "1";
            var condition = " ";
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_company> list = new List<B2b_company>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company
                    {
                        ID = reader.GetValue<int>("id"),
                        Com_name = reader.GetValue<string>("com_name"),
                        Com_state = reader.GetValue<int>("com_state"),
                        Com_type = reader.GetValue<int>("com_type"),
                        Scenic_name = reader.GetValue<string>("Scenic_name"),
                        Imprest = reader.GetValue<decimal>("imprest")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }

        internal string UpCom(int comid, int id, string state)
        {
            int statebool = 0;
            if (state == "屏蔽")
            {
                statebool = 1;
            }
            if (state == "显示")
            {
                statebool = 0;
            }


            var sqlTxt = @"update b2b_company set com_state=@com_state where ID=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@com_state", statebool);
            cmd.AddParam("@id", id);

            cmd.ExecuteNonQuery();

            return "OK";
        }

        //分销开通商户赋值,并且设定为开通
        internal string Agent_Open_Comid(int comid, int agentid, DateTime opendate, DateTime enddate)
        {
            var sqlTxt = @"update b2b_company set agentid=@agentid,agentopenstate=1,opendate=@opendate,enddate=@enddate where ID=@comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@agentid", agentid);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@opendate", opendate);
            cmd.AddParam("@enddate", enddate);
            cmd.ExecuteNonQuery();

            return "OK";
        }

        internal int AdjustFee(string id, decimal fee)
        {
            var sqlTxt = @"update b2b_company set fee=@fee where ID=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@fee", fee);
            cmd.AddParam("@id", id);

            return cmd.ExecuteNonQuery();
        }

        internal int AdjustServiceFee(string id, decimal ServiceFee)
        {
            var sqlTxt = @"update b2b_company set Servicefees=@Servicefees where ID=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@Servicefees", ServiceFee);
            cmd.AddParam("@id", id);

            return cmd.ExecuteNonQuery();
        }

        internal List<B2b_company> GetAllCompanys(out int totalcount)
        {
            string sql = @"SELECT [ID]
      ,[com_name]
      ,[Scenic_name]
      ,[com_type]
      ,[com_state]
      ,[imprest]
      ,[Fee]
      ,[Servicefees]
  FROM [dbo].[b2b_company]  ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            try
            {
                List<B2b_company> list = new List<B2b_company>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new B2b_company
                        {
                            ID = reader.GetValue<int>("id"),
                            Com_name = reader.GetValue<string>("com_name"),
                            Com_state = reader.GetValue<int>("com_state"),
                            Com_type = reader.GetValue<int>("com_type"),
                            Scenic_name = reader.GetValue<string>("Scenic_name"),
                            Imprest = reader.GetValue<decimal>("imprest")
                        });

                    }
                }
                totalcount = list.Count;
                return list;
            }
            catch (Exception e)
            {
                totalcount = 0;
                return null;
            }

        }

        internal List<B2b_company> GetAllCompanys(string comstate, out int totalcount)
        {
            string sql = @"SELECT [ID]
      ,[com_name]
      ,[Scenic_name]
      ,[com_type]
      ,[com_state]
      ,[imprest]
      ,[Fee]
      ,[Servicefees]
  FROM [dbo].[b2b_company]  where com_state in (" + comstate + ")";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            try
            {
                List<B2b_company> list = new List<B2b_company>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new B2b_company
                        {
                            ID = reader.GetValue<int>("id"),
                            Com_name = reader.GetValue<string>("com_name"),
                            Com_state = reader.GetValue<int>("com_state"),
                            Com_type = reader.GetValue<int>("com_type"),
                            Scenic_name = reader.GetValue<string>("Scenic_name"),
                            Imprest = reader.GetValue<decimal>("imprest")
                        });

                    }
                }
                totalcount = list.Count;
                return list;
            }
            catch (Exception e)
            {
                totalcount = 0;
                return null;
            }

        }

        internal B2b_company GetCompanyBasicById(int comid)
        {
            string sql = @"SELECT [ID]
      ,[com_name]
      ,[Scenic_name]
      ,[com_type]
      ,[com_state]
      ,[imprest]
      ,[Fee]
      ,[Servicefees]
,agent_nuomi_bindcomname
,Bindingagent
  FROM [dbo].[b2b_company]  where  id=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            try
            {
                B2b_company u = null;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        u = new B2b_company
                        {
                            ID = reader.GetValue<int>("id"),
                            Com_name = reader.GetValue<string>("com_name"),
                            Com_state = reader.GetValue<int>("com_state"),
                            Com_type = reader.GetValue<int>("com_type"),
                            Scenic_name = reader.GetValue<string>("Scenic_name"),
                            Imprest = reader.GetValue<decimal>("imprest"),
                            Agent_nuomi_bindcomname = reader.GetValue<string>("agent_nuomi_bindcomname"),
                            Bindingagent = reader.GetValue<int>("Bindingagent"),
                        };

                    }
                }

                return u;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        internal int ChangeComType(int comid, int hangye)
        {
            string sql = "update b2b_company set com_type=" + hangye + " where id=" + comid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal string GetComDayRandomstr(int comid, string posid,int num=3)
        {
            string dayrandom = CommonFunc.RandCode(num);
            string now_str = DateTime.Now.ToString("yyyy-MM-dd hh") + ":00:00";
            sqlHelper.BeginTrancation();
            try
            {


                string nowdayrandom_selsql = "select * from b2b_company_nowdayrandom where nowdate='" + now_str + "' and comid=" + comid;
                var cmd = sqlHelper.PrepareTextSqlCommand(nowdayrandom_selsql);
                B2b_company_nowdayrandom t = null;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        t = new B2b_company_nowdayrandom()
                        {
                            id = reader.GetValue<int>("id"),
                            comid = reader.GetValue<int>("comid"),
                            createposid = reader.GetValue<string>("createposid"),
                            createtime = reader.GetValue<DateTime>("createtime"),
                            nowdate = reader.GetValue<DateTime>("nowdate"),
                            randomstr = reader.GetValue<string>("randomstr"),
                            remark = reader.GetValue<string>("remark"),
                        };
                    }
                }

                if (t == null)
                {
                    string nowdayrandom_inssql = "INSERT INTO  [b2b_company_nowdayrandom] ([comid] ,[randomstr]  ,[nowdate] ,[createtime] ,[createposid] ,[remark])VALUES(" + comid + "  ,'" + dayrandom + "' ,'" + now_str + "' ,'" + DateTime.Now + "','" + posid + "','')";
                    cmd = sqlHelper.PrepareTextSqlCommand(nowdayrandom_inssql);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    dayrandom = t.randomstr;
                }

                sqlHelper.Commit();
                sqlHelper.Dispose();
                return dayrandom;
            }
            catch
            {
                sqlHelper.Rollback();
                sqlHelper.Dispose();
                return "";
            }

        }


        internal List<B2b_company_nowdayrandom> GetComDayRandomlist(int comid, DateTime searchdate, int num = 3)
        {
            sqlHelper.BeginTrancation();
            try
            {
                string now_str = searchdate.ToString("yyyy-MM-dd");
                string towday_str = searchdate.AddDays(1).ToString("yyyy-MM-dd");

                string nowdayrandom_selsql = "select * from b2b_company_nowdayrandom where nowdate >= '" + now_str + "' and nowdate<'" + towday_str + "' and comid=" + comid + " order by nowdate";
                var cmd = sqlHelper.PrepareTextSqlCommand(nowdayrandom_selsql);

                List<B2b_company_nowdayrandom> list = new List<B2b_company_nowdayrandom>();
                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        list.Add(new B2b_company_nowdayrandom
                        {
                            id = reader.GetValue<int>("id"),
                            comid = reader.GetValue<int>("comid"),
                            createposid = reader.GetValue<string>("createposid"),
                            createtime = reader.GetValue<DateTime>("createtime"),
                            nowdate = reader.GetValue<DateTime>("nowdate"),
                            randomstr = reader.GetValue<string>("randomstr"),
                            remark = reader.GetValue<string>("remark"),
                        });

                    }
                }

                return list;
                
            }
            catch
            {
                sqlHelper.Rollback();
                sqlHelper.Dispose();
                return null;
            }

        }


        internal int CreateComDayRandom(int comid, DateTime searchdate, int num = 3)
        {
            string now_str = searchdate.ToString("yyyy-MM-dd");
            sqlHelper.BeginTrancation();
            try
            {
                for (int i = 0;i < 24;i++)
                {//循环24次每小时产生一个随机码
                    string date_temp=searchdate.AddHours(i).ToString("yyyy-MM-dd HH:mm:ss");

                    string nowdayrandom_selsql = "select * from b2b_company_nowdayrandom where nowdate='" + date_temp + "' and comid=" + comid;
                    var cmd = sqlHelper.PrepareTextSqlCommand(nowdayrandom_selsql);

                    B2b_company_nowdayrandom t = null;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            t = new B2b_company_nowdayrandom()
                            {
                                id = reader.GetValue<int>("id"),
                                comid = reader.GetValue<int>("comid"),
                                createposid = reader.GetValue<string>("createposid"),
                                createtime = reader.GetValue<DateTime>("createtime"),
                                nowdate = reader.GetValue<DateTime>("nowdate"),
                                randomstr = reader.GetValue<string>("randomstr"),
                                remark = reader.GetValue<string>("remark"),
                            };
                        }
                    }

                    if (t == null)
                    {
                        string dayrandom = CommonFunc.RandCode(num);
                        string nowdayrandom_inssql = "INSERT INTO  [b2b_company_nowdayrandom] ([comid] ,[randomstr]  ,[nowdate] ,[createtime] ,[createposid] ,[remark])VALUES(" + comid + "  ,'" + dayrandom + "' ,'" + date_temp + "' ,'" + DateTime.Now + "','999999999','')";
                        cmd = sqlHelper.PrepareTextSqlCommand(nowdayrandom_inssql);
                        cmd.ExecuteNonQuery();
                    }
                    
                }

                sqlHelper.Commit();
                sqlHelper.Dispose();
                return 1;
            }
            catch
            {
                sqlHelper.Rollback();
                sqlHelper.Dispose();
                return 0;
            }

        }

        internal string GetComPhoneLogo(int comid)
        {
            try
            {
                string sql = "SELECT top 1  [smalllogo] FROM  b2b_company_saleset  where com_id=" + comid;
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

        internal string GetComWebLogo(int comid)
        {
            try
            {
                string sql = "SELECT top 1  [logo] FROM  b2b_company_saleset  where com_id=" + comid;
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

        internal int Editqunarbycomid(int comid, int isqunar, string qunar_username, string qunar_pass)
        {
            string sql = "update b2b_company set isqunar=" + isqunar + " ,qunar_username='" + qunar_username + "',qunar_pass='" + qunar_pass + "' where id=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal B2b_company Getqunarbycomid(int comid)
        {
            string sql = "select isqunar,qunar_username,qunar_pass from b2b_company where id=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_company m = null;
                if (reader.Read())
                {
                    m = new B2b_company
                    {
                        isqunar = reader.GetValue<int>("isqunar"),
                        qunar_username = reader.GetValue<string>("qunar_username"),
                        qunar_pass = reader.GetValue<string>("qunar_pass")
                    };
                }
                return m;
            }
        }

        internal B2b_company GetqunarbyQunarname(string supplierIdentity)
        {
            string sql = "select * from b2b_company where qunar_username='" + supplierIdentity + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_company m = null;
                if (reader.Read())
                {
                    m = new B2b_company
                    {
                        isqunar = reader.GetValue<int>("isqunar"),
                        qunar_username = reader.GetValue<string>("qunar_username"),
                        qunar_pass = reader.GetValue<string>("qunar_pass"),

                        ID = reader.GetValue<int>("id"),
                        Com_name = reader.GetValue<string>("com_name"),
                        Com_state = reader.GetValue<int>("com_state"),
                        Com_type = reader.GetValue<int>("com_type"),
                        Scenic_name = reader.GetValue<string>("Scenic_name"),
                        Imprest = reader.GetValue<decimal>("imprest"), 
                        Bindingagent = reader.GetValue<int>("Bindingagent"),
                    };
                }
                return m;
            }
        }

        internal int Getmicromallimgbycomid(int comid)
        {
            string sql = "select micromallimgid from b2b_company where id=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("micromallimgid");
                }
                return 0;
            }
        }

        internal int Insmicroimg(int comid, int micromallimgid)
        {
            string sql = "update b2b_company set micromallimgid=" + micromallimgid + " where id=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal B2b_company GetMicromallByComid(int comid)
        {
            string sql = "select a.id as aid,a.com_name,a.scenic_name,a.micromallimgid,b.Scenic_intro from b2b_company as a  left join b2b_company_info as b on a.id=b.com_id where a.id=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_company m = null;
                if (reader.Read())
                {
                    m = new B2b_company
                    {
                        ID = reader.GetValue<int>("aid"),
                        Com_name = reader.GetValue<string>("com_name"),
                        Scenic_name = reader.GetValue<string>("scenic_name"),
                        micromallimgid = reader.GetValue<int>("micromallimgid"),
                        com_scenic_intro = reader.GetValue<string>("Scenic_intro"),
                    };
                }
                return m;
            }
        }

        internal int GetBindingAgentByComid(int comid)
        {
            string sql = "select bindingagent from b2b_company where id=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int r = 0;
                if (reader.Read())
                {
                    r = reader.GetValue<int>("bindingagent");
                }
                return r;
            }
        }

        internal List<B2b_company> Getcompanylist(string isapicompany)
        {
            string sql = "select * from b2b_company where 1=1";
            if(isapicompany=="1")
            {
                sql += " and id in (select comid from agent_asyncsendlog)";
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<B2b_company> list = new List<B2b_company>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company
                    {
                        ID = reader.GetValue<int>("id"),
                        Com_name = reader.GetValue<string>("com_name"),
                        Com_state = reader.GetValue<int>("com_state"),
                        Com_type = reader.GetValue<int>("com_type"),
                        Scenic_name = reader.GetValue<string>("Scenic_name"),
                        Imprest = reader.GetValue<decimal>("imprest")
                    });

                }
            }
        
            return list;
        }
    }
}
