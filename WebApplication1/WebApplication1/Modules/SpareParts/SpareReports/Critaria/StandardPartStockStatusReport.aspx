<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="StandardPartStockStatusReport.aspx.cs" Inherits="DXBMS.Modules.Reports.StandardPartStockStatusReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">

    <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>

    <div class="container">
        <center>
    <div class="Heading">STANDARD PART STOCK STATUS REPORT</div>
        <dx:ASPxLabel ID="lblErr" runat="server" Text="" ForeColor="#669900">
        </dx:ASPxLabel>
    <dx:ASPxFormLayout ID="chkAllItems" runat="server" Width="1000px">
        <Items>
            <dx:LayoutGroup ColCount="4" ShowCaption="False">
                <Items>
                    
                    <dx:LayoutItem ShowCaption="False" Width="25%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer  runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btnPrintLocRpt" runat="server" 
                                    Text="Stock Report Location Wise" Width="100%" OnClick="btnPrintLocRpt_Click" >
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem ShowCaption="False" Width="25%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btnPrint" runat="server" Text="Stock Report" Width="100%" 
                                    OnClick="btnPrint_Click">
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem ShowCaption="False" Width="25%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btnClear" runat="server" Text="Clear" Width="100%" 
                                    OnClick="btnClear_Click">
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:EmptyLayoutItem Width="25%">
                    </dx:EmptyLayoutItem>
                </Items>
            </dx:LayoutGroup>
            <dx:LayoutGroup Caption="From Code" ColCount="3">
                <Items>
                    <dx:LayoutItem ShowCaption="False">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxCheckBox ID="chkAllItem" runat="server" AutoPostBack="True" 
                                    Checked="True" CheckState="Checked" 
                                    OnCheckedChanged="chkAllItem_CheckedChanged" Width="100%">
                                </dx:ASPxCheckBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Part Item" Width="25%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxGridLookup ID="glFromItemPartNo" runat="server" 
                                    AutoGenerateColumns="False" AutoPostBack="True" ClientEnabled="False" 
                                    DataSourceID="SqlDataSourceItem" KeyFieldName="PartItemNo" 
                                    OnTextChanged="glFromItemPartNo_TextChanged" TextFormatString="{1}" 
                                    Width="100%">
                                    <GridViewProperties EnableCallBacks="False">
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" 
                                            AllowSelectSingleRowOnly="True" EnableRowHotTrack="True" />
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="True" 
                                            ShowFilterRowMenuLikeItem="True" ShowFooter="True" />
                                    </GridViewProperties>
                                    <Columns>
                                        <dx:GridViewDataTextColumn FieldName="ItemCode" ReadOnly="True" 
                                            ShowInCustomizationForm="True" VisibleIndex="0">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="PartItemNo" 
                                            ShowInCustomizationForm="True" VisibleIndex="1">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="ItemDesc" ShowInCustomizationForm="True" 
                                            VisibleIndex="2">
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                </dx:ASPxGridLookup>
                                <asp:SqlDataSource ID="SqlDataSourceItem" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                                    SelectCommand="select * from Item where DealerCode=@DealerCode">
                                    <SelectParameters>
                                        <asp:SessionParameter DefaultValue="00001" Name="DealerCode" 
                                            SessionField="DealerCode" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Description">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtPartItemDescFrom" runat="server" Width="100%">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
            <dx:LayoutGroup Caption="To Code" ColCount="3">
                <Items>
                    <dx:EmptyLayoutItem>
                    </dx:EmptyLayoutItem>
                    <dx:LayoutItem Caption="Part Item" Width="25%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxGridLookup ID="glToItemPartNo" runat="server" 
                                    AutoGenerateColumns="False" AutoPostBack="True" ClientEnabled="False" 
                                    DataSourceID="SqlDataSourceItem" KeyFieldName="PartItemNo" 
                                    OnTextChanged="glToItemPartNo_TextChanged" TextFormatString="{1}" Width="100%">
                                    <GridViewProperties EnableCallBacks="False">
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" 
                                            AllowSelectSingleRowOnly="True" EnableRowHotTrack="True" />
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="True" 
                                            ShowFilterRowMenuLikeItem="True" />
                                    </GridViewProperties>
                                    <Columns>
                                        <dx:GridViewDataTextColumn FieldName="ItemCode" ReadOnly="True" 
                                            ShowInCustomizationForm="True" VisibleIndex="0">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="PartItemNo" 
                                            ShowInCustomizationForm="True" VisibleIndex="1">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="ItemDesc" ShowInCustomizationForm="True" 
                                            VisibleIndex="2">
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                </dx:ASPxGridLookup>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Description">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtPartItemDescTo" runat="server" Width="100%">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
            <dx:LayoutGroup ShowCaption="False">
                <Items>
                    <dx:LayoutItem ShowCaption="False">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxRadioButtonList ID="radioItemType" runat="server" 
                                    RepeatDirection="Horizontal" Width="100%" SelectedIndex="5">
                                    <Items>
                                        <dx:ListEditItem Text="Local" Value="Local" />
                                        <dx:ListEditItem Text="C.K.D." Value="C.K.D." />
                                        <dx:ListEditItem Text="Market Parts" Value="In - House" />
                                        <dx:ListEditItem Text="Lubes" Value="Lubes" />
                                        <dx:ListEditItem Text="HGO" Value="HGO" />
                                        <dx:ListEditItem Text="All" Value="--Select--" Selected="True" />
                                    </Items>
                                </dx:ASPxRadioButtonList>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
        </Items>
    </dx:ASPxFormLayout>
    </center>
    </div>
</asp:Content>
