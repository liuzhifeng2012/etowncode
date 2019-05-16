<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="CardList.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.CardList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数

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
                    url: "/JsonFactory/CardHandler.ashx?oper=pagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询活动列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                //                                $("#tblist").html("<tr><td colspan='8'>查询数据为空</td></tr>");
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


            function GoCreate(idtemp) {
                alert(idtemp);
                var comid = $("#hid_comid").trimVal();
                $.post("/JsonFactory/CardHandler.ashx?oper=CreateCard", { comid: comid, cardid: idtemp
                }, function (data) {
                    data = eval('(' + data + ')');
                    if (data.type == '100') {
                        $.prompt("卡号生成成功");
                        window.location();
                        return;
                    } else {
                        $.prompt("生成出错");
                        return;
                    }
                });
            }



        })
        function GoOut(crid) {
          
            location.href = "/Excel/DownExcel.aspx?oper=outcardno&crid=" + crid + "&comid=" + $("#hid_comid").trimVal();
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="CardList.aspx" onfocus="this.blur()"><span>卡片管理</span></a></li>
                <li><a href="CardEdit.aspx" onfocus="this.blur()"><span>添加卡片</span></a></li>
                <li><a href="MemberCardList.aspx" onfocus="this.blur()"><span>已录入卡号管理</span></a></li>
                <li><a href="PublishList.aspx" onfocus="this.blur()"><span>发行管理</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    全部卡片 实体卡 电子卡 储值礼品卡</h3>
                <h3>
                    &nbsp;</h3>
                <table width="780" border="0">
                    <tr>
                        <td width="33">
                            <p align="left">
                                序号</p>
                        </td>
                        <td width="144">
                            <p align="left">
                                卡片名称
                            </p>
                        </td>
                        <td width="59">
                            卡种类
                        </td>
                        <td width="69">
                            <p align="left">
                                发行张数</p>
                        </td>
                        <td width="103">
                            卡号管理
                        </td>
                        <td width="78">
                            <p align="left">
                                状态
                            </p>
                        </td>
                        <td width="162">
                            <p align="left">
                                创建日期
                            </p>
                        </td>
                        <td width="128">
                            <p align="left">
                                操作</p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <td width="51">
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td width="157">
                            <p align="left">
                                ${Cname}
                            </p>
                        </td>
                        <td width="72">
                            <p align="left">
                                ${Ctype}</p>
                        </td>
                        <td width="55">
                            <p align="left">
                                ${Printnum}</p>
                        </td>
                        <td width="40">
                            <p align="left">
                             
                              <a  href="javascript:void(0)" id="GoOut" onclick="GoOut('${Id}')">${Outstate}</a>
                              </p>
                        </td>
                        <td width="145">
                            <p align="left">
                               ${Createstate}
                                </p>
                        </td>
                        <td width="69">
                            <p align="left">
                                   ${ChangeDateFormat(Subdate)}
                            </p>
                        </td>
                        <td width="137">
                            <p align="left"><a href="CardEdit.aspx?id=${Id}">修改</a>
                              </p>
                        </td>
                    </tr>
    </script>
</asp:Content>
