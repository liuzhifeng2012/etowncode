<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="PromotionManage.aspx.cs" Inherits="ETS2.WebApp.UI.MemberUI.PromotionManage" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {


        });
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="PromotionList.aspx" onfocus="this.blur()"><span>优惠券管理</span></a></li>
                <li class="on"><a href="PromotionEdit.aspx" onfocus="this.blur()"><span>添加优惠券</span></a></li>
            </ul>
        </div>--%>
        
        <div id="setting-home" class="vis-zone">
            <div class="inner">
            <% if (IsParentCompanyUser)
               { %>
           <div class="title-area fn-clear title-area-scend">
                <h2>线上积分券</h2>
                <p>在线赠送，在线使用,方便组织活动</p>
           </div>
           <div class="fn-clear content-list">
				<ul>
                    	<li>
                            <a smartracker="on" seed="contentList-mainLinkbox" class="main-linkbox" href="PromotionEdit.aspx?acttype=4"></a>
                        	<div class="content-list-app">
                            	<b class="content-list-add"><a smartracker="on" seed="contentListAdd-mainMenu" class="main-menu" href="PromotionEdit.aspx?acttype=4">+</a></b>
                            	<h5 class="text-overflow">积分券</h5>
                                <p class="text-overflow">积分打入用户的账户中(在线使用)</p>
                            </div>
                            <div class="content-list-des text-overflow"></div>
                        </li>
               </ul>
          </div>
                <%} %>        

          <div class="title-area fn-clear title-area-scend">
                <h2>线下优惠券</h2>
                <p>赠送各种抵扣券,折扣券，线上宣传带动线下消费</p>
           </div>

           <div class="fn-clear content-list">
				<ul>
                    	<li>
                            <a smartracker="on" seed="contentList-mainLinkbox" class="main-linkbox" href="PromotionEdit.aspx?acttype=1"></a>
                        	<div class="content-list-app">
                            	<b class="content-list-add"><a smartracker="on" seed="contentListAdd-mainMenu" class="main-menu" href="PromotionEdit.aspx?acttype=1">+</a></b>
                            	<h5 class="text-overflow">消费抵扣券</h5>
                                <p class="text-overflow">赠送一张消费抵扣券，消费时验证抵扣(不可在线使用)</p>
                            </div>
                            <div class="content-list-des text-overflow"></div>
                        </li>
                        <li>
							<a smartracker="on" seed="contentList-mainLinkboxT1" class="main-linkbox" href="PromotionEdit.aspx?acttype=2"></a>
                        	<div class="content-list-app">
                            	<b class="content-list-add"><a smartracker="on" seed="contentListAdd-mainMenuT1" class="main-menu" href="PromotionEdit.aspx?acttype=2">+</a></b>
                            	<h5>消费折扣券</h5>
                                <p>赠送一张优惠折扣券，消费时验证抵扣(不可在线使用)</p>
                            </div>
                            <div class="content-list-des"></div>
                        </li>
                        <li>
							<a smartracker="on" seed="contentList-mainLinkboxT2" class="main-linkbox" href="PromotionEdit.aspx?acttype=3"></a>
                        	<div class="content-list-app">
                            	<b class="content-list-add"><a smartracker="on" seed="contentListAdd-mainMenuT2" class="main-menu" href="PromotionEdit.aspx?acttype=3">+</a></b>
                            	<h5>消费满就送</h5>
                                <p>赠送一张消费满就送优惠券，消费时验证抵扣(不可在线使用)</p>
                            </div>
                            <div class="content-list-des"></div>
                        </li>
               </ul>
          </div>

                    &nbsp;

            </div>
        </div>

    </div>
    <div class="data">
    </div>

</asp:Content>
