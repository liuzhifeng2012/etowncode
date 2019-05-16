(function ($) {
    $.widget('ets.formView', {
        options: {
            callback: {},
            btnPannel: ''
        },
        _create: function () {
        },
        show: function () {
            var _this = this;
            $(this.options.btnPannel).slideUp('800');
            $('.input-hook', this.element).each(function () {
                var v = _this._getVal(this, true);
                var p = $(this).parent('span');
                var show = $(p).next('span.input-show');
                if (show.length == 0) {
                    show = $('<span class="input-show slongInfo"></span>').insertAfter(p);
                }
                p.hide();
                v = v == '' ? '<span class="nodata">没有填写</span>' : v;
                show.html(v).show();
            });

        },
        _getVal: function (o, isShow, val) {
            var v;
            var meta = $.metadata.get(o).callback;
            if (meta) {
                if (isShow) {
                    v = this.options.callback[meta].show(o);
                }
                else {
                    v = this.options.callback[meta].edit(o, val);
                }
                return v;
            }
            var $o = $(o);
            if ($o.is('input') || $o.is('textarea')) {
                return $o.val();
            }
            else if ($o.is('select')) {
                return $o.children('option:selected').text();
            }
            return null;
        },
        _getmeta: function (o) {
            return $.metadata.get(o).callback;
        },
        edit: function () {
            $(this.options.btnPannel).show();
            var _this = this;
            $('.input-hook', this.element).each(function () {
                var valSpan = $(this).parent('span').next('span.input-show');
                var v = _this._getVal(this, false, valSpan.text());
                var m = _this._getmeta(this);
                if (m) {
                    return;
                }
                $(this).val(v).parent('span').show();
                valSpan.hide();
            });

        }
    });
})(jQuery);


(function ($) {
    $.widget('ets.newFormView', {
        options: {
            callback: {},
            btnPannel: ''
        },
        _create: function () {
            var _this = this;
            $('.input-hook', this.element).each(function () {
                var p = $(this).parent('span');
                var show = $(p).next('span.input-show');
                var v = _this._getVal(this, true, show.text(), true);
                v = v == '' ? '<span class="nodata">没有填写</span>' : v;
                if (show.length == 0) {
                    show = $('<span class="input-show slongInfo">' + v + '</span>').insertAfter(p);
                }
                p.hide();
                show.html(v).show();
            });
        },
        _init: function () {

        },
        init: function () {
        },
        show: function () {
            var _this = this;
            $(this.options.btnPannel).slideUp('800');
            $('.input-hook', this.element).each(function () {
                var p = $(this).parent('span');
                var show = $(p).next('span.input-show');
                var v = _this._getVal(this, true, show.text(), false);
                p.hide();
                show.show();
            });

        },
        _getVal: function (o, isShow, val, isInit) {
            var v;
            var meta = $.metadata.get(o).callback;
            if (meta) {
                if (isInit) {
                    this.options.callback[meta].init(o);
                }
                if (isShow) {
                    v = this.options.callback[meta].show(o);
                }
                else {
                    v = this.options.callback[meta].edit(o, val);
                }
                return v;
            }
            var $o = $(o);
            if ($o.is('input') || $o.is('textarea')) {
                return $o.val();
            }
            else if ($o.is('select')) {
                return $o.children('option:selected').text();
            }
            return null;
        },
        _getmeta: function (o) {
            return $.metadata.get(o).callback;
        },
        edit: function () {
            $(this.options.btnPannel).show();
            var _this = this;
            $('.input-hook', this.element).each(function () {
                var valSpan = $(this).parent('span').next('span.input-show');
                var v = _this._getVal(this, false, valSpan.html(), false);
                var m = _this._getmeta(this);
                if (m) {
                    return;
                }
                var val = valSpan.html();
                if (valSpan.html().indexOf('>没有填写</') > -1) {
                    val = '';
                }
                $(this).val(val).parent('span').show();
                valSpan.hide();
            });

        },
        setVal: function () {
            var _this = this;
            $('.input-hook', this.element).each(function () {
                var p = $(this).parent('span');
                var show = $(p).next('span.input-show');
                var v = _this._getVal(this, true, show.text(), true);
                if (show.length == 0) {
                    show = $('<span class="input-show slongInfo"></span>').insertAfter(p);
                }
                p.hide();
                v = v == '' ? '<span class="nodata">没有填写</span>' : v;
                show.html(v).show();
            });
        }

    });
})(jQuery);

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
        return (/^(?:13\d|15\d|18\d|14\d)-?\d{5}(\d{3}|\*{3})$/.test($.trim(v)));
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
    }
});

