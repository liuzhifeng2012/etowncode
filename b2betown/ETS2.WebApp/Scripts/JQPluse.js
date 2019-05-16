(function ($) {
    $.extend({
        metadata: {
            defaults: {
                type: 'class',
                name: 'metadata',
                cre: /({.*})/,
                single: 'metadata'
            },
            setType: function (type, name) {
                this.defaults.type = type;
                this.defaults.name = name;
            },
            get: function (elem, opts) {
                var settings = $.extend({}, this.defaults, opts);
                // check for empty string in single property
                if (!settings.single.length) settings.single = 'metadata';

                var data = $.data(elem, settings.single);
                // returned cached data if it already exists
                if (data) return data;

                data = "{}";

                var getData = function (data) {
                    if (typeof data != "string") return data;

                    if (data.indexOf('{') < 0) {
                        data = eval("(" + data + ")");
                    }
                }

                var getObject = function (data) {
                    if (typeof data != "string") return data;

                    data = eval("(" + data + ")");
                    return data;
                }

                if (settings.type == "html5") {
                    var object = {};
                    $(elem.attributes).each(function () {
                        var name = this.nodeName;
                        if (name.match(/^data-/)) name = name.replace(/^data-/, '');
                        else return true;
                        object[name] = getObject(this.nodeValue);
                    });
                } else {
                    if (settings.type == "class") {
                        var m = settings.cre.exec(elem.className);
                        if (m)
                            data = m[1];
                    } else if (settings.type == "elem") {
                        if (!elem.getElementsByTagName) return;
                        var e = elem.getElementsByTagName(settings.name);
                        if (e.length)
                            data = $.trim(e[0].innerHTML);
                    } else if (elem.getAttribute != undefined) {
                        var attr = elem.getAttribute(settings.name);
                        if (attr)
                            data = attr;
                    }
                    object = getObject(data.indexOf("{") < 0 ? "{" + data + "}" : data);
                }

                $.data(elem, settings.single, object);
                return object;
            }
        }
    });

    /**
    * Returns the metadata object for the first member of the jQuery object.
    *
    * @name metadata
    * @descr Returns element's metadata object
    * @param Object opts An object contianing settings to override the defaults
    * @type jQuery
    * @cat Plugins/Metadata
    */
    $.fn.metadata = function (opts) {
        return $.metadata.get(this[0], opts);
    };

})(jQuery);

