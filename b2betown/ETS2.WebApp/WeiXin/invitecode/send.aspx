<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="send.aspx.cs" MasterPageFile="/UI/Etown.Master"
    Inherits="ETS2.WebApp.WeiXin.invitecode.send" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#GoSmsSend").removeAttr("disabled");

            //首先加载数据
            var comid = $("#hid_comid").trimVal();

            var qunfa = $("#hid_qunfa").val();
            if (qunfa == "yes")//获得群发电话号码字符串
            {
                Getqunfaphone($("#selsendnum").val());
            } else {

                $("#span_phonetotal").html(1);
                $("#span_hassend").html(0);
                $("#span_surplussend").html(1);
                $("#hid_surplusnum").val(1);
            }

            //确认发送
            $("#GoSmsSend").click(function () {

                var Smsphone = $("#hid_phone").val();
                var Smstext = $("#Smstext").trimVal();

                //                if (!Smsphone.match(/^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1}))+\d{8})$/)) {
                //                    $.prompt("发送号码不能为空或不满足手机号码规范");
                //                    return;
                //                }
                if (Smsphone == "") {
                    $.prompt('发送电话号码不能为空');
                    return;
                }

                if (Smstext == '') {
                    $.prompt('发送内容不能为空！');
                    return;
                }
                if (Smstext.length > 500) {
                    $.prompt('发送内容不能超过500字！');
                    return;
                }

                //对输入内容进行处理 去掉单，双引号
                Smstext = Smstext.replace(/\'/g, "").replace(/\"/g, "");

                //去掉斜杠\
                Smstext = Smstext.replace(/\//g, "");
                //去掉反斜杠/
                Smstext = Smstext.replace(/\\/g, "")

                //判断输入内容中是否有 邀请码替换符
                if (Smstext.indexOf("$invitecode$") < 0) {
                    $.prompt("输入内容必须含有邀请码替换符$invitecode$");
                    return;
                }
                //判断输入内容中是否有 微信号替换符
                if (Smstext.indexOf("$comweixin$") < 0) {
                    $.prompt("输入内容必须含有微信号替换符$comweixin$");
                    return;
                }

                //得到公司信息中的微信号
                if ($("#hid_weixinname").trimVal() == "") {
                    $.prompt("商家微信号需要进行设置");
                    return;
                };

                var len = Smstext.length;
                var ts = Math.ceil(len / 65);
                if (ts == 0) ts += 1;


                if (confirm("信息内容为" + len + "字，短信将按每人" + ts + "条发送！您确定发送吗？") == false) return false;
                $("#GoSmsSend").attr("disabled", "disabled");
                openpage(Smsphone, Smstext);


            })

            $("#selsendnum").change(function () {
                var num = $("#selsendnum").val();
                Getqunfaphone(num);
            })

           
        })

        function Getqunfaphone(qunfanum) {
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=Getqunfaphone", { comid: $("#hid_comid").trimVal(), qunfanum: qunfanum }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {

                }
                if (data.type == 100) {
                    $("#hid_phone").val(data.msg);
                    $("#span_phonetotal").html(data.total);
                    $("#span_hassend").html(0);
                    $("#span_surplussend").html(data.total);
                    $("#hid_surplusnum").val(data.total);
                }
            })

            //            $("#hid_phone").val("4,65,6666,6,6,6,888,5,55,24,242,0505220,00000,555553,44,11,7222");
            //            $("#span_phonetotal").html(17);
            //            $("#span_hassend").html(0);
            //            $("#span_surplussend").html(17);

            //            $("#hid_surplusnum").val(17);
        }

        var i = 0;
        function openpage(dirid, sendtext) {
            var arr = new Array();
            arr = dirid.split(",");

            var arr_len = arr.length;
            if (arr[i]) {
                if (i < arr_len) setTimeout(function () { create_html(arr[i - 1], sendtext); openpage(dirid, sendtext) }, 1000);
            }
            i++;
        }

        function create_html(str1, str3) {
            var SurplusNum = $("#hid_surplusnum").val();
            $("#hid_surplusnum").val(parseInt(SurplusNum) - 1);

            var hassendNum = parseInt($("#span_phonetotal").html()) - (parseInt(SurplusNum) - 1);

            $("#span_hassend").html(hassendNum);
            $("#span_surplussend").html(parseInt(SurplusNum) - 1);


            window.open("/weixin/invitecode/duanxin_send.aspx?isqunfa=" + $("#hid_qunfa").trimVal() + "&comid=" + $("#hid_comid").trimVal() + "&userid=" + $("#hid_userid").trimVal() + "&etmobile=" + str1 + "&duanxintext1=" + str3, target = 'ifm');
            if (parseInt(SurplusNum) - 1 == 0) {
                $("#span_sendresult").html('发送成功，请点击 <a href="sendlog.aspx">发送记录</a> 查看');
            } else {
                $("#span_sendresult").html('');
            }
            //            if (parseInt(SurplusNum) - 1 == 0) {
            //                setTimeout(function () {
            //                    window.open("sendlog.aspx");
            //                }, 2000);
            //            }

        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/crmui/BusinessCustomersList.aspx" onfocus="this.blur()">会员列表</a></li>
                <li class="on"><a href="/weixin/invitecode/send.aspx" onfocus="this.blur()">发送邀请码</a></li>
                <li><a href="/weixin/invitecode/sendlog.aspx" onfocus="this.blur()">邀请码发送记录</a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    发送邀请码
                </h3>
                <table class="grid">
                    <%if (qunfa == "no")
                      { %>
                    <tr>
                        <td class="tdHead">
                            发送号码：
                        </td>
                        <td>
                            <input id="Smsphone" cols="60" name="Smsphone" placeholder="发送号码" value="<%=phone %>"
                                readonly="readonly">
                            <%if (qunfa == "no" && phone == "")
                              {
                            %>
                            只能对已有会员进行发送短信，请在会员列表点击 “短信”
                            <%
                                }%>
                        </td>
                    </tr>
                    <%}
                      else
                      { %>
                    <tr>
                        <td class="tdHead">
                            已发码次数
                        </td>
                        <td>
                            <select id="selsendnum">
                                <option value="-1">全部</option>
                                <option value="0">0次</option>
                                <option value="1">1次</option>
                                <option value="2">2次</option>
                            </select>
                        </td>
                    </tr>
                    <%} %>
                    <tr>
                        <td class="tdHead" colspan="2" style="font-size: 16px;">
                            电话号码个数:<span id="span_phonetotal"></span> &nbsp;&nbsp; &nbsp;&nbsp;已发送:<span id="span_hassend">0</span>&nbsp;&nbsp;
                            &nbsp;&nbsp;剩余发送:<span id="span_surplussend"></span>
                            <input type="hidden" id="hid_surplusnum" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            发送邀请码内容：
                        </td>
                        <td>
                            <textarea id="Smstext" rows="8" cols="60" name="Smstext">尊敬的会员，欢迎您关注我们开通的微信服务公众帐号：$comweixin$ 并在微信中输入下面邀请码$invitecode$，即可将您原会员帐号与微信会员卡绑定。 感谢您的支持，期待在微信上与您相见！</textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead" colspan="2">
                            <span style="color: Red;">邀请码用$invitecode$代替； 微信号用$comweixin$代替<%if (IsParentCompanyUser == true)
                                                                                             {%>(微信号在<a href="/ui/userui/AccountInfo.aspx">商家信息</a>编辑)；
                                <%} %></span>
                        </td>
                    </tr>
                </table>
                <table border="0">
                    <tr>
                        <td width="600" height="80" align="center">
                            <input type="button" name="GoSmsSend" id="GoSmsSend" value="  确认发送  " />
                            <span id="span_sendresult" style="color: Red;"></span>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <iframe id="ifm" name="ifm" src="" width="100%" height="480px;" marginheight="0"
        marginwidth="0" frameborder="0" scrolling="no"></iframe>
    <div class="data">
        <input type="hidden" id="hid_phone" value="<%=phone %>" />
        <input type="hidden" id="hid_qunfa" value="<%=qunfa %>" />
        <input type="hidden" id="hid_weixinname" value="<%=weixinname %>" />
        <input type="hidden" id="hid_IsParentCompanyUser" value="<%=IsParentCompanyUser %>" />
    </div>
</asp:Content>
