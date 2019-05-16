<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ERNIEAwardList.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.ERNIEAwardList" %>

    <asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数

        $(function () {
            var hid_actid = $("#hid_actid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1);

            //装载摇奖活动
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/PromotionHandler.ashx?oper=ERNIEgetActById",
                    data: { actid: hid_actid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询摇奖活动列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist").html("查询数据为空");
                            } else {
                                //$("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                // setpage(data.totalCount, pageSize, pageindex);

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
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="PromotionList.aspx" onfocus="this.blur()" >
                    <span>促销活动管理</span></a></li>
                <li><a href="PromotionEdit.aspx" onfocus="this.blur()" ><span>添加促销活动</span></a></li>
                 <li class="on"><a href="ERNIEActList.aspx" onfocus="this.blur()" ><span>摇奖活动管理</span></a></li>
                 <li><a href="ERNIEActEdit.aspx" onfocus="this.blur()" ><span>添加摇奖活动</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    摇奖活动列表</h3>
                <h3>
                    &nbsp;</h3>
                    <table>
                    <tr>
                    <td>
                    添加奖项
                    </td>
                    </tr>
                    </table>
                <table width="780" border="0">
                    <tr>
                        <td width="51">
                            <p align="left">
                                序号</p>
                        </td>
                        <td width="157">
                            <p align="left">
                                活动名称
                            </p>
                        </td>
                        <td width="50">
                            活动方式
                        </td>
                        <td width="120">
                            <p align="left">
                                活动时间</p>
                        </td>
                        <td width="40">
                            摇奖基数
                        </td>
                        <td width="60">
                            <p align="left">
                                摇奖用户限定次数
                            </p>
                        </td>
                        <td width="45"> 是否过期 </td>
                        <td width="40">
                            运行状态
                        </td>
                        <td width="70">
                            <p align="left">
                                操作</p>
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
                        <td >
                            <p align="left">
                                ${Title}
                            </p>
                        </td>
                        <td >
                            <p align="left">
                                ${ERNIE_type}</p>
                        </td>
                        <td >
                            <p align="left">
                                 ${ChangeDateFormat(ERNIE_star)} 至 ${ChangeDateFormat(ERNIE_end)}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${ERNIE_RateNum}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${ERNIE_Limit} -- ${Limit_Num}  </p>
                        </td>
                        <td></td>
                        <td>
                            <p align="left">
                                  ${Runstate}
                            </p>
                        </td>
                        <td>
                            <p align="left"><a href="ERNIEAwardEdit.aspx?actid=${Id}">修改</a>
                              </p>
                        </td>
                    </tr>
    </script>
        <input type="hidden" id="hid_actid" value="<%=actid %>" />
</asp:Content>
