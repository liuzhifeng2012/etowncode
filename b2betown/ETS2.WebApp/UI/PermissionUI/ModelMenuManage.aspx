<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master"  CodeBehind="ModelMenuManage.aspx.cs" Inherits="ETS2.WebApp.UI.PermissionUI.ModelMenuManage" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
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



            bindViewImg();

            //首先加载数据
            var hid_id = $("#hid_id").trimVal();
            var hid_modelid = $("#hid_modelid").trimVal();
            if (hid_id != '0') {
                $.post("/JsonFactory/ModelHandler.ashx?oper=getmodelmenubyId", { modelid: hid_modelid, id: hid_id }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#name").val(data.msg.Name);
                        $("#linkurl").val(data.msg.Linkurl);
                        $("#hid_imgurl").val(data.msg.Imgurl);
                        $("#tab").addClass(data.msg.Fonticon); 
                    }

                })

            }

            SearchList(1);
            SearchFontList(1);
            if (hid_modelid != '0') {
                $.post("/JsonFactory/ModelHandler.ashx?oper=getModelById", { modelid: hid_modelid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#title").text(data.msg.Title);


                        if (data.msg.Daohangimg == 1) {
                            $("#img").show();
                            $("#fonttab").hide();
                        }

                        if (data.msg.Daohangimg == 0) {
                            $("#fonttab").show();
                            $("#img").hide();
                        }
                    }
                })
            }

            //读取菜单类型
            $.post("/JsonFactory/ModelHandler.ashx?oper=modelzhidingpagelist", { id: 0 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取数据出现错误");
                    return;
                }
                if (data.type == 100) {
                    for (i = 0; i < data.totalCount; i++) {
                        $("#linktype").append("<option value='" + data.msg[i].Id + "'>" + data.msg[i].Name + "</option>");
                    }
                }

            })

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
                } else {
                    $("#linkurl").val("");
                    $("#linkurl").removeAttr("disabled");
                }
            })



            //确认发布按钮
            $("#GoBotton").click(function () {
                var imgurl = $("#<%=headPortrait.FileUploadId_ClientId %>").val();

                if (imgurl == "") {
                    imgurl = $("#hid_imgurl").trimVal();
                }


                var name = $("#name").trimVal();
                var linkurl = $("#linkurl").trimVal();
                var linktype = $("#linktype").trimVal();
                var fonticon = $("#tab").attr('class');

                if (name == '') {
                    $.prompt('请填写栏目名称');
                    return;
                }

                if (linkurl == '') {
                    $.prompt('请填写链接地址！');
                    return;
                }


                $.post("/JsonFactory/ModelHandler.ashx?oper=ModelMenuInsertOrUpdate", { modelid: hid_modelid, name: name, linkurl: linkurl, linktype: linktype, id: hid_id, imgurl: imgurl, fonticon: fonticon }, function (data) {
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
                                    location.href = "ModelMenulist.aspx?modelid=" + hid_modelid;
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


            $("#selectfontlibrary").click(function () {
                $("#fontlibrary").show();
            })
            $("#closefontlibrary").click(function () {
                $("#fontlibrary").hide(); ;
            })

        })


        function OnSelectFont(icon) {
            $("#tab").removeClass().addClass(icon); 
            $("#fontlibrary").hide();
        }
        //图标库
        var libraryfont_html = "";
        var pageSize = 24; //每页显示条数

        function SearchFontList(pageindex) {
            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.post("/JsonFactory/ModelHandler.ashx?oper=fontLibraryList", { usertype: 0, Pageindex: pageindex, Pagesize: pageSize }, function (data) {
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
                        setpage(data.totalCount, pageSize, pageindex);
                    }
                }
            })

        }



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
        }


        //图片库
        var library_html = "";

        function SearchList(pageindex) {
            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.post("/JsonFactory/ModelHandler.ashx?oper=imageLibraryList", { usertype: 0, Pageindex: pageindex, Pagesize: 18 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取图片列表出错");
                    return;
                }
                if (data.type == 100) {
                    if (data.msg != null) {
                        library_html = "<ul>";
                        for (i = 0; i < data.msg.length; i++) {
                            library_html += "<li style=\"float:left; margin:1px;cursor:pointer;\" ondblclick=\"OnSelectImg('" + data.msg[i].Imgurl + "','" + data.msg[i].Imgurl_address + "')\"> " + "<img alt=\"\" class=\"headPortraitImgSrc\" id=\"Img1\" src=" + data.msg[i].Imgurl_address + "  height=\"60px\"  style=\"background-color:#0099ff\"/></p>" + " </li>"
                        }
                        library_html += "</ul> ";

                        $("#imglibrarytext").html(library_html);
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

                    SearchFontList(page);

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
                    SearchList(page);
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
                   <span id="title"></span> 添加栏目
                </h3>
                <table class="grid">
                    <tr>
                        <td class="tdHead">
                            栏目名称：
                        </td>
                        <td>
                            <input name="name" type="text" id="name" value="" size="80" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                           链接地址：
                        </td>
                        <td>
                            <input name="linkurl" type="text" id="linkurl" value="" size="50" />
                            <select name="linktype" id="linktype">
	                            <option value="0" selected="selected">自定义链接</option>
	                        </select>
                        </td>
                    </tr>
                    <tr id="img">
                        <td class="tdHead">
                            图片图标：
                        </td>
                        <td>
                            <div class="C_head">
                                <dl>
                                    <dt>
                                        <input type="hidden" id="Hidden1" value="" />
                                        <img alt="" class="headPortraitImgSrc" id="headPortraitImg" src="/images/defaultThumb.png"  style=" background-color: #0099ff"/></dt>
                                    <dd>上传图片或在图库中选择图片
                                       </dd>
                                </dl>
                                <div class="cl">
                                </div>
                            </div>
                            <div class="C_head_no">
                                <div class="C_head_1">
                                    <ul>


                                        <li style="height: 20px; margin-left: 10px;float:left;"">
                                            <div class="C_verify">
                                                <label>
                                                   </label>
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
                            </div>
                        </td>
                    </tr>
                    <tr id="fonttab">
                        <td class="tdHead">
                            栏目图标：
                        </td>
                        <td>
                            <div class="C_head">
                                <dl>
                                    <dt><span id="tab" style=" font-size:50px;"></span>
                                        </dt>
                                    <dd>
                                       </dd>
                                </dl>
                                <div class="cl">
                                </div>
                            </div>
                            <div class="C_head_no">
                                <div class="C_head_1"> 
                                <ul>
                                        <li style="height: 20px; margin-left: 10px;float:left;"">
                                       <div class="C_verify">
                                                <span>
                                                    <input type="button" name="selectimglibrary" id="selectfontlibrary" value="  请选择图标  " class="buttonblue-a" />
                                                </span>
                                       </div>
</li>
</ul>
                                </div>
                            </div>
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
    
    <div id="fontlibrary" style="background-color: #ffffff; border: 2px solid #5984bb;
                    margin: 0px auto; width: 420px; height: 300px; display: none; left: 20%; position: absolute;
                    top: 20%;">
        <div style=" height:20px;padding:5px;text-align: right; "><div  style="float:left;">请双击图标选择</div><div  id="closefontlibrary" style="cursor:pointer; float:right;">×</div></div>
        <div id="fontlibrarytext"style="width: 410px; height: 215px;"></div>
        <div id="divPage" style="width: 410px; height:25px;">
        </div>
    </div>
    <div id="imglibrary" style="background-color: #ffffff; border: 2px solid #5984bb;
                    margin: 0px auto; width: 420px; height: 300px; display: none; left: 20%; position: absolute;
                    top: 20%;">
        <div style=" height:20px;padding:5px;text-align: right; "><div  style="float:left;">请双击图片选择</div><div  id="closeimglibrary" style="cursor:pointer; float:right;">×</div></div>
        <div id="imglibrarytext"style="width: 410px; height: 215px;"></div>
        <div id="divPage1" style="width: 410px; height:25px;">
        </div>
    </div>

    <input type="hidden" id="hid_id" value="<%=id %>" />
    <input type="hidden" id="hid_icon" value="" />
    <input type="hidden" id="hid_modelid" value="<%=modelid %>" />
    <input type="hidden" id="hid_imgurl" value="<%=headPortraitImgSrc %>" />
</asp:Content>
