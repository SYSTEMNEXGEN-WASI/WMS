<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" Theme="Theme1" AutoEventWireup="true" CodeBehind="VendorPaymentReceipt.aspx.cs" Inherits="DXBMS.Modules.Service.Forms.VendorPaymentReceipt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <script type="text/javascript" src="../../../Jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../../Jquery/MaxLength.min.js"></script>
    <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function AmtTot() {
            var TotalInsAmt = document.getElementById('<%=txtInstAmt.ClientID%>').value;
            var TotalAdvAmt = document.getElementById('<%=txtAdvAmount.ClientID%>').value;
            if (TotalInsAmt == "") {
                TotalInsAmt = 0;
            }
            if (TotalAdvAmt == "") {
                TotalAdvAmt = 0;
            }

            document.getElementById('<%=txttotAmount.ClientID %>').value = parseFloat( TotalAdvAmt) + parseFloat( TotalInsAmt);
            document.getElementById('<%=txtAdj.ClientID %>').value = 'f';
//            var totIns = document.getElementById('<%=txtInstAmt.ClientID%>').value;
//            document.getElementById('<%=txttotAmount.ClientID %>').value = totIns;
//            document.getElementById('<%=txtAdj.ClientID %>').value = 'f';
        }
        function CalcAdvAdjAmt() {
            var TotalInsAmt = document.getElementById('<%=txtInstAmt.ClientID%>').value;
            var TotalAdvAmt = document.getElementById('<%=txtAdvAmount.ClientID%>').value;
            if (TotalInsAmt == "") {
                TotalInsAmt = 0;
            }
            if (TotalAdvAmt == "") {
                TotalAdvAmt = 0;
            }

            document.getElementById('<%=txttotAmount.ClientID %>').value = TotalAdvAmt + TotalInsAmt;
        }
//        function CalcTax() {
//            
//            var TaxPercent = document.getElementById('<%=((TextBox)gvTaxDetail.FooterRow.FindControl("txtFooterTaxPerc")).ClientID %>').value;
//            var TotalInvAmount = document.getElementById('<%=txtSelectedTotalRefAmount.ClientID %>').value;
//            if (TaxPercent == "") TaxPercent = "0";
//            if (TotalInvAmount == "") TotalInvAmount = "0";


//            var TotalTax = (TotalInvAmount * TaxPercent) / 100;
//           document.getElementById('<%=((TextBox)gvTaxDetail.FooterRow.FindControl("txtFooterTaxAmt")).ClientID %>').value = parseFloat(TotalTax).toFixed(2).toString();

//       }
//       function CalcTaxPercOnAmount() {
//           
//           var TaxPercentAmt = document.getElementById('<%=((TextBox)gvTaxDetail.FooterRow.FindControl("txtFooterTaxAmt")).ClientID %>').value;
//           var TotalInvAmount = document.getElementById('<%=txtSelectedTotalRefAmount.ClientID %>').value;
//           if (TaxPercentAmt == "") TaxPercent = "0";
//           if (TotalInvAmount == "") TotalInvAmount = "0";


//           var TotalTax = (TaxPercentAmt / TotalInvAmount) * 100;
//           document.getElementById('<%=((TextBox)gvTaxDetail.FooterRow.FindControl("txtFooterTaxPerc")).ClientID %>').value = parseFloat(TotalTax).toFixed(2).toString();

