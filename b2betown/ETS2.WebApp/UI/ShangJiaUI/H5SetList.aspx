﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="H5SetList.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.H5SetList" %>

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
                    url: "/JsonFactory/DirectSellHandler.ashx?oper=getimagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize,typeid:0 },
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


        //设置状态
        function setstate(id) {
            var comid = $("#hid_comid").trimVal();
            $.ajax({
                type: "post",
                url: "/JsonFactory/DirectSellHandler.ashx?oper=updwonstate",
                data: { comid:comid,id: id },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取信息失败");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("修改成功!");
                        location.href = "H5Setlist.aspx";
                    }
                }
            })
        }


        function Deletebanner(id) {
            var comid = $("#hid_comid").trimVal();
             $.post("/JsonFactory/DirectSellHandler.ashx?oper=deleteimage", { id: id, comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("操作出现错误");

                    return;
                }
                if (data.type == 100) {
                    $.prompt("删除成功");
                    location.href = "H5Setlist.aspx";
                    return;
                }
            })
       }

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/shangjiaui/H5Default.aspx" onfocus="this.blur()" target="">模版设置</a></li>
                <li><a href="/ui/shangjiaui/H5SetMenu.aspx" onfocus="this.blur()" target="">栏目管理</a></li>
                <li class="on"><a href="/ui/shangjiaui/H5Setlist.aspx" onfocus="this.blur()" target="">背景图片管理</a></li>
                <li><a href="/ui/shangjiaui/StoreList.aspx" onfocus="this.blur()" target="">门店模板设置</a></li>
                <li><a href="/ui/shangjiaui/ShopManage.aspx" onfocus="this.blur()" target="">微商城设置</a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                 <a href="H5Setting.aspx"  class="a_anniu">添加新景图片</a>
                <table width="780" border="0">
                    <tr>
                        <td width="20">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="100">
                            <p align="left">
                                名称
                            </p>
                        </td>
                        <td width="150">
                            <p align="left">
                                图片
                            </p>
                        </td>
                        <td width="200">
                            <p align="left">
                                连接地址
                            </p>
                        </td>
                        <td width="80">
                            <p align="left">
                                上线状态
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
                                ${Title}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                <img alt="" class="headPortraitImgSrc" id="Img1" src="${Imgurl_address}"  height="80px"/></p>
                        </td>
                                                <td>
                            <p align="left">
                                ${Linkurl}</p>
                        </td>
                         <td>
                            <p align="left">
                                ${State} <input type="button" onclick="setstate('${Id}')"  value="调整状态"/></p>
                        </td>
                        <td>
                            <p align="left">
                           
                            <div style=" float:left;"><a href="H5Setting.aspx?id=${Id}"  class="a_anniu"> 管理 </a>  </div><div style=" float:left;padding-left:10px;">  <input type="button"  name="deletebanner" id="deletebanner" onclick="Deletebanner('${Id}')" value="  删除图片  " /></div>
                            </p>
 
                        </td>
                    </tr>
    </script>

</asp:Content>
