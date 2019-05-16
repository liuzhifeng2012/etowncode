<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ShopManage.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.ShopManage" %>
<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link href="/Styles/shopmanage/css.css" rel="stylesheet" type="text/css" />

    
    <script src="/Scripts/jquery-ui-1.10.3.custom/ui/jquery.ui.core.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.10.3.custom/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.10.3.custom/ui/jquery.ui.mouse.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.10.3.custom/ui/jquery.ui.sortable.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.cookie.2.2.0.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="/Scripts/jquery-ui-1.10.3.custom/demos.css" />




    <script type="text/javascript">
        function Deletemenu() {
            var comid = $("#hid_comid").trimVal();
            var id = $("#hid_id").trimVal();
            $.post("/JsonFactory/DirectSellHandler.ashx?oper=deletemenu", { id: id, comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("操作出现错误");
                    return;
                }
                if (data.type == 100) {
                    $.prompt("删除成功");
                    location.href = "ShopManage.aspx";
                    return;
                }
            })
        }

        function Deletebanner() {
            var comid = $("#hid_comid").trimVal();
            var id = $("#hid_imgid").trimVal();
            $.post("/JsonFactory/DirectSellHandler.ashx?oper=deleteimage", { id: id, comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("操作出现错误");
                    return;
                }
                if (data.type == 100) {
                    $.prompt("删除成功");
                    location.href = "ShopManage.aspx";
                    return;
                }
            })
        }


        $(function () {
            var pageSize = 12; //每页显示条数


            $("#menutype_0").click(function () {
                $("#viewpro").show();
                $("#viewwx").hide();
                $("#viewtype").html("显示产品：");
            });

            $("#menutype_1").click(function () {
                $("#viewpro").hide();
                $("#viewwx").show();
                $("#viewtype").html("显示文章：");
            });



            //根据公司id获得关注作者信息
            $.post("/JsonFactory/AccountInfo.ashx?oper=getcurcompanyguanzhu", { comid: $("#hid_comid").trimVal() }, function (data) {
                dat = eval("(" + data + ")");
                if (dat.type == 1) {

                }
                if (dat.type == 100) {
                    $(".links").html("<a href=\"#" + dat.msg + "\" class=\"mp-homepage btn btn-follow\">关注我们</a>");
                }
            });

            //读取菜单类型
            $.post("/JsonFactory/ModelHandler.ashx?oper=modelzhidingpagelist", { id: 0 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                    for (i = 0; i < data.msg.length; i++) {
                        $("#button_linktype").append("<option value='" + data.msg[i].Id + "'>" + data.msg[i].Name + "</option>");
                    }
                }
            })


            $(".editconent").hover(function () {
                $(this).addClass("hoveractions");
            }, function () {
                $(this).removeClass("hoveractions");
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
                var act = $(this).attr("act");

                edittype(upcontentid, act);

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

                            if (data.msg.Setsearch == 1) {
                                $("input[name='setsearch'][value='1']").attr("checked", true);
                            }

                            $("#title").click();
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


            //修改查询设定
            $("#subsearch").click(function () {
                var setsearch = 0;
                if ($('input:checkbox[name="setsearch"]:checked').val() == 1) {
                    setsearch = 1;
                };

                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AccountInfo.ashx?oper=editsearchset",
                    data: { comid: $("#hid_comid").trimVal(), setsearch: setsearch },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            alert("出错，" + data.msg);
                        }
                        if (data.type == 100) {
                            alert("设定成功");

                        }
                    }
                })
            })

            //读取banner
            $.ajax({
                type: "post",
                url: "/JsonFactory/DirectSellHandler.ashx?oper=getimagelist",
                data: { comid: $("#hid_comid").trimVal(), pageindex: 1, pagesize: 1, typeid: 2 },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询错误");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.totalCount == 0) {

                        } else {

                            $("#banner").html("<img src=\"" + data.msg[0].Imgurl_address + "\" style=\"max-width: 320px;\" />");
                            $("#Img1").attr("src", data.msg[0].Imgurl_address);
                            $("#headPortraitImg2").attr("src", data.msg[0].Imgurl_address);

                            $("#banner_title").val(data.msg[0].Title);
                            $("#linkurl").val(data.msg[0].Linkurl);
                            $("#Hidden2").val(data.msg[0].Imgurl);
                            $("#hid_imgid").val(data.msg[0].Id);
                        }

                    }
                }
            })

            //修改banner
            $("#subbanner").click(function () {

                var imgurl = $("#<%=headPortrait2.FileUploadId_ClientId %>").val(); ;
                if (imgurl == "") {
                    imgurl = $("#Hidden2").trimVal();
                }
                var title = $("#banner_title").trimVal();
                if (title == "") {
                    $.prompt("标题不可为空");
                    return;
                }
                var linkurl = $("#linkurl").trimVal();
                if (title == "") {
                    $.prompt("连接地址不可为空");
                    return;
                }

                $.post("/JsonFactory/DirectSellHandler.ashx?oper=editimage", { id: $("#hid_imgid").val(), typeid: 2, comid: $("#hid_comid").trimVal(), imgurl: imgurl, title: title, linkurl: linkurl }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("添加出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("添加成功");
                        $(".app-sidebar").hide();
                        location.href = "ShopManage.aspx";
                        return;
                    }
                });
            })



            var comid = $("#hid_comid").trimVal();

            $("#erweima").html("扫二维码访问<br><img src='../PMUI/ETicket/showtcode.aspx?pno=http://shop" + comid + ".etown.cn/h5/order/'  '/>");
            SearchList(1);

            //装载栏目列表
            function SearchList(pageindex) {
                var menuindex = $("#menuindex").val();
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/DirectSellHandler.ashx?oper=getmenulist",
                    data: { comid: comid, pageindex: pageindex, pagesize: 30, usetype: 1, menuindex: menuindex },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            //$.prompt("查询错误");
                            return;
                        }
                        if (data.type == 100) {
                            if (data.totalCount == 0) {
                                var html_str = "";
                                html_str += '<div class="editconent" id="0"  act="menu">'
                                html_str += ' <div class="app-field clearfix ">';
                                html_str += '<div class="control-group">';
                                html_str += ' <ul class="custom-nav clearfix js-custom-nav ">';
                                html_str += '   <li><a class="clearfix " href="javascript:void(0);"><span class="custom-nav-title">栏目名称</span><i class="pull-right right-arrow"></i></a></li>';
                                html_str += ' </ul>';
                                html_str += '  <div class="component-border"></div>';
                                html_str += ' </div>';
                                html_str += ' </div>';



                                html_str += '<div class="app-field clearfix">';
                                html_str += '   <div class="control-group">';
                                html_str += '      <div class="js-goods-style-region"><div>';
                                html_str += '        <ul class="sc-goods-list clearfix size-1 card pic">';
                                for (var j = 1; j < 3; j++) {
                                    html_str += '	        <li class="goods-card small-pic card ">';
                                    html_str += '		        <a href="javascript: void(0);" class="link js-goods clearfix ">';
                                    html_str += '			        <div class="photo-block">';
                                    html_str += '				        <img class="goods-photo js-goods-lazy" src="/images/defaultThumb.png" />';
                                    html_str += '			        </div>';
                                    html_str += '				        <div class="info clearfix ">';
                                    html_str += '						        <p class="goods-title">第' + j + '个产品</p>';
                                    html_str += '						        <p class="goods-price"><em>￥1</em></p>';
                                    html_str += '						        <p class="goods-price-taobao">原价：00</p>';
                                    html_str += '				        </div>';
                                    html_str += '				        <div class="goods-buy btn4">';
                                    html_str += '				        </div>';
                                    html_str += '		        </a>';
                                    html_str += '	        </li>';
                                }

                                html_str += '       </ul>';
                                html_str += '    </div>';
                                html_str += '</div>';
                                html_str += '</div>';
                                html_str += '</div>';
                                html_str += '</div>';
                                $("#content").html(html_str);
                            } else {
                                var html_str = "";
                                for (var i = 0; i < data.msg.length; i++) {
                                    html_str += '<div class="editconent" id="' + data.msg[i].Id + '" act="menu">'
                                    html_str += ' <div class="app-field clearfix ">';
                                    html_str += '<div class="control-group">';
                                    html_str += ' <ul class="custom-nav clearfix js-custom-nav ">';
                                    if (data.msg[i].Usestyle == 1) {
                                        html_str += '   <li><img alt="" class="headPortraitImgSrc" src="' + data.msg[i].Imgurl_address + '" style="max-height: 84px; max-width: 100%;" height="50" width="320"/></li>';
                                    } else {
                                        html_str += '   <li><a class="clearfix " href="javascript:void(0);"><span class="custom-nav-title">' + data.msg[i].Name + '</span><i class="pull-right right-arrow"></i></a></li>';
                                    }
                                    html_str += ' </ul>';
                                    html_str += '  <div class="component-border"></div>';
                                    html_str += ' </div>';
                                    html_str += ' </div>';
                                    html_str += '<div class="app-field clearfix">';
                                    html_str += '   <div class="control-group">';
                                    html_str += '      <div class="js-goods-style-region"><div>';

                                    if (data.msg[i].Menutype == 0) {

                                        if (data.msg[i].menuviewtype == 0) {
                                            html_str += '        <ul class="sc-goods-list clearfix size-1 card pic">';
                                            for (var j = 0; j < data.msg[i].prolist.length; j++) {
                                                html_str += '	        <li class="goods-card small-pic card ">';
                                                html_str += '		        <a href="javascript: void(0);" class="link js-goods clearfix ">';
                                                html_str += '			        <div class="photo-block">';
                                                html_str += '				        <img class="goods-photo js-goods-lazy" src="' + data.msg[i].prolist[j].Imgaddress + '" />';
                                                html_str += '			        </div>';
                                                html_str += '				        <div class="info clearfix ">';
                                                html_str += '						        <p class="goods-title">' + data.msg[i].prolist[j].Pro_name + '</p>';
                                                html_str += '						        <p class="goods-price"><em>￥' + data.msg[i].prolist[j].Advise_price + '</em></p>';
                                                html_str += '						        <p class="goods-price-taobao">原价：00</p>';
                                                html_str += '				        </div>';
                                                html_str += '				        <div class="goods-buy btn4">';
                                                html_str += '				        </div>';
                                                html_str += '		        </a>';
                                                html_str += '	        </li>';
                                            }
                                        }
                                        if (data.msg[i].menuviewtype == 3) {
                                            html_str += '        <ul class="sc-goods-list clearfix size-1 card pic">';
                                            for (var j = 0; j < data.msg[i].prolist.length; j++) {
                                                html_str += '	        <li class="goods-card small-pic card " style="width: 100%;">';
                                                html_str += '		        <a href="javascript: void(0);" class="link js-goods clearfix ">';
                                                html_str += '			        <div class="photo-block">';
                                                html_str += '				        <img class="goods-photo js-goods-lazy" src="' + data.msg[i].prolist[j].Imgaddress + '" />';
                                                html_str += '			        </div>';
                                                html_str += '				        <div class="info clearfix ">';
                                                html_str += '						        <p class="goods-title">' + data.msg[i].prolist[j].Pro_name + '</p>';
                                                html_str += '						        <p class="goods-price"><em>￥' + data.msg[i].prolist[j].Advise_price + '</em></p>';
                                                html_str += '						        <p class="goods-price-taobao">原价：00</p>';
                                                html_str += '				        </div>';
                                                html_str += '				        <div class="goods-buy btn4">';
                                                html_str += '				        </div>';
                                                html_str += '		        </a>';
                                                html_str += '	        </li>';
                                            }
                                        }

                                        if (data.msg[i].menuviewtype == 1) {//酒店
                                            html_str += '        <ul class="sc-goods-list clearfix size-1 card pic">';
                                            for (var j = 0; j < data.msg[i].hotellist.length; j++) {

                                               html_str += '	        <li class="cnt_box clearfix" url-id="">';
                                               html_str += '	           <div class="cnt_img"><img src="' + data.msg[i].hotellist[j].Imgaddress + '" title="' + data.msg[i].hotellist[j].Projectname + '"></div>';
                                               html_str += '	           <div class="cnt_right">';
                                               html_str += '	             <p class="hotel_tit">' + data.msg[i].hotellist[j].Projectname + '</p>';
                                               html_str += '	             <div class="hotel_cnt1_box"><span class="grade">' + data.msg[i].hotellist[j].grade + '</span><span class="stop"></span>';
                                               html_str += '	               <p class="info"><span>¥</span><span class="info_c">' + data.msg[i].hotellist[j].minprice + '</span>起</p>';
                                               html_str += '	             </div>';
                                               html_str += '	             <div class="hotel_cnt2_box"><span class="star">' + data.msg[i].hotellist[j].star + '</span>';
                                               html_str += '	               <p class="tb"><span class="cu">' + data.msg[i].hotellist[j].cu + '</span><span class="li"></span></p>';
                                               html_str += '	             </div>';
                                               html_str += '	             <div class="hotel_cnt3_box"><span class="address">' + data.msg[i].hotellist[j].Province + '' + data.msg[i].hotellist[j].City + '</span><span class="addnum"></span></div>';
                                               html_str += '	           </div>';
                                               html_str += '	         </li>';

                                            }
                                        }

                                       if (data.msg[i].menuviewtype == 2) {//班车
                                            html_str += '        <ul class="sc-goods-list clearfix size-1 card pic">';
                                            for (var j = 0; j < data.msg[i].prolist.length; j++) {

                                                
                                               html_str += '	        <li class="recommend_item " data-lineid="25">';
		                                       html_str += '	           <div class="item_left">';
		                                       html_str += '	              <p class="recommend_station">' + data.msg[i].prolist[j].Pro_name + '</p>';
			                                   html_str += '	              <p class="recommend_time"></p>';
		                                       html_str += '	           </div>';
		                                       html_str += '	           <div class="item_right">';
		                                       html_str += '	               <div class="recommend_price"><span line-id="25" data-strategytag=""><small><i>￥</i>' + data.msg[i].prolist[j].Advise_price + ' 购票</small></span></div>';
		                                       html_str += '	          </div>';
		                                       html_str += '	         </li> ';


                                            }
                                        } 





                                        html_str += '       </ul>';
                                    }
                                    if (data.msg[i].Menutype == 1) {
                                        html_str += '<ul id="list" class="sc-goods-list clearfix list size-3">';
                                        for (var j = 0; j < data.msg[i].Materiallist.length; j++) {

                                            html_str += '   <li class="goods-card  normalwx">';
                                            html_str += '       <a title="" class="link-wx js-goods clearfix" href="javascript:;">';
                                            html_str += '            <div class="photo-block-wx">';
                                            html_str += '            <img data-src="' + data.msg[i].Materiallist[j].Imgurl + '" class="goods-photo-wx js-goods-lazy" style="display: block; width:120px;max-height: 60px;" src="' + data.msg[i].Materiallist[j].Imgurl + '"> ';
                                            html_str += '            </div>';
                                            html_str += '            <div class="info-wx">';
                                            html_str += '            <p class="goods-title">' + data.msg[i].Materiallist[j].Title + '</p>';
                                            html_str += '             </div>';
                                            html_str += '         </a>      ';
                                            html_str += '     </li>';
                                        }

                                        html_str += '</ul>';
                                    }




                                    html_str += '    </div>';
                                    html_str += '</div>';
                                    html_str += '</div>';
                                    html_str += '</div>';
                                    html_str += '</div>';
                                }


                                var serchstr = '  <div class="custom-search editconent "   act="search"> <input class="custom-search-input" placeholder="商品搜索：请输入商品关键字" name="q" value="" readonly="readonly" type="search"> </div>'



                                $("#content").html(serchstr + html_str);

                            }

                        }
                    }
                })


            }


            SearchListbutton(1);

            //装载菜单列表
            function SearchListbutton(pageindex) {
                var html_str = "";

                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/DirectSellHandler.ashx?oper=getbuttonlist",
                    data: { comid: comid, pageindex: pageindex, pagesize: 3, linktype: 0 },
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
                                    html_str += '  <a class="mainmenu editconent" id="0" act="button" href="javascript:;">';
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
                                    html_str += '  <a class="mainmenu editconent" act="button" id="' + data.msg[i].Id + '" href="javascript:;">';
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
                                        html_str += '  <a class="mainmenu editconent" act="button" id="0" href="javascript:;">';
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

                //读取底部版权导航
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/DirectSellHandler.ashx?oper=getbuttonlist",
                    data: { comid: comid, pageindex: pageindex, pagesize: 5, linktype: 1 },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            //$.prompt("查询错误");
                            return;
                        }
                        if (data.type == 100) {
                            var daohang_str = "";

                            if (data.totalCount == 0) {
                                daohang_str += ' <a href="javascript:;" class="btn mainmenu editconent btn-orange-dark "  act="button1" id="0"> + </a>';
                                $("#copydaohang").html(daohang_str);
                            } else {

                                for (var i = 0; i < data.msg.length; i++) {
                                    daohang_str = '<a href="javascript:;" class="btn mainmenu editconent " act="button1"  id="' + data.msg[i].Id + '" >' + data.msg[i].Name + '</a>' + daohang_str
                                }
                                daohang_str += ' <a href="javascript:;" class="btn mainmenu editconent btn-orange-dark "  act="button1" id="0"> + </a>';
                                $("#copydaohang").html(daohang_str);

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
            $(".cancel-img").click(function () {
                $(".app-sidebar").hide();
            })
            function edittype(actid, type) {
                //先初始化都隐藏
                $("#edittitle").hide();
                $("#editsearch").hide();
                $("#editbanner").hide();
                $("#editimage").hide();
                $("#editmenu").hide();
                $("#editbutton").hide();
                $("#menuorder").hide();

                if (type == "title") {
                    $("#edittitle").show();
                }
                else if (type == "search") {
                    $("#editsearch").show();
                }
                else if (type == "banner") {
                    $("#editbanner").show();
                }
                else if (type == "button") {
                    $("#editbutton").show();
                    readerbutton(actid);
                }
                else if (type == "button1") {
                    $("#editbutton").show();
                    $("#hid_linktype").val(1);
                    readerbutton(actid);
                }
                else if (type == "order") {
                    $("#menuorder").show();
                    Menuorder();
                }
                else {
                    $("#editmenu").show();
                    $("#editimage").show();
                    readerbanner(actid);
                }
            }


            //读取
            function readerbutton(id) {

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
                            $("#button_name").val(data.msg.Name);
                            $("#button_Linkurl").val(data.msg.Linkurl);
                            $("#button_linktype").val(data.msg.Linkurlname);
                            $("#button_sort").val(data.msg.Sort);
                            return;
                        }
                    })
                } else {
                    $("#button_name").val("");
                    $("#button_Linkurl").val("");
                }
            }


            //读取产品
            function readerbanner(id) {

                var comid = $("#hid_comid").trimVal();
                $("#hid_id").val(id);
                if (id != 0) {
                    $.post("/JsonFactory/DirectSellHandler.ashx?oper=getmenubyid", { id: id, comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt("操作出现错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#name").val(data.msg.Name);
                            $("#Linkurl").val(data.msg.Linkurl);

                            $("input[name='usestyle'][value='" + data.msg.Usestyle + "']").attr("checked", true);
                            $("input[name='menuviewtype'][value='" + data.msg.menuviewtype + "']").attr("checked", true);
                            if (data.msg.Usestyle == 1) {
                                $("#imgtab").show();
                                $("#fonttab").hide();
                            } else {
                                $("#imgtab").hide();
                                if (data.msg.Fonticon != "") {
                                    //$("#fonttab").show();
                                }
                            }

                            if (data.msg.Menutype == 0) {
                                $("#menutype_0").click();
                            }
                            if (data.msg.Menutype == 1) {
                                $("#menutype_1").click();
                            }
                            if (data.msg.Menutype == 2) {
                                $("#menutype_2").click();
                            }

                            $("#tab").removeClass().addClass(data.msg.Fonticon);
                            $("#headPortraitImg").attr("src", data.msg.Imgurl_address);

                            $("#Hidden1").val(data.msg.Imgurl);

                            if (data.msg.Menutype == 0) {
                                $("#projectlist").val(data.msg.Projectlist);
                            }

                            if (data.msg.Menutype == 1) {
                                $("#Materiallist").val(data.msg.Projectlist);

                                if (data.Materiallist != null) {
                                    if (data.Materiallist.length > 0) {
                                        var wenzhangstr = "";
                                        for (var i = 0; i < data.Materiallist.length; i++) {
                                            if (data.selectpro == 1) {
                                                wenzhangstr += '<option value="' + data.Materiallist[i].MaterialId + '" selected>' + data.Materiallist[i].Title + '</option>';
                                            } else {
                                                wenzhangstr += '<option value="' + data.Materiallist[i].MaterialId + '">' + data.Materiallist[i].Title + '</option>';
                                            }
                                        }
                                        $("#MaterialId").html(wenzhangstr);
                                    }
                                }

                            }

                            if (data.prolist != null) {
                                if (data.prolist.length > 0) {
                                    var wenzhangstr = "";
                                    for (var i = 0; i < data.prolist.length; i++) {
                                        if (data.selectpro == 1) {
                                            wenzhangstr += '<option value="' + data.prolist[i].Id + '" selected>' + data.prolist[i].Pro_name + '</option>';
                                        } else {
                                            wenzhangstr += '<option value="' + data.prolist[i].Id + '">' + data.prolist[i].Pro_name + '</option>';
                                        }
                                    }
                                    $("#prolist").html(wenzhangstr);
                                }
                            }




                            return;
                        }
                    })
                } else {
                    $("#imgtab").hide();
                    $("#fonttab").hide();
                    $("#name").val("");
                    $("#Linkurl").val("");
                    $("#prolist").html("");
                    $("#projectlist").val("");
                    $("#tab").removeClass();
                    $("#headPortraitImg").attr("src", "");
                    $("#menutype_0").click();
                }
            }



            //动态获取有效项目
            $.post("/JsonFactory/ProductHandler.ashx?oper=projectpageuserlist", { comid: $("#hid_comid").trimVal(), pageindex: 1, pagesize: 200, projectstate: 1 }, function (data) {

                data = eval("(" + data + ")");
                if (data.type == 100) {

                    if (data.totalCount > 0) {
                        $("#textsalepromotetypeid").html("");

                        var groupstr = "";

                        var projectid = $("#hide_projectlist").val();

                        groupstr = '<option value="0">请选择项目</option>';
                        groupstr += '<option value="0">全部产品</option>';
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

            $("#selectlink1").click(function () {
                $("#linktype_2").hide();
                $("#linktype_1").show();
            })
            $("#selectlink2").click(function () {
                $("#linktype_1").hide();
                $("#linktype_2").show();
            })


            $("#subbutton").click(function () {

                var name = $("#button_name").trimVal();
                if (name == "") {
                    $.prompt("标题不可为空");
                    return;
                }
                var linktype = $("#hid_linktype").trimVal();
                var linkurl = $("#button_Linkurl").trimVal();
                if (linkurl == "") {
                    $.prompt("请填写要连接的地址或选择链接到的功能区");
                    return;
                }
                var linkurlname = $("#button_linktype").trimVal();


                $.post("/JsonFactory/DirectSellHandler.ashx?oper=editbutton", { id: $("#hid_id").val(), comid: comid, name: name, linkurl: linkurl, linkurlname: linkurlname, linktype: linktype }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        alert("操作成功");
                        location.href = "ShopManage.aspx";
                        return;
                    }
                })

            })

            $("#delbutton").click(function () {
                $.post("/JsonFactory/DirectSellHandler.ashx?oper=deletebutton", { id: $("#hid_id").val(), comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        alert("删除操作成功");
                        location.href = "ShopManage.aspx";
                        return;
                    }
                })

            })


            //当选择指定栏目
            $("#button_linktype").change(function () {
                var linktype_temp = $("#button_linktype").val();

                if (linktype_temp != 0) {
                    $("#Linkurldiv").hide();
                    $.post("/JsonFactory/ModelHandler.ashx?oper=modelzhidingpagelist", { id: linktype_temp }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            for (i = 0; i < data.msg.length; i++) {
                                $("#button_Linkurl").val(data.msg[i].Linkurl);
                                //$("#button_Linkurl").attr("disabled", "disabled");
                            }
                        }
                    })
                } else {

                    $("#Linkurldiv").show();
                }
            })




            $("#selectimglibrary").click(function () {
                $("#imglibrary").show();
            })
            $("#closeimglibrary").click(function () {
                $("#imglibrary").hide(); ;
            })

            $("#imgtab_select").click(function () {

                var usestyle = 0;
                if ($('input:checkbox[name="usestyle"]:checked').val() == 1) {
                    usestyle = 1;
                };

                if (usestyle == 1) {
                    $("#imgtab").show();
                } else {
                    $("#imgtab").hide();
                }

                $("#fonttab").hide(); ;
            })
            $("#fonttab_select").click(function () {
                //$("#fonttab").show();
                $("#imgtab").hide();

            })

            $("#selectlink1").click(function () {
                $("#linktype_2").hide();
                $("#linktype_1").show();
            })
            $("#selectlink2").click(function () {
                $("#linktype_1").hide();
                $("#linktype_2").show();
            })

            $("#selectfontlibrary").click(function () {
                SearchFontList(1);
                $("#fontlibrary").show();
            })
            $("#closefontlibrary").click(function () {
                $("#fontlibrary").hide(); ;
            })

            //栏目显示内容 
            $("#projectlist").change(function () {
                projectlist = $("#projectlist").val();
                $("#prolist").html("");
                $.post("/JsonFactory/ProductHandler.ashx?oper=pagelist", { comid: $("#hid_comid").trimVal(), pageindex: 1, pagesize: 500, pro_state: 2, projectid: projectlist }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 100) {
                        if (data.totalCount > 0) {
                            var wenzhangstr = "";
                            wenzhangstr = '';
                            for (var i = 0; i < data.msg.length; i++) {
                                wenzhangstr += '<option value="' + data.msg[i].Id + '">' + data.msg[i].Pro_name + '</option>';
                            }
                            $("#prolist").html(wenzhangstr);
                            $("#viewprolist").show();
                            $("#pro_Linkurl").val("/h5/order/list.aspx?projectid=" + projectlist);

                        }
                    }
                })

            })





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
                                libraryfont_html += "<li style=\"float:left; margin:1px;cursor:pointer; font-size:50px\" ondblclick=\"OnSelectFont('" + data.msg[i].Fonticon + "')\"> " + "<span  class=\"" + data.msg[i].Fonticon + "\" style=\"background-color:#0099ff;font-size:50px;\"/></span>" + " </li>"
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




            $("#subpro").click(function () {
                var imgurl = $("#<%=headPortrait.FileUploadId_ClientId %>").val();
                if (imgurl == "") {
                    imgurl = $("#Hidden1").trimVal();
                }
                var name = $("#name").trimVal();
                if (name == "") {
                    $.prompt("标题不可为空");
                    return;
                }
                var linkurl = $("#pro_Linkurl").trimVal();
                if ($("#projectlist").trimVal() == "0") {
                    linkurl = "/h5/order/list.aspx";
                    // return;
                } else {
                    linkurl = "/h5/order/list.aspx?projectid=" + $("#projectlist").trimVal();
                }
                var fonticon = $("#tab").attr('class');
                var usestyle = 0;
                if ($('input:checkbox[name="usestyle"]:checked').val() == 1) {
                    usestyle = 1;
                };

                var menuviewtype = $('input:radio[name="menuviewtype"]:checked').trimVal(); ;


                var projectlist = $("#projectlist").trimVal();
                var prolist = $("#prolist").trimVal();
                var Materiallist = $("#Materiallist").trimVal();
                //var Materiallist = "javascript:;";
                var MaterialId = $("#MaterialId").trimVal();
                var menuindex = $("#menuindex").trimVal();

                var menutype = $('input:radio[name="menutype"]:checked').trimVal();
                if (menutype == 0) {
                    if (projectlist == "" || projectlist == 0) {
                        if (prolist == "") {
                            $.prompt("请选择项目或显示的产品！");
                            return;
                        }
                    }
                }

                if (menutype == 1) {

                    if (MaterialId == "") {
                        $.prompt("请选择显示的文章！");
                        return;
                    }

                    projectlist = Materiallist;
                    prolist = MaterialId;
                    linkurl = $("#Material_Linkurl").trimVal();
                }


                $.post("/JsonFactory/DirectSellHandler.ashx?oper=editmenu", { id: $("#hid_id").val(), comid: comid, imgurl: imgurl, name: name, linkurl: linkurl, fonticon: fonticon, usetype: 1, usestyle: usestyle, projectlist: projectlist, prolist: prolist, menutype: menutype, menuindex: menuindex, menuviewtype: menuviewtype }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        alert("操作成功");
                        location.href = "ShopManage.aspx?menuindex=" + menuindex;
                        return;
                    }
                })
            })

            ReaderWxMaterialType();
            ReaderWxMaterialId();



            $("#Materiallist").change(function () {
                ReaderWxMaterialId();
            });


            //排序

            function Menuorder() {



                $.post("/JsonFactory/DirectSellHandler.ashx?oper=getmenulist", { comid: comid, pageindex: 1, pagesize: 50, usetype: 1 }, function (data2) {
                    data2 = eval("(" + data2 + ")");
                    if (data2.type == 1) {
                        $.prompt("列表获取出现问题");
                        return;
                    }
                    if (data2.type == 100) {
                        $("#myList").empty();
                        var str = "";
                        for (var i = 0; i < data2.totalCount; i++) {
                            str += '<li id="' + data2.msg[i].Id + '">' + data2.msg[i].Name + '</li>';
                        }
                        $("#myList").html(str);
                    }
                })
            }


            $("#myList").sortable({ delay: 1, stop: function () {
                $.cookie("myCookie", $("#myList").sortable('toArray'), { expires: 7 })
            }
            });

            $("#ordersave").click(function () {
                if ($.cookie("myCookie")) {
                    var ids = $.cookie("myCookie");

                    $.post("/JsonFactory/DirectSellHandler.ashx?oper=MenuSort", { ids: ids }, function (data2) {
                        data2 = eval("(" + data2 + ")");
                        if (data2.type == 1) {
                            alert("排序操作出现问题");
                            return;
                        }
                        if (data2.type == 100) {
                            alert("素材排序成功");
                            $.cookie('myCookie', null);
                            location.href = "ShopManage.aspx?menuindex=" + menuindex;
                        }
                    })
                } else {
                    alert("请先对栏目拖拽进行排序");
                }
            })



        })

        //选择图标
        function OnSelectFont(icon) {
            $("#tab").removeClass().addClass(icon);
            $("#fontlibrary").hide();
        }


        function ReaderWxMaterialType() {
            //动态获取全部微信素材类型
            $.post("/jsonfactory/WeiXinHandler.ashx?oper=GetAllWxMaterialType", { comid: $("#hid_comid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("操作出现错误" + data.msg);
                    return;
                }
                if (data.type == 100) {
                    if (data.totalCount > 0) {
                        var groupstr = "";
                        var projectid = $("#hide_projectlist").val();
                        groupstr = '<option value="0">请选择素材分类</option>';
                        groupstr += '<option value="">全部文章</option>';
                        for (var i = 0; i < data.msg.length; i++) {
                            if (projectid != data.msg[i].Id) {
                                groupstr += '<option value="' + data.msg[i].Id + '">' + data.msg[i].TypeName + '</option>';
                            } else {
                                groupstr += '<option value="' + data.msg[i].Id + '"  selected="selected">' + data.msg[i].TypeName + '</option>';
                            }
                        }
                        $("#Materiallist").html(groupstr);
                    }

                }
            });
        }

        function ReaderWxMaterialId() {

            var Materiallist = $("#Materiallist").val();
            $("#MaterialId").html("");

            $.ajax({
                type: "post",
                url: "/JsonFactory/WeiXinHandler.ashx?oper=pagelist",
                data: { comid: $("#hid_comid").trimVal(), pageindex: 1, pagesize: 50, applystate: 1, promotetypeid: $("#Materiallist").val() },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //$.prompt("查询微信素材列表错误");
                        return;
                    }
                    if (data.type == 100) {
                        var wenzhangstr = "";
                        wenzhangstr = '';
                        for (var i = 0; i < data.msg.length; i++) {
                            wenzhangstr += '<option value="' + data.msg[i].MaterialId + '">' + data.msg[i].Title + '</option>';
                        }
                        $("#MaterialId").html(wenzhangstr);


                        var mlist = $("#Materiallist").val();
                        if (mlist == "0" || mlist == "") {
                            $("#Material_Linkurl").val("javascript:;");
                        } else {

                            $("#Material_Linkurl").val("/M/period.aspx?type=" + $("#Materiallist").val());
                        }

                    }
                }
            })

        }



    </script>

