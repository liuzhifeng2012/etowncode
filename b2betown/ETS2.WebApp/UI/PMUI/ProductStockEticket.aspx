<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ProductStockEticket.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.ProductStockEticket" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数

        $(function () {
            var comid = $("#hid_comid").trimVal();
            var pro_id = $("#hid_proid").trimVal();
            var statetype = $("#hid_statetype").trimVal();

            SearchList(1);

            //装载产品列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=searchpnolist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, pro_id: pro_id, statetype: statetype },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询产品列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                               // setpage(data.totalCount, pageSize, pageindex);
                            }
                        }
                    }
                })
            }

        })


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
                })
            }



    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="ProductList.aspx" onfocus="this.blur()" target=""><span>产品管理</span></a></li>
                <li><a href="ProductAdd.aspx" onfocus="this.blur()" target=""><span>添加产品</span></a></li>
                <li class="on"><a href="ProductStockEticket.aspx" onfocus="this.blur()" target=""><span>库存电子票管理</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    库存电子票</h3><a href="#" onclick="javascript:history.go(-1);" > <<<返回</a>
                <table width="780" border="0">
                    <tr>
                        <td width="30">
                            <p align="left">
                                ID
                            </p>
                        </td>
                        <td width="200">
                            <p align="left">
                                产品名称
                            </p>
                        </td>
                        <td width="120">
                            <p align="left">
                                电子票码
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                数量
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                状态
                            </p>
                        </td>
                        <td width="200">
                            <p align="left">
                                录入日期
                            </p>
                        </td>
                        <td width="200">
                            <p align="left">
                            使用日期
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
                        <td>
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${E_proname}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                ${Pno}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Pnum}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Runstate}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${Subdate}</p>
                        </td>
                        <td>
                            <p align="left">
                                  ${Sendtime}    
                            </p>
                        </td>
                        <td width="71">
                            <p align="left">
                               --
                            </p>
 
                        </td>
                    </tr>
    </script>
   <input type="hidden" id="hid_proid" value="<%=proid%>" />
   <input type="hidden" id="hid_statetype" value="<%=statetype%>" />
</asp:Content>
