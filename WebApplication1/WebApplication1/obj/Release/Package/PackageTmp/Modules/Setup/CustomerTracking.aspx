<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="CustomerTracking.aspx.cs" Inherits="DXBMS.Modules.Setup.CustomerTracking" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <link href="../../css/TabStyle.css" rel="stylesheet" />

     <style type="text/css">

    .mGrid { 
    width: 100%; 
    background-color: #fff; 
    margin: 5px 0 10px 0; 
    border: solid 1px #525252; 
    border-collapse:collapse; 
}
.mGrid td { 
    padding: 2px; 
    border: solid 1px #c1c1c1; 
    color: #717171; 
}
.mGrid th { 
    padding: 4px 2px; 
    color: #fff; 
    background: #424242 repeat-x top; 
    border-left: solid 1px #525252; 
    font-size: 0.9em; 
}
.mGrid .alt { background: #fcfcfc repeat-x top; }
.mGrid .pgr { background: #424242 repeat-x top; }
.mGrid .pgr table { margin: 5px 0; }
.mGrid .pgr td { 
    border-width: 0; 
    padding: 0 6px; 
    border-left: solid 1px #666; 
    font-weight: bold; 
    color: #fff; 
    line-height: 12px; 
 }   
.mGrid .pgr a { color: #666; text-decoration: none; }
.mGrid .pgr a:hover { color: #000; text-decoration: none; }

image{
    float: right;
}

    </style>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">

        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div class="col-lg-12">
                        <fieldset>
                            <legend> Customer Tracking </legend>

                            <div class="table-responsive">


                                <table width="100%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">                                   
                                    <tr>
                                        <td >Customer
                                            <%--<asp:ImageButton runat="server" ID="imgCustomer" CssClass="image" ImageUrl="~/Images/LookupNew.png" OnClick="imgCustomer_Click"/>--%>
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtCustomer" runat="server" MaxLength="50" Width="70%" ></asp:TextBox>
                                        </td>
                                        <td >Cell No
                                            <%--<asp:ImageButton runat="server" ID="ImageButton1" CssClass="image" ImageUrl="~/Images/LookupNew.png" />--%>
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtCellNo" runat="server" MaxLength="11" Width="70%" ></asp:TextBox>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td >CNIC
                                            <%--<asp:ImageButton runat="server" ID="ImageButton2" ImageUrl="~/Images/LookupNew.png" />--%>
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtCNIC" runat="server"  MaxLength="15" Width="70%" ></asp:TextBox>
                                        </td>
                                        <td >NTN
                                            <%--<asp:ImageButton runat="server" ID="ImageButton3" ImageUrl="~/Images/LookupNew.png" />--%>
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtNTN" runat="server"  MaxLength="15" Width="70%" ></asp:TextBox>
                                        </td>
                                    </tr>  
                                    
                                    <tr>
                                        <td >Chassis No
                                            <%--<asp:ImageButton runat="server" ID="imgChasisNo" ImageUrl="~/Images/LookupNew.png" OnClick="imgChasisNo_Click"/>--%>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtChasisNo" runat="server"  MaxLength="20" Width="70%" ></asp:TextBox>
                                        </td>
                                        <td>Engine No
                                            <%--<asp:ImageButton runat="server" ID="ImageButton5" ImageUrl="~/Images/LookupNew.png" />--%>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEngineNo" runat="server"  MaxLength="20" Width="70%" ></asp:TextBox>
                                        </td>
                                    </tr>        
                                    <tr>
                                        <td >Reg No
                                            <%--<asp:ImageButton runat="server" ID="ImageButton6" ImageUrl="~/Images/LookupNew.png" />--%>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRegno" runat="server"  MaxLength="15" Width="70%" ></asp:TextBox>
                                        </td>
                                        <td>Product
                                            <%--<asp:ImageButton runat="server" ID="ImageButton7" ImageUrl="~/Images/LookupNew.png" />--%>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProduct" runat="server"  MaxLength="50" Width="70%" ></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Grand Total</td>
                                        <td>
                                            <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button id="btnSearch" Text="Search" runat="server" OnClick="txtCustomer_TextChanged"/>
                                            <asp:Button ID="btnClear" Text="Clear" runat="server" OnClick="btnClear_Click" />
                                        </td>
                                    </tr>

                                </table>


                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>Details</legend>
                            <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Width="100%" CssClass="TabStyle">
                                <asp:TabPanel ID="tab1" runat="server"><HeaderTemplate>Customer & Vehicle</HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:GridView ID="gvCusVeh" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                            Width="100%"  ForeColor="Black" BackColor="White" CssClass="mGrid">
                                            <Columns>
                                                <asp:BoundField DataField="Customer" HeaderText ="Customer"/>
                                                <asp:BoundField DataField="CellNo" HeaderText ="Cell No"/>
                                                <asp:BoundField DataField="Regno" HeaderText="Reg No"/>
                                                <asp:BoundField DataField="ChassisNo" HeaderText="Chassis No"/>
                                                <asp:BoundField DataField="EngineNo" HeaderText="Engine No"/>
                                                <asp:BoundField DataField="ProdCode" HeaderText="Product"/>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="tab2" runat="server"><HeaderTemplate>JobCard</HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:GridView ID="gvJobCard" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                            Width="100%"  ForeColor="Black" BackColor="White" CssClass="mGrid">
                                            <Columns>
                                                <asp:BoundField DataField="JobcardCode" HeaderText ="JobCard"/>
                                                <asp:BoundField DataField="JobTypeDesc" HeaderText ="JobCard Type"/>
                                                <asp:BoundField DataField="UserName" HeaderText="End User"/>
                                                <asp:BoundField DataField="ChassisNo" HeaderText="Chassis No"/>
                                                <asp:BoundField DataField="EngineNo" HeaderText="Engine No"/>
                                                <asp:BoundField DataField="JobTotal" HeaderText="JobCard Total"/>
                                                <asp:BoundField DataField="ProdDesc" HeaderText="Product"/>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="tab3" runat="server"><HeaderTemplate>Counter Sales</HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:GridView ID="gvCounterSale" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                            Width="100%"  ForeColor="Black" BackColor="White" CssClass="mGrid">
                                            <Columns>
                                                <asp:BoundField DataField="SaleInvNo" HeaderText ="Sale Invoice No"/>
                                                <asp:BoundField DataField="CusDesc" HeaderText ="Customer"/>
                                                <asp:BoundField DataField="TotReceipt" HeaderText="Total Receipt"/>
                                                <asp:BoundField DataField="PaymentType" HeaderText="Payment Type"/>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="tab4" runat="server"><HeaderTemplate>Payment Receipt</HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:GridView ID="gvPaymentReceipt" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                            Width="100%"  ForeColor="Black" BackColor="White" CssClass="mGrid">
                                            <Columns>
                                                <asp:BoundField DataField="ReceiptNo" HeaderText ="Receipt No"/>
                                                <asp:BoundField DataField="CusDesc" HeaderText ="Customer"/>
                                                <asp:BoundField DataField="InvoiceType" HeaderText="Invoice Type"/>
                                                <asp:BoundField DataField="PayModeDesc" HeaderText="Payment Type"/>
                                                <asp:BoundField DataField="InvTotal" HeaderText="Invoice Total"/>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="tab5" runat="server"><HeaderTemplate>Balance Amount</HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:GridView ID="gvBalanceAmount" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                            Width="100%"  ForeColor="Black" BackColor="White" CssClass="mGrid">
                                            <Columns>
                                                <asp:BoundField DataField="CusDesc"  HeaderText ="Customer"/>
                                                <asp:BoundField DataField="InvoiceNo"  HeaderText ="Invoice"/>
                                                <asp:BoundField DataField="CustomerOustanding" HeaderText ="Customer Oustanding"/>
                                                <asp:BoundField DataField="InsuranceOustanding" HeaderText="Insurance Oustanding"/>
                                                <asp:BoundField DataField="SaleInvNo" HeaderText="Sale Invoice"/>
                                                <asp:BoundField DataField="DirectSale" HeaderText="DirectSales Outstanding"/>
                                            </Columns>
                                        </asp:GridView>
                                        <table width="100%" style="border-collapse: separate; border-spacing: 5px;"
                                            class="table table-condensed table-bordered table-hover">    
                                            <tr>
                                                <td>Customer Total</td>
                                                <td><asp:Label ID="lblCusOut" runat="server"></asp:Label></td>
                                                <td>Insurance Total</td>
                                                <td><asp:Label ID="lblInsOut" runat="server"></asp:Label></td>
                                                <td>DirectSales Total</td>
                                                <td><asp:Label ID="lblDirectSales" runat="server"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:TabPanel>
                            </asp:TabContainer>
                        </fieldset>

                    </div>
                </div>
            </div>

        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>
