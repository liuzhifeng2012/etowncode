<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master" CodeBehind="Unwarrant_Supplier.aspx.cs" Inherits="ETS2.WebApp.Agent.Unwarrant_Supplier" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
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
                    url: "/JsonFactory/AgentHandler.ashx?oper=unwarrantlist",
                    data: { pageindex: pageindex, pagesize: pageSize, agentid: agentid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
//                                $("#tblist").html("<tr><td colspan='15'>查询数据为空</td></tr>");
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
                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/AgentHandler.ashx?oper=unwarrantlist",
                        data: { pageindex: pageindex, pagesize: pageSize, key: key, agentid: agentid },
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");

                            if (data.type == 1) {
                                $.prompt("查询列表错误");
                                return;
                            }
                            if (data.type == 100) {
                                $("#tblist").empty();
                                $("#divPage").empty();
                                if (data.totalCount == 0) {
//                                    $("#tblist").html("<tr><td colspan='15'>查询数据为空</td></tr>");
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

        function autowarrant(comid,title) {
            var agentid = $("#hid_agentid").trimVal();

            if (confirm("当你点击确定后，"+title+"将自动授权,请在已授权供应商进行查看！")) {

                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=autowarrant",
                    data: { comid: comid,agentid: agentid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            alert("授权成功，进入已授权商户查");
                            window.location.href="Default.aspx"
                            return;
                        }
                    }
                })

            }
        }
       
    </script>
     
    
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
              <ul class="composetab">
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="Default.aspx">授权商户</a></div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_sel"><div><a href="Unwarrant_Supplier.aspx">更多供应商</a></div></div>
            </li>
         </ul>
          <div class="toolbg toolbgline toolheight nowrap" style="">
         <div class="right">
                      <label>
                           供应商查询
                            <input name="key" type="text" id="key" style="width: 160px; height: 20px;" />
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
                </h3>
                <table width="780" border="0" class="O2">
                    <tr class="O2title">
                        <td width="35">
                            <p align="left">
                                供应商ID</p>
                        </td>
                        <td width="150">
                            <p align="left">
                                供应商名称</p>
                        </td>
                        
                        <td width="300">
                            <p align="left">
                                简介</p>
                        </td>
                        <td width="35">
                            <p align="left">
                               上线产品数 </p>
                        </td>
                       <!-- <td width="35">
                            <p align="left">
                               成交数 </p>
                        </td>
                        <td width="35">
                            <p align="left">
                               本周成交数 </p>
                        </td>-->
                        <td width="100">
                            管理
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
      <tr class="d_out" onmouseover="this.className='d_over'" onmouseout="this.className='d_out'">
                         <td >
                            <p align="left" >
                               ${Id}</p>
                        </td>
                        <td >
                            <p align="left" >
                               ${Com_name}</p>
                        </td>
                        <td >
                            <p align="left">
                              ${Companyinfo.Scenic_intro} </p>
                        </td>
                         <td >
                            <p align="left">
                                ${Countpro}</p>
                        </td>
                        
                        <td> 
                           <a class="a_anniu" href="Unwarrant_pro.aspx?comid=${Id}"  >查看</a>  <a class="a_anniu" href="javascript:;" onclick="autowarrant('${Id}',' ${Com_name}');"  >我也要分销</a>
                        </td>

                    </tr>
                    
    </script>
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    
</asp:Content>
