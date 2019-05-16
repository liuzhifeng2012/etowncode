<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master"  CodeBehind="ModelMenuList.aspx.cs" Inherits="ETS2.WebApp.UI.PermissionUI.ModelMenuList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数

        $(function () {

            var modelid = $("#hid_modelid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1);

            //装载模板
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ModelHandler.ashx?oper=modelmenupagelist",
                    data: { comid: comid, modelid: modelid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询模板导航列表错误");
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
                    }
                })



                $.post("/JsonFactory/ModelHandler.ashx?oper=getModelById", { modelid: modelid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#title").text(data.msg.Title);
                        if (data.msg.Daohangimg == 0) {
                            $(".fonttab").show();
                        } else {
                            $(".imgtab").show();
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
        <div id="secondary-tabs" class="navsetting ">
            <ul>

                <li><a href="MasterList.aspx" onfocus="this.blur()" target=""><span>人员管理</span></a></li>
                <li><a href="SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>
                <li><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                <li><a href="ticketlist.aspx" onfocus="this.blur()"><span>退票管理</span></a></li>
                <!--<li><a href="bankmanager.aspx" onfocus="this.blur()"><span>绑定银行管理</span></a></li>-->
                <li class="on"><a href="ModelList.aspx" onfocus="this.blur()" target=""><span>模板管理</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                     <span id="title"></span> 栏目列表</h3> <a href="ModelMenuManage.aspx?modelid=<%=Modelid %>">添加栏目</a> <a href="ModelMenuSort.aspx?modelid=<%=Modelid %>">栏目排序</a>
                <table width="780" border="0">
                    <tr>
                        <td width="20">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="80">
                            <p align="left">
                                栏目名称
                            </p>
                        </td>
                        <td width="120">
                            <p align="left">
                                图片
                            </p>
                        </td>
                        <td width="200">
                            <p align="left">
                                链接地址
                            </p>
                        </td>
                         <td width="50">
                            <p align="left">
                                顺序
                            </p>
                        </td>
                        <td width="90">
                            <p align="left">
                                &nbsp;</p>
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
                    <tr>
                        <td>
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${Name}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                <div class="imgtab" style=" display:none;">
                                 <img alt="" class="headPortraitImgSrc" id="Img1" src="${Imgurl_address}"  height="80px" style=" background-color: #0099ff" />
                                </div>
                                <div class="fonttab" style=" display:none;">
                                <span class="${Fonticon}" sytle=""></span>
                                </div>
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                ${Linkurl}</p>
                        </td>
                                                <td>
                            <p align="left">
                                ${Sortid}</p>
                        </td>
                        <td>
                            <p align="left">
                             <a href="ModelMenuManage.aspx?id=${Id}&modelid=${Modelid}">管理 </a>  &nbsp;
                            </p>
 
                        </td>
                    </tr>
    </script>
    <input id="hid_modelid" type="hidden" value="<%=Modelid %>" />
</asp:Content>