/*(function ($) {

$.widget('ets.jTable', {
options: {
url: '',
tblParam: null,
columnsDefs: [],
bPaginate: true,
aaSorting: [[0, 'desc']],
callback: null,
bSort: true,
bLengthChange: true,
iDisplayLength: 25,
titleFloat: true,
bAutoWidth: false,
fnInitComplete: null,
sScrollX: null
},
_create: function () {
var _this = this;
this._bindTbl();

$('input:checkbox', this.element).click(function () {
var cls = _this.otbl.fnSettings().aoColumns;
for (var c in cls) {
if (cls[c].sName == $(this).val()) {
_this.otbl.fnSetColumnVis(c, $(this).is(':checked'));
}
}
});

$('.config-hook', this.element).poshytip({
content: $('.select-hook', _this.element),
className: 'tip-gray',
alignTo: 'target',
alignY: 'bottom',
alignX: 'center',
offsetY: 0
});
},
_init: function () {
},

rebind: function (param) {
//            var p = this.options.tblParam || [];
//            for (var item in param) {
//                p.push(param[item]);
//            }
this.options.tblParam = param;

this.otbl.fnDraw();
//this.otbl.fn
},
_bindTbl: function () {
var cls = this._getSelect();
var _this = this;
var remove = false;
this.otbl = $("table", this.element).dataTable({
"sDom": 'lfrt<"listfooter"ip>',
"bProcessing": true,
"bServerSide": true,
"sAjaxSource": this.options.url,
'aoColumns': cls,
'bFilter': false,
'bDestroy': true,
'bPaginate': this.options.bPaginate,
"sPaginationType": "full_numbers",
"iDisplayLength": this.options.iDisplayLength,
"bLengthChange": this.options.bLengthChange,
"aaSorting": this.options.aaSorting,
"bSort": this.options.bSort,
"bAutoWidth": this.options.bAutoWidth,
sColumns: _this._getSelect(true).join(','),
aoColumnDefs: _this.options.columnsDefs,
'fnInitComplete': this.options.fnInitComplete,
"sScrollX": this.options.sScrollX,
oLanguage: {
"sLengthMenu": "每页显示 _MENU_ 条",
"sZeroRecords": "没有您要搜索的内容",
"sInfo": "从_START_ 到 _END_ 条记录——总记录数为 _TOTAL_ 条",
"sInfoEmpty": "记录数为0",
"sInfoFiltered": "(全部记录数 _MAX_  条)",
"sInfoPostFix": "",
"oPaginate": {
"sLast": "尾页",
"sNext": "下一页",
"sPrevious": "上一页",
"sFirst": "首页"
},
"sProcessing": "正在加载数据..."
},
"fnServerData": function (sSource, aoData, fnCallback) {
if (_this.options.tblParam && _this.options.tblParam.length > 0) {
$(_this.options.tblParam).each(function (i, n) {
aoData.push(n);
});
}
$.post(sSource, aoData, function (data) {

if (_this.options.callback != null) {
_this.options.callback(data);
}

fnCallback(data);
});
},
"fnDrawCallback": function (oSettings) {
var wrapper = $(this).closest('dataTables_wrapper');
//                    var len = $('tr:first', _this.otbl).children('th').length;
//                    var settings = oSettings;
//                    $('tbody tr', _this.otbl).each(function (i, n) {
//                        var title = $('td',n).eq(2).html();
//                        $('<tr><td colspan="' + len + '">'+ title +'</td></tr>').insertBefore(n);
//                    });
}
});
if (_this.options.titleFloat) {
new FixedHeader(this.otbl);
}

},
_getSelect: function (isSelected) {
var cls = [];
$('input:checkbox', this.element).each(function () {
if (isSelected) {
//                    if ($(this).attr("row") != "true") {
if ($(this).is(':checked')) {
cls.push($(this).val());

}
//                    }
}
else {
var l = {};
l.sName = $(this).val();


l.sTitle = $(this).attr('title');
l.mDataProp = $(this).val();
var twidth = $(this).attr("tWidth");
if ($.trim(twidth) != "")
l.sWidth = twidth;
var data = $.metadata.get(this);

l.sClass = data == null ? null : data.className;
if (!$(this).is(':checked')) {
l.bVisible = false;
}

cls.push(l);
}
});
return cls;
}
});
})(jQuery);
*/

