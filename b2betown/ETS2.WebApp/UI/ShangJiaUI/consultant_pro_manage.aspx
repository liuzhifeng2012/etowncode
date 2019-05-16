<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="consultant_pro_manage.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.consultant_pro_manage" %>

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
//            $("#linkurl").attr("disabled", "disabled");

            $.post("/JsonFactory/DirectSellHandler.ashx?oper=getconsultantbyid", { comid: comid, id: id }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    //$.prompt("获取图片列表出错");
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
                        $("#textsalepromotetypeid").val(data.msg.Linktype); 
                        
                        $("input:radio[name='outdata'][value=" + data.msg.Outdata + "]").attr("checked", true);

                        if(data.msg.Outdata==0){
                            $("#proclass").show();
                            $("#outproclass").hide();
                            $("#wenzhangclass").hide();
                        }else if(data.msg.Outdata==1){
                            $("#proclass").hide();
                            $("#outproclass").show();
                            $("#wenzhangclass").hide();
                            $("#outpromotetypeid").val(data.msg.Linktype); 

                        }else if(data.msg.Outdata==2){
                            $("#proclass").hide();
                            $("#outproclass").hide();
                            $("#wenzhangclass").show();
                            $("#wenzhangid").val(data.msg.Linktype); 
                        }
                        else if(data.msg.Outdata==3){
                            $("#proclass").hide();
                            $("#outproclass").hide();
                            $("#wenzhangclass").hide();
                            
                        }
                        else if(data.msg.Outdata==4){
                            $("#proclass").hide();
                            $("#outproclass").hide();
                            $("#wenzhangclass").hide();
                          
                        }
                         $("#hid_projectid").val(data.msg.Linktype); 
                    

                        if (data.msg.Linktype != 0) {
                            $("#linkurl").attr("disabled", "disabled");
                        };


                    }

                        if(data.Prolist != null){
                            for (var i = 0; i < data.Prolist.length; i++) {
                                   $("#proid").append('<option value="' + data.Prolist[i].Id + '" selected="selected">' + data.Prolist[i].Pro_name + '</option>');
                            }
                        }
                        if(data.WxMaterial != null){
                            for (var i = 0; i < data.WxMaterial.length; i++) {
                                   $("#MaterialId").append('<option value="' + data.WxMaterial[i].MaterialId + '" selected="selected">' + data.WxMaterial[i].Title + '</option>');
                            }
                        }
                        

                }
            })

            var seled=$("#hid_projectid").val();


            $("#projectid").append('<option value="0"  >请选择项目</option>');
            //加载项目类目
            $.post("/JsonFactory/ProductHandler.ashx?oper=projectlist", { comid:  $("#hid_comid").trimVal() ,prosort:1,projectstate:1}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                }
                if (data.type == 100) {
                  
                    if (data.totalCount > 0) {
                        for (var i = 0; i < data.totalCount; i++) {
                            if (data.msg[i].Id == seled) {
                                $("#projectid").append('<option value="' + data.msg[i].Id + '" selected="selected">' + data.msg[i].Projectname + '</option>');
                            } else {
                                $("#projectid").append('<option value="' + data.msg[i].Id + '"  >' + data.msg[i].Projectname + '</option>');
                            }
                        }
                    }
                }
            })


              $("#projectid").change(function () {
                     var projectid_temp=$("#projectid").val();
                     //加载产品
                     $.post("/JsonFactory/ProductHandler.ashx?oper=pagelistname", { comid:  $("#hid_comid").trimVal(),projectid:projectid_temp,pro_state:1,pageindex:1,pagesize:100}, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                        }
                        if (data.type == 100) {
                            $("#proid").empty();
                            if (data.totalCount > 0) {
                                for (var i = 0; i < data.msg.length; i++) {
                                    if (data.msg[i].Id == seled) {
                                        $("#proid").append('<option value="' + data.msg[i].Id + '" selected="selected">' + data.msg[i].Pro_name + '</option>');
                                    } else {
                                        $("#proid").append('<option value="' + data.msg[i].Id + '"  >' + data.msg[i].Pro_name + '</option>');
                                    }
                                }


                            }
                        }
                    })



              })



            //动态获取全部微信素材类型
            $.post("/jsonfactory/WeiXinHandler.ashx?oper=GetAllWxMaterialType", { comid: $("#hid_comid").trimVal() }, function (data) {

                data = eval("(" + data + ")");

                if (data.type == 1) {
                    $.prompt("操作出现错误" + data.msg);
                    return;
                }
                if (data.type == 100) {

                    if (data.totalCount > 0) {
                        $("#tdgroups").html("");

                        var groupstr = '<option value="0">请选文章类型：</option>';
                        for (var i = 0; i < data.totalCount; i++) {
                            if (data.msg[i].Id == proclass) {
                                groupstr += '<option value="' + data.msg[i].Id + '"  selected="selected">' + data.msg[i].TypeName  + '</option>';
                            }else {
                                groupstr += '<option value="' + data.msg[i].Id + '">' + data.msg[i].TypeName  + '</option>';
                            }
                        }
                        $("#wenzhangid").html(groupstr);
                    }

                }
            });

            $("#wenzhangid").change(function () {
                     var wenzhangid_temp=$("#wenzhangid").val();

                     if(wenzhangid_temp==0){
                        wenzhangid_temp=1000000;
                     }

                     //加载产品
                     $.post("/JsonFactory/WeiXinHandler.ashx?oper=pagelist", { comid:  $("#hid_comid").trimVal(),promotetypeid:wenzhangid_temp,pro_state:1,pageindex:1,pagesize:100,applystate:10}, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                        }
                        if (data.type == 100) {
                            $("#MaterialId").empty();
                            if (data.totalCount > 0) {
                                for (var i = 0; i < data.msg.length; i++) {
                                        $("#MaterialId").append('<option value="' + data.msg[i].MaterialId + '"  >' + data.msg[i].Title + '</option>');
                                }
                            }
                        }
                })
            });





            $("#button").click(function () {
              
                var name = $("#name").trimVal();
                if (name == "") {
                    $.prompt("名称不可为空");
                    return;
                }
                var linkurl = $("#linkurl").trimVal();
                
                var imgurl = $("#<%=headPortrait.FileUploadId_ClientId %>").val();

                if (imgurl == "") {
                    imgurl = $("#hid_imgurl").trimVal();
                }
                var outdata = $('input[name=outdata]:checked').val();

                var fonticon = $("#tab").attr('class');
                var textsalepromotetypeid = $("#projectid").trimVal();
                var proid = $("#proid").trimVal();
                var MaterialId= $("#MaterialId").trimVal();

                if(outdata ==1){
                    textsalepromotetypeid = $("#outpromotetypeid").trimVal();
                }

                if(outdata ==2){
                    textsalepromotetypeid = $("#wenzhangid").trimVal();
                }
                //顾问空间、
                if(outdata ==3){
                    textsalepromotetypeid = 0;
                }
                //顾问咨询
                if(outdata ==4){
                    textsalepromotetypeid = 0;
                }

                $.post("/JsonFactory/DirectSellHandler.ashx?oper=editconsultant", { id: $("#hid_id").val(), comid: comid, linktype: textsalepromotetypeid, name: name, linkurl: linkurl,fonticon:fonticon,imgurl: imgurl,outdata:outdata,proid:proid,MaterialId:MaterialId}, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("添加出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("添加成功");
                        location.href = "consultant_pro.aspx";
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

             $("#oudate_no").click(function () {
                $("#proclass").show();
                $("#outproclass").hide();
                $("#wenzhangclass").hide();
             })

             $("#oudate_yes").click(function () {
                $("#proclass").hide();
                $("#outproclass").show();
                $("#wenzhangclass").hide();
             })

             $("#wenzhang").click(function () {
                $("#proclass").hide();
                $("#outproclass").hide();
                $("#wenzhangclass").show();
             })
             $("#guwen_space").click(function(){
                 $("#proclass").hide();
                $("#outproclass").hide();
                $("#wenzhangclass").hide();
             })
              $("#guwen_consult").click(function(){
                $("#proclass").hide();
                $("#outproclass").hide();
                $("#wenzhangclass").hide();
             })
              $("#guwen_pingjia").click(function(){
                $("#proclass").hide();
                $("#outproclass").hide();
                $("#wenzhangclass").hide();
             })
        })
        
        

        //图标库
        var libraryfont_html = "";
                var pageSize = 36; //每页显示条数
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


        //选择图标
        function OnSelectFont(icon) {
            $("#tab").removeClass().addClass(icon);
            $("#fontlibrary").hide();
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
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/shangjiaui/ShopManage.aspx" onfocus="this.blur()" target="">微商城设置</a></li>
                <li class="on"><a href="/ui/shangjiaui/consultant_pro.aspx" onfocus="this.blur()" target="">顾问页面设置</a></li>
                <li><a href="/ui/shangjiaui/StoreList.aspx" onfocus="this.blur()" target="">门店模板设置</a></li>
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
                        <label class="mi-label"> 显示产品来源</label>
                          <label ><input type="radio" id="oudate_no" name="outdata" value="0" checked/>内部产品</label > 
                          <label ><input type="radio" id="wenzhang" name="outdata" value="2"/>微信文章</label > 
                          <label>  <input type="radio" id="oudate_yes" name="outdata"  value="1" /> 外部数据</label >
                          <label>  <input type="radio" id="guwen_space" name="outdata"  value="3" /> 顾问空间</label >
                          <label>  <input type="radio" id="guwen_consult" name="outdata"  value="4" /> 顾问咨询</label >
                          <label>  <input type="radio" id="guwen_pingjia" name="outdata"  value="5" /> 服务评价</label >
                          
                   </div>
                   <div class="mi-form-item" id="proclass">
                        <label class="mi-label">  连接到项目</label>
                           <select id="projectid" >
                            </select>
                        <label class="mi-label">  请选择产品</label>
                           <select id="proid" size="6" multiple="multiple">
                            </select>

                            <br>（当您未选取具体产品，默认显示所选择的项目下所有有效产品，如果选择产品 ，只显示选择的产品，按Ctrl 可以进行多选。当需要选择不同项目的产品，选择 “请选择产品”时会列出最新100个产品）
                   </div>
                   <div class="mi-form-item" id="outproclass" style=" display:none;">
                        <label class="mi-label"> 连接到产品分类</label>
                           <select id="outpromotetypeid" >
                           <option value="1">周边</option>
                           <option value="2">国内</option>
                           <option value="4">出境</option>
                            </select>
                   </div>
                   <div class="mi-form-item" id="wenzhangclass" style=" display:none;">
                        <label class="mi-label"> 连接到文章分类</label>
                           <select id="wenzhangid" >
                            </select>
                            <label class="mi-label">请选择具体文章</label>
                           <select id="MaterialId"  size="6" multiple="multiple">
                            </select>
                            <br>（当您未选取文章，默认显示所选择的分类文章，如果选择文章 ，只显示选择的文章，按Ctrl 可以进行多选。当需要选择不同分类的文章，选择 “请选择具体文章”时会列出最新100个文章）
                   </div>

                   <div class="mi-form-explain"></div>
               </div>
               <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10; display:none">
                <h2 class="p-title-area">栏目图标</h2>

                   <div class="mi-form-item"  id="fonttab">
                        <label class="mi-label"> 按钮图片</label>
                        <table>
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

                                    </ul>
                                </div>
                            </div>
                        </td>
                    </tr>
                        </table>
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
    <input type="hidden" id="hid_imgurl" value="<%=headPortraitImgSrc %>" />
    <input type="hidden" id="hid_projectid" value="" />
</asp:Content>
