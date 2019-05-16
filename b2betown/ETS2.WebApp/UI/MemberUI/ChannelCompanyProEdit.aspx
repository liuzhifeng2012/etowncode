<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ChannelCompanyProEdit.aspx.cs" Inherits="ETS2.WebApp.UI.MemberUI.ChannelCompanyProEdit" %>


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
                            $("#txtcompanyproject").val(data.msg[0].Companyproject);

                        }
                    }
                })
            }



            $("#btnd").click(function () {

                var companyproject = $("#txtcompanyproject").val();
                var comid = $("#hid_comid").val();
                var channelcompanyid = $("#hid_channelcompanyid").val();

                $.post("/JsonFactory/ChannelHandler.ashx?oper=upchannelcompanproject", { channelcompanyid: channelcompanyid, comid: comid,companyproject: companyproject }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {

                    }
                    if (data.type == 100) {
                        $.prompt("操作成功", { buttons: [{ title: "确定", value: true}], submit: function (m, v, e, f) { if (v == true) { location.href = "Channelstatistics.aspx?channelcompanytype=" + $("#hid_channeltype").val(); } } })
                    }
                })


            })
                        //项目简介
                        $('#txtcompanyproject').tinymce({
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
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <%if (IsParentCompanyUser)
                  { %>
                <li id="addoutchanneltongji"><a href="Channelstatistics.aspx?channelcompanytype=out"
                    onfocus="this.blur()">合作单位</a></li>
                <li id="addoutchannelcompany"><a href="ChannelCompanyEdit.aspx?channeltype=out" onfocus="this.blur()">
                    添加合作单位</a></li>
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
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    店长推荐</h3>
                <table class="grid">
                    <tr>
                        <td class="tdHead" id="td7">
                            店长推荐：
                        </td>
                        <td>
                            <textarea id="txtcompanyproject" rows="15" cols="80" style="width: 50%;"></textarea>
                        </td>
                    </tr>
                   
                    <tr>
                        <td class="tdHead" colspan="2">
                            <input type="button" id="btnd" value="  确   定  " class="buttonblue"/>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <input type="hidden" id="hid_channelcompanyid" value="<%=channelcompanyid %>" />
    <input type="hidden" id="hid_channeltype" value="<%=channeltype %>" />
</asp:Content>
