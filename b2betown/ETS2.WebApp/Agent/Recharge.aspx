<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master" CodeBehind="Recharge.aspx.cs" Inherits="ETS2.WebApp.Agent.Recharge" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var proid = $("#hid_proid").trimVal();
            var comid = $("#hid_comid_temp").trimVal();
            
            $("#confirmButton").click(function () {
                    var payprice = $("#payprice").trimVal();
                    var u_name = $("#u_name").trimVal();
                    var u_phone = $("#u_phone").trimVal();


                    if (payprice == "") {
                        $.prompt("请填写充值金额");
                        return;
                    }
                    if (u_name == "") {
                        $.prompt("请填写姓名");
                        return;
                    }
                    if (u_phone == "") {
                        $.prompt("请填写手机号");
                        return;
                    } else {
                        if (!isMobel(u_phone)) {
                            $.prompt("请正确填写手机号");
                            return;
                        }
                    }
                    //创建订单
                    $.post("/JsonFactory/OrderHandler.ashx?oper=agentRecharge", { agentid: agentid, comid: comid, payprice: payprice, u_name: u_name, u_phone: u_phone}, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            //alert("http://shop.etown.cn/ui/VASUI/pay.aspx?orderid=" + data.msg + "&comid=" + comid);
                            location.href = "http://shop.etown.cn/ui/VASUI/pay.aspx?orderid=" + data.msg + "&comid=" + comid;
                            return;
                        }
                    })
                    function callbackfunc(e, v, m, f) {
                        if (v == true){
                            location.href = "Order.aspx?comid=" + comid;
                            }
                    }

                })

        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <table>
              <tr>
                  <td class="" style="font-size:24px;">
                    <div class="left">
                    <img id="comlogo" src="" class="" height="42"></div>
                    
                    <div class="left comleft">
                    <div ><span> 商户名称：
                    <%=company %> </span>
                    
                    </div>
                     </div>
                          
                  </td>
              </tr>
        </table>
         <div id="secondary-tabs" class="navsetting ">
           <ul class="composetab">
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_sel"><div><a href="javascript:;">账户充值</a></div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="ProjectList.aspx?comid=<%=comid_temp %>">项目列表</a></div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div>
                <a href="Manage_sales.aspx?comid=<%=comid_temp%>">产品列表</a></div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="Order.aspx?comid=<%=comid_temp%>">订单记录</a>
                </div></div>
            </li>
            <%if (Warrant_type == 2)
                  { %>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="EticketCount.aspx?comid=<%=comid_temp %>">验码统计</a>
                </div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="Verification.aspx?comid=<%=comid_temp %>">验码记录</a>
                </div></div>
            </li>
            <% } %>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="Finane.aspx?comid=<%=comid_temp %>">财务列表</a>
                </div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="EticketBack.aspx?comid=<%=comid_temp %>">退订/订单状态</a>
                </div></div>
            </li>

         </ul>
          <div class="toolbg toolbgline toolheight nowrap" style="">
         <div class="right searchtool">
                 <span>&nbsp;</span>   
         </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
         <!--<a class="btn_gray btn_space" hidefocus="" id="quick_del" href="javascript:;" name="del">删除</a>-->
         
         
         </div></div>
        </div>
         <div id="setting-home" class="vis-zone">
            <div class="inner">
             <h3>
                    在线充值</h3>
                <table width="700px" class="grid"  class="O2" >
                    <tr >
                        <td class="tdHead" style=" width:60px;">
                            商户名称 : 
                        </td>
                         <td>
                           <%=company%>
                        
                        </td>
                    </tr>
                        <tr>
                        <td class="tdHead">
                            充值金额 : 
                        </td>
                         <td>
                    <input id="payprice" name="payprice" type="text" value="" class="input-ticket" autocomplete="off"
                        maxlength="8"  size="6">元
                        </td>
                    </tr> 
                </table>
                <h3>
                    联系人信息</h3>
                    <table width="300px" class="grid">
                    <tr>
                        <td class="tdHead">
                            姓    名 : 
                        </td>
                         <td>
                             <input name="Input" class="dataNum dataIcon" id="u_name" value="" />
                        </td>
                    </tr> 
                    <tr>
                        <td class="tdHead">
                            接收手机 : 
                        </td>
                         <td>
                            <input name="u_phone" class="dataNum dataIcon" id="u_phone" value="" />
                        </td>
                    </tr> 
                </table>
                 <table width="300px" class="grid">
                 <tr>
                        <td class="tdHead">
                       <input id="confirmButton" type="button" value="    在此商户充值    " name="confirmButton"></input>
                </td>
                    </tr> 
                </table>
                <div id="divPage">
                </div>
            </div>
        </div>

    </div>
    <div class="data">
    </div>

    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
    <input id="hid_payprice" type="hidden" value="0" />
    </asp:Content>