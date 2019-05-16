<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="linetrip.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.travel.linetrip"
    MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="/Scripts/1ujqueryui/jquery.ui.core.js" type="text/javascript"></script>
    <script src="/Scripts/1ujqueryui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="/Scripts/1ujqueryui/minified/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="/Scripts/1ujqueryui/jquery.ui.accordion.js" type="text/javascript"></script>
    <script src="/Scripts/poshytip-1.1/jquery.poshytip.js" type="text/javascript"></script>
    <script src="/Scripts/JQPluse.js" type="text/javascript"></script>
    <script src="/Scripts/ETS.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script src="/Scripts/form2object.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.uploadify-v2.1.4/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.uploadify-v2.1.4/swfobject.js" type="text/javascript"></script>
 
    <style type="text/css">
        #divTourLineManageOutBox tr
        {
            line-height: 30px;
        }
        #divTourLineManageBox .sr, #divTourLineManageBox .btninput, #divTourLineManageBox .savebox
        {
            display: none;
        }
        .tourlinedesc
        {
            width: 620px;
        }
        .checkDestinationLi
        {
            float: left;
            padding: 0 10px;
        }
    
        .attention
        {
            background: none repeat scroll 0 0 #fef8ea;
            border: 1px solid #f0efed;
            color: #999;
            line-height: normal;
            margin-top: 3px;
            padding: 10px 0;
            text-align: center;
        }
        #mail-main ul li
        {
            display: block;
            list-style-type: none;
        }
        .pagelist ul li
        {
            border-bottom: 1px dashed #c9caca;
            margin: 0 auto;
            overflow: hidden;
            padding: 3px 0;
            width: 720px;
        }
        
        .tourLineBox
        {
            background-color: #fff;
        }
        .ui-accordion
        {
            width: 100%;
        }
        .ui-widget
        {
            font-family: Lucida Grande,Lucida Sans,Arial,sans-serif;
            font-size: 1.1em;
        }
        
        .tourLineBox .ui-state-active
        {
            color: #2779aa;
        }
        .ui-accordion .ui-accordion-header
        {
            cursor: pointer;
            margin-top: 1px;
            position: relative;
        }
        .ui-state-active, .ui-widget-content .ui-state-active, .ui-widget-header .ui-state-active
        {
            border-color: #c0e4f6 #c0e4f6 #fff;
            border-style: solid;
            border-width: 2px 1px 1px 0;
            color: #000;
            cursor: pointer;
        }
        .ui-state-active, .ui-widget-content .ui-state-active, .ui-widget-header .ui-state-active
        {
            background: url("/images/ui-bg_flat_50_3baae3_40x100.png") repeat-x scroll 50% 50% #3baae3;
            border: 1px solid #2694e8;
            font-weight: bold;
        }
        .ui-helper-reset
        {
            border: 0 none;
            font-size: 100%;
            line-height: 1.3;
            list-style: none outside none;
            margin: 0;
            outline: 0 none;
            padding: 0;
            text-decoration: none;
        }
        
        .ui-state-active, .ui-widget-content .ui-state-active, .ui-widget-header .ui-state-active
        {
            border-color: #c0e4f6 #c0e4f6 #fff;
            border-style: solid;
            border-width: 2px 1px 1px 0;
            color: #000;
            cursor: pointer;
        }
        .ui-state-default, .ui-widget-content .ui-state-default, .ui-widget-header .ui-state-default
        {
            color: #0071bc;
        }
        .ui-state-active, .ui-widget-content .ui-state-active, .ui-widget-header .ui-state-active
        {
            background: url("/images/ui-bg_flat_50_3baae3_40x100.png") repeat-x scroll 50% 50% #3baae3;
            border: 1px solid #2694e8;
            font-weight: bold;
        }
        .ui-state-default, .ui-widget-content .ui-state-default, .ui-widget-header .ui-state-default
        {
            background: url("/images/ui-bg_flat_80_d7ebf9_40x100.png") repeat-x scroll 50% 50% rgba(0, 0, 0, 0);
            border: 1px solid #aed0ea;
            color: #2779aa;
            font-weight: bold;
        }
        a:link, a:visited
        {
            color: black;
        }
        
        .a_anniu
        {
            height: 25px;
            line-height: 25px;
            border: 2px outset #eee;
            background: #ccc;
            text-align: center;
            font-size: 12px;
            color: #000;
            text-decoration: none;
            padding: 2px 15px;
            margin: 5px 0;
        }
    </style>
   
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="divTourLineManageOutBox" style="display: none; padding: 15px 15px;">
        <div class="title" style="padding: 15px 15px 15px 0; font-size: 15px;" id="secondary-tabs">
            <span><b><a href="/ui/pmui/ProductAdd.aspx?proid=<%=CurrentTourProductId%>">基本信息</a></b>
                <b><a href="/ui/pmui/travel/linetrip.aspx?lineid=<%=CurrentTourProductId%>">详细行程</a></b>
                <b><a href="/ui/pmui/travel/linegroupdate.aspx?lineid=<%=CurrentTourProductId%>">出团日期</a></b>
                <b><a href="/ui/pmui/productlist.aspx">产品列表</a></b> </span><span class="r"><font></font>
                    &nbsp;<em class="putdown"></em></span>
            <input type="hidden" id="hidlineid" value="<%=CurrentTourProductId%>" />
        </div>
        <div id="divTourLineManageBox" class="pagelist">
            <div>
                <div id="tourLineBox" class="tourLineBox">
                </div>
                <div style="text-align: right; padding-right: 15px;">
                    <a class="addQ" href="javascript:void(0);" onclick="addQ(this);" runat="server" id="addTour">
                        <span class="design-icon design-add"></span><span class="a_anniu" style="font-size: 13px;
                            font-weight: bold;">添加</span></a><div>
                            </div>
                </div>
                <script type="text/x-jquery-tmpl" id="tourLineContorlFoot">
