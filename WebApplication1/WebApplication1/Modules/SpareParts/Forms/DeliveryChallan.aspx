<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="DeliveryChallan.aspx.cs" Inherits="DXBMS.Modules.Transactions.DeliveryChallan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="container">
    <dx:ASPxPanel ID="ASPxPanelWrapper" runat="server" Height="100%" Width="100%"
     FixedPosition="WindowTop" ClientInstanceName="topPanel" Collapsible="true" 
        ScrollBars="Vertical" >
        <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
<script type="text/javascript">
//    function onCellClick(rowIndex, fieldName) {
//        ASPxGridView1.PerformCallback(rowIndex + "|" + fieldName);
//    }
//    function OnEditorKeyPress(editor, e) {
//        if (e.htmlEvent.keyCode == 13 || e.htmlEvent.keyCode == 9) {
//            ASPxGridView1.UpdateEdit();
//        }
//        else
//            if (e.htmlEvent.keyCode == 27)
//                ASPxGridView1.CancelEdit();
//    }
//    function OnGetRowValuesCallback(values) {
//        alert(values);
    //    }
    var row = '';
    var itm = '';
    var dq = '0';
    var bq = '0';
    function OnGetRowValues(values) {
        //alert(mval + '|' + values);
        bq = values[0];
        itm = values[1];
        //alert(dq + '|' + bq);
        if (dq > bq) {
            alert(row+'-'+dq + '|' + bq+'Delivery Qauntity can not greater than Balance Quantity');
        }
        //clb.PerformCallback(row + '|' + itm+'|'+bq+'|'+dq);
    }
    </script>
                <center>
                    <div class="Heading">
            DELIVERY CHALLAN
        </div>
                    <dx:ASPxLabel ID="ErrorMsg" runat="server" Text="" ForeColor="#009900" 
                        Visible="False">
                    </dx:ASPxLabel>
                   <table width="100%">
                   <tr>
                   <td>
                   <center>
                   </center>
                   </td>
                   </tr>
                   </table>
                    <dx:ASPxFormLayout ID="ASPxFormLayoutDCTop" runat="server" Width="100%">
                        <Items>
                            <dx:LayoutGroup ColCount="5" ShowCaption="False">
                                <Items>
                                    <dx:EmptyLayoutItem>
                                    </dx:EmptyLayoutItem>
                                    <dx:LayoutItem ShowCaption="False">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" Width="100%" 
                                                    OnClick="btnSave_Click">
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
                                                <dx:ASPxButton ID="btnDelete" runat="server" Text="Delete" Width="100%" 
                                                    OnClick="btnDelete_Click">
                                                    <Image Url="~/Images/Delete_16x16.png">
                                                    </Image>
                                                </dx:ASPxButton>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem ShowCaption="False">
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
                                    <dx:EmptyLayoutItem>
                                    </dx:EmptyLayoutItem>
                                </Items>
                            </dx:LayoutGroup>
                            
                            <dx:LayoutGroup ColCount="3" ShowCaption="False">
                                <Items>
                                    <dx:LayoutItem Caption="D/C No.*">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxGridLookup ID="ddDCNumber" runat="server" 
                                                     DataSourceID="SqlDataSourceDCLookup" KeyFieldName="DCNo" 
                                                    TextFormatString="{0}">
                                                    <GridViewProperties>
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" 
                                                            AllowSelectByRowClick="True" AllowSort="False" EnableRowHotTrack="True" />
                                                        <Settings ShowFilterBar="Auto" ShowFilterRow="True" />
                                                    </GridViewProperties>
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn FieldName="DCNo" ShowInCustomizationForm="True" 
                                                            VisibleIndex="0">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="DCDate" ReadOnly="True" 
                                                            ShowInCustomizationForm="True" VisibleIndex="1">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="PONo" ShowInCustomizationForm="True" 
                                                            VisibleIndex="2">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="VendorDesc" 
                                                            ShowInCustomizationForm="True" VisibleIndex="3">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="VendorDCNo" 
                                                            ShowInCustomizationForm="True" VisibleIndex="3">
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <ClientSideEvents ValueChanged="function(s, e) {
	btnDCSelect.DoClick();
}" />
                                                </dx:ASPxGridLookup>
                                                <asp:SqlDataSource ID="SqlDataSourceDCLookup" runat="server" 
                                                    ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" SelectCommand="select  a.DCNo,  convert(varchar(12),a.DCDate,103) as DCDate , a.PONo,  b.VendorDesc, a.VendorDCNo  from DCMaster a,Vendor b where  a.VendorCode = b.VendorCode and a.DealerCode in ('00001','AAAAA') and a.DelFlag = 'N' and a.VendorCode = b.VendorCode and a.DCNo Not IN  (Select DCNo from GRNMaster Where DealerCode in (@DealerCode,'AAAAA') and Delflag = 'N') 
