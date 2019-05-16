<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ImglibraryManage.aspx.cs" Inherits="ETS2.WebApp.UI.PermissionUI.ImglibraryManage" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {


            var comid = $("#hid_comid").trimVal();
            var id = $("#hid_id").trimVal();
            var usetype = $("#hid_usetype").trimVal();

            bindViewImg();
            $.post("/JsonFactory/ModelHandler.ashx?oper=imageLibraryByid", { comid: comid, id: id, usetype: usetype }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                    if (data.msg != null) {
                        $("#hid_id").val(data.msg.Id);
                        $("#hid_com_id").val(data.msg.Com_id);
                        $("#title").val(data.msg.Title);
                        $("#hid_logo").val(data.msg.Imgurl_address);
                        $("#hid_imgurl").val(data.msg.Imgurl);

                    }
                }
            })


            $("#button").click(function () {
                var imgurl = $("#<%=headPortrait.FileUploadId_ClientId %>").val();
                if (imgurl == "") {
                    imgurl = $("#hid_imgurl").trimVal();
                }
                var modelid = $("#modelid").trimVal();
                $.post("/JsonFactory/ModelHandler.ashx?oper=libraryInsertOrUpdate", { id: $("#hid_id").val(), usetype: usetype, imgurl: imgurl, modelid: modelid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("添加出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("添加成功,继续添加");
                        if ($("#hid_id").val() == 0) {
                            location.href = "ImglibraryManage.aspx?usetype=" + usetype + "&modelid=" + modelid;
                        } else {
                            location.href = "ModelList.aspx";
                        }
                        return;
                    }
                })
            })

            $("#deletebanner").click(function () {
                var modelid = $("#modelid").trimVal();
                $.post("/JsonFactory/ModelHandler.ashx?oper=deleteLibraryimage", { id: $("#hid_id").val(), comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作出现错误");

                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("删除成功");
                        if (usetype == 0) {
                            location.href = "ImglibraryManage.aspx?usetype=" + usetype + "&modelid=" + modelid;
                        } else {
                            location.href = "ModelList.aspx";
                        }
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
        }

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
                <li><a href="Modellist.aspx" onfocus="this.blur()" target=""><span>模板管理</span></a></li>
                
                
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <div>
                </div>
                <h3>
                
                 图片管理
                  
                  </h3>
                <table class="grid">
                  <tr>
                        <td>
                        </td>
                        <td style=" text-align:right;">
                            <%if (id !=0) {%>
                                 <input type="button" name="deletebanner" id="deletebanner" value="  删除此图片  " />
                            <%} %>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            上传图片 ：
                        </td>
                        <td>
                            <div class="C_head">
                                <dl>
                                    <dt>
                                        <input type="hidden" id="hid_imgurl" value="" />
                                        <img alt="" class="headPortraitImgSrc" id="headPortraitImg" src="/images/defaultThumb.png" style=" background-color:#0099ff" /></dt>
                                    <dd>
                上传图片
                 </dd>
                                </dl>
                                <div class="cl">
                                </div>
                            </div>
                            <div class="C_head">
                                选择图片库
                            </div>
                            <div class="C_head_no">
                                <div class="C_head_1">
                                    <ul>
                                        <li style="height: 20px; margin-left: 40px">
                                            <div class="C_verify">

                                                <span>
                                                    <uc1:uploadFile ID="headPortrait" runat="server" />
                                                </span>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </td>
                    </tr>

                </table>
                <table width="340" class="grid">
                    <tr>
                        <td height="80" colspan="2" align="center">
                            <input type="button" name="button" id="button" value="  确认提交  "  class="buttonblue">
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_id" value="<%=id %>" />
    <input type="hidden" id="hid_usetype" value="<%=usetype %>" />
    <input type="hidden" id="hid_imgurl" value="" />
    <input type="hidden" id="modelid" value="<%=modelid %>" />

</asp:Content>
