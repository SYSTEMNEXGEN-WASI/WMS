<%@ Page Language="C#" Theme="Theme1" AutoEventWireup="true" CodeBehind="GL.aspx.cs" Inherits="DXBMS.Modules.Service.Forms.GL" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Content/Grid.css" rel="stylesheet" />
    <%--<link href="../../../Content/Style.css" rel="stylesheet" />--%>
    <link href="../../../css/style.css" rel="stylesheet" />
    <link href="../../../css/chosen.css" rel="stylesheet" />
    <style>
        body{
            font-family: "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
            font-size: 12px;
        }

        .makespace{
            margin-bottom : 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
		</asp:ScriptManager>
            <div class="container" style="margin-top: 30px;">
                <div class="row">
                    <fieldset class="makespace">
                    <legend>Voucher Type</legend>

                    <div class="row">
                        <div class="col-lg-12">
                            <asp:RadioButtonList ID="RBLTransType"  runat="server" AutoPostBack="True" Font-Bold="true" RepeatDirection="Horizontal"
                                            OnSelectedIndexChanged="RBLTransType_SelectedIndexChanged"    width="100%">
                                                <asp:ListItem Value="CR" Selected="True">Cash Receipt</asp:ListItem>
                                                <asp:ListItem Value="CP">Cash Payment</asp:ListItem> 
                                                <asp:ListItem Value="BR">Bank Receipt</asp:ListItem>
                                                <asp:ListItem Value="BP">Bank Payment</asp:ListItem>
                                            </asp:RadioButtonList>  
                        </div>
                    </div>
                </fieldset>
                <fieldset style="width: 60%; float: left;" class="makespace">
                   <table style="width:100%">
                       <tr>
                           <td style="width:15%;" >Journal No.</td>
                           <td>
                               <asp:DropDownList ID="ddlJournalNo" runat="server" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlJournalNo_SelectedIndexChanged"></asp:DropDownList>
                           </td>
                       </tr>
                   </table>
                 </fieldset>

                <div style="width: 35%;float: right;text-align: center;padding-top: 10px;">
                    <asp:Label runat="server" id="lblText" Text="Add Mode" Font-Size="X-Large" ForeColor="Blue" Font-Bold="true"></asp:Label>
                </div>

                <fieldset style="width: 60%;" class="makespace">
                    <table style="border-collapse:separate; border-spacing:5px; width:100%;" class="table table-condensed table-bordered table-hover">
                       <tr>
                           <td>Voucher No.</td>
                           <td><asp:TextBox id="txtVoucherNo" runat="server" ReadOnly="true"></asp:TextBox></td>
                           <td>Voucher Date</td>
                           <td>
                               <asp:TextBox id="txtVoucherDate" runat="server" AutoComplete="off"  Style="background-image: url(../../../Images/Calendar_16x16.png); 
                                background-repeat: no-repeat; padding-left: 25px;" OnTextChanged="txtVoucherDate_TextChanged" AutoPostBack="true" CausesValidation="false"  ></asp:TextBox>
                               <ajax:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MM-yyyy" TargetControlID="txtVoucherDate" ></ajax:CalendarExtender>
                           </td>
                       </tr>
                        <tr>
                           <td>Cheque No.</td>
                           <td><asp:TextBox id="txtChqNo" runat="server" ReadOnly="true"></asp:TextBox></td>
                           <td>Cheque Date</td>
                           <td>
                               <asp:TextBox id="txtChqDate" runat="server" ReadOnly="true" Style="background-image: url(../../../Images/Calendar_16x16.png); 
                                background-repeat: no-repeat; padding-left: 25px;" ></asp:TextBox>
                           </td>
                       </tr>
                   </table>
                </fieldset>

                 <fieldset class="makespace">

                    <table  style="border-collapse:separate; border-spacing:5px; width:100%;" class="table table-condensed table-bordered table-hover">                                        
                    <tr>                       
                        <td colspan="6">
                            <asp:GridView ID="gvJV"  runat="server" AutoGenerateColumns="false" Width="100%" CellPadding="3"  Font-Size="9pt" CssClass="Grid input-lg">
                                <Columns>
                                    <asp:BoundField HeaderText="Account Code" DataField="AccountCode" >
                                        <ItemStyle  Width="15%"/>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Account Title" DataField="AccountTitle" >
                                        <ItemStyle  Width="20%"/>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Naration" DataField="Naration"  >
                                        <ItemStyle  Width="49%" HorizontalAlign="Left"/>
                                    </asp:BoundField>  
                                    <asp:BoundField HeaderText="Debit" DataField="Debit" >
                                        <ItemStyle  Width="8%" HorizontalAlign="Right"/>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Credit" DataField="Credit"  >
                                        <ItemStyle  Width="8%" HorizontalAlign="Right"/>
                                    </asp:BoundField>  
                                    
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <FooterStyle CssClass="GridFooter" />
                            </asp:GridView>
                        </td>                       
                    </tr>                                    
                </table>
                </fieldset>
                <fieldset style="width: 45%;float:left;">
                    <table style="width:100%">
                        <tr>
                            <td colspan="2">Are you sure you want to generate the voucher?</td>
                        </tr>
                        <tr>
                            <td style="width:50%; float:right"><asp:Button runat="server" UseSubmitBehavior="false" ID="btnYes" Text="Yes" OnClick="btnYes_Click"  OnClientClick="this.disabled = true; this.value = 'Please Wait';"/></td>
                            <td style="width:50%"><asp:Button runat="server" ID="btnNo" Text="No" OnClientClick="javascript:window.close()"/></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblMsg" runat="server" Width="100%"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset style="width: 48%; float: right;">
                    <table style="width:100%">
                        <tr>
                            <td style="width:66%;padding-right:10px;text-align:right;">Total</td>
                            <td style="width:17%"><asp:TextBox ID="txtTotalDebit" runat="server" style="text-align:right; width:100%;"></asp:TextBox></td>
                            <td style="width:17%"><asp:TextBox ID="txtTotalCredit" runat="server" style="text-align:right; width:100%;"></asp:TextBox></td>
                        </tr>
                    </table>
                </fieldset>
                </div>
                
            </div>
    </form>
</body>
<script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
<script src="../../../../js/chosen.jquery.js" type="text/javascript"></script>
	<script src="../../../../js/chosen.jquery.min.js" type="text/javascript"></script>

<script>
    $(document).ready(function () {

        $('select').chosen({ width: '100%' });

        $('.chosen-container.chosen-container-single').css({ "width": "100%" });

    })
</script>

</html>
