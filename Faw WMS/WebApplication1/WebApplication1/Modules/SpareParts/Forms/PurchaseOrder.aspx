<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="PurchaseOrder.aspx.cs" Inherits="DXBMS.Modules.Transactions.PurchaseOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">

    <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="container">
    <dx:ASPxPanel ID="ASPxPanelWrapper" runat="server" Height="100%" Width="100%"
     FixedPosition="WindowTop" ClientInstanceName="topPanel" Collapsible="true" >
        <PanelCollection>
            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                <center>
                <div class="Heading">
            PURCHASE ORDER
        </div>
                <dx:ASPxFormLayout ID="FormLayoutPOInfo" runat="server" Width="100%" >
                    <Items>
                        <dx:LayoutGroup ColCount="5" HorizontalAlign="Center" ShowCaption="False">
                            <Items>
                                <dx:LayoutItem ShowCaption="False">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxButton ID="btnSave" runat="server" Text="Save" 
                                                Width="100%" OnClick="btnSave_Click">
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
                                            <dx:ASPxButton ID="btnDelete" runat="server" Text="Delete" 
                                                Width="100%" Enabled="False" OnClick="btnDelete_Click" 
                                                ClientInstanceName="btnDelete" AutoPostBack="False">
                                                <ClientSideEvents Click="function(s, e) {
	var yn = confirm(&quot;Are you sure to delete this PO?&quot;);
    if(yn==true)
	{
		//alert(&quot;you press ok&quot;);
		//this.DeletePO();
		e.processOnServer = true;
	}
	else
	{
		 e.processOnServer = false;
	}
}" />
                                                <Image Url="~/Images/Remove_16x16.png">
                                                </Image>
                                            </dx:ASPxButton>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem ShowCaption="False">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxButton ID="btnClear" runat="server" Text="Clear" 
                                                Width="100%" OnClick="btnClear_Click">
                                                <Image Url="~/Images/Clear_16x16.png">
                                                </Image>
                                            </dx:ASPxButton>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem ShowCaption="False">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxButton ID="btnPrint" runat="server" Text="Print" 
                                                Width="100%" Enabled="False" OnClick="btnPrint_Click">
                                                <Image Url="~/Images/Print_16x16.png">
                                                </Image>
                                            </dx:ASPxButton>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem ShowCaption="False">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxButton ID="btnPost" runat="server" Text="Post" 
                                                Width="100%" Enabled="False" OnClick="btnPost_Click">
                                                <ClientSideEvents Click="function(s, e) {
	var yn = confirm(&quot;Are you sure to post this PO?&quot;);
    if(yn==true)
	{
		//alert(&quot;you press ok&quot;);
		//this.DeletePO();
		e.processOnServer = true;
	}
	else
	{
		 e.processOnServer = false;
	}
}" />
                                                <Image Url="~/Images/Wizard_16x16.png">
                                                </Image>
                                            </dx:ASPxButton>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                            </Items>
                        </dx:LayoutGroup>
                        <dx:LayoutGroup ColCount="2" ShowCaption="False">
                            <Items>
                                <dx:LayoutItem Caption="P/O No.">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxGridLookup ID="ddPONumber" runat="server" AutoGenerateColumns="False" 
                                                ClientInstanceName="ddPONumber" DataSourceID="SqlDataSourcePOLookup" 
                                                IncrementalFilteringMode="Contains" KeyFieldName="POno" 
                                                OnTextChanged="ddPONumber_TextChanged" TextFormatString="{0}">
                                                <GridViewProperties>
                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" 
                                                        AllowSelectSingleRowOnly="True" EnableRowHotTrack="True" />
                                                    <Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowStatusBar="Visible" />
                                                </GridViewProperties>
                                                <Columns>
                                                    <dx:GridViewDataTextColumn FieldName="POno" ReadOnly="True" 
                                                        ShowInCustomizationForm="True" VisibleIndex="0" SortIndex="0" 
                                                        SortOrder="Descending">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="DealerCode" ReadOnly="True" 
                                                        ShowInCustomizationForm="True" VisibleIndex="1">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataDateColumn FieldName="PODate" ShowInCustomizationForm="True" 
                                                        VisibleIndex="2">
                                                        <PropertiesDateEdit DisplayFormatInEditMode="True" 
                                                            DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" 
                                                            EditFormatString="dd/MM/yyyy">
                                                        </PropertiesDateEdit>
                                                    </dx:GridViewDataDateColumn>
                                                    <dx:GridViewDataTextColumn FieldName="VendorCode" 
                                                        ShowInCustomizationForm="True" VisibleIndex="3">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="VendorDesc" 
                                                        ShowInCustomizationForm="True" VisibleIndex="4">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="TotalAmt" ShowInCustomizationForm="True" 
                                                        VisibleIndex="5">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="NetTotal" ShowInCustomizationForm="True" 
                                                        VisibleIndex="6">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="POSource" ShowInCustomizationForm="True" 
                                                        VisibleIndex="7">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="TransferStatus" 
                                                        ShowInCustomizationForm="True" VisibleIndex="8">
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>
                                                <ClientSideEvents ValueChanged="function(s, e) {
	//alert(&quot;test&quot;);
	//cbp.PerformCallback();
	//e.processOnServer = true;
	btnCheck.DoClick();
}" />
                                            </dx:ASPxGridLookup>
                                            <asp:SqlDataSource ID="SqlDataSourcePOLookup" runat="server" 
                                                ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>"                                               
                                                
                                                SelectCommand="SELECT POMaster.POno, POMaster.DealerCode, POMaster.PODate, POMaster.VendorCode, Vendor.VendorDesc, POMaster.TotalAmt, POMaster.NetTotal, POMaster.POSource, POMaster.TransferStatus FROM POMaster INNER JOIN Vendor ON POMaster.VendorCode = Vendor.VendorCode and POMaster.DealerCode = Vendor.DealerCode where POMaster.DealerCode = @DealerCode ORDER BY POMaster.POno DESC">
                                                <selectparameters>
                                                <asp:sessionparameter Name="DealerCode" SessionField="DealerCode" Type="String" />
                                                </selectparameters>

                                            </asp:SqlDataSource>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="P/O Date">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxDateEdit ID="dpPODate" runat="server" EditFormat="Custom" 
                                                EditFormatString="dd-MM-yyyy"  ViewStateMode="Enabled" DisplayFormatString="dd-MM-yyyy" >
                                            </dx:ASPxDateEdit>
                                           
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Vendor *">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxComboBox ID="ddVendorName" runat="server" 
                                                DataSourceID="SqlDataSourceVendors" TextField="VendorDesc" 
                                                ValueField="VendorCode" AutoPostBack="True" 
                                                OnSelectedIndexChanged="ddVendorName_SelectedIndexChanged">
                                            </dx:ASPxComboBox>
                                            <asp:SqlDataSource ID="SqlDataSourceVendors" runat="server" 
                                                ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                                                SelectCommand="SELECT [VendorCode], [VendorDesc] FROM [Vendor]">
                                            </asp:SqlDataSource>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Delivery Type *">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxComboBox ID="ddDeliveryType" runat="server" 
                                                DataSourceID="SqlDataSourceDeliveryType" TextField="NName" 
                                                ValueField="NName" AutoPostBack="True" 
                                                OnSelectedIndexChanged="ddDeliveryType_SelectedIndexChanged">
                                            </dx:ASPxComboBox>
                                            <asp:SqlDataSource ID="SqlDataSourceDeliveryType" runat="server" 
                                                ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                                                SelectCommand="SELECT Sno, NName, SStatus FROM PartsDeliveryTypes WHERE (SStatus = 1)">
                                            </asp:SqlDataSource>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Payment Terms">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxTextBox ID="txtPaymentTerms" runat="server" Width="170px" MaxLength="2">
                                                <MaskSettings Mask="99" />
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Parts Source *">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxComboBox ID="ddPartsSource" runat="server" 
                                                OnSelectedIndexChanged="ddPartsSource_SelectedIndexChanged" 
                                                AutoPostBack="True">
                                            </dx:ASPxComboBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Remarks" ColSpan="2">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxTextBox ID="txtRemarks" runat="server" Width="100%">
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                            </Items>
                        </dx:LayoutGroup>
                        <dx:LayoutGroup Caption="Item Details" ColCount="4">
                            <Items>
                                <dx:LayoutItem ShowCaption="False" Width="150px">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxButton ID="btnAddItem" runat="server" Text="Add Item" 
                                                Width="100%" OnClick="btnAddItem_Click">
                                                <ClientSideEvents Click="function(s, e) {
	popupItem.Show();
