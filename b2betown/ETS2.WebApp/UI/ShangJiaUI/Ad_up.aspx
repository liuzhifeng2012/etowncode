<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="Ad_up.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.Ad_up" %>

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

                $.post("/JsonFactory/WeiXinHandler.ashx?oper=Getwxad", { comid: comid, id: id }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        //$.prompt("获取图片列表出错");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg != null) {
                            $("#hid_id").val(data.msg.Id);
                            $("#hid_com_id").val(data.msg.Com_id);
                            $("#Title").val(data.msg.Title);
                            $("#Link").val(data.msg.Link);
                            $("#hid_Musicid").val(data.msg.Musicid);

                            $("input:radio[name='Adtype'][value=" + data.msg.Adtype + "]").attr("checked", true);

                        }
                    }
                })


            $("#button").click(function () {
              
                var Title = $("#Title").trimVal();
                if (Title == "") {
                    $.prompt("标题不可为空");
                    return;
                }
                var Link = $("#Link").trimVal();
                
                var Musicid = $("#<%=headPortrait.FileUploadId_ClientId %>").val();

                if (Musicid == "") {
                    Musicid = $("#headPortraitImg").trimVal();
                }
                var Adtype = $('input[name=Adtype]:checked').val();

               
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=editad", { id: $("#hid_id").val(), comid: comid, Link: Link, Title: Title, Musicid: Musicid,Adtype:Adtype}, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("添加出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("添加成功");
                        location.href = "Ad_manage.aspx";
                        return;
                    }
                })
            })



            $("#audio_btn").click(function () {
                var audioEle = $("#headPortraitImg")[0];
                if ($('#audio_btn').is('.rotate')) {
                    audioEle.pause(); //暂停
                    $('#audio_btn').removeClass('rotate')
                } else {
                    audioEle.play(); //播放
                    $('#audio_btn').addClass('rotate')
                }
            })

        })
        
        



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

    <style>
      #audio_btn {
    
    right: 20px;
    top: 20px;
    z-index: 200;
    display: none;
    width: 30px;
    height: 30px;
    background-repeat: no-repeat;
	}
	.off {
		background-image: url(/images/normalmusic.svg);
		background-size: contain;
		background-repeat: no-repeat;
	}
	.rotate {
    -webkit-animation: rotating 1.2s linear infinite;
    -moz-animation: rotating 1.2s linear infinite;
    -o-animation: rotating 1.2s linear infinite;
    animation: rotating 1.2s linear infinite;
	}

	
	@-webkit-keyframes rotating {
		from {
			-webkit-transform:rotate(0deg)
		}
		to {
			-webkit-transform:rotate(360deg)
		}
	}
	@keyframes rotating {
		from {
			/*! transform:rotate(0deg) */
		}
		to {
			/*! transform:rotate(360deg) */
		}
	}
	@-moz-keyframes rotating {
		from {
			-moz-transform:rotate(0deg);
		}
		to {
			 -moz-transform:rotate(360deg); 
		}
	}
    </style>
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
                   <h2 class="p-title-area">图片滚动广告管理</h2>
                   <div class="mi-form-item">
                        <label class="mi-label"> 标题</label>
                       <input name="name" type="text" id="Title"  size="25" class="mi-input" value="<%=Title %>" style="width:200px;"/>
                   </div>
                    <div class="mi-form-item">
                        <label class="mi-label"> 类型</label>
                          <label ><input type="radio" id="Adtype" name="Adtype" value="0" checked/>链接</label > 
                       
                          
                          
                   </div>
                   <div class="mi-form-item" id="proclass">
                         <label class="mi-label"> 链接地址</label>
                       <input name="name" type="text" id="Link"  size="25" class="mi-input" value="<%=Link %>" style="width:200px;"/></div>
                  
                   <div class="mi-form-explain"></div>
               </div>
               <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10; display:;">
                <h2 class="p-title-area">背景音乐</h2>

                   <div class="mi-form-item"  id="fonttab">
                        <label class="mi-label"> 点击音乐进行播放</label>
                        <table>
                        <tr id="img">
                        <td class="tdHead">
                            上传MP3 ：
                        </td>
                        <td>
                            <div class="C_head">
                                <dl>
                                    <dt>
                                        <input type="hidden" id="Hidden1" value="" />
                                       <div style="display: block;" id="audio_btn" class="off video_exist ">
                                                     <audio loop="" src="<%=Musicscr %>" id="headPortraitImg"  preload=""></audio>
                                        </div>
                                         
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
    <input type="hidden" id="hid_Musicid" value="<%=headPortraitImgSrc %>" />
</asp:Content>
