var daysInMonth = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
function Today() {
    this.now = new Date();
    this.year = this.now.getFullYear();
    this.month = this.now.getMonth();
    this.day = this.now.getDate()
}
var today = new Today();
var year = today.year;
var month = today.month;
var obj = "";
var holiday = [["2013-02-24", "元宵节"], ["2013-03-08", "妇女节"],
["2013-04-01", "愚人节"], ["2013-04-04", "清明节"], ["2013-05-12", "母亲节"],
["2013-05-01", "劳动节"], ["2013-06-01", "儿童节"], ["2013-06-12", "端午节"],
 ["2013-08-01", "建军节"], ["2013-08-13", "七夕节"], ["2013-09-10", "教师节"],
  ["2013-09-19", "中秋节"], ["2013-10-01", "国庆节"], ["2013-10-13", "重阳节"], ["2013-12-25", "圣诞节"]];
$("#last_mon").click(function () { prevMonth() });
$("#next_mon").click(function () { nextMonth() });
function prevMonth() {
    if ((month - 1) < 0) {
        month = 11; year--
    } else {
        month--
    } fillBox(obj)
}
function nextMonth() {
    if ((month + 1) > 11) {
        month = 0; year++
    } else {
        month++
    } fillBox(obj)
}
function thisMonth(a) {
    obj = a; year = today.year; month = today.month; fillBox(obj)
}
function fillBox(j) {
    var s = "", g = 0;
    for (var k = 0; k < 6; k++) {
        s += "<tr>"; for (var p = 0; p < 7; p++) {
            if (p == 0) { s += '<td class="calBox sun" id="calBox' + g + '"></td>' } else {
                if (p == 6) { s += '<td class="calBox sat" id="calBox' + g + '"></td>' } else {
                    s += '<td class="calBox" id="calBox' + g + '"></td>'
                }
            } g++
        }
        s += "</tr>"
    }
    $("#calendar").html('<table id="calTable"><tr><th class="week_day">周日</th><th class="week_day">周一</th><th class="week_day">周二</th><th class="week_day">周三</th><th class="week_day">周四</th><th class="week_day">周五</th><th class="week_day">周六</th></tr>' + s + "</table>");
    updateDateInfo();
    $("td.calBox").empty();
    var t = 1;
    var a = new Date(year, month, 1);
    var m = a.getDay();
    var b = m + getDays(a.getMonth(), a.getFullYear()) - 1;
    var q = -1;
    if (today.year == year && today.month == month) {
        q = today.day
    }
    for (var k = m; k <= b; k++) {
        var f = month + 1, r = t; f = f < 10 ? "0" + f : f; r = r < 10 ? "0" + r : r; var l = "";
        if (checkHoliday(year + "-" + f + "-" + r)) { l = "<span class='sale'></span>" }
        //        if (t == q) {
        //            $("#calBox" + k).html("<div class='everyday today'>" + l +
        //        "<a href='javascript:;' id='" + year + "-" + f + "-" + r + "'><span class='sp_time'>" + t + 
        //        "</span></a></div>") 
        //        } else {
        var u = today.year, e = today.month + 1, o = today.day;
        e = e < 10 ? "0" + e : e;
        if (t < q || !timeContrast(u + "-" + e + "-" + o, year + "-" + f + "-" + r)) {
            $("#calBox" + k).html("<div class='week_day'>" + l + "<span class='week_day ui-state-default'>" + r + "</span></div>")
        } else {
            $("#calBox" + k).html("<div class='week_day'>" + l + "<a href='javascript:;' class='week_day' id='" + year + "-" + f + "-" + r + "'><span class='week_day'>" + t + "</span></a></div>")
        }
        //        } 
        t++
    }
    var l = "";
    for (k = 0; k < holiday.length; k++) {
        var n = holiday[k][0].split("-");
        if (n[0] == year && n[1] == month + 1) {
            l += "<li><span><em></em>" + holiday[k][0] + "</span>" + holiday[k][1] + "</li>"
        }
    }
    $("#holidayList").html(l);
    $("#calTable a").click(function () {
        var v = $(this).attr("id");
        if (v != null) {
            SearchList(1, 3, $("#hid_proid").val(), v, DateAdd(v,1).format("yyyy-MM-dd"));
            $("#goBacktime").click();
            $("#indate").html(v);
            $("#outdate").html(DateAdd(v, 1).format("yyyy-MM-dd"));
        }
        else {
            $("#indate").html("请选择时间");
            $("#goBacktime").click()
        }
    });      //getTasks()
}
function getTasks() {
    $.getJSON($("#getListUrl").html(), {
        sceneryid: $("#sceneryid").val(),
        priceid: $("#tp_id").val(),
        advanceDay: $("#advanceDay").val(),
        timeLimit: $("#timeLimit").val(),
        month: year + "-" + (month + 1)
    }, function (a) {
        for (i = 0; i < a.ticketList.length; i++) {
            buildTask(a.ticketList[i].builddate,
        a.ticketList[i].price,
        a.ticketList[i].cash,
        a.ticketList[i].priceid,
        a.ticketList[i].isCashOrder,
            a.ticketList[i].haveShow)
        }
    })
}
function buildTask(f, b, e, a, c, d) {
    if (b != 0) { $("#" + f).removeClass("last_day").append("<div class='sp_pice'><span class='font_dol' data-cash='" + e + "' data-priceId='" + a + "' data-isCash='" + c + "'data-haveShow='" + d + "'>&yen;" + b + "</span></div>") }
}
function getDays(b, a) {
    if (1 == b) {
        if (((0 == a % 4) && (0 != (a % 100))) || (0 == a % 400)) {
            return 29
        } else { return 28 }
    } else { return daysInMonth[b] }
}
function updateDateInfo() {
    $("#dateInfo").html(year + "年" + (month + 1) + "月");
    if (month == 0) { $("#last_mon span").html("12月") } else { $("#last_mon span").html(month + "月") }
    if (month == 11) { $("#next_mon span").html("1月") } else { $("#next_mon span").html((month + 2) + "月") }
}
function timeContrast(f, d) {
    var c = f.split("-");
    var j = new Date(c[0], c[1], c[2]);
    var k = j.getTime();
    var g = d.split("-");
    var h = new Date(g[0], g[1], g[2]);
    var e = h.getTime(); if (k > e) {
        return false
    } else { return true }
}
function checkHoliday(b) {
    var a = false; for (i = 0; i < holiday.length; i++)
    { if (holiday[i][0] == b) { a = true } } return a
}
function getTimes(e, k) {
    var g = e.split("-");
    var l = g[0], h = g[1] - 1, o = g[2];
    var c = new Date(l, h, o);
    var p = new Array("星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六");
    var b = p[c.getDay()]; if (k == 1) {
        var g = new Date();
        var n = g.getFullYear(); var f = g.getMonth() + 1;
        var j = g.getDate(); f = f < 10 ? "0" + f : f;
        j = j < 10 ? "0" + j : j;
        var a = n + "-" + f + "-" + j;
        if (getTimeDiff(a, e) == 0) { b = "今天" }
        if (getTimeDiff(a, e) == 1) { b = "明天" }
        if (getTimeDiff(a, e) == 2) { b = "后天" }
    } return b
}
function getTimeDiff(f, c) {
    var e = f.split("-");
    var d = c.split("-");
    var j = new Date(e[0], Number(e[1]) - 1, e[2]);
    var h = new Date(d[0], Number(d[1]) - 1, d[2]);
    var a = parseInt(Math.abs(h - j) / 1000 / 60 / 60 / 24); return a
};