<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="projectedit.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.projectedit"
    MasterPageFile="/UI/Etown.Master" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //商户行业类型
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetIndustryList", {}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                    $("#com_type").empty();
                    $("#com_type").append('<option value="0">请选择所属行业</option>');
                    for (var i = 0; i < data.msg.length; i++) {
                        $("#com_type").append('<option value="' + data.msg[i].Id + '">' + data.msg[i].Industryname + '</option>');
                    }

                }
            })

            var projectid = $("#hid_projectid").trimVal();
            if (projectid > 0)//编辑
            {
                $.post("/JsonFactory/ProductHandler.ashx?oper=getproject", { id: projectid, comid: $("#hid_comid").trimVal() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取信息失败" + data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#projectname").val(data.msg[0].Projectname);
                        $("#province").val(data.msg[0].Province);
                        $("#city").append("<option value='" + data.msg[0].City + "' selected>" + data.msg[0].City + "</option>");
                        $("#com_type").val(data.msg[0].Industryid);
                        $("#briefintroduce").val(data.msg[0].Briefintroduce);
                        $("#address").val(data.msg[0].Address);
                        $("#mobile").val(data.msg[0].Mobile);
                        $("#coordinate").val(data.msg[0].Coordinate);
                        $("#serviceintroduce").val(data.msg[0].Serviceintroduce);
                        $("#onlinestate").val(data.msg[0].OnlineState);

                        $("#hid_imgurl").val(data.msg[0].Projectimg);
                        $("#headPortraitImg").attr("src", data.msg[0].Projectimgurl);

                        if (data.msg[0].hotelset == 1) {
                            $("#hotelinfo").show();
                        }
                        $("#hotelset").val(data.msg[0].hotelset);
                        $("#grade").val(data.msg[0].grade);
                        $("#star").val(data.msg[0].star);
                        $("#parking").val(data.msg[0].parking);
                        $("#cu").val(data.msg[0].cu);



                    }
                })
            }

            $("#hotelset").change(function () {

                if ($("#hotelset").val() == 1) {
                    $("#hotelinfo").show();
                } else {
                    $("#hotelinfo").hide();
                }
            })


            $("#confirmpub").click(function () {
                var projectid = $("#hid_projectid").trimVal();
                var projectname = $("#projectname").trimVal();
                if (projectname == "") {
                    $.prompt("项目名称不可为空");
                    return;
                }
                var img = $("#<%=headPortrait.FileUploadId_ClientId %>").val();

                if (img == "") {
                    img = $("#hid_imgurl").trimVal();
                }

                var province = $("#province").val();
                if (province == "省份") {
                    $.prompt("请选择省份");
                    return;
                }
                var city = $("#city").val();
                if (city == "城市") {
                    $.prompt("请选择市县");
                    return;
                }
                var com_type = $("#com_type").val();
                if (com_type == "0") {
                    $.prompt("请选择所属行业");
                    return;
                }
                var briefintroduce = $("#briefintroduce").val();
                if (briefintroduce == "") {
                    $.prompt("请输入项目简介");
                    return;
                }
                var address = $("#address").val();
                if (address == "") {
                    $.prompt("请输入地址");
                    return;
                }
                var mobile = $("#mobile").val();
                if (mobile == "") {
                    $.prompt("请输入电话");
                    return;
                }
                var coordinate = $("#coordinate").val();
                //                if (coordinate == "") {
                //                    $.prompt("请输入地址坐标");
                //                    return;
                //                }
                var serviceintroduce = $("#serviceintroduce").val();

                var onlinestate = $("#onlinestate").val();

                var hotelset = $("#hotelset").val();
                var grade = $("#grade").val();
                var star = $("#star").val();
                var parking = $("#parking").val();
                var cu = $("#cu").val();






                $.post("/JsonFactory/ProductHandler.ashx?oper=editproject", { projectid: projectid, projectname: projectname, img: img, province: province, city: city, com_type: com_type, briefintroduce: briefintroduce, address: address, mobile: mobile, coordinate: coordinate, serviceintroduce: serviceintroduce, onlinestate: onlinestate, comid: $("#hid_comid").trimVal(), userid: $("#hid_userid").trimVal(), hotelset: hotelset, grade: grade, star: star, parking: parking, cu: cu }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作失败" + data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("操作成功", { submit: function () { location.href = "projectlist.aspx" } });
                        return;
                    }
                })

            })
        })
    </script>
    <script type="text/javascript">
        $(function () {
            $('#serviceintroduce').tinymce({
                // Location of TinyMCE script
                script_url: '/Scripts/tiny_mce/tiny_mce.js',
                width: '422',
                height: '300',
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
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/pmui/ProjectList.aspx" onfocus="this.blur()" target=""><span>项目列表</span></a></li>
                <li class="on"><a href="/ui/pmui/Projectedit.aspx" onfocus="this.blur()" target=""><span>
                    添加项目</span></a></li>
                <li><a href="/ui/pmui/ProductList.aspx" onfocus="this.blur()" target=""><span>产品列表</span></a></li>
                <li><a href="/ui/pmui/ProductServerTypeList.aspx" onfocus="this.blur()" target=""><span>添加产品</span></a></li>
               <li><a href="/ui/pmui/order/Salecount.aspx" onfocus="this.blur()" target="">产品统计</a></li>
                 <li><a href="/ui/pmui/BindingAgent.aspx" onfocus="this.blur()" target="">导入分销系统产品</a></li>
                    <li  ><a href="/ui/pmui/eticket_useset.aspx" onfocus="this.blur()" target="">
                        <span>商户特定日期设定</span></a></li>
                          <li><a href="/ui/pmui/delivery/deliverylist.aspx" onfocus="this.blur()" target="">
                    <span>运费模版管理</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                </h3>

                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                   <h2 class="p-title-area">项目基本信息</h2>
                   <div class="mi-form-item">
                        <label class="mi-label"> 项目名称</label>
                       <input name="projectname" type="text" id="projectname"  size="25" class="mi-input"  style="width:200px;"/>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 项目图片</label>
                        <input type="hidden" id="hid_imgurl" value="" />
                        <img alt="" class="headPortraitImgSrc" id="headPortraitImg" src="/images/defaultThumb.png" />
                        <uc1:uploadFile ID="headPortrait" runat="server" />
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 所在城市</label>
                        <select name="province" id="province" class="ui-input">
                            <option value="省份"  class="mi-input" selected="selected">省份</option>
                        </select>
                        <select name="city" id="city" class="ui-input">
                            <option value="城市" class="mi-input" selected="selected">市县</option>
                        </select>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 所属行业</label>
                        <select name="com_type" id="com_type" class="ui-input">
                                    <option value="0" class="mi-input" selected="selected">请选择所属行业</option>
                        </select>
                   </div>

                   <div class="mi-form-item">
                        <label class="mi-label"> 项目简介</label>
                         <input name="briefintroduce" type="text" id="briefintroduce" class="mi-input" value="" size="100">
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 地址</label>
                         <input name="address" type="text" id="address" class="mi-input" value="" size="100">
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 电话</label>
                         <input name="mobile" type="text" id="mobile" class="mi-input" value="" size="100">
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 坐标</label>
                         <input name="coordinate" type="text" id="coordinate" class="mi-input" value="" size="100"><input class="button" type="button" value=" 查找坐标 " onclick="MapPrtDlg()">
                   </div>

                   <div class="mi-form-item">
                        <label class="mi-label"> 是否为酒店（设置酒店信息）</label>
                        <select name="hotelset" id="hotelset" class="ui-input">
                            <option value="0" class="mi-input" selected="selected">非酒店</option>
                            <option value="1" class="mi-input" selected="selected">设置酒店信息</option>
                        </select>
                   </div>
                   <div class="mi-form-item" style=" display:none; background-color:#efefef; margin:20px; padding-bottom:10px;" id="hotelinfo">
                        <label class="mi-label"> 评分</label>
                         <input name="grade" type="text" id="grade" class="mi-input" value="" size="100">
                         <label class="mi-label" style="padding-top: 10px;"> 星级</label>
                          <select name="star" id="star" class="ui-input">
                            <option value="" class="mi-input" selected="selected">不显示星级</option>
                            <option value="公寓" class="mi-input">公寓</option>
                            <option value="准2星" class="mi-input">准2星</option>
                            <option value="2星级" class="mi-input">2星级</option>
                            
                            <option value="准3星" class="mi-input">准3星</option>
                            <option value="3星级" class="mi-input">3星级</option>
                            
                            <option value="准4星" class="mi-input">准4星</option>
                            <option value="4星级" class="mi-input">4星级</option>
                            
                            <option value="准5星" class="mi-input">准5星</option>
                            <option value="5星级" class="mi-input">5星级</option>
                            
                            <option value="6星级" class="mi-input">6星级</option>
                            <option value="7星级" class="mi-input">7星级</option>
                        </select>
                         <label class="mi-label" style="padding-top: 10px;"> 有停车位</label>
                         <select name="parking" id="parking" class="ui-input">
                            <option value="0" class="mi-input" selected="selected">无</option>
                            <option value="1" class="mi-input">有</option>
                        </select>
                         <label class="mi-label" style="padding-top: 10px;"> 促销 文字</label>
                         <input name="cu" type="text" id="cu" class="mi-input" value="" size="100">（4个文字以内）
                   </div>


                   <div class="mi-form-item">
                        <label class="mi-label"> 服务介绍</label>
                          <textarea name="serviceintroduce" cols="50" class="mi-input"  rows="5" id="serviceintroduce"> </textarea>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 上线状态</label>
                        <select id="onlinestate"  class="mi-input">
                                    <option value="1">上线</option>
                                    <option value="0">下线</option>
                        </select>
                   </div>
                   <div class="mi-form-explain"></div>
               </div>

                <table border="0">
                    <tbody>
                        <tr>
                            <td width="600" height="80" align="center">
                                <input type="hidden" id="hid_projectid" value="<%=projectid %>" />
                                <input type="button" name="confirmpub"  class="mi-input"  id="confirmpub" value="  确认提交  " />
                                <input type="button" name="Submit"  class="mi-input"  value="返回上一步" onclick=" history.back()" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var province = document.getElementById('province');
        var city = document.getElementById('city');
    </script>
    <script src="/Scripts/City.js" type="text/javascript"></script>
</asp:Content>
