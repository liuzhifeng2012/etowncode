<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Etown.Master" AutoEventWireup="true"
    CodeBehind="notelist.aspx.cs" Inherits="ETS2.WebApp.UI.WebForm4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数

        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1);

            //列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AccountInfo.ashx?oper=notelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询短信列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#NoteItemEdit").tmpl(data.msg).appendTo("#tblist");
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

                function del(id, key) {
                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/AccountInfo.ashx?oper=delnote",
                        data: { id: id, key: key },
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");

                            if (data.type == 1) {
                                $.prompt("删除短信错误");
                                return;
                            }
                            if (data.type == 100) {
                                $("#tblist").empty();
                                $("#divPage").empty();
                                if (data.totalCount == 0) {
                                    $("#tblist").html("查询数据为空");
                                } else {
                                    $("#NoteItemEdit").tmpl(data.msg).appendTo("#tblist");
                                    setpage(data.totalCount, pageSize, pageindex);
                                }
                            }
                        }
                    })
                }
            }
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <div id="secondary-tabs" class="navsetting ">
            <ul>
               
                <li class="on"><a href="notelist.aspx" onfocus="this.blur()" target=""><span>短信管理</span></a></li>
                
                <li ><a href="/ui/userui/bangdingpos.aspx" onfocus="this.blur()" target="">
                    <span>Pos绑定</span></a></li>
            </ul>
        </div> 
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    <a href="note.aspx" target="_blank">添加短信</a></h3>
                <div>
                </div>
                <table width="780" border="0">
                    <tr>
                        <td width="51" height="31">
                            ID
                        </td>
                        <td width="54">
                            key
                        </td>
                        <td width="150">
                            标题
                        </td>
                        <td width="48">
                            是否发送
                        </td>
                        <td width="148">
                            时间
                        </td>
                        <td width="148">
                            IP
                        </td>
                        <td width="103">
                            管理
                            <%-- <a href="#" onclick="del(${Id}, ${Sms_key})" > 删除 </a>  --%>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
                <br />
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="NoteItemEdit">   
                    <tr>
                        <td width="51" height="31" >
                            <p align="left">
                                ${Id}</p>                     </td>
                        <td width="54" >
                            <p align="left">
                                ${Sms_key}</p>                       </td>
                        <td width="150" >
                        <p align="left">
                                ${Title}</p></td>
                        <td width="48" >
                            <p align="left">
                                ${Openstate}</p>                        </td>
                        <td width="148" >
                            <p align="left">
                                ${ChangeDateFormat(Subdate)}</p>                         </td>
                        <td width="148" >
                            <p align="left">
                                ${Ip}</p>                        </td>
                        <td width="103" >
                            <a href="note.aspx?note_id=${Id}">修改</a>                     </td>
                    </tr>
    </script>
</asp:Content>
