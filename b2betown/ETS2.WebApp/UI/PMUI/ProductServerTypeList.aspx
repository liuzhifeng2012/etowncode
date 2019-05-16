<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ProductServerTypeList.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.ProductAdd2" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/pmui/projectlist.aspx" onfocus="this.blur()" target=""><span>项目管理</span></a></li>
                <li><a href="/ui/pmui/Projectedit.aspx" onfocus="this.blur()" target=""><span>添加项目</span></a></li>
                <li><a href="/ui/pmui/ProductList.aspx?projectid=" onfocus="this.blur()" target=""><span>
                    产品列表</span></a></li>
                <li class="on"><a href="/ui/pmui/ProductServerTypeList.aspx" onfocus="this.blur()"
                    target=""><span>添加产品</span></a></li>
                <li><a href="/ui/pmui/order/Salecount.aspx" onfocus="this.blur()" target="">产品统计</a></li>
                <li><a href="/ui/pmui/BindingAgent.aspx" onfocus="this.blur()" target="">导入分销系统产品</a></li>
                <li><a href="/ui/pmui/eticket_useset.aspx" onfocus="this.blur()" target=""><span>商户特定日期设定</span></a></li>
                  <li><a href="/ui/pmui/delivery/deliverylist.aspx" onfocus="this.blur()" target="">
                    <span>运费模版管理</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <div class="title-area fn-clear title-area-scend">
                    <h2>
                        电子票/景区门票</h2>
                </div>
                <div class="fn-clear content-list">
                    <ul>
                        <li><a smartracker="on" seed="contentList-mainLinkbox" class="main-linkbox" href="/ui/pmui/ProductAdd.aspx?servertype=1&projectid=<%=projectid %>">
                        </a>
                            <div class="content-list-app">
                                <b class="content-list-add"><a smartracker="on" seed="contentListAdd-mainMenu" class="main-menu"
                                    href="/ui/pmui/ProductAdd.aspx?servertype=1&projectid=<%=projectid %>">+</a></b>
                                <h5 class="text-overflow">
                                    电子凭证</h5>
                                <%--   <p class="text-overflow">
                                    </p>--%>
                            </div>
                            <div class="content-list-des text-overflow">
                            </div>
                        </li>
                    </ul>
                    <ul>
                        <li><a smartracker="on" seed="contentList-mainLinkbox" class="main-linkbox" href="/ui/pmui/ProductAdd.aspx?servertype=3&projectid=<%=projectid %>">
                        </a>
                            <div class="content-list-app">
                                <b class="content-list-add"><a smartracker="on" seed="contentListAdd-mainMenu" class="main-menu"
                                    href="/ui/pmui/ProductAdd.aspx?servertype=3&projectid=<%=projectid %>">+</a></b>
                                <h5 class="text-overflow">
                                    赠送产品</h5>
                                <%--   <p class="text-overflow">
                                    </p>--%>
                            </div>
                            <div class="content-list-des text-overflow">
                            </div>
                        </li>
                    </ul>
                    <ul>
                        <li><a smartracker="on" seed="contentList-mainLinkbox" class="main-linkbox" href="/ui/pmui/ProductAdd.aspx?servertype=12&projectid=<%=projectid %>">
                        </a>
                            <div class="content-list-app">
                                <b class="content-list-add"><a smartracker="on" seed="contentListAdd-mainMenu" class="main-menu"
                                    href="/ui/pmui/ProductAdd.aspx?servertype=12&projectid=<%=projectid %>">+</a></b>
                                <h5 class="text-overflow">
                                    预约产品</h5>
                                <%--   <p class="text-overflow">
                                    </p>--%>
                            </div>
                            <div class="content-list-des text-overflow">
                            </div>
                        </li>
                    </ul>
                    <%if (com_type == 1 || com_type == 2 || com_type == 4)
                      {%>
                    <ul>
                        <li><a smartracker="on" seed="contentList-mainLinkbox" class="main-linkbox" href="/ui/pmui/ProductAdd.aspx?servertype=10&projectid=<%=projectid %>">
                        </a>
                            <div class="content-list-app">
                                <b class="content-list-add"><a smartracker="on" seed="contentListAdd-mainMenu" class="main-menu"
                                    href="/ui/pmui/ProductAdd.aspx?servertype=10&projectid=<%=projectid %>">+</a></b>
                                <h5 class="text-overflow">
                                    旅游大巴</h5>
                            </div>
                            <div class="content-list-des text-overflow">
                            </div>
                        </li>
                    </ul>
                    <%} %>
                    <ul>
                        <li><a smartracker="on" seed="contentList-mainLinkbox" class="main-linkbox" href="/ui/pmui/ProductAdd.aspx?servertype=13&projectid=<%=projectid %>">
                        </a>
                            <div class="content-list-app">
                                <b class="content-list-add"><a smartracker="on" seed="contentListAdd-mainMenu" class="main-menu"
                                    href="/ui/pmui/ProductAdd.aspx?servertype=13&projectid=<%=projectid %>">+</a></b>
                                <h5 class="text-overflow">
                                    教练预约</h5>
                                <%--   <p class="text-overflow">
                                    </p>--%>
                            </div>
                            <div class="content-list-des text-overflow">
                            </div>
                        </li>
                    </ul>
                    <%if (comid == 106||comid==1532)
                      {%>
                    <ul>
                        <li><a smartracker="on" seed="contentList-mainLinkbox" class="main-linkbox" href="/ui/pmui/ProductAdd.aspx?servertype=14&projectid=<%=projectid %>">
                        </a>
                            <div class="content-list-app">
                                <b class="content-list-add"><a smartracker="on" seed="contentListAdd-mainMenu" class="main-menu"
                                    href="/ui/pmui/ProductAdd.aspx?servertype=14&projectid=<%=projectid %>">+</a></b>
                                <h5 class="text-overflow">
                                    保险产品</h5>
                                <%--   <p class="text-overflow">
                                    </p>--%>
                            </div>
                            <div class="content-list-des text-overflow">
                            </div>
                        </li>
                    </ul>
                    <%} %>

                    <ul style=" display:none;">
                        <li><a smartracker="on" seed="contentList-mainLinkbox" class="main-linkbox" href="/ui/pmui/ProductAdd.aspx?servertype=15&projectid=<%=projectid %>">
                        </a>
                            <div class="content-list-app">
                                <b class="content-list-add"><a smartracker="on" seed="contentListAdd-mainMenu" class="main-menu"
                                    href="/ui/pmui/ProductAdd.aspx?servertype=15&projectid=<%=projectid %>">+</a></b>
                                <h5 class="text-overflow">
                                    电子凭证套票</h5>
                                <%--   <p class="text-overflow">
                                    </p>--%>
                            </div>
                            <div class="content-list-des text-overflow">
                            </div>
                        </li>
                    </ul>

                </div>
                <%if (com_type == 1 || com_type == 2 || com_type == 4)
                  {%>
                <div class="title-area fn-clear title-area-scend">
                    <h2>
                        旅游线路/订房</h2>
                    <%--   <p>
                        客户微信关注公众号后赠送</p>--%>
                </div>
                <div class="fn-clear content-list">
                    <ul>
                        <li><a smartracker="on" seed="contentList-mainLinkboxT1" class="main-linkbox" href="/ui/pmui/ProductAdd.aspx?servertype=2&projectid=<%=projectid %>">
                        </a>
                            <div class="content-list-app">
                                <b class="content-list-add"><a smartracker="on" seed="contentListAdd-mainMenuT1"
                                    class="main-menu" href="/ui/pmui/ProductAdd.aspx?servertype=2&projectid=<%=projectid %>">
                                    +</a></b>
                                <h5>
                                    跟团游</h5>
                                <%-- <p>
                                    客户关注微信公众账户，赠送一张现金抵扣券(不可在线使用)</p>--%>
                            </div>
                            <div class="content-list-des">
                            </div>
                        </li>
                        <li><a smartracker="on" seed="contentList-mainLinkboxT2" class="main-linkbox" href="/ui/pmui/ProductAdd.aspx?servertype=8&projectid=<%=projectid %>">
                        </a>
                            <div class="content-list-app">
                                <b class="content-list-add"><a smartracker="on" seed="contentListAdd-mainMenuT2"
                                    class="main-menu" href="/ui/pmui/ProductAdd.aspx?servertype=8&projectid=<%=projectid %>">
                                    +</a></b>
                                <h5>
                                    当地游</h5>
                                <p>
                                </p>
                            </div>
                            <div class="content-list-des">
                            </div>
                        </li>
                        <li><a smartracker="on" seed="contentList-mainLinkboxT1" class="main-linkbox" href="/ui/pmui/ProductAdd.aspx?servertype=9&projectid=<%=projectid %>">
                        </a>
                            <div class="content-list-app">
                                <b class="content-list-add"><a smartracker="on" seed="contentListAdd-mainMenuT1"
                                    class="main-menu" href="/ui/pmui/ProductAdd.aspx?servertype=9&projectid=<%=projectid %>">
                                    +</a></b>
                                <h5>
                                    酒店客房</h5>
                                <%-- <p>赠送一张现金抵扣券(不可在线使用)</p>--%>
                            </div>
                            <div class="content-list-des">
                            </div>
                        </li>
                    </ul>
                </div>
                <%} %>
                <div class="title-area fn-clear title-area-scend">
                    <h2>
                        实物产品
                    </h2>
                </div>
                <div class="fn-clear content-list">
                    <ul>
                        <li><a smartracker="on" seed="contentList-mainLinkbox" class="main-linkbox" href="/ui/pmui/ProductAdd.aspx?servertype=11&projectid=<%=projectid %>">
                        </a>
                            <div class="content-list-app">
                                <b class="content-list-add"><a smartracker="on" seed="contentListAdd-mainMenu" class="main-menu"
                                    href="/ui/pmui/ProductAdd.aspx?servertype=11&projectid=<%=projectid %>">+</a></b>
                                <h5 class="text-overflow">
                                    实物产品</h5>
                            </div>
                            <div class="content-list-des text-overflow">
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
</asp:Content>
