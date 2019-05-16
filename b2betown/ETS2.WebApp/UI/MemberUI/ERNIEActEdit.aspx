<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ERNIEActEdit.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.ERNIEActEdit" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //日历
            var nowdate = '<%=this.nowdate %>';
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {

                $(this).val(nowdate);
                $($(this)).datepicker({
                    numberOfMonths: 2,
                    minDate: 0,
                    defaultDate: +4,
                    maxDate: '+8m +1w'
                });
            });


            //首先加载数据
            var hid_actid = $("#hid_actid").trimVal();
            if (hid_actid != '0') {
                $.post("/JsonFactory/PromotionHandler.ashx?oper=ERNIEgetActById", { actid: hid_actid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#title").val(data.msg.Title);
                        $("#ERNIE_RateNum").val(data.msg.ERNIE_RateNum);
                        $("#Limit_Num").val(data.msg.Limit_Num);
                        $("#ERNIE_star").val(ChangeDateFormat(data.msg.ERNIE_star));
                        $("#ERNIE_end").val(ChangeDateFormat(data.msg.ERNIE_end));
                        $("#remark").val(data.msg.Remark);
                        $("#ERNIE_type").val(data.msg.ERNIE_type);
                        $("input:radio[name='ERNIE_Limit'][value=" + data.msg.ERNIE_Limit + "]").attr("checked", true);
                        $("input:radio[name='Runstate'][value="+data.msg.runstate+"]").attr("checked", true);
                    }
                })


                $.post("/JsonFactory/PromotionHandler.ashx?oper=ERNIEAwardget", { actid: hid_actid, topclass: 1 }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 100 && data.msg != null) {
                        $("#Award_title1").val(data.msg.Award_title);
                        $("#Award_num1").val(data.msg.Award_num);
                        $("#Award_type1").val(data.msg.Award_type);
                        $("#Award_Get_Num1").val(data.msg.Award_Get_Num);
                    }
                })

                $.post("/JsonFactory/PromotionHandler.ashx?oper=ERNIEAwardget", { actid: hid_actid, topclass: 2 }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 100 && data.msg != null) {
                        $("#Award_title2").val(data.msg.Award_title);
                        $("#Award_num2").val(data.msg.Award_num);
                        $("#Award_type2").val(data.msg.Award_type);
                        $("#Award_Get_Num2").val(data.msg.Award_Get_Num);
                    }
                })

                $.post("/JsonFactory/PromotionHandler.ashx?oper=ERNIEAwardget", { actid: hid_actid, topclass: 3 }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 100 && data.msg != null) {
                        $("#Award_title3").val(data.msg.Award_title);
                        $("#Award_num3").val(data.msg.Award_num);
                        $("#Award_type3").val(data.msg.Award_type);
                        $("#Award_Get_Num3").val(data.msg.Award_Get_Num);
                    }
                })
                $.post("/JsonFactory/PromotionHandler.ashx?oper=ERNIEAwardget", { actid: hid_actid, topclass: 4 }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 100 && data.msg != null) {
                        $("#Award_title4").val(data.msg.Award_title);
                        $("#Award_num4").val(data.msg.Award_num);
                        $("#Award_type4").val(data.msg.Award_type);
                        $("#Award_Get_Num4").val(data.msg.Award_Get_Num);
                    }
                })

                $.post("/JsonFactory/PromotionHandler.ashx?oper=ERNIEAwardget", { actid: hid_actid, topclass: 5 }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 100 && data.msg != null) {
                        $("#Award_title5").val(data.msg.Award_title);
                        $("#Award_num5").val(data.msg.Award_num);
                        $("#Award_type5").val(data.msg.Award_type);
                        $("#Award_Get_Num5").val(data.msg.Award_Get_Num);
                    }
                })

                $.post("/JsonFactory/PromotionHandler.ashx?oper=ERNIEAwardget", { actid: hid_actid, topclass: 6 }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 100 && data.msg != null) {
                        $("#Award_title6").val(data.msg.Award_title);
                        $("#Award_num6").val(data.msg.Award_num);
                        $("#Award_type6").val(data.msg.Award_type);
                        $("#Award_Get_Num6").val(data.msg.Award_Get_Num);
                    }
                })

                <% if (Online==1) {//如果已经上线将部分信息不能修改
                %>
                    $("#Award_title1").attr("disabled", "disabled");
                    $("#Award_title2").attr("disabled", "disabled");
                    $("#Award_title3").attr("disabled", "disabled");
                    $("#Award_title4").attr("disabled", "disabled");
                    $("#Award_title5").attr("disabled", "disabled");
                    $("#Award_title6").attr("disabled", "disabled");

                    $("#Award_num1").attr("disabled", "disabled");
                    $("#Award_num2").attr("disabled", "disabled");
                    $("#Award_num3").attr("disabled", "disabled");
                    $("#Award_num4").attr("disabled", "disabled");
                    $("#Award_num5").attr("disabled", "disabled");
                    $("#Award_num6").attr("disabled", "disabled");

                    $("#Award_type1").attr("disabled", "disabled");
                    $("#Award_type2").attr("disabled", "disabled");
                    $("#Award_type3").attr("disabled", "disabled");
                    $("#Award_type4").attr("disabled", "disabled");
                    $("#Award_type5").attr("disabled", "disabled");
                    $("#Award_type6").attr("disabled", "disabled");

                    $("#Award_Get_Num1").attr("disabled", "disabled");
                    $("#Award_Get_Num2").attr("disabled", "disabled");
                    $("#Award_Get_Num3").attr("disabled", "disabled");
                    $("#Award_Get_Num4").attr("disabled", "disabled");
                    $("#Award_Get_Num5").attr("disabled", "disabled");
                    $("#Award_Get_Num6").attr("disabled", "disabled");
                <%} %>
            }


            //添加摇奖活动
            $("#GoActAddNext").click(function () {
                var hid_actid = $("#hid_actid").trimVal();
                var comid = $("#hid_comid").trimVal();
                var Title = $("#title").trimVal();
                var ERNIE_RateNum = $("#ERNIE_RateNum").val();
                var Limit_Num = $("#Limit_Num").trimVal();

                var Remark = $("#remark").trimVal();
                var ERNIE_star = $("#ERNIE_star").trimVal();
                var ERNIE_end = $("#ERNIE_end").trimVal();

                var ERNIE_type = $("#ERNIE_type").val();
                var ERNIE_Limit = $('input:radio[name="ERNIE_Limit"]:checked').val();
                var Runstate = $('input:radio[name="Runstate"]:checked').val();


                if (Title == '') {
                    $.prompt('促销活动名称不可为空');
                    return;
                }

                if (ERNIE_star == '') {
                    $.prompt('活动开始日期不可为空');
                    return;
                }
                if (ERNIE_end == '') {
                    $.prompt('活动截止日期不可为空');
                    return;
                }

                if (Remark == '') {
                    $.prompt('活动备注不可为空');
                    return;
                }



                var Award_title1 = $("#Award_title1").trimVal();
                var Award_num1 = $("#Award_num1").trimVal();
                var Award_type1 = $("#Award_type1").trimVal();
                var Award_Get_Num1 = $("#Award_Get_Num1").trimVal();
                if (Award_title1 == '') {
                    $.prompt('请填写至少一个奖项，请逐一填写');
                    return;
                }
                if (Award_title1 != '') {
                    if (Award_num1 == '') {
                        $.prompt('请填写奖品数量');
                        return;
                    }
                    if (Award_type1 == '0') {
                        $.prompt('请选择奖品类型');
                        return;
                    }
                    if (Award_type1 == '2') {
                        if (Award_Get_Num1 == '') {
                            $.prompt('一等奖赠送积分/积分，请输入赠送金额');
                            return;
                        }
                    }
                }


                var Award_title2 = $("#Award_title2").trimVal();
                var Award_num2 = $("#Award_num2").trimVal();
                var Award_type2 = $("#Award_type2").trimVal();
                var Award_Get_Num2 = $("#Award_Get_Num2").trimVal();
                if (Award_title2 != '') {
                    if (Award_num2 == '') {
                        $.prompt('请填写奖品数量');
                        return;
                    }
                    if (Award_type2 == '0') {
                        $.prompt('请选择奖品类型');
                        return;
                    }
                    if (Award_type2 == '2') {
                        if (Award_Get_Num2 == '') {
                            $.prompt('二等奖赠送积分/积分，请输入赠送金额');
                            return;
                        }
                    }
                }
                var Award_title3 = $("#Award_title3").trimVal();
                var Award_num3 = $("#Award_num3").trimVal();
                var Award_type3 = $("#Award_type3").trimVal();
                var Award_Get_Num3 = $("#Award_Get_Num3").trimVal();
                if (Award_title3 != '') {
                    if (Award_num3 == '') {
                        $.prompt('请填写奖品数量');
                        return;
                    }
                    if (Award_type3 == '0') {
                        $.prompt('请选择奖品类型');
                        return;
                    }
                    if (Award_type3 == '2') {
                        if (Award_Get_Num3 == '') {
                            $.prompt('三等奖赠送积分/积分，请输入赠送金额');
                            return;
                        }
                    }
                }
                var Award_title4 = $("#Award_title4").trimVal();
                var Award_num4 = $("#Award_num4").trimVal();
                var Award_type4 = $("#Award_type4").trimVal();
                var Award_Get_Num4 = $("#Award_Get_Num4").trimVal();
                if (Award_title4 != '') {
                    if (Award_num4 == '') {
                        $.prompt('请填写奖品数量');
                        return;
                    }
                    if (Award_type4 == '0') {
                        $.prompt('请选择奖品类型');
                        return;
                    }
                    if (Award_type4 == '2') {
                        if (Award_Get_Num4 == '') {
                            $.prompt('四等奖赠送积分/积分，请输入赠送金额');
                            return;
                        }
                    }
                }


                var Award_title5 = $("#Award_title5").trimVal();
                var Award_num5 = $("#Award_num5").trimVal();
                var Award_type5 = $("#Award_type5").trimVal();
                var Award_Get_Num5 = $("#Award_Get_Num5").trimVal();
                if (Award_title5 != '') {
                    if (Award_num5 == '') {
                        $.prompt('请填写奖品数量');
                        return;
                    }
                    if (Award_type5 == '0') {
                        $.prompt('请选择奖品类型');
                        return;
                    }
                    if (Award_type5 == '2') {
                        if (Award_Get_Num5 == '') {
                            $.prompt('五等奖赠送积分/积分，请输入赠送金额');
                            return;
                        }
                    }
                }
                var Award_title6 = $("#Award_title6").trimVal();
                var Award_num6 = $("#Award_num6").trimVal();
                var Award_type6 = $("#Award_type6").trimVal();
                var Award_Get_Num6 = $("#Award_Get_Num6").trimVal();
                if (Award_title6 != '') {
                    if (Award_num6 == '') {
                        $.prompt('请填写奖品数量');
                        return;
                    }
                    if (Award_type6 == '0') {
                        $.prompt('请选择奖品类型');
                        return;
                    }
                    if (Award_type6 == '2') {
                        if (Award_Get_Num6 == '') {
                            $.prompt('六等奖赠送积分/积分，请输入赠送金额');
                            return;
                        }
                    }
                }


                $.post("/JsonFactory/PromotionHandler.ashx?oper=ERNIEeditActinfo", { Title: Title, ERNIE_type: ERNIE_type, ERNIE_star: ERNIE_star, ERNIE_end: ERNIE_end
                , ERNIE_RateNum: ERNIE_RateNum, ERNIE_Limit: ERNIE_Limit, Limit_Num: Limit_Num, Runstate: Runstate, comid: comid, actid: hid_actid, Remark: Remark, Award_title1: Award_title1, Award_title2: Award_title2, Award_title3: Award_title3, Award_title4: Award_title4, Award_title5: Award_title5, Award_title6: Award_title6, Award_num1: Award_num1, Award_num2: Award_num2, Award_num3: Award_num3, Award_num4: Award_num4, Award_num5: Award_num5, Award_num6: Award_num6, Award_type1: Award_type1, Award_type2: Award_type2, Award_type3: Award_type3, Award_type4: Award_type4, Award_type5: Award_type5, Award_type6: Award_type6, Award_Get_Num1: Award_Get_Num1, Award_Get_Num2: Award_Get_Num2, Award_Get_Num3: Award_Get_Num3, Award_Get_Num4: Award_Get_Num4, Award_Get_Num5: Award_Get_Num5, Award_Get_Num6: Award_Get_Num6
                }, function (data) {
                    data = eval('(' + data + ')');
                    if (data.type == '100') {
                        location.href = "ERNIEActList.aspx";
                        return;
                    } else {
                        $.prompt("摇奖活动出错");
                        return;
                    }
                });
            })
        });
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="ERNIEActList.aspx" onfocus="this.blur()" ><span>摇奖活动列表</span></a></li>
                <li class="on"><a href="ERNIEActEdit.aspx" onfocus="this.blur()" ><span>添加摇奖活动</span></a></li>
                <li><a href="ERNIERecordList.aspx" onfocus="this.blur()" ><span>中奖管理</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    </h3>
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                               <h2 class="p-title-area">设置摇奖活动</h2>
                               <div class="mi-form-item">
                                    <label class="mi-label">填写活动标题</label>
                                    <input name="title" type="text" id="title" size="25" maxlength="24"  class="mi-input" />
                               </div>
                               <div class="mi-form-item">
                                    <label class="mi-label">摇奖活动方式</label>
                                    <select name="ERNIE_type" id="ERNIE_type" class="mi-input">
                                         <option value="1" selected>大转盘摇奖</option>
                                    </select>
                               </div>
                               <div class="mi-form-item">
                                    <label class="mi-label">摇奖限定</label>
                                    <input name="ERNIE_Limit" type="radio" value="0" checked />
                                    一次性抽奖活动
                                    <input  name="ERNIE_Limit"  type="radio" name="UseOnce" value="1">
                                    每天抽奖活动
                               </div>
                               <div class="mi-form-item">
                                    <label class="mi-label">摇奖限定次数</label>
                                    <input name="Limit_Num" type="text" id="Limit_Num" size="6" class="mi-input" style="width:100px;"/> （一次性活动）可抽奖几次/每天可抽奖几次
                               </div>
                               <div class="mi-form-item">
                                    <label class="mi-label">摇奖基数</label>
                                     <input name="ERNIE_RateNum" type="text" id="ERNIE_RateNum" size="6" value="10000" class="mi-input" style="width:100px;"/>
                            （功能是设定中奖几率 奖品总数/摇奖基数 如 摇奖基数为1000 ,奖项共10个 10/1000=1%中奖几率，每次抽奖就在这1000范围内产生随机数来匹配是否中奖）
                               </div>
                               <div class="mi-form-item">
                                    <label class="mi-label">活动有效期</label>
                                    开始
                                    <input name="ERNIE_star" type="text" id="ERNIE_star" size="12" isdate="yes" class="mi-input" />
                                    截止
                                    <input name="ERNIE_end" type="text" id="ERNIE_end" size="12" isdate="yes" class="mi-input" />
                               </div>
                               <div class="mi-form-item">
                                    <label class="mi-label">活动内容说明</label>
                                     <textarea name="remark" cols="50" rows="6" id="remark" class="mi-input"  style="width:500px;"></textarea>
                               </div>

                                <div class="mi-form-explain"></div>
                </div>

                  <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                               <h2 class="p-title-area">摇奖奖项</h2>
                               <div class="mi-form-item">
                                    <label class="mi-label">一等奖</label>
                                    <label>
                                名称<input name="Award_title1" id="Award_title1" type="text" size="12"  class="mi-input"  style="width:100px;" />
                                </label> 
                                
                                <label>
                                数量   <input name="Award_num1" id="Award_num1" type="text" size="6"  class="mi-input"  style="width:40px;" />
                                   </label>                        
                                    <select name="Award_type1" id="Award_type1"  class="mi-input"  style="width:100px;">
                                        <option value="0">奖品类型</option>
                                        <option value="1">实物奖</option>
                                        <option value="2">会员积分/积分</option>
                                        <!--<option value="3">优惠券</option>-->
                                     </select>
                                     <label> 
                                价值<input name="Award_Get_Num1" id="Award_Get_Num1" size="6" type="text"  class="mi-input"  style="width:50px;"/>(会员积分数，优惠券ID)
                                     </label> 
                               </div>
                               <div class="mi-form-item">
                                    <label class="mi-label">二等奖</label>
                                    <label>
                                名称<input name="Award_title2" id="Award_title2" type="text" size="12"  class="mi-input" style="width:100px;"/>
                                </label> 
                                
                                <label>
                                数量   <input name="Award_num2" id="Award_num2" type="text" size="6" class="mi-input" style="width:40px;" />
                                   </label>                        
                                    <select name="Award_type2" id="Award_type2" class="mi-input" style="width:100px;">
                                        <option value="0">奖品类型</option>
                                        <option value="1">实物奖</option>
                                        <option value="2">会员积分/积分</option>
                                        <!--<option value="3">优惠券</option>-->
                                     </select>
                                     <label> 
                                价值<input name="Award_Get_Num2" id="Award_Get_Num2" size="6" type="text" class="mi-input" style="width:50px;" />(会员积分数，优惠券ID)
                                     </label> 
                               </div>
                               <div class="mi-form-item">
                                    <label class="mi-label">三等奖</label>
                                    <label>
                                名称<input name="Award_title3" id="Award_title3" type="text" size="12"  class="mi-input" style="width:100px;"/>
                                </label> 
                                
                                <label>
                                数量   <input name="Award_num3" id="Award_num3" type="text" size="6"  class="mi-input" style="width:40px;"/>
                                   </label>                        
                                    <select name="Award_type3" id="Award_type3" class="mi-input" style="width:100px;">
                                        <option value="0">奖品类型</option>
                                        <option value="1">实物奖</option>
                                        <option value="2">会员积分/积分</option>
                                        <!--<option value="3">优惠券</option>-->
                                     </select>
                                     <label> 
                                价值<input name="Award_Get_Num3" id="Award_Get_Num3" size="6" type="text"  class="mi-input" style="width:50px;"/>(会员积分数，优惠券ID)
                                     </label> 
                               </div>
                               <div class="mi-form-item" style=" display:none;">
                                    <label class="mi-label">四等奖</label>
                                    <!--<tr>
                                        <td class="tdHead">
                                            四等奖：
                                        </td>
                                        <td>
                                             <label>
                                                名称<input name="Award_title4" id="Award_title4" type="text" size="12" />
                                                </label> 
                                
                                                <label>
                                                数量   <input name="Award_num4" id="Award_num4" type="text" size="6" />
                                                   </label>                        
                                                    <select name="Award_type4" id="Award_type4">
                                                        <option value="0">奖品类型</option>
                                                        <option value="1">实物奖</option>
                                                        <option value="2">会员积分/积分</option>
                                                        <option value="3">优惠券</option>
                                                     </select>
                                                     <label> 
                                                价值<input name="Award_Get_Num4" id="Award_Get_Num4" size="6" type="text" />(会员积分数，优惠券ID)
                                                     </label> 
                                        </td>
                                    </tr>
                                                        <tr>
                                        <td class="tdHead">
                                            五等奖：
                                        </td>
                                        <td>
                                             <label>
                                                名称<input name="Award_title5" id="Award_title5" type="text" size="12" />
                                                </label> 
                                
                                                <label>
                                                数量   <input name="Award_num5" id="Award_num5" type="text" size="6" />
                                                   </label>                        
                                                    <select name="Award_type5" id="Award_type5">
                                                        <option value="0">奖品类型</option>
                                                        <option value="1">实物奖</option>
                                                        <option value="2">会员积分/积分</option>
                                                        <option value="3">优惠券</option>
                                                     </select>
                                                     <label> 
                                                价值<input name="Award_Get_Num5" id="Award_Get_Num5" size="6" type="text" />(会员积分数，优惠券ID)
                                                     </label> 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdHead">
                                            六等奖：
                                        </td>
                                        <td>
                                             <label>
                                                名称<input name="Award_title6" id="Award_title6" type="text" size="12" />
                                                </label> 
                                
                                                <label>
                                                数量   <input name="Award_num6" id="Award_num6" type="text" size="6" />
                                                   </label>                        
                                                    <select name="Award_type6" id="Award_type6">
                                                        <option value="0">奖品类型</option>
                                                        <option value="1">实物奖</option>
                                                        <option value="2">会员积分/积分</option>
                                                        <option value="3">优惠券</option>
                                                     </select>
                                                     <label> 
                                                价值<input name="Award_Get_Num6" id="Award_Get_Num6" size="6" type="text" />(会员积分数，优惠券ID)
                                                     </label> 
                                        </td>
                                    </tr>-->
                               </div>
                               <div class="mi-form-item">
                                    <label class="mi-label">运行状态</label>
                                    <label>
                                        <input name="Runstate"  type="radio" value="1" checked />
                                        运行中</label>
                                    <label>
                                        <input type="radio"  name="Runstate" value="0" >
                                        停止运行</label>
                                        (新添加项目，只有设置完成，运行后才能上线)
                               </div>
                                <div class="mi-form-explain"></div>
                  </div>


                <table width="780" border="0">
                    <tr>
                        <td width="699" height="44" align="center">
                            <input type="button" name="GoActAddNext" id="GoActAddNext" value="  确  认  "   class="mi-input"/>
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
    <input type="hidden" id="hid_actid" value="<%=actid %>" />
    <script type="text/x-jquery-tmpl" id="UseChannelList">
        <option value='${Id}'>${Companyname}</option>          
    </script>
</asp:Content>
