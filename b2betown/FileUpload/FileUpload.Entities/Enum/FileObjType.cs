using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace FileUpload.FileUpload.Entities.Enum
{

    [Flags]
    public enum FileObjType
    {

        [EnumAttribute("无")]
        Null = 0,
        [EnumAttribute("Logo")]

        Logo = 1,

        [EnumAttribute("证件扫描")]
        Certificate = 2,

        [EnumAttribute("劳动合同")]
        LabourContract = 3,

        [EnumAttribute("营业执照")]
        Licence = 4,

        [EnumAttribute("责任保险")]
        LiabilityInsurance = 5,

        [EnumAttribute("照片")]
        Photo = 6,

        [EnumAttribute("日志")]

        Log = 7,

        [EnumAttribute("订单")]
        Order = 8,

        [EnumAttribute("公告")]
        Notice = 9,

        [EnumAttribute("订单日志")]
        OrderLog = 0x0A,

        [EnumAttribute("餐饮")]
        Dinner = 11,

        [EnumAttribute("酒店")]
        Hotel = 12,

        [EnumAttribute("票卡")]
        Ticket = 13,

        [EnumAttribute("购物")]
        Shopping = 14,

        [EnumAttribute("娱乐")]
        Amusement = 15,

        [EnumAttribute("线路")]
        Journey = 16,

        [EnumAttribute("投宿")]
        Complaint = 17,

        [EnumAttribute("回访日志")]
        ReturnvisitLog = 18,

        [EnumAttribute("提款日志")]
        WithdrawalLog = 19,

        [EnumAttribute("账单日志")]
        BillLog = 20,

        [EnumAttribute("发票日志")]
        Invoice = 21,

        [EnumAttribute("团房")]
        HotelProduct = 23,


        [EnumAttribute("编辑器")]
        Editor = 24,


    }
}
