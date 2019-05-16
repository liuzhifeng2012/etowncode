<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/UI/Etown.Master" CodeBehind="H5Setting.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.H5Setting" %>


<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
        <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {


            var comid = $("#hid_comid").trimVal();
            var id = $("#hid_id").trimVal();
            var typeid = $("#hid_typeid").trimVal();


            $("#selectimglibrary").click(function () {
                $("#imglibrary").show();
            })
            $("#closeimglibrary").click(function () {
                $("#imglibrary").hide(); ;
            })
            SearchList(1);
            bindViewImg();
            $.post("/JsonFactory/DirectSellHandler.ashx?oper=getimagebyid", { comid: comid, id: id, typeid: typeid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取图片列表出错");
                    return;
                }
                if (data.type == 100) {
                    if (data.msg != null) {
                        $("#hid_id").val(data.msg.Id);
                        $("#hid_com_id").val(data.msg.Com_id);
                        $("#title").val(data.msg.Title);
                        $("#hid_logo").val(data.msg.Imgurl_address);
                        $("#hid_imgurl").val(data.msg.Imgurl);
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

                $.post("/JsonFactory/DirectSellHandler.ashx?oper=editimage", { id: $("#hid_id").val(), typeid: typeid, comid: comid, imgurl: imgurl, title: title, linkurl: linkurl }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("添加出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("添加成功");

                        if (typeid == 0) {
                            location.href = "H5Setlist.aspx";
                        } else if(typeid == 1) { 
                            location.href = "StoreList.aspx";
                        } else if(typeid == 2) { 
                            location.href = "ShopManage.aspx";
                        }
                        return;
                    }
                })
            })

            $("#deletebanner").click(function () {
                $.post("/JsonFactory/DirectSellHandler.ashx?oper=deleteimage", { id: $("#hid_id").val(), comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作出现错误");

                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("删除成功");
                        if (typeid == 0) {
                            location.href = "H5Setlist.aspx";
                        } else if (typeid == 1){
                            location.href = "StoreList.aspx";
                        } else if (typeid == 2) {
                            location.href = "ShopManage.aspx";
                        }
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

        //图片库
        var library_html = "";
        var pageSize = 36; //每页显示条数

        function SearchList(pageindex) {
            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.post("/JsonFactory/ModelHandler.ashx?oper=imagemodelLibraryList", { usetype: 1, modelid: $("#hid_modelid").trimVal(), Pageindex: pageindex, Pagesize: pageSize }, function (data) {
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
                    SearchList(page);
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
                <li <%if (typeid == 2){ %>class="on"<%} %>><a href="/ui/shangjiaui/ShopManage.aspx" onfocus="this.blur()" target="">微商城设置</a></li>
                   <li><a href="/ui/shangjiaui/consultant_pro.aspx" onfocus="this.blur()" target="">顾问页面设置</a></li>
                
                <li <%if (typeid == 1){ %>class="on"<%} %>><a href="/ui/shangjiaui/StoreList.aspx" onfocus="this.blur()" target="">门店模板设置</a></li>
                 
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                
                  </h3>

               <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                   <h2 class="p-title-area"><%if (typeid == 0)
                  { %>
                  图片管理
                  <%}
                                              else if (typeid == 1)
                  { %>
                  门店图片管理
                  <%} 
                      else if (typeid ==2)
                    
                  { %>
                  微商城Banner管理
                  <%} %>
                  </h2>
                   <div class="mi-form-item">
                        <label class="mi-label"> 标题</label>
                       <input name="title" type="text" id="title"  size="25" class="mi-input"  value="<%=Title %>" style="width:200px;"/>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 链接地址</label>
                       <input name="linkurl" type="text" id="linkurl"  size="25" class="mi-input"  value="<%=linkurl %>" style="width:300px;"/>
                   </div>
                   <div class="mi-form-item">
                      <label class="mi-label"> 上传图片</label>
                      <input type="hidden" id="Hidden1" value="" />
                      <img alt="" class="headPortraitImgSrc" id="headPortraitImg" src="/images/defaultThumb.png" />
                      <br/><%=bgtishi%>

                      <ul>
                                        <li style="height: 20px; margin-left: 10px;float:left;">
                                            <div class="C_verify">
                                                <span>
                                                    <uc1:uploadFile ID="headPortrait" runat="server" />
                                                </span>
                                            </div>
                                        </li>

                                        <li style="height: 20px; margin-left: 20px; float:left; display:none;">
                                            <div class="C_verify">
                                                <span>
                                                    <input type="button" name="selectimglibrary" id="selectimglibrary" value="  图片库选择图片  " class="buttonblue-a" />
                                                </span>
                                            </div>
                                        </li>
                                    </ul>
                   </div>
                   <div class="mi-form-explain"></div>
                   <div class="mi-form-explain"></div>
               </div>





                <table width="780" class="grid">
                    <tr>
                        <td height="80" colspan="2" align="center">
                            <input type="button" name="button" id="button" value="  确认提交  "   class="mi-input">
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

    <input type="hidden" id="hid_id" value="<%=id %>" />
    <input type="hidden" id="hid_typeid" value="<%=typeid %>" />
    <input type="hidden" id="hid_imgurl" value="" />
        <input type="hidden" id="hid_modelid" value="<%=Modelid %>" />
</asp:Content>
