<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="WxSingleSendMsgPage.aspx.cs"
    Inherits="ETS2.WebApp.WeiXin.WxSingleSendMsgPage" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var pagee = 1;
            var pageSize = 10; //每页显示条数
            SearchList(pagee);

            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/WeiXinHandler.ashx?oper=wxsendmsglistbyfromuser",
                    data: { comid: $("#hid_comid").trimVal(), fromusername: $("#hid_fromusername").trimVal(), pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询微信交互信息列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist").html("<tr><td height=\"26\" colspan=\"7\">查询数据为空</td></tr>");
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

            $('input[name="radpromotetype"]').live("click", function () {
                var type = $('input:radio[name="radpromotetype"]:checked').trimVal();
                if (type == "1") {//回复文字
                    $("#trtxt").show();
                    $("#trimg").hide();
                }
                else if (type == "2") {//回复图片
                    $("#trtxt").hide();
                    $("#trimg").show();
                }
                else {//回复图文
                    $("#trtxt").show();
                    $("#trimg").show();
                }
            });

        })
        //发送微信交互信息
        function sendwxmsg() {
            var type = $('input:radio[name="radpromotetype"]:checked').trimVal();
            var img1 = $("#<%=UploadFile1.FileUploadId_ClientId %>").val();
            var txt1 = $("#sendmsgtxt").trimVal();
            if (type == 1) {
                if (txt1 == "") {
                    alert("文字必须为1到600个字");
                    return;
                }
                img1 = "";

            }
            else if (type == 2) {
                txt1 = "";
                if (img1 == "") {
                    alert("内容必须含有图片");
                    return;
                }
            }
            else {
                if (txt1 == "") {
                    alert("文字必须为1到600个字");
                    return;
                }
                if (img1 == "") {
                    alert("内容必须含有图片");
                    return;
                }
            }

            $.post("/JsonFactory/WeiXinHandler.ashx?oper=sendwxmsg", { userid: $("#hid_userid").trimVal(), comid: $("#hid_comid").trimVal(), fromusername: $("#hid_fromusername").trimVal(), type: type, img: img1, txt: txt1 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                    window.location.reload();
                    //                    $.prompt("回复客服信息成功", {
                    //                        buttons: [
                    //                                                     { title: '确定', value: true }
                    //                                                    ],
                    //                        opacity: 0.1,
                    //                        focus: 0,
                    //                        show: 'slideDown',
                    //                        submit: function (e, v, m, f) {
                    //                            if (v == true) {
                    //                                $("#sendmsgtxt").val("");
                    //                                window.location.reload();
                    //                            }
                    //                        }
                    //                    });
                }
                if (data.type == 1) {
                    //                    alert("回复客服信息出错");
                    alert(data.msg);
                    return;
                }
            })
        }
    </script>
    <style type="text/css">
        .btn_primary
        {
            background-color: #56a447;
            background-image: -moz-linear-gradient(top,#60b452 0,#56a447 100%);
            background-image: -webkit-gradient(linear,0 0,0 100%,from(#60b452),to(#56a447));
            background-image: -webkit-linear-gradient(top,#60b452 0,#56a447 100%);
            background-image: -o-linear-gradient(top,#60b452 0,#56a447 100%);
            background-image: linear-gradient(to bottom,#60b452 0,#56a447 100%);
            border-color: #3d810c;
            color: #fff;
        }
        .btn
        {
            display: inline-block;
            overflow: visible;
            padding: 0 36px;
            height: 30px;
            line-height: 30px;
            vertical-align: middle;
            text-align: center;
            text-decoration: none;
            border-radius: 3px;
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            font-size: 14px;
            border-width: 1px;
            border-style: solid;
            cursor: pointer;
        }
        .tab_panel
        {
            background-color: #fff;
            min-height: 120px;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="WxAllSendMsgPage.aspx" onfocus="this.blur()"><span>微信留言</span></a></li>
                <li><a href="wx_kflist.aspx" onfocus="this.blur()"><span>微信多客服列表</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div id="msgSender">
                <div class="msg_sender" id="js_msgSender">
                    <div class="tab_panel" style="padding: 20px 0 10px 0;">
                        <table>
                            <tr>
                                <td id="tdgroups">
                                    <lable style="font-size: medium; font-weight: bold;"> 回复微信内容:</lable>
                                    <label style="display: none;">
                                        <input checked="checked" name="radpromotetype" type="radio" value="1" />文 字
                                    </label>
                                    <%--  <label>
                                <input name="radpromotetype" type="radio" value="2" />图 片
                            </label>
                             <label>
                                <input name="radpromotetype" type="radio" value="3" />图 文
                            </label>--%>
                                </td>
                            </tr>
                            <tr id="trtxt">
                                <td>
                                    <textarea id="sendmsgtxt" rows="8" cols="120" autocomplete="off"></textarea>
                                </td>
                            </tr>
                            <tr id="trimg" style="display: none;">
                                <td>
                                    <div class="C_head">
                                        <dl>
                                            <dt>
                                                <input type="hidden" id="Hidden1" value="" />
                                                <img alt="" class="headPortraitImgSrc" id="Img1" src="" /></dt>
                                            <dd>
                                                Logo格式为jpg
                                            </dd>
                                        </dl>
                                        <div class="cl">
                                        </div>
                                    </div>
                                    <div class="C_head_no">
                                        <div class="C_head_1">
                                            <ul>
                                                <li style="height: 20px; margin-left: 40px">
                                                    <div class="C_verify">
                                                        <span>
                                                            <uc1:uploadFile ID="UploadFile1" runat="server" />
                                                        </span>
                                                    </div>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="btn btn_primary btn_input" id="js_submit" onclick="sendwxmsg()">发送
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <h4 style="margin: 0px; padding: 10px 0px; font-weight: 400; font-style: normal;
                color: rgb(34, 34, 34); font-family: 'Microsoft YaHei', 微软雅黑, Helvetica, 黑体, Arial, Tahoma;
                font-size: 14px; font-variant: normal; letter-spacing: normal; line-height: 22px;
                orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal;
                widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);">
                最近5天聊天记录<a href="javascript:void(0)" onclick="javascript:history.go(0)" style="font-size: 15px;
                    color: Blue;">点击刷新按钮</a></h4>
            <ul id="tblist" class="message_list" style="margin: 10px 0; padding-left: 0px; padding-top: 10px;
                list-style-type: none; border-top-width: 1px; border-top-style: solid; border-top-color: rgb(220, 220, 220);
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
     <li id="msgListItem200015966" class="message_item " data-id="200015966" style="min-height: 46px;
            border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: rgb(221, 220, 220);
            padding: 15px 0px;">
            <div class="message_info" style="margin-right: 20px;">
               
                <div class="message_time" style="float: right; width: 85px; margin-top: 0px; color: rgb(123, 123, 123);">
                   ${CreateTime}</div>
                <div class="user_info" style="position: relative; margin-left: 80px; margin-right: 215px;">
                    <a class="remark_name" data-fakeid="731902502" href="#"
                        style="outline: 0px; color: rgb(34, 34, 34); text-decoration: none;" target="_blank">
                        ${FromUserName}</a>
                        <span class="Apple-converted-space">&nbsp;
                        {{if ContentType==1}}
                        (客户信息:{{if KeRenName!=""}}姓名:${KeRenName},{{/if}}{{if KeRenPhone!=""}}电话:${KeRenPhone},{{/if}}{{if Nickname!=""}}昵称:${Nickname},{{/if}}{{if Province!=""}}省份:${Province},{{/if}}&nbsp;{{if City!=""}}城市:${City}{{/if}}&nbsp;{{if Sex!=""}}性别:${Sex}{{/if}})
                        {{else}}
                        (客服信息:{{if KeFuName!=""}}${KeFuName}{{else}}无{{/if}})
                        {{/if}}
                        </span><span class="nickname" data-fakeid="731902502"></span><a class="icon14_common edit_gray js_changeRemark" data-fakeid="731902502" href="javascript:;"
                            style="outline: 0px; color: rgb(46, 125, 198); text-decoration: none; width: 14px;
                            height: 14px; vertical-align: middle; display: inline-block; line-height: 100px;
                            overflow: hidden; background-image: url(https://res.wx.qq.com/mpres/htmledition/style/xss/base/base_z.png);
                            margin-top: -0.2em; margin-left: 4px; background-position: 0px -1305px; background-repeat: no-repeat no-repeat;"
                            title="修改备注"></a><a class="avatar" data-fakeid="731902502" href="#"
                                style="outline: 0px; color: rgb(46, 125, 198); text-decoration: none; position: absolute;
                                top: 0px; left: -60px;" target="_blank">
                                <img data-fakeid="731902502" src="${Headimgurl}"
                                    style="border: 0px; width: 48px; height: 48px;" /></a>
                </div>
            </div>
            <div class="message_content text" style="margin-left: 80px; margin-right: 365px;
                padding-top: 0px; color: rgb(123, 123, 123); word-wrap: break-word; word-break: break-all;
                padding-bottom: 2px;">
                <div id="wxMsg200015966" class="wxMsg" data-id="200015966">
                     {{if MsgType=="text"}}
                   ${Content}&nbsp;&nbsp;<label style=" color:#9bb668; font-size:10px;">${CreateTime}</label>
                 {{else MsgType=="voice"}}
                  ${Recognition}&nbsp;&nbsp;<label style=" color:#9bb668; font-size:10px;">${CreateTime}</label>
                {{else  MsgType=="image"}}
                   <a href="${PicUrl}"><img   src="${PicUrl}" width="50px" height="50px" /></a> &nbsp;&nbsp;<label style=" color:#9bb668; font-size:10px;">${CreateTime}</label>
                {{else  MsgType=="location"}}
                   ${Content}&nbsp;${Label}&nbsp;<label style=" color:#9bb668; font-size:10px;">${CreateTime}</label>
                   {{else MsgType=="event"}}
                 ${Content}&nbsp;&nbsp;<label style=" color:#9bb668; font-size:10px;">${CreateTime}</label>
                 
                 {{/if}}
                </div>
            </div>
        </li>
        <hr />
    </script>
    <input type="hidden" id="hid_fromusername" value="<%=fromusername %>" />
</asp:Content>
