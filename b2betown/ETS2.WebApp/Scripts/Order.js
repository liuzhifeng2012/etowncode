$(function () {
    $("#goBack").click(function (c) {
        if (!$("#orderFome").hasClass("order-form")) {
            c.preventDefault();
            $(".header h1").html("\u586b\u5199\u8ba2\u5355");
            $("#orderFome").removeClass().addClass("order-form");
            $("#calDiv").fadeOut();
            $("footer").show()
        }
    });
    $("#sDes").click(function () {
        var c = $(this).find(".ico-right");
        if (c.size() > 0) {
            if (c.hasClass("return")) {
                $(this).find(".isover").hide();
                $(this).find(".border").show();
                c.removeClass("return")
            } else {
                $(this).find(".isover").show();
                $(this).find(".border").hide();
                c.addClass("return")
            }
        }
    });
    mCal = document.createElement("link");
    mCal.rel = "stylesheet";
    mCal.href = "/Scripts/mCal.css";
    document.getElementsByTagName("head")[0].appendChild(mCal);
    $.getScript("/Scripts/mCal.js");
   //删除了日历部分
    var b = $("#maxpeapalMin").val(),
	a = "";
    b = b == 0 ? 1 : b;
    $("#orderNum").val(b);
    ticketsChange();
    $("#reduceP").bind("click",
	function () {
	    var d = parseInt($("#maxpeapalMin").val(), 10);
	    var c = $(this).siblings("#orderNum").val();
	    if (/^\d+$/g.test(c) && d < c) {
	        c = parseInt(c, 10) - 1;
	        $(this).siblings("#orderNum").val(c);
	        ticketsChange()
	    }
	});
    $("#plusP").bind("click",
	function () {
	    var c = parseInt($("#maxpeapalMax").val(), 10);
	    var d = $(this).siblings("#orderNum").val();
	    if (/^\d+$/g.test(d) && c > d) {
	        d = parseInt(d, 10) + 1;
	        $(this).siblings("#orderNum").val(d);
	        ticketsChange()
	    }
	});
    $(".in-item-number .btn").bind("touchstart touchmove",
	function () {
	    if (!$(this).hasClass("enabled")) {
	        $(this).addClass("mousedown")
	    }
	}).bind("touchend",
	function () {
	    $(this).removeClass("mousedown")
	});
    $("#slShowItem").change(function () {
        $("#showItem").html($(this).find("option:selected").text());
        if ($(this).val() != "") {
            $("#showItem").addClass("writeok")
        } else {
            $("#showItem").removeClass()
        }
    });
    $("#orderNum").bind("keypress",
	function (d) {
	    var c = d.which;
	    if (c < 48 || c > 57) {
	        return false
	    }
	});
    $("#orderNum").bind("input",
	function () {
	    var e = parseInt($("#maxpeapalMin").val(), 10);
	    var c = parseInt($("#maxpeapalMax").val(), 10);
	    var d = $("#orderNum").val();
	    if (/^\d+$/.test(d) && e > d) {
	        $("#orderNum").val(e);
	        ticketsChange()
	    } else {
	        if (/^\d+$/.test(d) && d > c) {
	            $("#orderNum").val(c);
	            ticketsChange()
	        } else {
	            if (/^\d+$/.test(d)) {
	                ticketsChange()
	            }
	        }
	    }
	}).bind("focus",
	function () {
	    $(this).removeClass("notice").addClass("active")
	}).bind("blur",
	function () {
	    $(this).removeClass("active");
	    var d = parseInt($("#maxpeapalMin").val(), 10);
	    var c = $("#orderNum").val();
	    if (c == "" || isNaN(c)) {
	        $("#orderNum").val(d);
	        ticketsChange()
	    }
	});
    $(".in-item-number .btn").bind("touchstart touchmove",
	function () {
	    if (!$(this).hasClass("enabled")) {
	        $(this).addClass("mousedown")
	    }
	}).bind("touchend",
	function () {
	    $(this).removeClass("mousedown")
	});
    $(".in-item input").live("focus",
	function () {
	    var c = $(this).attr("data-tip");
	    if ($(this).val() == c) {
	        $(this).val("").addClass("writeok")
	    }
	});
    $(".in-item input").live("blur",
	function () {
	    var c = $(this).attr("data-tip");
	    if ($(this).val() == "") {
	        $(this).removeClass().val(c)
	    }
	});
    $(document).click(function () {
        if ($("#tips").is(":visible")) {
            $(".qtips_div").fadeOut()
        }
    });
    $("#submitBtn").click(function (j) {
        j.preventDefault();
        if ($(this).hasClass("disabled")) {
            return false
        }
        if ($("#traveldate").val() == "" || $("#playDay").html() == "" || $("#playDay").html() == "\u8bf7\u9009\u62e9\u6e38\u73a9\u65e5\u671f") {
            showErr("\u8bf7\u9009\u62e9\u6e38\u73a9\u65e5\u671f\uff01");
            return false
        }
        var g = $("#orderNum").val();
        if (!/^[1-9]?[0-9]*$/.test(g) || g == "") {
            showErr("\u8bf7\u9009\u62e9\u9884\u8ba2\u6570\u91cf\uff01");
            return false
        }
        if (g > Number($("#maxpeapalMax").val()) || g < 1) {
            showErr("\u8bf7\u91cd\u65b0\u9009\u62e9\u9884\u8ba2\u6570\u91cf\uff01<br /><strong class='f60'>1</strong> \u5f20\u8d77\u8ba2\uff0c\u6700\u591a\u53ef\u9884\u8ba2 <strong class='f60'>" + $("#maxpeapalMax").val() + "</strong> \u5f20\u3002");
            return false
        }
        if ($("#showList").hasClass("fn-hide") == false && $("#slShowItem").val() == "") {
            showErr("\u8bf7\u9009\u62e9\u573a\u6b21\uff01");
            return false
        }
        if ($("#bookEmail").val() != undefined && $("#bookEmail").val() == "") {
            showErr("\u8bf7\u586b\u5199\u7535\u5b50\u90ae\u7bb1\uff01");
            return false
        } else {
            if ($("#bookEmail").val() != undefined && !/.+@.+\.[a-zA-Z]{2,4}$/.test($("#bookEmail").val())) {
                showErr("\u8bf7\u586b\u5199\u6b63\u786e\u7684\u7535\u5b50\u90ae\u7bb1\uff01");
                return false
            }
        }
        if (!checkPerName("getName_1")) {
            return false
        }
        if (!checkPerPhone("getPhone_1")) {
            return false
        }
        if ($("#isCard").val() == "1" && $("#getCard").val() != null && !checkCard()) {
            //return false
        }
        var f = Number($("#orderNum").val()),
		h = true,
		d = true;
        if ($("#isRealName").val() == "1" && f > 1) {
            for (var c = 2; c <= f; c++) {
                if (!checkPerName("getName_" + c)) {
                    return false
                }
                if (!checkPerPhone("getPhone_" + c)) {
                    return false
                }
            }
        }
        $(this).addClass("disabled");
        $("#orderFome").submit()
    });
    $("#goBack").click(function () {
        if ($("#calDiv").is(":hidden")) {
            var c = "";
            showBackNotice("\u60a8\u7684\u8ba2\u5355\u8fd8\u672a\u5b8c\u6210\u54e6\uff0c\u786e\u5b9a\u8981\u79bb\u5f00\u5417?", c);
            return false
        }
    })
});
function resetOrder() {
    var c = $("#amount").val(),
	d = $("#orderNum").val();
    var b;
    var a = $("#cashcoupon").val();
    b = a * d;
    if (!!$(".remind").html() && $(".remind").html().indexOf("\u5df2\u51cf") > -1) {
        if (a > 0) {
            $("#priceTotal").html("&yen;" + (c * d - b))
        } else {
            $(".yj").remove();
            $("#priceTotal").html("&yen;" + c * d)
        }
    } else {
        $("#priceTotal").html("&yen;" + c * d)
    }
    if (a == "0" || balance == "0") {
        $(".remind").hide()
    } else {
        $("#couponMax").html("&yen;" + b);
        $(".remind").show()
    }
    $("#CashUse").val(b)
}
function checkPerName(e) {
    $("#" + e).val($("#" + e).val().replace(/\s/g, ""));
    var c = true,
	b = e.replace("getName_", ""),
	a = $("#isRealName").val();
    if ($("#" + e).val() != "" && $("#" + e).val() != "\u8bf7\u586b\u5199\u53d6\u7968\u4eba\u59d3\u540d") {
        if (/^(.*?)+[\d~!@#$%^&*()_\-+\={}\[\];:'"\|,.<>?\uff01\uffe5\u2026\u2026\uff08\uff09\u2014\u2014\uff5b\uff5d\u3010\u3011\uff1b\uff1a\u2018\u201c\u2019\u201d\u3001\u300a\u300b\uff0c\u3002\u3001\uff1f]/.test($("#" + e).val())) {
            if (a == "1") {
                showErr("\u53d6\u7968\u4eba" + b + "\u7684\u59d3\u540d\u4e0d\u5f97\u5305\u542b\u6570\u5b57\u6216\u7279\u6b8a\u5b57\u7b26\uff01")
            } else {
                showErr("\u53d6\u7968\u4eba\u59d3\u540d\u4e0d\u5f97\u5305\u542b\u6570\u5b57\u6216\u7279\u6b8a\u5b57\u7b26\uff01")
            }
            c = false
        } else {
            var f = "";
            var d = Number($("#orderNum").val());
            for (i = 1; i < d; i++) {
                if (b != i) {
                    f += "@" + $("#getName_" + i).val() + ","
                }
            }
            if (f.indexOf("@" + $("#" + e).val() + ",") > -1) {
                showErr("\u53d6\u7968\u4eba\u59d3\u540d\u6709\u91cd\u590d\uff0c\u8bf7\u91cd\u65b0\u586b\u5199\uff01");
                c = false
            }
        }
    } else {
        if (a == "1") {
            showErr("\u8bf7\u586b\u5199\u53d6\u7968\u4eba" + b + "\u7684\u59d3\u540d\uff01")
        } else {
            showErr("\u8bf7\u586b\u5199\u53d6\u7968\u4eba\u59d3\u540d\uff01")
        }
        c = false
    }
    return c
}
function checkPerPhone(f) {
    var c = true,
	b = f.replace("getPhone_", ""),
	a = $("#isRealName").val();
    if ($("#" + f).val() != "" && $("#" + f).val() != "\u514d\u8d39\u63a5\u6536\u8ba2\u5355\u786e\u8ba4\u77ed\u4fe1") {
        if (!/^1[3,4,5,8]\d{9}$/i.test($("#" + f).val())) {
            if (a == "1") {
                showErr("\u8bf7\u6b63\u786e\u586b\u5199\u53d6\u7968\u4eba" + b + "\u7684\u624b\u673a\u53f7\u7801\uff01")
            } else {
                showErr("\u8bf7\u6b63\u786e\u586b\u5199\u53d6\u7968\u4eba\u624b\u673a\u53f7\u7801\uff01")
            }
            c = false
        } else {
            var e = "";
            var d = Number($("#orderNum").val());
            for (i = 1; i < d; i++) {
                if (b != i) {
                    e += "@" + $("#getPhone_" + i).val() + ","
                }
            }
            if (e.indexOf("@" + $("#" + f).val() + ",") > -1) {
                showErr("\u53d6\u7968\u4eba\u624b\u673a\u53f7\u7801\u6709\u91cd\u590d\uff0c\u8bf7\u91cd\u65b0\u586b\u5199\uff01");
                c = false
            }
        }
    } else {
        if (a == "1") {
            showErr("\u8bf7\u586b\u5199\u53d6\u7968\u4eba" + b + "\u7684\u624b\u673a\u53f7\u7801\uff01")
        } else {
            showErr("\u8bf7\u53d6\u7968\u4eba\u624b\u673a\u53f7\u7801\uff01")
        }
        c = false
    }
    return c
}
function checkCard() {
    var a = true;
    if ($("#getCard").val() != "" || $("#getCard").val() == "\u8bf7\u586b\u5199\u53d6\u7968\u4eba\u6709\u6548\u7684\u8eab\u4efd\u8bc1") {
        if (!cidInfo($("#getCard").val())) {
            showErr("\u8bf7\u6b63\u786e\u8f93\u5165\u6301\u5361\u4eba\u8bc1\u4ef6\u53f7\u7801\uff01");
            a = false
        }
    } else {
        showErr("\u8bf7\u8f93\u5165\u6301\u5361\u4eba\u8bc1\u4ef6\u53f7\u7801\uff01");
        a = false
    }
    //return a
}
function cidInfo(a) {
    var c = true;
    var f = {
        11: "\u5317\u4eac",
        12: "\u5929\u6d25",
        13: "\u6cb3\u5317",
        14: "\u5c71\u897f",
        15: "\u5185\u8499\u53e4",
        21: "\u8fbd\u5b81",
        22: "\u5409\u6797",
        23: "\u9ed1\u9f99\u6c5f",
        31: "\u4e0a\u6d77",
        32: "\u6c5f\u82cf",
        33: "\u6d59\u6c5f",
        34: "\u5b89\u5fbd",
        35: "\u798f\u5efa",
        36: "\u6c5f\u897f",
        37: "\u5c71\u4e1c",
        41: "\u6cb3\u5357",
        42: "\u6e56\u5317",
        43: "\u6e56\u5357",
        44: "\u5e7f\u4e1c",
        45: "\u5e7f\u897f",
        46: "\u6d77\u5357",
        50: "\u91cd\u5e86",
        51: "\u56db\u5ddd",
        52: "\u8d35\u5dde",
        53: "\u4e91\u5357",
        54: "\u897f\u85cf",
        61: "\u9655\u897f",
        62: "\u7518\u8083",
        63: "\u9752\u6d77",
        64: "\u5b81\u590f",
        65: "\u65b0\u7586",
        71: "\u53f0\u6e7e",
        81: "\u9999\u6e2f",
        82: "\u6fb3\u95e8",
        91: "\u56fd\u5916"
    };
    var e = 0;
    var g = "";
    if (!/^\d{17}(\d|x)$/i.test(a)) {
        c = false
    }
    a = a.replace(/x$/i, "a");
    if (f[parseInt(a.substr(0, 2))] == null) {
        c = false
    }
    sBirthday = a.substr(6, 4) + "-" + Number(a.substr(10, 2)) + "-" + Number(a.substr(12, 2));
    var h = new Date(sBirthday.replace(/-/g, "/"));
    if (sBirthday != (h.getFullYear() + "-" + (h.getMonth() + 1) + "-" + h.getDate())) {
        c = false
    }
    for (var b = 17; b >= 0; b--) {
        e += (Math.pow(2, b) % 11) * parseInt(a.charAt(17 - b), 11)
    }
    if (e % 11 != 1) {
        c = false
    }
    return c
}
function showErr(a) {
    $("html").css({
        "overflow-y": "hidden"
    });
    if ($("#bgDiv").html() == null) {
        $('<div id="bgDiv"></div>').appendTo("body")
    }
    if ($("#showMsg").html() != null) {
        $("#showMsg").remove()
    }
    $('<div id="showMsg"><div class="msg-title">\u6e29\u99a8\u63d0\u793a</div><div class="msg-content">' + a + '</div><div class="msg-btn"><a href="javascript:;" onclick="hideErr()">\u77e5\u9053\u4e86</a></div></div>').appendTo("body");
    var b = $(window).height();
    var d = $(window).scrollTop();
    var c = $("#showMsg").height();
    $("#bgDiv").css({
        height: $(document).innerHeight()
    }).show();
    $("#showMsg").css({
        top: (b - c) / 2
    }).show()
}
function showBackNotice(a, c) {
    $("html").css({
        "overflow-y": "hidden"
    });
    if ($("#bgDiv").html() == null) {
        $('<div id="bgDiv"></div>').appendTo("body")
    }
    if ($("#showMsg").html() != null) {
        $("#showMsg").remove()
    }
    $('<div id="showMsg"><div class="msg-title">\u6e29\u99a8\u63d0\u793a</div><div class="msg-content">' + a + '</div><div class="msg-btn"><a href="javascript:;" onclick="hideErr()" style="margin-right:20px;">\u53d6\u6d88</a><a href="' + (c || "javascript:history.go(-1);") + '">\u786e\u5b9a</a></div></div>').appendTo("body");
    var b = $(window).height();
    var e = $(window).scrollTop();
    var d = $("#showMsg").height();
    $("#bgDiv").css({
        height: $(document).innerHeight()
    }).show();
    $("#showMsg").css({
        top: (b - d) / 2
    }).show()
}
function hideErr() {
    $("html").css({
        "overflow-y": "auto"
    });
    $("#bgDiv, #showMsg").hide()
}
function ticketsChange() {
    var f = parseInt($("#maxpeapalMin").val(), 10);
    var d = parseInt($("#maxpeapalMax").val(), 10);
    if (f >= $("#orderNum").val()) {
        $("#reduceP").addClass("enabled")
    } else {
        $("#reduceP").removeClass("enabled")
    }
    if (d <= $("#orderNum").val()) {
        $("#plusP").addClass("enabled")
    } else {
        $("#plusP").removeClass("enabled")
    }
    $("#count").html($("#orderNum").val() + "\u5f20");
    if ($("#isRealName").val() == "1") {
        var a = "",
		e = $("#perList .w-item").length / 2,
		c = $("#orderNum").val();
        if (c == 1) {
            $("#perList").html("")
        } else {
            if (e + 2 > c) {
                if (e == 1) {
                    e = 2
                }
                for (var b = e * 2; b > c * 2 - 3; b--) {
                    $("#perList .w-item:eq(" + b + ")").remove()
                }
            } else {
                for (var b = e + 2; b <= c; b++) {
                    a += '<div class="w-item"><dl class="in-item fn-clear"><dt>\u53d6\u7968\u4eba' + b + '</dt><dd><input type="text" id="getName_' + b + '" name="GName" placeholder="\u8bf7\u586b\u5199\u53d6\u7968\u4eba\u59d3\u540d" value=""  /></dd></dl></div><div class="w-item"><dl class="in-item fn-clear"><dt>\u624b\u673a\u53f7</dt><dd><input type="tel" id="getPhone_' + b + '" name="GMobile" maxlength="11" placeholder="\u514d\u8d39\u63a5\u6536\u8ba2\u5355\u786e\u8ba4\u77ed\u4fe1" value="" /></dd></dl></div>'
                }
                $("#perList").append(a)
            }
        }
    }
};



//距离显示
function ViewDistance(str) {
    //如果大于1000按公里显示否则安米显示

    var juli = parseInt(str);

    if (juli < 1000) {
        return "(距离:" + juli + "米)";
    } else {
        if (juli == 99999999) {//坐标不完全时显示为空，设定为99999999方便排序
            return "";
        } else {
            return "(距离:" + (juli / 1000).toFixed(1) + "公里)";
        }
    }
}

//距离显示
function ViewDistance_jiandan(str) {
    //如果大于1000按公里显示否则安米显示

    var juli = parseInt(str);
    if (juli >= 99999999 || juli == 0) {//坐标不完全时显示为空，设定为99999999方便排序
        return "";
    } 

    if (juli < 1000) {
        return juli + "m";
    } else {
        if (juli >= 99999999 || juli == 0) {//坐标不完全时显示为空，设定为99999999方便排序
            return "";
        } else {
            return (juli / 1000).toFixed(0) + "km";
        }
    }
}

function cutstr(str, len) {
    var str_length = 0;
    var str_len = 0;
    str_cut = new String();
    str = str.replace(/(^s*)|(s*$)/g, "")//首先去除两边空格
    str_len = str.length;
    for (var i = 0; i < str_len; i++) {
        a = str.charAt(i);
        str_length++;
        if (escape(a).length > 4) {
            //中文字符的长度经编码之后大于4
            str_length++;
        }
        str_cut = str_cut.concat(a);
        if (str_length > len) {
            str_cut = str_cut.concat("...");
            return str_cut;
        }
    }
    //如果给定字符串小于指定长度，则返回源字符串；
    if (str_length <= len) {
        return str;
    }
}