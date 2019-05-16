using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_com_pro_SpeciData
    {
        public int EditB2b_com_pro_Speci(B2b_com_pro_Speci m)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalb2b_com_pro_Speci(helper).EditB2b_com_pro_Speci(m);
                return r;
            }
        }

        public List<B2b_com_pro_Speci> Getgglist(int proid)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_com_pro_Speci> r = new Internalb2b_com_pro_Speci(helper).Getgglist(proid);
                return r;
            }
        }

        public static B2b_com_pro_Speci Getgginfobyggid(int ggid)
        {
            using (var helper = new SqlHelper())
            {
                var r = new Internalb2b_com_pro_Speci(helper).Getgginfobyggid(ggid);
                return r;
            }
        }
        /// <summary>
        /// 得到特定分销下产品的有效规格
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="Agentlevel"></param>
        /// <returns></returns>
        public List<B2b_com_pro_Speci> AgentGetgglist(int proid, int Agentlevel)
        {
            using (var helper = new SqlHelper())
            {
                //得到产品规格的过期规格值id
                List<string> expiredidlist = new List<string>();
                List<B2b_com_pro_Specitypevalue> expiredlist = new B2b_com_pro_SpecitypevalueData().Getexpiredggvallist(proid);
                if (expiredlist != null)
                {
                    if (expiredlist.Count > 0)
                    {
                        foreach (B2b_com_pro_Specitypevalue v in expiredlist)
                        {
                            expiredidlist.Add(v.id.ToString());
                        }
                    }
                }

                //得到全部产品规格
                List<B2b_com_pro_Speci> result = new Internalb2b_com_pro_Speci(helper).AgentGetgglist(proid, Agentlevel);


                #region 过期的产品规格
                List<B2b_com_pro_Speci> Expiredresult = new List<B2b_com_pro_Speci>();

                if (result != null)
                {
                    if (result.Count > 0 && expiredidlist.Count > 0)
                    {
                        foreach (B2b_com_pro_Speci s in result)
                        {
                            if (s.speci_type_nameid_Array != "")
                            {
                                foreach (string t in s.speci_type_nameid_Array.Split('-'))
                                {
                                    if (t != "")
                                    {
                                        if (expiredidlist.Contains(t))
                                        {
                                            Expiredresult.Add(s);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                List<B2b_com_pro_Speci> Youxiaoresult = new List<B2b_com_pro_Speci>();
                foreach (B2b_com_pro_Speci df in result)
                {
                    if (!Expiredresult.Contains(df))
                    {
                        Youxiaoresult.Add(df);
                    }
                }
                return Youxiaoresult;
            }
        }
        /// <summary>
        /// 得到产品下有效规格
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="Agentlevel"></param>
        /// <returns></returns>
        public List<B2b_com_pro_Speci>  GetCanUsegglist(int proid)
        {
            using (var helper = new SqlHelper())
            {
                //得到产品规格的过期规格值id
                List<string> expiredidlist = new List<string>();
                List<B2b_com_pro_Specitypevalue> expiredlist = new B2b_com_pro_SpecitypevalueData().Getexpiredggvallist(proid);
                if (expiredlist != null)
                {
                    if (expiredlist.Count > 0)
                    {
                        foreach (B2b_com_pro_Specitypevalue v in expiredlist)
                        {
                            expiredidlist.Add(v.id.ToString());
                        }
                    }
                }

                //得到全部产品规格
                List<B2b_com_pro_Speci> result = new Internalb2b_com_pro_Speci(helper).Getgglist(proid);


                #region 过期的产品规格
                List<B2b_com_pro_Speci> Expiredresult = new List<B2b_com_pro_Speci>();

                if (result != null)
                {
                    if (result.Count > 0 && expiredidlist.Count > 0)
                    {
                        foreach (B2b_com_pro_Speci s in result)
                        {
                            if (s.speci_type_nameid_Array != "")
                            {
                                foreach (string t in s.speci_type_nameid_Array.Split('-'))
                                {
                                    if (t != "")
                                    {
                                        if (expiredidlist.Contains(t))
                                        {
                                            Expiredresult.Add(s);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                List<B2b_com_pro_Speci> Youxiaoresult = new List<B2b_com_pro_Speci>();
                foreach (B2b_com_pro_Speci df in result)
                {
                    if (!Expiredresult.Contains(df))
                    {
                        Youxiaoresult.Add(df);
                    }
                }
                return Youxiaoresult;
            }
        }

        public string Getspecinamebyid(int speci)
        {
            using (var helper = new SqlHelper())
            {
                var r = new Internalb2b_com_pro_Speci(helper).Getspecinamebyid(speci);
                return r;
            }
        }

        public decimal Getspeciminpricebyid(int proid)
        {
            using (var helper = new SqlHelper())
            {
                var r = new Internalb2b_com_pro_Speci(helper).Getspeciminpricebyid(proid);
                return r;
            }
        }

        public decimal Getspeciminfacepricebyid(int proid)
        {
            using (var helper = new SqlHelper())
            {
                var r = new Internalb2b_com_pro_Speci(helper).Getspeciminfacepricebyid(proid);
                return r;
            }
        }

        public decimal Gettop1availableprice(int Agentlevel, int proid)
        {
            using (var helper = new SqlHelper())
            {
                var r = new Internalb2b_com_pro_Speci(helper).Gettop1availableprice(Agentlevel,proid);
                return r;
            }
        }
    }
}
