// JavaScript Document
var LODOP; //声明为全局变量  
//LODOP.SET_LICENSES("", "B394A2AE75F8100EEFFD09D0536B694A", "0E0EBF7986BB37DF2EBD8F13DBC13B3C", "74922499EDC8DBCFED7FF44823362A48");

function prn_Print(str0, str1, str2, str3, str4, str5, str6, str8, strnum, pritadmin, printname) {
    CreatePrintPage(str0, str1, str2, str3, str4, str5, str6, str8, strnum, pritadmin);
    if (LODOP.SET_PRINTER_INDEX(printname))
        LODOP.PRINT();
};
function CreatePrintPage(str0, str1, str2, str3, str4, str5, str6, str8, strnum, pritadmin) {
    LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
    LODOP.SET_LICENSES("", "B394A2AE75F8100EEFFD09D0536B694A", "0E0EBF7986BB37DF2EBD8F13DBC13B3C", "74922499EDC8DBCFED7FF44823362A48");
    //LODOP.PRINT_INIT("电子凭证打印");
    LODOP.SET_PRINT_PAGESIZE(3, 550, 0, "");
    LODOP.ADD_PRINT_TEXT(8, 0, 185, 28, str0);
    LODOP.SET_PRINT_TEXT_STYLE(1, "黑体", 16, 1, 0, 0, 2);
    LODOP.ADD_PRINT_TEXT(44, 53, 130, 22, "请妥善保存");
    LODOP.SET_PRINT_TEXT_STYLE(2, "黑体", 9, 0, 0, 0, 3);
    LODOP.ADD_PRINT_TEXT(97, 0, 70, 20, "服务项目:");
    LODOP.SET_PRINT_TEXT_STYLE(3, "黑体", 9, 0, 0, 0, 1);
    LODOP.ADD_PRINT_TEXT(154, 57, 130, 20, str4);
    LODOP.SET_PRINT_TEXT_STYLE(4, "黑体", 9, 0, 0, 0, 1);
    LODOP.ADD_PRINT_TEXT(135, 0, 70, 20, "票    价:");
    LODOP.SET_PRINT_TEXT_STYLE(5, "黑体", 9, 0, 0, 0, 1);
    LODOP.ADD_PRINT_TEXT(135, 57, 130, 20, str3);
    LODOP.SET_PRINT_TEXT_STYLE(6, "黑体", 9, 0, 0, 0, 1);
    LODOP.ADD_PRINT_TEXT(77, 0, 70, 20, "商户编号:");
    LODOP.SET_PRINT_TEXT_STYLE(7, "黑体", 9, 0, 0, 0, 1);
    LODOP.ADD_PRINT_TEXT(116, 16, 170, 20, str2);
    LODOP.SET_PRINT_TEXT_STYLE(8, "黑体", 9, 0, 0, 0, 1);
    LODOP.ADD_PRINT_TEXT(154, 0, 70, 20, "出票单位:");
    LODOP.SET_PRINT_TEXT_STYLE(9, "黑体", 9, 0, 0, 0, 1);
    LODOP.ADD_PRINT_TEXT(77, 57, 130, 20, str1);
    LODOP.SET_PRINT_TEXT_STYLE(10, "黑体", 9, 0, 0, 0, 1);
    LODOP.ADD_PRINT_TEXT(173, 0, 70, 20, "");
    LODOP.SET_PRINT_TEXT_STYLE(11, "黑体", 9, 0, 0, 0, 1);
    LODOP.ADD_PRINT_TEXT(173, 57, 130, 20, str5);
    LODOP.SET_PRINT_TEXT_STYLE(12, "黑体", 9, 0, 0, 0, 1);
    LODOP.ADD_PRINT_TEXT(44, 0, 70, 20, "商户存根:");
    LODOP.SET_PRINT_TEXT_STYLE(13, "黑体", 9, 0, 0, 0, 1);
    LODOP.ADD_PRINT_DNLINE(70, 6, 267, 0, 0, 1);
    LODOP.ADD_PRINT_UPLINE(196, 8, 262, 1, 0, 1);
    LODOP.ADD_PRINT_TEXT(200, 0, 70, 20, "使用数量:");
    LODOP.ADD_PRINT_TEXT(200, 57, 130, 20, strnum);
    LODOP.ADD_PRINT_TEXT(218, 0, 106, 20, "电子票号:");
    LODOP.ADD_PRINT_TEXT(230, 30, 163, 28, str6);
    LODOP.SET_PRINT_TEXT_STYLE(17, "宋体", 12, 0, 0, 0, 1);
    LODOP.ADD_PRINT_BARCODE(245, 30, 116, 114, "QRCode", str6);
    LODOP.SET_PRINT_STYLEA(0, "QRCodeVersion", 3);
    LODOP.ADD_PRINT_TEXT(340, 0, 70, 20, "验票员:");
    LODOP.ADD_PRINT_TEXT(340, 57, 130, 20, pritadmin);
    LODOP.ADD_PRINT_TEXT(360, 0, 70, 20, "验票时间:");
    LODOP.ADD_PRINT_TEXT(360, 57, 130, 20, str8);
    LODOP.ADD_PRINT_DNLINE(355, 8, 262, 1, 0, 1);
    LODOP.ADD_PRINT_TEXT(380, 0, 130, 20, "");
};



