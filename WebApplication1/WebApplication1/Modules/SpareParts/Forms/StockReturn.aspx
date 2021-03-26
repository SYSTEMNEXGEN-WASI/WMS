<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockReturn.aspx.cs" Inherits="DXBMS.Modules.SpareParts.Forms.StockReturn" Theme="Theme1" MasterPageFile="~/Root.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
               window.location.href = "StockReturn.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "StockReturn.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "StockReturn.aspx";
           }
           function reloads() {
               window.location.href = "StockReturn.aspx";
           }

       </script>
<script type="text/javascript">
    function calc() {
        var Rate = document.getElementById('<%=txtIssueRate.ClientID%>').value;
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
            
            <div class="Heading">
                STOCK RETURN
            </div>
            <fieldset>
            <table style="width: 100%; border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover" >
                
                <tr>
                    <td style="width: 10%">
                        Return No.*
                    </td>
                    <td style="width: 500px">
                        <asp:DropDownList ID="ddlStockReturnNo" runat="server" AutoPostBack="True" width="400px" Font-Names="monospace"
                            onselectedindexchanged="ddlStockReturnNo_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddlStockReturnNo"
                            PromptText="" QueryPattern="Contains">
                        </cc1:ListSearchExtender>
                    </td>
                    <td style="width: 12%">
                        Return Date *
                    </td>
                    <td>
                        <asp:TextBox ID="txtRetDate" runat="server"  ReadOnly="true" Style="background-image: url(../../../Images/Calendar_16x16.png);
                            background-repeat: no-repeat; padding-left: 25px; width: 100%;"
                            ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td >
                        Issue No.
                    </td>
                    <td >
                        <asp:DropDownList ID="ddlIssueNo" runat="server" width="400px" Font-Names="monospace"
                            onselectedindexchanged="ddlIssueNo_SelectedIndexChanged" 
                            AutoPostBack="True">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlIssueNo"
                            PromptText="" QueryPattern="Contains">
                        </cc1:ListSearchExtender>
                    </td>
                    <td >
                        Remarks</td>
                    <td >
                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="100" 
                            TextMode="MultiLine" Width="400px"></asp:TextBox>
                        <asp:TextBox ID="txtPartItemDesc" runat="server" ReadOnly="True" Visible="false"
                            SkinID="ROTextBox" Width="341px"></asp:TextBox>
                    </td>
                </tr>

            </table>
            </fieldset>
            <fieldset>
                <legend>RETURN PARTS FROM STOCK</legend>
            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                <tr>
                    <td style="width: 15%">
                        Part Item*
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlPartsNo" runat="server" Width="450px" Font-Names="monospace"
                            AutoPostBack="True" onselectedindexchanged="ddlPartsNo_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddlPartsNo"
                            PromptText="" QueryPattern="Contains">
                        </cc1:ListSearchExtender>
                    </td>
                    <%-- <td colspan="4">
                        <table border="0" cellpadding="0" width="100%" cellspacing="0">
                            <tr>--%>
                    <td align="right" style="width: 10%">
                        Unit
                    </td>
                    <td>
                        <asp:TextBox ID="txtUnit" runat="server" Width="82px" ReadOnly="True" 
                            SkinID="ROTextBox"></asp:TextBox>
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
                    <td style="width: 90px">
                        <asp:TextBox ID="txtLocDesc" runat="server" ReadOnly="true" SkinID="ROTextBox"></asp:TextBox>
                    </td>
                    <%-- <td colspan="4">
                        <table border="0" cellpadding="0" width="100%" cellspacing="0">
                            <tr>
                                <td colspan="2">
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>--%>
                    <td align="right">
                    <asp:TextBox ID="txtLocCode" runat="server" Visible="False"></asp:TextBox>

                        Issue Qty.
                    </td>
                    <td style="width: 194px">
                        <asp:TextBox ID="txtIssueQty" runat="server" Width="82px" ReadOnly="true"
                            Style="text-align: right" SkinID="ROTextBox"></asp:TextBox>
                    </td>
                    <td style="width: 210px" align="right">
                        Remaining
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemaining" runat="server" ReadOnly="true"
                            Style="text-align: right" Width="82px" SkinID="ROTextBox"></asp:TextBox>
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
                        Total Stock in hand
                    </td>
                    <%--<td colspan="5">
                        <table width="100%">
                            <tr>--%>
                    <td style="width: 140px">
                        <asp:TextBox ID="txtTotalStock" runat="server" Enabled="false"
                            Style="text-align: right" SkinID="ROTextBox"></asp:TextBox>
                    </td>
                    <td align="right">
                        Stock in Location
                    </td>
                    <td style="width: 9%">
                        <asp:TextBox ID="txtStockLoc" runat="server" ReadOnly="true"
                            Style="text-align: right" Width="82px" SkinID="ROTextBox"></asp:TextBox>
                    </td>
                    <td align="right" style="width: 245px">
                        Return Qty
                    </td>
                    <td>
                        <%--ontextchanged="txtIssueQty_TextChanged1"--%>
                        <asp:TextBox ID="txtRetQty" runat="server" Style="text-align: right" Width="82px"
                            MaxLength="4" onblur="calc()"></asp:TextBox>
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
                        Amount
                    </td>
                    <td>
                        <asp:TextBox ID="txtTotalAmt" runat="server" Style="text-align: right"
                            Enabled="False" SkinID="ROTextBox"></asp:TextBox>
                    </td>
                    <td align="right">
                        Issue Rate
                    </td>
                    <td>
                        <asp:TextBox ID="txtIssueRate" runat="server" ReadOnly="true"
                            Style="text-align: right" Width="82px" SkinID="ROTextBox"></asp:TextBox>
                    </td>
                    <td align="right" colspan="2">
                        <asp:TextBox ID="txtPartItemNo" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>
                         <asp:Label ID="lblSNo" runat="server"></asp:Label>
                        <asp:Button ID="BtnAdd" runat="server" OnClick="BtnAdd_Click" Text="Add" />
                        <asp:Button ID="BtnClear1" runat="server" OnClick="BtnClear1_Click" Text="Clear" />
                    </td>
                   
                </tr>
               
            </table>
            <asp:GridView ID="GvMain" runat="server" AutoGenerateColumns="False" Width="100%" ShowFooter="true"
                OnRowDataBound="GvMain_RowDataBound" ShowHeaderWhenEmpty="true" 
                    onrowcancelingedit="GvMain_RowCancelingEdit" onrowediting="GvMain_RowEditing" 
                    onrowupdating="GvMain_RowUpdating">
                <HeaderStyle CssClass="GridHeader" />
                                <FooterStyle CssClass="GridFooter" />
                <Columns>
                <asp:CommandField ShowEditButton="True" ButtonType="Image"  CancelImageUrl="~/Images/Grid_Cancel.png" EditImageUrl="~/Images/Grid_Edit.png" UpdateImageUrl="~/Images/Grid_Update.png" />
                    
                    <asp:TemplateField HeaderText="ItemCode">
                        <ItemTemplate>
                            <asp:Label ID="LbItemCode" runat="server" Text='<%# Bind("ItemCode") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                        <asp:Label ID="lblEditItemCode" runat="server" Text='<%# Bind("ItemCode") %>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PartItemNo">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("PartItemNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Part Item Name">
                        <ItemTemplate>
                            <asp:Label ID="LbPartDesc" runat="server" Text='<%# Bind("PartItemName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit">
                        <ItemTemplate>
                            <asp:Label ID="LbUnit" runat="server" Text='<%# Bind("Unit") %>'></asp:Label>
                            <asp:HiddenField ID="hdUnitCode" Value='<%# Bind("UnitCode") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remaining Qty.">
                        <ItemTemplate>
                            <asp:Label ID="lblRemainingQty" runat="server" Text='<%# Bind("RemainingQty") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblEditRemainingQty" runat="server" Text='<%# Bind("RemainingQty") %>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Return Qty.">
                        <ItemTemplate>
                            <asp:Label ID="LbRetQty" runat="server" Text='<%# Bind("ReturnQty") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditRetQty" runat="server" Text='<%# Bind("ReturnQty") %>' Width="50px"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                        <asp:Label ID="lblFooterTotalQty" runat="server" ></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Amount">
                        <ItemTemplate>
                            <asp:Label ID="LbAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                        <asp:Label ID="lblEditAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        <asp:Label ID="lblFooterTotalAmount" runat="server" ></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
                            <asp:Label ID="LbRate" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblEditRate" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Location">
                        <ItemTemplate>
                            <asp:Label ID="LbPartsTransLocCode" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                            <asp:HiddenField ID="HDPartsTransLocCode" Value='<%# Bind("LocationCode") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remove" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                                                    <asp:ImageButton ID="BtnRemovePart" runat="server" ImageUrl="~/Images/Delete_16x16.png"
                                                ToolTip="Remove" onclick="BtnRemovePart_Click"  />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </fieldset>
           <table width="100%" style="margin-bottom: 10px;margin-top:10px;">
                <tr>
                 <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*) Sterik Feilds Are Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
                    <td >
                        <asp:Label ID="LbErr" runat="server" Text=""></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Button ID="btnPosting" runat="server" Text="Save" OnClick="btnPosting_Click"
                            OnClientClick="this.disabled = true; this.value = 'Please Wait';" UseSubmitBehavior="false" />
                        &nbsp;&nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                        &nbsp;<%--<asp:Button ID="BtnPrint" runat="server" Text="Print" OnClick="BtnPrint_Click" />--%>&nbsp;</td>
                </tr>
            </table>
            </div>
            <%-- </td>
                </tr>
            </table>
            <table width="100%" a>
                <tr align="right">
                    <td style="width: 85%">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
            </table>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
