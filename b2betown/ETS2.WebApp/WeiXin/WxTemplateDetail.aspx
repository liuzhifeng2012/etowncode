<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/UI/Etown.Master" CodeBehind="WxTemplateDetail.aspx.cs" Inherits="ETS2.WebApp.WeiXin.WxTemplateDetail" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var id = $("#hid_id").trimVal();
            var comid = $("#hid_comid").trimVal();

            if (id != 0) {
                //创建
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=templatemodelinfo", { id: id }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("出现错误，请刷新重新提交！");
                        return;
                    }
                    if (data.type == 100) {
                        $("#infotype").val(data.msg.Infotype);
                        $("#template_name").val(data.msg.Template_name);
                        $("#first_DATA").val(data.msg.First_DATA);
                        $("#remark_DATA").val(data.msg.Remark_DATA);
                    }
                })
            }

            //提交按钮
            $("#btn-submit").click(function () {
                var infotype = $("#infotype").val();

                var template_name = $("#template_name").val();
                var first_DATA = $("#first_DATA").val();
                var remark_DATA = $("#remark_DATA").val();

                if (infotype == 0) {
                    $("#infotypeVer").html("请选择操作类型");
                    $("#infotypeVer").css("color", "red");
                    return;
                } else {
                    $("#infotypeVer").html("");
                }

                if (template_name == "") {
                    $("#template_nameVer").html("微信模板名称不能为空");
                    $("#template_nameVer").css("color", "red");
                    return;
                } else {
                    $("#template_nameVer").html("");
                }

                $("#loading").html("正在提交信息，请稍后...")

                //创建
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=templatemodeledit", { id: id, infotype: infotype, template_name: template_name, first_DATA: first_DATA, remark_DATA: remark_DATA }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("出现错误，请刷新重新提交！");
                        return;
                    }
                    if (data.type == 100) {
                             $.prompt("编辑成功");
                            location.href = "WxtemplateManage.aspx";
                            return;
                        
                    }
                })

            })

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
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/permissionui/MasterList.aspx" onfocus="this.blur()" target=""><span>人员管理</span></a></li>
                <li><a href="/ui/permissionui/SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>
                <li><a href="/ui/permissionui/AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                <li><a href="/ui/permissionui/ticketlist.aspx" onfocus="this.blur()"><span>退票管理</span></a></li>
                <!--<li><a href="bankmanager.aspx" onfocus="this.blur()"><span>绑定银行管理</span></a></li>-->
                <li><a href="/ui/permissionui/ModelList.aspx" onfocus="this.blur()" target=""><span>模板管理</span></a></li>
                <li class="on"><a href="WxTemplateManage.aspx" onfocus="this.blur()" target="">微信模板管理</a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                     </h3>

                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                   <h2 class="p-title-area">微信模板管理</h2>
                   <div class="mi-form-item">
                        <label class="mi-label"> 操作类型</label>
                      <select id="infotype">
                      <option value="0" selected="selected">请选择操作类型</option>
                      <option value="1">新订单模板</option>
                      <option value="2">订单状态变更通知(成功，取消等)</option>
                      <option value="3">门票订单预订成功通知</option>
                      <option value="4">酒店预订确认通知</option>
                      <option value="5">会员充值通知</option>
                      <option value="6">会员消费通知</option>
                      <option value="7">积分奖励提醒</option>
                      <option value="8">订阅活动开始提醒</option>
                      </select><span id="infotypeVer"></span>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 微信模板名称</label>
                       <input name="template_name" type="text" id="template_name"  size="25" class="mi-input"  style="width:200px;"/><span id="template_nameVer"></span>（请按微信模板名称填写）	
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 前置说明</label>
                       <input name="first_DATA" type="text" id="first_DATA"  size="25" class="mi-input"  style="width:300px;"/><span id="first_DATAVer"></span>
                   </div>
                                      <div class="mi-form-item">
                        <label class="mi-label"> 后置备注</label>
                       <input name="remark_DATA" type="text" id="remark_DATA"  size="25" class="mi-input"  style="width:300px;"/><span id="remark_DATAVer"></span>
                   </div>
                   
<div class="mi-form-explain"></div>
               </div>




                 <table  width="600px;">

                       <tr>
                        <td align="center">
                            <input type="button" name="Search" id="btn-submit"  class="mi-input" value="  确   认  " />
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
