<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/UI/Etown.Master" CodeBehind="BindingAgentPorList.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.BindingAgentPorList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 15; //每页显示条数

        $(function () {
            var comid = $("#hid_comid").trimVal();
            SearchList(1);

            //装载产品列表
            function SearchList(pageindex) {
                var key = $("#key").trimVal();
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }

                $.post("/JsonFactory/AgentHandler.ashx?oper=bindingwarrantprolist", { pageindex: pageindex, pagesize: pageSize, comid: comid, key: key }, function (data) {

                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询渠道列表错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalCount == 0) {
                            $("#tblist").html("<tr><td colspan='15'>查询数据为空</td></tr>");
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalCount, pageSize, pageindex);
                        }
                    }
                })
            }

            $("#ImportPro").click(function () {
                var proid = "";

                $("input[name='proid']:checkbox:checked").each(function () { 
                    proid += $(this).val() + ",";
                })
                if (proid == "") {
                    $.prompt("请选择导入的产品");
                    return;
                }

                $.post("/JsonFactory/AgentHandler.ashx?oper=imprestpro", { comid: comid, proid: proid }, function (data) {

                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("导入出错，" + data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("导入成功");
                        location.reload();
                    }
                })



            })

            //查询
            $("#Search").click(function () {
                var key = $("#key").val();

                SearchList(1, key);
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
                });
            }
        })

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/pmui/projectlist.aspx" onfocus="this.blur()" target=""><span>项目管理</span></a></li>
                <li><a href="/ui/pmui/Projectedit.aspx" onfocus="this.blur()" target=""><span>添加项目</span></a></li>
                <li><a href="/ui/pmui/ProductList.aspx" onfocus="this.blur()"
                    target=""><span>产品列表</span></a></li>
                <li><a href="/ui/pmui/ProductServerTypeList.aspx" onfocus="this.blur()"
                    target=""><span>添加产品</span></a></li>
                <li><a href="/ui/pmui/order/Salecount.aspx" onfocus="this.blur()" target="">产品统计</a></li>
                <li class="on"><a href="/ui/pmui/BindingAgent.aspx" onfocus="this.blur()" target="">导入分销系统产品</a></li>
            </ul>
        </div>
      <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                    
                <table width="780" border="0">
                    <tr>
                        <td width="26">
                            <p align="left">
                                序号</p>
                        </td>
                        <td width="120">
                            <p align="left">
                                项目</p>
                        </td>
                        <td width="160">
                            <p align="left">
                                产品名称</p>
                        </td>
                        <td width="80">
                            产品有效期
                        </td>
                          <td width="160">
                            使用有效期
                        </td>
                        <td width="50">
                            门市价
                        </td>
                        <td width="50">
                            销售价
                        </td>
                        <td width="50">
                            分销结算价
                        </td>
                        <td width="100">
                            <p align="left">
                                备注</p>
                        </td>
                        <td width="100">
                            <input type="button" id="ImportPro" value="批量导入产品" />
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
            </div >
        </div>

    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">

                <tr>
                        <td >
                            <p align="left">
                               ${Id}</p>
                        </td>
                        <td >
                            <p align="left">
                               ${Projectname}</p>
                        </td>
                        <td >
                            <p align="left" >
                              <a href="javascript:;" onclick="Proadd('${Pro_name}','${Pro_Remark}','${Service_Contain}','${Service_NotContain}','${Precautions}')"> ${Pro_name}</a></p>
                        </td>
                        <td >${ChangeDateFormat(Pro_end)}
                        </td>
                        <td >
                        {{if (Iscanuseonsameday==0)}}
                        购买当天不可使用
                        {{/if}}

                        {{if (ProValidateMethod=="2")}}

                                   {{if (Appointdata == 1)}}
                                    出票一周有效
                                   {{/if}}
                                   
                                   {{if (Appointdata == 2)}}
                                   出票一月有效
                                   {{/if}}
                                   
                                   {{if (Appointdata == 3)}}
                                   出票三月有效
                                   {{/if}}
                                   
                                   {{if (Appointdata == 4)}}
                                   出票半年有效
                                   {{/if}}
                                   
                                   {{if (Appointdata == 5)}}
                                    出票一年有效
                                   {{/if}}
                        {{else}}
                        同产品有效期
                        {{/if}}

                        </td>
                        <td >
                            ${Face_price}
                        </td>
                        <td >
                            <p align="left">
                                ${Advise_price}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Agent_price}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Pro_explain}</p>
                        </td>
                        <td >
                            
                                <input type="checkbox" name="proid" value="${Id}" />
                                
                        </td>
                    </tr>
       
    </script>
    <input type="hidden" id="hid_comid" value="<%=wacomid %>" />
</asp:Content>

