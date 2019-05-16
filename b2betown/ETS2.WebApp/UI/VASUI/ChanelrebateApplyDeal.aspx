<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChanelrebateApplyDeal.aspx.cs"
    Inherits="ETS2.WebApp.UI.VASUI.ChanelrebateApplyDeal" MasterPageFile="/UI/Etown.Master" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 15; //每页显示条数

        $(function () {
//            $.post("/JsonFactory/ChannelHandler.ashx?oper=getchannellistbycomid", {comid:$("#hid_comid").trimVal()}, function (data) {
//                data = eval("(" + data + ")");
//                if (data.type == 1) { }
//                if(data.type==100){
//                   
//                }
//            }) 



            SearchList(1, "0,1");

            //装载返佣申请提现列表
            function SearchList(pageindex, operstatus) {
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/PermissionHandler.ashx?oper=channelrebateapplyalllist",
                    data: { comid: $("#hid_comid").trimVal(), pageindex: pageindex, pagesize: pageSize, operstatus: operstatus },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {

                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalcount == 0) {
                                //                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#FinanceItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalcount, pageSize, pageindex, operstatus);
                            }


                        }
                    }
                })


            }

            //分页
            function setpage(newcount, newpagesize, curpage, operstatus) {
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

                        SearchList(page, operstatus);

                        return false;
                    }
                });
            }

            //确定完成打款
            $("#submit_conf").click(function () {
                var imgurl = $("#<%=headPortrait.FileUploadId_ClientId %>").val();
                if (imgurl == "") {
                    imgurl = $("#hid_imgurl").trimVal();
                }
                $.post("/JsonFactory/PermissionHandler.ashx?oper=confirmcompletedakuan", { id: $("#hid_applyid").trimVal(), operstatus: "1", opertor: $("#hid_userid").trimVal(), operremark: $("#remarks").trimVal(), zhuanzhangsucimg: imgurl }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert("处理失败");
                        return;
                    }
                    if (data.type == 100) {
                        alert("处理成功");
                        location.reload();
                        return;
                    }
                })
            })
            //关闭
            $("#cancel_conf").click(function () {
                $("#hid_applyid").val("");

                $("#f0").text("提现处理");
                $("#f1").text("");
                $("#f2").text("");
                $("#f3").text("");

                $("#f4").text("");
                $("#remarks").val("");
                $("#hid_logo").val("0");
                $("#headPortraitImg").attr("src", "");

                $("#manageWithdraw").hide();
            })
        })

        function referrer_Withdraw(id, applytime, applytype, applydetail, applymoney, truename, alipayaccount, zhuanzhangsucimg, zhuanzhangsucimgurl, operremark) {
            $("#hid_applyid").val(id);

            $("#f0").text("提现处理");
            $("#f1").text(applytime);
            $("#f2").text(applytype + "-" + applydetail);
            $("#f3").text(applymoney);

            $("#f4").text("真实姓名:" + truename + ";支付宝账户:" + alipayaccount);
            $("#remarks").val(operremark);
            $("#hid_imgurl").val(zhuanzhangsucimg);
            $("#headPortraitImg").attr("src", zhuanzhangsucimgurl);

            $("#manageWithdraw").show();
        };
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <table width="780" border="0">
                    <tr>
                        <td height="32">
                            <a href="javascript:void(0)">顾问返佣申请记录</a> &nbsp;&nbsp; 
                          <%--  <a href="javascript:void(0)">
                                顾问返佣记录</a>--%>
                        </td>
                    </tr>
                </table>
                <table width="780" border="0">
                    <tr>
                        <td width="100">
                            <p align="left">
                                申请时间
                            </p>
                        </td>
                        <td width="150">
                            <p align="left">
                                申请人支付宝
                            </p>
                        </td>
                        <td width="100">
                            <p align="left">
                                申请详情
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                申请提现金额
                            </p>
                        </td>
                        <td width="76">
                            <p align="left">
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
    <script type="text/x-jquery-tmpl" id="FinanceItemEdit">   
                    <tr>
                         
                        <td>
                            <p>
                                ${jsonDateFormat(applytime)}
                            </p>
                        </td>
                        
                         <td>
                            <p title="真实姓名:${channelinfo.truename};支付宝账号:${channelinfo.alipayaccount}">
                                 真实姓名:${channelinfo.truename};支付宝账号:${channelinfo.alipayaccount}
                            </p>
                        </td>
                        <td > 
                            <p  title="${applytype}(${applydetail})">
                                 ${applytype}(${applydetail})</p>
                        </td>
                         <td >
                            <p  >
                                ${applymoney}</p>
                        </td>
                         
                         <td>
                            <p  >
                                 {{if operstatus==0}}
                                    申请中
                                      <a class="b_anniu" href="javascript:void(0)" onclick="referrer_Withdraw('${id}','${ChangeDateFormat(applytime)}','${applytype}','${applydetail}','${applymoney}','${channelinfo.truename}','${channelinfo.alipayaccount}','${zhuanzhangsucimg}','${zhuanzhangsucimgurl}','${operremark}')">处理</a>
                                 {{/if}}
                                 {{if operstatus==1}}
                                   已提现
                                     <a class="b_anniu" href="javascript:void(0)" onclick="referrer_Withdraw('${id}','${ChangeDateFormat(applytime)}','${applytype}','${applydetail}','${applymoney}','${channelinfo.truename}','${channelinfo.alipayaccount}','${zhuanzhangsucimg}','${zhuanzhangsucimgurl}','${operremark}')">重新处理</a>
                                     <a href="${zhuanzhangsucimgurl}" target="_blank">截图</a>
                                 {{/if}}

                                 </p>
                        </td>
                         
                    </tr>
    </script>
    <div id="manageWithdraw" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 600px; height: 500px; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 18px;" id="f0"></span>
                </td>
            </tr>
            <tr>
                <td width="21%" height="22" align="right" bgcolor="#E7F0FA" class="tdHead">
                    申请时间:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="">
                    <span style="padding-left: 5px;" id="f1"></span>
                </td>
            </tr>
            <tr>
                <td height="22" align="right" bgcolor="#E7F0FA" class="tdHead">
                    操作类型:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="td1">
                    <span style="padding-left: 5px;" id="f2"></span>
                </td>
            </tr>
            <tr>
                <td height="22" align="right" bgcolor="#E7F0FA" class="tdHead">
                    金额:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="td2">
                    <span style="padding-left: 5px;" id="f3"></span>
                </td>
            </tr>
            <tr>
                <td height="22" align="right" bgcolor="#E7F0FA" class="tdHead">
                    申请人支付宝:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="td5">
                    <span style="padding-left: 5px;" id="f4"></span>
                </td>
            </tr>
            <tr>
                <td height="35" align="right" bgcolor="#E7F0FA" class="tdHead">
                    打款成功:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="td4">
                    <textarea name="remarks" cols="35" rows="3" id="remarks"></textarea>
                    <br>
                    打款成功后详细说明，此内容商家可看到
                </td>
            </tr>
            <tr bgcolor="#E7F0FA" class="tdHead">
                <td class="tdHead" align="right">
                    提现截图：
                </td>
                <td>
                    <div class="C_head">
                        <dl>
                            <dt>
                                <input type="hidden" id="hid_imgurl" value="" />
                                <img alt="" class="headPortraitImgSrc" id="headPortraitImg" src="" width="100" height="60" /></dt>
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
                                            <uc1:uploadFile ID="headPortrait" runat="server" />
                                        </span>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td height="20" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input type="hidden" id="hid_applyid" value="" />
                    <input name="submit_luru" id="submit_conf" type="button" class="formButton" value="  确定完成打款  " />
                    <input name="cancel_luru" id="cancel_conf" type="button" class="formButton" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
