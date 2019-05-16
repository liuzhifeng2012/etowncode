<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="H5SetMenu_Manage.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.H5SetMenu_Manage" %>


<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>

    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script> 

    <script type="text/javascript">
        $(function () {


            $("#selectimglibrary").click(function () {
                $("#imglibrary").show();
            })
            $("#closeimglibrary").click(function () {
                $("#imglibrary").hide(); ;
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

                    }
                }
            })

            SearchList(1);


            //读取菜单类型
            $.post("/JsonFactory/ModelHandler.ashx?oper=modelzhidingpagelist", { id: 0 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                    for (i = 0; i < data.totalCount; i++) {
                        $("#linktype").append("<option value='" + data.msg[i].Id + "'>" + data.msg[i].Name + "</option>");
                    }
                }
            })

            //动态获取全部微信素材类型
            $.post("/jsonfactory/WeiXinHandler.ashx?oper=GetAllWxMaterialType", { comid: $("#hid_comid").trimVal() }, function (data) {

                data = eval("(" + data + ")");
                if (data.type == 100) {

                    if (data.totalCount > 0) {
                        $("#textsalepromotetypeid").html("");

                        var groupstr = "";
                        groupstr = '<option value="0">请选择分类</option>';
                        for (var i = 0; i < data.totalCount; i++) {

                            //                            groupstr += '<label><input name="radpromotetype"  type="radio" value="' + data.msg[i].Id + '">' + data.msg[i].TypeName + '</label>';
                            groupstr += '<option value="' + data.msg[i].Id + '">' + data.msg[i].TypeName + '</option>';
                        }
                        $("#textsalepromotetypeid").html(groupstr);
                    }
                }
            });

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


            $("#textwenzhang").change(function () {
                var promoteid = $("#textwenzhang").val();
                $("#linkurl").val("/weixin/wxmaterialdetail.aspx?materialid=" + promoteid);
                $("#linkurl").attr("disabled", "disabled");
            })


            $("#textsalepromotetypeid").change(function () {

                promotetypeid = $("#textsalepromotetypeid").val();
                $("#linkurl").val("/m/period.aspx?type=" + promotetypeid);
                $("#linkurl").attr("disabled", "disabled");

                $.ajax({
                    type: "post",
                    url: "/JsonFactory/WeiXinHandler.ashx?oper=pagelist",
                    data: { comid: $("#hid_comid").trimVal(), pageindex: 1, pagesize: 50, applystate: 1, promotetypeid: promotetypeid },
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
                                wenzhangstr = '<option value="0">请选择文章</option>';
                                for (var i = 0; i < data.totalCount; i++) {
                                    wenzhangstr += '<option value="' + data.msg[i].MaterialId + '">' + data.msg[i].Title + '</option>';
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


                $.post("/JsonFactory/DirectSellHandler.ashx?oper=editmenu", { id: $("#hid_id").val(), comid: comid, imgurl: imgurl, name: name, linkurl: linkurl,fonticon:fonticon }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("添加出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("添加成功");
                        location.href = "H5SetMenu.aspx";
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


             <%if (Daohangimg==1){ %>
                $("#imgtab").show();
                $("#fonttab").hide();
            <%}else{ %>
                $("#imgtab").hide();
                $("#fonttab").show();
            <%} %>

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
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/shangjiaui/H5Default.aspx" onfocus="this.blur()" target="">模板设置</a></li>
                 <li class="on"><a href="/ui/shangjiaui/H5SetMenu.aspx" onfocus="this.blur()" target="">栏目管理</a></li>
                <li><a href="/ui/shangjiaui/StoreList.aspx" onfocus="this.blur()" target="">门店模板设置</a></li>
                <li><a href="/ui/shangjiaui/consultant_pro.aspx" onfocus="this.blur()" target="">员工页面设置</a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <div>
                </div>
                <h3>
                  </h3>

               <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                   <h2 class="p-title-area">栏目管理</h2>
                   <div class="mi-form-item">
                        <label class="mi-label"> 名称</label>
                       <input name="name" type="text" id="name"  size="25" class="mi-input" value="<%=Name %>" style="width:200px;"/>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 连接地址</label>
                       <input name="linkurl" type="text" id="linkurl"  size="25" class="mi-input" value="<%=linkurl %>" style="width:300px;"/>
                       <br>选择连接到固定栏目：<select name="linktype" id="linktype">
	                            <option value="0" selected="selected">请选择固定栏目</option>
	                        </select>

                            <br>连接到文章分类：<select id="textsalepromotetypeid" >
                            </select>

                            <br>连接到详细文章：<select id="textwenzhang" >
                            </select>
                   </div>
                   <div class="mi-form-explain"></div>
               </div>
               <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                <h2 class="p-title-area">栏目图标</h2>
                   <div class="mi-form-item"  id="imgtab">
                       <label class="mi-label"> 按钮图片</label>
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
                   

                   <div class="mi-form-item"  id="fonttab">
                        <label class="mi-label"> 按钮图片</label>
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
</asp:Content>
