using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Modle.Enum;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalRandomCode
    {
        private SqlHelper sqlHelper;
        public InternalRandomCode(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        #region 得到随机码列表
        internal List<RandomCode> getRandomCodeList(int topnum)
        {
            var randomcmd = sqlHelper.PrepareTextSqlCommand("SELECT TOP " + topnum + " [id],[code],[state] FROM [EtownDB].[dbo].[RandomCode] where [state]=0  order by id asc");
            try
            {
                var randomlist = new List<RandomCode>();
                using (var reader = randomcmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var infomodel = new RandomCode();
                        infomodel.Id = (int)reader["Id"];
                        infomodel.Code = (int)reader["Code"];
                        infomodel.State = (int)reader["State"];

                        randomlist.Add(infomodel);
                    }
                }
                if (randomlist != null)
                {
                    if (randomlist.Count > 0)
                    {
                        foreach (RandomCode r in randomlist)
                        {
                            r.State = 1;
                            InsertOrUpdateRandomCode(r);
                        }
                        return randomlist;
                    }
                    else 
                    {
                        return null;
                    }
                }
                else 
                {
                    return null;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        #region 编辑随机码信息
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateRandomCode";
        public int InsertOrUpdateRandomCode(RandomCode model)
        {

            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Code", model.Code);
            cmd.AddParam("@State", model.State);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;


        }
        #endregion
        #region 得到随机码实体
        private static readonly string GetModelSql = @"SELECT Top 1  [id]
      ,[code]
      ,[state]
  FROM [EtownDB].[dbo].[RandomCode] where [state]=@State order by id asc";
        internal RandomCode GetRandomCode(int state = 0)
        {
            var cmd = sqlHelper.PrepareTextSqlCommand(GetModelSql);
            cmd.AddParam("@State", state);
            //return cmd.ExecSingleReader<RandomCode>(reader => new RandomCode
            //{
            //    Id = reader.GetValue<int>("ID"),
            //    Code = reader.GetValue<int>("code"),
            //    State = reader.GetValue<int>("state")
            //});
            try
            {
                using (var reader = cmd.ExecuteReader())
                {

                    var infomodel = new RandomCode();
                    while (reader.Read())
                    {
                        infomodel.Id = (int)reader["Id"];
                        infomodel.Code = (int)reader["Code"];
                        infomodel.State = (int)reader["State"];
                    }

                    return infomodel;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        internal List<RandomCode> GetAllTotalRandomCode()
        {
            var randomcmd = sqlHelper.PrepareTextSqlCommand("SELECT   [id],[code],[state] FROM [EtownDB].[dbo].[RandomCode]");
            try
            {
                using (var reader = randomcmd.ExecuteReader())
                {
                    var randomlist = new List<RandomCode>();
                    while (reader.Read())
                    {
                        var infomodel = new RandomCode();
                        infomodel.Id = (int)reader["Id"];
                        infomodel.Code = (int)reader["Code"];
                        infomodel.State = (int)reader["State"];

                        randomlist.Add(infomodel);
                    }

                    return randomlist;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal RandomCode GetRandomCodeByCode(int codee)
        {

            const string GetModelSqlq = @"SELECT    [id]
      ,[code]
      ,[state]
  FROM [EtownDB].[dbo].[RandomCode] where code=@code ";
            var cmd = sqlHelper.PrepareTextSqlCommand(GetModelSqlq);
            cmd.AddParam("@code", codee);
            return cmd.ExecSingleReader<RandomCode>(reader => new RandomCode
            {
                Id = reader.GetValue<int>("ID"),
                Code = reader.GetValue<int>("code"),
                State = reader.GetValue<int>("state")
            });


        }
    }
}
