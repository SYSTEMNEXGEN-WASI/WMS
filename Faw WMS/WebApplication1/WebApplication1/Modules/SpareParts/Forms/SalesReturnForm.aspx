<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesReturnForm.aspx.cs" MasterPageFile="~/Root.master" Theme="Theme1"
    Inherits="DXBMS.Modules.SpareParts.Forms.SalesReturnForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<asp:content id="Content1" contentplaceholderid="Content" runat="server">
    <%-- <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>--%>
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
               window.location.href = "SalesReturnForm.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "SalesReturnForm.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "SalesReturnForm.aspx";
           }
           function reloads() {
               window.location.href = "SalesReturnForm.aspx";
           }

       </script>
     
   <script type="text/javascript">
     function calc() {
         var Rate = document.getElementById('<%=txtRate.ClientID%>').value;
         var IssueQty = document.getElementById('<%=txtQty.ClientID%>').value;
         if (Rate == "") {
             Rate = "0.0";
         }
         if (IssueQty == "") {
             IssueQty = "0.0";
         }
         var total = parseFloat(Rate) * parseFloat(IssueQty);
         document.getElementById('<%=txtTotalAmt.ClientID%>').value = parseFloat(total).toFixed(2).toString();
     }

     //function MaxTxtLen() {
     //    $(function () {
     //        //Normal Configuration
     //        $("[id*=txtRemarks]").MaxLength({ MaxLength: 100 });
     //    });
     //}

     function ShowFailurMSG() {
         $(function () {
             $("div.failure").fadeIn(300).delay(1500).fadeOut(400);
         });
     }
    </script>
 <asp:UpdatePanel ID="UpdPanel1" runat="server">
        <ContentTemplate>
        <div class="container">
        <script type="text/javascript">

            //Sys.Application.add_load(MaxTxtLen);
