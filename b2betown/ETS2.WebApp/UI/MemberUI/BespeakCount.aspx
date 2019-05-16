<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BespeakCount.aspx.cs" MasterPageFile="/UI/Etown.Master"
    Inherits="ETS2.WebApp.UI.MemberUI.BespeakCount" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var pageindex = 1;
            var pagesize = 20;

            var bespeaktype = "0,1"; //预约类型:0提单直接预约；1自助预约
            var bespeakstate = "0"; //预约状态:-1全部；0未处理预约；1预约成功处理；2预约失败处理

            //日历
            var startdate = '<%=this.nowdate %>';
            var dateinput = $("input[isdate='yes']");
            $.each(dateinput, function (i) {
                //                $("#startdate").val(startdate);
                $($(this)).datepicker();
            });


            var comid = $("#hid_comid").trimVal();
            SearchList(pageindex, "", comid, "", bespeaktype, bespeakstate);


            $("#chaxun").click(function () {
                var startdate = $("#startdate").trimVal();
                var comid = $("#hid_comid").trimVal();
                var key = $("#key").trimVal();
                bespeakstate = $("#bespeakstate").trimVal();

                SearchList(pageindex, startdate, comid, key, bespeaktype, bespeakstate);
            })
        })

        function opersuc(id) {
            $.post("/JsonFactory/ProductHandler.ashx?oper=operbespeakstate", { id: id, bespeakstate: 1 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { alert("操作出错；请联系技术人员"); return; }
                if (data.type == 100) {
                    $("#btnsuc_" + id).hide();
                    $("#btnfail_" + id).hide();
                    $("#label_" + id).text("预约成功");
                    alert("操作成功");
                    return;
                }
            })

        }
        function operfail(id) {
            $.post("/JsonFactory/ProductHandler.ashx?oper=operbespeakstate", { id: id, bespeakstate: 2 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { alert("操作出错；请联系技术人员"); return; }
                if (data.type == 100) {
                    $("#btnsuc_" + id).hide();
                    $("#btnfail_" + id).hide();
                    $("#label_" + id).text("预约失败");
                    alert("操作成功");
                    return;
                }
            })

        }
        function SearchList(pageindex, bespeakdate, comid, key, bespeaktype, bespeakstate) {

            $.ajax({
                type: "post",
                url: "/JsonFactory/ProductHandler.ashx?oper=getbespeaklist",
                data: { bespeakdate: bespeakdate, comid: comid, pageindex: pageindex, pagesize: 20, key: key, bespeaktype: bespeaktype, bespeakstate: bespeakstate },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //                        $.prompt("");
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalcount == 0) {
                            //                                $("#tblist").html("查询数据为空");
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalcount, 20, pageindex, bespeakdate, comid, key, bespeaktype, bespeakstate);
                        }


                    }
                }
            })


        }
        //分页
        function setpage(totalcount, newpagesize, curpage, bespeakdate, comid, key, bespeaktype, bespeakstate) {
            $("#divPage").paginate({
                count: Math.ceil(totalcount / newpagesize),
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

                    SearchList(page, bespeakdate, comid, key, bespeaktype, bespeakstate);

                    return false;
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <div class="navsetting ">
                <li><a href="/ui/pmui/order/orderlist.aspx" onfocus="this.blur()" target="">订单列表</a></li>
                <li><a href="/ui/MemberUI/ChannelFinance.aspx" onfocus="this.blur()">门票返佣 </a></li>
                <li><a href="/ui/MemberUI/travelbusordercount.aspx" onfocus="this.blur()">旅游大巴订单统计 </a>
                </li>
                <li class="on"><a href="/ui/MemberUI/BespeakCount.aspx" onfocus="this.blur()" target="">
                    预约统计</a></li>
            </div>
        </div>--%>
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
                <h3>
                    预约统计</h3>
                预约状态
                <select id="bespeakstate" class="mi-input" style="width: 120px;">
                    <option value="-1">全 部</option>
                    <option value="0" selected="selected">未处理预约</option>
                    <option value="1">预约成功处理</option>
                    <option value="2">预约失败处理</option>
                </select>
                预约日期:
                <input type="text" style="width: 100px;" id="startdate" isdate="yes" class="mi-input">
                <label>
                    关键词</label>
                <input name="key" type="text" id="key" class="mi-input" style="width: 120px;" />
                <input type="button" value="查询" id="chaxun" style="width: 120px; height: 26px;">
                <table width="780px" border="0">
                    <tr>
                        <td width="35px;" height="30px">
                            <p>
                                预约人姓名
                            </p>
                        </td>
                        <td width="50px;">
                            <p>
                                预约人手机
                            </p>
                        </td>
                        <td width="70px;">
                            <p>
                                预约人身份证
                            </p>
                        </td>
                        <td width="50px;">
                            <p>
                                预约日期
                            </p>
                        </td>
                        <td width="30px;">
                            <p>
                                预约人数
                            </p>
                        </td>
                        <td style="display: none;">
                            <p>
                                预约类型
                            </p>
                        </td>
                        <td width="50px;">
                            <p>
                                预约状态
                            </p>
                        </td>
                        <td width="50px;">
                            <p>
                                预约备注
                            </p>
                        </td>
                        <td style="display: none;">
                            <p>
                                提交时间
                            </p>
                        </td>
                        <td width="50px;">
                            <p>
                                电子票号
                            </p>
                        </td>
                        <td width="30px;">
                            <p>
                                订单号
                            </p>
                        </td>
                        <td style="display: none;">
                            <p>
                                产品编号
                            </p>
                        </td>
                        <td width="50px;">
                            <p>
                                产品名称
                            </p>
                        </td>
                        <td width="100px;">
                            <p>
                                预约管理
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
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr class="fontcolor">
                        <td >
                           ${Bespeakname}
                        </td>
                       
                        <td >
                               ${Phone}
                        </td>
                        <td >
                               ${Idcard}
                        </td>
                        <td >
                             ${ChangeDateFormat(Bespeakdate)}
                        </td>
                        <td >
                               ${Bespeaknum}
                        </td>
                        <td   style="display:none;">
                          ${beaspeaktype}
                        </td>
                        <td >
                               ${beaspeakstate}
                        </td>
                        <td>
                        ${remark}
                        </td>
                         <td  style="display:none;">
                        ${jsonDateFormat(subtime)}
                        </td>
                         <td>
                        ${Pno}
                        </td>
                         <td>
                        ${Orderid}
                        </td>
                        <td style="display:none;">
                        ${Proid}
                        </td>
                        <td>${Proname}</td>
                        <td>
                        {{if beaspeakstate==0}}
                        <input type="button" id="btnsuc_${Id}" value="成功处理" onclick="opersuc(${Id})"/>
     <input type="button" id="btnfail_${Id}" value="失败处理"  onclick="operfail(${Id})"/>
      <label id="label_${Id}"></label>
                        {{else}}
                         <label id="label_${Id}">${beaspeakstatedesc}</label>
                        {{/if}}
                       
                         
                        </td>
                    </tr>
    </script>
</asp:Content>
