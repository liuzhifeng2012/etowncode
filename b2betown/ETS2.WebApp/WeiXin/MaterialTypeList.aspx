<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="MaterialTypeList.aspx.cs"
    Inherits="ETS2.WebApp.WeiXin.MaterialTypeList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">


        $(function () {

            var pagee = 1;
            var pageSize = 10; //每页显示条数

            SearchList(pagee);

            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/WeiXinHandler.ashx?oper=wxmaterialtypepagelist",
                    data: { pageindex: pageindex, pagesize: pageSize, comid: $("#hid_comid").trimVal() },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询微信素材列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist").html("<tr><td height=\"26\" colspan=\"3\">查询数据为空</td></tr>");
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

        function insnewmaterial() {

            window.open("Materialtypedetail.aspx", target = "_self");
        }
    
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="materiallist.aspx" onfocus="this.blur()"><span>文章列表</span></a></li>
                <li class="on"><a href="MaterialTypeList.aspx" onfocus="this.blur()">添加文章类型</a></li>
                <li><a href="periodical.aspx" onfocus="this.blur()"><span>文章期号管理</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    <label>
                        素材类型列表</label></h3>
                <h3>
                    <label style="float: right">
                        <a href="javascript:void(0)" style="color: #2D65AA;" onclick="insnewmaterial()">添加新素材类型</a>
                    </label>
                </h3>
                <table border="0" width="780" class="mail-list-title">
                    <tr>
                        <td width="6%" align="center" bgcolor="#CCCCCC">
                            素材类型id
                        </td>
                        <td width="10%" height="26" align="left" bgcolor="#CCCCCC">
                            <p align="left">
                                类型名称
                            </p>
                        </td>
                        
                        <td width="5%" height="26" align="left" bgcolor="#CCCCCC">
                            <p align="center">
                                是否显示往期
                            </p>
                        </td>
                        <td width="5%" height="26" align="left" bgcolor="#CCCCCC">
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
                                ${Id}</p>
                        </td>
                        <td width="10%" height="26" class="sender item">
                            <p align="left">
                                ${TypeName}
                            </p>
                        </td>
                        
                        <td width="5%" height="26" class="sender item">
                            <p align="center">
                                ${Isshowpast}
                            </p>
                        </td>
              <td width="5%" height="26" class="sender item">
                            <p align="center">
                                 <a href="Materialtypedetail.aspx?id=${Id}">编   辑</a>
                            </p>
                        </td>
                    </tr>
    </script>
</asp:Content>