<div class="spanRight" style="clear: none;  display:none;">
                <ul class="stuff">
                    <li style=" float:left; width:80px; border:none; font-size:13px; font-weight:bold;"><a  href="javascript:void(0);" onclick="Save(this);" runat="server" id="saveBtn"><span
                        class="design-icon design-save"></span><span class="a_anniu">保存</span></a></li><li style=" float:left; width:80px; border:none;"><a title="删除"  class="deleteQ" href="javascript:void(0);" onclick="deleteQ(this,${data.Id});">
                                <span class="design-icon design-delete"></span><span class="a_anniu" style=" font-size:13px; font-weight:bold;">删除</span></a></li> <div style="clear: both;">
                                                    </div>
                </ul>
            </div>
           
                </script>
                <script type="text/x-jquery-tmpl" id="tourLineContorl">
      <h3 style="line-height:3;"  >
       <a href="#">第${dataNum}天</a>&nbsp;&nbsp;&nbsp;${data.ActivityArea}</h3>
        <div class='accordion${dataNum}' >
            <div class="editeChangeBox" style="height: 40px;">
                <div class="attention editbase-tip" style="display: none">
                    <span class="cancleEditd" id="cancleeditd${dataNum}" onclick="cacle(this,${dataNum})" style="cursor: pointer;">取消编辑</span></div> 
                <div class="b-a b-a-U editbase-hook" style="-webkit-user-select: none; width: 100px; float: right; cursor: pointer; font-size:13px; font-weight:bold;"  id="savePageBtn${dataNum}" onclick="edite(this,${dataNum})"   >
                编辑行程信息</div>
            </div>
            <div class="tourFildBox" id="tourFildBox${dataNum}" >
                <input type="hidden"   value="${data.Id}" name="Id" class="tourId"/>
                <input type="hidden"   value="${dataNum}" name="numId" class="numId"/>
                <input type="hidden"   name="ProductId" value="${CurrentTourProductId}" />
                <ul>
                    <li><span class="sl">活动地区：</span> 
                        <span class="sm"  style=" width:300px;">
                            <input type="text" id="txtActivityArea${dataNum}" value="${data.ActivityArea}"  name="ActivityArea"   class="ActivityArea tip-hook txtinput input-hook" title="请填写活动区域">  
                        </span>  
                        <span class="sr" style=" width:300px;">点击选择活动地区,可多选.如有多个城市请用","隔开 </span>
                        </li> 
                    <li><span class="sl">交通信息：</span>
                        <span class="sm">
                            <input type="text" id="Traffic${dataNum}"   value="${data.Traffic}"  class="tip-hook txtinput input-hook Traffic"  title="请填写交通信息"/>
                        </span>
                        <span class="sr">可输入航班参考号或其他交通信息</span>
                    </li>
                    <li><span class="sl" style="padding-right: 10px;text-align: right; ">景点及活动：</span>
                        <span class="sm">
                           <input type="text" id="ScenicActivity${dataNum}"  value="${data.ScenicActivity}"  class="tip-hook txtinput input-hook ScenicActivity" maxlength="200"  title="请填主要景点及活动信息"/>
                        </span> 
                        <span class="sr"></span>
                    </li>
                    <li>
                        <span class="sl">行程描述：</span> 
                        <span   title="请填行程描述信息"> 
                            <textarea rows="3" cols="60" class="areabox input-hook " style="width: 420px; height: 80px"  id="textarea${dataNum}">${data.Description}</textarea>
                        </span>
                        <span class="sr"></span>
                    </li>
                    <li>
                        <span class="sl">住宿信息：</span> 
                        <span   style=" width:300px;">
                        <input type="text" id="Hotel${dataNum}"  value="${data.Hotel}"   class="tip-hook txtinput input-hook Hotel" maxlength="50" title="请填写住宿信息"/>
                         </span>
                         <span class="sr" style=" width:200px;">可输入当日住宿酒店信息等(必填)</span>
                    </li>
                    <li><span class="sl">用餐信息：</span>
                        <span class="eatingLableBox"></span> 
                        <span class="input-hook">
                  
                        <span class="DiningInforBox  DataBindCheck${dataNum}" style=" width:160px;">
                        <label> <input   type="checkbox" name="checkDining${dataNum}" value="早餐">早餐</label>
                        <label><input   type="checkbox" name="checkDining${dataNum}" value="中餐">中餐</label>
                        <label><input   type="checkbox" name="checkDining${dataNum}" value="晚餐">晚餐</label>

                        <input type="hidden"    id="Dining" class="Dining"/>
                        </span>  
                        </span>
                        </li>
                </ul>
            </div>
        </div>
                </script>
                <script type="text/javascript">
