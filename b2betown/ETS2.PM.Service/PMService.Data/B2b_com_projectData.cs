using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_com_projectData
    {
        public int EditProject(B2b_com_project model)
        {
            using (var helper = new SqlHelper())
            {
                int id = new Internal_b2b_com_project(helper).EditProject(model);
                return id;
            }
        }

        public B2b_com_project GetProject(int projectid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                B2b_com_project result = new Internal_b2b_com_project(helper).GetProject(projectid, comid);
                return result;
            }
        }

        public string GetProjectname(int projectid)
        {
            using (var helper = new SqlHelper())
            {
                string result = new Internal_b2b_com_project(helper).GetProjectname(projectid);
                return result;
            }
        }

        public int GetProjectBindingid(int projectid)
        {
            using (var helper = new SqlHelper())
            {
                var result = new Internal_b2b_com_project(helper).GetProjectBindingid(projectid);
                return result;
            }
        }


        public List<B2b_com_project> Projectlist(int pageindex, int pagesize, int agentid, out int totalcount,string key="")
        {
            using (var helper = new SqlHelper())
            {
                var result = new Internal_b2b_com_project(helper).Projectlist(pageindex, pagesize, agentid, out totalcount,key);
                return result;
            }
        }

        public List<Agent_regiinfo> ProjectAgentlist(int pageindex, int pagesize, int comid, int projectid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                var result = new Internal_b2b_com_project(helper).ProjectAgentlist(pageindex, pagesize, comid, projectid, out totalcount);
                return result;
            }
        }

        public string GetAgentPhonelist(int comid)
        {
            using (var helper = new SqlHelper())
            {
                var result = new Internal_b2b_com_project(helper).GetAgentPhonelist(comid);
                return result;
            }
        }

        public List<B2b_com_pro> ProlistbyProjectid(int pageindex, int pagesize, int agentid, int projectid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                var result = new Internal_b2b_com_project(helper).ProlistbyProjectid(pageindex, pagesize, agentid, projectid, out totalcount);
                return result;
            }
        }

        public IList<B2b_com_project> Projectpagelist(string comid, int pageindex, int pagesize, string projectstate, out int totalcount, string key = "", int runpro = 0, int projectid=0, int servertype = 0)
        {
            using (var helper = new SqlHelper())
            {
                IList<B2b_com_project> result = new Internal_b2b_com_project(helper).Projectpagelist(comid, pageindex, pagesize, projectstate, out totalcount, key, runpro, projectid, servertype);
                return result;
            }
        }

      

        public int GetProjectCountByComId(int comid)
        {
          using(var helper=new SqlHelper())
          {
              int count = new Internal_b2b_com_project(helper).GetProjectCountByComId(comid);
              return count;
          }
        }

        public string  GetProjectNameByid(int projectid)
        {
            using (var helper = new SqlHelper())
            {
                string r = new Internal_b2b_com_project(helper).GetProjectNameByid(projectid);
                return r;
            }
        }

        //根据产品id得到项目id
        public int GetProjectidByproid(int proid)
        {
            using (var helper = new SqlHelper())
            {
                var r = new Internal_b2b_com_project(helper).GetProjectidByproid(proid);
                return r;
            }
        }

        public IList<B2b_com_project> Projectlist(string comid, string projectstate, out int totalcount,int prosort=0)
        {
            using (var helper = new SqlHelper())
            {
                IList<B2b_com_project> result = new Internal_b2b_com_project(helper).Projectpagelist(comid, projectstate, out totalcount, prosort);
                return result;
            }
        }
        public List<B2b_com_project> ProjectSelectpagelist(int projectstate, int pageindex, int pagesize, string key, out int totalcount, int proclass, string comid, int projectid = 0, int price = 0)
        {
            using (var sql = new SqlHelper())
            {
                try
                {

                    var result = new Internal_b2b_com_project(sql).ProjectSelectpagelist(projectstate, pageindex, pagesize, key, out totalcount, proclass, comid,projectid,price);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }



        public int SortMenu(string menuid, int sortid)
        {
            using (var helper = new SqlHelper())
            {
                var id = new Internal_b2b_com_project(helper).SortMenu(menuid, sortid);
                return id;
            }
        }

        public B2b_com_project GetProject(int projectid)
        {
            using (var helper = new SqlHelper())
            {
                var r = new Internal_b2b_com_project(helper).GetProject(projectid);
                return r;
            }
        }

        public int GetProjectUnyuding(int projectid)
        {
            using (var helper = new SqlHelper())
            {
                var r = new Internal_b2b_com_project(helper).GetProjectUnyuding(projectid);
                return r;
            }
        }



        public List<B2b_com_project> GetProjectlistByComid(int comid)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_com_project> r = new Internal_b2b_com_project(helper).GetProjectlistByComid(comid);
                return r;
            }
        }

        public List<B2b_com_project> Selhotelprojectlist(int comid)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_com_project> r = new Internal_b2b_com_project(helper).Selhotelprojectlist(comid);
                return r;
            }
        }
        public List<B2b_com_project> Selprojectlist(int comid)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_com_project> r = new Internal_b2b_com_project(helper).Selprojectlist(comid);
                return r;
            }
        }
    }
}
