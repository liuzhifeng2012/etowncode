using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Qunar_Ms.Data.Internal;

namespace ETS2.PM.Service.Qunar_Ms.Data
{
    public class Qunar_pushOrderForBeforePaySyncvisitpersonData
    {
        /// <summary>
        /// 录入请求信息(出游人信息) ，返回id
        /// </summary>
        /// <param name="v_name"></param>
        /// <param name="v_namePinyin"></param>
        /// <param name="v_credentials"></param>
        /// <param name="v_credentialsType"></param>
        /// <param name="v_defined1Value"></param>
        /// <param name="v_defined2Value"></param>
        /// <param name="insrequest1"></param>
        /// <returns></returns>
        public int InsRequest(string v_name, string v_namePinyin, string v_credentials, string v_credentialsType, string v_defined1Value, string v_defined2Value, int insrequest1)
        {
            using(var helper=new SqlHelper())
            {
                int r = new InternalpushOrderForBeforePaySyncvisitperson(helper).InsRequest(v_name,v_namePinyin,v_credentials,v_credentialsType,v_defined1Value,v_defined2Value,insrequest1);
                return r;
            }
        }
    }
}
