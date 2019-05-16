<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ProductList.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.ProductList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 20; //每页显示条数

        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1);

            $.post("/JsonFactory/ProductHandler.ashx?oper=getservertypelist", {}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $("#sel_servertype").html('<option value="0">全部</option>');
                }
                if (data.type == 100) {
                    var optionstr = '<option value="0">全部</option>';
                    for (var i = 0; i < data.msg.length; i++) {
                        optionstr += '<option value="' + data.msg[i].ID + '">' + data.msg[i].Value + '</option>'
                    }
                    $("#sel_servertype").html(optionstr);
                }
            })

            $("#Search").click(function () {
                SearchList(1);
            })
            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#Search").click();    //这里添加要处理的逻辑  
                    return false;
                }
            });

            //装载产品列表
            function SearchList(pageindex) {
                var servertype = $("#sel_servertype").trimVal();
                var key = $("#key").trimVal();
                var pro_state = $("#pro_state").trimVal();

                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }

                $.post("/JsonFactory/ProductHandler.ashx?oper=pagelist", {servertype:servertype, projectid: $("#hid_projectid").trimVal(), comid: comid, pageindex: pageindex, pagesize: pageSize, key: key, pro_state: pro_state, userid: $("#hid_userid").trimVal() }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询产品列表错误");
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
                })
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



            //调整产品状态：上线；下线
            $("#cancel_rh").click(function () {

                $("#rhshow").hide();
            })



            $("#submit_rh").click(function () {

                var proid = $("#hid_proid").trimVal();
                if (proid == "") {
                    $.prompt("操作出现错误");
                    return;
                }

                var pro_state = $('input:radio[name="radchannelsource"]:checked').trimVal(); //  0 下线，   1 上线

                $.post("/JsonFactory/ProductHandler.ashx?oper=modifyprostate", { prostate: pro_state, proid: proid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("操作成功", {
                            buttons: [{ title: '确定', value: true}],
                            opacity: 0.1,
                            focus: 0,
                            show: 'slideDown',
                            submit: callbackfunc
                        });
                    }
                })
                function callbackfunc(e, v, m, f) {
                    if (v == true)
                    //window.location.reload();
                        $("#rhshow").hide();
                }

            })


            //录入，关闭录入电子票窗口
            $("#cancel_luru").click(function () {
                $("#lurueticket").hide();
            })




            //录入电子码提交
            $("#submit_luru").click(function () {

                var proid = $("#hid_luru_proid").trimVal();
                if (proid == "") {
                    $.prompt("操作出现错误");
                    return;
                }

                var eticketkey = $("#eticketkey").trimVal(); //  电子码
                var leixing = $("#leixing").trimVal(); //  电子码

                if (eticketkey == "") {
                    $.prompt("请填写电子码");
                    return;
                }
                if (leixing == "") {
                    $.prompt("传递参数错误，请刷新重新提交");
                    return;
                }
                $.post("/JsonFactory/ProductHandler.ashx?oper=lurueticket", { key: eticketkey, leixing: leixing, proid: proid, comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#eticketkey").val("");
                        $("#hid_luru_proid").val("")
                        $("#lurueticket").hide();

                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#eticketkey").val("");
                        $("#hid_luru_proid").val("")
                        $("#lurueticket").hide();

                        $.prompt("操作成功" + data.msg, {
                            buttons: [{ title: '确定', value: true}],
                            opacity: 0.1,
                            focus: 0,
                            show: 'slideDown',
                            submit: callbackfunc
                        });
                    }
                })
                function callbackfunc(e, v, m, f) {
                    if (v == true)
                        window.location.reload();
                }

            })


        })


        function viewclass(id) {
            if ($("#class" + id).html() == "+") {
                $("#viewclass" + id).show();
                $("#class" + id).html("-");
            } else {
                $("#viewclass" + id).hide();
                $("#class" + id).html("+");
            }
        };

        //弹出产品状态修改兰
        function referrer_ch(proid, state) {
            $("#hid_proid").val(proid);
            $("#span_rh").text("产品状态调整:");
            $("#rhshow").show();
            $("input[name='radchannelsource'][value='" + state + "']").attr("checked", true);
        };

        //弹出录入电子票兰
        function referrer_luru(proid, proname) {
            $("#hid_luru_proid").val(proid);
            $("#lurutitle").text(proname + "-录入电子票:");
            $("#submit_luru").val("确定录入电子票");
            $("#leixing").val("1");
            $("#lurueticket").show();
        };


        //弹出录入电子票兰
        function referrer_zuofei(proid, proname) {
            $("#hid_luru_proid").val(proid);
            $("#lurutitle").text(proname + "-作废电子票:");
            $("#submit_luru").val("确定，作废");
            $("#leixing").val("0");
            $("#lurueticket").show();
        };
        
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/pmui/projectlist.aspx" onfocus="this.blur()" target=""><span>项目管理</span></a></li>
                <li><a href="/ui/pmui/Projectedit.aspx" onfocus="this.blur()" target=""><span>添加项目</span></a></li>
                <li class="on"><a href="/ui/pmui/ProductList.aspx?projectid=<%=projectid %>" onfocus="this.blur()"
                    target=""><span>产品列表</span></a></li>
                <li><a href="/ui/pmui/ProductServerTypeList.aspx?projectid=<%=projectid %>" onfocus="this.blur()"
                    target=""><span>添加产品</span></a></li>
                <li><a href="/ui/pmui/order/Salecount.aspx" onfocus="this.blur()" target="">产品统计</a></li>
                <li><a href="/ui/pmui/BindingAgent.aspx" onfocus="this.blur()" target="">导入分销系统产品</a></li>
                <li><a href="/ui/pmui/eticket_useset.aspx" onfocus="this.blur()" target=""><span>商户特定日期设定</span></a></li>
                <li><a href="/ui/pmui/delivery/deliverylist.aspx" onfocus="this.blur()" target="">
                    <span>运费模版管理</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    产品列表</h3>
                <h4 style="float: right">
                    <a style="" href="ProductServerTypeList.aspx?projectid=<%=projectid %>" class="a_anniu">
                        添加新产品</a> <a href="ProductSort.aspx" class="a_anniu">点击对产品排序</a></h4>
                <div style="text-align: center;">
                    <label>
                        <select id="pro_state" class="mi-input" style="width:120px;">
                            <option value="0">全部</option>
                            <option value="2" selected>上线</option>
                            <option value="3">下线</option>
                        </select></label>
                    <label>
                        <select id="sel_servertype" class="mi-input" style="width:120px;">
                            
                        </select>
                    </label>
                    <label>
                        关键词查询
                        <input name="key" type="text" id="key" style="width: 160px; height: 20px;" placeholder="产品编号、产品名称" />
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
                    </label>
                </div>
                <table width="780" border="0">
                    <tr>
                        <td width="30">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="150">
                            <p align="left">
                                产品名称
                            </p>
                        </td>
                        <td width="3">
                            <p align="left">
                            </p>
                        </td>
                        <td width="35">
                            <p align="left">
                                类型
                            </p>
                        </td>
                        <td width="20">
                            <p align="left">
                                显示
                            </p>
                        </td>
                        <td width="30">
                            <p align="left">
                                库存数
                            </p>
                        </td>
                        <td width="30">
                            <p align="left">
                                门市价
                            </p>
                        </td>
                        <td width="30">
                            <p align="left">
                                建议价
                            </p>
                        </td>
                        <td width="30">
                            <p align="left">
                                结算价
                            </p>
                        </td>
                        <td width="60">
                            <p align="left">
                                产品有效期
                            </p>
                        </td>
                        <td width="35">
                            <p align="left">
                                票来源
                            </p>
                        </td>
                        <td width="40">
                            <p align="left">
                                状态
                            </p>
                        </td>
                        <td width="240">
                            <p align="left">
                                管理 &nbsp;</p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <td>
                            <p align="left">
                                ${Id}</p>
                        </td>
                      
                        <td  onclick="viewclass('${Id}');" id="${Id}">
                            <p align="left" title="${Pro_name}">
                                ${Pro_name}<span id="class${Id}">+</span>
                            </p>
                            <span  style="display:none;" id="viewclass${Id}">分类：${ProClassName} 项目:${Projectname}</span>
                        </td>
                        <td>
                        </td>
                           <td>
                            <p align="left">
                               {{if Servertype==1}}
                                 票务
                                 {{else}}
                                   {{if  Servertype==2}}
                                    跟团
                                   
                                   {{/if}}
                                   {{if  Servertype==8}}
                                    当地游
                                  
                                   {{/if}}
                                   {{if Servertype==9}}
                                   客房
                                   {{/if}}

                                    {{if Servertype==10}}
                                   大巴
                                   {{/if}}

                                      {{if Servertype==11}}
                                   实物
                                   {{/if}}
                                    {{if Servertype==12}}
                                   预订产品
                                   {{/if}}
                                    {{if Servertype==13}}
                                   教练预约
                                   {{/if}}
                                    {{if Servertype==14}}
                                   保险产品
                                   {{/if}}
                                   {{if Servertype==15}}
                                   套票
                                   {{/if}}
                               {{/if}}
                               
                            </p>
                        </td>
                        
                           <td>
                            <p align="left">
                               {{if Viewmethod==1}}
                                 全部
                               {{/if}}
                                   {{if  Viewmethod==2}}
                                    分销
                                   
                                   {{/if}}
                                   {{if  Viewmethod==3}}
                                    微信
                                  
                                   {{/if}}
                                   {{if Viewmethod==4}}
                                   官网
                                   {{/if}}

                                    {{if Viewmethod==5}}
                                   官+微
                                   {{/if}}
                               
                               
                            </p>
                        </td>
                        <td >
                            <p align="left">
                          {{if Ispanicbuy==0}}
                            --
                          {{else}}
                             ${StockNum}
                          {{/if}}
                           </p>
                        </td>
                        <td >
                            <p align="left">
                                ${Face_price}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Advise_price}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Agentsettle_price}</p>
                        </td>
                        <td>
                            <p align="left" title="${ChangeDateFormat(Pro_end)}">
                                ${ChangeDateFormat(Pro_end)}</p>
                        </td>
                        <td  title="${ProComeCom}">
                            <p align="left">
                                ${Source_type}    
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                {{if Pro_state=="上线"}}
                                <a href ="javascript:;" onclick="referrer_ch('${Id}','1')" class="a_anniu">${Pro_state}</a>   
                                {{else}} 
                                <a href ="javascript:;" onclick="referrer_ch('${Id}','0')" class="a_anniu">${Pro_state}</a>   
                                {{/if}}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                               {{if Source_type=="倒码"}}<a href="ProductStocketicket.aspx?proid=${Id}&statetype=0" style="text-decoration: underline;">总</a>:${Count_Num} <a href="ProductStocketicket.aspx?proid=${Id}&statetype=2" style="text-decoration: underline;">已</a>:${Use_Num} <a href="ProductStocketicket.aspx?proid=${Id}&statetype=1" style="text-decoration: underline;">未</a>:${UnUse_Num} <a href="ProductStocketicket.aspx?proid=${Id}&statetype=3" style="text-decoration: underline;">停</a>:${Invalid_Num}<input  type="button" onclick="referrer_luru('${Id}','${Pro_name}')" class="formButton" value="  录入码  " /><input  type="button" onclick="referrer_zuofei('${Id}','${Pro_name}')" class="formButton" value=" 停用码 " /> {{else}} {{/if}} 

                                <a href="ProductAdd.aspx?proid=${Id}&projectid=<%=projectid %>" class="a_anniu">编辑基本</a>   
                                
                                {{if Source_type=="自动生成" &&  Servertype==1}}
                                (验证数量:${Use_pnum})
                                {{/if}}
                                {{if Source_type=="分销导入" &&  Servertype==1}}
                                (验证数量:${Use_pnum})
                                {{/if}}

                                {{if Servertype==15}}
                                    <a href="/ui/pmui/PackageBindingpro.aspx?id=${Id}" class="a_anniu">设定绑定产品</a> 
                                {{/if}}

                                {{if  Servertype==2||Servertype==8}}
                                       <a href="/ui/pmui/travel/linetrip.aspx?lineid=${Id}" class="a_anniu">编辑行程</a>      
                                       <a href="/ui/pmui/travel/linegroupdate.aspx?lineid=${Id}" class="a_anniu">编辑团期</a>   
                                   
                                {{/if}}
                                {{if  Servertype==9}}
                                     {{if Source_type != "分销导入"}}
                                    <a href="/ui/pmui/travel/linegroupdate.aspx?lineid=${Id}" class="a_anniu">编辑房态</a>   
                                   {{/if}}
                                {{/if}}
                                {{if  Servertype==10}}
                                    {{if Source_type != "分销导入"}}
                                    <a href="/ui/pmui/travel/linegroupdate.aspx?lineid=${Id}" class="a_anniu">编辑班车日期</a>
                                    {{/if}}   
                                {{/if}}

                              <span id="span1" style="color:blue;cursor: pointer;"   onclick="referrer_ch1('${Id}',150)">二维码</span>   
                            
                               </p>
                        </td>
                    </tr>
    </script>
    <div id="rhshow" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 400px; height: 130px; display: none; left: 20%; position: absolute; top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 18px;" id="span_rh"></span>
                </td>
            </tr>
            <tr>
                <td height="30" align="right" bgcolor="#E7F0FA" class="tdHead">
                    产品状态:
                </td>
                <td height="30" bgcolor="#E7F0FA" class="tdHead" id="tdgroups">
                    <label>
                        <input type="radio" value="1" name="radchannelsource">上 线</label>
                    <label>
                        <input type="radio" value="0" name="radchannelsource">下 线</label>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input type="hidden" id="hid_proid" value="" />
                    <input type="hidden" id="leixing" value="" />
                    <input name="submit_rh" id="submit_rh" type="button" class="formButton" value="  确  定  " />
                    <input name="cancel_rh" id="cancel_rh" type="button" class="formButton" value="  取  消  " />
                </td>
            </tr>
        </table>
    </div>
    <div id="lurueticket" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 400px; height: 300px; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 18px;" id="lurutitle"></span>
                </td>
            </tr>
            <tr>
                <td height="30" align="right" bgcolor="#E7F0FA" class="tdHead">
                    录入电子码:
                </td>
                <td height="30" bgcolor="#E7F0FA" class="tdHead" id="td1">
                    <textarea name="eticketkey" id="eticketkey" cols="40" rows="10"></textarea><br>
                    电子码默认每个电子码 1张票，录入时每个电子码一行
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input type="hidden" id="hid_luru_proid" value="" />
                    <input name="submit_luru" id="submit_luru" type="button" class="formButton" value="  确定录入电子码  " />
                    <input name="cancel_luru" id="cancel_luru" type="button" class="formButton" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <input type="hidden" id="hid_projectid" value="<%=projectid %>" />
    <div id="proqrcode_rhshow" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; display: none; left: 20%; position: absolute; top: 20%;">
        <input type="hidden" id="hid_proid" value="" />
        <table width="500px" border="0" cellpadding="10" cellspacing="1" style="margin: 10px 5px;">
            <tr>
                <td align="center">
                    <span style="font-size: 14px;">产品微信二维码</span>
                </td>
                <td align="center">
                    <span style="font-size: 14px;">产品预订二维码</span>
                </td> 
            </tr>
            <tr>
                <td align="center">
                    <img src="/Images/defaultThumb.png" id="img1" height="150" width="150" />
                </td>
                <td align="center">
                    <img src="/Images/defaultThumb.png" id="img2" height="135" width="135" />
                </td> 
            </tr>
            <tr>
                <td colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <label>
                        *二维码尺寸请按照43像素的整数倍缩放，以保持最佳效果</label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input name="cancel_rh" id="closebtn" type="button" class="formButton" value="  关 闭  " />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">

        //弹出二维码大图
        function referrer_ch1(proid, pixsize) {
            $("#hid_proid").val(proid);
            referrer_ch2(pixsize, 1);
            $("#proqrcode_rhshow").show();
        };
        //弹出二维码大图
        function referrer_ch2(pixsize, qrcodetype) {

            var proid = $("#hid_proid").trimVal();

            var comid = $("#hid_comid").trimVal();
            //              if (qrcodetype == 1)
            //              {
            //微信公众平台生成的产品带参二维码
            //                  $("#li_d1").css("color", "#56aeff");
            //                  $("#li_d2").css("color", "#888888");

            $("#img1").attr("src", "/Images/defaultThumb.png")

            $.post("/JsonFactory/WeiXinHandler.ashx?oper=editwxqrcode", { productid: proid, onlinestatus: 1, channelid: 0, qrcodeid: 0, comid: $("#hid_comid").trimVal(), qrcodename: "系统生成产品id:" + proid, promoteact: 0, promotechannelcompany: 0 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    //alert(data.msg);
                    return;
                }
                if (data.type == 100) {
                    $("#img1").attr("src", data.qrcodeurl);
                }
            })
            //              } else {
            //自定义方法生成的预订网址二维码
            //                  $("#li_d2").css("color", "#56aeff");
            //                  $("#li_d1").css("color", "#888888");


            $("#img2").attr("src", "/Images/defaultThumb.png")

            var url = "http://shop" + comid + ".etown.cn/h5/order/pro.aspx?id=" + proid;
            $("#img2").attr("src", "/ui/pmui/eticket/showtcode.aspx?pno=" + url);

            
