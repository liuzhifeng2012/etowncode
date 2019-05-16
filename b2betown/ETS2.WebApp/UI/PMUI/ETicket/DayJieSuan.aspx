<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="DayJieSuan.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.ETicket.DayJieSuan" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            //查询最后一次结算后到今天的结算
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1);

            //装载产品列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/EticketHandler.ashx?oper=dayjslist",
                    data: { comid: comid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询每日结算错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();

                            if (data.totalCount == 0) {
                                $("#tblist").html("查询数据为空");

                            } else {
                                $("#ProductItemEdit").tmpl(data.msg.dayjs).appendTo("#tblist");
                                $("#hid_jsid").val(data.msg.dayjs[0].jsid);



                                //每日结算统计数据
                                $("#jsmsg").show();
                                $.post("/JsonFactory/EticketHandler.ashx?oper=dayjsresult", { comid: comid, jsid: data.msg.dayjs[0].jsid }, function (data) {
                                    data = eval("(" + data + ")");
                                    if (data.type == 1) {
                                        $.prompt("查询验票统计错误");
                                        return;
                                    }
                                    if (data.type == 100) {
                                        $("#renyuan").text(data.msg.dayjsresult[0].Accounts);
                                        $("#jstime").text(data.msg.dayjsresult[0].jstime);
                                        $("#jstotalnum").text(data.msg.dayjsresult[0].TotalConsumedNum);
                                        $("#jsdate").text(data.msg.dayjsresult[0].jsstartdate + "--" + data.msg.dayjsresult[0].jsenddate);
                                    }
                                })
                            }



                        }
                    }
                })
            }



            $(".productmx").each(function (i, item) {
                var proid = $(item).attr("data");
                $(item).click(function () {
                    location.href = "ETicketList.aspx?proid=" + proid + "&jsid=" + $("hid_jsid").trimVal();

                })
            })
            $(".tongjimx").click(function () {
                location.href = "ETicketList.aspx?eclass=-1" + "&jsid=" + $("hid_jsid").trimVal();

            })
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="eticketindex.aspx" onfocus="this.blur()" target=""><span>电脑验码</span></a></li> 
                <li><a href="eticketlist.aspx" onfocus="this.blur()" target=""><span>验码明细</span></a></li> 
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    上次结算时间：2013-5-31 &nbsp; 下载数据
                </h3>
                <table width="780" border="0">
                    <tr>
                        
                        <td width="188">
                            <p align="left">
                                产品
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                门市价</p>
                        </td>
                        <td width="74">
                            <p align="left">
                                电子码
                            </p>
                        </td>
                        <td width="62">
                            <p align="left">
                                使用张数
                            </p>
                        </td>
                        <td width="55">
                            <p align="left">
                                直销
                            </p>
                        </td>
                        <td width="49">
                            <p align="left">
                                非直销
                            </p>
                        </td>
                        <td width="130">
                            <p align="left">
                                &nbsp;验票日期
                            </p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                    <tr id="jsmsg" style="display:none;">
                        <td >
                            <p align="left">
                                <br />
                                结算操作人：<label id="renyuan"></label>
                                <br />
                                操作时间：<label id="jstime"></label></p>
                        </td>
                        <td width="50">
                            <p align="left">
                                &nbsp;</p>
                        </td>
                        <td width="74">
                            <p align="left">
                                <u class="tongjimx"  >查看明细 </u>
                            </p>
                        </td>
                        <td width="62">
                            <p align="left">
                                <label id="jstotalnum"></label></p>
                        </td>
                        <td width="55">
                            &nbsp;
                        </td>
                        <td width="49">
                            &nbsp;
                        </td>
                        <td width="130">
                            <p align="left" id="jsdate">
                                </p>
                        </td>
                    </tr>
                </table>
                <p>
                </p>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_jsid"  value=""/>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                     <tr>
                         
                        <td width="188">
                            <p align="left">
                                ${proname}
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                ${fmoney(e_face_price,2)}</p>
                        </td>
                        <td width="74">
                            <p align="left">
                                <u class="productmx" data="${pro_id}">查看明细 </u>
                            </p>
                        </td>
                        <td width="62">
                            <p align="left">
                                &nbsp;${TotalConsumedNum}</p>
                        </td>
                        <td width="55">
                            <p align="left">
                                ${DirectsaleConsumedNum}</p>
                        </td>
                        <td width="49">
                            <p align="left">
                               ${DistributesaleConsumedNum}</p>
                        </td>
                        <td width="130">
                            <p align="left">
                                &nbsp;${jsstartdate}--${jsenddate}</p>
                        </td>
                    </tr>
    </script>
</asp:Content>
