<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" theme="Theme1" AutoEventWireup="true" CodeBehind="FreeService.aspx.cs" Inherits="DXBMS.Modules.Service.Forms.FreeService" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
      <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>
 <script src="../../Data/sweetalert.js" type="text/javascript"></script>  
 <script type="text/javascript">
           function Savealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been saved',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "FreeService.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "FreeService.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "FreeService.aspx";
           }
           function reloads() {
               window.location.href = "FreeService.aspx";
           }

       </script>

 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                <div class="container">

                <h3 class="Heading">Free Service Covering Letter</h3>
                    
            
            <fieldset>
                <legend>Billings Detail</legend>
                <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                    <%--Row1--%>
                    <tr>
                        <td colspan="1">
                            Billing No : <asp:ImageButton ID="ImageButton1" runat="server" Height="17px" AutoPostBack="true" ImageUrl="~/Images/LookupNew.png"
                                Width="17px" onclick="ImageButton1_Click1" />
                        </td>
                        <td colspan="2">
                         <asp:TextBox ID="txtBillingNo" runat="server" Width="100%" AutoPostBack="True" 
                                ontextchanged="txtBillingNo_TextChanged" ReadOnly="True"></asp:TextBox>
                                <asp:DropDownList ID="ddlBillingNo" Visible="false" runat="server" AutoPostBack="True" style="width: 100% !important;"
                                onselectedindexchanged="ddlBillingNo_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td colspan="1">
                            Bill Date:
                        </td>
                        <td colspan="2">
                         <%--<asp:TextBox ID="txtBillDate" width="100%" runat="server" TabIndex="8" ReadOnly="true" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>--%>
                           <asp:TextBox ID="txtBillDate" runat="server"  CausesValidation="false" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="txtInvoiceDate_MaskedEditExtender" runat="server"
                                                ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="true" Mask="99-99-9999" TargetControlID="txtBillDate"
                                                UserDateFormat="DayMonthYear" />
                                            <ajaxToolkit:CalendarExtender ID="txtInvoiceDate_CalendarExtender" runat="server"
                                                Enabled="true" Format="dd-MM-yyyy" TargetControlID="txtBillDate" />
                        </td>
                       <td colspan="1">
                            Remarks :
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="100" TabIndex="16" TextMode="MultiLine" Width="100%" ></asp:TextBox>
                        </td>
                    </tr>
                    
                    <%-- <tr>
                        <td align="right">
                            PST Code:</td>
                        <td>
                            &nbsp;</td>
                        <td colspan="5">
                            <asp:TextBox ID="txtCode" runat="server" AutoPostBack="True" ReadOnly="true" CssClass="ROTextBoxes" 
                                ontextchanged="txtBillingNo_TextChanged" Width="90px" MaxLength="8"></asp:TextBox>
                        </td>
                    </tr>--%>
                </table>
            </fieldset>
            <%--Jobcards Section Starts here--%>
            <fieldset>
                <legend>Job Cards</legend>
                <table width=100% style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                    <tr>
                        <td colspan="1">
                            From Date :
                        </td>                        
                        <td colspan="2">
                       <asp:TextBox ID="txtFromDate" runat="server" Width="90px"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server"
                                                ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="true" Mask="99-99-9999" TargetControlID="txtFromDate"
                                                UserDateFormat="DayMonthYear" />
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                                                Enabled="true" Format="dd-MM-yyyy" TargetControlID="txtFromDate" />
                            <%--<asp:TextBox ID="txtFromDate" runat="server" Width="90px"></asp:TextBox>
                            <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" Enabled="True"
                                Format="dd-MM-yyyy" TargetControlID="txtFromDate">
                            </asp:CalendarExtender>--%>
                        </td>
                        <td colspan="1">
                            To Date:
                        </td>
                        <td colspan="2">
                         <asp:TextBox ID="txtToDate" runat="server" Width="90px"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server"
                                                ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="true" Mask="99-99-9999" TargetControlID="txtToDate"
                                                UserDateFormat="DayMonthYear" />
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server"
                                                Enabled="true" Format="dd-MM-yyyy" TargetControlID="txtToDate" />
                           <%-- <asp:TextBox ID="txtToDate" runat="server" Width="90px"></asp:TextBox>                            
                            <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Enabled="True"
                                Format="dd-MM-yyyy" TargetControlID="txtToDate">
                            </asp:CalendarExtender>--%>
                        </td>
                        <td colspan="1">
                            Type:
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlTypes" runat="server" Font-Bold="True" Font-Names="Arial Narrow" AutoPostBack="True" CausesValidation="false"
                                Width="70%" Font-Size="Small" OnSelectedIndexChanged="ddlType_IndexChanged">                                
                            </asp:DropDownList>
                        </td>
                        
                        <%--<td align="left">
                            <asp:Button ID="btnListJobCards" runat="server" width="117px" Text="List Job Cards" OnClick="btnListJobCards_Click" />
                        </td>--%>
                    </tr>
                    <tr>
                        <td colspan="1">
                            PST No:</td>
                        
                        <td colspan="3">
                            <asp:TextBox ID="txtPSTCode" runat="server" AutoPostBack="True" ReadOnly="true" CssClass="ROTextBoxes" SkinID="ROTextBox"
                                 Width="100%" MaxLength="8"></asp:TextBox>
                        </td>
                           <td colspan="1">
                            GST No:</td>
                        
                        <td colspan="3">
                            <asp:TextBox ID="txtGSTCode" runat="server" AutoPostBack="True" ReadOnly="true" CssClass="ROTextBoxes" SkinID="ROTextBox"
                                 Width="100%" MaxLength="8"></asp:TextBox>
                        </td>
                       
                    </tr>
                    <tr>
                       
                        
                        <td colspan="9">
                            <asp:GridView ID="Grdbillings" runat="server" AutoGenerateColumns="False" Width="100%" OnSelectedIndexChanged="Grdbillings_SelectedIndexChanged" OnRowDataBound="Grdbillings_RowDataBound" ShowFooter="true">
                                <Columns>
                                    <asp:TemplateField>
                                     <HeaderTemplate>
                                        <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged"
                                            Text="Select All" />
                                    </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkBox" runat="server" 
                                                oncheckedchanged="ChkBox_CheckedChanged1" AutoPostBack="True" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                     <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                     </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="JobCard Code" DataField="JobCardCode" />
                                    <asp:BoundField HeaderText="Date" DataField="RecDate" DataFormatString="{0:dd/MM/yyyy}"
                                     HtmlEncode="False" />
                                    <asp:BoundField HeaderText="JobType" DataField="JobTypeDesc" />
                                    <asp:BoundField HeaderText="Chassis No" DataField="ChassisNo" />
                                    <asp:TemplateField HeaderText="Parts">
                                    <ItemTemplate>
                                    <asp:Label ID="lblPart" runat="server" Width="60px" 
                                     Style="text-align:right" Text ='<%# Bind("PartsTotal") %>'></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                     <asp:Label ID="lblFootParts" Style="text-align: right" runat="server"></asp:Label>
                                     </FooterTemplate>
                                    </asp:TemplateField>
                                     <%--<asp:BoundField HeaderText="Part" DataField="PartsTotal" />--%>
                                      <asp:BoundField HeaderText="GST %" DataField="GST" />
                                      <asp:TemplateField HeaderText="GST Amount">
                                      <ItemTemplate>
                                        <asp:Label ID="lblGSTAmt" runat="server" Width="60px" 
                                                            Style="text-align:right" Text ='<%# Bind("GSTAmount") %>'></asp:Label>
                                      </ItemTemplate>
                                       <FooterTemplate>
                                                            <asp:Label ID="lblFootGSTAmt" Style="text-align: right" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                      </asp:TemplateField>
                                       <asp:BoundField HeaderText="FurTax %" DataField="Fur" />
                                       <asp:TemplateField HeaderText="Fur Amount">
                                      <ItemTemplate>
                                        <asp:Label ID="lblFurAmt" runat="server" Width="60px" 
                                                            Style="text-align:right" Text ='<%# Bind("FurAmount") %>'></asp:Label>
                                      </ItemTemplate>
                                       <FooterTemplate>
                                                            <asp:Label ID="lblFootFurAmt" Style="text-align:right" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                      </asp:TemplateField>
                                      <%--<asp:BoundField HeaderText="GST Amount" DataField="GSTAmount" />--%>
                                      <asp:TemplateField HeaderText="Labor">
                                      <ItemTemplate>
                                        <asp:Label ID="lblLabor" runat="server" Width="60px" 
                                                            Style="text-align: right" Text ='<%# Bind("LaborTotal") %>'></asp:Label>
                                      </ItemTemplate>
                                       <FooterTemplate>
                                                            <asp:Label ID="lblFootLabor" Style="text-align:right" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                      </asp:TemplateField>
                                       <%--<asp:BoundField HeaderText="Labor" DataField="LaborTotal" />--%>
                                       <asp:BoundField HeaderText="Service Tax %" DataField="PST" />
                                        <asp:TemplateField HeaderText="Service Tax Amount">
                                      <ItemTemplate>
                                        <asp:Label ID="lblPSTAmt" runat="server" Width="60px" 
                                                            Style="text-align:right" Text ='<%# Bind("PSTAmount") %>'></asp:Label>
                                      </ItemTemplate>
                                       <FooterTemplate>
                                                            <asp:Label ID="lblFootPSTAmt" Style="text-align:right" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                      </asp:TemplateField>
                                      <%--<asp:BoundField HeaderText="Service Tax Amount" DataField="PSTAmount" />--%>
                                      <asp:TemplateField HeaderText="Total">
                                      <ItemTemplate>
                                        <asp:Label ID="lblGrossAmt" runat="server" Width="60px" 
                                                            Style="text-align:right" Text ='<%# Bind("PaidC") %>'></asp:Label>
                                      </ItemTemplate>
                                       <FooterTemplate>
                                                            <asp:Label ID="lblFootGrossAmt" Style=" text-align:right" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                      </asp:TemplateField>
                                      
                              
                                                       
                                                        
                                                       
                                                    
                                </Columns>
                                 
                                <HeaderStyle CssClass="GridHeader" />
                                <FooterStyle CssClass="GridFooter" />
                            </asp:GridView>
                        </td>                       
                    </tr>
                  
                </table>
            </fieldset>
            <fieldset>
                <legend>Summary</legend>
                <table width=100% style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                    <tr>
                       <td colspan="8" align="right"> 
                           Sub Total Amount :
                        </td>                        
                        <td style="width: 8%">
                            <asp:TextBox ID="txtSubTotal" runat="server" Width="80px" ReadOnly="true" SkinID="ROTextBox" CssClass="ROTextBoxes" style="text-align :right">0</asp:TextBox>
                            
                        </td>
                    </tr>
                    <tr>
                  
                       <td colspan="8" align="right"> 
                           GST Amount :
                        </td>                        
                        <td style="width: 8%">
                            <asp:TextBox ID="txtGSTAmt" runat="server" Width="80px" ReadOnly="true" SkinID="ROTextBox" style="text-align :right">0</asp:TextBox>
                            
                        </td>
                        </tr>
                    <tr>
                       <td colspan="8" align="right"> 
                            PST:
                        </td>
                         <td style="width: 8%">
                            <asp:TextBox ID="txtLabPST" runat="server" Width="80px" ReadOnly="true" CssClass="ROTextBoxes" SkinID="ROTextBox" style="text-align :right" >0</asp:TextBox>                            
                           
                        </td>
                       
                    </tr>
                     <tr>
                                                  
                        <td colspan="8" align="right">                            
                                        Total Amount:
                        </td>
                        <td align="left">
                              <asp:TextBox ID="txtTotalAmount" runat="server" Width="80px" ReadOnly="true" SkinID="ROTextBox" style="text-align :right">0</asp:TextBox>
                        </td>
                                         
                    </tr>
                   
                </table>
            </fieldset>
           <table width="100%" style="margin-bottom: 10px;margin-top:10px;">
                <tr>
                 <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*) Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
                 <td>
                            <asp:Label ID="lblMessage" runat="server" ForeColor="#339933" Font-Bold="True"></asp:Label>
                        </td>
                    <td>
                    <td>
                        <asp:Label ID="lblGstCode" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#339933"></asp:Label>
                        <asp:Label ID="lblPstCode" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#339933"></asp:Label>
                        <asp:Label ID="lblLastBillNo" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#339933"></asp:Label>
                    </td>
                   <td style="text-align: right;">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait';" UseSubmitBehavior="false"/>
                   
                        <asp:Button ID="btnDelete" runat="server" Width="100px" Text="Delete" OnClientClick=" return confirm('Are you sure to delete?');"
                            OnClick="btnDelete_Click" />
                    
                        <asp:Button ID="btnClear" runat="server" Text="Clear" onclick="btnClear_Click" />
                    
                       <asp:Button ID="btnBillReport" runat="server" Text="Print" width="124px"
                                onclick="btnBillReport_Click" />
                        <asp:Button ID="btnPost" runat="server" Visible="true" Text="Post" onclick="btnPost_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait';" UseSubmitBehavior="false"/>
                        <asp:Button runat="server" ID="btnGL" OnClick="btnGL_Click" Text="GL" />
                    </td>
                </tr>
                
            </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
