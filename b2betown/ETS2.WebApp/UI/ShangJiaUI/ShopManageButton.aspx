<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master"  CodeBehind="ShopManageButton.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.ShopManageButton" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link href="/Styles/shopmanage/css.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        $(function () {
            var pageSize = 12; //每页显示条数


            //读取菜单类型
            $.post("/JsonFactory/ModelHandler.ashx?oper=modelzhidingpagelist", { id: 0 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                    for (i = 0; i < data.totalCount; i++) {
                        $("#linktype").append("<option value='" + data.msg[i].Id + "'>" + data.msg[i].Name + "</option>");
                    }
                }
            })

            $(".editconent").hover(function () {
                $(this).addClass("hoveractions");
            }, function () {
                $(this).removeClass("hoveractions");
            });

            //点击后修改内容
            $(".editconent").click(function () {
                var upcontentid = $(this).attr("id"); //修改内容id

                edittype(upcontentid);


                $(".editconent").removeClass("clickactions");
                $(this).addClass("clickactions");
                $(".app-sidebar").show();
                var top = 0;
                var yuluoffset = $(this).offset();
                top = yuluoffset.top;
                //alert(top);
                if (top < 0) {
                    top = top - 200;
                }
                top = Math.abs(top - 200);
                // $(".app-sidebar").css("margin-top", top);

            });


            $(".editconent").live({
                mouseenter:
                   function () {
                       $(this).addClass("hoveractions");
                   },
                mouseleave:
                   function () {
                       $(this).removeClass("hoveractions");

                   }
            })


            //针对js加载html 点击效果
            $(".editconent").live("click", function () {
                var upcontentid = $(this).attr("id"); //修改内容id

                edittype(upcontentid);

                $(".editconent").removeClass("clickactions");
                $(this).addClass("clickactions");

                $(".app-sidebar").show();
                var top = 0;
                var yuluoffset = $(this).offset();
                top = yuluoffset.top;
                // alert(top);
                if (top < 0) {
                    top = top - 200;
                }

                top = Math.abs(top - 200);
                //$(".app-sidebar").css("margin-top", top);
            });





            //根据当前用户获得商家信息---需要处理
            $.ajax({
                type: "post",
                url: "/JsonFactory/AccountInfo.ashx?oper=getcurcompany",
                data: { comid: $("#hid_comid").trimVal() },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        // $.prompt("获取信息出错，" + data.msg);
                    }
                    if (data.type == 100) {
                        if (data.msg != null) {
                            $("#title").html(data.msg.Com_name);
                            $("#title_txt").val(data.msg.Com_name);
                        }
                    }
                }
            })

            //修改标题
            $("#subtitle").click(function () {
                var title = $("#title_txt").val();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AccountInfo.ashx?oper=editComName",
                    data: { comid: $("#hid_comid").trimVal(), com_name: title },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            alert("出错，" + data.msg);
                        }
                        if (data.type == 100) {
                            $(".app-sidebar").hide();
                            $("#title").html(title);

                        }
                    }
                })
            })


            var comid = $("#hid_comid").trimVal();

            $("#erweima").html("扫二维码访问<br><img src='../PMUI/ETicket/showtcode.aspx?pno=http://shop" + comid + ".etown.cn/h5/order/'  '/>");
            SearchList(1);

            //装载列表
            function SearchList(pageindex) {
                var html_str = "";

                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/DirectSellHandler.ashx?oper=getbuttonlist",
                    data: { comid: comid, pageindex: pageindex, pagesize: 3 },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            //$.prompt("查询错误");
                            return;
                        }
                        if (data.type == 100) {
                            if (data.totalCount == 0) {
                                for (var i = 0; i < 3; i++) {
                                    html_str += '<div class="nav-item-www ">';
                                    html_str += '  <a class="mainmenu editconent" id="0" href="javascript:;">';
                                    html_str += '  <span class="mainmenu-txt">菜单名称</span>    </a>';
                                    html_str += '<div class="submenu js-submenu">';
                                    html_str += '     <span class="arrow before-arrow"></span>';
                                    html_str += '      <span class="arrow after-arrow"></span>';
                                    html_str += '       <div class="js-nav-2nd-region"><ul></ul></div>';
                                    html_str += '   </div>';
                                    html_str += ' </div>';
                                }

                                $("#buttonlist").html(html_str);
                            } else {

                                for (var i = 0; i < data.msg.length; i++) {
                                    html_str += '<div class="nav-item-www ">';
                                    html_str += '  <a class="mainmenu editconent"  id="' + data.msg[i].Id + '" href="javascript:;">';
                                    html_str += '  <span class="mainmenu-txt">' + data.msg[i].Name + '</span>    </a>';
                                    html_str += '<div class="submenu js-submenu">';
                                    html_str += '     <span class="arrow before-arrow"></span>';
                                    html_str += '      <span class="arrow after-arrow"></span>';
                                    html_str += '       <div class="js-nav-2nd-region"><ul></ul></div>';
                                    html_str += '   </div>';
                                    html_str += ' </div>';
                                }

                                if (data.msg.length < 3) {
                                    for (var i = 0; i < 3 - data.msg.length; i++) {
                                        html_str += '<div class="nav-item-www ">';
                                        html_str += '  <a class="mainmenu editconent"  id="0" href="javascript:;">';
                                        html_str += '  <span class="mainmenu-txt">菜单名称</span>    </a>';
                                        html_str += '<div class="submenu js-submenu">';
                                        html_str += '     <span class="arrow before-arrow"></span>';
                                        html_str += '      <span class="arrow after-arrow"></span>';
                                        html_str += '       <div class="js-nav-2nd-region"><ul></ul></div>';
                                        html_str += '   </div>';
                                        html_str += ' </div>';
                                    }
                                }
                                $("#buttonlist").html(html_str);

                            }

                        }
                    }
                })
            }


            $("#viewsite").click(function () {

                var h = 680;
                var w = 430;
                var t = screen.availHeight / 2 - h / 2;
                var l = screen.availWidth / 2 - w / 2;
                var prop = "dialogHeight:" + h + "px; dialogWidth:" + w + "px; dialogLeft:" + l + "px; dialogTop:" + t + "px;toolbar:no; menubar:no; scrollbars:yes; resizable:no;location:no;status:no;help:no";
                var path = "http://shop" + comid + ".etown.cn/h5/";
                var ret = window.showModalDialog(path, "", prop);

            })


            function edittype(type) {
                //先初始化都隐藏
                $("#edittitle").hide();
                $("#editbutton").hide();

                if (type == "title") {
                    $("#editbutton").show();
                }
                else {
                    $("#editbutton").show();
                    readerbanner(type);
                }
            }

            //读取
            function readerbanner(id) {

                var comid = $("#hid_comid").trimVal();
                $("#hid_id").val(id);
                if (id != 0) {
                    $.post("/JsonFactory/DirectSellHandler.ashx?oper=getbuttonbyid", { id: id, comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt("操作出现错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#name").val(data.msg.Name);
                            $("#Linkurl").val(data.msg.Linkurl);
                            $("#linktype").val(data.msg.Linkurlname);
                            $("#sort").val(data.msg.Sort);
                            return;
                        }
                    })
                } else {
                    $("#name").val("");
                    $("#Linkurl").val("");
                }
            }



            $("#selectlink1").click(function () {
                $("#linktype_2").hide();
                $("#linktype_1").show();
            })
            $("#selectlink2").click(function () {
                $("#linktype_1").hide();
                $("#linktype_2").show();
            })



            $("#subbutton").click(function () {

                var name = $("#name").trimVal();
                if (name == "") {
                    $.prompt("标题不可为空");
                    return;
                }
                var linkurl = $("#Linkurl").trimVal();
                if (linkurl == "") {
                    $.prompt("请填写要连接的地址或选择链接到的功能区");
                    return;
                }
                var linkurlname = $("#linktype").trimVal();


                $.post("/JsonFactory/DirectSellHandler.ashx?oper=editbutton", { id: $("#hid_id").val(), comid: comid, name: name, linkurl: linkurl, linkurlname: linkurlname }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        alert("操作成功");
                        location.href = "ShopManageButton.aspx";
                        return;
                    }
                })

            })

             $("#delbutton").click(function () {
              $.post("/JsonFactory/DirectSellHandler.ashx?oper=deletebutton", { id: $("#hid_id").val(), comid: comid}, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        alert("删除操作成功");
                        location.href = "ShopManageButton.aspx";
                        return;
                    }
                })

             })


            //当选择指定栏目
            $("#linktype").change(function () {
                var linktype_temp = $("#linktype").val();

                if (linktype_temp != 0) {
                    $.post("/JsonFactory/ModelHandler.ashx?oper=modelzhidingpagelist", { id: linktype_temp }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            for (i = 0; i < data.totalCount; i++) {
                                $("#Linkurl").val(data.msg[i].Linkurl);
                                $("#Linkurl").attr("disabled", "disabled");
                            }
                        }
                    })
                }
            })


        })

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="/ui/shangjiaui/ShopManage.aspx" onfocus="this.blur()" target="">微商城设置</a></li>
              
                <li><a href="/ui/shangjiaui/consultant_pro.aspx" onfocus="this.blur()" target="">顾问页面设置</a></li>
                <li><a href="/ui/shangjiaui/StoreList.aspx" onfocus="this.blur()" target="">门店模板设置</a></li>
                <li><a href="/ui/shangjiaui/H5Default.aspx" onfocus="this.blur()" target="">微站形象首页管理</a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                
