<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ModelManage.aspx.cs" Inherits="ETS2.WebApp.UI.PermissionUI.ModelManage" %>


<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            bindViewImg();


            //首先加载数据
            var hid_modelid = $("#hid_modelid").trimVal();
            if (hid_modelid != '0') {
                $.post("/JsonFactory/ModelHandler.ashx?oper=getModelById", { modelid: hid_modelid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#title").val(data.msg.Title);
                        $("#daohangnum").val(data.msg.Daohangnum);
                        $("#style_str").val(data.msg.Style_str);
                        $("#html_str").val(data.msg.Html_str);
                        $("#hid_imgurl").val(data.msg.Bgimage);
                        $("#bgimage_w").val(data.msg.Bgimage_w);
                        $("#bgimage_h").val(data.msg.Bgimage_h);
                        $("#hid_smallimgurl").val(data.msg.Smallimg);

                        if (data.msg.Daohangimg == 1) {
                            $("[name = daohangimg]:checkbox").attr("checked", true);
                        }
//                        if (data.msg.Bgimage == 1) {
//                            //$("[name = bgimage]:checkbox").attr("checked", true);
//                        }


                    }

                })
            }



            //确认发布按钮
            $("#GoBotton").click(function () {
                var imgurl = $("#<%=headPortrait.FileUploadId_ClientId %>").val();

                if (imgurl == "") {
                    imgurl = $("#hid_imgurl").trimVal();
                }
                var smallimg = $("#<%=SmallHeadPortrait.FileUploadId_ClientId %>").val();
                if (smallimg == "") {
                    smallimg = $("#hid_smallimgurl").trimVal();
                }


                var title = $("#title").trimVal();
                var daohangnum = $("#daohangnum").trimVal();
                var style_str = $("#style_str").trimVal();
                var html_str = $("#html_str").trimVal();

                var bgimage_w = $("#bgimage_w").trimVal();
                var bgimage_h = $("#bgimage_h").trimVal();
                var daohangimg = 0;


                if ($("#daohangimg").is(':checked') == true) {
                    daohangimg = 1;
                }
                
//                if ($("#bgimage").is(':checked') == true) {
//                    bgimage = 1;
//                }

                if (title == '') {
                    $.prompt('请填写模板名称');
                    return;
                }

                if (style_str == '') {
                    $.prompt('请填写模板样式！');
                    return;
                }
                if (html_str == '') {
                    $.prompt('请填写模板页面！');
                    return;
                }

                if (daohangnum == '') {
                    $.prompt('请填写菜单数量！');
                    return;
                }


                $.post("/JsonFactory/ModelHandler.ashx?oper=ModelInsertOrUpdate", { modelid: hid_modelid, title: title, daohangnum: daohangnum, style_str: style_str, html_str: html_str, daohangimg: daohangimg, bgimage: imgurl, smallimg: smallimg, bgimage_w: bgimage_w, bgimage_h: bgimage_h }, function (data) {
                    data = eval('(' + data + ')');
                    if (data.type == '100') {
                        $.prompt("模板发布成功", {
                            buttons: [
                                 { title: '确定', value: true }
                                ],
                            opacity: 0.1,
                            focus: 0,
                            show: 'slideDown',
                            submit: function (e, v, m, f) {
                                if (v == true) {
                                    location.href = "Modellist.aspx";
                                }
                            }
                        });
                        return;
                    } else {
                        $.prompt("模板信息添加出错");
                        return;
                    }
                })

            })



        })

        function bindViewImg() {
            var defaultPath = "";
            var imgSrc = '<%=headPortraitImgSrc %>';
            if (imgSrc == "") {
                //                $(".headPortraitImg").attr("src", defaultPath);
            } else {
                var filePath = '<%=headPortrait.fileUrl %>';
                var headlogoImgSrc = filePath + imgSrc;
                $("#headPortraitImg").attr("src", headlogoImgSrc);
            }

            //缩略图
            var smallimgSrc = '<%=headSmalltraitImgSrc %>';
            if (smallimgSrc == "") {
                //                $("#SmallHeadPortraitImg").attr("src", defaultPath);

            } else {
                var filePath = '<%=SmallHeadPortrait.fileUrl %>';
                var smallheadlogoImgSrc = filePath + smallimgSrc;
                $("#SmallHeadPortraitImg").attr("src", smallheadlogoImgSrc);
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
                <!--<li><a href="bankmanager.aspx" onfocus="this.blur()"><span>绑定银行管理</span></a></li>-->
                <li class="on"><a href="ModelList.aspx" onfocus="this.blur()" target=""><span>模板管理</span></a></li>

            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    模板管理
                </h3>
                <table class="grid">
                    <tr>
                        <td class="tdHead">
                            模板名称：
                        </td>
                        <td>
                            <input name="title" type="text" id="title" value="" size="80" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            菜单数量：
                        </td>
                        <td>
                            <input name="daohangnum" type="text" id="daohangnum" value="" size="5" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            菜单图标类型：
                        </td>
                        <td>
                            <label><input name="daohangimg" type="radio" id="daohangimg" value="1" />图片图标(可自行上传图片)</label>
                            <label><input name="daohangimg" type="radio" id="daohangimg2" value="0" checked />字体图标</label>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td class="tdHead">
                            默认Banner图片：
                        </td>
                        <td>
                        <div class="C_head">
                                <dl>
                                    <dt>
                                        <input type="hidden" id="b1" value="" />
                                        <img alt="" class="headPortraitImgSrc" id="headPortraitImg" src="/images/defaultThumb.png" /></dt>
                                    <dd>
                                        上传图片格式为jpg,gif,png，为保证手机浏览正常，建议图片大小在100k以内</dd>
                                </dl>
                                <div class="cl">
                                </div>
                            </div>
                            <div class="C_head_no">
                                <div class="C_head_1">
                                    <ul>
                                        <li style="height: 20px; margin-left: 40px">
                                            <div class="C_verify">
                                                <label>
                                                    Banner图片：</label>
                                                <span>
                                                    <uc1:uploadFile ID="headPortrait" runat="server" />
                                                </span>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <label>
                        </td>
                    </tr>
                    <tr >
                        <td class="tdHead">
                           Banner图片建议尺寸：
                        </td>
                         <td>
                            长<input name="bgimage_h" type="text" id="bgimage_h" value="" size="5" />px 宽<input name="bgimage_w" type="text" id="bgimage_w" value="" size="5" />px
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            缩略图：
                        </td>
                        <td>
                           <div class="C_head">
                                <dl>
                                    <dt>
                                        <input type="hidden" id="b2" value="" />
                                        <img alt="" class="headPortraitImgSrc" id="SmallHeadPortraitImg" src="/images/defaultThumb.png" /></dt>
                                    <dd>
                                        Logo格式为jpg、gif，小logo</dd>
                                </dl>
                                <div class="cl">
                                </div>
                            </div>
                            <div class="C_head_no">
                                <div class="C_head_1">
                                    <ul>
                                        <li style="height: 20px; margin-left: 40px">
                                            <div class="C_verify">
                                                <label>
                                                    缩略图：</label>
                                                <span>
                                                    <uc1:uploadFile ID="SmallHeadPortrait" runat="server" />
                                                </span>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </td>
                    </tr>


                    <tr>
                        <td class="tdHead">
                            样式代码(CSS)：
                        </td>
                        <td>
                           <textarea name="style_str" cols="80" rows="6" id="style_str"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            页面代码(HTML)：
                        </td>
                        <td>
                           <textarea name="html_str" cols="80" rows="6" id="html_str"></textarea>
                        </td>
                    </tr>
                    
                </table>
                <table border="0">
                    <tr>
                        <td width="600" height="80" align="center">

                            <input type="button" name="GoBotton" id="GoBotton" value="  确认发布该模板  " />
                        </td>
                    </tr>
                </table>
                
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_modelid" value="<%=modelid %>" />
    <input type="hidden" id="hid_imgurl" value="<%=headPortraitImgSrc %>" />
    <input type="hidden" id="hid_smallimgurl" value="<%=headSmalltraitImgSrc %>" />
</asp:Content>