e.processOnServer = true;
}" />
                                                <Image Url="~/Images/Add_16x16.png">
                                                </Image>
                                            </dx:ASPxButton>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem ShowCaption="False" Width="150px">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxButton ID="btnEditItem" runat="server" Text="Edit Item" 
                                                Width="100%" OnClick="btnEditItem_Click">
                                                <Image Url="~/Images/Edit_16x16.png">
                                                </Image>
                                            </dx:ASPxButton>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem ShowCaption="False" Width="150px">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxButton ID="btnDeleteItem" runat="server" Text="Delete Item" 
                                                Width="100%" OnClick="btnDeleteItem_Click">
                                                <Image Url="~/Images/Delete_16x16.png">
                                                </Image>
                                            </dx:ASPxButton>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem HorizontalAlign="Right" ShowCaption="False">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxLabel ID="lblStatus" runat="server" Text="-">
                                            </dx:ASPxLabel>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                            </Items>
                        </dx:LayoutGroup>
                    </Items>
                    <Border BorderStyle="Solid" BorderWidth="1px" />
                </dx:ASPxFormLayout>
                    <dx:ASPxGridView ID="GridViewItems" runat="server" Width="100%" 
                        AutoGenerateColumns="False" DataSourceID="SqlDataSourcePODetail" 
                        KeyFieldName="PartItemNo" 
                        OnPageIndexChanged="GridViewItems_PageIndexChanged" >
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="ItemCode" ShowInCustomizationForm="True" 
                                Visible="False" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="PartItemNo" 
                                ShowInCustomizationForm="True" VisibleIndex="2" Width="100px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="PartItemName" 
                                ShowInCustomizationForm="True" VisibleIndex="3" Width="200px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Unit" ShowInCustomizationForm="True" 
                                VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Qty" FieldName="Quantity" ShowInCustomizationForm="True" 
                                VisibleIndex="5">
                                <PropertiesTextEdit>
                                    <MaskSettings Mask="&lt;0..999999&gt;.&lt;00..99&gt;" />
                                </PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            
                            <dx:GridViewDataTextColumn FieldName="Rate" ShowInCustomizationForm="True" 
                                VisibleIndex="6">
                                <PropertiesTextEdit>
                                    <MaskSettings Mask="&lt;0..999999&gt;.&lt;00..99&gt;" />
                                </PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Liter" ShowInCustomizationForm="True" 
                                VisibleIndex="6">
                                <PropertiesTextEdit>
                                    <MaskSettings Mask="&lt;0..999999&gt;.&lt;00..99&gt;" />
                                </PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Disc %" FieldName="DiscountPerc" ShowInCustomizationForm="True" 
                                VisibleIndex="6" HeaderStyle-Wrap="True">
                                <PropertiesTextEdit>
                                    <MaskSettings Mask="&lt;0..999999&gt;.&lt;00..99&gt;" />
                                </PropertiesTextEdit>
                                <HeaderStyle Wrap="True" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Discount" FieldName="Discount" ShowInCustomizationForm="True" 
                                VisibleIndex="6" HeaderStyle-Wrap="True">
                                <PropertiesTextEdit>
                                    <MaskSettings Mask="&lt;0..999999&gt;.&lt;00..99&gt;" />
                                </PropertiesTextEdit>
                                <HeaderStyle Wrap="True" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Amount" ShowInCustomizationForm="True" 
                                VisibleIndex="7" Name="&lt;0..999999g&gt;.&lt;00..99&gt;">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="G.S.T %" FieldName="GST" 
                                ShowInCustomizationForm="True" VisibleIndex="8" 
                                Name="&lt;0..99&gt;.&lt;00..99&gt;">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="G.S.T Amt" FieldName="GSTAmount" 
                                ShowInCustomizationForm="True" VisibleIndex="9" HeaderStyle-Wrap="True">
                                <PropertiesTextEdit>
                                    <MaskSettings Mask="&lt;0..999999&gt;.&lt;00..99&gt;" />
                                </PropertiesTextEdit>
                                <HeaderStyle Wrap="True" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Fur.Tax%" FieldName="PFGST" 
                                ShowInCustomizationForm="True" VisibleIndex="8" 
                                Name="&lt;0..99&gt;.&lt;00..99&gt;">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Fur.Tax Amt" FieldName="PFGSTAmt" 
                                ShowInCustomizationForm="True" VisibleIndex="9" HeaderStyle-Wrap="True">
                                <PropertiesTextEdit>
                                    <MaskSettings Mask="&lt;0..999999&gt;.&lt;00..99&gt;" />
                                </PropertiesTextEdit>
                                <HeaderStyle Wrap="True" />
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn Caption="Ex.Tax %" FieldName="ExTaxPerc" 
                                ShowInCustomizationForm="True" VisibleIndex="8" HeaderStyle-Wrap="True" 
                                Name="&lt;0..99&gt;.&lt;00..99&gt;">
                                <HeaderStyle Wrap="True" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Ex.Tax Amt" FieldName="ExTaxAmt" 
                                ShowInCustomizationForm="True" VisibleIndex="9" HeaderStyle-Wrap="True">
                                <PropertiesTextEdit>
                                    <MaskSettings Mask="&lt;0..999999&gt;.&lt;00..99&gt;" />
                                </PropertiesTextEdit>
                                <HeaderStyle Wrap="True" />
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn FieldName="WHTAge" ShowInCustomizationForm="True" 
                                Visible="False" VisibleIndex="10">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="WHTAmount" ShowInCustomizationForm="True" 
                                Visible="False" VisibleIndex="11">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Net Amt" FieldName="NetAmount" ShowInCustomizationForm="True" 
                                VisibleIndex="12">
                                <PropertiesTextEdit>
                                    <MaskSettings Mask="&lt;0..999999g&gt;.&lt;00..99&gt;" />
                                </PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" 
                            AllowSelectSingleRowOnly="True" />
                        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" 
                            AllowInsert="False" />
                            
                    </dx:ASPxGridView>
                    <asp:SqlDataSource ID="SqlDataSourcePODetail" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                        DeleteCommand="DELETE FROM [POTempDetail] WHERE [DealerCode] = @DealerCode AND [POTempNo] = @POTempNo AND [ItemCode] = @ItemCode" 
                        
                        InsertCommand="INSERT INTO [POTempDetail] ([DealerCode], [POTempNo], [ItemCode], [POQty], [Rate], [Amount], [GSTage], [GSTAmount], [NetAmount]) VALUES (@DealerCode, @POTempNo, @ItemCode, @POQty, @Rate, @Amount, @GSTage, @GSTAmount, @NetAmount)" SelectCommand="select     
