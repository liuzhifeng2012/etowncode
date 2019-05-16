/**
* 播放声音
* @param path
*/
var retUrl;
var preFlag = false;
var mp3isexist = false;
function audioLoad(checkAudioPath, return_url) {
    retUrl = return_url;
    //首先判断mp3文件是否存在
    var ajax = InitAjax();
    ajax.open("GET", checkAudioPath, true);
    //获取执行状态
    ajax.onreadystatechange = function () {
        //如果执行是状态正常，那么就把返回的内容赋值给上面指定的层
        if (ajax.readyState == 4 && ajax.status == 200) {
            var data = JSON.parse(ajax.responseText);
            if (data.status == 0) {
                //查询mp3文件失败
                preFlag = true;
                return;
            } else {
                mp3isexist = true;
            }
        }
    }
    //发送空
    ajax.send(null);
}

/***
* 生成audio，加载声音
* @audioId 声音文件id
*/
var t;
var handler;
var eventHandler;
var loadContent;
var showDivFlag = false;
function audioInit(checkAudioPath, getAudioPath, imgUrl, cardType, dCode, dCodeImgUrl) {
    if (mp3isexist) {
        if (loadContent != null) {
            $("soundLoad").innerHTML = loadContent;
        }
        if (!showDivFlag) {
            //显示凭证加载图片
            showPop("soundLoad");
        }
        //每次清除audio标签
        removeHtml("audioId");

        //获取ios加载时间
        var deviceIsIOS = /iP(ad|hone|od)/.test(navigator.userAgent);
        var isWp = navigator.userAgent.toLocaleLowerCase().indexOf("windows phone") > 0;

        var audioPath = getAudioPath;
        audio = document.createElement("audio");
        audio.id = "audioId";
        audio.src = audioPath;

        //handler定义为全局变量
        handler = function () {
            audio.play();
        };
        //eventHandler定义为全局变量
        eventHandler = function () {
            audio.play();
        };
        //ios播放完成和暂停事件监听
        if (deviceIsIOS || isWp) {
            audio.addEventListener('ended', handler, false);
            audio.addEventListener('pause', eventHandler, false);
        }
        var eventName;
        //windows phone 处理
        if (isWp) {
            eventName = "play";
        } else {
            eventName = "canplaythrough";
        }
        //可以播放，加载完毕target.attachEvent("on" + eventName, eventHander);
        audio.addEventListener(eventName, function () {
            loadContent = $("soundLoad").innerHTML;
            var url = imgUrl;
            if (cardType == 1) {
                var strCheck;
                if (dCode == 1) {
                    //先停止播放
                    removeAudio("audioId");
                    //采用真实二维码验证
                    strCheck = '<div class="header"><a href="javascript:cancelUseCard();" id="checkCancelBtn" class="btn_small btn_black">取消</a>扫描验证</div><div class="pop_up_box_cont"><div class="code_proving"><span id="provingLine"></span><img src="' + dCodeImgUrl + '" alt="验证"></div><p><a href="javascript:qrCheck();" id="checkCodeBtn" class="btn_big btn_green">确定</a></p></div>';
                } else {
                    strCheck = '<div class="header"><a href="javascript:cancelUseCard();" id="checkCancelBtn" class="btn_small btn_black" >取消</a>扫描验证<a href="javascript:checkCodeBtn();" id="checkCodeBtn" class="btn_small btn_green">验证码</a></div><div class="pop_up_box_cont"><div class="code_proving"><span id="provingLine" class="code_proving_line"></span><img src="' + url + '" alt="验证"></div><p>请使用微护照<span class="text_green"> 电子印章</span>，然后输入验证码。</p></div>';
                }
                divDisplay(strCheck);
            } else {
                var strFast
                if (dCode == 1) {
                    //先停止播放
                    removeAudio("audioId");
                    //采用真实二维码验证
                    strFast = '<div class="header"><a href="javascript:cancelUseCard();" id="checkCancelBtn" class="btn_small btn_black">取消</a>扫描验证</div><div class="pop_up_box_cont"><div class="code_proving"><span id="provingLine"></span><img src="' + dCodeImgUrl + '" alt="验证"></div><p><a href="javascript:qrCheck();" id="checkCodeBtn" class="btn_big btn_green">确定</a></p></div>';
                } else {
                    strFast = '<div class="header"><a href="javascript:cancelUseCard();" id="checkCancelBtn" class="btn_small btn_black">取消</a>扫描验证</div><div class="pop_up_box_cont"><div class="code_proving"><span id="provingLine" class="code_proving_line"></span><img src="' + url + '" alt="验证"></div><p>请使用微护照<span class="text_green"> 电子印章</span>验证。</p><p><a href="javascript:checkCodeBtn();" id="checkCodeBtn" class="btn_big btn_green">通过验证</a></p></div>';
                }
                divDisplay(strFast);
            }
        }, false);
        audio.play();

        //第三种处理方式
        if (!deviceIsIOS) {
            t = window.setInterval("onTimer()", 200);
        }
        //append 到页面
        $("audioDiv").appendChild(audio);
    } else {
        if (preFlag == true) {
            alert("该凭证不支持网页版验证");
        } else {
            return;
        }
    }
    showDivFlag = false;
}
//用于动态改变soundLoad div 中的内容
var oldDivInnerContent;
function divDisplay(innerContent) {
    oldDivInnerContent = $("soundLoad").innerHTML;
    $("soundLoad").innerHTML = innerContent;
}
//返回按钮调用
function divDisplayGoBack() {
    $("soundLoad").innerHTML = oldDivInnerContent;
}
//设置定时器
function onTimer() {
    au = document.getElementById("audioId");
    if (au.currentTime + 1 > 1) {
        au.play();
        au.currentTime = 0;
    }
}
//移除定时器
function clearBtn() {
    if (t != null) {
        window.clearInterval(t);
    }
}
/***
* 在点击取消或验证码按钮时移除audio
* @param id
*/
function removeAudio(id) {
    var oAudio = document.getElementById(id);
    if (oAudio != null) {
        //暂停
        oAudio.pause();
        //移除监听事件
        oAudio.removeEventListener('ended', handler, false);
        //移除监听事件
        oAudio.removeEventListener('pause', eventHandler, false);
    }
    //移除定时器
    clearBtn();
}
/***
* 取消card使用按钮事件
*/
function cancelUseCard() {
    //先停止播放
    removeAudio("audioId");
    //隐藏扫描图片
    hidePop("soundLoad");
}
/***
* 验证印章发出的声音数字
*/
function checkCode(cardType, backArrow) {
    //先停止播放
    removeAudio("audioId");
    var str;
    if (cardType == '1') {
        str = '<div class="header"><a id="checkBackBtn" href="javascript:checkBackBtn();" class="btn_small btn_black">返回</a>验证码<a id="checkBtn" href="javascript:checkBtn();" class="btn_small btn_green">确定</a></div><div class="pop_up_box_cont"><p class="input_group"><span id="checkCode" class="keyboard_display"></span></p>'
               + '<ul class="keyboard_02" id="numberUl"><li>1</li><li>2</li><li>3</li><li>4</li><li>5</li>'
               + '<li>6</li><li>7</li><li>8</li><li>9</li><li>.</li><li>0</li>'
               + '<li><img src=' + backArrow + ' alt="删除" width="38" height="25"></li></ul></div>';
    } else {
        str = '<div class="header"><a href="javascript:checkBackBtn();" id="checkBackBtn" class="btn_small btn_black">返回</a>确认使用<a href="javascript:checkBtn();"  id="checkBtn" class="btn_small btn_green">确认</a></div><div class="pop_up_box_cont"><p class="tips_important">确认使用吗？<br>确认后本凭证<span class="text_orange">即视作已经使用</span>,<br>不可再次使用！</p></div>';
    }
    //显示验证后的内容
    divDisplay(str);
    regisKeyBoard();
}
//注册键盘事件
function regisKeyBoard() {
    //注册键盘事件
    var deviceIsIOS = /iP(ad|hone|od)/.test(navigator.userAgent);
    var deviceIsAndroid = navigator.userAgent.toLocaleLowerCase().indexOf("android") > 0;
    var deviceIsWp = navigator.userAgent.toLocaleLowerCase().indexOf("windows phone") > 0;
    if (deviceIsIOS || deviceIsAndroid || deviceIsWp) {
        new NoClickDelay($('numberUl'));
    } else {
        keyBoardClickEvent();
    }
}
/**
* 取消验证按钮事件
*/
function checkBack(checkAudioPath, getAudioPath, imgUrl, cardType, dCode, dCodeImgUrl) {
    //标志
    showDivFlag = true;
    //返回按钮事件
    divDisplayGoBack();
    //返回到生成声音
    audioInit(checkAudioPath, getAudioPath, imgUrl, cardType, dCode, dCodeImgUrl);
}

