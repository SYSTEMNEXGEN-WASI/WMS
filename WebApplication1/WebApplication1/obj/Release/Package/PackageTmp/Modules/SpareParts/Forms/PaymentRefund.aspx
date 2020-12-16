<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" Theme="Theme1" AutoEventWireup="true" CodeBehind="PaymentRefund.aspx.cs" Inherits="DXBMS.Modules.SpareParts.Forms.PaymentRefund" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>
 <script src="../../Data/sweetalert.js" type="text/javascript"></script>  
 <script type="text/javascript">
           function Savealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been saved',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "PaymentRefund.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "PaymentRefund.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "PaymentRefund.aspx";
           }
           function reloads() {
               window.location.href = "PaymentRefund.aspx";
           }

       </script>
<asp:UpdatePanel runat="server">
<ContentTemplate>
<div class="container">
                <div class="Heading">
                Refund Payment
                </div>
                
                <fieldset>
                <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                        <tr>
                            
                            <td style="" >
                                <asp:RadioButtonList ID="RbtnType" runat="server" RepeatDirection="Vertical" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="RbtnType_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Text="Customer" Value="Customer"></asp:ListItem>
                                    <asp:ListItem Text="Vendor" Value="Vendor"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                  <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                                <%-- Row 1 --%>
                                <tr>
                                <td>
                                Refund No<asp:ImageButton runat="server" ID="imgReceiptNo" CausesValidation="false" ImageUrl="~/Images/LookupNew.png" OnClick="imgReceiptNo_Click" /><span  style="font-size:large; color:red;" >*</span>
                                </td>
                                <td width="39%">
                        <asp:TextBox runat="server" ID="txtRefundCode" ReadOnly="true" Width="100%" TabIndex="1" SkinID="ROTextBox"></asp:TextBox>
                         
                        </td>
                        <td>
                        Refund Date <span  style="font-size:large; color:red;" >*</span>
                        </td>
                        <td>
                             <asp:TextBox ID="txtRefundDate" runat="server"  
                                Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"
                                Width="100%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtRefundDate" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtRefundDate" Format="dd/MM/yyyy"></asp:CalendarExtender>
                       
                        </td>
                        <td>
                        Receipt No<span  style="font-size:large; color:red;" >*</span>
                        </td>
                        <td>
                        <asp:DropDownList runat="server" ID="ddlReceiptNo" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddlReceiptNo_SelectedIndexChanged">

                        </asp:DropDownList>
                        <asp:TextBox runat="server" ID="txtAdvReceiptNo" ReadOnly="true" SkinID="ROTextBox" Visible="false"></asp:TextBox>

                        </td>
                                </tr>
                                <%-- Row 2 --%>
                                <tr>
                     <td><asp:Label runat="server" ID="lblType"></asp:Label></td>
                    <td>
                          <asp:TextBox ID="txtCusVenCode" runat="server"  Enabled="false" style="text-align:left;" Width="25%"></asp:TextBox>
                        <asp:TextBox ID="txtCUSVen" runat="server"  Enabled="false" style="text-align:left;" Width="72%"></asp:TextBox>

                    </td>
                    <td>
                   
                    Invoice Type
                   
                    </td>
                    <td>
                      <asp:TextBox ID="txtInvType" runat="server"  Enabled="false" style="text-align:left;" Width="100%"></asp:TextBox>
                    </td>
                    <td>
                    Balance
                    </td>
                    <td>
                    <asp:TextBox ID="txtBalance" runat="server" Text="0.00" Enabled="false" style="text-align:right;" Width="100%"></asp:TextBox>
                    </td>
                                </tr>
                                  <%-- Row 3 --%>
                                
                <tr>
                    <td width="11%">Payment Mode <span  style="font-size:large; color:red;" >*</span></td>
                    <td width="30%">
                        <asp:DropDownList ID="ddlpayment_mode" runat="server" Width="80%" AutoPostBack="true" OnSelectedIndexChanged="ddlpayment_mode_SelectedIndexChanged"></asp:DropDownList>
                        <asp:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddlpayment_mode" QueryPattern="Contains"></asp:ListSearchExtender>
                    </td>
                   
                    
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
                </tr>
                <%-- Row 3 --%>
                <tr>
                    
                    
                    <td>Branch</td>
                    <td>
                        <asp:TextBox ID="txtbranch" runat="server" MaxLength="5" Width="100%"></asp:TextBox>
                     </td>
                     <td >Instrument No.</td>
                    <td>
                        <asp:TextBox ID="txtinstrument_no" runat="server" MaxLength="10" Width="100%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Enabled="false" runat="server" ErrorMessage="*" ControlToValidate="txtinstrument_no" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                    </td>
                     <td>Instrument Date</td>
                    <td >
                        <asp:TextBox ID="txtinstrument_date" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"
                        Width="100%" ></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ControlToValidate="txtinstrument_date"  ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>

                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtinstrument_date" Format="dd/MM/yyyy" ></asp:CalendarExtender>
                    
                    </td>
                </tr>
                <%-- Row 4 --%>
                <tr>
                   
                    
                    <td >Ins. Amount <span  style="font-size:large; color:red;" >*</span></td>
                    <td>
                        <asp:TextBox ID="txtins_amount" runat="server" style="text-align:right;" MaxLength="10" Width="100%"></asp:TextBox>
                       <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="txtins_amount"  ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                       <asp:FilteredTextBoxExtender ID="txtOtherChargers_FilteredTextBoxExtender" 
                    runat="server" ValidChars=".0123456789" TargetControlID="txtins_amount" >
                </asp:FilteredTextBoxExtender>
                    </td>
                     <td style="display:none">Total Amount</td>
                    <td>
                        <asp:TextBox ID="txttotal_amount" runat="server" Text="0"  Enabled="false" style="text-align:right;display:none;" Width="100%"></asp:TextBox>
                    </td>
                    <td>
                    <asp:Button runat="server" ID="btnAdjAmt"  Text="Adjustment" OnClick="btnAdjAmt_Click"/>
                    </td>
                    <td>
                    <asp:Label runat="server" ForeColor="Red" Text="Note: Please Adjust Amount Before Save "></asp:Label>
                    </td>
                   
                </tr>
                <tr>
                <td>Voucher No</td>
                <td>
                <asp:TextBox ID="txtVoucherNo" runat="server" ReadOnly="true" SkinID="ROTextBox"></asp:TextBox>
                </td>
                </tr>
   </table>

   <table width="100%">
        <tr>
         <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*)  Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
            <td>
                <asp:Label ID="success_add_item" runat="server"  ForeColor="Green" Font-Bold="true" ></asp:Label>
            </td>
            <td style="text-align:center;">
             
                <asp:Button ID="btn_save" runat="server" Text="Save" Width="100px"  CausesValidation="True" OnClick="btn_save_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait';"
                            UseSubmitBehavior="false" />
                <asp:Button ID="btn_delete" runat="server" Visible="false" Text="Delete" Width="100px"  CausesValidation="False" OnClick="btn_delete_Click" />
                <asp:Button ID="btn_clear" runat="server" Text="Clear" Width="100px" CausesValidation="false" OnClick="btn_clear_Click"/>  
                <asp:Button ID="btn_print" runat="server" Text="Print" Width="100px" CausesValidation="false" OnClick="btn_print_Click" />      
                <asp:Button ID="btnGL" runat="server" Text="GL" Width="100px" CausesValidation="false" OnClick="btnGL_Click" />  
            </td>
        </tr>
    </table>
                </fieldset>
                              


</div>
</ContentTemplate>

</asp:UpdatePanel>

</asp:Content>
