<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgentRecharge_Person.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.AgentRecharge_Person"   MasterPageFile="/UI/Etown.Master" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 20; //每页显示条数

        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1);

            //            $("#Search").click(function () {
            //                SearchList(1);
            //            })

            //            $("html").die().live("keydown", function (event) {
            //                if (event.keyCode == 13) {
            //                    $("#Search").click();    //这里添加要处理的逻辑  
            //                    return false;
            //                }
            //            });

            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }

                $.post("/JsonFactory/AgentHandler.ashx?oper=personrechargepagelist", { comid: comid, pageindex: pageindex, pagesize: pageSize, key: "", payment_type: "", agentid: "" }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询列表错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalCount == 0) {
                            //                                $("#tblist").html("查询数据为空");
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalCount, pageSize, pageindex);
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
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
<div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/pmui/AgentList.aspx" onfocus="this.blur()" target=""><span>分销商列表</span></a></li>
                <li><a href="/ui/pmui/AgentRegi.aspx" onfocus="this.blur()" target=""><span>新增分销商</span></a></li>
                <li ><a href="/ui/pmui/AgentManage.aspx" onfocus="this.blur()" target=""><span>易城商户分销商</span></a></li>
                <li><a href="/ui/pmui/AgentSalesCode.aspx" onfocus="this.blur()" target=""><span>后台销售订单</span></a></li>
                <li><a href="/ui/pmui/AgentBackCode.aspx" onfocus="this.blur()" target=""><span>导码订单</span></a></li>
                <li><a href="/ui/pmui/AgentRecharge.aspx" onfocus="this.blur()" target=""><span>充值订单</span></a></li>
                <li  class="on"><a href="/ui/pmui/AgentRecharge_Person.aspx" onfocus="this.blur()" target=""><span>人工充值记录</span></a></li>
                <li><a href="/ui/pmui/AgentMessage.aspx" onfocus="this.blur()" target="">管理分销通知</a></li>
                <li><a href="/ui/pmui/AgentFinanceCount.aspx" onfocus="this.blur()" target="">分销商统计</a>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
                <h3>
                    人工充值记录</h3>
                <div style="text-align: center; display:none;">
                    <label>
                        关键词
                        <input name="key" type="text" id="key" style="width: 160px; height: 20px;">
                    </label>
                    <select id="order_state">
                        <option value="0" selected>全部</option>
                        
                    </select>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px;
                            height: 26px;" >
                    </label>
                </div>
                <table width="780px" border="0">
                    <tr>
                        <td width="45px" height="30px">
                            <p align="left">
                                ID
                            </p>
                        </td>
                        <td width="100px">
                            <p align="left">
                                提交时间
                            </p>
                        </td>
                        <td width="100px">
                            <p align="left">
                                充值分销商
                            </p>
                        </td>
                        <td width="50px">
                            <p align="left">
                                交易号
                            </p>
                        </td>
                         <td width="50px">
                            <p align="left">
                                充值操作人  
                            </p>
                        </td> 
                        <td width="50px">
                            <p align="left">
                                支付类型 
                            </p>
                        </td>
                        <td width="30px">
                            <p align="left">
                               支付金额
                            </p>
                        </td>
                        <td width="130px">
                            <p align="left">
                                备注 
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
                            <p >
                                ${Id}</p>
                        </td>
                        <td valign="top">
                            <p  >
                                ${jsonDateFormatKaler(Subdate)}</p>
                        </td>
                        <td valign="top">
                            <p >
                                ${AgentCompanyName}
                            </p>
                        </td>
                        <td valign="top">
                            <p  >
                                交易号:${Id}
                                </p>
                        </td>
                        <td valign="top">
                            <p  >
                                 ${UserName}
                            </p>
                        </td> 
                        <td valign="top">
                            <p >
                            ${Payment_type}
                              </p>
                        </td>
                        <td valign="top">
                            <p >
                                ${Money} </p>
                        </td>
                        
                        <td valign="top">
                            <p title="${Servicesname}">
                             ${Servicesname}
                                </p>
                        </td>
                    </tr>
    </script>
   
     
    <input type="hidden" id="hid_id" value="0" />
    <input type="hidden" id="hid_proid" value="0" />
</asp:Content>