</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li  class="on"><a href="/ui/shangjiaui/ShopManage.aspx" onfocus="this.blur()" target="">微商城设置</a></li>
                <li><a href="/ui/shangjiaui/consultant_pro.aspx" onfocus="this.blur()" target="">顾问页面设置</a></li>
                <li><a href="/ui/shangjiaui/StoreList.aspx" onfocus="this.blur()" target="">门店模板设置</a></li>
                <li><a href="/ui/shangjiaui/H5Default.aspx" onfocus="this.blur()" target="">微站形象首页管理</a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                
<div class="app-design clearfix without-add-region"><div class="widget-app-board ui-box">
    <div class="widget-app-board-info">
        <div>
            <p><a href="ShopManage.aspx">默认站点</a> <a href="ShopManage.aspx?menuindex=1">站点1</a> <a href="ShopManage.aspx?menuindex=2">站点2</a>  <a href="ShopManage.aspx?menuindex=3">站点3</a>  <a href="ShopManage.aspx?menuindex=4">站点4</a>  <a href="ShopManage.aspx?menuindex=5">站点5</a></p>
            <p>请在手机预览里，点击需要修改的内容，在编辑框中修改。您修改的内容将自动发布到网站上。</p>
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
    <span class="editconent" id="title"  act="title" >[页面标题]</span>
