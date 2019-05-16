//扩展静态方法
$.extend($, {
    //弹出消息
    ppkAlert: function (msg, flag) {
        $.ppkOverlay();
        $("body").append("<div class='ppkdialogCon'><div class='ppkalert'>" + msg + "</div></div>");
        if (!flag) {
            setTimeout(function () {
                $.ppkAlertRemove();
            }, 1500)
        }
    },
    //移除消息
    ppkAlertRemove: function () {
        $(".ppkdialogCon").remove();
        $(".ppkoverlay").remove();
    },
    ppkDialog: function (htm, flag) {
        $.ppkOverlay(flag);
        $("body").append("<div class='ppkdialogCon'>" + htm + "</div>");
    },
    //遮罩层  clickRemove 点击移除
    ppkOverlay: function (clickRemove) {
        $("body").append("<div class='ppkoverlay'></div>");
        if (clickRemove) {
            $(".ppkoverlay").bind("click", function () {
                $.ppkAlertRemove();
            });
        }
    },
    //ppkshare
    ppkShare: function () {
        var _h = $(window).scrollTop();
        var htm = '<div style="top:' + _h + 'px" class="ppkshare"><img src="guide.png" /></div>'
        $.ppkDialog(htm, true);
    },
	ppkShareGuide:function(act,keyword,flag){
		var s ='<p>4、 输入关键词：'+keyword+'　'+act+'</p><div style=" position:relative;"><img width="100%" style="display:block;" src="yd3.jpg"><span style="position:absolute; left:42px; top:8px; color:#333;">'+keyword+'</span></div>';
		if(flag){
			s="";
		}
		$.ppkDialog('<div class="ppkalert" style="top: 8%; background: 4c4c4c; font-size: 12px; line-height: 20px;text-align: left;"><p>1、请点击右上角图标<img src=yd1.png" style="position: absolute;width: 60px; right: 20px; top: -8%;"></p><p>2、在打开的菜单查看/关注公众号</p><p><img width="100%" src="yd2.jpg"></p><p>3、进入该公众帐号，点击关注并查看消息（已关注用户直接查看消息）</p><p><img width="100%" src="yd4.png" /></p>'+s+'</div>', true);
	},
    ppkWeiShare: function (options) {
        $.extend(dataForWeixin, options)
    },
    ppkBack: function () {
        $(".J_backsave").bind("click", function () {
            if (window.confirm("确定不保存离开吗？")) {
                return true;
            }
            return false;
        });
    },
	ppkNoshare:function(){
		document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
			WeixinJSBridge.call('hideOptionMenu');
		});
	},
	ppkNoTools:function(){
		document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
			WeixinJSBridge.call('hideToolbar');
		});
	},
    //获取url参数
    getQueryString: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null)
            return unescape(decodeURIComponent(r[2]));
        else
            return "";
    },
    disabled_A: function (tag) {
        $(tag).unbind("click").attr("href", "javascript:;").addClass("ppk-btn-hui");
    }
})

$.ppkBack();


var dataForWeixin = {
	path: location.href,
	title: document.title,
	desc: "",
	image:""
};
(function(){
   var onBridgeReady=function(){
	   WeixinJSBridge.on('menu:share:appmessage', function (argv) {
		   WeixinJSBridge.invoke('sendAppMessage', {
			   "link": dataForWeixin.path,
			   "desc": dataForWeixin.desc,
			   "title": dataForWeixin.title,
			   "img_url":dataForWeixin.image,
			   "img_width":"120",
			   "img_height":"120"
		   }, function (res) { 
                if(res.err_msg=="send_app_msg:ok")
                {
                    //分享朋友圈成功
                    var recevieId=$("#hidReceiveId").val();
                    if(recevieId!=null && recevieId.length>0)
                    {
                        $.ajax({
                                type: "get",
                                cache: false,
                                url: "/Product/ActivationRewardReceive/"+recevieId,
                                dataType: "json",
                                success: function (data) {
                                    
                                }
                         });
                     }
                }
           });
	   });
	   WeixinJSBridge.on('menu:share:timeline', function(argv){
		  WeixinJSBridge.invoke('shareTimeline',{
			 "link": dataForWeixin.path,
			 "desc":dataForWeixin.desc,
			 "title":dataForWeixin.title,
			  "img_url":dataForWeixin.image,
			   "img_width":"120",
			   "img_height":"120"
		  }, function(res){});
	   });
	   WeixinJSBridge.on('menu:share:weibo', function(argv){
		  WeixinJSBridge.invoke('shareWeibo',{
			 "content":dataForWeixin.title,
			 "url": dataForWeixin.path
		  }, function(res){});
	   });
	};
	if(document.addEventListener){
	   document.addEventListener('WeixinJSBridgeReady', onBridgeReady, false);
	}else if(document.attachEvent){
	   document.attachEvent('WeixinJSBridgeReady'   , onBridgeReady);
	   document.attachEvent('onWeixinJSBridgeReady' , onBridgeReady);
	}
})();

$(function(){
	
	document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
		WeixinJSBridge.call('showOptionMenu');
	});
})