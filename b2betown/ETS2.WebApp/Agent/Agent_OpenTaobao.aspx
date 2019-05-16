<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Agent_OpenTaobao.aspx.cs"
    Inherits="ETS2.WebApp.Agent.Agent_OpenTaobao" MasterPageFile="/Agent/Manage.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
  <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            $.post("/JsonFactory/AgentHandler.ashx?oper=GetTb_agent_relationList", {agentid:agentid}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    $("#tblist").empty(); 
                    if (data.msg == '') {
//                        $("#tblist").html("<tr><td colspan='5'>还暂未添加店铺信息</td></tr>");
                    } else {
                        $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist"); 
                    }
                }
            })
        })
    </script>
    <style type="text/css">
        .dataIcon
        {
            width: 400px;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
                <ul class="composetab">
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="AgentStaff.aspx">员工管理</a></div></div>
            </li>
             <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="AgentCompany.aspx">分销商信息</a></div></div>
            </li>
             <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_sel"><div><a href="Agent_OpenTaobao.aspx">淘宝店铺</a></div></div>
            </li>

         </ul>
          <div class="toolbg toolbgline toolheight nowrap" style="">
         <div class="right searchtool">
                 <span>&nbsp;</span>   
         </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
         <a class="btn_gray btn_space" hidefocus="" id="quick_add" href="Agent_OpenTaobao_Manage.aspx?tbid=0" name="add">开通新淘宝店铺</a>
         
         </div></div>

        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">

                <table width="780" border="0" class="O2">
                    <tr class="O2title">
                        <td width="26">
                            <p align="left">
                                序号</p>
                        </td>
                       <td width="60">
                            <p align="left">
                                淘宝ID</p>
                        </td>
                        <td width="60">
                            <p align="left">
                                淘宝旺旺号
                            </p>
                        </td>
                        <td width="100">
                            <p align="left">
                                淘宝店铺名称</p>
                        </td>
                        <td width="100">
                            <p align="left">
                                淘宝店铺地址</p>
                        </td>
                        <td width="50">
                            <p align="left">
                                审核情况</p>
                        </td>
                    </tr>
                    <tbody id="tblist" class="O2title">
                    </tbody>
                </table>
             
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">
      <tr>
                        <td >
                            <p align="left">
                               ${serialnum}</p>
                        </td>
                        <td >
                            <p align="left" >
                                ${tb_id}</p>
                        </td> 
                        <td >
                            <p align="left">
                                ${tb_seller_wangwang} </p>
                        </td>
                        <td >
                            <p align="left">
                                ${tb_shop_name}</p>
                        </td>
                                                <td >
                            <p align="left">
                                ${tb_shop_url}</p>
                        </td>
                              <td >
                            <p align="left">

                                {{if $.trim(tb_seller_wangwangid)==""}}
                                 待审核
                                 {{else}}
                                 审核通过
                                {{/if}}
                           </p>
                        </td>
                    </tr>
                    
    </script>
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
</asp:Content>
