<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="H5Default.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.H5Default" %>

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

            $("#iframurl").attr("src", "/h5/manage/?comid=" +comid );
            $("#erweima").html("微站二维码地址：扫描就可以进入<br><br><img src='../PMUI/ETicket/showtcode.aspx?pno=http://shop" + comid + ".etown.cn/h5/'  width='200px;'/>");
            //加载设定
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
                        $.prompt("您尚未设定微站点模板，请先选择模板！");
                        location.href = "H5SetStep.aspx";

                    }
                }
            })

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
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
               <li class="on"><a href="/ui/shangjiaui/H5Default.aspx" onfocus="this.blur()" target="">模版设置</a></li>
               <li><a href="/ui/shangjiaui/H5SetMenu.aspx" onfocus="this.blur()" target="">栏目管理</a></li>
                <li><a href="/ui/shangjiaui/StoreList.aspx" onfocus="this.blur()" target="">门店模版设置</a></li>
                <li><a href="/ui/shangjiaui/consultant_pro.aspx" onfocus="this.blur()" target="">员工页面设置</a></li>
                <li><a href="/ui/shangjiaui/ShopManage.aspx" onfocus="this.blur()" target="">微商城设置</a></li>
            </ul>
        </div>--%>
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
                           <div id="oldset" style="display:none">
                              <input type="button" name="sethtml" id="sethtml" value=" 更换模板 " />
                              <input type="button" name="viewsite" id="viewsite" value="  预览微网站 " />
                           </div>
                        </td>
                    </tr>
                </table>


                <br/>
                <div><p><span>
                  微网站管理,请点击进入相应的栏目进行设置 </span> </p>
                  <p> <a href="H5SetMenu_Manage.aspx" class="a_anniu">新增栏目</a>  <a href="H5Setting.aspx" class="a_anniu">新增背景图片</a></p>
                </div>
                  <iframe id="iframurl" width="320" height="500" style=" float:left;" marginheight="0" marginwidth="0" frameborder="0" scrolling="no"> </iframe>
                  <div style=" float:left; width:300px; padding-left:20px;" id="erweima"></div>
             </div>
         </div>
                  
    </div>


</asp:Content>
