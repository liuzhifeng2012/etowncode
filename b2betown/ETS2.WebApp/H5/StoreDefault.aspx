<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StoreDefault.aspx.cs" Inherits="ETS2.WebApp.H5.StoreDefault" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="content-type" content="text/html; charset=UTF-8" />
<meta name="viewport" content="width=device-width, user-scalable=no, initial-scale=1.0; maximum-scale=1.0; user-scalable=0;" />

<title>微网站</title>
<link rel="stylesheet" type="text/css" href="/Styles/H5/mh5-3.css" />
<link rel="stylesheet" type="text/css" href="/Styles/H5/flexslider.css" />
<link rel="stylesheet" type="text/css" href="/Styles/H5/global.css" />
<link rel="stylesheet" type="text/css" href="/Styles/H5/menshi_media.css" />
<script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>
<script type="text/javascript" src="/Scripts/global.js"></script>


</head>
<body>
<div id="web_page_contents_loading"><img src="images/loading.gif" /></div>

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
	<div class="list-box">
		<div class="item-box">
        <%if (menshi!=null){ %>
        <% if (menshi.Companyintro != "")
           { %>
        <a href="StoreInfo.aspx?menshiid=<%=menshiid %>&type=1"  class="cate-item">
        <img src="/images/93c4e40677.png" class="cate-icon">
        <div class="cate-title"> 门市介绍 </div></a>
        <%}

        if (menshi.Companyproject != "")
        { %>
        <a href="StoreInfo.aspx?menshiid=<%=menshiid %>&type=2"  class="cate-item"><img src="/images/a84c73503c.png" class="cate-icon"><div class="cate-title"> 店长推荐 </div></a>
        <%} %>
        <a href="StoreInfo.aspx?menshiid=<%=menshiid %>&type=3"  class="cate-item"><img src="/images/03513624cb.png" class="cate-icon"><div class="cate-title"> 联系地址 </div></a>
        <a href="/M/?menshiid=<%=menshiid %>&type=4"  class="cate-item"><img src="/images/79c872c57e.png" class="cate-icon"><div class="cate-title"> 优惠活动 </div></a>
        <%} %>
					</div>
		<div class="clear"></div>
	</div>
	<!--<div class="toolbar-item left"></div>
	<div class="toolbar-item right"></div>-->
</div>
</div><div id="footer_points"></div>

<input type="hidden" id="hid_comid" value="<%=comid %>" />

        <script type="text/javascript" src="/Scripts/ppkextend.js"></script>
    <script type="text/javascript">
        $(function () {
            //分享  
            $.ppkWeiShare({
                path: location.href,
                image: "<%=menshiimgurl %>",
                desc: "",
                title: '<%=companyname%>'
            });
        });
    </script>
</body>
</html>