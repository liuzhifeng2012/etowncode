<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WxZixunReply.aspx.cs" Inherits="ETS2.WebApp.WeiXin.WxZixunReplay" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>微客服 </title>
    <meta charset="utf-8" />
    <meta name="description" />
    <meta name="keywords" />
    <meta content="application/xhtml+xml;charset=UTF-8" http-equiv="Content-Type" />
    <meta content="no-cache,must-revalidate" http-equiv="Cache-Control" />
    <meta content="no-cache" http-equiv="pragma" />
    <meta content="0" http-equiv="expires" />
    <meta content="telephone=no, address=no" name="format-detection" />
    <meta content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no"
        name="viewport" />
    <link rel="stylesheet" type="text/css" href="/styles/weixinzixunreply/mobilemain.css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <meta name="viewport" content="width=device-width, initial-scale=1, user-scalable=0">
    <link rel="stylesheet" href="http://demo.open.weixin.qq.com/jssdk/css/style.css?ts=1420774989">
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"> </script>
    <script>
        wx.config({
            debug: false,
            appId: '<%=appId %>',
            timestamp: '<%=timestamp %>',
            nonceStr: '<%=nonceStr %>',
            signature: '<%=signature %>',
            jsApiList: [
        'checkJsApi',
        'onMenuShareTimeline',
        'onMenuShareAppMessage',
        'onMenuShareQQ',
        'onMenuShareWeibo',
        'hideMenuItems',
        'showMenuItems',
        'hideAllNonBaseMenuItem',
        'showAllNonBaseMenuItem',
        'translateVoice',
        'startRecord',
        'stopRecord',
        'onRecordEnd',
        'playVoice',
        'pauseVoice',
        'stopVoice',
        'uploadVoice',
        'downloadVoice',
        'chooseImage',
        'previewImage',
        'uploadImage',
        'downloadImage',
        'getNetworkType',
        'openLocation',
        'getLocation',
        'hideOptionMenu',
        'showOptionMenu',
        'closeWindow',
        'scanQRCode',
        'chooseWXPay',
        'openProductSpecificView',
        'addCard',
        'chooseCard',
        'openCard'
      ]
        }); 
    </script>
    <script src="http://demo.open.weixin.qq.com/jssdk/js/api-6.1.js?ts=1420774989"> </script>
    <script type="text/javascript">
        wx.error(function (res) {
            alert(res.errMsg);
        });
    </script>
    <style type="text/css">
        .btn
        {
            display: inline-block;
            background-color: #ccc;
            border: 1px solid #E5E5E5;
            border-radius: 2px;
            padding: 4px;
            text-align: center;
            margin: 0px auto;
            color: #999;
            font-size: 12px;
            cursor: pointer;
            line-height: 22px;
        }
        .luyin
        {
            color: #FFF;
            border-color: #F15A0C;
            background-color: #F15A0C;
        }
        .zhunbeiluyin
        {
            background-color: #2792FF;
            border-color: #2792FF;
            color: #ccc;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var pageSize = 20;

            SearchList(1, pageSize, "");

            function SearchList(pageindex, pagesize, key) {
                //获取多客服列表
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=Getzixunlog", { pageindex: pageindex, pagesize: pagesize, comid: $("#hid_comid").val(), userweixin: '<%=userweixin %>', guwenweixin: '<%=guwenweixin %>', key: key }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                    }
                    if (data.type == 100) {
                        stop = true;
                        if (data.totalCount == 0) {
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#containertop");
                        }

                    }
                })
            }


            var stop = true;
            $(window).scroll(function () {

                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
                if ($(document).height() <= 0) {
                    if (stop == true) {
                        var pageindex = parseInt($("#pageindex").val()) + 1;
                        stop = false;
                        SearchList(pageindex, pagesize, "");
                        $("#pageindex").val(pageindex);
                    }
                }
            });


            setInterval("SearchEndList()", 8000);



            $("#facePoint").click(function () {
                $("#face_area").show();
                $("#text_area").hide();
            })

            $("#closeluyin").click(function () {
                $("#face_area").hide();
                $("#text_area").show();
            })


            //发送微信交互信息
            $("#btnsend").click(function () {
                var type = 1;
                var img1 = "";
                var txt1 = $("#sendtext").val();
                $("#sendtext").val("");
                if (type == 1) {
                    if (txt1 == "") {
                        alert("发送内容不能为空，请重新输入！");
                        return;
                    }
                    img1 = "";
                }

                $.post("/JsonFactory/WeiXinHandler.ashx?oper=guwenwxsendmsg", { comid: $("#hid_comid").val(), userweixin: '<%=userweixin %>', guwenweixin: '<%=guwenweixin %>', type: type, img: img1, txt: txt1 }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 100) {
                        SearchEndList();
                    }
                    if (data.type == 1) {
                        alert(data.msg);
                        return;
                    }
                })
            })

        })

        function SearchEndList() {
            //获取多客服列表
            var endwxzixunid = $("#endwxzixunid").val();

            SearchList1(1, 20, endwxzixunid)
        }
        function SearchList1(pageindex, pagesize, key) {
            //获取多客服列表
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=Getzixunlog", { pageindex: pageindex, pagesize: pagesize, comid: $("#hid_comid").val(), userweixin: '<%=userweixin %>', guwenweixin: '<%=guwenweixin %>', key: key }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                }
                if (data.type == 100) {
                    stop = true;
                    if (data.totalCount == 0) {
                    } else {
                        $("#ProductItemEdit").tmpl(data.msg).appendTo("#containertop");
                    }

                }
            })
        }
         

        function endzixunid(id) {
            if (Number($("#endwxzixunid").val()) < Number(id)) {
                $("#endwxzixunid").val(id);
            }
            return "";

        }

        //        function luyin() {
        //            var luyinstate = $("#luyinstate").val();
        //            if (luyinstate == "1") {
        //                $("#luyin").html("再次点击结束录音");
        //                $("#luyin").addClass("luyin");
        //                $("#luyin").removeClass("zhunbeiluyin");
        //                $("#luyinstate").val(0);
        //                //录音
        //            } else {
        //                $("#luyin").html("点击开始录音");
        //                $("#luyin").removeClass("luyin");
        //                $("#luyin").addClass("zhunbeiluyin");
        //                $("#luyinstate").val(1);
        //                //停止录音
        //            }

        //        }

        //bid 标示id
        function playVoice(guwen, bid, mediaid) {

            $(".yonghuimg").attr("src", "/Images/weixinzixunreply/mobile_voice.png");
            $(".guwenimg").attr("src", "/Images/weixinzixunreply/mobile_replyvoice.png");

            if (guwen == 0) {
                $("#playVoice" + bid).attr("src", "/Images/weixinzixunreply/mobile_voice.gif");
            } else {
                $("#playVoice" + bid).attr("src", "/Images/weixinzixunreply/mobile_replyvoice.gif");
            }
            wx.downloadVoice({
                serverId: mediaid,
                success: function (res) {

                    wx.playVoice({
                        localId: res.localId
                    });  
                }
            });
        }

        wx.ready(function () {
            // 3 智能接口
            var voice = {
                localId: '',
                serverId: ''
            };
            // 4 音频接口

            document.querySelector('#luyin').onclick = function () {

                var luyinstate = $("#luyinstate").val();
                if (luyinstate == "1") { // 4.2 开始录音
                    $("#luyin").html("再次点击结束录音");
                    $("#luyin").addClass("luyin");
                    $("#luyin").removeClass("zhunbeiluyin");
                    $("#luyinstate").val(0);
                    //开始录音
                    wx.startRecord({
                        success: function () {
//                            alert("开始录音");
                        },
                        cancel: function () {
                            alert('用户拒绝授权录音');
                        },
                        fail: function (res) {
                            alert(res.errMsg);
                        }
                    });
                } else { // 4.3 停止录音
                    $("#luyin").html("点击开始录音");
                    $("#luyin").removeClass("luyin");
                    $("#luyin").addClass("zhunbeiluyin");
                    $("#luyinstate").val(1);
                    //停止录音
                    wx.stopRecord({
                        success: function (res) {
                            voice.localId = res.localId;
                            //                        alert("停止录音");
                            //----上传语音----
                            if (voice.localId == '') {
                                alert('请先录制一段声音');
                                return;
                            }
                            wx.uploadVoice({
                                localId: voice.localId, // 需要上传的音频的本地ID，由stopRecord接口获得
                                isShowProgressTips: 0, // 默认为1，显示进度提示
                                success: function (res) {
                                    //                        alert('上传语音成功，serverId 为' + res.serverId);
                                    voice.serverId = res.serverId;
                                    $.post("/JsonFactory/WeiXinHandler.ashx?oper=guwenwxsendmsg", { comid: $("#hid_comid").val(), userweixin: '<%=userweixin %>', guwenweixin: '<%=guwenweixin %>', type: 3, img: "", txt: "", mediaid:  res.serverId }, function (data) {
                                        data = eval("(" + data + ")");
                                        if (data.type == 100) {
                                            SearchEndList();
                                        }
                                        if (data.type == 1) {
                                            alert(data.msg);
                                            return;
                                        }
                                    })
                                }
                            });
                            //----上传语音----
                        },
                        fail: function (res) {
                            alert(JSON.stringify(res));
                        }
                    });
                }

            };

            // 4.4 监听录音自动停止
            wx.onVoiceRecordEnd({
                complete: function (res) {
                    voice.localId = res.localId;
                    //                    alert('录音时间已超过一分钟，自动停止录音');
                    //----上传语音----
                    if (voice.localId == '') {
                        alert('请先录制一段声音');
                        return;
                    }
                    wx.uploadVoice({
                        localId: voice.localId,
                        isShowProgressTips: 0, // 默认为1，显示进度提示
                        success: function (res) {
                            //                        alert('上传语音成功，serverId 为' + res.serverId);
                            voice.serverId = res.serverId;
                            //微信多媒体接口下载语音到自己的服务器
                            $.post("/JsonFactory/WeiXinHandler.ashx?oper=guwenwxsendmsg", { comid: $("#hid_comid").val(), userweixin: '<%=userweixin %>', guwenweixin: '<%=guwenweixin %>', type: 3, img: "", txt: "", mediaid: res.serverId }, function (data) {
                                data = eval("(" + data + ")");
                                if (data.type == 100) {
                                    alert("录音时间已超过一分钟，自动停止录音");
                                    SearchEndList();
                                }
                                if (data.type == 1) {
                                    alert(data.msg);
                                    return;
                                }
                            })

                        }
                    });
                    //----上传语音----
                }
            });
        })
    </script>