//       }
        function MaxTxtLen() {
            $(function () {
                //Normal Configuration
                $("[id*=txtRemarks]").MaxLength({ MaxLength: 200 });
            });
        }
    </script>
    <style>
    .ddl{
    width:48%!important;
    }
     </style>

    <asp:UpdatePanel ID="UpdPanel1" runat="server">
        <ContentTemplate>
            <div class="container">
            <script type="text/javascript">
            Sys.Application.add_load(MaxTxtLen);
            </script>
            
            <div class="Heading">
                Dealer Payment Receipt
            </div>
           
          <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
              <%-- Row 1 --%>
               <tr>
                    <td >
                     Receipt No<asp:ImageButton runat="server" ID="imgReceiptNo" CausesValidation="false" ImageUrl="~/Images/LookupNew.png" OnClick="imgReceiptNo_Click" /><span  style="font-size:large; color:red;" >*</span>

                    </td>
                    <td>
                                    <asp:DropDownList ID="ddlReceptNo" runat="server" AutoPostBack="True" 
                                        OnSelectedIndexChanged="ddlReceptNo_SelectedIndexChanged" Width="100%">
                                    </asp:DropDownList>
                                </td>
                    <td >
                                        Receipt Date*

                             </td>
                    <td>
                                        <asp:TextBox ID="txtReceiptDate" runat="server" Enabled="false" 
                                            Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;" 
                                           Width="100%"></asp:TextBox>
                                        <asp:MaskedEditExtender ID="txtReceiptDate_MaskedEditExtender" runat="server" 
                                            ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" 
                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                            CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                                            Mask="99-99-9999" TargetControlID="txtReceiptDate" />
                                    </td>
                    <td>
               Transaction Type
               </td>       
                    <td >                                
                                            <asp:RadioButtonList ID="RBLTransType" runat="server" AutoPostBack="True" Font-Bold="true"
                                                OnSelectedIndexChanged="RBLTransType_SelectedIndexChanged" RepeatDirection="Horizontal" Visible="false"
                                                width="100%">
                                                <asp:ListItem Value="Advance">Advance</asp:ListItem>
                                                <asp:ListItem Value="Customer">Customer</asp:ListItem>
                                                <asp:ListItem Value="Principle">Principle</asp:ListItem>
                                                <%--<asp:ListItem Value="SaleReturn">Sale Return</asp:ListItem>--%>
                                            </asp:RadioButtonList>
                                            <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlTransType" OnSelectedIndexChanged="ddlTransType_SelectedIndexChanged">
                                             <%-- <asp:ListItem Value="Advance">Advance</asp:ListItem>
                                                <asp:ListItem Value="Customer">Customer</asp:ListItem>--%>
                                                <asp:ListItem Value="Principle">Principle</asp:ListItem>

                                            </asp:DropDownList>                                
                                        </td>
                   
               </tr>
              <%-- Row 2 --%>
               <tr>
               <td >
               Invoice Types 
                  </td>
               <td >
                <asp:DropDownList runat="server" ID="ddlPaymentReceiptType" OnSelectedIndexChanged="ddlPaymentReceiptType_SelectedIndexChanged">
               </asp:DropDownList>
              <%--  <asp:RadioButtonList ID="RBLPaymentReceiptType" runat="server" AutoPostBack="True" 
                   OnSelectedIndexChanged="RBLPaymentReceiptType_SelectedIndexChanged" 
                  RepeatDirection="Horizontal" Width="100%" Visible="false">
                  </asp:RadioButtonList>--%>
                   </td>
               <td >
                 Customer<asp:ImageButton runat="server" ID="imgCustomerCode" CausesValidation="false" ImageUrl="~/Images/LookupNew.png" OnClick="imgCustomerCode_Click" /><span  style="font-size:large; color:red;" >*</span>
                </td>
               <td>
                                        <asp:DropDownList ID="ddlCust" runat="server" AutoPostBack="True" Enabled="false"
                                            Font-Names="monospace" OnSelectedIndexChanged="ddlCust_SelectedIndexChanged" 
                                            Width="100%">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtCust" runat="server" Visible="false"></asp:TextBox>
                                    </td>
               <td>
               Document No
               </td>
               <td>
              <asp:TextBox runat="server" ID="txtDocNo" Width="100%"></asp:TextBox>
               </td>
              
             </tr>
              <%-- Row 3 --%>
               <tr>
                <td>
                        Remarks
                        </td>
                <td>
                                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="100" 
                                            TextMode="MultiLine" Width="100%"></asp:TextBox>
                                    </td>
                <td >
                 <asp:CheckBox ID="chkAdvance" runat="server" Text="Is Adv" AutoPostBack="True" 
                   oncheckedchanged="chkAdvance_CheckedChanged" />
                 </td>
                <td >
               <asp:DropDownList runat="server" ID="ddlAdvance" class="ddl" AutoPostBack="true"  
                      OnSelectedIndexChanged="ddlAdvance_OnSelectedIndexChanged">
                      </asp:DropDownList>
                      </td>   
                <td>
              Advance Amount
              </td>
                <td>
                       <asp:TextBox ID="txtAdvAmount" runat="server" Width="100%" MaxLength="6" onblur="AmtTot()" placeholder="Adv Amt"></asp:TextBox>
               </td>
                  </tr>
              <%-- Row 4 --%>
               <tr >
             <td>
                                        Total Amount

                                    </td>
            <td>
                                        <asp:TextBox ID="txttotAmount" runat="server" Enabled="False" Style="text-align: right" Visible="true"
                                            SkinID="ROTextBox" Width="100%"></asp:TextBox>
                                        <asp:TextBox ID="txtInvNo" runat="server" Visible="false" Width="10px"></asp:TextBox>
                                        <asp:HiddenField ID="txtAdj" runat="server" Value="f" />
                                    </td>
                <td colspan="4" align="right">
                                        <asp:ImageButton ID="ImgPendingInv" runat="server" Height="50px" 
                                            ImageUrl="~/Images/InvImgBtn.png" OnClick="ImgPendingInv_Click" 
                                            ToolTip="View Pending Invoice" Width="60px" /></td> 
             </tr>
              
              
              </table>
            
            <asp:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" Width="100%"
                        Style="direction: ltr; text-align: left;">
                        <%-- Tab Of Invoice List  --%>
            <asp:TabPanel class="TabPanel" ID="TabPanel1" runat="server" HeaderText="Sele Person Information">
          <HeaderTemplate>
           Invoice List of Selected Customer
           </HeaderTemplate>
           <ContentTemplate>
            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
            <tr>
            <td>
              <asp:TextBox ID="txttotAdj" runat="server" ReadOnly="True" SkinID="ROTextBox" 
                            Visible="False" Width="80px"></asp:TextBox>
             <asp:GridView ID="gvPendingInvoice" runat="server" AutoGenerateColumns="false" 
                            OnRowCreated="gvPendingInvoice_RowCreated" 
                            OnRowDataBound="gvPendingInvoice_RowDataBound" ShowFooter="true" 
                            ShowHeaderWhenEmpty="true" Width="100%">
                            <HeaderStyle CssClass="GridHeader" />
                            <FooterStyle CssClass="GridFooter" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" 
                                            OnCheckedChanged="chkSelectAll_CheckedChanged" Text="Select All" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" 
                                            oncheckedchanged="chkSelect_CheckedChanged" />
                                        <asp:ImageButton ID="BtnRemove" runat="server" 
                                            ImageUrl="~/Images/Delete_16x16.png" OnClick="BtnRemove_Click1" 
                                            ToolTip="Remove" Visible="false" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="S NO" HeaderText="S NO" HeaderStyle-Width ="30px"/>
                                 <asp:TemplateField HeaderText="Invoice No.">
                                                    <ItemTemplate>
                                                       <asp:LinkButton ID="lnkInvoiceNo" Text='<%# Bind("RefNo") %>' runat="server" OnClick="lnkInvoiceNo_Click"></asp:LinkButton></ItemTemplate> <%----%>
                                                    <ItemStyle Width="5%"></ItemStyle>
                                            </asp:TemplateField>
                               <%-- <asp:BoundField DataField="Ref No" HeaderText="Invoice No" HeaderStyle-Width ="50px" />--%>
                                 <asp:BoundField DataField="JobCardCode" HeaderText="JobCard No" HeaderStyle-Width ="50px" />
                                <asp:BoundField DataField="Ref Date" HeaderText="Ref Date" />
                                <asp:BoundField DataField="Ref Amount" HeaderText="Ref Amount" />
                                <asp:BoundField DataField="OutStanding" HeaderText="Pending Dues" />
                                <asp:BoundField DataField="Adjustment" HeaderText="Received" />
                                <asp:BoundField DataField="Remaining" HeaderText="Balance" />
                                <%--<asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" onclick="lnkDelete_Click" >Remove</asp:LinkButton></ItemTemplate>
                                            <ItemStyle Width="5%"></ItemStyle>
                                        </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
            </td>
            </tr>
            </table>
           </ContentTemplate>
            </asp:TabPanel>
              <%-- Tab Of Payment Detail   --%>
            <asp:TabPanel class="TabPanel" ID="TabPanel2" runat="server" HeaderText="Sele Person Information">
          <HeaderTemplate>
          Payment Information
           </HeaderTemplate>
           <ContentTemplate>
            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
           <%-- ROW 1 --%>
            <tr>
               <td >

                   Paid by *</td>
               <td >

                   <asp:DropDownList ID="DDLPaymentMode" runat="server" AutoPostBack-="true" Width="100%" OnSelectedIndexChanged="DDLPaymentMode_SelectedIndexChanged">
                   </asp:DropDownList>

               </td>
              
               <td>
            Bank
            </td>
               <td >
               <asp:DropDownList runat="server" ID="ddlbank" AppendDataBoundItems="true" Visible="false">
               <asp:ListItem Selected="True" Value="0">Select Bank (if)</asp:ListItem>
               </asp:DropDownList>
            <%-- <asp:DropDownList  runat="server" AppendDataBoundItems="True" ID="DDLBank" Visible="false";
                       Width="100%">
                       <asp:ListItem Selected="True" Value="0">Select Bank (if)</asp:ListItem>
                   </asp:DropDownList>--%>

               </td>
               <td>
               Branch
                </td>
               <td >

                   <asp:TextBox ID="txtBranch" runat="server" MaxLength="50" Width="100%" Visible="false"></asp:TextBox>

               </td>
              <td >
             Bank Ins No.
             </td>
            <td>
                                        <asp:TextBox ID="txtInstNo" runat="server" MaxLength="15" Width="80px" Visible="false"></asp:TextBox>
                                    </td>  
             </tr>
           <%-- ROW 4 --%>
            <tr>
             <td>
                 Receipt Amt
               </td>
               <td>
                                        <asp:TextBox ID="txtInstAmt" runat="server" MaxLength="6" onblur="AmtTot()" 
                                            OnTextChanged="txtInstAmt_TextChanged" Placeholder="0.00" 
                                            Style="text-align: right" Width="100%"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtInstAmt_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtInstAmt" 
                                            ValidChars="." />
                                    </td>
                                
            <td >
                                        Bank Ins Date.

                                    </td>
            <td>
                                        <asp:TextBox ID="txtInstDate" runat="server"  Visible="false"
                                            Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;" 
                                            Width="100%"></asp:TextBox>
                                        <asp:MaskedEditExtender ID="txtInstDate_MaskedEditExtender" runat="server" 
                                            ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" 
                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                            CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                                            Mask="99-99-9999" TargetControlID="txtInstDate" />
                                        <asp:CalendarExtender ID="txtInstDate_CalendarExtender" runat="server" 
                                            Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtInstDate" />
                                    </td>
           
            <td >
                                        <asp:Label ID="lblTotSelectedInv" runat="server" Text="Selected Total" Visible="false"></asp:Label>
                                    
                                        <asp:TextBox ID="txtSelectedTotalRefAmount" runat="server" Width ="80px" Visible="false"></asp:TextBox>
                                    </td>                                  
            <td >
                                         <asp:Button ID="btnAutoAdj" runat="server" Height="40px" 
                                               OnClick="btnAutoAdj_Click" Text="Amount Adjustment" Width="170px" style="float:right;"  /></td>
            </tr>
            </table>
           </ContentTemplate>
            </asp:TabPanel>
             <%-- Tab Of Payment Tax  Detail   --%>
            <asp:TabPanel class="TabPanel" ID="TabPanel3" runat="server" HeaderText="Sele Person Information">
          <HeaderTemplate>
           Discount and Tax charges
           </HeaderTemplate>
           <ContentTemplate>
            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
           <tr>
           <td>
            <asp:GridView ID="gvTaxDetail" runat="server" Width="100%" ShowFooter="true" ShowHeaderWhenEmpty="true"
                                OnRowDataBound="gvTaxDetail_RowDataBound" AutoGenerateColumns="false">
                                <HeaderStyle CssClass="GridHeader" />
                                <FooterStyle CssClass="GridFooter" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Receipt Head">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecHead" runat="server" Text='<%# Bind("ReceiptHead") %>'></asp:Label>
                                            <asp:Label ID="lblTaxID" runat="server" Text='<%# Bind("TaxID") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:ImageButton ID="BtnAdd" runat="server" Height="30px" ImageUrl="~/Images/Export.png"
                                                            Width="30px" OnClick="BtnAdd_Click1" />
                                                    </td>
                                                    <td align="center" style="margin-left:15px;">
                                                        <asp:DropDownList ID="DDLFooterReceiptHead" runat="server" Width="200px" ToolTip="List of Taxes" 
                                                            onselectedindexchanged="DDLFooterReceiptHead_SelectedIndexChanged" AutoPostBack="True" >
                                                        </asp:DropDownList>
                                                    </td>
                                                    
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Percent %">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPercent" runat="server" Text='<%# Bind("TaxPerc") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFooterTaxPerc" runat="server" Width="50px" MaxLength="6" AutoPostBack="true" 
                                                Style="text-align: right" ToolTip="Tax Percentage" 
                                                ontextchanged="txtFooterTaxPerc_TextChanged" ></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                            Enabled="True" TargetControlID="txtFooterTaxPerc" ValidChars="0123456789."></asp:FilteredTextBoxExtender>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        
                                        <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>                                       
                                            
                                            <asp:TextBox ID="txtFooterTaxAmt" runat="server" Width="80px" MaxLength="10"  AutoPostBack="true"
                                                Style="text-align: right" ToolTip="Tax Amount" 
                                                ontextchanged="txtFooterTaxAmt_TextChanged"  ></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="txtFooterTaxAmt_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" TargetControlID="txtFooterTaxAmt" ValidChars="0123456789."></asp:FilteredTextBoxExtender>
                                            
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remove">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="BtnRemove" runat="server" ImageUrl="~/Images/Delete_16x16.png"
                                                ToolTip="Remove" OnClick="BtnRemove_Click2" />
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
           </td>
           </tr>
            </table>
           </ContentTemplate>
            </asp:TabPanel>
             <asp:TabPanel class="TabPanel" ID="TabPanel4" runat="server" HeaderText="Sele Person Information">
          <HeaderTemplate>
          Insurance
           </HeaderTemplate>
           <ContentTemplate>
            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
           <tr>
             <td >
                   <asp:CheckBox ID="chkInsRec" runat="server" AutoPostBack="True" 
                       OnCheckedChanged="chkInsRec_CheckedChanged" Text="Insurance Receipt" />
               </td>
             <td >
                                        <asp:Label ID="lblInsComp" runat="server" Text="Ins Comp."></asp:Label>
                                    </td>
             <td>
                                        <asp:DropDownList ID="ddlInsCo" runat="server" AutoPostBack="True" 
                                            OnSelectedIndexChanged="ddlInsCo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
             <td >
                                        <asp:Label ID="lblInsBr" runat="server" Text="Ins Branch."></asp:Label>
                                    </td>
             <td>
                                        <asp:DropDownList ID="ddlInsBranch" runat="server" AutoPostBack="True" 
                                            OnSelectedIndexChanged="ddlInsBranch_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
           
           </tr>
            </table>
           </ContentTemplate>
            </asp:TabPanel>
            </asp:TabContainer>
           
          
          
            <table width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblMsg" runat="server" Width="100%"></asp:Label>
                    </td>
                    <td align="center">
                        <asp:Button ID="btnSave" runat="server" Width="100px" Text="Save" OnClick="btnSave_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait';"
                            UseSubmitBehavior="false" />
                        <asp:Button ID="btnDelete" runat="server" Width="100px" Text="Delete" OnClientClick=" return confirm('Are you sure to delete?');"
                            OnClick="btnDelete_Click" />
                        <asp:Button ID="btnClear" runat="server" Width="100px" Text="Clear" OnClick="btnClear_Click" />
                        <asp:Button ID="btnPrint" runat="server" Width="100px" Text="Print" OnClick="btnPrint_Click" />
                        <asp:Button ID="btnGL" runat="server" Width="100px" Text="GL" OnClick="btnGL_Click"/>
                    </td>
                </tr>
            </table>
            </div>        
       </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