</h1>



<div class="app-fields ">
<div  class="app-fields ui-sortable">
	<div class="app-field clearfix editing  ">
		<div class="control-group">

            <div class="custom-image-swiper">
                <div class="swiper-container" style="width: 320px">
                    <div class="swiper-wrapper">
					<div style="width: 320px" class="swiper-slide">
						<a href="javascript: void(0);" style="width: 320px;" class="editconent"  act="banner" id="banner">
							<img src="" alt="" style="max-width: 320px;" />
						</a>
					</div>
					</div>
                </div>
            </div>
   		 	<div class="component-border"></div>
		</div>
	</div>


    <div id="content" >


    </div>
    
    <div style=" margin:10px 0;">
            <a href="javascript:;" class="btn btn-block btn-orange-dark editconent"  act="menu" id="0">增加栏目</a>
    </div>
    <div style=" margin:4px 0;">
            <a href="javascript:;" class="btn btn-block btn-orange-dark editconent"  act="order" id="A2">栏目排序</a>
    </div>
    </div>
    <div class="copyright">
		<div class="ft-links">
           <span id="copydaohang"></span>
           <span class="links"></span>

		</div>
	</div>


    <div class="">
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
 </div>

</div></div>
       
    </div>
    <div class="js-add-region"><div></div></div>
   
