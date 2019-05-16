<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="DirectSellSetting.aspx.cs"
    Inherits="ETS2.WebApp.UI.ShangJiaUI.DirectSellSetting" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {


            var comid = $("#hid_comid").trimVal();
            var userid = $("#hid_userid").trimVal();

            bindViewImg();
            $.post("/JsonFactory/DirectSellHandler.ashx?oper=getdirectsellset", { comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取直销信息出错");
                    return;
                }
                if (data.type == 100) {
                    if (data.msg != null) {
                        $("#hid_id").val(data.msg.Id);
                        $("#hid_com_id").val(data.msg.Com_id);
                        $("#hid_payto").val(data.msg.Payto);
                        $("#hid_model_style").val(data.msg.Model_style);
                        $("#hid_title").val(data.msg.Title);
                        $("#hid_Service_Phone").val(data.msg.Service_Phone);
                        $("#hid_WorkingHours").val(data.msg.WorkingHours);
                        $("#hid_Copyright").val(data.msg.Copyright);
                        $("#hid_tophtml").val(data.msg.Tophtml);
                        $("#hid_bottomHtml").val(data.msg.BottomHtml);
                        $("#hid_dealuserid").val(data.msg.Dealuserid);
                        $("#txttitle").val(data.msg.Title);
                        $("#txtkefu").val(data.msg.Service_Phone);
                        $("#txtbuttom").val(data.msg.Copyright);
                        $("#smsaccount").val(data.msg.Smsaccount);
                        $("#smspass").val(data.msg.Smspass);
                        $("#smssign").val(data.msg.Smssign);

                        $("#hid_logo").val(data.msg.Logo);
                        $("#hid_smalllogo").val(data.msg.Smalllogo);
                        $("#hid_compressionlogo").val(data.msg.Compressionlogo);

                    }
                }
            })

            $("#button").click(function () {

                var logo = $("#<%=headPortrait.FileUploadId_ClientId %>").val();
                if (logo == "") {
                    logo = $("#hid_logo").trimVal();
                }
                var smalllogo = $("#<%=SmallHeadPortrait.FileUploadId_ClientId %>").val();
                if (smalllogo == "") {
                    smalllogo = $("#hid_smalllogo").trimVal();
                }


                //                var title = $("#txttitle").trimVal();
                var kefu = $("#txtkefu").trimVal();
                var smsaccount = $("#smsaccount").trimVal();
                var smspass = $("#smspass").trimVal();
                var smssign = $("#smssign").trimVal();



                var bottom = $("#txtbuttom").trimVal();

                //                if (title == "") {
                //                    $.prompt("title不可为空");
                //                    return;
                //                }
                if (kefu == "") {
                    $.prompt("客服电话不可为空");
                    return;
                }
                if (bottom == "") {
                    $.prompt("底部版权信息不可为空");
                    return;
                }

                $.post("/JsonFactory/DirectSellHandler.ashx?oper=editcdirectset", { smalllogo: smalllogo, directsellsetid: $("#hid_id").val(), comid: comid, userid: userid, logo: logo, title: "", kefu: kefu, bottom: bottom, smsaccount: smsaccount, smspass: smspass, smssign: smssign }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("设置出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("设置成功");
                        return;
                    }
                })
            })

        })

        function bindViewImg() {
            var defaultPath = "";
            //大logo
            var imgSrc = '<%=headPortraitImgSrc %>';
            if (imgSrc == "") {
//                $("#headPortraitImg").attr("src", defaultPath);
                
            } else {
                var filePath = '<%=headPortrait.fileUrl %>';
                var headlogoImgSrc = filePath + imgSrc;
                $("#headPortraitImg").attr("src", headlogoImgSrc);
            }

            //小logo
            var smallimgSrc = '<%=smallheadPortraitImgSrc %>';
            if (smallimgSrc == "") {
//                $("#SmallHeadPortraitImg").attr("src", defaultPath);
                
            } else {
                var filePath = '<%=SmallHeadPortrait.fileUrl %>';
                var smallheadlogoImgSrc = filePath + smallimgSrc;
                $("#SmallHeadPortraitImg").attr("src", smallheadlogoImgSrc);
            }
        }

        function viewbookpage() {
            var bookurl = "http://shop" + $("#hid_comid").trimVal() + ".etown.cn";
            window.open(bookurl, target = "_blank");
        }
        function opensite() {
            window.open("http://shop" + $("#hid_comid").trimVal() + ".etown.cn/ui/shangjiaui/ProductList.aspx", target = "_blank");
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/userui/AccountInfo.aspx" target="" title="">商家基本信息</a></li>
                <li class="on"><a href="/ui/shangjiaui/DirectSellSetting.aspx" onfocus="this.blur()" target="">展示页面设置</a></li>
                                 <li><a href="javascript:;" onfocus="this.blur()" target="" onclick="opensite()">
                    页面预览</a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">

                <h3></h3>

               <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                   <h2 class="p-title-area">页面Logo</h2>
                   <div class="mi-form-item">
                        <label class="mi-label"> LOGO</label>
                        <input type="hidden" id="Hidden1" value="" />
                        <img alt="" class="headPortraitImgSrc" id="headPortraitImg" src="/images/defaultThumb.png" />
                          <uc1:uploadFile ID="headPortrait" runat="server" />
                   </div>
                   <div class="mi-form-explain"></div>
               </div>

               <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                   <h2 class="p-title-area">手机页面Logo</h2>
                   <div class="mi-form-item">
                        <label class="mi-label"> 小LOGO</label>
                        <input type="hidden" id="Hidden2" value="" />
                        <img alt="" class="headPortraitImgSrc" id="SmallHeadPortraitImg" src="/images/defaultThumb.png" /></dt>
                         <uc1:uploadFile ID="SmallHeadPortrait" runat="server" />
                   </div>
                   <div class="mi-form-explain"></div>
               </div>


               <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                   <h2 class="p-title-area">电话及底部版权设置</h2>
                   <div class="mi-form-item">
                        <label class="mi-label"> 客服电话</label>
                        <input name="text" type="text" id="txtkefu" size="30" value=""  class="mi-input">
                   </div>
                    <div class="mi-form-item">
                        <label class="mi-label"> 页面底部说明</label>
                         <textarea name="textarea" cols="60" rows="5" id="txtbuttom"  class="mi-input"  style="width:500px;"></textarea>
                         <input name="smsaccount" type="hidden" id="smsaccount" size="30" value="">
                            <input name="smspass" type="hidden" id="smspass" size="30" value="">
                            <input name="smssign" type="hidden" id="smssign" size="30" value="">
                   </div>
                   <div class="mi-form-explain"></div>
               </div>
                    
                <table width="780" class="grid">
                    <tr>
                        <td height="80" colspan="2" align="center">
                            <input type="button" name="button" id="button" value="  确认提交  "   class="mi-input">
                            &nbsp;&nbsp;
                            <%-- <input type="button" name="view" id="view" value="  预  览  ">--%>
                           <%-- <a href="#"   onclick="viewbookpage()">点击预览客户预订页面</a>--%>
                             <input type="button" name="button" id="button1" class="mi-input" value="      网 站 预 览     "  onclick="viewbookpage()" />
                        </td>
                    </tr>
                    <%--  <tr>
                        <td height="90">
                            编辑器上传我的头部及底部HTML
                        </td>
                        <td>
                            头部HTML代码：
                            <textarea name="textfield5" cols="60" rows="5" id="textfield5"></textarea>
                            <br>
                            <br>
                            底部版权HTML：
                            <textarea name="textfield6" cols="60" rows="5" id="textfield6"></textarea>
                        </td>
                    </tr>--%>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_id" value="" />
    <input type="hidden" id="hid_com_id" value="" />
    <input type="hidden" id="hid_payto" value="" />
    <input type="hidden" id="hid_model_style" value="" />
    <input type="hidden" id="hid_title" value="" />
    <input type="hidden" id="hid_Service_Phone" value="" />
    <input type="hidden" id="hid_WorkingHours" value="" />
    <input type="hidden" id="hid_Copyright" value="" />
    <input type="hidden" id="hid_tophtml" value="" />
    <input type="hidden" id="hid_bottomHtml" value="" />
    <input type="hidden" id="hid_dealuserid" value="" />
    <input type="hidden" id="hid_logo" value="" />
    <input type="hidden" id="hid_smalllogo" value="" />
    <input type="hidden" id="hid_compressionlogo" value="" />
</asp:Content>
