<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="SparePartsLocations.aspx.cs" Inherits="DXBMS.Modules.SpareParts.SparePartsLocations" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">

    <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>

    <dx:ASPxPanel ID="ASPxPanel2" runat="server" Width="100%" Height="100%" 
        FixedPosition="WindowTop" ClientInstanceName="topPanel" Collapsible="true" 
        ScrollBars="Vertical" >

        <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
<div style="width:50%; margin-right: auto; margin-left: auto;">
<center>
<h2>Spare Parts Locations</h2>
    <dx:ASPxFormLayout ID="FormLayoutTop" runat="server" Theme="Office2010Blue" 
        Width="100%">
        <Items>
            <dx:LayoutGroup ColCount="3" HorizontalAlign="Center" ShowCaption="False">
                <Items>
                    <dx:LayoutItem HorizontalAlign="Center" ShowCaption="False">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btnDelete" runat="server" Enabled="False" Text="Delete" 
                                    Visible="False">
                                    <Image Url="~/Images/Remove_16x16.png">
                                    </Image>
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem HorizontalAlign="Center" ShowCaption="False">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btnClear" runat="server" OnClick="btnClear_Click" 
                                    Text="Clear">
                                    <Image Url="~/Images/Clear_16x16.png">
                                    </Image>
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem HorizontalAlign="Center" ShowCaption="False">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btnSave" runat="server" Enabled="False" 
                                    OnClick="btnSave_Click" Text="Save" Visible="False">
                                    <Image Url="~/Images/Ribbon_Save_16x16.png">
                                    </Image>
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
            <dx:LayoutGroup Caption="Main Location" ColCount="2">
                <Items>
                    <dx:LayoutGroup ColCount="2" ShowCaption="False" Width="250px">
                        <Items>
                            <dx:LayoutItem Caption="Code" Width="80px">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server" 
                                        SupportsDisabledAttribute="True">
                                        <dx:ASPxTextBox ID="txtLocCode" runat="server" AutoPostBack="True" 
                                            OnTextChanged="txtLocCode_TextChanged" Width="75px" CssClass="uppercase" 
                                            MaxLength="2">
                                            <MaskSettings Mask="&lt;00..99&gt;" IncludeLiterals="None" />
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem HorizontalAlign="Left" ShowCaption="False" Width="30px">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server" 
                                        SupportsDisabledAttribute="True">
                                        <dx:ASPxButton ID="btnLocCodeLookup" runat="server" 
                                            OnClick="btnLocCodeLookup_Click" Width="25px">
                                            <Image Url="~/Images/Ribbon_Find_16x16.png">
                                            </Image>
                                        </dx:ASPxButton>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>
                    <dx:LayoutItem Caption="Description" HorizontalAlign="Left" Width="250px">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtLocDesc" runat="server" Enabled="False" Width="100%" 
                                    CssClass="uppercase">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:EmptyLayoutItem>
                    </dx:EmptyLayoutItem>
                    <dx:LayoutGroup ColCount="2" ShowCaption="False">
                        <Items>
                            <dx:LayoutItem ShowCaption="False">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server" 
                                        SupportsDisabledAttribute="True">
                                        <dx:ASPxButton ID="btnSaveMainLoc" runat="server" Enabled="False" 
                                            OnClick="btnSaveMainLoc_Click" Text="Save">
                                            <Image Url="~/Images/Ribbon_Save_16x16.png">
                                            </Image>
                                        </dx:ASPxButton>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem ShowCaption="False">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server" 
                                        SupportsDisabledAttribute="True">
                                        <dx:ASPxButton ID="btnDeleteMain" runat="server" Enabled="False" 
                                            OnClick="btnDeleteMain_Click" Text="Delete">
                                            <Image Url="~/Images/Remove_16x16.png">
                                            </Image>
                                        </dx:ASPxButton>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>
                </Items>
            </dx:LayoutGroup>
            <dx:LayoutGroup Caption="Sub Location" ColCount="2">
                <Items>
                    <dx:LayoutGroup ColCount="2" ShowCaption="False" Width="250px">
                        <Items>
                            <dx:LayoutItem Caption="Code" Width="80px">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server" 
                                        SupportsDisabledAttribute="True">
                                        <dx:ASPxTextBox ID="txtSubLocCode" runat="server" AutoPostBack="True" 
                                            Enabled="False" OnTextChanged="txtSubLocCode_TextChanged" Width="75px" 
                                            CssClass="uppercase">
                                            <MaskSettings Mask="&lt;* |a|b|c|d|e|f|g|h|i|j|k|l|m|n|o|p|q|r|s|t|u|v|w|x|y|z|A|B|C|D|E|F|G|H|I|J|K|L|M|N|O|P|Q|R|S|T|U|V|W|X|Y|Z&gt;" />
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem HorizontalAlign="Left" ShowCaption="False" Width="30px">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server" 
                                        SupportsDisabledAttribute="True">
                                        <dx:ASPxButton ID="btnSubLocLookup" runat="server" Enabled="False" 
                                            OnClick="btnSubLocLookup_Click" Width="25px">
                                            <Image Url="~/Images/Ribbon_Find_16x16.png">
                                            </Image>
                                        </dx:ASPxButton>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>
                    <dx:LayoutItem Caption="Description" HorizontalAlign="Left" Width="250px">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtSubCodeDesc" runat="server" Enabled="False" Width="100%" 
                                    CssClass="uppercase">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem ShowCaption="False">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutGroup ColCount="2" ShowCaption="False">
                        <Items>
                            <dx:LayoutItem ShowCaption="False">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server" 
                                        SupportsDisabledAttribute="True">
                                        <dx:ASPxButton ID="btnSaveSubLoc" runat="server" Enabled="False" 
                                            OnClick="btnSaveSubLoc_Click" Text="Save">
                                            <Image Url="~/Images/Ribbon_Save_16x16.png">
                                            </Image>
                                        </dx:ASPxButton>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem ShowCaption="False">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server" 
                                        SupportsDisabledAttribute="True">
                                        <dx:ASPxButton ID="btnDeleteSubLoc" runat="server" Enabled="False" 
                                            OnClick="btnDeleteSubLoc_Click" Text="Delete">
                                            <Image Url="~/Images/Remove_16x16.png">
                                            </Image>
                                        </dx:ASPxButton>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>
                </Items>
            </dx:LayoutGroup>
        </Items>
    </dx:ASPxFormLayout>
    <dx:ASPxFormLayout ID="FormLayoutDetail" runat="server" Width="100%" 
            Visible="False">
            <Items>
                <dx:LayoutGroup Caption="Part Transition Location" ColCount="2">
                    <Items>
                        <dx:LayoutGroup ColCount="3" ShowCaption="False" Width="300px">
                            <Items>
                                <dx:LayoutItem Caption="Code" HorizontalAlign="Left">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer1" runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxTextBox ID="txtTransLocCode" runat="server" Width="75px" 
                                                Enabled="False">
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem HorizontalAlign="Left" ShowCaption="False">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer2" runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxTextBox ID="txtTransSubLocCode" runat="server" Width="75px" 
                                                Enabled="False">
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem HorizontalAlign="Left" ShowCaption="False">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer3" runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxTextBox ID="txtTransCode" runat="server" Width="50px" 
                                                AutoPostBack="True" OnTextChanged="txtTransCode_TextChanged" 
                                                Enabled="False">
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                            </Items>
                        </dx:LayoutGroup>
                        <dx:LayoutItem Caption="Description" Width="300px">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer4" runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtTransLocDesc" runat="server" Width="100%" 
                                        CssClass="uppercase">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
                <dx:LayoutGroup ShowCaption="False">
                    <Items>
                        <dx:LayoutGroup ColCount="3" ShowCaption="False">
                            <Items>
                                <dx:LayoutItem HorizontalAlign="Center" ShowCaption="False">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer5" runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxButton ID="btnClearBottom" runat="server" Text="Save" 
                                                OnClick="btnClearBottom_Click">
                                                <Image Url="~/Images/Ribbon_Save_16x16.png">
                                                </Image>
                                            </dx:ASPxButton>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem HorizontalAlign="Center" ShowCaption="False">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer6" runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxButton ID="btnCreateLocation" runat="server" Text="Create Location" 
                                                Enabled="False" OnClick="btnCreateLocation_Click">
                                                <Image Url="~/Images/Ribbon_New_16x16.png">
                                                </Image>
                                            </dx:ASPxButton>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem HorizontalAlign="Center" ShowCaption="False">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer7" runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxButton ID="btnDeleteLocation" runat="server" Text="Delete Location" 
                                                Enabled="False" OnClick="btnDeleteLocation_Click">
                                                <Image Url="~/Images/Remove_16x16.png">
                                                </Image>
                                            </dx:ASPxButton>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                            </Items>
                        </dx:LayoutGroup>
                    </Items>
                </dx:LayoutGroup>
            </Items>
        </dx:ASPxFormLayout>
        <dx:ASPxButton ID="btnSelectTransCode" runat="server" Text="Select Trans Code" 
            onclick="btnSelectTransCode_Click" Visible="False">
        </dx:ASPxButton>
        <dx:ASPxGridView ID="GridViewTransLocs" runat="server" AutoGenerateColumns="False" 
            DataSourceID="SqlDataSourceTransLocations" KeyFieldName="DealerCode" 
            Width="100%" Visible="False" 
            onfocusedrowchanged="GridViewTransLocs_FocusedRowChanged" 
            ClientInstanceName="GridViewTrans" KeyboardSupport="True">
            <ClientSideEvents RowDblClick="function(s, e) {
	cbp.PerformCallback();
}" />
            <Columns>
                <dx:GridViewDataTextColumn FieldName="DealerCode" ReadOnly="True" 
                    Visible="False" VisibleIndex="1">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="PartsLocCode" Visible="False" 
                    VisibleIndex="2">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="PartsSubLocCode" Visible="False" 
                    VisibleIndex="3">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="PartsTransLocCode" ReadOnly="True" 
                    VisibleIndex="5">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="PartsTransLocDesc" VisibleIndex="6">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="UpdUser" Visible="False" VisibleIndex="7">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="UpdDate" Visible="False" VisibleIndex="8">
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataDateColumn FieldName="UpdTime" Visible="False" VisibleIndex="9">
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="UpdTerm" Visible="False" 
                    VisibleIndex="10">
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" 
                AllowSelectSingleRowOnly="True" />
            <SettingsDataSecurity AllowDelete="False" AllowEdit="False" 
                AllowInsert="False" />
        </dx:ASPxGridView>
        <asp:SqlDataSource ID="SqlDataSourceTransLocations" runat="server" 
            ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
            SelectCommand="SELECT * FROM [PartsTransLocation]"></asp:SqlDataSource>
            </center>
        </div>
            </dx:PanelContent>
