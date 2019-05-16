<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="WxAllSendMsgPage.aspx.cs"
    Inherits="ETS2.WebApp.WeiXin.WxAllSendMsgPage" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var pagee = 1;
            var pageSize = 10; //每页显示条数

            //得到公司是否把消息转发到多客服
            $.post("/JsonFactory/AccountInfo.ashx?oper=getcurcompany", { comid: $("#hid_comid").val() }, function (data) {
                dat = eval("(" + data + ")");
                if (dat.type == 1) {

                }
                if (dat.type == 100) {
                    if (dat.msg.B2bcompanyinfo.Istransfer_customer_service == 1) {
                        $("#istransfer_customer_service").attr("checked", "checked");
                    }
                    else {
                        $("#istransfer_customer_service").attr("checked", false);
                    }
                }
            });

            //判断是否将消息转发到多客服
            $("#istransfer_customer_service").change(function () {
                if ($("#istransfer_customer_service").attr("checked") == "checked") {
                    istransfer_customer_service = 1;
                } else {
                    istransfer_customer_service = 0;
                }
                //判断公司是否是出于开发模式，即判断weixinbasic表是否含有记录
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=getwxbasic", { comid: $("#hid_comid").trimVal() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#istransfer_customer_service").attr("checked", false);
                        alert("只有微信公众号处于开发模式才可将消息转发到多客服");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == null) {
                            $("#istransfer_customer_service").attr("checked", false);
                            alert("只有微信公众号处于开发模式才可将消息转发到多客服");
                            return;
                        } else {
                            if (data.msg.Weixintype != 4) {
                                $("#istransfer_customer_service").attr("checked", false);
                                alert("只有微信公众号是认证服务号才可将消息转发到多客服");
                                return;
                            } else {
                                $.post("/JsonFactory/AccountInfo.ashx?oper=editcompanyistran_customer_service", { comid: $("#hid_comid").val(), istransfer_customer_service: istransfer_customer_service }, function (data1) {
                                    data1 = eval("(" + data1 + ")");
                                    if (data.type == 1) {
                                        $("#istransfer_customer_service").attr("checked", false);
                                        alert("调整失败");
                                        return;
                                    }
                                    if (data1.type == 100) {
                                        alert("调整成功");
                                        return;
                                    }
                                })
                            }
                        }
                    }
                })




            })

            SearchList(pagee);



            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/WeiXinHandler.ashx?oper=wxsendmsglistbycomid",
                    data: { userid: $("#hid_userid").trimVal(), comid: $("#hid_comid").trimVal(), pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            //                            $.prompt("查询微信交互信息列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                //                                $("#tblist").html("<tr><td height=\"26\" colspan=\"7\">查询数据为空</td></tr>");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);
                            }


                        }
                    }
                })


            }

            //分页
            function setpage(newcount, newpagesize, curpage) {
                $("#divPage").paginate({
                    count: Math.ceil(newcount / newpagesize),
                    start: curpage,
                    display: 5,
                    border: false,
                    text_color: '#888',
                    background_color: '#EEE',
                    text_hover_color: 'black',
                    images: false,
                    rotate: false,
                    mouse: 'press',
                    onChange: function (page) {

                        SearchList(page);

                        return false;
                    }
                });
            }
        })
    </script>
    <style type="text/css">
        .message_status
        {
            float: right;
            width: 200px;
            min-height: 1em;
            margin-top: 0;
            color: #7b7b7b;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="WxAllSendMsgPage.aspx" onfocus="this.blur()"><span>微信留言</span></a></li>
                <li><a href="wx_kflist.aspx" onfocus="this.blur()"><span>微信多客服列表</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                   只显示48小时内的留言及会员互动信息<a href="javascript:void(0)" onclick="javascript:history.go(0)" style="color: rgb(123, 123, 123);" class="a_anniu">点击刷新按钮</a>
                    <%--  <label style="padding-left: 20px;">
                        <input type="checkbox" id="istransfer_customer_service" value="1" />将消息发送到多客服[<span
                            title="如果公众号处于开发模式,微信服务器在收到这条消息时，会把当次发送的消息转发至多客服系统。消息被转发到多客服以后，会被自动分配给一个在线的客服帐号。"
                            style="cursor: pointer">?</span>]</label>--%>
                </h3>
            </div>
            <ul id="tblist" class="message_list" style="margin: 0px; padding-left: 0px; list-style-type: none;
                border-top-width: 1px; border-top-style: solid; border-top-color: rgb(220, 220, 220);
                color: rgb(34, 34, 34); font-family: 'Microsoft YaHei', 微软雅黑, Helvetica, 黑体, Arial, Tahoma;
                font-size: 14px; font-style: normal; font-variant: normal; font-weight: normal;
                letter-spacing: normal; line-height: 22px; orphans: auto; text-align: start;
                text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px;
                -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);">
            </ul>
            <div id="divPage">
            </div>
        </div>
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
     <li id="msgListItem200015966" class="message_item " data-id="200015966" style="min-height: 56px;
            border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: rgb(221, 220, 220);
            padding: 15px 0px;">
            <div class="message_info" style="margin-right: 160px;min-height:60px;">
                <div class="message_status" style="float: right;min-width:130px;  min-height: 1em; margin-top: 0px;
                    color: rgb(123, 123, 123);">
                     ${ChannelCompanyName}
                </div>
                {{if WhetherRenZheng==true}}
                <div class="message_time" style="float: right; width: 85px; margin-top: 0px; color: rgb(123, 123, 123);">
                  <a href="WxSingleSendMsgPage.aspx?fromusername=${FromUserName}&comid=${ComId}" style="color:Blue;font-size:12px;">微信回复</a>
                  
                 </div> {{/if}}
                {{if WhetherReply==true}}<div class="message_status"><em class="tips" style="color:#a67c52"><i>●</i>已回复<label style=" color:#9bb668; font-size:10px;">${LasterReplyTime}</label></em></div>
                {{else}}
                <div class="message_status"><em class="tips" style="color:Red;"><i>●</i>未回复</em></div>
                {{/if}}
                <div class="user_info" style="position: relative; margin-left: 80px; margin-right: 315px;">
                    <a class="remark_name" data-fakeid="731902502" href="#"
                        style="outline: 0px; color: rgb(34, 34, 34); text-decoration: none;" target="_blank">
                        会员ID:${Crmid},</a>
                        <span class="Apple-converted-space">&nbsp;{{if KeRenName!=""}}姓名:${KeRenName},{{/if}}&nbsp;{{if KeRenPhone!=""}}电话:${KeRenPhone},{{/if}}&nbsp;{{if Nickname!=""}}昵称:${Nickname},{{/if}}{{if Province!=""}}省份:${Province},{{/if}}&nbsp;{{if City!=""}}城市:${City},{{/if}}&nbsp;{{if Sex!=""}}性别:${Sex}{{/if}} </span>
                        <span class="nickname" data-fakeid="731902502"></span>
                        {{if WhetherRenZheng==true}}
                        <a  class="icon14_common edit_gray js_changeRemark" data-fakeid="731902502" href="WxSingleSendMsgPage.aspx?fromusername=${FromUserName}&comid=${ComId}"
                            style="outline: 0px; color: rgb(46, 125, 198); text-decoration: none; width: 14px;
                            height: 14px; vertical-align: middle; display: inline-block; line-height: 100px;
                            overflow: hidden; background-image: url(https://res.wx.qq.com/mpres/htmledition/style/xss/base/base_z.png);
                            margin-top: -0.2em; margin-left: 4px; background-position: 0px -1305px; background-repeat: no-repeat no-repeat;"
                            title="点击回复">
                        </a>
                        {{else}}
                        <a  class="icon14_common edit_gray js_changeRemark" data-fakeid="731902502" href="#"
                            style="outline: 0px; color: rgb(46, 125, 198); text-decoration: none; width: 14px;
                            height: 14px; vertical-align: middle; display: inline-block; line-height: 100px;
                            overflow: hidden; background-image: url(https://res.wx.qq.com/mpres/htmledition/style/xss/base/base_z.png);
                            margin-top: -0.2em; margin-left: 4px; background-position: 0px -1305px; background-repeat: no-repeat no-repeat;"
                            title="微信账户还没有认证，只有认证后才能直接回复客户留言">
                        {{/if}}
                        <a class="avatar" data-fakeid="731902502" href="#"
                                style="outline: 0px; color: rgb(46, 125, 198); text-decoration: none; position: absolute;
                                top: 0px; left: -60px;" target="_blank">
                                <img data-fakeid="731902502" src="${Headimgurl}"
                                    style="border: 0px; width: 48px; height: 48px;" />
                        </a>
                </div>
            </div>
            <div class="message_content text" style="margin-left: 80px; margin-right: 365px;
                padding-top: 0px; color: rgb(123, 123, 123); word-wrap: break-word; word-break: break-all;
                padding-bottom: 2px;">
                <div id="wxMsg200015966" class="wxMsg" data-id="200015966">
                     {{if MsgType=="text"}}
                   ${Content} &nbsp;&nbsp;<label style=" color:#9bb668; font-size:10px;">${CreateTime}</label>
                 {{else MsgType=="voice"}}${Recognition}&nbsp;&nbsp;<label style=" color:#9bb668; font-size:10px;">${CreateTime}</label>
                 {{else  MsgType=="location"}}
                    ${Content}&nbsp;${Label}&nbsp;<label style=" color:#9bb668; font-size:10px;">${CreateTime}</label>
                 {{else  MsgType=="image"}}
                   <a href="${PicUrl}"><img   src="${PicUrl}" width="50px" height="50px" /></a>&nbsp;&nbsp;<label style=" color:#9bb668; font-size:10px;">${CreateTime}</label>
                   {{else MsgType=="event"}}${Content}&nbsp;&nbsp;<label style=" color:#9bb668; font-size:10px;">${CreateTime}</label>
                 {{/if}}
                </div>
            </div>
        </li>
    <hr />    
    </script>
    <div>
    </div>
</asp:Content>
