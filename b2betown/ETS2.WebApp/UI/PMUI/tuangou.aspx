<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/UI/Etown.Master"  CodeBehind="tuangou.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.tuangou" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/tuangou.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/layer/layer.js" type="text/javascript"></script> 
    <script src="../../Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.tmpl.min.js" type="text/javascript"></script> 
    <script src="../../Scripts/tuangou.js" type="text/javascript"></script>
</asp:Content>    
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
  <div class="tuangou">
       <h3>美团上架产品列表(*暂时只是支持 商家自家产品 的 单规格 票务产品)</h3> 
       <div class="tuangoulist">
           <div class="header">
             <input type="text"  id="keytxt"  placeholder="产品名称或产品编号搜索"/>
             <input type="button" id="searchbtn" value="查询"/>
             <input type="button" id="addbtn" value="上架新产品" />
           </div>
           <div class="content">  
             <theader>
                 <div class="row active">
                      <div class="col col-10">
                              <div>产品编号 </div>
                      </div>
                      <div class="col col-33">
                           <div>产品名称 </div>    
                      </div>
                      <div class="col col-33">
                           <div>售卖分销 </div>    
                      </div>
                      <div class="col">
                            <div>操作 </div>   
                      </div>
                </div> 
             </theader>
             <thbody>
                  
             </thbody>
            
             <div  id="divPage"  class="pagediv" >
                  
             </div>
            
           </div>
     </div>
</div>  


    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
        <div class="row">
                      <div class="col col-10">
                              <div>${proid} </div>
                      </div>
                      <div class="col col-33">
                                <div>${proname} </div>
                      </div>
                      <div class="col col-33">
                                <div>${stockagentcompanyname} </div>
                      </div>
                      <div class="col">
                             <input type="button" value="下架产品" onclick="downstockpro(this,${proid},${stockagentcompanyid})" />  
                            <span></span>
                      </div>
         </div>
    </script>

    <div  class="mtk"   id="objmodal">
        <div class="head modal-header">
            <h4 class="modal-title" >
                 上架新产品
            </h4>
        </div> 
  
        <div class="body modal-body">

            <div class="list-group pre-scrollable objlistgroup">
                     <section class="section1">
                         <section-header class="section-header1">
                             <h2>选择售卖分销</h2>
                         </section-header>
                         <section-body>
                            <%-- <div class="user_list_item">
                                  <div class="radio">
                                        <label>
                                          <input type="radio" name="radagent"  value="" checked><span>狼牙山美团分销</span>
                                        </label>
                                  </div>
                             </div> --%>
                         </section-body> 
                     </section>
                     <section class="section2">
                         <section-header  class="section-header2">
                             <h2>选择上架产品</h2>
                         </section-header>
                         <section-body>
                               <%--<div class="user_list_item"> 
                                  <div class="checkbox"> 
                                    <label> 
                                      <input type="checkbox" value="" name="chkpro" checked><span> qqqq</span> 
                                    </label> 
                                  </div> 
                              </div>  --%>
                              
                         </section-body>  
                     </section> 
            </div>
        </div>
        <div class="modal-footer"> 
            <input type="button"  class="btn btn-default closePopup" value="关闭" />
            <input type="button"  class="btn btn-default submitobjbtn" value="上架" />
        </div>
    </div> 
    <div class="kout"></div>

</asp:Content>
