using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class Taobao_agent_relationData
    {
        public Taobao_agent_relation GetTb_agent_relation(int agentid, int serialnum)
        {
            using (var helper = new SqlHelper())
            {
                Taobao_agent_relation r = new Internaltaobao_agent_relation(helper).GetTb_agent_relation(agentid, serialnum);
                return r;
            }
        }

        public int EditpartTb_agent_relation(Taobao_agent_relation r)
        {
            using (var helper = new SqlHelper())
            {
                int result = new Internaltaobao_agent_relation(helper).EditpartTb_agent_relation(r);
                return result;
            }
        }

        public IList<Taobao_agent_relation> GetTb_agent_relationList(int agentid)
        {
            using (var helper=new SqlHelper())
            {
                IList<Taobao_agent_relation> list = new Internaltaobao_agent_relation(helper).GetTb_agent_relationList(agentid);
                return list;
            }
        }

        public int EditTb_agent_relation(Taobao_agent_relation r)
        {
            using (var helper = new SqlHelper())
            {
                int result = new Internaltaobao_agent_relation(helper).EditTb_agent_relation(r);
                return result;
            }
        }

        //public bool IsAskByTbid(string tbid)
        //{
        //    using (var helper = new SqlHelper())
        //    {
        //        bool result = new Internaltaobao_agent_relation(helper).IsAskByTbid(tbid);
        //        return result;
        //    }
        //}

       
        /// <summary>
        /// 判断合作卖家是否绑定了分销
        /// </summary>
        /// <param name="wangwangid"></param>
        /// <param name="agentid"></param>
        /// <returns></returns>
        public bool IsbAgentBytbwangwangid(string wangwangid)
        {
            using (var helper = new SqlHelper())
            {
                bool result = new Internaltaobao_agent_relation(helper).IsbAgentBytbwangwangid(wangwangid);
                return result;
            }
        }

        public bool IsAskByTbWangwang(string wangwang)
        {
            using (var helper = new SqlHelper())
            {
                bool result = new Internaltaobao_agent_relation(helper).IsAskByTbWangwang(wangwang);
                return result;
            }
        }
    }
}
