<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="news_edit_multi.aspx.cs"
    Inherits="ETS2.WebApp.WeiXin.masssend.news.news_edit_multi" MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var createuserid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();

            $.post("/JsonFactory/WeiXinHandler.ashx?oper=Getwxqunfa_newslist", { opertype: $("#hid_opertype").trimVal(), news_recordid: $("#hid_news_recordid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) { //newsid为0-添加；newsid>0 当前news为编辑
                    if ($("#hid_opertype").trimVal() == 1) {
                        $("#hid_canadd").val("1");
                    } else {
                        $("#hid_canadd").val("0");
                    }

                    var retdate = { newss: data.msg, newsid: $("#hid_newsid").trimVal(), opertype: $("#hid_opertype").trimVal() };
                    $("#accordion").empty();
                    $("#ProductItemEdit").tmpl(retdate).appendTo("#accordion");


                    //除了opertype=2 编辑特定素材 展示 外；其他类型则都是最后一个素材展示，其他隐藏起来
                    if ($("#hid_opertype").trimVal() == 2) {
                        $("#divbox" + $("#hid_newsid").trimVal()).show();

                        loadTiny('txtcontent' + $("#hid_newsid").trimVal());
                    } else {
                        $("[name='divbox']").last().show();
                        loadTiny('txtcontent' + $("[name='divbox']").last().attr("item"));
                    }
                }
                else {
                    alert("加载错误");
                    return;
                }
            })

            //上传到微信服务器
            $("#btnsave").click(function () {
                if (parseInt($("[name='divbox']").length) < 2) {
                    alert("多图文消息至少应含有2条图文");
                    return;
                }
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=uploadwxqunfa_news_multi", { news_recordid: $("#hid_news_recordid").trimVal(), comid: $("#hid_comid").trimVal() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) { }
                    if (data.type == 100) {
                        alert("上传到微信服务器成功");
                        window.open("news_list.aspx", target = "_self");
                    }
                });
            })
        })
        function getImg(imgurl) {
            if (imgurl == ""||imgurl==null) {
                return "http://image.etown.cn/UploadFile//images/defaultThumb.png";
            } else {
                return imgurl
            }
        }
        //新建图文
        function newcreatenews() {
            //判断图文素材不可超过10条
            if (parseInt($("[name='divbox']").length) == 10) {
                alert("一个图文消息不可超过10条图文");
                return;
            }

            if ($("#hid_canadd").val() == 1) {
                window.open("news_edit_multi.aspx?newsid=0&news_recordid=" + $("#hid_news_recordid").trimVal() + "&opertype=0", target = "_self");
            } else {
                alert("请先把图文消息编辑完");
                return;
            }
        }
        function accordionclick(obj) {
            var hideattr = $(obj).next("div").css("display");

            if (hideattr == "none") {
                $(obj).next("div").show();
            } else {
                $(obj).next("div").hide();
            }
        }
        function delitem(newsid) {
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=delwxqunfa_news", { newsid: newsid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("删除有误");
                    return;
                }
                if (data.type == 100) {
                    alert("删除成功");
                    window.open("news_edit_multi.aspx?newsid=0&news_recordid=" + $("#hid_news_recordid").trimVal() + "&opertype=1", target = "_self");
                }
            })
        }
        function saveitem(newsid) {
            //保存素材时用 服务器控件的值；全部保存上传微信服务器时用 一般html控件的值
            var thumb_url = $("#Image1").attr("src");
            var thumb_media_id = $("#hid_thumb_media_id").trimVal();

            if (thumb_media_id == "") {
                alert("封面图片必须上传!");
                return;
            }
            var txttitle = $("#txttitle" + newsid).trimVal();
            if (txttitle == "") {
                alert("标题不可为空!");
                return;
            }
            else {
                if (parseInt(GetLength(txttitle)) > 128) {
                    alert("标题字数不可超过64字");
                    return;
                }
            }



            var txtauthor = $("#txtauthor" + newsid).trimVal();
            if (txtauthor != "") {
                if (parseInt(GetLength(txtauthor)) > 16) {
                    alert("作者字数不可超过8字");
                    return;
                }
            }
            var txtdigest = $("#txtdigest" + newsid).trimVal();
            if (txtdigest != "") {
                if (parseInt(GetLength(txtdigest)) > 240) {
                    alert("摘要字数不可超过120字");
                    return;
                }
            }
            var txtcontent = $("#txtcontent" + newsid).val();
            if (txtcontent == "") {
                alert("图文消息内容不可为空");
                return;
            }
            var sel_show_cover_pic = $("#sel_show_cover_pic" + newsid).val();
            var txtcontent_source_url = $("#txtcontent_source_url" + newsid).val();

            $.post("/JsonFactory/WeiXinHandler.ashx?oper=savewxqunfa_multinews", { newsid: newsid, news_recordid: $("#hid_news_recordid").trimVal(), createuserid: $("#hid_userid").trimVal(), comid: $("#hid_comid").trimVal(), txttitle: txttitle, txtauthor: txtauthor, txtdigest: txtdigest, thumb_media_id: thumb_media_id, sel_show_cover_pic: sel_show_cover_pic, txtcontent: txtcontent, txtcontent_source_url: txtcontent_source_url, thumb_url: thumb_url, materialid: $("#hid_materialid").val() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    return;
                }
                if (data.type == 100) {
                    alert("编辑图文消息成功");
                    window.open("news_edit_multi.aspx?newsid=0&news_recordid=" + data.msg + "&opertype=1", target = "_self");
                }
            })
        }
        function loadTiny(objectId) {
            $('#' + objectId).tinymce({
                // Location of TinyMCE script
                script_url: '/Scripts/tiny_mce/tiny_mce.js',
                width: '422',
                height: '120',
                // General options
                theme: "advanced",
                language: 'cn',
                plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,advlist",

                // Theme options
                theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,fontselect,fontsizeselect,|,forecolor,backcolor,|,insertdate,image,preview,code",
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

        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/weixin/masssend/send.aspx" onfocus="this.blur()"><span>新建群发消息</span></a></li>
                <li><a href="/weixin/masssend/list.aspx" onfocus="this.blur()">已发送</a></li>
                <li class="on"><a href="news_list.aspx" onfocus="this.blur()">图文信息管理</a> </li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <div style="width: 600px; height: 40px; padding-left: 10px; border: 1px solid #FF4444;
                    display: none;" id="viewrenzheng">
                    <div style="padding: 12px 0 14px 0;">
                        <h5 class="text-overflow">
                            <a smartracker="on" seed="contentList-mainLinkbox" href="http://shop1143.etown.cn/v/about.aspx?id=2661"
                                target="_blank" style="font-size: 16px">该功能需将微信服务号进行认证后才能使用 ,点击查看如何进行认证... </a>
                        </h5>
                    </div>
                    <div class="content-list-des text-overflow">
                    </div>
                </div>
                <h2 style="font-size: 16px; font-weight: 800; height: 24px; line-height: 24px; padding: 5px 0px;
                    color: #4D4D4D;">
                    新建多图文消息</h2>
                <div id="accordion">
                </div>
                <table border="0">
                    <tbody>
                        <tr>
                            <td width="600" height="80" align="center">
                                <input type="hidden" id="hid_materialid" value="<%=materialid %>" />
                                <input type="hidden" id="hid_newsid" value="<%=newsid %>" />
                                <input type="hidden" id="hid_news_recordid" value="<%=news_recordid %>" />
                                <input type="hidden" id="hid_opertype" value="<%=opertype %>" />
                                <!--可否进行新增操作-->
                                <input type="hidden" id="hid_canadd" value="0" />
                                <input type="button" class="mi-input" value="  新建图文  " style="margin-top: 2px; float: left;
                                    width: 100px;" onclick="newcreatenews()" />
                                <input type="button" class="mi-input" value="  保存并上传  " id="btnsave" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="mi-label">
                                    注：编辑完图文素材后必须上传到微信服务器才可群发</label>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">
      {{each(i,mn) newss}}
       <h3 style="font-size: 15px; background-color: #C1D9F3; cursor: pointer; margin: 10px 0;padding:0 10px; border-top-left-radius: 3px;
border-top-right-radius: 3px; width:982px;"
                        onclick="accordionclick(this)">
                        {{if mn.id==0}}
                            新建图文 
                        {{else}}
                            {{if newsid==mn.id}}
                                  ${mn.title} 
                                {{else}}
                                  ${mn.title} 
        <span style="float:right;"><a href="news_edit_multi.aspx?newsid=${mn.id}&news_recordid=${mn.newsrecordid}&opertype=2"  >编辑</a>  <a href="javascript:void(0)"   onclick="delitem('${mn.id}')">删除</a></span>
                            {{/if}}
                        {{/if}}
                    </h3>
                    <div class="edit-box J-commonSettingsModule" style="opacity: 1; padding: 10px 0;
                        position: relative; z-index: 10; width: 1000px; margin-left: 0; display:none; " id="divbox${mn.id}" name="divbox" item="${mn.id}">
                        <div class="mi-form-item">
                            <label class="mi-label">
                                封面</label>
                            
                        {{if (mn.id!=0&&newsid!=mn.id)||opertype==1}}
                            <input type="image" name="imagee"  src="${getImg(mn.thumb_url)}">
                        {{else}}
                  <asp:Image ID="Image1" runat="server" ImageUrl="${getImg(mn.thumb_url)}"   ClientIDMode="Static" /><br>

                  <asp:FileUpload ID="FileUpload1"   runat="server"    ToolTip="选择图片" onchange="txt_fileupload.value=this.value"  style="visibility: hidden;" ClientIDMode="Static"/> <br>   
                  <input id="txt_fileupload" type="text"  readonly="readonly"/>
                  <input id="Button_file" type="button" value="选择图片" class="button"  onclick="FileUpload1.click()" />

                   <asp:Button ID="bt_upload" runat="server" Text="上传图片" OnClick="bt_upload_Click"    ClientIDMode="Static"/><br>
                            <asp:Label ID="lb_info" runat="server" ForeColor="Red" Width="515px"  ClientIDMode="Static">图片只支持jpg格式</asp:Label>
                            <br>
                            <asp:HiddenField ID="hid_thumb_media_id" runat="server" ClientIDMode="Static"   Value="${mn.thumb_media_id}" />
                           
                          
                        {{/if}}
                            <input type="hidden" id="hid_image${mn.id}" value=""/>
                            <input type="hidden" id="hid_thumbmediaid${mn.id}" value="">   
                            
                        </div>
                        <div class="mi-form-item" style="display: none">
                            <label class="mi-label">
                                是否显示封面</label>
                                 {{if mn.id==0}}
                                    <select id="sel_show_cover_pic${mn.id}" class="mi-input">
                                            <option value="1">是</option>
                                            <option value="0">否</option> 
                                    </select>
                                 {{else}}
                                        {{if newsid==mn.id}}
                                                    <select id="sel_show_cover_pic${mn.id}" class="mi-input">
                                                    {{if mn.show_cover_pic==1}}
                                                        <option value="1" checked>是</option>
                                                        <option value="0">否</option>
                                                    {{else}}
                                                        <option value="1" >是</option>
                                                        <option value="0" checked>否</option>
                                                    {{/if}}
                                                    </select>
                                        {{else}}
                                                    {{if mn.show_cover_pic==1}}
                                                       是
                                                    {{else}}
                                                       否
                                                    {{/if}}
                                        {{/if}}
                                 {{/if}}
                           
                        </div>
                        <div class="mi-form-item">
                            <label class="mi-label">
                                标题(64字以内)</label>
                                    {{if mn.id==0}}
                                         &nbsp;&nbsp;<input type="text" id="txttitle${mn.id}" value="" size="50" />* <span id="tdchildobj"></span>
                                    {{else}}
                                        {{if newsid==mn.id}}
                                               &nbsp;&nbsp;<input type="text" id="txttitle${mn.id}" value="${mn.title}" size="50" />* <span id="tdchildobj"></span>
                                        {{else}}
                                             &nbsp;&nbsp; ${mn.title} 
                                        {{/if}}
                                    {{/if}}
                                     
                        </div>
                        <div class="mi-form-item">
                            <label class="mi-label">
                                作者(8字以内)</label>
                                {{if mn.id==0}}
                                    &nbsp;&nbsp;<input type="text" id="txtauthor${mn.id}" value="" size="50" />
                                {{else}}
                                    {{if newsid==mn.id}}
                                         &nbsp;&nbsp;<input type="text" id="txtauthor${mn.id}" value="${mn.author}" size="50" />
                                        {{else}}
                                         &nbsp;&nbsp; ${mn.author} 
                                    {{/if}}
                                {{/if}}
                            
                        </div>
                        <div class="mi-form-item">
                            <label class="mi-label">
                                摘要(120字以内)</label>
                          
                                {{if mn.id==0}}
                                     &nbsp;&nbsp;<input type="text" id="txtdigest${mn.id}" value="" size="50" />
                                {{else}}
                                    {{if newsid==mn.id}}
                                          &nbsp;&nbsp;<input type="text" id="txtdigest${mn.id}" value="${mn.digest}" size="50" />
                                        {{else}}
                                         &nbsp;&nbsp; ${mn.digest} 
                                    {{/if}}
                                {{/if}}
                        </div>
                        <div class="mi-form-item">
                            <label class="mi-label">
                                内容</label>
                          
                                 {{if mn.id==0}}
                                     &nbsp;&nbsp;<textarea style="width: 400px;" cols="200" rows="4" id="txtcontent${mn.id}" placeholder=""
                                class="mi-input"></textarea>*
                                {{else}}
                                    {{if newsid==mn.id}}
                                          &nbsp;&nbsp;<textarea style="width: 400px;" cols="200" rows="4" id="txtcontent${mn.id}" placeholder=""
                                class="mi-input">${mn.content}</textarea>*
                                        {{else}}
                                         &nbsp;&nbsp; ${mn.content} 
                                    {{/if}}
                                {{/if}}
                        </div>
                        <div class="mi-form-item">
                            <label class="mi-label">
                                原文链接</label>
                           
                            {{if mn.id==0}}
                               &nbsp;&nbsp; <input type="text" id="txtcontent_source_url${mn.id}" value="" size="50" />
                            {{else}}
                                {{if newsid==mn.id}}
                                     &nbsp;&nbsp;<input type="text" id="txtcontent_source_url${mn.id}" value="${mn.content_source_url}" size="50" />
                                    {{else}}
                                     &nbsp;&nbsp; ${mn.content_source_url} 
                                {{/if}}
                            {{/if}}

                            <br>
                             {{if mn.id==0}}
                                    <input type="button" class="mi-input" value="  保存  "  style="margin-top:10px;width: 100px;cursor:pointer;" onclick="saveitem('${mn.id}')"/>
                                   
                             {{else}}
                                    {{if newsid==mn.id}}
                                          <input type="button" class="mi-input" value="  保存  "  style="margin-top:10px;width: 100px;cursor:pointer;" onclick="saveitem('${mn.id}')"/>
                                         
                                    {{/if}}
                             {{/if}} 
                        </div>
                    </div>

                    
        {{/each}}
    </script>
</asp:Content>
