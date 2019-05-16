<%@ Page Title="" Language="C#" MasterPageFile="~/H5/Hotel/hotel.Master" AutoEventWireup="true" CodeBehind="HotelIntroduction.aspx.cs" Inherits="ETS2.WebApp.H5.Hotel.HotelIntroduction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../../Styles/Hotel2/base.2.0.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/Hotel2/hotel_explain_com.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <header class="header">
      <h1>酒店介绍</h1>
      <div class="left-head">
              <a id="goBack" href="javascript:history.go(-1);" class="tc_back head-btn"><span class="inset_shadow"><span class="head-return"></span></span></a>
      </div>
      <div class="right-head">
        <a href="" class="head-btn fn-hide"><span class="inset_shadow"><span class="head-home"></span></span></a>
      </div>
    </header>
    <div class="explainCenter">
		
			<%--<div class="detailInfo">
				<h1>设施与服务</h1>
				<table>
					<tbody><tr>
							<td class="gray">
								<span class="commonImg img01 grayImg"></span>
								无线上网公共区
							</td>
							<td class="gray">
								<span class="commonImg img02 grayImg"></span>
								宽带上网
							</td> 
					</tr>
					<tr>
							<td>
								<span class="commonImg img03 free"></span>
								停车场
							</td>
							<td class="gray">
								<span class="commonImg img04 grayImg"></span>
								接机服务
							</td>
					</tr>
					<tr>
							<td>
								<span class="commonImg img05"></span>
								餐厅
							</td>
							<td>
								<span class="commonImg img06"></span>
								会议室
							</td>
					</tr>
					<tr>
							<td class="gray">
								<span class="commonImg img07 grayImg"></span>
								游泳池
							</td>
							<td class="gray">
								<span class="commonImg img08 grayImg"></span>
								健身房
							</td>
					</tr>
				</tbody></table>
			</div>--%>
		<div class="detailInfo">
				<h1>酒店介绍</h1>
				<div class="pd10 lh22 fn-clear">
					<span class="intro">
                    <p><%=article%></p></br><p><%=serviceinfo%></p>
                  </div>  
				<div class="intro fn-hide"></div>                        
		</div>

        <div class="detailInfo">
			<p>
				<span class="title">公交路线</span></p>
				<dl>
					<p><%=Scenic_Takebus%></p>
				</dl>
		</div>

        <div class="gray_back pd10 topInfo">
			<p>
				<span class="title">开车路线</span></p>
				<dl>
					<p><%=Scenic_Drivingcar%></p>
				</dl>
		</div>

		<a class="sbtn" href="FangxingChoice.aspx">查看酒店房型</a>
	</div>
</asp:Content>