ORDER BY A.DCNO DESC">
<SelectParameters>
                                                        <asp:SessionParameter DefaultValue="00001" Name="DealerCode" 
                                                            SessionField="DealerCode" />
                                                    </SelectParameters>
</asp:SqlDataSource>

                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="D/C Date">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxDateEdit ID="dpDCDate" runat="server" ViewStateMode="Enabled" EditFormat="Custom"
                                                    DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                </dx:ASPxDateEdit>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Status" HorizontalAlign="Right">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxLabel ID="lblDCStatus" runat="server" Text="-">
                                                </dx:ASPxLabel>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="PO No.*">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxGridLookup ID="ddPONumber" runat="server" AutoGenerateColumns="False" 
                                                    DataSourceID="SqlDataSourcePODCLookup" KeyFieldName="POno" 
                                                    TextFormatString="{0}" OnTextChanged="ddPONumber_TextChanged">
                                                    <GridViewProperties>
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" 
                                                            EnableRowHotTrack="True" />
                                                        <Settings ShowFilterBar="Auto" ShowFilterRow="True" />
                                                    </GridViewProperties>
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn FieldName="POno" ShowInCustomizationForm="True" 
                                                            VisibleIndex="0">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="PODate" ReadOnly="True" 
                                                            ShowInCustomizationForm="True" VisibleIndex="1">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="VendorDesc" 
                                                            ShowInCustomizationForm="True" VisibleIndex="2">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="NName" ShowInCustomizationForm="True" 
                                                            VisibleIndex="3">
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <ClientSideEvents ValueChanged="function(s, e) {
	btnPOSelect.DoClick();
}" />
                                                </dx:ASPxGridLookup>
                                                <dx:ASPxLabel ID="lblPONo" runat="server" Text="PONo" Visible="False">
                                                </dx:ASPxLabel>
                                                <asp:SqlDataSource ID="SqlDataSourcePODCLookup" runat="server" 
                                                    ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" SelectCommand="Select  a.POno,  convert(varchar(20),a.PODate,103) as PODate, c.VendorDesc, d.NName  
	from  POMaster a,PODetail b,Vendor c,partsDeliveryTypes d   
                Where a.VendorCode = c.VendorCode  and a.DealerCode = b.DealerCode 
                 and a.PONo = b.PONo  
				 and a.DeliveryType = d.NName
                 and a.DealerCode in( @DealerCode)
				 and a.TransferStatus ='T'
