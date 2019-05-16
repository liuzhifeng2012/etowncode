<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="send.aspx.cs" MasterPageFile="/UI/Etown.Master"
    Inherits="ETS2.WebApp.WeiXin.masssend.send" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").val();
            var userid = $("#hid_userid").val();

            //加载图文列表
            SearchList(1, 5);

            $("#selmsgtype").change(function () {
                var seld = $("#selmsgtype").val();
                if (seld == "news") {
                    $("#div_tuwen").show();
                    $("#txtcontent").hide();
                    $(".appmsg_content").hide();
                }

                if (seld == "text") {
                    $("#div_tuwen").hide();
                    $("#txtcontent").show();
                    $(".appmsg_content").hide();
                }
            })

            $("#sel_country").empty();
            $("#sel_province").empty();
            $("#sel_city").empty();
            //获取群发国家
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetWxMemberCountry", {}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $("#sel_country").append('<option value="国家">国家</option>');

                    $("#sel_province").append('<option value="省市">省市</option>');
                    $("#sel_city").append('<option value="城市">城市</option>');
                    $("#sel_province").hide();
                    $("#sel_city").hide();
                }
                if (data.type == 100) {
                    $("#sel_country").empty();

                    $("#sel_country").append('<option value="国家">国家</option>');
                    $("#sel_country").append('<option value="全部">全部</option>');
                    if (data.msg.length > 0) {
                        for (var i = 0; i < data.msg.length; i++) {
                            $("#sel_country").append('<option value="' + data.msg[i] + '">' + data.msg[i] + '</option>');
                        }
                    }

                    $("#sel_province").append('<option value="省市">省市</option>');
                    $("#sel_city").append('<option value="城市">城市</option>');
                    $("#sel_province").hide();
                    $("#sel_city").hide();
                }

            })

            $("#sel_country").change(function () {


                var country = $("#sel_country").val();
                if (country != "国家") {
                    //去掉国家列表中 “国家”
                    $("#sel_country option[value='国家']").remove();

                    //为省市添加值
                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetWxMemberProvince", { country: country }, function (data) {
                        data = eval("(" + data + ")");
                        $("#sel_province").empty();
                        $("#sel_city").empty();

                        if (data.type == 1) {

                            $("#sel_province").append('<option value="省市">省市</option>');
                            $("#sel_city").append('<option value="城市">城市</option>');
                            $("#sel_province").hide();
                            $("#sel_city").hide();
                        }
                        if (data.type == 100) {
                            $("#sel_province").append('<option value="省市">省市</option>');

                            if (data.msg.length > 0) {
                                for (var i = 0; i < data.msg.length; i++) {
                                    $("#sel_province").append('<option value="' + data.msg[i] + '">' + data.msg[i] + '</option>');
                                }
                                $("#sel_province").show();
                                $("#sel_city").hide();
                            }
                        }
                    })
                }
            })

            $("#sel_province").change(function () {


                var province = $("#sel_province").val();
                if (province != "省市") {
                    //去掉省市列表中 “省市”
                    $("#sel_province option[value='省市']").remove();

                    //为城市添加值
                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetWxMemberCity", { province: province }, function (data) {
                        data = eval("(" + data + ")");

                        $("#sel_city").empty();

                        if (data.type == 1) {

                            $("#sel_city").append('<option value="城市">城市</option>');

                            $("#sel_city").hide();
                        }
                        if (data.type == 100) {
                            $("#sel_city").append('<option value="城市">城市</option>');

                            if (data.msg.length > 0) {
                                for (var i = 0; i < data.msg.length; i++) {
                                    $("#sel_city").append('<option value="' + data.msg[i] + '">' + data.msg[i] + '</option>');
                                }
                                $("#sel_province").show();
                                $("#sel_city").show();
                            }
                        }
                    })
                }
            })
            $("#sel_city").change(function () {


                var city = $("#sel_city").val();
                if (city != "城市") {
                    //去掉省市列表中 “省市”
                    $("#sel_city option[value='城市']").remove();
                }

            })



            //得到员工详细信息 包括登录用户所在公司/门市信息->设定群发对象 内容
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetCompanyUser", { userid: userid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {

                }
                if (data.type == 100) {
                    if (data.msg[0].Channelcompanyid == 0) {//总公司账户
                        $("#sel_sendobj").empty();
                        $("#sel_sendobj").append('<option value="0">全部用户</option><option value="1">按门店选择</option><option value="2">按分组选择</option>');
                        $("#sel_childobj").empty();
                        $("#sel_childobj").append('<option value="0">全部</option>');
                        $("#sel_childobj").hide();
                    } else {//门市账户
                        $("#sel_sendobj").empty();
                        $("#sel_sendobj").append('<option value="1">按门店选择</option>');
                        $("#sel_childobj").empty();
                        $("#sel_childobj").append('<option value="' + data.msg[0].Channelcompanyid + '">' + data.msg[0].ChannelCompanyName + '</option>');
                        $("#sel_childobj").show();
                    }
                }

            })




            $("#sel_sendobj").change(function () {
                var sendobj = $("#sel_sendobj").val();
                if (sendobj == "1") {//按门店查询
                    $("#tdchildobj").show();
                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetMenshisByComid", { comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $("#sel_childobj").empty();
                            $("#sel_childobj").append('<option value="0">全部</option>');
                            $("#sel_childobj").hide();
                        }
                        if (data.type == 100) {
                            if (data.msg != null) {
                                $("#sel_childobj").empty();
                                $("#sel_childobj").append('<option value="0">全部</option>');
                                for (var i = 0; i < data.msg.length; i++) {
                                    $("#sel_childobj").append('<option value="' + data.msg[i].Id + '">' + data.msg[i].Companyname + '</option>');
                                }
                                $("#sel_childobj").show();
                            } else {
                                $("#sel_childobj").empty();
                                $("#sel_childobj").append('<option value="0">全部</option>');
                                $("#sel_childobj").hide();
                            }
                        }
                    })
                }
                else if (sendobj == "2") {//按分组查询
                    $("#tdchildobj").show();
                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetCompanyB2bgroup", { comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $("#sel_childobj").empty();
                            $("#sel_childobj").append('<option value="0">默认组</option>');
                            $("#sel_childobj").hide();
                        }
                        if (data.type == 100) {
                            if (data.total > 0) {
                                $("#sel_childobj").empty();
                                $("#sel_childobj").append('<option value="0">默认组</option>');
                                for (var i = 0; i < data.msg.length; i++) {
                                    $("#sel_childobj").append('<option value="' + data.msg[i].Id + '">' + data.msg[i].Groupname + '</option>');
                                }
                                $("#sel_childobj").show();
                            } else {
                                $("#sel_childobj").empty();
                                $("#sel_childobj").append('<option value="0">默认组</option>');
                                $("#sel_childobj").hide();
                            }
                        }
                    })
                }
                else {
                    $("#tdchildobj").hide();
                    $("#sel_childobj").empty();
                    $("#sel_childobj").append('<option value="0">全部</option>');
                    $("#sel_childobj").hide();
                }
            })


            //----------兴趣类型BEGIN----------
            //获得公司详情
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetCompanyDetail", {}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("获得公司详情失败");
                    return;
                }
                if (data.type == 100) {
                    if (data.msg.Com_type == 0) {
                        GetTagTypeList(0)
                    } else {
                        GetTagTypeList(data.msg.Com_type)
                    }
                }
            })

            $("#sel_tagtype").change(function () {
                var tagtype = $("#sel_tagtype").val();
                if (tagtype == 0) {
                    $("#sel_tag").hide();
                } else {
                    $("#sel_tag").show();
                }
                GetTagListByType(tagtype);
            })
            //----------兴趣类型END------------


            $("#btn").click(function () {
                var sendobj = $("#sel_sendobj").val();
                var sendchildobj = $("#sel_childobj").val();
                //                if (sendobj != "0") {
                //                    if (sendchildobj == "0") {
                //                        alert("请选择需要群发的门店");
                //                        return;
                //                    }
                //                }

                var country = $("#sel_country").val();
                var province = $("#sel_province").val();
                var city = $("#sel_city").val();
                var sex = $("#sel_sex").val();

                var tagtype = $("#sel_tagtype").val();
                var tag = $("#sel_tag").val();



                var content = $("#txtcontent").val();
                var msgtype = $("#selmsgtype").val();

                if (msgtype == "text") {
                    if ($.trim(content) == "") {
                        alert("群发内容都不可为空!");
                        return;
                    }
                }
                else if (msgtype == "news") {
                    if ($("#hid_mediaid").val() == "") {
                        alert("请选择群发图文!");
                        return;
                    }
                } else {
                    alert("暂时只支持群发文本，图文!");
                    return;
                }


                //获取群发条数
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=wxqunfanum", { tagtype: tagtype, tag: tag, sendobj: $("#sel_sendobj").val(), sendchildobj: $("#sel_childobj").val(), country: $("#sel_country").val(), province: $("#sel_province").val(), city: $("#sel_city").val(), sex: $("#sel_sex").val(), userid: $("#hid_userid").val(), comid: $("#hid_comid").val() }, function (data1) {
                    data1 = eval("(" + data1 + ")");
                    if (data1.type == 1) {

                    }
                    if (data1.type == 100) {
                        if (data1.msg == 0) {
                            alert("满足条件的人数为0人，不符合群发条件");
                            return;
                        } else {
                            if (confirm("满足条件的人数为" + data1.msg + "人,是否确认群发?")) {
                                $.post("/JsonFactory/WeiXinHandler.ashx?oper=wxqunfa", { tagtype: tagtype, tag: tag, sendobj: sendobj, sendchildobj: sendchildobj, country: country, province: province, city: city, sex: sex, userid: userid, comid: comid, content: content, msgtype: msgtype, media_id: $("#hid_mediaid").val() }, function (data) {
                                    data = eval("(" + data + ")");
                                    if (data.type == 1) {
                                        alert("群发失败" + data.msg);
                                    }
                                    if (data.type == 100) {

                                        $.prompt("群发成功", {
                                            buttons: [{ title: "确定", value: true}],
                                            submit: function (e, v, m, f) {
                                                if (v == true)
                                                    location.href = "list.aspx";
                                            }
                                        })
                                    }
                                })
                            }
                            else {
                                //                                    alert("群发操作取消");
                                return;
                            }
                        }
                    }
                })


            })

            //微信群发测试
            $("#lbltestqunfa").click(function () {
                var content = $("#txtcontent").val();
                var msgtype = $("#selmsgtype").val();

                if (msgtype == "text") {
                    if ($.trim(content) == "") {
                        alert("群发内容都不可为空!");
                        return;
                    }
                }
                else if (msgtype == "news") {
                    if ($("#hid_mediaid").val() == "") {
                        alert("请选择群发图文!");
                        return;
                    }
                } else {
                    alert("暂时只支持群发文本，图文!");
                    return;
                }
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=wxqunfa_dddtest", { userid: userid, comid: comid, content: content, msgtype: msgtype, media_id: $("#hid_mediaid").val(), weixins: "osaHEjm6eZ0V-PwZtsoBNB6tUrr8,osaHEjsHg4_hZUHVHPvT4aRUV67M,osaHEji8AHbyuaWGqLQo0rXJtN5I,osaHEjqhTUVNzGVC_nGV_Eh69Nlw" }, function (data) {
                                    data = eval("(" + data + ")");
                                    if (data.type == 1) {
                                        alert("群发失败" + data.msg);
                                    }
                                    if (data.type == 100) {

                                        $.prompt("群发成功", {
                                            buttons: [{ title: "确定", value: true}],
                                            submit: function (e, v, m, f) {
                                                if (v == true)
                                                    location.href = "list.aspx";
                                            }
                                        })
                                    }
                                })    
            })

            var tuwen_recordid = $("#hid_tuwen_recordid").trimVal();
            if (tuwen_recordid != 0) {
                $("#selmsgtype").val("news");
                //根据图文消息id得到图文消息
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=wxqunfa_news_addrecord", { userid: $("#hid_userid").trimVal(), comid: $("#hid_comid").trimVal(), tuwen_recordid: tuwen_recordid }, function (data) {
                    data = eval("(" + data + ")");
                    selednews(data.msg.id, data.msg.createtime, data.msg.media_id);
                })


            }
        })

        function GetTagTypeList(industryid) {
            //兴趣标签类型列表
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetTagTypeListByIndustryid", { industryid: industryid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $("#sel_tagtype").empty();
                    $("#sel_tagtype").append("<option value=\"0\">全部</option>");
                    //                    alert("获取兴趣标签类型列表失败");
                    return;
                }
                if (data.type == 100) {
                    $("#sel_tagtype").empty();
                    if (data.msg.length > 0) {
                        $("#sel_tagtype").append("<option value=\"0\">全部</option>");
                        for (var i = 0; i < data.msg.length; i++) {
                            $("#sel_tagtype").append("<option value=\"" + data.msg[i].Id + "\">" + data.msg[i].Typename + "</option>");
                        }
                    } else {
                        $("#sel_tagtype").empty();
                        $("#sel_tagtype").append("<option value=\"0\">全部</option>");
                        //                        alert("还未添加兴趣标签类型");
                        return;
                    }
                }
            })
        }
        function GetTagListByType(typeid) {


            //兴趣标签列表
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetTagListByTypeid", { typeid: typeid, comid: $("#hid_comid").trimVal(), issystemadd: "0,1" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $("#sel_tag").empty();
                    $("#sel_tag").append('<option value="0">全部</option>');
                    return;
                }
                if (data.type == 100) {
                    $("#sel_tag").empty();
                    $("#sel_tag").append('<option value="0">全部</option>');
                    if (data.msg.length > 0) {
                        for (var i = 0; i < data.msg.length; i++) {
                            $("#sel_tag").append('<option value="' + data.msg[i].Id + '">' + data.msg[i].TagName + '</option>');
                        }
                    }
                    else {
                        $("#sel_tag").empty();
                        $("#sel_tag").append('<option value="0">全部</option>');
                        return;
                    }
                }
            })

        }
    </script>
    <script type="text/javascript">
        $(function () {
            $("#cancel_rh").click(function () {
                $("#div_tuwen").hide();
            })
        })
        function selednews(recordid, createtime, mediaid) {
             if(mediaid==''){
                 alert("图文还未上传到微信服务器，不可上传");
                 return;
             }
            //根据记录id得到 记录下图文消息
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetNewsListByRecordid", { recordid: recordid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("获取图文列表失败");
                    return;
                }
                if (data.type == 100) {
                    $("#div_tuwen").hide();
                    $("#hid_mediaid").val(mediaid);
                    $(".appmsg_content").html("");

                    var news = "";
                    for (var i = 0; i < data.msg.length; i++) {
                        news += '<h4 class="appmsg_title">' +
                                '<a href="#" target="_blank">' + data.msg[i].title + '</a></h4>' +
                            '<div class="appmsg_info">' +
                                '<em class="appmsg_date">' + createtime + '</em>' +
                            '</div>' +
                            '<div class="appmsg_thumb_wrp">' +
                                '<img src="' + data.msg[i].thumb_url + '" alt="" class="appmsg_thumb" width="292px" height="160px"></div>';
                        if (data.msg[i].digest != "") {
                            news += '<p class="appmsg_desc">' + data.msg[i].digest + '</p>';
                        } else {
                            news += '<p class="appmsg_desc">' + data.msg[i].content + '</p>';
                        }
                    }
                    $(".appmsg_content").html(news);
                    $(".appmsg_content").show();
                }
            })
        }
        //查询图文列表
        function SearchList(pageindex, pagesize) {
            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.ajax({
                type: "post",
                url: "/JsonFactory/WeiXinHandler.ashx?oper=wxqunfa_news_addrecordpagelist",
                data: { userid: $("#hid_userid").trimVal(), comid: $("#hid_comid").trimVal(), pageindex: pageindex, pagesize: pagesize, key: "" },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalCount == 0) {

                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalcount, pagesize, pageindex);
                        }


                    }
                }
            })


        }

        //分页
        function setpage(newcount, newpagesize, curpage) {
            $("#divPage").paginate({
                count: Math.ceil(newcount / newpagesize),
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

                    SearchList(page, newpagesize);

                    return false;
                }
            });
        }
    </script>
    <style type="text/css">
        .appmsg_desc
        {
            padding: 5px 0 10px;
            word-wrap: break-word;
            word-break: break-all;
            text-align: left;
        }
        .appmsg_content
        {
            padding: 0 14px;
            position: relative;
            width: 320px;
            height: auto;
        }
        .appmsg_thumb_wrp
        {
            height: 160px;
            overflow: hidden;
        }
        
        
        .button
        {
            width: 140px;
            height: 40px;
            line-height: 38px;
            text-align: center;
            font-weight: bold;
            color: #fff;
            text-shadow: 1px 1px 1px #333;
            border-radius: 5px;
            margin: 0 20px 20px 0;
            position: relative;
            overflow: hidden;
        }
        
        .button.blue
        {
            border: 1px solid #1e7db9;
            box-shadow: 0 1px 2px #8fcaee inset,0 -1px 0 #497897 inset,0 -2px 3px #8fcaee inset;
            background: -webkit-linear-gradient(top,#42a4e0,#2e88c0);
            background: -moz-linear-gradient(top,#42a4e0,#2e88c0);
            background: linear-gradient(top,#42a4e0,#2e88c0);
            margin-top: 20px;
            margin-left: 5px;
        }
        .blue:hover
        {
            background: -webkit-linear-gradient(top,#70bfef,#4097ce);
            background: -moz-linear-gradient(top,#70bfef,#4097ce);
            background: linear-gradient(top,#70bfef,#4097ce);
        }
        .trline
        {
            line-height: 40px;
        }
        .vis-zone, td
        {
            font-size: 15px;
        }
        #settings h3
        {
            padding: 10px 0 5px 0;
            font-size: 20px;
            font-weight: bold;
            clear: both;
            color: #2D65AA;
        }
    </style>
    <script type="text/javascript">
        jQuery.extend({ alertWindow: function (e, t, n) {
            var e = e, t = t, r; n === undefined ? r = "#FF7C00" : r = n; if ($("body").find(".alertWindow1").length === 0) {
                var i = '<div  class="alertWindow1" style="width: 100%;height: 100%; background:rgba(0,0,0,0.5);position: fixed; left:0px; top: 0px; z-index: 9999;"><div  style="width: 400px; height: 200px;background: #FFF;margin: 180px auto;border: 2px solid #CFCFCF; border-bottom: 10px solid ' + r + ';">' + '<div  style="width: inherit;height: 20px;">' + '<div class="alertWindowCloseButton1" style="float: right; width: 10px; height: 20px;margin-right:5px;font-family:\'microsoft yahei\';color:' + r + ';cursor: pointer;">X</div>' + "</div>" + '<h1 class="alertWindowTitle" style="margin-top:20px;text-align:center;font-family:\'\';font-size: 18px;font-weight: normal;color: ' + r + ';">' + e + "</h1>" + '<div class="alertWindowContent" style="width:360px;px;height: 60px;padding-left:20px;padding-right:20px;text-align:center;font-size: 15px;color: #7F7F7F;">' + t + "</div>" + '<p><input class="alertWindowCloseSure1" type="button" value="确 定" style="width: 100px;height: 50px;background:' + r + ';border:none;position: relative;bottom: 18px;font-size:18px;color:#FFFFFF;-webkit-border-radius: 10px;-moz-border-radius: 10px;border-radius: 10px;cursor: pointer;"></p>' + "</div>" + "</div>"; $("body").append(i); var s = $(".alertWindow1");
                $(".alertWindowCloseButton1").click(function () {
                    //                   s.hide() 
                    window.location.href = "/manage.aspx";
                }),
                $(".alertWindowCloseSure1").click(function () {
                    //                    s.hide()
                    window.location.href = "/manage.aspx";
                })
            } else $(".alertWindowTitle").text(e), $(".alertWindowContent").text(t), $(".alertWindow1").show()
        }
        })
    </script>
    <style type="text/css">
        /*弹出层的样式*/
        .alertWindowContent h1, p
        {
            text-align: center;
            font-size: 18px;
            font-weight: bolder;
        }
        .alertWindowContent input
        {
            width: 100px;
            height: 50px;
            cursor: pointer;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
        }
    </style>
    <script>
        $(function () {
          if(<%=isrenzheng %>==0){
        $("#viewrenzheng").show();
            //            jQuery.alertWindow("标题设置", "内容设置");
            //jQuery.alertWindow("该功能需将微信服务号进行认证后才能使用", "");
                
          }
        });
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="/weixin/masssend/send.aspx" onfocus="this.blur()"><span>新建群发消息</span></a></li>
                <li><a href="/weixin/masssend/list.aspx" onfocus="this.blur()">已发送</a></li>
                <li><a href="/weixin/masssend/news/news_list.aspx" onfocus="this.blur()">图文信息管理</a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <div style="width: 600px; height: 40px; padding-left: 10px; border: 1px solid #FF4444;
                    display: none;" id="viewrenzheng">
                    <div style="padding: 12px 0 14px 0;">
                        <h5 class="text-overflow">
                            <a smartracker="on" seed="contentList-mainLinkbox" href="http://shop1143.etown.cn/v/about.aspx?id=2661"
                                target="_blank" style="font-size: 16px">该功能需将微信服务号进行认证后才能使用 ,点击查看如何进行认证... </a>
                        </h5>
                    </div>
                    <div class="content-list-des text-overflow">
                    </div>
                </div>
                <h3>
                </h3>
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        微信群发消息</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            群发对象</label>
                        <select id="sel_sendobj" style="width: 120px;" class="mi-input">
                            <option value="0">全部用户</option>
                            <option value="1">按门店选择</option>
                            <option value="2">按分组选择</option>
                        </select>
                        <select id="sel_childobj" style="width: 130px;" class="mi-input">
                            <option value="0">未选择</option>
                        </select>
                        <span id="tdchildobj"></span>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            群发地区</label>
                        <select id="sel_country" class="mi-input">
                            <option value="">国家</option>
                        </select>
                        <select id="sel_province" class="mi-input">
                            <option value="">省市</option>
                        </select>
                        <select id="sel_city" class="mi-input">
                            <option value="">城市</option>
                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            兴趣</label>
                        <select id="sel_tagtype" class="mi-input">
                            <option value="0">全部</option>
                        </select>
                        <select id="sel_tag" style="width: 130px; display: none;" class="mi-input">
                            <option value="0">全部</option>
                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            性别</label>
                        <select id="sel_sex" class="mi-input">
                            <option value="0">全部</option>
                            <option value="1">男</option>
                            <option value="2">女</option>
                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            群发信息类型</label>
                        <select id="selmsgtype" style="width: 120px;" class="mi-input">
                            <option value="text">文本</option>
                            <option value="news">图文</option>
                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            群发内容</label>
                        <textarea id="txtcontent" cols="80" rows="6" style="margin-left: 5px; margin-top: 15px;"></textarea>
                        <div class="appmsg_content" style="display: none;">
                            <h4 class="appmsg_title">
                                <a href="#" target="_blank">title</a></h4>
                            <div class="appmsg_info">
                                <em class="appmsg_date">日期</em>
                            </div>
                            <div class="appmsg_thumb_wrp">
                                <img src="#图片" alt="" class="appmsg_thumb" width="292px" height="160px"></div>
                            <p class="appmsg_desc">
                                content</p>
                        </div>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <table width="780">
                    <tr class="trline">
                        <td align="center">
                            <input id="btn" type="button" value="群 发" class="button blue" class="mi-input" />
                        </td>
                    </tr>
                    <tr class="trline">
                        <td>
                            注1.微信用户每月只能接收4条群发消息，多于4条的群发将对该用户发送失败;<br />
                            注2.门店店长每月可向所属会员群发4次信息； <lable id="lbltestqunfa">..</label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div id="div_tuwen" class="edit-box J-commonSettingsModule" style="left: 5%; top: 20%;
        opacity: 1; margin-top: 0px; position: absolute; z-index: 10; width: 698px; height: 400px;
        background-color: #E7F0FA; display: none;">
        <input type="hidden" id="hid_mediaid" value="" />
        <h2 class="p-title-area">
            <label>
                图文消息列表(3天内)</label><label style="float: right; cursor: pointer;" id="cancel_rh">X</lable></h2>
        <div class="mi-form-item" style="float: right;">
         
                <a href="news/news_edit.aspx" onfocus="this.blur()" style="color: Blue; text-decoration: underline; font-size:15px;">
                    新建单图文信息</a> 
           
                <a href="news/news_edit_multi.aspx" onfocus="this.blur()" style="color: Blue; text-decoration: underline; font-size:15px;">
                    新建多图文信息</a> 
        </div>
        <div class="mi-form-item">
            <table border="0" width="640" class="mail-list-title">
                <tr>
                    <td width="6%" align="center" bgcolor="#CCCCCC">
                        编号
                    </td>
                    <td width="6%" align="center" bgcolor="#CCCCCC">
                        封面
                    </td>
                    <td width="13%" align="left" bgcolor="#CCCCCC">
                        标题
                    </td>
                    <td width="5%" align="left" bgcolor="#CCCCCC">
                        作者
                    </td>
                    <td width="10%" bgcolor="#CCCCCC">
                        摘要
                    </td>
                    <td width="5%" bgcolor="#CCCCCC">
                        创建时间
                    </td>
                    <td width="6%" bgcolor="#CCCCCC">
                    </td>
                </tr>
                <tbody id="tblist">
                </tbody>
            </table>
            <div id="divPage">
            </div>
            <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr style="height:46px;" >
                     <td class="sender item">
                           
                               ${Id}  
                        </td>
                        {{each NewsList}}

                        <td class="sender item">
                           
                           <img alt=""   id="headPortraitImg" src="${thumb_url}"  width="50px"  height="30px"/>    
                        </td>
                        <td   class="sender item">
                           
                                 {{if is_singlenews==1}}
                                 单图文
                                {{else}}
                                多图文
                                {{/if}}
                                -${title} 
                             
                        </td>
                        <td   class="sender item">
                         
                                ${author}
                          
                        </td>
                        <td   class="sender item">
                         
                              ${digest}    
                        </td>
                      
                     
                   
                        
                        {{/each}}
                          <td width="4%"   class="sender item">
                           
                                ${jsonDateFormatKaler(CreateTime)} 
                        </td>
                        <td   class="sender item">
                         
                           <a href="javascript:void(0)" onclick="selednews('${Id}','${jsonDateFormatKaler(CreateTime)}','${MediaId}')"  class="a_anniu">选中</a>   
                          
                        </td>
                      
                    </tr>
            </script>
        </div>
    </div>
    <input type="hidden" id="hid_tuwen_recordid" value="<%=tuwen_recordid %>" />
</asp:Content>
