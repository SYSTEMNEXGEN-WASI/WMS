<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="CustomerPaymentReceipt.aspx.cs" Inherits="DXBMS.Modules.Transactions.CustomerPaymentReceipt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <dx:ASPxPanel ID="ASPxPanel1" runat="server" Height="100%" Width="100%"
      FixedPosition="WindowTop" ClientInstanceName="topPanel" Collapsible="true" 
        ScrollBars="Vertical"
    >
        <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
     <center>
    <div class="Heading">
            CUSTOMER PAYMENT RECIEPT ENTRY
        </div>
    <dx:ASPxFormLayout ID="ASPxFormLayoutCPRTop" runat="server" Width="100%">
        <Items>
            <dx:LayoutGroup ColCount="5" ShowCaption="False">
                <Items>
                    <dx:LayoutItem ShowCaption="False" Width="20%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer1" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" 
                                    Width="100%" Enabled="False" OnClick="btnSave_Click">
                                    <Image Url="~/Images/Ribbon_Save_16x16.png">
                                    </Image>
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem ShowCaption="False" Width="20%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer2" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btnDelete" runat="server" Text="Delete" 
                                    Width="100%" Enabled="False" OnClick="btnDelete_Click">
                                    <ClientSideEvents Click="function(s, e) {
	var yn = confirm(&quot;Are you sure to delete?&quot;);
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
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer3" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btnClear" runat="server" Text="Clear" 
                                    Width="100%" OnClick="btnClear_Click">
                                    <Image Url="~/Images/Clear_16x16.png">
                                    </Image>
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem ShowCaption="False" Width="20%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer4" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btnPrint" runat="server" Text="Print" 
                                    Width="100%" Enabled="False" OnClick="btnPrint_Click">
                                    <Image Url="~/Images/Print_16x16.png">
                                    </Image>
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:EmptyLayoutItem Width="20%">
                    </dx:EmptyLayoutItem>
                </Items>
            </dx:LayoutGroup>
            <dx:LayoutGroup ColCount="5" ShowCaption="False">
                <Items>
                    <dx:EmptyLayoutItem Width="20%">
                    </dx:EmptyLayoutItem>
                    <dx:LayoutItem ColSpan="3" ShowCaption="False" Width="60%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer5" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxRadioButtonList ID="rbPaymentReceiptType" runat="server" 
                                    RepeatDirection="Horizontal" SelectedIndex="1" Width="100%" 
                                    AutoPostBack="True" 
                                    OnSelectedIndexChanged="rbPaymentReceiptType_SelectedIndexChanged">
                                    <Items>
                                        <dx:ListEditItem Text="Service Invoice(s)" Value="Service" />
                                        <dx:ListEditItem Selected="True" Text="Counter Sales Invoice(s)" 
                                            Value="CounterS" />
                                    </Items>
                                </dx:ASPxRadioButtonList>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:EmptyLayoutItem Width="20%">
                    </dx:EmptyLayoutItem>
                    <dx:LayoutItem Caption="Receipt No." Width="20%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer6" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxGridLookup ID="gvlReceiptNo" runat="server" Width="100%" 
                                    AutoGenerateColumns="False" DataSourceID="SqlDataSourcePayRctLookup" 
                                    KeyFieldName="ReceiptNo" TextFormatString="{0}">
                                    <GridViewProperties>
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" 
                                            AllowSelectByRowClick="True" EnableRowHotTrack="True" />
                                        <Settings ShowFilterBar="Auto" ShowFilterRow="True" />
                                    </GridViewProperties>
                                    <Columns>
                                        <dx:GridViewDataTextColumn FieldName="ReceiptNo" ShowInCustomizationForm="True" 
                                            VisibleIndex="0">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="ReceiptDate" ReadOnly="True" 
                                            ShowInCustomizationForm="True" VisibleIndex="1">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="CusCode" ShowInCustomizationForm="True" 
                                            VisibleIndex="2">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="CusDesc" ShowInCustomizationForm="True" 
                                            VisibleIndex="3">
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                    <ClientSideEvents ValueChanged="function(s, e) {
	btnSelectReceipt.DoClick();
}" />
                                </dx:ASPxGridLookup>
                                <asp:SqlDataSource ID="SqlDataSourcePayRctLookup" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" SelectCommand="Select Distinct ReceiptNo,dbo.getNewDate(ReceiptDate) As ReceiptDate,a.CusCode,CusDesc from PaymentReceiptMaster a,Customer b 
WHERE a.DelFlag = 'N'
 AND a.DealerCode = b.DealerCode
  AND a.CusCode = b.CusCode
