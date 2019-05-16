<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ProductManage_costrili.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.ProductManage_costrili" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script> 
    <script type="text/javascript">


        $(function () {
            var proid = $("#hid_proid").trimVal();
            var speciid = $("#hid_speciid").trimVal();
            var manyspeci = $("#hid_manyspeci").trimVal();
            if (speciid == 0 && manyspeci==1) {
                speciid = $("#selectspeciid").trimVal()
            }

          
            var comid = $("#hid_comid").trimVal();
            var d_stardate = $("#hid_stardate").trimVal();
            var d_enddate = $("#hid_enddate").trimVal();

            //日历
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();
            });



            SearchList(1, 10);

            function SearchList(pageindex, pagesize) {
                if (manyspeci == 1) {
                    if (speciid == 0) {
                        alert("此产品为多规格产品，请选择规格。");
                        return;
                    }
                }


                $.post("/JsonFactory/ProductHandler.ashx?oper=procostrilipagelist", { comid: comid, proid: proid, speciid: speciid, pageindex: pageindex, pagesize: pagesize }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //$.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalcount == 0) {
                            //$("#tblist").html("查询数据为空");
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


            $("#sub_endtime").click(function () {

                var id = $("#hid_id").val();
                var costprice = $("#costprice").val();
                var stardate = $("#stardate").val();
                var enddate = $("#enddate").val();


                $.post("/JsonFactory/ProductHandler.ashx?oper=upprocostrili", { id: id, proid: proid, speciid: speciid, comid: $("#hid_comid").val(), costprice: costprice, stardate: stardate, enddate: enddate }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        alert(" 操作成功！");
                        window.location.reload();
                    }
                })
            })

        })

        function addtmp(tmpid) {
            var speciid = $("#hid_speciid").trimVal();
            var manyspeci = $("#hid_manyspeci").trimVal();
            if (manyspeci == 1) {
                if (speciid == 0) {
                    alert("此产品为多规格产品，请选择规格。");
                    return;
                }
            }

            $("#orderinfo").show();
            $("#hid_id").val(tmpid);
            if (tmpid == 0) {
                $("#costprice").val("");
                $("#stardate").val("");
                $("#enddate").val("");
            } else {

                $.post("/JsonFactory/ProductHandler.ashx?oper=procostrilibyid", { id: tmpid, comid: $("#hid_comid").trimVal() }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //$.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#costprice").val(data.msg.costprice);
                        $("#stardate").val(data.msg.stardate);
                        $("#enddate").val(data.msg.enddate);
                    }
                })
            }
        }

        function deltmp(tmpid) {
            if (confirm("确认删除吗?")) {
                if (tmpid == 0) {
                    alert("删除失败");
                    return;
                } else {
                    $.post("/JsonFactory/ProductHandler.ashx?oper=delcostrili", { id: tmpid, comid: $("#hid_comid").trimVal() }, function (data) {
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

        function selectspeci() {
            var selectspeciid = $("#selectspeciid").trimVal();
            $("#hid_speciid").val(selectspeciid);
        
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                  <%=proname%> 
                  <%if (gglist != null)
                    { %>
                  <select id="selectspeciid" class="mi-input" onchange="selectspeci();" style="margin-left: 0px;">
                   <option value="0">请选择规格</option>
                  <%
                        for (int i = 0; i < gglist.Count; i++)
                        {
                         %>
                         <option value="<%=gglist[i].id %>"><%=gglist[i].speci_name %>(<%=gglist[i].speci_agentsettle_price %>)</option>

                      <%}
                        %>
                        </select>
                   <%}
                    %>- 结算金额设定</h3>
                <h4 style="float: right">
                    <a style="" href="javascript:;" onclick="addtmp(0);" class="a_anniu">新建结算日期</a>
                </h4>
                <table width="780" border="0">
                    <tr>
                        <td width="50px">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                价格
                            </p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                开始日期
                            </p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                结束日期
                            </p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                最后操作人
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
                            <p>${id}</p>
                        </td>
                        <td>
                            <p>${costprice}</p>
                        </td>
                        <td>
                            <p>${stardate}</p>
                        </td>
                        <td>
                            <p>${enddate}</p>
                        </td>
                      <td>
                            <p>${admin}</p>
                        </td>
                        <td>
                            <p>  <a href="javascript:;" onclick='addtmp(${id});' class="a_anniu">管理</a>  <a href="javascript:void(0)" onclick="deltmp('${id}')" class="a_anniu">删除</a>   </p>
                        </td>
                    </tr>
    </script>
     <div id="orderinfo" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px 300px;
        width: 400px; height: auto; display: none; left: 20%; position: absolute; top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    成本价：<input type="text" id="costprice" value="" style="width: 120px;" />
                </td>
            </tr>
             <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    开始日期：<input type="text" id="stardate"  isdate="yes" value="" style="width: 120px;" />
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    结束日期：<input type="text" id="enddate"  isdate="yes" value="" style="width: 120px;" />
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input id="sub_endtime" name="sub_endtime" type="button" class="formButton" value="  确认  " />
                    <input name="cancel_endtime" type="button" onclick="$('#orderinfo').hide();"
                        value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>

    <input type="hidden" id="hid_proid" value="<%=proid %>" />
    <input type="hidden" id="hid_manyspeci" value="<%=manyspeci %>" />
    

    <input type="hidden" id="hid_speciid" value="0" />
    <input type="hidden" id="hid_enddate" value="" />
    <input type="hidden" id="hid_stardate" value="" />
</asp:Content>