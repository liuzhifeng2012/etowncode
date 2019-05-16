<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="headoffice_PeopleList.aspx.cs" Inherits="ETS2.WebApp.H5.headoffice_PeopleList" %>

<!DOCTYPE html >
<html class="ng-scope" ng-app="zhimawuApp">
<head>

<meta charset="UTF-8">
<meta http-equiv="pragma" content="no-cache">
<meta http-equiv="Cache-Control" content="no-cache, must-revalidate">
<meta http-equiv="expires" content="0">
<title><%=title %></title>
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0,user-scalable=no">
<!-- Place favicon.ico and apple-touch-icon.png in the root directory -->
<link rel="stylesheet" href="/Styles/H5/peoplelist.css">
<script type="text/javascript" src="/Scripts/jquery-1.4.2.min.js"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="../Scripts/Order.js" type="text/javascript"></script>
    <script src="/Scripts/MenuButton.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageindex = 1;
        var pageSize = 12;
        $(function () {

            var comid = $("#hid_comid").val();
            var pro_class = $("#firstdaohang").val();
            var stop = true;
            var channelcompanyid = $("#channelcompanyid").val();
            SearchList(pageindex, 12);
            getmenubutton(comid, 'js-navmenu');

            //根据公司id获得关注作者信息
            $.post("/JsonFactory/AccountInfo.ashx?oper=getcurcompanyguanzhu", { comid: comid }, function (data) {
                dat = eval("(" + data + ")");
                if (dat.type == 1) {

                }
                if (dat.type == 100) {
                    $(".links").html("<a href=\"" + dat.msg + "\" class=\"mp-homepage btn btn-follow\">关注我们</a>");
                }
            });



            $(window).scroll(function () {
                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());

                if ($(document).height() <= totalheight) {
                    if (stop == true) {
                        //var pageSize = parseInt($("#num").val()) + 12;
                        var pageindex = parseInt($("#pageindex").val()) + 1;

                        stop = false;
                        $("#loading").show();
                        $.ajax({
                            type: "post",
                            url: "/JsonFactory/CrmMemberHandler.ashx?oper=peopleList",
                            data: { comid: comid, pageindex: pageindex, pagesize: pageSize, channelcompanyid: 0, key: "", openid: $("#hid_openid").val(), usern: $("#usern").val(), usere: $("#usere").val(), isheadofficekf: "1", isviewjiaolian: "0" },
                            async: false,
                            success: function (data) {
                                data = eval("(" + data + ")");
                                stop = true;
                                $("#loading").hide();
                                if (data.type == 1) {
                                    return;
                                }
                                if (data.type == 100) {
                                    $("#loading").hide();
                                    $("#ProductItemEdit").tmpl(data.msg).appendTo("#line-list");
                                    $("#pageindex").val(pageindex);

                                }
                            }
                        })


                    }
                }
            });

        });

        //装载产品列表
        function SearchList(pageindex, pageSize) {
            var comid = $("#hid_comid").val();
            var channelcompanyid = $("#channelcompanyid").val();
            if (pageindex == '') {
                pageindex = 1;
            }
            $("#loading").show();


            $.ajax({
                type: "post",
                url: "/JsonFactory/CrmMemberHandler.ashx?oper=peopleList",
                data: { comid: comid, pageindex: pageindex, pagesize: pageSize, channelcompanyid: 0, key: "", openid: $("#hid_openid").val(), usern: $("#usern").val(), usere: $("#usere").val(),isheadofficekf:"1",isviewjiaolian:"0" },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    $("#loading").hide();
                    if (data.type == 1) {
                        return;
                    }
                    if (data.type == 100) {
                        $("#line-list").empty();
                        $("#ProductItemEdit").tmpl(data.msg).appendTo("#line-list");
                        $("#pageindex").val(pageindex);


                    }
                }
            })
        }


        //获取坐标

        function getLocation() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition);
            }
            else {// 基于H5浏览器获取坐标不支持 "Geolocation is not supported by this browser."; 
                SearchList(1, 12);
            }
        }

        function showPosition(position) {
            $("#usern").val(position.coords.latitude);
            $("#usere").val(position.coords.longitude);
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=editcrmposition", { comid: $("#hid_comid").val(), openid: $("#hid_openid").val(), usern: $("#usern").val(), usere: $("#usere").val() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {

                }
            })

            SearchList(1, 12);
            //("Latitude: " + position.coords.latitude +  "Longitude: " + position.coords.longitude);
        }

        getLocation();
        
    </script>
