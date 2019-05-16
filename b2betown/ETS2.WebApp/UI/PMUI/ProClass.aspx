<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ProClass.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.ProClass" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 100; //每页显示条数

        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1);

            //装载产品列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=proclasslist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询产品分类列表错误");
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


            //录入，关闭录入电子票窗口
            $("#cancel_luru").click(function () {
                $("#proclass").hide();
            })

            //录入电子码提交
            $("#submit_luru").click(function () {

                var classid = $("#hid_classid").trimVal();
                var Classname = $("#Classname").trimVal();

                if (Classname == "") {
                    $.prompt("请填写名称");
                    return;
                }
                $.post("/JsonFactory/ProductHandler.ashx?oper=proclassmanage", { Classname: Classname, classid: classid, comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#Classname").val("");
                        $("#hid_classid").val("0")
                        $("#proclass").hide();
                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#Classname").val("");
                        $("#hid_classid").val("0")
                        $("#proclass").hide();

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


        //弹出录入电子票兰
        function manageclass(classid) {
            $("#hid_classid").val(classid);
            $("#lurutitle").text("产品分类管理:");
            $("#proclass").show();
        };

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li ><a href="/ui/pmui/ProductList.aspx" onfocus="this.blur()" target=""><span>
                    产品管理</span></a></li>
                <li><a href="/ui/pmui/ProductAdd.aspx" onfocus="this.blur()" target=""><span>添加产品</span></a></li>
                <li class="on"><a href="/ui/pmui/ProClass.aspx" onfocus="this.blur()" target=""><span>
                    产品分类管理</span></a></li>
                <li><a href="/UI/hotel/RoomTypeList.aspx" onfocus="this.blur()" target=""><span>客房管理</span></a></li>
                <li><a href="/UI/hotel/RoomTypeEdit.aspx" onfocus="this.blur()" target=""><span>添加房型
                </span></a></li>
                <li><a href="/ui/shangjiaui/DirectSellSetting.aspx" onfocus="this.blur()" target="">
                    <span>产品展示页面设置</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    产品分类管理</h3>
                <input  type="button" onclick="manageclass('0'})" class="formButton" value="  添加新分类  " />
                <table width="780" border="0">
                    <tr>
                        <td width="20">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="200">
                            <p align="left">
                                分类名称
                            </p>
                        </td>
                        <td width="71">
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
                        <td >
                            <p align="left">
                                ${Classname}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                             <input  type="button" onclick="manageclass(${Id})" class="formButton" value="  管 理  " />
                            </p>
 
                        </td>
                    </tr>
    </script>
    <div id="proclass" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 400px; height: 300px; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="30" align="right" bgcolor="#E7F0FA" class="tdHead">
                   名称:
                </td>
                <td height="30" bgcolor="#E7F0FA" class="tdHead" id="td1">
                      <input type="text" id="Classname" value="" />
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input type="hidden" id="hid_classid" value="0" />
                    <input name="submit_class" id="submit_class" type="button" class="formButton" value="  确  定  " />
                    <input name="cancel_class" id="cancel_class" type="button" class="formButton" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
