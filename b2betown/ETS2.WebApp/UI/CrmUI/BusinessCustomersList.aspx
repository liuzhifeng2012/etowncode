<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="BusinessCustomersList.aspx.cs"
    Inherits="ETS2.WebApp.UI.CrmUI.BusinessCustomersList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();

            var pageindex = $("#hid_pageindex").trimVal();

            SearchList(pageindex, userid, comid, "", "0,1", "0,1", "0,1");

            //导出所有会员信息
            $("#downcrm").click(function () {
                location.href = "/ui/crmui/downcrm.aspx?comid=" + comid + "&md5info=<%=md5info %>";
            })

            //验票明细列表
            $("#Search").click(function () {
                var pageindex = 1;
                var key = $("#key").val();

                if (key == "") {
                    $.prompt("查询关键词不能为空");
                    return;
                }
                SearchList(1, userid, comid, key, "0,1", "0,1", "0,1");
            })

            $("body").keydown(function (e) {
                if (e.keyCode == 13) {
                    $("#Search").click();
                }
            });


            //            $(".touxiang").hover(
            //              function (e) {
            //                                  $('.rich_buddy').show();
            //                                  var v_id = $(e.target).attr('id');
            //                                  var top = $("#" + v_id).offset().top - 80;
            //                                  $("#" + v_id).css("border", "2px solid #A7C5E2");
            //                                  $('.rich_buddy').css("top", top + "px");

            //                                  $("#buddy_name").html($("#hid_name" + v_id).val());
            //                                  $("#buddy_city").html($("#hid_city" + v_id).val());
            //                                  $("#buddy_sex").html($("#hid_sex" + v_id).val());
            //                                  $("#buddy_Wxname").html($("#hid_Wxname" + v_id).val());
            //                                  $("#buddy_email").html($("#hid_email" + v_id).val());
            //                                  $("#buddy_openid").html($("#hid_openid" + v_id).val());
            //              },
            //              function (e) {
            //                  var v_id = $(e.target).attr('id');
            //                                  $('.rich_buddy').delay(5000).hide(0); ;
            //                                  $("#" + v_id).css("border", "0");
            //              }
            //            );

            if ($("#hid_isactivate").trimVal() == 1 && $("#hid_IsParentCompanyUser").trimVal().toLowerCase() == 'false') {
                $("#secondary-tabs").find("li").first().show().nextAll().hide();
            }



        })

        //列表
        function SearchList(pageindex, userid, comid, key, iswxfocus, ishasweixin, ishasphone) {
            $("#hid_pageindex").val(pageindex);

            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.ajax({
                type: "post",
                url: "/JsonFactory/BusinessCustomersHandler.ashx?oper=searchpagelist2",
                data: { ishasphone: ishasphone, iswxfocus: iswxfocus, ishasweixin: ishasweixin, isactivate: $("#hid_isactivate").trimVal(), userid: userid, comid: comid, pageindex: pageindex, pagesize: pageSize, key: key },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("<tr><td colspan='11'>查询会员信息出错</td></tr>");
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalCount == 0) {
                            //                                $("#tblist").html("<tr><td colspan='11'>没有查到会员信息。</td></tr>");
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalCount, pageSize, pageindex, userid, comid, key, iswxfocus, ishasweixin);
                            iscanupimprest();
                        }
                    }
                }
            })
        }


        //分页
        function setpage(newcount, newpagesize, curpage, userid, comid, key, iswxfocus, ishasweixin) {
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

                    SearchList(page, userid, comid, key, iswxfocus, ishasweixin);

                    return false;
                }
            });
        }





        function viewcrmlist(iswxfocus, ishasweixin, ishasphone) {

            SearchList(1, $("#hid_userid").val(), $("#hid_comid").val(), "", iswxfocus, ishasweixin, ishasphone);
        }

        function setxingqu(crmid) {

            window.open("member_interest_manage.aspx?crmid=" + crmid + "&crmpageindex=" + $("#hid_pageindex").val(), target = "_self");
        }


        //----此部分是分组部分BEGIN---//
        function changefenzu() {
            var crmid = $("#hid_edit_crmid").val();
            var fenzuid = $("#sel_fenzu").val();

            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=Changefenzu", { crmid: crmid, groupid: fenzuid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("调整失败");
                } else {
                    alert("调整成功");
                    $("#Span_fenzu").text("");

                    $("#div_showfenzu").hide();
                }
            })
        }

        function fenzucancel() {

            $("#Span_fenzu").text("");
            $("#div_showfenzu").hide();
        }
        function fenzu_set(id, name, groupid) {
            $("#Span_fenzu").html(name);
            $("#hid_edit_crmid").val(id);
            $("#div_showfenzu").show();
            //获得公司详情
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetCompanyB2bgroup", { comid: $("#hid_comid").val() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("获得公司分组失败");
                    return;
                }
                if (data.type == 100) {
                    $("#sel_fenzu").empty();
                    if (data.total == 0) {
                        $("#sel_fenzu").append('<option value="0">默认组</option>');
                    } else {
                        for (var i = 0; i < data.total; i++) {
                            if (data.msg[i].Id == groupid) {
                                $("#sel_fenzu").append('<option value="' + data.msg[i].Id + '" checked="checked">' + data.msg[i].Groupname + '</option>');
                            } else {
                                $("#sel_fenzu").append('<option value="' + data.msg[i].Id + '">' + data.msg[i].Groupname + '</option>');
                            }
                        }
                    }
                }
            })
        }

        function touxiang_show(id) {
            $('.rich_buddy').show();
            var v_id = id;
            var top = $("#" + v_id).offset().top - 80;
            $("#" + v_id).css("border", "2px solid #A7C5E2");
            $('.rich_buddy').css("top", top + "px");

            $("#buddy_name").html($("#hid_name" + v_id).val());
            $("#buddy_city").html($("#hid_city" + v_id).val());
            $("#buddy_sex").html($("#hid_sex" + v_id).val());
            $("#buddy_Wxname").html($("#hid_Wxname" + v_id).val());
            $("#buddy_email").html($("#hid_email" + v_id).val());
            $("#buddy_openid").html($("#hid_openid" + v_id).val());
        }

        function touxiang_hid(id) {
            var v_id = id;
            $('.rich_buddy').delay(5000).hide(0); ;
            $("#" + id).css("border", "0");
        }

        //----此部分是分组部分END---//
        function iscanupimprest() {
            //判断是否可以设置预付款
            $.post("/JsonFactory/PermissionHandler.ashx?oper=getGroupByUserId", { userid: $("#hid_userid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {

                    if (data.msg.iscanset_imprest == 0) {
                        $(".a_anniu").hide();
                    }
                    if (data.msg.iscanset_imprest == 1) {

                    }
                }
            })
        }
    </script>
    <style>
        .popover.arrow_left
        {
            margin-left: 8px;
            margin-top: 0;
        }
        .rich_buddy
        {
            z-index: 1;
            width: 240px;
            padding-top: 0;
        }
        .rich_buddy .popover_inner
        {
            padding: 25px 25px 35px;
        }
        .popover .popover_inner
        {
            border: 1px solid #D9DADC;
            word-wrap: break-word;
            word-break: break-all;
            padding: 30px 25px;
            background-color: white;
            box-shadow: none;
            -moz-box-shadow: none;
            -webkit-box-shadow: none;
            height: 180px;
        }
        .rich_buddy .popover_content
        {
            width: auto;
        }
        .rich_buddy_hd
        {
            padding-bottom: 10px;
        }
        v .vm_box
        {
            display: inline-block;
            height: 100%;
            vertical-align: middle;
        }
        .rich_buddy .frm_control_group
        {
            padding-bottom: 0;
            float: left;
            width: 160px;
            padding: 5px 0;
        }
        .rich_buddy .frm_label
        {
            width: 150px;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            word-wrap: normal;
        }
        .frm_label
        {
            float: left;
            margin-right: 1em;
            font-size: 14px;
        }
        .frm_controls
        {
            display: table-cell;
            vertical-align: top;
            float: none;
            width: auto;
        }
        .frm_vertical_pt
        {
            padding-top: .3em;
        }
        .remark_name
        {
            margin-top: 5px;
            padding-top: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>    
                <%if (isactivate == "1")
                  {
                %>
                <li class="on"><a href="/ui/crmui/BusinessCustomersList.aspx" onfocus="this.blur()">
                    会员列表</a></li>

                <%
                    if (IsParentCompanyUser == true)
                    {
                %>
                   <li><a href="/ui/crmui/B2bCrm_LevelManage.aspx" onfocus="this.blur()">
                    会员级别设置</a></li>
                <li><a href="/ui/vasui/IntegralList.aspx" target="" title="">积分详情</a></li>
                <li><a href="/ui/vasui/ImprestList.aspx" target="" title="">会员预付款详情</a></li>
                <li><a href="/ui/crmui/Member_group_list.aspx" onfocus="this.blur()">分组管理</a></li>
                <%
                    }

                  }
                  else
                  {%>
                <li class="on"><a href="/ui/crmui/BusinessCustomersList.aspx?isactivate=0" onfocus="this.blur()">
                    未激活用户列表</a></li>
                <li><a href="/excel/importexcel.aspx" onfocus="this.blur()">导入历史会员信息</a></li>
                <li><a href="/excel/ObtainGZList.aspx" onfocus="this.blur()">导入已有微信粉丝</a></li>
                <% }%>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
                <h3>
                    订单管理</h3>
                <div style="text-align: center;">
                    <label>
                        请输入(手机，姓名)
                        <input name="key" type="text" id="key" style="width: 160px; height: 20px;">
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="  查  询  " style="width: 120px;
                            height: 26px;">
                    </label>
                    <label>
                        <a href="javascript:void(0)" style="color: Blue; text-decoration: underline;" onclick="viewcrmlist('0,1','0,1','0,1')">
                            全部会员列表</a>&nbsp;<a href="javascript:void(0)" style="color: Blue; text-decoration: underline;"
                                onclick="viewcrmlist('0','1','0,1')">已取消微信关注会员</a>&nbsp;<a href="javascript:void(0)"
                                    style="color: Blue; text-decoration: underline;" onclick="viewcrmlist('0','0','0,1')">尚未关注微信会员</a>
                    </label>
                    <%-- <label>
                       <a href="/weixin/invitecode/send.aspx?qunfa=yes" style="color: Blue; text-decoration: underline;">
                            群发邀请码</a></label>--%>
                    <label>
                        <a href="javascript:void(0)" onclick="viewcrmlist('0','0','1')" style="color: Blue;
                            text-decoration: underline;">需要发送邀请码的会员</a></label>
                </div>
                <div style="text-align: right;">
                    <label>
                        <a href="#" class="a_anniu" style="font-size: 14px; font-weight: bold; color: #333933" id="downcrm">导出全部会员信息</a>
                    </label>
                </div>
                <table width="780" border="0">
                    <tr>
                        <td width="5%">
                            微信头像
                        </td>
                        <td width="10%">
                            卡号
                        </td>
                        <td width="6%">
                            用户姓名
                        </td>
                        <td width="10%">
                            手机
                        </td>
                        <td width="4%">
                            微信情况
                            <td width="3%">
                                预付款
                            </td>
                            <td width="3%">
                                积分
                            </td>
                            <td width="5%">
                                注册时间&nbsp;
                            </td>
                            <td width="9%">
                                渠道单位
                            </td>
                            <td width="4%">
                                推荐人
                            </td>
                            <td width="4%">
                                分组
                            </td>
                            <td width="9%">
                                管理
                            </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
                <p>
                    &nbsp;
                </p>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr style="height: 60px;">
                    <td >
                            <p align="left">
                            <img src="${WxHeadimgurl}" id="img${id}" class="touxiang" width="50px" height="50px;"  title='${weixin}' " onmousemove="touxiang_show('img${id}')" onmouseout="touxiang_hid('img${id}')"> </p>
                            <input type="hidden" id="hid_nameimg${id}" value=" ${customername} " />
                            <input type="hidden" id="hid_cityimg${id}" value=" ${WxProvince}-${WxCity} " />
                            <input type="hidden" id="hid_seximg${id}" value="  ${WxSex} " />
                            <input type="hidden" id="hid_Wxnameimg${id}" value=" ${WxNickname} " />
                            <input type="hidden" id="hid_emailimg${id}" value=" ${email}  " />
                            <input type="hidden" id="hid_openidimg${id}" value=" ${weixin} " />
                        </td>
                        
                        <td>
                            ${idcard} 
                        </td>
                        <td>
                                {{if customername!=""}}${customername}{{else}} ${WxNickname} {{/if}}
                        </td>
                        <td>
                            ${phone} {{if phone!=""}}<a href="Sms.aspx?Phone=${phone}&Name=${customername}">短信</a>{{/if}}
                        </td>
                          <td>
                            ${isfocuswinxin} 
                        </td>

                        <td >
                                ${imprest}
                        </td>
                        <td >
                               ${integral}
                        </td>
                        <td >

                                ${ChangeDateFormat(registerdate)}
                            
                        </td>
                        <td>${channel}</td>
                        <td>${referrer}</td>
                        <td>
                            <a href="javascript:void(0)" onclick="fenzu_set('${id}','${customername}','${GroupId}')">${GroupName}</a>
                        </td>
                        <td>
                                
                               
                                {{if idcard>0}}
                                   <a href="member_manage.aspx?id=${id}" class="a_anniu">管理</a>
                                {{/if}}
                                  <!--<a href="javascript:void(0)" onclick="setxingqu('${id}')"  class="a_anniu">兴趣</a>-->
                                {{if phone!=""&weixin==""}}
                                  <a href="/weixin/invitecode/send.aspx?qunfa=no&phone=${phone}"  class="a_anniu">邀请码</a>
                                {{/if}}
                                 
                                {{if  weixin!=""&IsCanReplyWx==1&whetherwxfocus==true}}
                                  <a href="/weixin/WxSingleSendMsgPage.aspx?fromusername=${weixin}&comid=${comid}"  class="a_anniu">微信客服</a>
                                {{/if}}
                        </td>
                    </tr>
    </script>
    <div id="showticket" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: auto; height: auto; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="span_ticket">
                    </span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    产 品：<span id="pro_name"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    数 量：<span id="pro_num"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    价 格：<span id="span_price"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    退 量：<input type="text" id="tnum" value="" style="width: 30px;" />*按实际未使用的数量退票
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    备 注：<textarea id="testpro" cols="35" rows="2" style="margin-right: 10px;"></textarea>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input id="Enter" name="Enter" type="button" class="formButton" value="  提  交  " />
                    <input name="cancel_rh" type="button" class="formButton" onclick="cancel();" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <div id="div_showfenzu" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 350px; height: 200px; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#E7F0FA"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="span1">会员分组设置
                    </span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    会员名称：<span id="Span_fenzu"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    分组：
                    <select id="sel_fenzu">
                    </select>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#E7F0FA" class="tdHead">
                    <input id="Button1" type="button" class="formButton" onclick="changefenzu()" value="  提  交  " />
                    <input name="cancel_rh" type="button" class="formButton" onclick="fenzucancel();"
                        value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <div class="rich_buddy popover arrow_left" style="left: 70px; top: 537px; position: absolute;
        opacity: 1; display: none;" onmousemove="$('.rich_buddy').show();" onmouseout="  $('.rich_buddy').hide();)">
        <div class="popover_inner">
            <div class="popover_content">
                <div class="rich_buddy_hd">
                    详细资料</div>
                <div class="rich_buddy_bd buddyRichContent" style="display: block;">
                    <div class="frm_control_group nickName">
                        <label class="frm_label">
                            地区:<span id="buddy_city"></span></label>
                    </div>
                    <div class="frm_control_group nickName">
                        <label class="frm_label">
                            微信昵称:<span id="buddy_Wxname"></span></label>
                    </div>
                    <div class="frm_control_group nickName">
                        <label class="">
                            微信ID:<span id="buddy_openid"></span></label>
                    </div>
                    <div class="frm_control_group nickName">
                        <label class="frm_label">
                            姓名:<span id="buddy_name"></span></label>
                    </div>
                    <div class="frm_control_group nickName">
                        <label class="frm_label">
                            性别:<span id="buddy_sex"></span></label>
                    </div>
                    <div class="frm_control_group nickName">
                        <label class="frm_label">
                            邮箱:<span id="buddy_email"></span></label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" id="hid_edit_crmid" value="0" />
    <input type="hidden" id="hid_isactivate" value="<%=isactivate %>" />
    <input type="hidden" id="hid_pageindex" value="<%=pageindex %>" />
    <input type="hidden" id="hid_IsParentCompanyUser" value="<%=IsParentCompanyUser %>" />
</asp:Content>
