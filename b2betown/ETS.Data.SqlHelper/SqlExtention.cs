using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ETS.Data.SqlHelper;
using System.IO;
using Newtonsoft.Json;

namespace ETS.Data.SqlHelper
{
    public static class SqlExtention
    {
        public static T ExecSqlHelper<T>(this SqlHelper helper, Func<SqlHelper, T> func)
        {
            using (var sqlHelper = new SqlHelper())
            {
                return func(sqlHelper);
            }
        }

        public static void QuerySqlHelper(this SqlHelper helper, Action<SqlHelper> actin)
        {
            using (var sqlHelper = new SqlHelper())
            {
                actin(sqlHelper);
            }
        }


        private static string[] filters = new string[] { "--", "drop", "truncate", "delete", "Exec", "'" };


        /// <summary>
        /// 过度字符串,防止Sql注入..
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string ValidateStringParm(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return null;
            }
            foreach (string s in filters)
            {
                if (inputString.IndexOf(s, StringComparison.CurrentCultureIgnoreCase) > -1)
                {
                    return null;
                }
            }
            return inputString;
        }

        /// <summary>
        /// 分页SqlCommand扩展
        /// </summary>  
        /// <param name="cmd">Sqlcommand</param>
        /// <param name="pageIndex">pageindex</param>
        /// <param name="pageSize">pagesize</param>
        /// <param name="sqlText">T-sql</param>
        /// <param name="orderby">order by desc or asc default desc</param>
        /// <param name="sort">order by field name</param>
        /// <param name="principalKey">principal key</param>
        /// <param name="condition">select condition</param>
        /// <returns></returns>
        public static SqlCommand PagingCommand(this SqlCommand cmd, string tblName, string strGetFields, int pageInde, int pageSiz, string orderByKey, string orderByMode, string condition)
        {
            //SqlParameter[] parameters =
            // {
            //     new SqlParameter("@tblName",SqlDbType.VarChar,255),
            //     new SqlParameter("@strGetFields",SqlDbType.VarChar,1000),
            //     new SqlParameter("@OrderfldName",SqlDbType.VarChar,255),
            //     new SqlParameter("@PageSize",SqlDbType.Int),           
            //     new SqlParameter("@PageIndex",SqlDbType.Int),
            //     new SqlParameter("@OrderType",SqlDbType.Int),
            //     new SqlParameter("@strWhere",SqlDbType.VarChar,500),
            //     new SqlParameter("@doCount",SqlDbType.Int)            

            // };
            //parameters[0].Value = "b2b_com_pro";
            //parameters[1].Value = "*";
            //parameters[2].Value = "createtime";
            //parameters[3].Value = pagesize;
            //parameters[4].Value = pageindex;
            //parameters[5].Value = 1;
            //parameters[6].Value = "com_id=" + comid;
            //parameters[7].Direction = ParameterDirection.Output;

            //foreach (var p in parameters)
            //{
            //    cmd.Parameters.Add(p);
            //}

            cmd.AddParam("@tblName", tblName);
            cmd.AddParam("@strGetFields", strGetFields);
            cmd.AddParam("@OrderfldName", orderByKey);
            cmd.AddParam("@OrderType", orderByMode);
            cmd.AddParam("@PageSize", pageSiz);
            cmd.AddParam("@PageIndex", pageInde);
            cmd.AddParam("@strWhere", condition);
            cmd.AddOutParam("@doCount", SqlDbType.Int, 32);
            return cmd;
        }
        public static SqlCommand PagingCommand1(this SqlCommand cmd, string Table, string Column, string OrderColumn, string GroupColumn, int PageSize, int CurrentPage, string Group, string Condition)
        {

            cmd.AddOutParam("@TotalCount", SqlDbType.Int, 32);
            cmd.AddOutParam("@TotalPage", SqlDbType.Int, 32);
            cmd.AddParam("@Table", Table);
            cmd.AddParam("@Column", Column);
            cmd.AddParam("@OrderColumn", OrderColumn);
            cmd.AddParam("@GroupColumn", GroupColumn);
            cmd.AddParam("@PageSize", PageSize);
            cmd.AddParam("@CurrentPage", CurrentPage);
            cmd.AddParam("@Group", Group);
            cmd.AddParam("@Condition", Condition);

            return cmd;
        }

        public static SqlParameter AddReturnValueParameter(this SqlCommand cmd, string paramName)
        {
            return cmd.Parameters.Add(
                new SqlParameter(paramName, SqlDbType.Int, 4, ParameterDirection.ReturnValue, false, 1, 0, null, DataRowVersion.Default, null));
        }

        public static SqlParameter AddParam(this SqlCommand cmd, string paramName, object value)
        {
            if (!paramName.StartsWith("@"))
            {
                paramName = String.Concat("@", paramName);
            }
            return cmd.Parameters.AddWithValue(paramName, value);
        }
        public static SqlParameter AddParam(this SqlCommand cmd, string paramName, SqlDbType type, object value, int size)
        {
            if (!paramName.StartsWith("@"))
            {
                paramName = String.Concat("@", paramName);
            }
            SqlParameter param = new SqlParameter(paramName, type, size);
            param.Value = value;
            return cmd.Parameters.Add(param);

        }

        public static SqlParameter AddOutParam(this SqlCommand cmd, string paramName, SqlDbType type, int size)
        {
            SqlParameter param = cmd.Parameters.Add(paramName, type, size);
            param.Direction = ParameterDirection.Output;
            return param;
        }

        public static T GetValue<T>(this IDataReader reader, int i)
        {
            if (reader[i] == DBNull.Value)
            {
                return default(T);
            }
            return (T)reader[i];
        }

        public static T GetValue<T>(this IDataReader reader, string name)
        {
            var value = reader[name];
            if (value == DBNull.Value)
            {
                return default(T);
            }
            return (T)value;
        }

        public static T ExecSingleReader<T>(this SqlCommand cmd, Func<IDataReader, T> func)
        {
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return func(reader);
                }
                return default(T);
            }
        }

        public static List<T> ExecCollectionReader<T>(this SqlCommand cmd, Func<IDataReader, T> func)
        {
            using (var reader = cmd.ExecuteReader())
            {
                List<T> listT = new List<T>();
                while (reader.Read())
                {
                    listT.Add(func(reader));
                }
                return listT;
            }
        }
        /// <summary>
        /// 将Json数据转为对象（C# json 转换其他格式）
        /// </summary>
        /// <typeparam name="T">目标对象</typeparam>
        /// <param name="jsonText">json数据字符串</param>
        /// <returns></returns>
        public static T FromJson<T>(this string jsonText)
        {
            Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();

            json.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            json.ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace;
            json.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            json.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            StringReader sr = new StringReader(jsonText);
            Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
            T result = (T)json.Deserialize(reader, typeof(T));
            reader.Close();

            return result;
        }
    }
}
