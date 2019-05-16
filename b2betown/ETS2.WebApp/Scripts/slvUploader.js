//var slvUploader = function (pannel, result, showBox, max) {
//    this.pannel = $('#' + pannel);
//    this.result = $('#' + result);
//    this.max = max || 0;
//    this.showBox = showBox || true;
//    this.init();
//};
//slvUploader.prototype = {
//    init: function () {
//        var v = this.result.val();
//        if ($.trim(v) == '') {
//            return;
//        }
//        //var ary = JSON.parse(v);
//        var ary = jQuery.parseJSON(v);//兼容IE6，7,8
//        var _this = this;
//        $(ary).each(function (i, n) {
//            if (n) {
//                _this.insertItem(n.Id, n.RelativePath, n.OrigenalName);
//            }
//        });
//    },
//    onUploaded: function (jsonFileEntity) {
//        var entity = JSON.parse(jsonFileEntity);

//        this.insertItem(entity.Id, entity.RelativePath, entity.OrigenalName);

//        this.save(entity.Id, entity.RelativePath, entity.OrigenalName);
//    },
//    insertItem: function (id, path, name) {

//        if (parseInt(this.max) == 1 && this.pannel.find('input[type="checkbox"]').length > 0) {
//            this.pannel.find('input[type="checkbox"]').val(path);
//            this.pannel.find('input[type="checkbox"]').attr('pId', id);
//            this.pannel.find('span').html(name);
//        }
//        else {
//            if (this.pannel.find('input[value="' + path + '"]').length > 0) {
//                return;
//            }
//            var html = $('<div><input type="checkbox" value="' + path + '" pId="' + id + '" checked="true"/><span>' + name + '</span></div>');
//            var _this = this;
//            html.children('input').click(function () {
//                _this.onCheck();
//            });
//            if (!this.showBox) {
//                $('input', html).hide();
//            }
//            html.appendTo(this.pannel);
//        }
//    },
//    save: function (id, path, name) {
//        var v = this.result.val();
//        var ary = [];
//        if ($.trim(v) != '') {
//            ary = JSON.parse(v)
//        }
//        ary.push({ Id: id, RelativePath: path, OrigenalName: name });
//        v = JSON.stringify(ary)
//        this.result.val(v);
//    },
//    onCheck: function (o) {
//        var ary = [];
//        this.pannel.find('input').each(function (i, n) {
//            if ($(this).attr('checked')) {
//                ary.push({ Id: $(this).attr("pId"), RelativePath: $(this).val(), OrigenalName: $(this).next('span').text() });
//                //                val += $(this).val();
//                //                val += '^';
//                //                val += $(this).next('span').text();
//                //                val += '*';
//            }
//        });
//        var val = JSON.stringify(ary);
//        this.result.val(val);
//    }
//};

//(function ($) {
//    $.widget('ets.showFile',{
//        options : {
//            data : '',
//            host : ''
//        },
//        _create : function(){
//           this.load(this.options.data);    
//        },
//        load : function(data){ 
//            var _this = this;
//            //var host = $.metadata.get(this.element);
//            $.post('/json/file/',{aryId : data},function(retData){
//                $(_this.element).empty();
//                $(retData).each(function(i,n){
//                    $(['<div style="border:1px solid #cccccc; padding:5px; float:left; margin:5px;">',
//                        '<a class="" href="'+ _this.options.host + n.RelativePath +'" target="_blank">',
//                        n.OrigenalName,
//                        '</a>',
//                       '</div>'].join(''))
//                       .poshytip({
//                        className: 'tip-gray',
//                        bgImageFrameSize: 11,
//                        alignX : 'center',
//                        alignY : 'top',
//                        alignTo : 'target',
//                        offsetY : 0,
//                        offsetX : 0,
//                        content: '<img src="'+ _this.options.host + n.RelativePath +'" />' }) 
//                       .appendTo(_this.element);
//                });
//            });
//        },
//        reBind : function(){
//            var ary = []
//            $('input[type="checkbox"]:checked',$(this.element).prev('div')).each(function(i,n){
//                ary.push($(this).val());
//            });
//            this.load(ary.join(','));
//        }
//    });
//})(jQuery);

function getUploadResult(cssName){
    var ary = [];
    $('input[type="checkbox"]:checked', cssName).each(function (i, n) {
        ary.push($(this).val());
    });
    return ary.join(',');
}
function getUploadResultXml(cssName) {
    var jsonData = $(cssName).find('input[type="hidden"]').val();
    return jsonData;
}