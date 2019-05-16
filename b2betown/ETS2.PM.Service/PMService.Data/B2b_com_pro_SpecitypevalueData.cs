using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_com_pro_SpecitypevalueData
    {
        /// <summary>
        /// 更改产品下的规格值
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="linestatus"></param>
        /// <returns></returns>
        public int UpLinestatus(int proid, int linestatus)
        {
            using (var helper = new SqlHelper())
            {
                int result = new Internalb2b_com_pro_Specitypevalue(helper).UpLinestatus(proid,linestatus);
                return result;
            }
        }

        public int Editguigetypevalue(B2b_com_pro_Specitypevalue m)
        {
            using (var helper = new SqlHelper())
            {
                int result = new Internalb2b_com_pro_Specitypevalue(helper).Editguigetypevalue(m);
                return result;
            }
        }

        public int Getguigevalueid(int proid, string ggtype, string ggvalue)
        {
            using (var helper = new SqlHelper())
            {
                int result = new Internalb2b_com_pro_Specitypevalue(helper).Getguigevalueid(proid,ggtype,ggvalue);
                return result;
            }
        }

        public List<B2b_com_pro_Specitypevalue> Getggvallist(int ggtypeid)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_com_pro_Specitypevalue> result = new Internalb2b_com_pro_Specitypevalue(helper).Getggvallist(ggtypeid);
                return result;
            }
        }

        internal List<B2b_com_pro_Specitypevalue> Getexpiredggvallist(int proid)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_com_pro_Specitypevalue> result = new Internalb2b_com_pro_Specitypevalue(helper).Getexpiredggvallist(proid);
                return result;
            }
        }
    }
}
