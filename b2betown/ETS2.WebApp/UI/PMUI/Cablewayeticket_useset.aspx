<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="Cablewayeticket_useset.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.Cablewayeticket_useset" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">


        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();

            SearchList(1, 10);

            function SearchList(pageindex, pagesize) {

                $.post("/JsonFactory/ProductHandler.ashx?oper=pro_worktimepagelist", { comid: comid, pageindex: pageindex, pagesize: pagesize }, function (data) {
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


            $("#sub_endtime").click(function () {
                var id = $("#hid_id").val();
                var title = $("#title").val();
                var defaultendtime = $("#defaultendtime").val();
                var defaultstartime = $("#defaultstartime").val();
                $.post("/JsonFactory/ProductHandler.ashx?oper=uppro_worktime", { id: id, comid: $("#hid_comid").trimVal(), title: title, defaultendtime: defaultendtime, defaultstartime: defaultstartime }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        alert("添加修改成功！");
                        window.location.reload();
                    }
                })
            })

        })

        function addtmp(tmpid) {
            $("#orderinfo").show();
            $("#hid_id").val(tmpid);
            if (tmpid == 0) {
                $("#title").val("");
                $("#defaultendtime").val("");
            } else {

                $.post("/JsonFactory/ProductHandler.ashx?oper=pro_worktimebyid", { id: tmpid, comid: $("#hid_comid").trimVal() }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //$.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#title").val(data.msg.title);
                        $("#defaultendtime").val(data.msg.defaultendtime);
                        $("#defaultstartime").val(data.msg.defaultstartime);
                        
                    }
                })
            }
        }

        function deltmp(tmpid, tmpname) {
            if (confirm("确认删除" + tmpname + "吗?")) {
                if (tmpid == 0) {
                    alert("删除失败");
                    return;
                } else {
                    $.post("/JsonFactory/ProductHandler.ashx?oper=delRentserver", { id: tmpid, comid: $("#hid_comid").trimVal() }, function (data) {
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
        function setSpecialData(id, title) {
            window.open("Cablewayeticket_useset_specialdate.aspx?proworktimeid=" + id+"&proworktimetitle="+title, target = "_blank"); 
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    默认下班时间设定</h3>
                <h4 style="float: right">
                    <a style="" href="javascript:;" onclick="addtmp(0);" class="a_anniu">新建服务</a>
                </h4>
                <table width="780" border="0">
                    <tr>
                        <td width="50px">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                名称
                            </p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                默认上班时间
                            </p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                默认下班时间
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
                            <p>${title}</p>
                        </td>
                        <td>
                            <p>${defaultstartime}</p>
                        </td>
                        <td>
                            <p>${defaultendtime}</p>
                        </td>
                      
                        <td>
                            <p> <a href="javascript:;" onclick="setSpecialData('${id}','${title}')" class="a_anniu">设定特殊日期下班时间</a>  <a href="javascript:;" onclick='addtmp(${id});' class="a_anniu">管理</a>  <a href="javascript:void(0)" onclick="deltmp('${id}','${title}')" class="a_anniu">删除</a>   </p>
                        </td>
                    </tr>
    </script>
     <div id="orderinfo" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px 300px;
        width: 400px; height: auto; display: none; left: 20%; position: absolute; top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    标题名称：<input type="text" id="title" value="" style="width: 120px;" />
                </td>
            </tr>
             <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    上班时间：<input type="text" id="defaultstartime" value="" style="width: 120px;" />(格式：09:00)
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    下班时间：<input type="text" id="defaultendtime" value="" style="width: 120px;" />(格式：16:00)
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

    <input type="hidden" id="hid_id" value="0" />
</asp:Content>