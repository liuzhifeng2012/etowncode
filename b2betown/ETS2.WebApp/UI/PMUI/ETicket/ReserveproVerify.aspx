<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReserveproVerify.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.ETicket.ReserveproVerify" MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            //获取预订产品验证二维码
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetReserveproVerifywxqrcode", { comid: $("#hid_comid").trimVal(), qrcodeviewtype:9}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $("#img1").attr("src", "/Images/defaultThumb.png");

                    //创建预订产品验证二维码
                    $.post("/JsonFactory/WeiXinHandler.ashx?oper=editchannelwxqrcode", { viewchannelcompanyid: 0, id: 0, qrcodename: "公司预订产品验证二维码(系统生成)", viewtype: 9, projectid: 0, productid: 0, materialtype: 0, materialid: 0, channelcompanyid: 0, channelid: 0, promoteact: 0, comid: $("#hid_comid").trimVal(), onlinestatus: 1, micromallimgid: 0 }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            alert(data.msg);
                            return;
                        }
                        if (data.type == 100) { 
                            $("#hid_qrcodeid").val(data.qrcodeid);
                            $("#img1").attr("src", data.qrcodeurl); 
                        }
                    })
                }
                if (data.type == 100) {
                    $("#hid_qrcodeid").val(data.msg.Id);

                    $("#img1").attr("src", data.msg.Qrcodeurl); 
                }
            })

        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/pmui/eticket/eticketindex.aspx" onfocus="this.blur()" target=""><span>
                    电子凭证验证</span></a></li>
                <li><a href="/ui/pmui/eticket/eticketlist.aspx" onfocus="this.blur()" target=""><span>
                    验证明细</span></a></li>
                    <li><a href="/ui/pmui/eticket/InterfaceUseLog.aspx" onfocus="this.blur()"
                    target=""><span>Wl接口验证明细</span></a></li>
                <li class="on"><a href="/ui/pmui/eticket/ReserveproVerify.aspx" onfocus="this.blur()"
                    target=""><span>预订产品验证</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <img src="/Images/defaultThumb.png" id="img1" height="300" width="300" />
                <br />
                <label>
                    二 维 码 图 片</label>
            </div>
        </div>
    </div>
    <input type="hidden" id="hid_qrcodeid" value="" />
</asp:Content>
