<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Root.master" Theme="Theme1" CodeBehind="outgoingpayments.aspx.cs" Inherits="DXBMS.Modules.SpareParts.Forms.outgoingpayments" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>



<asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">
   <script type="text/javascript" src="../../../Jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../../Jquery/MaxLength.min.js"></script>
    <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
          <script src="../../Data/sweetalert.js" type="text/javascript"></script>  
     <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>
 <script type="text/javascript">
           function Savealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been saved',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "outgoingpayments.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "outgoingpayments.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "outgoingpayments.aspx";
           }
           function reloads() {
               window.location.href = "outgoingpayments.aspx";
           }

       </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
                
                <div class="Heading">
                    Payment To Vendor
                </div>
                <table width="100%">
                        <tr>
                            <td style="border:1px solid #808080; width:85%;">
                            <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal"
                                 OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" AutoPostBack="true">
                                <asp:listitem Text="Advance Payment To Vendor" Value="Advance Payment To Vendor"></asp:listitem>
                                <asp:ListItem Text="P/A Invoice Payment To Vendor" Value="P/A Invoice Payment To Vendor"></asp:ListItem>
                                <asp:ListItem Text="Payment To Vendor W/O Invoice" Value="Payment To Vendor W/O Invoice"></asp:ListItem>
                                <asp:ListItem Text="Refund Advance Reciept To Customer" Value="Refund Advance Reciept To Customer"></asp:ListItem>
                            </asp:RadioButtonList>
                            </td>
                            <td style="border:1px solid #808080;" >
                                <asp:RadioButtonList ID="RadioButtonList2" runat="server" RepeatDirection="Vertical">
                                    <asp:ListItem Text="Item" Value="Item"></asp:ListItem>
                                    <asp:ListItem Text="Service" Value="Service"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                <fieldset>
                <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                    <tr>
                        <td width="11%">Payment No.</td>
                        <td width="30%">
                            <asp:DropDownList ID="ddlpayment_no" runat="server" Width="100%" OnSelectedIndexChanged="ddlpayment_no_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlpayment_no" QueryPattern="Contains"></asp:ListSearchExtender>
                        </td>
                        <td width="10%">Payment Date</td>
                        <td>
                            <asp:TextBox ID="txtpayment_date" runat="server"  
                                Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"
                                Width="100%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtpayment_date" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtpayment_date" Format="dd/MM/yyyy"></asp:CalendarExtender>
                        </td>
                        <td>Cost Center</td>
                        <td>
                            <asp:DropDownList ID="ddlcostcenter" runat="server" Width="100%"></asp:DropDownList>
                            <asp:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddlcostcenter" QueryPattern="Contains"></asp:ListSearchExtender>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Vendor" runat="server" Text="Vendor"></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="ddlvendor" runat="server" AutoPostBack="True" width="100%" onselectedindexchanged="ddlvendor_SelectedIndexChanged"></asp:DropDownList>
                            <asp:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddlvendor" QueryPattern="Contains"></asp:ListSearchExtender>

                        </td>
                        <td>Remarks</td>
                        <td >
                            <asp:TextBox ID="txtremarks" runat="server" Width="100%" TextMode="MultiLine" MaxLength="100" Wrap="true" style="resize:none;" ></asp:TextBox>
                        </td>

                    
                       
                    
                    <td>Total Amount</td>
                    <td>
                        <asp:TextBox ID="txttotal_amount" runat="server" Text="0" Enabled="false" style="text-align:right;" Width="100%"></asp:TextBox>
                    </td>
                   
                </tr>
                <tr>
                
                    <td >
                        <asp:CheckBox ID="cbinlude_adv" Text="Include Adv." runat="server" OnCheckedChanged="cbinlude_adv_CheckedChanged" AutoPostBacK="true" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlinclude_adv" runat="server" Enabled="false" Width="100%" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddlinclude_adv_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td>Adv. Adjusted Amt</td>
                    <td>
                        <asp:TextBox ID="txtadvajdusted_amt" runat="server" Text="0.00" Enabled="false" style="text-align:right;" Width="100%" AutoPostBack="true" CausesValidation="false" OnTextChanged="txtadvajdusted_amt_TextChanged"></asp:TextBox>
                    </td>
                <td>
                Total OutStanding
                </td>
                <td>
                <asp:TextBox runat="server" ID="txttotOutStanding" ReadOnly="true" SkinID="ROTextBox" Width="100%"></asp:TextBox>
                </td>
                </tr>
                 <%-- Row 4 --%>
               <tr >
               <td>
               Invoice total
               </td>
               
           <td> <asp:TextBox ID="txttotAdj" runat="server" ReadOnly="True" SkinID="ROTextBox" 
                            Visible="false" Width="80px"></asp:TextBox> 
                            <asp:TextBox runat="server" ReadOnly="true" SkinID="ROTextBox" Width="100%" ID="txtInvTotal"></asp:TextBox>
           <asp:HiddenField ID="txtAdj" runat="server" Value="f" />
           </td>
           <td>Voucher No 

           </td>
              <td>
              <asp:TextBox ID="txtVoucherNo" runat="server" ReadOnly="true" SkinID="ROTextBox"></asp:TextBox>
              </td>
          
            <td colspan="1" align="right">
             <asp:Button ID="btnPendingInv" runat="server" Text="Pending Invoices" Width="150px" OnClick="btnPendingInv_Click" CausesValidation="false" /></td>

                                            
                 <td  align="right" >
                   <asp:Button ID="btn_fifo" runat="server" Text="Auto Adjust (FIFO)" Width="150px" OnClick="btn_fifo_Click" CausesValidation="false" /></td>
                  <asp:TextBox ID="txtSelectedTotalRefAmount" runat="server" Width ="80px" Visible="false"></asp:TextBox>
                                         
             </tr>
                
              
                </table>
                </fieldset>
                  <asp:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" Width="100%"
                        Style="direction: ltr; text-align: left;">
                                  <%-- Tab For Pending Invoice View --%>   
                                   <asp:TabPanel class="TabPanel" ID="TabPanel5" runat="server" HeaderText="Sele Person Information">
          <HeaderTemplate>
           Invoice List of Selected Vendor
           </HeaderTemplate>
           <ContentTemplate>
            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
            <tr>
            <td>
              
             <asp:GridView ID="gvPendingInvoice" runat="server" AutoGenerateColumns="false" 
                            OnRowCreated="gvPendingInvoice_RowCreated" 
                            OnRowDataBound="gvPendingInvoice_RowDataBound" ShowFooter="true" 
                            ShowHeaderWhenEmpty="true" Width="100%">
                            <HeaderStyle CssClass="GridHeader" />
                            <FooterStyle CssClass="GridFooter" />
                            <Columns>
                            <%--   --%>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged"
                                            Text="Select All" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                    <%--   oncheckedchanged="chkSelect_CheckedChanged" /> --%>
                                        <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true"  oncheckedchanged="chkSelect_CheckedChanged" />
                                          
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
                                 <asp:BoundField DataField="JobCardCode" HeaderText="GRN No" HeaderStyle-Width ="50px" />
                                <asp:BoundField DataField="Ref Date" HeaderText="Ref Date" />
                                <asp:BoundField DataField="Ref Amount" HeaderText="Inv Amount" />
                                <asp:BoundField DataField="OutStanding" HeaderText="OutStanding" />
                                <asp:BoundField DataField="Adjustment" HeaderText="Adjusted" />
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
                                     
                                     
                                     
           
             <%-- Tab Of Tax And Discount  --%>
            <asp:TabPanel class="TabPanel" ID="TabPanel1" runat="server" HeaderText="Sele Person Information">
          <HeaderTemplate>
            Tax And Discount
           </HeaderTemplate>
           <ContentTemplate>
           <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                <tr>
                <td width="11%">Payment Head</td>
                <td width="30%">
                    <asp:DropDownList ID="ddlpayment_head" Width="100%" runat="server" OnSelectedIndexChanged="ddlpayment_head_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <asp:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddlpayment_head" QueryPattern="Contains"></asp:ListSearchExtender>
                </td>
                <td width="11%">Account Code
                    <asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png" OnClick="imgLookup_Click"  CausesValidation="false"/>
                </td>
                <td>
                    <asp:TextBox ID="txtAccountCode" runat="server" ReadOnly="false" SkinID="ROTextBox"></asp:TextBox>
                </td>
                <td width="12%"> <asp:TextBox ID="txtpaymenttax_amount" runat="server" placeholder="Enter Amount.." Width="80px"></asp:TextBox></td>
                <td>
                    <asp:ImageButton ID="ib_addtax" runat="server" AlternateText="Add"  OnClick="ib_addtax_Click"
                        ImageUrl="~/Images/Grid_Update.png" CausesValidation="false" /></td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:GridView ID="GridView_payment" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" Width="100%"
                        ShowFooter= "true" OnRowDataBound="GridView_payment_RowDataBound" OnRowCancelingEdit="GridView_payment_RowCancelingEdit"
                        OnRowEditing="GridView_payment_RowEditing" OnRowUpdating="GridView_payment_RowUpdating" >
                        <HeaderStyle CssClass="GridHeader" />
                        <FooterStyle CssClass="GridFooter" />
                        <Columns>
                            <asp:CommandField ShowEditButton="True"></asp:CommandField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Receipt Head">
                                <ItemTemplate >
                                <asp:Label ID="lblReceiptHead" runat="server" Text='<%# Bind("TaxID") %>' ></asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Description" DataField="TaxHead" ReadOnly="true" />
                            <asp:TemplateField  HeaderText="Account">
                                <ItemTemplate>
                                    <asp:Label ID="lblAccountCode" runat="server" Text='<%#Bind("AccountCode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Right"    >
                            <ItemTemplate >
                                <asp:Label ID="lblTaxAmount" runat="server" Text='<%# Bind("Amount") %>' ></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtTaxAmount" runat="server" Text='<%# Bind("Amount") %>' ></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="txttotaltax_amt" runat="server" Text="0" ></asp:Label>
                            </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remove" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate >
                            <asp:ImageButton ID="BtnRemove_Tax" runat="server" CausesValidation="false"
                            ImageUrl="~/Images/Delete_16x16.png" onclick="BtnRemove_Tax_Click"
                            ToolTip="Remove" />
                            </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
             </tr>
            </table>
           </ContentTemplate>
           </asp:TabPanel>
            <%-- Tab Of Payment Detail  --%>
           <asp:TabPanel class="TabPanel" ID="TabPanel2" runat="server" HeaderText="Sele Person Information">
          <HeaderTemplate>
           Payment Detail 
           </HeaderTemplate>
           <ContentTemplate>
           <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                <tr>
                    <td width="11%">Payment Mode</td>
                    <td width="30%">
                        <asp:DropDownList ID="ddlpayment_mode" runat="server" Width="80%" AutoPostBack="true" OnSelectedIndexChanged="ddlpayment_mode_SelectedIndexChanged"></asp:DropDownList>
                        <asp:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddlpayment_mode" QueryPattern="Contains"></asp:ListSearchExtender>
                    </td>
                    
                </tr>
                <tr>
                    <td >Payment A/C</td>
                    <td >
                        <asp:DropDownList ID="ddlpayment_account" runat="server" width="80%"
                            OnSelectedIndexChanged="ddlpayment_account_SelectedIndexChanged"></asp:DropDownList>
                        <asp:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddlpayment_account" QueryPattern="Contains"></asp:ListSearchExtender>
                    </td>
                    <td>Bank</td>
                    <td>
                        <asp:DropDownList ID="ddlbank" runat="server" Width="80%"></asp:DropDownList>
                        <asp:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddlbank" QueryPattern="Contains"></asp:ListSearchExtender>
                    </td>
                    <td>Branch</td>
                    <td>
                        <asp:TextBox ID="txtbranch" runat="server" MaxLength="5" Width="100%"></asp:TextBox>
                     </td>
                </tr>
                <tr>
                    <td>Instrument Date</td>
                    <td >
                        <asp:TextBox ID="txtinstrument_date" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"
                        Width="100%" ></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ControlToValidate="txtinstrument_date"  ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>

                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtinstrument_date" Format="dd/MM/yyyy" ></asp:CalendarExtender>
                    
                    </td>
                    <td >Instrument No.</td>
                    <td>
                        <asp:TextBox ID="txtinstrument_no" runat="server" MaxLength="10" Width="100%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Enabled="false" runat="server" ErrorMessage="*" ControlToValidate="txtinstrument_no" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                    </td>
                    <td >Ins. Amount</td>
                    <td>
                        <asp:TextBox ID="txtins_amount" runat="server" style="text-align:right;" MaxLength="10" Width="100%" AutoPostBack="true" CausesValidation="false" OnTextChanged="txtins_amount_TextChanged"></asp:TextBox>
                       <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="txtins_amount"  ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                       <asp:FilteredTextBoxExtender ID="txtOtherChargers_FilteredTextBoxExtender" 
                    runat="server" FilterType="Custom" TargetControlID="txtins_amount" ValidChars=".0123456789" >
                </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
            
            </table>
           </ContentTemplate>
           </asp:TabPanel>
            <%-- Tab Of Refund Advance Payment --%>
           <asp:TabPanel class="TabPanel" Visible="false" ID="TabPanel3" runat="server" HeaderText="Sele Person Information">
          <HeaderTemplate>
            List of Invoices
           </HeaderTemplate>
           <ContentTemplate>
           <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
            <tr>
            <td>
             Receipt No 
            </td>
            <asp:DropDownList ID="ddlAdvReceipt" runat="server" Width="80%"  AutoPostBack="true" Font-Names ="monospace"></asp:DropDownList>
                        <asp:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="ddlAdvReceipt" QueryPattern="Contains"></asp:ListSearchExtender>
            </tr>
            </table>
           </ContentTemplate>
           </asp:TabPanel>
          
         
                  </asp:TabContainer>
                
                <table width="100%">
        <tr>
         <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*) Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
            <td>
                <asp:Label ID="success_add_item" runat="server" Text="Item Added Succesfully" ForeColor="Green" Font-Bold="true" ></asp:Label>
            </td>
            <td style="text-align:center;">
                <asp:Button ID="btn_save" runat="server" Text="Save" Width="100px"  CausesValidation="True" OnClick="btn_save_Click" />
              
                   <asp:Button ID="btn_delete" runat="server" Width="100px" Text="Delete" OnClientClick=" return confirm('Are you sure to delete?');"
                            OnClick="btn_delete_Click" />

                <asp:Button ID="btn_clear" runat="server" Text="Clear" Width="100px" CausesValidation="false" OnClick="btn_clear_Click"/>  
                <asp:Button ID="btn_print" runat="server" Text="Print" Width="100px" CausesValidation="false" OnClick="btn_print_Click" />      
                <asp:Button ID="btnGL" runat="server" Text="GL" Width="100px" CausesValidation="false" OnClick="btnGL_Click" />  
            </td>
        </tr>
    </table>
            </div>
   </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>