function ACT_Print(str0, str1, str2, str3, str4, str5, str6, str8, strnum, printname, orderid, servername, salepeople, money, admin) {
    CreatePrintACT(str0, str1, str2, str3, str4, str5, str6, str8, strnum, orderid, servername, salepeople, money, admin);
    if (LODOP.SET_PRINTER_INDEX(printname))
        LODOP.PRINT();
};

function CreatePrintACT(str0, str1, str2, str3, str4, str5, str6, str8, strnum, orderid, servername, salepeople, money, admin) {
    LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
    LODOP.SET_LICENSES("", "B394A2AE75F8100EEFFD09D0536B694A", "0E0EBF7986BB37DF2EBD8F13DBC13B3C", "74922499EDC8DBCFED7FF44823362A48");
    //LODOP.PRINT_INIT("优惠活动验票单");
    LODOP.SET_PRINT_PAGESIZE(3, 550, 0, "");
    LODOP.ADD_PRINT_TEXT(8, 0, 185, 28, str0);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 16);
    LODOP.SET_PRINT_STYLEA(0, "Alignment", 2);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.ADD_PRINT_TEXT(44, 0, 70, 20, "商户存根:");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.ADD_PRINT_TEXT(44, 53, 130, 22, "请妥善保存");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "Alignment", 3);
    LODOP.ADD_PRINT_LINE(70, 6, 70, 273, 0, 1);
    LODOP.ADD_PRINT_TEXT(77, 0, 70, 20, "验票账户:");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.ADD_PRINT_TEXT(77, 57, 130, 20, str1);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.ADD_PRINT_TEXT(116, 16, 170, 20, str2);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.ADD_PRINT_TEXT(97, 0, 70, 20, "服务项目:");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.ADD_PRINT_TEXT(154, -1, 70, 20, "抵扣金额:");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.ADD_PRINT_TEXT(154, 56, 130, 20, str3);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.ADD_PRINT_TEXT(277, 0, 70, 20, "渠道卡号");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.ADD_PRINT_TEXT(277, 57, 130, 20, str5);
    LODOP.ADD_PRINT_LINE(301, 8, 300, 270, 0, 1);
    LODOP.ADD_PRINT_TEXT(304, 0, 70, 20, "使用数量:");
    LODOP.ADD_PRINT_TEXT(304, 57, 130, 20, strnum);
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 12);
    LODOP.ADD_PRINT_TEXT(322, 0, 106, 20, "客人卡号:");
    LODOP.ADD_PRINT_TEXT(334, 30, 163, 28, str6);
    LODOP.ADD_PRINT_BARCODE(349, 30, 116, 114, "QRCode", str6);
    LODOP.SET_PRINT_STYLEA(0, "QRCodeVersion", 3);
    LODOP.ADD_PRINT_LINE(439, 8, 440, 270, 0, 1);
    LODOP.ADD_PRINT_TEXT(444, 0, 70, 20, "验票时间:");
    LODOP.ADD_PRINT_TEXT(444, 57, 130, 20, str8);
    LODOP.ADD_PRINT_TEXT(174, -2, 70, 20, "订单编号:");
    LODOP.ADD_PRINT_TEXT(174, 56, 130, 20, orderid);
    LODOP.ADD_PRINT_TEXT(194, 0, 70, 20, "服务项目:");
    LODOP.ADD_PRINT_TEXT(194, 56, 130, 20, servername);
    LODOP.ADD_PRINT_TEXT(214, 0, 70, 20, "消费人数:");
    LODOP.ADD_PRINT_TEXT(214, 56, 130, 20, salepeople);
    LODOP.ADD_PRINT_TEXT(234, 0, 70, 20, "会员奖励:");
    LODOP.ADD_PRINT_TEXT(234, 56, 130, 20, money);
    LODOP.ADD_PRINT_TEXT(254, 0, 70, 20, "服务专员:");
    LODOP.ADD_PRINT_TEXT(254, 56, 130, 20, admin);
    LODOP.ADD_PRINT_TEXT(464, 56, 130, 20, "");
};