//注释时间:20140626 可能以后会用到
///* Chinese initialisation for the jQuery UI date picker plugin. */
///* Written by Cloudream (cloudream@gmail.com). */
//jQuery(function ($) {
//    $.datepicker.regional['zh-CN'] = {
//        closeText: '关闭',
//        prevText: '&#x3c;上月',
//        nextText: '下月&#x3e;',
//        currentText: '今天',
//        monthNames: ['一月', '二月', '三月', '四月', '五月', '六月',
//		'七月', '八月', '九月', '十月', '十一月', '十二月'],
//        monthNamesShort: ['一', '二', '三', '四', '五', '六',
//		'七', '八', '九', '十', '十一', '十二'],
//        dayNames: ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],
//        dayNamesShort: ['周日', '周一', '周二', '周三', '周四', '周五', '周六'],
//        dayNamesMin: ['日', '一', '二', '三', '四', '五', '六'],
//        weekHeader: '周',
//        dateFormat: 'yy-mm-dd',
//        firstDay: 1,
//        isRTL: false,
//        showMonthAfterYear: true,
//        yearSuffix: '年'
//    };
//    $.datepicker.setDefaults($.datepicker.regional['zh-CN']);
//    $.datepicker.setDefaults({ altFormat: 'yyyy-MM-dd' });
//});

