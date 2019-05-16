<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShangJiaSet3.aspx.cs" Inherits="ETS2.WebApp.WeiXin.ShangJiaSet3"
    MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();

            $("#span_domain").html($("#hid_domain").trimVal());
            //获取公司的微信公众平台信息
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=getwxbasic", { comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取信息失败");
                    return;
                }
                if (data.type == 100) {
                    if (data.msg == null) {
                        $.prompt("请先补全第一步信息!", {
                            submit: function () {
                                location.href = "shangjiaset.aspx";
                            }
                        });
                    } else {
                        //判断微信认证情况:认证服务号出现域名设置，其他不出现
                        if (data.msg.Weixintype == 4) {
                            $("#tr_domain").show();
                        } else {
                            $("#tr_domain").hide();
                        }

                    }
                }
            });

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
                <table width="900" border="0" class="grid"  style=" margin-left:0px;">
                    <tr>
                        <td colspan="2">
                            <h3>
                                授权设置第三步:
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center; font-size: 20px;">
                            <span>微信接口设置成功！</span>
                        </td>
                    </tr>
                    <tr id="tr_domain">
                        <td colspan="2">
                            授权域名: <span id="span_domain"></span><span style="color: Red">(请将此域名填写至微信公众平台OAuth2.0网页授权处,以帮助你获取访问者用户信息.)</span>
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_domain" value="<%=domain %>" />
</asp:Content>
