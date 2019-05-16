<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/UI/Etown.Master" CodeBehind="AgentMessageUP.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.AgentMessageUP" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
     <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var id = $("#hid_id").trimVal();
            var comid = $("#hid_comid").trimVal();
            if (id != 0) {
                $("#sms").hide();
                //
                $.post("/JsonFactory/AgentHandler.ashx?oper=agentmessageinfo", { comid: comid, id: id }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 100) {
                        $("#Title").val(data.msg.Title);
                        $("#Message").val(data.msg.Message);
                        $("input:radio[name='State'][value='" + data.msg.State + "']").attr("checked", true);
                    }
                })
            } else {

                $("#sms").show();

            }


            $("#Sendsms").click(function () {
                var Sendsms = $("input:checkbox[name='Sendsms']:checked").val();

                if (Sendsms == 1) {
                    $("#smscontent").show();
                } else {
                    $("#smscontent").hide();
                }

            })



            //提交按钮
            $("#btn-submit").click(function () {
                var Title = $("#Title").val();
                var Message = $("#Message").val();
                var State = $("input:radio[name='State']:checked").val();

                var Sendsms = $("input:checkbox[name='Sendsms']:checked").val();
                var SmsText = $("#SmsText").val();


                if (Title == "") {
                    $("#TitleVer").html("请填写标题");
                    $("#TitleVer").css("color", "red");
                    return;
                }

                if (Message == "") {
                    $("#MessageVer").html("请填写通知内容");
                    $("#MessageVer").css("color", "red");
                    return;
                }

                if (Sendsms == 1) {
                    if (SmsText == "") {
                        $("#SmsTextVer").html("请填写短信内容");
                        $("#SmsTextVer").css("color", "red");
                        return;
                    }
                }


                $("#loading").html("正在提交注册信息，请稍后...")

                //创建订单
                $.post("/JsonFactory/AgentHandler.ashx?oper=agentmessageup", { comid: comid, id: id, Title: Title, Message: Message, State: State, Sendsms: Sendsms, SmsText: SmsText }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("出现错误，请刷新重新提交！");
                        return;
                    }
                    if (data.type == 100) {
                        location.href = "AgentMessage.aspx";
                        return;
                    }
                })

            })


            $('#Message').tinymce({
                // Location of TinyMCE script
                script_url: '/Scripts/tiny_mce/tiny_mce.js',
                width: '422',
                height: '320',
                // General options
                theme: "advanced",
                language: 'cn',
                plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,advlist",

                // Theme options
                theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,fontselect,fontsizeselect,|,forecolor,backcolor,|,insertdate,image,preview",
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

    </script>
        <style type="text/css">
.ui-input {
    border: 1px solid #C1C1C1;
    color: #343434;
    font-size: 14px;
    height: 25px;
    line-height: 25px;
    padding: 2px;
    vertical-align: middle;
    width: 200px;
}
    </style>
</head>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                
                 
                <li><a href="AgentSalesCode.aspx" onfocus="this.blur()" target=""><span>后台销售订单</span></a></li>
                <li><a href="AgentBackCode.aspx" onfocus="this.blur()" target=""><span>导码订单</span></a></li>
                <li><a href="AgentRecharge.aspx" onfocus="this.blur()" target=""><span>充值订单</span></a></li>
                <li class="on"><a href="AgentMessage.aspx" onfocus="this.blur()" target="">管理分销通知</a></li>
                <li><a href="AgentFinanceCount.aspx" onfocus="this.blur()" target="">分销商统计</a>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                     </h3>

                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                   <h2 class="p-title-area">通知管理</h2>
                   <div class="mi-form-item">
                        <label class="mi-label"> 标题</label>
                       <input name="Title" type="text" id="Title"  size="25" class="mi-input"  style="width:350px;"/><span id="TitleVer"></span>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 通知内容</label>
                        <textarea name="Message" cols="110" rows="5" id="Message"></textarea>
                       <span id="MessageVer"></span>
                   </div>

                   <div class="mi-form-item" id="sms">
                        <label class="mi-label"> 对所有分销商发送短信通知</label>
                       <label>
                            <input name="Sendsms" id="Sendsms" type="checkbox" value="1" />
                            给分销商发送短信通知</label>
                            <br /><br>
                            <div id="smscontent" style="display:none;">
                               <label  class="mi-label">短信内容：</label>
                                <textarea name="SmsText" id="SmsText"  cols="110" rows="2"></textarea>
                               
                              <span id="SmsTextVer"></span>
                           </div>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 状态</label>
                       <label>
                            <input name="State" type="radio" value="0" >
                            下线</label>
                        <label>
                            <input name="State" type="radio" value="1" checked>
                            上线</label>
                   </div>
                   
<div class="mi-form-explain"></div>
               </div>




                 <table  width="600px;">

                       <tr>
                        <td align="center">
                            <input type="button" name="Search" id="btn-submit"  class="mi-input" value="  确认  " />
						</td>
                    </tr>
                </table>
                <div id="divPage">
                </div>


            </div>
        </div>

    </div>
    <div class="data">
    </div>

    <input type="hidden" id="hid_id" value='<%=id %>' />
</asp:Content>
