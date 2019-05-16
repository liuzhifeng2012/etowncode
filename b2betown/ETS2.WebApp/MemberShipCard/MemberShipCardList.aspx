<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberShipCardList.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.MemberShipCard.MemberShipCardList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            var pagee = 1;
            var pageSize = 100; //每页显示条数
            SearchList(pagee);



            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/WeiXinHandler.ashx?oper=AllMembershipcardpagelist",
                    data: { comid: $("#hid_comid").trimVal(), pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询素材列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                //                                $("#tblist").html("<tr><td height=\"26\" colspan=\"7\">查询数据为空</td></tr>");
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
        function delmaterial(materialid) {
            if (confirm("确认删除此素材信息吗？")) {
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=delMemberShipCardmaterial", { materialid: materialid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("删除素材信息出错" + data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("删除素材信息成功", {
                            buttons: [{ title: '确定', value: true}],
                            opacity: 0.1,
                            focus: 0,
                            show: 'slideDown',
                            submit: function (e, v, m, f) {
                                if (v == true)
                                    location.href = "MemberShipCardList.aspx";
                            }
                        });
                    }
                });
            } else {
                alert("你取消了删除操作");
            }
        }
        function insnewmaterial() {

            window.open("MemberShipCardDetail.aspx", target = "_self");
        }
        function sortmaterial() {

            window.open("MemberShipCardSort.aspx", target = "_self");
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/weixin/ShangJiaSet2.aspx" onfocus="this.blur()"><span>微信接口设置</span></a></li>
                <li><a href="/weixin/WxDefaultReply.aspx" onfocus="this.blur()"><span>微信默认设置</span></a></li>
                <li><a href="/weixin/menulist.aspx" onfocus="this.blur()"><span>自定义菜单管理</span></a></li>
                <li class="on"><a href="/MemberShipCard/MemberShipCardList.aspx" onfocus="this.blur()">
                    <span>会员卡专区管理</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    <label>
                        微信端会员卡素材列表</label></h3>
                <h3>
                    <label style="float: right">
                        <a href="javascript:void(0)" style="color: #2D65AA;" onclick="insnewmaterial()">添加微信端会员卡新素材</a>
                        <a href="javascript:void(0)" style="color: #2D65AA;" onclick="sortmaterial()">微信端会员卡素材排序</a></label>
                </h3>
                <table border="0" width="780" class="mail-list-title">
                    <tr>
                        <td width="6%" align="center" bgcolor="#CCCCCC">
                            素材id
                        </td>
                        <td width="10%" height="26" align="left" bgcolor="#CCCCCC">
                            <p align="left">
                                标题
                            </p>
                        </td>
                        <td width="5%" height="26" align="left" bgcolor="#CCCCCC">
                            <p align="center">
                            </p>
                        </td>
                        <td width="25%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                封面
                            </p>
                        </td>
                        <td width="25%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                            </p>
                        </td>
                        <td width="8%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                使用状态
                            </p>
                        </td>
                        <td width="30%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                管 理
                            </p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
                <p>
                </p>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <td width="6%" class="sender item">
                            <p align="center">
                                ${MaterialId}</p>
                        </td>
                        <td width="10%" height="26" class="sender item">
                            <p align="left">
                                ${Title}
                            </p>
                        </td>
                        <td width="5%" height="26" class="sender item">
                            <p align="center">
                                 --
                            </p>
                        </td>
                        <td width="15%" height="26" class="sender item">
                            <p align="center">
                               <img alt=""   id="headPortraitImg" src="${Imgpath}"  width="50px"  height="30px"/>  </p>
                        </td>
                      
                     
                        <td width="25%" height="26" class="sender item">
                            <p align="center">
                                --</p>
                        </td>
                          <td width="4%" height="26" class="sender item">
                            <p align="center">
                                ${Applystate}</p>
                        </td>
                        <td width="30%" height="26" class="sender item">
                         <p align="center">
                           <a href="MemberShipCardDetail.aspx?materialid=${MaterialId}"  >编辑</a>  <a   href="javascript:void(0)" onclick="delmaterial('${MaterialId}')" >删除</a>
                           </p>
                        </td>
                    </tr>
    </script>
</asp:Content>