<div class="app-design clearfix without-add-region"><div class="widget-app-board ui-box">
    <div class="widget-app-board-info">
        <div>
          <a href="/ui/shangjiaui/ShopManage.aspx" class="a_anniu">微商城设置</a>  <a href="/ui/shangjiaui/ShopManageButton.aspx" class="a_anniu">底部菜单设定</a>
            <p>点击菜单，修改菜单文字及连接。</p>
        </div>
        <div style=" float:right;position: fixed;top: 125px;right:30px;">
         <div style="" id="erweima"></div>
        </div>
    </div>
    <div class="widget-app-board-control">
        <label class="js-switch ui-switcher pull-right ui-switcher-on"></label>
    </div>
</div>

<div class="app-preview">
    <div class="app-header"></div>
    <div class="app-entry" style="border:none;">
        <div class="app-config js-config-region"><div class="app-field clearfix editing"><h1 >
    <span class="editconent" id="title" >[页面标题]</span>
</h1>




<div class="preview-nav-menu">
        <div class="js-navmenu nav-show nav-menu-1 nav-menu has-menu-3 ">
            <div class="nav-special-item nav-item">
                <a href="javascript:;" class="home">主页</a>            
			</div>
            <div class="js-nav-preview-region nav-items-wrap">
				<div class="nav-items" id="buttonlist">
					
				</div>
			</div>

        </div>