//            Sys.Application.add_load(ShowFailurMSG);
           
            </script>
        
        <div class="Heading" >
           SALES RETURN (SR) (TRANSACTION)
        </div>
        
        <asp:HiddenField ID="HDUnitCode" runat="server" />
                        <asp:HiddenField ID="HDDeleted" runat="server" />
        <fieldset>
        <table style="width: 100%;  border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
        <tr>
        <td style="width: 12%" nowrap="nowrap">Return No.* </td>
    
        <td style="width: 55%">
            <asp:DropDownList ID="ddlSRNO" runat="server" Font-Names="monospace" width="350px"
                AutoPostBack="True" onselectedindexchanged="ddlSRNO_SelectedIndexChanged">
            </asp:DropDownList>
            </td>        
        <td style="width: 9%" nowrap="nowrap">Return Date *</td>
    
        <td>
            <asp:TextBox ID="txtRetDate" runat="server" CssClass="ROTextBoxes" 
                ReadOnly="true" Width="100%" style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"></asp:TextBox>
            <cc1:MaskedEditExtender ID="txtPODate_MaskedEditExtender" runat="server" 
                Mask="99/99/9999" MaskType="Date" TargetControlID="txtRetDate">
            </cc1:MaskedEditExtender>
            </td>        
        </tr>
            <tr>
                <td nowrap="nowrap" >
                    Customer
                    </td>
                <td >
                    <asp:DropDownList ID="ddlCustomer" runat="server" width="90%" Font-Names="monospace" AutoPostBack="True" 
                        onselectedindexchanged="ddlCustomer_SelectedIndexChanged">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlCustomer" QueryPattern="Contains">
                    </cc1:ListSearchExtender>
                    </td>
                    <td></td>           
                
                <td>
                    <asp:RadioButton ID="rdCounterSale" runat="server" GroupName="Type" 
                        Text="Counter Sale" AutoPostBack="True" 
                        oncheckedchanged="rdCounterSale_CheckedChanged" />
                    <asp:RadioButton ID="rdJobCard" runat="server" GroupName="Type" 
                        Text="JobCard" AutoPostBack="True" 
                        oncheckedchanged="rdJobCard_CheckedChanged" />
                </td>
                
                
            </tr>
            <tr>
                <td nowrap="nowrap">
                    Sale Inv No.</td>
                <td>
                    <asp:DropDownList ID="ddlSaleInvNo" runat="server" Font-Names="monospace" 
                        Width="350px" AutoPostBack="True" 
                        onselectedindexchanged="ddlSaleInvNo_SelectedIndexChanged">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ddlSaleInvNo_ListSearchExtender" runat="server" 
                        QueryPattern="Contains" TargetControlID="ddlSaleInvNo">
                    </cc1:ListSearchExtender>
                </td>
                <td nowrap="nowrap">
                    <asp:TextBox ID="txtIssueNo" runat="server" Visible="false"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td nowrap="nowrap">
                    Remarks
                </td>
                <td>
                    <asp:TextBox ID="txtRemarks" runat="server" MaxLength="100" 
                        TextMode="MultiLine" Width="350px"></asp:TextBox>
                </td>
                <td nowrap="nowrap">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
        </fieldset>
           <fieldset >
                    <legend>
                        PARTS</legend>
            <table width="100%" style=" border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                <tr>
                    <td style="width: 12%">
                        Part Item*
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlParts" runat="server" Font-Names="monospace" width="450px"
                            AutoPostBack="True" onselectedindexchanged="ddlParts_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddlParts" QueryPattern="Contains">
                    </cc1:ListSearchExtender>
                    </td>
                    <%-- <td colspan="4">
                        <table border="0" cellpadding="0" width="100%" cellspacing="0">
                            <tr>--%>
                    <td align="right" style="width: 10%">
                        Unit
                    </td>
                    <td>
                        <asp:TextBox ID="txtUnit" SkinID="ROTextBox" runat="server" Width="82px" ReadOnly="True"></asp:TextBox>
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
                        Quantity</td>
                    <td style="width: 194px">
                        <asp:TextBox ID="txtQty" runat="server" Onblur="calc()" 
                            Style="text-align: right" Width="121px"></asp:TextBox>
                    </td>
                    <td style="width: 210px" align="right">
                        
                        Remaining Qty</td>
                    <td>
                        <asp:TextBox ID="txtRemainingQty" runat="server" SkinID="ROTextBox" ReadOnly="true"
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
                        Invoice Quantity</td>
                    <%--<td colspan="5">
                        <table width="100%">
                            <tr>--%>
                    <td style="width: 140px">
                        <asp:TextBox ID="txtInvQuantity" runat="server" SkinID="ROTextBox" Enabled="false" Style="text-align: right"></asp:TextBox>
                    </td>
                    <td align="right">
                        &nbsp;Rate</td>
                    <td style="width: 9%">
                        <asp:TextBox ID="txtRate" runat="server" ReadOnly="true" SkinID="ROTextBox" Style="text-align: right" Width="121px"></asp:TextBox>
                    </td>
                    <td align="right" style="width: 245px">
                        Amount</td>
                    <td>
                        <%--ontextchanged="txtIssueQty_TextChanged1"--%>
                        <asp:TextBox ID="txtTotalAmt" runat="server" Enabled="False" SkinID="ROTextBox" 
                            Style="text-align: right" Width="82px"></asp:TextBox>
                    </td>
                    <%--</tr>
                        </table>
                    </td>--%>
                </tr>
                <tr>
                    <td>
                        Average Cost</td>
                    <td>
                        
                        <%--   Remarks&nbsp;--%>
                        <asp:TextBox ID="txtAvgCost" runat="server" SkinID="ROTextBox"
                            Enabled="false" Style="text-align: right"></asp:TextBox>
                   
                    </td>
                    <td align="right" colspan="4">
                        <asp:TextBox ID="txtItemCode" runat="server" Visible="false"></asp:TextBox>
                        <asp:Label ID="lblSNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:ImageButton ID="BtnAdd" runat="server" Height="45px" 
                            ImageUrl="~/Images/AddIMG.png"  Width="45px" onclick="BtnAdd_Click"  />
                        <asp:ImageButton ID="BtnClear1" runat="server" Height="40px" 
                            ImageUrl="~/Images/ClearImg.png" Width="40px" onclick="BtnClear1_Click" />
                    </td>
                    <td align="right" colspan="4">
                        &nbsp;</td>
                </tr>
            </table>
            <asp:GridView ID="GvMain" runat="server" AutoGenerateColumns="False" Width="100%"
                OnRowDataBound="GvMain_RowDataBound" ShowHeaderWhenEmpty="True" ShowFooter="true">
                <HeaderStyle CssClass="GridHeader" />
                <FooterStyle CssClass="GridFooter" />
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
                    <%--<asp:TemplateField HeaderText="Return Qty.">
                        <ItemTemplate>
                            <asp:Label ID="LbRetQty" runat="server" Text='<%# Bind("IssueQty") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("IssueQty") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
                            <asp:Label ID="LbRate" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("Rate") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Label ID="LbQty" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("Qty") %>'></asp:TextBox>
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
                    
                    <asp:TemplateField HeaderText="Location">
                        <ItemTemplate>
                            <asp:Label ID="LbPartsTransLocCode" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                            <asp:HiddenField ID="HDPartsTransLocCode" Value='<%# Bind("LocationCode") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("Location") %>'></asp:TextBox>
                        </EditItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Avg Cost">
                        <ItemTemplate>
                            <asp:Label ID="LbAvgCost" runat="server" Text='<%# Bind("AvgRate") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("AvgRate") %>'></asp:TextBox>
                        </EditItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remove" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            
                            <asp:ImageButton ID="ImageButton1" runat="server" 
                                ImageUrl="~/Images/Delete_16x16.png"  
                                ToolTip="Remove" onclick="ImageButton1_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                <tr>
                    <td align ="right" >Total Quantity :</td>
                    <td>
                        <%--<asp:Label ID="lblTotalQty" runat="server" ></asp:Label>--%>
                        <asp:TextBox ID="txtTotalQty" runat="server" Enabled="false" SkinID="ROTextBox" Width="60px" style=" text-align:right;"></asp:TextBox>
                    </td>
                    <td align ="right">Total Amount :</td>
                    <td>               
                        <asp:TextBox ID="txtTotalAmount" runat="server" Enabled="false" SkinID="ROTextBox" Width="60px" style=" text-align:right;"> </asp:TextBox>
                    </td>
                </tr>
            </table>
            </fieldset>
            <table width="100%">
                <tr>
                    <td style="width: 40%">
                        
                        <asp:Label ID="LbErr" runat="server" Text=""></asp:Label>
                    <%--<div class="alert-box failure"  ><asp:Label ID="LbErr" runat="server" Text=""></asp:Label></div>--%>
                    </td>
                    <td align="center">
                     <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*)Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
                        <asp:Button ID="btnPosting" runat="server" Text="Save" OnClick="btnPosting_Click"
                            OnClientClick="this.disabled = true; this.value = 'Please Wait';" UseSubmitBehavior="false" />
                        <asp:Button ID="BtnSave" runat="server" Text="Save_blocked"  OnClientClick="this.disabled = true; this.value = 'Please Wait';"
                            UseSubmitBehavior="false" Visible="false" />
                        &nbsp;&nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                        &nbsp;<asp:Button ID="BtnPrint" runat="server" Text="Print" OnClick="BtnPrint_Click" />
                        &nbsp; <asp:Button ID="btnGL" runat="server" Text="GL" OnClick="btnGL_Click"/>

                    </td>
                </tr>
                
            </table>
           </div>
        </ContentTemplate>
</asp:UpdatePanel>
</asp:content>
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
</html>
--%>