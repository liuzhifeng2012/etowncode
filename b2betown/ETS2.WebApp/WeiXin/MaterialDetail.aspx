<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="MaterialDetail.aspx.cs"
    Inherits="ETS2.WebApp.WeiXin.MaterialDetail" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            var comid = $("#hid_comid").val();
            //动态获取全部微信素材类型
            $.post("/jsonfactory/WeiXinHandler.ashx?oper=GetAllWxMaterialType", { comid: comid }, function (data) {

                data = eval("(" + data + ")");

                if (data.type == 1) {
                    $.prompt("操作出现错误" + data.msg);
                    return;
                }
                if (data.type == 100) {

                    if (data.totalCount > 0) {
                        $("#tdgroups").html("");

                        var groupstr = "";
                        for (var i = 0; i < data.totalCount; i++) {
                            if (data.msg[i].Id == $("#hid_promotetypeid").trimVal()) {
                                groupstr += '<label><input name="radpromotetype"  type="radio" value="' + data.msg[i].Id + '" checked="checked">' + data.msg[i].TypeName + '</label>';

                            } else {
                                groupstr += '<label><input name="radpromotetype"  type="radio" value="' + data.msg[i].Id + '">' + data.msg[i].TypeName + '</label>';
                            }
                        }
                        $("#tdgroups").html(groupstr);

                        InitMaterial();

                    }

                }
            });


            //日历
            var nowdate = '<%=this.nowdate %>';
            var monthdate = '<%=this.monthdate %>';
            var dateinput = $("input[isdate=yes]");
            $("#Actstar").val(nowdate);
            $("#Actend").val(monthdate);
            $.each(dateinput, function (i, item) {

                //                $(item).val(nowdate);
                $($(this)).datepicker({
                    numberOfMonths: 1,
                    minDate: 0,
                    //                    defaultDate: +4,
                    maxDate: '+8m +1w'
                });
            });

            function InitMaterial() {

                var materialid = $("#hid_materialid").trimVal();
                //判断是修改操作还是添加素材操作
                if (materialid != 0) {//修改操作;需要加载素材信息

                    $("#periodicaladd").attr("disabled", "disabled");

                    $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetWxMaterial", { comid: $("#hid_comid").trimVal(), materialid: materialid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            $("#txttitle").val(data.msg.Title);
                            $("#txtauthor").val(data.msg.Author);

                            $("#txtauthorpayurl").val(data.msg.Authorpayurl);

                            $("#hid_logo").val(data.msg.Imgpath);
                            $("#txtsummary").val(data.msg.Summary);
                            $("#txtcontent").val(data.msg.Article);
                            $("#txturl").val(data.msg.Articleurl);
                            $("#txtkeyword").val(data.msg.Keyword);
                            $("#txtphone").val(data.msg.Phone);
                            $("#txtprice").val(data.msg.Price);
                            $('input[name="radpromotetype"][value=' + data.msg.SalePromoteTypeid + ']').attr("checked", true);

                            $("#Actstar").val(ChangeDateFormat(data.msg.Staticdate));
                            $("#Actend").val(ChangeDateFormat(data.msg.Enddate));

                            $("#periodical").html(data.per.Percal); //期号
                            $("#Year").html(data.per.Peryear); //年号
                            $("#hid_percalid").val(data.per.Id); //期标识列id

                            $('input[name="radApplyState"][value=' + data.msg.Applystate + ']').attr("checked", true);

                            //根据素材类型id 判断素材类型的分类(a.详情b.预订)
                            $.post("/JsonFactory/WeiXinHandler.ashx?oper=getmaterialtype", { id: data.msg.SalePromoteTypeid, comid: $("#hid_comid").trimVal() }, function (data11) {
                                data11 = eval("(" + data11 + ")");
                                if (data11.type == 1) {

                                    return;
                                }
                                if (data11.type == 100) {

                                    var class1 = data11.msg.Typeclass;
                                    if (class1 == "book") {
                                        $("#tdOriginalurl").html("添加预订链接（选填）：");
                                    } else {
                                        $("#tdOriginalurl").html("添加原文链接（选填）：");
                                    }

                                }
                            })

                        } else {
                            $.prompt("获取微信素材信息出错");
                            return;
                        }
                    });
                }
                else {
                    //作者名称 与 客服电话，自动默认为 商户公司名称，电话
                    $.post("/JsonFactory/AccountInfo.ashx?oper=getcurcompany", { comid: $("#hid_comid").val() }, function (dat) {
                        dat = eval("(" + dat + ")");
                        if (dat.type == 1) {

                        }
                        if (dat.type == 100) {
                            $("#txtauthor").val(dat.msg.B2bcompanyinfo.Wxfocus_author);
                            $("#txtphone").val(dat.msg.B2bcompanyinfo.Tel);
                            $("#txtauthorpayurl").val(dat.msg.B2bcompanyinfo.Wxfocus_url);
                        }
                    })

                    //添加素材，对期的处理
                    var promotetype = $("#hid_promotetypeid").trimVal();

                    if (promotetype == "0" || promotetype == "" || promotetype == undefined) {//如果没有产品类型不进行操作

                    } else {//有产品类型，根据产品类型和公司id得到期
                        $.post("/JsonFactory/WeiXinHandler.ashx?oper=selecttypeid", { comid: $("#hid_comid").trimVal(), promotetypeid: promotetype }, function (data) {
                            data = eval("(" + data + ")");
                            if (data.type == 100) {
                                $("#periodical").html(data.msg.Percal); //期号
                                $("#Year").html(data.msg.Peryear); //年号
                                $("#hid_percalid").val(data.msg.Id); //期标识列id
                            }
                            if (data.type == 1) {
                                $.prompt("获取微信素材期信息出错");
                                return;
                            }
                        })
                    }

                }
            }



            $('input[name="radpromotetype"]').live("click", function () {
                var promotetypeid = $('input:radio[name="radpromotetype"]:checked').trimVal();
                if (promotetypeid != 0) {
                    $.post("/JsonFactory/WeiXinHandler.ashx?oper=selecttypeid", { comid: comid, promotetypeid: promotetypeid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            //添加一个新操作，第一次发布文章自动到第一期
                            //                            if (data.msg == null) {
                            //                                $("#periodical").html(0); //期号
                            //                                $("#Year").html(0); //年号
                            //                                $("#hid_percalid").val(0); //期标识列id
                            //                                return;
                            //                            }
                            $("#periodical").html(data.msg.Percal); //期号
                            $("#Year").html(data.msg.Peryear); //年号
                            $("#hid_percalid").val(data.msg.Id); //期标识列id
                            return;
                        }
                        else {
                            $.prompt("查询错误,请联系管理员");
                            return;
                        }
                    })
                }
            });

            $("#periodicaladd").click(function () { //点击添加期号

                var periodical = $("#periodical").html();
                var promotetypeid = $('input:radio[name="radpromotetype"]:checked').trimVal();

                if (confirm("确认添加下一期吗?")) {
                    if (promotetypeid != 0 && promotetypeid != "" && promotetypeid != undefined) {
                        $.post("/JsonFactory/WeiXinHandler.ashx?oper=IsCanAddperiod", { comid: comid, promotetypeid: promotetypeid, periodical: periodical }, function (data) {
                            data = eval("(" + data + ")");
                            if (data.type == 100) {

                                $("#periodical").html(data.Percal); //期号
                                $("#Year").html(data.Peryear); //年号
                                $("#hid_isaddpercal").val(1); //是否添加了期：0否；1是

                                return;

                            }
                            else {
                                alert(data.msg);
                                //                                $.prompt("添加下一期错误");
                                return;
                            }
                        })
                    }
                    else {
                        $.prompt("请选择类型");
                        return;
                    }

                }
            })


            $("#aedit").click(function () {
                var materialid = $("#hid_materialid").trimVal();
                var txttitle = $("#txttitle").trimVal();
                var txtauthor = $("#txtauthor").trimVal();
                var logo = $("#<%=headPortrait.FileUploadId_ClientId %>").val();
                var txtsummary = $("#txtsummary").trimVal();
                var txtcontent = $("#txtcontent").trimVal();
                var txturl = $("#txturl").trimVal();
                var txtkeyword = $("#txtkeyword").trimVal();
                var applystate = $('input:radio[name="radApplyState"]:checked').trimVal();

                var txtphone = $("#txtphone").trimVal();
                var txtprice = $("#txtprice").trimVal();
                var promotetype = $('input:radio[name="radpromotetype"]:checked').trimVal();

                var Actstar = $("#Actstar").val();
                var Actend = $("#Actend").val();



                if (promotetype == "") {
                    $.prompt("产品类型不可为空");
                    return;
                }


                if (Actstar == null || Actend == null || Actstar == "" || Actend == "") {
                    $.prompt("有效日期不可为空");
                    return;
                }

                //                if (txtphone == "") {
                //                    $.prompt("电话不可为空");
                //                    return;
                //                }
                //                else {
                //                    if (!$("#txtphone").checkMobile() && !$("#txtphone").checkTel()) {
                //                        $.prompt("请正确填写电话号码，例如:13415764179或0321-4816048");
                //                        return;
                //                    }
                //                }



                if (txttitle == "") {
                    $.prompt("请输入素材的标题！");
                    return;
                }
                if (logo == "") {
                    if (logo == "") {
                        logo = $("#hid_logo").trimVal();
                    }
                }
                //                if (txtsummary == "") {
                //                    $.prompt("请输入摘要");
                //                    return;
                //                }
                if (txtcontent == "") {
                    $.prompt("请输入正文");
                    return;
                }
                //                if (txturl == "") {
                //                    $.prompt("请输入原文链接");
                //                    return;
                //                }
                if (txturl != "") {
                    if (!$("#txturl").CheckUri()) {
                        $.prompt("原文链接格式不正确");
                        return;
                    }
                }
                if (txtkeyword == "") {
                    $.prompt("请输入搜索的关键词语");
                    return;
                }

                if (isNaN($("#txtprice").trimVal())) {
                    $.prompt("价格格式不正确");
                    return;
                }

                var periodical = $("#periodical").html();

                var isaddpercal = $("#hid_isaddpercal").trimVal();
                if (isaddpercal == 1) {//添加了期
                    periodical = parseInt(periodical) - 1;
                    $.post("/JsonFactory/WeiXinHandler.ashx?oper=Addperiod", { comid: comid, promotetypeid: promotetype, periodical: periodical }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            $("#hid_percalid").val(data.msg.Id); //期标识列id

                            $.post("/JsonFactory/WeiXinHandler.ashx?oper=EditWxMaterial", { authorpayurl: $("#txtauthorpayurl").trimVal(), comid: comid, periodicalid: data.msg.Id, phone: txtphone, price: txtprice, promotetype: promotetype, materialid: materialid, title: txttitle, author: txtauthor, imgurl: logo, summary: txtsummary, content: txtcontent, articleurl: txturl, keywords: txtkeyword, applystate: applystate, Actstar: Actstar, Actend: Actend }, function (data) {
                                data = eval("(" + data + ")");
                                if (data.type == '100') {
                                    $.prompt("编辑微信素材信息成功", {
                                        buttons: [{ title: '确定', value: true}],
                                        opacity: 0.1,
                                        focus: 0,
                                        show: 'slideDown',
                                        submit: function (e, v, m, f) {
                                            if (v == true)
                                                location.href = "materiallist.aspx";
                                        }
                                    });
                                } else {
                                    $.prompt("编辑微信素材信息出错");
                                    return;
                                }
                            });
                        }
                        else {
                            alert(data.msg);
                            //                                $.prompt("添加下一期错误");
                            return;
                        }
                    })
                } else {  
                var periodicalid = $("#hid_percalid").val();
                if (periodicalid == "" || periodicalid == "0") {
                    $.prompt("素材发布的期不可为空");
                    return;
                }

                $.post("/JsonFactory/WeiXinHandler.ashx?oper=EditWxMaterial", { authorpayurl: $("#txtauthorpayurl").trimVal(), comid: comid, periodicalid: periodicalid, phone: txtphone, price: txtprice, promotetype: promotetype, materialid: materialid, title: txttitle, author: txtauthor, imgurl: logo, summary: txtsummary, content: txtcontent, articleurl: txturl, keywords: txtkeyword, applystate: applystate, Actstar: Actstar, Actend: Actend }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == '100') {
                        $.prompt("编辑微信素材信息成功", {
                            buttons: [{ title: '确定', value: true}],
                            opacity: 0.1,
                            focus: 0,
                            show: 'slideDown',
                            submit: function (e, v, m, f) {
                                if (v == true)
                                    location.href = "materiallist.aspx";
                            }
                        });
                    } else {
                        $.prompt("编辑微信素材信息出错");
                        return;
                    }
                });
                }

            })


            bindViewImg();

            function bindViewImg() {
                var defaultPath = "";
                var imgSrc = '<%=headPortraitImgSrc %>';
                if (imgSrc == "") {
                    //                    $(".headPortraitImg").attr("src", defaultPath);
                } else {
                    var filePath = '<%=headPortrait.fileUrl %>';
                    var headlogoImgSrc = filePath + imgSrc;
                    $("#headPortraitImg").attr("src", headlogoImgSrc);
                }
            }
            $('#txtcontent').tinymce({
                // Location of TinyMCE script
                script_url: '/Scripts/tiny_mce/tiny_mce.js',
                width: '422',
                height: '320',
                // General options
                theme: "advanced",
                language: 'cn',
                plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,advlist",

                // Theme options
                theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,fontselect,fontsizeselect,|,forecolor,backcolor,|,insertdate,image,preview,code",
                theme_advanced_buttons2: "",
                theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,ltr,rtl",
                theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,|,nonbreaking,template,pagebreak",
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
       
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="materiallist.aspx" onfocus="this.blur()"><span>文章列表</span></a></li>

                <li><a href="AuthorFocus.aspx" onfocus="this.blur()"><span>关注作者管理</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                </h3>
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        编辑微信素材</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            素材类型</label>
                        单图文信息
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            产品类型</label>
                        <span id="tdgroups"></span>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            期号</label>
                        年：<span id="Year"></span> 期号：第<span id="periodical"> </span>期
                        <input id="periodicaladd" type="button" value="发布下一期" /><span style="color: Red">点击增加后将不能修改期号</span>*
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            标题</label>
                        <input type="text" id="txttitle" value="" size="50" class="mi-input" />*
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            作者（选填）</label>
                        <input type="text" id="txtauthor" value="" size="50" class="mi-input" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            关注链接（选填）</label>
                        <input type="text" id="txtauthorpayurl" value="" size="50" class="mi-input" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            封面图</label>
                        <dl>
                            <dt>
                                <input type="hidden" id="hid_logo" value="" />
                                <img alt="" class="headPortraitImgSrc" id="headPortraitImg" src="/images/defaultThumb.png" /></dt>
                            <dd>
                                图片格式为jpg、gif、png，建议尺寸：720像素 * 400像素封面</dd>
                        </dl>
                        <div class="cl">
                        </div>
                        <uc1:uploadFile ID="headPortrait" runat="server" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            添加摘要</label>
                        <textarea id="txtsummary" rows="3" cols="114" class="mi-input" style="width: 500px;"></textarea>*
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            正文</label>
                        <textarea id="txtcontent" rows="8" cols="114" class="mi-input" style="width: 500px;"></textarea>*
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            添加原文链接（选填）</label>
                        <input type="text" id="txturl" value="" size="114" class="mi-input" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            添加关键词语：(必须以中文逗号隔开)</label>
                        <input type="text" id="txtkeyword" value="" size="114" class="mi-input" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            使用状态</label>
                        <input name="radApplyState" type="radio" value="1" checked>
                        使用
                        <input name="radApplyState" type="radio" value="0">
                        暂停
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            有效期</label>
                        开始
                        <input name="Actstar" type="text" id="Actstar" size="12" isdate="yes" class="mi-input" />
                        截止
                        <input name="Actend" type="text" id="Actend" size="12" isdate="yes" class="mi-input" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            商户电话(选填)</label>
                        <input type="text" id="txtphone" value="" size="40" class="mi-input" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            产品价格</label>
                        <input type="text" id="txtprice" value="" size="40" class="mi-input" />
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <table width="780">
                    <tr>
                        <td align="center">
                            <a href="javascript:void(0)" id="aedit" class="font_14"><strong>完成添加，确认提交</strong></a>
                        </td>
                    </tr>
                </table>
                <p>
                    <br>
                </p>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_materialid" value="<%=materialid %>" />
    <input type="hidden" id="hid_promotetypeid" value="<%=promotetypeid %>" />
    <input type="hidden" id="hid_percalid" value="" />
    <!-是否增加了期-!>
    <input type="hidden" id="hid_isaddpercal" value="0" />
</asp:Content>
