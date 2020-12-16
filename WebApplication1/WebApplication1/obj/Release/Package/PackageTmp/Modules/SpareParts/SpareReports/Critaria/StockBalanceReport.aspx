<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="StockBalanceReport.aspx.cs" Inherits="DXBMS.Modules.Reports.StockBalanceReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    
        <%--<script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>--%>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
               
            <div class="Heading">Stock Balance Report</div>
            <div class="row">
                <div class="col-lg-12">
                <fieldset>
                    <table style="border-collapse: separate; width:100%; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                        <tr>
                            <td width="20%">Vendor :</td>
                            <td width="40%">
                                <asp:DropDownList ID="ddlVendor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVendor" runat="server" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>For Month :</td>
                            <td>
                                 <asp:TextBox ID="txtMonth" runat="server" Height="21px" MaxLength="7" Style="background-image: url(../../../../Images/Calendar_16x16.png);
                                           background-repeat: no-repeat; padding-left: 25px; width: 100%;" ></asp:TextBox>
                                 <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="MM/yyyy"
                                     TargetControlID="txtMonth" DefaultView="Months">
                                 </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Button ID="ASPxButton1" runat="server" Text="Show" OnClick="btnShow_Click">
                                </asp:Button>
                            </td>
                            
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>        
                
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%--<dx:ASPxLabel ID="lblMSG" runat="server" Text="">
        </dx:ASPxLabel>
    <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" Width="700px">
        <Items>
            <dx:LayoutGroup ColCount="2" ShowCaption="False">
                <Items>
                    <dx:LayoutItem Caption="Vendor">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer1" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxComboBox ID="cmbVendorCode" runat="server" 
                                    DataSourceID="SqlDataSourceVendors" TextField="VendorDesc" 
                                    TextFormatString="{0}" ValueField="VendorCode" Width="200px" 
                                    AutoPostBack="True" 
                                    OnSelectedIndexChanged="cmbVendorCode_SelectedIndexChanged">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="Code" FieldName="VendorCode" />
                                        <dx:ListBoxColumn Caption="Vendor Description" FieldName="VendorDesc" />
                                    </Columns>
                                </dx:ASPxComboBox>
                                <asp:SqlDataSource ID="SqlDataSourceVendors" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                                    SelectCommand="select * from Vendor where DealerCode in ('AAAAA',@DealerCode)">
                                    <SelectParameters>
                                        <asp:SessionParameter DefaultValue="000001" Name="DealerCode" 
                                            SessionField="DealerCode" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem ShowCaption="False" HorizontalAlign="Left">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer2" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtVendorName" runat="server" Width="100%" ReadOnly="True">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="For Month">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer3" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtForMonth" runat="server" Width="200px">
                                    <MaskSettings Mask="00/0000" />
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem ShowCaption="False" VerticalAlign="Middle" 
                        HorizontalAlign="Left">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer4" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btnShow" runat="server" Text="Show" Width="100%" 
                                    OnClick="btnShow_Click">
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
        </Items>
    </dx:ASPxFormLayout>--%>
