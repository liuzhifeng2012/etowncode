<%@ Page Title="" Language="C#" MasterPageFile="~/V/Member.Master" AutoEventWireup="true" CodeBehind="top10.aspx.cs" Inherits="ETS2.WebApp.V.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <style type="text/css">
       .top20disp-col2{ padding-left:20px;}
       .top20disp-col2 a{ border-bottom:1px solid #aaa;}
     .top20disp-col2 a:hover{ border:none;}
   </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
  <div class="main">
                   <div class="leftCol rightBodyColumn">
                <div class="row mediumTopMargin">
                    <div class="leftCol">
                        <h1>本期 Top 10 精选会员特惠</h1>
                        <span style="font-size:12px">发布日期2013年8月26日 -  VCTrip.com微旅行<sup></sup>
                        <br></span>
                        
                    </div>
                </div>
				
                <div class="row">
                    <div class="leftCol mediumTopMargin" style="width:100%">
                        
                        
                  <div class="hr gray">&nbsp;</div>
					<asp:Repeater ID="menu" runat="server" onitemdatabound="menu_ItemDataBound">
                        <ItemTemplate>
                           <dl>
                              <dt><h3><%#this.strsub(Eval("typename").ToString())%> &nbsp;&nbsp;&nbsp;<%# this.pernum(int.Parse(Eval("id").ToString()), comid)%></h3></dt>
                              <dd> 
                             <table cellspacing="0" cellpadding="0" class="top20disp-table" data-taid="Top20PageTop20MainTable">
                                 <asp:Repeater ID="Rplist" runat="server">
                                    <ItemTemplate>
                                       <tr class="top20disp-row row0; text-align:left;">
                                    <td class="top20disp-col1 tzred mediumPadding" style="width:20%;">
                                        <h2>￥<%#Eval("Price")%>起</h2>
                                    </td>
                                    <td class="top20disp-col2" width="500px">
                                        <a href="info.aspx?id=<%#Eval("MaterialId") %>" target="_blank" id="Deals__ctl1_lnkDeal" style="font-size:1.083em"><%#Eval("Title")%></a>
                                    </td>
                                </tr>
                                    </ItemTemplate>
                                 </asp:Repeater>
                               </table>
                              </dd>
                           </dl>
                        </ItemTemplate>
                    </asp:Repeater>
                        

                    </div>
                </div>
            </div>
       </div>

					
 



</asp:Content>
