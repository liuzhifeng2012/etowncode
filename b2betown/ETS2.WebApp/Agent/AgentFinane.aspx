<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master" CodeBehind="AgentFinane.aspx.cs" Inherits="ETS2.WebApp.Agent.AgentFinane" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 200; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var comid = $("#hid_comid_temp").trimVal();
            SearchList(1);
            //装载列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=agentFinacelist",
                    data: { comid: comid, agentid: agentid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询财务列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist").html("查询数据为空");
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
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
                    <table>
                    <tr>
                        <td class="tdHead" style="font-size:14px;height:26px;">
                           <%=yufukuan%>  (<a href="Recharge.aspx?comid=<%=comid_temp %>">在线充值</a>) 
                        </td>

                    </tr>
                </table>
        <div id="secondary-tabs" class="navsetting ">
         <ul class="composetab">
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="Agent_Com_list.aspx">商户列表</a></div></div>
            </li>
             <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="Agent_Com_Open.aspx">开通新商户</a></div></div>
            </li>
             <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_sel"><div><a href="AgentFinane.aspx">财务列表</a></div></div>
            </li>

         </ul>
          <div class="toolbg toolbgline toolheight nowrap" style="">
         <div class="right searchtool">
                 <span>&nbsp;</span>   
         </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
        
         
         </div></div>
        </div>

        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <table width="780" border="0">
                    <tr>
                        <td width="51">
                            <p align="left">
                                流水号</p>
                        </td>
                        <td width="100">
                            <p align="left">
                                操作时间
                            </p>
                        </td>
                        <td width="204">
                            <p align="left">
                                内容
                            </p>
                      </td>
                        <td width="76">
                            <p align="left">
                                收支类型
                            </p>
                      </td>
                        <td width="50">
                            <p align="left">
                                收入
                            </p>
                      </td>
                        <td width="44">
                            <p align="left">
                                支出
                            </p>
                      </td>
                        <td width="66">
                            <p align="left">
                                余额
                            </p>
                      </td>
                        <td width="79">
                            <p align="left">
                                资金渠道
                            </p>
                      </td>
                        <td width="72">
                            <p align="left">&nbsp;
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
                        <td width="51">
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td width="100">
                            <p align="left">
                                ${ChangeDateFormat(Subdate)}
                            </p>
                        </td>
                        <td width="204">
                            <p align="left">
                                ${Servicesname}</p>
                        </td>
                        <td width="76">
                            <p align="left">
                                ${Payment_type}</p>
                        </td>
                        <td width="50">
                            <p align="left">
                                {{if Money>= 0}}${Money}{{/if}}</p>
                        </td>
                        <td width="44">
                            <p align="left">
                              {{if Money< 0}}${Money}{{/if}}</p>
                        </td>
                        <td width="66">
                            <p align="left">
                                ${Over_money}    
                            </p>
                        </td>
                        <td width="79">
                            <p align="left">
                                ${Money_come}</p>
                        </td>
                        <td width="72">
                            <p align="left">
                            ${Payment_type}${Remarks}
                            </p>
                        </td>
                    </tr>
                    
    </script>
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
</asp:Content>

