<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/Etown.Master"
    CodeBehind="AccountInfo.aspx.cs" Inherits="ETS2.WebApp.UI.UserUI.AccountInfo" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            bindViewImg();


            //根据当前用户获得商家信息---需要处理
            $.ajax({
                type: "post",
                url: "/JsonFactory/AccountInfo.ashx?oper=getcurcompany",
                data: { comid: $("#hid_comid").trimVal() },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取信息出错，" + data.msg);
                    }
                    if (data.type == 100) {
                        $("#label_comid").text($("#hid_comid").trimVal());
                        $("#com_name").val(data.msg.Com_name);
                        $("#Scenic_name").val(data.msg.Scenic_name);
                        $("#Contact").val(data.msg.B2bcompanyinfo.Contact);
                        $("#tel").val(data.msg.B2bcompanyinfo.Tel);
                        $("#phone").val(data.msg.B2bcompanyinfo.Phone);
                        $("#qq").val(data.msg.B2bcompanyinfo.Qq);
                        $("#email").val(data.msg.B2bcompanyinfo.Email);

                        $("#com_type").val(data.msg.Com_type);
                        $("#com_city").val(data.msg.B2bcompanyinfo.Com_city);

                        if (data.msg.B2bcompanyinfo.Com_city != "") {
                            $("#com_city").append("<option value='" + data.msg.B2bcompanyinfo.Com_city + "' selected>" + data.msg.B2bcompanyinfo.Com_city + "</option>");
                        }
                        if (data.msg.B2bcompanyinfo.Com_province != "") {
                            $("#com_province option[value='" + data.msg.B2bcompanyinfo.Province + "']").attr("selected", true);
                        }


                        $("#com_add").val(data.msg.B2bcompanyinfo.Com_add);
                        $("#com_class").val(data.msg.B2bcompanyinfo.Com_class);

                        $("#Scenic_address").val(data.msg.B2bcompanyinfo.Scenic_address);
                        $("#Scenic_intro").val(data.msg.B2bcompanyinfo.Scenic_intro);
                        $("#Scenic_Takebus").val(data.msg.B2bcompanyinfo.Scenic_Takebus);
                        $("#Scenic_Drivingcar").val(data.msg.B2bcompanyinfo.Scenic_Drivingcar);
                        $("#Defaultprint").val(data.msg.B2bcompanyinfo.Defaultprint);
                        $("#hid_comextid").val(data.msg.B2bcompanyinfo.Id);
                        $("#hid_com_code").val(data.msg.B2bcompanyinfo.Com_code);
                        $("#hid_com_sitecode").val(data.msg.B2bcompanyinfo.Com_sitecode);
                        $("#hid_com_license").val(data.msg.B2bcompanyinfo.Com_license);
                        $("#hid_sale_Agreement").val(data.msg.B2bcompanyinfo.Sale_Agreement);
                        $("#hid_agent_Agreement").val(data.msg.B2bcompanyinfo.Agent_Agreement);

                        $("#txtServiceInfo").val(data.msg.B2bcompanyinfo.Serviceinfo);
                        $("#txtmerchantintro").val(data.msg.B2bcompanyinfo.Merchant_intro);
                        $("#coordinate").val(data.msg.B2bcompanyinfo.Coordinate);
                        $("#coordinatesize").val(data.msg.B2bcompanyinfo.Coordinatesize);
                        $("#domainname").val(data.msg.B2bcompanyinfo.Domainname);
                        $("#admindomain").val(data.msg.B2bcompanyinfo.Admindomain);

                        $("#wl_PartnerId").val(data.msg.B2bcompanyinfo.wl_PartnerId);
                        $("#wl_userkey").val(data.msg.B2bcompanyinfo.wl_userkey);

                        $("#domain_temp").html("shop" + $("#hid_comid").trimVal() + ".etown.cn")

                        $("#hid_weixinimg").val(data.msg.B2bcompanyinfo.Weixinimg);
                        $("#hid_hasinnerchannel").val(data.msg.B2bcompanyinfo.HasInnerChannel);

                        $("#weixinname").val(data.msg.B2bcompanyinfo.Weixinname);
                    }
                }
            })

            //传参时把商家基本表和商家扩展表的标识列id传递过去
            $("#button").click(function () {
                var com_name = $("#com_name").trimVal();
                if (com_name == '') {
                    $.prompt("公司全称不可为空");
                    return;
                }
                var scenic_name = $("#Scenic_name").trimVal();
                if (scenic_name == '') {
                    $.prompt("品牌/景区名称不可为空");
                    return;
                }

                var com_province = $("#com_province").trimVal();
                var com_city = $("#com_city").trimVal();


                if (com_province == "" || com_province == "省份") {
                    $.prompt("请选择所在省份");
                    return;
                }
                if (com_city == "" || com_city == "城市") {
                    $.prompt("请选择所属城市");
                    return;
                }



                var com_add = $("#com_add").trimVal();
                var com_class = $("#com_class").val();
                var com_type = $("#com_type").val();

                var Scenic_address = $("#Scenic_address").trimVal();
                var Scenic_intro = $("#Scenic_intro").trimVal();

                var Scenic_Takebus = $("#Scenic_Takebus").trimVal();
                var Scenic_Drivingcar = $("#Scenic_Drivingcar").trimVal();
                var Contact = $("#Contact").trimVal();
                if (Contact == '') {
                    $.prompt("联系人姓名不可为空");
                    return;
                }
                var tel = $("#tel").trimVal();
                var phone = $("#phone").trimVal();
                if (tel == '' && phone == '') {
                    $.prompt("联系人电话或者手机必须填写一个");
                    return;
                }
                var qq = $("#qq").trimVal();
                var email = $("#email").trimVal();
                var Defaultprint = $("#Defaultprint").trimVal();
                var weixinimg = $("#<%=UploadFile1.FileUploadId_ClientId %>").val();
                if (weixinimg == "") {
                    weixinimg = $("#hid_weixinimg").trimVal();
                }
                var weixinname = $("#weixinname").val();
                var wl_PartnerId = $("#wl_PartnerId").val();
                var wl_userkey = $("#wl_userkey").val();




                $('#button').hide().after('<span id="spLoginLoading" style="margin-left:105px;color:#f39800; ">操作中……</span>');
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AccountInfo.ashx?oper=editall",
                    data: { hasinnerchannel: $("#hid_hasinnerchannel").trimVal(), wl_userkey: wl_userkey, wl_PartnerId: wl_PartnerId, weixinimg: weixinimg, weixinname: weixinname, merchantintro: $("#txtmerchantintro").trimVal(), serviceinfo: $("#txtServiceInfo").trimVal(), coordinate: $("#coordinate").trimVal(), coordinatesize: $("#coordinatesize").trimVal(), domainname: $("#domainname").trimVal(), admindomain: $("#admindomain").trimVal(), comid: $("#hid_comid").trimVal(), userid: $("#hid_userid").trimVal(), comextid: $("#hid_comextid").trimVal(), com_name: com_name, scenic_name: scenic_name, com_province: com_province, com_city: com_city, com_add: com_add, com_class: com_class, com_type: com_type, scenic_address: Scenic_address, scenic_intro: Scenic_intro, scenic_takebus: Scenic_Takebus, scenic_drivingcar: Scenic_Drivingcar, contact: Contact, tel: tel, phone: phone, qq: qq, email: email, defaultprint: Defaultprint, com_code: $("#hid_com_code").trimVal(), com_sitecode: $("#hid_com_sitecode").trimVal(), com_license: $("#hid_com_license").trimVal(), sale_Agreement: $("#hid_sale_Agreement").trimVal(), agent_Agreement: $("#hid_agent_Agreement").trimVal() },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt("商家基本信息修改出错");
                            $("#spLoginLoading").hide();
                            $('#button').show();
                            return;
                        }
                        if (data.type == 100) {
                            $.prompt("商家基本信息修改完成");
                            //                            window.location.reload();
                            $("#spLoginLoading").hide();
                            $('#button').show();
                            return;
                        }
                    }
                })
            })


            $('#txtServiceInfo').tinymce({
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

            $('#txtmerchantintro').tinymce({
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
        function bindViewImg() {
            var defaultPath = "";

            var imgSrc = '<%=weixinqrcodeurl %>';
            if (imgSrc == "") {
                //                $("#Img1").attr("src", defaultPath);

            } else {
                var filePath = '<%=UploadFile1.fileUrl %>';
                var headlogoImgSrc = filePath + imgSrc;
                $("#Img1").attr("src", headlogoImgSrc);
            }


        }

        function MapPrtDlg() {
            var child = window.open("/baidumap.html", "child", "height=520,width=1000,status=yes,toolbar=no,menubar=no,location=no");
            if (child == undefined) {
                child = window.returnValue;
                $("#coordinate").val(child);
            }  
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
                <li class="on"><a href="/ui/userui/AccountInfo.aspx" target="" title="">商户基本信息</a></li>
                <li><a href="/ui/shangjiaui/DirectSellSetting.aspx" onfocus="this.blur()" target="">
                    页面设置</a></li>
                <li></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                </h3>
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        商家基本信息</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            公司Id</label>
                         <label id="label_comid"></label>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            公司全称</label>
                        <input name="com_name" type="text" id="com_name" size="25" class="mi-input" style="width: 300px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            品牌/景区名称</label>
                        <input name="Scenic_name" type="text" id="Scenic_name" size="25" maxlength="50" class="mi-input"
                            style="width: 300px;" />
                    </div>
                     <div class="mi-form-item">
                        <label class="mi-label">
                            公司微信号</label>
                        <input name="weixinname" type="text" id="weixinname" size="25" maxlength="50" class="mi-input"
                            style="width: 300px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            网站前台域名</label>
                        <input name="domainname" type="text" id="domainname" size="25" maxlength="50" class="mi-input"
                            style="width: 200px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            后台管理域名</label>
                        <input name="admindomain" type="text" id="admindomain" size="25" maxlength="50" class="mi-input"
                            style="width: 200px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            所在省市</label>
                        <select name="com_province" id="com_province" class="mi-input">
                            <option value="省份" selected="selected">省份</option>
                        </select>
                        <select name="com_city" id="com_city" class="mi-input">
                            <option value="城市" selected="selected">市县</option>
                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            实体店/景区地址</label>
                        <input name="Scenic_address" type="text" id="Scenic_address" size="25" maxlength="50"
                            class="mi-input" style="width: 500px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            坐标</label>
                        <input name="coordinate" type="text" id="coordinate" size="25" maxlength="50" readonly="readonly"
                            class="mi-input" style="width: 200px;" /><input class="button" type="button" value=" 查找坐标 "
                                onclick="MapPrtDlg()">
                        <input type="hidden" id="coordinatesize" value="13" />
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>

                

                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        联系人信息</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            联系人姓名</label>
                        <input name="Contact" type="text" id="Contact" size="25" class="mi-input" style="width: 200px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            联系人电话</label>
                        <input name="tel" type="text" id="tel" size="25" class="mi-input" style="width: 300px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            手机</label>
                        <input name="phone" type="text" id="phone" size="25" class="mi-input" style="width: 300px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            Email</label>
                        <input name="email" type="text" id="email" size="25" class="mi-input" style="width: 300px;" />
                        <input name="qq" type="hidden" id="qq" />
                        
                    </div>
                    <div class="mi-form-item" style="display: none">
                        <label class="mi-label">
                            上传微信二维码图片</label>
                        <input type="hidden" id="Hidden2" value="" />
                        <img alt="" class="headPortraitImgSrc" id="Img1" src="/images/defaultThumb.png" />
                        <uc1:uploadFile ID="UploadFile1" runat="server" />
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>

                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        万龙接口</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            PartnerId</label>
                        <input name="Contact" type="text" id="wl_PartnerId" size="25" class="mi-input" style="width: 200px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            userkey</label>
                        <input name="tel" type="text" id="wl_userkey" size="25" class="mi-input" style="width: 300px;" />
                    </div>
                    
                    <div class="mi-form-explain">
                    </div>
                </div>


                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        景区拓展信息</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            品牌/景区简介</label>
                        <input name="Scenic_intro" type="text" id="Scenic_intro" size="25" class="mi-input"
                            style="width: 400px;" />(限80字)
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            服务介绍</label>
                        <textarea name="txtServiceInfo" cols="50" rows="6" id="txtServiceInfo" class="mi-input"
                            style="width: 500px;"></textarea>
                    </div>
                    <div class="mi-form-item" style="display: none">
                        <label class="mi-label">
                            商家介绍</label>
                        <textarea name="txtmerchantintro" cols="50" rows="6" id="txtmerchantintro" class="mi-input"
                            style="width: 500px;"></textarea>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            公交线路</label>
                        <textarea name="Scenic_Takebus" cols="50" rows="6" id="Scenic_Takebus" class="mi-input"
                            style="width: 500px;"></textarea>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            开车线路</label>
                        <textarea name="Scenic_Drivingcar" cols="50" rows="6" id="Scenic_Drivingcar" class="mi-input"
                            style="width: 500px;"></textarea>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <table class="grid" width="780px;">
                    <tr>
                        <td height="80" align="center">
                            <input type="button" name="button" id="button" class="mi-input" value="      确 认 提 交     " />
                            
                           
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </div>
        <input name="com_add" type="hidden" id="com_add" />
         <input type="hidden" id="com_type" value="" />
        <input type="hidden" id="hid_comextid" value="" />
        <input type="hidden" id="hid_com_code" value="" />
        <input type="hidden" id="com_class" value="" />
        <input type="hidden" id="hid_com_sitecode" value="" />
        <input type="hidden" id="hid_com_license" value="" />
        <input type="hidden" id="hid_sale_Agreement" value="" />
        <input type="hidden" id="hid_agent_Agreement" value="" />
        <input type="hidden" id="hid_weixinimg" value="" />
        <input type="hidden" id="hid_hasinnerchannel" value="" />
    </div>
    <div class="data">
    </div>
    <script type="text/javascript">
        var province = document.getElementById('com_province');
        var city = document.getElementById('com_city');
    </script>
    <script src="/Scripts/City.js" type="text/javascript"></script>
</asp:Content>
