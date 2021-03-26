<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="NewCustomerVehicalInformation.aspx.cs" Inherits="DXBMS.Modules.Service.NewCustomerVehicalInformation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="container">
    <dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="100%">
        <PanelCollection>
            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                 <center>
                    <div class="Heading">
                        &nbsp; VEHICAL ENROLMENT
                    </div>
                     <dx:ASPxFormLayout ID="ASPxFormLayoutTop" runat="server" Width="100%">
                         <Items>
                             <dx:LayoutGroup ColCount="6" ShowCaption="False" Width="100%">
                                 <Items>
                                     <dx:LayoutItem ShowCaption="False" Width="15%">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxButton ID="btnSave" runat="server" Text="Save" Width="100%" 
                                                     OnClick="btnSave_Click" Enabled="False">
                                                     <Image Url="~/Images/Ribbon_Save_16x16.png">
                                                     </Image>
                                                 </dx:ASPxButton>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:LayoutItem ShowCaption="False" Width="15%">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxButton ID="btnDelete" runat="server" Text="Delete" Width="100%" 
                                                     Enabled="False" OnClick="btnDelete_Click">
                                                     <Image Url="~/Images/Delete_16x16.png">
                                                     </Image>
                                                 </dx:ASPxButton>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:LayoutItem ShowCaption="False" Width="15%">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxButton ID="btnClear" runat="server" Text="Clear" Width="100%" 
                                                     OnClick="btnClear_Click">
                                                     <Image Url="~/Images/Clear_16x16.png">
                                                     </Image>
                                                 </dx:ASPxButton>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:EmptyLayoutItem ColSpan="2" Width="35%">
                                     </dx:EmptyLayoutItem>
                                     <dx:LayoutItem ShowCaption="False" Width="20%">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxLabel ID="lblMasg" runat="server" Width="100%">
                                                 </dx:ASPxLabel>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                 </Items>
                             </dx:LayoutGroup>
                             <dx:LayoutGroup ColCount="4" ShowCaption="False" Width="100%">
                                 <Items>
                                     <dx:LayoutItem Width="25%" Caption="Customer">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxGridLookup ID="cmbCustomerCode" runat="server" 
                                                     AutoGenerateColumns="False" 
                                                     DataSourceID="SqlDataSourceCustomer" KeyFieldName="Code" 
                                                     TextFormatString="{0}" Width="100%">
<GridViewProperties>
<SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" 
        AllowSelectByRowClick="True" EnableRowHotTrack="True"></SettingsBehavior>
    <Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFooter="True" />
</GridViewProperties>
                                                     <Columns>
                                                         <dx:GridViewDataTextColumn FieldName="Code" ShowInCustomizationForm="True" 
                                                             VisibleIndex="0">
                                                         </dx:GridViewDataTextColumn>
                                                         <dx:GridViewDataTextColumn FieldName="Description" ReadOnly="True" 
                                                             ShowInCustomizationForm="True" VisibleIndex="1">
                                                         </dx:GridViewDataTextColumn>
                                                         <dx:GridViewDataTextColumn FieldName="Address" ReadOnly="True" 
                                                             ShowInCustomizationForm="True" VisibleIndex="2">
                                                         </dx:GridViewDataTextColumn>
                                                         <dx:GridViewDataTextColumn FieldName="Phone" ReadOnly="True" 
                                                             ShowInCustomizationForm="True" VisibleIndex="3">
                                                         </dx:GridViewDataTextColumn>
                                                     </Columns>
                                                     <ClientSideEvents TextChanged="function(s, e) {
	btnCustomerLookup.DoClick();
}" />
                                                 </dx:ASPxGridLookup>

                                                 
                                                 <asp:SqlDataSource ID="SqlDataSourceCustomer" runat="server" 
                                                     ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" SelectCommand="select cusCode as Code,CusDesc+' '+FatherHusName as [Description], Address1+' '+Address2+' '+Address3 as [Address],isnull(phone1,isnull(phone2,CellNo)) as Phone from Customer