/**
* 动态二维码直接验证通过
*/
function qrCheck() {
    //刷新当前页面
    window.location.reload();
}

/**
* 验证并显示验证结果
* @id 声音文件id
*/
function checkResult(checkPath, passId, cardType, imgUrl) {
    //根据卡片类型来
    var data = "";
    if (cardType == 1) {
        data = $("checkCode").innerText;
    } else {
        data = "";
    }
    //在线直接到服务器验证
    if (navigator.onLine == true) {
        checkPath = checkPath + "?checkCode=" + data + "&passId=" + passId + "&cardType=" + cardType;
        var ajax = InitAjax();
        ajax.open("GET", checkPath, true);
        //获取执行状态
        ajax.onreadystatechange = function () {
            //如果执行是状态正常，那么就把返回的内容赋值给上面指定的层
            if (ajax.readyState == 4 && ajax.status == 200) {
                var data = JSON.parse(ajax.responseText);
                if (data.status == 1) {
                    var str = '<div class="header">验证成功<a href=javascript:checkRetPageBack("soundLoad","success"); class="btn_small btn_green">确定</a></div>'
                            + '<div class="pop_up_box_cont"><div class="code_proving"><span class="proving_success"></span><img src=' + imgUrl + ' alt="验证"></div><p id="succTip"></p></div>';
                    //显示成功提示信息
                    //showPop("checkSuccDiv");
                    divDisplay(str);
                    //对累计奖励盖满后赠送提示
                    if (data.info == "dialog") {
                        $("succTip").innerText = data.data;
                    }
                } else {
                    var str = '<div class="header">验证失败<a href=javascript:checkRetPageBack("soundLoad","fail"); class="btn_small btn_green">确定</a></div><div class="pop_up_box_cont"><p>验证失败，请重新验证。</p><p id="failInfo"></p></div>';
                    //显示失败提示信息
                    //showPop("checkFailDiv");
                    divDisplay(str);
                    $("failInfo").innerText = "失败：" + data.info;
                }
            }
        }
        //发送空
        ajax.send(null);
    } else {
        //离线？？
    }
}
/***
* 验证结果页面的取消按钮事件
* @id 结果div id
*/
function checkRetPageBack(id, flag) {
    if (flag == "success") {
        if (retUrl != null && retUrl != '') {
            window.location.href = retUrl;
        } else {
            //刷新当前页面
            window.location.reload();
        }
    } else if (flag == "fail") {
        hidePop(id);
    }
}

