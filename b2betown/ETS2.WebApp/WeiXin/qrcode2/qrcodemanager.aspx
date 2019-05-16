<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="qrcodemanager.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.WeiXin.qrcode2.qrcodemanager" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();
            loadPromoteActivity(0)
            loadPromoteChannelCompany(0);

            $("#btnadd").click(function () {
                var qrcodename = $("#qrcode_name").val();
                var promoteact = $("#Select1").val();
                var promotechannelcompany = $("#Select2").val();

                if (qrcodename == "") {
                    alert("二维码名称不可为空");
                    return;
                }
                if (promoteact == "0") {
                    alert("请选择推广活动");
                    return;
                }

                $.post("/JsonFactory/WeiXinHandler.ashx?oper=editwxqrcode", { onlinestatus: $("#selonlinestatus").val(), channelid: $("#Select3").val(), qrcodeid: $("#hid_qrcodeid").val(), comid: comid, qrcodename: qrcodename, promoteact: promoteact, promotechannelcompany: promotechannelcompany }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        alert("二维码添加成功");
                        window.open("qrcodelist.aspx?isqudao=2", target = "_self");
                    }

                })
            })


            //加载二维码信息
            if ($("#hid_qrcodeid").val() != "0") {

                $.post("/JsonFactory/WeiXinHandler.ashx?oper=getwxqrcode", { qrcodeid: $("#hid_qrcodeid").val() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {

                    }
                    if (data.type == 100) {
                        $("#qrcode_name").val(data.msg.Title);
                        $("#selonlinestatus").val(data.msg.Onlinestatus);

                        loadPromoteActivity(data.msg.Activityid)//加载活动列表
                        loadPromoteChannelCompany(data.msg.Channelcompanyid); //加载渠道单位列表
                        LoadChannel(data.msg.Channelcompanyid, data.msg.Channelid); //加载渠道列表
                    }
                })
            } else {
                $("#hid_onlinestatus").val("1");
            }

            //渠道类型切换
            $("input:radio[name='radchanneltype']").change(function () {
                var channeltype = $("input:radio[name='radchanneltype']:checked").val();
                //获得所属渠道列表
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetChannelCompanyList", { comid: comid, issuetype: channeltype, companystate: "1", whetherdepartment: "0" }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {

                    }
                    if (data.type == 100) {
                        $("#Select2").empty();

                        $("#Select2").append("<option value='0' >未 选 择</option>");
                        for (var i = 0; i < data.msg.length; i++) {

                            $("#Select2").append("<option value='" + data.msg[i].Id + "' >" + data.msg[i].Companyname + "</option>");

                        }

                        //清空渠道人
                        $("#Select3").empty();

                        $("#Select3").append("<option value='0' >未 选 择</option>");

                    }
                })
            })

            $("#Select2").change(function () {
                var channelcomid = $("#Select2").val();
                LoadChannel(channelcomid, 0);
            })
        })
        function LoadChannel(channelcomid, seledval) {
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetChannels", { channelcomid: channelcomid, runstate: "1", whetherdefaultchannel: "0" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    return;
                }
                if (data.type == 100) {
                    $("#Select3").empty();

                    $("#Select3").append("<option value='0'>未 选 择</option>");
                    for (var i = 0; i < data.msg.length; i++) {
                        if (data.msg[i].Id == seledval) {
                            $("#Select3").append("<option value='" + data.msg[i].Id + "' selected='selected'>" + data.msg[i].Name + "</option>");
                        } else {
                            $("#Select3").append("<option value='" + data.msg[i].Id + "'>" + data.msg[i].Name + "</option>");
                        }
                    }

                }
            });
        }
        function loadPromoteChannelCompany(seledval) {
            var comid = $("#hid_comid").trimVal();
            //加载推广渠道单位
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetChannelCompanyList", { comid: comid, issuetype: "0,1", companystate: "1", whetherdepartment: "0" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    return;
                }
                if (data.type == 100) {
                    $("#Select2").empty();

                    $("#Select2").append("<option value='0'>未 选 择</option>");
                    for (var i = 0; i < data.msg.length; i++) {
                        if (data.msg[i].Id == seledval) {
                            $("#Select2").append("<option value='" + data.msg[i].Id + "' selected='selected'>" + data.msg[i].Companyname + "</option>");
                        } else {
                            $("#Select2").append("<option value='" + data.msg[i].Id + "'>" + data.msg[i].Companyname + "</option>");
                        }
                    }

                }
            });
        }
        function loadPromoteActivity(seledval) {
            var comid = $("#hid_comid").trimVal();
            //加载推广活动
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetActivityList", { comid: comid, runstate: "1", whetherexpired: "0" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    return;
                }
                if (data.type == 100) {
                    $("#Select1").empty();

                    $("#Select1").append("<option value='0'>未 选 择</option>");
                    for (var i = 0; i < data.msg.length; i++) {
                        if (data.msg[i].Id == seledval) {
                            $("#Select1").append("<option value='" + data.msg[i].Id + "' selected='selected'>" + data.msg[i].Title + "</option>");
                        }
                        else {
                            $("#Select1").append("<option value='" + data.msg[i].Id + "'>" + data.msg[i].Title + "</option>");
                        }
                    }

                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="qrcodeuserlist.aspx" onfocus="this.blur()"><span>扫码实时记录</span></a></li>
                <li><a href="qrcodelist.aspx?isqudao=1" onfocus="this.blur()">渠道二维码列表</a></li>
                <li><a href="qrcodelist.aspx?isqudao=0" onfocus="this.blur()">活动二维码列表</a></li>
                <li class="on"><a href="qrcodemanager.aspx" onfocus="this.blur()">二维码添加</a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    
                </h3>
            <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
               <h2 class="p-title-area">微信二维码添加</h2>
               <div class="mi-form-item">
                    <label class="mi-label">二维码名称</label>
                    <input name="qrcode_name" type="text" id="qrcode_name" value="" size="60" class="mi-input" />
               </div>

               <div class="mi-form-item">
                    <label class="mi-label">推广活动</label>
                    <select id="Select1" class="mi-input" >
                                <option value="0" style="min-width: 150px;">未 选 择</option>
                            </select>
               </div>
               <div class="mi-form-item" style="display:none;">
                    <label class="mi-label"> 推广渠道单位(可选)</label>
                     <select id="Select2" class="mi-input" >
                                <option value="0" style="min-width: 150px;">未 选 择</option>
                            </select>
                            <label>
                                <input type="radio" name="radchanneltype" value="0" />所属门市</label>
                            <label>
                                <input type="radio" name="radchanneltype" value="1" />合作公司</label>
               </div>
               <div class="mi-form-item" style="display:none;">
                    <label class="mi-label"> 推广渠道人(可选)</label>
                      <select id="Select3" class="mi-input">
                                <option value="0" style="min-width: 150px;">未 选 择</option>
                            </select>
               </div>
               <div class="mi-form-item">
                    <label class="mi-label" class="mi-input"> 运行状态</label>
                      <select id="selonlinestatus" class="mi-input">
                                <option value="1" style="min-width: 150px;">运行</option>
                                <option value="0" style="min-width: 150px;">暂停</option>
                            </select>
               </div>

               <div class="mi-form-item" style="color: Red;">
                     注:和渠道关联的二维码请到渠道管理下添加编辑；
               </div>
               <div class="mi-form-explain"></div>
            </div>

                <table width="780px" class="grid">
                    <tr>
                        <td class="tdHead" style="text-align: center">
                            <input type="hidden" id="hid_qrcodeid" value="<%=qrcodeid %>" />
                            
                            <input type="button" value="确 认" id="btnadd" class="mi-input" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
</asp:Content>
