<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProDefault.aspx.cs" Inherits="ETS2.WebApp.H5.ProDefault" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <script type="text/javascript">
        var _tcopentime = new Date().getTime();
        var _hmt = _hmt || [];
    </script>
    <title>景点门票</title>
    <!-- meta信息，可维护 --> 
    <meta charset="UTF-8" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta content="telephone=no" name="format-detection" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <meta name="keywords" content="景点门票" />
    <meta name="description" content="" />
    <link rel="stylesheet" type="text/css" href="/Styles/fontcss/font-awesome.min.css" />

    <link rel="stylesheet" type="text/css" href="/Styles/h5/mh5pro.css">
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {

            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#Search").click();    //这里添加要处理的逻辑  
                }
            });


            $("#Search").click(function () {
                var key = $("#SearchName").val();
                if (key != "") {
                    location.href = "http://shop106.etown.cn/h5/List.aspx?key=" + key + "&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>";

                } else {
                    alert("请输入查询的景区");
                }
            })


            $("#confirm").click(function () {
                var key = $("#f-level-val").val();
                var theme = $("#f-theme-val").val();
                var price = $("#f-price-val").val();

                if (key != "") {
                    location.href = "http://shop106.etown.cn/h5/List.aspx?key=" + key + "&class=" + theme + "&price=" + price + "&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>";

                } else {
                    alert("请输入查询的景区");
                }
            })

            $("#morecity").click(function () {
                $("#morelist").toggle();
            })
            $("#viewbeijingmore").click(function () {
                if ($("#beijingmore").is(":hidden") == true) {
                    $("#beijingmore").show();
                } else {
                    $("#beijingmore").hide();
                }

            })

            $("#shaixuan").click(function () {
                $("#firstpage").hide();
                $("#selectTypePage").show();

            })

            $("#f-level").click(function () {
                if ($("#f-level-ul").is(":hidden") == true) {
                    $("#f-level-ul").show();
                } else {
                    $("#f-level-ul").hide();
                }
            })

            $("#f-level-ul li").click(function () {
                $("#f-level-val").val($(this).attr("data-id"));
                $("#f-level-viw").text($(this).attr("data-type-name"));

                $(this).parent.hide();


            })



            $("#f-theme").click(function () {
                if ($("#f-theme-ul").is(":hidden") == true) {
                    $("#f-theme-ul").show();
                } else {
                    $("#f-theme-ul").hide();
                }
            })

            $("#f-theme-ul li").click(function () {
                $("#f-theme-val").val($(this).attr("data-id"));
                $("#f-theme-viw").text($(this).attr("data-type-name"));
                $("#f-theme-ul").parent.hide();
            })

            $("#f-price").click(function () {
                if ($("#f-price-ul").is(":hidden") == true) {
                    $("#f-price-ul").show();
                } else {
                    $("#f-price-ul").hide();
                }
            })

            $("#f-price-ul li").click(function () {
                $("#f-price-val").val($(this).attr("data-id"));
                $("#f-price-viw").text($(this).attr("data-type-name"));
                $(this).parent.hide();
            })




        })
    </script>
    <style type="text/css">
        .none
        {
            display: none;
        }
        .headerhover
        {
            background-color: #EEEEEE;
        }
        .ofix p
        {
            font-size: 15px;
            line-height: 28px;
            padding-top: 2px;
        }
    </style>
</head>
<body>
    <!-- 公共页头  -->
    <header class="header">                    <h1>景点门票</h1>
        <div class="left-head">
          <!--<a id="goBack" href="/" class="tc_back head-btn"><span class="inset_shadow"><span class="head-return"></span></span></a>
        -->
        </div>
        <div class="right-head">
        </div>
