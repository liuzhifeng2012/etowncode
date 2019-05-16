<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/UI/Etown.Master" CodeBehind="BgImglibrary.aspx.cs" Inherits="ETS2.WebApp.UI.PermissionUI.BgImglibrary" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 100; //每页显示条数

        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1);

            //装载Banner列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ModelHandler.ashx?oper=imageLibraryList",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, usetype: 1,modelid:<%=modelid %>},
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {

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
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="MasterList.aspx" onfocus="this.blur()" target=""><span>人员管理</span></a></li>
                
                <li><a href="SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>
                <li><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                <li><a href="ticketlist.aspx" onfocus="this.blur()"><span>退票管理</span></a></li>
                <li ><a href="Modellist.aspx" onfocus="this.blur()" target=""><span>模板管理</span></a></li>
                
                <li class="on"><a href="BgImglibrary.aspx" onfocus="this.blur()" target=""><span>背景图片库管理</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                  图片库管理  </h3> <a href="ImglibraryManage.aspx?usetype=1">添加新图片</a>
                <table width="780" border="0">
                    <tr>
                        <td width="50">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="100">
                            <p align="left">
                                类型
                            </p>
                        </td>
                        <td width="100">
                            <p align="left">
                                所属模板
                            </p>
                        </td>
                        <td width="300">
                            <p align="left">
                                图片
                            </p>
                        </td>
                        <td width="80">
                            <p align="left">
                                长
                            </p>
                        </td>
                        <td width="80">
                            <p align="left">
                                宽
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
                        <td>
                            <p align="left">
                                {{if Usetype==0}}图标{{else}}其他{{/if}}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                ${ModelName}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                <img alt="" class="headPortraitImgSrc" id="Img1" src="${Imgurl_address}"  height="80px"  style=" background-color:#0099ff"/></p>
                        </td>
                                                <td>
                            <p align="left">
                               --</p>
                        </td>
                         <td>
                            <p align="left">
                                --
                        </td>
                        <td>
                            <p align="left">
                           <a href=ImglibraryManage.aspx?id=${Id}&usetype=1> 管理 </a>  
                            </p>
 
                        </td>
                    </tr>
    </script>

</asp:Content>