//            $("#img3").attr("src", "/Images/defaultThumb.png")
//            $.post("/JsonFactory/WeiXinHandler.ashx?oper=getNativePayQrcode", { proid: proid, comid: $("#hid_comid").trimVal(),paramtype:"pid"}, function (data) {
//                data = eval("(" + data + ")");
//                if (data.type == 1) {return;}
//                if (data.type == 100) {
//                    $("#img3").attr("src", "/ui/pmui/eticket/showtcode.aspx?pno=" +data.qrcodeurl);
//                }
//            })

            
            //              }

        };
        $(function () {
            $("#closebtn").click(function () {
                $("#img1").attr("src", "/Images/defaultThumb.png")
                $("#img2").attr("src", "/Images/defaultThumb.png")
                $("#proqrcode_rhshow").hide();
                $("#hid_proid").val("0");
            })
        })
    </script>
    <style type="text/css">
        .indexnav
        {
            background: none repeat scroll 0% 0% #F0F0F0;
            display: table;
            width: 100%;
        }
        .indexnav li
        {
            text-align: center;
            cursor: pointer;
            font-size: 14px;
            display: table-cell;
            display: inline;
        }
        
        
        .indexnav li span
        {
            background-color: #FFF;
            text-align: center;
        }
    </style>
</asp:Content>