--and a.POno not in (select PONo from DCMaster) 
                 Group By a.PONo,a.PODate,c.VendorDesc, a.deliveryType, d.NName  
                 Having sum(b.POQty) &gt; sum(b.RecQty) Order by a.PONo DESC">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="DealerCode" SessionField="DealerCode" 
                                                            DefaultValue="00001" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="PO Date">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxDateEdit ID="dpPODate" runat="server" Enabled="False" 
                                                    DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                </dx:ASPxDateEdit>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Vendor D/C No.">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxTextBox ID="txtVendorDCNo" runat="server" Width="170px">
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Vendor">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxTextBox ID="txtVendorCode" runat="server" Width="170px" Enabled="False">
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem ColSpan="2" ShowCaption="False">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server" 
                                                SupportsDisabledAttribute="True">
                                                <dx:ASPxTextBox ID="txtVendorName" runat="server" Width="100%" Enabled="False">
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Remarks" ColSpan="3">
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
                        </Items>
                    </dx:ASPxFormLayout>
                    <dx:ASPxGridView ID="GridViewDC" runat="server" AutoGenerateColumns="False" 
                        ClientInstanceName="grid" DataSourceID="SqlDataSourceDCItems" 
                        KeyFieldName="PartItemNo" OnBatchUpdate="GridViewDC_BatchUpdate1" 
                        OnRowUpdating="GridViewDC_RowUpdating1" Width="100%" 
                        OnSelectionChanged="GridViewDC_SelectionChanged" style="margin-right: 0px" 
                        EnableCallBacks="False">
                        <ClientSideEvents RowClick="function(s, e) {
	s.StartEditRow(e.visibleIndex);
}" />
                        <Columns>
                            <dx:GridViewCommandColumn ShowInCustomizationForm="True" VisibleIndex="0">
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn FieldName="PartItemNo" 
                                ShowInCustomizationForm="True" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="PartItemName" 
                                ShowInCustomizationForm="True" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="unit" ShowInCustomizationForm="True" 
                                VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="POQuantity" 
                                ShowInCustomizationForm="True" VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="BalanceQuantity" 
                                ShowInCustomizationForm="True" VisibleIndex="7">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataSpinEditColumn FieldName="DCQuantity" 
                                ShowInCustomizationForm="True" VisibleIndex="8">
                                <PropertiesSpinEdit DisplayFormatString="g">
                                </PropertiesSpinEdit>
                            </dx:GridViewDataSpinEditColumn>
                            <dx:GridViewDataCheckColumn FieldName="Selected" ShowInCustomizationForm="True" 
                                VisibleIndex="1">
                                <DataItemTemplate>
                                    <dx:ASPxCheckBox ID="cbCheck" runat="server" AutoPostBack="True" 
                                        oncheckedchanged="cbCheck_CheckedChanged" onload="cbCheck_Load">
                                    </dx:ASPxCheckBox>
                                </DataItemTemplate>
                                <HeaderTemplate>
                                    <dx:ASPxCheckBox ID="SelectAllCheckBox" runat="server" CheckState="Checked">
                                        <ClientSideEvents CheckedChanged="function(s, e) { grid.SelectAllRowsOnPage(s.GetChecked()); grid.PerformCallback(); }" />
                                    </dx:ASPxCheckBox>
                                </HeaderTemplate>
                            </dx:GridViewDataCheckColumn>
                        </Columns>
                        <SettingsBehavior AllowFocusedRow="True" AllowSort="False" />
                        <SettingsEditing Mode="Inline">
                        </SettingsEditing>
                        <SettingsDataSecurity AllowDelete="False" AllowInsert="False" />
                    </dx:ASPxGridView>
                    <asp:SqlDataSource ID="SqlDataSourceDCItems" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                        SelectCommand="EXEC SP_Spare_Get_DCDetails '',''" UpdateCommandType="Text">
                        <UpdateParameters>
                            <asp:Parameter Name="BalQty" />
                        </UpdateParameters>
                    </asp:SqlDataSource>

                </center>
    
            </dx:PanelContent>
</PanelCollection>
    </dx:ASPxPanel>
    <dx:ASPxButton ID="btnPOSelect" runat="server" Text="PO" 
        ClientInstanceName="btnPOSelect" onclick="btnPOSelect_Click" 
        CssClass="MoveOut" ClientVisible="False">
    </dx:ASPxButton>
    <dx:ASPxButton ID="btnDCSelect" runat="server" Text="DC" 
        ClientInstanceName="btnDCSelect" onclick="btnDCSelect_Click" 
        CssClass="MoveOut" ClientVisible="False">
    </dx:ASPxButton>
    <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="clb" 
        oncallback="ASPxCallback1_Callback">
        <ClientSideEvents EndCallback="function(s, e) {
	lp.Hide();
}" />
    </dx:ASPxCallback>
    <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" 
        ClientInstanceName="lp">
    </dx:ASPxLoadingPanel>
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
