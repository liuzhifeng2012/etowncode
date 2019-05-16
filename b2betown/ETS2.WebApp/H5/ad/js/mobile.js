var _resources = {
    special: [],
    collection: []
},
_service = {
    addVote: function (a) {
        return $.ajax({
            method: "PUT",
            url: "api/post/vote?id=" + a
        })
    },
    updateVote: function (a) {
        return $.ajax("api/post/vote?ids=" + a.join(","))
    },
    wxConfig: function (a) {
        return $.ajax("/wechat/api/jssdk-params?url=" + encodeURIComponent(window.location.href)).done(function (b, c, d) {
            a(b)
        }).fail(function (b, c, d) {
            a(null)
        })
    }
},
STORE_KEY = "vote-ts",
VOTE_LIMIT = 864e5,
_tools = {
    saveKey: function (a) {
        var c, b = window.localStorage;
        if (!a) return !1;
        try {
            return c = JSON.parse(b.getItem(STORE_KEY)),
            c || (c = {}),
            c[a] = +new Date,
            b.setItem(STORE_KEY, JSON.stringify(c)),
            !0
        } catch (d) {
            return !1
        }
    },
    getKey: function (a) {
        var c, b = window.localStorage;
        try {
            if (c = b.getItem(STORE_KEY)) return c = JSON.parse(c),
            c[a]
        } catch (d) {
            return ""
        }
    },
    _kv: {},
    _ids: function () {
        function b(b) {
            b.id && (_tools._kv[b.id] = b, a.push(b.id))
        }
        var a = [];
        return _.each(_resources.special,
        function (a) {
            _.each(a, b)
        }),
        _.each(_resources.collection,
        function (a) {
            _.each(a.cards, b)
        }),
        _tools._ids = a,
        a
    },
    updateVote: function (a, b) {
        var c = null;
        return a = a || _tools._ids,
        _service.updateVote(a).always(function (a) {
            _.isObject(a) && "success" === a.status && a.data && (a.time && (_tools._serverTime = a.time), _.each(a.data,
            function (a) {
                $('[data-cid="' + a._id + '"] .vote-count').text(a.vote)
            }), c = a.data),
            _.isFunction(b) && b(c)
        })
    },
    shouldVote: function (a) {
        var b = _tools.getKey(a),
        c = new Date;
        return b ? c - b > VOTE_LIMIT ? (_tools.saveKey(a), !0) : !1 : (_tools.saveKey(a), !0)
    },
    qr: function () {
        var a = Math.random() < .7 ? "./images/qr-bc.png" : "./images/qr-ac.png";
        $(".modal-qr").addClass("active").find(".image-qr").attr("src", a)
    },
    re: function () {
        $(".modal-re").addClass("active")
    },
    vote: function (a) {
        a.preventDefault();
        var b = $(a.currentTarget),
        c = b.closest("[data-cid]"),
        d = c.find(".vote-count"),
        e = c.data("cid"),
        f = +d.text() || 1,
        g = f += 1;
        return _tools.shouldVote0(e) ? void (b.hasClass("active") || _service.addVote(e).always(function (a) {
            return _.isObject(a) && a.time && (_tools._serverTime = a.time),
            _.isObject(a) && 429 === a.status ? alert("\u4eca\u65e5\u6295\u7968\u6570\u91cf\u5df2\u8fbe\u4e0a\u9650\uff0c\u8bf7\u660e\u5929\u518d\u6295\uff01") : (d.text(g), b.addClass("active"), void _.delay(function () {
                _tools.qr()
            },
            700))
        })) : _tools.re()
    },
    wxShare: function (a) {
        var b = window.wx,
        c = {
            title: "",
            link: "http://shop.etown.cn/h5/order",
            imgUrl: "http://shop.etown.cn/images/m-share.jpg",
            desc: "",
            type: "",
            dataUrl: "",
            success: function () { },
            cancel: function () { }
        },
        d = _.extend(c, a || {});
        b && /MicroMessenger/i.test(navigator.userAgent) && _service.wxConfig(function (a) {
            a && (b.config(a), b.ready(function () {
                b.onMenuShareTimeline(d),
                b.onMenuShareAppMessage(d)
            }))
        })
    },
    shouldVote0: function (a) {
        var b = _tools.genKey0(a),
        c = window.localStorage;
        try {
            var d = JSON.parse(c.getItem(STORE_KEY)) || {};
            return "1" == d[b] ? !1 : (d[b] = 1, c.setItem(STORE_KEY, JSON.stringify(d)), !0)
        } catch (e) {
            return !0
        }
        return !0
    },
    _serverTime: "",
    genKey0: function (a) {
        var b = _tools._serverTime ? new Date(_tools._serverTime) : new Date,
        c = a + "__" + b.getFullYear() + _tools.left0(b.getMonth() + 1) + _tools.left0(b.getDate());
        return c
    },
    left0: function (a) {
        return 10 > a ? "0" + a : a
    }
};
_tools._ids();
var _query = function () {
    for (var d, a = {},
    b = window.location.search.slice(1), c = /([^&=]+)=([^&]*)/g; d = c.exec(b); ) a[decodeURIComponent(d[1])] = decodeURIComponent(d[2]);
    return a
} (); !
function (a, b, c, d, e, f, g) {
    a.GoogleAnalyticsObject = e,
    a[e] = a[e] ||
    function () {
        (a[e].q = a[e].q || []).push(arguments)
    },
    a[e].l = 1 * new Date,
    f = b.createElement(c),
    g = b.getElementsByTagName(c)[0],
    f.async = 1,
    f.src = d,
    g.parentNode.insertBefore(f, g)
} (window, document, "script", "//www.google-analytics.com/analytics.js", "ga"),
ga("create", "UA-70689708-1", "auto"),
ga("send", "pageview");
var _paq = _paq || [];
_paq.push(["trackPageView"]),
_paq.push(["enableLinkTracking"]),
function () {
    var a = "//shop.etown.cn/";
    _paq.push(["setTrackerUrl", a + ""]),
    _paq.push(["setSiteId", 11]);
    var b = document,
    c = b.createElement("script"),
    d = b.getElementsByTagName("script")[0];
    c.type = "text/javascript",
    c.async = !0,
    c.defer = !0,
    c.src = a + "tracker",
    d.parentNode.insertBefore(c, d)
} ();