II.ItemCode,  
II.PartItemNo,    
II.ItemDesc as PartItemName,    
UU.UnitDesc as Unit,    
POD.POQty As Quantity,    
POD.Rate,    
POD.Amount,    
POD.GSTage as GST,    
POD.GSTAmount as GSTAmount,    
POD.SEDage as WHTAge,    
POD.SEDAmount as WHTAmount,    
POD.NetAmount    
from POMaster as POM    
inner join POdetail as POD on POM.PONO = POD.PONO   and POM.dealerCode = POD.dealerCode   
inner join item as II on II.ItemCode = POD.ItemCode 
inner join unit as UU on UU.UnitCode = II.unitCode 

where II.PartItemNo=''" 
                        
                        UpdateCommand="UPDATE [POTempDetail] SET [POQty] = @POQty, [Rate] = @Rate, [Amount] = @Amount, [GSTage] = @GSTage, [GSTAmount] = @GSTAmount, [NetAmount] = @NetAmount WHERE [DealerCode] = @DealerCode AND [POTempNo] = @POTempNo AND [ItemCode] = @ItemCode">
                        <DeleteParameters>
                            <asp:Parameter Name="DealerCode" Type="String" />
                            <asp:Parameter Name="POTempNo" Type="String" />
                            <asp:Parameter Name="ItemCode" Type="String" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="DealerCode" Type="String" />
                            <asp:Parameter Name="POTempNo" Type="String" />
                            <asp:Parameter Name="ItemCode" Type="String" />
                            <asp:Parameter Name="POQty" Type="Double" />
                            <asp:Parameter Name="Rate" Type="Double" />
                            <asp:Parameter Name="Amount" Type="Double" />
                            <asp:Parameter Name="GSTage" Type="Double" />
                            <asp:Parameter Name="GSTAmount" Type="Double" />
                            <asp:Parameter Name="NetAmount" Type="Double" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="POQty" Type="Double" />
                            <asp:Parameter Name="Rate" Type="Double" />
                            <asp:Parameter Name="Amount" Type="Double" />
                            <asp:Parameter Name="GSTage" Type="Double" />
                            <asp:Parameter Name="GSTAmount" Type="Double" />
                            <asp:Parameter Name="NetAmount" Type="Double" />
                            <asp:Parameter Name="DealerCode" Type="String" />
                            <asp:Parameter Name="POTempNo" Type="String" />
                            <asp:Parameter Name="ItemCode" Type="String" />
                        </UpdateParameters>
                    </asp:SqlDataSource>

                    <dx:ASPxFormLayout ID="ASPxFormLayoutSummries" runat="server" Width="100%">
                        <Items>
                            <dx:LayoutGroup ColCount="4" ShowCaption="False" Width="50%">
                                <Items>
                                    <dx:LayoutItem Caption="Total Quantity" Width="25%">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxTextBox ID="txtTotalQTY" runat="server" Width="100%" Enabled="False" 
                                                    Text="0">
                                                    <MaskSettings Mask="&lt;0..999999&gt;.&lt;00..99&gt;" />
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Total Litres" Width="25%">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxTextBox ID="txtTotalLtrs" runat="server" Width="100%" Enabled="False" 
                                                    Text="0">
                                                    <MaskSettings Mask="&lt;0..999999&gt;.&lt;00..99&gt;" />
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="G.S.T. Amount" Width="25%">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxTextBox ID="txtTotalGST" runat="server" Width="100%" Enabled="False" 
                                                    Text="0">
                                                    <MaskSettings Mask="&lt;0..999999&gt;.&lt;00..99&gt;" />
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Further Tax Amount" Width="25%">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer3" runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxTextBox ID="txtTotalFurtherTaxAmt" runat="server" Width="100%" Enabled="False" 
                                                    Text="0">
                                                    <MaskSettings Mask="&lt;0..999999&gt;.&lt;00..99&gt;" />
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Total Extra Tax" Width="25%">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer6" runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxTextBox ID="txtTotExTax" runat="server" Width="100%" Enabled="False" 
                                                    Text="0">
                                                    <MaskSettings Mask="&lt;0..999999&gt;.&lt;00..99&gt;" />
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Gross Amount" Width="25%">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxTextBox ID="txtGrossAmount" runat="server" Width="100%" Enabled="False" 
                                                    Text="0">
                                                    
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Other Charges" Width="25%">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxTextBox ID="txtOtherCharges" runat="server" Width="100%" 
                                                    AutoPostBack="True" OnTextChanged="txtOtherCharges_TextChanged" Text="0">
                                                    <MaskSettings Mask="&lt;0..999999&gt;.&lt;00..99&gt;" />
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:EmptyLayoutItem Width="25%">
                                    </dx:EmptyLayoutItem>
                                    
                                    
                                    <dx:LayoutItem Caption="Net Amount" Width="25%">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxTextBox ID="txtNetAmount" runat="server" Width="100%" Enabled="False" 
                                                    Text="0">
                                                    
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                </Items>
                            </dx:LayoutGroup>
                        </Items>
                    </dx:ASPxFormLayout>
                </center>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>
    <dx:ASPxLoadingPanel ID="LoadingPanel1" runat="server" 
    ClientInstanceName="lp">
    </dx:ASPxLoadingPanel>
    <dx:ASPxButton ID="btnCheck" runat="server" Text="Check" 
        ClientInstanceName="btnCheck" onclick="btnCheck_Click" 
        ClientVisible="False"  >
    </dx:ASPxButton>
    <dx:ASPxButton ID="btnItemSelect" runat="server" Text="Select Item" 
        ClientInstanceName="btnItemSelect" onclick="btnItemSelect_Click" 
        ClientVisible="False">
    </dx:ASPxButton>
    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="200px" 
        ClientInstanceName="cbp" oncallback="ASPxCallbackPanel1_Callback">
        <ClientSideEvents BeginCallback="function(s, e) {
	//lp.Show();
}" EndCallback="function(s, e) {
	//lp.Hide();
}" />
        <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True"></dx:PanelContent>
