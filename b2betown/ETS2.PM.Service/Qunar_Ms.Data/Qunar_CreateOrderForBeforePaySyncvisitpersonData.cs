using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.Qunar_Ms.QMRequestDataSchema;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Qunar_Ms.Data.Internal;

namespace ETS2.PM.Service.Qunar_Ms.Data
{
    public class Qunar_CreateOrderForBeforePaySyncvisitpersonData
    {

        public int InsQunar_visitperson(CreateOrderForBeforePaySyncRequestBodyorderInfovisitPersonperson mperson, string qunar_orderId)
        {
             using(var helper=new SqlHelper())
             {
                 int id = new InternalQunar_CreateOrderForBeforePaySyncvisitperson(helper).InsQunar_visitperson(mperson,qunar_orderId);
                 return id;
             }
        }

        public bool Ishasvisitperson(string qunar_orderId)
        {
            using (var helper = new SqlHelper())
            {
                bool r = new InternalQunar_CreateOrderForBeforePaySyncvisitperson(helper).Ishasvisitperson(qunar_orderId);
                return r;
            }
        }
    }
}
