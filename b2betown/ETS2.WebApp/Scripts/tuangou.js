$(function () {
    var userid = $("#hid_userid").trimVal();
    var comid = $("#hid_comid").trimVal();

    var isload = true; //是否可以继续加载未上架产品
    var checked_value = ""; //选中的售卖分销id
    var checked_text = ""; //选中的售卖分销名称
    var notstockpro_pageindex = 1; //未上架产品列表页码

    $.fn.popup = function (operationType) {
        if (operationType == "show") {
            $(".kout").css("display", "block");
            $(this).css("display", "block");
            $("html").css({ "width": "100%", "height": "100%", "overflow-y": "hidden" });
            $("body").css({ "width": "100%", "height": "100%", "overflow-y": "hidden" });
        }
        else {
            $(".kout").css("display", "none");
            $(this).css("display", "none");
            $("html").css({ "width": "100%", "height": "100%", "overflow-y": "scroll" });
            $("body").css({ "width": "100%", "height": "100%", "overflow-y": "scroll" });
        }
    }

    $("#addbtn").on("click", function () {
        $(".mtk").popup("show");
        //获取商户授权的美团分销(并且需要美团分销中含有美团接口信息)
        $.post("/JsonFactory/AgentHandler.ashx?oper=getmeituanagentcompanylist", { comid: $("#hid_comid").trimVal() }, function (data) {
            data = eval("(" + data + ")");
            $(".section1").find("section-body").empty();
            if (data.type == 1) {
                layer.msg("加载美团分销失败");
                return;
            }
            if (data.type == 100) {
                if (data.msg.lengh == 0) {
                    layer.msg("美团分销未授权或者美团参数未设置");
                    return;
                }
                else {
                    var str = "";
                    for (var i = 0; i < data.msg.length; i++) {
                        str += '<div class="user_list_item">' +
                                  '<div class="radio">' +
                                        '<label>' +
                                          '<input type="radio" name="radagent"  value="' + data.msg[i].Id + '" ><span>' + data.msg[i].Company + '</span>' +
                                        '</label>' +
                                  '</div>' +
                             '</div>';
                    }
                    $(".section1").find("section-body").append(str);
                }
            }
        })

        $(".section1").find("section-body").show();
        $(".section2").find("section-body").hide();
    })



    $(".section-header1").on("click", function () {

        if ($(this).next().css("display") == "none") {
            $(this).next().show();
            $(".section2").find("section-body").hide();
        }
        else {
            $(this).next().hide();
        }
    })
    $(".section-header2").on("click", function () {

        //        var checked_value = "";
        //        var checked_text = "";
        $("input[name='radagent']:checked").each(function () {
            checked_value = $(this).val();
            checked_text = $(this).next().text();
        })
        if (checked_value == "") {
            layer.msg("请选择项后在操作!");
            return;
        }
        else {
            $(".section2").find("section-body").empty();
            notstockpro_pageindex = 1;
            GetNotStockProList();
        }

        if ($(this).next().css("display") == "none") {
            $(this).next().show();
            $(".section1").find("section-body").hide();
        }
        else {
            $(this).next().hide();
        }
    })

    //未上架产品下拉加载更多
    $(".modal-body").on("scroll", function () {
        if (!isload) {
            return;
        }

        var scrollTop = $(this).scrollTop(); //滚动高度
        var windowHeight = $(this).height(); //可视高度
        var documentHeight = $(this)[0].scrollHeight; //实际高度

        if (windowHeight >= documentHeight) {
            return;
        }

        if (scrollTop >= documentHeight - windowHeight) {
            notstockpro_pageindex++;
            GetNotStockProList();
        }
    })

    function GetNotStockProList() {

        //获取 选中的美团分销 还未上架的产品
        $.post("/JsonFactory/ProductHandler.ashx?oper=getNotStockProPagelist", { pageindex: notstockpro_pageindex, pagesize: 10, comid: $("#hid_comid").trimVal(), agentid: checked_value }, function (data) {
            data = eval("(" + data + ")");
            if (data.type == 1) {
                layer.msg("加载未上架产品失败");
                isload = false;
                return;
            }
            if (data.type == 100) {
                if (data.msg.length == 0) {
                    isload = false;
                }
                else {
                    isload = true;
                    var str = "";
                    for (var i = 0; i < data.msg.length; i++) {
                        str += '<div class="user_list_item">' +
                                  '<div class="checkbox">' +
                                    '<label>' +
                                      '<input type="checkbox" value="' + data.msg[i].Id + '" name="chkpro"><span>' + data.msg[i].Proname + '</span>' +
                                    '</label>' +
                                  '</div>' +
                              '</div>';
                    }
                    $(".section2").find("section-body").append(str);
                }
            }
        })

    }

    //关闭上架弹出层
    $(".closePopup").on("click", function () {
        $(".mtk").popup("hide");
        window.location.reload();
    });

    //提交上架产品
    $(".submitobjbtn").on("click", function () {
        var checked_valuearr = new Array();
        var checked_textarr = new Array();
        $("input[name='chkpro']:checked").each(function () {

            checked_valuearr.push($(this).val());
            checked_textarr.push($(this).next().text());
        })
        if (checked_valuearr.length == 0) {
            layer.msg("请选择项后在操作!");
            return;
        }
        else {
            var checkvaluestr = "";
            checkvaluestr = checked_valuearr.join(",");

            var checktextstr = "";
            checktextstr = checked_textarr.join(",");

//            layer.msg('商品上架中，请稍候..', {
//                icon: 16,
//                shade: 0.01,
//                time: 0
//            });

            $.post("/JsonFactory/ProductHandler.ashx?oper=stockPro", { proidstr: checkvaluestr, pronamestr: checktextstr, isstock: 1, groupbuytype: 1, operuserid: userid, comid: comid, stockagentcompanyid: checked_value, stockagentcompanyname: checked_text }, function (data) {
                data = eval("(" + data + ")");

//                var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
//                parent.layer.close(index);

                if (data.type == 1) {
                    layer.msg(data.msg);

                    //关闭上架产品弹出层
                    $(".mtk").popup("hide");
                    return;
                }
                if(data.type==100) {
                    layer.msg("上架产品成功");
                    //关闭上架产品弹出层
                    $(".mtk").popup("hide"); 

                    window.location.reload();
                    return;
                }
            })
             

        }
    })

    //回车进行搜索操作
    $("html").die().live("keydown", function (event) {
        if (event.keyCode == 13) {
            $("#searchbtn").click();    //这里添加要处理的逻辑  
            return false;
        }
    });

    //搜索按钮
    $("#searchbtn").on("click", function () {
        var key = $("#keytxt").trimVal();
        if (key == "") {
            layer.msg("请输入关键字！");
        }
        SearchList(1);
    })

    SearchList(1);

    //装载团购产品上架列表
    function SearchList(pageindex) {
        var key = $("#keytxt").trimVal();
        var stockstate = "1";
        var groupbuytype = 1;

        if (pageindex == '' || pageindex == 0) {
            $.prompt("请选择跳到的页数");
            return;
        }

        $.post("/JsonFactory/ProductHandler.ashx?oper=groupbuystocklogpagelist", { groupbuytype: 1, pageindex: pageindex, pagesize: 10, key: key, stockstate: stockstate, comid: comid }, function (data) {
            data = eval("(" + data + ")");

            $("thbody").empty();
            $("#divPage").empty();

            if (data.type == 1) {
                layer.msg("加载数据失败");
                return;
            }
            if (data.type == 100) {

                if (data.totalcount == 0) {
                    layer.msg("查询数据为空");
                } else {
                    $("#ProductItemEdit").tmpl(data.msg).appendTo("thbody");
                    setpage(data.totalcount, 10, pageindex);
                }
            }
        })
    }

    //分页
    function setpage(totalcount, pagesize, curpage) {
        $("#divPage").paginate({
            count: Math.ceil(totalcount / pagesize),
            start: curpage,
            display: 5,
            border: false,
            text_color: '#888',
            background_color: '#EEE',
            text_hover_color: 'black',
            images: false,
            rotate: false,
            mouse: 'press',
            onChange: function (page) {

                SearchList(page);

                return false;
            }
        });
    }


})
function downstockpro(obj, proid, agentcompanyid) {
  if(confirm("确认下架吗？"))
  {
      $.post("/JsonFactory/ProductHandler.ashx?oper=downstockpro", { proid: proid, agentcompanyid: agentcompanyid }, function (data) {
          data = eval("(" + data + ")");
          if (data.type == 1) {
              layer.msg(data.msg);
              return;
          }
          else {
              layer.msg(data.msg);
              $(obj).hide();
              $(obj).next().text("已经下架");
              return;
          }
      })
  }
  
}