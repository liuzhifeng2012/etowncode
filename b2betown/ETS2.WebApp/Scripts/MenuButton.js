
function getmenubutton(comid,buttonname) {
    var html_str = "";
    //加载底部菜单
    $.ajax({
        type: "post",
        url: "/JsonFactory/DirectSellHandler.ashx?oper=getbuttonlist",
        data: { comid: comid, pageindex: 1, pagesize: 3,linktype: 0},
        async: false,
        success: function (data) {
            data = eval("(" + data + ")");

            if (data.type == 1) {
                //$.prompt("查询错误");
                //return;
            }
            if (data.type == 100) {
                if (data.msg != null) {
                    for (var i = 0; i < data.msg.length; i++) {
                        html_str += '<div class="nav-item" style="width: 28%;">';
                        html_str += '  <a class="mainmenu  js-mainmenu"  id="' + data.msg[i].Id + '" href="' + data.msg[i].Linkurl + '">';
                        html_str += '  <span class="mainmenu-txt">' + data.msg[i].Name + '</span>    </a>';
                        html_str += ' </div>';
                    }
                }

            }


            if (html_str == "") {
                html_str += '<div class="nav-special-item nav-item">';
                html_str += '    <a href="/h5/order/Default.aspx" class="home">主页</a>';
                html_str += '</div>';
                html_str += '<div class="nav-item">';
                html_str += '        <a class="mainmenu js-mainmenu" href="/m/indexcard.aspx">';
                html_str += '         <span class="mainmenu-txt">会员中心</span>';
                html_str += '        </a>';
                html_str += '</div>';
                html_str += ' <div class="nav-item">';
                html_str += '         <a class="mainmenu js-mainmenu" href="/h5/order/Order.aspx">';
                html_str += '            <span class="mainmenu-txt">我的订单</span>';
                html_str += '</div>';
            } else {
                html_str = '<div class="nav-special-item nav-item"><a href="/h5/order/Default.aspx" class="home">主页</a></div>' + html_str;


            }
            $("."+buttonname).html(html_str);


        }
    })

    //加载导航
    //读取底部版权导航
    $.ajax({
        type: "post",
        url: "/JsonFactory/DirectSellHandler.ashx?oper=getbuttonlist",
        data: { comid: comid, pageindex: 1, pagesize: 5, linktype: 1 },
        async: false,
        success: function (data) {
            data = eval("(" + data + ")");
            if (data.type == 1) {
                return;
            }
            if (data.type == 100) {
                var daohang_str = "";
                if (data.totalCount == 0) {
                    //$("#copydaohang").html(daohang_str);
                } else {
                    for (var i = 0; i < data.msg.length; i++) {
                        daohang_str = '<a href="' + data.msg[i].Linkurl + '" class="btn bottombtn mainmenu " >' + data.msg[i].Name + '</a>' + daohang_str
                    }
                    $("#copydaohang").html(daohang_str);
                }
            }
        }
    })
}


function cutstr(str, len) {
    var str_length = 0;
    var str_len = 0;
    str_cut = new String();
    str_len = str.length;
    for (var i = 0; i < str_len; i++) {
        a = str.charAt(i);
        str_length++;
        if (escape(a).length > 4) {
            //中文字符的长度经编码之后大于4
            str_length++;
        }
        str_cut = str_cut.concat(a);
        if (str_length >= len) {
            str_cut = str_cut.concat("...");
            return str_cut;
        }
    }
    //如果给定字符串小于指定长度，则返回源搜索字符串；
    if (str_length < len) {
        return str;
    }
}

//制保留2位小数，如：2，会在2后面补上00.即2.00 
function toDecimal2(x) {
    var f = parseFloat(x);
    if (isNaN(f)) {
        return false;
    }
    var f = Math.round(x * 100) / 100;
    var s = f.toString();
    var rs = s.indexOf('.');
    if (rs < 0) {
        rs = s.length;
        s += '.';
    }
    while (s.length <= rs + 2) {
        s += '0';
    }
    return s;
} 


//价格显示
function ViewPrice(str) {
    //如果大于1000按公里显示否则安米显示

    var price_temp = parseInt(str);

    if (price_temp == 0) {
        return toDecimal2(str);
        //return "";
    }
    if (price_temp < 10000) {
        return str;
    } else {

         return (price_temp/ 10000).toFixed(2) + "万";
    }
 }

 //价格显示
 function ViewPrice_html(str) {
     //如果大于1000按公里显示否则安米显示

     var price_temp = parseInt(str);

     if (price_temp == 0) {//坐标不完全时显示为空，设定为99999999方便排序
         document.write("");
     }
     if (price_temp < 10000) {
         document.write(str);
     } else {
         document.write((price_temp / 10000).toFixed(2) + "万");
     }
 }
