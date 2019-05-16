<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ChannelCompanyEdit.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.ChannelCompanyEdit" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#hid_companystate").val(1);
            //判断公司是否含有添加内部渠道单位(所属门市)的权限:和平台总账户商户管理中是否含有所属门市挂钩
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetCompanyInfo", { comid: $("#hid_comid").val() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("判断商家是否含有所属门市出错");
                    return;
                }
                if (data.type == 100) {
                    if (data.msg.HasInnerChannel == true) {
                        $("#addinnerchannelcompany").show();

                        $("#addinnerchanneltongji").show();
                    }
                    else {
                        $("#addinnerchannelcompany").hide();

                        $("#addinnerchanneltongji").hide();
                    }
                }
            })

            var channelcompanyid = $("#hid_channelcompanyid").val();
            var channeltype = $("#hid_channeltype").val();
            if (channelcompanyid != "0")//修改操作，加载数据
            {
                $.post("/JsonFactory/ChannelHandler.ashx?oper=GetChannelCompany", { channelcompanyid: channelcompanyid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert("获取渠道公司出现意外:" + data.msg);
                    }
                    if (data.type == 100) {
                        if (data.msg == "") {
                            alert("渠道公司不存在");
                        } else {
                            $("#hid_channelcompanyid").val(data.msg[0].Id);
                            $("#txtchannelcompanyname").val(data.msg[0].Companyname);
                            $("#txtcompanyaddress").val(data.msg[0].Companyaddress);
                            $("#txtcompanyphone").val(data.msg[0].Companyphone);
                            $("#coordinate").val(data.msg[0].CompanyLocate);
                            
                            $("#txtbookurl").val(data.msg[0].Bookurl);

                            if (data.msg[0].Issuetype == "0") {
                                viewhtml("inner");
                                $("#hid_channeltype").val("inner");
                            } else {
                                viewhtml("out");
                                $("#hid_channeltype").val("out");
                            }

                            $("#hid_companyimg").val(data.msg[0].CompanyimgID);
                            $("#Img1").attr("src", data.msg[0].CompanyImgUrl);
                            $("#txtcompanyintro").val(data.msg[0].Companyintro);
                            $("#txtcompanyproject").val(data.msg[0].Companyproject);

                            $("#whetherdepartment").val(data.msg[0].WhetherDepartment);
                            $("#selcompanystate").val(data.msg[0].CompanyState);
                            $("#Outshop").val(data.msg[0].Outshop)
                            if (data.msg[0].City != "") {
                                $("#city").append("<option value='" + data.msg[0].City + "' selected>" + data.msg[0].City + "</option>");
                            }
                            if (data.msg[0].Province != "") {
                                $("#province option[value='" + data.msg[0].Province + "']").attr("selected", true);
                            }

                        }
                    }
                })
            } else {
                if (channeltype == "inner") {
                    viewhtml("inner");
                    $("#hid_channeltype").val("inner");
                } else {
                    viewhtml("out");
                    $("#hid_channeltype").val("out");
                }
            }
            function viewhtml(type) {
                if (type == "inner") {
                    $("#addinnerchannelcompany").addClass("on").siblings().removeClass("on");

                    $("#lblchanneltype").html("所属门市单位");
                    $("#td1").html("门市名称");
                    $("#td2").html("门市地址");
                    $("#td3").html("门市电话");
                    $("#td4").html("门市地址坐标");
                    $("#td5").html("门市图片");
                    $("#td6").html("门市介绍");
                    $("#td7").html("门市项目");
                    $("#td10").html("门市预订页面网址");

                    $("#trbookurl").hide();
                    $("#trrrr").show();

                } else {
                    $("#addoutchannelcompany").addClass("on").siblings().removeClass("on");

                    $("#lblchanneltype").html("合作公司单位");
                    $("#td1").html("公司名称");
                    $("#td2").html("公司地址");
                    $("#td3").html("公司电话");
                    $("#td4").html("公司地址坐标");

                    $("#td5").html("公司图片");
                    $("#td6").html("公司介绍");
                    $("#td7").html("公司项目");
                    $("#td10").html("公司预订页面网址");

                    $("#trbookurl").show();
                    $("#trrrr").hide();
                }
            }

            $("#btnd").click(function () {
                var channelcompanyname = $("#txtchannelcompanyname").val();
                var channeltype = $("#hid_channeltype").val();
                var companyaddress = $("#txtcompanyaddress").val();
                var companyphone = $("#txtcompanyphone").val();
                var companycoordinate = 0;
                var companyLocate = $("#coordinate").val();
                var companyimg = $("#<%=UploadFile1.FileUploadId_ClientId %>").val();
                if (companyimg == "") {
                    companyimg = $("#hid_companyimg").trimVal();
                }

                var companyintro = $("#txtcompanyintro").val();
                // var companyproject = $("#txtcompanyproject").val();

                var companyproject = "";
                if (channeltype == "inner") {
                    channeltype = "0";
                } else {
                    channeltype = "1";
                }
                var comid = $("#hid_comid").val();
                var channelcompanyid = $("#hid_channelcompanyid").val();
                var city = $("#city").val();
                var province = $("#province").val();
                var outshop = $("#Outshop").val();


                $.post("/JsonFactory/ChannelHandler.ashx?oper=editchannelcompany", { bookurl: $("#txtbookurl").val(), companystate: $("#selcompanystate").val(), whetherdepartment: $("#whetherdepartment").val(), channelcompanyid: channelcompanyid, companyname: channelcompanyname, Issuetype: channeltype, comid: comid, companyaddress: companyaddress, companyphone: companyphone, companycoordinate: companycoordinate, companyLocate: companyLocate, companyimg: companyimg, companyintro: companyintro, companyproject: companyproject, city: city, province: province, outshop: outshop }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {

                    }
                    if (data.type == 100) {
                        $.prompt("操作成功", { buttons: [{ title: "确定", value: true}], submit: function (m, v, e, f) { if (v == true) { location.href = "Channelstatistics.aspx?channelcompanytype=" + $("#hid_channeltype").val(); } } })
                    }
                })


            })
            //公司简介
            $('#txtcompanyintro').tinymce({
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
                //                plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,advlist",

                //                // Theme options
                //                theme_advanced_buttons1: "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect",
                //                theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
                //                theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
                //                theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak",
                //                theme_advanced_toolbar_location: "top",
                //                theme_advanced_toolbar_align: "left",
                //                theme_advanced_statusbar_location: "bottom",
                //                //theme_advanced_resizing: true,
                //                template_external_list_url: "lists/template_list.js",
                //                external_link_list_url: "lists/link_list.js",
                //                external_image_list_url: "lists/image_list.js",
                //                media_external_list_url: "lists/media_list.js",

                // Replace values for the template plugin
                template_replace_values: {
                    username: "Some User",
                    staffid: "991234"
                }
            });
            //            //项目简介
            //            $('#txtcompanyproject').tinymce({
            //                // Location of TinyMCE script
            //                script_url: '/Scripts/tiny_mce/tiny_mce.js',
            //                width: '422',
            //                height: '320',
            //                // General options
            //                theme: "advanced",
            //                language: 'cn',
            //                plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,advlist",

            //                // Theme options
            //                theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,fontselect,fontsizeselect,|,forecolor,backcolor,|,insertdate,image,preview",
            //                theme_advanced_buttons2: "",
            //                theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,ltr,rtl",
            //                // theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,|,nonbreaking,template,pagebreak",
            //                theme_advanced_toolbar_location: "top",
            //                theme_advanced_toolbar_align: "left",
            //                theme_advanced_statusbar_location: "bottom",
            //                //theme_advanced_resizing: true,
            //                template_external_list_url: "lists/template_list.js",
            //                external_link_list_url: "lists/link_list.js",
            //                external_image_list_url: "lists/image_list.js",
            //                media_external_list_url: "lists/media_list.js",

            //                // Replace values for the template plugin
            //                template_replace_values: {
            //                    username: "Some User",
            //                    staffid: "991234"
            //                }
            //            });
        })


        //调取地图
        function MapPrtDlg() {
            var child = window.open("/baidumap.html", "child", "height=520,width=1000,status=yes,toolbar=no,menubar=no,location=no");
            if (child == undefined) {
                child = window.returnValue;
                $("#coordinate").val(child);
            }  
        }

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <%if (IsParentCompanyUser)
                  { %>
                <li id="addoutchanneltongji"><a href="Channelstatistics.aspx?channelcompanytype=out"
                    onfocus="this.blur()">合作商户</a></li>
                <li id="addoutchannelcompany"><a href="ChannelCompanyEdit.aspx?channeltype=out" onfocus="this.blur()">
                    添加合作商户</a></li>
                <li id="addinnerchanneltongji"><a href="Channelstatistics.aspx?channelcompanytype=inner"
                    onfocus="this.blur()">所属门店 </a></li>
                <li id="addinnerchannelcompany"><a href="ChannelCompanyEdit.aspx?channeltype=inner"
                    onfocus="this.blur()"><span>添加门店</span></a></li>
                <%}
                  else
                  { %>
                <li id="addinnerchanneltongji"><a href="Channelstatistics.aspx?channelcompanytype=inner"
                    onfocus="this.blur()">所属门店 </a></li>
                <%} %>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    </h3>


               <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                   <h2 class="p-title-area">渠道管理</h2>
                   <div class="mi-form-item">
                        <label class="mi-label"> 渠道类型</label>
                        <label id="lblchanneltype">
                            </label>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 公司名称</label>
                       <input name="txtchannelcompanyname" type="text" id="txtchannelcompanyname"  size="25" class="mi-input"  style="width:200px;"/>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 所在城市</label>
                        <select name="province" id="province"  class="mi-input" >  
                            <option value="省份" selected="selected">省份</option>  
                        </select>  
                        <select name="city" id="city"   class="mi-input" >  
                            <option value="城市" selected="selected">市县</option>  
                        </select>  
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 公司地址</label>
                       <input name="txtcompanyaddress" type="text" id="txtcompanyaddress"  size="25" class="mi-input"  style="width:300px;"/>
                   </div>
                    <div class="mi-form-item">
                        <label class="mi-label"> 手机</label>
                       <input name="txtcompanyphone" type="text" id="txtcompanyphone"  size="25" class="mi-input"  style="width:100px;"/>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 网址</label>
                       <input name="txtbookurl" type="text" id="txtbookurl"  size="25" class="mi-input"  style="width:300px;"/>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 坐标</label>
                       <input name="coordinate" type="text" id="coordinate"  size="25" class="mi-input"  readonly="readonly" style="width:300px;"/><input class="button" type="button"  value=" 查找坐标 " onclick="MapPrtDlg()">
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 图片</label>
                       <input type="hidden" id="hid_companyimg" value="" />
                       <img alt="" class="headPortraitImgSrc" id="Img1" src="/images/defaultThumb.png" />
                       <uc1:uploadFile ID="UploadFile1" runat="server" />
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 简要介绍</label>
                       <textarea id="txtcompanyintro" rows="15" cols="80" class="mi-input" style="width: 50%;"></textarea>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 运行状态</label>
                      <select id="selcompanystate"  class="mi-input">
                                <option value="1">运行</option>
                                <option value="0">暂停</option>
                            </select>
                   </div>
                                      <div class="mi-form-item">
                        <label class="mi-label"> 是否是内部部门</label>
                       <select id="whetherdepartment" class="mi-input">
                                <option value="0">否</option>
                                <option value="1">是</option>
                            </select>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 合作商户ID</label>
                       <input name="Outshop" type="text" id="Outshop"  size="25" class="mi-input"  style="width:100px;"/>
                   </div>
                   <div class="mi-form-explain"></div>
               </div>
               <table width="760">
                    <tr>
                        <td class="tdHead" align="center">
                            <input type="button" id="btnd" value="确 定"  class="mi-input"/>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
     <script type="text/javascript">
         var province = document.getElementById('province');
         var city = document.getElementById('city');
    </script>
    <script src="/Scripts/City.js" type="text/javascript"></script>
    <input type="hidden" id="hid_channelcompanyid" value="<%=channelcompanyid %>" />
    <input type="hidden" id="hid_channeltype" value="<%=channeltype %>" />
</asp:Content>