</div>

<div class="app-sidebar" style="margin-top:55px; display:none;">
    <div class="arrow"></div>
    <div class="app-sidebar-inner js-sidebar-region"><div>
    <div class="js-cancel sku-cancel">
                <div class="cancel-img"></div>
    </div>




    <div class="control-group" id="edittitle"  style="display:none;">
        <label class="control-label">修改商户名称（标题）：</label>
        <div class="controls">
            <p class="help-desc"><input type="text" class="mi-input" id="title_txt" value=""/></p>
        </div>
        <div class="btnconfirm">
        <a href="javascript:;" class="js-confirm-it btn btn-block btn-orange-dark" id="subtitle">确认，保存</a>
        </div>
    </div>

    <div class="control-group" id="editsearch"  style="display:none;">
        <label class="control-label">设定搜索框是否显示：</label>
        <div class="controls">
            <p class="help-desc"><input name="setsearch" value="1" id="setsearch" type="checkbox">显示搜索框</p>
        </div>
        <div class="btnconfirm">
        <a href="javascript:;" class="js-confirm-it btn btn-block btn-orange-dark" id="subsearch">确认，保存</a>
        </div>
    </div>


<div class="js-goods-style-region" style="margin-top: 10px;"><div>


    <div class="control-group" id="editbanner" style="display:none;">
        <label class="control-label">Banner标题：</label>
        <div class="controls">
           <div class="mi-form-item">
                       <input name="title" type="text"  id="banner_title"  size="25" class="mi-input"  value="bannar" style="width:200px;"/>
                        <input name="linkurl" type="hidden"  id="linkurl"  size="25" class="mi-input"  value="#" style="width:300px;"/>
            </div>
            </div>
