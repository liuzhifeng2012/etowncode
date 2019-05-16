<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master"  CodeBehind="H5SetStep.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.H5SetStep" %>


<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
        <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var pageSize = 100; //每页显示条数
            var comid = $("#hid_comid").trimVal();



            SearchList(1);

            //装载列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ModelHandler.ashx?oper=modelpagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
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

        //选择模板
        function selectmodel(modelid) {
            var comid = $("#hid_comid").trimVal();
            var r = confirm("更换模板时，如果已经有栏目则不更换栏目及图标，可能影响微站点美观,请确认是否更换！");
            if (r == true) {
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ModelHandler.ashx?oper=selectmodel",
                    data: { modelid: modelid,comid:comid},
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("模板设定错误,请刷新重新操作");
                            return;
                        }
                        if (data.type == 100) {
                            location.href = "H5Default.aspx";
                        }
                    }
                })



            } else {

            }
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/shangjiaui/H5Default.aspx" onfocus="this.blur()" target="">模版设置</a></li>
                <li class="on"><a href="/ui/shangjiaui/H5SetStep.aspx" onfocus="this.blur()" target="">模版选择</a></li>
                <li><a href="/ui/shangjiaui/StoreList.aspx" onfocus="this.blur()" target="">门店模板设置</a></li>
                <li><a href="/ui/shangjiaui/ShopManage.aspx" onfocus="this.blur()" target="">微商城设置</a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
               
                <h3>
                  微站点模板选择  </h3>
                <table width="780" border="0">
                    <tr>
                        <td width="20">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="200">
                            <p align="left">
                                模板名称
                            </p>
                        </td>
                        <td width="200">
                            <p align="left">
                                缩略图
                            </p>
                        </td>
                        <td width="100">
                            <p align="left">
                                导航数量
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
                    <tr {{if Usestate == 1}}style="background-color:#0099FF"{{/if}}>
                        <td>
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${Title}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                <img alt="" class="headPortraitImgSrc" id="Img1" src="${Smallimg}" /></p>
                        </td>
                            <td>
                            <p align="left">
                                ${Daohangnum}</p>
                            </td>
                        <td>
                            <p align="left">
                            {{if Usestate == 0}}
                             <input type="button" name="button" id="button" value="  选择此模板  " onclick="selectmodel('${Id}')" >
                             {{else}}
                             使用中
                             {{/if}}

                            </p>
 
                        </td>
                    </tr>
    </script>
    <input type="hidden" id="hid_modelid" value="" />
    <input type="hidden" id="modelid" value="<%=Modelid %>" />
</asp:Content>
