<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" Theme="Theme1" CodeBehind="GatePassTemp.aspx.cs" Inherits="DXBMS.Modules.Service.Forms.GatePassTemp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
               window.location.href = "GatePassTemp.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "GatePassTemp.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "GatePassTemp.aspx";
           }
           function reloads() {
               window.location.href = "GatePassTemp.aspx";
           }

       </script>
<asp:UpdatePanel runat="server">
<ContentTemplate>
<div class="container">
<div>
<fieldset>
<legend>Gate Pass</legend>
<div class="container">
   


                                    <table width="70px" style="border-collapse: separate; border-spacing: 5px;"
                                        class="table table-condensed table-bordered table-hover">
                                        <tr>
                                            <td >GatePass ID <asp:ImageButton runat="server" ID="ImgGatePass" ImageUrl="~/Images/LookupNew.png" OnClick="ImgGatePass_Click"  /><font color="red"><b>*</b></font> 
                                            </td>
                                            <td >
                                                <asp:TextBox ID="txtGatePassCode" runat="server" Width="95%"></asp:TextBox>

                                            </td>
                                            <td >Gate Pass Date</td>
                                            <td >
                                                <asp:TextBox ID="txtGatePassDate"  ReadOnly="true" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png);
                                        background-repeat: no-repeat; padding-left: 25px;" TabIndex="2" Width="100%"></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server"
                             ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                             CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                             CultureTimePlaceholder="" Enabled="true" Mask="99-99-9999" TargetControlID="txtGatePassDate"
                             UserDateFormat="DayMonthYear" />
                                <td class="auto-style1">Gate Pass Type</td>
                                            <td class="auto-style6">
                                               <asp:DropDownList ID="ddlGType" runat="server" Font-Names="monospace" Width="95%">
                                                     <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                                    <asp:ListItem Value="Y">Returnable</asp:ListItem>
                                                    <asp:ListItem Value="N">Non Returnable</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                      
                                       
                                       <tr>
                                       <td >
                           IN Date/Time
                           </td>
                           <td >
                           <asp:TextBox ID="txtInDate" Width="45%" runat="server"  ReadOnly="true" SkinID="ROTextBox" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 48%;"></asp:TextBox>
                                           <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender" runat="server"
                             ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                             CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                             CultureTimePlaceholder="" Enabled="true" Mask="99-99-9999" TargetControlID="txtInDate"
                             UserDateFormat="DayMonthYear" />
                                            <%--<ajaxToolkit:CalendarExtender ID="txdiveciepdivate_CalendarExtender" runat="server"
                                                Enabled="true" Format="dd-MM-yyyy" TargetControlID="txtRecieptDate" />--%>
                                                <asp:TextBox ID="txtInTime" runat="server" Width="40%" MaxLength="5"  TabIndex="4" Enabled="true"
                                                placeholder="00:23" ></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txdiveciptTime_MaskedEditExtender" runat="server"
                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="true" Mask="99:99" MaskType="Time" TargetControlID="txtInTime"
                                                ClearMaskOnLostFocus="False" />
                           </td>
                           <td >
                           OUT Date/Time<font color="red"><b>*</b></font>
                           </td>
                           <td >
                           <asp:TextBox ID="txtOutDate" TabIndex="5" runat="server" ReadOnly="true" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 48%;"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txtPromisedDate_MaskedEditExtender" runat="server"
                                                ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="true" Mask="99-99-9999" TargetControlID="txtOutDate"
                                                UserDateFormat="DayMonthYear" />
                                            <ajaxToolkit:CalendarExtender ID="txtPromisedDate_CalendarExtender" runat="server"
                                                Enabled="true" Format="dd-MM-yyyy" TargetControlID="txtOutDate" />
                                                <asp:TextBox ID="txtOutTime" runat="server"  Width="48%" MaxLength="5" TabIndex="6"
                           placeholder="00:23"></asp:TextBox>
                           <ajaxToolkit:MaskedEditExtender ID="txtPromistedTime_MaskedEditExtender" runat="server"
                           CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                           CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                           CultureTimePlaceholder="" Enabled="true" Mask="99:99" MaskType="Time" TargetControlID="txtOutTime" />
                           </td>
                           <td>
                           Trans Type
                           </td>
                                <td>
                                <asp:DropDownList ID="ddlModule" runat="server" AutoPostBack="True" Font-Names="monospace" Width="95%">
                                                     <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                                    <asp:ListItem Value="Service">Invoice</asp:ListItem>
                                                    <asp:ListItem Value="Service/JC">JobCard</asp:ListItem>
                                                    <asp:ListItem Value="Spare Part">Counter Sale Part</asp:ListItem>
                                                     <asp:ListItem Value="Spare Part Stock Adj">Stock Adjustment Part</asp:ListItem>
                                                </asp:DropDownList>
                                </td>
                                       </tr>
                                       <tr>
                                       <td>
                                       Trans No
                                       </td>
                                       <td>
                                       <asp:TextBox runat="server" ID="txtTransCode" ReadOnly="true" SkinID="ROTextBox" Width="100%"></asp:TextBox>
                                       </td>
                                       <td class="auto-style1">Remarks </td>
                                            <td class="auto-style6">
                                               <asp:TextBox runat="server" ID="txtRemarks" TextMode="MultiLine" Width="100%">

                                               </asp:TextBox>
                                            </td>
                                       </tr>
                                 
                                    </table>

                               
