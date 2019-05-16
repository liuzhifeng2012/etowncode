/**
* ��������
* @param path
*/
var retUrl;
var preFlag = false;
var mp3isexist = false;
function audioLoad(checkAudioPath, return_url) {
    retUrl = return_url;
    //�����ж�mp3�ļ��Ƿ����
    var ajax = InitAjax();
    ajax.open("GET", checkAudioPath, true);
    //��ȡִ��״̬
    ajax.onreadystatechange = function () {
        //���ִ����״̬��������ô�Ͱѷ��ص����ݸ�ֵ������ָ���Ĳ�
        if (ajax.readyState == 4 && ajax.status == 200) {
            var data = JSON.parse(ajax.responseText);
            if (data.status == 0) {
                //��ѯmp3�ļ�ʧ��
                preFlag = true;
                return;
            } else {
                mp3isexist = true;
            }
        }
    }
    //���Ϳ�
    ajax.send(null);
}

/***
* ����audio����������
* @audioId �����ļ�id
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
            //��ʾƾ֤����ͼƬ
            showPop("soundLoad");
        }
        //ÿ�����audio��ǩ
        removeHtml("audioId");

        //��ȡios����ʱ��
        var deviceIsIOS = /iP(ad|hone|od)/.test(navigator.userAgent);
        var isWp = navigator.userAgent.toLocaleLowerCase().indexOf("windows phone") > 0;

        var audioPath = getAudioPath;
        audio = document.createElement("audio");
        audio.id = "audioId";
        audio.src = audioPath;

        //handler����Ϊȫ�ֱ���
        handler = function () {
            audio.play();
        };
        //eventHandler����Ϊȫ�ֱ���
        eventHandler = function () {
            audio.play();
        };
        //ios������ɺ���ͣ�¼�����
        if (deviceIsIOS || isWp) {
            audio.addEventListener('ended', handler, false);
            audio.addEventListener('pause', eventHandler, false);
        }
        var eventName;
        //windows phone ����
        if (isWp) {
            eventName = "play";
        } else {
            eventName = "canplaythrough";
        }
        //���Բ��ţ��������target.attachEvent("on" + eventName, eventHander);
        audio.addEventListener(eventName, function () {
            loadContent = $("soundLoad").innerHTML;
            var url = imgUrl;
            if (cardType == 1) {
                var strCheck;
                if (dCode == 1) {
                    //��ֹͣ����
                    removeAudio("audioId");
                    //������ʵ��ά����֤
                    strCheck = '<div class="header"><a href="javascript:cancelUseCard();" id="checkCancelBtn" class="btn_small btn_black">ȡ��</a>ɨ����֤</div><div class="pop_up_box_cont"><div class="code_proving"><span id="provingLine"></span><img src="' + dCodeImgUrl + '" alt="��֤"></div><p><a href="javascript:qrCheck();" id="checkCodeBtn" class="btn_big btn_green">ȷ��</a></p></div>';
                } else {
                    strCheck = '<div class="header"><a href="javascript:cancelUseCard();" id="checkCancelBtn" class="btn_small btn_black" >ȡ��</a>ɨ����֤<a href="javascript:checkCodeBtn();" id="checkCodeBtn" class="btn_small btn_green">��֤��</a></div><div class="pop_up_box_cont"><div class="code_proving"><span id="provingLine" class="code_proving_line"></span><img src="' + url + '" alt="��֤"></div><p>��ʹ��΢����<span class="text_green"> ����ӡ��</span>��Ȼ��������֤�롣</p></div>';
                }
                divDisplay(strCheck);
            } else {
                var strFast
                if (dCode == 1) {
                    //��ֹͣ����
                    removeAudio("audioId");
                    //������ʵ��ά����֤
                    strFast = '<div class="header"><a href="javascript:cancelUseCard();" id="checkCancelBtn" class="btn_small btn_black">ȡ��</a>ɨ����֤</div><div class="pop_up_box_cont"><div class="code_proving"><span id="provingLine"></span><img src="' + dCodeImgUrl + '" alt="��֤"></div><p><a href="javascript:qrCheck();" id="checkCodeBtn" class="btn_big btn_green">ȷ��</a></p></div>';
                } else {
                    strFast = '<div class="header"><a href="javascript:cancelUseCard();" id="checkCancelBtn" class="btn_small btn_black">ȡ��</a>ɨ����֤</div><div class="pop_up_box_cont"><div class="code_proving"><span id="provingLine" class="code_proving_line"></span><img src="' + url + '" alt="��֤"></div><p>��ʹ��΢����<span class="text_green"> ����ӡ��</span>��֤��</p><p><a href="javascript:checkCodeBtn();" id="checkCodeBtn" class="btn_big btn_green">ͨ����֤</a></p></div>';
                }
                divDisplay(strFast);
            }
        }, false);
        audio.play();

        //�����ִ���ʽ
        if (!deviceIsIOS) {
            t = window.setInterval("onTimer()", 200);
        }
        //append ��ҳ��
        $("audioDiv").appendChild(audio);
    } else {
        if (preFlag == true) {
            alert("��ƾ֤��֧����ҳ����֤");
        } else {
            return;
        }
    }
    showDivFlag = false;
}
//���ڶ�̬�ı�soundLoad div �е�����
var oldDivInnerContent;
function divDisplay(innerContent) {
    oldDivInnerContent = $("soundLoad").innerHTML;
    $("soundLoad").innerHTML = innerContent;
}
//���ذ�ť����
function divDisplayGoBack() {
    $("soundLoad").innerHTML = oldDivInnerContent;
}
//���ö�ʱ��
function onTimer() {
    au = document.getElementById("audioId");
    if (au.currentTime + 1 > 1) {
        au.play();
        au.currentTime = 0;
    }
}
//�Ƴ���ʱ��
function clearBtn() {
    if (t != null) {
        window.clearInterval(t);
    }
}
/***
* �ڵ��ȡ������֤�밴ťʱ�Ƴ�audio
* @param id
*/
function removeAudio(id) {
    var oAudio = document.getElementById(id);
    if (oAudio != null) {
        //��ͣ
        oAudio.pause();
        //�Ƴ������¼�
        oAudio.removeEventListener('ended', handler, false);
        //�Ƴ������¼�
        oAudio.removeEventListener('pause', eventHandler, false);
    }
    //�Ƴ���ʱ��
    clearBtn();
}
/***
* ȡ��cardʹ�ð�ť�¼�
*/
function cancelUseCard() {
    //��ֹͣ����
    removeAudio("audioId");
    //����ɨ��ͼƬ
    hidePop("soundLoad");
}
/***
* ��֤ӡ�·�������������
*/
function checkCode(cardType, backArrow) {
    //��ֹͣ����
    removeAudio("audioId");
    var str;
    if (cardType == '1') {
        str = '<div class="header"><a id="checkBackBtn" href="javascript:checkBackBtn();" class="btn_small btn_black">����</a>��֤��<a id="checkBtn" href="javascript:checkBtn();" class="btn_small btn_green">ȷ��</a></div><div class="pop_up_box_cont"><p class="input_group"><span id="checkCode" class="keyboard_display"></span></p>'
               + '<ul class="keyboard_02" id="numberUl"><li>1</li><li>2</li><li>3</li><li>4</li><li>5</li>'
               + '<li>6</li><li>7</li><li>8</li><li>9</li><li>.</li><li>0</li>'
               + '<li><img src=' + backArrow + ' alt="ɾ��" width="38" height="25"></li></ul></div>';
    } else {
        str = '<div class="header"><a href="javascript:checkBackBtn();" id="checkBackBtn" class="btn_small btn_black">����</a>ȷ��ʹ��<a href="javascript:checkBtn();"  id="checkBtn" class="btn_small btn_green">ȷ��</a></div><div class="pop_up_box_cont"><p class="tips_important">ȷ��ʹ����<br>ȷ�Ϻ�ƾ֤<span class="text_orange">�������Ѿ�ʹ��</span>,<br>�����ٴ�ʹ�ã�</p></div>';
    }
    //��ʾ��֤�������
    divDisplay(str);
    regisKeyBoard();
}
//ע������¼�
function regisKeyBoard() {
    //ע������¼�
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
* ȡ����֤��ť�¼�
*/
function checkBack(checkAudioPath, getAudioPath, imgUrl, cardType, dCode, dCodeImgUrl) {
    //��־
    showDivFlag = true;
    //���ذ�ť�¼�
    divDisplayGoBack();
    //���ص���������
    audioInit(checkAudioPath, getAudioPath, imgUrl, cardType, dCode, dCodeImgUrl);
}

/**
* ��̬��ά��ֱ����֤ͨ��
*/
function qrCheck() {
    //ˢ�µ�ǰҳ��
    window.location.reload();
}

/**
* ��֤����ʾ��֤���
* @id �����ļ�id
*/
function checkResult(checkPath, passId, cardType, imgUrl) {
    //���ݿ�Ƭ������
    var data = "";
    if (cardType == 1) {
        data = $("checkCode").innerText;
    } else {
        data = "";
    }
    //����ֱ�ӵ���������֤
    if (navigator.onLine == true) {
        checkPath = checkPath + "?checkCode=" + data + "&passId=" + passId + "&cardType=" + cardType;
        var ajax = InitAjax();
        ajax.open("GET", checkPath, true);
        //��ȡִ��״̬
        ajax.onreadystatechange = function () {
            //���ִ����״̬��������ô�Ͱѷ��ص����ݸ�ֵ������ָ���Ĳ�
            if (ajax.readyState == 4 && ajax.status == 200) {
                var data = JSON.parse(ajax.responseText);
                if (data.status == 1) {
                    var str = '<div class="header">��֤�ɹ�<a href=javascript:checkRetPageBack("soundLoad","success"); class="btn_small btn_green">ȷ��</a></div>'
                            + '<div class="pop_up_box_cont"><div class="code_proving"><span class="proving_success"></span><img src=' + imgUrl + ' alt="��֤"></div><p id="succTip"></p></div>';
                    //��ʾ�ɹ���ʾ��Ϣ
                    //showPop("checkSuccDiv");
                    divDisplay(str);
                    //���ۼƽ���������������ʾ
                    if (data.info == "dialog") {
                        $("succTip").innerText = data.data;
                    }
                } else {
                    var str = '<div class="header">��֤ʧ��<a href=javascript:checkRetPageBack("soundLoad","fail"); class="btn_small btn_green">ȷ��</a></div><div class="pop_up_box_cont"><p>��֤ʧ�ܣ���������֤��</p><p id="failInfo"></p></div>';
                    //��ʾʧ����ʾ��Ϣ
                    //showPop("checkFailDiv");
                    divDisplay(str);
                    $("failInfo").innerText = "ʧ�ܣ�" + data.info;
                }
            }
        }
        //���Ϳ�
        ajax.send(null);
    } else {
        //���ߣ���
    }
}
/***
* ��֤���ҳ���ȡ����ť�¼�
* @id ���div id
*/
function checkRetPageBack(id, flag) {
    if (flag == "success") {
        if (retUrl != null && retUrl != '') {
            window.location.href = retUrl;
        } else {
            //ˢ�µ�ǰҳ��
            window.location.reload();
        }
    } else if (flag == "fail") {
        hidePop(id);
    }
}

/**
* ��ȡ��Ƭ��չ��Ϣ
*/
function getCardExternalInfo(getExternalInfoUrl, pDiv) {
    var ajax = InitAjax();
    ajax.open("GET", getExternalInfoUrl, true);
    //��ȡִ��״̬
    ajax.onreadystatechange = function () {
        //���ִ����״̬��������ô�Ͱѷ��ص����ݸ�ֵ������ָ���Ĳ�
        if (ajax.readyState == 4 && ajax.status == 200) {
            var data = ajax.responseText;
            $(pDiv).innerHTML = data;
            //��ʼ��
            subsInfo();
        }
    }
    //���Ϳ�
    ajax.send(null);
}

/**
* ***************************************************************************
* ���input�е���֤��
* ***************************************************************************
*/
var keyString = "";
function backSpaceEvent(btnId) {
    //$("#"+btnId).attr("value","");
    document.getElementById(btnId).innerText = "";
    keyString = "";
}

/**
* ��ȡkey ֵ �����¼�������������ֻ�ȡ�ü���ֵ��������input��
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
    var eventHandler = handler; //ƴд����eventHander
    if (argsObject) {
        eventHander = function (e) {
            handler.call(argsObject, e); //�ĳ�handler.call(this, argsObject);
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
            //��click�¼�
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

//onclick �¼�
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
//������
var oldContent;
function showPop(a) {
    var _screenWidth = window.screen.width;
    var _isoFlag = /iP(ad|hone|od)/.test(navigator.userAgent);
    //320 ��ios�ͷ����飬��־div������ʽ
    if (_isoFlag && _screenWidth <= 320) {
        iospBoxStyle(a);
    }
    if (oldContent != null) {
        if (oldContent == "soundLoad") {
            //��ֹͣ����
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
//���320��ios��Ԥ����
function iospBoxStyle(id) {
    var _st = document.getElementById(id);
    _st.style.position = "fixed";
    _st.style.zIndex = "99999";
    _st.style.left = "0";
    _st.style.bottom = "-320px";
    _st.style.height = "270px";
}

//�ֵ���Ϣ
function subsInfo() {
    //����ֵ�������1�����̼���Ϣ����ʾ
    var subsNum = parseInt($("subsNum").value);
    if (subsNum > 0) {
        //�����̼���Ϣ
        $("merchantInfo").style.display = "none";
        //�����̼��б�����ʾ��ǰ�̼���Ϣ,ͨ���������ȡ��ǰ�̼���Ϣ��id
        var id = $("thisMerchId").value;
        $(id).style.display = "block";
    } else {
        $("subsSec").style.display = "none";
    }
}

//��ʾ�̼�����subsInfoLi
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

    //��ԭ����ֵ���û�ȥ
    if (oldLiId) {
        //oldLiId="_oldLiId";
        if (oldLiId != hideId) {
            $(oldLiId).innerHTML = oldLiContent;
        }
    }
    //���õ�ǰli�ڵ�ֵ
    $(hideId).innerHTML = _sHtml;

    oldLiId = hideId;
    oldLiContent = tempStr;
}