function IMPINT_Print(str0, str1, str2, str3, str4, str5, str6, str8, strnum, printname, orderid, servername, salepeople, money, admin) {
    CreatePrintIMPINT(str0, str1, str2, str3, str4, str5, str6, str8, strnum, orderid, servername, salepeople, money, admin);
    if (LODOP.SET_PRINTER_INDEX(printname))
        LODOP.PRINT();
};

function CreatePrintIMPINT(str0, str1, str2, str3, str4, str5, str6, str8, strnum, orderid, servername, salepeople, money, admin) {
    LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
    LODOP.SET_LICENSES("", "B394A2AE75F8100EEFFD09D0536B694A", "0E0EBF7986BB37DF2EBD8F13DBC13B3C", "74922499EDC8DBCFED7FF44823362A48");
    //LODOP.PRINT_INIT("预付款积分使用验证单");
    LODOP.SET_PRINT_PAGESIZE(3, 550, 0, "");
    LODOP.ADD_PRINT_TEXT(8, 0, 185, 28, str0);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 16);
    LODOP.SET_PRINT_STYLEA(0, "Alignment", 2);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.ADD_PRINT_TEXT(44, 0, 70, 20, "商户存根:");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.ADD_PRINT_TEXT(44, 53, 130, 22, "请妥善保存");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "Alignment", 3);
    LODOP.ADD_PRINT_LINE(70, 6, 70, 273, 0, 1);
    LODOP.ADD_PRINT_TEXT(77, 0, 70, 20, "验票账户:");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.ADD_PRINT_TEXT(77, 57, 130, 20, str1);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.ADD_PRINT_TEXT(116, 16, 170, 20, str2);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.ADD_PRINT_TEXT(97, 0, 70, 20, "服务项目:");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.ADD_PRINT_TEXT(154, -1, 70, 20, "使用金额:");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.ADD_PRINT_TEXT(154, 56, 130, 20, str3);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.ADD_PRINT_TEXT(174, -2, 70, 20, "订单编号:");
    LODOP.ADD_PRINT_TEXT(174, 56, 130, 20, orderid);
    LODOP.ADD_PRINT_TEXT(194, 0, 70, 20, "服务专员:");
    LODOP.ADD_PRINT_TEXT(194, 56, 130, 20, admin);
    LODOP.ADD_PRINT_LINE(214, 8, 214, 270, 0, 1);
    LODOP.ADD_PRINT_TEXT(234, 0, 106, 20, "客人卡号:");
    LODOP.ADD_PRINT_TEXT(246, 30, 163, 28, str6);
    LODOP.ADD_PRINT_BARCODE(261, 30, 116, 114, "QRCode", str6);
    LODOP.SET_PRINT_STYLEA(0, "QRCodeVersion", 3);
    LODOP.ADD_PRINT_LINE(351, 8, 351, 270, 0, 1);
    LODOP.ADD_PRINT_TEXT(371, 0, 70, 20, "验票时间:");
    LODOP.ADD_PRINT_TEXT(371, 57, 130, 20, str8);
    LODOP.ADD_PRINT_TEXT(391, 56, 130, 20, "");

};



