Date.ParseString = function (h) { var f = /(\d{4})-(\d{1,2})-(\d{1,2})(?:\s+(\d{1,2}):(\d{1,2}):(\d{1,2}))?/i, g = f.exec(h), j = 0, i = null; if (g && g.length) { if (g.length > 5 && g[6]) { j = Date.parse(h.replace(f, "$2/$3/$1 $4:$5:$6")) } else { j = Date.parse(h.replace(f, "$2/$3/$1")) } } else { j = Date.parse(h) } if (!isNaN(j)) { i = new Date(j) } return i }; Date.prototype.format = function (b) { var c = { "M+": this.getMonth() + 1, "d+": this.getDate(), "h+": this.getHours(), "m+": this.getMinutes(), "s+": this.getSeconds(), "q+": Math.floor((this.getMonth() + 3) / 3), S: this.getMilliseconds() }; if (/(y+)/.test(b)) { b = b.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length)) } for (var a in c) { if (new RegExp("(" + a + ")").test(b)) { b = b.replace(RegExp.$1, RegExp.$1.length == 1 ? c[a] : ("00" + c[a]).substr(("" + c[a]).length)) } } return b }; Date.prototype.addDays = function (d, b) { var c, a = function () { var g = c.getFullYear(), e = c.getMonth(), f = c.getDate(); if (e === 0) { e = 1 } else { e = e + 1 } if (e < 10) { e = "0" + e.toString() } if (f < 10) { f = "0" + f.toString() } return g + "-" + e + "-" + f }; c = this.setDate(this.getDate() + d); c = new Date(c); if (b) { return c } return a() }; var daysInMonth = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31); var ma = [["1", "3", "5", "7", "8", "10"], ["4", "6", "9", "11"]]; today = new Date(), year = today.getFullYear(), month = today.getMonth() + 1, day = today.getDate(); month = month < 10 ? "0" + month : month; day = day < 10 ? "0" + day : day; var tDate = year + "-" + month + "-" + day; var toDate = dateDiff(tDate.replace(/-0/g, "-"), 1); var thDate = dateDiff(toDate.replace(/-0/g, "-"), 1); $(window).resize(function () { if ($("#showMsg").html() != null && $("#showMsg").is(":visible")) { var a = $(window).height(); var c = $(window).scrollTop(); var b = $("#showMsg").height(); $("#bgDiv").css({ height: $(document).innerHeight() }).show(); $("#showMsg").css({ top: (a - b) / 2 }).show() } }); var title = $("header h1").html(), cla = $("header h1").attr("class"); $(function () { var e = $.cookie("comedate"), b = $.cookie("leavedate"); var a = dateDiff(tDate.replace(/-0/g, "-"), 1); var f = a.split("-"); if (today.getHours() >= 20) { e = a; b = thDate } if (e != null && e != "" && Date.ParseString(e) - Date.ParseString(tDate) >= 0) { var d = e.replace(/-0/g, "-").split("-") } else { d = tDate.replace(/-0/g, "-").split("-") } ny = d[0]; nm = d[1] < 10 ? "0" + d[1] : d[1]; nd = d[2] < 10 ? "0" + d[2] : d[2]; $("#sInDate").attr("data-val", ny + "-" + nm + "-" + nd); $("#sInDate label").html(nm + "\u6708" + nd + "\u65e5"); if (b != null && b != "" && Date.ParseString(b) - Date.ParseString(a) >= 0) { var f = b.replace(/-0/g, "-").split("-") } ny = f[0]; nm = f[1] < 10 ? "0" + f[1] : f[1]; nd = f[2] < 10 ? "0" + f[2] : f[2]; $("#sOutDate").attr("data-val", ny + "-" + nm + "-" + nd); $("#sOutDate label").html(nm + "\u6708" + nd + "\u65e5"); $("#goBack").bind("click", function (g) { if ($("#page2").is(":visible")) { g.preventDefault(); $("header h1").html(title).addClass(cla); $("#page2").fadeOut("fast"); $("#page1, footer").show() } }); var c = ""; $("#sInDate, #sOutDate").bind("click", function () { var i = "\u9009\u62e9\u5165\u4f4f\u65e5\u671f", j = $(this).attr("id"), g = null; if (j == "sOutDate") { i = "\u9009\u62e9\u79bb\u5e97\u65e5\u671f"; g = $("#sOutDate") } c = j; $("header h1").html(i).removeClass(); $("#page2").fadeIn("fast"); $("#page1, footer").hide(); var h = new DisplayCalendar($("#calendarDiv"), j, $("#sInDate"), $("#sOutDate"), $("#sInDate").attr("data-val"), $("#sOutDate").attr("data-val")) }); $("#calendarDiv").delegate("td", "click", function (l) { l.preventDefault(); if (!$(this).hasClass("disable")) { var k = $(this).attr("date"); sDate_ = k.split("-"); if (c == "sInDate") { var i = new Date(sDate_[0], sDate_[1], sDate_[2]).getTime(); var g = new Date(year, parseInt(Number(month) + 2, [10]), day).getTime(); if (g <= i) { showErr("\u9152\u5e97\u53ea\u53ef\u4ee5\u9884\u8ba22\u4e2a\u6708\u5185\u7684\u9152\u5e97"); return false } } else { if (c == "sOutDate") { var h = $("#sInDate").attr("data-val").split("-"); var i = new Date(sDate_[0], sDate_[1], sDate_[2]).getTime(); var g = new Date(h[0], h[1], parseInt(Number(h[2]) + 20, [10])).getTime(); var j = new Date(year, parseInt(Number(month) + 2, [10]), day).getTime(); if (j <= i) { showErr("\u9152\u5e97\u53ea\u53ef\u4ee5\u9884\u8ba22\u4e2a\u6708\u5185\u7684\u9152\u5e97"); return false } else { if (g <= i) { showErr("\u5982\u679c\u60a8\u9700\u8981\u5728\u9152\u5e97\u5165\u4f4f20\u5929\u4ee5\u4e0a\uff0c\u8bf7\u81f4\u7535" + tcTel + "\uff0c\u6211\u4eec\u4f1a\u7aed\u8bda\u4e3a\u60a8\u670d\u52a1\u3002"); return false } } } } $("#" + c).attr("data-val", k); $("#" + c).find("label").text(Date.ParseString(k).format("MM\u6708dd\u65e5")); window.scroll(0, 0); $("#goBack").click(); $(this).addClass("curr"); if (c == "sInDate") { var m = dateDiff($("#sInDate").attr("data-val").replace(/-0/g, "-"), 1).split("-"); ny = m[0]; nm = Number(m[1]); nm = nm < 10 ? "0" + nm : nm; nd = Number(m[2]); nd = nd < 10 ? "0" + nd : nd; $("#sOutDate").attr("data-val", ny + "-" + nm + "-" + nd); $("#sOutDate").find("label").html(nm + "\u6708" + nd + "\u65e5") } getRoomList(); $("#showmore").removeClass("return") } }); getRoomList(); $("#roomList").delegate(".roomitem dt", "click", function () { var i = $(this), h = i.parent().next(".roominfo"); if (i.hasClass("showdetail")) { i.removeClass("showdetail") } else { i.addClass("showdetail"); var g = h.find("dt img"); if (g.attr("src") == "") { g.attr("src", g.attr("isrc")) } } h.stop(true, true).slideToggle("fast") }); $("#showmore").bind("click", function () { if ($(this).hasClass("return")) { scrollTo(0, 150); $(this).removeClass("return"); $("#roomList").find(".roomitem").each(function (g, h) { if (g > 2) { $(this).hide(); $(this).find("dt").removeClass("showdetail"); $(this).next(".roominfo").hide() } }); $(this).find("span").html("\u5c55\u5f00\u66f4\u591a\u4f18\u60e0\u623f\u578b") } else { $(this).addClass("return"); $("#roomList").find(".roomitem").each(function (g, h) { if (g > 2) { $(this).show() } }); $(this).find("span").html("\u6536\u8d77\u66f4\u591a\u4f18\u60e0\u623f\u578b") } }) }); function getRoomList() {
    var d = $("#roomList").attr("data-hotelid"), a = $("#sInDate").attr("data-val"), b = $("#sOutDate").attr("data-val"), c = $("#roomList").attr("data-url"); var f = 0; var e = null;
    $.ajax({ url: c, 
    data: "comedate=" + a + "&leavedate=" + b, 
    beforeSend: function () {
        //$("#roomList").html('');
        $("#showmore").hide()
    },
    success: function (m) {
        m = JSON.parse(m);
        if (m.State == 100) {
            var l = [], n = m.roominfo; var h = false; var g = ""; for (var j = 0; j < n.length; j++) {
                var o = "", k = ""; if (j > 2) { o = " fn-hide" } l.push('<dl class="roomitem fn-clear' + o + '">');
                l.push("  <dt>"); l.push("    <h5><strong>" + n[j].name + "</strong></h5>");
                   l.push('    <div class="append">');
                if (n[j].promoId == 9) { e = "\u897f\u5858" } else { if (n[j].promoId == 10) { e = "\u5468\u5e84" } }
                if (n[j].PromoType == 4 && n[j].rebateFloatAmount != 0) {
                    l.push('<span class="lifan">\u9996\u5355\u7acb\u8fd4&yen;' + n[j].rebateFloatAmount + "</span>");
                    h = true; f = n[j].rebateFloatAmount
                } else {
                    if (n[j].baiduFlag == "1") {
                        l.push('      <span class="baiduzx">\u767e\u5ea6\u4e13\u4eab</span>')
                    } if (n[j].presentFlag == "1") {
                        l.push('      <span class="gift">\u793c</span>')
                    } if (n[j].limitBuy == "1") {
                        l.push('      <span class="chuxiao">\u8fd1\u671f\u7279\u60e0</span>')
                    } if (n[j].wf == "4") {
                        l.push('      <span class="wf">\u5348\u591c</span>')
                    } if (n[j].wf == "5") {
                        l.push('      <span class="wf">\u949f\u70b9</span>')
                    } 
                }
                if (n[j].guaranteeType == "1") {
                    k = '<span class="danbao">\u62c5\u4fdd</span>'
                } else {
                    if (n[j].guaranteeType != "0") { k = '<span class="paytype">\u9884\u4ed8</span>' } 
                }
                l.push("      " + k); if (n[j].allowBooking == "True") {
                    if (n[j].surplus != "0") {
                        l.push('      <span class="last">\u4ec5\u5269' + n[j].surplus + "\u95f4</span>")
                    } 
                }
                l.push("    </div>"); l.push("  </dt>"); l.push("  <dd>");
                if (n[j].guaranteeType != "0" && n[j].guaranteeType != "1") {
                    l.push('    <div class="price minu">')
                } else {
                    l.push('    <div class="price">')
                }
                l.push("      <strong>&yen;" + n[j].price + "</strong>");
                if (n[j].cashback != 0) {
                    l.push("      <span><em>&yen;" + n[j].cashback + "</em></span>")
                }
                l.push("    </div>"); l.push('    <div class="btn">');
                l.push("      <a" + (n[j].allowBooking !== "True" ? (" href='javascript:;' id='no-room'>\u6ee1\u623f") : " href='" + n[j].aurl + "'>\u9884\u8ba2") + "</a>");
                l.push("    </div>"); l.push("  </dd>"); l.push("</dl>");
                l.push('<div class="roominfo fn-hide">');
                l.push('  <dl class="detailmore fn-clear">');
//                if (n[j].imgurl != "") {
//                    l.push('    <dt><img isrc="' + n[j].imgurl + '" src="" /></dt>')
//                } 
                l.push('    <dd class="fn-clear">');
                l.push('      <span class="half">\u65e9\u9910\uff1a' + n[j].breakfast + "</span>"); if (n[j].allowAddBedRemark != "") {
                    l.push('      <span class="half">' + n[j].allowAddBed + "(" + n[j].allowAddBedRemark + ")</span>")
                } else {
                    l.push('      <span class="half">' + n[j].allowAddBed + "</span>")
                }
                l.push('      <span class="half">\u697c\u5c42\uff1a' + n[j].floor + "</span>");
                l.push('      <span class="half">\u9762\u79ef\uff1a' + n[j].area + "</span>");
                l.push("      <span>\u5e8a\u578b\uff1a" + n[j].bed + "</span>");
                l.push("      <span>\u5bbd\u5e26\uff1a" + n[j].adsl + "</span>");
                l.push("      <span>\u65e0\u70df\u623f\uff1a" + n[j].isNoSmoking + "</span>");
                l.push("      <span>\u536b\u6d74\uff1a" + n[j].isOwnBath + "</span>");
                l.push("    </dd>");
                l.push("  </dl>");
                if (n[j].presentFlag == "1") {
                    l.push('  <dl class="roomgift fn-clear">');
                    l.push("    <dt><span>\u793c</span></dt>");
                    l.push("    <dd>");
                    l.push("      <h6>\u4ece" + n[j].startTime + "\u5230" + n[j].endTime + "</h6>");
                    l.push("      <p>" + n[j].description + "</p>"); l.push("    </dd>");
                    l.push("  </dl>")
                }
                l.push("</div>")
            } 
            $("#roomList").html(l.join("")); if (n.length > 3) { $("#showmore").show() } if (h == true) { $("#tip").show(); $("#price").html("&yen;" + f) } $("#tip").bind("click", function () { showTip("\u6d3b\u52a8\u65f6\u95f4\uff1a2013.12.5\u20142014.1.31<br/>\u6d3b\u52a8\u4ecb\u7ecd\uff1a\u5728\u6d3b\u52a8\u671f\u95f4\uff0c\u901a\u8fc7\u624b\u673a\u7ad9\uff08m.17u.cn\uff09\u6216\u626b\u63cf" + e + "\u5f53\u5730\u6307\u5b9a\u4e8c\u7ef4\u7801\uff0c\u9996\u5355\u9884\u8ba2\u6307\u5b9a\u7684" + e + "\u9152\u5e97\uff0c\u6210\u529f\u5165\u4f4f\u5e76\u53d1\u8868\u70b9\u8bc4\u540e\uff0c\u901a\u8fc7\u540c\u7a0b\u65c5\u6e38\u5ba2\u6237\u7aef\u53ef\u7acb\u5373\u63d0\u53d6&yen;" + f + "\u5956\u91d1\u3002<br/>\u6ce8\u610f\u4e8b\u9879\uff1a<br/>1.\u6bcf\u4f4d\u7528\u6237\u4ec5\u6709\u4e00\u6b21\u9996\u5355\u8fd4\u73b0\u7684\u673a\u4f1a\uff0c\u9884\u8ba2\u540e\u53d6\u6d88\u8ba2\u5355\uff0c\u5373\u89c6\u4e3a\u5df2\u53c2\u52a0\u8fc7\u6b64\u6d3b\u52a8<br/>2.\u6b64\u6d3b\u52a8\u4e0e\u8ba2\u5355\u539f\u6709\u7684\u70b9\u8bc4\u5956\u91d1\u4e0d\u80fd\u5171\u4eab"); return false }) } else { $("#roomList").html('<div class="loading">\u62b1\u6b49\uff0c\u6b64\u65e5\u671f\u4e0b\u8be5\u9152\u5e97\u56e0\u6ee1\u623f\u6216\u5176\u5b83\u539f\u56e0\u6682\u4e0d\u63d0\u4f9b\u9884\u8ba2\u3002\u60a8\u53ef\u4ee5\u9884\u8ba2\u8be5\u9152\u5e97\u7684<a style="color:#ff6600" href="/hotel/hotelnearby_' + $("#roomList").attr("data-hotelid") + '.html">\u5468\u8fb9\u9152\u5e97</a>\u54e6!</div>'); $("#showmore").hide() } if (m.Error == "\u5168\u90e8\u6ee1\u623f") { $("#roomList").html('<div class="loading">\u62b1\u6b49\uff0c\u6b64\u65e5\u671f\u4e0b\u8be5\u9152\u5e97\u56e0\u6ee1\u623f\u6216\u5176\u5b83\u539f\u56e0\u6682\u4e0d\u63d0\u4f9b\u9884\u8ba2\u3002\u60a8\u53ef\u4ee5\u9884\u8ba2\u8be5\u9152\u5e97\u7684<a style="color:#ff6600" href="/hotel/hotelnearby_' + $("#roomList").attr("data-hotelid") + '.html">\u5468\u8fb9\u9152\u5e97</a>\u54e6!</div>'); $("#showmore").hide() } } }) } function showErr(a) { $("html").css({ "overflow-y": "hidden" }); if ($("#bgDiv").html() == null) { $('<div id="bgDiv"></div>').appendTo("body") } if ($("#showMsg").html() != null) { $("#showMsg").remove() } $('<div id="showMsg"><div class="msg-title">\u6e29\u99a8\u63d0\u793a</div><div class="msg-content">' + a + '</div><div class="msg-btn"><a href="javascript:;" onclick="hideErr()">\u77e5\u9053\u4e86</a></div></div>').appendTo("body"); var b = $(window).height(); var d = $(window).scrollTop(); var c = $("#showMsg").height(); $("#bgDiv").css({ height: $(document).innerHeight() }).show(); $("#showMsg").css({ top: (b - c) / 2 }).show() } function hideErr() { $("html").css({ "overflow-y": "auto" }); $("#bgDiv, #showMsg").hide() } function dateDiff(e, b) { var c = e.split("-"); var a = isLeap(c[0]); if (b == "1") { if ((checkArray(c[1], ma[0]) && (c[2] == "31")) || (checkArray(c[1], ma[1]) && (c[2] == "30")) || (c[1] == "2" && c[2] == "28" && !a) || (c[1] == "2" && c[2] == "29" && a)) { return c[0] + "-" + (c[1] * 1 + 1) + "-" + 1 } else { if (c[1] == "12" && c[2] == "31") { return (c[0] * 1 + 1) + "-1-1" } else { return c[0] + "-" + c[1] + "-" + (c[2] * 1 + 1) } } } else { if (b == "0") { if (checkArray(c[1] - 1, ma[0]) && (c[2] == "1")) { return c[0] + "-" + (c[1] - 1) + "-31" } else { if (checkArray(c[1] - 1, ma[1]) && (c[2] == "1")) { return c[0] + "-" + (c[1] - 1) + "-30" } else { if (c[1] == "1" && c[2] == "1") { return (c[0] - 1) + "-12-31" } else { if (c[1] == "3" && c[2] == "1" && !a) { return c[0] + "-2-28" } else { if (c[1] == "3" && c[2] == "1" && a) { return c[0] + "-2-29" } else { return c[0] + "-" + c[1] + "-" + (c[2] - 1) } } } } } } else { return } } } function isLeap(a) { return ((a % 4 == 0 && a % 100 != 0) || a % 400 == 0) ? true : false } function checkArray(e, c) { for (var d = 0, b = c.length; d < b; d++) { if (c[d] == e) { return true } } return false } function getTimeDiff(f, c) { var e = f.split("-"); var d = c.split("-"); var i = new Date(e[0], Number(e[1]) - 1, e[2]); var h = new Date(d[0], Number(d[1]) - 1, d[2]); var a = parseInt(Math.abs(h - i) / 1000 / 60 / 60 / 24); return a } function showTip(a) { $("html").css({ "overflow-y": "hidden" }); if ($("#bgDiv").html() == null) { $('<div id="bgDiv"></div>').appendTo("body") } if ($("#showDiv").html() != null) { $("#showDiv").remove() } $('<div id="showDiv"><div class="packet"><div class="msg-title">\u6d3b\u52a8\u8be6\u60c5</div><div class="show-content">' + a + '<a href="javascript:;" onclick="hideMsg()"><em class="show-btn">\u00d7</em></a></div></div></div>').appendTo("body"); var b = $(window).height(); var d = $(window).scrollTop(); var c = $("#showDiv").height(); $("#bgDiv").css({ height: $(document).innerHeight() }).show(); $("#showDiv").css({ top: (b - c) / 2 }).show() } function hideMsg() { $("html").css({ "overflow-y": "auto" }); $("#bgDiv, #showDiv").hide() } $("#hx").click(function () { $("#layer img").attr("src", "http://"); $("#layer img").attr("width", "90%"); $("#layer").show() }); $("#layer").click(function () { $(this).hide() });