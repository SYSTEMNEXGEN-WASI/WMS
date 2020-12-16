<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseReturn.aspx.cs" Inherits="DXBMS.Modules.Service.Forms.PurchaseReturn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
--%>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseReturn.aspx.cs" Theme="Theme1"
    Inherits="DXBMS.Modules.SpareParts.Forms.PurchaseReturn" MasterPageFile="~/Root.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
    function calc() {
        var Rate = document.getElementById('<%=txtRate.ClientID%>').value;
        var IssueQty = document.getElementById('<%=txtRetQty.ClientID%>').value;
        if (Rate == "") {
            Rate = "0.0";
        }
        if (IssueQty == "") {
            IssueQty = "0.0";
        }
        var total = parseFloat(Rate) * parseFloat(IssueQty);
        document.getElementById('<%=txtTotalAmt.ClientID%>').value = parseFloat(total).toFixed(2).toString();
    }
  
    </script>
    <asp:UpdatePanel ID="UpdPanel1" runat="server">
        <ContentTemplate>
        
         <table width="100%">
                <tr>
                    <td style="width: 40%">
                        <asp:Label ID="LbErr" runat="server" Text=""></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait';"
                            UseSubmitBehavior="false" />
                        &nbsp;&nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                        &nbsp;<asp:Button ID="BtnPrint" runat="server" Text="Print" OnClick="BtnPrint_Click" />
                        &nbsp;<asp:Button ID="btnPosting" runat="server" Text="Post" OnClick="btnPosting_Click"
                            OnClientClick="this.disabled = true; this.value = 'Please Wait';" UseSubmitBehavior="false" />
                    </td>
                </tr>
                
            </table>
             <div class="Heading" >
            PURCHASE RETURNE (PR) (TRANSACTION)
        </div>
            <table style="width: 100%">
                <tr>
                    <td colspan="7">
                        <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>--%>
                        <asp:HiddenField ID="HDUnitCode" runat="server" />
                        <asp:HiddenField ID="HDDeleted" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%">
                        PR No.*
                    </td>
                    <td style="width: 3%; height: 21px;">
                        <asp:ImageButton ID="ImgLookupIssueNo" runat="server" Height="17px" ImageUrl="~/Images/LookUpNew.png"
                            Width="17px" OnClick="ImgLookupIssueNo_Click" />
                    </td>
                    <td style="width: 25%">
                        <asp:TextBox ID="txtReturnNo" runat="server" ReadOnly="true"></asp:TextBox>
                        <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" CultureAMPMPlaceholder=""
                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                            Enabled="True" ErrorTooltipEnabled="True" Mask="99/99999" MaskType="None" TargetControlID="txtReturnNo"
                            ClearMaskOnLostFocus="False" />
                    </td>
                    <td style="width: 8%">
                        PR Date *
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtRetDate" runat="server" Width="70px" ReadOnly="true" CssClass="ROTextBoxes"></asp:TextBox>
                        <cc1:MaskedEditExtender ID="txtPODate_MaskedEditExtender" runat="server" Mask="99/99/9999"
                            MaskType="Date" TargetControlID="txtRetDate">
                        </cc1:MaskedEditExtender>
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%; height: 19px;">
                        Vendor</td>
                    <td style="width: 3%; height: 19px;">
                        &nbsp;</td>
                    <td >
                        <asp:DropDownList ID="ddlVendor" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td >
                        </td>
                    <td colspan="3" style="height: 19px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%; height: 19px;">
                        GRN No. </td>
                    <td style="width: 3%; height: 19px;">
                        <asp:ImageButton ID="ImgIssueNO" runat="server" Height="17px" 
                            ImageUrl="~/Images/LookUpNew.png" OnClick="ImgIssueNO_Click" Width="17px" />
                    </td>
                    <td >
                        <asp:TextBox ID="txtGRNNo" runat="server" ReadOnly="true"></asp:TextBox>
                        <cc1:MaskedEditExtender ID="txtIssueNo_MaskedEditExtender" runat="server" 
                            ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" 
                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                            CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                            ErrorTooltipEnabled="True" Mask="99/99999" MaskType="None" 
                            TargetControlID="txtGRNNo" />
                    </td>
                    <td >
                       </td>
                    <td style="height: 19px;" colspan="3">
                        &nbsp;</td>
                </tr>
                <tr>
                    
                    <td >
                        Remarks
                    </td>
                    <td></td>
                    <td colspan="4" >
                         <asp:TextBox ID="txtRemarks" runat="server" MaxLength="100" Width="500px"></asp:TextBox>
                    </td>
                </tr>
                </table>
                <table width="100%">
                <tr valign="middle">
                    <td>
                        <asp:Label ID="Label1" runat="server" SkinID="Heading" Text="Item Details"></asp:Label>
                    </td>
                </tr>
                <tr>
                <td>
                <asp:TextBox ID="txtPartItem" runat="server" Visible="false"></asp:TextBox>
                </td>
                </tr>
                </table>
               
         
            <table width="100%">
                <tr>
                    <td style="width: 13%">
                        Part Item*
                    </td>
                    <td style="width: 3%; height: 21px;">
                        <asp:ImageButton ID="BtnPartItem" runat="server" Height="17px" ImageUrl="~/Images/LookUpNew.png"
                            Width="17px" OnClick="BtnPartItem_Click" />
                    </td>
                    <td style="width: 15%">
                        <asp:TextBox ID="txtItemCode" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <%-- <td colspan="4">
                        <table border="0" cellpadding="0" width="100%" cellspacing="0">
                            <tr>--%>
                    <td style="width: 39%" colspan="2">
                        <asp:TextBox ID="txtPartItemDesc" CssClass="ROTextBoxes" runat="server" Width="341px"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td align="right" style="width: 10%">
                        Unit
                    </td>
                    <td>
                        <asp:TextBox ID="txtUnit" CssClass="ROTextBoxes" runat="server" Width="82px" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtUnitCode" runat="server" Width="30px" Visible="False"></asp:TextBox>
                    </td>
                    <%--</tr>
                        </table>
                    </td>--%>
                </tr>
                <tr>
                    <td>
                        Part Loc.*
                    </td>
                    <td>
                        &nbsp;</td>
                    <td style="width: 90px">
                        <asp:TextBox ID="txtLocDesc" runat="server" ReadOnly="true" CssClass="ROTextBoxes"></asp:TextBox>
                    </td>
                    <%-- <td colspan="4">
                        <table border="0" cellpadding="0" width="100%" cellspacing="0">
                            <tr>
                                <td colspan="2">
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>--%>
                    <td align="right">
                    <asp:TextBox ID="txtLocCode" runat="server" Visible="False"></asp:TextBox>

                        GRN Qty.
                    </td>
                    <td style="width: 194px">
                        <asp:TextBox ID="txtIssueQty" runat="server" Width="82px" ReadOnly="true" CssClass="ROTextBoxes"
                            Style="text-align: right"></asp:TextBox>
                    </td>
                    <td style="width: 210px" align="right">
                        Remaining
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemaining" runat="server" CssClass="ROTextBoxes" ReadOnly="true"
                            Style="text-align: right" Width="82px"></asp:TextBox>
                    </td>
                    <%--</tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>--%>
                </tr>
                <tr>
                    <td>
                        Total Stock Qty
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <%--<td colspan="5">
                        <table width="100%">
                            <tr>--%>
                    <td style="width: 140px">
                        <asp:TextBox ID="txtTotalStock" runat="server" CssClass="ROTextBoxes"
                            Style="text-align: right"></asp:TextBox>
                    </td>
                    <td align="right">
                        Stock in Location
                    </td>
                    <td style="width: 9%">
                        <asp:TextBox ID="txtStockLoc" runat="server" CssClass="ROTextBoxes" ReadOnly="true"
                            Style="text-align: right" Width="82px"></asp:TextBox>
                    </td>
                    <td align="right" style="width: 245px">
                        Return Qty
                    </td>
                    <td>
                        <%--ontextchanged="txtIssueQty_TextChanged1"--%>
                        <asp:TextBox ID="txtRetQty" runat="server"  Style="text-align: right" Width="82px"
                            MaxLength="4" onblur="calc()" ></asp:TextBox>
                             <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" 
                                            FilterType="Numbers" TargetControlID="txtRetQty">
                                        </cc1:FilteredTextBoxExtender>
                    </td>
                    <%--</tr>
                        </table>
                    </td>--%>
                </tr>
                <tr>
                    <td>
                        Amount</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtTotalAmt" runat="server" CssClass="ROTextBoxes" Style="text-align: right"
                            Enabled="False"></asp:TextBox>
                    </td>
                    <td align="right">
                        Rate
                    </td>
                    <td>
                        <asp:TextBox ID="txtRate" runat="server" CssClass="ROTextBoxes" ReadOnly="true" 
                            Style="text-align: right" Width="82px"></asp:TextBox>
                    </td>
                    <td align="right">
                        Average Cost</td>
                    <td align="left" >
                        <asp:TextBox ID="txtAvgCost" runat="server" CssClass="ROTextBoxes" 
                            ReadOnly="true" Style="text-align: right" Width="82px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        
                        &nbsp;</td>
                    <td>
                        
                    </td>
                    <td>
                       
                        &nbsp;</td>
                    <td align="right" colspan="4">
                        <asp:Label ID="lblSNo" runat="server"></asp:Label>
                        <asp:Button ID="BtnAdd" runat="server" OnClick="BtnAdd_Click" Text="Add" />
                        <asp:Button ID="BtnClear1" runat="server" OnClick="BtnClear1_Click" Text="Clear" />
                    </td>
                </tr>
            </table>
            <asp:GridView ID="GvMain" runat="server" AutoGenerateColumns="False" Width="100%"
                OnRowDataBound="GvMain_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                            <asp:LinkButton ID="BtnSelect" runat="server" OnClick="BtnSelect_Click">Select</asp:LinkButton>
                            <asp:HiddenField ID="HdLoctionCode" Value='<%# Bind("LocationCode") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ItemCode">
                        <ItemTemplate>
                            <asp:Label ID="LbItemCode" runat="server" Text='<%# Bind("ItemCode") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ItemCode") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PartItemNo">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("PartItemName") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("PartItemName") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Part Item Name">
                        <ItemTemplate>
                            <asp:Label ID="LbPartDesc" runat="server" Text='<%# Bind("PartItemName") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("PartItemName") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit">
                        <ItemTemplate>
                            <asp:Label ID="LbUnit" runat="server" Text='<%# Bind("Unit") %>'></asp:Label>
                            <asp:HiddenField ID="hdUnitCode" Value='<%# Bind("UnitCode") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Unit") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Return Qty.">
                        <ItemTemplate>
                            <asp:Label ID="LbRetQty" runat="server" Text='<%# Bind("IssueQty") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("IssueQty") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Amount">
                        <ItemTemplate>
                            <asp:Label ID="LbAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("Amount") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
                            <asp:Label ID="LbRate" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("Rate") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Location">
                        <ItemTemplate>
                            <asp:Label ID="LbPartsTransLocCode" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                            <asp:HiddenField ID="HDPartsTransLocCode" Value='<%# Bind("LocationCode") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("Location") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Avg Cost.">
                        <ItemTemplate>
                            <asp:Label ID="LbAvgCost" runat="server" Text='<%# Bind("AvgRate") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("AvgRate") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remove">
                        <ItemTemplate>
                            <asp:LinkButton ID="BtnRemove" runat="server" OnClick="BtnRemove_Click">Remove</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>


    <table width ="100%" > 
     <tr align="right" >
    
     <td style ="width :60%">Total Qty</td>
     <td>
        <asp:TextBox ID="txtTotalQty" runat="server" ReadOnly ="true" CssClass="ROTextBoxes" Style="text-align: right" ></asp:TextBox>
     </td>
     <td>Total Amount</td>

        <td>
            <asp:TextBox ID="txtTotalAmount" runat="server" ReadOnly ="true" CssClass="ROTextBoxes" Style="text-align: right"></asp:TextBox>
        </td>
        
     </tr>
    </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