/*!
* jQuery UI Widget 1.8.16
*/
(function ($, undefined) {
    if ($.cleanData) {
        var _cleanData = $.cleanData;
        $.cleanData = function (elems) {
            for (var i = 0, elem; (elem = elems[i]) != null; i++) {
                try {
                    $(elem).triggerHandler("remove");
                    // http://bugs.jquery.com/ticket/8235
                } catch (e) { }
            }
            _cleanData(elems);
        };
    } else {
        var _remove = $.fn.remove;
        $.fn.remove = function (selector, keepData) {
            return this.each(function () {
                if (!keepData) {
                    if (!selector || $.filter(selector, [this]).length) {
                        $("*", this).add([this]).each(function () {
                            try {
                                $(this).triggerHandler("remove");
                                // http://bugs.jquery.com/ticket/8235
                            } catch (e) { }
                        });
                    }
                }
                return _remove.call($(this), selector, keepData);
            });
        };
    }

    $.widget = function (name, base, prototype) {
        var namespace = name.split(".")[0],
		fullName;
        name = name.split(".")[1];
        fullName = namespace + "-" + name;

        if (!prototype) {
            prototype = base;
            base = $.Widget;
        }

        // create selector for plugin
        $.expr[":"][fullName] = function (elem) {
            return !!$.data(elem, name);
        };

        $[namespace] = $[namespace] || {};
        $[namespace][name] = function (options, element) {
            // allow instantiation without initializing for simple inheritance
            if (arguments.length) {
                this._createWidget(options, element);
            }
        };

        var basePrototype = new base();
        basePrototype.options = $.extend(true, {}, basePrototype.options);
        $[namespace][name].prototype = $.extend(true, basePrototype, {
            namespace: namespace,
            widgetName: name,
            widgetEventPrefix: $[namespace][name].prototype.widgetEventPrefix || name,
            widgetBaseClass: fullName
        }, prototype);

        $.widget.bridge(name, $[namespace][name]);
    };

    $.widget.bridge = function (name, object) {
        $.fn[name] = function (options) {
            var isMethodCall = typeof options === "string",
			args = Array.prototype.slice.call(arguments, 1),
			returnValue = this;

            // allow multiple hashes to be passed on init
            options = !isMethodCall && args.length ?
			$.extend.apply(null, [true, options].concat(args)) :
			options;

            // prevent calls to internal methods
            if (isMethodCall && options.charAt(0) === "_") {
                return returnValue;
            }

            if (isMethodCall) {
                this.each(function () {
                    var instance = $.data(this, name),
					methodValue = instance && $.isFunction(instance[options]) ?
						instance[options].apply(instance, args) :
						instance;
                    // TODO: add this back in 1.9 and use $.error() (see #5972)
                    //				if ( !instance ) {
                    //					throw "cannot call methods on " + name + " prior to initialization; " +
                    //						"attempted to call method '" + options + "'";
                    //				}
                    //				if ( !$.isFunction( instance[options] ) ) {
                    //					throw "no such method '" + options + "' for " + name + " widget instance";
                    //				}
                    //				var methodValue = instance[ options ].apply( instance, args );
                    if (methodValue !== instance && methodValue !== undefined) {
                        returnValue = methodValue;
                        return false;
                    }
                });
            } else {
                this.each(function () {
                    var instance = $.data(this, name);
                    if (instance) {
                        instance.option(options || {})._init();
                    } else {
                        $.data(this, name, new object(options, this));
                    }
                });
            }

            return returnValue;
        };
    };

    $.Widget = function (options, element) {
        // allow instantiation without initializing for simple inheritance
        if (arguments.length) {
            this._createWidget(options, element);
        }
    };

    $.Widget.prototype = {
        widgetName: "widget",
        widgetEventPrefix: "",
        options: {
            disabled: false
        },
        _createWidget: function (options, element) {
            // $.widget.bridge stores the plugin instance, but we do it anyway
            // so that it's stored even before the _create function runs
            $.data(element, this.widgetName, this);
            this.element = $(element);
            this.options = $.extend(true, {},
			this.options,
			this._getCreateOptions(),
			options);

            var self = this;
            this.element.bind("remove." + this.widgetName, function () {
                self.destroy();
            });

            this._create();
            this._trigger("create");
            this._init();
        },
        _getCreateOptions: function () {
            return $.metadata && $.metadata.get(this.element[0])[this.widgetName];
        },
        _create: function () { },
        _init: function () { },

        destroy: function () {
            this.element
			.unbind("." + this.widgetName)
			.removeData(this.widgetName);
            this.widget()
			.unbind("." + this.widgetName)
			.removeAttr("aria-disabled")
			.removeClass(
				this.widgetBaseClass + "-disabled " +
				"ui-state-disabled");
        },

        widget: function () {
            return this.element;
        },

        option: function (key, value) {
            var options = key;

            if (arguments.length === 0) {
                // don't return a reference to the internal hash
                return $.extend({}, this.options);
            }

            if (typeof key === "string") {
                if (value === undefined) {
                    return this.options[key];
                }
                options = {};
                options[key] = value;
            }

            this._setOptions(options);

            return this;
        },
        _setOptions: function (options) {
            var self = this;
            $.each(options, function (key, value) {
                self._setOption(key, value);
            });

            return this;
        },
        _setOption: function (key, value) {
            this.options[key] = value;

            if (key === "disabled") {
                this.widget()
				[value ? "addClass" : "removeClass"](
					this.widgetBaseClass + "-disabled" + " " +
					"ui-state-disabled")
				.attr("aria-disabled", value);
            }

            return this;
        },

        enable: function () {
            return this._setOption("disabled", false);
        },
        disable: function () {
            return this._setOption("disabled", true);
        },

        _trigger: function (type, event, data) {
            var callback = this.options[type];

            event = $.Event(event);
            event.type = (type === this.widgetEventPrefix ?
			type :
			this.widgetEventPrefix + type).toLowerCase();
            data = data || {};

            // copy original event properties over to the new event
            // this would happen if we could call $.event.fix instead of $.Event
            // but we don't have a way to force an event to be fixed multiple times
            if (event.originalEvent) {
                for (var i = $.event.props.length, prop; i; ) {
                    prop = $.event.props[--i];
                    event[prop] = event.originalEvent[prop];
                }
            }

            this.element.trigger(event, data);

            return !($.isFunction(callback) &&
			callback.call(this.element[0], event, data) === false ||
			event.isDefaultPrevented());
        }
    };

})(jQuery);