</PanelCollection>
    </dx:ASPxCallbackPanel>
    <dx:ASPxPopupControl ID="PopupControlItemInfo" runat="server" 
        HeaderText="Item Information" Width="1000px" Modal="True" 
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
        CloseAction="None" ClientInstanceName="popupItem">
        <ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
    <dx:ASPxFormLayout ID="ASPxFormLayoutItemInfo" runat="server">
        <Items>
            <dx:LayoutGroup ColCount="4" ShowCaption="False">
                <Items>
                    <dx:LayoutItem Caption="Part Item">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer1" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxGridLookup ID="ddPartItem" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourcePOItems" 
                                    KeyFieldName="PartItemNo" OnTextChanged="ddPartItem_TextChanged" 
                                    TextFormatString="{3}" ViewStateMode="Enabled">
                                    <GridViewProperties>
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" 
                                            AllowSelectByRowClick="True" EnableRowHotTrack="True" />
                                        <Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
                                            ShowFilterRowMenuLikeItem="True" />
                                    </GridViewProperties>
                                    <Columns>
                                        <dx:GridViewDataTextColumn FieldName="ItemCode" ShowInCustomizationForm="True" 
                                            VisibleIndex="0">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="ItemDesc" ShowInCustomizationForm="True" 
                                            VisibleIndex="1">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="UnitDesc" ShowInCustomizationForm="True" 
                                            VisibleIndex="2">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="PartItemNo" 
                                            ShowInCustomizationForm="True" VisibleIndex="3">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="BalQty" ReadOnly="True" 
                                            ShowInCustomizationForm="True" VisibleIndex="4">
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                    <ClientSideEvents ValueChanged="function(s, e) {
	btnItemSelect.DoClick();
}" />
                                </dx:ASPxGridLookup>
                                <asp:SqlDataSource ID="SqlDataSourcePOItems" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                                    SelectCommand="SELECT A.ItemCode, A.ItemDesc, U.UnitDesc, A.PartItemNo, ISNULL((SUM(B.RecQty) + SUM(B.PurRetQty) + SUM(B.RetQty)) - (SUM(B.IssQty) + SUM(B.ChargeOutQty)), 0) AS BalQty 
