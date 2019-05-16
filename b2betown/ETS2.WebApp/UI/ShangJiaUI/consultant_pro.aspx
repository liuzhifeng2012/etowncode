<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="consultant_pro.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.consultant_pro" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
        <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Deletebanner(id) {
            var comid = $("#hid_comid").trimVal();
            $.post("/JsonFactory/DirectSellHandler.ashx?oper=deleteConsultant", { id: id, comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("操作出现错误");
                    return;
                }
                if (data.type == 100) {
                    $.prompt("删除成功");
                    location.href = "consultant_pro.aspx";
                    return;
                }
            })
        }


        $(function () {
            var pageSize = 100; //每页显示条数
            var comid = $("#hid_comid").trimVal();


            SearchList(1);

            //装载栏目
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/DirectSellHandler.ashx?oper=getconsultantlist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {

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

            $("#viewsite").click(function () {

                var h = 680;
                var w = 430;
                var t = screen.availHeight / 2 - h / 2;
                var l = screen.availWidth / 2 - w / 2;
                var prop = "dialogHeight:" + h + "px; dialogWidth:" + w + "px; dialogLeft:" + l + "px; dialogTop:" + t + "px;toolbar:no; menubar:no; scrollbars:yes; resizable:no;location:no;status:no;help:no";
                var path = "http://shop" + comid + ".etown.cn/h5/";
                var ret = window.showModalDialog(path, "", prop);

            })



        })


       
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/shangjiaui/ShopManage.aspx" onfocus="this.blur()" target="">微商城设置</a></li>
                <li class="on"><a href="/ui/shangjiaui/consultant_pro.aspx" onfocus="this.blur()" target="">顾问页面设置</a></li>
                <li><a href="/ui/shangjiaui/StoreList.aspx" onfocus="this.blur()" target="">门店模板设置</a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                
                <table >
                  <tr>
                        <td>
                           <div id="oldset" style="display:none;float:left">
                                <input type="button" name="viewsite" id="viewsite" value="  预览微网站 " />
                           </div>
                        </td>
                    </tr>
                </table>

                <h3>
                员工页栏目管理  </h3>  <span><a href="consultant_pro_Manage.aspx" class="a_anniu">添加栏目</a></span>  <span> 因手机屏幕宽度限制，请添加3-4个栏目</span>
                <table width="780" border="0">
                    <tr>
                        <td width="20">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="100">
                            <p align="left">
                                名称
                            </p>
                        </td>
                        <td width="120">
                            <p align="left">
                                显示产品来源
                            </p>
                        </td>
                        <td width="120">
                            <p align="left">
                                显示产品分类
                            </p>
                        </td>
                        <td width="130" > 
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
                        <td>
                            <p align="left">
                                ${Name}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                {{if Outdata ==0}}
                                    内部产品
                                {{/if}}

                                {{if Outdata ==2}}
                                      微信文章
                                {{/if}}

                                {{if Outdata ==1}}
                                      外部数据 
                                {{/if}}
                                
                                {{if Outdata ==3}}
                                      顾问空间 
                                {{/if}} 

                                {{if Outdata ==4}}
                                      顾问咨询
                                {{/if}} 
                                {{if Outdata ==5}}
                                      服务评价
                                {{/if}}
                                
                                </p>
                        </td>
                        <td>
                            <p align="left">
                            {{if Outdata ==0}}
                               {{if Projectlist != null}}
                                ${Projectlist.Projectname}
                                {{/if}}
                             {{/if}}
                                </p>
                        </td>

                        <td>
                            <p align="left">
                           <div style=" float:left;"><a href="consultant_pro_manage.aspx?id=${Id}"  class="a_anniu"> 管理 </a></div><div style=" float:left;padding-left:10px;">  <input type="button"  name="deletebanner" id="deletebanner" onclick="Deletebanner('${Id}')" value="  删除此栏目  " /></div>
                            </p>
 
                        </td>
                    </tr>
    </script>

</asp:Content>
