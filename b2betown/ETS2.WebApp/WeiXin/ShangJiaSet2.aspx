<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShangJiaSet2.aspx.cs" Inherits="ETS2.WebApp.WeiXin.ShangJiaSet2"
    MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();



            $("#h33").html("");
            //获取公司的微信公众平台信息
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=getwxbasic", { comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $("#h33").html("获取信息失败");
                    return;
                }
                if (data.type == 100) {
                    if (data.msg == null) {
                        $("#h33").html("微信配置信息自动录入失败!");
                        return;
                    } else {
                        $("#hid_weixinbasicid").val(data.msg.Id);
                        $("input:radio[value='" + data.msg.Weixintype + "']").attr("checked", "checked");
                        if (data.msg.AppId != "" && data.msg.AppSecret != "") {
                            $("#h33").html("微信接口已经设置过!");
                        }
                        $("#txtappid").val(data.msg.AppId);
                        $("#txtappsecret").val(data.msg.AppSecret);
                        $("#span_url").html(data.msg.Url);
                        $("#span_token").html(data.msg.Token);
                        $("#span_domain").html(data.msg.Domain);
                    }
                }
            });
            $("#button1").click(function () {
                var wxbasicid = $("#hid_weixinbasicid").trimVal();
                if (wxbasicid == 0) {
                    $("#h33").html("微信配置信息自动录入失败!");
                    return;
                }
                var appid = $("#txtappid").trimVal();
                var appsecret = $("#txtappsecret").trimVal();

                var weixintype = $('input:radio[name="radio_weixintype"]:checked').trimVal();


                if (weixintype == "") {
                    $("#h33").html("请选择公众账号类型");
                    return;
                }
//                if (appid == "" || appsecret == "") {
//                    $("#h33").html("请把appid,appsecret补充完善");
//                    return;
//                }

                $.post("/JsonFactory/WeiXinHandler.ashx?oper=editwxbasicstep", { wxbasicid: wxbasicid, weixintype: weixintype, appid: appid, appsecret: appsecret }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#h33").html("操作失败!");
                        return;
                    }
                    if (data.type == 100) {
                        $("#h33").html("操作成功!下一步可以去设置<a href='MenuList.aspx'>微信菜单</a>");
                        return;
                    }
                });


            })


        })
 

    </script>
    <style type="text/css">
        .button
        {
            width: 140px;
            height: 40px;
            line-height: 8px;
            text-align: center;
            font-weight: bold;
            color: #fff;
            text-shadow: 1px 1px 1px #333;
            border-radius: 5px;
            margin: 0 20px 20px 0;
            position: relative;
            overflow: hidden;
        }
        
        .button.blue
        {
            border: 1px solid #1e7db9;
            box-shadow: 0 1px 2px #8fcaee inset,0 -1px 0 #497897 inset,0 -2px 3px #8fcaee inset;
            background: -webkit-linear-gradient(top,#42a4e0,#2e88c0);
            background: -moz-linear-gradient(top,#42a4e0,#2e88c0);
            background: linear-gradient(top,#42a4e0,#2e88c0);
            margin-top: 20px;
            margin-left: 5px;
        }
        .blue:hover
        {
            background: -webkit-linear-gradient(top,#70bfef,#4097ce);
            background: -moz-linear-gradient(top,#70bfef,#4097ce);
            background: linear-gradient(top,#70bfef,#4097ce);
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="/weixin/ShangJiaSet2.aspx" onfocus="this.blur()"><span>微信接口设置</span></a></li>
                <li><a href="/weixin/WxDefaultReply.aspx" onfocus="this.blur()"><span>微信默认设置</span></a></li>
                <li><a href="/weixin/menulist.aspx" onfocus="this.blur()"><span>自定义菜单管理</span></a></li>
                <li><a href="/MemberShipCard/MemberShipCardList.aspx" onfocus="this.blur()"><span>会员卡专区管理</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
            <h3></h3>

            <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
               <h2 class="p-title-area">微信接口授权设置</h2>
               <div class="mi-form-item">
                    <label class="mi-label">公众账号类型</label>
                     <label>
                                <input id="Radio1" type="radio" name="radio_weixintype" value="1" />订阅号</label>
                            <label>
                                <input id="Radio2" type="radio" name="radio_weixintype" value="2" />认证订阅号</label>
                            <label>
                                <input id="Radio3" type="radio" name="radio_weixintype" value="3" />服务号</label>
                            <label>
                                <input id="Radio4" type="radio" name="radio_weixintype" value="4" />认证服务号</label>
               </div>
               <div class="mi-form-item">
                    <label class="mi-label">URL</label>
                     <span id="span_url"></span>
               </div>
               <div class="mi-form-item">
                    <label class="mi-label">Token</label>
                     <span id="span_token"></span>
               </div>
               <div class="mi-form-item">
                     <span style="font-size: 15px; color: Red;">请把 上方URL和Token 分别填入微信公众平台的服务器配置信息的url和token中，并把得到的开发者凭据数据填入下方的AppId和AppSecret；</span>
               </div>
               <div class="mi-form-item">
                    <label class="mi-label">AppId</label>
                    <input type="text" value="" id="txtappid" autocomplete="off" size="80" />*
               </div>
               <div class="mi-form-item">
                    <label class="mi-label">AppSecret</label>
                     <input type="text" value="" id="txtappsecret" autocomplete="off" size="80" />*
               </div>
               <div class="mi-form-item">
                    <label class="mi-label">授权域名</label>
                     <span id="span_domain"></span><span style="color: Red">(请将此域名填写至微信公众平台OAuth2.0网页授权处,以帮助你获取访问者用户信息.)</span>
               </div>
                <div class="mi-form-explain"></div>
            </div>

            <table width="780" border="0" class="grid" style="margin-left: 0px;">
                    <tr>
                        <td  align="center">
                            <input type="button" id="button1" value="  保存  " class="button blue" style="width: 80px;
                                height: 30px;" />
                            <span id="h33" style="color: Red; text-align: center; font-size: 15px;"></span>
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
</asp:Content>
