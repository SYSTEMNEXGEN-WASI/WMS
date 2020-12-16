
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseReturn.aspx.cs" Theme="Theme1"
    Inherits="DXBMS.Modules.SpareParts.Forms.PurchaseReturn" MasterPageFile="~/Root.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>--%>
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
               window.location.href = "PurchaseReturn.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "PurchaseReturn.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "PurchaseReturn.aspx";
           }
           function reloads() {
               window.location.href = "PurchaseReturn.aspx";
           }

       </script>

<script type="text/javascript">
    function calc() {
    debugger
        var Rate = document.getElementById('<%=txtRate.ClientID%>').value;
        var IssueQty = document.getElementById('<%=txtRetQty.ClientID%>').value;
        var Gst = document.getElementById('<%=txtGstPerc.ClientID%>').value;
        var FGST = document.getElementById('<%=txtFGSTPerc.ClientID%>').value;
        var Extra = document.getElementById('<%=txtExTaxPerc.ClientID%>').value;

        if (Rate == "") {
            Rate = "0.0";
        }
        if (IssueQty == "") {
            IssueQty = "0.0";
        }
        if (Gst == "") {
            Gst = "0.0";
        }
        if (FGST == "") {
            FGST = "0.0";
        }
        if (Extra == "") {
            Extra = "0.0";
        }

        var total = parseFloat(Rate) * parseFloat(IssueQty);
        var GstAmount = (total * parseFloat(Gst)) / 100;
        var FGSTAmount = (total * parseFloat(FGST)) / 100;
        var ExtraAmount = (total * parseFloat(Extra)) / 100;

        document.getElementById('<%=txtTotalAmt.ClientID%>').value = parseFloat(total).toFixed(2).toString();
        document.getElementById('<%=txtGstAmount.ClientID%>').value = parseFloat(GstAmount).toFixed(2).toString();
        document.getElementById('<%=txtFGSTAmount.ClientID%>').value = parseFloat(FGSTAmount).toFixed(2).toString();
        document.getElementById('<%=txtExTaxAmount.ClientID%>').value = parseFloat(ExtraAmount).toFixed(2).toString();

        $("#hfGST").val(parseFloat(GstAmount).toFixed(2).toString());
        $("#hfFGST").val(parseFloat(FGSTAmount).toFixed(2).toString());
        $("#ExTax").val(parseFloat(ExtraAmount).toFixed(2).toString());
    }

    //function MaxTxtLen() {
    //    $(function () {
    //        //Normal Configuration
    //        $("[id*=txtRemarks]").MaxLength({ MaxLength: 100 });
    //    });
    //}
    function blinker() {
        $('.blinking').fadeOut(500);
        $('.blinking').fadeIn(500);
    }
    setInterval(blinker, 2000);
    </script>
    <style type="text/css">
        .blinking
        {
             height:15px;
             text-align:left;
             
        }
    </style>
    <asp:UpdatePanel ID="UpdPanel1" runat="server">
        <ContentTemplate>
            <div class="container">
        <script type="text/javascript">

            //Sys.Application.add_load(MaxTxtLen);
            </script>
         <div class="row">
                <div class="col-lg-12">
              
                <div class="col-md-4 pull-right" style="margin-top:10px;">
                <asp:Label runat="server" ID="lblMode" Font-Size="16px" Font-Bold="true" ForeColor="Black"> </asp:Label>
                </div>
                </div>
                </div>
             <div class="Heading" >
            PURCHASE RETURNE (PR) (TRANSACTION)
        </div>

        <fieldset>
            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                <tr>
                    <td style="width: 10%">
                        PR No.*<asp:ImageButton runat="server" ID="ImageButtonPR" ImageUrl="~/Images/LookupNew.png" OnClick="ImageButtonPR_Click" Width="16px" AccessKey="i"  /> 
                    </td>
                    <td style="width: 25%">
                        <asp:DropDownList ID="ddlPR" runat="server" AutoPostBack="True" Font-Names="monospace" Width ="300px"
                            onselectedindexchanged="ddlPR_SelectedIndexChanged" Enabled="false">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlPR" PromptText=""
                                QueryPattern="Contains">
                            </cc1:ListSearchExtender>
                    </td>
                    <td style="width: 8%">
                        PR Date *
                    </td>
                    <td>
                        <asp:TextBox ID="txtRetDate" runat="server"  ReadOnly="true" style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;" ></asp:TextBox>
                       
                        <%--<cc1:MaskedEditExtender ID="txtPODate_MaskedEditExtender" runat="server" Mask="99/99/9999"
                            MaskType="Date" TargetControlID="txtRetDate">
                        </cc1:MaskedEditExtender>--%>
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%; height: 19px;">
                        Vendor</td>
                    <td >
                        <asp:DropDownList ID="ddlVendor" runat="server" AutoPostBack="True" width="300px"
                            onselectedindexchanged="ddlVendor_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td >
                        GRN No.
                        </td>
                    <td style="height: 19px">
                        <asp:DropDownList ID="ddlGRNNo" runat="server" AutoPostBack="True" Font-Names="monospace" Width="300px"
                            onselectedindexchanged="ddlGRNNo_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddlGRNNo" PromptText=""
                                QueryPattern="Contains">
                            </cc1:ListSearchExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        Remarks</td>
                    <td colspan="3" >
                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="100" Width="320px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                </table>
                </fieldset><%--<div class="Heading">PARTS
                </div>--%><fieldset >
                    <legend>PARTS</legend>
                <table width="100%">
                <tr>
                <td>
                    <asp:TextBox ID="txtItemCode" runat="server" ReadOnly="true" Visible="False"></asp:TextBox>
                </td>
                </tr>
                </table>
               
         
            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                <tr>
                    <td style="width: 13%">
                        Search Parts*</td>
                    <td colspan="6">
                        <asp:DropDownList ID="ddlParts" runat="server" AutoPostBack="True" Font-Names="monospace" Width="510px"
                            onselectedindexchanged="ddlParts_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddlParts" PromptText=""
                                QueryPattern="Contains">
                            </cc1:ListSearchExtender>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td >Part Item No.</td>
                    <td style="width: 12%">
                        <asp:TextBox ID="txtPartItem" runat="server"></asp:TextBox>
                    </td>                    
                    <td colspan="4" style="width: 39%">
                        <asp:TextBox ID="txtPartItemDesc" runat="server" SkinID="ROTextBox" 
                            ReadOnly="True" Width="352px"></asp:TextBox>
                    </td>
                    <td align="right" style="width: 12%">
                        Unit
                    </td>
                    <td style="width: 13%">
                        <asp:TextBox ID="txtUnit" runat="server" SkinID="ROTextBox" ReadOnly="True" 
                            Width="100%"></asp:TextBox>
                        <asp:TextBox ID="txtUnitCode" runat="server" Visible="False" Width="30px"></asp:TextBox>
                    </td>                    
                </tr>
                <tr>
                    <td>
                        Part Loc.*
                    </td>
                    <td>
                        <asp:TextBox ID="txtLocDesc" runat="server" ReadOnly="true" SkinID="ROTextBox" Width="100%"></asp:TextBox>
                    </td>                    
                    <td align="right" width="12%">
                    <asp:TextBox ID="txtLocCode" runat="server" Visible="False"></asp:TextBox>

                        GRN Qty.
                    </td>
                    <td style="width: 12%">
                        <asp:TextBox ID="txtIssueQty" runat="server" Width="100%" ReadOnly="true" SkinID="ROTextBox"
                            Style="text-align: right"></asp:TextBox>
                    </td>
                    <td style="width: 12%;" align="right">
                        Remaining
                    </td>
                    <td width="13%">
                        <asp:TextBox ID="txtRemaining" runat="server" SkinID="ROTextBox" ReadOnly="true"
                            Style="text-align: right" Width="100%"></asp:TextBox>
                    </td>
                    <td align="right" >
                        Total Stock Qty
                    </td>
                    
                    <td>
                        <asp:TextBox ID="txtTotalStock" runat="server" SkinID="ROTextBox" ReadOnly="true" Width="100%"
                            Style="text-align: right"></asp:TextBox>
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
                        Stock in Location
                    </td>
                    <td>
                        <asp:TextBox ID="txtStockLoc" runat="server" SkinID="ROTextBox" ReadOnly="true"
                            Style="text-align: right" Width="100%"></asp:TextBox>
                    </td>
                    <td align="right">
                        Return Qty
                    </td>
                    <td>
                        <%--ontextchanged="txtIssueQty_TextChanged1" onblur="txtRetQty_OnTextChanged"--%>
                        <asp:TextBox ID="txtRetQty" runat="server"  Style="text-align: right" Width="100%" onblur="calc()"
                                MaxLength="4"  ></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers" TargetControlID="txtRetQty">
                        </cc1:FilteredTextBoxExtender>
                        
                    </td>
                    <td align="right">GST</td>
                    <td>
                        <asp:TextBox ID="txtGstPerc" runat="server" ReadOnly="true" SkinID="ROTextBox" width="40%" Style="text-align: right"></asp:TextBox>
                        <asp:TextBox ID="txtGstAmount" runat="server" ReadOnly="true" SkinID="ROTextBox" width="57%" Style="text-align: right"></asp:TextBox>
                        <asp:HiddenField runat="server" ID="hfGST"/>
                    </td>
                    <td align="right" >FGST</td>
                    <td>
                        <asp:TextBox ID="txtFGSTPerc" runat="server" ReadOnly="true" SkinID="ROTextBox" width="40%" Style="text-align: right"></asp:TextBox>
                        <asp:TextBox ID="txtFGSTAmount" runat="server" ReadOnly="true" SkinID="ROTextBox" width="57%" Style="text-align: right"></asp:TextBox>
                        <asp:HiddenField runat="server" ID="hfFGST"/>
                    </td>
                    
                </tr>
                <tr>
                    <td>Ex Tax</td>
                    <td>
                        <asp:TextBox ID="txtExTaxPerc" runat="server" ReadOnly="true" SkinID="ROTextBox" width="40%" Style="text-align: right"></asp:TextBox>
                        <asp:TextBox ID="txtExTaxAmount" runat="server" ReadOnly="true" SkinID="ROTextBox" width="57%" Style="text-align: right"></asp:TextBox>
                        <asp:HiddenField runat="server" ID="hfExTax"/>
                    </td>
                    <td align="right">
                        Amount</td>
                    <td>
                        <asp:TextBox ID="txtTotalAmt" runat="server" SkinID="ROTextBox" Style="text-align: right" Width="100%"
                            Enabled="False"></asp:TextBox>
                    </td>
                    <td align="right">
                        Rate
                    </td>
                    <td>
                        <asp:TextBox ID="txtRate" runat="server" SkinID="ROTextBox" ReadOnly="true" Width="100%"
                            Style="text-align: right"></asp:TextBox>
                    </td>
                    <td align="right" nowrap="nowrap">
                        Average Cost</td>
                    <td align="left" >
                        <asp:TextBox ID="txtAvgCost" runat="server"  SkinID="ROTextBox"
                            ReadOnly="true" Style="text-align: right" Width="100%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        
                        <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Images/AddIMG.png" 
                            Width="45px" Height="45px" onclick="BtnAdd_Click1"/>
                        <asp:ImageButton ID="BtnClear1" runat="server"  
                            ImageUrl="~/Images/ClearImg.png" Width="40px" Height="40px" 
                            onclick="BtnClear1_Click1" />
                        
                        <asp:Label ID="lblSNo" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="GvMain" runat="server" AutoGenerateColumns="False" Width="100%"
                OnRowDataBound="GvMain_RowDataBound" ShowHeaderWhenEmpty="True" ShowFooter="true">
                <HeaderStyle CssClass="GridHeader" />
                <FooterStyle CssClass="GridFooter" />
                <Columns>
                    <asp:TemplateField HeaderText="Select" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="BtnSelect" runat="server" >Select</asp:LinkButton>
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
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("PartItemNo") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("PartItemNo") %>'></asp:TextBox>
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
                    <asp:TemplateField HeaderText="GST Amount">
                        <ItemTemplate>
                            <asp:Label ID="lblGstAMount" runat="server" Text='<%# Bind("GSTAmount") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_GstAmount" runat="server" Text='<%# Bind("GSTAmount") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="FGST Amount">
                        <ItemTemplate>
                            <asp:Label ID="lblFGSTAMount" runat="server" Text='<%# Bind("FurTaxAmt") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_FGSTAMount" runat="server" Text='<%# Bind("FurTaxAmt") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ExTax Amount">
                        <ItemTemplate>
                            <asp:Label ID="lblExTaxAMount" runat="server" Text='<%# Bind("ExTaxAmt") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_ExTaxAMount" runat="server" Text='<%# Bind("ExTaxAmt") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remove" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate >
                            <asp:ImageButton ID="BtnRemove" runat="server" 
                                ImageUrl="~/Images/Delete_16x16.png" onclick="BtnRemove1_Click" 
                                ToolTip="Remove" />
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
    </fieldset>
    <table width="100%">
                <tr>
                 <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*) Sterik Feilds Are Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
                    <td style="width: 40%">
                        <p class ="blinking"><asp:Label ID="LbErr" runat="server" Text=""></asp:Label></p>
                    </td>
                    <td align="center">
                        <asp:Button ID="btnPosting" runat="server" Text="Save" OnClick="btnPosting_Click"
                            OnClientClick="this.disabled = true; this.value = 'Please Wait';" UseSubmitBehavior="false" />
                        <asp:Button ID="BtnSave" runat="server" Text="Block Save btn" OnClick="BtnSave_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait';"
                            UseSubmitBehavior="false" Visible="False" />
                        &nbsp;&nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                        &nbsp;<asp:Button ID="BtnPrint" runat="server" Text="Print" OnClick="BtnPrint_Click" />
                          &nbsp; <asp:Button ID="btnGL" runat="server" Text="GL" OnClick="btnGL_Click"/>
                        &nbsp;</td>
                </tr>
                
            </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
