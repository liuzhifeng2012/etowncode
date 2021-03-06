﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default4.aspx.cs" Inherits="ETS2.WebApp.H5.Default4" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="content-type" content="text/html; charset=UTF-8" />
<meta name="viewport" content="width=device-width, user-scalable=no, initial-scale=1.0; maximum-scale=1.0; user-scalable=0;" />

<title>微网站</title>
<link rel="stylesheet" type="text/css" href="/Styles/H5/mh5-4.css" />
<link rel="stylesheet" type="text/css" href="/Styles/H5/mh5-4_media.css" />
<script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>

<script type="text/javascript">
    var pageSize = 20; //只显示条数

    $(function () {
        var comid = $("#hid_comid").val();
        var bannerlist = "";
        var daohanglist = "";
        var imgurl = "";
        SearchList(1);

        //装载
        function SearchList(pageindex) {
            var daohanglist = "";
            //加载导航 ;
            $.ajax({
                type: "post",
                url: "/JsonFactory/DirectSellHandler.ashx?oper=getmenulist",
                data: { comid: comid, pageindex: pageindex, pagesize: 20, typeid: 0 },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("查询错误");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.totalCount == 0) {
                        }
                        else {

                            for (i = 0; i < data.totalCount; i++) {
                                imgurl = data.msg[i].Imgurl_address;
                                if (imgurl == "/Images/defaultThumb.png") {
                                    imgurl = "/images/93c4e40677.png";
                                }
                                daohanglist += "<li><a href=\"" + data.msg[i].Linkurl + "\"><div><p>" + data.msg[i].Name + "</p></div></a></li>"
                            }
                            $("#daohang").html(daohanglist);
                        }
                    }
                }
            })

        }

    })
   
    </script>
</head>
<body>
<div id="web_page_contents_loading"><img src="images/loading.gif" /></div>
<div class="wrap" style=" margin-top:20%;z-index:1000;position:absolute;">
	<ul class="list_ul" id="daohang">
	
	</ul>
</div>


<div id="web_page_contents">
<script type='text/javascript' src='/Scripts/flexslider.js'></script>
<script type='text/javascript' src='/Scripts/index-3.js'></script>

<script language="javascript">

    var web_skin_data = [{ "PId": "0", "MemberId": "1", "SId": "1", "TradeId": "0", "ContentsType": "1", "Title": "[<%=title_arr %>]", "ImgPath": "[<%=img_arr %>]", "Url": "[<%=url_arr %>]", "Postion": "t01", "Width": "640", "Height": "1010", "NeedLink": "0"}];
    var MusicPath = '';
    $(document).ready(index_obj.index_init);
</script>

<script language="javascript">
    var skin_index_init = function () {
        $('#header, #footer, #footer_points').hide();
        var resize = function () {
            $('#web_page_contents').css({
                height: $(window).height(),
                overflow: 'hidden'
            });
        };
        setInterval(resize, 50);
        $('#web_skin_index .banner *').not('img').height($(window).height());

        $(window).load(function () {
            $('#skin_index_control .toolbar-item').on('click', function () {
                if ($(this).hasClass('left')) {
                    scroll_fun(1);
                } else if ($(this).hasClass('right')) {
                    scroll_fun(0);
                }
            });

            var scroll_fun = function (flg) {
                var box = $('#skin_index_control .item-box');
                var list = $('#skin_index_control .list-box');
                var flgScrolling = false;

                if (flgScrolling) { return false; }
                var left = parseInt(box.css('left'));

                if (flg == 0) {
                    if (left >= (-box.width()) && (box.width() + left) > list.width()) {
                        flgScrolling = true;
                        box.animate({
                            left: '-=' + list.width()
                        }, 500, function () {
                            flgScrolling = false;
                        });
                    }
                } else {
                    if (left != 0) {
                        flgScrolling = true;
                        box.animate({
                            left: '+=' + list.width()
                        }, 500, function () {
                            flgScrolling = false;
                        });
                    }
                }
            }
        });
    }
</script>
<div id="web_skin_index">
    <div class="web_skin_index_list banner" rel="edit-t01">
        <div class="img"></div>
    </div>
</div>
<div id="skin_index_control">
	<div class="bg"></div>
	<div style="padding-top:10px; text-align:center; color:#FFFFFF;">

			版权：<%=companyname %>

	</div>
</div>
</div><div id="footer_points"></div>

<input type="hidden" id="hid_comid" value="<%=comid %>" />
</body>
</html>
