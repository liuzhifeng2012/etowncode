<%@ Page Title="" Language="C#" MasterPageFile="~/V/Member.Master" AutoEventWireup="true" CodeBehind="about.aspx.cs" Inherits="ETS2.WebApp.V.about" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style>

        body ,article, section, h1, h2, hgroup, p, a, ul, li, em, div, small, span, footer, canvas, figure, figcaption, input
        {
            margin: 0;
            padding: 0;
        }
        a
        {
            text-decoration: none;
            cursor: pointer;
        }
        a.autotel
        {
            text-decoration: none;
            color: inherit;
        }
        
        .inner
        {


            padding: 10px 25px;
            margin: 0 auto;
        }
        h1
        {
            font-size: 22px;
            font-weight: normal;
            line-height: 26px;
            margin-bottom: 18px;
        }
        img
        {
            border: none;
            margin-bottom: 8px;
        }
        .old_message
        {
            line-height: 20px;
            text-indent: 2em;
            font-size: 14px;
            color: #565752;
            text-align: left;
            margin-bottom: 10px;
            word-wrap: break-word;
        }
        p
        {
            color: #565752;
            text-align: left;
            margin-bottom: 10px;
            word-wrap: break-word;
            line-height: 25px;
        }
        .phone
        {
        }
        .phone a
        {
            font-size: 16px;
            font-weight: bold;
            color: #fff;
            background: #fe932b;
            padding: 5px 10px;
            text-align: center;
            vertical-align: middle;
        }
        .tel
        {
            width: 134px;
            height: 33px;
            line-height: 33px;
            display: inline-block;
            text-align: center;
            font-size: 14px;
            border: 1px solid #4a4a4a;
            margin: 0 7px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            text-shadow: 0 1px #2daf35;
            color: #fff;
            box-shadow: inset 0 0 5px #8ee392;
            background-color: #29a832;
            background-image: -webkit-gradient(linear,0% 0,0% 100%,from(#4cbd51),to(#079414));
        }
        .yuding
        {
            width: 134px;
            height: 33px;
            line-height: 33px;
            display: inline-block;
            text-align: center;
            font-size: 14px;
            border: 1px solid #4a4a4a;
            margin: 0 7px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            text-shadow: 0 1px #2daf35;
            color: #fff;
            box-shadow: inset 0 0 5px #8ee392;
            background-color: #29a832;
            background-image: -webkit-gradient(linear,0% 0,0% 100%,from(#4cbd51),to(#079414));
        }
        #mcover
        {
            position: fixed;
            top: 0px;
            left: 0px;
            width: 100%;
            height: 100%;
            background: none repeat scroll 0% 0% rgba(0, 0, 0, 0.7);
            display: none;
            z-index: 20000;
        }
        #mcover img
        {
            position: fixed;
            right: 18px;
            top: 5px;
            width: 260px !important;
            height: 180px !important;
            z-index: 20001;
        }
        .text
        {
            margin: 15px 0px;
            font-size: 14px;
            word-wrap: break-word;
            color: #727272;
        }
        #mess_share
        {
            margin: 15px 0px;
            display: block;
        }
        #share_1
        {
            float: left;
            width: 49%;
            display: block;
        }
        .button2
        {
            font-size: 16px;
            padding: 8px 0px 0px 0px;
            border: 1px solid #ADADAB;
            color: #000;
            background-color: #E8E8E8;
            background-image: linear-gradient(to top, #DBDBDB, #F4F4F4);
            box-shadow: 0px 1px 1px rgba(0, 0, 0, 0.45), 0px 1px 1px #EFEFEF inset;
            text-shadow: 0.5px 0.5px 1px #FFF;
            text-align: center;
            border-radius: 3px;
            width: 100%;
            vertical-align: middle;
        }
        #share_2
        {
            float: right;
            width: 49%;
            display: block;
        }
        #mess_share img
        {
            width: 22px !important;
            height: 22px !important;
            vertical-align: top;
            border: 0px none;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
<div class="main">

	<div class="inner">
       <div style="margin-bottom:6px">
          <span style="font-size:14px"> <a href="/ui/shangjiaui/News.aspx"><%=wxtype %></a> </span><span style="font-size:20px">
            - <%=titile%> </span>
       </div>
        <p>
            <strong><span style="color: #ff0000;">
                <%=imgurl %></span>
			</strong>
		</p>
		<div class="row semiLargeBottomMargin">
               
            </div>
		<br>		
        <p><strong><span style="font-size: medium;"><%=titile%></span>
		</strong>
		</p>
        <p><%=contxt%></p>

         <%
            if (phone_tel != "")
            {
        %><br>
        <p>
            <%=phone %><a class="tel"><%=phone_tel %></a>
        </p>
        <%
            }
        %>
    </div>			
</div>
</asp:Content>
