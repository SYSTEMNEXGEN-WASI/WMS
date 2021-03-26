<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" Theme="Theme1" AutoEventWireup="true" CodeBehind="ACMemo.aspx.cs" Inherits="DXBMS.Modules.Service.Forms.ACMemo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                <div class="container">

                <h3 class="Heading">Warranty Claim Memo</h3>
                    
            
            <fieldset>
                <legend>Warranty Bill Claim / Covering Letter</legend>
                <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                    <tr>
                        <td width="10%" align="right">
                            Billing No :
                        </td>
                        <td style="width: 20%">
                           <%-- <asp:ImageButton ID="ImageButton1" runat="server" Height="17px" ImageUrl="~/Images/LookupNew.png"
                                Width="17px" onclick="ImageButton1_Click1" />
                         style="width: 10%">
                            <asp:TextBox ID="txtBillingNo" runat="server" Width="90px" AutoPostBack="True" 
                                ontextchanged="txtBillingNo_TextChanged" ReadOnly="True"></asp:TextBox>--%>
                                <asp:DropDownList ID="ddlBillingNo" runat="server" AutoPostBack="True" style="width: 100% !important;"
                                onselectedindexchanged="ddlBillingNo_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td align="right" style="width: 8%">
                            Bill Date:
                        </td>

                        <td style="width: 10%">
                            <asp:TextBox ID="txtBillDate" runat="server" Width="90px" ></asp:TextBox>
                            <%--<asp:MaskedEditExtender ID="txtBillDate_MaskedEditExtender" runat="server" ClearMaskOnLostFocus="False"
                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                CultureTimePlaceholder="" Enabled="True" Mask="99/99/9999" TargetControlID="txtBillDate">
                            </asp:MaskedEditExtender>--%>
                            <asp:CalendarExtender ID="txtBillDate_CalendarExtender" runat="server" Enabled="True"
                                Format="dd/MM/yyyy" TargetControlID="txtBillDate">
                            </asp:CalendarExtender>
                        </td>
                      <td align="right" style="width: 8%">
                            Credit Memo Number :
                        </td>
                         <td style="width: 10%">
                            <asp:TextBox ID="txtCreditNo" runat="server" Width="100%" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" width="10%">
                            Remarks :
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemarks" runat="server" Width="49%" MaxLength="100"></asp:TextBox>
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
                <legend>Warranty</legend>
                <table width=100% style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                    <tr>
                        <td align="right" style="width: 10%">
                            From Date :
                        </td>                        
                        <td style="width: 8%">
                            <asp:TextBox ID="txtFromDate" runat="server" Width="90px"></asp:TextBox>
                        <%-- <ajaxToolkit:MaskedEditExtender ID="txtFromDate_MaskedEditExtender" runat="server"
                                                ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="true" Mask="99-99-9999" TargetControlID="txtFromDate"
                                                UserDateFormat="DayMonthYear" />--%>

                            <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" Enabled="True"
                                Format="dd-MM-yyyy" TargetControlID="txtFromDate">
                            </asp:CalendarExtender>
                        </td>
                        <td align="right" style="width: 8%">
                            To Date:
                        </td>
                        <td style="width: 8%">
                            <asp:TextBox ID="txtToDate" runat="server" Width="90px" AutoPostBack="True"  ></asp:TextBox>                            
                          <%-- <ajaxToolkit:MaskedEditExtender ID="txtToDate_MaskedEditExtender" runat="server"
                                                ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="true" Mask="99-99-9999" TargetControlID="txtToDate"
                                                UserDateFormat="DayMonthYear" />--%>
                            <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Enabled="True"
                                Format="dd-MM-yyyy" TargetControlID="txtToDate">
                            </asp:CalendarExtender>
                        </td>
                         <td align="right" style="width: 10%">
                            Type:
                        </td>
                        
                        <td align="left" style="width: 15%">
                            <asp:DropDownList ID="ddlTypes" runat="server" Font-Bold="True" Font-Names="Arial Narrow" AutoPostBack="True"
                                Width="70%" Font-Size="Small" OnSelectedIndexChanged="ddlType_IndexChanged">                                
                            </asp:DropDownList>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="#339933" Font-Bold="True"></asp:Label>
                        </td>
                        <%--<td align="left">
                            <asp:Button ID="btnListJobCards" runat="server" width="117px" Text="List Job Cards" OnClick="btnListJobCards_Click" />
                        </td>--%>
                    </tr>
                      <tr>
                        <td align="right">
                            PST No:</td>
                        
                        <td colspan="5">
                            <asp:TextBox ID="txtPSTCode" runat="server" AutoPostBack="True" ReadOnly="true" CssClass="ROTextBoxes" 
                                 Width="90px" MaxLength="8"></asp:TextBox>
                        </td>
                           <td align="right">
                            GST No:</td>
                        
                        <td colspan="5">
                            <asp:TextBox ID="txtGSTCode" runat="server" AutoPostBack="True" ReadOnly="true" CssClass="ROTextBoxes" 
                                 Width="90px" MaxLength="8"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                       
                        
                        <td colspan="9">
                            <asp:GridView ID="Grdbillings" runat="server" AutoGenerateColumns="False" Width="100%">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkBox" runat="server" 
                                                oncheckedchanged="ChkBox_CheckedChanged1" AutoPostBack="True" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Calim No" DataField="PPRCode" />
                                    <asp:BoundField HeaderText="Date" DataField="PPRDate" DataFormatString="{0:dd/MM/yyyy}"
                                        HtmlEncode="False" />
                                     <asp:BoundField HeaderText="Model" DataField="ProdDesc" />
                                        <asp:BoundField HeaderText="ChassisNo" DataField="VehChassisNo" />
                                     <asp:BoundField HeaderText="Sub Part" DataField="SubPart" />
                                      <asp:BoundField HeaderText="Handling Amount" DataField="HandlingAmount" />
                                      <asp:BoundField HeaderText="GST Amount" DataField="GSTAmount" />
                                      <asp:BoundField HeaderText="Sub Sublet" DataField="SubSublet" />
                                      <asp:BoundField HeaderText="PST Sublet Amount" DataField="PSTSubletAmount" />
                                      <asp:BoundField HeaderText="Sub Labor" DataField="SubLabor" />
                                      <asp:BoundField HeaderText="PST Labor Amount" DataField="PSTLaborAmount" />
                                  
                                      <asp:BoundField HeaderText="Parts" DataField="TotalParts" />
                                      <asp:BoundField HeaderText="Sublet" DataField="TotalSublet" />
                                      <asp:BoundField HeaderText="Labor" DataField="TotalLabor" />
                                      <asp:BoundField HeaderText="Bill Amount" DataField="TotalAmount">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <FooterStyle CssClass="GridFooter" />
                            </asp:GridView>
                        </td>                       
                    </tr>
                   
                </table>
            </fieldset>

