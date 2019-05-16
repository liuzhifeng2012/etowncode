//抖动效果
function shake(o) {
    var $panel = $("." + o);
    box_left = ($(window).width() - $panel.width()) / 2;
    $panel.css({ 'left': box_left, 'position': 'absolute' });
    for (var i = 1; 4 >= i; i++) {
        $panel.animate({ left: box_left - (40 - 10 * i) }, 50);
        $panel.animate({ left: box_left + 2 * (40 - 10 * i) }, 50);
    }
}

(function ($) {
    $.fn.hoverDelay = function (options) {
        var defaults = {
            hoverDuring: 200,
            outDuring: 200,
            hoverEvent: function () {
                $.noop();
            },
            outEvent: function () {
                $.noop();
            }
        };
        var sets = $.extend(defaults, options || {});
        var hoverTimer, outTimer;
        return $(this).each(function () {
            $(this).hover(function () {
                clearTimeout(outTimer);
                hoverTimer = setTimeout(sets.hoverEvent, sets.hoverDuring);
            }, function () {
                clearTimeout(hoverTimer);
                outTimer = setTimeout(sets.outEvent, sets.outDuring);
            });
        });
    }
})(jQuery);