</head>
<body style="height: auto">
<div style="" class="container view-animate ng-scope" ng-view="" onload="viewloadHandler()" autoscroll="">
	<header class="ng-scope">
		<div class="b-shadow">
    
            <div class="js-mp-info share-mp-info">
                    <a class="page-mp-info" href="/h5/order/default.aspx">
                        <img class="mp-image" src="<%=comlogo %>"g" height="24" width="24">
                        <i class="mp-nickname">
                            <%=title %>                </i>
                    </a>
                   <div class="links"></div>
            </div>
		</div>
	</header>
	<div zm-scroll="" srcroll-ref="true" class="scrollWrapper ng-scope">
		<div class="scroll">
			<div ng-transclude="">
				<!-- ngIf: artis.length==0 -->
				<ul id="line-list" class="designer-list ng-scope" style="padding-bottom:40px">
				
					
					
					
				</ul>
				<div class="loadIndicator animate-show ng-scope ng-binding ng-hide" ng-show="loading"></div>
				<!-- <div class="margin-b"></div> -->
			</div>
		</div>
	</div>
</div>
 <script type="text/x-jquery-tmpl" id="ProductItemEdit"> 
                    <!-- ngRepeat: art in artis -->
					<li class="clearfix ng-scope" ng-repeat="art in artis">

						<div class="distance"><span class="in ng-binding" ng-bind-html="art.distance | num2mi"><strong>${ViewDistance_jiandan(Distance)}</strong></span></div>
						<div class="info box-shadow">
						<a href="People.aspx?MasterId=${Id}&come=list">
							<div class="avatar">
							<span style="background-image:url(${Headimgurl})"></span></div>
							<ul class="title">
								<li class="ng-binding"><strong class="name ng-binding">${Name}</strong>${cutstr(Job,10)}
                                <span style="float:right;padding-right:5px">
                                {{if Workingyears!=0}}
                                    {{if Workingyears==1}}从业1年
                                    {{/if}}
                                    {{if Workingyears==2}}从业2年
                                    {{/if}}
                                    {{if Workingyears==3}}从业3年
                                    {{/if}}
                                    {{if Workingyears==4}}从业4年
                                    {{/if}}
                                    {{if Workingyears==5}}从业5年+
                                    {{/if}}
                                    {{/if}}
                                </span>
                                </li>
								<li>
									<div class="pull-left star">
										{{if ChannelCompany !=""}}
                                        ${ChannelCompany}
                                        {{else}}
                                        ${CompanyName}
                                        {{/if}}
                                        
									</div>
									<div class="pull-right count ng-binding" style="display: none;">
                                    
                                    </div>
								</li>
							</ul>
							<div class="text ng-binding">${Selfbrief}</div>
						</a>
						</div>
					</li>
					<!-- end ngRepeat: art in artis -->

        </script>
<style>
    .copyrightbox{ padding: 30px 0px; font-size:14px; background:#f0f0f0; color:#666666;   text-align:center;}
    .copyrightbox .copyrightbtn{ color:#0066cc; padding-right:15px;}
</style>

<div class="copyrightbox fn-clear">
<span id="copydaohang"></span>
    <span class="links"></span>

<div style=" padding-top:15px;font-size:12px;">
    易城商户平台技术支持
   </div>
</div>

<div id="mcover" class="mcover" onclick="document.getElementById('mcover').style.display='';" style="display: none;">
		<img src="/images/weixinkefu.png">
	</div>
    <div id="loading" class="loading">
       正在加载..
    </div>
    <script type="text/javascript" src="/Scripts/ppkextend.js"></script>
    <script type="text/javascript">
        $(function () {

            //分享  
            $.ppkWeiShare({
                path: location.href,
                image: "",
                desc: "点击选择您的服务顾问，就可以微信语音时时咨询",
                title: '服务顾问列表'
            });
        });
</script>
<input id="hid_comid" type="hidden" value="<%=comid %>" />
<input id="pageindex" type="hidden" value="1" />
<input id="usern" type="hidden" value="0" />
<input id="usere" type="hidden" value="0" />

<input id="hid_openid" type="hidden" value="osaHEjsHg4_hZUHVHPvT4aRUV67M" />
    <script type="text/javascript" src="/Scripts/ppkextend.js"></script>
    <script type="text/javascript">
        $(function () {
            //分享  
            $.ppkWeiShare({
                path: location.href,
                image: "<%=comlogo %>",
                desc: "",
                title: ' <%=title %>'
            });
        });
    </script>



</body>
</html>
