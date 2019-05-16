<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ERNIEActList.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.ERNIEActList" %>

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
                    url: "/JsonFactory/PromotionHandler.ashx?oper=ERNIEActpagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, runstate:"0,1" },
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

        })

        function Online(actid) {
            if (actid == '') {
                $.prompt("请选择完成上线的抽奖活动");
                return;
            }



            $.ajax({
                type: "post",
                url: "/JsonFactory/PromotionHandler.ashx?oper=ERNIEeditActOnline",
                data: { actid: actid},
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("完成上线");
                        return;
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

                 <li class="on"><a href="ERNIEActList.aspx" onfocus="this.blur()" ><span>摇奖活动列表</span></a></li>
                 <li><a href="ERNIEActEdit.aspx" onfocus="this.blur()" ><span>添加摇奖活动</span></a></li>
                 <li><a href="ERNIERecordList.aspx" onfocus="this.blur()" ><span>中奖管理</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    摇奖活动列表</h3>
                <h3>
                    &nbsp;</h3>
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
                        <td width="80">
                            <p align="left">
                                摇奖用户限定次数
                            </p>
                        </td>
                        <td width="45"> 完成状态 </td>
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
                                ${ERNIE_Limit}(${Limit_Num}次)  </p>
                        </td>
                        <td>${Online_str}</td>
                        <td>
                            <p align="left">
                                  ${Runstate}
                            </p>
                        </td>
                        <td>
                            <p align="left"> 
                            {{if Online==0}}
                            <input type="button" Onclick="javascript:if(confirm('确认上线，请确认已编辑完成抽奖活动，确认后，奖品数量等信息将不能修改！'))Online('${Id}')" value="编辑完成上线"> 
                            {{/if}}
                            <a href="ERNIEActEdit.aspx?actid=${Id}">修改</a>
                              </p>
                        </td>
                    </tr>
    </script>
</asp:Content>
