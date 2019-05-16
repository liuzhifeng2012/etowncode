<%@ Page Title="" Language="C#" MasterPageFile="/UI/Etown.Master" AutoEventWireup="true"
    CodeBehind="periodical.aspx.cs" Inherits="ETS2.WebApp.UI.periodical" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">


        $(function () {
            var promotetypeid = $("#hid_promotetypeid").trimVal();
            var hidture = $("#hid_ture").val();
            var comid = $("#hid_comid").val();

            $("#cancel").click(function () {
                //$("#show").hide();
                $("#show").css("display", "none");
            })


            //动态获取全部微信素材类型
            $.post("/jsonfactory/WeiXinHandler.ashx?oper=GetAllWxMaterialType", { comid: comid }, function (data) {

                data = eval("(" + data + ")");

                if (data.type == 1) {
                    $.prompt("操作出现错误" + data.msg);
                    return;
                }
                if (data.type == 100) {

                    if (data.totalCount > 0) {
                        $("#tdgroups").html("");

                        var groupstr = "微信素材类型：";
                        for (var i = 0; i < data.totalCount; i++) {
                            if (data.msg[i].Id == promotetypeid) {
                                groupstr += '<label><input name="radpromotetype"  type="radio" value="' + data.msg[i].Id + '" checked="checked">' + data.msg[i].TypeName + '</label>';

                            } else {
                                groupstr += '<label><input name="radpromotetype"  type="radio" value="' + data.msg[i].Id + '">' + data.msg[i].TypeName + '</label>';
                            }
                        }
                        $("#tdgroups").html(groupstr);
                    }

                }
            });



            $('input[name="radpromotetype"]').live("click", function () {
                var issuetype = $('input:radio[name="radpromotetype"]:checked').trimVal();
                window.open("periodical.aspx?promotetypeid=" + issuetype, target = "_self");
            });



            var pagee = 1;
            var pageSize = 10; //每页显示条数
            SearchList(pagee, promotetypeid);



            function SearchList(pageindex, promotetypeid) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/WeiXinHandler.ashx?oper=periodicallist",
                    data: { comid: $("#hid_comid").trimVal(), pageindex: pageindex, pagesize: pageSize, applystate: 10, promotetypeid: promotetypeid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询微信'期'列表错误");
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


        var pageSize = 10; //每页显示条数
        function perclock(pageindex, promotetypeid, type) {

            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }

            $.post("/JsonFactory/WeiXinHandler.ashx?oper=periodicaltypelist", { comid: $("#hid_comid").trimVal(), pageindex: pageindex, pagesize: pageSize, applystate: 10, promotetypeid: promotetypeid, type: type }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("查询微信'期'列表错误");
                    return;
                }
                if (data.type == 100) {

                    $("#show").show();

                    $("#tblist1").empty();
                    $("#divPage1").empty();
                    if (data.totalCount == 0) {
                        //                        $("#tblist1").html("<tr><td height=\"26\" colspan=\"7\">查询数据为空</td></tr>");
                    } else {
                        $("#ProductItemEdit1").tmpl(data.msg).appendTo("#tblist1");
                        setpage1(data.totalCount, pageSize, pageindex);
                    }


                }
            })
        }

        //分页
        function setpage1(newcount, newpagesize, curpage) {
            $("#divPage1").paginate({
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

                    perclock(page);

                    return false;
                }
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="materiallist.aspx" onfocus="this.blur()"><span>文章列表</span></a></li>
                <li class="on"><a href="periodical.aspx" onfocus="this.blur()"><span>文章期号管理</span></a></li>
                <li><a href="AuthorFocus.aspx" onfocus="this.blur()"><span>关注作者管理</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3 id="tdgroups">
                </h3>
                <h3>
                    <label>
                        '期'列表</label></h3>
                <table border="0" width="780" class="mail-list-title">
                    <tr>
                        <td width="6%" align="center" bgcolor="#CCCCCC">
                            ID
                        </td>
                        <td width="10%" align="center" bgcolor="#CCCCCC">
                            Comid
                        </td>
                        <td width="10%" height="26" align="left" bgcolor="#CCCCCC">
                            <p align="left">
                                期号
                            </p>
                        </td>
                        <td width="5%" height="26" align="left" bgcolor="#CCCCCC">
                            <p align="center">
                                年
                            </p>
                        </td>
                        <td width="25%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                创建时间
                            </p>
                        </td>
                        <td width="25%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                备注
                            </p>
                        </td>
                        <td width="8%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                管理
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
                        <td width="10%" class="sender item">
                            <p align="center">
                                ${Comid}</p>
                        </td>
                        <td width="10%" height="26" class="sender item">
                            <p align="left">
                                第${Percal}期
                            </p>
                        </td>
                        <td width="5%" height="26" class="sender item">
                            <p align="center">
                                ${Peryear}
                            </p>
                        </td>
                        <td width="15%" height="26" class="sender item">
                            <p align="center">
                               ${ChangeDateFormat(Uptime)}  </p>
                        </td>
                      
                     
                        <td width="25%" height="26" class="sender item">
                            <p align="center">
                                ${Perinfo}</p>
                        </td>
                        <td width="8%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                <a href="#" onclick="perclock('1','${Id}','${Wxsaletypeid}')">查看</a>
                            </p>
                        </td>
                    </tr>
    </script>
    <div id="show" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 780px; height: 400px; display: none; left: 10%; position: absolute; top: 10%;">
        <div class="inner">
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
                            作者
                        </p>
                    </td>
                    <td width="25%" height="26" bgcolor="#CCCCCC">
                        <p align="center">
                            封面
                        </p>
                    </td>
                    <td width="25%" height="26" bgcolor="#CCCCCC">
                        <p align="center">
                            关键词
                        </p>
                    </td>
                    <td width="8%" height="26" bgcolor="#CCCCCC">
                        <p align="center">
                            使用状态
                        </p>
                    </td>
                    <td width="8%" height="26" bgcolor="#CCCCCC">
                        <p align="center">
                            期号
                        </p>
                    </td>
                    <td width="22%" height="26" bgcolor="#CCCCCC">
                        <p align="center">
                            管 理
                        </p>
                    </td>
                </tr>
                <tbody id="tblist1">
                </tbody>
            </table>
            <div id="divPage1">
            </div>
        </div>
        <input name="cancel" id="cancel" type="button" value=" 返  回 " style="width: 70px;
            height: 25px; color: #5984bb" />
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit1">   
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
                                ${Author}
                            </p>
                        </td>
                        <td width="15%" height="26" class="sender item">
                            <p align="center">
                               <img alt=""   id="headPortraitImg" src="${Imgpath}"  width="50px"  height="30px"/>  </p>
                        </td>
                      
                     
                        <td width="25%" height="26" class="sender item">
                            <p align="center">
                                ${Keyword}</p>
                        </td>
                          <td width="4%" height="26" class="sender item">
                            <p align="center">
                                ${Applystate}</p>
                        </td>
                        <td width="8%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                第${Periodical}期
                            </p>
                        </td>
                        <td width="30%" height="26" class="sender item">
                         <p align="center">
                           <a href="materialdetail.aspx?materialid=${MaterialId}" >编辑</a>
                           </p>
                        </td>
                    </tr>
    </script>
    <input type="hidden" id="hid_promotetypeid" value="<%=promotetypeid %>" />
    <input type="hidden" id="hid_ture" value="0" />
</asp:Content>
