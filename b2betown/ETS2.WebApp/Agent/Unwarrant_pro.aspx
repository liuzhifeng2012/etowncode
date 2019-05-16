<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/Agent/Manage.Master" CodeBehind="Unwarrant_pro.aspx.cs" Inherits="ETS2.WebApp.Agent.Unwarrant_pro" %>

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

                $.post("/JsonFactory/AgentHandler.ashx?oper=unwarrantprolist", { pageindex: pageindex, pagesize: pageSize, agentid: agentid, comid: comid, key: key, projectid: projectid, viewmethod: "1,2" }, function (data) {

                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //$.prompt("查询渠道列表错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalCount == 0) {
                            $("#tblist").html("<tr><td colspan='15'>查询数据为空</td></tr>");
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalCount, pageSize, pageindex);
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

            //分页 
            function setpage1(newcount, newpagesize, curpage, opval) {
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

                        select(opval, page);

                        return false;
                    }
                });
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
            $("#Proinfocancel").click(function () {
                $("#ProInfo").hide();
            })
            $("#closeProInfo").click(function () {
                $("#ProInfo").hide();
            })



        })



        function Proadd(Pro_name, Pro_Remark, Service_Contain, Service_NotContain, Precautions) {
            $("#ProInfo").show();
            $("#Pro_name").html(Pro_name);
            $("#Pro_Remark").html(Pro_Remark);
            $("#Service_Contain").html(Service_Contain);
            $("#Service_NotContain").html(Service_NotContain);
            $("#Precautions").html(Precautions);

        }


        function addcart(proid) {
            alert("添加到购物车");
            $.post("/JsonFactory/OrderHandler.ashx?oper=agentaddcart", { agentid: $("#hid_agentid").trimVal(), comid: $("#hid_comid_temp").trimVal(), proid: proid, u_num: 1 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                }
                if (data.type == 100) {
                    $("#cart").show();
                    searchcart();
                }
            })
        }
        

    </script>
    <style type="text/css">
        #Service_Contain img
        {
            max-width: 400px;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <table>
            <tr>
                <td class="tdHead" style="font-size: 14px; height: 26px;">
                  <div class="left"><img id="comlogo" src="" class="" height="42"></div>
                    
                    <div class="left comleft">
                    <div ><span> 商户名称：
                    <%=company %> </span>
                      <a class="a_anniu" href="javascript:;"  onclick="history.go(-1);" > <<<返回 </a>
                    </div>
                     </div>
                </td>
            </tr>
        </table>
        <div id="secondary-tabs" class="navsetting ">
             <ul class="composetab">
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="Unwarrant_Supplier.aspx">更多供应商</a></div></div>
            </li>
                        <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_sel"><div><a href="Unwarrant_pro.aspx?comid=<%=comid_temp %>">供应商商品</a></div></div>
            </li>
         </ul>
          <div class="toolbg toolbgline toolheight nowrap" style="">
         <div class="right">
                    <label>
                        关键词查询
                        <input name="key" type="text" id="key" style="width: 160px; height: 20px;" />
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
                    </label>   
         </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
         <!--<a class="btn_gray btn_space" hidefocus="" id="quick_del" href="javascript:;" name="del">删除</a>-->
         
         </div></div>

        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <table width="780" border="0" class="O2">
                    <tr  class="O2title">
                        <td width="26">
                            <p align="left">
                                序号</p>
                        </td>
                        <td width="100">
                            <p align="left">
                                项目</p>
                        </td>

                        <td width="280">
                            <p align="left">
                                产品名称</p>
                        </td>

                        <td width="35">
                            <p align="left">
                                库存</p>
                        </td>
                        <td width="60">
                            产品有效期
                        </td>
                        <td width="80">
                            使用有效期
                        </td>
                        <td width="35">
                            门市价
                        </td>
                        <td width="35">
                            销售价
                        </td>
                        <td width="50">
                            分销结算价
                        </td>
                        <td width="120">
                        </td>
                        <td width="50">
                            <p align="left">
                                备注</p>
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
                        <td >
                            <p align="left">
                               ${Id}</p>
                        </td>
                        <td title="${Projectname}">
                            <p align="left">
                               ${Projectname}</p>
                        </td>

                        <td title="${Pro_name}">
                            <p align="left" >
                              <a href="javascript:;" onclick="Proadd('${Pro_name}','${Pro_Remark}','${Service_Contain}','${Service_NotContain}','${Precautions}')"> ${Pro_name}   {{if IsViewStockNum==1}}
                              
                            {{/if}}</a></p>
                        </td>
                          <td >
                            <p align="left">${StockNum}
                              </p>
                        </td>
                        <td >${ChangeDateFormat(Pro_end)}
                        </td>
                        <td >
                        
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


                        </td>
                        <td >
                            ${Face_price}
                        </td>
                        <td >
                            <p align="left">
                                ${Advise_price}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Agent_price}</p>
                        </td>

                        <td > 

                        </td>
                        <td  title="${Pro_explain}">
                            <p align="left">
                               </p>
                        </td>
                    </tr>
               
                    
    </script>
    <div id="ProInfo" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 550px; height: 350px; display: none; left: 20%; position: absolute; top: 20%;
        overflow: auto;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span class="STYLE1">&nbsp;&nbsp;&nbsp;&nbsp;产品介绍</span><span style="float: right;
                        font-size: 18px; padding-right: 10px; cursor: pointer;" id="closeProInfo">X</span>
                </td>
            </tr>
            <tr>
                <td width="80" height="30" bgcolor="#E7F0FA">
                    &nbsp;&nbsp;&nbsp;&nbsp;产品名称:
                </td>
                <td height="30" bgcolor="#E7F0FA">
                    <span id="Pro_name"></span>
                </td>
            </tr>
            <tr>
                <td height="30" bgcolor="#E7F0FA" class="tdHead">
                    &nbsp;&nbsp;&nbsp;&nbsp;备注:
                </td>
                <td height="30" bgcolor="#E7F0FA" class="tdHead">
                    <span id="Pro_Remark"></span>
                </td>
            </tr>
            <tr>
                <td height="30" bgcolor="#E7F0FA">
                    <span id="Span1"></span>
                </td>
                <td height="30" bgcolor="#E7F0FA">
                    <span id="Service_Contain"></span>
                </td>
            </tr>
            <tr>
                <td height="30" bgcolor="#E7F0FA" class="tdHead">
                </td>
                <td height="30" bgcolor="#E7F0FA" class="tdHead">
                    <span id="Service_NotContain"></span>
                </td>
            </tr>
            <tr>
                <td height="30" bgcolor="#E7F0FA" class="tdHead">
                </td>
                <td height="30" bgcolor="#E7F0FA" class="tdHead">
                    <span id="Precautions"></span>
                </td>
            </tr>
            <tr>
                <td height="30" bgcolor="#E7F0FA" class="tdHead">
                </td>
                <td height="30" bgcolor="#E7F0FA" class="tdHead">
                    <input name="Proinfocancel" id="Proinfocancel" type="button" class="formButton" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>

    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
    <input id="hid_projectid" type="hidden" value="<%=projectid %>" />
</asp:Content>
