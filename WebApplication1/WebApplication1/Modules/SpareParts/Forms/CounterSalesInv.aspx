<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CounterSalesInv.aspx.cs"
    Inherits="DXBMS.Modules.SpareParts.Forms.CounterSalesInv" MasterPageFile="~/Root.master"
    Theme="Theme1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
     <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../../Jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../../Jquery/MaxLength.min.js"></script>
   
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
               window.location.href = "CounterSalesInv.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "CounterSalesInv.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "CounterSalesInv.aspx";
           }
           function reloads() {
               window.location.href = "CounterSalesInv.aspx";
           }

       </script>
    <script type="text/javascript"> 

        function calc() {
            var OtherCharges = document.getElementById('<%=txtOtherChargers.ClientID%>').value;
            var FreightCharges = document.getElementById('<%=txtFreightCharges.ClientID%>').value;
            var NetAmount = document.getElementById('<%=GridNetAmount.ClientID%>').value;
            if (OtherCharges == "") {
                OtherCharges = "0.0";
            }
            if (FreightCharges == "") {
                FreightCharges = "0.0";
            }
            if (NetAmount == "") {
                NetAmount = "0.0";
            }
            var total = parseFloat(OtherCharges) + parseFloat(NetAmount) + parseFloat(FreightCharges);
            document.getElementById('<%=txtNetAmount.ClientID%>').value = parseFloat(total).toFixed(2).toString();
        }
        function MaxTxtLen() {
            $(function () {
                //Normal Configuration
                $("[id*=txtRemarks]").MaxLength({ MaxLength: 100 });
            });
        }

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
            <script type="text/javascript">

                Sys.Application.add_load(MaxTxtLen);
            </script>  
            <div class="row">
                <div class="col-lg-12">
              
                <div class="col-md-4 pull-right" style="margin-top:10px;">
                <asp:Label runat="server" ID="lblMode" Font-Size="16px" Font-Bold="true" ForeColor="Black"> </asp:Label>
                </div>
                </div>
                </div>          
            <div class="Heading">
                COUNTER SALES INVOICE
            </div>
            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                <tr>
                    <td style="width: 12%" nowrap="nowrap">
                        Counter Sales No*<asp:ImageButton runat="server" ID="ImageButtonDS" ImageUrl="~/Images/LookupNew.png" OnClick="ImageButtonDS_Click" Width="16px" AccessKey="i"  />
                    </td>
                    <td width="57%">
                        <asp:DropDownList ID="ddlCounterNo" runat="server" Font-Names="monospace" Width="83%" Enabled="false"
                            AutoPostBack="True" onselectedindexchanged="ddlCounterNo_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddlCounterNo"
                            PromptText="" QueryPattern="Contains">
                        </asp:ListSearchExtender>
                        <asp:Label ID="lblPostFlag" runat="server" Visible="False"></asp:Label>
                    </td>
                    <td style="width: 13%" nowrap="nowrap">
                        Counter Sales Date
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="txtCounterDate" runat="server" ReadOnly="true" placeholder="dd-MM-yyyy" Style="background-image: url(../../../Images/Calendar_16x16.png);
                            background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        Customer *<asp:ImageButton runat="server" ID="ImageButtonCustomer" ImageUrl="~/Images/LookupNew.png" OnClick="ImageButtonCustomer_Click" Width="16px" AccessKey="i"  />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCustomer" runat="server" Font-Names="monospace" Enabled="false" Width="83%">
                        </asp:DropDownList>
                        <asp:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddlCustomer"
                            PromptText="" QueryPattern="Contains">
                        </asp:ListSearchExtender>
                    </td>
                    <td>
                        Tax Type
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTaxType" runat="server" Width="100%">
                            <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                            <asp:ListItem Text="Filer" Value="Filer"></asp:ListItem>
                            <asp:ListItem Text="Non-Filer" Value="Non-Filer"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Requisition No.
                    </td>
                    <td>
                        <asp:TextBox ID="txtRequisitionNo" runat="server" Enabled="false" SkinID="ROTextBox"
                            Width="80px"></asp:TextBox>
                    </td>
                    <td>
                        Payment Type
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPaymentType" runat="server" Width="100%">
                            <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                            <asp:ListItem Text="Cash" Value="Cash"></asp:ListItem>
                            <asp:ListItem Text="Credit" Value="Credit"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        G.S.T. Invoice No.
                    </td>
                    <td>
                        <asp:TextBox ID="txtGSTInvoice" runat="server" Enabled="false"  SkinID="ROTextBox"
                            Width="80px"></asp:TextBox>
                    </td>
                    <td>
                        Discount %</td>
                    <td>
                        <asp:DropDownList ID="ddlDiscount" runat="server" AutoPostBack="True" Width="100%" 
                            onselectedindexchanged="ddlDiscount_SelectedIndexChanged" >
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Remarks
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="100" TextMode="MultiLine"
                            Width="400px"></asp:TextBox>
                    </td>
                    <td>
                        Voucher No
                    </td>
                    <td>
                        <asp:TextBox ID="txtVoucherNo" runat="server" Enabled="false"  SkinID="ROTextBox" Width="100%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td>
                Receipt Mode
                </td>
                <td>
                 <asp:DropDownList ID="ddlReceiptMode" runat="server" Width="100%">
                           
                            <asp:ListItem Selected="True" Text="Manual" Value="Manual"></asp:ListItem>
                            <asp:ListItem Text="Auto" Value="Auto"></asp:ListItem>
                        </asp:DropDownList>
                </td>
                <td>Receipt No</td>
                <td>
                <asp:TextBox runat="server" Width="100%" ID="txtReceiptNo"></asp:TextBox>
                </td>
                </tr>
            </table>
            <fieldset>
                <legend>PARTS</legend>
                
                <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                    <tr>
                        <td>Item Code<font color="red"><b>*</b></font>
                            <div style="padding-left: 0px; float: right">
                                <asp:ImageButton runat="server"  ID="imgLookup"  ImageUrl="~/Images/LookupNew.png" OnClick="btnLookup_Click" />
                            </div>                            
                        </td>
                        <td width="13%">
                             <asp:TextBox ID="txtItemCode" ReadOnly="true" runat="server" Width="100%"></asp:TextBox>
                        </td>
                        <td>Item No</td>
                        <td width="20%">
                            <asp:TextBox ID="txtItemNo" runat="server" Width="100%"></asp:TextBox>
                        </td>
                        <td width="13%">Item Description</td>
                        <td width="30%">
                            <asp:TextBox ID="txtItemDesc" runat="server" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td> Quantity<font color="red"><b>*</b></font></td>
                        <td>
                            <asp:TextBox ID="txtQuantity" runat="server" MaxLength="3" Style="text-align: right" ValidationGroup="JCParts" Width="100%" onkeypress="return IsOneDecimalPoint(event);"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" BehaviorID="_content_FilteredTextBoxExtender1"
                             FilterType="Custom" ValidChars="0123456789." TargetControlID="txtQuantity" />
                        </td>
                        <td>Price<font color="red"><b>*</b></font></td>
                        <td>
                            <asp:TextBox ID="txtPartPrice" runat="server" MaxLength="6"  Style="text-align: right" ValidationGroup="JCParts" Width="100%">0</asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" BehaviorID="_content_FilteredTextBoxExtender1"
                            FilterType="Numbers, Custom" TargetControlID="txtPartPrice" ValidChars="." />
                        </td>
                        <td colspan="2">
                            <asp:ImageButton ID="BtnAdd" runat="server" Height="30px" ImageUrl="~/Images/Export.png"  Width="30px" onclick="BtnAdd_Click"  />
                        </td>
                    </tr>

                </table>

                <table width="100%">
                    <tr>
                        <td>
                        <%--<div class="GridHeader">
                        <table width="100%" style=" text-align:left"> 
                        <tr>
                        <td style="width:100%">
                        Select Parts
                        </td>
                        <td>
                        <asp:DropDownList ID="ddlParts" runat="server" Font-Names="monospace" AutoPostBack="True" Width="500px"
                                OnSelectedIndexChanged="ddlParts_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlParts"
                                PromptText="" QueryPattern="Contains">
                            </asp:ListSearchExtender>
                            </td>
                        </tr>
                        </table>
                            </div>--%>
                            <asp:GridView ID="GvMain" runat="server" AutoGenerateColumns="False" Width="100%"
                                ShowHeaderWhenEmpty="true" Font-Size="9pt" ShowFooter="true" OnRowDataBound="GvMain_RowDataBound"
                                OnRowEditing="GvMain_RowEditing" OnRowCancelingEdit="GvMain_RowCancelingEdit"
                                OnRowUpdating="GvMain_RowUpdating">
                                <HeaderStyle CssClass="GridHeader" />
                                <FooterStyle CssClass="GridFooter" />
                                <Columns>
                                    <%--<asp:BoundField HeaderText="ExcelRateOnStock" Visible="False" />--%>
                                    <asp:CommandField ShowEditButton="True" ButtonType="Image"  CancelImageUrl="~/Images/Grid_Cancel.png" EditImageUrl="~/Images/Grid_Edit.png" UpdateImageUrl="~/Images/Grid_Update.png" />
                                    <asp:TemplateField HeaderText="Item Code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("ItemCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditItemCode" runat="server" Text='<%# Bind("ItemCode") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemStyle Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Item No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartItemNo" runat="server" Text='<%# Bind("PartItemNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditPartItemNo" runat="server" Text='<%# Bind("PartItemNo") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            
                                        </FooterTemplate>
                                        <ItemStyle Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Item Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemDesc" runat="server" Text='<%# Bind("ItemDesc") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>

                                            
                                            <%--<asp:DropDownList ID="ddlParts" runat="server" Font-Names="monospace" Width="150px" >
                                            </asp:DropDownList>
                                            <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlParts"
                                                PromptText="" QueryPattern="Contains">
                                            </asp:ListSearchExtender>--%>
                                        </FooterTemplate>
                                        <ItemStyle Width="100%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit">
                                        <ItemTemplate>
                                            <asp:Label ID="LbUnitDesc" runat="server" Text='<%# Bind("UnitDesc") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccQty" runat="server" Text='<%# Bind("Qty") %>' Width="100%" style="text-align:right;" ></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtAcceptedQty" Text='<%# Bind("Qty") %>' runat="server" Width="60px" style="text-align:right;"
                                                MaxLength="5"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtAcceptedQty_FilteredTextBoxExtender" runat="server"
                                                FilterType="Numbers" TargetControlID="txtAcceptedQty"></asp:FilteredTextBoxExtender>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                        <table>
                                        <tr>
                                        <td>
                                        
                                            <asp:Label ID="lblTotalAccQty" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <%--<tr>
                                        <td>
                                            <asp:TextBox ID="txtFooterQty" runat="server" Width="50px" placeholder="Qty"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtFooterQty_FilteredTextBoxExtender" 
                                                runat="server" FilterType="Custom, Numbers" TargetControlID="txtFooterQty" 
                                                ValidChars="." />
                                        </td>
                                        </tr>--%>
                                        </table>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRate" runat="server" Text='<%# Bind("SaleRate") %>' style="text-align:right; width:100%"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <%--<asp:Label ID="lblEditRate" runat="server" Text='<%# Bind("SaleRate") %>' style="text-align:right; width:100%"></asp:Label>--%>
                                            <asp:TextBox ID="txtEditRate" Text='<%# Bind("SaleRate") %>' runat="server" Width="50px" placeholder="Rate"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtFooterRate_FilteredTextBoxExtender" 
                                                runat="server" FilterType="Custom, Numbers" TargetControlID="txtEditRate" ValidChars="." />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <%--<asp:TextBox ID="txtFooterRate" runat="server" Width="50px" placeholder="Rate"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtFooterRate_FilteredTextBoxExtender" 
                                                runat="server" FilterType="Custom, Numbers" TargetControlID="txtFooterRate"  />--%>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Disc %">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiscountPerc" Text='<%# Bind("DiscountPerc") %>' runat="server" style="text-align:right; width:100%"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditDiscountPerc" Text='<%# Bind("DiscountPerc") %>' runat="server" style="text-align:right; width:100%"></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discount Amt">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiscount" Text='<%# Bind("DiscountAmt") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditDiscount" Text='<%# Bind("DiscountAmt") %>' runat="server"></asp:Label>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalDiscount" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Net Amt" HeaderStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNetAmount" Text='<%# Bind("NetAmount") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditNetAmount" Text='<%# Bind("NetAmount") %>' runat="server"></asp:Label>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalNetAmount" runat="server"></asp:Label>
                                        </FooterTemplate>
                                        <HeaderStyle Wrap="True" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Rec Qty.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDCQty" runat="server" Text='<%# Bind("RecQty") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditDCQty" runat="server" Text='<%# Bind("RecQty") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="G.S.T %">
                                        <ItemTemplate>
                                            <asp:Label ID="LBGSTage" runat="server" Text='<%# Bind("GSTage") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtGSTPerc" Text='<%# Bind("GSTage") %>' runat="server" Width="50px"
                                                MaxLength="5"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtGSTPerc_FilteredTextBoxExtender" runat="server"
                                                FilterType="Custom, Numbers" ValidChars="." TargetControlID="txtGSTPerc"></asp:FilteredTextBoxExtender>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="G.S.T Amt." HeaderStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="LbGSTAmount" runat="server" Text='<%# Bind("GSTAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="LbEditGSTAmount" runat="server" Text='<%# Bind("GSTAmount") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalGSTAmt" runat="server"></asp:Label>
                                        </FooterTemplate>
                                        <HeaderStyle Wrap="True" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ex.Tax %" HeaderStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExTaxPerc" runat="server" Text='<%# Bind("SalesExTaxPerc") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtExTaxPerc" Text='<%# Bind("SalesExTaxPerc") %>' runat="server"
                                                Width="50px" MaxLength="5"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtExTaxPerc_FilteredTextBoxExtender" runat="server"
                                                FilterType="Custom, Numbers" ValidChars="." TargetControlID="txtExTaxPerc"></asp:FilteredTextBoxExtender>
                                        </EditItemTemplate>
                                        <HeaderStyle Wrap="True" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ex. Tax Amt" HeaderStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExTaxAmt" runat="server" Text='<%# Bind("SalesExTaxAmt") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditExTaxAmt" runat="server" Text='<%# Bind("SalesExTaxAmt") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalExAmt" runat="server"></asp:Label>
                                        </FooterTemplate>
                                        <HeaderStyle Wrap="True" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fur.Tax %" HeaderStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFurTaxPerc" runat="server" Text='<%# Bind("FGSTPERC") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtFurTaxPerc" Text='<%# Bind("FGSTPERC") %>' runat="server" Width="50px"
                                                MaxLength="5"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtFurTaxPerc_FilteredTextBoxExtender" runat="server"
                                                FilterType="Custom, Numbers" ValidChars="." TargetControlID="txtFurTaxPerc">
                                            </asp:FilteredTextBoxExtender>
                                        </EditItemTemplate>
                                        <HeaderStyle Wrap="True" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fur Tax Amt" HeaderStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFurTaxAmt" runat="server" Text='<%# Bind("FGSTAmt") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditFurTaxAmt" runat="server" Text='<%# Bind("FGSTAmt") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalFurAmt" runat="server"></asp:Label>
                                        </FooterTemplate>
                                        <HeaderStyle Wrap="True" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="LbAmountMain" Text='<%# Bind("TotalAmount") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="LbEditAmountMain" Text='<%# Bind("TotalAmount") %>' runat="server"></asp:Label>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblFooterTotalAmt" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remove" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="BtnRemovePart" runat="server" ImageUrl="~/Images/Delete_16x16.png"
                                                ToolTip="Remove" OnClick="BtnRemovePart_Click" />
                                        </ItemTemplate>
                                        
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                    <tr>
                        <td style="width : 100%;">Total Liters</td>
                        <td >
                            <asp:TextBox ID="txtTotalLiters" runat="server" placeholder="0.00"  Style="background-image: url(../../../Images/oil.png);
                            background-repeat: no-repeat; padding-left: 25px; width: 80px; height:20pt; text-align : right" ></asp:TextBox>
                        </td>
                        <td Style="text-align:right;">Other Charges</td>
                        <td width="140px">
                            <asp:TextBox ID="txtOtherChargers" runat="server"  style="text-align:right" MaxLength ="6" onblur="calc()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtOtherChargers_FilteredTextBoxExtender" runat="server" FilterType="Custom, Numbers" TargetControlID="txtOtherChargers" 
                                ValidChars="."></asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        
                        <td Style="text-align:right;"  colspan="3">Freight Charges</td>
                        <td width="140px">
                            <asp:TextBox ID="txtFreightCharges" runat="server"  style="text-align:right" MaxLength ="6" onblur="calc()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Custom, Numbers" TargetControlID="txtFreightCharges" 
                                ValidChars="."></asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:right;" colspan="3">Total Amount</td>
                        <td width="140px" >
                            <asp:TextBox ID="GridNetAmount" runat="server" Enabled="false" style="text-align:right" SkinID="ROTextBox"></asp:TextBox>            
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:right;" colspan="3"><b>Net Amount</b>&nbsp;</td>
                        <td >
                            <asp:TextBox ID="txtNetAmount" runat="server" Enabled="false" style="text-align:right" SkinID="ROTextBox"></asp:TextBox>
            
                        </td>
                    </tr>
                </table>
            </fieldset>
           <table width="100%" style="margin-bottom: 10px;margin-top:10px;">
                <tr>
                 <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*) Sterik Feilds Are Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
                    <td>
                        <asp:Label ID="LbErr" runat="server" Text=""></asp:Label>
                    </td>
                     <td style="text-align: right;">
                        <asp:Button ID="BtnSave" Width="100px" runat="server" Text="Save" OnClientClick="this.disabled = true; this.value = 'Please Wait';"
                            UseSubmitBehavior="false" onclick="BtnSave_Click" />
                        &nbsp;
                               <asp:Button ID="BtnDelete" runat="server" Width="100px" Text="Delete" OnClientClick=" return confirm('Are you sure to delete?');"
                            OnClick="BtnDelete_Click" />
                        &nbsp;<asp:Button ID="btnClear" Width="100px" runat="server" Text="Clear" 
                            onclick="btnClear_Click" />
                        &nbsp;<asp:Button ID="BtnPrint" Width="100px" runat="server" Text="Print" 
                            onclick="BtnPrint_Click" />
                        &nbsp;<asp:Button ID="btnPosting" Width="100px" runat="server" Text="Post" OnClientClick="this.disabled = true; this.value = 'Please Wait';"
                            UseSubmitBehavior="false" onclick="btnPosting_Click" />
                            &nbsp;<asp:Button ID="btnGatePass" Width="100px" runat="server" Text="GatePass" OnClientClick="this.disabled = true; this.value = 'Please Wait';"
                            UseSubmitBehavior="false" onclick="btnGatePass_Click"
                             />
                        <asp:Button ID="btnGL" Width="100px" runat="server" Text="GL" OnClick="btnGL_Click"/>
                    </td>
                </tr>
            </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    
<script type="text/javascript">

function IsOneDecimalPoint(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode; // restrict user to type only one . point in number
                var parts = evt.srcElement.value.split('.');
                if(parts.length > 1 && charCode==46)
                    return false;
                return true;
            }
</script>
</asp:Content>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

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
</html>--%>
