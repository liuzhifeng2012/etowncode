<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="WLProList.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.WLProList" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
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
    <script type="text/javascript">
        var pageSize = 20; //每页显示条数

        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1);


            $("#Search").click(function () {
                SearchList(1);
            })
            $("#upwlpro").click(function () {
                upwlprosub();
            })

            function upwlprosub() {

                $("#loading").show();
                $.post("/JsonFactory/ProductHandler.ashx?oper=wluppro", { comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#loading").hide();
                        $.prompt("更新出错");
                        return;
                    }
                    if (data.type == 100) {
                        $("#loading").hide();
                        $.prompt("更新完成");
                        return;
                    }
                })
            }



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

                $.post("/JsonFactory/ProductHandler.ashx?oper=wlpagelist", { comid: comid, pageindex: pageindex, pagesize: pageSize, key: key, pro_state: pro_state, userid: $("#hid_userid").trimVal() }, function (data) {
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
                    <input name="up" type="button" id="upwlpro" value="更新产品列表" style="width: 120px; height: 26px;" /></h3>
                <h4 style="float: right">
                    </h4>
                <div style="text-align: center;">
                    <label>
                        <select id="pro_state" class="mi-input" style="width:120px;">
                            <option value="0">全部</option>
                            <option value="2" selected>上线</option>
                            <option value="3">下线</option>
                        </select></label>
                    <label>
                        关键词查询
                        <input name="key" type="text" id="key" style="width: 160px; height: 20px;" placeholder="产品名称" />
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
                        <td width="120">
                            <p align="left">
                                产品ID
                            </p>
                        </td>
                        <td width="30">
                            <p align="left">
                                上线状态
                            </p>
                        </td>
                        <td width="80">
                            <p align="left">
                                上线日期
                            </p>
                        </td>
                        <td width="80">
                            <p align="left">
                                下线日期
                            </p>
                        </td>
                        <td width="35">
                            <p align="left">
                                门市价
                            </p>
                        </td>
                        <td width="35">
                            <p align="left">
                                销售价
                            </p>
                        </td>
                        <td width="35">
                            <p align="left">
                                结算价
                            </p>
                        </td>
                        <td width="35">
                            <p align="left">
                                库存数
                            </p>
                        </td>
                        <td width="80">
                            <p align="left">
                                下架时间
                            </p>
                        </td>
                        <td width="10">
                            <p align="left">
                               </p>
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
                                ${id}</p>
                        </td>
                      
                        <td  onclick="viewclass('${id}');" id="${Id}">
                            <p align="left" title="${title}">
                                ${title}<span id="class${id}">+</span>
                            </p>
                            <span  style="display:none;" id="viewclass${id}">包含：${include} <br>不含:${exclude}</span>
                        </td>
                        <td>
                        </td>

                           <td>
                            <p align="left">
                              
                               ${proID}
                            </p>
                        </td>
                        <td >
                            <p align="left">
                            {{if state == 1}}
                            上线
                            {{/if}}  
                            {{if state == 0}}
                                关闭
                            {{/if}}   
                           
                           </p>
                        </td>
                        <td >
                            <p align="left">
                                ${voucherDateBegin}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${voucherDateEnd}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${marketPrice}</p>
                        </td>
                        <td>
                            <p align="left" >
                                ${wlPrice}</p>
                        </td>
                        <td  >
                            <p align="left">
                                ${settlementPrice}    
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                ${stock}    
                            </p>
                        </td>
                        <td>
                            <p align="left">
                               ${scheduleOfflineTime} 
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

     <div id="loading" style="top: 150px; display: none;">
        <div class="lbk">
        </div>
        <div class="lcont">
            <img src="/Images/loading.gif" alt="loading..." />正在加载...</div>
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