(function (d, c) {
    d.extend({
        format: function (g, b) {
            g = String(g);
            if (b) {
                if ("[object Object]" == Object.prototype.toString.call(b)) {
                    return g.replace(/#\{(.+?)\}/g, function (f, e) {
                        var j = b[e];
                        if ("function" == typeof j) {
                            j = j(e)
                        }
                        return ("undefined" == typeof j ? "" : j)
                    })
                } else {
                    var a = Array.prototype.slice.call(arguments, 1),
                        h = a.length;
                    return g.replace(/#\{(\d+)\}/g, function (e, f) {
                        f = parseInt(f, 10);
                        return (f >= h ? e : a[f])
                    })
                }
            }
            return g
        },
        filterFormat: function (h, g) {
            var b = Array.prototype.slice.call(arguments, 1),
                a = Object.prototype.toString;
            if (b.length) {
                b = b.length == 1 ? (g !== null && (/\[object Array\]|\[object Object\]/.test(a.call(g))) ? g : b) : b;
                return h.replace(/#\{(.+?)\}/g, function (i, e) {
                    var p, o, f, q, r;
                    if (!b) {
                        return ""
                    }
                    p = e.split("|");
                    o = b[p[0]];
                    if ("[object Function]" == a.call(o)) {
                        o = o(p[0])
                    }
                    for (f = 1, q = p.length; f < q; ++f) {
                        r = d.filterFormat[p[f]];
                        if ("[object Function]" == a.call(r)) {
                            o = r(o)
                        }
                    }
                    return (("undefined" == typeof o || o === null) ? "" : o)
                })
            }
            return h
        }
    });
    d.extend(d.filterFormat, {
        escape: function (a) {
            if (!a || "string" != typeof a) {
                return a
            }
            return a.replace(/"/g, "&#34;").replace(/'/g, "&#39;").replace(/</g, "&#60;").replace(/>/g, "&#62;").replace(/\\/g, "&#92;").replace(/\//g, "&#47;").replace(/`/g, "&#96;")
        },
        nl2br: function (a) {
            if (!a || "string" != typeof a) {
                return a
            }
            return a.replace(/\n/g, "<br />")
        },
        strip: function (a) {
            if (!a || "string" != typeof a) {
                return a
            }
            return a.replace(/ /g, "&nbsp;")
        }
    })
} )(jQuery);
jQuery.cookie = function (x, o, p) {
    if (typeof o != "undefined") {
        p = p || {};
        if (o === null) {
            o = "";
            p.expires = -1
        }
        var t = "";
        if (p.expires && (typeof p.expires == "number" || p.expires.toUTCString)) {
            var w;
            if (typeof p.expires == "number") {
                w = new Date();
                w.setTime(w.getTime() + (p.expires * 24 * 60 * 60 * 1000))
            } else {
                w = p.expires
            }
            t = "; expires=" + w.toUTCString()
        }
        var i = p.path ? "; path=" + (p.path) : "";
        var v = p.domain ? "; domain=" + (p.domain) : "";
        var q = p.secure ? "; secure" : "";
        document.cookie = [x, "=", encodeURIComponent(o), t, i, v, q].join("")
    } else {
        var r = null;
        if (document.cookie && document.cookie != "") {
            var n = document.cookie.split(";");
            for (var u = 0; u < n.length; u++) {
                var s = jQuery.trim(n[u]);
                if (s.substring(0, x.length + 1) == (x + "=")) {
                    r = decodeURIComponent(s.substring(x.length + 1));
                    break
                }
            }
        }
        return r
    }
};
(function (b) {
    b.fn.bgIframe = b.fn.bgiframe = function (f) {
        if (b.browser.msie && /6.0/.test(navigator.userAgent)) {
            f = b.extend({
                top: "auto",
                left: "auto",
                width: "auto",
                height: "auto",
                opacity: true,
                src: "javascript:false;"
            }, f || {});
            var e = function (c) {
                return c && c.constructor == Number ? c + "px" : c
            },
                a = '<iframe class="bgiframe"frameborder="0"tabindex="-1"src="' + f.src + '"style="display:block;position:absolute;z-index:-1;' + (f.opacity !== false ? "filter:Alpha(Opacity='0');" : "") + "top:" + (f.top == "auto" ? "expression(((parseInt(this.parentNode.currentStyle.borderTopWidth)||0)*-1)+'px')" : e(f.top)) + ";left:" + (f.left == "auto" ? "expression(((parseInt(this.parentNode.currentStyle.borderLeftWidth)||0)*-1)+'px')" : e(f.left)) + ";width:" + (f.width == "auto" ? "expression(this.parentNode.offsetWidth+'px')" : e(f.width)) + ";height:" + (f.height == "auto" ? "expression(this.parentNode.offsetHeight+'px')" : e(f.height)) + ';"/>';
            return this.each(function () {
                if (b("> iframe.bgiframe", this).length == 0) {
                    this.insertBefore(document.createElement(a), this.firstChild)
                }
            })
        }
        return this
    }
})(jQuery);