order by ReceiptNo Desc"></asp:SqlDataSource>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:EmptyLayoutItem ColSpan="3" Width="60%">
                    </dx:EmptyLayoutItem>
                    <dx:LayoutItem Caption="Receipt Date" Width="20%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer7" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxDateEdit ID="dpReceiptDate" runat="server" Width="100px" 
                                    DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" 
                                    EditFormatString="dd/MM/yyyy">
                                </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Customer" Width="20%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer8" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxGridLookup ID="gvlCustomerCode" runat="server" Width="100%" 
                                    AutoGenerateColumns="False" DataSourceID="SqlDataSourceCustomerList" 
                                    KeyFieldName="Ref No" TextFormatString="{0}">
                                    <GridViewProperties>
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" 
                                            AllowSelectByRowClick="True" EnableRowHotTrack="True" />
                                        <Settings ShowFilterBar="Auto" ShowFilterRow="True" />
                                    </GridViewProperties>
                                    <Columns>
                                        <dx:GridViewDataTextColumn FieldName="Customer Code" 
                                            ShowInCustomizationForm="True" VisibleIndex="0">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Customer" ShowInCustomizationForm="True" 
                                            VisibleIndex="1">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Ref No" ShowInCustomizationForm="True" 
                                            VisibleIndex="2">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Balance" ReadOnly="True" 
                                            ShowInCustomizationForm="True" VisibleIndex="3">
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                    <ClientSideEvents ValueChanged="function(s, e) {
	btnSelectCustomer.DoClick();
}" />
                                </dx:ASPxGridLookup>
                                <asp:SqlDataSource ID="SqlDataSourceCustomerList" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" SelectCommand="Select b.CusCode [Customer Code],b.CusDesc Customer,a.SaleInvNo [Ref No],ROUND(a.InvoiceAmount,0) - ROUND(a.TotReceipt,0) as 'Balance' 
