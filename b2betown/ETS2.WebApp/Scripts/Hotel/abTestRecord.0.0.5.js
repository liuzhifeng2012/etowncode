if (typeof String.prototype.trim != "function") {
    String.prototype.trim = function () {
        return this.replace(/^\s+/g, "").replace(/\s+$/g, "");
    }
}
function loadScript(url) {
    var snippet = document.createElement("script");
    snippet.type = "text/javascript";
    snippet.async = true;
    snippet.src = url;
    var snippets = document.getElementsByTagName("script");
    if (snippets != null) {
        snippets[0].parentNode.insertBefore(snippet, snippets[0]);
    }
    else {
        document.getElementsByTagName("head").appendChild(snippet);
    }
}
(function () {
    // var testIds = ["0"];				// 针对的是那些测试
    var cookies = document.cookie.split(";");
    var tests = [];
    for (var i = 0; i < cookies.length; i++) {
        for (var j = 0; j < testIds.length; j++) {
            var cookieKV = cookies[i].split("=");
            if (cookieKV[0].trim() === "ABTest_" + testIds[j]) {			// cookie的命名规则前缀ABTest加id值
                // cookie值的格式是versionId#pageType#visitorId，没有参加时versionId的值为notJoin
                var values = cookieKV[1].split("#");
                if (values[0] !== "notJoin") {
                    loadScript("http://vstlog.17usoft.com/abtest/ABMeasureHandler.ashx?ToPage=" + encodeURIComponent('[{"ABMeasureId":' + testIds[j] + ',"URLId":' + values[0] + ',"UserId":' + values[2] + '}]'));
                    break;
                }
            }
        }
    }
})();