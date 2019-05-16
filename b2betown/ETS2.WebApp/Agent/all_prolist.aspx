<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master"
    CodeBehind="all_prolist.aspx.cs" Inherits="ETS2.WebApp.Agent.all_prolist" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <title>分销全部商品</title>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {

            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#Search").click();    //这里添加要处理的逻辑  
                    return false;
                }
            });

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

            SearchList(1);

            $("#Search").click(function () {
                SearchList(1);
            })

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

        //        document.onkeydown = keyDownSearch;

        //        function keyDownSearch(e) {
        //            // 兼容FF和IE和Opera  
        //            var theEvent = e || window.event;
        //            var code = theEvent.keyCode || theEvent.which || theEvent.charCode;
        //            if (code == 13) {
        //                $("#Search").click(); //具体处理函数  
        //                return false;
        //            }
        //            return true;
        //        }

        function Proadd(Pro_name, Pro_Remark, Service_Contain, Service_NotContain, Precautions) {
            $("#ProInfo").show();
            $("#Pro_name").html(Pro_name);
            $("#Pro_Remark").html(Pro_Remark);
            $("#Service_Contain").html(Service_Contain);
            $("#Service_NotContain").html(Service_NotContain);
            $("#Precautions").html(Precautions);

        }
//        function selbyservertype(servertype) {
//            SearchList(1, servertype);
//        }
        //装载产品列表
        function SearchList(pageindex) {
            var servertype = $("#sel_servertype").trimVal();
            var agentid = $("#hid_agentid").trimVal();
            var key = $("#key").trimVal();
            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }

            $.post("/JsonFactory/AgentHandler.ashx?oper=allwarrantprolist", { pageindex: pageindex, pagesize: pageSize, agentid: agentid, key: key, viewmethod: "1,2", servertype: servertype }, function (data) {

                data = eval("(" + data + ")");

                if (data.type == 1) {
                    //                        $.prompt("查询渠道列表错误");
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
         
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul class="composetab">
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_sel">
                        <div>
                            <a href="all_prolist.aspx">所有产品</a></div>
                    </div>
                </li>
            </ul>
            <div class="toolbg toolbgline toolheight nowrap" style="">
                <div class="right">
                    <label>
                        <select id="sel_servertype">
                        </select>
                    </label>
                    <label>
                        关键词查询
                        <input name="key" class="mi-input" type="text" id="key" style="width: 160px;" placeholder="产品编号、产品名称" />
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
                    </label>
                 <%--   <a href="javascript:;" onclick="selbyservertype('1')">电子门票</a> <a href="javascript:;"
                        onclick="selbyservertype('9')">酒店订房</a> <a href="javascript:;" onclick="selbyservertype('10')">
                            旅游巴士</a> <a href="javascript:;" onclick="selbyservertype('14')">旅游户外保险</a>--%>
                </div>
                <div class="nowrap left" unselectable="on" onselectstart="return false;">
                    <!--<a class="btn_gray btn_space" hidefocus="" id="quick_del" href="javascript:;" name="del">删除</a>-->
                </div>
            </div>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <table width="780" border="0" class="O2">
                    <tr class="O2title">
                        <td width="50">
                            <p align="left">
                                产品编号</p>
                        </td>
                        <td width="40">
                            <p align="left">
                                图片
                            </p>
                        </td>
                        <td width="330">
                            <p align="left">
                                产品名称</p>
                        </td>
                        <td width="150">
                            <p align="left">
                                商户名称</p>
                        </td>
                        <td width="25">
                            淘
                        </td>
                        <td width="50">
                            <p align="left">
                                库存</p>
                        </td>
                        <td width="50">
                            门市价
                        </td>
                        <td width="50">
                            销售价
                        </td>
                        <td width="50">
                            结算价
                        </td>
                        <td width="120">
                            <p align="left">
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                备注
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
   
                <tr class="d_out" onmouseover="this.className='d_over'" onmouseout="this.className='d_out'">
                        <td >  
                                    <p>{{if Manyspeci==1}}--{{else}}${Id} {{/if}}</p> 
                        </td>
                        <td title="${Projectname}">
                            <p align="left">
                               {{if ProImg !=""}}
                                <img src="${ProImg}" height="30" width="30">
                              {{/if}}</p>
                        </td>
                         
                        <td title="${Projectname} ${Pro_name}">
                            <p align="left" >
                              <a href="javascript:;" onclick="Proadd('${Pro_name}','${Pro_Remark}','${Service_Contain}','${Service_NotContain}','${Precautions}')">  
                              ${Pro_name}   
                              {{if IsViewStockNum==1}}
                              
                            {{/if}}</a></p>
                        </td>
                         <td >
                            <p align="left" title="${Comname}">
                            ${Comname}
                              </p>
                        </td>
                        <td class="home-10">
                        {{if Server_type==1}}
                            {{if Bindingid ==0 }}
                                {{if Source_type=="自动生成"}}
                                    淘
                                {{/if}}
                            {{else}}
                                {{if BindingSource_type=="1"}}
                                    淘
                                {{/if}}
                            {{/if}}
                        {{/if}}
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
                           {{if Manyspeci==1}}--{{else}}  ${Face_price}{{/if}}
                        </td>
                        <td >
                            <p align="left">
                               {{if Manyspeci==1}}--{{else}}  ${Advise_price}{{/if}}</p>
                        </td>
                        <td >
                            <p align="left">
                               {{if Manyspeci==1}}--{{else}}  ${Agent_price}{{/if}}</p></p>
                        </td>

                        <td > 
                        {{if Agent_price>0}} 
                           {{if Warrant_type==1}}
                                {{if Ispanicbuy==0}}
                                 <a href="pro_sales.aspx?id=${Id}" class="qianggou_anniu">抢购</a>
                                {{else}}
                                 <a  href="pro_sales.aspx?id=${Id}" class="qianggou_anniu">订购</a>
                                {{/if}}
                           {{else}}
                                {{if Warrant_type==3}}
                                  <a href="pro_sales.aspx?id=${Id}" class="qianggou_anniu">订购</a>
                                {{else}}
                                     {{if binding_Warrant_type==0}}
                                       <a  href="pro_sales.aspx?id=${Id}" class="qianggou_anniu">倒码</a>
                                    {{else}}
                                       只接受接口出票
                                    {{/if}}
                                {{/if}}
                           {{/if}} 
                            
                             {{else}}
                              已售完
                         {{/if}}
                        </td>
                        <td  title="${Pro_explain}">
                            <p align="left">
                                ${Pro_explain.length>8?Pro_explain.substring(0,8)+"..":Pro_explain}</p>
                        </td>
                    </tr>
                    {{if Manyspeci==1}}
                        {{each(i,user) GuigeList}}
                         <tr class="d_out guigelist${Id}" style="background-color: #cccccc;" onmouseover="this.className='d_over'" onmouseout="this.className='d_out'">
                            <td > 
                            </td>
                            <td>
                            ${Id}-${user.id}
                            </td>
                            <td title="${Projectname} ${Pro_name}${user.speci_name}">
                             ${Pro_name}${user.speci_name}
                            </td>
                            <td class="home-10">
                            </td>
                            <td >
                            </td>
                            <td >
                            ${user.speci_totalnum}
                            </td>
                            <td >
                            ${user.speci_face_price}
                            </td>
                            <td >
                            ${user.speci_advise_price}
                            </td>
                            <td >
                           ${user.speci_agentsettle_price}
                            </td>
                            <td > 
                            </td>
                            <td>
                            </td>
                         </tr>
                        {{/each}}
                     {{/if}}
               
                    
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
    <div id="divBackToTop" style="display: none">
    </div>
    <div class="sessionToken">
    </div>
    <input type="hidden" id="hid_accountid" value="<%=accountid %>" />
    <input type="hidden" id="hid_accountlevel" value="<%=AccountLevel %>" />
</asp:Content>