</head>
<body onselectstart="return true;" ondragstart="return false;">
    <div id="container" class="container animate">
        <div class="containertop" id="containertop">
        </div>
        <div name="bottom1">
        </div>
        <footer>
            <section class="nav_footer" id="nav_footer">
                <ul>
                    <ol  id="text_area"  class="tbox">
                       <%if (1 == 0)
                         { %>
                        <li>
                            <a class="pointer toolsvoice" style="-webkit-tap-highlight-color: rgba(0, 0, 0, 0);" id="facePoint"></a>
                        </li>
                        <%}  %>
                         <li>
                            <a class="pointer" style="-webkit-tap-highlight-color: rgba(0, 0, 0, 0);" ></a>
                        </li>

                        <li style="width: 100%;">
                            <input type="text" id="sendtext" class="toolstext" style="width: 100%;" />
                        </li>

                        <li>
                            <a id="btnsend" class="toolssend" style="height: 24px; padding-top: 6px; margin: 0 7px;">发送</a>
                        </li>
                    </ol>

                    <ol id="face_area" style="display:none;" >
                        <li class="page_emotion box_swipe" id="Li2" style="height:10px;" >
                            <dl id="closeluyin" class="list_emotion pt_10">
                                <dd style="width:100%; -webkit-transform: rotate(-90deg);  -moz-transform: rotate(-90deg);  filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=3);  display:block; font-size:30px;" > &lsaquo;</dd>
                            </dl> 
                        </li>
                        <li class="page_emotion box_swipe" id="Li1" style="height: 80px; margin-bottom:15px;" >
                            <dl id="Dl1" class="list_emotion pt_10">
                                <dd style="width:100%; " class="btn zhunbeiluyin" id="luyin"  > 点击开始录音 </dd>
                            </dl> 
                        </li>
                        <!--
                        <li class="page_emotion box_swipe" id="page_smiley" >
                            <dl id="list_smiley" class="list_emotion pt_10">
                                <dd style="width:50%;">开始录制</dd>
                                <dd style="width:50%;">停止录制</dd>
                            </dl> 
                        </li>-->

                    </ol>


                 <%--   <ol>
                        <li class="facetype" id="facetype" style="display:none;"><span data-key="jingdian" class="jingdian on">经典</span><span data-key="xiong" class="xiong">熊大&兔兔</span><span data-key="mayi" class="mayi">黑蚂蚁</span></li>
                    </ol>--%>
                     
                </ul>
            </section>
        </footer>
        <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
            {{if guwen==0}} 
          <%if (isrightwxset==1) { %>
           {{if Mediaid ==""}} <!-- 用户语音信息不显示，只是显示其语音识别文字-->
            <p class="time" style="display: block;">
                ${CreateTime}</p>

            <ul class="ul_talk ">
                <li class="tbox">
                    <div>
                        <span class="head">
                            <img src="${Headimgurl}" />
                        </span>
                    </div>
                    <div>
                        <span class="arrow">
                            
                        </span>
                    </div>
                    <article class="content">
                            ${Content}${endzixunid(Id)}
                          {{if Mediaid !=""}}
                            {{if IsOver2Days==0}}
                                <img id="playVoice${Id}"  src="/Images/weixinzixunreply/mobile_voice.png"  onclick="playVoice('${guwen}','${Id}','${Mediaid}')" class="yonghuimg"/> 
                            {{else}}
                                <img id="playVoice${Id}"  src="/Images/weixinzixunreply/mobile_voice.png" class="yonghuimg"/>语音已超时
                            {{/if}} 
                          {{/if}}
                        </article>
                </li>
            </ul>
            {{/if}}
            <%}
            else{  %><!--如果是不满足播放语音条件的，则只是显示文字咨询内容-->
           {{if Mediaid ==""}}
                 <p class="time" style="display: block;">
                ${CreateTime}</p>

            <ul class="ul_talk ">
                <li class="tbox">
                    <div>
                        <span class="head">
                            <img src="${Headimgurl}" />
                        </span>
                    </div>
                    <div>
                        <span class="arrow">
                            
                        </span>
                    </div>
                    <article class="content">
                            ${Content}${endzixunid(Id)}
                          {{if Mediaid !=""}}
                            {{if IsOver2Days==0}}
                                <img id="playVoice${Id}"  src="/Images/weixinzixunreply/mobile_voice.png"  onclick="playVoice('${guwen}','${Id}','${Mediaid}')" class="yonghuimg"/> 
                            {{else}}
                                <img id="playVoice${Id}"  src="/Images/weixinzixunreply/mobile_voice.png" class="yonghuimg"/>语音已超时
                            {{/if}} 
                          {{/if}}
                        </article>
                </li>
            </ul>
               {{/if}}
            <%} %>
           {{else}}
             <%if (isrightwxset==1) { %>
            <ul class="ul_talk reply">
                <li class="tbox">
                    <div>
                        <span class="head">
                            <img src="${Headimgurl}" />
                        </span>
                    </div>
                    <div>
                        <span class="arrow">

                        </span>
                    </div>
                   <article class="content">
                          ${Content} ${endzixunid(Id)}

                         {{if Mediaid !=""}}
                            {{if IsOver2Days==0}}
                                <img id="playVoice${Id}"  src="/Images/weixinzixunreply/mobile_replyvoice.png"  onclick="playVoice('${guwen}','${Id}','${Mediaid}')" class="guwenimg"/> 
                            {{else}}
                                <img id="playVoice${Id}"  src="/Images/weixinzixunreply/mobile_replyvoice.png" class="guwenimg"/>语音已超时
                            {{/if}} 
                         {{/if}}

                        </article>
                </li>
            </ul>
                <%}
             else{  %><!--如果是不满足播放语音条件的，则只是显示文字咨询内容-->
               {{if Mediaid ==""}}
                <ul class="ul_talk reply">
                <li class="tbox">
                    <div>
                        <span class="head">
                            <img src="${Headimgurl}" />
                        </span>
                    </div>
                    <div>
                        <span class="arrow">

                        </span>
                    </div>
                   <article class="content">
                          ${Content} ${endzixunid(Id)}

                         {{if Mediaid !=""}}
                            {{if IsOver2Days==0}}
                                <img id="playVoice${Id}"  src="/Images/weixinzixunreply/mobile_replyvoice.png"  onclick="playVoice('${guwen}','${Id}','${Mediaid}')" class="guwenimg"/> 
                            {{else}}
                                <img id="playVoice${Id}"  src="/Images/weixinzixunreply/mobile_replyvoice.png" class="guwenimg"/>语音已超时
                            {{/if}} 
                         {{/if}}

                        </article>
                </li>
            </ul>
               {{/if}}
                <%} %>
            {{/if}} 
        </script>
        <input type="hidden" id="hid_comid" value="<%=comid %>" />
        <input type="hidden" id="pageindex" value="1" />
        <input type="hidden" id="endwxzixunid" value="0" />
        <input type="hidden" id="luyinstate" value="1" />
        <input type="hidden" id="pageNumber" value="2" />
        <input type="hidden" id="lastmsgtime" value="" />
    </div>
</body>
</html>
