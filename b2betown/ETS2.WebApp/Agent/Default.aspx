<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/Agent/Manage.Master" CodeBehind="Default.aspx.cs" Inherits="ETS2.WebApp.Agent.Default" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            if (IsPC() == false) {
                location.href = "/agent/m";
            }

            var agentid = $("#hid_agentid").trimVal();

            SearchList(1);

            //装载列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=warrantlist",
                    data: { pageindex: pageindex, pagesize: pageSize, agentid: agentid },
                    async: false,
                    success: function (data) {
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
                    }
                })


                //查询
                $("#Search").click(function () {
                    var pageindex = 1;
                    var key = $("#key").val();
                    var select = '';

                    if (key == "" || key == null) {
                        $.prompt("查询条件为空");
                        return;
                    }
                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/AgentHandler.ashx?oper=warrantlist",
                        data: { pageindex: pageindex, pagesize: pageSize, key: key, agentid: agentid },
                        async: false,
                        success: function (data) {
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
                        }
                    })
                })

                $("#0").click(function () {
                    select(0, 1);
                    $("#0").css("color", "red");
                })
                $("#1").click(function () {
                    select(1, 1);
                    $("#1").css("color", "red");
                })
                $("#3").click(function () {
                    select(3, 1);
                    $("#3").css("color", "red");
                })
                $("#4").click(function () {
                    select(4, 1);
                    $("#4").css("color", "red");
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

        function IsPC() {
            var userAgentInfo = navigator.userAgent;
            var Agents = ["Android", "iPhone",
                "SymbianOS", "Windows Phone",
                "iPad", "iPod"];
            var flag = true;
            for (var v = 0; v < Agents.length; v++) {
                if (userAgentInfo.indexOf(Agents[v]) > 0) {
                    flag = false;
                    break;
                }
            }
            return flag;
        }
       
    </script>
     
    
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">

        
        <ul class="composetab">
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_sel"><div><a href="Default.aspx">授权商户</a></div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="Unwarrant_Supplier.aspx">更多供应商</a></div></div>
            </li>
         </ul>
          <div class="toolbg toolbgline toolheight nowrap" style="">
         <div class="right">
                     <label>
                           商户查询
                            <input name="key" class="mi-input" type="text" id="key" style="width: 160px;" />
                        </label>
                        <label>
                            <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
                        </label>
         </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
         <!--<a class="btn_gray btn_space" hidefocus="" id="quick_del" href="javascript:;" name="del">删除</a>-->
         
         
         </div></div>


        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <table width="780" border="0"  class="O2">
                    <tr class="O2title">
                        <td width="26">
                            <p align="left">
                                商户ID</p>
                        </td>
                        <td width="150">
                            <p align="left">
                                名称</p>
                        </td>
                        <td width="60">
                            <p align="left">
                                 城市
                            </p>
                        </td>
                        <td width="60">
                            <p align="left">
                                 上线产品数量
                            </p>
                        </td>
                        <td width="80">
                            <p align="left">
                                预付款金额</p>
                        </td>
                         <td width="80">
                            <p align="left">
                                信用额</p>
                        </td>
                        <td width="100">
                            管理
                        </td>
                                                <td width="60">
                            <p align="left">
                                商户通知</p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage" >
                </div>
            </div>
        </div>

    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">
    {{if ComState==1}}
      <tr class="d_out" onmouseover="this.className='d_over'" onmouseout="this.className='d_out'">
                        <td >
                            <p align="left">
                               ${Comid}</p>
                        </td>
                        <td >
                            <p align="left" title="${Comname}" >
                               ${Comname}</p>
                        </td>
                        <td >
                            <p align="left">
                            {{if Cominfo !=null}}
                                ${Cominfo.Province} - ${Cominfo.Com_city}
                                
                                {{/if}}
                            </p>
                        </td>
                        <td >
                            <p align="left">
                                ${Countpro}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Imprest}</p>
                        </td>
                                                <td >
                            <p align="left">
                                ${Credit}</p>
                        </td>
                        <td> 
                             <a   class="a_anniu" href="Manage_sales.aspx?comid=${Comid}"  >进入此商户</a>
                        </td>
                                                <td >
                            <p align="left">
                               {{if MessageNew==0}}
                               
                               {{else}}
                               <a href="javascript:;" onclick="MessageList(1,'${Comid}','${Comname}')"><span style="color:#ff0000">商户通知 ${MessageNew} 条</span></a></p>
                               {{/if}}
                        </td>
                    </tr>
   {{/if}}
    </script>
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />

    
</asp:Content>
