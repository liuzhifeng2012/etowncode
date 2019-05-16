<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="OrderCount.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.Order.OrderCount" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

         //日历

            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();
            });



            var pageSize = 10; //每页显示条数
            var comid = $("#hid_comid").trimVal();
            var userid = $("#hid_userid").trimVal();



              $("#chaxun").click(function () {
                SearchList();
              })
              


            //装载列表
            function SearchList() {
                var startime = $("#startime").trimVal();
                var endtime = $("#endtime").trimVal();

                var searchtype = $("#searchtype").trimVal();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=getordercount",
                    data: { userid: $("#hid_userid").trimVal(),comid: comid, startime: startime,endtime:endtime, searchtype: searchtype },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询订单列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                //$("#tblist").html("查询数据为空");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");

                            }


                        }
                    }
                })


            }

        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">

        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
                <h3>
                    直销订单统计</h3>
                <div style="text-align: center;">

                        <select name="searchtype" id="searchtype" class="mi-input">
                        <option value="1" selected="selected">选择按日期查询</option>
                        <option value="2">选择按月查询</option>
                        </select>

                     从
                    <input type="text" maxlength="100" size="15" name="startime" id="startime" isdate="yes" class="mi-input">
                     到
                    <input type="text" maxlength="100" size="15" name="endtime" id="endtime" isdate="yes" class="mi-input">
                    <input type="button"   value="查询" method="post" id="chaxun" >
                </div>
                
                <table width="780px" border="0">
                    <tr>
                        <td width="85px" height="30px">
                            <p align="left">
                                日期       
                            </p>
                        </td>
                        <td width="35px">
                            <p align="left">
                                订单数  
                            </p>
                        </td>
                        <td width="35px">
                            <p align="left">
                                购买量 
                            </p>
                        </td>
                        <td width="35px">
                            <p align="left">
                            应收   
                            </p>
                        </td>
                        <td width="35px">
                            <p align="left">
                                优惠
                            </p>
                        </td>
                        <td width="35px">
                            <p align="center">
                                退款
                            </p>
                        </td>
                        <td width="35px">
                            <p align="center">
                               收入
                            </p>
                        </td>
                        <td width="35px">
                            <p align="center">
                                微信
                            </p>
                        </td>
                        <td width="35px">
                            <p align="center">
                                 官网
                            </p>
                        </td>
                        <td width="35px">
                            <p align="center">
                                电商
                            </p>
                        </td>
                        <td width="35px">
                            <p align="center">
                               毛利
                            </p>
                        </td>
                        <td width="35px">
                            <p align="center">
                                已消费 
                            </p>
                        </td>
                        <td width="35px">
                            <p align="center">
                               未消费
                            </p>
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
                    <tr class="fontcolor">
                        <td valign="top">
                            <p align="left">${Datestr}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="left">${Countnum}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                               ${U_num}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                ${Pay_price}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                               ${Integral1}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                              ${BackPrice}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                             ${Pay_price - Integral1 -BackPrice}
                             </p>
                        </td>
                        
                        <td valign="top">
                            <p align="center">
                                 ${WeixinSale}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                            ${WebSale}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                         --
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                            ${Profit}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                         ${UseState}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                            ${UnUseState}
                            </p>
                        </td>
                    </tr>
    </script>
    
    <br>
注1：退款为当期订单后续退订退款的金额，统计表中的退款及收入金额会随后续退款的增加而变化。<br>
注2：毛利按实际订单成本汇总计算，不受随后续产品调价影响。<br>
注3：已消费及未消费会随后续实际验证的不断增加而产生变化。<br>
注4：按月查询则统计选择的当月的数据。<br>
注5：统计按订单处理完成时间进行统计<br>
    <input type="hidden" id="hid_id" value="0" />
    <input type="hidden" id="hid_proid" value="0" />
</asp:Content>
