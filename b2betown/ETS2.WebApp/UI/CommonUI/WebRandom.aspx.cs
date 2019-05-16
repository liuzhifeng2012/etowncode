using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.UI.CommonUI
{
    public partial class WebRandom : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 生成二维码的随机码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            int totalnum = 0;
            IList<RandomCode> totallist = new RandomCodeData().GetAllTotalRandomCode();
            if (totallist == null)
            {
                totalnum = 0;
            }
            else
            {
                totalnum = totallist.Count;
            }


            for (int i = 0; i < 20000000 - totalnum; i++)
            {
                int codeeee = CreateNum();

                RandomCode randddd = new RandomCodeData().GetRandomCodeByCode(codeeee);
                if (randddd == null)
                {

                    RandomCode randomm = new RandomCode
                    {
                        Id = 0,
                        Code = codeeee,
                        State = 0
                    };
                    try
                    {
                        int identity = new RandomCodeData().InsertOrUpdate(randomm);
                    }
                    catch(Exception ex)
                    {
                        i--;
                    }
                }
                else
                {
                    i--;
                }
            }
            Label1.Text = "20000000已经插入";
        }
        /// <summary>
        /// 生成8位随机数字 
        /// </summary>
        /// <returns></returns>
        public static int CreateNum()
        {
            ArrayList MyArray = new ArrayList();
            Random random = new Random();
            return random.Next(10000001, 99999998);
        }


    }
}