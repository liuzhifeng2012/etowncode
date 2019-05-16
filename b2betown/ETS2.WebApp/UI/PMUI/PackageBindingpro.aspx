<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/UI/Etown.Master" CodeBehind="PackageBindingpro.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.PackageBindingpro" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
        <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Deletebanner(id) {
            var comid = $("#hid_comid").trimVal();
            var pid = $("#hid_pid").trimVal();
            $.post("/JsonFactory/ProductHandler.ashx?oper=DelPackageid", { id: id, comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("操作出现错误");
                    return;
                }
                if (data.type == 100) {
                    $.prompt("删除成功");
                    location.href = "PackageBindingpro.aspx?id=" + pid;
                    return;
                }
            })
        }


        $(function () {
            var pageSize = 20; //每页显示条数
            var comid = $("#hid_comid").trimVal();
            var pid = $("#hid_pid").trimVal();

            SearchList(1);

            //装载栏目
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=ProPackagelist",
                    data: { comid: comid,pid:pid, pageindex: pageindex, pagesize: pageSize },
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
                 <%=fproname%>  绑定产品管理  </h3>  <span><a href="#" class="a_anniu">添加新绑定</a></span> 
                <table width="780" border="0">
                    <tr>
                        <td width="20">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="150">
                            <p align="left">
                                绑定产品ID
                            </p>
                        </td>
                        <td width="300">
                            <p align="left">
                                绑定产品名称
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                数量
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
                                ${Sid}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                               ${Proname}
                                
                                </p>
                        </td>
                        <td>
                            <p align="left">
                            ${Snum
                                </p>
                        </td>

                        <td>
                            <p align="left">
                            <input type="button"  name="deletebanner" id="deletebanner" onclick="Deletebanner('${Id}')" value="  删除  " /></div>
                            </p>
 
                        </td>
                    </tr>
    </script>
    <script type="text/javascript">

        //弹出二维码大图
        function referrer_ch1(proid, pixsize) {
            $("#hid_id").val(proid);
            referrer_ch2(pixsize, 1);
            $("#proqrcode_rhshow").show();
        };
        //弹出二维码大图
        function referrer_ch2(pixsize, qrcodetype) {

            var id = $("#hid_id").trimVal();
            var comid = $("#hid_comid").trimVal();
            $("#img2").attr("src", "/Images/defaultThumb.png")

            var url = "http://shop" + comid + ".etown.cn/h5/ad?id=" + id;
            $("#img2").attr("src", "/ui/pmui/eticket/showtcode.aspx?pno=" + url);
        };
        $(function () {
            $("#closebtn").click(function () {
                $("#img2").attr("src", "/Images/defaultThumb.png")
                $("#proqrcode_rhshow").hide();
                $("#hid_id").val("0");
            })
        })
    </script>


    <div id="proqrcode_rhshow" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; display: none; left: 20%; position: absolute; top: 20%;">
        <input type="hidden" id="hid_proid" value="" />
        <table width="500px" border="0" cellpadding="10" cellspacing="1" style="margin: 10px 5px;">
            <tr>
                <td align="center">
                    <span style="font-size: 14px;">二维码</span>
                </td> 
            </tr>
            <tr>

                <td align="center">
                    <img src="/Images/defaultThumb.png" id="img2" height="135" width="135" />
                </td> 
            </tr>
            <tr>
                <td colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <label>
                        *二维码尺寸请按照43像素的整数倍缩放，以保持最佳效果</label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input name="cancel_rh" id="closebtn" type="button" class="formButton" value="  关 闭  " />
                </td>
            </tr>
        </table>
    </div>
    <input type="hidden" id="hid_pid" value="<%=id %>" />
</asp:Content>