<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/Agent/Manage.Master"  CodeBehind="ShopCart.aspx.cs" Inherits="ETS2.WebApp.Agent.ShopCart" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var comid = $("#hid_comid_temp").trimVal();
            var projectid = $("#hid_projectid").trimVal();

            SearchList(1);


            //装载产品列表
            function SearchList(pageindex) {
                var key = $("#key").trimVal();
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }

                $.post("/JsonFactory/OrderHandler.ashx?oper=agentcartlist", { agentid: agentid, comid: comid }, function (data) {

                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询渠道列表错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalCount == 0) {
                            $("#tblist").html("<tr><td colspan='15'>查询数据为空</td></tr>");
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            //setpage(data.totalCount, pageSize, pageindex);
                        }


                    }
                })
            }

            $("#Search").click(function () {
                SearchList(1);
            })

            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    // $("#Search").click();    //这里添加要处理的逻辑  
                }
            });

            $("#0").click(function () {
                select(0, 1);
                $("#0").css("color", "red");
            })
            $("#1").click(function () {
                select(1, 1);
                $("#1").css("color", "red");
            })
            $("#3").click(function () {
                select(3, 1);
                $("#3").css("color", "red");
            })
            $("#4").click(function () {
                select(4, 1);
                $("#4").css("color", "red");
            })


            $("#Proinfocancel").click(function () {
                $("#ProInfo").hide();
            })
            $("#closeProInfo").click(function () {
                $("#ProInfo").hide();
            })

            $("#all").click(function () {

                if ($(this).attr("checked")) {
                    $("[name='Id']").attr("checked", 'true'); //全选
                }
                else {

                    $("[name='Id']").removeAttr("checked"); //取消全选
                }
            })

            $("#confirmButton").click(function () {
                var cartid = "";

                $("[name='Id']").each(function () {
                    if ($(this).attr("checked")) {
                        cartid += $(this).val() + ",";
                    }
                })

                if (cartid == "") {
                    alert("请选择结算的产品");
                    return;
                }


                location.href = "ShopCartSales.aspx?comid=" + comid + "&agentid=" + agentid + "&cartid=" + cartid;


            })
        })

        function addnum(proid, speciid, cartid, money) {
            var num = $("#u_num" + cartid).val();
            num++;
            $("#u_num" + cartid).val(num);
            addcart(proid,speciid,cartid);

            $("#sum" + cartid).html(num * money);
            
        }

        function reducenum(proid, speciid, cartid, money) {
            var num = $("#u_num" + cartid).val();
            num--;
            if (num == 0) {
                 if(confirm("是否产品移除购物车?")){

                     $.post("/JsonFactory/OrderHandler.ashx?oper=agentdelcart", { agentid: $("#hid_agentid").trimVal(), comid: $("#hid_comid_temp").trimVal(), cartid: cartid,proid:proid }, function (data) {
                         data = eval("(" + data + ")");
                         if (data.type == 1) {
                         }
                         if (data.type == 100) {
                             $("#prolist" + proid).remove();
                             return;
                         }
                     })
                 }
            } 

                if (num <= 0) {
                    num = 1
                }
                $("#u_num" + cartid).val(num);
                reducecart(proid, speciid, cartid, num);
                $("#sum" + cartid).html(num * money);

        }

        function rread(proid, speciid, cartid, money) {
            var num = $("#u_num" + cartid).val();
            if (num == 0) {
                if (confirm("是否产品移除购物车?")) {

                    $.post("/JsonFactory/OrderHandler.ashx?oper=agentdelcart", { agentid: $("#hid_agentid").trimVal(), comid: $("#hid_comid_temp").trimVal(), cartid: cartid, proid: proid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                        }
                        if (data.type == 100) {
                            $("#prolist" + proid).remove();
                            return;
                        }
                    })
                }
            }

            if (num <= 0) {
                num = 1
            }
            $("#u_num" + cartid).val(num);
            reducecart(proid, speciid, cartid, num);
            $("#sum" + cartid).html(num * money);

        }



        function addcart(proid,speciid, cartid) {
            $.post("/JsonFactory/OrderHandler.ashx?oper=agentaddcart", { agentid: $("#hid_agentid").trimVal(), comid: $("#hid_comid_temp").trimVal(), cartid: cartid, proid: proid, speciid: speciid, u_num: 1 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                }
                if (data.type == 100) {
                    //$("#cart").show();
                }
            })
        }



        function reducecart(proid, speciid, cartid, num) {
            $.post("/JsonFactory/OrderHandler.ashx?oper=agentreducecart", { agentid: $("#hid_agentid").trimVal(), comid: $("#hid_comid_temp").trimVal(), cartid: cartid, proid: proid, speciid: speciid, u_num: num }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                }
                if (data.type == 100) {
                    //$("#cart").show();
                }
            })
        }


        function sub() {
            $("[name='Id']").each(function () {
                if ($(this).attr("checked")) {
                }
                else {
                    $("#all").removeAttr("checked");
                }

            })
        }

    </script>
    
    <style type="text/css">
        #Service_Contain img 
        {
            max-width:400px;
}
        
    </style>
    <style type="text/css">
        .Bty
        {
            background-position: 0 -23px;
            color: #fff;
            font-weight: bold;
            text-shadow: 0 0 2px #D84803;
        }
        .Bt
        {
            display: inline-block;
            position: relative;
            z-index: 0;
            padding: 6px 12px;
            padding: 7px 12px\0;
            font-size: 12px;
            line-height: 14px;
            cursor: pointer;
        }
        .Bty
        {
            background-position: 0 -23px;
            color: #fff;
            font-weight: bold;
            text-shadow: 0 0 2px #D84803;
        }
        .Bt
        {
            display: inline-block;
            position: relative;
            z-index: 0;
            padding: 6px 12px;
            padding: 7px 12px\0;
            font-size: 12px;
            line-height: 14px;
            cursor: pointer;
        }
        .Bt, .Bt s
        {
            background-image: url(http://shop.etown.cn/images/bgAddressNew.png);
            background-repeat: no-repeat;
        }
        
        .btSubOrder1, .noOrderSubmit1
        {
            width: 150px;
            height: 45px;
            border: none;
            background: #ff4800 url(http://shop.etown.cn/images/bgRedBt.png) no-repeat;
            font-family: microsoft yahei;
            color: #fff;
            font-weight: bold;
            font-size: 22px;
            line-height: 35px;
            margin-right
        }
        .payTotal strong
        {
            display: block;
            padding-top: 10px;
            text-align: right;
            font-size: 14px;
        }
        

    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
                    <table>
                    <tr>
                        <td class="tdHead" style="font-size:14px; height:26px;">
                                               <div class="left">
                    <img id="comlogo" src="" class="" height="42"></div>
                    
                    <div class="left comleft">
                    <div ><span> 商户名称：
                    <%=company %> </span>
                    <span>授权类型：
                    <%=Warrant_type_str%>；</span> 
                     </div>
                      <div>
                      <%=yufukuan%>
                    <a class="a_anniu" href="Recharge.aspx?comid=<%=comid_temp %>" target="_blank">在线充值</a> <span id="messagenew"
                        style="padding-left: 30px;"></span>
                    </span>
                    </div>
                     </div>
                           </td>
                    </tr>

                </table>

        <div id="secondary-tabs" class="navsetting ">
        <ul class="composetab">
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_sel"><div><a href="ShopCart.aspx">购物车</a></div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div>
                <a href="Manage_sales.aspx?comid=<%=comid_temp%>">继续购买</a></div></div>
            </li>
         </ul>
         <div class="toolbg toolbgline toolheight nowrap" style="">
         <div class="right">
                   
         </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
         <!--<a class="btn_gray btn_space" hidefocus="" id="quick_del" href="javascript:;" name="del">删除</a>-->
         
         
         </div></div>

        </div>

        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">

                <table width="780" border="0">
                    <tr>                        
                    <td width="30">
                            <p align="left">
                                <label><input id="all" name="all" type="checkbox" checked="checked" /> 全选</label></p>
                        </td>
                        <td width="200">
                            <p align="left">
                                产品名称</p>
                        </td>
                        <td width="60">
                           有效期
                        </td>
                          <td width="80">
                           
                        </td>
                        <td width="55">
                            门市价(元)
                        </td>
                        <td width="55">
                            销售价(元)
                        </td>
                        <td width="55">
                            分销结算价(元)
                        </td>

                        <td width="60">
                            数量
                        </td>
                        <td width="100">
                        小计(元)
                        </td>                      
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                   
                </table>
                <table class="grid" width="100%">
                    <tbody><tr>
                        <td class="tdHead" style="text-align: right;">
                            <button id="confirmButton" name="confirmButton" type="button" class="btSubOrder1 noOrderSubmit1">
                                去结算</button>    
                        </td>
                    </tr>
                </tbody></table>
                <div id="divPage">
                </div>
            </div >
        </div>

    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">
    {{if Agent_price !=0}}
                <tr id="prolist${Id}">
                        <td >
                            <p align="left">
                              <input name="Id" value="${Cartid}" type="checkbox" checked="checked" click="sub();" /></p>
                        </td>

                        <td title="${Pro_name}">
                            <p align="left" >
                              <a href="javascript:;" onclick="Proadd('${Pro_name}','${Pro_Remark}','${Service_Contain}','${Service_NotContain}','${Precautions}')"> ${Pro_name}</a></p>
                        </td>
                        <td >
                        {{if Server_type ==11}}
                        有效期请查看实物
                        {{else}}
                        ${ChangeDateFormat(Pro_end)}
                        {{/if}}
                        </td>
                        <td >
                         {{if Server_type ==11}}
                         --
                         {{else}}
                        {{if (ProValidateMethod=="2")}}

                                   {{if (Appointdata == 1)}}
                                    出票一周有效
                                   {{/if}}
                                   
                                   {{if (Appointdata == 2)}}
                                   出票一月有效
                                   {{/if}}
                                   
                                   {{if (Appointdata == 3)}}
                                   出票三月有效
                                   {{/if}}
                                   
                                   {{if (Appointdata == 4)}}
                                   出票半年有效
                                   {{/if}}
                                   
                                   {{if (Appointdata == 5)}}
                                    出票一年有效
                                   {{/if}}
                        {{else}}
                        同产品有效期
                        {{/if}}
                        {{if (Iscanuseonsameday==0)}}
                        <br>出票当天不可用
                        {{/if}}
                        {{if (Iscanuseonsameday==2)}}
                        <br>出票2小时内不可用
                        {{/if}}
                        {{/if}}

                        </td>
                        <td >
                            <del>${Face_price}</del>
                        </td>
                        <td >
                            <p align="left">
                                <del>${Advise_price}</del></p>
                        </td>
                        <td >
                            <p align="left">
                                ${Agent_price}
                                </p>
                        </td>

                        <td >
                        <div class="wrap-input">
                             <a href="javascript:void(0);" class="btn-reduce" style=" margin-top:0;"  onclick="reducenum('${Id}','${Speciid}','${Cartid}','${Agent_price}')">减少数量</a>
                             <input id="u_num${Cartid}" name="u_num" value="${U_num}" class="input-ticket" onblur="rread('${Id}','${Speciid}','${Cartid}','${Agent_price}')" autocomplete="off" maxlength="4" size="4"  type="text">
                             <a href="javascript:void(0);" class="btn-add"  style=" margin-top:0;" onclick="addnum('${Id}','${Speciid}','${Cartid}','${Agent_price}')" >增加数量</a>
                        </div>
                        </td>
                        <td >
                           <span id="sum${Cartid}" name="sum" style="padding-left:10px;">${U_num*Agent_price} </span>

                        </td>
                    </tr>
                    {{/if}}
    </script>

    <div id="ProInfo" style="background-color:#ffffff;border:2px solid #5984bb;  margin-top:5px; margin:0px auto;width:550px; height:350px;display:none;left:20%; position:absolute; top:20%;overflow:auto;">
                <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999" style="padding:5px;">
                  <tr>
                    <td height="42" colspan="2" bgcolor="#C1D9F3" ><span class="STYLE1" >  &nbsp;&nbsp;&nbsp;&nbsp;产品介绍</span><span style=" float:right; font-size:18px; padding-right:10px; cursor:pointer;" id="closeProInfo">X</span></td>
                  </tr>
                  <tr>
                    <td width="80" height="30"  bgcolor="#E7F0FA" >&nbsp;&nbsp;&nbsp;&nbsp;产品名称: </td>
                    <td  height="30"  bgcolor="#E7F0FA"><span id="Pro_name"></span>
                      </td>
                  </tr>
                  <tr>
                    <td height="30"  bgcolor="#E7F0FA" class="tdHead">&nbsp;&nbsp;&nbsp;&nbsp;备注:</td>
                    <td height="30" bgcolor="#E7F0FA" class="tdHead"><span id="Pro_Remark"></span>
                    </td>
                  </tr>
                  <tr>
                    <td height="30"  bgcolor="#E7F0FA" > <span id="Span1"></span> </td>
                    <td height="30" bgcolor="#E7F0FA"><span id="Service_Contain"></span></td>
                  </tr>
                  <tr>
                    <td height="30"  bgcolor="#E7F0FA" class="tdHead"></td>
                    <td height="30" bgcolor="#E7F0FA" class="tdHead"><span id="Service_NotContain"></span></td>
                  </tr>
                  <tr>
                    <td height="30"  bgcolor="#E7F0FA" class="tdHead"></td>
                    <td height="30" bgcolor="#E7F0FA" class="tdHead"><span id="Precautions"></span></td>
                  </tr>
                  <tr>
                    <td height="30" colspan="2" align="center" bgcolor="#E7F0FA" class="tdHead">
                      <input name="Proinfocancel" id="Proinfocancel" type="button" class="formButton" value="  关  闭  " /></td>
                  </tr>
                </table>
			</div>	
            <div id='cart' style=" display:none;position: absolute; bottom: 6em; right: 2em; width: 55px; height:55px; background-color: #FFFAFA; margin:10px; border-radius:60px; border: solid rgb(55,55,55)  #FF0000   1px;cursor:pointer;"><img src="/images/cart.gif" width="39" style="padding:8px 0 0 9px;"/></div>
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
    <input id="hid_projectid" type="hidden" value="<%=projectid %>" />
</asp:Content>