<label class="control-label">上传图片：</label>
        <div class="controls">
            <div class="mi-form-item">
                      <input type="hidden" id="Hidden2" value="" />
                      <img alt="" class="headPortraitImgSrc" id="headPortraitImg2" style="max-height: 100px;" src="/images/defaultThumb.png" />
                      <uc1:uploadFile ID="headPortrait2" runat="server" />
            </div>
     
        </div>
        <div class="btnconfirm">
            <a href="javascript:;" class="js-confirm-it btn btn-block btn-orange-dark" id="subbanner">确认，保存</a>
             <a href="javascript:;" style=" margin:5px 0; background-color:#cccccc;" class="js-confirm-it btn btn-block " id="A1" onclick="Deletebanner()">删除此Banner</a>
        </div>
    </div>

     <div class="control-group"  id="editmenu" style="display:none;">
     <label class="control-label">栏目类型：</label>
        <div class="controls">
            <label><input name="menutype" value="0" id="menutype_0" type="radio" checked>显示产品</label>
            <label><input name="menutype" value="1" id="menutype_1" type="radio">显示文章</label>

        </div>
        <div class="controls">
            <label><input name="menuviewtype" value="0" id="menuviewtype_0" type="radio" checked>双排样式</label>
             <label><input name="menuviewtype" value="3" id="menuviewtype_3" type="radio">单排样式</label>
            <label><input name="menuviewtype" value="1" id="menuviewtype_1" type="radio">酒店样式</label>
            <label><input name="menuviewtype" value="2" id="menuviewtype_2" type="radio">班车样式</label>
        </div>
       <label class="control-label">栏目名称：</label>
        <div class="controls">
             <input name="name" type="text" id="name" size="25" class="mi-input"  value="" style="width:200px;"/>
         <input name="pro_Linkurl" type="hidden" id="pro_Linkurl"  size="25" class="mi-input"  value=""/>
         <input name="Material_Linkurl" type="hidden" id="Material_Linkurl"  size="25" class="mi-input"  value=""/>
        </div>



       <label class="control-label" id="viewtype">显示产品：</label>
        <div class="controls" id="viewpro">
            <div id="linktype_2" style="display: block;">
                            <select class="mi-input" id="projectlist"  placeholder="链接到项目" style="margin-left: 0;">
                            <option value="" >请选择显示产品或项目</option>
                            </select>

                            <select class="mi-input" id="prolist" name="prolist"  size="3" multiple="multiple" style="margin-left: 0; margin-top:5px;"></select>
                            <br />注：产品可以按住"Ctrl"进行点击多选，当未选择产品时，显示此项目下前6个产品。当选择产品则只显示选择的产品。如果按酒店 不现实具体房间信息。当选择酒店样式时，只有选择图片才会显示标题栏。
            </div>
        </div>
        <div class="controls" id="viewwx" style="display: none;">
            <div style="display: block;">
                            <select class="mi-input" id="Materiallist"  placeholder="链接到素材分类" style="margin-left: 0;">
                            <option value="" >请选择显示文章或文章分类</option>
                            </select>

                            <select class="mi-input" id="MaterialId" name="MaterialId"  size="3" multiple="multiple" style="margin-left: 0; margin-top:5px;"></select>
                            <br />注：产品可以按住"Ctrl"进行点击多选，只显示选择的文章,当未选择文章时，不显示详细文章。
            </div>
        </div>

    </div>
    <div class="control-group"  id="menuorder" style="display:none;">
     <label class="control-label">栏目排序：(请拖拽你栏目进行排序)</label>

        <div class="controls" >
            <div style="display: block;">
                  <ul id="myList" class="Sortsytle">

                  </ul>
            </div>
        </div>
       <div class="btnconfirm">
            <a href="javascript:;" class="js-confirm-it btn btn-block btn-orange-dark" id="ordersave">确认，保存</a>
        </div>
    </div>


    
    <div class="control-group" id="editimage" style="display:none;">
       <label class="control-label">栏目图片：</label>
        <div class="controls">
            <div class="controls-card">
                <div class="controls-card-tab">
                    <label class="radio inline">
                        <input type="checkbox" name="usestyle" value="1" id="imgtab_select"/>
                         栏目按图片显示
                    </label>
                    
                </div>
                <div class="controls-card-item">
                    <div class="mi-form-item"  id="imgtab" style=" display:none;">
                       <label class="mi-label"> 栏目图片</label>
                      <input type="hidden" id="Hidden1" value="" />
                        <img alt="" class="headPortraitImgSrc" style="max-height: 100px;" id="headPortraitImg" src="/images/defaultThumb.png" />
                      <ul>
                                        <li style="height: 20px; margin-left: 10px;float:left;">
                                            <div class="C_verify">
                                                <span>
                                                    <label class="checkbox inline">
                                                    <uc1:uploadFile ID="headPortrait" runat="server" />
                                                    </label>
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
                   

                   <div class="mi-form-item"  id="fonttab" style="display:none;">
                        <label class="mi-label"> 栏目图标（可以不选择只显示标题文字）</label>
                        <span id="tab" style=" font-size:50px;"></span>
                        <input type="button" name="selectimglibrary" id="selectfontlibrary" value="  请选择图标  " class="buttonblue-a" />
                   </div>
                </div>


            </div>
        </div>
               <div class="btnconfirm">
        <a href="javascript:;" class="js-confirm-it btn btn-block btn-orange-dark" id="subpro">确认，保存</a>
         <a href="javascript:;" style=" margin:5px 0; background-color:#cccccc;" class="js-confirm-it btn btn-block " id="delpro" onclick="Deletemenu()">删除此栏目</a>
        </div>
    </div>


         <div class="control-group"  id="editbutton" style="display:none;">
       <label class="control-label">名称：</label>
        <div class="controls">
             <input name="button_name" type="text" id="button_name" size="25" class="mi-input"  value="" style="width:200px;"/>
        
        </div>
       <label class="control-label">链接到：</label>
          <div class="controls">
        <div id="Div1" style="display: block;">
                            <select class="mi-input" id="button_linktype"  style="margin-left: 0;">
                            <option value="" >请选择链接到的功能区</option>
                            <option value="0" >自定义</option>
                            </select>
                            
        </div>
        <div id="Linkurldiv" style="display: none;">
                            <input name="button_Linkurl" type="text" id="button_Linkurl"  size="25" class="mi-input"  value=""/>
        </div>
            <div class="btnconfirm">
             <input type="hidden" id="button_sort" value="1"/>
             
                
            </div>
    
        </div>
<a href="javascript:;" class="js-confirm-it btn btn-block btn-orange-dark" id="subbutton">确认，保存</a>
                 <a href="javascript:;" style=" margin:5px 0; background-color:#cccccc;" class="js-confirm-it btn btn-block " id="delbutton"" onclick="Deletebanner()">删除此项</a>
    </div>

    
</div></div>
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
    <input type="hidden" id="hid_imgid" value="0" />
    <input type="hidden" id="hid_linktype" value="0" />
    <input type="hidden" id="menuindex" value="<%=menuindex %>" />
</asp:Content>