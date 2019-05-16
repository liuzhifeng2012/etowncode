<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="agent_joinpolicy.aspx.cs"
    Inherits="ETS2.WebApp.Agent.agent_joinpolicy" MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.post("/JsonFactory/ProductHandler.ashx?oper=getjoinpolicy", { comid: $("#hid_comid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    $("#joinpolicy").val(data.msg);
                }
            })
            $("#GoNext").click(function () {
                var policy = $("#joinpolicy").trimVal();

                if (policy == "") {
                    alert("加盟政策不可为空");
                    return;
                }
                $.post("/JsonFactory/ProductHandler.ashx?oper=editjoinpolicy", { comid: $("#hid_comid").trimVal(), joinpolicy: policy }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) { }
                    if (data.type == 100) {
                        alert("加盟政策编辑成功");
                        return;
                    }
                })
            })
            $('#joinpolicy').tinymce({
                // Location of TinyMCE script
                script_url: '/Scripts/tiny_mce/tiny_mce.js',
                width: '622',
                height: '300',
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
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                
                 
                <li><a href="AgentSalesCode.aspx" onfocus="this.blur()" target=""><span>后台销售订单</span></a></li>
                <li><a href="AgentBackCode.aspx" onfocus="this.blur()" target=""><span>导码订单</span></a></li>
                <li><a href="AgentRecharge.aspx" onfocus="this.blur()" target=""><span>充值订单</span></a></li>
                <li><a href="/ticketservice/" onfocus="this.blur()" target=""><span>人工充值记录</span></a></li>
                <li><a href="AgentMessage.aspx" onfocus="this.blur()" target="">管理分销通知</a></li>
                <li><a href="AgentFinanceCount.aspx" onfocus="this.blur()" target="">分销商统计</a> </li>
                <li class="on"><a href="agent_joinpolicy.aspx" onfocus="this.blur()" target=""><span>
                    分销商加盟政策</span></a></li>
            </ul>
        </div>
        <div class="mi-form-item">
            <label class="mi-label">
                加盟政策</label>
            <textarea cols="150" rows="10" class="mi-input" id="joinpolicy" style="width: auto;"></textarea>
        </div>
        <table border="0">
            <tr>
                <td width="600" height="80" align="center">
                    <input type="button"   id="GoNext" value="  确认提交  " class="mi-input" />
                </td>
            </tr>
        </table>
    </div>
    <div class="data">
    </div>
</asp:Content>