</PanelCollection>

    </dx:ASPxPanel>
    

    

    <dx:ASPxPopupControl ID="PopupControlLocCode" runat="server" 
        HeaderText="Location Lookup" Height="300px" Modal="True" 
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
        Width="500px" ShowCloseButton="False">
        <ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
    <dx:ASPxPanel ID="PanelControls" runat="server" Height="30px" Width="100%">
        <PanelCollection>
            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                <dx:ASPxButton ID="btnLocSelect" runat="server" Text="Select" 
                    OnClick="btnLocSelect_Click">
                    <Image Url="~/Images/Apply_16x16.png">
                    </Image>
                </dx:ASPxButton>
                <dx:ASPxButton ID="btnLocCancel" runat="server" Text="Cancel" 
                    OnClick="btnLocCancel_Click">
                    <Image Url="~/Images/Cancel_16x16.png">
                    </Image>
                </dx:ASPxButton>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>
    <dx:ASPxGridView ID="GridViewLocCode" runat="server" 
        AutoGenerateColumns="False" DataSourceID="SqlDataSourceLocCode" 
        KeyFieldName="DealerCode" Width="100%">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="DealerCode" ReadOnly="True" 
                ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PartsLocCode" ReadOnly="True" 
                ShowInCustomizationForm="True" VisibleIndex="1" Width="30px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PartsLocDesc" 
                ShowInCustomizationForm="True" VisibleIndex="2" Width="300px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="UpdUser" ShowInCustomizationForm="True" 
                Visible="False" VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="UpdDate" ShowInCustomizationForm="True" 
                Visible="False" VisibleIndex="4">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn FieldName="UpdTime" ShowInCustomizationForm="True" 
                Visible="False" VisibleIndex="5">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="UpdTerm" ShowInCustomizationForm="True" 
                Visible="False" VisibleIndex="6">
            </dx:GridViewDataTextColumn>
        </Columns>
        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" 
            AllowSelectSingleRowOnly="True" />
        <Settings ShowFilterBar="Visible" ShowFilterRow="True" />
        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" 
            AllowInsert="False" />
    </dx:ASPxGridView>
            <asp:SqlDataSource ID="SqlDataSourceLocCode" runat="server" 
        ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
        SelectCommand="SELECT DealerCode, PartsLocCode, PartsLocDesc, UpdUser, UpdDate, UpdTime, UpdTerm FROM PartsLocation ">
    </asp:SqlDataSource>
            </dx:PopupControlContentControl>
