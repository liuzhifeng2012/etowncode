/* 验证提示 */
$.fn.extend({
    inputTip: function (showOn, content) {
        return $(this).each(function () {
            $(this).poshytip({
                content: content,
                className: 'tip-yellowsimple',
                showOn: showOn,
                alignTo: 'target',
                alignX: 'right',
                alignY: 'center',
                offsetX: 5
            });
        });
    },
    showTip: function (hideEvent) {
        return $(this).each(function () {
            $(this).poshytip('show').bind(hideEvent, function () {
                $(this).poshytip('hide');
            });
        });
    },

    hideTip: function () {
        $(this).poshytip('hide');
    },

    updateTip: function (content) {
        return $(this).each(function () {
            $(this).poshytip('update', content);
        });
    },
    trimVal: function () {
        return $.trim($(this).val());
    },
    trimLen: function () {
        var v = $(this).trimVal();
        return v.length;
    },
    checkTel: function () {
        var v = $(this).trimVal();
        return (/^(([0\+]\d{2,3}-)?(0\d{2,3})-)(\d{7,8})(-(\d{3,}))?$/.test($.trim(v)));
    },
    checkDate: function () {
        var v = $(this).trimVal();
        return (/^\d{4}-\d{1,2}-\d{1,2}$/.test($.trim(v)));
    },
    int: function () {
        var v = $(this).trimVal();
        return /^[0-9]+$/.test($.trim(v));
    },
    Amount: function () {
        return /^[+]?(([1-9]\d*[.]?)|(0.))(\d{0,2})?$/.test($.trim($(this).val()));
    },
    checkZipcode: function () {
        return (/^\d{6}$/.test($.trim($(this).val())));
    },
    checkMobile: function () {
        var v = $(this).trimVal();
        return (/^(?:13\d|15\d|18\d|14\d|17\d)-?\d{5}(\d{3}|\*{3})$/.test($.trim(v)));
    },
    mail: function () {
        var v = $(this).trimVal();
        return (/^([a-zA-Z0-9_-]+[_|\_|\.]?)*[a-zA-Z0-9_-]+@([a-zA-Z0-9_-]+[_|\_|\.]?)*[a-zA-Z0-9_]+\.[a-zA-Z]{2,3}$/.test($.trim(v)));
    },
    percent: function () {
        if (parseFloat($.trim($(this).val())) <= 1 && parseFloat($.trim($(this).val())) >= 0 && /^[+]?(([1-9]\d*[.]?)|(0.))(\d{0,5})?$/.test($.trim($(this).val()))) {
            return true;

        }
        return false;
    },
    checkIdCardNo: function () {
        var v = $(this).trimVal();
        return (/^(\d{14}|\d{17})(\d|[xX])$/.test($.trim(v)));
    },
    CheckPassport: function () {
        var v = $(this).trimVal();
        return (/^P\d{7}|G\d{8}$/.test($.trim(v)));
    },
    CheckUri: function () {
        var v = $(this).trimVal();
        return (/^http:\/\/[a-zA-Z0-9]+\.[a-zA-Z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/.test($.trim(v)));
    }
});


//Javascript 格式化金额
//格式化：
var fmoney = function (s, n) {
    n = n > 0 && n <= 20 ? n : 2;
    s = parseFloat((s + "").replace(/[^\d\.-]/g, "")).toFixed(n) + "";
    var l = s.split(".")[0].split("").reverse(),
	    r = s.split(".")[1];
    t = "";
    for (i = 0; i < l.length; i++) {
        t += l[i] + ((i + 1) % 3 == 0 && (i + 1) != l.length ? "," : "");
    }
    return t.split("").reverse().join("") + "." + r;
}

//Javascript 格式化金额
//复原：
var rmoney = function (s) {
    s = s + "";
    return parseFloat(s.replace(/[^\d\.-]/g, ""));
}

/*json返回日期格式转化*/
function ChangeDateFormat(cellval) {
    var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    return date.getFullYear() + "-" + month + "-" + currentDate;
}

/*json返回日期格式转化只保留月-日*/
function ChangeDate_MDFormat(cellval) {
    var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    return month + "-" + currentDate;
}

function jsonDateFormat(jsonDate) {//json日期格式转换为正常格式
    try {
        var date = new Date(parseInt(jsonDate.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
        var milliseconds = date.getMilliseconds();
        return date.getFullYear() + "-" + month + "-" + day + " " + hours + ":" + minutes;
    } catch (ex) {
        return "";
    }
}

//wcl  2013-10-17
function jsonDateFormatKaler(jsonDate) {//json日期格式转换为正常格式
    try {
        var date = new Date(parseInt(jsonDate.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
        var milliseconds = date.getMilliseconds();
        return date.getFullYear() + "-" + month + "-" + day + " " + hours + ":" + minutes + ":" + seconds;
    } catch (ex) {
        return "";
    }
}
function Short_jsonDateFormatKaler(jsonDate) {//json日期格式转换为正常格式
    try {
        var date = new Date(parseInt(jsonDate.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
        var milliseconds = date.getMilliseconds();
        return date.getFullYear() + "-" + month + "-" + day;
    } catch (ex) {
        return "";
    }
}

/*日期格式化，截位数*/
(function ($) {
    var FormatDateTime = function FormatDateTime() { };
    $.FormatDateTime = function (obj, IsMi) {
        var correcttime1 = eval('( new ' + obj.replace(new RegExp("\/", "gm"), "") + ')');
        var myDate = correcttime1;
        var year = myDate.getFullYear();
        var month = ("0" + (myDate.getMonth() + 1)).slice(-2);
        var day = ("0" + myDate.getDate()).slice(-2);
        var h = ("0" + myDate.getHours()).slice(-2);
        var m = ("0" + myDate.getMinutes()).slice(-2);
        var s = ("0" + myDate.getSeconds()).slice(-2);
        var mi = ("00" + myDate.getMilliseconds()).slice(-3);
        if (IsMi == 'd') {

        } else if (IsMi == 's') {
            return year + "-" + month + "-" + day + " " + h + ":" + m + ":" + s;
        }
        else {
            return year + "-" + month + "-" + day + " " + h + ":" + m + ":" + s + " " + mi;
        }
    }
})(jQuery);
//------------------------------------字符串长度检测-----------------------------------------------//
///字符串长度检测方法，由于JS原先的长度中文跟英文一样一个字符为1个长度。所以这里就得需要大家自己判断并获取字符串的实际长度了。
//核心代码：

var GetLength = function (str) {
    ///<summary>获得字符串实际长度，中文2，英文1</summary>
    ///<param name="str">要获得长度的字符串</param>
    var realLength = 0, len = str.length, charCode = -1;
    for (var i = 0; i < len; i++) {
        charCode = str.charCodeAt(i);
        if (charCode >= 0 && charCode <= 128) realLength += 1;
        else realLength += 2;
    }
    return realLength;
};
//    执行代码：
//alert(jmz.GetLength('测试测试ceshiceshi));


function CurentTime() {
    var now = new Date();

    var year = now.getFullYear();       //年
    var month = now.getMonth() + 1;     //月
    var day = now.getDate();            //日

    var hh = now.getHours();            //时
    var mm = now.getMinutes();          //分

    var clock = year + "-";

    if (month < 10)
        clock += "0";

    clock += month + "-";

    if (day < 10)
        clock += "0";

    clock += day + " ";

    if (hh < 10)
        clock += "0";

    clock += hh + ":";
    if (mm < 10) clock += '0';
    clock += mm;
    return (clock);
}

function CurentDate() {
    var now = new Date();

    var year = now.getFullYear();       //年
    var month = now.getMonth() + 1;     //月
    var day = now.getDate();            //日


    var clock = year + "-";

    if (month < 10)
        clock += "0";

    clock += month + "-";

    if (day < 10)
        clock += "0";

    clock += day;

    return (clock);
} 
//验证手机号
function isMobel(value) {
    if (/^13\d{9}$/g.test(value) || /^15\d{9}$/g.test(value) || /^17\d{9}$/g.test(value) || /^14\d{9}$/g.test(value) ||
	/^18\d{9}$/g.test(value)) {
        return true;
    } else {
        return false;
    }
}
//验证数字
function CheckNumber(myform, name) {
    string = myform.value;
    var letters = "1234567890";
    var charlength = string.length;
    for (i = 0; i < charlength; i++) {
        var tempChar = string.charAt(i);
        if (letters.indexOf(tempChar) == -1) {
            alert("信息错误：\n\n " + name + " 必须由数字组成！");
            myform.focus();
            return false;
        }
    }
    return true;
}

//格式化日期
Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(), //day
        "h+": this.getHours(), //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds() //millisecond
    }

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}
//向日期添加天数
function DateAdd(sdate, days) {
    var a = new Date(sdate);
    a = a.valueOf();
    a = a + days * 24 * 60 * 60 * 1000;
    a = new Date(a);
    return a;
}

//日期差(d1-d2) 返回天数
function DateDiff(d1, d2) {
    var day = 24 * 60 * 60 * 1000;
    try {
        var dateArr = d1.split("-");
        var checkDate = new Date();
        checkDate.setFullYear(dateArr[0], dateArr[1] - 1, dateArr[2]);
        var checkTime = checkDate.getTime();

        var dateArr2 = d2.split("-");
        var checkDate2 = new Date();
        checkDate2.setFullYear(dateArr2[0], dateArr2[1] - 1, dateArr2[2]);
        var checkTime2 = checkDate2.getTime();

        var cha = (checkTime - checkTime2) / day;
        return cha;
    } catch (e) {
        return false;
    }
}


//时间差，返回秒钟
function dateDiff_seconds(date1, date2) {
    var type1 = typeof date1, type2 = typeof date2;
    if (type1 == 'string')
        date1 = stringToTime(date1);
    else if (date1.getTime)
        date1 = date1.getTime();
    if (type2 == 'string')
        date2 = stringToTime(date2);
    else if (date2.getTime)
        date2 = date2.getTime();
    return (date1 - date2) / 1000; //结果是秒 
}

//字符串转成Time(dateDiff)所需方法 
function stringToTime(string) {
    var f = string.split(' ', 2);
    var d = (f[0] ? f[0] : '').split('-', 3);
    var t = (f[1] ? f[1] : '').split(':', 3);
    return (new Date(
    parseInt(d[0], 10) || null,
    (parseInt(d[1], 10) || 1) - 1,
    parseInt(d[2], 10) || null,
    parseInt(t[0], 10) || null,
    parseInt(t[1], 10) || null,
    parseInt(t[2], 10) || null
    )).getTime();

}
//调用 dateDiff("2009-10-10 19:00:00","2009-10-10 18:00:00")
//返回的是秒钟

//------------------------------------身份证验证-----------------------------------------------//
var Wi = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1];    // 加权因子   
var ValideCode = [1, 0, 10, 9, 8, 7, 6, 5, 4, 3, 2];            // 身份证验证位值.10代表X   
function IdCardValidate(idCard) {
    idCard = trim(idCard.replace(/ /g, ""));               //去掉字符串头尾空格                     
    if (idCard.length == 15) {
        return isValidityBrithBy15IdCard(idCard);       //进行15位身份证的验证    
    } else if (idCard.length == 18) {
        var a_idCard = idCard.split("");                // 得到身份证数组   
        if (isValidityBrithBy18IdCard(idCard) && isTrueValidateCodeBy18IdCard(a_idCard)) {   //进行18位身份证的基本验证和第18位的验证
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
}
/**  
* 判断身份证号码为18位时最后的验证位是否正确  
* @param a_idCard 身份证号码数组  
* @return  
*/
function isTrueValidateCodeBy18IdCard(a_idCard) {
    var sum = 0;                             // 声明加权求和变量   
    if (a_idCard[17].toLowerCase() == 'x') {
        a_idCard[17] = 10;                    // 将最后位为x的验证码替换为10方便后续操作   
    }
    for (var i = 0; i < 17; i++) {
        sum += Wi[i] * a_idCard[i];            // 加权求和   
    }
    valCodePosition = sum % 11;                // 得到验证码所位置   
    if (a_idCard[17] == ValideCode[valCodePosition]) {
        return true;
    } else {
        return false;
    }
}
/**  
* 验证18位数身份证号码中的生日是否是有效生日  
* @param idCard 18位书身份证字符串  
* @return  
*/
function isValidityBrithBy18IdCard(idCard18) {
    var year = idCard18.substring(6, 10);
    var month = idCard18.substring(10, 12);
    var day = idCard18.substring(12, 14);
    var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
    // 这里用getFullYear()获取年份，避免千年虫问题   
    if (temp_date.getFullYear() != parseFloat(year)
          || temp_date.getMonth() != parseFloat(month) - 1
          || temp_date.getDate() != parseFloat(day)) {
        return false;
    } else {
        return true;
    }
}
/**  
* 验证15位数身份证号码中的生日是否是有效生日  
* @param idCard15 15位书身份证字符串  
* @return  
*/
function isValidityBrithBy15IdCard(idCard15) {
    var year = idCard15.substring(6, 8);
    var month = idCard15.substring(8, 10);
    var day = idCard15.substring(10, 12);
    var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
    // 对于老身份证中的你年龄则不需考虑千年虫问题而使用getYear()方法   
    if (temp_date.getYear() != parseFloat(year)
              || temp_date.getMonth() != parseFloat(month) - 1
              || temp_date.getDate() != parseFloat(day)) {
        return false;
    } else {
        return true;
    }
}
//去掉字符串头尾空格   
function trim(str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
}

/**  
* 通过身份证判断是男是女  
* @param idCard 15/18位身份证号码   
* @return 'female'-女、'male'-男  
*/
function maleOrFemalByIdCard(idCard) {
    idCard = trim(idCard.replace(/ /g, ""));        // 对身份证号码做处理。包括字符间有空格。   
    if (idCard.length == 15) {
        if (idCard.substring(14, 15) % 2 == 0) {
            return 'female';
        } else {
            return 'male';
        }
    } else if (idCard.length == 18) {
        if (idCard.substring(14, 17) % 2 == 0) {
            return 'female';
        } else {
            return 'male';
        }
    } else {
        return null;
    }
}


//显示转换折行  
function replacebr(str) {
    if (str == null) {
        return "";
    }
    if (str == "") {
        return "";
    }
    return str.replace(/\n|\r|(\r\n)|(\u0085)|(\u2028)|(\u2029)/g, "<br>");
}

//过滤html标签
function removeHTMLTag(str) {
    str = str.replace(/<[^>]+>/g, ''); //去除HTML tag
    str = str.replace(/&nbsp;/g, ''); //去掉 &nbsp;
    //str = str.replace(/[ ]/g, '');
    str = str.replace(/\s*/g, '');

    return str;
}


//酒店显示价格 单价*天数
function Hotelviewdanjia(price, star, end) {
    var daynum = DateDiff(end, star);
    if (daynum > 0) {
        if (daynum == 1) {
            price = price + "*1天";
        } else {
        price = price / daynum;
            price = price + "*" + daynum + "天";
        }
    }
    return price;
}

//判断浏览器类型
function fBrowserRedirect() {
    var sUserAgent = navigator.userAgent.toLowerCase();
    var bIsIpad = sUserAgent.match(/ipad/i) == "ipad";
    var bIsIphoneOs = sUserAgent.match(/iphone os/i) == "iphone os";
    var bIsMidp = sUserAgent.match(/midp/i) == "midp";
    var bIsUc7 = sUserAgent.match(/rv:1.2.3.4/i) == "rv:1.2.3.4";
    var bIsUc = sUserAgent.match(/ucweb/i) == "ucweb";
    var bIsAndroid = sUserAgent.match(/android/i) == "android";
    var bIsCE = sUserAgent.match(/windows ce/i) == "windows ce";
    var bIsWM = sUserAgent.match(/windows mobile/i) == "windows mobile";
    if (bIsIpad) {
        var sUrl = location.href;
        if (!bForcepc) {
            //            window.location.href = "http://ipad.mail.163.com/";
            return "ipad";
        }
    }
    else if (bIsIphoneOs || bIsAndroid) {
        var sUrl = location.href;
        if (!bForcepc) {
            //            window.location.href = "http://smart.mail.163.com/";
            return "mobile";
        }
    }
    else if (bIsMidp || bIsUc7 || bIsUc || bIsCE || bIsWM) {
        var sUrl = location.href;
        if (!bForcepc) {
            //            window.location.href = "http://m.mail.163.com/";
            return "handset";
        }
    }
    else {
        return "pc";
    }
}





