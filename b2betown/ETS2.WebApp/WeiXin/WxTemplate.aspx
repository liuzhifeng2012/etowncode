<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master"  CodeBehind="WxTemplate.aspx.cs" Inherits="ETS2.WebApp.WeiXin.WxTemplate" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();
            var modellist = "";
            //读取模板
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=templatecompagelist", { comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("出现错误，请刷新重新提交！");
                    return;
                }
                if (data.type == 100) {

                    if (data.totalcount > 0) {
                        for (var i=0;i<data.totalcount;i++){
                            modellist += "<div class=\"mi-form-item\"><label class=\"mi-label\"> " + data.msg[i].Template_name + "</label><span>模板ID:</span><input name=\"template_id\" id=\"template_id" + i + "\" value=\"" + data.msg[i].Template_id + "\" type=\"text\" size=\"35\" class=\"mi-input\"  style=\"width:200px;\"/><input type=\"hidden\" id=\"id" + i + "\" value=\"" + data.msg[i].Id + "\"> （请复制名称，到微信查询，把模板ID复制填写到此）</div>";
                        }
                     }
                     $("#templatelist").html(modellist);
                 }

                 $("#hid_count").val(data.totalcount);
            })


            //提交按钮
            $("#btn-submit").click(function () {
                var hid_count= $("#hid_count").val();
                var id="";
                var Template_id="";

                for (var i=0;i<hid_count;i++){
                    id +=  $("#id"+i).val() +",";
                    Template_id +=  $("#template_id"+i).val() +",";

                    if($("#template_id"+i).val() ==""){
                        //$.prompt("请全部添加模板！");
                       // return;
                    }

                }

                $("#loading").html("正在提交信息，请稍后...")

                //创建
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=templateedit", { id: id, Template_id: Template_id,comid:comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("出现错误，请刷新重新提交！");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("编辑成功");
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
      <%--  <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="WxTemplate.aspx" onfocus="this.blur()" target="">微信消息模板管理</a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                     </h3>

                <div class="edit-box J-commonSettingsModule"  style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                   <h2 class="p-title-area">微信消息模板设定</h2>
                   <div id="templatelist"></div>
                   
                   <!--
                   <div class="mi-form-item">
                        <label class="mi-label"> 前置说明</label>
                       <input name="first_DATA" type="text" id="first_DATA"  size="25" class="mi-input"  style="width:300px;"/><span id="first_DATAVer"></span>
                       （系统回复时调用此内容，显示在最前面）
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 后置备注</label>
                       <input name="remark_DATA" type="text" id="remark_DATA"  size="25" class="mi-input"  style="width:300px;"/><span id="remark_DATAVer"></span>
                       （系统回复时调用此内容，显示在最后面）
                   </div>-->
                   
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
    <input type="hidden" id="hid_count" value='' />
</asp:Content>
