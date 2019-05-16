<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="B2bCrm_LevelManage.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.UI.CrmUI.B2bCrm_LevelManage" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            var comid = $("#hid_comid").trimVal();
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=getb2bcrmlevels", { comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                }
                if (data.type == 100) {
                    $("#tblist").empty();
                    if (data.totalcount == 0) {
                    } else {
                        var levelids = "";
                        for (var i = 0; i < data.totalcount; i++) {
                            if (data.msg[i].id != "") {
                                levelids += data.msg[i].id + ",";
                            }
                        }
                        levelids = levelids.substr(0, levelids.length - 1);

                        $("#hid_levelids").val(levelids);
                        $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");

                        $('textarea').tinymce({
                            // Location of TinyMCE script
                            script_url: '/Scripts/tiny_mce/tiny_mce.js',
                            width: '272',
                            height: '80',
                            // General options
                            theme: "advanced",
                            language: 'cn',
                            plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,advlist",

                            // Theme options
                            theme_advanced_buttons1: "",
                            theme_advanced_buttons2: "",
                            theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,ltr,rtl",
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
                    }
                }
            })


            $("#GoNext").click(function () {
                var levelids = $("#hid_levelids").val();
                var crmlevels = "";
                var levelnames = "";
                var dengjifens = "";
                var tequans = "";
                var isavailables = "";

                var arr = levelids.split(',');
                for (var i = 0; i < arr.length; i++) {
                    var id = arr[i];
                    crmlevels += $("#crmlevel" + id).html() + ",";
                    levelnames += $("#levelname" + id).val() + ",";
                    tequans += $("#tequan" + id).val() + ",";
                    isavailables += $("#isavailable" + id).val() + ",";
                    if ($("#isavailable" + id).val() == 1) {
                        dengjifens += $("#dengjifen" + id).val() + ",";
                    } else {
                        dengjifens += "0-0" + ",";
                    }
                }
                //判断等级分规则a.判断累计数是否含有“无上限”,不含有提示"会员所需等级分最后一项设置不正确，形式应为：区间最小值-无上限" b.判断累计数高级别区间最小值-低级别区间最大值必须为1 c.判断会员最高级别之前(包括最高级别)的启用状态必须都启用，其他级别需要停用；

                //查询等级分第一个"无上限"项
                var arrdengjifens = dengjifens.substr(0, dengjifens.length - 1).split(',');

                var firstnohighline_No = 0;
                for (var i = 0; i < 4; i++) {
                    var dengjifen_begin = arrdengjifens[i].substr(0, arrdengjifens[i].indexOf('-'));
                    var dengjifen_end = arrdengjifens[i].substr(arrdengjifens[i].indexOf('-') + 1);

                    if (dengjifen_begin != "无上限" && dengjifen_end == "无上限") {
                        firstnohighline_No = i;
                        break;
                    }
                }
                if (firstnohighline_No == 0) {
                    alert("累计数输入规则应为:区间最小数-无上限");
                    return;
                }

                //判断a,b
                for (var i = firstnohighline_No; i > 0; i--) {
                    //当前等级分区间的最小值
                    var now_dengjifen_begin = arrdengjifens[i].substr(0, arrdengjifens[i].indexOf('-'));
                    //前一级等级分区间的最大值
                    var pre_dengjifen_end = arrdengjifens[i - 1].substr(arrdengjifens[i - 1].indexOf('-') + 1);
                   
                    if (now_dengjifen_begin - pre_dengjifen_end != 1) {
                        alert("累计数高级别区间最小值-低级别区间最大值必须为1");
                        break;
                        return;
                    }
                }
                //判断c
                var arrisavailables = isavailables.substr(0, isavailables.length - 1).split(',');
                for (var i = firstnohighline_No; i >= 0; i--) {
                    if (arrisavailables[i] == 0) {
                        alert("应用的会员级别的启用状态应该都启用");
                        break;
                        return;
                    }
                }
                for (var i = firstnohighline_No + 1; i <= 4; i++) {
                    if (arrisavailables[i] == 1) {
                        alert("停用的会员级别的启用状态应该都停用");
                        break;
                        return;
                    }
                }


                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=editb2bcrmlevels", { comid: $("#hid_comid").val(), levelids: levelids, crmlevels: crmlevels, levelnames: levelnames, dengjifens: dengjifens, tequans: tequans, isavailables: isavailables }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) { }
                    if (data.type == 100) {
                        alert(data.msg);
                        return;
                    }
                })
            })

        })
        function adjustisavailable(id) {
            var isavailable = $("#isavailable" + id).val();
            if (isavailable == 0) {
                $("#lblmsg" + id).html("已启用");
                $("#a_" + id).html("停用");
                $("#isavailable" + id).val(1);
            }
            else {
                $("#lblmsg" + id).html("已停用");
                $("#a_" + id).html("启用");
                $("#isavailable" + id).val(0);

                $("#levelname" + id).val("");
                $("#dengjifen" + id).val("");
                $("#tequan" + id).val("");
            }
        }
    </script>
    <style type="text/css">
        .mi-input
        {
            vertical-align: middle;
        }
        .mi-label
        {
            margin: 0px;
            text-align: center;
            padding-bottom: 1px;
        }
        .mi-form-item
        {
            padding: 0;
        }
        
        /*按钮*/
        a:link, a:visited
        {
            color: #666;
            text-decoration: none;
        }
        .a_anniu
        {
            background: none repeat scroll 0 0 #ccc;
            border: 2px outset #eee;
            color: #000;
            font-size: 12px;
            height: 25px;
            line-height: 25px;
            margin: 5px 0;
            padding: 2px 15px;
            text-align: center;
            text-decoration: none;
        }
        /*编辑器*/
        .defaultSkin .mceLeft
        {
            display: none;
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting">
            <ul>
                <li><a href="/ui/crmui/BusinessCustomersList.aspx" onfocus="this.blur()">会员列表</a></li>
                <li class="on"><a href="/ui/crmui/B2bCrm_LevelManage.aspx" onfocus="this.blur()">会员级别设置</a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10; width: 860px;">
                    <h2 class="p-title-area">
                        会员级别设置</h2>
                    <table border="0" style="margin: 2px 25px;">
                        <tr>
                            <td style="width: 50px;">
                                <label class="mi-label">
                                    级别
                                </label>
                            </td>
                            <td style="width: 180px;">
                                <label class="mi-label">
                                    名称
                                </label>
                            </td>
                            <td style="width: 180px;">
                                <label class="mi-label">
                                    所需积分累计数
                                </label>
                            </td>
                            <td style="width: 280px;">
                                <label class="mi-label">
                                    级别特权
                                </label>
                            </td>
                            <td>
                                <label class="mi-label">
                                    启用状态
                                </label>
                            </td>
                        </tr>
                        <tbody id="tblist">
                        </tbody>
                        <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                        <tr style=" background:#DDDDDD;border:1px solid #FFFFFF;">
                            <td>
                                <label class="mi-label" id="crmlevel${id}">
                                    ${crmlevel}
                                </label>
                            </td>
                            <td>
                                <label class="mi-label">
                                    <input id="levelname${id}" type="text" value="${levelname}"  class="mi-input" style="width: 150px;" />
                                </label>
                            </td>
                            <td>
                                <label class="mi-label">
                                   {{if dengjifen_begin==0&&dengjifen_end==0}}
                                    <input id="dengjifen${id}" type="text" value="" class="mi-input" style="width: 150px;" />


                                     {{else}}
                                        {{if dengjifen_begin>=0}}


                                            {{if dengjifen_end==1000000000}}
                                          <input id="dengjifen${id}" type="text" value="${dengjifen_begin}-无上限" class="mi-input" style="width: 150px;" />
                                             {{else}}
                                              <input id="dengjifen${id}" type="text" value="${dengjifen_begin}-${dengjifen_end}" class="mi-input" style="width: 150px;" />
                                            {{/if}}


                                         {{else}}
                                          <input id="dengjifen${id}" type="text" value="" class="mi-input" style="width: 150px;" />
                                          {{/if}}



                                    {{/if}}
                                </label>
                            </td>
                            <td>
                                <label class="mi-label">
                                    <textarea id="tequan${id}" rows="2"   > ${tequan}</textarea>
                               </label>
                            </td>
                            <td>
                                  <label class="mi-label" id="lblmsg${id}">
                                       {{if isavailable==1}} 已启用{{else}}已停用{{/if}} 
                                  </label>
                                  <a href="javascript:;" class="a_anniu" onclick="adjustisavailable(${id})" id="a_${id}">
                                       {{if isavailable==1}}停用{{else}}启用{{/if}}
                                  </a>
                            <input type="hidden" id="isavailable${id}" value="${isavailable}">
                            </td>
                        </tr>
                        </script>
                        <tr>
                            <td colspan="4">
                                <div class="mi-form-item">
                                    <label class="mi-label" style="text-align: left; font-weight: normal; padding-left: 0;">
                                        <p>
                                            注：</p>
                                        <p>
                                            (a)累计数必须为整数,输入格式为:区间最小值-区间最大值,中间用"-"隔开，其他格式不通过；
                                        </p>
                                        <p>
                                            (b)累计数最高级别，输入格式为:区间最小值-无上限，中间用"-"隔开，区间最大值输入"无上限",其他格式不通过；
                                        </p>
                                        <p>
                                            (c)累计数高级别区间最小值-低级别区间最大值必须为1，否则不通过；</p>
                                        <p>
                                            (d)会员最高级别之前(包括最高级别)的启用状态必须都启用，其他级别需要停用；</p>
                                    </label>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="mi-form-explain">
                        <input type="hidden" value="" id="hid_levelids" />
                    </div>
                </div>
                <table width="960" border="0">
                    <tr>
                        <td width="869" height="44" align="center">
                            <input type="button" id="GoNext" value="  确  认  " class="mi-input" />
                        </td>
                        <td width="59">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
</asp:Content>
