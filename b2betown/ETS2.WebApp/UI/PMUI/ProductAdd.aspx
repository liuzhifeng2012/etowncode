<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ProductAdd.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.ProductAdd" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<%@ Register Src="/UI/CommonUI/Control/MultiUploadFileControl.ascx" TagName="multiUploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <%--  <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>--%>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script src="/Scripts/convertToPinyinLower.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <style type="text/css">
        element.style
        {
            opacity: 1;
            margin-top: 0px;
            position: relative;
            z-index: 10;
        }
        .edit-box
        {
            width: 860px;
        }
        
        .seled
        {
            width: 181px;
            position: relative;
            height: 24px;
            line-height: 24px;
            display: block;
            background-color: #528BCB;
        }
        .seled a
        {
            color: #FFFFFF !important;
        }
        .notseled
        {
            width: 181px;
            position: relative;
            height: 24px;
            line-height: 24px;
            display: block;
        }
        .dialog
        {
            position: fixed;
            _position: absolute;
            z-index: 10;
            border: 1px solid #CCC;
            text-align: center;
            font-size: 14px;
            background-color: #F4F4F4;
            overflow: hidden;
        }
        .inner tbody td
        {
            white-space: inherit !important;
        }
    </style>
    <script type="text/javascript">
        function SetReadOnly(obj, backgroundColor) {  
        if (obj) {  
            var ieVer = GetIeVersion();// 获取IE版本  
            if (obj.type == 'select-one') {  
            // 下拉框时  
                if (ieVer > 6) {  
                    obj.onfocus = function() {  
                        var index = this.selectedIndex;  
                        this.onchange = function() {  
                            this.selectedIndex = index;  
                        };  
                    };  
                } else {  
                    obj.onbeforeactivate = function() { return false; };  
                    obj.onfocus = function() { obj.blur(); };  
                    obj.onmouseover = function() { obj.setCapture(); };  
                    obj.onmouseout = function() { obj.releaseCapture(); };  
                }  
            } else if (obj.type == 'checkbox') {  
            // 复选框时  
                obj.onclick = function() { return false; };  
            } else if (obj.type == 'radio') {  
            // 单选框时，设置所有具有相同name的radio为只读  
                if (obj.name) {  
                    var arr = document.getElementsByName(obj.name);  
                    var len = arr.length;  
                    var tmp = null;  
                    for (var i = 0; i < len; i++)  
                        if (arr[i].checked) {  
                        tmp = arr[i];  
                        break;  
                    }  
                    var func;  
                    if (tmp)  
                        func = function() { tmp.checked = true; };  
                    else  
                        func = function() { return false; };  
                    for (var i = 0; i < len; i++)  
                        arr[i].onclick = func;  
                } else  
                    obj.onclick = function() { return false; };  
            } else {  
                obj.readOnly = true;  
                if (obj.type == 'text')  
                    obj.style.borderWidth = '0px';  
            }  
      
            if (backgroundColor)  
                obj.style.backgroundColor = backgroundColor;  
        }  
    }  
      
        function GetIeVersion() {  
        var exp;  
        try {  
            var str = navigator.userAgent;  
            var strIe = 'MSIE';  
            if (str && str.indexOf(strIe) >= 0) {  
                str = str.substring(str.indexOf(strIe) + strIe.length);  
                str = str.substring(0, str.indexOf(';'));  
                return parseFloat(str.trim());  
            }  
        }  
        catch (exp) {  
        }  
        return 0;  
    }  

    //得到公司含有的运费模板
    function getTmp(seled)
    {
     $("#sel_deliverytmp").html("");
        $.post("/JsonFactory/ProductHandler.ashx?oper=getdeliverytmplist",{comid:$("#hid_comid").trimVal()},function(data){
           data=eval("("+data+")");
           if(data.type==1)
           {
              $("#sel_deliverytmp").html('<option value="0">未设置</option>');
           }
           else
           {
                var str="<option value=\"0\">请选择模板</option>";
                for(var i=0;i<data.msg.length;i++)
                {
                  if(data.msg[i].id==seled)
                  {
                   str+='<option value="'+data.msg[i].id+'" selected="selected">'+data.msg[i].tmpname+'</option>';
                  }else{
                    str+='<option value="'+data.msg[i].id+'">'+data.msg[i].tmpname+'</option>';
                   }
                }
                 $("#sel_deliverytmp").html(str);
           }
        })
    }


        $(function () { 


            $("input:radio[name='manyspeci'][value='0']").attr("checked","checked");
            $("input:radio[name='ishasdeliveryfee'][value='0']").attr("checked","checked");
            $("input:radio[name='ishasdeliveryfee']").click(function(){
                var seled=$("input:radio[name='ishasdeliveryfee']:checked").trimVal();
                if(seled==0)
                {
                  $("#div_deliverytmp").hide();
                }
                else
                {
                   $("#div_deliverytmp").show();
                }
            })
          
//           $("#loading").show();
            var projectid = $("#hid_projectid").val();
            if (projectid != 0) {
                LoadProjectList($("#hid_comid").val(), projectid);
            } else { LoadProjectList($("#hid_comid").val(), projectid); 
            
            }

            <%if(servertype==12 && proid==0){ %>
                var html_temp='房屋概况：<table width="100%" border="0">	<tr>		<td width="40%" height="26">房屋位置：</td>		<td width="60%" height="26">&nbsp;</td>	</tr>	<tr>		<td height="26">房本面积：</td>		<td height="26">&nbsp;</td>	</tr>	<tr>		<td height="26">户型立面：</td>		<td height="26">&nbsp;</td>	</tr>	<tr>		<td height="26">装修状态：</td>		<td height="26">&nbsp;</td>	</tr>	<tr>		<td height="26">居室数量：</td>		<td height="26">&nbsp;</td>	</tr>	<tr>		<td height="26">坐落朝向：</td>		<td height="26">&nbsp;</td>	</tr>	<tr>		<td height="26">产权情况：</td>		<td height="26">&nbsp;</td>	</tr>	<tr>		<td height="26">居住状态：</td>		<td height="26">&nbsp;</td>	</tr></table>';
                $("#service_Contain").val(html_temp);
            <%} %>


            bindViewImg();
            //日历
            var nowdate = '<%=this.nowdate %>';
            var monthdate = '<%=this.monthdate %>';

            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {

                $("#pro_start").val(nowdate);
                $("#pro_end").val(monthdate);
                $($(this)).datepicker();
            });

            //商户行业类型
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetIndustryList", {}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                    $("#sel_hangye").empty();000
                    for (var i = 0; i < data.msg.length; i++) {
                        $("#com_type").append('<option value="' + data.msg[i].Id + '">' + data.msg[i].Industryname + '</option>');
                    }

                }
            })

            //加载下班工作时间
            $.post("/JsonFactory/ProductHandler.ashx?oper=pro_worktimepagelist", {comid: $("#hid_comid").trimVal(), pageindex: 1, pagesize: 20}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {

                    for (var i = 0; i < data.msg.length; i++) {
                        $("#worktimeid").append('<option value="' + data.msg[i].id + '">' + data.msg[i].title + '</option>');
                    }

                }
            })

            

            //首先加载数据
            var hid_proid = $("#hid_proid").trimVal();
            if (hid_proid != '0') {
                $.post("/JsonFactory/ProductHandler.ashx?oper=getProById", { proid: hid_proid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取数据出现错误");
                        return;
                    }
                    if (data.type == 100) { 

                     getprogrouplist(data.msg.progroupid,data.initprocomid);
                     $("#hid_initprocomid").val(data.initprocomid);

                     $("input:radio[name='isSetVisitDate'][value='"+data.msg.isSetVisitDate+"']").attr("checked","checked");
                     $("input:radio[name='issetidcard'][value='"+data.msg.issetidcard+"']").attr("checked","checked");
                     $("input:radio[name='ishasdeliveryfee'][value='"+data.msg.ishasdeliveryfee+"']").attr("checked","checked");
                        getTmp(data.msg.deliverytmp);//得到运费模板
                        if(data.msg.ishasdeliveryfee==0)
                        {
                            $("#div_deliverytmp").hide();
                        }
                        else
                        {
                            $("#div_deliverytmp").show();
                        } 

                        //绑定验证服务

                        $("input[name='Wrentserver'][value='" + data.msg.Wrentserver + "']").attr("checked", true); 
                        if(data.msg.Wrentserver==1){
                            
                            $("input[name='WDeposit'][value='" + data.msg.WDeposit + "']").attr("checked", true);  
                            $("#Depositprice").val(data.msg.Depositprice);
                            readRentserver();
                        }
                         $("#worktimehour").val(data.msg.worktimehour);
                         $("#worktimeid").val(data.msg.worktimeid);
//                         $("#SpecifyPosid").val(data.msg.SpecifyPosid);
                         selectCompanyPos(data.msg.SpecifyPosid);





                        //产品预订一张产生电子码个数
                        $("#txt_pnonumperticket").val(data.msg.pnonumperticket);
                         
                        if(data.msg.Source_type==4){ 
                          $("#sel_progroupid").attr("disabled","disabled").css("background-color", "#cccccc");
                          $("#a_groupmanage").parent().hide();

                          $("#SpecifyPosid").attr("disabled","disabled").css("background-color", "#cccccc");
                          $("#txt_pnonumperticket").val(data.msg.pnonumperticket).attr("disabled","disabled").css("background-color", "#cccccc");
                            $("#pro_yanzheng_method").val(data.msg.pro_yanzheng_method).attr("disabled","disabled").css("background-color", "#cccccc");
                            $("#firststationtime").val(data.msg.firststationtime).attr("disabled","disabled").css("background-color", "#cccccc");
                            $("input:[name='source_type']").each(function(){
                              //写入代码
                              $(this).attr("disabled","disabled").css("background-color", "#cccccc");
                            });
                            $("input:[name='manyspeci']").each(function(){
                              //写入代码
                              $(this).attr("disabled","disabled").css("background-color", "#cccccc");
                            });
                         
                            $("input:[name='ProValidateMethod']").each(function(){
                              //写入代码
                              $(this).attr("disabled","disabled").css("background-color", "#cccccc");
                            });
                            $("#face_price").attr("readonly","readonly").css("background-color", "#cccccc");
                            $("#Sms").attr("readonly","readonly").css("background-color", "#cccccc");
                            $("#pro_start").attr("readonly","readonly").css("background-color", "#cccccc");
                            $("#pro_end").attr("readonly","readonly").css("background-color", "#cccccc");
                            $("#server_type").attr("disabled","disabled").css("background-color", "#cccccc");
                            $("#pro_type").attr("disabled","disabled").css("background-color", "#cccccc");
                            $("#sel_iscanuseonsameday").attr("disabled","disabled").css("background-color", "#cccccc"); 
                            $("#pro_name").attr("readonly","readonly").css("background-color", "#cccccc"); 
                            $("textarea").attr("readonly","readonly").css("background-color", "#cccccc"); 
                            $("#pickuppoint").attr("readonly","readonly").css("background-color", "#cccccc");
                            $("#dropoffpoint").attr("readonly","readonly").css("background-color", "#cccccc");
                            $("#pro_note").attr("readonly","readonly").css("background-color", "#cccccc");
                            $("#sel_QuitTicketMechanism").attr("disabled","disabled").css("background-color", "#cccccc");
                            $("#sel_deliverytmp").attr("disabled","disabled").css("background-color", "#cccccc"); 
                            $("input:[name='ishasdeliveryfee']").each(function(){
                            //写入代码
                            $(this).attr("disabled","disabled").css("background-color", "#cccccc");
                             }); 
                            
                        $("input:[name='ispanicbuy']").each(function(){
                            //写入代码
                            $(this).attr("disabled","disabled").css("background-color", "#cccccc");
                        }); 
                         $("#buynum").attr("readonly","readonly").css("background-color", "#cccccc");
                         $("#panicbuy_begintime").attr("readonly","readonly").css("background-color", "#cccccc");
                          $("#panicbuy_endtime").attr("readonly","readonly").css("background-color", "#cccccc"); 
                            $("input:[name='isneedbespeak']").each(function(){
                              //写入代码
                              $(this).attr("disabled","disabled").css("background-color", "#cccccc");
                            }); 
                             $("#daybespeaknum").attr("readonly","readonly").css("background-color", "#cccccc");
                              $("#bespeaksucmsg").attr("readonly","readonly").css("background-color", "#cccccc");
                               $("#bespeakfailmsg").attr("readonly","readonly").css("background-color", "#cccccc");
                                $("#customservicephone").attr("readonly","readonly").css("background-color", "#cccccc");
                            $("input:[name='isblackoutdate']").each(function(){
                              //写入代码
                              $(this).attr("disabled","disabled").css("background-color", "#cccccc");
                            });
                           $("input:[name='etickettype']").each(function(){
                              //写入代码
                              $(this).attr("disabled","disabled").css("background-color", "#cccccc");
                            });
                         //$("#sel_viewmethod").attr("disabled","disabled").css("background-color", "#cccccc");
                         $("#setagent").attr("disabled","disabled").css("background-color", "#cccccc");
                        // $("#selprostate").attr("disabled","disabled").css("background-color", "#cccccc"); 
                           $("#btnuplimitbuytotalnum").hide();
                            $("#setagent").hide(); 

                             $("#selbindbx").attr("disabled","disabled").css("background-color", "#cccccc"); 
                        }; 

                         
                        ViewBaoxian(data.msg.selbindbx);
                        $("#firststationtime").val(data.msg.firststationtime)
                        $("#pro_yanzheng_method").val(data.msg.pro_yanzheng_method);
                        $("#pro_weight").val(data.msg.pro_weight);
                        $("#sel_QuitTicketMechanism").val(data.msg.QuitTicketMechanism);

                        LoadProjectList($("#hid_comid").val(), data.msg.Projectid);
                         $("input[name='manyspeci'][value='" + data.msg.Manyspeci + "']").attr("checked", true);  
                        $("input[name='ispanicbuy'][value='" + data.msg.Ispanicbuy + "']").attr("checked", true);  
                         $("#buynum").val($("#hid_buynum").trimVal());
                         $("#panicbuy_limitbuytotalnum").val($("#hid_limitbuytotalnum").trimVal());
                         $("#saletotalnum").text("销售总量:"+parseInt(parseInt($("#hid_buynum").trimVal())+parseInt($("#hid_limitbuytotalnum").trimVal())));
                         

                        $("#panicbuy_begintime").val(jsonDateFormatKaler(data.msg.Panic_begintime));
                        $("#panicbuy_endtime").val(jsonDateFormatKaler(data.msg.Panicbuy_endtime));

                        $("#pro_name").val(data.msg.Pro_name);
                        $("#pro_note").val(data.msg.pro_note);
                        $("#merchant_code").val(data.msg.merchant_code);


                        $("#linepro_booktype").val(data.msg.Linepro_booktype);

                        $("#server_type").val(data.msg.Server_type);
                        LoadServertype(data.msg.Server_type);

                         ViewIspanicbuy(data.msg.Ispanicbuy);
                        


                         //产品验证有效期
                        $("input:radio[name='ProValidateMethod'][value='"+data.msg.ProValidateMethod+"']").attr("checked",true);
                        if(data.msg.ProValidateMethod==1)//按产品有效期
                         {
                           $("#div_appointdata").hide();
                         }
                         else//按指定有效期
                         {
                          $("#div_appointdata").show();
                          $("#sel_appointdata").val(data.msg.Appointdata);
                         }

                         $("#sel_iscanuseonsameday").val(data.msg.Iscanuseonsameday);
                         $("#sel_viewmethod").val(data.msg.Viewmethod);

                         $("#childreduce").val(data.msg.Childreduce);
                          
                        if(data.msg.Server_type=="9")//酒店客房，则需要查询扩展表信息
                         {
                             $.post("/JsonFactory/ProductHandler.ashx?oper=GetHouseType", { proid:hid_proid, comid: $("#hid_comid").trimVal() }, function (data1) {
                                data1 = eval("(" + data1 + ")");
                                if (data1.type == 1) {
                                    $.prompt("获取房型有误");
                                    return;
                                }
                                if (data1.type == 100) {
                  
                                    
                                    $("#bedtype").val(data1.msg.Bedtype);
                                    $("#bedwidth").val(data1.msg.Bedwidth);
                                    $("input[name='whetherextrabed'][value='" + data1.msg.Whetherextrabed + "']").attr("checked", true);
                                    $("#extrabedprice").val(data1.msg.Extrabedprice);
                                    $("#ReserveType").val(data1.msg.ReserveType);
                                    $("#wifi").val(data1.msg.Wifi);
                                    $("#Breakfast").val(data1.msg.Breakfast);
                                    
                                    $("#Builtuparea").val(data1.msg.Builtuparea);
                                    $("#floor").val(data1.msg.Floor);
                                    $("#largestguestnum").val(data1.msg.Largestguestnum);

                                    $("input[name='whethernon-smoking'][value='" + data1.msg.Whethernonsmoking + "']").attr("checked", true);
                                    $("#amenities").val(data1.msg.Amenities);
                                    $("#Mediatechnology").val(data1.msg.Mediatechnology);
                                    $("#Foodanddrink").val(data1.msg.Foodanddrink);
                                    $("#ShowerRoom").val(data1.msg.ShowerRoom);
                                    $("#roomtyperemark").val(data1.msg.Roomtyperemark);
                                    //$("#RecerceSMSName").val(data1.msg.RecerceSMSName);
                                    //$("#RecerceSMSPhone").val(data1.msg.RecerceSMSPhone);


                              }
                    })
                }

                        $("#pro_type").val(data.msg.Pro_type);
                        $("#travelproductid").val(data.msg.Travelproductid);
                        $("#traveltype").val(data.msg.Traveltype);
                        $("#travelstarting").val(data.msg.Travelstarting);
                        $("#travelending").val(data.msg.Travelending);

                        $("#pickuppoint").val(data.msg.pickuppoint);
                        $("#dropoffpoint").val(data.msg.dropoffpoint);

                        $("#pro_Remark").val(data.msg.Pro_Remark);
                        $("#pro_start").val(ChangeDateFormat(data.msg.Pro_start));
                        $("#pro_end").val(ChangeDateFormat(data.msg.Pro_end));
                        $("#face_price").val(data.msg.Face_price);
                        $("#advise_price").val(data.msg.Advise_price);
                        $("#agent1_price").val(data.msg.Agent1_price);
                        $("#agent2_price").val(data.msg.Agent2_price);
                        $("#agent3_price").val(data.msg.Agent3_price);

                        $("#agentsettle_price").val(data.msg.Agentsettle_price);
                        $("input[name='tuan_pro'][value='" + data.msg.Tuan_pro + "']").attr("checked", true);
                        $("input[name='zhixiao'][value='" + data.msg.Zhixiao + "']").attr("checked", true);
                        $("input[name='agentsale'][value='" + data.msg.Agentsale + "']").attr("checked", true);
                        $("input[name='ThatDay_can'][value='" + data.msg.ThatDay_can + "']").attr("checked", true);
                        $("#hid_servicecontain").val(data.msg.Service_Contain);
                        $("#hid_servicenotcontain").val(data.msg.Service_NotContain);
                        $("#hid_precautions").val(data.msg.Precautions); 
                        LoadProclass($("#hid_comid").val(), data.proclass);

                        $("input:radio[name='source_type'][value=" + data.msg.Source_type + "]").attr("checked", true);
                        if (data.msg.Source_type == "3") {
                            $("#trservice").show();
                            $("#trservice_proid").show();
                            $("#trrealnametype").show();
                        } 
                        else {
                            $("#trservice").hide();
                            $("#trservice_proid").hide();
                            $("#trrealnametype").hide();
                        }

                         $("#unsure").val(data.msg.unsure);
                          $("#unyuyueyanzheng").val(data.msg.unyuyueyanzheng);
                       

                        $("#Thatday_can_day").val(data.msg.Thatday_can_day);

                        $("#hid_imgurl").val(data.msg.Imgurl);
                        $("#pro_Number").val(data.msg.Pro_number);
                        $("#pro_Explain").val(data.msg.Pro_explain);
                        $("#Integral_price").val(data.msg.Pro_Integral);
                        $("#selprostate").val(data.msg.Pro_state);

                        $("input[name='tuipiao'][value='1']").attr("checked", true); //退票
                        if (data.msg.Tuipiao_guoqi == 1) {
                            $("input[name='tuipiao_guoqi'][value='1']").attr("checked", true); //退票
                        }
                        $("#tuipiao_endday").val(data.msg.Tuipiao_endday); //退票有效期

                        $("#service_Contain").val(data.msg.Service_Contain);
                        $("#service_NotContain").val(data.msg.Service_NotContain);
                        $("#Precautions").val(data.msg.Precautions);
                        if (data.msg.Sms == "" || data.msg.Sms == null) {
                            if(data.msg.Server_type==10)
                            {
                             $("#Sms").val("您订购的 $产品名称$ ，$数量$张，有效期到：$有效期$,上车地点$上车地点$,持订购人身份证上车,电子码：$票号$,请在准时到达过时不候！");
                            }
                            else{
                            //新添加产品，自动设定电子票内容
                            $("#Sms").val("您订购的 $产品名称$ ，$数量$张，有效期到：$有效期$，电子码：$票号$,请在有效期内使用！");
                            }
                        } 
                        else {
                            $("#Sms").val(data.msg.Sms);
                        }

                        $("#selservice").val(data.msg.Serviceid);
                        $("#txtservice_proid").val(data.msg.Service_proid);
                        $("#selreal_name_type").val(data.msg.Realnametype);

                         $("#daybespeaknum").val(data.msg.daybespeaknum);
                         $("input[name='isneedbespeak'][value='"+data.msg.isneedbespeak+"']").attr("checked", true);
                         if(data.msg.isneedbespeak==1)
                         {
                            $("#ddiv_isneedbespeak").show();
                         }
                           $("#bespeaksucmsg").val(data.msg.bespeaksucmsg);
                           $("#bespeakfailmsg").val(data.msg.bespeakfailmsg);
                           $("#customservicephone").val(data.msg.customservicephone);
                           $("input[name='isblackoutdate'][value='"+data.msg.isblackoutdate+"']").attr("checked", true);
                           if(data.msg.isblackoutdate==1)
                           {
                             $("#ddiv_isblackoutdate").show();
                           }
                           $("input[name='etickettype'][value='"+data.msg.etickettype+"']").attr("checked", true);
                           
                           $("#txt_bookpro_bindname").val(data.msg.bookpro_bindname);
                           $("#txt_bookpro_bindphone").val(data.msg.bookpro_bindphone);
                           $("#txt_bookpro_bindcompany").val(data.msg.bookpro_bindcompany);
                           $("#RecerceSMSName").val(data.msg.bookpro_bindname);
                           $("#RecerceSMSPhone").val(data.msg.bookpro_bindphone);
                           $("#RecerceSMSCompany").val(data.msg.bookpro_bindcompany);

                           $("#sel_bookpro_ispay").val(data.msg.bookpro_ispay);
                           $("#sel_isrebate").val(data.msg.isrebate);
//                        $("#loading").hide();

//                        alert(data.msg.Manyspeci);
                          //产品规格
                          if(data.msg.Manyspeci==1){
                               $("#hid_guigeNum").val(data.ggtypemaxid);
                               $("input:radio[name='manyspeci'][value='1']").attr("checked","checked");
                               selectspeci();
                               if(data.ggtypelist!=""&&data.gglist!=""){
                                  $('.add_standard_wrp').before($('#speciTmpl').tmpl(data.ggtypelist));
                                   step.Creat_Table();

                                   //给规格赋值
                                   if(data.gglist.length>0){
                                       for(var i=0;i<data.gglist.length;i++)
                                       {
                                          $("tr[data-id='"+data.gglist[i].speci_name+"']").find("input[name=Txt_facePriceSon]").val(data.gglist[i].speci_face_price);
                                          $("tr[data-id='"+data.gglist[i].speci_name+"']").find("input[name=Txt_advicePriceSon]").val(data.gglist[i].speci_advise_price);
                                          $("tr[data-id='"+data.gglist[i].speci_name+"']").find("input[name=Txt_agent1PriceSon]").val(data.gglist[i].speci_agent1_price);
                                          $("tr[data-id='"+data.gglist[i].speci_name+"']").find("input[name=Txt_agent2PriceSon]").val(data.gglist[i].speci_agent2_price);
                                          $("tr[data-id='"+data.gglist[i].speci_name+"']").find("input[name=Txt_agent3PriceSon]").val(data.gglist[i].speci_agent3_price);
                                          $("tr[data-id='"+data.gglist[i].speci_name+"']").find("input[name=Txt_settlePriceSon]").val(data.gglist[i].speci_agentsettle_price);
                                          $("tr[data-id='"+data.gglist[i].speci_name+"']").find("input[name=Txt_WeightSon]").val(data.gglist[i].speci_pro_weight);
                                          $("tr[data-id='"+data.gglist[i].speci_name+"']").find("input[name=Txt_CountSon]").val(data.gglist[i].speci_totalnum);
                                          
                                       }
                                   }
                               }
                               else{
                                  alert("产品规格查询失败");
                                  return ;
                               }

                               //导入产品的话禁止编辑多规格
                               if(data.msg.Source_type==4)
                               {
                                    $("input:[name='manyspeci']").last().attr("checked","checked");
                                    $("input:[name='manyspeci']").each(function(){
                                                //写入代码
                                      $(this).attr("disabled","disabled").css("background-color", "#cccccc");
                                    }); 

                                  $("#js_guige_main .btn_del").attr("disabled","disabled").css("color","#8d8d8d");
                                  $("#js_guige_main .btn_edit").addClass("dn");
                                  $("#js_guige_main .btn_del").addClass("dn");
                                  $("#js_guige_main .js_sku_add").addClass("dn");
                                  $("input[name='ckguige']").attr("disabled","disabled").css("background-color","#8d8d8d");
                                 $("#process input[name='Txt_facePriceSon']").attr("readonly","readonly").css("background-color","#8d8d8d");
                                 $("#process input[name='Txt_settlePriceSon']").attr("readonly","readonly").css("background-color","#8d8d8d");
                                 $("#process input[name='Txt_WeightSon']").attr("readonly","readonly").css("background-color","#8d8d8d");
                                 $("#process input[name='Txt_CountSon']").attr("readonly","readonly").css("background-color","#8d8d8d");
                                 //导入产品库存量隐藏
                                 $("#process").find("th:last").hide();
                                  $("#process").find("tr").each(function(ti,tiitem){
                                     $(tiitem).find("td:last").hide();
                                  })

                               }
                          }
                    }
                })
                 
            } 
            else {
                $("#hid_initprocomid").val($("#hid_comid").trimVal());
                getprogrouplist(0,$("#hid_comid").trimVal());
                selectCompanyPos("");
                //添加产品时剩余可销售数量 只读属性去掉，同时调整按钮隐藏
                $("#panicbuy_limitbuytotalnum").removeAttr("readonly").css("background-color","#FFFFFF");
                $("#btnuplimitbuytotalnum").hide();

                getTmp(0);//得到运费模板 
                LoadProclass($("#hid_comid").val(), 0);
                LoadServertype(<%=servertype %>);

                //除了旅游大巴,短信内容默认
              
                if (<%=servertype %>==10)
                {
                    $("#Sms").val("您订购的$产品名称$($数量$人),有效期$有效期$,上车地点$上车地点$,持订购人身份证上车,请您做好防寒措施");
                }else if (<%=servertype %>==9){
                    $("#Sms").val("您提交的订房已经预订成功 $产品名称$，共$数量$间，入住日期：$入住日期$ 离店日期：$离店日期$ ,入住：$几天$ 天 ，请持 $姓名$ 份证到酒店前台办理预订。");
                }else if (<%=servertype %>==12){
                    $("#Sms").val("您已经成功提交预约 $产品名称$，电子码为：$票号$。请在指定预约日期使用。");
                }
                else
                {
                   $("#Sms").val("您已经成功购买$产品名称$，$数量$张，电子码为：$票号$，有效期$有效期$。请在有效期内使用。");
                }
               
                $("#panicbuy_begintime").val(new Date().format("yyyy-MM-dd hh:mm:ss"));
                $("#panicbuy_endtime").val(new Date().format("yyyy-MM-dd hh:mm:ss"));
            }

             $('input:radio[name="ispanicbuy"]').click(function(){
               var isseled=$('input:radio[name="ispanicbuy"]:checked').trimVal();
               ViewIspanicbuy(isseled);
              
            })

             $('input:radio[name="isneedbespeak"]').click(function(){
               var isseled=$('input:radio[name="isneedbespeak"]:checked').trimVal();
               if(isseled==1)
               {
                 $("#ddiv_isneedbespeak").show();
               }
               if(isseled==0)
               {
                 $("#ddiv_isneedbespeak").hide();
               }
            })
             $('input:radio[name="isblackoutdate"]').click(function(){
               var isseled=$('input:radio[name="isblackoutdate"]:checked').trimVal();
               if(isseled==1)
               {
                 $("#ddiv_isblackoutdate").show();
               }
               if(isseled==0)
               {
                 $("#ddiv_isblackoutdate").hide();
               }
            })
           
            
            
            <%if(servertype==2||servertype==8){ %>
             $('#pro_Remark').tinymce({
                // Location of TinyMCE script
                script_url: '/Scripts/tiny_mce/tiny_mce.js',
                width: '622',
                height: '300',
                // General options
                theme: "advanced",
                language: 'cn',
                plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,advlist",

                // Theme options
                theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,fontselect,fontsizeselect,|,forecolor,backcolor,|,preview,|,table",
                theme_advanced_buttons2: "",
                theme_advanced_buttons3: "",
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
            <%} %>

             $('#service_Contain').tinymce({
                // Location of TinyMCE script
                script_url: '/Scripts/tiny_mce/tiny_mce.js',
                width: '622',
                height: '300',
                // General options
                theme: "advanced",
                language: 'cn',
                plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,advlist",

                // Theme options
               theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,fontselect,fontsizeselect,|,forecolor,backcolor,|,image,preview,|,table",
                theme_advanced_buttons2: "",
                theme_advanced_buttons3: "",
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
             <%if(servertype!=14){ //非保险产品加载以下编辑器%>
             $('#Precautions').tinymce({
                // Location of TinyMCE script
                script_url: '/Scripts/tiny_mce/tiny_mce.js',
                width: '622',
                height: '300',
                // General options
                theme: "advanced",
                language: 'cn',
                plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,advlist",

                // Theme options
                theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,fontselect,fontsizeselect,|,forecolor,backcolor,|,preview,|,table",
                theme_advanced_buttons2: "",
                theme_advanced_buttons3: "",
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
             $('#service_NotContain').tinymce({
                // Location of TinyMCE script
                script_url: '/Scripts/tiny_mce/tiny_mce.js',
                width: '622',
                height: '300',
                // General options
                theme: "advanced",
                language: 'cn',
                plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,advlist",

                // Theme options
                theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,fontselect,fontsizeselect,|,forecolor,backcolor,|,preview,|,table",
                theme_advanced_buttons2: "",
                theme_advanced_buttons3: "",
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
            <%} %>

            $("#GoProAddNext").click(function () {//确认提交按钮 
                var imgurl = $("#<%=headPortrait.FileUploadId_ClientId %>").val();
                if (imgurl == "") {
                    imgurl = $("#hid_imgurl").trimVal();
                }

                var MultiImgUpIds=$("#<%=childImg.FileUploadId_ClientId %>").val();
                

                var pro_name = $("#pro_name").trimVal();
                var merchant_code = $("#merchant_code").trimVal();
                
                var server_type = $("#server_type").trimVal();
                var pro_type = $("#pro_type").trimVal();
                var source_type = $('input:radio[name="source_type"]:checked').trimVal();
                var pro_Remark = $("#pro_Remark").trimVal();
                var proclass = $("#proclass").trimVal();
                var pro_start = $("#pro_start").trimVal();
                var pro_end = $("#pro_end").trimVal();
                var tuipiao = $("#tuipiao").trimVal();
                var tuipiao_guoqi = 0;
                var tuipiao_endday = $("#tuipiao_endday").trimVal();
                var face_price = $("#face_price").trimVal();
                var advise_price = $("#advise_price").trimVal();
                var agent1_price = $("#agent1_price").trimVal();
                var agent2_price = $("#agent2_price").trimVal();
                var agent3_price = $("#agent3_price").trimVal();
                var agentsettle_price = $("#agentsettle_price").trimVal();
                var tuan_pro = $('input:radio[name="tuan_pro"]:checked').trimVal();
                var zhixiao = $('input:radio[name="zhixiao"]:checked').trimVal();
                var agentsale = $('input:radio[name="agentsale"]:checked').trimVal();
                var ThatDay_can = $('input:radio[name="ThatDay_can"]:checked').trimVal();
                var Thatday_can_day = $("#Thatday_can_day").trimVal();
                var userid = $("#hid_userid").trimVal();
                var comid = $("#hid_comid").trimVal();
                var service_Contain = $("#hid_servicecontain").trimVal();
                var service_NotContain = $("#hid_servicenotcontain").trimVal();
                var Precautions = $("#hid_precautions").trimVal();
                var pro_Integral = $("#Integral_price").trimVal(); //产品加积分
                var travelproductid = $("#travelproductid").trimVal();
                //           var traveltype = $("#traveltype").trimVal();//多余参数
                var travelstarting = $("#travelstarting").trimVal();
                var travelending = $("#travelending").trimVal();
                var serviceid = $("#selservice").trimVal();
                var service_proid = $("#txtservice_proid").trimVal();
                var pro_Number = $("#pro_Number").trimVal();
                var pro_Explain = $("#pro_Explain").trimVal();
                var service_Contain = $("#service_Contain").trimVal();
                var service_NotContain = $("#service_NotContain").trimVal();
                var Precautions = $("#Precautions").trimVal();
                var Sms = $("#Sms").trimVal();

                var ishasdeliveryfee=$("input:radio[name='ishasdeliveryfee']:checked").trimVal();
                var deliverytmp=$("#sel_deliverytmp").trimVal();
                var pro_weight = $("#pro_weight").trimVal();
                var unsure = $("#unsure").trimVal();
                var unyuyueyanzheng = $("#unyuyueyanzheng").trimVal();

                var worktimehour = $("#worktimehour").trimVal();
                var worktimeid = $("#worktimeid").trimVal();
                
                var isSetVisitDate = $('input:radio[name="isSetVisitDate"]:checked').trimVal();
                var zhaji_usenum= $("#zhaji_usenum").trimVal();
              
                if ($("#tuipiao_guoqi").attr("checked") == true) {
                    tuipiao_guoqi = $("#tuipiao_guoqi").trimVal();
                }

                if(server_type==1||server_type==8||server_type==10)//服务类型是票务或者当地游或者旅游大巴的话，则判断验证有效期和产品当天是否可用,否则不处理
                {
                   var validmethod=  $("input:radio[name='ProValidateMethod']:checked").val();
                   if(validmethod==2)
                   {
                     if($("#sel_appointdata").val()==0)
                     {
                       alert("请指定验证有效期");
                       return;
                     }
                   }
                }
                else
                {
                  $("input:radio[name='ProValidateMethod'][value='1']").attr("checked",true);
                  $("#div_appointdata").val("0");
                  $("#sel_iscanuseonsameday").val("1");
                }

                if (server_type == "1" || server_type == "2" || server_type == "8"|| server_type == "10"|| server_type == "11")//服务类型为电子票或者旅游或旅游大巴
                {

                    if (pro_name == '') {
                        $.prompt('产品名称不可为空');
                        return;
                    } 
                    if (source_type == '') {
                        $.prompt('请选择出票来源！');
                        return;
                    }
                    else {
                        if (source_type == "3") {
                            if ($("#txtservice_proid").val() == "") {
                                $.prompt("请填写服务商产品编号");
                                return;
                            }
                        }
                        else {
                            serviceid = 0;
                            service_proid = "";
                        }
                    }
                    if (pro_Number == '') {
                       pro_Number="0";
                    } else {
                        if (isNaN($("#pro_Number").Amount())) {
                            $.prompt("产品数量格式不正确");
                            return;
                        }
                    } 
                    if (pro_start == '') {
                        $.prompt('产品开始日期不可为空');
                        return;
                    }
                    if (pro_end == '') {
                        $.prompt('产品截止日期不可为空');
                        return;
                    }
                    if (tuipiao_guoqi != '') {
                        if (tuipiao_endday == '') {
                            alert(tuipiao_guoqi);
                            $.prompt('您选择了允许过期退票，请选择退票时限');
                            return;
                        }
                  }
                  if(manyspeci==0){//单规格产品判断；多规格不判断
                        if (face_price == '') {
                            $.prompt('门市价格不可为空');
                            return;
                        }

                        if (!$("#face_price").Amount()) {
                            $.prompt('门市价格格式不对');
                            return;
                        }
                        if (advise_price == '') {
                            $.prompt('建议销售价不可为空');
                            return;
                        }

                        if (!$("#advise_price").Amount()) {
                            $.prompt('建议销售价格式不对');
                            return;
                        }
                        if (agentsettle_price == '') {
                            $.prompt('结算价格不可为空');
                            return;
                        }
                        //结算价格格式 
                        var jiesuanreg = new RegExp("^\\d+(?:\\.\\d+)?$", "g");
                        if(!jiesuanreg.test(agentsettle_price)){
                          alert("结算价格格式不正确");
                          return;
                        } 
                   }
                    if (ThatDay_can == '0') {
                        if (Thatday_can_day == '') {
                            $.prompt('提前天数不可为空');
                            return;
                        }
                        if (Thatday_can_day == '0') {
                            $.prompt('提前天数不可为0');
                            return;
                        }
                        if (!$("#ThatDay_can").Amount()) {
                            $.prompt('提前天数格式不对');
                            return;
                        }
                    } 
                }
                if (server_type == "2" || server_type == "8")//服务类型为旅游
                {
                    if(server_type!="10"){
                        if (travelproductid == "") {
                            $.prompt("请录入产品编号");
                            return;
                        }
                    }else
                    {
                      $("#travelproductid").val(0);
                    }
                    if (travelstarting == "") {
                        $.prompt("请录入出发地");
                        return;
                    }
                    if (travelending == "") {
                        $.prompt("请录入目的地");
                        return;
                    }
                }
                var pickuppoint = $("#pickuppoint").trimVal();
                var dropoffpoint = $("#dropoffpoint").trimVal();
                var firststationtime=$("#firststationtime").trimVal();
                 if (server_type == "10")//服务类型为“旅游大巴”，判断上车地点 和 下车地点
                 {
                   if(pickuppoint=="")
                   {
                     alert("上车地点不可为空");
                     return;
                   }
//                    if(dropoffpoint=="")
//                   {
//                     alert("下车地点不可为空");
//                     return;
//                   }
                    //判断首发站发车时间是否符合要求 
                    if(firststationtime=="")
                    {
                     alert("请录入首发站发车时间");
                     return;
                    }
                    else{
                      //检查发车时间格式：需要形如 07:30:00
                      firststationtime=firststationtime.replace('：',':');
                   
                      if(firststationtime.search(/^\d{2}:\d{2}:\d{2}/)==-1){
                        alert("发车时间格式错误");
                        return;
                      }
                    }
                 }

                  var pro_note = $("#pro_note").trimVal();
                //房型信息验证
                var bedtype = $("#bedtype").trimVal();
                var bedwidth = $("#bedwidth").trimVal();
                var whetherextrabed = $("input:radio[name='whetherextrabed']:checked").val();
                var extrabedprice = $("#extrabedprice").trimVal();
                var ReserveType = $("#ReserveType").val();
                var wifi = $("#wifi").trimVal();
                var Breakfast = $("#Breakfast").val();
                var Sms = $("#Sms").trimVal();
                var Builtuparea = $("#Builtuparea").trimVal();
                var floor = $("#floor").trimVal();
                var largestguestnum = $("#largestguestnum").trimVal();
                var whethernonsmoking = $('input:radio[name="whethernon-smoking"]:checked').val();
                var amenities = $("#amenities").trimVal();
                var Mediatechnology = $("#Mediatechnology").trimVal();
                var Foodanddrink = $("#Foodanddrink").trimVal();
                var ShowerRoom = $("#ShowerRoom").trimVal();
                var roomtyperemark = $("#roomtyperemark").trimVal();

                if (server_type == "9")//服务类型为房型
                {
                    if(source_type !="4"){
                    if (bedtype == "") {
                        $.prompt("床型不可为空!");
                        return;
                    }
                    if (bedwidth == "") {
                        $.prompt("床宽不可为空!");
                        return;
                    }
                    if (whetherextrabed == "true") {
                        if (extrabedprice == "") {
                            $.prompt("请填写加床费用");
                            return;
                        } else {
                            if (isNaN($("#extrabedprice").trimVal())) {
                                $.prompt("加床费用请输入数字");
                                return;
                            }
                        }
                    }
                    if (wifi == "") {
                        $.prompt("请填写wifi情况");
                        return;
                    }
                    if (largestguestnum == "") {
                        $.prompt("请填写最多入住人数");
                        return;
                    }
                    else {
                        if (isNaN($("#largestguestnum").trimVal())) {
                            $.prompt("最多入住人数请输入数字");
                            return;
                        }
                    }
                    if (isMobel($("#RecerceSMSPhone").trimVal()) == false) {
                        alert("接收预定短信人员电话格式不正确");
                        return;
                    }


                    if(face_price==""){
                        alert("请填写默认显示门市价");
                        return;
                    }
                    if(advise_price==""){
                        alert("请填写默认显示销售价");
                        return;
                    }



                    }

                }

                //添加抢购
                var ispanicbuy=  $('input:radio[name="ispanicbuy"]:checked').trimVal();
                var panic_begintime=$("#panicbuy_begintime").val();
                var panicbuy_endtime=$("#panicbuy_endtime").val();
                
                if(panic_begintime!=""&&panicbuy_endtime!=""){
                if(dateDiff_seconds(panicbuy_endtime,panic_begintime)<0)
                {
                  alert("抢购结束时间需要大于开始时间");
                  return;
                }
                }
                var panicbuy_limitbuytotalnum=$("#panicbuy_limitbuytotalnum").val();
                if(parseInt(panicbuy_limitbuytotalnum)<0)
                {
                alert("剩余可销售数量必须不小于0");
                return;
                }

                //产品预约  
                var isneedbespeak= $('input:radio[name="isneedbespeak"]:checked').trimVal();
                var daybespeaknum=$("#daybespeaknum").trimVal();
                var bespeaksucmsg=$("#bespeaksucmsg").trimVal();
                var bespeakfailmsg=$("#bespeakfailmsg").trimVal();
                var customservicephone=$("#customservicephone").trimVal();
                //限制使用日期
                var isblackoutdate=$('input:radio[name="isblackoutdate"]:checked').trimVal();
                var etickettype=$('input:radio[name="etickettype"]:checked').trimVal();
                
                if(server_type == 1)//票务
                {
                   if(isneedbespeak==1)//需要预约
                   {
                      if(daybespeaknum=="")
                      {
                       alert("每日预约人数不可为空");
                        return;
                      }
                      if(isNaN(daybespeaknum))
                      {
                        alert("每日预约人数格式必须为数字");
                        return;
                      }
                      if(parseInt(daybespeaknum)==0)
                      {
                         alert("每日预约人数不可为0");
                        return;
                      }
                      if(bespeaksucmsg=="")
                      {
                         alert("预约成功短信不可为空");
                        return;
                      }
                      if(bespeakfailmsg=="")
                      { alert("预约失败短信不可为空");
                        return;
                        
                      }
                      if(customservicephone=="")
                      {
                         alert("预约处理人电话不可为空");
                        return;
                      }
                   }
                  
                }

                if(server_type==11)// 实物商品配送费用
                {
                    //实物重量 
                    var floatreg = new RegExp("^\\d+(?:\\.\\d+)?$", "g");
                    if(!floatreg.test(pro_weight)){
                      alert("重量格式不正确");
                      return;
                    } 
                    

                    if(ishasdeliveryfee==1)//不包邮
                    {
                       if(deliverytmp==0||deliverytmp=="")
                       {
                          var sourcetype = $("input:radio[name='source_type']:checked").val();
                          if(sourcetype!=4){
                              alert("请建立配送模板");
                              return;
                          }
                       }
                    }else
                    {
                    deliverytmp==0
                    }
                }

                var bookpro_bindname=$("#txt_bookpro_bindname").trimVal();
                var bookpro_bindphone=$("#txt_bookpro_bindphone").trimVal();
                var bookpro_bindcompany=$("#txt_bookpro_bindcompany").trimVal();


                if(server_type==9){
                     bookpro_bindname=$("#RecerceSMSName").val();
                     bookpro_bindphone=$("#RecerceSMSPhone").val();
                     bookpro_bindcompany=$("#RecerceSMSCompany").val();
                }

                


                var bookpro_ispay=$("#sel_bookpro_ispay").trimVal();
                if(server_type == 12 || server_type == 13)//预约产品
                {
                   if(bookpro_bindname=="")
                   {
                     alert("产品关联人姓名不可为空");
                     return;
                   }
                   if(bookpro_bindphone=="")
                   {
                     alert("产品关联人手机不可为空");
                     return;
                   }
                  
                }

                //多规格
                var manyspeci=$("input:radio[name='manyspeci']:checked").trimVal();
                var guigestr=""; 
                if(manyspeci==1){//多规格
                  
                  //保险产品 规格中必须包含规格类型:承保年龄;保障期限;购买份数
                  var ishaschengbao=0;
                  var ishasqixian=0;
                  var ishasfenshu=0;

                  $("#process").find("tbody tr").each(function(i,item){
                       var guigetitle=$.trim($(item).attr("data-id"));
                        
                       if(guigetitle.indexOf("承保年龄")>-1){
                          ishaschengbao=1;
                       }
                       if(guigetitle.indexOf("保障期限")>-1){
                          ishasqixian=1;
                       }
                       if(guigetitle.indexOf("购买份数")>-1){
                          ishasfenshu=1;
                       }

                       var guigevalue=$(item).find("input[name='Txt_facePriceSon']").val()+"-"+$(item).find("input[name='Txt_advicePriceSon']").val()+"-"+$(item).find("input[name='Txt_agent1PriceSon']").val()+"-"+$(item).find("input[name='Txt_agent2PriceSon']").val()+"-"+$(item).find("input[name='Txt_agent3PriceSon']").val()+"-"+$(item).find("input[name='Txt_settlePriceSon']").val()+"-"+$(item).find("input[name='Txt_WeightSon']").val()+"-"+$(item).find("input[name='Txt_CountSon']").val();
                       guigestr+=guigetitle+"-"+guigevalue+"|";
                  })
                  if(guigestr.length>0){
                    guigestr=guigestr.substring(0,guigestr.length-1);
                    if(server_type==14){
                        if(ishaschengbao==0||ishasqixian==0||ishasfenshu==0){
                          alert("保险产品规格类型至少包含:承保年龄;保障期限;购买份数");
                          return;
                        }
                    }
                  }else{
                    alert("请设置规格信息");
                    return;
                  }
                }
//                alert(guigestr);
//                return ;
                var isrebate=$("#sel_isrebate").trimVal();

                var selbindbx=$("#selbindbx").val();

                var Wrentserver = $("input:radio[name='Wrentserver']:checked").val();
                var WDeposit = $("input:radio[name='WDeposit']:checked").val();
                var Depositprice=$("#Depositprice").val();
//                if(WDeposit==1){
//                    if(Depositprice<=0){
//                    
//                     alert("需要支付押金时，请填写押金金额");
//                     return ;
//                    
//                    }
//                }


                var Rentserverid = "";


                $("input:[name='Rentserverid']:checked").each(function(){
                        Rentserverid += $(this).val() + ",";

                 }); 

                 var bandingzhajiid = "";


                $("input:[name='bandingzhajiid']:checked").each(function(){
                        bandingzhajiid += $(this).val() + ",";

                 }); 


                if(Wrentserver==1){
                if(WDeposit==1 || WDeposit==2){
                    if(Depositprice=="" || Depositprice=="0"){
//                         alert("如果选择需要押金，请填写押金金额！");
//                          return;
                      Depositprice="0";
                    }
                }
                }
                //发卡服务，必须选择时长
//                 if(Wrentserver=="1")
//                 {
//                   if($("#worktimehour").val()=="")
//                   {
//                     alert("发卡服务，请选择时长");
//                     return ;
//                   }
//                 }

                 //产品预订一张产生电子码个数
                 var pnonumperticket=$("#txt_pnonumperticket").trimVal();
                 if(isNaN(pnonumperticket)){
                    alert("产品预订一张产生电子码个数需要为数字");
                    return;
                 }
                  //产品指定验证POSID
                   var SpecifyPosid=$("#SpecifyPosid").trimVal();

                   var progroupid=$("#sel_progroupid").trimVal();

                 var issetidcard=$("input:radio[name='issetidcard']:checked").trimVal();
               

                $.post("/JsonFactory/ProductHandler.ashx?oper=editprobasic", {issetidcard:issetidcard, progroupid:progroupid, SpecifyPosid:SpecifyPosid, pnonumperticket:pnonumperticket,firststationtime:firststationtime,pro_yanzheng_method:$("#pro_yanzheng_method").val(),selbindbx:selbindbx, isrebate:isrebate,manyspeci:manyspeci,guigestr:guigestr,bookpro_bindname:bookpro_bindname,bookpro_bindphone:bookpro_bindphone,bookpro_bindcompany:bookpro_bindcompany,bookpro_ispay:bookpro_ispay,   pro_weight:pro_weight, MultiImgUpIds:MultiImgUpIds,  ishasdeliveryfee:ishasdeliveryfee,deliverytmp:deliverytmp,  isneedbespeak:isneedbespeak,daybespeaknum:daybespeaknum,bespeaksucmsg:bespeaksucmsg,bespeakfailmsg:bespeakfailmsg,customservicephone:customservicephone,isblackoutdate:isblackoutdate,etickettype:etickettype,  QuitTicketMechanism:$("#sel_QuitTicketMechanism").trimVal(),  pro_note:pro_note,pickuppoint:pickuppoint,dropoffpoint:dropoffpoint, childreduce:$("#childreduce").trimVal(), ProValidateMethod:$("input:radio[name='ProValidateMethod']:checked").val(),appointdata:$("#sel_appointdata").val(),  iscanuseonsameday:$("#sel_iscanuseonsameday").val(),viewmethod:$("#sel_viewmethod").val(),linepro_booktype:$("#linepro_booktype").val(),ispanicbuy:ispanicbuy, panic_begintime:panic_begintime,panicbuy_endtime:panicbuy_endtime,panicbuy_limitbuytotalnum:panicbuy_limitbuytotalnum,  travelproductid: travelproductid, traveltype: 0, travelstarting: travelstarting, travelending: travelending, projectid: $("#selproject").val(), realnametype: $("#selreal_name_type").val(), serviceid: serviceid, service_proid: service_proid, service_Contain: service_Contain, service_NotContain: service_NotContain, Precautions: Precautions, Sms: Sms, prostate: $("#selprostate").val(), pro_Number: pro_Number, pro_Explain: pro_Explain, imgurl: imgurl,  pro_id: hid_proid, pro_name: pro_name, server_type: server_type, pro_type: pro_type, source_type: source_type, pro_remark: pro_Remark, pro_start: pro_start, pro_end: pro_end, face_price: face_price, advise_price: advise_price, agent1_price: agent1_price, agent2_price: agent2_price, agent3_price: agent3_price, agentsettle_price: agentsettle_price, tuan_pro: tuan_pro, zhixiao: zhixiao, agentsale: agentsale, ThatDay_can: ThatDay_can, Thatday_can_day: Thatday_can_day, userid: userid, comid: comid, pro_Integral: pro_Integral, tuipiao: tuipiao, tuipiao_guoqi: tuipiao_guoqi, tuipiao_endday: tuipiao_endday, proclass: proclass,unsure:unsure,unyuyueyanzheng:unyuyueyanzheng,Wrentserver:Wrentserver,WDeposit:WDeposit,Depositprice:Depositprice,Rentserverid:Rentserverid,worktimeid:worktimeid,worktimehour:worktimehour,isSetVisitDate:isSetVisitDate,bandingzhajiid:bandingzhajiid,zhaji_usenum:zhaji_usenum,merchant_code:merchant_code  }, function (data) {
                    data = eval('(' + data + ')');
                    if (data.type == '100') {

                        if (server_type == "9")//房型信息,需要继续编辑房型扩展表信息
                        {
                            $.post("/JsonFactory/ProductHandler.ashx?oper=edithousetype", { proid: data.msg, RecerceSMSName: $("#RecerceSMSName").trimVal(), RecerceSMSPhone: $("#RecerceSMSPhone").trimVal(),  comid: $("#hid_comid").trimVal(), bedtype: bedtype, bedwidth: bedwidth, whetherextrabed: whetherextrabed, extrabedprice: extrabedprice, ReserveType: ReserveType, wifi: wifi, Breakfast: Breakfast, Builtuparea: Builtuparea, floor: floor, largestguestnum: largestguestnum, whethernonsmoking: whethernonsmoking, amenities: amenities, Mediatechnology: Mediatechnology, Foodanddrink: Foodanddrink, ShowerRoom: ShowerRoom, roomtyperemark: roomtyperemark }, function (data11) {
                                data11 = eval('(' + data11 + ')');
                                if (data11.type == '100') {
                                    $.prompt("产品发布成功", {
                                        buttons: [
                                                 { title: '确定', value: true }
                                                ],
                                        submit: function (e, v, m, f) {
                                            if (v == true)
                                            if(source_type!=4){
                                                location.href = "/ui/pmui/travel/linegroupdate.aspx?lineid=" + data.msg;   
                                                }else{
                                                 location.href = "productlist.aspx";
                                                }          }
                                    });
                                    return;
                                } 
                                else {
                                    $.prompt("产品信息编辑失败");
                                    return;
                                }
                            })
                        } 
                        else {
                            $.prompt("产品发布成功", {
                                buttons: [
                                 { title: '确定', value: true }
                                ],
                                submit: function (e, v, m, f) {
                                    if (v == true) {
                                        if ($("#server_type").val() == 1||$("#server_type").val() == 11||$("#server_type").val() == 3||$("#server_type").val() == 12||$("#server_type").val() == 13||$("#server_type").val() == 14) {//门票
                                            location.href = "ProductList.aspx?projectid=" + $("#hid_projectid").trimVal();
                                        } else if($("#server_type").val()==10){//旅游大巴

                                            if(source_type!=4){
                                                location.href = "/ui/pmui/travel/linegroupdate.aspx?lineid=" + data.msg;
                                            }
                                        } 
                                        else{//旅游 
                                            if(source_type==15){
                                                location.href = "/ui/pmui/PackageBindingpro.aspx?id=" + data.msg;
                                            }

                                            if(source_type!=4){
                                                location.href = "/ui/pmui/travel/linetrip.aspx?lineid=" + data.msg;
                                            }

                                            
                                        }
                                    }
                                }
                            });
                            return;
                        }
                    } else {
                        $.prompt("产品信息编辑失败");
                        return;
                    }
                })

            })

            $("input:radio[name='source_type']").change(function () {
                var sourcetype = $("input:radio[name='source_type']:checked").val();
                if (sourcetype == "3") {
                    $("#trservice").show();
                    $("#trservice_proid").show();
                    $("#trrealnametype").show();
                } else {
                    $("#trservice").hide();
                    $("#trservice_proid").hide();
                    $("#trrealnametype").hide();
                }
                if(sourcetype=="1")
                {
                   $("#txt_pnonumperticket").removeAttr("disabled").css("background-color", "#FFFFFF");
                }else{
                   $("#txt_pnonumperticket").val("1").attr("disabled","disabled").css("background-color", "#cccccc");
                }
            })

            //根据服务类型 显示产品内容
            $("#server_type").change(function () {
                var servertype = $("#server_type").trimVal();
                ViewByServerType(servertype);
            })

            //产品验证有效期
            $("input:radio[name='ProValidateMethod']").change(function(){
                
                var validmethod=$("input:radio[name='ProValidateMethod']:checked").val();
                if(validmethod==1){//按产品有效期
                $("#div_appointdata").hide();
                }else//按指定有效期
                {
                 $("#div_appointdata").show();
                }
              
            })

            $("#panicbuy_limitbuytotalnum").blur(function(){
               if(isNaN($("#panicbuy_limitbuytotalnum").val()))
               {
                alert("可销售数量必须为数字");
                return;
               }

               $("#saletotalnum").text("销售总量:"+ parseInt( parseInt           ($("#panicbuy_limitbuytotalnum").val())+parseInt($("#buynum").val())));
            })

            
             $(".selectspeci").click(function () {//更换多规格单规格
                selectspeci();
             });

             //多规格▼
             $("input:checkbox[name='ckguige']").live("change", function () {
                    step.Creat_Table();
             })
              
        var step = {
          //SKU信息组合
           Creat_Table: function () {
            step.hebingFunction();
            var SKUObj = $(".show_standard_wrp");
            // var skuCount = SKUObj.length; //规格组数
            var arrayTile = new Array(); //标题组数
            var arrayInfor = new Array(); //盛放每组选中的CheckBox值的对象
            var arrayColumn = new Array(); //指定列，用来合并哪些列
            var bCheck = true; //是否全选
            var columnIndex = 0;
            $.each(SKUObj, function (i, item) {
                arrayColumn.push(columnIndex);
                columnIndex++;

                arrayTile.push($(item).next().attr("data-name"));
                var itemName = "js_wrp_level" + $(item).next().attr("data-index");
                //选中的CHeckBox取值
                var order = new Array();
                $("." + itemName + " input[type=checkbox]:checked").each(function () {
                    order.push($(this).val());
                });
                arrayInfor.push(order);
                if (order.join() == "") {
                    bCheck = false;
                }
                //var skuValue = SKUObj.find("li").eq(index).html();
            });
            //开始创建Table表
            if (bCheck == true) {
                var RowsCount = 0;
                $("#createTable").html("");
                var table = $("<table id=\"process\" border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"width:100%;padding:5px;\"></table>");
                table.appendTo($("#createTable"));
                var thead = $("<thead></thead>");
                thead.appendTo(table);
                var trHead = $("<tr></tr>");
                trHead.appendTo(thead);
                //创建表头
                $.each(arrayTile, function (index, item) {
                    var td = $("<th>" + item + "</th>");
                    td.appendTo(trHead);
                });
                var itemColumHead = $("<th  style=\"width:360px;\">门市价--直销价--1级分销价--2级分销价--3级分销价--结算价</th><th style=\"width:30px; display:none;\">重量</th><th style=\"width:30px;\">库存</th> ");
                itemColumHead.appendTo(trHead);
                //var itemColumHead2 = $("<td >商家编码</td><td >商品条形码</td>");
                //itemColumHead2.appendTo(trHead);
                var tbody = $("<tbody></tbody>");
                tbody.appendTo(table);
                ////生成组合
                var zuheDate = step.doExchange(arrayInfor);
                if (zuheDate.length > 0) {
                    //创建行
                    $.each(zuheDate, function (index, item) {
                        var td_array = item.split(",");

                        var thdataid = ""; //行标识
                        var thtitle = new Array();
                        var thvalue = new Array();
                        $.each(arrayTile, function (ii, itemii) {
                            thtitle.push(itemii);
                        });
                        $.each(td_array, function (iii, valuesiii) {
                            thvalue.push(valuesiii);
                        });
                        for (var y = 0; y < thtitle.length; y++) {
                            if (thtitle[y] != "") {
                                thdataid += $.trim(thtitle[y]) + ":" + $.trim(thvalue[y]) + ";";
                            }
                        }
                        if (thdataid.length > 0) {
                            thdataid = thdataid.substring(0, thdataid.length - 1);
                        }

                        var tr = $("<tr data-id='" + thdataid + "'></tr>");
                        tr.appendTo(tbody);
                        $.each(td_array, function (i, values) {
                            var td = $("<td>" + values + "</td>");
                            td.appendTo(tr);
                        });
                        var td1 = $("<td ><input name=\"Txt_facePriceSon\" class=\"l-text\" type=\"text\" value=\"\" size='4' placeholder='门市价'>-" +
                        "<input name=\"Txt_advicePriceSon\" class=\"l-text\" type=\"text\" value=\"\" size='4' placeholder='直销价'>-" +
                        "<input name=\"Txt_agent1PriceSon\" class=\"l-text\" type=\"text\" value=\"\" size='4' placeholder='1级分销价'>-" +
                        "<input name=\"Txt_agent2PriceSon\" class=\"l-text\" type=\"text\" value=\"\" size='4' placeholder='2级分销价'>-" +
                        "<input name=\"Txt_agent3PriceSon\" class=\"l-text\" type=\"text\" value=\"\" size='4' placeholder='3级分销价'>-" +
                        "<input name=\"Txt_settlePriceSon\" class=\"l-text\" type=\"text\" value=\"\" size='4' placeholder='结算价'></td>");
                        td1.appendTo(tr);
                        var td2 = $("<td style='display:none;'><input name=\"Txt_WeightSon\" class=\"l-text\" type=\"text\" value=\"0\"  size='5'></td>");
                        td2.appendTo(tr);
                        var td3 = $("<td ><input name=\"Txt_CountSon\" class=\"l-text\" type=\"text\" value=\"\"  size='4'></td>");
                        td3.appendTo(tr);
                        //var td4 = $("<td ><input name=\"Txt_SnSon\" class=\"l-text\" type=\"text\" value=\"\"></td>");
                        //td4.appendTo(tr);
                    });
                }
                //结束创建Table表
                arrayColumn.pop(); //删除数组中最后一项
                //合并单元格
                $(table).mergeCell({
                    // 目前只有cols这么一个配置项, 用数组表示列的索引,从0开始
                    cols: arrayColumn
                });
            } 
            else {
                //未全选中,清除表格
                document.getElementById('createTable').innerHTML = "";
            }
        }, //合并行
                hebingFunction: function () {
            $.fn.mergeCell = function (options) {
                return this.each(function () {
                    var cols = options.cols;
                    for (var i = cols.length - 1; cols[i] != undefined; i--) {
                        // fixbug console调试
                        // console.debug(cols[i]);
                        mergeCell($(this), cols[i]);
                    }
                    dispose($(this));
                });
            };
            function mergeCell($table, colIndex) {
                $table.data('col-content', ''); // 存放单元格内容
                $table.data('col-rowspan', 1); // 存放计算的rowspan值 默认为1
                $table.data('col-td', $()); // 存放发现的第一个与前一行比较结果不同td(jQuery封装过的), 默认一个"空"的jquery对象
                $table.data('trNum', $('tbody tr', $table).length); // 要处理表格的总行数, 用于最后一行做特殊处理时进行判断之用
                // 进行"扫面"处理 关键是定位col-td, 和其对应的rowspan
                $('tbody tr', $table).each(function (index) {
                    // td:eq中的colIndex即列索引
                    var $td = $('td:eq(' + colIndex + ')', this);
                    // 取出单元格的当前内容
                    var currentContent = $td.html();
                    // 第一次时走此分支
                    if ($table.data('col-content') == '') {
                        $table.data('col-content', currentContent);
                        $table.data('col-td', $td);
                    } else {
                        // 上一行与当前行内容相同
                        if ($table.data('col-content') == currentContent) {
                            // 上一行与当前行内容相同则col-rowspan累加, 保存新值
                            var rowspan = $table.data('col-rowspan') + 1;
                            $table.data('col-rowspan', rowspan);
                            // 值得注意的是 如果用了$td.remove()就会对其他列的处理造成影响
                            $td.hide();
                            // 最后一行的情况比较特殊一点
                            // 比如最后2行 td中的内容是一样的, 那么到最后一行就应该把此时的col-td里保存的td设置rowspan
                            if (++index == $table.data('trNum'))
                                $table.data('col-td').attr('rowspan', $table.data('col-rowspan'));
                        } else { // 上一行与当前行内容不同
                            // col-rowspan默认为1, 如果统计出的col-rowspan没有变化, 不处理
                            if ($table.data('col-rowspan') != 1) {
                                $table.data('col-td').attr('rowspan', $table.data('col-rowspan'));
                            }
                            // 保存第一次出现不同内容的td, 和其内容, 重置col-rowspan
                            $table.data('col-td', $td);
                            $table.data('col-content', $td.html());
                            $table.data('col-rowspan', 1);
                        }
                    }
                });
            }
            // 同样是个private函数 清理内存之用
            function dispose($table) {
                $table.removeData();
            }
        },
               //组合数组
                doExchange: function (doubleArrays) {
            var len = doubleArrays.length;
            if (len >= 2) {
                var arr1 = doubleArrays[0];
                var arr2 = doubleArrays[1];
                var len1 = doubleArrays[0].length;
                var len2 = doubleArrays[1].length;
                var newlen = len1 * len2;
                var temp = new Array(newlen);
                var index = 0;
                for (var i = 0; i < len1; i++) {
                    for (var j = 0; j < len2; j++) {
                        temp[index] = arr1[i] + "," + arr2[j];
                        index++;
                    }
                }
                var newArray = new Array(len - 1);
                newArray[0] = temp;
                if (len > 2) {
                    var _count = 1;
                    for (var i = 2; i < len; i++) {
                        newArray[_count] = doubleArrays[i];
                        _count++;
                    }
                }
                //console.log(newArray);
                return step.doExchange(newArray);
            }
            else {
                return doubleArrays[0];
            }
        }
             }
             return step;
             //多规格▲
        })
        //显示绑定保险产品列表
        function ViewBaoxian(seled){
          $.post("/JsonFactory/ProductHandler.ashx?oper=getbaoxianlist",{comid:$("#hid_comid").trimVal()},function(data){
            data=eval("("+data+")");
            if(data.type==1){
               var bxstr='<option value="0">不绑定</option>'; 
               $("#selbindbx").append(bxstr);
            }
            if(data.type==100){
               var bxstr='<option value="0">不绑定</option>';
               for(var i=0;i<data.msg.length;i++)
               {
                   if(data.msg[i].Id==seled){
                      bxstr+='<option value="'+data.msg[i].Id+'" selected="selected">'+data.msg[i].Pro_name+'</option>';
                   }else{
                      bxstr+='<option value="'+data.msg[i].Id+'" >'+data.msg[i].Pro_name+'</option>';
                   }
               }
               $("#selbindbx").append(bxstr);
            }
          })
        }
        function ViewByServerType(servertype) {
            
            if (servertype == 2)//跟团游
            {
                $("#div_ProValidateMethod").hide();
                $("#div_appointdata").hide();
                $("#div_iscanuseonsameday").hide();

                $("#tbody_roomtype").hide();
                $("#tbody_product").show();

                 $("#childreduce").parent().show();
                $("#travelproductid").parent().show();
                $("#travelstarting").parent().show();
                $("#travelending").parent().show();

                $("#pro_type").parent().hide(); //验证类型
                $("#pro_Number").parent().hide(); //限购数量
                //$("#tuipiao").parent().hide();


                $("#pro_Explain").parent().hide();
//                $("#pro_start").parent().hide();

                $("#agent1_price").parent().show();
                $("#agent2_price").parent().show();
                $("#agent3_price").parent().show();
                $("#agent1_price").parent().find("label").html("一级分销价");
                $("#agent2_price").parent().find("label").html("二级分销价");
                $("#agent3_price").parent().find("label").html("三级分销价");

                $("#Sms").parent().show();

                $("#div_linepro_booktype").show();

                $("#div_pro_Remark").show();

                //旅游大巴产品 上车地点下车地点
                $("#pickuppoint").parent().hide();
                $("#dropoffpoint").parent().hide();
                //纸质票备注说明
                 $("#div_pro_note").hide();
                      //票务退票机制 
                 $("#div_QuitTicketMechanism").hide();
                 
                 //是否参加抢购
                 $("#div_ispanicbuy").show();
                   //是否需要预约
                 $("#div_isneedbespeak").hide();
                   //是否限定使用日期
                 $("#div_isblackoutdate").hide();
                      //是否显示配送费用
                 $("#div_deliveryfee").hide();

                   $("#pro_weight").parent().hide(); //重量

                //只有票务，实物,优惠劵 需要设置限购，其他类型产品 在团期中设置限购数量
                $("input:radio[name='ispanicbuy'][value='2']").parent().hide();

                //多规格只有实物、票务、保险显示，其他隐藏
                $("input:radio[name='manyspeci'][value='1']").parent().hide();
            }
            else if (servertype == 8)//当地游
            {
//                $("#div_ProValidateMethod").show();
//                $("#div_appointdata").show();
//                $("#div_iscanuseonsameday").show();

                $("#tbody_roomtype").hide();
                $("#tbody_product").show();

                $("#childreduce").parent().show();
                $("#travelproductid").parent().show();
                $("#travelstarting").parent().show();
                $("#travelending").parent().show();

                $("#pro_type").parent().show(); //验证类型
                $("#pro_Number").parent().hide(); //限购数量
                //$("#tuipiao").parent().hide();

                $("#pro_Explain").parent().hide();
//                $("#pro_start").parent().show();

                $("#agent1_price").parent().show();
                $("#agent2_price").parent().show();
                $("#agent3_price").parent().show();
                $("#agent1_price").parent().find("label").html("一级分销价");
                $("#agent2_price").parent().find("label").html("二级分销价");
                $("#agent3_price").parent().find("label").html("三级分销价");

                $("#Sms").parent().show();
                $("#div_linepro_booktype").show();
                $("#div_pro_Remark").show();
                   //旅游大巴产品 上车地点下车地点
                $("#pickuppoint").parent().hide();
                $("#dropoffpoint").parent().hide();
                 //纸质票备注说明
                 $("#div_pro_note").hide();
                      //票务退票机制 
                 $("#div_QuitTicketMechanism").hide();

                  //是否参加抢购
                 $("#div_ispanicbuy").show();
                   //是否需要预约
                 $("#div_isneedbespeak").hide();
                   //是否限定使用日期
                 $("#div_isblackoutdate").hide();
                   //是否显示配送费用
                 $("#div_deliveryfee").hide();
                  $("#pro_weight").parent().hide(); //重量

                     //只有票务，实物,优惠劵 需要设置限购，其他类型产品 在团期中设置限购数量
                $("input:radio[name='ispanicbuy'][value='2']").parent().hide();
                 //多规格只有实物、票务、保险显示，其他隐藏
                $("input:radio[name='manyspeci'][value='1']").parent().hide();
            }
             else if (servertype == 10)//旅游大巴
            {
//                $("#div_ProValidateMethod").show();
//                $("#div_appointdata").show();
//                $("#div_iscanuseonsameday").show();

                $("#tbody_roomtype").hide();
                $("#tbody_product").show();

                $("#childreduce").parent().show();
                $("#travelproductid").parent().show();
                $("#travelstarting").parent().show();
                $("#travelending").parent().show();

                $("#pro_type").parent().show(); //验证类型
                $("#pro_Number").parent().show(); //限购数量
                //$("#tuipiao").parent().hide();

                $("#pro_Explain").parent().hide();
//                $("#pro_start").parent().show();

                $("#agent1_price").parent().show();
                $("#agent2_price").parent().show();
                $("#agent3_price").parent().show();


                $("#Sms").parent().show();
                $("#div_linepro_booktype").show();
                $("#div_pro_Remark").show();

                $("#travelproductid").parent().hide();
                   //旅游大巴产品 上车地点下车地点
                $("#pickuppoint").parent().show();
                $("#dropoffpoint").parent().show();
                 //纸质票备注说明
                 $("#div_pro_note").hide();
                      //票务退票机制 
                 $("#div_QuitTicketMechanism").hide();

                  //是否参加抢购
                 $("#div_ispanicbuy").show();
                   //是否需要预约
                 $("#div_isneedbespeak").hide();
                   //是否限定使用日期
                 $("#div_isblackoutdate").hide();
                   //是否显示配送费用
                 $("#div_deliveryfee").hide();
                  $("#pro_weight").parent().hide(); //重量
                     //只有票务，实物 需要设置限购，其他类型产品 在团期中设置限购数量
                $("input:radio[name='ispanicbuy'][value='2']").parent().hide();

                //显示 绑定保险产品
                $("#div_bindbx").show();

                //显示首发站发车时间
                $("#div_firststationtime").show();
                 //多规格只有实物、票务、保险显示，其他隐藏
                $("input:radio[name='manyspeci'][value='1']").parent().hide();

                $("#agent1_price").parent().find("label").html("一级分销价");
                $("#agent2_price").parent().find("label").html("二级分销价");
                $("#agent3_price").parent().find("label").html("三级分销价");

                //产品分组显示 
                $("#div_progroupid").show();
            }
            else if (servertype == 9)//房型信息
            {
                 $("#div_ProValidateMethod").hide();
                $("#div_appointdata").hide();
                $("#div_iscanuseonsameday").hide();

                $("#tbody_roomtype").show();
                //$("#tbody_product").hide();

                //订房样式

                $("#travelproductid").parent().hide();
                $("#travelstarting").parent().hide();
                $("#travelending").parent().hide();
                $("#pro_type").parent().hide();
                $("#pro_Explain").parent().hide();
                $("#pro_Number").parent().hide();
                $(".selectspeci").parent().hide();

                 $("#face_price").prev().text("默认显示门市价");
                 $("#advise_price").prev().text("默认显示销售价");
                 $("#agent1_price").prev().text("一级分销价返还(请注意！！！)");
                 $("#agent2_price").prev().text("二级分销价返还(请注意！！！)");
                 $("#agent3_price").prev().text("三级分销价返还(请注意！！！)");
                 //$("#agentsettle_price").parent().hide();
                 //$("#Integral_price").prev().text("返积分");

                $("#div_reservetype").show();
                $("#div_linepro_booktype").hide();

                $("#div_pro_Remark").hide();

                $("#childreduce").parent().hide();
                   //旅游大巴产品 上车地点下车地点
                $("#pickuppoint").parent().hide();
                $("#dropoffpoint").parent().hide();
                 //纸质票备注说明
                 $("#div_pro_note").hide();

                    //票务退票机制 
                 $("#div_QuitTicketMechanism").hide();

                  //是否参加抢购
                 $("#div_ispanicbuy").show();
                   //是否需要预约
                 $("#div_isneedbespeak").hide();
                   //是否限定使用日期
                 $("#div_isblackoutdate").hide();
                   //是否显示配送费用
                 $("#div_deliveryfee").hide();

                  $("#pro_weight").parent().hide(); //重量
                     //只有票务，实物 需要设置限购，其他类型产品 在团期中设置限购数量
                $("input:radio[name='ispanicbuy'][value='2']").parent().hide();
                 //多规格只有实物、票务、保险显示，其他隐藏
                $("input:radio[name='manyspeci'][value='1']").parent().hide();
            }
            else if (servertype == 11)//实物产品
            {
            
//                $("#div_ProValidateMethod").show();
//                $("#div_appointdata").show();
//                $("#div_iscanuseonsameday").show();
                selectspeci();
                $("#tbody_roomtype").hide();
                $("#tbody_product").show();

                $("#travelproductid").parent().hide();
                $("#travelstarting").parent().hide();
                $("#travelending").parent().hide();

                $("#pro_type").parent().show(); //验证类型
                $("#pro_Number").parent().show(); //限购数量
                //$("#tuipiao").parent().show();


                $("#pro_Explain").parent().show();
//                $("#pro_start").parent().show();

                $("#agent1_price").parent().show();
                $("#agent2_price").parent().show();
                $("#agent3_price").parent().show();

                $("#Sms").parent().show();
                $("#div_linepro_booktype").hide();
                $("#div_pro_Remark").hide();

                $("#childreduce").parent().hide();

                   //旅游大巴产品 上车地点下车地点
                $("#pickuppoint").parent().hide();
                $("#dropoffpoint").parent().hide();
                 //纸质票备注说明
                 $("#div_pro_note").hide();

                 //票务退票机制 
                 $("#div_QuitTicketMechanism").show();

                  //是否参加抢购
                 $("#div_ispanicbuy").show();
                   //是否需要预约
                 $("#div_isneedbespeak").hide();
                   //是否限定使用日期
                 $("#div_isblackoutdate").hide();
                   //是否显示配送费用
                 $("#div_deliveryfee").show();

                 //不包含服务 和 注意事项 隐藏
                 $("#service_NotContain").parent().hide();
                 $("#Precautions").parent().hide();

                 //验证类型隐藏
                 $("#pro_type").parent().hide();
                  $("#pro_weight").parent().show(); //重量
                     //只有票务，实物 需要设置限购，其他类型产品 在团期中设置限购数量
                $("input:radio[name='ispanicbuy'][value='2']").parent().show();

                $("#service_Contain").prev().text("实物描述");
                $("#Sms").prev().text("实物短信设置");
                //是否需要预约:不需要
                $("[name='isneedbespeak'][value='1']").parent().hide();
                 //是否限定使用日期:不限定
                $("[name='isblackoutdate'][value='1']").parent().hide();
                //产品验证有效期：不需要设置
                 $("[name='ProValidateMethod'][value='1']").parents(".mi-form-item").hide();
                 //产品使用限制：不需要设置
                 $("#sel_iscanuseonsameday").parents("#div_iscanuseonsameday").hide();
                 //产品退票机制:不需要设置
                 $("#sel_QuitTicketMechanism").parents("#div_QuitTicketMechanism").hide();
                  //多规格只有实物、票务、保险显示，其他隐藏
                $("input:radio[name='manyspeci'][value='1']").parent().show();

                //产品分组显示 
                $("#div_progroupid").show();
            }
            else if(servertype == 3)//优惠劵
            {

//                $("#div_ProValidateMethod").show();
//                $("#div_appointdata").show();
//                $("#div_iscanuseonsameday").show();
                selectspeci();
                $("#tbody_roomtype").hide();
                $("#tbody_product").show();

                $("#travelproductid").parent().hide();
                $("#travelstarting").parent().hide();
                $("#travelending").parent().hide();

                $("#pro_type").parent().show(); //验证类型
                $("#pro_Number").parent().show(); //限购数量
                //$("#tuipiao").parent().show();


                $("#pro_Explain").parent().show();
//                $("#pro_start").parent().show();

                $("#agent1_price").parent().hide();
                $("#agent2_price").parent().hide();
                $("#agent3_price").parent().hide();

                $("#Sms").parent().show();
                $("#div_linepro_booktype").hide();
                $("#div_pro_Remark").hide();

                $("#childreduce").parent().hide();

                   //旅游大巴产品 上车地点下车地点
                $("#pickuppoint").parent().hide();
                $("#dropoffpoint").parent().hide();
                 //纸质票备注说明
                 $("#div_pro_note").show();

                 //票务退票机制 
                 $("#div_QuitTicketMechanism").show();

                  //是否参加抢购
                 $("#div_ispanicbuy").show();
                   //是否需要预约
                 $("#div_isneedbespeak").show();
                   //是否限定使用日期
                 $("#div_isblackoutdate").show();
                   //是否显示配送费用
                 $("#div_deliveryfee").show();
                   $("#pro_weight").parent().hide(); //重量
                   //只有票务，实物，优惠劵 需要设置限购，其他类型产品 在团期中设置限购数量
                $("input:radio[name='ispanicbuy'][value='2']").parent().show();

                $("#service_NotContain").parent().hide();
                $("#Precautions").parent().hide(); 
                $("#div_deliveryfee").hide();
                 //多规格只有实物、票务、保险显示，其他隐藏
                $("input:radio[name='manyspeci'][value='1']").parent().hide();
            }
            else if(servertype==12)//预约产品
            {
                selectspeci();
                   //产品验证有效期
                $("#div_ProValidateMethod").hide();
                //指定有效期
                $("#div_appointdata").hide();
                //使用限制  
                $("#div_iscanuseonsameday").hide();
                //产品显示设置
                $("#sel_viewmethod").parent().hide();

                $("#tbody_roomtype").hide();
                $("#tbody_product").show();

                $("#travelproductid").parent().hide();
                $("#travelstarting").parent().hide();
                $("#travelending").parent().hide();

                $("#pro_type").parent().hide(); //验证类型
                $("#pro_Number").parent().hide(); //限购数量
                //$("#tuipiao").parent().show();


                $("#pro_Explain").parent().show();
//                $("#pro_start").parent().show();

                $("#advise_price").prev().text("预约价");
                $("#agent1_price").parent().hide();
                $("#agent2_price").parent().hide();
                $("#agent3_price").parent().hide();

                $("#Sms").prev().text("单独设置短信内容 ").parent().show();
                $("#div_linepro_booktype").hide();
                $("#div_pro_Remark").hide();

                $("#childreduce").parent().hide();

                   //旅游大巴产品 上车地点下车地点
                $("#pickuppoint").parent().hide();
                $("#dropoffpoint").parent().hide();
                 //纸质票备注说明
                 $("#div_pro_note").hide();

                 //票务退票机制 
                 $("#div_QuitTicketMechanism").hide();

                  //是否参加抢购
                 $("#div_ispanicbuy").hide();
                   //是否需要预约
                 $("#div_isneedbespeak").hide();
                   //是否限定使用日期
                 $("#div_isblackoutdate").hide();
                   //是否显示配送费用
                 $("#div_deliveryfee").hide();
                 $("#pro_weight").parent().hide(); //重量
                   //只有票务，实物 需要设置限购，其他类型产品 在团期中设置限购数量
                  $("input:radio[name='ispanicbuy'][value='2']").parent().hide();

                  $("#service_Contain").prev().text("房屋参数");
                   $("#service_NotContain").prev().text("房屋介绍");
                   $("#Precautions").parent().hide();

                 

                  //预订产品 附加信息 显示
                  $("#div_bookpro").show();
                   //多规格只有实物、票务、保险显示，其他隐藏
                $("input:radio[name='manyspeci'][value='1']").parent().hide();
            }
            else if(servertype==13)//教练预约
            {
                selectspeci();
                   //产品验证有效期
                $("#div_ProValidateMethod").hide();
                //指定有效期
                $("#div_appointdata").hide();
                //使用限制  
                $("#div_iscanuseonsameday").hide();
                //产品显示设置
                $("#sel_viewmethod").parent().hide();

                $("#tbody_roomtype").hide();
                $("#tbody_product").show();

                $("#travelproductid").parent().hide();
                $("#travelstarting").parent().hide();
                $("#travelending").parent().hide();

                $("#pro_type").parent().hide(); //验证类型
                $("#pro_Number").parent().hide(); //限购数量
                //$("#tuipiao").parent().show();


                $("#pro_Explain").parent().show();
//                $("#pro_start").parent().show();

                $("#advise_price").prev().text("预约价");
                $("#agent1_price").parent().hide();
                $("#agent2_price").parent().hide();
                $("#agent3_price").parent().hide();

                $("#Sms").prev().text("单独设置短信内容 ").parent().show();
                $("#div_linepro_booktype").hide();
                $("#div_pro_Remark").hide();

                $("#childreduce").parent().hide();

                   //旅游大巴产品 上车地点下车地点
                $("#pickuppoint").parent().hide();
                $("#dropoffpoint").parent().hide();
                 //纸质票备注说明
                 $("#div_pro_note").hide();

                 //票务退票机制 
                 $("#div_QuitTicketMechanism").hide();

                  //是否参加抢购
                 $("#div_ispanicbuy").hide();
                   //是否需要预约
                 $("#div_isneedbespeak").hide();
                   //是否限定使用日期
                 $("#div_isblackoutdate").hide();
                   //是否显示配送费用
                 $("#div_deliveryfee").hide();
                 $("#pro_weight").parent().hide(); //重量
                   //只有票务，实物 需要设置限购，其他类型产品 在团期中设置限购数量
                  $("input:radio[name='ispanicbuy'][value='2']").parent().hide();

                  $("#service_Contain").prev().text("预约教练说明");
                   $("#service_NotContain").prev().text("教练介绍");
                   $("#Precautions").parent().hide();

                   $("#sel_bookpro_ispay").val("1");
                  $("#sel_bookpro_ispay option[value='0']").remove()

                  //预订产品 附加信息 显示
                  $("#div_bookpro").show();

                  //产品关联人姓名、产品关联人手机隐藏
                  //$("#txt_bookpro_bindname").parent().hide();
                  //$("#txt_bookpro_bindphone").parent().hide();
                   //多规格只有实物、票务、保险显示，其他隐藏
                $("input:radio[name='manyspeci'][value='1']").parent().hide();
            }
            else if(servertype==14)//保险产品
            { 
               //服务商默认是慧择网
               $("#selservice").val(2);

                $("#tbody_roomtype").hide();
                $("#tbody_product").show();

                $("#travelproductid").parent().hide();
                $("#travelstarting").parent().hide();
                $("#travelending").parent().hide();

                $("#pro_type").parent().show(); //验证类型
                $("#pro_Number").parent().hide(); //限购数量
                
                 
                $("#pro_Explain").parent().hide(); 

                $("#agent1_price").parent().hide();
                $("#agent2_price").parent().hide();
                $("#agent3_price").parent().hide();

                $("#face_price").parent().hide();
                $("#advise_price").parent().hide();
                $("#Integral_price").parent().hide(); 
               $("#pro_note").parent().hide();
               
               $("#service_Contain").prev().text("产品详情");
               $("#service_NotContain").parent().hide();
               $("#Precautions").parent().hide();
               
                $("#Sms").parent().hide();
                $("#div_linepro_booktype").hide();
                $("#div_pro_Remark").hide();

                $("#childreduce").parent().hide();

                //旅游大巴产品 上车地点下车地点
                $("#pickuppoint").parent().hide();
                $("#dropoffpoint").parent().hide();
                 //纸质票备注说明
                 $("#div_pro_note").hide();

                 //票务退票机制 
                 $("#div_QuitTicketMechanism").show();

                  //是否参加抢购
                 $("#div_ispanicbuy").hide();
                   //是否需要预约
                 $("#div_isneedbespeak").hide();
                   //是否限定使用日期
                 $("#div_isblackoutdate").hide();
                   //是否显示配送费用
                 $("#div_deliveryfee").hide();
                   $("#pro_weight").parent().hide(); //重量
               
                   $("input:[name='source_type']").last().attr("checked","checked");
                   $("input:[name='source_type']").each(function(){
                              //写入代码
                              $(this).attr("disabled","disabled").css("background-color", "#cccccc");
                   }); 
                   $("#trservice").show();
                   $("#trservice_proid").show();
                    
                   $("input:[name='manyspeci']").last().attr("checked","checked");
                   $("input:[name='manyspeci']").each(function(){
                              //写入代码
                              $(this).attr("disabled","disabled").css("background-color", "#cccccc");
                   }); 
                    selectspeci();

                   $("#div_ProValidateMethod").hide();
                   $("#sel_iscanuseonsameday").parent().hide();
                   $("#div_QuitTicketMechanism").hide();
                    //多规格只有实物、票务、保险显示，其他隐藏
                $("input:radio[name='manyspeci'][value='1']").parent().show();
            }
            else//门票
            {

//                $("#div_ProValidateMethod").show();
//                $("#div_appointdata").show();
//                $("#div_iscanuseonsameday").show();
                selectspeci();
                $("#tbody_roomtype").hide();
                $("#tbody_product").show();

                $("#div_bindserver").show();
                $("#div_bindservercode").show();
                $("#travelproductid").parent().hide();
                $("#travelstarting").parent().hide();
                $("#travelending").parent().hide();

                $("#pro_type").parent().show(); //验证类型
                $("#pro_Number").parent().show(); //限购数量
                //$("#tuipiao").parent().show();


                $("#pro_Explain").parent().show();
//                $("#pro_start").parent().show();

                $("#agent1_price").parent().show();
                $("#agent2_price").parent().show();
                $("#agent3_price").parent().show();

                $("#Sms").parent().show();
                $("#div_linepro_booktype").hide();
                $("#div_pro_Remark").hide();

                $("#childreduce").parent().hide();

                   //旅游大巴产品 上车地点下车地点
                $("#pickuppoint").parent().hide();
                $("#dropoffpoint").parent().hide();
                 //纸质票备注说明
                 $("#div_pro_note").show();

                 //票务退票机制 
                 $("#div_QuitTicketMechanism").show();

                  //是否参加抢购
                 $("#div_ispanicbuy").show();
                   //是否需要预约
                 $("#div_isneedbespeak").show();
                   //是否限定使用日期
                 $("#div_isblackoutdate").show();
                   //是否显示配送费用
                 $("#div_deliveryfee").show();
                   $("#pro_weight").parent().hide(); //重量
                   //只有票务，实物 需要设置限购，其他类型产品 在团期中设置限购数量
                $("input:radio[name='ispanicbuy'][value='2']").parent().show();

                //电子票核销类型 显示
                $("#div_pro_yanzheng_method").show();
                //产品预订一张产生电子码个数
                $("#div_pnonumperticket").show();
                  //产品指定验证POSID
                $("#div_SpecifyPosid").show();
                //是否需要提交游玩日期
                $("#div_isSetVisitDate").show();
                 //多规格只有实物、票务、保险显示，其他隐藏
                $("input:radio[name='manyspeci'][value='1']").parent().show();

                //产品分组显示 
                $("#div_progroupid").show();

                //是否需要提交身份证号
                $("#div_issetidcard").show();
            }


        }

         //对多规格的判断
         function selectspeci() {
            var manyspeci=$('input:radio[name="manyspeci"]:checked').trimVal();
            if(manyspeci=="1"){//多规格
                $(".speci").show();

                $("#agent1_price").parent().hide();
                $("#agent2_price").parent().hide();
                $("#agent3_price").parent().hide();
                $("#face_price").parent().hide();
                $("#advise_price").parent().hide();
                $("#agentsettle_price").parent().hide();
                $("#Integral_price").parent().hide();
                
            }else{//单规格
                $("#agent1_price").parent().show();
                $("#agent2_price").parent().show();
                $("#agent3_price").parent().show();
                $("#face_price").parent().show();
                $("#advise_price").parent().show();
                $("#agentsettle_price").parent().show();
                $("#Integral_price").parent().show();

                $(".speci").hide();
            }
         
         }

         

        function bindViewImg() {
            var defaultPath = "";
            var imgSrc = '<%=headPortraitImgSrc %>';
            if (imgSrc == "") {
                //                $(".headPortraitImg").attr("src", defaultPath);
            } else {
                var filePath = '<%=headPortrait.fileUrl %>';
                var headlogoImgSrc = filePath + imgSrc;
                $("#headPortraitImg").attr("src", headlogoImgSrc);
            }
        }
        function LoadProjectList(comid, seled) {
//            //获取项目表
            $.post("/JsonFactory/ProductHandler.ashx?oper=projectlist", { comid: comid ,prosort:1,projectstate:1}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                }
                if (data.type == 100) {
                    $("#selproject").empty();
                    if (data.totalCount > 0) {
                        for (var i = 0; i < data.totalCount; i++) {
                            if (data.msg[i].Id == seled) {
                                $("#selproject").append('<option value="' + data.msg[i].Id + '" selected="selected">' + data.msg[i].Projectname + '</option>');
                            } else {
                                $("#selproject").append('<option value="' + data.msg[i].Id + '"  >' + data.msg[i].Projectname + '</option>');
                            }
                        }
                    }
                }
            })
        }

        function LoadProclass(comid, proclass) {
            //加载类目
            $.ajax({
                type: "post",
                url: "/JsonFactory/ProductHandler.ashx?oper=proclasslist",
                data: { pageindex: 1, pagesize: 200, industryid: $("#hid_industryid").trimVal() },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    $("#proclass").empty();
                    $("#proclass").append("<option value='0' >请选择所属类目</option>");
                    if (data.type == 1) {
                        return;
                    }
                    if (data.type == 100) {
                        for (i = 0; i < data.totalCount; i++) {
                            if (data.msg[i].Id == proclass) {
                                $("#proclass").append("<option value='" + data.msg[i].Id + "' selected='selected'>"+ data.msg[i].Classname + "</option>");
                            } else {
                                $("#proclass").append("<option value='" + data.msg[i].Id + "'>" +  data.msg[i].Classname + "</option>");
                            }
                        }
                    }
                }
            })

        }


        function LoadServertype(servertype)
        {
          $("#server_type").empty();
          if(servertype==1)//电子凭证
          {
          $("#server_type").append('<option value="1">电子凭证</option>');
          }
          if(servertype==15)//电子凭证套票
          {
          $("#server_type").append('<option value="1">电子凭证套票</option>');
          }
          if(servertype==2)//跟团游
          {
            $("#server_type").append('<option value="2">跟团游</option>');
          }
          if(servertype==8)//当地游
          {
           $("#server_type").append('<option value="8">当地游</option>');
          }
          if(servertype==9)//酒店客房
          {
           $("#server_type").append('<option value="9">酒店客房</option>');
          }
           if(servertype==10)//旅游大巴
          {
           $("#server_type").append('<option value="10">旅游大巴</option>');
          }
          if(servertype==3)//赠送产品(不需要支付)
          {
           $("#server_type").append('<option value="3">赠送产品(不需要支付)</option>');
          }
          if(servertype==5)//特价机票
          {
           $("#server_type").append('<option value="5">特价机票</option>');
          }
            if(servertype==6)//度假套餐
          {
           $("#server_type").append('<option value="6">度假套餐</option>');
          }
            if(servertype==7)//其他产品
          {
           $("#server_type").append('<option value="7">其他产品</option>');
          }
            if(servertype==11)//实物产品
          {
           $("#server_type").append('<option value="11">实物产品</option>');
          }
            if(servertype==12)//预约产品
          {
           $("#server_type").append('<option value="12">预约产品</option>');
          }
          if(servertype==13)//教练预约产品
          {
           $("#server_type").append('<option value="13">教练预约</option>');
          }
          if(servertype==14)//保险产品
          {
           $("#server_type").append('<option value="14">保险产品</option>');
          }

          ViewByServerType(servertype);
        }
        //得到公司绑定pos
        function selectCompanyPos(bindposid){
           $.post("/JsonFactory/ProductHandler.ashx?oper=CompanyPoslist",{comid:$("#hid_comid").trimVal()},function(data){
               data=eval("("+data+")");
               if(data.type==1){
                 var vhtml='<option value="">不指定</option>';
                 $("#SpecifyPosid").html(vhtml);
               }
               if(data.type==100){
                   var vhtml='<option value="">不指定</option>';
                   var inputstr = "";//闸机使用
                   for(var i=0;i<data.msg.length;i++){ 

                     inputstr += '<label><input name="bandingzhajiid" value="' + data.msg[i].Posid + '" type="checkbox"> ' + data.msg[i].Posid+" ("+data.msg[i].Remark+") </label><br>";
 
                     if(data.msg[i].Posid==bindposid){
                       vhtml+='<option value="'+data.msg[i].Posid+'" selected="selected">'+data.msg[i].Posid+'</option>';
                     }
                     else{
                       vhtml+='<option value="'+data.msg[i].Posid+'">'+data.msg[i].Posid+'</option>';
                     }
                   }
                    $("#SpecifyPosid").html(vhtml);
                    $("#bandingzhaji").html(inputstr);

                    selectCompanybandignzhaji($("#hid_proid").trimVal());//
               }
           })
        }


         //获取已绑定闸机
        function selectCompanybandignzhaji(proid){
           $.post("/JsonFactory/ProductHandler.ashx?oper=CompanyProbandignzhaji",{comid:$("#hid_comid").trimVal(),proid:proid},function(data){
               data=eval("("+data+")");
               if(data.type==100){
                   for(var i=0;i<data.msg.length;i++){ ;
                          $(":checkbox[value='"+data.msg[i].pos_id+"']").prop("checked",true);
                   } 
               }
           })
        }

        //商户产品组合管理
        function getprogrouplist(seled,comid){
          $.post("/JsonFactory/ProductHandler.ashx?oper=GetProgrouplistByComid",{comid:comid,runstate:1},function(data){
            data=eval("("+data+")");
            
            var groupstr='<option value="0">无</option>';
            if(data.type==1){
             $("#sel_progroupid").html(groupstr);
            }
            if(data.type==100){ 
                 for(var i=0;i<data.msg.length;i++){
                   if(data.msg[i].id==seled){
                      groupstr+='<option value="'+data.msg[i].id+'"  selected="selected">'+data.msg[i].groupname+'</option>';
                   }
                   else{
                     groupstr+='<option value="'+data.msg[i].id+'">'+data.msg[i].groupname+'</option>';
                   } 
               
                 $("#sel_progroupid").html(groupstr);
               }
            }
          })
        }
        function groupmanage(){ 
           $("#div_articaltype").show();
           SearchProGrouplist(1,10); 
        }
           //分页
            function progroupsetpage(count, pagesize, curpage) {
                $("#divgroupPage").paginate({
                    count: Math.ceil(count / pagesize),
                    start: curpage,
                    display: 5,
                    border: false,
                    text_color: '#888',
                    background_color: '#EEE',
                    text_hover_color: 'black',
                    images: false,
                    rotate: false,
                    mouse: 'press',
                    onChange: function (page) {

                        SearchProGrouplist(page,pagesize);

                        return false;
                    }
                });
            }
            function SearchProGrouplist(pageindex,pagesize){
                 $.post("/JsonFactory/ProductHandler.ashx?oper=GetProgroupPagelistByComid",{comid:$("#hid_initprocomid").trimVal(),pageindex:pageindex,pagesize:pagesize},function(data){
                     data=eval("("+data+")");
                     if(data.type==1){}
                     if(data.type==100){
                        $("#tbody_progrouplist").empty();
                        $("#divgroupPage").empty(); 
                        if (data.totalCount == 0) {
                        } else { 
                            $("#Progrouptmpl").tmpl(data.msg).appendTo("#tbody_progrouplist");
                            progroupsetpage(data.totalCount, pagesize, pageindex);
                        }
                     }
                   })
            }
            function addprogroup()
            {  
               var progroup=$("#text_progroup").trimVal();
               if(progroup=="")
               {
                alert("产品组合不可为空");
                return;
               }
               $.post("/JsonFactory/ProductHandler.ashx?oper=editprogroup",{progroupid:0,comid:$("#hid_initprocomid").trimVal(),groupname:progroup,runstate:1},function(data){
                 data=eval("("+data+")");
                 if(data.type==1){
                   alert(data.msg);
                 }
                 if(data.type==100){
                   alert("成功");
                   SearchProGrouplist(1,10);
                 }
               })
            }
            function editprogroup(progroupid,obj){ 
               if($(obj).val()=="编辑"){
                 $(obj).val("确定");
                 $("#span1_"+progroupid).hide();
                 $("#tgroup_"+progroupid).show();
                  $("#span2_"+progroupid).hide();
                 $("#sgroup_"+progroupid).show();
               }
               else{
                $(obj).val("编辑");
                var ggroupname= $("#tgroup_"+progroupid).val();
                var grunstate=$("#sgroup_"+progroupid).val();
                if(grunstate==0){
                 if(confirm("下线组合会导致组合下的产品的组合都将归为 无 ,确认下线吗？")){
                      execeditprogroup(progroupid,$("#hid_initprocomid").trimVal(),ggroupname,grunstate);
                 }
                }
                else{
                      execeditprogroup(progroupid,$("#hid_initprocomid").trimVal(),ggroupname,grunstate);
                }
                
               }
            }
            function execeditprogroup(progroupid,comid,groupname,runstate){
               $.post("/JsonFactory/ProductHandler.ashx?oper=editprogroup",{progroupid:progroupid,comid:$("#hid_initprocomid").trimVal(),groupname:groupname,runstate:runstate},function(data){
                             data=eval("("+data+")");
                             if(data.type==1){
                               alert(data.msg);
                             }
                             if(data.type==100){
                                 alert("成功");
            //                   SearchProGrouplist(1,10);

                                 $("#span1_"+progroupid).text(groupname).show();
                                 $("#tgroup_"+progroupid).val(groupname).hide();
                                 if(runstate==0){
                                   $("#span2_"+progroupid).text("下线").show();
                                 }
                                 else{
                                   $("#span2_"+progroupid).text("在线").show();
                                 } 
                                 $("#sgroup_"+progroupid).val(runstate).hide();
                             }
                           }) 
            }
            function progroupdivcancel(){
              $("#div_articaltype").hide();
              getprogrouplist(0,$("#hid_initprocomid").trimVal());
            }
    </script>
    <link type="text/css" href="/Scripts/timepicker/css/jquery-ui-1.8.17.custom.css"
        rel="stylesheet" />
    <link type="text/css" href="/Scripts/timepicker/css/jquery-ui-timepicker-addon.css"
        rel="stylesheet" />
    <script type="text/javascript" src="/Scripts/timepicker/js/jquery-ui-1.8.17.custom.min.js"></script>
    <script type="text/javascript" src="/Scripts/timepicker/js/jquery-ui-timepicker-addon.js"></script>
    <script type="text/javascript" src="/Scripts/timepicker/js/jquery-ui-timepicker-zh-CN.js"></script>
    <script type="text/javascript">
        $(function () {
            $(".ui_timepicker").datetimepicker({
                //showOn: "button",
                //buttonImage: "./css/images/i con_calendar.gif",
                //buttonImageOnly: true,
                showSecond: true,
                timeFormat: 'hh:mm:ss',
                stepHour: 1,
                stepMinute: 1,
                stepSecond: 1
            })
        })

        //剩余可销售数量调整
        function referrer_ch(proid, pixsize) {
            $("#rhshow").show();
        };
        function closerh() {
            $("#rhshow").hide();
        }
        function addrh() {
            if (isNaN($("#txtupnum").trimVal())) {
                alert("调整数量必须为整数");
                return;
            }
            $.post("/JsonFactory/ProductHandler.ashx?oper=uplimitbuytotalnum", { proid: $("#hid_proid").trimVal(), upnum: $("#txtupnum").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("调整失败");
                }
                if (data.type == 100) {
                    $("#rhshow").hide();

                    var initnum = $("#panicbuy_limitbuytotalnum").trimVal();
                    var upnum = $("#txtupnum").trimVal();

                    var resultnum = parseInt(initnum) + parseInt(upnum);
                    $("#panicbuy_limitbuytotalnum").val(resultnum);

                    var buynum = $("#buynum").trimVal();
                    var totalnum = parseInt(resultnum) + parseInt(buynum);
                    $("#saletotalnum").text("销售总量:" + totalnum);
                }
            })
        }
        function reducerh() {
            if (isNaN($("#txtupnum").trimVal())) {
                alert("调整数量必须为整数");
                return;
            }
            $.post("/JsonFactory/ProductHandler.ashx?oper=uplimitbuytotalnum", { proid: $("#hid_proid").trimVal(), upnum: "-" + $("#txtupnum").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("调整失败");
                }
                if (data.type == 100) {
                    $("#rhshow").hide();

                    var initnum = $("#panicbuy_limitbuytotalnum").trimVal();
                    var upnum = $("#txtupnum").trimVal();

                    var resultnum = parseInt(initnum) - parseInt(upnum);
                    $("#panicbuy_limitbuytotalnum").val(resultnum);

                    var buynum = $("#buynum").trimVal();
                    var totalnum = parseInt(resultnum) + parseInt(buynum);
                    $("#saletotalnum").text("销售总量:" + totalnum);
                }
            })
        }
        function ViewIspanicbuy(isseled) {
            if (isseled == 0) {
                $("#ddiv_ispanicbuy").hide();
            }
            if (isseled == 1) {
                if ($("#server_type").trimVal() == 1 || $("#server_type").trimVal() == 11 || $("#server_type").trimVal() == 3) {//票务,实物产品，优惠劵，抢购需要设置抢购数量
                    $("#ddiv_ispanicbuy").show();

                    $("#ddiv_begintime").show();
                    $("#ddiv_endtime").show();


                    $("#panicbuy_limitbuytotalnum").parent().show();
                    $("#buynum").parent().show();
                    $("#saletotalnum").parent().show();
                }
                else {//跟团游，当地游，旅游大巴，酒店 ，抢购不需要设置抢购数量(在团期中设置)
                    $("#ddiv_ispanicbuy").show();

                    $("#ddiv_begintime").show();
                    $("#ddiv_endtime").show();

                    $("#panicbuy_limitbuytotalnum").parent().hide();
                    $("#buynum").parent().hide();
                    $("#saletotalnum").parent().hide();
                }
            }
            if (isseled == 2) {
                //限购只是针对票务，实物，优惠劵，其他类型产品不可选，在团期中可以设置
                if ($("#server_type").trimVal() == 1 || $("#server_type").trimVal() == 11 || $("#server_type").trimVal() == 3) {
                    $("#ddiv_ispanicbuy").show();

                    $("#ddiv_begintime").hide();
                    $("#ddiv_endtime").hide();

                    $("#panicbuy_limitbuytotalnum").parent().show();
                    $("#buynum").parent().show();
                    $("#saletotalnum").parent().show();
                }
            }
        }

        //读取服务
        function readRentserver() {
           
            //$("#serverDepositprice").hide();
            $("#ishasyajin").hide();
            var Wrentserver = $("input:radio[name='Wrentserver']:checked").val();
            if (Wrentserver == 1) {
                $(".payyajin").show(); 
            } else {
                $(".payyajin").hide(); 
            }



            $.post("/JsonFactory/ProductHandler.ashx?oper=Rentserverpagelist", { proid: $("#hid_proid").trimVal(), comid: $("#hid_comid").trimVal(),pagesize:50 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("读取服务失败");
                }
                if (data.type == 100) {
                   

                    if (Wrentserver == 1) {
                        //$(".rentserver").show();
                        var inputstr = "";
                        for (var i = 0; i < data.msg.length; i++) {
                            var selectchecked = "checked";
                            if (data.msg[i].bindingstate == 0) {
                                selectchecked = "";
                            }
                            inputstr += '<input name="Rentserverid" value="' + data.msg[i].id + '" type="checkbox" ' + selectchecked + '>' + data.msg[i].servername;
                        }

                        $("#serverlist").html(inputstr);
                        $("#pro_yanzheng_method").val("1");

                        $("input:radio[name='WDeposit'][value='1']").attr("checked", "checked");
                        $("#Depositprice").val(0);

                    } else {
                        $("#pro_yanzheng_method").val("0");
                        $(".rentserver").hide();
                    }


                }
            })
        }

        function readWDeposit() {
            var WDeposit = $("input:radio[name='WDeposit']:checked").val();
            if (WDeposit == 0) {
                $("#serverDepositprice").hide();
            } else {
                $("#serverDepositprice").show();

            }
        }

    </script>
    <style type="text/css">
        .none
        {
            display: none;
        }
        #loading
        {
            position: absolute;
            left: 50%;
            top: 60px;
            z-index: 99;
        }
        #loading, #loading .lbk, #loading .lcont
        {
            width: 146px;
            height: 146px;
        }
        #loading .lbk, #loading .lcont
        {
            position: relative;
        }
        #loading .lbk
        {
            background-color: #000;
            opacity: .5;
            border-radius: 10px;
            margin: -73px 0 0 -73px;
            z-index: 0;
        }
        #loading .lcont
        {
            margin: -146px 0 0 -73px;
            text-align: center;
            color: #f5f5f5;
            font-size: 14px;
            line-height: 35px;
            z-index: 1;
        }
        #loading img
        {
            width: 35px;
            height: 35px;
            margin: 30px auto;
            display: block;
        }
    </style>
    <!--多规格css-->
    <link type="text/css" href="/styles/guige.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/pmui/projectlist.aspx" onfocus="this.blur()" target=""><span>项目管理</span></a></li>
                <li><a href="/ui/pmui/Projectedit.aspx" onfocus="this.blur()" target=""><span>添加项目</span></a></li>
                <li><a href="/ui/pmui/ProductList.aspx?projectid=<%=projectid %>" onfocus="this.blur()"
                    target=""><span>产品列表</span></a></li>
                <li class="on"><a href="/ui/pmui/ProductServerTypeList.aspx" onfocus="this.blur()"
                    target=""><span>添加产品</span></a></li>
                <li><a href="/ui/pmui/order/Salecount.aspx" onfocus="this.blur()" target="">产品统计</a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <!--产品基本信息-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        产品基本信息</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            名称</label>
                        <input name="pro_name" type="text" id="pro_name" size="25" class="mi-input" style="width: 400px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            产品主图片(单张，尺寸：宽大于640像素,比例 宽3:高2)</label>
                        <input type="hidden" id="Hidden1" value="" />
                        <img alt="" class="headPortraitImgSrc" id="headPortraitImg" src="/images/defaultThumb.png" />
                        <uc1:uploadFile ID="headPortrait" runat="server" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            产品子图片(不限张数)</label>
                        <uc1:multiUploadFile ID="childImg" runat="server" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            所属项目</label>
                        <select id="selproject" class="mi-input" style="margin-left: 0px;">
                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            产品分类</label>
                        <select name="com_type" class="mi-input" id="com_type" size="8" style="margin-left: 0px;">
                            <option value="" selected>请选择所属分类</option>
                        </select>
                        <select name="proclass" class="mi-input" id="proclass" size="8" style="margin-left: 0px;">
                            <option value="" selected>请选择所属类目</option>
                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            产品类型</label>
                        <select name="server_type" class="mi-input" id="server_type" style="margin-left: 0px;">
                            <%--  <option value="1">电子凭证</option>
                                <option value="2">跟团游</option>
                                <option value="8">当地游</option>
                                <option value="9">酒店客房</option>--%>
                            <%--  <option value="3">旅游优惠券(不需要支付)</option>
                                <option value="5">特价机票</option>
                                <option value="6">度假套餐</option>
                                <option value="7">其他产品</option>--%>
                        </select>
                    </div>
                    <div class="mi-form-item" id="div_reservetype" style="display: none;">
                        <label class="mi-label">
                            预订类型</label>
                        <select name="ReserveType" id="ReserveType" class="mi-input">
                            <option value="2" selected>网上预付</option>
                            <%--    <option value="3">电子房券(需验证)</option>
                                    <option value="1">前台现付</option>
                            --%>
                        </select>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <!--房型信息-->
                <div id="tbody_roomtype" class="edit-box J-commonSettingsModule" style="opacity: 1;
                    margin-top: 0px; position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        房型信息</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            床型</label>
                        <input name="bedtype" type="text" id="bedtype" size="25" class="mi-input" style="width: 200px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            床宽</label>
                        <input name="bedwidth" type="text" id="bedwidth" value="" size="25" class="mi-input"
                            style="width: 200px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            加床</label>
                        <label>
                            <input name="whetherextrabed" type="radio" value="true" checked>
                            可以</label>
                        <label>
                            <input name="whetherextrabed" type="radio" value="false">
                            不可以</label>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            加床费用</label>
                        <input name="extrabedprice" type="text" id="extrabedprice" size="25" class="mi-input"
                            style="width: 100px;" />(单位：“/床/间夜”)
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            Wifi</label>
                        <input name="wifi" type="text" id="wifi" size="25" class="mi-input" style="width: 100px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            早餐情况</label>
                        <select name="Breakfast" class="mi-input" id="Breakfast">
                            <option value="1">无早</option>
                            <option value="2">单早</option>
                            <option value="3">双早</option>
                            <option value="4">多早</option>
                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            建筑面积(选填)</label>
                        <input name="Builtuparea" type="text" id="Builtuparea" size="25" class="mi-input"
                            style="width: 100px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            楼层(选填)</label>
                        <input name="floor" type="text" id="floor" size="25" class="mi-input" style="width: 100px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            最多入住人数(数字格式,必填)</label>
                        <input name="largestguestnum" type="text" id="largestguestnum" size="25" class="mi-input"
                            style="width: 100px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            可否安排无烟楼层</label>
                        <label>
                            <input name="whethernon-smoking" type="radio" value="true">
                            可以</label>
                        <label>
                            <input name="whethernon-smoking" type="radio" value="false" checked>
                            不可以</label>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            便利设施(选填)</label>
                        <textarea name="amenities" cols="50" rows="2" class="mi-input" id="amenities"></textarea>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            媒体/科技(选填)</label>
                        <textarea name="Mediatechnology" cols="50" rows="2" class="mi-input" id="Mediatechnology"></textarea>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            食品和饮品(选填)</label>
                        <textarea name="Foodanddrink" cols="50" rows="2" class="mi-input" id="Foodanddrink"></textarea>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            浴室(选填)</label>
                        <textarea name="ShowerRoom" cols="50" rows="2" class="mi-input" id="ShowerRoom"></textarea>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            接收预定人公司</label>
                        <input name="RecerceSMSCompany" type="text" id="RecerceSMSCompany" size="25" class="mi-input"
                            style="width: 100px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            接收预定短信人员姓名</label>
                        <input name="RecerceSMSName" type="text" id="RecerceSMSName" size="25" class="mi-input"
                            style="width: 100px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            接收预定短信人员电话</label>
                        <input name="RecerceSMSPhone" type="text" id="RecerceSMSPhone" size="25" class="mi-input"
                            style="width: 100px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            房型备注(选填)</label>
                        <textarea name="roomtyperemark" cols="50" rows="5" class="mi-input" id="roomtyperemark"></textarea>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <!--产品基本信息-->
                <div id="tbody_product" class="edit-box J-commonSettingsModule" style="opacity: 1;
                    margin-top: 0px; position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        产品信息</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            产品编号</label>
                        <input name="travelproductid" type="text" id="travelproductid" size="25" class="mi-input"
                            style="width: 200px;" />
                    </div>
                    <div class="mi-form-item" id="div_linepro_booktype" style="display: none;">
                        <label class="mi-label">
                            线路预订类型</label>
                        <select id="linepro_booktype" class="mi-input">
                            <option value="1">无需预付</option>
                            <option value="2" selected>需要预付</option>
                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            出发地</label>
                        <input name="travelstarting" type="text" id="travelstarting" size="25" class="mi-input"
                            style="width: 200px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            目的地(如果多个目的地，请用中文逗号隔开)</label>
                        <input name="travelending" type="text" id="travelending" size="25" class="mi-input"
                            style="width: 300px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            上车地点(如果多个上车地点，请用中文逗号隔开，必填)</label>
                        <input type="text" id="pickuppoint" class="mi-input" style="width: 400px;" placeholder="例如:8点张家口火车站，9点崇礼县城" /><br />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            下车地点(如果多个下车地点，请用中文逗号隔开,选填)</label>
                        <input type="text" id="dropoffpoint" class="mi-input" style="width: 400px;" placeholder="例如:万龙雪场，云顶雪场" /><br />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            验证类型</label>
                        <select name="pro_type" id="pro_type" style="margin-left: 0px;">
                            <option value="1">电子凭证</option>
                        </select>
                        <label>
                            <input name="source_type" type="radio" id="source_type1" value="1" checked="checked" />
                            系统自动生成
                        </label>
                        <label>
                            <input name="source_type" type="radio" id="source_type2" value="2" />
                            从外部倒码
                        </label>
                        <label>
                            <input name="source_type" type="radio" id="source_type4" value="4" />
                            分销导入产品
                        </label>
                       
                        <label>
                            <input name="source_type" type="radio" id="source_type3" value="3" />
                            外来产品接口
                        </label>
                      
                    </div>

                    <div class="mi-form-item" id="trservice" style="display: none;">
                        <label class="mi-label">
                            服务商</label>
                        <select id="selservice" style="margin-left: 0px;">
                            <option value="0">请选择供应商</option>
                       <%if (comid == 106 || comid == 1532)
                      { %> <option value="1">阳光绿洲</option>
                            <option value="2">慧择网</option>
                            <option value="3">美景联动</option>
                        <%} %>
                            <option value="4">万龙接口</option>
                        </select>
                    </div>
                    <div class="mi-form-item" id="trservice_proid" style="display: none;">
                        <label class="mi-label">
                            服务商产品编号（产品id）</label>
                        <input type="text" id="txtservice_proid" class="mi-input" value="" /><input name="up" type="button" id="selectwl" value="选择WL产品" style="width: 80px; height: 26px;" />
                    </div>
                    <div class="mi-form-item" id="trrealnametype" style="display: none;">
                        <label class="mi-label">
                            服务商实名制类型</label>
                        <select id="selreal_name_type" class="mi-input" style="margin-left: 0px;">
                            <option value="0">非实名制</option>
                            <option value="1">一张一人</option>
                            <option value="2">一单一人</option>
                            <option value="3">一单一人+身份证</option>
                        </select>
                    </div>

                    <div class="mi-form-item">
                        <label class="mi-label">
                            重量(kg)</label>
                        <input name="pro_weight" type="text" id="pro_weight" class="mi-input" value="0" style="width: 100px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            优惠及说明</label>
                        <input name="pro_Explain" type="text" id="pro_Explain" class="mi-input" value=""
                            size="100" style="width: 300px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            每单限购数量(如不限购设为0)</label>
                        <input name="pro_Number" type="text" id="pro_Number" class="mi-input" value="" size="100"
                            style="width: 100px;" />
                    </div>
                    <div class="mi-form-item">
                        <label>
                            <input name="manyspeci" class="selectspeci" value="0" checked="checked" type="radio">
                            单规格产品
                        </label>
                        <label>
                            <input name="manyspeci" class="selectspeci" value="1" type="radio">
                            多规格产品<span style="color: Red;">(规格名称和规格值不可包含标点符号;保险产品规格类型至少包含:承保年龄;保障期限;购买份数)</span>
                        </label>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            门市价</label>
                        <input name="face_price" type="text" class="mi-input" id="face_price" value="" style="width: 100px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            直销价格</label>
                        <input name="advise_price" type="text" class="mi-input" id="advise_price" value=""
                            style="width: 100px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            一级分销价</label>
                        <input name="agent1_price" type="text" class="mi-input" id="agent1_price" value=""
                            style="width: 100px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            二级分销价</label>
                        <input name="agent2_price" type="text" class="mi-input" id="agent2_price" value=""
                            style="width: 100px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            三级分销价</label>
                        <input name="agent3_price" type="text" class="mi-input" id="agent3_price" value=""
                            style="width: 100px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            结算价</label>
                        <input name="agentsettle_price" type="text" class="mi-input" id="agentsettle_price"
                            value="" style="width: 100px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            儿童减免</label>
                        <input type="text" class="mi-input" id="childreduce" value="0" style="width: 100px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            返积分</label>
                        <input name="Integral_price" type="text" class="mi-input" id="Integral_price" value=""
                            style="width: 100px;" />(产品支付成功后返还用户的积分，将直接充入用户账户。)
                    </div>
                    <script type="text/javascript">
                        $(function () {
                            $("#hid_guigeNum").val("-1");
                            //添加规格
                            $(".js_sku_add").live("click", function () {


                                //编辑和添加操作隐藏
                                $(".show_standard_wrp").each(function (i, item) {
                                    $(item).find(".js_sku_sxedit").addClass("dn");
                                    $(item).find(".js_delete").addClass("dn");
                                })
                                //$(".add_standard_wrp").addClass("dn");

                                $(this).next().removeClass("dn");

                                var guigeNum = $("#hid_guigeNum").val();
                                $("#hid_guigeNum").val(parseInt(guigeNum) + 1);
                            })
                            //添加规格值
                            $(".js_add_itembtn").live("click", function () {
                                var specititle = $(this).parents(".js_sku_form").find(".js_sku_title").val();

                                if (specititle == "") {
                                    alert("规格名称不可为空");
                                    return;
                                } else {
                                    var dealspecititle = specititle.replace(/[\ |\~|\`|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\-|\_|\+|\=|\||\\|\[|\]|\{|\}|\;|\:|\"|\'|\,|\<|\.|\>|\/|\?]/g, "");
                                    if (specititle != dealspecititle) {
                                        alert("规格名称中不可包含标点符号");
                                        return;
                                    }
                                }
                                var itemval = $(this).prev().find(".js_add_itemname").val();

                                if (itemval == "") {
                                    alert("规格值不可为空");
                                    return;
                                }
                                else {
                                    var dealitemval = itemval.replace(/[\ |\~|\`|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\-|\_|\+|\=|\||\\|\[|\]|\{|\}|\;|\:|\"|\'|\,|\<|\.|\>|\/|\?]/g, "");
                                    if (itemval != dealitemval) {
                                        alert("规格值不可包含标点符号");
                                        return;
                                    }
                                }
                                $(this).siblings(".js_sku_addlist").append(
                               ' <span data-name="' + itemval + '" class="custom_standard_item js_span js_tbadd">' +
                                      '<span class="standard_item_name">' + itemval + '</span>' +
                                      '<a class="btn_del" href="javascript:void(0);">X</a>' +
                                '</span>');
                                $(this).prev().find(".js_add_itemname").val("");
                            })
                            //删除规格值 
                            $(".edit_btn_del").live("click", function () {
                                var delht = $(this).parent().prop("outerHTML");
                                var ht = $(this).parents(".js_sku_addlist").prop("outerHTML");
                                var newht = ht.replace(delht, "");
                                $(this).parents(".js_sku_addlist").html(newht);

                                $("#createTable").html("");
                            })
                            //取消按钮
                            $(".js_cancel").live("click", function () {
                                if ($(this).parents(".add_standard_wrp").length > 0) {
                                    //取消添加规格
                                    $(this).parents(".js_sku_form").addClass("dn");
                                    $(this).parents(".js_sku_form").find(".js_sku_title").val("");
                                    $(this).parents(".js_sku_form").find(".js_add_itemname").val("");
                                    $(this).parent().prev().empty();

                                    //编辑和添加操作显示
                                    $(".show_standard_wrp").each(function (i, item) {
                                        $(item).find(".js_sku_sxedit").removeClass("dn");
                                        $(item).find(".js_delete").removeClass("dn");
                                    })
                                }
                                else {
                                    //取消编辑规格
                                    var selindex = $(this).parents(".edit_standard_wrp").attr("data-index");
                                    $(".js_sku_box" + selindex).addClass("dn");
                                    $(".js_wrp_level" + selindex).removeClass("dn");

                                    //编辑和添加操作显示
                                    $(".show_standard_wrp").each(function (i, item) {
                                        $(item).find(".js_sku_sxedit").removeClass("dn");
                                        $(item).find(".js_delete").removeClass("dn");
                                    })
                                    $(".add_standard_wrp").removeClass("dn");
                                }
                            })
                            //确定按钮
                            $(".js_sure").live("click", function () {
                                if ($(this).parents(".add_standard_wrp").length > 0) {
                                    //新增规格
                                    var specititle = $(this).parents(".js_sku_form").find(".js_sku_title").val();
                                    var specivalues = "";
                                    $(this).parents(".js_sku_form").find(".js_sku_addlist").find(".js_tbadd").each(function (i, item) {
                                        var itemname = $(item).find(".standard_item_name").text();
                                        if (itemname != "") {
                                            specivalues += '{Name:"' + itemname + '",isonline:"0"}' + ',';
                                        }
                                    })

                                    if (specititle == "") {
                                        alert("规格名称不可为空!");
                                        return;
                                    }
                                    if (specivalues.length > 0) {
                                        specivalues = specivalues.substring(0, specivalues.length - 1);
                                        specivalues = '[' + specivalues + ']';
                                    } else {
                                        alert("规格值不可为空!");
                                        return;
                                    }

                                    var jsondata = [
                                       {
                                           GuigeNum: $("#hid_guigeNum").val(),
                                           GuigeTitle: specititle,
                                           GuigeValues: eval("(" + specivalues + ")")
                                       }
                                    ];

                                    $('.add_standard_wrp').before($('#speciTmpl').tmpl(jsondata));



                                    //编辑和添加操作显示
                                    $(".show_standard_wrp").each(function (i, item) {
                                        $(item).find(".js_sku_sxedit").removeClass("dn");
                                        $(item).find(".js_delete").removeClass("dn");
                                    })
                                    $(".add_standard_wrp").removeClass("dn");

                                    $(this).next().trigger("click");
                                    $(this).parents(".ext_box").find(".frm_tips").addClass("dn");
                                }
                                else {


                                    var selindex = $(this).parents(".edit_standard_wrp").attr("data-index");

                                    //编辑规格
                                    var specititle = $(this).parents(".js_sku_form").find(".js_sku_title").val();

                                    var specivalues = "";
                                    $(this).parents(".js_sku_form").find(".js_sku_addlist").find(".js_span").each(function (i, item) {
                                        var itemname = $(item).find(".standard_item_name").text();
                                        if (itemname != "") {
                                            specivalues += '{Name:"' + itemname + '",isonline:"0"}' + ',';
                                        }
                                    })

                                    if (specititle == "") {
                                        alert("规格名称不可为空!");
                                        return;
                                    }
                                    if (specivalues.length > 0) {
                                        specivalues = specivalues.substring(0, specivalues.length - 1);
                                        specivalues = '[' + specivalues + ']';
                                    } else {
                                        alert("规格值不可为空!");
                                        return;
                                    }

                                    var jsondata = [
                                       {
                                           GuigeNum: selindex,
                                           GuigeTitle: specititle,
                                           GuigeValues: eval("(" + specivalues + ")")
                                       }
                                    ];


                                    $('#speciTmpl').tmpl(jsondata).appendTo("#tempht");

                                    var inithtall = $(".goods_standard").html();

                                    var inithtpart1 = $(this).parents(".edit_standard_wrp").prev().prop("outerHTML");
                                    var inithtpart2 = $(this).parents(".edit_standard_wrp").prop("outerHTML");

                                    var newht = inithtall.replace(inithtpart1, $("#tempht").find(".edit_standard_wrp").prev().prop("outerHTML")).replace(inithtpart2, $("#tempht").find(".edit_standard_wrp").prop("outerHTML"));

                                    $("#tempht").html("");
                                    $(".goods_standard").html(newht);



                                    //编辑和添加操作显示
                                    $(".show_standard_wrp").each(function (i, item) {
                                        $(item).find(".js_sku_sxedit").removeClass("dn");
                                        $(item).find(".js_delete").removeClass("dn");
                                    })
                                    $(".add_standard_wrp").removeClass("dn");


                                    $(".goods_standard").find(".frm_tips").addClass("dn");

                                    //取消编辑规格 
                                    $(".js_sku_box" + selindex).addClass("dn");
                                    $(".js_wrp_level" + selindex).removeClass("dn");
                                }

                                $("#createTable").html("");
                            })

                            //编辑按钮
                            $(".js_sku_sxedit").live("click", function () {
                                //编辑和添加操作隐藏
                                $(".show_standard_wrp").each(function (i, item) {
                                    $(item).find(".js_sku_sxedit").addClass("dn");
                                    $(item).find(".js_delete").addClass("dn");
                                })
                                $(".add_standard_wrp").addClass("dn");

                                var selindex = $(this).attr("data-idx");
                                $(".js_wrp_level" + selindex).addClass("dn");
                                $(".js_sku_box" + selindex).removeClass("dn");

                            })

                            //删除按钮
                            $(".show_js_delete").live("click", function () {
                                var selid = $(this).attr("data-idx");

                                var inithtall = $(".goods_standard").html();

                                var inithtpart1 = $(this).parents(".show_standard_wrp").prop("outerHTML");
                                var inithtpart2 = $(this).parents(".show_standard_wrp").next().prop("outerHTML");

                                var newht = inithtall.replace(inithtpart1, "").replace(inithtpart2, "");


                                $(".goods_standard").html(newht);

                                $("#createTable").html("");

                            })

                            //第一行填入价格后，其他行如果不为空的话自动录入添加
                            $("#process input[name='Txt_facePriceSon']:first").live("blur", function () {

                                var defaultval = $("#process input[name='Txt_facePriceSon']:first").val();

                                $("#process input[name='Txt_facePriceSon']").each(function (i, item) {

                                    if ($(item).val() == "") {
                                        $(item).val(defaultval);
                                    }
                                });
                            });
                            $("#process input[name='Txt_advicePriceSon']:first").live("blur", function () {

                                var defaultval = $("#process input[name='Txt_advicePriceSon']:first").val();

                                $("#process input[name='Txt_advicePriceSon']").each(function (i, item) {

                                    if ($(item).val() == "") {
                                        $(item).val(defaultval);
                                    }
                                });
                            });
                            $("#process input[name='Txt_agent1PriceSon']:first").live("blur", function () {

                                var defaultval = $("#process input[name='Txt_agent1PriceSon']:first").val();

                                $("#process input[name='Txt_agent1PriceSon']").each(function (i, item) {

                                    if ($(item).val() == "") {
                                        $(item).val(defaultval);
                                    }
                                });
                            });
                            $("#process input[name='Txt_agent2PriceSon']:first").live("blur", function () {

                                var defaultval = $("#process input[name='Txt_agent2PriceSon']:first").val();

                                $("#process input[name='Txt_agent2PriceSon']").each(function (i, item) {

                                    if ($(item).val() == "") {
                                        $(item).val(defaultval);
                                    }
                                });
                            });
                            $("#process input[name='Txt_agent3PriceSon']:first").live("blur", function () {

                                var defaultval = $("#process input[name='Txt_agent3PriceSon']:first").val();

                                $("#process input[name='Txt_agent3PriceSon']").each(function (i, item) {

                                    if ($(item).val() == "") {
                                        $(item).val(defaultval);
                                    }
                                });
                            });
                            $("#process input[name='Txt_settlePriceSon']:first").live("blur", function () {

                                var defaultval = $("#process input[name='Txt_settlePriceSon']:first").val();

                                $("#process input[name='Txt_settlePriceSon']").each(function (i, item) {

                                    if ($(item).val() == "") {
                                        $(item).val(defaultval);
                                    }
                                });
                            });
                            $("#process input[name='Txt_WeightSon']:first").live("blur", function () {

                                var defaultval = $("#process input[name='Txt_WeightSon']:first").val();

                                $("#process input[name='Txt_WeightSon']").each(function (i, item) {

                                    if ($(item).val() == "") {
                                        $(item).val(defaultval);
                                    }
                                });
                            });
                            $("#process input[name='Txt_CountSon']:first").live("blur", function () {

                                var defaultval = $("#process input[name='Txt_CountSon']:first").val();

                                $("#process input[name='Txt_CountSon']").each(function (i, item) {

                                    if ($(item).val() == "") {
                                        $(item).val(defaultval);
                                    }
                                });
                            });


                        })
                    </script>
                    <div class="mi-form-item speci">
                        <div class="goods_standard js_guige_multi" id="js_guige_main" style="">
                            <div class="ext_box standard_wrp js_levels">
                                <p class="frm_tips">
                                    暂无规格数据，请先添加规格</p>
                                <div class="add_standard_wrp js_sku_box_new js_sku_box">
                                    <a href="javascript:void(0);" class="js_sku_add">添加规格</a>
                                    <div class="add_standard custom_standard_wrp dn js_sku_form">
                                        <div class="frm_control_group">
                                            <label for="" class="frm_label">
                                                规格名称
                                            </label>
                                            <div class="frm_controls">
                                                <span class="frm_input_box append limit_append">
                                                    <input type="text" class="frm_input js_sku_title" autocomplete="off">
                                                    <span class="frm_input_append js_addnum">0/15</span> </span>
                                                <p class="frm_msg fail">
                                                    <span class="frm_msg_content">请填写规格名称，不超过15个字</span>
                                                </p>
                                            </div>
                                        </div>
                                        <div class="frm_control_group">
                                            <label for="" class="frm_label">
                                                规格值</label>
                                            <div class="frm_controls">
                                                <span class="frm_input_box append limit_append">
                                                    <input name="" type="text" placeholder="" class="frm_input js_add_itemname" autocomplete="off">
                                                    <span class="frm_input_append js_addnum">0/15</span> </span><a href="javascript:;"
                                                        class="btn btn_default btn_add_standard js_add_itembtn">添加</a>
                                                <!-- <p class="frm_msg fail">
                                                    <span class="frm_msg_content">请至少添加一个规格值</span>
                                                </p>-->
                                                <div class="custom_standard js_sku_addlist">
                                                    <!--<span data-name="1寸" class="custom_standard_item js_span js_tbadd">
                                                        <span class="standard_item_name">1寸</span>
                                                        <a class="btn_del" href="javascript:void(0);">X</a>
                                                    </span>-->
                                                </div>
                                                <div class="oper">
                                                    <a href="javascript:;" class="btn btn_primary js_sure">确定</a> <a href="javascript:;"
                                                        class="btn btn_default js_cancel">取消</a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="mi-form-item speci">
                        <label class="mi-label">
                            价格&库存
                        </label>
                        <ul>
                            <li>
                                <div id="createTable">
                                </div>
                            </li>
                        </ul>
                    </div>
                    <input type="hidden" id="hid_guigeNum" value="-1" />
                    <script type="text/x-jquery-tmpl" id="speciTmpl">   
                         <div class="show_standard_wrp js_wrp_level js_wrp_level${GuigeNum}">
                                    <p>
                                        <strong>${GuigeTitle}</strong>&nbsp;&nbsp;
                                        <a href="javascript:void(0);" data-idx="${GuigeNum}" class="btn_edit js_sku_sxedit">编辑</a>&nbsp;&nbsp;
                                        <a  href="javascript:;"  data-idx="${GuigeNum}" data-name="${GuigeTitle}" class="btn_del js_delete show_js_delete">删除</a>
                                    </p>
                                    <div class="checkbox_wrp js_level js_level${GuigeNum}" data-id="$${GuigeTitle}" data-index="${GuigeNum}" data-name="${GuigeTitle}">
                                       {{each GuigeValues}}
                                        <label class="frm_checkbox_label" > 
                                            {{if $value.isonline==1}}
                                            <input type="checkbox" class="frm_checkbox js_guige_child" value="${$value.Name}" data-guige="${GuigeTitle}"  data-name="${$value.Name}" name="ckguige" checked="checked">
                                            {{else}}
                                             <input type="checkbox" class="frm_checkbox js_guige_child" value="${$value.Name}" data-guige="${GuigeTitle}"  data-name="${$value.Name}" name="ckguige">
                                            {{/if}}
                                            <span class="lbl_content">${$value.Name}</span> 
                                            
                                        </label>
                                        {{/each}}
                                    </div>
                          </div>
                         <div class="edit_standard_wrp   js_sku_box${GuigeNum} js_sku_box dn" data-id="$${GuigeTitle}" data-index="${GuigeNum}"   data-name="${GuigeTitle}">
                                    <div class="add_standard  js_sku_form">
                                        <div class="frm_control_group">
                                            <label for="" class="frm_label">
                                                规格名称
                                            </label>
                                            <div class="frm_controls">
                                                <span class="frm_input_box append limit_append">
                                                    <input type="text" class="frm_input js_sku_title" data-id="$${GuigeTitle}" data-name="${GuigeTitle}" value="${GuigeTitle}">
                                                    <span class="frm_input_append js_addnum">0/15</span> 
                                                </span>
                                                <p class="frm_msg fail">
                                                    <span class="frm_msg_content">请填写规格名称，不超过15个字</span>
                                                </p>
                                            </div>
                                        </div>
                                        <div class="frm_control_group">
                                            <label for="" class="frm_label">
                                                规格值</label>
                                            <div class="frm_controls">
                                                <span class="frm_input_box append limit_append">
                                                    <input name="" type="text" placeholder="" class="frm_input js_add_itemname">
                                                    <span class="frm_input_append js_addnum">0/15</span> </span><a href="javascript:;"
                                                        class="btn btn_default btn_add_standard js_add_itembtn">添加</a>
                                                <p class="frm_msg fail">
                                                    <span class="frm_msg_content">请至少添加一个规格值</span>
                                                </p>
                                                <p class="frm_tips">
                                                    输入回车即可直接添加</p> 
                                                <div class="custom_standard js_sku_addlist">
                                                  {{each GuigeValues}}
                                                    <span class="custom_standard_item js_span" data-name="${$value.Name}" data-id="$${$value.Name}">
                                                    <span class="standard_item_name">
                                                        ${$value.Name}</span>
                                                    <a href="javascript:void(0);" class="btn_del edit_btn_del">X</a>
                                                    </span> 
                                                   {{/each}}   
                                                </div>
                                                <div class="oper">
                                                    <a href="javascript:;" class="btn btn_primary js_sure">确定</a><a href="javascript:;"
                                                        class="btn btn_default js_cancel">取消</a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                    </script>
                    <div id="tempht" class="dn">
                        <!--用途:临时保存字符串-->
                    </div>
                    <div class="mi-form-item" id="div_pro_Remark">
                        <label class="mi-label">
                            产品特色(显示在旅游详情页面)</label>
                        <textarea name="pro_Remark" cols="119" rows="3" id="pro_Remark"></textarea>
                    </div>
                    <div class="mi-form-item" id="div_pro_note">
                        <label class="mi-label">
                            备注(显示在纸质票备注说明中)</label>
                        <input name="pro_note" type="text" class="mi-input" id="pro_note" value="" style="width: 450px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            服务内容
                        </label>
                        <textarea name="service_Contain" cols="119" rows="5" class="mi-input" id="service_Contain"
                            style="width: 300px;"></textarea>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            不含服务需自费项目说明</label>
                        <textarea name="service_NotContain" cols="119" rows="5" class="mi-input" id="service_NotContain"
                            style="width: 300px;"></textarea>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            注意事项</label>
                        <textarea name="Precautions" cols="119" rows="5" class="mi-input" id="Precautions"
                            style="width: 300px;"></textarea>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            电子凭证短信设置</label>
                        <textarea name="Sms" cols="119" rows="5" class="mi-input" id="Sms" style="width: 600px;">您已经成功购买$产品名称$，$数量$，电子码为：$票号$，有效期:$有效期$,使用日期：$使用日期$，预订电话：$预订电话$,$入住日期$ ,$离店日期$,入住天数:$几天$,$评价链接$。。</textarea>
                        <br />
                        $票号$ $姓名$ $数量$ $有效期$ $产品名称$ ,$二维码地址$ 进行替换为相应的值。
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <!--是否参加抢购-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;" id="div_ispanicbuy">
                    <div class="mi-form-item" style="margin-bottom: 20px;">
                        <label class="mi-label">
                            是否参加抢购/限购</label>
                        <label>
                            <input name="ispanicbuy" type="radio" value="0" checked="checked" />
                            不参加
                        </label>
                        <label>
                            <input name="ispanicbuy" type="radio" value="1" />
                            参加抢购
                        </label>
                        <label>
                            <input name="ispanicbuy" type="radio" value="2" />
                            参加限购
                        </label>
                    </div>
                    <div id="ddiv_ispanicbuy" style="display: none;">
                        <div class="mi-form-item" style="" id="ddiv_begintime">
                            <label class="mi-label">
                                开始抢购时间</label>
                            <input type="text" id="panicbuy_begintime" class="mi-input ui_timepicker" value="" />
                        </div>
                        <div class="mi-form-item" style="" id="ddiv_endtime">
                            <label class="mi-label">
                                结束抢购时间</label>
                            <input type="text" id="panicbuy_endtime" class="mi-input ui_timepicker" value="" />
                        </div>
                        <div class="mi-form-item" style="">
                            <label class="mi-label">
                                剩余可销售数量</label>
                            <input type="text" id="panicbuy_limitbuytotalnum" class="mi-input" value="10000"
                                readonly="readonly" style="background-color: #cccccc;" />
                            <input type="button" id="btnuplimitbuytotalnum" value="  调整  " onclick="referrer_ch()"
                                class="mi-input" />
                            <div id="rhshow" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
                                width: 400px; height: 130px; display: none; left: 20%; position: absolute; top: 20%;
                                z-index: 1000;" class="dialog">
                                <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
                                    style="padding: 5px;">
                                    <tr>
                                        <td height="42" colspan="2" bgcolor="#C1D9F3">
                                            <span style="padding-left: 10px; font-size: 18px;" id="span_rh">剩余可销售数量调整</span>
                                        </td>
                                    </tr>
                                    <tr bgcolor="#FFFFFF">
                                        <td>
                                            调整数量:
                                        </td>
                                        <td>
                                            <input id="reduce_rh" type="button" class="formButton" value="减少" onclick="reducerh()" /><input
                                                type="text" value="0" id="txtupnum" /><input id="add_rh" type="button" class="formButton"
                                                    value="增加" onclick="addrh()" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                                            <input id="cancel_rh" onclick="closerh()" type="button" class="formButton" value="  关闭  " />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <!--已销售数量,只读；编辑产品时不操作-->
                        <div class="mi-form-item" style="">
                            <label class="mi-label">
                                已销售数量</label>
                            <input type="text" id="buynum" class="mi-input" value="0" readonly="readonly" style="background-color: #cccccc;" />
                        </div>
                        <div class="mi-form-item" style="">
                            <label class="mi-label" id="saletotalnum">
                            </label>
                        </div>
                    </div>
                </div>
                <!--是否需要预约-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;" id="div_isneedbespeak">
                    <div class="mi-form-item" style="margin-bottom: 20px;">
                        <label class="mi-label">
                            是否需要预约</label>
                        <label>
                            <input name="isneedbespeak" type="radio" value="0" checked="checked" />
                            不需要
                        </label>
                        <label>
                            <input name="isneedbespeak" type="radio" value="1" />
                            需要
                        </label>
                    </div>
                    <div id="ddiv_isneedbespeak" style="display: none;">
                        <div class="mi-form-item" style="">
                            <label class="mi-label">
                                每日预约人数</label>
                            <input type="text" id="daybespeaknum" class="mi-input" value="" />
                        </div>
                        <div class="mi-form-item" style="">
                            <label class="mi-label">
                                预约成功短信</label>
                            <textarea cols="119" rows="5" class="mi-input" id="bespeaksucmsg" style="width: 600px;">您已经成功预约$产品名称$，$数量$人，电子码：$票号$，请在预约当日($预约日期$)使用。客服电话$客服电话$。</textarea>
                            <br />
                            $客服电话$ $票号$ $姓名$ $数量$ $预约日期$ $产品名称$ 进行替换为相应的值。
                        </div>
                        <div class="mi-form-item" style="">
                            <label class="mi-label">
                                预约失败短信</label>
                            <textarea cols="119" rows="5" class="mi-input" id="bespeakfailmsg" style="width: 600px;">对不起，你$预约日期$的预约失败，请预约其他日期，给你带来的不便敬请谅解。客服电话$客服电话$。</textarea>
                            <br />
                            $客服电话$ $票号$ $姓名$ $数量$ $预约日期$ $产品名称$ 进行替换为相应的值。
                        </div>
                        <div class="mi-form-item" style="margin-bottom: 20px;">
                            <label class="mi-label">
                                预约处理人电话</label>
                            <input type="text" id="customservicephone" class="mi-input" value="" />
                        </div>
                    </div>
                </div>
                <!--是否限定使用日期-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;" id="div_isblackoutdate">
                    <div class="mi-form-item" style="margin-bottom: 20px;">
                        <label class="mi-label">
                            是否限定使用日期</label>
                        <label>
                            <input name="isblackoutdate" type="radio" value="0" checked="checked" />
                            不限定
                        </label>
                        <label>
                            <input name="isblackoutdate" type="radio" value="1" />
                            限定
                        </label>
                    </div>
                    <div id="ddiv_isblackoutdate" style="display: none;">
                        <div class="mi-form-item" style="margin-bottom: 20px;">
                            <label class="mi-label">
                                电子票类型</label>
                            <label>
                                <input name="etickettype" type="radio" value="0" checked="checked" />
                                平日票
                            </label>
                            <label>
                                <input name="etickettype" type="radio" value="1" />
                                周末票
                            </label>
                            <label>
                                <input name="etickettype" type="radio" value="2" />
                                节假日票
                            </label>
                            <label>
                                <a href="/ui/pmui/eticket_useset.aspx" target="_blank">设置商户特定日期</a>
                            </label>
                        </div>
                    </div>
                </div>
                <!--配送费用-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;" id="div_deliveryfee">
                    <div class="mi-form-item" style="margin-bottom: 20px;">
                        <label class="mi-label">
                            配送费用</label>
                        <label>
                            <input name="ishasdeliveryfee" type="radio" value="0" checked="checked" />
                            包邮
                        </label>
                        <label>
                            <input name="ishasdeliveryfee" type="radio" value="1" />
                            不包邮
                        </label>
                    </div>
                    <div id="div_deliverytmp" style="display: none;">
                        <div class="mi-form-item" style="margin-bottom: 20px;">
                            <label class="mi-label">
                                选择运费模板</label>
                            <label>
                                <select id="sel_deliverytmp" class="mi-input">
                                </select>
                            </label>
                            <label>
                                <a href="/ui/pmui/delivery/deliverylist.aspx" target="_blank">设置运费模板</a>
                            </label>
                        </div>
                    </div>
                </div>
                <!--产品有效期-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <div class="mi-form-item">
                        <label class="mi-label">
                            产品有效期</label>
                        开始
                        <input name="pro_start" type="text" id="pro_start" class="mi-input" value="" size="12"
                            isdate="yes" />
                        截止
                        <input name="pro_end" type="text" id="pro_end" class="mi-input" value="" size="12"
                            isdate="yes" />
                    </div>
                    <div class="mi-form-item" id="div_ProValidateMethod">
                        <label class="mi-label">
                            产品验证有效期</label>
                        <label>
                            <input name="ProValidateMethod" type="radio" value="1" checked="checked" />
                            按产品有效期
                        </label>
                        <label>
                            <input name="ProValidateMethod" type="radio" value="2" />
                            按指定有效期
                        </label>
                    </div>
                    <div class="mi-form-item" id="div_appointdata" style="background-color: rgb(239, 239, 239);
                        display: none;">
                        <select id="sel_appointdata" class="mi-input">
                            <option value="0">未指定</option>
                            <option value="1">一星期</option>
                            <option value="2">一个月</option>
                            <option value="3">三个月</option>
                            <option value="4">六个月</option>
                            <option value="5">一 年</option>
                        </select>
                    </div>
                    <div class="mi-form-item" id="div_iscanuseonsameday">
                        <label class="mi-label">
                            使用限制</label>
                        <select id="sel_iscanuseonsameday" class="mi-input">
                            <option value="1">当天出票可用</option>
                            <option value="2">2小时内出票不可用</option>
                            <option value="0">当天出票不可用</option>
                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            产品显示设置</label>
                        <select id="sel_viewmethod" class="mi-input">
                            <option value="1">全部显示</option>
                            <option value="2">分销后台显示</option>
                            <option value="3">官方微信显示</option>
                            <option value="4">官方网站显示</option>
                            <option value="5">微信和官网显示</option>
                            <option value="6">全不显示,只特定分销授权例外</option>
                            <option value="7">默认不显示，可以通过产品二维码预定</option>
                        </select>
                        <input type="button" name="setagent" id="setagent" value="  特定分销授权  " class="mi-input" />
                        <%-- <div style="display: none;" id="setagentinfo">
                            特定授权请先添加产品，添加好后。再次管理的管理时进行授权</div>--%>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            产品上线状态</label>
                        <select id="selprostate" class="mi-input">
                            <option value="1">上线</option>
                            <option value="0">下线</option>
                        </select>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <!--退票机制-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10; display: none;" id="div_QuitTicketMechanism">
                    <div class="mi-form-item">
                        <label class="mi-label">
                            退票机制</label>
                        <select id="sel_QuitTicketMechanism" class="mi-input" style="margin-left: 0px;">
                            <option value="0">有效期内可退票</option>
                            <option value="1">有效期内/外 均可退票</option>
                            <option value="2">不可退票</option>
                        </select>
                    </div>
                </div>
                <!--预约产品 附加信息-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10; display: none;" id="div_bookpro">
                    <div class="mi-form-item">
                        <label class="mi-label">
                            关联人公司</label>
                        <input type="text" id="txt_bookpro_bindcompany" class="mi-input" value="" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            产品关联人及预约产确认</label>
                        <input type="text" id="txt_bookpro_bindname" class="mi-input" value="" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            产品关联人手机</label>
                        <input type="text" id="txt_bookpro_bindphone" class="mi-input" value="" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            预约方式</label>
                        <select id="sel_bookpro_ispay" class="mi-input" style="margin-left: 0px;">
                            <option value="0">免支付预约</option>
                            <option value="1">支付后预约</option>
                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            用户订购后需关联人确认后才能支付</label>
                        <select id="unsure" name="unsure" class="mi-input" style="margin-left: 0px;">
                            <option value="0" selected>不需要确认</option>
                            <option value="1">需要确认</option>
                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            产品有效期内可验证/限定订单预约当天验证</label>
                        <select id="unyuyueyanzheng" name="unyuyueyanzheng" class="mi-input" style="margin-left: 0px;">
                            <option value="0" selected>产品有效期内可以验证</option>
                            <option value="1">限定预约当天验证</option>
                        </select>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <!--是否返佣-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10; display: ;" id="div_isrebate">
                    <div class="mi-form-item">
                        <label class="mi-label">
                            是否给推荐人返佣</label>
                        <select id="sel_isrebate" class="mi-input" style="margin-left: 0px;">
                            <option value="0">不返佣</option>
                            <option value="1">返佣</option>
                        </select>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <!--绑定保险产品-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10; display: none;" id="div_bindbx">
                    <div class="mi-form-item">
                        <label class="mi-label">
                            绑定保险产品</label>
                        <select id="selbindbx" class="mi-input" style="margin-left: 0px;">
                        </select>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <!--绑定服务-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10; display: none;" id="div_bindserver">
                    <div class="mi-form-item">
                        <label class="mi-label">
                            是否需要支付押金</label>
                        <label>
                            <input name="Wrentserver" type="radio" value="0" checked="checked" onclick="readRentserver();" />
                            不需要支付（自带）
                        </label>
                        <label>
                            <input name="Wrentserver" type="radio" value="1" onclick="readRentserver();" />
                            需要支付（租用）
                        </label>
                    </div>
                    <div class="mi-form-item"  style="display:none;">
                        <label class="mi-label">
                            有效时长</label>
                        <label>
                        <select id="worktimehour" class="mi-input" style="margin-left: 0px;">
                            <option value="" selected="selected">请选择有效时长</option>
                            <option value="4">4小时</option>
                            <option value="8">1天</option>
                            <option value="12" >1.5天</option>
                            <option value="16">2天</option>
                        </select>
                           
                        </label>
                    </div>
                    <div class="mi-form-item"  style="display:none;">
                        <label class="mi-label">
                            下班时间</label>
                        <select id="worktimeid" class="mi-input" style="margin-left: 0px;">
                            <option value="0" selected="selected">默认下班时间17:00</option>
                        </select>
                        <br>
                        注意：系统默认下班时间为17:00点整
                    </div>
                     <div class="mi-form-item"  style="display:none;">
                        <label class="mi-label">
                            可使用次数</label>
                        <input type="text" id="zhaji_usenum" class="mi-input" value="999" />
                        <br>
                        注意：默认999次
                    </div>
                    <div class="mi-form-item rentserver" style="display: none;">
                        <label class="mi-label">
                            可通过的闸机（必须选择否则无法过闸机）</label>
                        <div id="bandingzhaji">
                        </div>
                    </div>
                    <div class="mi-form-item " id="ishasyajin" style="display:none;">
                        <label class="mi-label">
                            服务验证时是否需要支付押金</label>
                        <label  style="display: ;">
                            <input name="WDeposit" type="radio" value="1" onclick="readWDeposit();" />
                            需要支付
                        </label>
                        <label>
                            <input name="WDeposit" type="radio" value="0" onclick="readWDeposit();" checked="checked" />
                            不需要支付，不带其他验证服务
                        </label>
                        <div>
                            注意：没有支付或办理押金，不能验证后续服务。但不影响闸机刷卡使用
                        </div>
                    </div>
                    <div class="mi-form-item " id="serverDepositprice" style="display: none;">
                        <label class="mi-label">
                            押金金额</label>
                        <label>
                            <input name="Depositprice" id="Depositprice" class="mi-input" type="text" value="" />
                            元
                        </label>
                    </div>
                    <div class="mi-form-item payyajin" style="display: none;">
                        <label class="mi-label">
                            可购使用的服务列表</label>
                        <div id="serverlist">
                        </div>
                    </div>
                    
                    <div class="mi-form-explain">
                    </div>
                </div>
                <!--绑定 商品编号 用于万龙对接-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10; display: none;" id="div_bindservercode">
                    <div class="mi-form-item">
                        <label class="mi-label">
                            商品编号</label>
                         <input type="text" id="merchant_code" class="mi-input" value="" />
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>


                <!--电子码 核销类型(一码一验,一码多验)-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10; display: none;" id="div_pro_yanzheng_method">
                    <div class="mi-form-item">
                        <label class="mi-label">
                            核销类型（一码多验）</label>
                        <select id="pro_yanzheng_method" class="mi-input" style="margin-left: 0px;">
                            <option value="0" selected="selected">一码多验（一个订单只生成一个电子码）</option>
                            <option value="1">一码一验（一个订单产生多个电子码）</option>
                        </select>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <!--首发站发车时间-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10; display: none;" id="div_firststationtime">
                    <div class="mi-form-item">
                        <label class="mi-label">
                            首发站发车时间</label>
                        <label>
                            <input id="firststationtime" class="mi-input" type="text" value="" placeholder="形如:07:30:00" />
                        </label>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <!--产品预订一张产生电子码个数-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10; display: none;" id="div_pnonumperticket">
                    <div class="mi-form-item">
                        <label class="mi-label">
                            产品预订一张产生电子码个数（特别注意：实际使用数量=订购数量*电子码产生数量），不明白的不要修改，默认值为1</label>
                        <label>
                            <input id="txt_pnonumperticket" class="mi-input" type="text" value="1" placeholder="需要为数字格式" />
                        </label>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <!--产品指定验证POSID-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10; display: none;" id="div_SpecifyPosid">
                    <div class="mi-form-item">
                        <label class="mi-label">
                            产品指定验证POSID(如不清楚请不要设置，否则会造成验票出错)</label>
                        <label>
                            <select id="SpecifyPosid" class="mi-input" style="margin-left: 0px;">
                            </select>
                        </label>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <!--是否需要提交游玩日期,万龙系统没有对接功能，需要我们在用户下单后，在万龙系统重新提一笔订单(提单时会用到游玩日期)-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10; display: none;" id="div_isSetVisitDate">
                    <div class="mi-form-item">
                        <label class="mi-label">
                            购买人是否需要提交使用日期</label>
                        <label>
                            <input name="isSetVisitDate" type="radio" value="0" checked="checked" />
                            不需要
                        </label>
                        <label>
                            <input name="isSetVisitDate" type="radio" value="1" />
                            需要
                        </label>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                  <!--是否需要提交身份证号-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10; display: none;" id="div_issetidcard">
                    <div class="mi-form-item">
                        <label class="mi-label">
                            购买人是否需要提交身份证号</label>
                        <label>
                            <input name="issetidcard" type="radio" value="0" checked="checked" />
                            不需要
                        </label>
                        <label>
                            <input name="issetidcard" type="radio" value="1" />
                            需要
                        </label>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                 
                <!--产品组合 以便于用户提单-->
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10; display: none;" id="div_progroupid">
                    <div class="mi-form-item">
                        <label class="mi-label">
                            产品组合</label>
                        <select id="sel_progroupid" class="mi-input" style="margin-left: 0px;">
                        </select>
                        <label>
                            <a href="javascript:void(0)" onclick="groupmanage()" id="a_groupmanage">组合管理</a>
                        </label>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <table border="0">
                    <tr>
                        <td width="600" height="80" align="center">
                            <input type="hidden" name="ThatDay_can" value="1" />
                            <input type="hidden" name="tuan_pro" value="1" />
                            <input type="hidden" name="zhixiao" value="1" />
                            <input type="hidden" name="agentsale" value="1" />
                            <input name="tuipiao" type="hidden" id="tuipiao" value="1" />
                            <input name="tuipiao_guoqi" type="hidden" id="tuipiao_guoqi" value="0" />
                            <input name="tuipiao_endday" type="hidden" id="tuipiao_endday" value="" />
                            <input type="button" name="GoProAddNext" id="GoProAddNext" value="  确认提交  " class="mi-input" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/javascript">
        $(function () {

            $("#com_type option[value='<%=industryid %>']").attr("selected", true);
            $("#com_type").change(function () {
                //加载类目
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=proclasslist",
                    data: { pageindex: 1, pagesize: 200, industryid: $("#com_type").trimVal() },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        $("#proclass").empty();
                        //$("#proclass").append("<option value='0' >请选择所属类目</option>");
                        if (data.type == 1) {
                            return;
                        }
                        if (data.type == 100) {
                            for (i = 0; i < data.totalCount; i++) {
                                $("#proclass").append("<option value='" + data.msg[i].Id + "'>" + data.msg[i].Classname + "</option>");
                            }
                        }
                    }
                })
            })


            //加载分销商
            var agentlist
            var pageSize = 10;
            var pageindex = 1;
            SearchList(1);
            function SearchList(pageindex) {
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=agentsetwarrantpagelist",
                    data: { pageindex: pageindex, pagesize: 10, comid: $("#hid_comid").trimVal(), proid: $("#hid_proid").trimVal() },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();

                            if (data.totalCount == 0) {
                                //                                $("#tblist").html("查询数据为空");
                            } else {

                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);
                            }
                        }
                    }
                })
            }

            $("#setagent").click(function () {
                $("#bindingagent").show();
            })
            $("input:[name='cancel']").click(function () {

                $("#bindingagent").hide();
                $("#div_articaltype").hide();
            })


            if ($("#hid_proid").trimVal() == 0) {
                $("#setagent").hide();
                $("#setagentinfo").show();
            }


            //分页
            function setpage(newcount, newpagesize, curpage) {
                $("#divPage").paginate({
                    count: Math.ceil(newcount / newpagesize),
                    start: curpage,
                    display: 5,
                    border: false,
                    text_color: '#888',
                    background_color: '#EEE',
                    text_hover_color: 'black',
                    images: false,
                    rotate: false,
                    mouse: 'press',
                    onChange: function (page) {

                        SearchList(page);

                        return false;
                    }
                });
            }


        })

        function setwarrant(agentid, type) {
            $.ajax({
                type: "post",
                url: "/JsonFactory/AgentHandler.ashx?oper=setwarrant",
                data: { agentid: agentid, comid: $("#hid_comid").trimVal(), proid: $("#hid_proid").trimVal(), type: type },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        return;
                    }
                    if (data.type == 100) {
                        if (type == 0) {
                            $("#agent" + agentid).html("已经关闭授权");
                        } else {
                            $("#agent" + agentid).html("授权成功");
                        }
                    }
                }
            })
        }

    </script>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr style="background-color:#ffffff;border-width: 1px;border-style: solid;border-color: #A6A6A6 #CCC #CCC;">
                        <td>
                            <p align="center">
                                ${Agentid}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${Agentname}
                            </p>
                        </td>
                         <td>
                            <p align="left">
                                ${com_province}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                ${Warrant_type}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${Warrant_level}级</p>
                        </td>
                        <td>
                            <p align="left" id="agent${Agentid}">
                            {{if SetWarrant==0}}
                            <input name="setw" onclick="setwarrant('${Agentid}','1')" type="button" class="formButton" value="  特别授权销售  " />
                            {{else}}
                             <input name="setw" onclick="setwarrant('${Agentid}','0')" type="button" class="formButton" value="  取消授权  " />
                             {{/if}}
                            </p>
 
                        </td>
                    </tr>
    </script>
    <div id="bindingagent" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 760px; height: 450px; display: none; z-index: 10; left: 20%;
        top: 20%;" class="dialog">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="100%">
                    <div id="agentlist">
                        <table width="760" border="0">
                            <tr>
                                <td width="20" height="38">
                                    <p align="center">
                                        ID</p>
                                </td>
                                <td width="80">
                                    <p align="left">
                                        分销商公司名称
                                    </p>
                                </td>
                                <td width="40">
                                    <p align="left">
                                        所在地区
                                    </p>
                                </td>
                                <td width="25">
                                    <p align="left">
                                        类型
                                    </p>
                                </td>
                                <td width="25">
                                    <p align="left">
                                        级别
                                    </p>
                                </td>
                                <td width="40">
                                    <p align="left">
                                        &nbsp;</p>
                                </td>
                            </tr>
                            <tbody id="tblist">
                            </tbody>
                        </table>
                        <div id="divPage">
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input name="cancel" id="cancel" type="button" class="formButton" value="  关  闭  " />
                    <br />
                    注:在已授权的分销商进行特别授权可以销售，如果 产品显示设置 为分销可以销售时此设置无效。授权的分销价按指定授权的分销价格结算。如果为设定分销价此设置无效
                </td>
            </tr>
        </table>
    </div>
    <div id="div_articaltype" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 760px; height: 450px; display: none; z-index: 10; left: 20%;
        top: 20%;" class="dialog">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#E7F0FA"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="4" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="span1">产品组合管理
                    </span>
                    <input name="cancel12" type="button" class="formButton" onclick="progroupdivcancel();"
                        value="  关  闭  " style="float: right; padding-left:10px;" />
                </td>
            </tr>
            <tr>
                <td height="42" colspan="4">
                    <span>新增产品组合名称:</span><input type="text" id="text_progroup" value="" autocomplete="off"/><input type="button" id="button_addprogroup" onclick="addprogroup()" value="增加"/>
                </td>
            </tr>
            <tr>
                <td height="42">
                    <span style="font-size: 12px; font-weight: bold" id="span2">组合id
                    </span>
                </td>
                <td height="42">
                    <span style="font-size: 12px; font-weight: bold" id="span3">组合名称
                    </span>
                </td>
                <td height="42">
                    <span style="font-size: 12px; font-weight: bold" id="span4">组合状态
                    </span>
                </td>
                <td height="42">
                    <span style="font-size: 12px; font-weight: bold" id="span5">
                    </span>
                </td>
            </tr>
            <tbody id="tbody_progrouplist">
            </tbody>
            <script type="text/x-jquery-tmpl" id="Progrouptmpl">   
                    <tr>
                        <td class="sender item">
                            <p align="center">
                                ${id}</p>
                        </td>
                        <td  height="26" class="sender item">
                            <p align="left">
                               <span id="span1_${id}">
                                 ${groupname}
                               </span>
                                <input type="text"  value="${groupname}" id="tgroup_${id}" style="display:none;">
                            </p>
                        </td>
                        <td  height="26" class="sender item">
                            <p align="center">
                               <span id="span2_${id}">
                                    {{if runstate==0}}
                                     下线
                                    {{else}}
                                     在线
                                    {{/if}}
                               </span>
                   
                                <select id="sgroup_${id}" style="display:none;">
                                  <option value="1">在线</option>
                                  <option value="0">下线</option>
                                </select>
                            </p>
                        </td>
                        <td  height="26" class="sender item">
                            <input type="button" onclick="editprogroup(${id},this)" value="编辑"/>
                        </td> 
                    </tr>
            </script>
            <tr>
                <td height="38" colspan="4" align="center" bgcolor="#E7F0FA" class="tdHead">
                    <div id="divgroupPage">
                    </div>
                </td>
            </tr>
            
        </table>
    </div>

    <script type="text/javascript">
        var pageSize = 10; //每页显示条数

        $(function () {

            var comid = $("#hid_comid").trimVal();


            $("#selectwl").click(function () {
                WLSearchList(1);
                $("#WLview").show();
            })

            //装载WL产品列表
            function WLSearchList(pageindex) {
                var servertype = $("#sel_servertype").trimVal();
                var key = $("#key").trimVal();
                var pro_state = $("#pro_state").trimVal();

                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }

                $.post("/JsonFactory/ProductHandler.ashx?oper=wlpagelist", { comid: comid, pageindex: pageindex, pagesize: pageSize, pro_state: 2 }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询产品列表错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#WLtblist").empty();
                        $("#WLdivPage").empty();
                        if (data.totalCount == 0) {
                            //                                $("#tblist").html("查询数据为空");
                        } else {
                            $("#WLdeal").tmpl(data.msg).appendTo("#WLtblist");
                            setpage(data.totalCount, pageSize, pageindex);
                        }
                    }
                })
            }

            //分页
            function setpage(newcount, newpagesize, curpage) {
                $("#WLdivPage").paginate({
                    count: Math.ceil(newcount / newpagesize),
                    start: curpage,
                    display: 5,
                    border: false,
                    text_color: '#888',
                    background_color: '#EEE',
                    text_hover_color: 'black',
                    images: false,
                    rotate: false,
                    mouse: 'press',
                    onChange: function (page) {

                        WLSearchList(page);

                        return false;
                    }
                });
            }

        })
        function selectwldealid(wlid) {
            $("#WLview").hide();
            $("#txtservice_proid").val(wlid);

        }
        function closewldealid() {
            $("#WLview").hide();
          

        }
        
    </script>
    <div id="WLview" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 760px; height: 450px; display: none; z-index: 10; left: 20%;
        top: 20%;" class="dialog">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" 
            style="padding: 5px;" >
            <tr bgcolor="#999999">
                <td height="100%">
                    <div >
                        <table width="760" border="0">
                            <tr>
                                <td width="150" height="30">
                                    <p align="left">
                                        产品名称
                                    </p>
                                </td>
                                <td width="80">
                                    <p align="left">
                                        产品ID
                                    </p>
                                </td>
                                <td width="50">
                                    <p align="left">
                                       价格</p>
                                </td>
                                <td width="150">
                                    <p align="left">
                                       有效期</p>
                                </td>
                                <td width="50">
                                    <p align="right" onclick="closewldealid();">
                                        关闭</p>
                                </td>
                            </tr>
                            <tbody id="WLtblist">
                            </tbody>
                        </table>
                        <div id="WLdivPage">
                        </div>
                    </div>
                </td>
            </tr>
            
        </table>
    </div>
    <div class="wldata">
    </div>
    <script type="text/x-jquery-tmpl" id="WLdeal">   
                    <tr bgcolor="#ffffff">

                      
                        <td >
                            <p align="left" title="${title}">
                                ${title}
                            </p>
                        </td>
                           <td>
                            <p align="left">
                              
                               ${proID}
                            </p>
                        </td>
                         <td>
                            <p align="left">
                              
                               ${wlPrice}
                            </p>
                        </td>
                         <td>
                            <p align="left">
                              
                              ${voucherDateBegin}至${voucherDateEnd}
                            </p>
                        </td>
                         
                        <td >
                            <p align="left">
                                <input name="up" type="button" value="选择" style="width: 120px; height: 26px;" onclick="selectwldealid('${proID}')"  /></p>
                        </td>
                        
                    </tr>
    </script>

    <input type="hidden" id="hid_proid" value="<%=proid %>" />
    <input type="hidden" id="hid_industryid" value="<%=industryid %>" />
    <input type="hidden" id="hid_servicecontain" value="" />
    <input type="hidden" id="hid_servicenotcontain" value="" />
    <input type="hidden" id="hid_precautions" value="" />
    <input type="hidden" id="hid_imgurl" value="" />
    <input type="hidden" id="hid_prostate" value="" />
    <input type="hidden" id="hid_projectid" value="<%=projectid %>" />
    <!--产品的导入产品id；产品的已销售数量和可销售数量(如果是导入产品则查询原始产品的已销售数量和可销售数量)-->
    <input type="hidden" id="hid_bindingid" value="<%=bindingid %>" />
    <input type="hidden" id="hid_limitbuytotalnum" value="<%=limitbuytotalnum %>" />
    <input type="hidden" id="hid_buynum" value="<%=buynum %>" />
    <!--原产品公司编号-->
    <input type="hidden" id="hid_initprocomid" value="0" />
    <div id="loading" style="top: 150px; display: none;">
        <div class="lbk">
        </div>
        <div class="lcont">
            <img src="/Images/loading.gif" alt="loading..." />正在加载...</div>
    </div>
</asp:Content>