/*
(function ($) {

$.SelectProviser = function (option, func) {
var companyListCon = [
{

html: "<span class='company-hookLoading'><img src='http://file.etschina.com/Static/ETSApp/images/Ico/loading.gif'></span>",
submit: SelectCallback,
buttons: [{ title: '选择', value: true }, { title: '取消', value: false}],
focus: 1,
show: 'slideDown'
}
];
var prmp = $.prompt(companyListCon, { title: '选择卖家', opacity: 0.3 });

var businessStatus = 0;
if (option.businessStatus != undefined) {
businessStatus = option.businessStatus;
}
var status = 0;
if (option.status != undefined) {
status = option.status;
}

$.ajax({
type: "Get",
cache: false,
data: {},
url: "/UI/CRMUI/Control/CompanyControl/ProviderList.ajax",
success: function (data) {
$('.company-hookLoading', prmp).html(data);
}
});

function SelectCallback(e, v, m, f) {
if (v) {
var o = $(this).find('input:radio[name="company"]:checked').val();
func(JSON.parse(o));
}
return true;
}
};
})(jQuery);

(function ($) {

$.SelectCompany = function (option, func) {
var companyListCon = [
{

html: "<span class='company-hookLoading'><img src='http://file.etschina.com/Static/ETSApp/images/Ico/loading.gif'></span>",
submit: SelectCallback,
buttons: [{ title: '选择', value: true }, { title: '取消', value: false}],
focus: 1,
show: 'slideDown'
}
];
var prmp = $.prompt(companyListCon, { title: '选择公司', opacity: 0.3 });

var businessStatus = 0;
if (option.businessStatus != undefined) {
businessStatus = option.businessStatus;
}
var status = 0;
if (option.status != undefined) {
status = option.status;
}

$.ajax({
type: "Get",
data: { orgType: option.orgType, businessAreaIds: "", businessStatus: businessStatus, status: status, code: Math.random() },
url: "/UI/CRMUI/Control/CompanyControl/CompanySelectList.ajax",
success: function (data) {
$('.company-hookLoading', prmp).html(data);
}
});

function SelectCallback(e, v, m, f) {
if (v) {
var o = $(this).find('input:radio[name="company"]:checked').val();
func(JSON.parse(o));
}
return true;
}
};
})(jQuery);

(function ($) {

$.SelectCity = function (option, func) {
var cityListCon = [
{

html: "<span class='flight-hookLoading'><img src='http://file.etschina.com/Static/ETSApp/images/Ico/loading.gif'></span>",
submit: SelectCallback,
buttons: [{ title: '选择', value: true }, { title: '取消', value: false}],
focus: 1,
show: 'slideDown'
}
];
var prmp = $.prompt(cityListCon, { title: '选择机场', opacity: 0.3 });

$.ajax({
type: "Get",
data: { keyWord: option.keyWord, code: Math.random() },
url: "/UI/POUI/Control/FlightsControl/SelectCity.ajax",
success: function (data) {
$('.flight-hookLoading', prmp).html(data);
}
});

function SelectCallback(e, v, m, f) {
if (v) {
var o = $(this).find('input:radio[name="flight"]:checked').val();
func(JSON.parse(o));
}
return true;
}
};
})(jQuery);

(function ($) {

$.SelectCompanyByArea = function (option, func) {
var companyListCon = [
{

html: "<span class='company-hookLoading'><img src='http://file.etschina.com/Static/ETSApp/images/Ico/loading.gif'></span>",
submit: SelectCallback,
buttons: [{ title: '选择', value: true }, { title: '取消', value: false}],
focus: 1,
show: 'slideDown'
}
];
var prmp = $.prompt(companyListCon, { title: '选择公司', opacity: 0.3 });
$.ajax({
type: "Get",
data: { orgType: option.orgType, businessAreaIds: option.businessAreaIds },
url: "/UI/CRMUI/Control/CompanyControl/CompanySelectList.ajax",
success: function (data) {
$('.company-hookLoading', prmp).html(data);
}
});

function SelectCallback(e, v, m, f) {
if (v) {
var o = $(this).find('input:radio[name="company"]:checked').val();
func(JSON.parse(o));
}
return true;
}
};
})(jQuery);

(function ($) {

$.SelectProvider = function (option, func) {
var companyListCon = [
{

html: "<span class='company-hookLoading'><img src='http://file.etschina.com/Static/ETSApp/images/Ico/loading.gif'></span>",
submit: SelectCallback,
buttons: [{ title: '选择', value: true }, { title: '取消', value: false}],
focus: 1,
show: 'slideDown'
}
];
var prmp = $.prompt(companyListCon, { title: '选择公司', opacity: 0.3 });



$.ajax({
type: "Get",
data: { orgType: option.orgType, businessAreaIds: "", businessStatus: businessStatus, status: status, code: Math.random() },
url: "/UI/CRMUI/Control/CompanyControl/CompanySelectList.ajax",
success: function (data) {
$('.company-hookLoading', prmp).html(data);
}
});

//        $.ajax({
//            type: "Get",
//            data: { orgType: option.orgType },
//            url: "/UI/CRMUI/Control/newCompanyControl/SelectCompanyList.ajax",
//            success: function (data) {
//                $('.company-hookLoading', prmp).html(data);
//            }
//        });

function SelectCallback(e, v, m, f) {
if (v) {
var o = $(this).find('input:radio[name="company"]:checked').val();
func(JSON.parse(o));
}
return true;
}
};
})(jQuery);

(function ($) {

$.SelectBank = function (option, func) {
var bankListCon = [
{
html: "<span class='bank-hookLoading'><img src='http://file.etschina.com/Static/ETSApp/images/Ico/loading.gif'></span>",
submit: SelectCallback,
buttons: [{ title: '选择', value: true }, { title: '取消', value: false}],
focus: 1,
show: 'slideDown'
}
];
var prmp = $.prompt(bankListCon, { title: '选择开户行', opacity: 0.3 });

$.ajax({
type: "Get",
url: "/UI/CommonUI/Control/BanksSelectList.ajax",
data: { city: option.city, province: option.province, bankName: option.bankName },
success: function (data) {
$('.bank-hookLoading', prmp).html(data);
}
});

function SelectCallback(e, v, m, f) {
if (v) {
var o = $(this).find('input:radio[name="bank"]:checked').val();
func(JSON.parse(o));
}
return true;
}
};
})(jQuery);

(function ($) {

$.SelectUserList = function (option, func) {
var companyListCon = [
{
html: "<span class='company-hookLoading'><img src='http://file.etschina.com/Static/ETSApp/images/Ico/loading.gif'></span>",
submit: SelectCallback,
buttons: [{ title: '选择', value: true }, { title: '取消', value: false}],
focus: 1,
show: 'slideDown'
}
];
var prmp = $.prompt(companyListCon, { title: '选择联系人', opacity: 0.3 });

$.ajax({
type: "Get",
url: "/UI/CommonUI/Control/SelectUserList.ajax",
success: function (data) {
$('.company-hookLoading', prmp).html(data);
}
});

function SelectCallback(e, v, m, f) {
var aryRole = [];
if (v) {
$(this).find(':checkbox[name="user"]').each(function () {
if ($(this).attr("checked") == "checked")
aryRole.push(JSON.parse($(this).val()));
});

func(aryRole);
}
return true;
}
};
})(jQuery);

(function ($) {

$.SelectOwnProvider = function (option, func) {
var companyListCon = [
{

html: "<span class='company-hookLoading'><img src='http://file.etschina.com/Static/ETSApp/images/Ico/loading.gif'></span>",
submit: SelectCallback,
buttons: [{ title: '选择', value: true }, { title: '取消', value: false}],
focus: 1,
show: 'slideDown'
}
];
var prmp = $.prompt(companyListCon, { title: '选择公司', opacity: 0.3 });

$.ajax({
type: "Get",
url: "/UI/CustomerUI/Control/CompanyControl/SelectCompanyControl.ajax",
success: function (data) {
$('.company-hookLoading', prmp).html(data);
}
});

function SelectCallback(e, v, m, f) {
if (v) {
var o = $(this).find('input:radio[name="company"]:checked').val();
func(JSON.parse(o));
}
return true;
}
};
})(jQuery);

(function ($) {

$.SelectCustomer = function (option, func) {
var customerListCon = [
{
html: "<span class='customer-hookLoading'><img src='http://file.etschina.com/Static/ETSApp/images/Ico/loading.gif'></span>",
submit: SelectCallback,
buttons: [{ title: '选择', value: true }, { title: '取消', value: false}],
focus: 1,
show: 'slideDown'
}
];
var prmp = $.prompt(customerListCon, { opacity: 0.3 });

$.ajax({
type: "Get",
data: { sex: option.sex, name: option.name },
url: "/UI/CustomerUI/Control/CustomerControl/SelectCustomerControl.ajax",
success: function (data) {
$('.customer-hookLoading', prmp).html(data);
}
});

function SelectCallback(e, v, m, f) {
if (v) {
var o = $(this).find('input:radio[name="customer"]:checked').val();
func(JSON.parse(o));
}
return true;
}
};
})(jQuery);

(function ($) {

$.SelectUser = function (option, func) {
var userListCon = [
{
html: "<span class='company-hookLoading'><img src='http://file.etschina.com/Static/ETSApp/images/Ico/loading.gif'></span>",
submit: SelectCallback,
buttons: [{ title: '选择', value: true }, { title: '取消', value: false}],
focus: 1,
show: 'slideDown'
}
];
var prmp = $.prompt(userListCon, { title: '选择联系人', opacity: 0.3 });

$.ajax({
type: "Get",
url: "/UI/CRMUI/Control/UserControl/SelectUserList.ajax",
success: function (data) {
$('.company-hookLoading', prmp).html(data);
}
});

function SelectCallback(e, v, m, f) {
var aryRole = [];
if (v) {
$(this).find(':checkbox[name="user"]').each(function () {
if ($(this).attr("checked") == "checked")
aryRole.push(JSON.parse($(this).val()));
});

func(aryRole);
}
return true;
}
};
})(jQuery);



(function ($) {
$.SelectRole = function (option, func) {
var result = "";
var roleIDsAry = "";
if (option.roleIDs != "") {
roleIDsAry = option.roleIDs.split(",");
}
$.ajax({
type: "POST",
data: { cType: option.cType },
url: "/json/user/roles",
success: function (data) {
result = "<ul>";
for (var i = 0; i < data.length; i++) {
result += "<li>";
var a = jQuery.inArray(data[i].Id.toString(), roleIDsAry);
if (a > -1) {
result += "<input type='checkbox' checked='checked' value='{\"Id\":\"" + data[i].Id + "\",\"Name\":\"" + data[i].Name + "\"}'>" + data[i].Name;
} else {
result += "<input type='checkbox' value='{\"Id\":\"" + data[i].Id + "\",\"Name\":\"" + data[i].Name + "\"}'>" + data[i].Name;
}
result += "</li>";
}

result += "</ul>";
$.prompt(result, {
submit: SelectCallback,
buttons: [{ title: '确认', value: true }, { title: '取消', value: false}],
show: 'slideDown',
title: '选择角色'
});
$(this).find("a").each(function () {
var href = $(this).attr("href");
});
}
});

function SelectCallback(e, v, m, f) {
var aryRole = [];
if (v) {
$(this).find(':checkbox').each(function () {
if ($(this).attr("checked") == "checked")
aryRole.push(JSON.parse($(this).val()));
});

func(aryRole);
}
return true;
}
};
})(jQuery);



(function ($) {
$.widget('ets.exportData', {
options: {
sltPannel: '',
type: 0
},
_create: function () {
// jQuery.support.cors = true;
var _this = this;
this.$pmt = null;
this.stop = false;
$(this.element).click(function () {
_this.stop = false;
_this.$pmt = $.prompt('<div style="margin:auto;width:200px;" class="export-hook"><img src="http://file.etschina.com/Static/ETSApp/images/Ico/loading.gif"/><br/><span>准备提交数据...</span></div>', {
buttons: [
{ title: '取消', value: false }
],
title: '数据导出',
callback: function (v) {
_this.stop = true;
},
loaded: function () {
_this._postData();
}
});
});

},
_getColumns: function () {
var clm = [];
$('input[type="checkbox"]:checked', this.options.sltPannel).each(function () {
clm.push($(this).val());
});
return clm.join(',');
},
_loopQuery: function (url) {
var _this = this;
var box = $('.export-hook span', this.$pmt);
if (this.stop) {
return;
}
$.get(url + '?' + Math.random(), function (data) {
if (data.indexOf('http') == 0) {
box.parent('div').html('生成数据完成，<a href="' + data + '" target="_blank">点此下载</a>');
_this.stop = true;
return;
}
if (data.indexOf('Error') == 0 || _this.stop) {
box.html(data);
_this.stop = true;
return;
}
box.html(data);
setTimeout(function () {
_this._loopQuery(url);
}, 1000);
}).error(function (a, b, c) {
var e = a;
});
},
_postData: function (clms) {
var _this = this;
var box = $('.export-hook', this.$pmt);
$.post('/json/report/', { type: this.options.type, columns: this._getColumns() }, function (data) {
//                if (data.url.indexOf('http') == 0) {
_this._loopQuery(data.url);
return;
//                }
//                box.html(data.url);
});
}
});
})(jQuery);


(function ($) {

$.SelectProductSuject = function (option, func) {
var result = "";
var subjectNamesAry = "";
if (option.subjectNames != "") {
subjectNamesAry = option.subjectNames.split(",");
}
$.ajax({
type: "Get",
url: "/json/tourproduct/subject",
success: function (data) {

var itemsList = "";
if (data != null && data.length > 0) {
$(data).each(function (i, n) {
var a = jQuery.inArray(data[i].Name, subjectNamesAry);
if (a > -1) {
itemsList += "<div class=\"ddgreen\"><div class=\"t\"></div><div class=\"m\"><a href='javascript:void(0)'>" + data[i].Name + "</a></div><div class=\"w\"></div></div>";
} else {
itemsList += "<div class=\"ddblue\"><div class=\"t\"></div><div class=\"m\"><a href='javascript:void(0)'>" + data[i].Name + "</a></div><div class=\"w\"></div></div>";
}
});
}
result = "<div class=\"zhuti\">" + itemsList + "<div class=\"cls\"></div></div>";

$.prompt(result, {
title: '选择主题',
submit: SelectCallback,
buttons: [{ title: '选择', value: true }, { title: '取消', value: false}],
show: 'slideDown'
});


$(".zhuti .m a").click(function () {
var currentClass = $(this).parent().parent().attr("class");
if (currentClass == "ddgreen") {
$(this).parent().parent().attr("class", "ddblue");
} else {
$(this).parent().parent().attr("class", "ddgreen");
}
});
}
});

function SelectCallback(e, v, m, f) {

var arySubject = [];
if (v) {
var b = $(".zhuti .ddgreen .m a");
for (var i = 0; i < b.length; i++) {
arySubject.push(JSON.parse('{"Name":"' + $(b[i]).html() + '"}'));
}
func(arySubject);
}
return true;

}
};
})(jQuery);




function convertCurrency(currencyDigits) {
// Constants:
var MAXIMUM_NUMBER = 99999999999.99;
// Predefine the radix characters and currency symbols for output:
var CN_ZERO = "零";
var CN_ONE = "壹";
var CN_TWO = "贰";
var CN_THREE = "叁";
var CN_FOUR = "肆";
var CN_FIVE = "伍";
var CN_SIX = "陆";
var CN_SEVEN = "柒";
var CN_EIGHT = "捌";
var CN_NINE = "玖";
var CN_TEN = "拾";
var CN_HUNDRED = "佰";
var CN_THOUSAND = "仟";
var CN_TEN_THOUSAND = "万";
var CN_HUNDRED_MILLION = "亿";
var CN_SYMBOL = "人民币";
var CN_DOLLAR = "元";
var CN_TEN_CENT = "角";
var CN_CENT = "分";
var CN_INTEGER = "整";

// Variables:
var integral; // Represent integral part of digit number.
var decimal; // Represent decimal part of digit number.
var outputCharacters; // The output result.
var parts;
var digits, radices, bigRadices, decimals;
var zeroCount;
var i, p, d;
var quotient, modulus;

// Validate input string:
currencyDigits = currencyDigits.toString();
if (currencyDigits == "") {
alert("Empty input!");
return "";
}
if (currencyDigits.match(/[^,.\d]/) != null) {
alert("Invalid characters in the input string!");
return "";
}
if ((currencyDigits).match(/^((\d{1,3}(,\d{3})*(.((\d{3},)*\d{1,3}))?)|(\d+(.\d+)?))$/) == null) {
alert("Illegal format of digit number!");
return "";
}

// Normalize the format of input digits:
currencyDigits = currencyDigits.replace(/,/g, ""); // Remove comma delimiters.
currencyDigits = currencyDigits.replace(/^0+/, ""); // Trim zeros at the beginning.
// Assert the number is not greater than the maximum number.
if (Number(currencyDigits) > MAXIMUM_NUMBER) {
alert("Too large a number to convert!");
return "";
}

// Process the coversion from currency digits to characters:
// Separate integral and decimal parts before processing coversion:
parts = currencyDigits.split(".");
if (parts.length > 1) {
integral = parts[0];
decimal = parts[1];
// Cut down redundant decimal digits that are after the second.
decimal = decimal.substr(0, 2);
}
else {
integral = parts[0];
decimal = "";
}
// Prepare the characters corresponding to the digits:
digits = new Array(CN_ZERO, CN_ONE, CN_TWO, CN_THREE, CN_FOUR, CN_FIVE, CN_SIX, CN_SEVEN, CN_EIGHT, CN_NINE);
radices = new Array("", CN_TEN, CN_HUNDRED, CN_THOUSAND);
bigRadices = new Array("", CN_TEN_THOUSAND, CN_HUNDRED_MILLION);
decimals = new Array(CN_TEN_CENT, CN_CENT);
// Start processing:
outputCharacters = "";
// Process integral part if it is larger than 0:
if (Number(integral) > 0) {
zeroCount = 0;
for (i = 0; i < integral.length; i++) {
p = integral.length - i - 1;
d = integral.substr(i, 1);
quotient = p / 4;
modulus = p % 4;
if (d == "0") {
zeroCount++;
}
else {
if (zeroCount > 0) {
outputCharacters += digits[0];
}
zeroCount = 0;
outputCharacters += digits[Number(d)] + radices[modulus];
}
if (modulus == 0 && zeroCount < 4) {
outputCharacters += bigRadices[quotient];
}
}
outputCharacters += CN_DOLLAR;
}
// Process decimal part if there is:
if (decimal != "") {
for (i = 0; i < decimal.length; i++) {
d = decimal.substr(i, 1);
if (d != "0") {
outputCharacters += digits[Number(d)] + decimals[i];
}
}
}
// Confirm and return the final output string:
if (outputCharacters == "") {
outputCharacters = CN_ZERO + CN_DOLLAR;
}
if (decimal == "") {
outputCharacters += CN_INTEGER;
}
outputCharacters = outputCharacters;
return outputCharacters;
}

Date.prototype.dateAdd = function (interval, number) {
var d = this;
var k = { 'y': 'FullYear', 'q': 'Month', 'm': 'Month', 'w': 'Date', 'd': 'Date', 'h': 'Hours', 'n': 'Minutes', 's': 'Seconds', 'ms': 'MilliSeconds' };
var n = { 'q': 3, 'w': 7 };
eval('d.set' + k[interval] + '(d.get' + k[interval] + '()+' + ((n[interval] || 1) * number) + ')');
return d;
};

Date.prototype.Format = function (formatStr) {
var str = formatStr;
var Week = ['日', '一', '二', '三', '四', '五', '六'];
var month = this.getMonth() + 1;

str = str.replace(/yyyy|YYYY/, this.getFullYear());
str = str.replace(/yy|YY/, (this.getYear() % 100) > 9 ? (this.getYear() % 100).toString() : '0' + (this.getYear() % 100));

str = str.replace(/MM/, month > 9 ? month.toString() : '0' + month);
str = str.replace(/M/g, month);

str = str.replace(/w|W/g, Week[this.getDay()]);

str = str.replace(/dd|DD/, this.getDate() > 9 ? this.getDate().toString() : '0' + this.getDate());
str = str.replace(/d|D/g, this.getDate());

str = str.replace(/hh|HH/, this.getHours() > 9 ? this.getHours().toString() : '0' + this.getHours());
str = str.replace(/h|H/g, this.getHours());
str = str.replace(/mm/, this.getMinutes() > 9 ? this.getMinutes().toString() : '0' + this.getMinutes());
str = str.replace(/m/g, this.getMinutes());

str = str.replace(/ss|SS/, this.getSeconds() > 9 ? this.getSeconds().toString() : '0' + this.getSeconds());
str = str.replace(/s|S/g, this.getSeconds());

return str;
};

$('.lnkTipposhytip').poshytip({
className: 'tip-gray',
bgImageFrameSize: 11,
alignX: 'center',
alignY: 'top',
alignTo: 'target',
offsetY: 0,
offsetX: 0,
content: function (updateCallback) {
$(this).attr("onclick", 'return false;')
$.ajax({
url: $(this).attr("href"),
data: {
n: Math.random()
},
cache: false,
success: function (txt) {
updateCallback(txt);

}
});
return '正在加载...'
}
});

(function ($) {
$.widget('ets.stickyNav', {
options: {
left: 0
},
_create: function () {
this.$top = $(this.element).offset().top;
this._sticky();
var _this = this;
$(window).scroll(function () {
_this._sticky();
});
},
_sticky: function () {
var scrollTop = $(window).scrollTop();
if (scrollTop > this.$top) {
$(this.element).css({ 'position': 'fixed', 'top': '0', 'left': this.options.left });
}
else {
$(this.element).css({ position: 'static' });
}
}
});
})(jQuery);
*/
/*column setting start
$(function () {
    //save data
    //如果是mouseout，子元素也会触发该事件,改成mouseleave
    $("div ul.select-hook").mouseleave(function () {
        //alert('设置数据');
        var vCheck = "";
        $("div ul.select-hook :checked").each(function () {
            vCheck += $(this).val() + ",";
        });


        $.ajax({
            type: "POST",
            data: { 'Name': vCheck },
            dataType: "text",
            url: "/json/order/SetDisplayColumn",
            success: function (data) {

                //if(data!="0"){
                //alert(data);
                //}

            }
        });

    });

    if ($("div ul.select-hook :checkbox").length > 0) {
        //GetDisplayColumn(); //show colunm
    }

    function GetDisplayColumn() {

        $.ajax({
            type: "POST",
            //data: { 'PageUrl': vUrl },
            url: "/json/order/GetDisplayColumn",
            async: false,
            dataType: "text",
            success: function (data) {
                if (data != null && data != "default") {
                    var arrCheck = data.split(',');
                    $("div ul.select-hook :checkbox").each(function () {//#divColumn ul li
                        $(this).attr("checked", false);
                        for (var i = 0; i < arrCheck.length; i++) {
                            if ($(this).val() == arrCheck[i]) {
                                $(this).attr("checked", true);
                                break;
                            }


                        }
                    })
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(XMLHttpRequest.responseText);

            }
        });


    }

}); 

 column setting end 

//日期转换，兼容IE，Chrome
function NewDate(str) {
    str = str.split('-');
    var date = new Date();
    date.setUTCFullYear(str[0], str[1] - 1, str[2]);
    date.setUTCHours(0, 0, 0, 0);
    return date;
} */