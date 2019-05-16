<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ShopManage_Menu.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.ShopManage_Menu" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>

    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script> 

    <script type="text/javascript">
        $(function () {

            //读取菜单类型
            $.post("/JsonFactory/ModelHandler.ashx?oper=modelzhidingpagelist", { id: 0 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                    for (i = 0; i < data.totalCount; i++) {
                        $("#linktype").append("<option value='" + data.msg[i].Id + "'>" + data.msg[i].Name + "</option>");
                    }
                }
            })

            //动态获取有效项目
            $.post("/JsonFactory/ProductHandler.ashx?oper=projectpageuserlist", { comid: $("#hid_comid").trimVal(), pageindex: 1, pagesize: 200, projectstate: 1 }, function (data) {

                data = eval("(" + data + ")");
                if (data.type == 100) {

                    if (data.totalCount > 0) {
                        $("#textsalepromotetypeid").html("");

                        var groupstr = "";

                        var projectid = $("#hide_projectlist").val();

                        groupstr = '<option value="0">请选择项目</option>';
                        for (var i = 0; i < data.msg.length; i++) {
                            if (projectid != data.msg[i].Id) {
                                groupstr += '<option value="' + data.msg[i].Id + '">' + data.msg[i].Projectname + '</option>';
                            } else {
                                groupstr += '<option value="' + data.msg[i].Id + '"  selected="selected">' + data.msg[i].Projectname + '</option>';
                            }
                        }
                        $("#textsalepromotetypeid").html(groupstr);
                        $("#projectlist").html(groupstr);

                    }
                }
            });



            $("#selectimglibrary").click(function () {
                $("#imglibrary").show();
            })
            $("#closeimglibrary").click(function () {
                $("#imglibrary").hide(); ;
            })

            $("#imgtab_select").click(function () {
                $("#imgtab").show();
                $("#fonttab").hide(); ;
            })
            $("#fonttab_select").click(function () {
                $("#imgtab").hide();
                $("#fonttab").show();
            })

            $("#selectlink1").click(function () {
                $("#linktype_2").hide();
                $("#linktype_1").show();
            })
            $("#selectlink2").click(function () {
                $("#linktype_1").hide();
                $("#linktype_2").show();
            })



            var comid = $("#hid_comid").trimVal();
            var id = $("#hid_id").trimVal();
            $("#linkurl").attr("disabled", "disabled");
            bindViewImg();
            $.post("/JsonFactory/DirectSellHandler.ashx?oper=getmenubyid", { comid: comid, id: id }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取图片列表出错");
                    return;
                }
                if (data.type == 100) {
                    if (data.msg != null) {
                        $("#hid_id").val(data.msg.Id);
                        $("#hid_com_id").val(data.msg.Com_id);
                        $("#name").val(data.msg.Name);
                        $("#linkurl").val(data.msg.Linkurl);
                        $("#hid_logo").val(data.msg.Imgurl_address);
                        $("#hid_imgurl").val(data.msg.Imgurl);
                        $("#tab").addClass(data.msg.Fonticon);

                        if (data.msg.Linktype != 0) {
                            $("#linkurl").attr("disabled", "disabled");
                        };
                        $("input[name='usestyle'][value='" + data.msg.Usestyle + "']").attr("checked", true);
                        if (data.msg.Usestyle == 1) {
                            $("#imgtab").show();
                            $("#fonttab").hide(); ;
                        } else {
                            $("#imgtab").hide();
                            $("#fonttab").show();
                        }
                        $("#projectlist").val(data.msg.Projectlist);
                        $("#hide_projectlist").val(data.msg.Projectlist);

                        //读取设定的产品
                        $.ajax({
                            type: "post",
                            url: "/JsonFactory/ProductHandler.ashx?oper=SelectMenupagelist",
                            data: { comid: comid, pageindex: 1, pagesize: 50, menuid: id, projectid: 0 },
                            async: false,
                            success: function (data1) {
                                data1 = eval("(" + data1 + ")");
                                if (data1.type == 100) {
                                    var prolist = "";
                                    if (data1.totalCount > 0) {
                                        $("#viewprolist").show();
                                        for (var i = 0; i < data1.msg.length; i++) {
                                            prolist += '<option value="' + data1.msg[i].Id + '"  selected="selected">' + data1.msg[i].Pro_name + '</option>';
                                        }
                                        $("#prolist").html(prolist);
                                    }
                                }
                            }
                        });

                    }
                }
            })

            SearchList(1);



            //当选择指定栏目
            $("#linktype").change(function () {
                var linktype_temp = $("#linktype").val();

                if (linktype_temp != 0) {
                    $.post("/JsonFactory/ModelHandler.ashx?oper=modelzhidingpagelist", { id: linktype_temp }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            for (i = 0; i < data.totalCount; i++) {
                                $("#linkurl").val(data.msg[i].Linkurl);
                                $("#linkurl").attr("disabled", "disabled");
                            }
                        }
                    })
                }
            })



            //获取指定产品
            $("#textwenzhang").change(function () {
                var promoteid = $("#textwenzhang").val();
                $("#linkurl").val("/h5/order/pro.aspx?id=" + promoteid);
                $("#linkurl").attr("disabled", "disabled");
            })


            //栏目显示内容 
            $("#projectlist").change(function () {
                projectlist = $("#projectlist").val();
                $("#prolist").html("");
                $.post("/JsonFactory/ProductHandler.ashx?oper=pagelist", { comid: $("#hid_comid").trimVal(), pageindex: 1, pagesize: 50, pro_state: 2, projectid: projectlist }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 100) {
                        if (data.totalCount > 0) {
                            var wenzhangstr = "";
                            wenzhangstr = '';
                            for (var i = 0; i < data.totalCount; i++) {
                                wenzhangstr += '<option value="' + data.msg[i].Id + '">' + data.msg[i].Pro_name + '</option>';
                            }
                            $("#prolist").html(wenzhangstr);
                            $("#viewprolist").show();
                        }
                    }
                })


            })

            //链接指向内容
            $("#textsalepromotetypeid").change(function () {

                promotetypeid = $("#textsalepromotetypeid").val();
                $("#linkurl").val("/h5/order/list.aspx?projectid=" + promotetypeid);
                $("#linkurl").attr("disabled", "disabled");

                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=pagelist",
                    data: { comid: $("#hid_comid").trimVal(), pageindex: 1, pagesize: 50, pro_state: 2, projectid: promotetypeid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("文章列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#textwenzhang").html("");
                            if (data.totalCount > 0) {
                                var wenzhangstr = "";
                                wenzhangstr = '<option value="0">请选择产品</option>';
                                for (var i = 0; i < data.totalCount; i++) {
                                    wenzhangstr += '<option value="' + data.msg[i].Id + '">' + data.msg[i].Pro_name + '</option>';
                                }
                                $("#textwenzhang").html(wenzhangstr);
                            }

                        }
                    }
                })


            })


            $("#button").click(function () {
                var imgurl = $("#<%=headPortrait.FileUploadId_ClientId %>").val();
                if (imgurl == "") {
                    imgurl = $("#hid_imgurl").trimVal();
                }
                var name = $("#name").trimVal();
                if (name == "") {
                    $.prompt("标题不可为空");
                    return;
                }
                var linkurl = $("#linkurl").trimVal();
                if (linkurl == "") {
                    $.prompt("连接地址不可为空");
                    return;
                }
                var fonticon = $("#tab").attr('class');
                var usestyle = $('input:radio[name="usestyle"]:checked').val();



                var projectlist = $("#projectlist").trimVal();
                var prolist = $("#prolist").trimVal();

                if (projectlist == "") {
                    $.prompt("请选栏目下显示的项目");
                    return;
                }


                $.post("/JsonFactory/DirectSellHandler.ashx?oper=editmenu", { id: $("#hid_id").val(), comid: comid, imgurl: imgurl, name: name, linkurl: linkurl, fonticon: fonticon, usetype: 1, usestyle: usestyle, projectlist: projectlist, prolist: prolist }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("添加出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("添加成功");
                        location.href = "ShopManage.aspx";
                        return;
                    }
                })
            })


            $("#selectfontlibrary").click(function () {
                SearchFontList(1);
                $("#fontlibrary").show();
            })
            $("#closefontlibrary").click(function () {
                $("#fontlibrary").hide(); ;
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

        //图片库
        var library_html = "";
        var pageSize = 36; //每页显示条数

        function SearchList(pageindex) {
            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.post("/JsonFactory/ModelHandler.ashx?oper=imagemodelLibraryList", { usertype: 0, modelid: $("#hid_modelid").trimVal(), Pageindex: pageindex, Pagesize: pageSize }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取图片列表出错");
                    return;
                }
                if (data.type == 100) {
                    if (data.msg != null) {

                        library_html = "<ul>";
                        for (i = 0; i < data.bankCount; i++) {
                            library_html += "<li style=\"float:left; margin:1px;cursor:pointer;\" ondblclick=\"OnSelectImg('" + data.msg[i].Imgurl + "','" + data.msg[i].Imgurl_address + "')\"> " + "<img alt=\"\" class=\"headPortraitImgSrc\" id=\"Img1\" src=" + data.msg[i].Imgurl_address + "  height=\"60px\"  style=\"background-color:#0099ff\"/></p>" + " </li>"
                        }
                        library_html += "</ul> ";

                        $("#imglibrarytext").html(library_html);
                        setpage(data.totalCount, pageSize, pageindex);
                    }
                }
            })

        }

        //图标库
        var libraryfont_html = "";

        function SearchFontList(pageindex) {
            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.post("/JsonFactory/ModelHandler.ashx?oper=fontLibraryList", { usertype: 0, Pageindex: pageindex, Pagesize: 50 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取图片列表出错");
                    return;
                }
                if (data.type == 100) {
                    if (data.msg != null) {
                        libraryfont_html = "<ul>";
                        for (i = 0; i < data.msg.length; i++) {
                            libraryfont_html += "<li style=\"float:left; margin:1px;cursor:pointer; font-size:50px\" ondblclick=\"OnSelectFont('" + data.msg[i].Fonticon + "')\"> " + "<span  class=\"" + data.msg[i].Fonticon + "\" style=\"background-color:#0099ff\"/></span>" + " </li>"
                        }
                        libraryfont_html += "</ul> ";

                        $("#fontlibrarytext").html(libraryfont_html);
                        setpage1(data.totalCount, pageSize, pageindex);
                    }
                }
            })

        }


        function OnSelectImg(imgid, imgadd) {
            $("#hid_imgurl").val(imgid);

            //var filePath = '<%=headPortrait.fileUrl %>';
            var headlogoImgSrc = imgadd;
            $("#headPortraitImg").attr("src", headlogoImgSrc);

            $("#imglibrary").hide();
        }

        //选择图标
        function OnSelectFont(icon) {
            $("#tab").removeClass().addClass(icon);
            $("#fontlibrary").hide();
        }

        //分页
        function setpage(newcount, newpagesize, curpage) {
            $("#divPage").paginate({
                count: Math.ceil(newcount / newpagesize),
                start: curpage,
                display: 5,
                border: false,
                text_color: '#888',
                background_color: '#EEE',
                text_hover_color: 'black',
                images: false,
                rotate: false,
                mouse: 'press',
                onChange: function (page) {
                    SearchList(page);
                    return false;
                }
            });
        }


        //分页
        function setpage1(newcount, newpagesize, curpage) {
            $("#divPage1").paginate({
                count: Math.ceil(newcount / newpagesize),
                start: curpage,
                display: 5,
                border: false,
                text_color: '#888',
                background_color: '#EEE',
                text_hover_color: 'black',
                images: false,
                rotate: false,
                mouse: 'press',
                onChange: function (page) {
                    SearchFontList(page);
                    return false;
                }
            });
        }


       
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
              <li  class="on"><a href="/ui/shangjiaui/ShopManage.aspx" onfocus="this.blur()" target="">微商城设置</a></li>
                <li><a href="/ui/shangjiaui/consultant_pro.aspx" onfocus="this.blur()" target="">顾问页面设置</a></li>
                <li><a href="/ui/shangjiaui/StoreList.aspx" onfocus="this.blur()" target="">门店模板设置</a></li>

            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <div>
                </div>
                <h3>
                  </h3>

               <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                   <h2 class="p-title-area">栏目管理</h2>
                   <div class="mi-form-item">
                        <label class="mi-label"> 标题</label>
                       <input name="name" type="text" id="name"  size="25" class="mi-input" value="<%=Name %>" style="width:200px;"/>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 标题链接地址</label>
                       <input name="linkurl" type="text" id="linkurl"  size="25" class="mi-input" value="<%=linkurl %>" style="width:300px;"/>
                       
                 <div ><br>
                 <label><input id="selectlink1" name="usestyle" type="radio" value="0" >链接到固定栏目</label>
                 <label><input id="selectlink2" name="usestyle" type="radio" value="1" >链接到到产品</label>
                  </div>
                            <div id="linktype_1" style=" display:none;">
                                 <br>链接到固定栏目：<select name="linktype" id="linktype">
	                            <option value="0" selected="selected">请选择固定栏目</option>
	                            </select>
                            </div>
                            <div  id="linktype_2" style=" display:none;">
                            <br>链接到项目：<select id="textsalepromotetypeid" >
                            </select>

                            <br>链接到产品：<select id="textwenzhang" >
                            </select>
                            </div>
                   </div>
                   <div class="mi-form-explain"></div>
               </div>

               <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                  <h2 class="p-title-area">栏目下显示项目</h2>

                           <div class="mi-form-item">  
                            <br\>显示项目：<select id="projectlist" >
                            </select>
                             <div id="viewprolist" style=" display:none; padding-top:10px;" >
                            <br\>显示产品：<select id="prolist"  size="6" multiple="multiple" >
                            </select>
                            <br />注：产品可以进行多选，当未选择产品时，显示此项目下前6个产品。当选择产品则只显示选择的产品
                            </div>
                            </div>
                   <div class="mi-form-explain"></div>
                   <div class="mi-form-explain"></div>
               </div>
               <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                <h2 class="p-title-area">栏目图标</h2>
                  <div class="mi-form-item">
                <label><input id="fonttab_select" name="usestyle" type="radio" value="0" >文字栏目</label> 
                 <label><input id="imgtab_select" name="usestyle" type="radio" value="1" >图片栏目</label>
                  </div>
                   <div class="mi-form-item"  id="imgtab" style=" display:none;">
                       <label class="mi-label"> 栏目图片</label>
                       <input type="hidden" id="Hidden1" value="" />
                       <img alt="" class="headPortraitImgSrc" id="headPortraitImg" src="/images/defaultThumb.png"  style=" background-color: #0099ff"/>
                       
                      <ul>
                                        <li style="height: 20px; margin-left: 10px;float:left;">
                                            <div class="C_verify">
                                                <span>
                                                    <uc1:uploadFile ID="headPortrait" runat="server" />
                                                </span>
                                            </div>
                                        </li>

                                        <li style="height: 20px; margin-left: 20px; float:left;">
                                            <div class="C_verify">
                                                <span>
                                                    <input type="button" name="selectimglibrary" id="selectimglibrary" value="  图片库选择图片  " class="buttonblue-a" />
                                                </span>
                                            </div>
                                        </li>
                                    </ul>
                   </div>
                   

                   <div class="mi-form-item"  id="fonttab" style=" display:none;">
                        <label class="mi-label"> 栏目图标（可以不选择只显示标题文字）</label>
                        <span id="tab" style=" font-size:50px;"></span>
                        <input type="button" name="selectimglibrary" id="selectfontlibrary" value="  请选择图标  " class="buttonblue-a" />
                   </div>
                   
                   <div class="mi-form-explain"></div>
                    <div class="mi-form-explain"></div>
               </div>

                <table width="780px" class="grid">
                    <tr>
                        <td height="80" colspan="2" align="center">
                            <input type="button" name="button" id="button" value="  确 认 提 交  " class="mi-input" >
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>

    <div id="imglibrary" style="background-color: #ffffff; border: 2px solid #5984bb;
                    margin: 0px auto; width: 420px; height: 500px; display: none; left: 20%; position: absolute;
                    top: 20%; z-index: 100;">
        <div style=" height:20px;padding:5px;text-align: right; "><div  style="float:left;">请双击图片选择</div><div  id="closeimglibrary" style="cursor:pointer; float:right;">×</div></div>
        <div id="imglibrarytext"style="width: 410px; height: 430px;"></div>
        <div id="divPage" style="width: 410px; height:25px;">
        </div>
    </div>
        <div id="fontlibrary" style="background-color: #ffffff; border: 2px solid #5984bb;
                    margin: 0px auto; width: 420px; height: 500px; display: none; left: 20%; position: absolute;
                    top: 20%; z-index: 100;">
        <div style=" height:20px;padding:5px;text-align: right; "><div  style="float:left;">请双击图标选择</div><div  id="closefontlibrary" style="cursor:pointer; float:right;">×</div></div>
        <div id="fontlibrarytext"style="width: 410px; height: 430px;"></div>
        <div id="divPage1" style="width: 410px; height:25px;">
        </div>
    </div>

    <input type="hidden" id="hid_id" value="<%=id %>" />
        <input type="hidden" id="hid_icon" value="" />
    <input type="hidden" id="hid_modelid" value="<%=Modelid %>" />
    <input type="hidden" id="hid_imgurl" value="" />
    <input type="hidden" id="hide_projectlist" value="" />
    

</asp:Content>
