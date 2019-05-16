using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using System.Data;
using Newtonsoft.Json;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class Phone_code
    {
        private SqlHelper sqlHelper;
        public Phone_code(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        /// <summary>
        /// 插入手机验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <param name="comid"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        internal string insertphone(decimal phone, decimal code, int comid, int num)
        {
            string sqltxt = "";

            sqltxt = @"insert into Phone_code ([comid]
           ,[phone]
           ,[code]
           ,[codenum]
           ,[codebool]
           ,[codetime])
   values(@comid,@phone,@code,@codenum,@codebool,@codetime)";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.CommandType = CommandType.Text;

            cmd.AddParam("@comid", comid);
            cmd.AddParam("@phone", phone);
            cmd.AddParam("@code", code);
            cmd.AddParam("@codenum", num);
            cmd.AddParam("@codebool", 1);
            cmd.AddParam("@codetime", DateTime.Now);

            cmd.ExecuteNonQuery();

            return "OK";
        }

        public static string insertcode(decimal phone, decimal code, int comid, int num)
        {
            var pro = "";

            using (var helper = new SqlHelper())
            {
                pro = new Phone_code(helper).insertphone(phone, code, comid, num);
            }

            try
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                pro = ex.Message;
                return JsonConvert.SerializeObject(new { type = 1, msg = pro });
            }
        }

        /// <summary>
        /// 修改手机验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <param name="comid"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        internal string upphone(decimal phone, decimal code, int comid, int num, int codebool)
        {
            string sqltxt = "";
            if (codebool == 0)
            {
                sqltxt = @"update Phone_code set codenum=@codenum,codebool=0 where comid=@comid and phone=@phone and codebool=1";
            }
            else
            {
                sqltxt = @"update Phone_code set codenum=codenum+1 where comid=@comid and phone=@phone and codebool=1";
            }


            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.CommandType = CommandType.Text;

            cmd.AddParam("@comid", comid);
            cmd.AddParam("@phone", phone);
            cmd.AddParam("@codenum", num);
            cmd.AddParam("@codebool", codebool);

            cmd.ExecuteNonQuery();

            return "OK";
        }
        public static string upcode(decimal phone, decimal code, int comid, int num, int codebool)
        {
            var pro = "";

            using (var helper = new SqlHelper())
            {
                pro = new Phone_code(helper).upphone(phone, code, comid, num, codebool);
            }

            try
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                pro = ex.Message;
                return JsonConvert.SerializeObject(new { type = 1, msg = pro });
            }
        }


        /// <summary>
        /// 查询手机验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="comid"></param>
        /// <returns></returns>
        internal Phone_codemodel codemodel(decimal phone, int comid)
        {
            var sqltxt = "";
            sqltxt = @"select * from Phone_code where comid=@comid and phone=@phone and codebool=@codebool and  DATEDIFF(minute,codetime,GETDATE()) <= 30 order by codetime desc";



            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@phone", phone);
            cmd.AddParam("@codebool", 1);

            using (var reader = cmd.ExecuteReader())
            {
                Phone_codemodel codeinfo = null;
                if (reader.Read())
                {
                    codeinfo = new Phone_codemodel
                    {
                        Id = reader.GetValue<int>("Id"),
                        Comid = reader.GetValue<int>("comid"),
                        Phone = reader.GetValue<decimal>("phone"),
                        Code = reader.GetValue<decimal>("code"),
                        Codebool = reader.GetValue<int>("codebool"),
                        Codenum = reader.GetValue<int>("codenum"),
                        Codetime = reader.GetValue<DateTime>("codetime")
                    };
                }

                reader.Close();

                sqltxt = @"update  Phone_code set codebool=0  where DATEDIFF(minute,codetime,GETDATE()) > 30  and  codebool=1 ";
                cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
                cmd.ExecuteNonQuery();

                return codeinfo;
            }
        }

        public static Phone_codemodel code_info(decimal phone, int comid)
        {
            var list = new Phone_codemodel();

            using (var helper = new SqlHelper())
            {
                list = new Phone_code(helper).codemodel(phone, comid);
                return list;
            }
        }




        public static Phone_codemodel code_info(decimal phone, int comid, string openid)
        {
            var list = new Phone_codemodel();

            using (var helper = new SqlHelper())
            {
                list = new Phone_code(helper).codemodel(phone, comid, openid);
                return list;
            }
        }

        private Phone_codemodel codemodel(decimal phone, int comid, string openid)
        {
            var sqltxt = "";
            sqltxt = @"select * from Phone_code where openid=@openid and  comid=@comid and phone=@phone and codebool=@codebool and  DATEDIFF(minute,codetime,GETDATE()) <= 30 order by codetime desc";



            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@phone", phone);
            cmd.AddParam("@codebool", 1);
            cmd.AddParam("@openid", openid);

            using (var reader = cmd.ExecuteReader())
            {
                Phone_codemodel codeinfo = null;
                if (reader.Read())
                {
                    codeinfo = new Phone_codemodel
                    {
                        Id = reader.GetValue<int>("Id"),
                        Comid = reader.GetValue<int>("comid"),
                        Phone = reader.GetValue<decimal>("phone"),
                        Code = reader.GetValue<decimal>("code"),
                        Codebool = reader.GetValue<int>("codebool"),
                        Codenum = reader.GetValue<int>("codenum"),
                        Codetime = reader.GetValue<DateTime>("codetime"),
                        Openid = reader.GetValue<string>("openid")

                    };
                }

                reader.Close();

                sqltxt = @"update  Phone_code set codebool=0  where DATEDIFF(minute,codetime,GETDATE()) > 30  and  codebool=1 ";
                cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
                cmd.ExecuteNonQuery();

                return codeinfo;
            }
        }

        public static string upcode(decimal phone, decimal code, int comid, int sendnum, int codebool, string openid)
        {

            try
            {
                var pro = "";
                using (var helper = new SqlHelper())
                {
                    pro = new Phone_code(helper).upphone(phone, code, comid, sendnum, codebool, openid);
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

        private string upphone(decimal phone, decimal code, int comid, int sendnum, int codebool, string openid)
        {
            string sqltxt = "";
            if (codebool == 0)
            {
                sqltxt = @"update Phone_code set codenum=@codenum,codebool=0,code=@code where comid=@comid and phone=@phone and codebool=1 and openid=@openid";
            }
            else
            {
                sqltxt = @"update Phone_code set codenum=codenum+1,code=@code where comid=@comid and phone=@phone and codebool=1 and openid=@openid";
            }


            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.CommandType = CommandType.Text;

            cmd.AddParam("@comid", comid);
            cmd.AddParam("@phone", phone);
            cmd.AddParam("@codenum", sendnum);
            cmd.AddParam("@codebool", codebool);
            cmd.AddParam("@openid", openid);
            cmd.AddParam("@code", code);

            cmd.ExecuteNonQuery();

            return "OK";
        }

        public static string insertcode(decimal phone, decimal code, int comid, int sendnum, string openid)
        {

            try
            {
                var pro = "";

                using (var helper = new SqlHelper())
                {
                    pro = new Phone_code(helper).insertphone(phone, code, comid, sendnum, openid);
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

        private string insertphone(decimal phone, decimal code, int comid, int sendnum, string openid)
        {
            string sqltxt = "";

            sqltxt = @"insert into Phone_code ([comid]
           ,[phone]
           ,[code]
           ,[codenum]
           ,[codebool]
           ,[codetime]
           ,[openid])
   values(@comid,@phone,@code,@codenum,@codebool,@codetime,@openid)";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.CommandType = CommandType.Text;

            cmd.AddParam("@comid", comid);
            cmd.AddParam("@phone", phone);
            cmd.AddParam("@code", code);
            cmd.AddParam("@codenum", sendnum);
            cmd.AddParam("@codebool", 1);
            cmd.AddParam("@codetime", DateTime.Now);
            cmd.AddParam("@openid", openid);

            cmd.ExecuteNonQuery();

            return "OK";
        }

        public static Phone_codemodel GetNoteRecord(string openid, int comid)
        {
            var list = new Phone_codemodel();

            using (var helper = new SqlHelper())
            {
                list = new Phone_code(helper).GetNoteRecord1(openid, comid);
                return list;
            }
        }

        private Phone_codemodel GetNoteRecord1(string openid, int comid)
        {
            var sqltxt = "";
            sqltxt = @"select * from Phone_code where openid=@openid and  comid=@comid   and codebool=@codebool and  DATEDIFF(minute,codetime,GETDATE()) <= 30 order by codetime desc";



            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);

            cmd.AddParam("@codebool", 1);
            cmd.AddParam("@openid", openid);

            using (var reader = cmd.ExecuteReader())
            {
                Phone_codemodel codeinfo = null;
                if (reader.Read())
                {
                    codeinfo = new Phone_codemodel
                    {
                        Id = reader.GetValue<int>("Id"),
                        Comid = reader.GetValue<int>("comid"),
                        Phone = reader.GetValue<decimal>("phone"),
                        Code = reader.GetValue<decimal>("code"),
                        Codebool = reader.GetValue<int>("codebool"),
                        Codenum = reader.GetValue<int>("codenum"),
                        Codetime = reader.GetValue<DateTime>("codetime"),
                        Openid = reader.GetValue<string>("openid")

                    };
                }

                reader.Close();

                sqltxt = @"update  Phone_code set codebool=0  where DATEDIFF(minute,codetime,GETDATE()) > 30  and  codebool=1 ";
                cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
                cmd.ExecuteNonQuery();

                return codeinfo;
            }
        }
    }
}
