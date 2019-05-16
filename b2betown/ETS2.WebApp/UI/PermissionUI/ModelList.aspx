<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master"  CodeBehind="ModelList.aspx.cs" Inherits="ETS2.WebApp.UI.PermissionUI.ModelList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数

        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1);

            //装载模板
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ModelHandler.ashx?oper=modelpagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询模板列表错误");
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

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>

                <li><a href="MasterList.aspx" onfocus="this.blur()" target=""><span>人员管理</span></a></li>
                <li><a href="SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>
                <li><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                <li><a href="ticketlist.aspx" onfocus="this.blur()"><span>退票管理</span></a></li>
                <li class="on"><a href="ModelList.aspx" onfocus="this.blur()" target=""><span>模板管理</span></a></li>
                <li><a href="ProClass.aspx" onfocus="this.blur()" target=""><span>产品类目</span></a></li>
                
                
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    模板列表</h3>  <a href="ModelManage.aspx">添加新模板</a> <a href="Imglibrary.aspx">标签库管理</a> <a href="BgImglibrary.aspx">背景库管理</a>
                <table width="780" border="0">
                    <tr>
                        <td width="20">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="80">
                            <p align="left">
                                模板名称
                            </p>
                        </td>
                        <td width="80">
                            <p align="left">
                                图例
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                图标类型
                            </p>
                        </td>
                         <td width="50">
                            <p align="left">
                                Banner图片尺寸
                            </p>
                        </td>
                        <td width="100">
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
                                ${Title}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                               <img alt="" class="headPortraitImgSrc" id="Img1" src="${Smallimg}" /></p>
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                {{if Daohangimg==1}}图片图标{{else}}字体图标{{/if}}</p>
                        </td>
                                                <td>
                            <p align="left">
                                高:${Bgimage_h} 宽:${Bgimage_w}</p>
                        </td>
                        <td>
                            <p align="left">
                         

                         <a href="ImglibraryManage.aspx?usetype=1&modelid=${Id}">上传背景</a> &nbsp;  
                         <a href="BgImglibrary.aspx?modelid=${Id}">已传背景图</a> &nbsp;  
                         <a href="ModelMenuList.aspx?modelid=${Id}">栏目管理</a>  &nbsp;  
                         {{if Daohangimg==1}}<a href="ImglibraryManage.aspx?usetype=0&modelid=${Id}">上传标签</a>{{/if}}
                         <a href="ModelManage.aspx?modelid=${Id}">基本信息管理 </a>  &nbsp;
                            </p>
 
                        </td>
                    </tr>
    </script>

</asp:Content>