<fieldset>
                <legend>Taxes</legend>
                <table width=100% style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                    
                    <tr>
                       <td colspan="8" align="right"> 
                           GST Amount :
                        </td>                        
                        <td style="width: 8%">
                            <asp:TextBox ID="txtGSTAmt" runat="server" Width="80px" ReadOnly="true" CssClass="ROTextBoxes" style="text-align :right">0</asp:TextBox>
                            
                        </td>
                        </tr>
                    <tr>
                       <td colspan="8" align="right"> 
                            PST:
                        </td>
                         <td style="width: 8%">
                            <asp:TextBox ID="txtLabPST" runat="server" Width="80px" ReadOnly="true" CssClass="ROTextBoxes" style="text-align :right" >0</asp:TextBox>                            
                           
                        </td>
                       
                    </tr>
                     <tr>
                                                  
                        <td colspan="8" align="right">                            
                                        Total Amount:
                        </td>
                        <td align="left">
                               <asp:TextBox ID="txtTotalAmount" runat="server" Width="80px" ReadOnly="true" CssClass="ROTextBoxes" style="text-align :right">0</asp:TextBox>
                        </td>
                                         
                    </tr>
                   
                </table>
            </fieldset>



          <table width="100%" style="margin-bottom: 10px;margin-top:10px;">
                <tr>
                    <td>
                        <asp:Label ID="lblGstCode" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#339933"></asp:Label>
                        <asp:Label ID="lblPstCode" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#339933"></asp:Label>
                        <asp:Label ID="lblLastBillNo" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#339933"></asp:Label>
                    </td>
                     <td style="text-align: right;">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait';" UseSubmitBehavior="false"/>
                   
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" onclick="btnDelete_Click" />
                    
                        <asp:Button ID="btnClear" runat="server" Text="Clear" onclick="btnClear_Click" />
                      
                       <asp:Button ID="btnBillReport" runat="server" Text="Print Bill Report" width="124px"
                                onclick="btnBillReport_Click" />
                          <asp:Button ID="btnPost" runat="server" Text="Post" onclick="btnPost_Click" />
                        <%--<asp:Button ID="btnPost" runat="server" Visible="False" Text="Post" onclick="btnPost_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait';" UseSubmitBehavior="false"/>--%>
                    </td>
                </tr>
                
            </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
