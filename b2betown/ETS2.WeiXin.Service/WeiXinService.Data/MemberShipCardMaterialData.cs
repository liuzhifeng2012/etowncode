using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;
using ETS.Data.SqlHelper;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class MemberShipCardMaterialData
    {
        public MemberShipCardMaterial GetMembershipcardMaterial(int materialid)
        {
            using (var helper = new SqlHelper())
            {
                MemberShipCardMaterial material = new InternalMemberShipCardMaterial(helper).GetMembershipcardMaterial(materialid);
                return material;
            }
        }
        public MemberShipCardMaterial GetMembershipcardMaterial(int comid, int materialid)
        {
            using (var helper = new SqlHelper())
            {
                MemberShipCardMaterial material = new InternalMemberShipCardMaterial(helper).GetMembershipcardMaterial(comid, materialid);
                return material;
            }
        }
        public int EditMaterial(MemberShipCardMaterial material)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalMemberShipCardMaterial(helper).EditMaterial(material);
                return id;
            }
        }

        public List<MemberShipCardMaterial> Membershipcardpagelist(int comid, int pageindex, int pagesize, bool applystate, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberShipCardMaterial(helper).Membershipcardpagelist(comid, pageindex, pagesize, applystate, out totalcount);

                return list;
            }
        }
        public List<MemberShipCardMaterial> AllMembershipcardpagelist(int comid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberShipCardMaterial(helper).AllMembershipcardpagelist(comid, pageindex, pagesize, out totalcount);

                return list;
            }
        }
        public int DelMemberShipCardMaterial(int materialid)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalMemberShipCardMaterial(helper).DelMemberShipCardMaterial(materialid);
                return id;
            }
        }
        public int SortMaterial(string materialid, int sortid)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalMemberShipCardMaterial(helper).SortMaterial(materialid, sortid);
                return id;
            }
        }

        public List<MemberShipCardMaterial> GetMCMateralListByComId(int comid,out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                List<MemberShipCardMaterial> list = new InternalMemberShipCardMaterial(helper).GetMCMateralListByComId(comid,out totalcount);
                return list;
            }
        }
    }
}