</ContentCollection>
    </dx:ASPxPopupControl>


    <dx:ASPxPopupControl ID="PopupControlSubLocLookup" runat="server" 
        HeaderText="Sub Location Lookup" Height="300px" Modal="True" 
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
        Width="500px" ShowCloseButton="False">
        <ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
    <dx:ASPxPanel ID="ASPxPanel1" runat="server" Height="30px" Width="100%">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <dx:ASPxButton ID="btnSubLocSelect" runat="server" Text="Select" 
                    OnClick="btnSubLocSelect_Click">
                    <Image Url="~/Images/Apply_16x16.png">
                    </Image>
                </dx:ASPxButton>
                <dx:ASPxButton ID="btnSubLocCancel" runat="server" Text="Cancel" 
                    OnClick="btnSubLocCancel_Click">
                    <Image Url="~/Images/Cancel_16x16.png">
                    </Image>
                </dx:ASPxButton>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>
    <dx:ASPxGridView ID="GridViewSubLoc" runat="server" 
        AutoGenerateColumns="False" DataSourceID="SqlDataSourceSubLoc" 
        KeyFieldName="DealerCode" Width="100%">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="DealerCode" ReadOnly="True" 
                ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PartsSubLocCode" ReadOnly="True" 
                ShowInCustomizationForm="True" VisibleIndex="1" Width="30px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PartsLocCode" 
                ShowInCustomizationForm="True" VisibleIndex="2" ReadOnly="True" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PartsSubLocDesc" 
                ShowInCustomizationForm="True" VisibleIndex="3" Width="300px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="UpdUser" ShowInCustomizationForm="True" 
                Visible="False" VisibleIndex="4">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="UpdDate" ShowInCustomizationForm="True" 
                Visible="False" VisibleIndex="5">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn FieldName="UpdTime" ShowInCustomizationForm="True" 
                Visible="False" VisibleIndex="6">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="UpdTerm" ShowInCustomizationForm="True" 
                Visible="False" VisibleIndex="7">
            </dx:GridViewDataTextColumn>
        </Columns>
        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" 
            AllowSelectSingleRowOnly="True" />
        <Settings ShowFilterBar="Visible" ShowFilterRow="True" />
        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" 
            AllowInsert="False" />
    </dx:ASPxGridView>
            <asp:SqlDataSource ID="SqlDataSourceSubLoc" runat="server" 
        ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
        SelectCommand="SELECT * FROM [PartsSubLocation]">
    </asp:SqlDataSource>
            </dx:PopupControlContentControl>
</ContentCollection>
    </dx:ASPxPopupControl>


    <dx:ASPxPopupControl ID="PopupControlMSG" runat="server" Modal="True" 
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
            ShowCloseButton="False" HeaderText="Message" Width="300px">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
    <dx:ASPxLabel ID="lblRedirect" runat="server" Visible="False">
    </dx:ASPxLabel>
    <dx:ASPxLabel ID="lblMSG" runat="server" Font-Bold="True" Wrap="True">
    </dx:ASPxLabel>
    <br />
    <dx:ASPxButton ID="btnPopOk" runat="server" Text="Ok" OnClick="btnPopOk_Click">
    </dx:ASPxButton>
    <dx:ASPxButton ID="btnPopCancel" runat="server" Text="Cancel" Visible="False" 
        OnClick="btnPopCancel_Click">
    </dx:ASPxButton>


                </dx:PopupControlContentControl>
            </ContentCollection>
    </dx:ASPxPopupControl>   

    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="200px" 
        ClientInstanceName="cbp" oncallback="ASPxCallbackPanel1_Callback" 
        Visible="False">
    </dx:ASPxCallbackPanel>

</asp:Content>
