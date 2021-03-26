<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="SalesReturnReport.aspx.cs" Inherits="DXBMS.Modules.Reports.SalesReturnReport" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">

    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <div class="container">
              <center>
              <div class="Heading">Part Sale and Return Detail Report</div>
              <div class="row">
                <div class="col-lg-12">
                    <fieldset>
                        <table style="border-collapse: separate; width:100%; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">
                            <tr>
                                <td width="10%">
                                            <asp:Label ID="Label1" runat="server" Text="Check All" style="font-weight: 700; font-style: normal; font-size: small;"></asp:Label>
                                            <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" Checked="True" CheckState="Checked" 
                                                OnCheckedChanged="chkAllItem_CheckedChanged" >
                                            </asp:CheckBox>
                                        </td>
                                        <td>
                                            <span>From Item</span>
                                            <asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png" Enabled="false" OnClick="btnLookup_Click" />
                                        </td>
                                        <td width="35%">
                                            <asp:TextBox ID="txtFromItemCode" runat="server" ReadOnly="true"></asp:TextBox>
                                            <%--<asp:DropDownList ID="ddlFromCode" width="100%" runat="server" AutoPostBack="True" Enabled="false"></asp:DropDownList>--%>
                                        </td>
                                        <td width="10%">
                                            <span>To Item</span>
                                            <asp:ImageButton runat="server" ID="imgLookup2" ImageUrl="~/Images/LookupNew.png" Enabled="false" OnClick="imgLookup2_Click" />
                                        </td>
                                        <td width="35%">
                                            <asp:TextBox ID="txtToItemCode" runat="server" ReadOnly="true"></asp:TextBox>
                                            <%--<asp:DropDownList ID="ddlToCode" width="100%" runat="server" AutoPostBack="True" Enabled="false"></asp:DropDownList>--%>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                    <td></td>
                <td>
                    FromDate:
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" Width="150px"  style="background-image: url(../../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"></asp:TextBox>
                    <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" 
                        Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtFromDate">
                    </asp:CalendarExtender>
                </td>
                
                <td>
                    ToDate:
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" Width="150px" style="background-image: url(../../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"></asp:TextBox>
                    <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" 
                        Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtToDate">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
            <td colspan="4"><asp:Label ID="lblMessage" runat="server"></asp:Label></td>
                <td>
                    <asp:Button ID="btnRptPrint" runat="server" OnClick="btnPrint_Click" Text="Print" ></asp:Button>
                </td>
            </tr>
                        </table>
                    </fieldset>
                </div>
              </div>

       <%--<dx:ASPxLabel ID="lblReportTitle" runat="server" Font-Bold="True" Font-Size="20pt"></dx:ASPxLabel>
       
        
       
      <dx:ASPxFormLayout ID="ASPxFormLayoutDateRange" runat="server" Width="600px">
          <Items>
              <dx:LayoutGroup Caption="Item" ColCount="3">
                  <Items>
                      <dx:LayoutItem Caption="All" Width="35%" ShowCaption="False">
                          <LayoutItemNestedControlCollection>
                              <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer1" runat="server" 
                                  SupportsDisabledAttribute="True">
                                  <dx:ASPxCheckBox ID="chkAllItem" runat="server" Checked="True" 
                                      CheckState="Checked" Text="All" 
                                      OnCheckedChanged="chkAllItem_CheckedChanged" AutoPostBack="True" 
                                      Width="100%">
                                  </dx:ASPxCheckBox>
                              </dx:LayoutItemNestedControlContainer>
                          </LayoutItemNestedControlCollection>
                      </dx:LayoutItem>
                      <dx:LayoutItem Caption="From Item" Width="30%">
                          <LayoutItemNestedControlCollection>
                              <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer2" runat="server" 
                                  SupportsDisabledAttribute="True">
                                  <dx:ASPxGridLookup ID="glFromItem" runat="server" AutoGenerateColumns="False" 
                                      DataSourceID="SqlDataSourceItems" KeyFieldName="PartItemNo" 
                                      TextFormatString="{0}" ClientEnabled="False">
                                      <GridViewProperties>
                                          <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" 
                                              AllowSelectByRowClick="True" EnableRowHotTrack="True" />
                                          <Settings ShowFilterBar="Visible" ShowFilterRow="True" />
                                      </GridViewProperties>
                                      <Columns>
                                          <dx:GridViewDataTextColumn FieldName="PartItemNo" 
                                              ShowInCustomizationForm="True" VisibleIndex="0">
                                          </dx:GridViewDataTextColumn>
                                          <dx:GridViewDataTextColumn FieldName="ItemCode" ShowInCustomizationForm="True" 
                                              VisibleIndex="1">
                                          </dx:GridViewDataTextColumn>
                                          <dx:GridViewDataTextColumn FieldName="ItemDesc" ShowInCustomizationForm="True" 
                                              VisibleIndex="2">
                                          </dx:GridViewDataTextColumn>
                                          <dx:GridViewDataTextColumn FieldName="ProdCode" ShowInCustomizationForm="True" 
                                              VisibleIndex="3">
                                          </dx:GridViewDataTextColumn>
                                      </Columns>
                                  </dx:ASPxGridLookup>
                                  <asp:SqlDataSource ID="SqlDataSourceItems" runat="server" 
                                      ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                                      SelectCommand="select PartItemNo,ItemCode,ItemDesc,ProdCode from item where DealerCode in ('AAAAA',@DealerCode)">
                                      <SelectParameters>
                                          <asp:SessionParameter DefaultValue="00001" Name="DealerCode" 
                                              SessionField="DealerCode" />
                                      </SelectParameters>
                                  </asp:SqlDataSource>
                              </dx:LayoutItemNestedControlContainer>
                          </LayoutItemNestedControlCollection>
                      </dx:LayoutItem>
                      <dx:LayoutItem Width="30%" Caption="To Item">
                          <LayoutItemNestedControlCollection>
                              <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer3" runat="server" 
                                  SupportsDisabledAttribute="True">
                                  <dx:ASPxGridLookup ID="glToItem" runat="server" AutoGenerateColumns="False" 
                                      DataSourceID="SqlDataSourceItems" KeyFieldName="PartItemNo" 
                                      TextFormatString="{0}" ClientEnabled="False">
                                      <GridViewProperties>
                                          <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" 
                                              AllowSelectByRowClick="True" EnableRowHotTrack="True" />
                                      </GridViewProperties>
                                      <Columns>
                                          <dx:GridViewDataTextColumn FieldName="PartItemNo" 
                                              ShowInCustomizationForm="True" VisibleIndex="0">
                                          </dx:GridViewDataTextColumn>
                                          <dx:GridViewDataTextColumn FieldName="ItemCode" ShowInCustomizationForm="True" 
                                              VisibleIndex="1">
                                          </dx:GridViewDataTextColumn>
                                          <dx:GridViewDataTextColumn FieldName="ItemDesc" ShowInCustomizationForm="True" 
                                              VisibleIndex="2">
                                          </dx:GridViewDataTextColumn>
                                          <dx:GridViewDataTextColumn FieldName="ProdCode" ShowInCustomizationForm="True" 
                                              VisibleIndex="3">
                                          </dx:GridViewDataTextColumn>
                                      </Columns>
                                  </dx:ASPxGridLookup>
                              </dx:LayoutItemNestedControlContainer>
                          </LayoutItemNestedControlCollection>
                      </dx:LayoutItem>
                  </Items>
              </dx:LayoutGroup>
              <dx:LayoutGroup Caption="Date Range" ColCount="3">
                  <Items>
                      <dx:LayoutItem Caption="From" Width="35%">
                          <LayoutItemNestedControlCollection>
                              <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer4" runat="server" 
                                  SupportsDisabledAttribute="True">
                                  <dx:ASPxDateEdit ID="dpFromDate" runat="server" 
                                      DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" 
                                      EditFormatString="dd/MM/yyyy" Width="100%">
                                  </dx:ASPxDateEdit>
                              </dx:LayoutItemNestedControlContainer>
                          </LayoutItemNestedControlCollection>
                      </dx:LayoutItem>
                      <dx:LayoutItem Caption="To" Width="35%">
                          <LayoutItemNestedControlCollection>
                              <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer5" runat="server" 
                                  SupportsDisabledAttribute="True">
                                  <dx:ASPxDateEdit ID="dpToDate" runat="server" Width="100%">
                                  </dx:ASPxDateEdit>
                              </dx:LayoutItemNestedControlContainer>
                          </LayoutItemNestedControlCollection>
                      </dx:LayoutItem>
                      <dx:LayoutItem ShowCaption="False" Width="30%">
                          <LayoutItemNestedControlCollection>
                              <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer6" runat="server" 
                                  SupportsDisabledAttribute="True">
                                  <dx:ASPxButton ID="btnPrint" runat="server" OnClick="btnPrint_Click" 
                                      Text="Print" Width="100%">
                                      <Image Url="~/Images/Print_16x16.png">
                                      </Image>
                                  </dx:ASPxButton>
                              </dx:LayoutItemNestedControlContainer>
                          </LayoutItemNestedControlCollection>
                      </dx:LayoutItem>
                  </Items>
              </dx:LayoutGroup>
          </Items>
      </dx:ASPxFormLayout>
  </center>
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
    </dx:ASPxPopupControl>--%>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
            
</asp:Content>