from CounterSaleMaster a,Customer b
WHERE a.DelFlag = 'N'
 AND a.DealerCode = b.DealerCode
 AND a.CusCode = b.CusCode
 AND ROUND(a.InvoiceAmount,0) &gt; ROUND(a.TotReceipt,0)
 AND PostFlag = 'Y' AND A.InvoiceAmount - A.TotReceipt &gt; 1"></asp:SqlDataSource>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem ColSpan="4" ShowCaption="False" Width="60%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer9" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtCustomerName" runat="server" Width="100%" 
                                    ReadOnly="True">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Remarks" ColSpan="5">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer10" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtRemarks" runat="server" Width="100%">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
            <dx:LayoutGroup ColCount="5" ShowCaption="False">
                <Items>
                    <dx:LayoutItem Caption="Payment Mode" ColSpan="2" Width="40%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer11" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxComboBox ID="cmbPaymentMode" runat="server" Width="50%" 
                                    DataSourceID="SqlDataSourcePaymentMode" TextField="PayModeDesc" 
                                    ValueField="PayModeCode">
                                </dx:ASPxComboBox>
                                <asp:SqlDataSource ID="SqlDataSourcePaymentMode" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                                    SelectCommand="select * from PaymentMode"></asp:SqlDataSource>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Instrument No." Width="20%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer12" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtInstrumentNo" runat="server" Width="100%">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Instrument Date" Width="20%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer13" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxDateEdit ID="dpInstDate" runat="server" Width="100px" 
                                    DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" 
                                    EditFormatString="dd/MM/yyyy">
                                </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Amount" Width="20%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer14" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtInstAmt" runat="server" Width="100%">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
            <dx:LayoutGroup ColCount="4" ShowCaption="False">
                <Items>
                    <dx:LayoutItem Caption="Bank" Width="25%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer15" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxComboBox ID="cmbBank" runat="server" Width="100%" TextField="BankDesc" 
                                    ValueField="BankCode">
                                </dx:ASPxComboBox>
                                <asp:SqlDataSource ID="SqlDataSourceBankList" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                                    SelectCommand="exec sp_2W_Select_Bank"></asp:SqlDataSource>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Branch" Width="25%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer16" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtBranch" runat="server" Width="100%">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Width="25%" Caption="Total Amount">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer17" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtTotAmount" runat="server" Width="100%" ReadOnly="True">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Width="25%" Caption="Inv #">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer18" runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="txtInvNo" runat="server" Width="100%" ReadOnly="True">
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
        </Items>
        <Paddings Padding="0px" />
    </dx:ASPxFormLayout>
         <asp:HiddenField ID="txtAdj" runat="server" />
         <dx:ASPxGridView ID="gvPendingInvoice" runat="server" Width="100%" 
             ViewStateMode="Enabled">
            <TotalSummary>
                <dx:ASPxSummaryItem DisplayFormat="Total : {0}" FieldName="Ref Amount" 
                    ShowInColumn="Ref Amount" SummaryType="Sum" Tag="RefAmount" />
                <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="OutStanding" 
                    ShowInColumn="OutStanding" SummaryType="Sum" Tag="OutStanding" />
                <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="Adjustment" 
                    ShowInColumn="Adjustment" SummaryType="Sum" Tag="Adjustment" />
            </TotalSummary>
            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" 
                AllowSelectSingleRowOnly="True" EnableRowHotTrack="True" AllowGroup="False" 
                 AllowSort="False" />
            <Settings ShowFooter="True" />
             <SettingsDataSecurity AllowDelete="False" AllowEdit="False" 
                 AllowInsert="False" />
        </dx:ASPxGridView>
        <dx:ASPxFormLayout ID="ASPxFormLayoutBottom" runat="server" Width="100%">
            <Items>
                <dx:LayoutGroup ColCount="5" ShowCaption="False">
                    <Items>
                        <dx:LayoutItem ShowCaption="False" Width="20%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxButton ID="btnLoadAllPendingInv" runat="server" Enabled="False" 
                                        Text="Load Pending Invoices" Width="100%" 
                                        OnClick="btnLoadAllPendingInv_Click">
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Inv. # :" Width="20%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="txtSrchInvNo" runat="server" Enabled="False" Width="100%">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem ShowCaption="False" Width="20%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxButton ID="btnAddInvToGrid" runat="server" Enabled="False" 
                                        Text="Add Inv" Width="100%" OnClick="btnAddInvToGrid_Click">
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem ShowCaption="False" Width="20%" HorizontalAlign="Right">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxButton ID="btnRemovePendingRow" runat="server" OnClick="btnRemovePendingRow_Click" 
                                        Text="Remove Inv" Width="100%" Enabled="False">
                                        <ClientSideEvents Click="function(s, e) {
	var yn = confirm(&quot;Are you sure to delete?&quot;);
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
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem ShowCaption="False" Width="20%" HorizontalAlign="Right">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer22" runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxButton ID="btnAutoAdj" runat="server" Text="Auto Adjust (FIFO)" 
                                        OnClick="btnAutoAdj_Click" Enabled="False" Width="100%">
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
            </Items>
        </dx:ASPxFormLayout>
        <table width="1000">
            <tr>
                <td style="width:50%;">
                    <dx:ASPxFormLayout ID="ASPxFormLayoutTax" runat="server" Width="500px">
                        <Items>
                            <dx:LayoutGroup ColCount="4" ShowCaption="False">
                                <Items>
                                    <dx:LayoutItem ColSpan="2">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer23" runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxComboBox ID="cmbReceiptHead" runat="server" Width="100%" 
                                                    SelectedIndex="0">
                                                    <Items>
                                                        <dx:ListEditItem Selected="True" Text="Cash Discount" Value="CashDiscount" />
                                                        <dx:ListEditItem Text="Service Charges" Value="ServiceCharges" />
                                                        <dx:ListEditItem Text="Other Charges" Value="OtherCharges" />
                                                    </Items>
                                                </dx:ASPxComboBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Amount">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer24" runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxTextBox ID="txtTaxAmt" runat="server" Width="100%">
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:EmptyLayoutItem>
                                    </dx:EmptyLayoutItem>
                                    <dx:LayoutItem ShowCaption="False">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer25" runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxButton ID="btnAddTax" runat="server" Text="Add" 
                                                    Width="100%" OnClick="btnAddTax_Click" Enabled="False">
                                                </dx:ASPxButton>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem ShowCaption="False">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer26" runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxButton ID="btnTaxClear" runat="server" Text="Clear" 
                                                    Width="100%" Enabled="False" OnClick="btnTaxClear_Click">
                                                </dx:ASPxButton>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem ShowCaption="False">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer27" runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxButton ID="btnTaxRemove" runat="server" Text="Delete" 
                                                    Width="100%" Enabled="False" OnClick="btnTaxRemove_Click">
                                                </dx:ASPxButton>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:EmptyLayoutItem>
                                    </dx:EmptyLayoutItem>
                                </Items>
                            </dx:LayoutGroup>
                        </Items>
                    </dx:ASPxFormLayout>
                </td>
                <td style="width:50%;">
                    <dx:ASPxGridView ID="gvTaxDetail" runat="server" Width="400px" 
                        KeyFieldName="SNO">
                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" 
                            AllowSelectSingleRowOnly="True" EnableRowHotTrack="True" 
                            AllowSort="False" />
                    </dx:ASPxGridView>
                    <table>
                        <tr>
                            <th>
                                Total Tax:
                            </th>
                            <td>
                                <dx:ASPxTextBox ID="txtTotTax" runat="server" 
                                        Width="170px" ReadOnly="True">
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                    </table>
              
                    
                </td>
            </tr>
        
        </table>
    </center>
            </dx:PanelContent>
</PanelCollection>
    </dx:ASPxPanel>
   
    <dx:ASPxButton ID="btnSelectCustomer" runat="server" Text="Select Cust" 
    ClientInstanceName="btnSelectCustomer" onclick="btnSelectCustomer_Click" 
        CssClass="MoveOut" ClientVisible="False">
    </dx:ASPxButton>

     <dx:ASPxButton ID="btnSelectReceipt" runat="server" 
        ClientInstanceName="btnSelectReceipt" onclick="btnSelectReceipt_Click" 
        Text="Select Receipt" CssClass="MoveOut" ClientVisible="False">
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
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
