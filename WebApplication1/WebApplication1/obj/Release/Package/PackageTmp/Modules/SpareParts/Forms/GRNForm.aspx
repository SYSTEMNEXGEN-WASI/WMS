<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GRNForm.aspx.cs" Inherits="DXBMS.Modules.SpareParts.Forms.GRNForm" MasterPageFile="~/Root.master" Theme="Theme1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <script type="text/javascript" src="../../../Jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../../Jquery/MaxLength.min.js"></script>
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
               window.location.href = "GRNForm.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "GRNForm.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "GRNForm.aspx";
           }
           function reloads() {
               window.location.href = "GRNForm.aspx";
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
    <asp:UpdatePanel ID="UpdPanel1" runat="server">
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

<div class="Heading" >
            Goods Receipt and inspection Note(GRN)
        </div>

    <table  width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
        
        <tr>
            <td colspan="5">
              
                <asp:HiddenField ID="HDGVIndex" runat="server" />
                <asp:HiddenField ID="HDDelete" runat="server" />
                <%--<asp:ScriptManager ID="ScriptManager1" runat="server" 
                    AsyncPostBackTimeout="400">
                </asp:ScriptManager>--%>
            </td>
        </tr>
       
        <tr>
            <td style="width:10%">
                GRN No.*<asp:ImageButton runat="server" ID="imgGRNLookup" ImageUrl="~/Images/LookupNew.png" onclick="imgGRNLookup_Click"/>
            </td>
            <td width="60%">
                <asp:DropDownList ID="ddlGRNNo" runat="server" AutoPostBack="True" Font-Names="monospace" Width="85%" Enabled="false" onselectedindexchanged="ddlGRNNo_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:ListSearchExtender ID="ddlGRNNO_ListSearchExtender" runat="server" 
                    QueryPattern="Contains" TargetControlID="ddlGRNNo">
                </asp:ListSearchExtender>
            </td>
            <td style="width: 10%">
                GRN Date*</td>
            <td width="20%">

               
                <asp:TextBox ID="txtGRNDate" runat="server" ReadOnly="true" style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 100%" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
               D/C No:<asp:ImageButton runat="server" ID="imgDCLookup" ImageUrl="~/Images/LookupNew.png" onclick="imgDCLookup_Click"/> 
             
            </td>
            
            <td >
                <asp:DropDownList ID="ddlDCNo" runat="server" Font-Names="monospace" Width="85%" AutoPostBack="True" Enabled="false" onselectedindexchanged="ddlDCNo_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlDCNo" QueryPattern="Contains">
            </asp:ListSearchExtender>
            </td>
            <td style="width: 10%">
                D/C Date *</td>
            <td>
                <asp:TextBox ID="txtDCDate" runat="server" Enabled="false" style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width:100%;" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                P/O No.</td>
            <td>
                <asp:TextBox ID="txtPONO" runat="server" SkinID="ROTextBox" Enabled="false"></asp:TextBox>
            </td>

            <td style="width: 10%">
                PO Date </td>
            <td>
                <asp:TextBox ID="txtPODate" runat="server" Enabled="false" style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width:100%" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                P/O Type
            </td>
            <td>
                <asp:TextBox ID="txtPOType" runat="server" Enabled="false"  SkinID="ROTextBox"></asp:TextBox>
            </td>
            
            <td style="width: 10%">
                Invoice Date</td>
            <td>
                <asp:TextBox ID="txtInvDate" runat="server"
                    style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 100%"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtInvDate" Format="dd-MM-yyyy">
                </asp:CalendarExtender>
                <%--<asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtInvDate" Mask="99-99-9999" MaskType="Date" />--%>
            </td>
        </tr>
        <tr>
            <td>
                Vendor
            </td>
            <td>
                <asp:TextBox ID="txtVendorDesc" runat="server" SkinID="ROTextBox"
                    Enabled="false"></asp:TextBox>          
                     
                <asp:TextBox ID="txtVendorCode" runat="server" CssClass="ROTextBoxes" ReadOnly="True" Visible="false"></asp:TextBox>
                     
            </td>
            <td>
                Inv Ref No.</td>
            <td>
                <asp:TextBox ID="txtInvRefNo" runat="server"  width= "100%" MaxLength="15"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Doc No.</td>
            <td>
                <asp:TextBox ID="txtDocNo" runat="server" MaxLength="8"></asp:TextBox>
            </td>
            
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                Remarks
            </td>
            <td colspan="4">
               
                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="100" TextMode="MultiLine" Width="400px"></asp:TextBox>
                     
            </td>
        </tr>
        <%--        <tr>
            <td colspan="7">
                <asp:Label ID="Label2" runat="server" CssClass="Heading" Text="Item Details"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Part Item*
            </td>
            <td>
                &nbsp;
            </td>
            <td style="width: 137px">
                <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>
            </td>
            <td colspan="2">
                <asp:TextBox ID="TextBox11" runat="server" Width="300px"></asp:TextBox>
            </td>
            <td style="width: 78px">
                Unit
            </td>
            <td>
                <asp:TextBox ID="TextBox24" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Location*
            </td>
            <td>
                <asp:ImageButton ID="ImgLookupLocation" runat="server" Height="17px" ImageUrl="~/Images/LookUpNew.png"
                    Width="17px" />
            </td>
            <td style="width: 137px">
                <asp:TextBox ID="TextBox13" runat="server"></asp:TextBox>
            </td>
            <td colspan="2">
                <asp:TextBox ID="TextBox14" runat="server" Width="300px"></asp:TextBox>
            </td>
            <td style="width: 78px">
                Rate
            </td>
            <td>
                <asp:TextBox ID="TextBox25" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Accepted Qty
            </td>
            <td>
            </td>
            <td style="width: 137px">
                <asp:TextBox ID="TextBox16" runat="server"></asp:TextBox>
            </td>
            <td colspan="5">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td style="width: 108px">
                            D/C Qty
                        </td>
                        <td style="width: 96px">
                            <asp:TextBox ID="TextBox17" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <td style="width: 82px">
                            Rejected Qty
                        </td>
                        <td style="width: 124px">
                            <asp:TextBox ID="TextBox18" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <td style="width: 90px">
                            Net Rate
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox19" runat="server" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                GST %*
            </td>
            <td>
            </td>
            <td style="width: 137px">
                <asp:TextBox ID="TextBox20" runat="server"></asp:TextBox>
            </td>
            <td colspan="5">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td style="width: 108px">
                            GST Amount
                        </td>
                        <td style="width: 95px">
                            <asp:TextBox ID="TextBox21" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <td style="width: 82px">
                            W.H.T %age
                        </td>
                        <td style="width: 125px">
                            <asp:TextBox ID="TextBox22" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <td style="width: 90px">
                            W.H.T Amount
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox23" runat="server" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <b>Net Price<b />
            </td>
            <td colspan="5">
                <asp:TextBox ID="TextBox26" runat="server" Width="300px"></asp:TextBox>
            </td>
            <td align="right">
                <asp:Button ID="BtnAdd" runat="server" Text="Add" />
                <asp:Button ID="BtnClear1" runat="server" Text="Clear" />
            </td>
        </tr>
        <tr>
            <td colspan="7">
              
            </td>
        </tr>--%>
    </table>
    <%--<div id="div2" style="overflow: scroll; height: 300px; width: 99%">--%>
        <table width="100%">
            <tr>
                <td>
                    <asp:GridView ID="GvMain" runat="server" AutoGenerateColumns="False" Width="100%"
                        Font-Size="9pt" ShowFooter="true" OnRowCancelingEdit="GvMain_RowCancelingEdit" 
                        OnRowEditing="GvMain_RowEditing" 
                         OnRowDataBound="GvMain_OnRowDataBound" onrowupdating="GvMain_RowUpdating">
                            <HeaderStyle CssClass="GridHeader" />
                            <FooterStyle CssClass="GridFooter" />
                        <Columns>
                            <%--<asp:BoundField HeaderText="ExcelRateOnStock" Visible="False" />--%>
                            
                            <asp:CommandField ShowEditButton="True"  CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                  UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false" >
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:CommandField>
                                                    
                            <asp:TemplateField HeaderText="Item Code" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("ItemCode") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditItemCode" runat="server" Text='<%# Bind("ItemCode") %>'></asp:Label>
                                </EditItemTemplate>
                               
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Part Item No">
                                <ItemTemplate>
                                    <asp:Label ID="lblPartItemNo" runat="server" Text='<%# Bind("PartItemNo") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditPartItemNo" runat="server" Text='<%# Bind("PartItemNo") %>'></asp:Label>
                                </EditItemTemplate>
                               
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Part Item Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemDesc"  runat="server" Text='<%# Bind("ItemDesc") %>' ></asp:Label>
                                </ItemTemplate>
                               
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit">
                                <ItemTemplate>
                                    <asp:Label ID="LbUnitDesc" runat="server" Text='<%# Bind("UnitDesc") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GRN Qty">
                                <ItemTemplate>
                                    <asp:Label ID="lblAccQty" runat="server" Text='<%# Bind("AccQty") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtAcceptedQty" Text='<%# Bind("AccQty") %>' runat="server" Width="60px" MaxLength="5"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtAcceptedQty_FilteredTextBoxExtender" 
                                        runat="server" FilterType="Custom, Numbers" TargetControlID="txtAcceptedQty" 
                                        ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalAccQty" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rate">
                                <ItemTemplate>
                                    <asp:Label ID="lblRate" runat="server" Text='<%# Bind("Rate") %>' ></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                <asp:TextBox ID="txtEditRate" Text='<%# Bind("Rate") %>' runat="server" Width="60px" MaxLength="8"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="txtEditRate_FilteredTextBoxExtender" 
                                        runat="server" FilterType="Custom" TargetControlID="txtEditRate" 
                                        ValidChars="0123456789.">
                                    </asp:FilteredTextBoxExtender>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Location">
                                <ItemTemplate>
                                    
                                    <asp:Label ID="lblLocationCode"  runat="server" Text='<%# Bind("PartsTransLocCode") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblLocationName" runat="server" Text='<%# Bind("PartsTransLocDesc") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlLocation" runat="server">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" D/C Qty.">
                                <ItemTemplate>
                                    <asp:Label ID="lblDCQty" runat="server" Text='<%# Bind("DCQty") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                <asp:Label ID="lblEditDCQty" runat="server" Text='<%# Bind("DCQty") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="G.S.T %">
                                <ItemTemplate>
                                    <asp:Label ID="LBGSTage" runat="server" Text='<%# Bind("GSTage") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                <asp:Label ID="lblEditGSTage" runat="server" Text='<%# Bind("GSTage") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="G.S.T Amt.">
                                <ItemTemplate>
                                    <asp:Label ID="LbGSTAmount" runat="server" Text='<%# Bind("GSTAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="LbEditGSTAmount" runat="server" Text='<%# Bind("GSTAmount") %>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalGSTAmt" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ex.Tax %">
                                <ItemTemplate>
                                    <asp:Label ID="lblExTaxPerc" runat="server" Text='<%# Bind("ExTaxPerc") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditExTaxPerc" runat="server" Text='<%# Bind("ExTaxPerc") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ex. Tax Amt">
                                <ItemTemplate>
                                    <asp:Label ID="lblExTaxAmt" runat="server" Text='<%# Bind("ExTaxAmt") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditExTaxAmt" runat="server" Text='<%# Bind("ExTaxAmt") %>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalExAmt" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Fur.Tax %">
                                <ItemTemplate>
                                    <asp:Label ID="lblFurTaxPerc" runat="server" Text='<%# Bind("PFGSTPERC") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditFurTaxPerc" runat="server" Text='<%# Bind("PFGSTPERC") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fur Tax Amt">
                                <ItemTemplate>
                                    <asp:Label ID="lblFurTaxAmt" runat="server" Text='<%# Bind("PFGSTAmt") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                <asp:Label ID="lblEditFurTaxAmt" runat="server" Text='<%# Bind("PFGSTAmt") %>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalFurAmt" runat="server"></asp:Label>
                                </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Disc %">
                                <ItemTemplate>
                                    <asp:Label ID="lblDiscountPerc" Text='<%# Bind("DiscountPerc") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditDiscountPerc" Text='<%# Bind("DiscountPerc") %>' runat="server"></asp:Label>
                                </EditItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Discount Amt">
                                <ItemTemplate>
                                    <asp:Label ID="lblDiscount" Text='<%# Bind("Discount") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditDiscount" Text='<%# Bind("Discount") %>' runat="server"></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalDiscount" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Net Amount">
                                <ItemTemplate>
                                    <asp:Label ID="LbAmountMain" Text='<%# Bind("NetAmount") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                <asp:Label ID="LbEditAmountMain" Text='<%# Bind("NetAmount") %>' runat="server"></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblFooterTotalAmt" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    <%--</div>--%>
    <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
        <tr>
            <td Style="text-align:right;">
                Other Charges
            </td>
            <td width="140px">
                <asp:TextBox ID="txtOtherChargers" runat="server"  style="text-align:right" MaxLength ="6" onblur="calc()"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtOtherChargers_FilteredTextBoxExtender" 
                    runat="server" FilterType="Custom, Numbers" TargetControlID="txtOtherChargers" 
                    ValidChars=".">
                </asp:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td Style="text-align:right;">
                Freight Charges
            </td>
            <td width="140px">
                <asp:TextBox ID="txtFreightCharges" runat="server"  style="text-align:right" MaxLength ="6" onblur="calc()"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                    runat="server" FilterType="Custom, Numbers" TargetControlID="txtFreightCharges" 
                    ValidChars=".">
                </asp:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">
                Total Amount</td>
            <td width="140px" >
                <asp:TextBox ID="GridNetAmount" runat="server" Enabled="false" style="text-align:right" SkinID="ROTextBox"></asp:TextBox>
            
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">
                <b>Net Amount</b>&nbsp;</td>
            <td >
                <asp:TextBox ID="txtNetAmount" runat="server" Enabled="false"
                     style="text-align:right" SkinID="ROTextBox"></asp:TextBox>
            
            </td>
        </tr>
    </table>
  <table width="100%" style="margin-bottom: 10px;margin-top:10px;">
 <tr>
  <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*) Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
 <td >
   <asp:Label ID="LbErr" runat="server"  Text=""></asp:Label>
 </td>
          <td style="text-align: right;">
                <asp:Button ID="btnPosting" runat="server" Text="Stock" 
                    onclick="btnPosting_Click" Width="100px" OnClientClick="this.disabled = true; this.value = 'Please Wait';" UseSubmitBehavior="false"/>
                <asp:Button ID="BtnSave" Width="100px" runat="server" Text="Save Blocked" Visible="false" onclick="BtnSave_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait';" UseSubmitBehavior="false"/>
                &nbsp;<asp:Button ID="BtnDelete" Width="100px" runat="server" Text="Delete" Visible="false"
                    onclick="BtnDelete_Click" />
                &nbsp;<asp:Button ID="btnClear" Width="100px" runat="server" Text="Clear" 
                    onclick="btnClear_Click" />
                &nbsp;<asp:Button ID="BtnPrint" Width="100px" runat="server" Text="Print"
                    onclick="BtnPrint_Click" />
                
                &nbsp;<asp:Button ID="btnPost" Width="100px" runat="server" Text="Post"
                    onclick="btnPost_Click" />
                &nbsp;</td>
        </tr>
        
        <%--<tr>
            <td colspan="7" align="center">
                <asp:Label ID="Label1" runat="server" skinID="Heading" Text="Goods Receipt and inspection Note(GRN)"></asp:Label>
            </td>
        </tr>--%>
</table>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>