</div>


</div></div>
       
    </div>
    <div class="js-add-region"><div></div></div>
   
</div>

<div class="app-sidebar" style="margin-top:55px; display:none;">
    <div class="arrow"></div>
    <div class="app-sidebar-inner js-sidebar-region"><div>
    <div class="control-group" id="edittitle"  style="display:none;">
        <label class="control-label">修改商户名称（标题）：</label>
        <div class="controls">
            <p class="help-desc"><input type="text" class="mi-input" id="title_txt" value=""/></p>
        </div>
        <div class="btnconfirm">
        <a href="javascript:;" class="js-confirm-it btn btn-block btn-orange-dark" id="subtitle">确认，保存</a>
        </div>
    </div>


<div class="js-goods-style-region" style="margin-top: 10px;"><div>


     <div class="control-group"  id="editbutton" style="display:none;">
       <label class="control-label">名称：</label>
        <div class="controls">
             <input name="button_name" type="text" id="button_name" size="25" class="mi-input"  value="" style="width:200px;"/>
        
        </div>
         <label class="control-label">连接地址：</label>
        <div class="controls">
          <input name="button_Linkurl" type="text" id="button_Linkurl"  size="25" class="mi-input"  value="" style=" background:#ccc"/>
        </div>

       <label class="control-label">链接到：</label>
          <div class="controls">
        <div id="linktype_2" style="display: block;">
                            <select class="mi-input" id="button_linktype"  style="margin-left: 0;">
                            <option value="" >请选择链接到的功能区</option>
                            </select>
                            
        </div>
        <label class="control-label">顺序：</label>
          <div class="controls">
        
                            <select class="mi-input" id="button_sort"  style="margin-left: 0;">
                            <option value="0" >1</option>
                            <option value="1" >2</option>
                            <option value="2" >3</option>
                            </select>
                            
         </div>
            <div class="btnconfirm">
                <a href="javascript:;" class="js-confirm-it btn btn-block btn-orange-dark" id="subbutton">确认，保存</a>
                 <a href="javascript:;" style=" margin:5px 0; background-color:#cccccc;" class="js-confirm-it btn btn-block " id="delbutton"" onclick="Deletebanner()">删除此项</a>
            </div>
    
        </div>

    </div>


</div></div>
</div>


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
        <div id="fontlibrary" style="background-color: #ffffff; border: 2px solid #5984bb; width: 420px; height: 500px; display: none; position:fixed;margin:auto;left:0; right:0; top:0; bottom:0; z-index: 1500;  ">
        <div style=" height:20px;padding:5px;text-align: right; "><div  style="float:left;">请双击图标选择</div><div  id="closefontlibrary" style="cursor:pointer; float:right;">×</div></div>
        <div id="fontlibrarytext"style="width: 410px; height: 430px;"></div>
        <div id="divPage1" style="width: 410px; height:25px;">
        </div>
    </div>

    <input type="hidden" id="hid_id" value="0" />
</asp:Content>