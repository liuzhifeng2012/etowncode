<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="news_list.aspx.cs" MasterPageFile="/UI/Etown.Master"
    Inherits="ETS2.WebApp.WeiXin.masssend.news.news_list" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            SearchList(1, 20);
        })
        function SearchList(pageindex, pagesize) {
            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.ajax({
                type: "post",
                url: "/JsonFactory/WeiXinHandler.ashx?oper=wxqunfa_news_addrecordpagelist",
                data: { userid: $("#hid_userid").trimVal(), comid: $("#hid_comid").trimVal(), pageindex: pageindex, pagesize: pagesize, key: "" },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
//                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalCount == 0) {

                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalcount, pagesize, pageindex);
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

                    SearchList(page, newpagesize);

                    return false;
                }
            });
        }


    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/weixin/masssend/send.aspx" onfocus="this.blur()"><span>新建群发消息</span></a></li>
                <li><a href="/weixin/masssend/list.aspx" onfocus="this.blur()">已发送</a></li>
                <li class="on"><a href="news_list.aspx" onfocus="this.blur()">图文信息管理</a> </li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10; width: 960px;">
                    <h2 class="p-title-area">
                        图文消息列表(3天内)</h2>
                    <div class="mi-form-item" style="float: right;">
                       
                            <a href="news_edit.aspx?is_singlenews=1" onfocus="this.blur()" style="color: Blue;
                                text-decoration: underline;font-size:15px;">新建单图文信息</a> 
                                
                            <a href="news_edit_multi.aspx" onfocus="this.blur()" style="color: Blue;
                                text-decoration: underline;font-size:15px;">新建多图文信息</a> 
                    </div>
                    <div class="mi-form-item">
                        <table border="0" width="860" class="mail-list-title">
                            <tr>
                                <td width="6%" align="center" bgcolor="#CCCCCC">
                                    编号
                                </td>
                                <td width="6%" align="center" bgcolor="#CCCCCC">
                                    封面
                                </td>
                                <td width="13%" height="26" align="left" bgcolor="#CCCCCC">
                                    <p align="left">
                                        标题
                                    </p>
                                </td>
                                <td width="5%" height="26" align="left" bgcolor="#CCCCCC">
                                    <p align="center">
                                        作者
                                    </p>
                                </td>
                                <td width="10%" height="26" bgcolor="#CCCCCC">
                                    <p align="center">
                                        摘要
                                    </p>
                                </td>
                                <td width="5%" height="26" bgcolor="#CCCCCC">
                                    <p align="center">
                                        创建时间
                                    </p>
                                </td>
                                <td width="6%" height="26" bgcolor="#CCCCCC">
                                    <p align="center">
                                    </p>
                                </td>
                                <td width="14%" height="26" bgcolor="#CCCCCC">
                                    <p align="center">
                                    </p>
                                </td>
                            </tr>
                            <tbody id="tblist">
                            </tbody>
                        </table>
                        <div id="divPage">
                        </div>
                        <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr style="height:66px;" >
                     <td class="sender item">
                            <p align="center">
                               ${Id} </p>
                        </td>
                        {{each NewsList}}

                        <td class="sender item">
                            <p align="center">
                           <img alt=""   id="headPortraitImg" src="${thumb_url}"  width="50px"  height="30px"/>    </p>
                        </td>
                        <td  height="26" class="sender item">
                            <p align="left">
                                {{if is_singlenews==1}}
                                 单图文
                                {{else}}
                                多图文
                                {{/if}}
                                -${title}
                            </p>
                        </td>
                        <td  height="26" class="sender item">
                            <p align="center">
                                ${author}
                            </p>
                        </td>
                        <td  height="26" class="sender item">
                            <p align="center">
                              ${digest}   </p>
                        </td>
                      
                     
                        
                        
                        {{/each}}
                          <td width="4%" height="26" class="sender item">
                            <p align="center">
                                ${jsonDateFormatKaler(CreateTime)}</p>
                        </td>
                        <td  height="26" class="sender item">
                         <p align="center">
                           <!--<a href="/weixin/masssend/send.aspx?newid=${Id}"  class="a_anniu">选中</a>-->   
                           </p>
                        </td>
                        <td  height="26" class="sender item">
                         <p align="center">
                           <a href="news_edit_multi.aspx?newsid=0&news_recordid=${Id}&opertype=1" class="a_anniu">编辑</a>
                         </p>
                        </td>
                    </tr>
                        </script>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