//    var selectedAreaId = 0, selectedFullname = '',hotel='';
    $(function () { 
        //第一次加载页面;标示出可以 添加新行程
        $("#hid_canadd").val(1);


        if(<%=CurrentTourProductId%>>0)
        { 
            $('#divTourLineManageOutBox').show();
            
            var ControlLoadInfor=<%=ControlLoadInfor %>;
            
            $("#hid_dataNum").val(ControlLoadInfor.length);
           
            for(var  i=0; i<ControlLoadInfor.length;i++)
            {  
                loadSubControl(ControlLoadInfor[i].Id,i+1);
            }        
            //第一次添加行程:标示出不可以 添加新行程;并且为编辑状态
            if(ControlLoadInfor.length==1&&ControlLoadInfor[0].Id==0)
            {
                $("#hid_canadd").val(0);

                edite($("#savePageBtn1"),1);
            }

        }
        $("#tourLineBox").accordion({
            collapsible: true,
            autoHeight: false,
            navigation: true
        });
    });

    function Save(object) {
        var _this = object; 
        var currentFormBox = $(_this).parents('.spanRight').prev('.tourFildBox');
        var tripid=currentFormBox.find('.tourId').val();
        var ProductId=currentFormBox.find('[name="ProductId"]').val();

        var ActivityArea=currentFormBox.find('.ActivityArea').val();
        if (ActivityArea == '') {
            alert('活动地区为必填项'); return  false;
        }
        var Traffic=currentFormBox.find('.Traffic').val();
        if (Traffic == '') {
            alert('交通信息为必填项'); return  false;
        }
        var ScenicActivity=currentFormBox.find('.ScenicActivity').val();
        if (ScenicActivity == '') {
            alert('景点及活动为必填项'); return  false;
        }
        var textarea=currentFormBox.find('.areabox').val();
        if (textarea == '') {
            alert('行程描述为必填项'); return  false;
        }
        var Hotel=currentFormBox.find('.Hotel').val();
        if (Hotel == '') {
            alert('住宿信息为必填项'); return  false;
        }

        var diningInfor = "";
        $(currentFormBox).find('.DiningInforBox').find(':checked').each(function () {
            if(diningInfor=='')
            {
                diningInfor=$(this).val();
            }else{
                diningInfor+=','+$(this).val();
            }
        });
        if(diningInfor=='')
        {
//            alert('用餐信息为必填项！'); return false;
        }  
         
        $.ajax({
                type: "POST",
                url: "/JsonFactory/ProductHandler.ashx?oper=Neweditlinetrip",
                data: { tripid: tripid, ProductId: ProductId,ActivityArea:ActivityArea,Traffic:Traffic,ScenicActivity:ScenicActivity,textarea:textarea,Hotel:Hotel,diningInfor:diningInfor, providerId : $('#hid_userid').val() },
                async: false,
                success: function (reData) {
                    reData=eval("("+reData+")");
                    if (reData.msg > 0) {
                       location.reload();
                    }

                }, error: function () {
                    $(_this).show().next('span').remove(); return false; 
                }

        });
      
    }

    function deleteQ(object,tripid) {
        if ($('#tourLineBox .deleteQ').length == 1) {
            alert('线路至少要有一天的行程');   return false;
        }
        if (!confirm('确定要删除该该行程吗？删除将不可恢复')) {
            return false;
        }
        $.ajax({
               type: "post",
               url: "/JsonFactory/ProductHandler.ashx?oper=deletelinetrip",
               data: "tripid="+tripid+"&ProductId="+$('#hidlineid').val(),
               async: false,
               success: function (reData) {
                    location.reload();
               }, error: function () {
               
               }

        });
    }

    function addQ(object) {
      
      if($("#hid_canadd").val()=='0'){
           alert("请先保存新增的行程");
           return false;
      } 
      $("#hid_canadd").val('0');
      
      var dataNum=$("#hid_dataNum").val();
      dataNum=parseInt(dataNum)+1;
      $("#hid_dataNum").val(dataNum);

      loadSubControl(0,dataNum);

      Sort(true);

      edite($("#savePageBtn"+dataNum),dataNum);
       /* $('#divTourLineManageBox').newFormView('show');
        loadSubControl(0);
        $('.btnAreaAdd').show();
        Sort(true);*/
    }
     function Sort(isAdd) {
        $("#tourLineBox").find('h3').each(function (i, n) {
            $(n).children('a').html('第' + (parseInt($("#tourLineBox").find('h3').index($(this))) + 1) + '天');
        });
        var active ;
        if(isAdd)
        { 
            active=$('#tourLineBox .tourFildBox').length-1;            
        }
        else{
            active = $('#tourLineBox').accordion('option', 'active');
        }
       
        $("#tourLineBox").accordion('destroy').accordion(
        {
            collapsible: true,
            autoHeight: false,
            navigation: true
        });
       
        $('#tourLineBox').accordion('option', 'active', active);
//            $('.spanRight').show();
          
    }
    /*
    (function ($) {
        $.widget('ets.registerTourLineManage', {
            options: {
            },
            _create: function () {
                this.form = this.element;
                var _this = this;
                $('.tip-hook', this.form).inputTip('none');
            }
        });
    })(jQuery);
    */
    /*
 

    function SelectArea(object) {

        if($('#hidDestination')!=undefined)
        { 
    
            var  result = '<ul>';
            var  arearList=$('#hidDestination').val().split(',');
            $(arearList).each(function(i,n){
                if($.trim(n)!='')
                    result = result + '<li class="checkDestinationLi"><input type=\'checkbox\' class=\'checkDestinationArea\' name=\'checkDestinationArea\' value=\''+ n + '\' />' + n + '</li>';
            });
            result+="</ul><div style='clear:both;'></div>";
                    
            $.prompt(result, {
                title:'添加活动地区',
                className: 'tip-yellowsimple',
                buttons: [{ title: '选择', value: true }, { title: '取消', value: false}],
                show: 'slideDown',
                submit: function()
                {
                    var selectDestination='';
                    $('.checkDestinationArea:checked').each(function(i,n){
                        if(selectDestination=='')
                        {
                            selectDestination+= $(n).val();
                        }
                        else{
                            selectDestination+= ","+$(n).val();
                        }
                    });
                    $(object).parents('.sm').find('.ActivityArea').val(selectDestination);
                }
            });
        } 
    }

    function SelectedArea(object) {
        var selectedvalue =$(object).parents('.sm').find('.ActivityArea').val();
        if (selectedAreaId > 0 && ("," + selectedvalue + ",").indexOf("," + selectedAreaId + ",") < 0) {
            if (selectedvalue != "") selectedvalue += ",";
            selectedvalue += selectedFullname + "";
            $(object).parents('.sm').find('.ActivityArea').val(selectedvalue);
            selectedFullname = '';
            selectedAreaId = 0;
        }
    }

    function removeArea(object,fullname) {
        var selectedvalue = $(object).parents('.sm').find('.ActivityArea').val();
        selectedvalue = ("," + selectedvalue + ",").replace("," + fullname + ",", ",");
        selectedvalue = selectedvalue.substr(1, selectedvalue.length - 2);
        $(object).parents('.sm').find('.ActivityArea').val(selectedvalue);
        $(object).parents('.divArea').remove();
        
    }
    */
   function loadSubControl(tripid,dataNum)
   {   
         $.ajax({
           type: "get",
           url: "/JsonFactory/ProductHandler.ashx?oper=GetLineTripById",
           data: "tripid="+tripid+"&ProductId="+$('#hidlineid').val(),
           async: false,
           success: function (reData) {
              reData=eval("("+reData+")");
              var wrapData={dataNum:dataNum,CurrentTourProductId:$('#hidlineid').val(), data:reData};
              
              $('#tourLineContorl').tmpl(wrapData).appendTo('#tourLineBox');
 
              $('#tourLineContorlFoot').tmpl(wrapData).appendTo('.accordion'+dataNum);
              if(reData.ActivityArea!=null&&reData.ActivityArea!=undefined)
              {
                  var areainfor= reData.ActivityArea.split(',');

                  var addArea='';
                  for(var i=0;i<areainfor.length;i++)
                  {
                        if(addArea=='')
                        {
                        addArea=areainfor[i];
                        }
                        else{
                        addArea+=','+areainfor[i];
                        }             
                  }
                  $('#txtActivityArea'+dataNum).val(addArea);
              }
            
              $('.DataBindCheck'+dataNum).find('input[type="checkbox"]').each(function(i,n){             
                  if(reData.Dining!=null&&(reData.Dining.indexOf($(n).val())>=0))
                  {
                      $(n).attr('checked','checked');
                  }
              });
        
              loadTiny('textarea'+dataNum);
              creatFormView(dataNum,tripid);
           }, error: function () {
               
           }
       });
      
   }
   function edite(object,dayNum)
   {
        $(object).hide();
        $(object).prev().slideDown(800);
     
     /*edit*/
      //活动地区
     $("#txtActivityArea"+dayNum).parent('span').show();
     $("#txtActivityArea"+dayNum).parent('span').next('span.input-show').hide();
   
     //交通信息
      $("#Traffic"+dayNum).parent('span').show();
      $("#Traffic"+dayNum).parent('span').next('span.input-show').hide();
  
     //景点及活动
    $("#ScenicActivity"+dayNum).parent('span').show();
    $("#ScenicActivity"+dayNum).parent('span').next('span.input-show').hide();
    
     //行程描述
     $("#textarea"+dayNum).parent('span').show();
      $("#textarea"+dayNum).parent('span').next('span.input-show').hide();
   
     
      //住宿信息
       $("#Hotel"+dayNum).parent('span').show();
       $("#Hotel"+dayNum).parent('span').next('span.input-show').hide();
 
   
        //用餐信息
        $('.DataBindCheck'+dayNum).show();
        $('.DataBindCheck'+dayNum).parent().prev('.eatingLableBox').hide();
        
//        $(object).parents('.accordion'+dataNum).newFormView().newFormView('edit');
        $(object).parents('.accordion'+dayNum).find(".spanRight").show();
    }
   function cacle(object,dayNum) {
       $(object).parent('div').slideUp('800');
       $(object).parent('div').next().show();
     /*show*/
     $("#txtActivityArea"+dayNum).parent('span').hide();
     $("#txtActivityArea"+dayNum).parent('span').next('span.input-show').show();
   
     //交通信息
     $("#Traffic"+dayNum).parent('span').hide();
     $("#Traffic"+dayNum).parent('span').next('span.input-show').show();
  
     //景点及活动
     $("#ScenicActivity"+dayNum).parent('span').hide();
     $("#ScenicActivity"+dayNum).parent('span').next('span.input-show').show();
    
     //行程描述
      $("#textarea"+dayNum).parent('span').hide();
     $("#textarea"+dayNum).parent('span').next('span.input-show').show();
   
     
      //住宿信息
     $("#Hotel"+dayNum).parent('span').hide();
     $("#Hotel"+dayNum).parent('span').next('span.input-show').show();
 
   
       //用餐信息
       $('.DataBindCheck'+dayNum).hide();
       $('.DataBindCheck'+dayNum).parent().prev('.eatingLableBox').show();

       $(object).parents('.accordion' + dayNum).find(".spanRight").hide();
    } 
     /*
    function edite(object,dataNum)
    {
         $(object).hide();
         $(object).prev().slideDown(800);
        $(object).parents('.accordion'+dataNum).newFormView().newFormView('edit');
         $(object).parents('.accordion'+dataNum).find(".sr,.btninput,.spanRight").show();
    }
   
    function cacle(object,dataNum) {
       $(object).parent('div').slideUp('800');
       $(object).parent('div').next().show();
       $(object).parents('.accordion' + dataNum).newFormView().newFormView('show');
       $(object).parents('.accordion' + dataNum).find(".sr,.btninput,.spanRight").hide();
    }
    */
   function creatFormView(dayNum,tripid)
   {
     /*init*/
     //活动地区
     var p1 = $("#txtActivityArea"+dayNum).parent('span');
     var show1 = p1.next('span.input-show');
     var v1 =   $("#txtActivityArea"+dayNum).val();
     v1 = v1 == '' ? '<span class="nodata">没有填写</span>' : v1;
     if (show1.length == 0) {
           show1 = $('<span class="input-show slongInfo">' + v1 + '</span>').insertAfter(p1);
     }
     p1.hide();
     show1.html(v1).show();
     //交通信息
     var p2=$("#Traffic"+dayNum).parent("span");
     var show2=p2.next('span.input-show');
     var v2=$("#Traffic"+dayNum).val();
     v2=v2==''?'<span class="nodata">没有填写</span>' : v2;
     if(show2.length==0){
        show2= $('<span class="input-show slongInfo">' + v2 + '</span>').insertAfter(p2);
     }
     p2.hide();
     show2.html(v2).show();
     //景点及活动
     var p3=$("#ScenicActivity"+dayNum).parent("span");
     var show3=p3.next('span.input-show');
     var v3=$("#ScenicActivity"+dayNum).val();
     v3=v3==''?'<span class="nodata">没有填写</span>' : v3;
     if(show3.length==0){
        show3= $('<span class="input-show slongInfo">' + v3 + '</span>').insertAfter(p3);
     }
     p3.hide();
     show3.html(v3).show();
     //行程描述
      var p4=$("#textarea"+dayNum).parent("span");
     var show4=p4.next('span.input-show');
     var v4=$("#textarea"+dayNum).val();
     v4=v4==''?'<span class="nodata">没有填写</span>' : v4;
     if(show4.length==0){
        show4= $('<span class="input-show slongInfo">' + v4 + '</span>').insertAfter(p4);
     }
     p4.hide();
     show4.html(v4).show();
      //住宿信息
      var p5=$("#Hotel"+dayNum).parent("span");
     var show5=p5.next('span.input-show');
     var v5=$("#Hotel"+dayNum).val();
     v5=v5==''?'<span class="nodata">没有填写</span>' : v5;
     if(show5.length==0){
        show5= $('<span class="input-show slongInfo">' + v5 + '</span>').insertAfter(p5);
     }
     p5.hide();
     show5.html(v5).show();
    //用餐信息
   var eatingInfor = '';
   $('.DataBindCheck'+dayNum).find(':checked').each(function(i,n){             
        if (eatingInfor == '') { 
            eatingInfor += $(n).val();
        }
        else {
            eatingInfor += "|" + $(n).val();
        }
   }); 
   $('.DataBindCheck'+dayNum).hide();
   $('.DataBindCheck'+dayNum).parent().prev('.eatingLableBox').text(eatingInfor).show();

      /*
        $('.accordion'+dayNum).newFormView({ btnPannel: '.spanRight',
            callback: {
                eatingInfor: {
                    init: function (o, v) {
                        var eatingInfor = '';
                        $(o).find(':checked').each(function (i, n) {
                            if (eatingInfor == '') {
                                eatingInfor += $(n).next('label').text();
                            }
                            else {
                                eatingInfor += "|" + $(n).next('label').text();
                            }
                        });
                        $(o).hide();
                        $(o).prev('.eatingLableBox').html(eatingInfor).show();
                    },
                    edit: function (o, v) {
                        $(o).show();
                        var valArray = $.trim($(o).prev('.eatingLableBox').text()).split("|");
                        $(o).find('input[type=checkbox]').each(function() {
                            var i = jQuery.inArray($(this).val(), valArray);
                            if(i>-1){
                                $(this).attr("checked","checked");
                            }else{
                                $(this).removeAttr("checked");
                            }
                        });
                        $(o).prev('.eatingLableBox').hide();
                    },
                    show: function (o, v) {
                        $(o).hide();
                        $(o).prev('.eatingLableBox').show();
                        return;

                    }
                },
                textArea: {
                    init: function (o, v) {
                        var description = $.trim($(o).find('.Description').val()) == '' ? '<span class="nodata">没有填写</span>' : $(o).find('.Description').val();
                        $(o).prev('.tourtextArea').html(description);
                        },
                    edit: function (o, v) {
                        $(o).show();
                        $(o).find('.Description').val($(o).prev('.tourtextArea').text());
                        $(o).prev('.tourtextArea').hide();
                    },
                    show: function (o, v) {
                        $(o).hide();
                        $(o).prev('.tourtextArea').show();
                        return;
                    }
                }, 
                checkBoxList: {
                    init: function (o, v) {
                        var description = '';
                        $(o).find(':checked').each(function (i, n) {
                            if (description == '') {
                                description += $(n).next('label').text();
                            }
                            else {
                                description += "-" + $(n).next('label').text();
                            }
                        });
                        if (description != '') {
                            description += '-' + $(o).find('input[type="text"]').val();
                        }
                        else {
                            description += $(o).find('input[type="text"]').val();
                        }
                        var descriptionResult = description == '' ? '<span class="nodata">没有填写</span>' : description;
                        $(o).prev('.checkBoxInfor').html(descriptionResult);
                    },
                    edit: function (o, v) {
                        $(o).show();
                        $('.checkBoxInfor').hide();
                    },
                    show: function (o, v) {
                        $(o).hide();
                        $(o).prev('.checkBoxInfor').show();
                        return;
                    }
                }
            }
        }).formView('show');
        if(tourLineId<=0)
        {
            $('.accordion'+dayNum).find('.editbase-hook').click();
        }
        */
 }

    function loadTiny(objectId)
    {
        $('#'+objectId).tinymce({
              // Location of TinyMCE script
                script_url: '/Scripts/tiny_mce/tiny_mce.js',
                width: '422',
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
        
    }
                </script>
            </div>
        </div>
    </div>
    <div style="padding: 15px 15px;">
        <a href="linegroupdate.aspx?lineid=<%=CurrentTourProductId %>" class="a_anniu">下一步，添加团期</a>
    </div>
    <!--行程天数-->
    <input type="hidden" id="hid_dataNum" value="0" />
    <!--是否可以添加新行程:0不可以;1可以 -->
    <input type="hidden" id="hid_canadd" value="1" />
</asp:Content>
