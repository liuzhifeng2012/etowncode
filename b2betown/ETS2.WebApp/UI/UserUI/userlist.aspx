<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="userlist.aspx.cs"
    Inherits="ETS2.WebApp.UI.UserUI.userlist" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            //获取联系人列表
            SearchList(1, 10, '1');
            //验票明细列表
            $("#Search").click(function () {
                var pageindex = 1;
                var key = $("#key").val();

                if (key == "") {
                    $.prompt("查询关键词不能为空");
                    return;
                }
                SearchList(1, 10, '0,1');
            })


            $("body").keydown(function (e) {
                if (e.keyCode == 13) {
                    $("#Search").click();
                }
            });

        })
        function SearchList(pageindex, pagesize, employstate) {
            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.ajax({
                type: "post",
                url: "/JsonFactory/PermissionHandler.ashx?oper=masterpagelistByComIdAndEmploystate",
                data: { employstate: employstate, userid: $("#hid_userid").trimVal(), pageindex: pageindex, pagesize: pagesize, comid: $("#hid_comid").trimVal(), key: $("#key").val() },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询列表错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalCount == 0) {
                            //                            $("#tblist").html("查询数据为空");
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalCount, pagesize, pageindex, employstate);
                        }


                    }
                }
            })


        }

        //分页
        function setpage(newcount, newpagesize, curpage, employstate) {
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

                    SearchList(page, newpagesize, employstate);

                    return false;
                }
            });
        }
        function viewmaster(employstate) {
            SearchList(1, 10, employstate);
        }

        function isdefaultfk(userid, isdefault) {
            $.ajax({
                type: "post",
                url: "/JsonFactory/PermissionHandler.ashx?oper=isdefaultkf",
                data: { userid: userid, comid: $("#hid_comid").trimVal() },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        if (isdefault == 0) {
                            alert("设定默认客服成功");
                            location.href = "userlist.aspx";
                        } else {
                            alert("已取消默认客功");
                            location.href = "userlist.aspx";
                        }

                    }
                }
            })
        }

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--  <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="userlist.aspx" onfocus="this.blur()" target=""><span>员工管理</span></a></li>
                <li><a href="usermanage.aspx" onfocus="this.blur()" target=""><span>添加员工</span></a></li>
            </ul>
        </div>--%>
        <%--<label style="float: right; padding-right: 20px;">
            <a style="color: Blue;" href="usermanage.aspx">添加员工</a>
        </label>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <div style="text-align: center;">
                    <label>
                        请输入(姓名，手机,所属部门)
                        <input name="key" type="text" id="key" style="width: 160px; height: 20px;">
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;">
                    </label>
                    <label>
                        <a href="#" onclick="viewmaster('0,1')">显示全部</a>&nbsp;&nbsp;<a href="#" onclick="viewmaster('0')">显示离职</a>&nbsp;&nbsp;<a
                            href="#" onclick="viewmaster('1')">显示在职</a>
                    </label>
                </div>
                <br />
                <table width="780" border="0">
                    <tr>
                        <td width="25">
                            <p align="left">
                                人员编号</p>
                        </td>
                        <td width="80">
                            <p align="left">
                                账 号
                            </p>
                        </td>
                        <td width="70">
                            <p align="left">
                                人员姓名
                            </p>
                        </td>
                        <td width="80">
                            <p align="left">
                                所属部门
                            </p>
                        </td>
                        <td width="40">
                            <p align="left">
                                手机
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                所在分组
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                员工状态
                            </p>
                        </td>
                        <td width="100">
                            <p align="left">
                                申请返佣总额
                            </p>
                        </td>
                      
                        <td width="100">
                            <p align="left">
                                管理
                            </p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <td >
                             <p>${MasterId}</p>
                        </td>
                          <td>
                            <p align="left" title="${Accounts}">
                                ${Accounts}
                            </p>
                        </td>
                        <td>
                            <p align="left" title="${MasterName}">
                                ${MasterName}
                            </p>
                        </td>
                       
                        <td>
                            <p align="left" title="${CompanyName}">
                                ${CompanyName}
                            </p>
                        </td>
                      <td width="40">
                            <p align="left">
                                ${Tel}
                            </p>
                        </td>
                         <td >
                            <p align="left">
                                ${GroupName}
                            </p>
                        </td>
                         <td >
                            <p align="left">
                                {{if Employstate==1}}
                                在职
                                {{else}}
                                离职
                                {{/if}}
                                                                (
                                {{if weixinstate ==1}}
                                    {{if Employstate==1}}
                                        {{if Peoplelistview==1}}
                                        在线
                                        {{else}}
                                        未在线[不显示]
                                        {{/if}}
                                    {{else}}
                                        未在线[离职]
                                    {{/if}}
                                {{else}}
                                    未绑定
                                {{/if}}
                                )
                            </p>
                        </td>
                          <td >
                            <p align="left" title="${rebateapplytotal}(已提现${rebatehastixian+rebatenottixian};未提现${restrebate})">
                            ${rebateapplytotal}(已提现${rebatehastixian+rebatenottixian};未提现${restrebate})
                            </p>
                         </td>
                       
                         <td >
                            <p align="left">
                               <a href="usermanage.aspx?staffid=${MasterId}"  class="a_anniu"> 管理 </a> 

                               {{if Isdefaultkf==0}}
                                 <a href="javascript:;" onclick="isdefaultfk(${MasterId},${Isdefaultkf})" class="a_anniu"> 设定默认客服 </a> 
                               {{else}}
                                 <a href="javascript:;" onclick="isdefaultfk(${MasterId},${Isdefaultkf})" class="a_anniu"> 取消默认 </a> 
                               {{/if}}
                            <!-- <span id="span1" style="color:blue;cursor: pointer;"   onclick="referrer_ch1('${MasterId}',150, '${Tel}')">二维码</span>   -->
                              <!--{{if rebatenum>0||rebateapplytotal>0}}
                                  <a href="ChanelrebateApplyDeal.aspx?Channelid=${Channelid}"  class="a_anniu"> 返佣信息 </a>   
                              {{/if}}-->
                            </p>
                        </td>
                    </tr>
    </script>
    <div id="rhshow" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 400px; height: 130px; display: none; left: 20%; position: absolute; top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 18px;" id="span_rh"></span>
                </td>
            </tr>
            <tr>
                <td height="30" align="right" bgcolor="#E7F0FA" class="tdHead">
                    管理组类型:
                </td>
                <td height="30" bgcolor="#E7F0FA" class="tdHead">
                    <label>
                        <input type="checkbox" name="Uptype" value="1" />管理员</label>
                    <label>
                        <input type="checkbox" name="Uptype" value="2" />经理</label>
                    <label>
                        <input type="checkbox" name="Uptype" value="3" />员工</label>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input type="hidden" id="hid_masterid" value="" />
                    <input type="hidden" id="hid_mastername" value="" />
                    <input name="submit_rh" id="submit_rh" type="button" class="formButton" value="  确  定  " />
                    <input name="cancel_rh" id="cancel_rh" type="button" class="formButton" value="  取  消  " />
                </td>
            </tr>
        </table>
    </div>
    <div id="showticket" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: auto; height: auto; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    退 量：<input type="text" id="tnum" value="" style="width: 30px;" />*按实际未使用的数量退票
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
    <div id="proqrcode_rhshow" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; display: none; left: 20%; position: absolute; top: 20%;">
        <input type="hidden" id="hid_manageruserid" value="" />
        <table width="500px" border="0" cellpadding="10" cellspacing="1" style="margin: 10px 5px;">
            <tr>
                <td align="center">
                    <span style="font-size: 14px;">顾问二维码</span>
                </td>
                <td align="center">
                    <span style="font-size: 14px;">顾问页面二维码</span>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <img src="/Images/defaultThumb.png" id="img1" height="150" width="150" />
                </td>
                <td align="center">
                    <img src="/Images/defaultThumb.png" id="img2" height="135" width="135" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <label>
                        *二维码尺寸请按照43像素的整数倍缩放，以保持最佳效果</label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input name="cancel_rh" id="closebtn" type="button" class="formButton" value="  关 闭  " />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">

        //弹出二维码大图
        function referrer_ch1(manageruserid, pixsize, tel) {
            if (tel == "") {
                alert("请先绑定员工电话，然后查看二维码");
                return;
            }
            $("#hid_manageruserid").val(manageruserid);
            referrer_ch2(pixsize, 1);
            $("#proqrcode_rhshow").show();
        };
        //弹出二维码大图
        function referrer_ch2(pixsize, qrcodetype) {

            var manageruserid = $("#hid_manageruserid").trimVal();
            var comid = $("#hid_comid").trimVal();

            $("#img1").attr("src", "/Images/defaultThumb.png")

            //根据员工id得到渠道信息
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetChannelByMasterId", { masterid: manageruserid }, function (data1) {
                data1 = eval("(" + data1 + ")");
                if (data1.type == 1) { }
                if (data1.type == 100) {
                    $.post("/JsonFactory/WeiXinHandler.ashx?oper=editwxqrcode", { productid: 0, onlinestatus: 1, channelid: data1.msg.Id, qrcodeid: 0, comid: $("#hid_comid").trimVal(), qrcodename: "系统生成员工二维码id:" + manageruserid, promoteact: 0, promotechannelcompany: 0 }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            alert(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            $("#img1").attr("src", data.qrcodeurl);
                        }
                    })
                }
            })



            $("#img2").attr("src", "/Images/defaultThumb.png")

            var url = "http://shop" + comid + ".etown.cn/h5/People.aspx?MasterId=" + manageruserid + "&come=list";
            $("#img2").attr("src", "/ui/pmui/eticket/showtcode.aspx?pno=" + url);


        };
        $(function () {
            $("#closebtn").click(function () {
                $("#img1").attr("src", "/Images/defaultThumb.png")
                $("#img2").attr("src", "/Images/defaultThumb.png")
                $("#proqrcode_rhshow").hide();
                $("#hid_userid").val("0");
            })
        })
    </script>
    <style type="text/css">
        .indexnav
        {
            background: none repeat scroll 0% 0% #F0F0F0;
            display: table;
            width: 100%;
        }
        .indexnav li
        {
            text-align: center;
            cursor: pointer;
            font-size: 14px;
            display: table-cell;
            display: inline;
        }
        
        
        .indexnav li span
        {
            background-color: #FFF;
            text-align: center;
        }
    </style>
</asp:Content>
