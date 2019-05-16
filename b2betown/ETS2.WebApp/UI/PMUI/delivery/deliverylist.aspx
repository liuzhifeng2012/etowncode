<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="deliverylist.aspx.cs" MasterPageFile="/UI/Etown.Master"
    Inherits="ETS2.WebApp.UI.PMUI.delivery.deliverylist" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">


        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();

            SearchList(1, 20);

            function SearchList(pageindex, pagesize) {

                $.post("/JsonFactory/ProductHandler.ashx?oper=deliverytmppagelist", { comid: comid, pageindex: pageindex, pagesize: pagesize }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalcount == 0) {
                            //                                $("#tblist").html("查询数据为空");
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalcount, pagesize, pageindex);
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

                        SearchList(page, newpagesize);

                        return false;
                    }
                });
            }


        })
        function deltmp(tmpid, tmpname) {
            if (confirm("确认删除"+tmpname+"吗?")) {
                if (tmpid == 0) {
                    alert("模板删除失败");
                    return;
                } else {
                    $.post("/JsonFactory/ProductHandler.ashx?oper=deltmp", { tmpid: tmpid, comid: $("#hid_comid").trimVal() }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            alert(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            alert("运费模板删除成功");
                            window.location.reload();
                            return;
                        }
                    });
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/pmui/projectlist.aspx" onfocus="this.blur()" target=""><span>项目管理</span></a></li>
                <li><a href="/ui/pmui/Projectedit.aspx" onfocus="this.blur()" target=""><span>添加项目</span></a></li>
                <li><a href="/ui/pmui/ProductList.aspx" onfocus="this.blur()" target=""><span>产品列表</span></a></li>
                <li><a href="/ui/pmui/ProductServerTypeList.aspx" onfocus="this.blur()" target=""><span>
                    添加产品</span></a></li>
                <li><a href="/ui/pmui/order/Salecount.aspx" onfocus="this.blur()" target="">产品统计</a></li>
                <li><a href="/ui/pmui/BindingAgent.aspx" onfocus="this.blur()" target="">导入分销系统产品</a></li>
                <li><a href="/ui/pmui/eticket_useset.aspx" onfocus="this.blur()" target=""><span>商户特定日期设定</span></a></li>
                <li class="on"><a href="/ui/pmui/delivery/deliverylist.aspx" onfocus="this.blur()"
                    target=""><span>运费模版管理</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    运费模板列表</h3>
                <h4 style="float: right">
                    <a style="" href="/ui/pmui/delivery/deliverymanage.aspx" class="a_anniu">新建运费模板</a>
                </h4>
                <table width="780" border="0">
                    <tr>
                        <td width="10%">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="40%">
                            <p align="left">
                                模板名称
                            </p>
                        </td>
                          <td width="20%">
                            <p align="left">
                                计费方式
                            </p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                更新时间
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                管理 &nbsp;</p>
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
                        <td>
                            <p>${id}</p>
                        </td>
                        <td>
                            <p>${tmpname}</p>
                        </td>
                        <td>
                            <p>
                            {{if ComputedPriceMethod==1}}
                             按数量计费 
                            {{/if}}
                             {{if ComputedPriceMethod==2}}
                            按重量计费 
                            {{/if}}
                            </p>
                        </td>
                           <td>
                            <p>${jsonDateFormatKaler(opertime)}</p>
                        </td>
                        <td>
                            <p><a href="/ui/pmui/delivery/deliverymanage.aspx?tmpid=${id}">编辑</a>  <a href="javascript:void(0)" onclick="deltmp('${id}','${tmpname}')">删除</a>   </p>
                        </td>
                    </tr>
    </script>
</asp:Content>
