using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2bSmsMobileSendDate
    {
        #region 发送电子记录
        public int InsertOrUpdate(B2b_smsmobilesend eticket)
        {

            using (var helper = new SqlHelper())
            {
                return new InternalSmsMobileSend(helper).InsertOrUpdate(eticket);
            }
        }
        #endregion

        #region 发送电子记录
        public B2b_smsmobilesend Searchsmslog(int oid)
        {

            using (var helper = new SqlHelper())
            {
                return new InternalSmsMobileSend(helper).Searchsmslog(oid);
            }
        }
        #endregion



        public List<B2b_smsmobilesend> GetTop5SendFail()
        {
            using (var helper = new SqlHelper())
            {
                return new InternalSmsMobileSend(helper).GetTop5SendFail();
            }
        }
    }
}
