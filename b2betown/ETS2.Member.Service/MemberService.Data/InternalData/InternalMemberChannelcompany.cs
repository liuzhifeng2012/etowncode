using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Member.Service.MemberService.Model;
using ETS.Data.SqlHelper;




namespace ETS2.Member.Service.MemberService.Data.InternalData
{
    public class InternalMemberChannelcompany
    {
        private SqlHelper sqlHelper;
        public InternalMemberChannelcompany(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal List<Member_Channel_company> GetUnitList(int unittype = 2)
        {
            string sqltxt = @"SELECT [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyname]
  FROM [EtownDB].[dbo].[Member_Channel_company] ";

            if (unittype != 2)
            {
                sqltxt += " where issuetype=@issuetype";
            }

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            if (unittype != 2)
            {
                cmd.AddParam("@issuetype", unittype);
            }
            List<Member_Channel_company> list = new List<Member_Channel_company>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Channel_company
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Companyname = reader.GetValue<string>("companyname"),
                        Issuetype = reader.GetValue<int>("Issuetype")

                    });

                }
            }


            return list;
        }

        internal List<Member_Channel_company> GetUnitList(int comid, string unittype)
        {
            string sqltxt = @"SELECT [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyname]
  FROM [EtownDB].[dbo].[Member_Channel_company] where com_id= " + comid + " and issuetype in (" + unittype + ") and companystate=1";



            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);

            List<Member_Channel_company> list = new List<Member_Channel_company>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Channel_company
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Companyname = reader.GetValue<string>("companyname"),
                        Issuetype = reader.GetValue<int>("Issuetype")

                    });

                }
            }


            return list;
        }



        internal List<Member_Channel_company> GetUnitList(int comid, int unittype = 2, int channelcompanyid = 0)
        {



            string sqltxt = @"SELECT [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyname]
  FROM [EtownDB].[dbo].[Member_Channel_company] where com_id=@comid";

            if (unittype != 2)
            {
                sqltxt += " and issuetype=@issuetype";
            }

            if (channelcompanyid != 0)
            {
                sqltxt += " and id=" + channelcompanyid;
            }

            sqltxt += " order by issuetype";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            if (unittype != 2)
            {
                cmd.AddParam("@issuetype", unittype);
            }
            cmd.AddParam("@comid", comid);

            List<Member_Channel_company> list = new List<Member_Channel_company>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Channel_company
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Companyname = reader.GetValue<string>("companyname"),
                        Issuetype = reader.GetValue<int>("Issuetype")

                    });

                }
            }


            return list;
        }
        internal List<Member_Channel_company> GetUnitListselected(int actid)
        {
            string sqltxt = @"SELECT a.[id]
      ,a.[Com_id]
      ,a.[Issuetype]
      ,a.[companyname]
  FROM [EtownDB].[dbo].[Member_Channel_company] as a left join member_act_ch_company as b on a.id=b.companyid where b.actid=@actid";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@actid", actid);
            List<Member_Channel_company> list = new List<Member_Channel_company>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Member_Channel_company
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Companyname = reader.GetValue<string>("companyname"),
                        Issuetype = reader.GetValue<int>("Issuetype")
                    });

                }
            }


            return list;
        }


        internal int GetchannelUnitListselected(int actid, int channelcomid)
        {
            string sqltxt = @"SELECT a.[id]
      ,a.[Com_id]
      ,a.[Issuetype]
      ,a.[companyname]
  FROM [EtownDB].[dbo].[Member_Channel_company] as a left join member_act_ch_company as b on a.id=b.companyid where b.actid=@actid and b.companyid=@channelcomid";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@actid", actid);
            cmd.AddParam("@channelcomid", channelcomid);
            List<Member_Channel_company> list = new List<Member_Channel_company>();
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {

                    return reader.GetValue<int>("id");
                }
            }


            return 0;
        }

        internal Member_Channel_company GetCompanyById(int companyid)
        {
            const string sqltxt = @"SELECT [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyname]
  FROM [EtownDB].[dbo].[Member_Channel_company] where id=@id ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", companyid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Channel_company
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Companyname = reader.GetValue<string>("companyname"),
                        Issuetype = reader.GetValue<int>("Issuetype")

                    };
                }
                return null;
            }
        }

        internal string GetCompanyNameById(int companyid)
        {
            const string sqltxt = @"SELECT [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyname]
  FROM [EtownDB].[dbo].[Member_Channel_company] where id=@id ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", companyid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<string>("companyname");

                }
                return "";
            }
        }

        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateMemberChannelCompany";
        internal int EditChannelCompany(Member_Channel_company model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Com_id", model.Com_id);
            cmd.AddParam("@Issuetype", model.Issuetype);
            cmd.AddParam("@Companyname", model.Companyname);

            cmd.AddParam("@companyaddress", model.Companyaddress);
            cmd.AddParam("@companyphone", model.Companyphone);
            cmd.AddParam("@companycoordinate", model.CompanyCoordinate);
            cmd.AddParam("@companyLocate", model.CompanyLocate);
            cmd.AddParam("@companyimg", model.Companyimg);
            cmd.AddParam("@companyintro", model.Companyintro);
            cmd.AddParam("@companyproject", model.Companyproject);
            cmd.AddParam("@companystate", model.Companystate);
            cmd.AddParam("@whetherdepartment", model.Whetherdepartment);
            cmd.AddParam("@bookurl", model.Bookurl);

            cmd.AddParam("@City", model.City);
            cmd.AddParam("@Province", model.Province);
            cmd.AddParam("@Outshop", model.Outshop);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }

        internal Member_Channel_company GetChannelCompanyById(int id)
        {
            string sql = "select id,Com_id,Issuetype,companyname,whethercreateqrcode from Member_Channel_company   where id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Channel_company
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Companyname = reader.GetValue<string>("companyname"),
                        Issuetype = reader.GetValue<int>("issuetype"),
                        Whethercreateqrcode = reader.GetValue<bool>("whethercreateqrcode")
                    };
                }
            }
            return null;

        }

        internal int Upchannelcompanproject(int channelcompanyid, string companyproject)
        {
            string sql = "update Member_Channel_company set companyproject=@companyproject where id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@companyproject", companyproject);
            cmd.AddParam("@id", channelcompanyid);

            return cmd.ExecuteNonQuery();
        }

        internal int HandleQrCodeCreateStatus(int channelcompanyid, string checkstatus)
        {
            string sql = "update Member_Channel_company set whethercreateqrcode=@checkstatus where id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@checkstatus", checkstatus);
            cmd.AddParam("@id", channelcompanyid);

            return cmd.ExecuteNonQuery();
        }


        internal List<Member_Channel_company> GetPromoteChannelCompany(int comid)
        {
            string sql = @"SELECT [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyname]
      ,[whethercreateqrcode]
  FROM [EtownDB].[dbo].[Member_Channel_company] where com_id=@comid";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@comid", comid);

                List<Member_Channel_company> list = new List<Member_Channel_company>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Member_Channel_company
                        {
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("Com_id"),
                            Companyname = reader.GetValue<string>("companyname"),
                            Issuetype = reader.GetValue<int>("issuetype"),
                            Whethercreateqrcode = reader.GetValue<bool>("whethercreateqrcode")
                        });
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        internal List<Member_Channel_company> GetChannelCompanyList(int comid, string channeltype, int pageindex, int pagesize, out  int totalcount)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            var condition = "com_id=" + comid;
            if (channeltype != "")
            {
                condition += " and issuetype=" + channeltype;
            }

            cmd.PagingCommand1("Member_Channel_company", "*", "Issuetype", "", pagesize, pageindex, "", condition);
            try
            {
                List<Member_Channel_company> list = new List<Member_Channel_company>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Member_Channel_company
                        {
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("Com_id"),
                            Companyname = reader.GetValue<string>("companyname"),
                            Issuetype = reader.GetValue<int>("issuetype"),
                            Whethercreateqrcode = reader.GetValue<bool>("whethercreateqrcode")
                        });
                    }
                }
                totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
                return list;
            }
            catch (Exception e)
            {
                totalcount = 0;
                return null;
            }
        }

        internal Member_Channel_company GetChannelCompany(string channelcompanyid)
        {
            string sql = @"SELECT [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyname]
      ,[whethercreateqrcode]
      ,[companyaddress]
      ,[companyphone]
      ,[companyCoordinate]
,companyLocate
      , companyimg,
			 companyintro,
			 companyproject,
companystate,
whetherdepartment,
bookurl,
City,
Province,
outshop
  FROM [EtownDB].[dbo].[Member_Channel_company] where id=@id";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", channelcompanyid);



                using (var reader = cmd.ExecuteReader())
                {
                    Member_Channel_company model = null;
                    if (reader.Read())
                    {
                        model = new Member_Channel_company
                         {
                             Id = reader.GetValue<int>("id"),
                             Com_id = reader.GetValue<int>("Com_id"),
                             Companyname = reader.GetValue<string>("companyname"),
                             Issuetype = reader.GetValue<int>("issuetype"),
                             Whethercreateqrcode = reader.GetValue<bool>("whethercreateqrcode"),
                             Companyaddress = reader.GetValue<string>("companyaddress"),
                             Companyphone = reader.GetValue<string>("companyphone"),
                             CompanyCoordinate = reader.GetValue<decimal>("companycoordinate"),
                             CompanyLocate = reader.GetValue<string>("CompanyLocate"),
                             Bookurl = reader.GetValue<string>("bookurl"),

                             Companyimg = Convert.IsDBNull(reader.GetValue(8)) == true ? 0 : reader.GetValue<int>("companyimg"),
                             Companyintro = reader.GetValue(9) == null ? "" : reader.GetValue<string>("companyintro"),
                             Companyproject = reader.GetValue(10) == null ? "" : reader.GetValue<string>("companyproject"),
                             Companystate = Convert.IsDBNull(reader.GetValue(11)) == true ? 1 : reader.GetValue<int>("companystate"),
                             Whetherdepartment = Convert.IsDBNull(reader.GetValue(12)) == true ? 0 : reader.GetValue<int>("Whetherdepartment"),

                             City = reader.GetValue<string>("City"),
                             Province = reader.GetValue<string>("Province"),
                             Outshop = reader.GetValue<int>("outshop"),

                         };
                    }
                    return model;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }

        internal List<Member_Channel_company> GetChannelList(int comid, int channeltype)
        {
            string sql = @"SELECT [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyname]
      ,[whethercreateqrcode]
  FROM [EtownDB].[dbo].[Member_Channel_company] where com_id=@comid";
            if (channeltype != 100)
            {
                sql += "  and issuetype=@channeltype";
            }
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@comid", comid);
                if (channeltype != 100)
                {
                    cmd.AddParam("@channeltype", channeltype);
                }

                List<Member_Channel_company> list = new List<Member_Channel_company>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Member_Channel_company
                        {
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("Com_id"),
                            Companyname = reader.GetValue<string>("companyname"),
                            Issuetype = reader.GetValue<int>("issuetype"),
                            Whethercreateqrcode = reader.GetValue<bool>("whethercreateqrcode")
                        });
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        internal Member_Channel_company GetChannelCompany(string openid, int comid)
        {
            string sql = @" select * from member_channel_company where id in 
                         (
                          select companyid from member_channel where id in 
                          (
                            select issuecard from member_card where cardcode  in 
                            (
                               select idcard from b2b_crm where weixin=@weixin and com_id=@comid
                            )
                          )
                         )";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@weixin", openid);
                cmd.AddParam("@comid", comid);

                using (var reader = cmd.ExecuteReader())
                {
                    Member_Channel_company model = null;
                    if (reader.Read())
                    {
                        model = new Member_Channel_company
                        {
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("Com_id"),
                            Companyname = reader.GetValue<string>("companyname"),
                            Issuetype = reader.GetValue<int>("issuetype"),
                            Whethercreateqrcode = reader.GetValue<bool>("whethercreateqrcode"),
                            Companyaddress = reader.GetValue<string>("companyaddress"),
                            Companyphone = reader.GetValue<string>("companyphone"),
                            CompanyCoordinate = reader.GetValue<decimal>("companycoordinate"),
                            CompanyLocate = reader.GetValue<string>("CompanyLocate"),
                            Companyimg = Convert.IsDBNull(reader.GetValue(8)) == true ? 0 : reader.GetValue<int>("companyimg"),
                            Companyintro = reader.GetValue(9) == null ? "" : reader.GetValue<string>("companyintro"),
                            Companyproject = reader.GetValue(10) == null ? "" : reader.GetValue<string>("companyproject"),
                            Outshop = reader.GetValue<int>("Outshop"),

                        };
                    }
                    return model;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }



        internal Member_Channel_company GetMenShiByJumpId(string openid, int comid)
        {
            string sql = @" select * from member_channel_company where id in 
                         (
                          select companyid from member_channel where id in 
                          (
                            select issuecard from member_card where cardcode  in 
                            (
                               select idcard from b2b_crm where weixin=@weixin and com_id=@comid
                            )
                          )
                         )";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@weixin", openid);
                cmd.AddParam("@comid", comid);

                using (var reader = cmd.ExecuteReader())
                {
                    Member_Channel_company model = null;
                    if (reader.Read())
                    {
                        model = new Member_Channel_company
                        {
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("Com_id"),
                            Companyname = reader.GetValue<string>("companyname"),
                            Issuetype = reader.GetValue<int>("issuetype"),
                            Whethercreateqrcode = reader.GetValue<bool>("whethercreateqrcode"),
                            Companyaddress = reader.GetValue<string>("companyaddress"),
                            Companyphone = reader.GetValue<string>("companyphone"),
                            CompanyCoordinate = reader.GetValue<decimal>("companycoordinate"),
                            CompanyLocate = reader.GetValue<string>("CompanyLocate"),
                            Companyimg = Convert.IsDBNull(reader.GetValue(8)) == true ? 0 : reader.GetValue<int>("companyimg"),
                            Companyintro = reader.GetValue(9) == null ? "" : reader.GetValue<string>("companyintro"),
                            Companyproject = reader.GetValue(10) == null ? "" : reader.GetValue<string>("companyproject")

                        };
                    }
                    return model;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }



        internal List<Member_Channel_company> Channelcompanypagelist(string comid, int pageindex, int pagesize, string key, out int totalcount, int channelcompanyid = 0, string channelcompanytype = "0,1,3,4")
        {

            try
            {
                var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

                var condition = "com_id=" + comid + " and Issuetype in (" + channelcompanytype + ") and whetherdepartment=0 and companystate=1";



                if (channelcompanyid != 0)
                {
                    condition = "id=" + channelcompanyid;
                }
                else
                {
                    if (key != "")
                    {
                        condition = "com_id=" + comid + " and Issuetype=0 and companyname like '%" + key + "%' and and whetherdepartment=0 and companystate=1";
                    }
                }
                cmd.PagingCommand1("member_channel_company", "*", "id", "", pagesize, pageindex, "", condition);

                List<Member_Channel_company> list = new List<Member_Channel_company>();
                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        list.Add(new Member_Channel_company
                        {
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("Com_id"),
                            Companyname = reader.GetValue<string>("companyname"),
                            Issuetype = reader.GetValue<int>("issuetype"),
                            Whethercreateqrcode = reader.GetValue<bool>("whethercreateqrcode"),
                            Companyaddress = reader.GetValue<string>("companyaddress"),
                            Companyphone = reader.GetValue<string>("companyphone"),
                            CompanyCoordinate = reader.GetValue<decimal>("companycoordinate"),
                            CompanyLocate = reader.GetValue<string>("CompanyLocate"),
                            Companyimg = Convert.IsDBNull(reader.GetValue(8)) == true ? 0 : reader.GetValue<int>("companyimg"),
                            Companyintro = reader.GetValue(9) == null ? "" : reader.GetValue<string>("companyintro"),
                            Companyproject = reader.GetValue(10) == null ? "" : reader.GetValue<string>("companyproject")

                        });
                    }

                }
                totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
                return list;
            }
            catch (Exception e)
            {
                totalcount = 0;
                return null;
            }
        }


        internal List<Member_Channel_company> ChannelcompanyOrderlocation(string comid, int pageindex, int pagesize, string key, out int totalcount, int channelcompanyid = 0, string channelcompanytype = "0,1,3,4", string n1 = "", string e1 = "")
        {

            try
            {
                string sqltxt = @"select TOP 20 * from Member_Channel_company where ";

                sqltxt += "com_id=" + comid + " and whetherdepartment=0 and companystate=1";

                if (channelcompanyid != 0)
                {
                    sqltxt += " and id=" + channelcompanyid;
                }
                else
                {
                    if (key != "")
                    {
                        sqltxt += " and Issuetype=0 and companyname like '%" + key + "%'";
                    }
                }
                if (channelcompanytype=="1")//合作单位
                {
                    sqltxt += " and Issuetype=1";
                }

                if (n1 != "" && e1 != "")
                {
                    sqltxt += " and  not companylocate is null and not companylocate=''";

                    sqltxt += " order by power(power((" + n1 + "-convert(float,Substring(companylocate,CHARINDEX(',',companylocate)+1,len(companylocate)))),2)+power((" + e1 + "-convert(float,left(companylocate,CHARINDEX(',',companylocate)-1))),2),0.5)";


                }
                else
                {
                    sqltxt += " order by id";
                }

                var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
                int i = 0;
                List<Member_Channel_company> list = new List<Member_Channel_company>();
                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        list.Add(new Member_Channel_company
                        {
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("Com_id"),
                            Companyname = reader.GetValue<string>("companyname"),
                            Issuetype = reader.GetValue<int>("issuetype"),
                            Whethercreateqrcode = reader.GetValue<bool>("whethercreateqrcode"),
                            Companyaddress = reader.GetValue<string>("companyaddress"),
                            Companyphone = reader.GetValue<string>("companyphone"),
                            CompanyCoordinate = reader.GetValue<decimal>("companycoordinate"),
                            CompanyLocate = reader.GetValue<string>("CompanyLocate"),
                            Companyimg = Convert.IsDBNull(reader.GetValue(8)) == true ? 0 : reader.GetValue<int>("companyimg"),
                            Companyintro = reader.GetValue(9) == null ? "" : reader.GetValue<string>("companyintro"),
                            Companyproject = reader.GetValue(10) == null ? "" : reader.GetValue<string>("companyproject")

                        });
                        i++;
                    }

                }
                totalcount = i;
                return list;
            }
            catch (Exception e)
            {
                totalcount = 0;
                return null;
            }
        }



        internal int Adjustchannelcompanystatus(int companyid, int status)
        {
            try
            {
                string sql = "update Member_Channel_company set companystate=@status where id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@status", status);
                cmd.AddParam("@id", companyid);

                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {

                return 0;
            }
        }


        internal int jianchaguwenbyweixin(int comid, string weixin)
            {
              int backint = 0;  

              string sql = "select * from Member_Channel where com_id=@comid and mobile in (select phone from b2b_crm where com_id=@comid and weixin=@weixin)";
              var cmd = sqlHelper.PrepareTextSqlCommand(sql);
              cmd.AddParam("@comid", comid);
              cmd.AddParam("@weixin", weixin);
              try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            backint = 1;
                        }
                    }
                    return backint;
                }
                catch (Exception e)
                {

                    return 0;
                }
         }


        internal int getchannelidbyweixin(int comid, string weixin,int uid=0)
        {
            int backint = 0;

            string sql = "select * from Member_Channel where com_id=@comid and mobile in (select phone from b2b_crm where com_id=@comid and weixin=@weixin)";
            if (uid != 0)
            {
                sql = "select * from Member_Channel where com_id=@comid and mobile in (select phone from b2b_crm where com_id=@comid and id=@uid)";
            }
            
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@weixin", weixin);
            cmd.AddParam("@uid", uid);
            try
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        backint = reader.GetValue<int>("id");
                    }
                }
                return backint;
            }
            catch (Exception e)
            {

                return 0;
            }
        }


        internal List<Member_Channel_company> GetChannelCompanyList(int comid, string issuetype, string companystate, string whetherdepartment, int channelcompanyid = 0)
        {
            string sql = @"SELECT [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyname]
      ,[whethercreateqrcode]
      ,[companyaddress]
      ,[companyphone]
      ,[companyCoordinate]
,CompanyLocate
      ,[companyimg]
      ,[companyintro]
      ,[companyproject]
      ,[companystate]
      ,[whetherdepartment]
  FROM [EtownDB].[dbo].[Member_Channel_company] where com_id=" + comid + " and issuetype in (" + issuetype + ") and companystate in (" + companystate + ") and whetherdepartment in (" + whetherdepartment + ")";

            if (channelcompanyid > 0)
            {
                sql += " and id=" + channelcompanyid;
            }

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);


                List<Member_Channel_company> list = new List<Member_Channel_company>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Member_Channel_company
                        {
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("Com_id"),
                            Companyname = reader.GetValue<string>("companyname"),
                            Issuetype = reader.GetValue<int>("issuetype"),
                            Whethercreateqrcode = reader.GetValue<bool>("whethercreateqrcode"),
                            Companyaddress = reader.GetValue<string>("companyaddress"),
                            Companyphone = reader.GetValue<string>("companyphone"),
                            CompanyCoordinate = reader.GetValue<decimal>("companycoordinate"),
                            CompanyLocate = reader.GetValue<string>("CompanyLocate"),
                            Companyimg = Convert.IsDBNull(reader.GetValue(8)) == true ? 0 : reader.GetValue<int>("companyimg"),
                            Companyintro = reader.GetValue(9) == null ? "" : reader.GetValue<string>("companyintro"),
                            Companyproject = reader.GetValue(10) == null ? "" : reader.GetValue<string>("companyproject"),
                            Companystate = Convert.IsDBNull(reader.GetValue(11)) == true ? 1 : reader.GetValue<int>("companystate"),
                            Whetherdepartment = Convert.IsDBNull(reader.GetValue(12)) == true ? 0 : reader.GetValue<int>("Whetherdepartment")


                        });
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }





        internal Member_Channel_company GetChannelCompanyByUserId(int userid)
        {
            string sqltxt = @"SELECT [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyname]
      ,[companyaddress]
      ,[companyphone]
  FROM [EtownDB].[dbo].[Member_Channel_company]  where id = (select channelcompanyid from b2b_company_manageuser where id=" + userid + ") ";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);

            using (var reader = cmd.ExecuteReader())
            {
                Member_Channel_company u = null;
                if (reader.Read())
                {
                    u = new Member_Channel_company
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Companyname = reader.GetValue<string>("companyname"),
                        Issuetype = reader.GetValue<int>("Issuetype"),
                        Companyaddress = reader.GetValue<string>("companyaddress"),
                        Companyphone = reader.GetValue<string>("companyphone")
                    };
                }
                return u;
            }

        }

        internal Member_Channel_company GetChannelCompanyByCrmId(int crmid)
        {
            string sqltxt = @"select * from Member_Channel_company where id in (select companyid from Member_Channel where id in (select issuecard from Member_Card where cardcode in (select idcard from b2b_crm where id=@crmid)))";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@crmid", crmid);

            using (var reader = cmd.ExecuteReader())
            {
                Member_Channel_company u = null;
                if (reader.Read())
                {
                    u = new Member_Channel_company
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Companyname = reader.GetValue<string>("companyname"),
                        Issuetype = reader.GetValue<int>("Issuetype"),
                        Companyaddress = reader.GetValue<string>("companyaddress"),
                        Companyphone = reader.GetValue<string>("companyphone")

                    };
                }
                return u;
            }
        }

        internal Member_Channel_company GetChannelCompanyByWxsourceId(int wxsourceid)
        {
            string sqltxt = @"select * from Member_Channel_company where id =(select channelcompanyid from WxSubscribeSource where id=@wxsourceid)";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@wxsourceid", wxsourceid);

            using (var reader = cmd.ExecuteReader())
            {
                Member_Channel_company u = null;
                if (reader.Read())
                {
                    u = new Member_Channel_company
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Companyname = reader.GetValue<string>("companyname"),
                        Issuetype = reader.GetValue<int>("Issuetype"),
                        Companyaddress = reader.GetValue<string>("companyaddress"),
                        Companyphone = reader.GetValue<string>("companyphone"),
                        Bookurl = reader.GetValue<string>("bookurl")
                    };
                }
                return u;
            }
        }

        internal List<Member_Channel_company> GetMenshisByComid(int comid)
        {
            string sql = "select * from Member_Channel_company where Com_id=" + comid + " and Issuetype=0 and companystate=1";

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    List<Member_Channel_company> list = new List<Member_Channel_company>();
                    while (reader.Read())
                    {
                        list.Add(new Member_Channel_company()
                        {
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("com_id"),
                            Companyname = reader.GetValue<string>("companyname"),
                            Issuetype = reader.GetValue<int>("Issuetype"),
                            Companyaddress = reader.GetValue<string>("companyaddress"),
                            Companyphone = reader.GetValue<string>("companyphone"),
                            Bookurl = reader.GetValue<string>("bookurl")
                        });
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal Member_Channel_company GetMenshiByPhone(string phone, int comid)
        {
            string sql = "select * from Member_Channel_company where com_id=" + comid + " and  id In (select companyid from Member_Channel where mobile='" + phone + "' and com_id=" + comid + ")";

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    Member_Channel_company m = null;
                    if (reader.Read())
                    {
                        m = new Member_Channel_company()
                        {
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("com_id"),
                            Companyname = reader.GetValue<string>("companyname"),
                            Issuetype = reader.GetValue<int>("Issuetype"),
                            Companyaddress = reader.GetValue<string>("companyaddress"),
                            Companyphone = reader.GetValue<string>("companyphone"),
                            Bookurl = reader.GetValue<string>("bookurl")
                        };
                    }
                    return m;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        internal Member_Channel_company GetGuWenChannelCompanyByCrmWeixin(string weixin, int comid)
        {
            string sql = @"select * from Member_Channel_company where id =(
                               select companyid from Member_Channel where companyid>0  and  id=(select  IssueCard  from Member_Card where IssueCard>0 and cardcode =(select idcard from b2b_crm where weixin =@weixin and com_id=@comid))
                             )";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@weixin", weixin);
                cmd.AddParam("@comid", comid);

                using (var reader = cmd.ExecuteReader())
                {
                    Member_Channel_company m = null;
                    if (reader.Read())
                    {
                        m = new Member_Channel_company()
                        {
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("com_id"),
                            Companyname = reader.GetValue<string>("companyname"),
                            Issuetype = reader.GetValue<int>("Issuetype"),
                            Companyaddress = reader.GetValue<string>("companyaddress"),
                            Companyphone = reader.GetValue<string>("companyphone"),
                            Bookurl = reader.GetValue<string>("bookurl")
                        };
                    }
                    return m;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal Member_Channel_company GetMemberChanelCompanyByUserid(int userid)
        {
            string sql = "select * from Member_Channel_company where id>0 and id =(select channelcompanyid from b2b_company_manageuser where id=" + userid + ")";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);


                using (var reader = cmd.ExecuteReader())
                {
                    Member_Channel_company m = null;
                    if (reader.Read())
                    {
                        m = new Member_Channel_company()
                        {
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("com_id"),
                            Companyname = reader.GetValue<string>("companyname"),
                            Issuetype = reader.GetValue<int>("Issuetype"),
                            Companyaddress = reader.GetValue<string>("companyaddress"),
                            Companyphone = reader.GetValue<string>("companyphone"),
                            Bookurl = reader.GetValue<string>("bookurl")
                        };
                    }
                    return m;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<Member_Channel_company> Getchannelcompanylist(int comid, string Issuetype, string isrun, string key, string channelcompanyid = "0")
        {

            string sql = "";
            if (channelcompanyid != "0")//显示特定渠道单位信息
            {
                sql = @"SELECT    a.issuetype, a.companyname, a.id,a.companystate, COUNT(c.id) AS Expr1, SUM(isnull(c.opencardnum,0))  AS opencardnum, SUM(isnull(c.firstdealnum,0)) AS firstdealnum, SUM(isnull(c.summoney,0))  " +
                              " AS summoney, a.Com_id, a.id AS Expr2,COUNT(d.ID) AS companynum " +
                              " FROM         dbo.Member_Channel_company AS a LEFT OUTER JOIN " +
                              " dbo.Member_Channel AS c ON a.id = c.companyid left join  " +
                              " dbo.Member_Activity_Log AS d ON d.sales_admin = c.name " +
                              " where a.id=" + channelcompanyid +
                              " GROUP BY a.issuetype,a.companyname, a.Com_id, a.id,a.companystate";
                if (key != "")
                {
                    sql = "SELECT    a.issuetype, a.companyname, a.id,a.companystate, COUNT(c.id) AS Expr1, SUM(isnull(c.opencardnum,0))  AS opencardnum, SUM(isnull(c.firstdealnum,0)) AS firstdealnum, SUM(isnull(c.summoney,0)) " +
                              " AS summoney, a.Com_id, a.id AS Expr2,COUNT(d.ID) AS companynum " +
                              " FROM         dbo.Member_Channel_company AS a LEFT OUTER JOIN " +
                              " dbo.Member_Channel AS c ON a.id = c.companyid left join  " +
                              " dbo.Member_Activity_Log AS d ON d.sales_admin = c.name " +
                              " where a.id=" + channelcompanyid + "  and (a.companyname like '%" + key + "%'" + " or a.id in (select companyid from Member_Channel where name ='" + key + "' or mobile='" + key + "'))" +
                              " GROUP BY a.issuetype,a.companyname, a.Com_id, a.id,a.companystate";
                }



            }
            else //显示渠道列表
            {
                if (Issuetype == "0")//所属门店
                {

                    sql = "SELECT    a.issuetype, a.companyname, a.id,a.companystate, COUNT(c.id) AS Expr1, SUM(isnull(c.opencardnum,0))  AS opencardnum, SUM(isnull(c.firstdealnum,0)) AS firstdealnum, SUM(isnull(c.summoney,0))  " +
                  " AS summoney, a.Com_id, a.id AS Expr2,COUNT(d.ID) AS companynum " +
                  " FROM         dbo.Member_Channel_company AS a LEFT OUTER JOIN " +
                  " dbo.Member_Channel AS c ON a.id = c.companyid left join  " +
                  " dbo.Member_Activity_Log AS d ON d.sales_admin = c.name " +
                  "  where a.Issuetype=0 and a.com_id=" + comid + " and a.companystate in (" + isrun + ")" +//最后 and a.id=0默认不显示
                  " GROUP BY a.issuetype,a.companyname, a.Com_id, a.id,a.companystate";
                    if (key != "")
                    {
                        sql = "SELECT     a.issuetype,a.companyname, a.id,a.companystate, COUNT(c.id) AS Expr1, SUM(isnull(c.opencardnum,0))  AS opencardnum, SUM(isnull(c.firstdealnum,0)) AS firstdealnum, SUM(isnull(c.summoney,0))  " +
                                " AS summoney, a.Com_id, a.id AS Expr2,COUNT(d.ID) AS companynum " +
                                " FROM         dbo.Member_Channel_company AS a LEFT OUTER JOIN " +
                                " dbo.Member_Channel AS c ON a.id = c.companyid left join  " +
                                " dbo.Member_Activity_Log AS d ON d.sales_admin = c.name " +
                                "  where a.Issuetype=0 and a.com_id=" + comid + " and a.companystate in (" + isrun + ") and (a.companyname like '%" + key + "%'" + " or a.id in (select companyid from Member_Channel where name ='" + key + "' or mobile='" + key + "'))" +
                                " GROUP BY a.issuetype,a.companyname, a.Com_id, a.id,a.companystate";
                    }


                }
                else
                {
                    sql = @"SELECT     a.issuetype,a.companyname, a.id,a.companystate, COUNT(c.id) AS Expr1, SUM(isnull(c.opencardnum,0))  AS opencardnum, SUM(isnull(c.firstdealnum,0)) AS firstdealnum, SUM(isnull(c.summoney,0)) " +
                    " AS summoney, a.Com_id, a.id AS Expr2,COUNT(d.ID) AS companynum " +
                    " FROM         dbo.Member_Channel_company AS a LEFT OUTER JOIN " +
                    " dbo.Member_Channel AS c ON a.id = c.companyid left join  " +
                    " dbo.Member_Activity_Log AS d ON d.sales_admin = c.name " +
                   "  where a.Issuetype=1   and a.com_id=" + comid + " and a.companystate in (" + isrun + ") " +
                   "  GROUP BY a.issuetype,a.companyname, a.Com_id, a.id,a.companystate";
                    if (key != "")
                    {
                        sql = @"SELECT    a.issuetype, a.companyname, a.id,a.companystate, COUNT(c.id) AS Expr1, SUM(isnull(c.opencardnum,0))  AS opencardnum, SUM(isnull(c.firstdealnum,0)) AS firstdealnum, SUM(isnull(c.summoney,0)) " +
                        " AS summoney, a.Com_id, a.id AS Expr2,COUNT(d.ID) AS companynum " +
                        " FROM         dbo.Member_Channel_company AS a LEFT OUTER JOIN " +
                        " dbo.Member_Channel AS c ON a.id = c.companyid left join  " +
                        " dbo.Member_Activity_Log AS d ON d.sales_admin = c.name " +
                       "  where a.Issuetype=1   and a.com_id=" + comid + " and a.companystate in (" + isrun + ") and (a.companyname like '%" + key + "%'" + " or a.id in (select companyid from Member_Channel where name ='" + key + "' or mobile='" + key + "'))" +
                       "  GROUP BY a.issuetype,a.companyname, a.Com_id, a.id,a.companystate";
                    }

                }

            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                List<Member_Channel_company> list = new List<Member_Channel_company>();
                while (reader.Read())
                {
                    list.Add(new Member_Channel_company
                    {
                        Id = reader.GetValue<int>("id"),
                        Companyname = reader.GetValue<string>("companyname")
                    });
                }
                return list;
            }


        }
    }
}