(function (a) {
    var r = a.fn.domManip,
        d = "_tmplitem",
        q = /^[^<]*(<[\w\W]+>)[^>]*$|\{\{\! /,
        b = {},
        f = {},
        e, p = {
            key: 0,
            data: {}
        },
        i = 0,
        c = 0,
        l = [];

    function g(g, d, h, e) {
        var c = {
            data: e || (e === 0 || e === false) ? e : d ? d.data : {},
            _wrap: d ? d._wrap : null,
            tmpl: null,
            parent: d || null,
            nodes: [],
            calls: u,
            nest: w,
            wrap: x,
            html: v,
            update: t
        };
        g && a.extend(c, g, {
            nodes: [],
            parent: d
        });
        if (h) {
            c.tmpl = h;
            c._ctnt = c._ctnt || c.tmpl(a, c);
            c.key = ++i;
            (l.length ? f : b)[i] = c
        }
        return c
    }
    a.each({
        appendTo: "append",
        prependTo: "prepend",
        insertBefore: "before",
        insertAfter: "after",
        replaceAll: "replaceWith"
    }, function (f, d) {
        a.fn[f] = function (n) {
            var g = [],
                i = a(n),
                k, h, m, l, j = this.length === 1 && this[0].parentNode;
            e = b || {};
            if (j && j.nodeType === 11 && j.childNodes.length === 1 && i.length === 1) {
                i[d](this[0]);
                g = this
            } else {
                for (h = 0, m = i.length; h < m; h++) {
                    c = h;
                    k = (h > 0 ? this.clone(true) : this).get();
                    a(i[h])[d](k);
                    g = g.concat(k)
                }
                c = 0;
                g = this.pushStack(g, f, i.selector)
            }
            l = e;
            e = null;
            a.tmpl.complete(l);
            return g
        }
    });
    a.fn.extend({
        tmpl: function (d, c, b) {
            return a.tmpl(this[0], d, c, b)
        },
        tmplItem: function () {
            return a.tmplItem(this[0])
        },
        template: function (b) {
            return a.template(b, this[0])
        },
        domManip: function (d, m, k) {
            if (d[0] && a.isArray(d[0])) {
                var g = a.makeArray(arguments),
                    h = d[0],
                    j = h.length,
                    i = 0,
                    f;
                while (i < j && !(f = a.data(h[i++], "tmplItem")));
                if (f && c) g[2] = function (b) {
                    a.tmpl.afterManip(this, b, k)
                };
                r.apply(this, g)
            } else r.apply(this, arguments);
            c = 0;
            !e && a.tmpl.complete(b);
            return this
        }
    });
    a.extend({
        tmpl: function (d, h, e, c) {
            var i, k = !c;
            if (k) {
                c = p;
                d = a.template[d] || a.template(null, d);
                f = {}
            } else if (!d) {
                d = c.tmpl;
                b[c.key] = c;
                c.nodes = [];
                c.wrapped && n(c, c.wrapped);
                return a(j(c, null, c.tmpl(a, c)))
            }
            if (!d) return [];
            if (typeof h === "function") h = h.call(c || {});
            e && e.wrapped && n(e, e.wrapped);
            i = a.isArray(h) ? a.map(h, function (a) {
                return a ? g(e, c, d, a) : null
            }) : [g(e, c, d, h)];
            return k ? a(j(c, null, i)) : i
        },
        tmplItem: function (b) {
            var c;
            if (b instanceof a) b = b[0];
            while (b && b.nodeType === 1 && !(c = a.data(b, "tmplItem")) && (b = b.parentNode));
            return c || p
        },
        template: function (c, b) {
            if (b) {
                if (typeof b === "string") b = o(b);
                else if (b instanceof a) b = b[0] || {};
                if (b.nodeType) b = a.data(b, "tmpl") || a.data(b, "tmpl", o(b.innerHTML));
                return typeof c === "string" ? (a.template[c] = b) : b
            }
            return c ? typeof c !== "string" ? a.template(null, c) : a.template[c] || a.template(null, q.test(c) ? c : a(c)) : null
        },
        encode: function (a) {
            return ("" + a).split("<").join("&lt;").split(">").join("&gt;").split('"').join("&#34;").split("'").join("&#39;")
        }
    });
    a.extend(a.tmpl, {
        tag: {
            tmpl: {
                _default: {
                    $2: "null"
                },
                open: "if($notnull_1){__=__.concat($item.nest($1,$2));}"
            },
            wrap: {
                _default: {
                    $2: "null"
                },
                open: "$item.calls(__,$1,$2);__=[];",
                close: "call=$item.calls();__=call._.concat($item.wrap(call,__));"
            },
            each: {
                _default: {
                    $2: "$index, $value"
                },
                open: "if($notnull_1){$.each($1a,function($2){with(this){",
                close: "}});}"
            },
            "if": {
                open: "if(($notnull_1) && $1a){",
                close: "}"
            },
            "else": {
                _default: {
                    $1: "true"
                },
                open: "}else if(($notnull_1) && $1a){"
            },
            html: {
                open: "if($notnull_1){__.push($1a);}"
            },
            "=": {
                _default: {
                    $1: "$data"
                },
                open: "if($notnull_1){__.push($.encode($1a));}"
            },
            "!": {
                open: ""
            }
        },
        complete: function () {
            b = {}
        },
        afterManip: function (f, b, d) {
            var e = b.nodeType === 11 ? a.makeArray(b.childNodes) : b.nodeType === 1 ? [b] : [];
            d.call(f, b);
            m(e);
            c++
        }
    });

    function j(e, g, f) {
        var b, c = f ? a.map(f, function (a) {
            return typeof a === "string" ? e.key ? a.replace(/(<\w+)(?=[\s>])(?![^>]*_tmplitem)([^>]*)/g, "$1 " + d + '="' + e.key + '" $2') : a : j(a, e, a._ctnt)
        }) : e;
        if (g) return c;
        c = c.join("");
        c.replace(/^\s*([^<\s][^<]*)?(<[\w\W]+>)([^>]*[^>\s])?\s*$/, function (f, c, e, d) {
            b = a(e).get();
            m(b);
            if (c) b = k(c).concat(b);
            if (d) b = b.concat(k(d))
        });
        return b ? b : k(c)
    }
    function k(c) {
        var b = document.createElement("div");
        b.innerHTML = c;
        return a.makeArray(b.childNodes)
    }
    function o(b) {
        return new Function("jQuery", "$item", "var $=jQuery,call,__=[],$data=$item.data;with($data){__.push('" + a.trim(b).replace(/([\\'])/g, "\\$1").replace(/[\r\t\n]/g, " ").replace(/\$\{([^\}]*)\}/g, "{{= $1}}").replace(/\{\{(\/?)(\w+|.)(?:\(((?:[^\}]|\}(?!\}))*?)?\))?(?:\s+(.*?)?)?(\(((?:[^\}]|\}(?!\}))*?)\))?\s*\}\}/g, function (m, l, k, g, b, c, d) {
            var j = a.tmpl.tag[k],
                i, e, f;
            if (!j) throw "Unknown template tag: " + k;
            i = j._default || [];
            if (c && !/\w$/.test(b)) {
                b += c;
                c = ""
            }
            if (b) {
                b = h(b);
                d = d ? "," + h(d) + ")" : c ? ")" : "";
                e = c ? b.indexOf(".") > -1 ? b + h(c) : "(" + b + ").call($item" + d : b;
                f = c ? e : "(typeof(" + b + ")==='function'?(" + b + ").call($item):(" + b + "))"
            } else f = e = i.$1 || "null";
            g = h(g);
            return "');" + j[l ? "close" : "open"].split("$notnull_1").join(b ? "typeof(" + b + ")!=='undefined' && (" + b + ")!=null" : "true").split("$1a").join(f).split("$1").join(e).split("$2").join(g || i.$2 || "") + "__.push('"
        }) + "');}return __;")
    }
    function n(c, b) {
        c._wrap = j(c, true, a.isArray(b) ? b : [q.test(b) ? b : a(b).html()]).join("")
    }
    function h(a) {
        return a ? a.replace(/\\'/g, "'").replace(/\\\\/g, "\\") : null
    }
    function s(b) {
        var a = document.createElement("div");
        a.appendChild(b.cloneNode(true));
        return a.innerHTML
    }
    function m(o) {
        var n = "_" + c,
            k, j, l = {},
            e, p, h;
        for (e = 0, p = o.length; e < p; e++) {
            if ((k = o[e]).nodeType !== 1) continue;
            j = k.getElementsByTagName("*");
            for (h = j.length - 1; h >= 0; h--) m(j[h]);
            m(k)
        }
        function m(j) {
            var p, h = j,
                k, e, m;
            if (m = j.getAttribute(d)) {
                while (h.parentNode && (h = h.parentNode).nodeType === 1 && !(p = h.getAttribute(d)));
                if (p !== m) {
                    h = h.parentNode ? h.nodeType === 11 ? 0 : h.getAttribute(d) || 0 : 0;
                    if (!(e = b[m])) {
                        e = f[m];
                        e = g(e, b[h] || f[h]);
                        e.key = ++i;
                        b[i] = e
                    }
                    c && o(m)
                }
                j.removeAttribute(d)
            } else if (c && (e = a.data(j, "tmplItem"))) {
                o(e.key);
                b[e.key] = e;
                h = a.data(j.parentNode, "tmplItem");
                h = h ? h.key : 0
            }
            if (e) {
                k = e;
                while (k && k.key != h) {
                    k.nodes.push(j);
                    k = k.parent
                }
                delete e._ctnt;
                delete e._wrap;
                a.data(j, "tmplItem", e)
            }
            function o(a) {
                a = a + n;
                e = l[a] = l[a] || g(e, b[e.parent.key + n] || e.parent)
            }
        }
    }
    function u(a, d, c, b) {
        if (!a) return l.pop();
        l.push({
            _: a,
            tmpl: d,
            item: this,
            data: c,
            options: b
        })
    }
    function w(d, c, b) {
        return a.tmpl(a.template(d), c, b, this)
    }
    function x(b, d) {
        var c = b.options || {};
        c.wrapped = d;
        return a.tmpl(a.template(b.tmpl), b.data, c, b.item)
    }
    function v(d, c) {
        var b = this._wrap;
        return a.map(a(a.isArray(b) ? b.join("") : b).filter(d || "*"), function (a) {
            return c ? a.innerText || a.textContent : a.outerHTML || s(a)
        })
    }
    function t() {
        var b = this.nodes;
        a.tmpl(null, null, null, this).insertBefore(b[0]);
        a(b).remove()
    }
})(jQuery);


/**
 * jquery.timer.js
 *
 * Copyright (c) 2011 Jason Chavannes <jason.chavannes@gmail.com>
 *
 * http://jchavannes.com/jquery-timer
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use, copy,
 * modify, merge, publish, distribute, sublicense, and/or sell copies
 * of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
 * BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
 * ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

(function($) {
        $.timer = function(func, time, autostart) {     
                this.set = function(func, time, autostart) {
                        this.init = true;
                        if(typeof func == 'object') {
                                var paramList = ['autostart', 'time'];
                                for(var arg in paramList) {if(func[paramList[arg]] != undefined) {eval(paramList[arg] + " = func[paramList[arg]]");}};
                                func = func.action;
                        }
                        if(typeof func == 'function') {this.action = func;}
                        if(!isNaN(time)) {this.intervalTime = time;}
                        if(autostart && !this.active) {
                                this.active = true;
                                this.setTimer();
                        }
                        return this;
                };
                this.once = function(time) {
                        var timer = this;
                        if(isNaN(time)) {time = 0;}
                        window.setTimeout(function() {timer.action();}, time);
                        return this;
                };
                this.play = function(reset) {
                        if(!this.active) {
                                if(reset) {this.setTimer();}
                                else {this.setTimer(this.remaining);}
                                this.active = true;
                        }
                        return this;
                };
                this.pause = function() {
                        if(this.active) {
                                this.active = false;
                                this.remaining -= new Date() - this.last;
                                this.clearTimer();
                        }
                        return this;
                };
                this.stop = function() {
                        this.active = false;
                        this.remaining = this.intervalTime;
                        this.clearTimer();
                        return this;
                };
                this.toggle = function(reset) {
                        if(this.active) {this.pause();}
                        else if(reset) {this.play(true);}
                        else {this.play();}
                        return this;
                };
                this.reset = function() {
                        this.active = false;
                        this.play(true);
                        return this;
                };
                this.clearTimer = function() {
                        window.clearTimeout(this.timeoutObject);
                };
                this.setTimer = function(time) {
                        var timer = this;
                        if(typeof this.action != 'function') {return;}
                        if(isNaN(time)) {time = this.intervalTime;}
                        this.remaining = time;
                        this.last = new Date();
                        this.clearTimer();
                        this.timeoutObject = window.setTimeout(function() {timer.go();}, time);
                };
                this.go = function() {
                        if(this.active) {
                                this.action();
                                this.setTimer();
                        }
                };
                
                if(this.init) {
                        return new $.timer(func, time, autostart);
                } else {
                        this.set(func, time, autostart);
                        return this;
                }
        };
})(jQuery);