</div>
</fieldset>
<fieldset>
<legend>Vehicle/Item</legend>
<div class="row">
<div class="col-lg-12">
    <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" CssClass="TabStyle" 
                                    Width="100%" Style="direction: ltr; text-align: left; display: grid;" ActiveTabIndex="0">
                                      <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Vehicle  ">
                                    <HeaderTemplate>Vehicle</HeaderTemplate>
                                        <ContentTemplate >
                                        <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                                            <tr>
                                                <td>Reg No <asp:ImageButton runat="server" ID="imgVehRegNo" ImageUrl="~/Images/LookupNew.png" OnClick="imgVehRegNo_Click"  /><font color="red"><b>*</b></font></td>
                                                <td>
                                                
                                                  <asp:TextBox runat="server" ID="txtRegNo" ReadOnly="true" Width="100%" ></asp:TextBox>
                                                </td>
                                                <td>Chassis No</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtChassisNo" ReadOnly="true" Width="100%" ></asp:TextBox>
                                                </td>
                                                <td>Engine </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtEngineNo" ReadOnly="true" Width="100%" ></asp:TextBox>
                                                </td>
                                            </tr>
                                              <tr>
                                                <td>Brand</td>
                                                <td>
                                                  <asp:HiddenField runat="server" ID="txtBrandCode"  ></asp:HiddenField>
                                                  <asp:TextBox runat="server" ID="txtBrandDesc" ReadOnly="true" Width="100%" ></asp:TextBox>
                                                </td>
                                                <td>Product</td>
                                                <td>
                                                    <asp:HiddenField runat="server" ID="txtProdCode"  ></asp:HiddenField>
                                                  <asp:TextBox runat="server" ID="txtProdDesc" ReadOnly="true" Width="100%" ></asp:TextBox>
                                                </td>
                                                <td>Version </td>
                                                <td>
                                                    <asp:HiddenField runat="server" ID="txtVersionCode"  ></asp:HiddenField>
                                                  <asp:TextBox runat="server" ID="txtVersionDesc" ReadOnly="true" Width="100%" ></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                            <td>
                                            Customer
                                            </td>
                                            <td>
                                               <asp:HiddenField runat="server" ID="txtCusCode"  ></asp:HiddenField>
                                                  <asp:TextBox runat="server" ID="txtCusDesc" ReadOnly="true" Width="100%" ></asp:TextBox>
                                            </td>
                                            <td>
                                            Contact No
                                            </td>
                                            <td>
                                              <asp:TextBox runat="server" ID="txtUserPhNo" ReadOnly="true" Width="100%" ></asp:TextBox>
                                            </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:ImageButton ID="BtnAdd" runat="server" Height="45px" ImageUrl="~/Images/AddIMG.png" OnClick="BtnAdd_Click" Width="45px" ></asp:ImageButton>
                                                    <%----%>
                                                    <asp:ImageButton ID="BtnClear1" runat="server" Height="40px" ImageUrl="~/Images/ClearImg.png" OnClick="BtnClear1_Click" Width="40px" ></asp:ImageButton>
                                                    <%----%>
                                                </td>
                                                <td align="right">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="gvVehicle" runat="server" AutoGenerateColumns="False" ShowFooter="True" ShowHeaderWhenEmpty="True"
                                                        Width="100%" >
                                                        <%----%>
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                            <asp:BoundField DataField="RegNo" HeaderText="Reg No">
                                                             
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ChassisNo" HeaderText="Chassis No">
                                                              
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="EngineNo" HeaderText="Engine No">
                                                               
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="BrandCode" HeaderText="BrandCode" Visible="False" />
                                                            <asp:BoundField DataField="BrandDesc" HeaderText="Brand">
                                                                
                                                            </asp:BoundField>
                                                              <asp:BoundField DataField="ProdCode" HeaderText="ProdCode" Visible="False" />
                                                            <asp:BoundField DataField="ProdDesc" HeaderText="Product">
                                                               
                                                            </asp:BoundField>
                                                              <asp:BoundField DataField="VersionCode" HeaderText="BrandCode" Visible="False" />
                                                            <asp:BoundField DataField="VersionDesc" HeaderText="Version">
                                                             
                                                            </asp:BoundField>
                                                          
                                                            <asp:TemplateField HeaderText="Remove">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="BtnRemove" runat="server" ImageUrl="~/Images/Delete_16x16.png" OnClick="BtnRemove_Click" ToolTip="Remove"  />
                                                                    <%----%>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="60px" />
                                                                <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <FooterStyle CssClass="GridFooter" />
                                                        <HeaderStyle CssClass="GridHeader" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>                                    
                                        </ContentTemplate>
                                </ajaxToolkit:TabPanel>
                                 
                                 <ajaxToolkit:TabPanel runat="server" CssClass="TabStyle"
                                  Width="100%" Style="direction: ltr; text-align: left; display: grid;" >
                                    <HeaderTemplate>Item</HeaderTemplate>
                                    <ContentTemplate>
                                      <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">

                                          <tr>
                                                <td>Item <asp:ImageButton runat="server" ID="ImageButton1" ImageUrl="~/Images/LookupNew.png"  /><font color="red"><b>*</b></font></td>
                                                <td>
                                                
                                                  <asp:TextBox runat="server" ID="txtItemCode" ReadOnly="true" Width="100%" ></asp:TextBox>
                                                </td>
                                                <td>Part No</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPartNo" ReadOnly="true" Width="100%" ></asp:TextBox>
                                                </td>
                                                <td>Item Description </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtItemDesc" ReadOnly="true" Width="100%" ></asp:TextBox>
                                                </td>
                                            </tr>


                                      </table>
                                    </ContentTemplate>


                                  </ajaxToolkit:TabPanel>
                                 
                                    </ajaxToolkit:TabContainer>
                                    
                                   

</div>

</div>
</fieldset>
<div class="row">
                                   <table width="100%" style="margin-bottom: 10px;margin-top:10px;">
                                     <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Button ID="btnSave" runat="server"  Width="100px" Text="Save" OnClick="btnSave_Click" />
                                                 <asp:Button ID="btnPrint" runat="server"  Width="100px" Text="Print" OnClick="btnPrint_Click" />
                                                  <asp:Button ID="btnPost" runat="server"  Width="100px" Text="Post" OnClick="btnPost_Click" />
                                                    <asp:Button ID="btnDelete" runat="server"  Width="100px" Text="Post" OnClick="btnDelete_Click" />

                                                <asp:Button ID="btnClear" runat="server"  Width="100px" Text="Clear" OnClick="btnClear_Click"  />

                                            </td>
                                        </tr>
                                    </table>
                                    </div>
</div>

</div>
</ContentTemplate>
</asp:UpdatePanel>

</asp:Content>
