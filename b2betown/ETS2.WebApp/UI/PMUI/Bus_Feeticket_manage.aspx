<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="Bus_Feeticket_manage.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.Bus_Feeticket_manage" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">


        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();

            SearchList(1, 20);

            function SearchList(pageindex, pagesize) {

                $.post("/JsonFactory/ProductHandler.ashx?oper=busfeeticketpagelist", { comid: comid, pageindex: pageindex, pagesize: pagesize }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalcount == 0) {
                            //                                $("#tblist").html("查询数据为空");
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalcount, pagesize, pageindex);
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

                        SearchList(page, newpagesize);

                        return false;
                    }
                });
            }


            //加载产品
            var agentlist
            var pageSize = 10;
            var pageindex = 1;
            SearchList_pro(1);
            function SearchList_pro(pageindex) {
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=busfeeticketbindingpropagelist",
                    data: { pageindex: pageindex, pagesize: 10, comid: $("#hid_comid").trimVal(), busid: $("#hid_busid").trimVal(), bindingprotype: $("#hid_bindingprotype").trimVal() },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            return;
                        }
                        if (data.type == 100) {
                            $("#protblist").empty();
                            $("#prodivPage").empty();

                            if (data.totalCount == 0) {
                                //                                $("#tblist").html("查询数据为空");
                            } else {

                                $("#BindingProItemEdit").tmpl(data.msg).appendTo("#protblist");
                                setpage_pro(data.totalCount, pageSize, pageindex);
                            }
                        }
                    }
                })
            }

            //产品分页
            function setpage_pro(newcount, newpagesize, curpage) {
                $("#prodivPage").paginate({
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

                        SearchList_pro(page, newpagesize);

                        return false;
                    }
                });
            }


            $("#cancel").click(function () {

                $("#bindingprotable").hide();
            })


            $(".bindingpro").live("click", function () {
                var id = $(this).attr("data-id");
                var type = $(this).attr("data-type");
                $("#bindingprotable").show();
                $("#hid_busid").val(id);
                $("#hid_bindingprotype").val(type);
                if (type == 1) {
                    $("#typename").html("绑定使用产品");
                } else {
                    $("#typename").html("绑定码");
                }
                SearchList_pro(1);
            })

            $(".selectlimitweek").live("click", function () {

                var id = $(this).attr("data_id");
                var val = 0;

                if ($(this).attr("checked") == "checked") {
                    val = 1;
                } else {
                    val = 0;
                }

                if (val == 1) {
                    $("#limitweekdaynum" + id + "_star").text("平日预约码:");
                    $("#limitweekdaynum" + id + "_end").show();
                } else {
                    $("#limitweekdaynum" + id + "_star").text("限预约数量:");
                    $("#limitweekdaynum" + id + "_end").hide();
                }
            })

        })
        function deltmp(tmpid, tmpname) {
            if (confirm("确认删除" + tmpname + "吗?")) {
                if (tmpid == 0) {
                    alert("删除失败");
                    return;
                } else {
                    $.post("/JsonFactory/ProductHandler.ashx?oper=delbusfeeticket", { tmpid: tmpid, comid: $("#hid_comid").trimVal() }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            alert(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            alert("删除成功");
                            window.location.reload();
                            return;
                        }
                    });
                }
            }
        }


        function setwarrant(proid, busid, subtype) {


                    var limitweek = $("#limitweek_" + proid + "_" + busid).val();
                    var limitweekdaynum = $("#limitweekdaynum_" + proid + "_" + busid).val();
                    var limitweekendnum = $("#limitweekendnum_" + proid + "_" + busid).val();
                    //$("#hid_bindingprotype").val()=1为产品绑定

                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/ProductHandler.ashx?oper=busbindingpro",
                        data: { busid: busid, comid: $("#hid_comid").trimVal(), proid: proid, type: $("#hid_bindingprotype").val(), subtype: subtype, limitweek: limitweek, limitweekdaynum: limitweekdaynum, limitweekendnum: limitweekendnum },
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");
                            if (data.type == 1) {
                                return;
                            }
                            if (data.type == 100) {
                                if (subtype == 0) {
                                    $("#bpro" + proid).html("已经关闭关联");
                                } else {
                                    $("#bpro" + proid).html("关联成功");
                                }
                            }
                        }
                    })
        }


    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/pmui/projectlist.aspx" onfocus="this.blur()" target=""><span>项目管理</span></a></li>
                <li><a href="/ui/pmui/Projectedit.aspx" onfocus="this.blur()" target=""><span>添加项目</span></a></li>
                <li><a href="/ui/pmui/ProductList.aspx" onfocus="this.blur()" target=""><span>产品列表</span></a></li>
                <li><a href="/ui/pmui/ProductServerTypeList.aspx" onfocus="this.blur()" target=""><span>
                    添加产品</span></a></li>
                <li><a href="/ui/pmui/order/Salecount.aspx" onfocus="this.blur()" target="">产品统计</a></li>
                <li><a href="/ui/pmui/BindingAgent.aspx" onfocus="this.blur()" target="">导入分销系统产品</a></li>
                <li><a href="/ui/pmui/eticket_useset.aspx" onfocus="this.blur()" target=""><span>商户特定日期设定</span></a></li>
                <li class="on"><a href="/ui/pmui/delivery/deliverylist.aspx" onfocus="this.blur()"
                    target=""><span>运费模版管理</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    大巴车免费券列表</h3>
                <h4 style="float: right">
                    <a style="" href="Bus_Feeticket_add.aspx" class="a_anniu">新建大巴免费券</a>
                </h4>
                <table width="780" border="0">
                    <tr>
                        <td width="20px">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                免费券名称
                            </p>
                        </td>
                        <td width="8%">
                            <p align="left">
                                使用限制
                            </p>
                        </td>
                        <td width="28%">
                            <p align="left">
                                关联使用的产品
                            </p>
                        </td>
                        <td width="28%">
                            <p align="left">
                                免费券库
                            </p>
                        </td>
                        <td>
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
                            <p>${Id}</p>
                        </td>
                        <td>
                            <p>${Title}</p>
                        </td>
                           <td>
                            <p>{{if Iuse==0}}
                                不限制
                                {{/if}}
                                {{if Iuse==1}}
                                限本人使用
                                {{/if}}
                                {{if Iuse==2}}
                                限当天每班次只能预订一次
                                {{/if}}
                            </p>
                        </td>
                         <td>
                            <p> <input name="setw" type="button" class="formButton bindingpro" data-id="${Id}" data-type="1" value="  关联使用产品  " />
                                                        {{if BusProinfo != null}}
                                 {{each(i,data) BusProinfo}}  
                                    <br>${data.proname}
                                  {{/each}}  
                            {{/if}}
                            
                            </p>



                        </td>
                         <td>
                            <p><input name="setw"  type="button" class="formButton bindingpro"  data-id="${Id}" data-type="0" value="  绑定电子码  " />
                            {{if BusPnoinfo != null}}
                                 {{each(i,data) BusPnoinfo}}  
                                    <br>${data.proname}
                                  {{/each}}  
                            {{/if}}
                            
                            </p>
                        
                        </td>
                        <td>
                            <p><a href="Bus_Feeticket_add.aspx?id=${Id}" class="a_anniu">编辑</a> <br> <a href="javascript:void(0)" onclick="deltmp('${Id}','${Title}')" class="a_anniu">删除</a>   </p>
                        </td>
                    </tr>
    </script>

     <script type="text/x-jquery-tmpl" id="BindingProItemEdit"> 

                    <tr style="background-color:#ffffff;border-width: 1px;border-style: solid;border-color: #A6A6A6 #CCC #CCC;">
                        <td>
                            <p align="center">
                                ${Id}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${Pro_name}
                                 {{if bindingprotype==1}}
                                        <br>  <span style="display: ;">对周末单独限定:<input id="limitweek_${Id}_${busid}" data_id="${Id}" class="selectlimitweek" value="1" type="checkbox" {{if BusProinfo!=null}} {{if BusProinfo.limitweek==1}} checked="checked"  {{/if}}{{/if}}> </span>
                                         <span id="limitweekdaynum${Id}_star">限预约数量:</span>
                                         <input id="limitweekdaynum_${Id}_${busid}" style="width: 20px;" class="dataNum dataIcon" value="{{if BusProinfo!=null}}${BusProinfo.limitweekdaynum}{{else}}0{{/if}}">
                                         <span id="limitweekdaynum${Id}_end" style="{{if BusProinfo!=null}} {{if BusProinfo.limitweek!=1}} display: none;  {{/if}}{{/if}}">
                                            <span> 限购周末:</span>
                                            <input id="limitweekendnum_${Id}_${busid}"  style="width: 20px;" class="dataNum dataIcon" value="{{if BusProinfo!=null}}${BusProinfo.limitweekendnum}{{else}}0{{/if}}">
                                         </span>
                                 {{/if}}
                            </p>
                        </td>
                        <td>
                            <p align="left" id="bpro${Id}">
                            {{if bindingprotype==1}}
                                {{if BusProinfo !=null}}
                                    <input name="setw" onclick="setwarrant('${Id}','${busid}','0')" style="color:#f00" type="button" class="formButton" value="  解除绑定  " />
                                {{else}}
                                    <input name="setw" onclick="setwarrant('${Id}','${busid}','1')" type="button" class="formButton" value="   绑定使用 " />
                                {{/if}}
                            {{else}}
                                {{if BusPnoinfo !=null}}
                                    <input name="setw" onclick="setwarrant('${Id}','${busid}','0')"   style="color:#f00" type="button" class="formButton" value="  解除绑定码  " />
                                {{else}}
                                    <input name="setw" onclick="setwarrant('${Id}','${busid}','1')" type="button" class="formButton" value="  绑定使用码  " />
                                {{/if}}

                            {{/if}}
                            </p>
 
                        </td>
                    </tr>
    </script>

    <div id="bindingprotable" style="background-color: #ffffff; border: 1px solid #5984bb;
        margin: 0px auto; width: 760px; display: none; z-index: 10; 
        top: 20%;left:10%;position: absolute;" class="dialog">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr style="height: 30px; background-color: rgb(255, 255, 255); font-size:18px;  text-align:center;">
                <td height="100%" id="typename">

                  </td>
            </tr>
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
                                        产品名称
                                    </p>
                                </td>
                                
                                <td width="40">
                                    <p align="left">
                                        &nbsp;</p>
                                </td>
                            </tr>
                            <tbody id="protblist">
                            </tbody>
                        </table>
                        <div id="prodivPage">
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td height="38" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input name="cancel" id="cancel" type="button" class="formButton" value="  关  闭  " />
                    <br />
                    注:1.更改限定数量只能 解绑后重新绑定。2.如果不限制绑定数量则填写 0 。3.如果对周末，平日单独限定时，此票禁止预约周末把周末填为 -1。
                </td>
            </tr>
        </table>
    </div>
    <input type="hidden" id="hid_bindingprotype" value="0" />
    <input type="hidden" id="hid_busid" value="0" />

    
</asp:Content>