using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Qunar_Ms.Data.Internal;

namespace ETS2.PM.Service.Qunar_Ms.Data
{
    public class Qunar_pushOrderForBeforePaySyncData
    {
        /// <summary>
        /// 录入请求信息 （取消订单），返回新增id
        /// </summary>
        /// <param name="partnerorderId"></param>
        /// <param name="orderStatus"></param>
        /// <param name="c_mobile"></param>
        /// <returns></returns>
        public int InsRequest_cancel(string partnerorderId, string orderStatus,string cancelreason, string c_mobile)
        {
            using(var helper=new SqlHelper())
            {
                int id = new InternalpushOrderForBeforePaySync(helper).InsRequest_cancel(partnerorderId,orderStatus,cancelreason,c_mobile);
                return id;
            }
        }
        /// <summary>
        /// 录入请求信息(取票人/游玩人 信息变动)，返回新增id
        /// </summary>
        /// <param name="partnerorderId"></param>
        /// <param name="c_name"></param>
        /// <param name="c_namePinyin"></param>
        /// <param name="c_mobile"></param>
        /// <param name="c_email"></param>
        /// <param name="c_address"></param>
        /// <param name="c_zipCode"></param>
        /// <returns></returns>
        public int InsRequest_contactperson(string partnerorderId, string c_name, string c_namePinyin, string c_mobile, string c_email, string c_address, string c_zipCode, string orderRemark)
        {
            using (var helper = new SqlHelper())
            {
                int id = new InternalpushOrderForBeforePaySync(helper).InsRequest_contactperson(partnerorderId, c_name, c_namePinyin, c_mobile, c_email, c_address, c_zipCode, orderRemark);
                return id;
            }
        }

        /// <summary>
        /// 录入返回信息，返回id
        /// </summary>
        /// <param name="partnerorderId"></param>
        /// <param name="orderstates"></param>
        /// <param name="responseparam"></param>
        /// <returns></returns>
        public int InsResponse(string partnerorderId, string orderstates, string responseparam,int rid)
        {
            using (var helper = new SqlHelper())
            {
                int id = new InternalpushOrderForBeforePaySync(helper).InsResponse(partnerorderId,   orderstates,   responseparam,  rid);
                return id;
            }
        }
    }
}