/**
* 获取卡片扩展信息
*/
function getCardExternalInfo(getExternalInfoUrl, pDiv) {
    var ajax = InitAjax();
    ajax.open("GET", getExternalInfoUrl, true);
    //获取执行状态
    ajax.onreadystatechange = function () {
        //如果执行是状态正常，那么就把返回的内容赋值给上面指定的层
        if (ajax.readyState == 4 && ajax.status == 200) {
            var data = ajax.responseText;
            $(pDiv).innerHTML = data;
            //初始化
            subsInfo();
        }
    }
    //发送空
    ajax.send(null);
}

/**
* ***************************************************************************
* 清空input中的验证码
* ***************************************************************************
*/
var keyString = "";
function backSpaceEvent(btnId) {
    //$("#"+btnId).attr("value","");
    document.getElementById(btnId).innerText = "";
    keyString = "";
}

/**
* 获取key 值 键盘事件，点击键盘数字获取该键盘值，并填入input中
* @param id
*/
function getKeysVal(id) {
    var div = $("checkCode");
    if (id == 'img') {
        div.innerText = '';
    }
    else {
        var keyString = div.innerText;
        div.innerText = keyString + id;
    }
}

function getKeysVal2(e) {
    var div = $("checkCode");
    if (this.id == 'img') {
        div.innerText = '';
    }
    else {
        var keyString = div.innerText;
        div.innerText = keyString + this.id;
    }
}

function AttachEvent(target, eventName, handler, argsObject) {
    var eventHandler = handler; //拼写错误eventHander
    if (argsObject) {
        eventHander = function (e) {
            handler.call(argsObject, e); //改成handler.call(this, argsObject);
        }
    }
    target.addEventListener(eventName, eventHander, false);
    //target.attachEvent("on" + eventName, eventHander);
}

