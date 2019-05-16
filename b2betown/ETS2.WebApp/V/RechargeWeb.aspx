<%@ Page Title="" Language="C#" MasterPageFile="~/V/Member.Master" AutoEventWireup="true" CodeBehind="RechargeWeb.aspx.cs" Inherits="ETS2.WebApp.V.RechargeWeb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/Recharge/Recss.css" rel="stylesheet" type="text/css" />
 <style type="text/css">
   #brg{ width:100%; height:100%; background:#333; position:absolute; top:0; left:0; filter:alpha(opacity=60); -moz-opacity:0.6; opacity: 0.6; position:absolute; top:0; left:0; display:none;}
   #showdiv{ width:100%; height:auto; position:absolute; left:0; top:50px; z-index:20; display:none; }
   #testdiv{ width:283px; height:317px; margin:0 auto; border:1px solid #4d4d4d; background:#f2f2f2;}
   #close{ margin-left:242px; float:left; width:40px; height:27px; line-height:27px; font-size:14px; font-weight:bold; border:1px solid #4d4d4d; text-align:center; cursor:pointer; background:#333; color:#fff; z-index:9999; position:absolute}
   .btn{ margin:20px auto auto 20px; width:200px; height:27px; line-height:27px; font-size:14px; font-weight:bold; border:1px solid #4d4d4d; text-align:center; cursor:pointer;}
 </style>
  <script type="text/javascript">
      $(function () {
          var comid = $("#hid_comid").trimVal();
          var userid = $("#hid_userid").trimVal();

          //$("#c_input").attr("disabled", "disabled");

          $("#price").click(function () {
              $(".jmp_info").hide();
              //$("#c_input").removeAttr("disabled");
          })

          $("#c_input").click(function () {
              //$(".jmp_info").hide();
              $('input[name="price"][value=0]').attr("checked", true);
              $(".c_price_big").html($("#c_input").val());
              //$("#c_input").attr("disabled", "disabled");
          })
          $("#c_input").blur(function () {
              $('input[name="price"][value=0]').attr("checked", true);
              $(".c_price_big").html($("#c_input").val());
              var c_input = $("#c_input").val();

              if (c_input == "" || parseInt(c_input) == 0) {
                  $(".fc01").show();
                  return;
              }
              else {
                  $(".fc01").hide();
                  return;
              }
          });

          $("#close").click(function () {
              $("#brg").css("display", "none");
              $("#showdiv").css("display", "none");
          });

          $("#BuEntity").click(function () {
              if (userid == 0 || userid == "") {
                  $("#brg").css("display", "block");
                  $("#showdiv").css("display", "block");
                  return;
              }
              var price = $(".c_price_big").html();
              if (parseInt(price) == 0 || price == "") {
                  $(".jmp_info").show();
                  return;
              }
              var radioname = $("input:radio[name='price'][checked='checked']").val(); //$('input:radio[name="price"]:checked').val();
              var c_input = $("#c_input").val();
              if (radioname == 0 && c_input == "" || parseInt(c_input) == 0) {
                  $(".fc01").show();
                  return;
              }
              $.post("/JsonFactory/OrderHandler.ashx?oper=Recharge", { comid: comid, userid: userid, price: price }, function (data) {
                  data = eval("(" + data + ")");
                  if (data.type == 1) {
                      $(".jmp_info").show();
                      $(".jmp_cn").html("充值 Error");
                      return;
                  }
                  if (data.type == 100) {
                      location.href = "/UI/VASUI/Pay.aspx?orderid=" + data.msg + " &comid=" + comid;
                      return;
                  }

              })
          })
      })

      function getRadio() {
          var price = $('input:radio[name="price"]:checked').val();

          $("#c_input").removeAttr("disabled");

          $(".c_price_big").html($('input:radio[name="price"]:checked').val());
      }
      function copy() {
          //document.all["c_price_big"].value = document.all["ob_text_1"].value;
          $(".c_price_big").ajaxSuccess($("#c_input").val());
      }
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="base_main">
    <h2 class="">
                <span id="entitycard" class="current">会员充值</span>
            </h2>
            <div id="diventitycard" class="tabcn entity_card">
                <p class="h2_tit">
                    </p>
                <div class="card_cn">
                    <div class="card_category">
                    </div>
                    <ul class="card_list">
                        <li>
                            <div class="list_lt">
                                <span class="c_price">500元</span>
                            </div>
                            <div class="list_rt ">
                                <input name="price" type="radio" onclick="getRadio()" value="500" />
                            </div>
                        </li>
                        <li>
                            <div class="list_lt">
                                <span class="c_price">1000元</span>
                                <input type="text" value="1000" style="display:none;">
                            </div>
                            <div class="list_rt " price="1000">
                                <input name="price" type="radio" onclick="getRadio()" value="1000" />
                            </div>
                        </li>
                        <li>
                            <div class="list_lt">
                                <span class="c_price">2000元</span>
                                <input type="text" value="2000" style="display:none;">
                            </div>
                            <div class="list_rt " price="2000">
                                <input name="price" type="radio" onclick="getRadio()" value="2000" />
                            </div>
                        </li>
                        <li>
                            <div class="list_lt">
                                <span class="c_price">5000元</span>
                                <input type="text" value="5000" style="display:none;">
                            </div>
                            <div class="list_rt " price="5000">
                                <input name="price" type="radio" onclick="getRadio()" value="5000" />
                            </div>
                        </li>
                        <li>
                            <div class="list_lt">
                                <input type="text" id="c_input" placeholder="请输入金额"/>元
                                <span class="fc01" style="display:none;">(1~5000以内的整数)</span>
                            </div>
                            <div class="list_rt " price="0">
                                <input name="price" type="radio" id="price" value="0" />
                            </div>
                        </li>
                        
                        <li>
                            <div class="list_lt price_box">
                                <strong class="total">合计：</strong><span class="c_price_big" id="c_price_big" onkeyup="document.getElementById('c_input').value=this.value" onblur="document.getElementById('c_input').value=this.value">0</span>元
                                <span class="jmp_info" style="display: none;">
                                    <b></b><i></i><div class="jmp_cn">请确认购买金额及数量</div>
                                </span>
                            </div>
                            <div class="list_rt">
                                <input id="BuEntity" class="btn01" type="button" value="立即充值" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            
</div>
         <div id="brg"></div>
         <div id="showdiv">
           <div id="testdiv">
               <div id="close">关闭</div>
                <iframe style="" class="fn-hide" allowtransparency="true" src="/v/loginF.aspx?come=<%=comeurl %>" width="286" height="317" id="Iframe1" frameborder="0" scrolling="no" coor="aside-login" style="display: block;"></iframe>

           </div>
         </div>
</asp:Content>
