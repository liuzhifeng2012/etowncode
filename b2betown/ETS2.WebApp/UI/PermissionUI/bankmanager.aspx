<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Etown.Master" AutoEventWireup="true" CodeBehind="bankmanager.aspx.cs" Inherits="ETS2.WebApp.UI.bankmanager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        var pageSize = 15; //每页显示条数
        SearchList(1);

        //活动加载明细列表
        function SearchList(pageindex) {
            var comid = $("#hid_comid").trimVal();
            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.ajax({
                type: "post",
                url: "/JsonFactory/ProductHandler.ashx?oper=Comlist",
                data: { comid: comid, pageindex: pageindex, pagesize: pageSize, comstate: 1 },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询数据出现错误！");
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalCount == 0) {
                            $("#tblist").html("没有查到预订信息。");
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

        //提交
        $("#submit_conf").click(function () {
            var comid = $("#comid").val();
            var type = $('input:radio[name="Uptype"]:checked').val();

            $.post("/JsonFactory/FinanceHandler.ashx?oper=Upbanktype", { type: type, comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $("#manageWithdraw").hide();

                    $.prompt(data.msg);
                    return;
                }
                if (data.type == 100) {

                    $("#manageWithdraw").hide();
                    $.prompt("操作成功");
                }
            })
            function callbackfunc(e, v, m, f) {
                if (v == true)
                    window.location.reload();
            }

        })

        //关闭窗口
        $("#cancel_conf").click(function () {
            $("#manageWithdraw").hide();
        })
    })
        function upbank(id, comname) {
            $("#title").html(comname);
            $("#comid").val(id);
            $("#manageWithdraw").show();
            $.post("/JsonFactory/FinanceHandler.ashx?oper=getpaytype", { comid: id }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取信息出错，" + data.msg);
                    $("#manageWithdraw").hide();
                }
                if (data.type == 100) {
                    if (data.msg == null) {
                        $.prompt("您尚未设定收款方，请设置并完善收款银行信息");
                        $("#manageWithdraw").hide();
                        return;
                    } else {
                        $('input[name="Uptype"][value=' + data.msg.Uptype + ']').attr("checked", true);
                        return;
                    }
                }
            })
        }
    
       </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="MasterList.aspx" onfocus="this.blur()" target=""><span>人员管理</span></a></li>
                <li ><a href="SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>
                <li><a href="Withdraw_handle.aspx" onfocus="this.blur()"><span>提现财务管理</span></a></li>
                <li><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                <li><a href="ticketlist.aspx" onfocus="this.blur()"><span>退票管理</span></a></li>
                <!--<li><a href="bankmanager.aspx" onfocus="this.blur()"><span>绑定银行管理</span></a></li>-->
                <li ><a href="Modellist.aspx" onfocus="this.blur()" target=""><span>模板管理</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <table width="780" border="0" align="left">
                    <tr>
                        <td width="4%" height="42">
                            ID
                        </td>
                        <td width="30%">
                            商家名称
                        </td>
                        <td>
                            <p align="center"> 状态 </p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                   <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr class="fontcolor">
                        <td valign="top">
                            <p align="left">
                                ${ID}</p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                ${Com_name}</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                             <input type="button" onclick="upbank('${ID}','${Com_name}')"  value="修改"/>
                            <p>
                        </td>
                    </tr>
               </script>
                </table>
                
                <div id="divPage">
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    
     <div id="manageWithdraw" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 400px; height: auto; display:none ; left: 20%; position: absolute; top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 18px;" id="title"></span>                </td>
            </tr>
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <input type="radio" name="Uptype" value="1" /> 关闭
                    <input type="radio" name="Uptype" value="0"/>  开通
                </td>
            </tr>
            <tr>
                <td height="20" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input type="hidden" id="hid_luru_proid" value="" />
                    <input name="submit_luru" id="submit_conf" type="button" class="formButton" value="  确  定  " />
                    <input name="cancel_luru" id="cancel_conf" type="button" class="formButton" value="  关  闭  " />
               </td>
            </tr>
        </table>
    </div>
    <input id="comid" type="hidden" />
</asp:Content>
