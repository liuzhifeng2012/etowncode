<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="MenuDetail.aspx.cs"
    Inherits="ETS2.WebApp.WeiXin.MenuDetail" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();

            //自动生成链接分类下拉框
            if ($("#hid_whethervertify").val() == "True") {
                $("#sellinkurl").html('<option value="0">请选择</option><option value="1">微&nbsp;商&nbsp;城</option><option value="21">班车页面</option><option value="22">酒店列表</option> <option value="23">图片广告</option><option value="2">会&nbsp;员&nbsp;卡</option> <option value="5">所属门店</option>  <option value="6">幸运抽奖</option> <option value="7">位置导航</option>  <option value="9">抢&nbsp;&nbsp;&nbsp;&nbsp;购</option>  <option value="10">服务顾问</option> <option value="11">自助预约</option> <option value="12">单类产品链接</option> <option value="13">关键词链接</option><option value="14">微分销</option><option value="15">电子凭证</option><option value="16">总部客服</option><option value="17">合作单位列表</option><option value="18">服务顾问-教练员</option>');
            }
            else {
                $("#sellinkurl").html('<option value="0">请选择</option><option value="12">单类产品链接</option> <option value="13">关键词链接</option>');
            }

            //链接地址变动
            $("#sellinkurl").change(function () {
                var ck_val = $("#sellinkurl").trimVal();

                $("#tr_linkurl").show();
                $("#tr_linkprompt").show();
                $("#txtlinkurl").removeAttr("readonly");
                $("#tr_txt").hide();
                $("#tr_choose_pictexttype").hide();
                $("#tr_materialtypeid").hide();
                $("#tr_productclass").hide();
                $("#tr_keyy").hide();
                $("#tr_click").hide();
                $("#sellinkurl_sun").hide();
                var linkurl = "";
                if (ck_val == 1) {//微商城
                    linkurl = "http://shop" + comid + ".etown.cn/h5/order";
                }
                if (ck_val == 21) {//微班车
                    linkurl = "http://shop" + comid + ".etown.cn/h5/order/ShuttleList.aspx";
                }
                if (ck_val == 22) {//微酒店
                    linkurl = "http://shop" + comid + ".etown.cn/h5/order/hotel.aspx";
                }

                if (ck_val == 23) {//图片广告
                    linkurl = "";

                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/WeiXinHandler.ashx?oper=getadlist",
                        data: { comid: comid, pageindex: 1, pagesize: 20 },
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");

                            if (data.type == 1) {
                               // $.prompt("查询错误");
                                return;
                            }
                            if (data.type == 100) {
                                if (data.totalCount == 0) {

                                } else {

                                        $("#sellinkurl_sun").html("");
                                        var groupstr = '<option value="0">请选择图片广告</option>';
                                        for (var i = 0; i < data.totalCount; i++) {
                                            groupstr += '<option value="' + data.msg[i].Id + '">' + data.msg[i].Title + '</option>';
                                        }
                                        $("#sellinkurl_sun").html(groupstr);
                                }

                            }
                        }
                    })

                    $("#sellinkurl_sun").show();
                }


                if (ck_val == 2) {//会员卡
                    linkurl = "http://shop" + comid + ".etown.cn/m/indexcard.aspx";
                }
                if (ck_val == 3) {//微网站
                    linkurl = "http://shop" + comid + ".etown.cn/h5/Default.aspx";
                }
                if (ck_val == 4) {//在线预订
                    linkurl = "http://shop" + comid + ".etown.cn/h5/list.aspx";
                }
                if (ck_val == 5) {//所属门店
                    linkurl = "http://shop" + comid + ".etown.cn/h5/StoreList.aspx";
                }
                if (ck_val == 6) {//幸运抽奖
                    linkurl = "http://shop" + comid + ".etown.cn/m/Choujiang";
                }
                if (ck_val == 7) {//位置导航
                    linkurl = "http://shop" + comid + ".etown.cn/h5/mapinfo.aspx";
                }
                if (ck_val == 8) {//门票预订
                    linkurl = "http://shop" + comid + ".etown.cn/h5/prodefault.aspx";
                }
                if (ck_val == 9) {//抢购
                    linkurl = "http://shop" + comid + ".etown.cn/h5/QiangGou.aspx";
                }
                if (ck_val == 10) {//服务顾问
                    linkurl = "http://shop" + comid + ".etown.cn/weixin/skippage.aspx";
                }
                if (ck_val == 11) {//自助预约
                    linkurl = "http://shop" + comid + ".etown.cn/h5/AutoBespeak.aspx";
                }
                if (ck_val == 12)//单类产品链接
                {
                    $("#tr_linkurl").show();
                    $("#tr_linkprompt").show();
                    $("#txtlinkurl").attr("readonly", "readonly");
                    $("#tr_txt").hide();
                    $("#tr_materialtypeid").hide();
                    $("#tr_choose_pictexttype").hide();
                    $("#tr_productclass").show();
                    $("#tr_keyy").hide();
                    $("#tr_click").hide();
                }
                if (ck_val == 13) {//关键词链接
                    $("#tr_linkurl").show();
                    $("#tr_linkprompt").show();
                    $("#txtlinkurl").attr("readonly", "readonly");
                    $("#tr_txt").hide();
                    $("#tr_materialtypeid").hide();
                    $("#tr_choose_pictexttype").hide();
                    $("#tr_productclass").hide();
                    $("#tr_keyy").show();
                    $("#tr_click").hide();
                }
                if (ck_val == 14) {//微分销
                    //                    linkurl = "http://shop.etown.cn/agent/m";
                    linkurl = "http://shop" + comid + ".etown.cn/agent/m/login.aspx";
                }
                if (ck_val == 15) {//电子凭证 
                    linkurl = "http://shop" + comid + ".etown.cn/m/EticketList.aspx";
                }
                if (ck_val == 16) {//总部客服 
                    linkurl = "http://shop" + comid + ".etown.cn/h5/headoffice_PeopleList.aspx";
                }
                if (ck_val == 17) {//合作单位列表 
                    linkurl = "http://shop" + comid + ".etown.cn/h5/out_StoreList.aspx";
                }
                if (ck_val == 18) {//服务顾问-教练员 
                    linkurl = "http://shop" + comid + ".etown.cn/h5/coachList.aspx";
                }

                if (linkurl != "") {

                    if ($("#hid_whethervertify").val() == "True") {//现阶段只是微信认证的商家可以使用
                        //引导关注者打开如下页面(目前只是要获得openid，其他用户信息暂时不考虑)
                        $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetMenshiLink", { comid: $("#hid_comid").trimVal(), redirect_uri: linkurl }, function (data) {
                            data = eval("(" + data + ")");
                            if (data.type == 1) {
                                //                            $.prompt("获取链接出错");
                                return;
                            }
                            if (data.type == 100) {
                                $("#txtlinkurl").val(data.msg);
                            }
                        })
                    }

                }
            })

            $("#sellinkurl_sun").change(function () {


                var linkurl = "http://shop" + comid + ".etown.cn/h5/ad?id=" + $("#sellinkurl_sun").val();
                  if (linkurl != "") {

                      if ($("#hid_whethervertify").val() == "True") {//现阶段只是微信认证的商家可以使用
                          //引导关注者打开如下页面(目前只是要获得openid，其他用户信息暂时不考虑)
                          $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetMenshiLink", { comid: $("#hid_comid").trimVal(), redirect_uri: linkurl }, function (data) {
                              data = eval("(" + data + ")");
                              if (data.type == 1) {
                                  //                            $.prompt("获取链接出错");
                                  return;
                              }
                              if (data.type == 100) {
                                  $("#txtlinkurl").val(data.msg);
                              }
                          })
                      }

                  }
              })

            //动态获取全部微信素材类型
            $.post("/jsonfactory/WeiXinHandler.ashx?oper=GetAllWxMaterialType", { comid: $("#hid_comid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    //                    $.prompt("操作出现错误" + data.msg);
                    return;
                }
                if (data.type == 100) {
                    if (data.totalCount > 0) {
                        $("#textsalepromotetypeid").html("");

                        var groupstr = '<option value="0">请选择素材类型</option>';
                        for (var i = 0; i < data.totalCount; i++) {
                            groupstr += '<option value="' + data.msg[i].Id + '">' + data.msg[i].TypeName + '</option>';
                        }
                        $("#textsalepromotetypeid").html(groupstr);
                    } else {
                        var groupstr = '<option value="0">请选择素材类型</option>';
                        $("#textsalepromotetypeid").html(groupstr);
                    }
                }
            });

            //图文类型
            $('input[name="choose_pictexttype"]').bind("click", function () {
                var issuetype = $('input:radio[name="choose_pictexttype"]:checked').trimVal();

                if (issuetype == "1") {//图文素材
                    $("#tr_linkurl").hide();
                    $("#tr_linkprompt").hide();
                    $("#txtlinkurl").removeAttr("readonly");
                    $("#tr_txt").hide();
                    $("#tr_choose_pictexttype").show();
                    $("#tr_materialtypeid").show();
                    $("#tr_productclass").hide();
                    $("#tr_keyy").hide();
                    $("#tr_click").hide();
                }
                if (issuetype == "2") {//产品分类素材
                    $("#tr_linkurl").hide();
                    $("#tr_linkprompt").hide();
                    $("#txtlinkurl").removeAttr("readonly");
                    $("#tr_txt").hide();
                    $("#tr_choose_pictexttype").show();
                    $("#tr_materialtypeid").hide();
                    $("#tr_productclass").show();
                    $("#tr_keyy").hide();
                    $("#tr_click").hide();
                }
            })

            //操作类型
            $('input[name="radoprtype"]').bind("click", function () {
                var issuetype = $('input:radio[name="radoprtype"]:checked').trimVal();

                DisplayByOprtype(issuetype);

            })

            //初始化页面
            $("#txtparentmenu").val("");
            $("#txtname").val("");
            $("#txtmenuinstruction").val("");
            $('input[name="radoprtype"][value="2"]').attr("checked", true);
            $("#tr_linkurl").show();
            $("#txtlinkurl").val("");
            $("#tr_linkprompt").show();
            $("#txtwxtext").val("");


            //给页面赋值
            var parentmenuid = $("#hid_parentmenuid").val();
            var parentmenuname = $("#hid_parentmenuname").val();
            var menuid = $("#hid_menuid").val();

            if (parentmenuid != "0")//添加子菜单
            {
                $("#tr_parentmenu").show().find("#txtparentmenu").val(parentmenuname);
                $('input[name="radoprtype"][value="1"]').attr("disabled", "disabled");
            }

            if (menuid != "0")//编辑菜单
            {
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetMenuDetail", { menuid: menuid, comid: $("#hid_comid").trimVal() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("菜单获取出现问题");
                        return;
                    }
                    if (data.type == 100) {

                        $("#txtparentmenu").val(data.msg[0].FatherMenuName);
                        $("#txtname").val(data.msg[0].Name);
                        $("#txtmenuinstruction").val(data.msg[0].Instruction);
                        $("#txtlinkurl").val(data.msg[0].Linkurl);
                        $("#txtwxtext").val(data.msg[0].Wxanswertext);
                        if (data.msg[0].Operationtypeid > 4) {
                            $('input[name="radoprtype"][value="0"]').attr("checked", true);
                            $('input[name="radoprtype1"][value=' + data.msg[0].Operationtypeid + ']').attr("checked", true);
                        }
                        else {
                            $('input[name="radoprtype"][value=' + data.msg[0].Operationtypeid + ']').attr("checked", true);
                        }
                        //图文类型
                        $('input[name="choose_pictexttype"][value=' + data.msg[0].pictexttype + ']').attr("checked", true);

                        $("#textsalepromotetypeid").val(data.msg[0].SalePromoteTypeid);
                        LoadProclass($("#hid_comid").val(), data.msg[0].Product_class);

                        if (data.msg[0].Fathermenuid != 0) {
                            $('input[name="radoprtype"][value="1"]').attr("disabled", "disabled");
                        }
                        $("#txtkeyy").val(data.msg[0].Keyy);

                        DisplayByOprtype(data.msg[0].Operationtypeid);

                        $("#hid_initoprtype").val(data.msg[0].Operationtypeid);
                    }
                });
            }
            else {
                LoadProclass($("#hid_comid").val(), 0);
            }



            //编辑菜单
            $("#aedit").click(function () {
                var name = $("#txtname").trimVal();
                var instruction = $("#txtmenuinstruction").trimVal();
                var oprtype = $('input:radio[name="radoprtype"]:checked').trimVal();
                var oprtype1 = $('input:radio[name="radoprtype1"]:checked').trimVal();
                if (oprtype == 0) {
                    oprtype = oprtype1;
                }
                var linkurl = $("#txtlinkurl").trimVal();
                var txtwxtext = $("#txtwxtext").trimVal();
                var textsalepromotetypeid = $("#textsalepromotetypeid").trimVal();
                var fatherid = $("#hid_parentmenuid").trimVal();
                var product_class = $("#Sel_productclass").trimVal();
                var keyy = $("#txtkeyy").val();
                var choose_pictexttype = $('input:radio[name="choose_pictexttype"]:checked').trimVal();

                if (name == "") {
                    $.prompt("菜单名称不可为空");
                    return;
                }

                if (oprtype == "2") {//链接新页面
                    if (linkurl == "") {
                        $.prompt("链接地址不可为空");
                        return;
                    }
                    txtwxtext = "";
                    textsalepromotetypeid = "0";
                    product_class = "0";
                    keyy = "";
                }
                else if (oprtype == "3") {//回复文本
                    linkurl = "";
                    if (txtwxtext == "") {
                        $.prompt("回复微信文本信息不可为空");
                        return;
                    }
                    textsalepromotetypeid = "0";
                    product_class = "0";
                    keyy = "";
                }
                else if (oprtype == "4") {//回复图文
                    linkurl = "";
                    txtwxtext = "";

                    if (choose_pictexttype == 1)//素材图文
                    {
                        if (textsalepromotetypeid == "0") {
                            $.prompt("请选择素材类型");
                            return;
                        }
                    }
                    if (choose_pictexttype == 2)//单类产品图文
                    {
                        if (product_class == "0") {
                            $.prompt("请选择产品分类");
                            return;
                        }
                    }

                    keyy = "";
                }
                else {
                    linkurl = "";
                    txtwxtext = "";
                    textsalepromotetypeid = "0";
                    product_class = "0";
                    keyy = "";
                }

                var initoprtype = $("#hid_initoprtype").val();
                if ((fatherid == 0) && (oprtype != 1) && (menuid != 0))//如果是弹出子菜单
                {
                    //                    if (confirm("确认操作类型吗？这会导致下级菜单被删除")) {
                    $.post("/JsonFactory/WeiXinHandler.ashx?oper=DelChildrenMenu", { fathermenuid: menuid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt("删除下级菜单出现问题" + data.msg);
                            return;
                        }
                        if (data.type == 100) {

                        }

                    });
                    //                    } else {
                    //                        $.prompt("你取消了操作");
                    //                        return;
                    //                    }
                }

                $.post("/JsonFactory/WeiXinHandler.ashx?oper=EditMenuDetail", { keyy: keyy, product_class: product_class, comid: $("#hid_comid").trimVal(), menuid: menuid, name: name, instruction: instruction, oprtype: oprtype, linkurl: linkurl, txtwxtext: txtwxtext, textsalepromotetypeid: textsalepromotetypeid, fatherid: fatherid, pictexttype: choose_pictexttype }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("编辑菜单出现问题" + data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("编辑菜单成功",
                        { buttons: [{ title: "确定", value: true}],
                            submit: function (e, v, m, f) {
                                if (v == true)
                                    location.href = "menulist.aspx";
                            }
                        });

                    }
                });
            })

            //产品分类类型变动
            $("#Sel_productclass").change(function () {
                var oprtype = $('input:radio[name="radoprtype"]:checked').trimVal();
                var seled = $("#Sel_productclass").val();
                //                if (oprtype == 21)//项目分类(链接页面)
                //                {
                if ($("#hid_whethervertify").val() == "True") {
                    //引导关注者打开如下页面(目前只是要获得openid，其他用户信息暂时不考虑)
                    $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetMenshiLink", { comid: comid, redirect_uri: "http://shop" + comid + ".etown.cn/h5/list_" + seled + ".aspx" }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            //                                $.prompt("获取页面链接出错");
                            return;
                        }
                        if (data.type == 100) {
                            $("#txtlinkurl").val(data.msg);
                        }
                    })
                }
                else {
                    $("#txtlinkurl").val("http://shop" + comid + ".etown.cn/h5/list_" + seled + ".aspx");
                }
                //                }
            })

            //关键词变动
            $("#txtkeyy").keyup(function () {
                var keyy = $("#txtkeyy").trimVal();
                if (keyy != "") {
                    if ($("#hid_whethervertify").val() == "True") {
                        //引导关注者打开如下页面(目前只是要获得openid，其他用户信息暂时不考虑)
                        $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetMenshiLink", { comid: comid, redirect_uri: "http://shop" + comid + ".etown.cn/h5/list_" + keyy + ".aspx" }, function (data) {
                            data = eval("(" + data + ")");
                            if (data.type == 1) {
                                //                                $.prompt("获取页面链接出错");
                                return;
                            }
                            if (data.type == 100) {
                                $("#txtlinkurl").val(data.msg);
                            }
                        })
                    }
                    else {
                        $("#txtlinkurl").val("http://shop" + comid + ".etown.cn/h5/list_" + keyy + ".aspx");
                    }
                }

            })

            //文本编辑器
            $('#txtwxtext').tinymce({
                // Location of TinyMCE script
                script_url: '/Scripts/tiny_mce/tiny_mce.js',
                width: '522',
                height: '120',
                // General options
                theme: "advanced",
                language: 'cn',
                plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,advlist",

                // Theme options
                theme_advanced_buttons1: "",
                //                theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,fontselect,fontsizeselect,|,forecolor,backcolor,|,insertdate,image,preview",
                theme_advanced_buttons2: "",
                theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,ltr,rtl",
                // theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,|,nonbreaking,template,pagebreak",
                theme_advanced_toolbar_location: "top",
                theme_advanced_toolbar_align: "left",
                theme_advanced_statusbar_location: "bottom",
                //theme_advanced_resizing: true,
                template_external_list_url: "lists/template_list.js",
                external_link_list_url: "lists/link_list.js",
                external_image_list_url: "lists/image_list.js",
                media_external_list_url: "lists/media_list.js",

                // Replace values for the template plugin
                template_replace_values: {
                    username: "Some User",
                    staffid: "991234"
                }
            });
        })

        //加载产品分类
        function LoadProclass(comid, proclass) {
            //加载类目
            $.ajax({
                type: "post",
                url: "/JsonFactory/ProductHandler.ashx?oper=proclasslist",
                data: { pageindex: 1, pagesize: 200, industryid: $("#hid_industryid").trimVal() },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    $("#Sel_productclass").empty();

                    if (data.type == 1) {
                        return;
                    }
                    if (data.type == 100) {
                        if (data.totalCount > 0) {
                            $("#Sel_productclass").append("<option value='0' >请选择所属类目</option>");
                            for (i = 0; i < data.totalCount; i++) {
                                if (data.msg[i].Id == proclass) {
                                    $("#Sel_productclass").append("<option value='" + data.msg[i].Id + "' selected='selected'>" + data.msg[i].Classname + "</option>");
                                } else {
                                    $("#Sel_productclass").append("<option value='" + data.msg[i].Id + "'>" + data.msg[i].Classname + "</option>");
                                }
                            }
                        }
                        else {
                            $("#Sel_productclass").html("<option value='0' >请选择所属类目</option>");
                        }
                    }
                }
            })

        }

        //根据操作类型决定显示隐藏
        function DisplayByOprtype(issuetype) {
            if (issuetype == 1) {//弹出子菜单
                $("#tr_linkurl").hide();
                $("#tr_linkprompt").hide();
                $("#txtlinkurl").removeAttr("readonly");
                $("#tr_txt").hide();
                $("#tr_choose_pictexttype").hide();
                $("#tr_materialtypeid").hide();
                $("#tr_productclass").hide();
                $("#tr_keyy").hide();
                $("#tr_click").hide();
            }
            else if (issuetype == 2) {//链接新页面
                $("#tr_linkurl").show();
                $("#tr_linkprompt").show();
                $("#txtlinkurl").removeAttr("readonly");
                $("#tr_txt").hide();
                $("#tr_choose_pictexttype").hide();
                $("#tr_materialtypeid").hide();
                $("#tr_productclass").hide();
                $("#tr_keyy").hide();
                $("#tr_click").hide();
            }
            else if (issuetype == 3) {//回复微信文本信息
                $("#tr_linkurl").hide();
                $("#tr_linkprompt").hide();
                $("#txtlinkurl").removeAttr("readonly");
                $("#tr_txt").show();
                $("#tr_choose_pictexttype").hide();
                $("#tr_materialtypeid").hide();
                $("#tr_productclass").hide();
                $("#tr_keyy").hide();
                $("#tr_click").hide();
            }
            else if (issuetype == 4) {//回复微信图文信息
                $("#tr_linkurl").hide();
                $("#tr_linkprompt").hide();
                $("#txtlinkurl").removeAttr("readonly");
                $("#tr_txt").hide();
                $("#tr_choose_pictexttype").show();
                $("#tr_materialtypeid").hide();
                $("#tr_productclass").hide();
                $("#tr_keyy").hide();
                $("#tr_click").hide();
                var pictexttype = $('input:radio[name="choose_pictexttype"]:checked').trimVal();
                if (pictexttype == 1) {
                    $("#tr_materialtypeid").show();
                }
                if (pictexttype == 2) {
                    $("#tr_productclass").show();
                }
            }
            else {
                $("#tr_linkurl").hide();
                $("#tr_linkprompt").hide();
                $("#txtlinkurl").removeAttr("readonly");
                $("#tr_txt").hide();
                $("#tr_choose_pictexttype").hide();
                $("#tr_materialtypeid").hide();
                $("#tr_productclass").hide();
                $("#tr_keyy").hide();
                $("#tr_click").show();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/weixin/ShangJiaSet2.aspx" onfocus="this.blur()"><span>微信接口设置</span></a></li>
                <li><a href="/weixin/WxDefaultReply.aspx" onfocus="this.blur()"><span>微信默认设置</span></a></li>
                <li class="on"><a href="/weixin/menulist.aspx" onfocus="this.blur()"><span>自定义菜单管理</span></a></li>
                <li><a href="/MemberShipCard/MemberShipCardList.aspx" onfocus="this.blur()"><span>会员卡专区管理</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    编辑微信菜单</h3>
                <h3>
                    &nbsp;</h3>
                <table class="grid">
                    <tr id="tr_parentmenu" style="display: none;">
                        <td width="19%" class="tdHead">
                            父菜单：
                        </td>
                        <td width="81%">
                            <input type="text" id="txtparentmenu" value="" size="50" readonly />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            菜单名称：
                        </td>
                        <td width="81%">
                            <input type="text" id="txtname" value="" size="50" />*
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            菜单说明（选填）：
                        </td>
                        <td>
                            <textarea id="txtmenuinstruction" cols="100" rows="3"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            操作类型：
                        </td>
                        <td>
                            <label>
                                <input name="radoprtype" type="radio" value="1" checked>
                                弹出子菜单</label>
                            <label>
                                <input name="radoprtype" type="radio" value="2">
                                链接新页面</label>
                            <label>
                                <input name="radoprtype" type="radio" value="3">
                                回复文本消息</label>
                            <label>
                                <input name="radoprtype" type="radio" value="4">
                                回复图文消息</label>
                            <label>
                                <input name="radoprtype" type="radio" value="0">
                                微信菜单点击事件</label>
                        </td>
                    </tr>
                    <tr style="display: none" id="tr_linkurl">
                        <td class="tdHead">
                        </td>
                        <td>
                            <input type="text" id="txtlinkurl" value="" size="100" /><br />
                        </td>
                    </tr>
                    <tr style="display: none" id="tr_linkprompt">
                        <td class="tdHead">
                        </td>
                        <td>
                            <label>
                                可在后方选择自动生成，同时也可自己填写</label>
                            <select id="sellinkurl"> 
                            </select>

                            <select id="sellinkurl_sun" style=" display:none;"> 
                            </select>
                        </td>
                    </tr>
                    <tr style="display: none" id="tr_txt">
                        <td class="tdHead">
                            文本信息内容：
                        </td>
                        <td>
                            <textarea cols="100" rows="10" id="txtwxtext"></textarea>
                        </td>
                    </tr>
                    <tr style="display: none" id="tr_choose_pictexttype">
                        <td class="tdHead">
                            图文类型：
                        </td>
                        <td>
                            <label>
                                <input name="choose_pictexttype" type="radio" value="1" checked>
                                素材图文</label>
                            <label>
                                <input name="choose_pictexttype" type="radio" value="2">
                                单类产品图文</label>
                        </td>
                    </tr>
                    <tr style="display: none" id="tr_materialtypeid">
                        <td class="tdHead">
                            回复素材类型：
                        </td>
                        <td>
                            <select id="textsalepromotetypeid">
                            </select>
                        </td>
                    </tr>
                    <tr style="display: none" id="tr_productclass">
                        <td class="tdHead">
                            产品分类类型：
                        </td>
                        <td>
                            <select id="Sel_productclass">
                            </select>
                        </td>
                    </tr>
                    <tr style="display: none" id="tr_keyy">
                        <td class="tdHead">
                            关键词：
                        </td>
                        <td>
                            <input type="text" id="txtkeyy" value="" />
                        </td>
                    </tr>
                    <tr style="display: none" id="tr_click">
                        <td class="tdHead">
                            菜单点击事件：
                        </td>
                        <td>
                            <%-- <label>
                                <input name="radoprtype" type="radio" value="18">
                                产品分类(回复图文)</label>
                            <label>
                                <input name="radoprtype" type="radio" value="21">
                                产品分类(链接页面)</label>
                            <label>
                                <input name="radoprtype" type="radio" value="22">
                                关键词产品(链接页面)</label>--%>
                            <label>
                                <input name="radoprtype1" type="radio" value="29">
                                微商城</label>
                            <label>
                                <input name="radoprtype1" type="radio" value="16">
                                微网站</label>
                            <label>
                                <input name="radoprtype1" type="radio" value="19">
                                景区门票</label>
                            <label>
                                <input name="radoprtype1" type="radio" value="8">
                                在线票务预订</label>
                            <label>
                                <input name="radoprtype1" type="radio" value="11">
                                在线酒店预订</label>
                            <label>
                                <input name="radoprtype1" type="radio" value="20">
                                抢 购</label>
                            <br />
                            <label>
                                <input name="radoprtype1" type="radio" value="5">
                                会员信息</label>
                            <label>
                                <input name="radoprtype1" type="radio" value="14">
                                所属门市</label>
                            <label>
                                <input name="radoprtype1" type="radio" value="10">
                                电子凭证信息</label>
                            <label>
                                <input name="radoprtype1" type="radio" value="6">
                                优惠劵信息</label>
                            <label>
                                <input name="radoprtype1" type="radio" value="12">
                                大抽奖</label>
                            <label>
                                <input name="radoprtype1" type="radio" value="9">
                                关联手机</label>
                            <br />
                            <label>
                                <input name="radoprtype1" type="radio" value="27">
                                我的服务顾问(回复链接)</label>
                            <label>
                                <input name="radoprtype1" type="radio" value="13">
                                我要咨询</label>
                            <label>
                                <input name="radoprtype1" type="radio" value="28">
                                微咨询(自动分配顾问)</label>
                            <br />
                            <label>
                                <input name="radoprtype1" type="radio" value="17">
                                位置导航</label>
                            <label>
                                <input name="radoprtype1" type="radio" value="23">
                                扫码推事件</label>
                            <label>
                                <input name="radoprtype1" type="radio" value="24">
                                弹出拍照或者相册发图</label>
                            <label>
                                <input name="radoprtype1" type="radio" value="25">
                                弹出地理位置选择器</label>
                                <br />
                                <label>
                             <input name="radoprtype1" type="radio" value="30">
                                微分销</label>
                                <label>
                            
                               
                        </td>
                    </tr>
                </table>
                <p>
                    &nbsp;</p>
                <p>
                    &nbsp;</p>
                <p align="center">
                    <a href="javascript:void(0)" id="aedit" class="font_14"><strong>完成添加，确认提交</strong></a></p>
                <p>
                    &nbsp;</p>
                <h3>
                    &nbsp;</h3>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_parentmenuid" value="<%=fathermenuid %>" />
    <input type="hidden" id="hid_parentmenuname" value="<%=fathermenuname %>" />
    <input type="hidden" id="hid_menuid" value="<%=menuid %>" />
    <input type="hidden" id="hid_initoprtype" value="" />
    <input type="hidden" id="hid_whethervertify" value="<%=whethervertiry %>" />
    <input type="hidden" id="hid_industryid" value="<%=industryid %>" />
</asp:Content>
