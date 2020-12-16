<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PayableInvoice.aspx.cs"
    Inherits="DXBMS.Modules.SpareParts.Forms.PayableInvoice" MasterPageFile="~/Root.master"
    Theme="Theme1" %>

    

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">

<script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
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
               window.location.href = "PayableInvoice.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "PayableInvoice.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "PayableInvoice.aspx";
           }
           function reloads() {
               window.location.href = "PayableInvoice.aspx";
           }

       </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
            <div class="row">
                <div class="col-lg-12">
              
                <div class="col-md-4 pull-right" style="margin-top:10px;">
                <asp:Label runat="server" ID="lblMode" Font-Size="16px" Font-Bold="true" ForeColor="Black"> </asp:Label>
                </div>
                </div>
                </div>   

            <div class="Heading">
                PAYABLE INVOICE
            </div>

            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                <tr>
                    <td style="width: 10%">
                        Payable Inv No.<asp:ImageButton runat="server" ID="imgPayInvLookup" ImageUrl="~/Images/LookupNew.png" onclick="imgPayInvLookup_Click"/>
                    </td>
                    <td width="57%">
                        <asp:DropDownList ID="ddlPayInvNo" runat="server" Width="75%" 
                            Font-Names="monospace" AutoPostBack="True" 
                            onselectedindexchanged="ddlPayInvNo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 13%">
                        Payable Date.
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="txtPayInvDate" runat="server" placeholder="dd-MM-yyyy" Style="background-image: url(../../../Images/Calendar_16x16.png);
                            background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtPayInvDate"
                            Format="dd-MM-yyyy" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Vendor Desc <font color="red"><b>*</b></font>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlVendor" runat="server" Width="75%" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Principal Inv No.<font color="red"><b>*</b></font>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPrincipalInvNo" runat="server" Style="text-align: right; width: 100%;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        GRN Info <asp:ImageButton runat="server" ID="imgGRNLookup" ImageUrl="~/Images/LookupNew.png" onclick="imgGRNLookup_Click"/><font color="red"><b>*</b></font>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlGRN" runat="server" Width="75%" Font-Names="monospace"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlGRN_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Principal Inv Date. <font color="red"><b>*</b></font>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPrincipalInvDate" runat="server" placeholder="dd-MM-yyyy" Style="background-image: url(../../../Images/Calendar_16x16.png);
                            background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MM-yyyy" TargetControlID="txtPrincipalInvDate" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Payment Terms 
                    </td>
                    <td>
                        <asp:TextBox ID="txtPaymentTerms" runat="server" Style="text-align: right; width: 80px"></asp:TextBox>
                        Days
                    </td>
                    <td>
                        Order No. <font color="red"><b>*</b></font>
                    </td>
                    <td>
                        <asp:TextBox ID="txtOrder" runat="server" Style="text-align: right; width: 100%" Placeholder="Order"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Remarks
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                    </td>
                    <td>
                    Voucher No
                    </td>
                    <td>
                    <asp:TextBox runat="server" ID="txtVoucherNo" ReadOnly="true" SkinID="ROTextBox"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <fieldset>
                            <legend>Payable Invoice Detail</legend>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvPurInvDetail" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ShowHeaderWhenEmpty="True" ShowFooter="true" OnRowDataBound="gvPurInvDetail_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" 
                                                            oncheckedchanged="chkSelect_CheckedChanged" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ItemCode" HeaderText="ItemCode" />
                                                <asp:BoundField DataField="PartItemNo" HeaderText="Part Item No" />
                                                <asp:BoundField DataField="ItemDesc" HeaderText="Part Description" />
                                                <asp:BoundField DataField="AccQty" HeaderText="Quantity" />
                                                <asp:BoundField DataField="PaidQty" HeaderText="Paid Qty" />
                                                <%--<asp:TemplateField HeaderText="Payable Qty">
                        <ItemTemplate>
                        <asp:Label ID="lblPayableQty" runat ="server" Text ='<%# Bind("PayableQty") %>'></asp:Label>
                        </ItemTemplate>
                        txtPaidQty
                        </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Payable Qty">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPayableQty" runat="server" Width="60px" MaxLength="6"
                                                            Style="text-align: right" Text ='<%# Bind("PayableQty") %>' 
                                                            AutoPostBack="True" ontextchanged="txtPayableQty_TextChanged"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="txtOtherChargers_FilteredTextBoxExtender" 
                    runat="server" FilterType="Custom, Numbers" TargetControlID="txtPayableQty" 
                    ValidChars="." />
                                                    </ItemTemplate>
                                                    <FooterTemplate >
                                                    <asp:Label ID="lblPayableQty" runat ="server"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Rate" HeaderText="Unit Price" />
                                                <asp:BoundField DataField="DiscountPerc" HeaderText="Discount %" />
                                                <asp:TemplateField HeaderText="Discount Amt">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDiscountAmt" runat="server" Width="60px" 
                                                            Style="text-align: right" Text ='<%# Bind("DiscountAmt") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate >
                                                    <asp:Label ID="lblDicountTotal" runat ="server"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="Gross Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGrossAmt" runat="server" Width="60px" 
                                                            Style="text-align: right" Text ='<%# Bind("Amount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate >
                                                    <asp:Label ID="lblFooterGrossAmt" runat ="server"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:BoundField DataField="DiscountAmt" HeaderText="Discount Amt" />--%>
                                                <%--<asp:BoundField DataField="Amount" HeaderText="Gross Amount" />--%>
                                            </Columns>
                                            <AlternatingRowStyle BackColor="#CCCCCC" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <table width ="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                <tr>
            <td style ="width :80%; text-align : right ">
                Add Sales Tax
                <asp:Label ID="lblGstPerc" runat="server" ></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtGstAmt" runat="server" style="text-align :right " SkinID="ROTextBox"></asp:TextBox>
            </td>
            </tr>
                <tr>
                    <td style="text-align : right ">
                        Add Further Tax
                        <asp:Label ID="lblFurTaxPerc" runat="server" ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFurTaxAmt" runat="server" style="text-align :right " SkinID="ROTextBox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style=" text-align : right ">
                        Add Extra Tax
                        <asp:Label ID="lblExTaxPerc" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtExTaxAmt" runat="server" style="text-align :right " SkinID="ROTextBox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style=" text-align : right ">
                        Total Amount</td>
                    <td>
                        <asp:TextBox ID="txtTotalAmt" runat="server" style="text-align :right " SkinID="ROTextBox"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table width="100%" style="margin-bottom: 10px;margin-top:10px;">
                <tr>
                 <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*)  Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
                    <td>
                        <asp:Label ID="LbErr" runat="server" Text=""></asp:Label>
                    </td>
                    <td style="text-align: right;">
                        <asp:Button ID="BtnSave" runat="server" Text="Save" Width="100px" OnClientClick="this.disabled = true; this.value = 'Please Wait';"
                            UseSubmitBehavior="false" OnClick="BtnSave_Click"  />
                        &nbsp;
                               <asp:Button ID="BtnDelete" runat="server" Width="100px" Text="Delete" OnClientClick=" return confirm('Are you sure to delete?');"
                            OnClick="BtnDelete_Click" />
                        &nbsp;<asp:Button ID="btnClear" runat="server" Width="100px" Text="Clear" OnClick="btnClear_Click" />
                        &nbsp;<asp:Button ID="BtnPrint" runat="server"  Width="100px" Text="Print"  Visible="false"/>
                        <asp:Button ID="btnGL" runat="server" Text="GL" Width="100px" OnClick="btnGL_Click"/>
                        <%--&nbsp;<asp:Button ID="btnPosting" runat="server" Text="Post" OnClientClick="this.disabled = true; this.value = 'Please Wait';"
                            UseSubmitBehavior="false"  />--%>
                    </td>
                </tr>
            </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