//str0=名称,str1=核对号,str2=票价,str3=有效期，str4=电话,str5=地址，str6=备注，str7=票号,str8=出票时间（临时改动电子码）,str9=出票单位
function CreatePrintPageTicket(str0, str1, str2, str3, str4, str5, str6, str7, str8,str9,str10,i) {
    LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
    LODOP.SET_LICENSES("", "B394A2AE75F8100EEFFD09D0536B694A", "0E0EBF7986BB37DF2EBD8F13DBC13B3C", "74922499EDC8DBCFED7FF44823362A48");
    //LODOP.PRINT_INITA("纸质票");
    LODOP.SET_PRINT_PAGESIZE(2, 860, 2000, "Letter");
    LODOP.ADD_PRINT_TEXT(44, 110, 476, 28, str0);//212 -102
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 16);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.ADD_PRINT_TEXT(100, 109, 85, 20, "票    价:"); //211
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(100, 183, 260, 20, str2);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(121, 109, 85, 20, "有效日期:");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(121, 183, 260, 20, str3);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(142, 109, 84, 20, "出票单位:");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(142, 183, 260, 20, str9);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(163, 109, 85, 20, "服务电话:");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(163, 183, 260, 20, str4);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    //LODOP.ADD_PRINT_TEXT(210, 211, 47, 20, "备注:");
    //LODOP.SET_PRINT_STYLEA(0, "FontSize", 8);
    LODOP.ADD_PRINT_TEXT(210, 109, 340, 83, str6);
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 9);
    LODOP.ADD_PRINT_TEXT(295, 376, 35, 20, "NO:");
    LODOP.ADD_PRINT_TEXT(295, 397, 110, 20, str1);
    LODOP.ADD_PRINT_TEXT(295, 540, 37, 20, "NO:");
    LODOP.ADD_PRINT_TEXT(295, 560, 110, 20, str1);
    LODOP.ADD_PRINT_IMAGE(100, 447, 300, 150, "<img border='0' src='/ui/pmui/eticket/showtcode.aspx?pno=http://y.etown.cn' width='90'/><br><font size='2' align='center' style='line-height:2;'><b>扫描二维码预约</b></font><br>      <font  size='1' align='center'>&nbsp;&nbsp;</font>");
    LODOP.ADD_PRINT_TEXT(185, 109, 85, 20, "预约网址:"); //临时改动 原出票时间
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(185, 184, 260, 20, str8);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);

    LODOP.ADD_PRINT_TEXT(69,109,260,20,str5);
    LODOP.SET_PRINT_STYLEA(0,"FontName","黑体");
    LODOP.SET_PRINT_STYLEA(0,"FontSize",10);

    if (i == 0) {
        if (LODOP.SELECT_PRINTER() >= 0) {
            LODOP.PRINT();
        }
        else {
            alert("选择打印失败，将自动使用默认打印机进行打印！");
            LODOP.PRINT();
        }
    } else {
        LODOP.PRINT();
    }
};


//str0=名称,str1=核对号,str2=票价,str3=有效期，str4=电话,str5=地址，str6=备注，str7=票号,str8=出票时间（临时改动电子码）,str9=出票单位
function CreatePrintPageTicket_yuyuema(str0, str1, str2, str3, str4, str5, str6, str7, str8, str9, str10, i) {
    LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
    LODOP.SET_LICENSES("", "B394A2AE75F8100EEFFD09D0536B694A", "0E0EBF7986BB37DF2EBD8F13DBC13B3C", "74922499EDC8DBCFED7FF44823362A48");
    //LODOP.PRINT_INITA("纸质票");
    LODOP.SET_PRINT_PAGESIZE(2, 860, 2000, "Letter");
    LODOP.ADD_PRINT_TEXT(44, 110, 476, 28, str0); //212 -102
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 16);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.ADD_PRINT_TEXT(100, 109, 85, 20, "票    价:"); //211
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(100, 183, 260, 20, str2);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(121, 109, 85, 20, "有效日期:");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(121, 183, 260, 20, str3);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(142, 109, 84, 20, "出票单位:");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(142, 183, 260, 20, str9);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(163, 109, 85, 20, "班车服务:");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(163, 183, 260, 20, str4);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    //LODOP.ADD_PRINT_TEXT(210, 211, 47, 20, "备注:");
    //LODOP.SET_PRINT_STYLEA(0, "FontSize", 8);
    LODOP.ADD_PRINT_TEXT(210, 109, 340, 83, str6);
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 9);
    LODOP.ADD_PRINT_TEXT(295, 376, 35, 20, "NO:");
    LODOP.ADD_PRINT_TEXT(295, 397, 110, 20, str1);
    LODOP.ADD_PRINT_TEXT(295, 540, 37, 20, "NO:");
    LODOP.ADD_PRINT_TEXT(295, 560, 110, 20, str1);
    LODOP.ADD_PRINT_IMAGE(100, 447, 300, 150, "<img border='0' src='/ui/pmui/eticket/showtcode.aspx?pno=http://shop.etown.cn/H5/orderlist.aspx?pno=" + str7 + "' width='90'/><br><font size='2' align='center' style='line-height:2;'><b>扫描二维码预约</b></font><br>      <font  size='1' align='center'>&nbsp;&nbsp;</font>");
    //LODOP.ADD_PRINT_TEXT(185, 109, 85, 20, "预 约 码:"); //临时改动 原出票时间
   // LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    //LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    //LODOP.ADD_PRINT_TEXT(185, 184, 260, 20, str8);
   // LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
  //  LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);

    LODOP.ADD_PRINT_TEXT(69, 109, 260, 20, str5);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 10);

    if (i == 0) {
        if (LODOP.SELECT_PRINTER() >= 0) {
            LODOP.PRINT();
        }
        else {
            alert("选择打印失败，将自动使用默认打印机进行打印！");
            LODOP.PRINT();
        }
    } else {
        LODOP.PRINT();
    }
};