where DealerCode IN ('AAAAA',@DealerCode) Order by CusCode Desc">
                                                     <SelectParameters>
                                                         <asp:SessionParameter DefaultValue="00001" Name="DealerCode" 
                                                             SessionField="DealerCode" />
                                                     </SelectParameters>
                                                 </asp:SqlDataSource>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:LayoutItem ColSpan="2" ShowCaption="False" Width="50%">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxTextBox ID="txtCustomerName" runat="server" Width="100%" 
                                                     ReadOnly="True">
                                                 </dx:ASPxTextBox>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:EmptyLayoutItem Width="25%">
                                     </dx:EmptyLayoutItem>
                                     <dx:LayoutItem Width="25%" Caption="Account of">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxComboBox ID="cmbEndUser" runat="server" AutoPostBack="True" 
                                                     DataSourceID="SqlDataSourceCustomer" 
                                                     OnSelectedIndexChanged="cmbEndUser_SelectedIndexChanged" TextField="Code" 
                                                     TextFormatString="{0}" ValueField="Code" Width="100%">
                                                     <Columns>
                                                         <dx:ListBoxColumn FieldName="Code" />
                                                         <dx:ListBoxColumn FieldName="Description" />
                                                         <dx:ListBoxColumn FieldName="Address" />
                                                         <dx:ListBoxColumn FieldName="Phone" />
                                                     </Columns>
                                                 </dx:ASPxComboBox>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:LayoutItem ColSpan="2" ShowCaption="False" Width="50%">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxTextBox ID="txtEndUserName" runat="server" ReadOnly="True" 
                                                     Width="100%">
                                                 </dx:ASPxTextBox>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:EmptyLayoutItem Width="25%">
                                     </dx:EmptyLayoutItem>
                                     <dx:LayoutItem Width="25%" Caption="Reg. Date">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxDateEdit ID="dpRegDate" runat="server" Width="100%" AutoPostBack="True" 
                                                     Enabled="False" DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" 
                                                     EditFormatString="dd/MM/yyyy">
                                                 </dx:ASPxDateEdit>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:LayoutItem Width="25%" Caption="Brand">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxComboBox ID="cmbBrand" runat="server" Width="100%" 
                                                     DropDownStyle="DropDown" 
                                                     Enabled="False" OnSelectedIndexChanged="cmbBrand_SelectedIndexChanged" 
                                                     TextField="BrandDesc" ValueField="BrandCode" AutoPostBack="True">
                                                 </dx:ASPxComboBox>
                                                 <asp:SqlDataSource ID="SqlDataSourceBrands" runat="server" 
                                                     ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                                                     SelectCommand="select * from Brand where dealercode in ('AAAAA',@DealerCode)">
                                                     <SelectParameters>
                                                         <asp:SessionParameter DefaultValue="00001" Name="DealerCode" 
                                                             SessionField="DealerCode" />
                                                     </SelectParameters>
                                                 </asp:SqlDataSource>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:LayoutItem Width="25%" Caption="Registration No.">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxTextBox ID="txtRegNo" runat="server" Width="100%" AutoPostBack="True" 
                                                     Enabled="False" OnTextChanged="txtRegNo_TextChanged" style="text-transform: uppercase" >
                                                 </dx:ASPxTextBox>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:LayoutItem Width="25%" Caption="Engine No.">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxTextBox ID="txtEngNo" runat="server" AutoPostBack="True" style="text-transform: uppercase" 
                                                     Enabled="False" OnTextChanged="txtEngNo_TextChanged1" Width="100%">
                                                 </dx:ASPxTextBox>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:LayoutItem Width="25%" Caption="Chassis No.">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxTextBox ID="txtChassisNo" runat="server" AutoPostBack="True" style="text-transform: uppercase" 
                                                     Enabled="False" OnTextChanged="txtChassisNo_TextChanged" Width="100%">
                                                 </dx:ASPxTextBox>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:LayoutItem Caption="Product" Width="25%">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxComboBox ID="cmbProdCode" runat="server" Enabled="False" 
                                                     AutoPostBack="True" DataSourceID="SqlDataSourceProduct" 
                                                     OnButtonClick="cmbProdCode_ButtonClick" 
                                                     OnSelectedIndexChanged="cmbProdCode_SelectedIndexChanged" 
                                                     TextField="VehicleCode" TextFormatString="{0}" ValueField="VehicleCode" 
                                                     Width="100%">
                                                     <Columns>
                                                         <dx:ListBoxColumn FieldName="VehicleCode" />
                                                         <dx:ListBoxColumn FieldName="Description" />
                                                         <dx:ListBoxColumn FieldName="Version" />
                                                         <dx:ListBoxColumn FieldName="ColorCode" />
                                                         <dx:ListBoxColumn FieldName="Color" />
                                                     </Columns>
                                                 </dx:ASPxComboBox>
                                                 <asp:SqlDataSource ID="SqlDataSourceProduct" runat="server" 
                                                     ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                                                     SelectCommand="select  ProdCode as VehicleCode,VersionCode as[Version],ProdDesc as [Description],ColorCode,Color from vehicle where DealerCode IN ('AAAAA','00001') and BrandCode='001'">
                                                 </asp:SqlDataSource>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:LayoutItem ShowCaption="False" Width="25%">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxTextBox ID="txtProdDesc" runat="server" Width="100%">
                                                 </dx:ASPxTextBox>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:LayoutItem Caption="Model" Width="25%">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxTextBox ID="txtVersion" runat="server" Enabled="False" Width="100%">
                                                 </dx:ASPxTextBox>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:LayoutItem Caption="Color" Width="25%">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxComboBox ID="cmbColorCode" runat="server" Enabled="False" 
                                                     AutoPostBack="True" DataSourceID="SqlDataSourceColor" 
                                                     OnSelectedIndexChanged="cmbColorCode_SelectedIndexChanged" 
                                                     TextField="ColorCode" TextFormatString="{0}" ValueField="ColorCode" 
                                                     Width="100%">
                                                     <Columns>
                                                         <dx:ListBoxColumn FieldName="ColorCode" />
                                                         <dx:ListBoxColumn FieldName="ColorDesc" />
                                                     </Columns>
                                                 </dx:ASPxComboBox>
                                                 <asp:SqlDataSource ID="SqlDataSourceColor" runat="server" 
                                                     ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                                                     SelectCommand="select * from Color where Active='Y'  and DealerCode in ('AAAAA',@DealerCode)">
                                                     <SelectParameters>
                                                         <asp:SessionParameter DefaultValue="00001" Name="DealerCode" 
                                                             SessionField="DealerCode" />
                                                     </SelectParameters>
                                                 </asp:SqlDataSource>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:LayoutItem ShowCaption="False" Width="25%">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxTextBox ID="txtColorName" runat="server" Width="100%" ReadOnly="True">
                                                 </dx:ASPxTextBox>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:LayoutItem Caption="Warranty Book No." Width="25%">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxTextBox ID="txtInvoiceNo" runat="server" Width="100%" Enabled="False">
                                                 </dx:ASPxTextBox>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:LayoutItem Caption="Invoice Date" Width="25%">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxDateEdit ID="dpInvoiceDate" runat="server" Enabled="False" 
                                                     DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" 
                                                     EditFormatString="dd/MM/yyyy" Width="100%">
                                                 </dx:ASPxDateEdit>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:LayoutItem Caption="Insurance Company" Width="25%">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxComboBox ID="cmbInsCode" runat="server" Enabled="False" 
                                                     AutoPostBack="True" DataSourceID="SqlDataSourceInsurance" 
                                                     OnSelectedIndexChanged="cmbInsCode_SelectedIndexChanged" 
                                                     TextField="InsCompCode" TextFormatString="{0}" ValueField="InsCompCode" 
                                                     Width="100%">
                                                     <Columns>
                                                         <dx:ListBoxColumn FieldName="InsCompCode" />
                                                         <dx:ListBoxColumn Caption="Company Name" FieldName="InsCompDescription" />
                                                     </Columns>
                                                 </dx:ASPxComboBox>
                                                 <asp:SqlDataSource ID="SqlDataSourceInsurance" runat="server" 
                                                     ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                                                     SelectCommand="select * from InsuranceCompanies where DealerCode in ('AAAAA',@DealerCode)">
                                                     <SelectParameters>
                                                         <asp:SessionParameter DefaultValue="00001" Name="DealerCode" 
                                                             SessionField="DealerCode" />
                                                     </SelectParameters>
                                                 </asp:SqlDataSource>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:LayoutItem ShowCaption="False" Width="25%">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxTextBox ID="txtInsuranceName" runat="server" Width="100%" 
                                                     ReadOnly="True">
                                                 </dx:ASPxTextBox>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:LayoutItem Caption="Policy No." Width="25%">
                                         <LayoutItemNestedControlCollection>
                                             <dx:LayoutItemNestedControlContainer runat="server" 
                                                 SupportsDisabledAttribute="True">
                                                 <dx:ASPxTextBox ID="txtPolicyNo" runat="server" Width="100%">
                                                 </dx:ASPxTextBox>
                                             </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                     </dx:LayoutItem>
                                     <dx:EmptyLayoutItem Width="25%">
                                     </dx:EmptyLayoutItem>
                                 </Items>
                             </dx:LayoutGroup>
                         </Items>
                     </dx:ASPxFormLayout>
                     <dx:ASPxGridView ID="gvCVInfo" runat="server" Width="100%" 
                         AutoGenerateColumns="False" Enabled="False" KeyFieldName="CusCode">
                         <ClientSideEvents RowDblClick="function(s, e) {
	btnDCGV.DoClick();
}" />
                         <Columns>
                             <dx:GridViewDataTextColumn Caption="Customer Code" FieldName="CusCode" 
                                 ShowInCustomizationForm="True" VisibleIndex="0">
                             </dx:GridViewDataTextColumn>
                             <dx:GridViewDataTextColumn Caption="Date" FieldName="tdDate" 
                                 ShowInCustomizationForm="True" VisibleIndex="1">
                             </dx:GridViewDataTextColumn>
                             <dx:GridViewDataTextColumn Caption="Reg No." FieldName="RegNo" 
                                 ShowInCustomizationForm="True" VisibleIndex="2">
                             </dx:GridViewDataTextColumn>
                             <dx:GridViewDataTextColumn Caption="Chassis No." FieldName="ChassisNo" 
                                 ShowInCustomizationForm="True" VisibleIndex="3">
                             </dx:GridViewDataTextColumn>
                             <dx:GridViewDataTextColumn Caption="Engine No." FieldName="EngineNo" 
                                 ShowInCustomizationForm="True" VisibleIndex="4">
                             </dx:GridViewDataTextColumn>
                             <dx:GridViewDataTextColumn Caption="Brand" FieldName="BrandCode" 
                                 ShowInCustomizationForm="True" VisibleIndex="5">
                             </dx:GridViewDataTextColumn>
                             <dx:GridViewDataTextColumn Caption="Product Code" FieldName="ProdCode" 
                                 ShowInCustomizationForm="True" VisibleIndex="6">
                             </dx:GridViewDataTextColumn>
                             <dx:GridViewDataTextColumn Caption="Model Code" FieldName="VersionCode" 
                                 ShowInCustomizationForm="True" VisibleIndex="7">
                             </dx:GridViewDataTextColumn>
                             <dx:GridViewDataTextColumn Caption="Color" FieldName="ColorCode" 
                                 ShowInCustomizationForm="True" VisibleIndex="8">
                             </dx:GridViewDataTextColumn>
                             <dx:GridViewDataTextColumn Caption="Warranty Book No" FieldName="InvoiceNo" 
                                 ShowInCustomizationForm="True" VisibleIndex="9">
                             </dx:GridViewDataTextColumn>
                             <dx:GridViewDataTextColumn Caption="Invoice Date" FieldName="InvoiceDate" 
                                 ShowInCustomizationForm="True" VisibleIndex="10">
                             </dx:GridViewDataTextColumn>
                         </Columns>
                         <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" 
                             AllowSelectSingleRowOnly="True" AllowSort="False" EnableRowHotTrack="True" />
                         <SettingsDataSecurity AllowDelete="False" AllowEdit="False" 
                             AllowInsert="False" />
                     </dx:ASPxGridView>

                </center>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>
    <dx:ASPxButton ID="btnDCGV" runat="server" Text="Double Click On Grid" 
        ClientInstanceName="btnDCGV" ClientVisible="False" onclick="btnDCGV_Click">
    </dx:ASPxButton>
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
    <dx:ASPxButton ID="btnCustomerLookup" runat="server" Text="Customer Lookup" 
        ClientInstanceName="btnCustomerLookup" ClientVisible="False" 
        onclick="btnCustomerLookup_Click">
    </dx:ASPxButton>
    <asp:HiddenField ID="hfLookup" runat="server" />
                </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