function NoClickDelay(el) {
    this.element = typeof el == 'object' ? el : document.getElementById(el);
    var isWp = navigator.userAgent.toLocaleLowerCase().indexOf("windows phone") > 0;
    if (isWp) {
        if (window.navigator.msPointerEnabled) {
            this.element.addEventListener("MSPointerDown", this, false);
        } else {
            //绑定click事件
            keyBoardClickEvent();
        }
    } else {
        this.element.addEventListener('touchstart', this, false);
    }
}
NoClickDelay.prototype = {
    handleEvent: function (e) {
        switch (e.type) {
            case 'touchstart':
                this.onTouchStart(e);
                break;
            case 'MSPointerDown':
                this.onMSPointerDown(e);
                break;
        }
    },
    onTouchStart: function (e) {
        e.preventDefault(); this.moved = false;
        this.theTarget = document.elementFromPoint(e.targetTouches[0].clientX, e.targetTouches[0].clientY);
        if (this.theTarget.nodeType == 3) this.theTarget = theTarget.parentNode;
        var text = this.theTarget.innerText == '' ? 'img' : this.theTarget.innerText;
        getKeysVal(text);
    },
    onMSPointerDown: function (e) {
        e.preventDefault();
        this.theTarget = document.elementFromPoint(e.clientX, e.clientY);
        if (this.theTarget.nodeType == 3) this.theTarget = theTarget.parentNode;
        var text = this.theTarget.innerText == '' ? 'img' : this.theTarget.innerText;
        getKeysVal(text);
    }
};

//onclick 事件
function keyBoardClickEvent() {
    if ($('numberUl')) {
        var li = $('numberUl').getElementsByTagName("li");
        for (var i = 0; i < li.length; i++) {
            var liNode = li[i];
            var text = liNode.innerText == '' ? 'img' : liNode.innerText;
            var obj = new Object();
            obj.id = text;
            AttachEvent(liNode, "click", getKeysVal2, obj);
        }
    }
}
//弹出框
var oldContent;
function showPop(a) {
    var _screenWidth = window.screen.width;
    var _isoFlag = /iP(ad|hone|od)/.test(navigator.userAgent);
    //320 的ios和非详情，日志div增加样式
    if (_isoFlag && _screenWidth <= 320) {
        iospBoxStyle(a);
    }
    if (oldContent != null) {
        if (oldContent == "soundLoad") {
            //先停止播放
            removeAudio("audioId");
        }
        if (_screenWidth > 320 || _isoFlag) {
            $(oldContent).style.bottom = "-320px";
        }
        $(oldContent).style.display = "none";
    }
    $(a).style.display = "block";
    setTimeout(function () {
        if (_screenWidth > 320 || _isoFlag) {
            $(a).style.bottom = "0px";
        } else {
            $(a).style.bottom = "auto";
        }
    }, 100);
    oldContent = a;
}

function hidePop(a) {
    var _screenWidth = window.screen.width;
    var _isoFlag = /iP(ad|hone|od)/.test(navigator.userAgent);
    if (_screenWidth > 320 || _isoFlag) {
        $(a).style.bottom = "-320px";
    }
    setTimeout(function () { $(a).style.display = "none"; }, 500);
}
//针对320的ios做预处理
function iospBoxStyle(id) {
    var _st = document.getElementById(id);
    _st.style.position = "fixed";
    _st.style.zIndex = "99999";
    _st.style.left = "0";
    _st.style.bottom = "-320px";
    _st.style.height = "270px";
}

//分店信息
function subsInfo() {
    //如果分店数大于1，则商家信息不显示
    var subsNum = parseInt($("subsNum").value);
    if (subsNum > 0) {
        //隐藏商家信息
        $("merchantInfo").style.display = "none";
        //在子商家列表中显示当前商家信息,通过隐藏域获取当前商家信息的id
        var id = $("thisMerchId").value;
        $(id).style.display = "block";
    } else {
        $("subsSec").style.display = "none";
    }
}

//显示商家详情subsInfoLi
var oldLiId;
var oldLiContent;
function showSubsInfo(hideId, merchantName, merchantAdd, merchantTel, x, y, logoId) {

    //var logoUrl = "#url('account/getLogo/"+logoId+"/60/20')"
    var _telHtml = "<a href='tel:" + merchantTel + "'>" + merchantTel + "</a>";
    var _sHtml = '<section class="business_info"><dl><dt><img id="imgId" src=' + logoId + '  alt="">'
        + '<p class="name" id="subsName">' + merchantName + '</p>'
        + '<p class="add" id="subsAdd"></p>' + merchantAdd + '</dt>'
        + '<dd><i class="ico_tel"></i><span id="subsTel">' + _telHtml + '</span></dd></dl></section>';

    if (oldLiId == hideId) {
        $(hideId).innerHTML = _sHtml;
        return;
    }
    var tempStr = $(hideId).innerHTML

    //将原来的值设置回去
    if (oldLiId) {
        //oldLiId="_oldLiId";
        if (oldLiId != hideId) {
            $(oldLiId).innerHTML = oldLiContent;
        }
    }
    //设置当前li内的值
    $(hideId).innerHTML = _sHtml;

    oldLiId = hideId;
    oldLiContent = tempStr;
}