FROM Item AS A 
LEFT OUTER JOIN ItemStock AS B ON A.ItemCode = B.ItemCode
LEFT OUTER JOIN Unit AS U on A.UnitCode = U.UnitCode 
where a.DealerCode in ('AAAAA',@DealerCode) And A.Source=@PartSource
GROUP BY A.PartItemNo, A.ItemDesc, u.UnitDesc, A.ItemCode">
                                    <SelectParameters>
                                        <asp:SessionParameter DefaultValue="00001" Name="DealerCode" 
                                            SessionField="DealerCode" />
                                             <asp:SessionParameter  Name="PartSource" 
                                            SessionField="PartSource" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem ColSpan="3" ShowCaption="False">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer2" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtItemDesc" runat="server" Width="100%" Enabled="False">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Packing QTY">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer4" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtPackQty" runat="server" Width="170px" Enabled="False">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Unit">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer5" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtUnit" runat="server" Width="170px" Enabled="False">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Item Code">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer7" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtItemCode" runat="server" Width="170px" Enabled="False">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:EmptyLayoutItem>
                    </dx:EmptyLayoutItem>
                    <dx:LayoutItem Caption="Quantity">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer8" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtQty" runat="server" Width="170px" AutoPostBack="True" 
                                    ClientInstanceName="txtQty" OnTextChanged="txtQty_TextChanged">
                                    <MaskSettings IncludeLiterals="DecimalSymbol" 
                                        Mask="&lt;0..999999g&gt;.&lt;00..99&gt;" />
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    
                    <dx:LayoutItem Caption="Rate">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer9" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtRate" runat="server" Width="170px" AutoPostBack="True" 
                                    ClientInstanceName="txtRate" OnTextChanged="txtRate_TextChanged">
                                    <MaskSettings IncludeLiterals="DecimalSymbol" 
                                        Mask="&lt;0..999999&gt;.&lt;00..99&gt;" />
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Discount %">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer10" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxComboBox ID="ddlDiscount" runat="server" AutoPostBack="True" 
                                    OnSelectedIndexChanged="ddlDiscount_SelectedIndexChanged" SelectedIndex="0">
                               
                                            </dx:ASPxComboBox>

                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:EmptyLayoutItem>
                    </dx:EmptyLayoutItem>
                    <dx:LayoutItem Caption="Discount Amount">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer11" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtDiscount" runat="server" Width="170px" AutoPostBack="True" Enabled="false"
                                    ClientInstanceName="txtRate" OnTextChanged="txtRate_TextChanged">
                                    <MaskSettings IncludeLiterals="DecimalSymbol" 
                                        Mask="&lt;0..999999&gt;.&lt;00..99&gt;" />
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    
                    <dx:LayoutItem Caption="Amount">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer12" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtAmount" runat="server" Width="170px" 
                                    ClientInstanceName="txtAmount" Enabled="False">
                                    <MaskSettings IncludeLiterals="DecimalSymbol" 
                                        Mask="&lt;0..999999g&gt;.&lt;00..99&gt;" />
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    
                    <dx:LayoutItem Caption="G.S.T. %age">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer13" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtGSTPercent" runat="server" Width="170px" 
                                    AutoPostBack="True" OnTextChanged="txtGSTPercent_TextChanged">
                                    <MaskSettings IncludeLiterals="DecimalSymbol" 
                                        Mask="&lt;0..99&gt;.&lt;00..99&gt;" />
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:EmptyLayoutItem>
                    </dx:EmptyLayoutItem>
                    <dx:LayoutItem Caption="G.S.T. Amount">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer14" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtGSTAmount" runat="server" Width="170px" Enabled="False">
                                    <MaskSettings IncludeLiterals="DecimalSymbol" 
                                        Mask="&lt;0..999999g&gt;.&lt;00..99&gt;" />
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    
                    <dx:LayoutItem Caption="Further Tax %age">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer15" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtFurtherTaxPerc" runat="server" Width="170px" 
                                    AutoPostBack="True" OnTextChanged="txtFurtherTaxPerc_TextChanged" >
                                    <MaskSettings IncludeLiterals="DecimalSymbol" 
                                        Mask="&lt;0..99&gt;.&lt;00..99&gt;" />
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>

                    
                    
                    <dx:LayoutItem Caption="Further Tax Amount">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer16" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtFurtherTaxAmt" runat="server" Width="170px" 
                                    AutoPostBack="True" >
                                    <MaskSettings IncludeLiterals="DecimalSymbol" 
                                        Mask="&lt;0..999999g&gt;.&lt;00..99&gt;"  />
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:EmptyLayoutItem>
                    </dx:EmptyLayoutItem>
                    <dx:LayoutItem Caption="Extra Tax %age">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer17" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtExTaxPerc" runat="server" Width="170px" 
                                    AutoPostBack="True" OnTextChanged="txtExTaxPerc_TextChanged" >
                                    <MaskSettings IncludeLiterals="DecimalSymbol" 
                                        Mask="&lt;0..99&gt;.&lt;00..99&gt;" />
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    
                    <dx:LayoutItem Caption="Extra Tax Amount">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer18" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtExTaxAmt" runat="server" Width="170px" 
                                    AutoPostBack="True" >
                                    <MaskSettings IncludeLiterals="DecimalSymbol" 
                                        Mask="&lt;0..999999g&gt;.&lt;00..99&gt;"  />
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Liter">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer19" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtLiter" runat="server" Width="170px" Enabled="False">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:EmptyLayoutItem>
                    </dx:EmptyLayoutItem>
                    <dx:LayoutItem Caption="Net Amount">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer20" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtTotalAmount" runat="server" Width="170px" 
                                    Enabled="False" >
                                    <MaskSettings Mask="&lt;0..999999g&gt;.&lt;00..99&gt;" />
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:EmptyLayoutItem>
                    </dx:EmptyLayoutItem>
                    <dx:EmptyLayoutItem>
                    </dx:EmptyLayoutItem>
                    <dx:EmptyLayoutItem>
                    </dx:EmptyLayoutItem>
                    <dx:LayoutItem ShowCaption="False" HorizontalAlign="Right" Width="150px">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer21" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="txtSaveItem" runat="server" Text="Save" Width="100%" 
                                    OnClick="txtSaveItem_Click">
                                    <ClientSideEvents Click="function(s, e) {
	popupItem.Hide();
}" />
                                    <Image Url="~/Images/Ribbon_SaveAs_16x16.png">
                                    </Image>
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem ShowCaption="False" HorizontalAlign="Right" Width="150px">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer22" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btnCancelItem" runat="server" Text="Cancel" Width="100%" 
                                    OnClick="btnCancelItem_Click">
                                    <Image Url="~/Images/Cancel_16x16.png">
                                    </Image>
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
        </Items>
    </dx:ASPxFormLayout>
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
        </div> 
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
