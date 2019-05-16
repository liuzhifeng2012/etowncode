
$(function () {
    $("ul.tab li:first-child").addClass("tab_on");
    //设置字体
    $("ul.tab li:not(ul.tab li:first-child)").css("font-weight", "normal");

    $("div.tab_con:not(div.tab_con:first)").hide();
    $("div.tab_con").attr("id", function () { return "tab_con_" + $("div.tab_con").index(this) });

    $("ul.tab li").hover(function () {
        var c = $("ul.tab li");
        var div_name = "tab_con_" + c.index(this);

        $("div.tab_con").hide();
        $("#" + div_name).show();
        c.eq(c.index(this)).addClass("tab_on").siblings().removeClass("tab_on");
        //设置字体
        c.eq(c.index(this)).css("font-weight", "bold").siblings().css("font-weight", "normal");

    })

    //$("div.rightfont").hide();
    $("input.hid_chanpin").attr("id", function () { return "hid_chanpin_" + $("input.hid_chanpin").index(this) });
    $("div.rightfont").attr("id", function () { return "div_chanpin_" + $("div.rightfont").index(this) });

    $("a.a_chanpin").click(function () {
        var c = $("a.a_chanpin");
        var hid_id = "hid_chanpin_" + c.index(this);
        var div_id = "div_chanpin_" + c.index(this);

        var hid_val = $("#" + hid_id).val();
        if (hid_val == "+") {
            $("#" + hid_id).val("-");
            $("#" + div_id).show();
        } else {
            $("#" + hid_id).val("+");
            $("#" + div_id).hide();
        }
    })

})
 