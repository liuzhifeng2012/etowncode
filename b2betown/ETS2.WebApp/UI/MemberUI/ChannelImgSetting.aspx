<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ChannelImgSetting.aspx.cs" Inherits="ETS2.WebApp.UI.MemberUI.ChannelImgSetting" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {


            var comid = $("#hid_comid").trimVal();
            var id = $("#hid_id").trimVal();
            var channelcompanyid = $("#hid_channelcompanyid").trimVal();
            var typeid = $("#hid_typeid").trimVal();

            bindViewImg();
            $.post("/JsonFactory/DirectSellHandler.ashx?oper=getchannelimagebyid", { comid: comid, id: id, typeid: typeid, channelcompanyid: channelcompanyid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取图片出错");
                    return;
                }
                if (data.type == 100) {
                    if (data.msg != null) {
                        $("#hid_id").val(data.msg.Id);
                        $("#hid_com_id").val(data.msg.Com_id);
                        $("#title").val(data.msg.Title);
                        $("#hid_logo").val(data.msg.Imgurl_address);
                    }
                }
            })

            $("#button").click(function () {
                var imgurl = $("#<%=headPortrait.FileUploadId_ClientId %>").val();
                if (imgurl == "") {
                    imgurl = $("#hid_imgurl").trimVal();
                }
                var title = $("#title").trimVal();
                if (title == "") {
                    $.prompt("标题不可为空");
                    return;
                }
                var linkurl = $("#linkurl").trimVal();
                if (title == "") {
                    $.prompt("连接地址不可为空");
                    return;
                }

                $.post("/JsonFactory/DirectSellHandler.ashx?oper=editchannelimage", { id: $("#hid_id").val(), typeid: typeid, comid: comid, imgurl: imgurl, title: title, linkurl: linkurl,channelcompanyid:channelcompanyid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("添加出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("添加成功");

                        location.href = "ChannelImgList.aspx?channelcompanyid=" + channelcompanyid;
                        return;
                    }
                })
            })

            $("#deletebanner").click(function () {
                $.post("/JsonFactory/DirectSellHandler.ashx?oper=deletechannelimage", { id: $("#hid_id").val(), comid: comid,channelcompanyid:channelcompanyid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作出现错误");

                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("删除成功");

                        location.href = "ChannelImgList.aspx?channelcompanyid=" + channelcompanyid;
                       
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
                                <li id="addinnerchanneltongji"><a href="Channelstatistics.aspx?channelcompanytype=inner"
                    onfocus="this.blur()">所属门店 </a></li></ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <div>
                </div>
                <h3>
                
                  门店图片管理

                  </h3>
                <table class="grid">
                  <tr>
                        <td>
                           
                        </td>
                        <td style=" text-align:right;">
                            <%if (id !=0) {%>
                                 <input type="button" name="deletebanner" id="deletebanner" value="  删除此Banner图片  " />
                            <%} %>
                        </td>
                    </tr>
                                    <tr>
                        <td>
                            标题：
                        </td>
                        <td>
                            <input name="title" type="text" id="title" size="30" value="<%=Title %>" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            连接地址：
                        </td>
                        <td>
                            <input name="linkurl" type="text" id="linkurl" size="50" value="<%=linkurl %>" />
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
                                        <input type="hidden" id="Hidden1" value="" />
                                        <img alt="" class="headPortraitImgSrc" id="headPortraitImg" src="/images/defaultThumb.png" /></dt>
                                    <dd>
                                       
                  图片尺寸 长1010*宽640 小于1M(图片过大会影响打开速度)
                  </dd>
                                </dl>
                                <div class="cl">
                                </div>
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
    <input type="hidden" id="hid_typeid" value="<%=typeid %>" />
    <input type="hidden" id="hid_imgurl" value="" />
    <input type="hidden" id="hid_channelcompanyid" value="<%=channelcompanyid %>" />
</asp:Content>
