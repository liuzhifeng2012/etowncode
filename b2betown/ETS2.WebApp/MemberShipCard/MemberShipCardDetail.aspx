<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberShipCardDetail.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.MemberShipCard.MemberShipCardDetail" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            var comid = $("#hid_comid").val();

            InitMaterial();



            function InitMaterial() {

                var materialid = $("#hid_materialid").trimVal();
                //判断是修改操作还是添加素材操作
                if (materialid != 0) {//修改操作;需要加载素材信息


                    $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetMemberShipCardMaterial", { comid: $("#hid_comid").trimVal(), materialid: materialid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            $("#txttitle").val(data.msg.Title);

                            $("#hid_logo").val(data.msg.Imgpath);
                            $("#txtsummary").val(data.msg.Summary);
                            $("#txtcontent").val(data.msg.Article);
                            $("#txturl").val(data.msg.Articleurl);

                            $("#txtphone").val(data.msg.Phone);
                            $("#txtprice").val(data.msg.Price);
                            $('input[name="radApplyState"][value=' + data.msg.Applystate + ']').attr("checked", true);
                        } else {
                            $.prompt("获取微信素材信息出错");
                            return;
                        }
                    });
                }

            }





            $("#aedit").click(function () {
                var materialid = $("#hid_materialid").trimVal();
                var txttitle = $("#txttitle").trimVal();

                var logo = $("#<%=headPortrait.FileUploadId_ClientId %>").val();
                var txtsummary = $("#txtsummary").trimVal();
                var txtcontent = $("#txtcontent").trimVal();


                var applystate = $('input:radio[name="radApplyState"]:checked').trimVal();

                var txtphone = $("#txtphone").trimVal();
                var txtprice = $("#txtprice").trimVal();

                if (txtphone == "") {
                    $.prompt("电话不可为空");
                    return;
                }
                //                else {
                //                    if (!$("#txtphone").checkMobile() && !$("#txtphone").checkTel()) {
                //                        $.prompt("请正确填写电话号码，例如:13415764179或0321-4816048");
                //                        return;
                //                    }
                //                }



                if (txttitle == "") {
                    $.prompt("请输入素材的标题！");
                    return;
                }
                if (logo == "") {
                    if (logo == "") {
                        logo = $("#hid_logo").trimVal();
                    }
                }
                //                if (txtsummary == "") {
                //                    $.prompt("请输入摘要");
                //                    return;
                //                }
                if (txtcontent == "") {
                    $.prompt("请输入正文");
                    return;
                }
                if (isNaN($("#txtprice").trimVal())) {
                    $.prompt("价格格式不正确");
                    return;
                }
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=EditMemberShipCardMaterial", { price: txtprice, comid: comid, phone: txtphone, materialid: materialid, title: txttitle, imgurl: logo, summary: txtsummary, content: txtcontent, applystate: applystate }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == '100') {
                        $.prompt("编辑会员卡素材信息成功", {
                            buttons: [{ title: '确定', value: true}],
                            opacity: 0.1,
                            focus: 0,
                            show: 'slideDown',
                            submit: function (e, v, m, f) {
                                if (v == true)
                                    location.href = "MemberShipCardList.aspx";
                            }
                        });
                    } else {
                        $.prompt("编辑会员卡素材信息出错");
                        return;
                    }
                });
            })


            bindViewImg();

            function bindViewImg() {
                var defaultPath = "";
                var imgSrc = '<%=headPortraitImgSrc %>';
                if (imgSrc == "") {
                    $(".headPortraitImg").attr("src", defaultPath);
                } else {
                    var filePath = '<%=headPortrait.fileUrl %>';
                    var headlogoImgSrc = filePath + imgSrc;
                    $("#headPortraitImg").attr("src", headlogoImgSrc);
                }
            }
            $('#txtcontent').tinymce({
                // Location of TinyMCE script
                script_url: '/Scripts/tiny_mce/tiny_mce.js',
                width: '422',
                height: '320',
                // General options
                theme: "advanced",
                language: 'cn',
                plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,advlist",

                // Theme options
                theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,fontselect,fontsizeselect,|,forecolor,backcolor,|,insertdate,image,preview",
                theme_advanced_buttons2: "",
                theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,ltr,rtl",
                // theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,|,nonbreaking,template,pagebreak",
                theme_advanced_toolbar_location: "top",
                theme_advanced_toolbar_align: "left",
                theme_advanced_statusbar_location: "bottom",
                //theme_advanced_resizing: true,
                template_external_list_url: "lists/template_list.js",
                external_link_list_url: "lists/link_list.js",
                external_image_list_url: "lists/image_list.js",
                media_external_list_url: "lists/media_list.js",

                // Replace values for the template plugin
                template_replace_values: {
                    username: "Some User",
                    staffid: "991234"
                }
            });
        })
       
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="MemberShipCardList.aspx" onfocus="this.blur()"><span>微信端会员卡素材管理</span></a></li>
               
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    </h3>

                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                               <h2 class="p-title-area">编辑微信端会员卡素材</h2>
                               <div class="mi-form-item">
                                    <label class="mi-label">标题</label>
                                     <input type="text" id="txttitle" value="" size="50"  class="mi-input"/>*
                               </div>
                               <div class="mi-form-item">
                                    <label class="mi-label">封面</label>
                                     <input type="hidden" id="hid_logo" value="" />
                                     <img alt="" class="headPortraitImgSrc" id="headPortraitImg" src="" />
                                     <uc1:uploadFile ID="headPortrait" runat="server" />
                               </div>

                               <div class="mi-form-item">
                                    <label class="mi-label">添加摘要</label>
                                     <textarea id="txtsummary" rows="3" cols="114"></textarea>*
                               </div>

                               <div class="mi-form-item">
                                    <label class="mi-label">正文</label>
                                    <textarea id="txtcontent" rows="15" cols="80"></textarea>*
                               </div>
                               <div class="mi-form-item">
                                    <label class="mi-label">使用状态</label>
                                     <input name="radApplyState" type="radio" value="true" checked>
                                        使用
                                        <input name="radApplyState" type="radio" value="false">
                                        暂停
                               </div>
                               <div class="mi-form-item">
                                    <label class="mi-label">商户电话</label>
                                    <input type="text" id="txtphone" value="" size="40"  class="mi-input"/>*
                               </div>
                               <div class="mi-form-item">
                                    <label class="mi-label">产品价格</label>
                                    <input type="text" id="txtprice" value="" size="40"  class="mi-input"/>*
                               </div>
                 <div class="mi-form-explain"></div>
                </div>



                <table  width="780">
                <tr><td align="center">
                 <p align="center">
                    <a href="javascript:void(0)" id="aedit" class="font_14"><strong>完成添加，确认提交</strong></a></p>
                <p>
                    &nbsp;</p>
                </td></tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_materialid" value="<%=materialid %>" />
</asp:Content>
