<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ShangJiaSet.aspx.cs"
    Inherits="ETS2.WebApp.WeiXin.WxSet" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();

            //获取公司的微信公众平台信息
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=getwxbasic", { comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    return;
                }
                if (data.type == 100) {
                    if (data.msg == null) {
                        $("#hid_weixinbasicid").val(0);
                        
                        $("#txturl").val($("#hid_url").trimVal());
                        $("#txttoken").val($("#hid_token").trimVal());
                    } else {
                        $("#hid_weixinbasicid").val(data.msg.Id);
                         
                        $("#txturl").val(data.msg.Url);
                        $("#txttoken").val(data.msg.Token);
                        $("input:radio[name='radio_weixintype'][value=" + data.msg.Weixintype + "]").attr("checked", "checked");
                    }
                }
            });
            $("#button1").click(function () {
                var wxbasicid = $("#hid_weixinbasicid").trimVal();
                var domain = $("#hid_domain").trimVal();
                var url = $("#txturl").trimVal();
                var token = $("#txttoken").trimVal();
                var weixintype = $('input:radio[name="radio_weixintype"]:checked').trimVal();


                if (weixintype == "") {
                    $.prompt("请选择公众账号类型");
                    return;
                }
                else {
                    $.post("/JsonFactory/WeiXinHandler.ashx?oper=editwxbasicstep1", { weixintype: weixintype, comid: comid, wxbasicid: wxbasicid, domain: domain, url: url, token: token }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt("编辑操作出错");
                            return;
                        }
                        if (data.type == 100) {
                            $.prompt("操作成功，确认进入下一步", { submit: function () { location.href = "shangjiaset2.aspx" } });
                        }
                    });
                }

            })


        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="/weixin/ShangJiaSet.aspx" onfocus="this.blur()"><span>微信接口设置</span></a></li>
                <li><a href="/weixin/WxDefaultReply.aspx" onfocus="this.blur()"><span>微信默认设置</span></a></li>
                <li><a href="/weixin/menulist.aspx" onfocus="this.blur()"><span>菜单管理</span></a></li>
                <li><a href="/MemberShipCard/MemberShipCardList.aspx" onfocus="this.blur()"><span>会员卡专区管理</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">

                <table width="900" border="0" class="grid" style=" margin-left:0px;">
                    <tr>
                        <td colspan="2">
                            <h3>
                                授权设置第一步:
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            公众账号类型:
                        </td>
                        <td>
                            <label>
                                <input id="Radio1" type="radio" name="radio_weixintype" value="1" />订阅号</label>
                            <label>
                                <input id="Radio2" type="radio" name="radio_weixintype" value="2" />认证订阅号</label>
                            <label>
                                <input id="Radio3" type="radio" name="radio_weixintype" value="3" />服务号</label>
                            <label>
                                <input id="Radio4" type="radio" name="radio_weixintype" value="4" />认证服务号</label>
                        </td>
                    </tr>
                    <tr>
                        <td width="200" height="11">
                            URL：
                        </td>
                        <td width="500">
                            <input type="text" id="txturl" size="80" readonly style="background-color: rgb(239, 239, 239);" />
                        </td>
                    </tr>
                    <tr>
                        <td width="200" height="24">
                            <p>
                                Token：</p>
                        </td>
                        <td width="500">
                            <input type="text" id="txttoken" size="80" readonly style="background-color: rgb(239, 239, 239);" />
                        </td>
                    </tr>
                    <tr>
                        <td width="200" height="11" align="right">
                            &nbsp;
                        </td>
                        <td width="500">
                            <input type="button" id="button1" value="  下一步  " />
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_weixinbasicid" value="0" />
    <input type="hidden" id="hid_url" value="<%=url %>" />
    <input type="hidden" id="hid_token" value="<%=token %>" />
    <input type="hidden" id="hid_domain" value="<%=domain %>" />
</asp:Content>