//str0=名称,str1=核对号,str2=票价,str3=有效期，str4=电话,str5=地址，str6=备注，str7=票号,str8=出票时间（临时改动电子码）,str9=出票单位
function CreatePrintPageTicket_dianzipiao(str0, str1, str2, str3, str4, str5, str6, str7, str8, str9, str10, i) {
    LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
    LODOP.SET_LICENSES("", "B394A2AE75F8100EEFFD09D0536B694A", "0E0EBF7986BB37DF2EBD8F13DBC13B3C", "74922499EDC8DBCFED7FF44823362A48");
    //LODOP.PRINT_INITA("纸质票");
    LODOP.SET_PRINT_PAGESIZE(2, 860, 2000, "Letter");
    LODOP.ADD_PRINT_TEXT(44, 180, 476, 28, str0); //212 -102
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 16);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.ADD_PRINT_TEXT(100, 179, 85, 20, "票    价:"); //211
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(100, 253, 260, 20, str2);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(121, 179, 85, 20, "有效日期:");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(121, 253, 260, 20, str3);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(142, 179, 84, 20, "出票单位:");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(142, 253, 260, 20, str9);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(163, 179, 85, 20, "客服电话:");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(163, 253, 260, 20, str4);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    //LODOP.ADD_PRINT_TEXT(210, 191, 47, 20, "备注:");
    //LODOP.SET_PRINT_STYLEA(0, "FontSize", 8);
    LODOP.ADD_PRINT_TEXT(210, 179, 340, 83, str6);
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 9);
    LODOP.ADD_PRINT_TEXT(295, 446, 35, 20, "NO:");
    LODOP.ADD_PRINT_TEXT(295, 467, 110, 20, str1);
    LODOP.ADD_PRINT_TEXT(295, 610, 37, 20, "NO:");
    LODOP.ADD_PRINT_TEXT(295, 630, 110, 20, str1);
    LODOP.ADD_PRINT_IMAGE(100, 517, 300, 150, "<img border='0' src='/ui/pmui/eticket/showtcode.aspx?pno=http://shop.etown.cn/H5/orderlist.aspx?pno=" + str7 + "' width='90'/><br><font size='2' align='center' style='line-height:2;'><b>不要污损二维码</b></font><br>      <font  size='1' align='center'>&nbsp;&nbsp;</font>");
    LODOP.ADD_PRINT_TEXT(185, 179, 85, 20, "辅    码:");
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.ADD_PRINT_TEXT(185, 254, 260, 20, str8);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);

    LODOP.ADD_PRINT_TEXT(69, 179, 260, 20, str5);
    LODOP.SET_PRINT_STYLEA(0, "FontName", "黑体");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 10);

    if (i == 0) {
        if (LODOP.SELECT_PRINTER() >= 0) {
            LODOP.PRINT();
        }
        else {
            alert("选择打印失败，将自动使用默认打印机进行打印！");
            LODOP.PRINT();
        }
    } else {
        LODOP.PRINT();
    }
};	
