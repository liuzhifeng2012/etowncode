<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/V/Member.Master" CodeBehind="News.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.News" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
       .top20disp-col2{ padding-left:20px;}
       .top20disp-col2 a{ border-bottom:1px solid #aaa;}
     .top20disp-col2 a:hover{ border:none;}
   </style>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
 
    <style type="text/css">
      input{ border:1px solid #5984bb}
    </style>

</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
 
 <div class="grid-780 grid-780-border fn-clear">
 
<div class="mod-bottom ">
            <div class="slideTxtBox">
                <div class="clearfix">
                </div>
                <div class="tempWrap" style="overflow: hidden; position: relative; width: 980px">

            <div class="bd" id="bd_4" style="display:;">
			   <div class="main">
                <div class="leftCol rightBodyColumn">
                 <div class="row mediumTopMargin">
                    <div class="leftCol">
                        <h1><%=Com_name %></h1>
                        <span style="font-size:12px">发布日期<%=Listtime%> -  <%=Com_name%><sup></sup>
                        <br/></span>
                        
                    </div>
                </div>
				
                <div class="row">
                    <div class="leftCol mediumTopMargin" style="width:100%">
                                <div class="hr gray">&nbsp;</div>
                                <%--<table cellspacing="0" cellpadding="0" class="top20disp-table" data-taid="Top20PageTop20MainTable">
                            
                                <tbody>
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
                            </tbody>
                               </table>--%>
                    </div>
                </div>
                <div>
                    <asp:Repeater ID="menu" runat="server" onitemdatabound="menu_ItemDataBound">
                        <ItemTemplate>
                           <dl>
                              <dt><h3><%#Eval("typename").ToString()%> &nbsp;&nbsp;&nbsp;<%# this.pernum(int.Parse(Eval("id").ToString()), comid)%></h3></dt>
                              <dd> 
                             <table cellspacing="0" cellpadding="0" class="top20disp-table" data-taid="Top20PageTop20MainTable">
                                 <asp:Repeater ID="Rplist" runat="server">
                                    <ItemTemplate>
                                       <tr class="top20disp-row row0; text-align:left;">
                                    <td class="top20disp-col1 tzred mediumPadding" style="width:20%;">
                                        <h2><%#Eval("Price").ToString() != "0.00"? "￥"+Eval("Price")+"起" : ""%></h2>
                                    </td>
                                    <td class="top20disp-col2" width="500px">
                                        <a href="/v/about.aspx?id=<%#Eval("MaterialId") %>" target="_blank" id="Deals__ctl1_lnkDeal" style="font-size:1.083em"><%#Eval("Title")%></a>
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



            
 
		  <%-- <div class="ui-form-item">
	        <div id="submitBtn" class="ui-button  ui-button-morange "></div>
	       </div>--%>
 </div>
  </div>
   </div>
    </div>
    <input id="channelid" type="hidden" value="<%=channelid %>" />
    <input id="comid" type="hidden" value="<%=comid %>" />

</asp:Content>
