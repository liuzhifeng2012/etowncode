<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/ui/etown.master" CodeBehind="Manage.aspx.cs"
    Inherits="ETS2.WebApp.Manage" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        $(function () {
            //            if ($("#hid_comid").trimVal() == 101) {
            $("#setting-home").show();

            //获得登录账户信息
            $.post("/JsonFactory/UserHandle.ashx?oper=GetAccountInfo", { id: $("#hid_userid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    //                        $.prompt("获得登录账户信息出错");
                    window.open("/logout.aspx", target = "_self");
                    return;
                }
                if (data.type == 100) {
                    $("#Span19").html(data.msg[0].Accounts);
                    $("#span20").html(data.msg[0].MasterName);
                    $("#span21").html(data.msg[0].CompanyName);
                    $("#span22").html(data.msg[0].CompanyName + "(" + data.msg[0].ChannelCompanyName + ")");
                }
            })
//            //获得发展会员统计
//            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetCrmStatistics", { comid: $("#hid_comid").trimVal(), userid: $("#hid_userid").trimVal() }, function (data) {
//                data = eval("(" + data + ")");
//                if (data.type == 1) {
//                    //                        $.prompt("获得统计信息出错");
//                    return;
//                }
//                if (data.type == 100) {

//                    $("#Span25").html(data.msg[0].allcardopennum);
//                    $("#Span7").html(data.msg[0].Yesterdayallcardopennum);
//                    $("#Span8").html(data.msg[0].monthallcardopennum);
//                    $("#Span26").html(data.msg[0].entitycardopennum);
//                    $("#Span1").html(data.msg[0].Yesterdayentitycardopennum);
//                    $("#Span9").html(data.msg[0].monthentitycardopennum);
//                    $("#Span27").html(data.msg[0].webcardopennum);
//                    $("#Span2").html(data.msg[0].Yesterdaywebcardopennum);
//                    $("#Span10").html(data.msg[0].monthwebcardopennum);

//                    $("#Span28").html(data.msg[0].salewebcardopennum);
//                    $("#Span23").html(data.msg[0].saleyesterdaywebcardopennum);
//                    $("#Span24").html(data.msg[0].salemonthwebcardopennum);

//                    $("#Span29").html(data.msg[0].weixinnum);
//                    $("#Span3").html(data.msg[0].Yesterdayweixinnum);
//                    $("#Span11").html(data.msg[0].monthweixinnum);
//                    $("#Span12").html(data.msg[0].weixinphonenum);

//                    $("#Span30").html(data.msg[0].crmconsultnum);
//                    $("#Span13").html(data.msg[0].Yesterdaycrmconsultnum);
//                    $("#Span14").html(data.msg[0].monthcrmconsultnum);

//                    $("#Span31").html(data.msg[0].crmordernum);
//                    $("#Span4").html(data.msg[0].Yesterdaycrmordernum);
//                    $("#Span15").html(data.msg[0].monthcrmordernum);

//                    $("#Span32").html(data.msg[0].cardvalidatenum);
//                    $("#Span5").html(data.msg[0].Yesterdaycardvalidatenum);
//                    $("#Span16").html(data.msg[0].monthcardvalidatenum);

//                    $("#Span33").html(data.msg[0].carddealMoney);
//                    $("#Span6").html(data.msg[0].YesterdaycarddealMoney);
//                    $("#Span17").html(data.msg[0].monthcarddealMoney);

//                    $("#Span18").html(data.msg[0].cardavgdealmoney);
//                    $("#Span20").html(data.msg[0].cardeveryoneavgdealmoney);

//                }
//            })
            //            }
        })
    </script>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="body">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <%-- <li class="on"><a href="javascript:void(0)" onFocus="this.blur()" target="right-main">
                     </a></li>--%>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone" style="display: none">
            <div class="inner">
            <div id="yui_3_1_0_1_140601287152821" class="main homepage_wrapper yui3-widget yui3-app yui3-app_view">
                <div class="hello">

                    <span class="hello_left">
            	            		         尊敬的<span id="Span19">weilvxing</span>，您好！
                                    </span>	
            
                          
	                     <p>
	            	
	                    </p>
                </div>
                <table width="71%" class="grid">
                    <tr>
                        <td class="tdHead">
                            您所在商户：
                        </td>
                        <td>
                            <span id="span21">微旅行 无微不至 </span>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            服务部门名称：
                        </td>
                        <td>
                            <span id="span22">微旅行 无微不至(全部渠道)</span>
                        </td>
                    </tr>
                </table>
                <h3>
                    发展会员统计
                </h3>
                <table width="71%" class="grid" style=" display:none;">
                    <tr>
                        <td class="tdHead" width="25%">
                            开卡总量：<span id="Span25"></span>
                        </td>
                        <td width="30%">
                            昨日：<span id="Span7"></span>
                        </td>
                        <td>
                            本月：<span id="Span8"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            实体卡会员开卡：<span id="Span26"></span>
                        </td>
                        <td>
                            昨日：<span id="Span1"></span>
                        </td>
                        <td>
                            本月：<span id="Span9"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            网站直接注册：<span id="Span27"></span><input type="hidden" value="(卡号以2002开头，并且介绍人为空)" />
                        </td>
                        <td>
                            昨日：<span id="Span2"></span>
                        </td>
                        <td>
                            本月：<span id="Span10"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            销售帮助注册：<span id="Span28"></span><input type="hidden" value="(卡号以2002开头，并且介绍人不为空)" />
                        </td>
                        <td>
                            昨日：<span id="Span23"></span>
                        </td>
                        <td>
                            本月：<span id="Span24"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            微信关注：<span id="Span29"></span><input type="hidden" value="(卡号以2001开头)" />
                        </td>
                        <td>
                            昨日：<span id="Span3"></span>
                        </td>
                        <td>
                            本月：<span id="Span11"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            微信验证手机： <span id="Span12"></span>
                            <input type="hidden" value="(微信手机都不为空)" />
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                </table>
                <h3>
                    成交统计
                </h3>
                <table width="71%" class="grid"  style=" display:none;">
                    <tr>
                        <td class="tdHead" width="25%">
                            会员活跃数量：<span id="Span30"></span>
                        </td>
                        <td width="30%">
                            昨日：<span id="Span13"></span>
                        </td>
                        <td>
                            本月：<span id="Span14"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            会员订单数量：<span id="Span31"></span>
                        </td>
                        <td>
                            昨日：<span id="Span4"></span>
                        </td>
                        <td>
                            本月：<span id="Span15"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            会员卡验证笔数：<span id="Span32"></span>
                        </td>
                        <td>
                            昨日：<span id="Span5"></span>
                        </td>
                        <td>
                            本月：<span id="Span16"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            会员卡成交总金额：<span id="Span33"></span>
                        </td>
                        <td>
                            昨日：<span id="Span6"></span>
                        </td>
                        <td>
                            本月：<span id="Span17"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            会员卡平均单笔成交金额：
                        </td>
                        <td colspan="2">
                            <span id="Span18"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            会员卡人均成交金额：
                        </td>
                        <td colspan="2">
                            <span id="Span20"></span>
                        </td>
                    </tr>
                </table>
                <p>
                    &nbsp;
                </p>
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <%--    <asp:Label ID="lable1" runat="server"></asp:Label>--%>
</asp:Content>
