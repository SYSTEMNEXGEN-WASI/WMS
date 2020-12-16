<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="CustomerMaster.aspx.cs" Inherits="DXBMS.Modules.Masters.CustomerMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">

    <script src="../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../js/chosen.jquery.min.js" type="text/javascript"></script>

    <asp:UpdatePanel ID="UpdPanel1" runat="server">
        <ContentTemplate>
    <center>
        <div class="Heading">
            CUSTOMER PROFILE
        </div>
        <dx:ASPxFormLayout ID="FormLayoutCustomerMaster" runat="server" Width="100%" 
            ClientInstanceName="CustomerInfo">
            <Items>
                <dx:LayoutGroup ColCount="5" ShowCaption="False" Width="100%">
                    <Items>
                        <dx:EmptyLayoutItem Width="20%">
                        </dx:EmptyLayoutItem>
                        <dx:LayoutItem ShowCaption="False" Width="20%">
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
                        <dx:LayoutItem ShowCaption="False" Width="20%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxButton ID="btnDelete" runat="server" Enabled="False" Text="Delete" 
                                        Width="100%" OnClick="btnDelete_Click">
                                        <ClientSideEvents Click="function(s, e) {
	var yn = confirm(&quot;Are you sure to delete this customer?&quot;);
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
                                        <Image Url="~/Images/Delete_16x16.png">
                                        </Image>
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem ShowCaption="False" Width="20%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxButton ID="btnClear" runat="server" OnClick="btnClear_Click" 
                                        Text="Clear" Width="100%" CausesValidation="False">
                                        <Image Url="~/Images/Clear_16x16.png">
                                        </Image>
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:EmptyLayoutItem Width="20%">
                        </dx:EmptyLayoutItem>
                    </Items>
                </dx:LayoutGroup>
                <dx:LayoutGroup ColCount="4" ShowCaption="False">
                    <Items>
                        <dx:LayoutItem Caption="Customer Code" Width="25%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxGridLookup ID="ddCustomerCode" runat="server" Width="100%" AutoGenerateColumns="False" 
                                        ClientInstanceName="CustomerInfo" DataSourceID="SqlDataSourceCustomerLookup" 
                                        KeyFieldName="CusCode" TextFormatString="{0}">
                                        <GridViewProperties>
                                            <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" 
                                                AllowSelectByRowClick="True" EnableRowHotTrack="True" />
                                            <Settings ShowFilterBar="Auto" ShowFilterRow="True" />
                                        </GridViewProperties>
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="CusCode" ShowInCustomizationForm="True" 
                                                VisibleIndex="0" Width="100px">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="CusDesc" ShowInCustomizationForm="True" 
                                                VisibleIndex="1" Width="400px">
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <ClientSideEvents ValueChanged="function(s, e) {
	//alert(&quot;test&quot;);
	btnSelectCustomer.DoClick();
}" />
                                    </dx:ASPxGridLookup>
                                    <asp:SqlDataSource ID="SqlDataSourceCustomerLookup" runat="server" 
                                        ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                                        SelectCommand="Select CusCode,CusDesc From Customer WHERE DealerCode IN ('AAAAA',@DealerCode) Order by CusCode Desc">
                                        <SelectParameters>
                                            <asp:SessionParameter DefaultValue="00001" Name="DealerCode" 
                                                SessionField="DealerCode" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:EmptyLayoutItem ColSpan="2" Width="50%">
                        </dx:EmptyLayoutItem>
                        <dx:LayoutItem Caption="Customer Type" Width="25%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxComboBox ID="cmbCustomerType" runat="server" Width="100%" 
                                        ClientInstanceName="CustomerInfo" 
                                        OnSelectedIndexChanged="cmbCustomerType_SelectedIndexChanged" 
                                        SelectedIndex="0">
                                        <Items>
                                            <dx:ListEditItem Text="Indivisual" Value="Indivisual" Selected="True" />
                                            <dx:ListEditItem Text="Cooprate" Value="Cooprate" />
                                        </Items>
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Customer Name" ColSpan="4">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtCustomerName" runat="server" Width="100%" 
                                        ClientInstanceName="CustomerInfo" CssClass="uppercase" 
                                        OnTextChanged="txtCustomerName_TextChanged">
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
                <dx:LayoutGroup ColCount="4" ShowCaption="False">
                    <Items>
                        <dx:LayoutItem Caption="Address" ColSpan="4">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtAddress1" runat="server" Width="100%" 
                                        ClientInstanceName="CustomerInfo" CssClass="uppercase">
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:EmptyLayoutItem Width="10%">
                        </dx:EmptyLayoutItem>
                        <dx:LayoutItem ColSpan="3" ShowCaption="False" Width="90%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtAddress2" runat="server" Width="100%" 
                                        ClientInstanceName="CustomerInfo" CssClass="uppercase">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:EmptyLayoutItem Width="10%">
                        </dx:EmptyLayoutItem>
                        <dx:LayoutItem ColSpan="3" ShowCaption="False" Width="90%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtAddress3" runat="server" Width="100%" 
                                        ClientInstanceName="CustomerInfo" CssClass="uppercase">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
                <dx:LayoutGroup ColCount="5" ShowCaption="False">
                    <Items>
                        <dx:LayoutItem Caption="NIC No." Width="20%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtNICNo" runat="server" Width="100%" 
                                        ClientInstanceName="CustomerInfo">
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="NTN No." Width="20%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtNTNNo" runat="server" Width="100%" 
                                        ClientInstanceName="CustomerInfo">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="DOB" Width="20%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxDateEdit ID="dpDOB" runat="server" Width="100%" 
                                        DisplayFormatString="dd/MM/yyyy" ClientInstanceName="CustomerInfo" 
                                        EditFormat="Custom" EditFormatString="dd/MM/yyyy">
                                    </dx:ASPxDateEdit>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Sale Tax Reg. No." Width="20%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtSaleTaxRegNo" runat="server" Width="100%" 
                                        ClientInstanceName="CustomerInfo">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Distance(KM)" Width="20%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtDistance" runat="server" Width="100%">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
                <dx:LayoutGroup ColCount="5" ShowCaption="False">
                    <Items>
                        <dx:LayoutItem Caption="Phone" Width="20%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtPhoneNumber" runat="server" Width="100%">
                                        <MaskSettings IncludeLiterals="DecimalSymbol" />
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Cell No." Width="20%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtCellNumber" runat="server" Width="100%">
                                        <MaskSettings IncludeLiterals="DecimalSymbol" />
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Fax" Width="20%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtFaxNumber" runat="server" Width="100%">
                                        <MaskSettings IncludeLiterals="DecimalSymbol" />
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Email" Width="20%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtEmail" runat="server" Width="100%" CssClass="uppercase">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="URL" Width="20%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtURL" runat="server" Width="100%" CssClass="uppercase">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Phone 2" Width="25%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtPhoneNumber2" runat="server" Width="170px">
                                        <MaskSettings IncludeLiterals="DecimalSymbol" />
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
                <dx:LayoutGroup ColCount="4" ShowCaption="False">
                    <Items>
                        <dx:LayoutItem Caption="Credit Terms (Days)" Width="25%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxSpinEdit ID="spnCreditDays" runat="server" Number="0" Width="100%">
                                    </dx:ASPxSpinEdit>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Credit Limit" Width="25%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtCreditLimit" runat="server" Width="100%">
                                        <MaskSettings IncludeLiterals="DecimalSymbol" />
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Contact Person" Width="25%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtContactPerson" runat="server" Width="100%" 
                                        CssClass="uppercase">
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Father/Husband" Width="25%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtFatherHusband" runat="server" Width="100%" 
                                        CssClass="uppercase">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
            </Items>
        </dx:ASPxFormLayout>
    </center>
    <dx:ASPxButton ID="btnSelectCustomer" runat="server" Text="Select Customer" 
    CausesValidation="False" ClientInstanceName="btnSelectCustomer" 
    onclick="btnSelectCustomer_Click" CssClass="MoveOut" ClientVisible="False">
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
    <dx:ASPxButton ID="btnPopOk" runat="server" Text="Ok" OnClick="btnPopOk_Click" 
                        CausesValidation="False">
    </dx:ASPxButton>
    <dx:ASPxButton ID="btnPopCancel" runat="server" Text="Cancel" Visible="False" 
        OnClick="btnPopCancel_Click" CausesValidation="False">
    </dx:ASPxButton>


                </dx:PopupControlContentControl>
            </ContentCollection>
    </dx:ASPxPopupControl>  
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
