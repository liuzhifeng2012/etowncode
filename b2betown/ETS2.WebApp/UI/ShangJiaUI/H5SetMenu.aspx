<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="H5SetMenu.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.H5SetMenu" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
        <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Deletebanner(id) {
            var comid = $("#hid_comid").trimVal();
            $.post("/JsonFactory/DirectSellHandler.ashx?oper=deletemenu", { id: id, comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("操作出现错误");
                    return;
                }
                if (data.type == 100) {
                    $.prompt("删除成功");
                    location.href = "H5SetMenu.aspx";
                    return;
                }
            })
        }


        $(function () {
            var pageSize = 100; //每页显示条数
            var comid = $("#hid_comid").trimVal();

            //装在设定
            $.post("/JsonFactory/ModelHandler.ashx?oper=getComModel", { comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取模板出错请重新刷新");
                    return;
                }
                if (data.type == 100) {
                    if (data.msg != "err") {//已经使用模板 
                        $("#oldset").show();

                    } else { //未使用么模板
                        $("#seting").show();
                    }
                }
            })


            SearchList(1);

            //装载Banner列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/DirectSellHandler.ashx?oper=getmenulist",
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


            $("#setmodel").click(function () {

                location.href = "H5SetStep.aspx";

            })

            $("#sethtml").click(function () {

                //location.href = "H5SetHtml.aspx";
                location.href = "H5SetStep.aspx";

            })

            $("#viewsite").click(function () {

                var h = 680;
                var w = 430;
                var t = screen.availHeight / 2 - h / 2;
                var l = screen.availWidth / 2 - w / 2;
                var prop = "dialogHeight:" + h + "px; dialogWidth:" + w + "px; dialogLeft:" + l + "px; dialogTop:" + t + "px;toolbar:no; menubar:no; scrollbars:yes; resizable:no;location:no;status:no;help:no";
                var path = "http://shop" + comid + ".etown.cn/h5/";
                var ret = window.showModalDialog(path, "", prop);

            })

           

        })


       
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/shangjiaui/H5Default.aspx" onfocus="this.blur()" target="">模板设置</a></li>
                 <li class="on"><a href="/ui/shangjiaui/H5SetMenu.aspx" onfocus="this.blur()" target="">栏目管理</a></li>
                <li><a href="/ui/shangjiaui/StoreList.aspx" onfocus="this.blur()" target="">门店模板设置</a></li>
                <li><a href="/ui/shangjiaui/consultant_pro.aspx" onfocus="this.blur()" target="">员工页面设置</a></li>
                 <li><a href="/ui/shangjiaui/ShopManage.aspx" onfocus="this.blur()" target="">微商城设置</a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                   <h3>
                  模板设定</h3>
                <table >
                  <tr>
                        <td>
                            <div id="seting" style="display:none ">
                                您尚未设置微网站
                                <input type="button" name="setmodel" id="setmodel" value="  立即开始设置 " />
                           </div>
                           <div id="oldset" style="display:none float:left">
                                <input type="button" name="viewsite" id="viewsite" value="  预览微网站 " />
                           </div>
                        </td>
                    </tr>
                </table>

                <h3>
                栏目管理  </h3> <span><a href="H5SetMenu_Manage.aspx" class="a_anniu">添加栏目</a></span>  <span><a href="H5MenuSort.aspx"  class="a_anniu">栏目排序</a> </span>
                <table width="780" border="0">
                    <tr>
                        <td width="20">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="100">
                            <p align="left">
                                栏目名称
                            </p>
                        </td>
                        <td width="60">
                            <p align="left">
                                栏目图片
                            </p>
                        </td>
                        <td width="250">
                            <p align="left">
                                连接地址
                            </p>
                        </td>
                        <td width="130" > 
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
                                ${Name}
                            </p>
                        </td>
                        <td  > 
                            <p align="left" >
                                <%if (Daohangimg==1){ %>
                                <img alt="" class="headPortraitImgSrc" id="Img1" src="${Imgurl_address}" height="80px" style=" background-color:#0099ff"/>
                                <%}else{ %>
                                <span id="fonttab" style="font-size:30px;" class="${Fonticon}"> </span>
                                <%} %>
                                </p>

                        </td>
                        <td>
                            <p align="left">
                                ${Linkurl}</p>
                        </td>
                        <td>
                            <p align="left">
                           <div style=" float:left;"><a href="H5SetMenu_manage.aspx?id=${Id}"  class="a_anniu"> 管理 </a></div><div style=" float:left;padding-left:10px;">  <input type="button"  name="deletebanner" id="deletebanner" onclick="Deletebanner('${Id}')" value="  删除此栏目  " /></div>
                            </p>
 
                        </td>
                    </tr>
    </script>

</asp:Content>
