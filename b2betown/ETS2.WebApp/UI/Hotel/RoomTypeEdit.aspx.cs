using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using FileUpload.FileUpload.Entities.Enum;
using ETS2.PM.Service.PMService.Modle;
using FileUpload.FileUpload.Data;
using ETS2.PM.Service.PMService.Data;
using ETS2.Common.Business;

namespace ETS2.WebApp.UI.HotelUI
{
    public partial class RoomTypeEdit : System.Web.UI.Page
    {

        public int roomtypeid = 0;//房型id

        protected string headPortraitImgSrc = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            roomtypeid = Request["roomtypeid"].ConvertTo<int>(0);

            BindHeadPortrait();
            if (roomtypeid != 0)
            {
                ShowImgBind(roomtypeid);
            }

        }
        private void BindHeadPortrait()
        {
            headPortrait.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            headPortrait.displayImgId = "headPortraitImg";

        }
        private void ShowImgBind(int roomtypeid)
        {


            //根据产品id得到 房型信息

            B2b_com_roomtype proo = new B2b_com_roomtypeData().GetRoomType(roomtypeid, UserHelper.CurrentCompany.ID);

            if (proo != null)
            {
                var identityFileUpload = new FileUploadData().GetFileById(proo.Roomtypeimg.ToString().ConvertTo<int>(0));
                if(identityFileUpload!=null){
                headPortraitImgSrc = identityFileUpload.Relativepath;
                }


            }
        }
    }
}