<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="periodlist.aspx.cs" Inherits="ETS2.WebApp.M.periodlist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title></title>
<meta charset="UTF-8">
<meta name="HandheldFriendly" content="True">
<meta name="viewport" content="initial-scale=1, maximum-scale=1, width=device-width, user-scalable=no">
    <link href="../Styles/weixin/trivel.css" rel="stylesheet" type="text/css" />

<meta name="apple-mobile-web-app-title" content="Travelzoo" />
<meta name="apple-mobile-web-app-capable" content="yes" />
        <style type="text/css">
            .headerText{padding-top:12px}
            .titlebar{padding:4px 10px 4px;border-bottom:0 none}
        </style>

        <script type="text/javascript">
            //主函数
            function DX(n) {
                if (!/^(0|[1-9]\d*)(\.\d+)?$/.test(n))
                    return "数据非法";
                var unit = "千百拾亿千百拾万千百拾元角分", str = "";
                n += "00";
                var p = n.indexOf('.');
                if (p >= 0)
                    n = n.substring(0, p) + n.substr(p + 1, 2);
                unit = unit.substr(unit.length - n.length);
                for (var i = 0; i < n.length; i++)
                    str += '零壹贰叁肆伍陆柒捌玖'.charAt(n.charAt(i)) + unit.charAt(i);
                return str.replace(/零(千|百|拾|角)/g, "零").replace(/(零)+/g, "零").replace(/零(万|亿|元)/g, "$1").replace(/(亿)万|壹(拾)/g, "$1$2").replace(/^元零?|零分/g, "").replace(/元$/g, "元整");
            }
  </script>
  </head>
  <body>
  <div id="shell">
	    
        
        <div id="body" class="row">
            <div class="ContentFixedheight">
                
                
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="dealgrid">
                    <tr data-taid="DivTop20DealsTitle">
                    <td style="padding-top:6px"><div style="font-weight:bold;font-size:14px">&nbsp;&nbsp;&nbsp; <%=promotetype%> </div>
                                </td>
                    </tr>
                </table>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="dealgrid">
                    
                         <asp:Repeater ID="Repeater1" runat="server">
                           <ItemTemplate>
                              <tr data-taid="LinkTop20Deal0" style="cursor: pointer;" class="clickable" onclick="location.href='period.aspx?id=<%# Eval("id")%>&type=<%# Eval("Wxsaletypeid1")%>';return false;">
                                <td class="dealHeadline">
                                    <a href="period.aspx?id=<%# Eval("id")%>&type=<%# Eval("Wxsaletypeid1")%>"> <%# Eval("peryear")%>  第 <%# Eval("percal")%> 期</a>
                                    <div class="dealProvider"><a href="period.aspx?id=<%# Eval("id")%>&type=<%# Eval("Wxsaletypeid1")%>"></a></div>
                                </td>
                                <td width="9%" style="padding-right:10px;text-align:right"><img src="../Images/arrow@2x.png" alt=">" height="10" width="7" /></td>
                            </tr>
                           </ItemTemplate>
                         </asp:Repeater>

                </table>
            </div>
        <div class="footer">

   <p class="ftgrylite">&copy; 版权所有 不得转载 - </p>
</div>
	  </div>
  
    </div>
    
  </body>
</html>