</header>

    <!-- 页面内容块 -->
    <div id="firstpage">
        <div style="position: relative;">
            <div id="page1" class="default">
            <%if (comid == 106 && comid==0)
          { %>
            <div class="order-by" data-id="0">
            <ul class="fn-clear">
                <li data-id="0" class="act">推荐</li>
                <li id="shaixuan">筛选</li>
            </ul>
        </div>
        <%}%>
                <div class="bannerbox">
                    <div class="searchbox">
                        <div class="p_search">
                            <p class="input_span">
                                <input type="search" nohide="1" id="SearchName" placeholder="请输入景点名/主题" />
                                <span class="index2fdj" readonly="readonly" id="Search"></span>
                            </p>
                        </div>
                    </div>
                </div>
                <span id="IsFromBaidu" class="fn-hide"></span>
                <div class="stheme fn-clear">
                    <ul class="citylist">
                        <li><a href="javascript:;" id="viewbeijingmore">北京</a></li>
                        <div style="clear: both; display: none" id="beijingmore">
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=朝阳区&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">朝阳</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=昌平区&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">昌平</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=顺义区&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">顺义</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=怀柔区&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">怀柔</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=海淀区&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">海淀</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=丰台区&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">丰台</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=房山区&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">房山</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=密云县&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">密云</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=延庆县&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">延庆</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=大兴 区&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">大兴</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=通州区&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">通州</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=平谷区&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">平谷</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=东城区&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">东城</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=西城区&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">西城</a></li>

                        </div>
                        <li><a href="http://shop106.etown.cn/h5/List.aspx?key=河北省&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">河北</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=山东省&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">山东</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=山西省&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">山西</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=江苏省&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">江苏</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=福建省&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">福建</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=天津市&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">天津</a></li>

                        <li><a id="morecity" style="color: #006edc;">更多</a></li>
                        <div style="clear: both; display: none" id="morelist">
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=广西壮族&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">广西壮族</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=湖北省&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">湖北</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=内蒙古自治区&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">内蒙古</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=浙江省&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">浙江</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=上海市&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">上海</a></li>
                            <li><a href="http://shop106.etown.cn/h5/List.aspx?key=海南省&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">海南</a></li>
                        </div>
                    </ul>
                </div>
            </div>
        </div>
        <div style="width: 93%; text-align: center; margin: 0 auto; margin-top: 15px;">
            <div style="background-color: #dadee2; height: 1px; width: 100%;">
            </div>
            <div style="background-color: #fcfcfc; height: 1px; width: 100%;">
            </div>
        </div>
        <div>
            <div class="other">
                <div class="otherwrap">
                    <a href="http://shop106.etown.cn/h5/List.aspx?class=4&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">
                        <div id="zbjdbutt" class="spana spana1">
                            <div id="dwxs" class="ztname">
                                主题乐园</div>
                             <span class="icon-twitter daohangtubiao"></span>
                            <br />
                            <div class="ztinfo">
                                </div>
                        </div>
                    </a><a href="http://shop106.etown.cn/h5/List.aspx?class=6&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">
                        <div class="spana spana2">
                        
                            <div class="ztname">
                                景点门票
                            </div>
                            <span class="icon-picture daohangtubiao"></span>
                            <br />
                            <div class="ztinfo">
                                </div>
                                
                        </div>
                    </a><a href="http://shop106.etown.cn/h5/List.aspx?class=2&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">
                        <div class="spana spana3">
                            <div class="ztname">
                                温泉洗浴</div>
                             <span class="icon-beaker daohangtubiao"></span>
                            <br />
                            <div class="ztinfo">
                            </div>
                        </div>
                    </a><a href="http://shop106.etown.cn/h5/List.aspx?class=5&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">
                        <div class="spana spana4">
                            <div class="ztname">
                                休闲运功
                            </div>
                             <span class="icon-globe daohangtubiao"></span>
                            <br />
                            <div class="ztinfo">
                                </div>
                        </div>
                    </a><a href="http://shop106.etown.cn/h5/List.aspx?class=11&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">
                        <div class="spana spana5">
                            <div class="ztname">
                                戏水漂流</div>
                                 <span class="icon-umbrella daohangtubiao"></span>
                            <br />
                            <div class="ztinfo">
                                </div>
                        </div>
                    </a><a href="http://shop106.etown.cn/h5/List.aspx?class=12&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">
                        <div class="spana spana6">
                            <div class="ztname">
                                采摘烧烤</div>
                                 <span class="icon-github-alt daohangtubiao"></span>
                            <br />
                            <div class="ztinfo">
                                </div>
                        </div>
                    </a><a href="http://shop106.etown.cn/h5/List.aspx?class=8&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">
                        <div class="spana spana7">
                            <div class="ztname">
                                度假酒店</div>
                                 <span class="icon-home daohangtubiao"></span>
                            <br />
                            <div class="ztinfo">
                                </div>
                        </div>
                    </a><a href="http://shop106.etown.cn/h5/List.aspx?class=14&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">
                        <div class="spana spana8">
                            <div class="ztname">
                                功夫杂技</div>
                                 <span class="icon-github-sign daohangtubiao"></span>
                            <br />
                            <div class="ztinfo">
                                </div>
                        </div>
                    </a><a href="http://shop106.etown.cn/h5/List.aspx?class=16&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">
                        <div class="spana spana9">
                            <div class="ztname">
                                博物馆与名人故居</div>
                                 <span class="icon-leaf daohangtubiao"></span>
                            <br />
                            <div class="ztinfo">
                                </div>
                        </div>
                    </a><a href="http://shop106.etown.cn/h5/List.aspx?class=15&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>">
                        <div class="spana spana10">
                            <div class="ztname">
                                电影</div>
                                 <span class="icon-eye-open daohangtubiao"></span>
                            <br />
                            <div class="ztinfo">
                                </div>
                        </div>
                    </a>
                </div>
            </div>
            <div class="hx" id="hx">
                <span class="bottomsonw1"></span><span class="bottomsonw2"></span><span class="bottomsonw3">
                </span><span class="tree"></span><span class="sonw1"></span><span class="sonw2">
                </span><span class="sonw3"></span><span class="sonw4"></span><span class="sonw5">
                </span><span class="hx_bg"></span>
                <div class="bxts">
                    <!-- <img class="hx_bgimg" src="" />支持景点付款
                便捷无风险-->
                </div>
            </div>
        </div>
    </div>

    <!-- 筛选 -->
    <div style="display: none;" id="selectTypePage" class="page current">
    <div class="page-header">
        <a href="javascript:void(0);" class="page-back touchable"></a>
        <h2>景点筛选</h2>
        <!-- 需要头部菜单加上下面内容 -->
        <a href="javascript:void(0);" class="header-menu-btn touchable"><i></i></a>
    </div>

    <div style="transform-origin: 0px 0px 0px; opacity: 1; transform: scale(1, 1);" id="page2">
        <div class="filter" id="f-level" data-id="0">
            <div class="f-title">
                <h6>
                    按景点位置</h6>
                <span id="f-level-viw">不限</span> <em class="icon-right"></em>
            </div>
            <ul id="f-level-ul">
                 <li class="act" data-id="0" data-type-name="不限">不限<s></s></li>
                 <li data-id="北京市" data-type-name="北京">北京</li>
                 <li data-id="河北省" data-type-name="河北">河北</li>
                 <li data-id="山东省" data-type-name="山东">山东</li>
                 <li data-id="山西省" data-type-name="山西">山西</li>
                 <li data-id="江苏省" data-type-name="江苏">江苏</li>
                 <li data-id="福建省" data-type-name="福建">福建</li>
                 <li data-id="天津市" data-type-name="天津">天津</li>
                 <li data-id="广西壮族" data-type-name="广西">广西</li>
                 <li data-id="湖北省" data-type-name="湖北">湖北</li>
                 <li data-id="内蒙古自治区" data-type-name="内蒙古">内蒙古</li>
                 <li data-id="浙江省" data-type-name="浙江">浙江</li>
                 <li data-id="上海市" data-type-name="上海">上海</li>
                 <li data-id="海南省" data-type-name="海南">海南</li>
            </ul>
        </div>
        <div class="filter" id="f-theme" data-id="0">
            <div class="f-title">
                <h6>
                    按旅游主题</h6>
                <span  id="f-theme-viw">不限</span> <em class="icon-right"></em>
            </div>
            <ul id="f-theme-ul">
                <li class="act" data-id="0" data-type-name="不限">不限<s></s></li>
                <li data-id="4" data-type-name="主题乐园">主题乐园</li>
                <li data-id="6" data-type-name="景点门票">景点门票</li>
                <li data-id="2" data-type-name="温泉洗浴">温泉洗浴</li>
                <li data-id="5" data-type-name="休闲运动">休闲运动</li>
                <li data-id="11" data-type-name="戏水漂流">戏水漂流</li>
                <li data-id="12" data-type-name="采摘烧烤">采摘烧烤</li>
                <li data-id="8" data-type-name="度假酒店">度假酒店</li>
                <li data-id="14" data-type-name="功夫杂技">功夫杂技</li>
                <li data-id="16" data-type-name="博物馆与名人故居">博物馆与名人故居</li>
                <li data-id="15" data-type-name="电影">电影</li>
            </ul>
        </div>
        <div class="filter" id="f-price" data-id="0">
            <div class="f-title">
                <h6>
                    按价格</h6>
                <span  id="f-price-viw">不限</span> <em class="icon-right"></em>
            </div>
            <ul id="f-price-ul">
                <li class="act" data-id="0">不限<s></s></li>
                <li data-id="1" data-type-name="¥1-¥50">¥1-¥50</li>
                <li data-id="2" data-type-name="¥50-¥100">¥50-¥100</li>
                <li data-id="3" data-type-name="¥100以上">¥100以上</li>
            </ul>
        </div>
        <div class="confirm" id="confirm">
            确定</div>
    </div>
</div>
    <!-- 公共页脚  -->
    <footer>
          <div class="footer_link">
              
          </div>
          <div class="footer_link c_right">
                  <span style="display:block; padding-bottom:5px;  line-height:20px;">景点门票客服热线：<a href="tel:59059150" style="">010-59059150</a></span>
 &copy;门票在线预订 
          </div>
      </footer>

      <script type="text/javascript" src="/Scripts/ppkextend.js"></script>
    <script type="text/javascript">
        $(function () {

            //分享  
            $.ppkWeiShare({
                path: location.href,
                image: "<%=comlogo %>",
                desc: "欢迎订购景点门票",
                title: '景点门票'
            });
        });
</script>
      <input id="f-level-val" type="hidden" value="" />
      <input id="f-theme-val" type="hidden" value="0" />
      <input id="f-price-val" type="hidden" value="0" />

</body>
</html>
