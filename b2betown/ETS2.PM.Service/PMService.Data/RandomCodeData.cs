using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data
{
    public class RandomCodeData
    {
        #region 编辑随机码信息
        public int InsertOrUpdate(RandomCode randomcode)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalRandomCode(sql);
                    int result = internalData.InsertOrUpdateRandomCode(randomcode);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
        public List<RandomCode> Gettopcode(int topnum = 1)
        {
            using (var sql = new SqlHelper())
            {
                List<RandomCode> list = new List<RandomCode>();
                try
                {
                    var internalData = new InternalRandomCode(sql);
                    list = internalData.getRandomCodeList(topnum);
                    return list;
                }
                catch
                {
                    throw;
                }
            }

        }

        public RandomCode GetRandomCode(int state = 0)
        {

            using (var sql = new SqlHelper())
            {
                RandomCode code = new RandomCode();
                try
                {
                    var internalData = new InternalRandomCode(sql);
                    code = internalData.GetRandomCode();
                    return code;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }


        public IList<RandomCode> GetAllTotalRandomCode()
        {
            using (var sql = new SqlHelper())
            {
                List<RandomCode> list = new List<RandomCode>();
                try
                {
                    var internalData = new InternalRandomCode(sql);
                    list = internalData.GetAllTotalRandomCode();
                    return list;
                }
                catch
                {
                    throw;
                }
            }
        }

        public RandomCode GetRandomCodeByCode(int codee)
        {
            using (var sql = new SqlHelper())
            {

                try
                {
                    var internalData = new InternalRandomCode(sql);
                    RandomCode code = internalData.GetRandomCodeByCode(codee);
                    return code;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
