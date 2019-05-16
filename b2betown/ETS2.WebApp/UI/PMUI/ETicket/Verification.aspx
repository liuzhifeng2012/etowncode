<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="Verification.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.ETicket.Verification" %>


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
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/V/VerCard.aspx" onfocus="this.blur()"><span>会员卡验证</span></a></li>
                <li><a href="/ui/pmui/eticket/eticketindex.aspx" onfocus="this.blur()"><span>电子凭证验证</span></a></li>
            </ul>
        </div>
        
        <div id="setting-home" class="vis-zone">
            <div class="inner">

           <div class="title-area fn-clear title-area-scend">
                <h2>后台验证</h2>
                <p>电子凭证验证，会员卡验证</p>
           </div>
           <div class="fn-clear content-list">
				<ul>
                    	<li>
                            <a smartracker="on" seed="contentList-mainLinkbox" class="main-linkbox" href="/V/VerCard.aspx"></a>
                        	<div class="content-list-app">
                            	<b class="content-list-add"><a smartracker="on" seed="contentListAdd-mainMenu" class="main-menu" href="/V/VerCard.aspx">+</a></b>
                            	<h5 class="text-overflow">会员卡验证</h5>
                                <p class="text-overflow">验证会员，使用优惠券</p>
                            </div>
                            <div class="content-list-des text-overflow"></div>
                        </li>
                        <li>
							<a smartracker="on" seed="contentList-mainLinkboxT1" class="main-linkbox" href="/ui/pmui/eticket/eticketindex.aspx"></a>
                        	<div class="content-list-app">
                            	<b class="content-list-add"><a smartracker="on" seed="contentListAdd-mainMenuT1" class="main-menu" href="/ui/pmui/eticket/eticketindex.aspx">+</a></b>
                            	<h5>电子凭证验证</h5>
                                <p>验证电子凭证</p>
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
