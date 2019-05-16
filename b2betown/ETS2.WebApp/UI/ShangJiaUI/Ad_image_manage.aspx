<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="Ad_image_manage.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.Ad_image_manage" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
        <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Deletebanner(id) {
            var comid = $("#hid_comid").trimVal();
            var adid = $("#hid_adid").trimVal();
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=DelWxadimages", { id: id, adid: adid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("操作出现错误");
                    return;
                }
                if (data.type == 100) {
                    $.prompt("删除成功");
                    location.href = "Ad_image_manage.aspx?adid="+adid;
                    return;
                }
            })
        }


        $(function () {
            var pageSize = 20; //每页显示条数
            var comid = $("#hid_comid").trimVal();
            var adid = $("#hid_adid").trimVal();

            SearchList(1);

            //装载栏目
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/WeiXinHandler.ashx?oper=Getwxadimagespagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, adid: adid },
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

        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                

                <h3>
                <%=Title %>  </h3>  <span><a href="Ad_image_up.aspx?adid=<%=adid %>" class="a_anniu" style=" float:right;">上传新图片</a></span>  <span><a href="Ad_image_sort.aspx?adid=<%=adid %>" class="a_anniu" style=" float:right;">排序</a></span> 
                <table width="780" border="0">
                    <tr>
                        <td width="20">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="150">
                            <p align="left">
                                图片
                            </p>
                        </td>
                       

                        <td width="250" > 
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
                                <img src="${Imageurl}" height="60px">
                            </p>
                        </td>
                        
                        <td>
                            <p align="left">
                            <div style=" float:left;padding-left:10px;">  <input type="button"  name="deletebanner" id="deletebanner" onclick="Deletebanner('${Id}')" value="  删除  " /></div>
                            </p>
 
                        </td>
                    </tr>
    </script>
    <input type="hidden" id="hid_adid" value="<%=adid %>" />
</asp:Content>
