<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="InvoicesList.aspx.cs" Inherits="DXBMS.Modules.Reports.InvoicesList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">

    <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <center>
    <div class="Heading">Invoices Detail Report</div>
            
        <dx:ASPxLabel ID="lblMSG" runat="server" Text="">
        </dx:ASPxLabel>
    <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" Width="700px">
        <Items>
            <dx:LayoutGroup ColCount="2" ShowCaption="False">
                <Items>
                    <dx:LayoutItem Caption="From">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer1" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxDateEdit ID="dtFrom" runat="server" DisplayFormatString="dd/MM/yyyy" 
                                    EditFormat="Custom" EditFormatString="dd/MM/yyyy" 
                                    OnDateChanged="dtFrom_DateChanged">
                                </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem HorizontalAlign="Left" Caption="To">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer2" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxDateEdit ID="dtTo" runat="server" DisplayFormatString="dd/MM/yyyy" 
                                    EditFormat="Custom" EditFormatString="dd/MM/yyyy" 
                                    OnDateChanged="dtTo_DateChanged">
                                </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="For Month" ColSpan="2" HorizontalAlign="Center" 
                        ShowCaption="False">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer3" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btnShowReport" runat="server" OnClick="btnShowReport_Click" 
                                    Text="Show Report">
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
        </Items>
    </dx:ASPxFormLayout>
</center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
