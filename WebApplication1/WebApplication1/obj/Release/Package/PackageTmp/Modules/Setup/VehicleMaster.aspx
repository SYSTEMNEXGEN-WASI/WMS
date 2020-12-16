<%@ Page Title="" Language="C#" Theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="VehicleMaster.aspx.cs" Inherits="DXBMS.Modules.Setup.VehicleMaster" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
       <%-- <Triggers>
           <asp:PostBackTrigger ControlID="btnSave1" />
        </Triggers>--%>
        <ContentTemplate>
            <div class="container">
            

        <%--<div class="Heading">
        Vehicle Master
        </div>--%>
        
            <fieldset>
                <legend>Vehicle Information</legend>

                <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                    <tr>
                        <td width="13%">Vehicle Id</td>
                        <td>
                            <asp:DropDownList ID="ddlvehicle" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlvehicle_id_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td colspan="2" style="text-align:center;">
                            <asp:CheckBoxList ID="CheckBoxList1" runat="server" RepeatDirection="Horizontal" Width="100%" 
                                AutoPostBack="true" OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged" >
                                <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                                <asp:ListItem Text="Dummy Vehicle" Value="DummyVehicle"></asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td>Made</td>
                        <td width="39%">
                            <asp:DropDownList ID="ddlmade" runat="server" Width="90%"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RFVMade" runat="server" InitialValue="0" ControlToValidate="ddlmade" Text="*" ErrorMessage="Brand Code is Required" ForeColor="red"></asp:RequiredFieldValidator>
                        </td>
                        <td width="13%">Vehicle Code</td>
                        <td>
                            <asp:TextBox ID="txtvehicle_code" runat="server" Width="90%" MaxLength="10" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RFVVehicleCode" runat="server" ErrorMessage="Vehicle Code is required" ForeColor="red" ControlToValidate="txtvehicle_code" Text="*"></asp:RequiredFieldValidator>
                        </td width="35%">
                    </tr>
                    <tr>
                        <td >Model Code</td>
                        <td >
                            <asp:TextBox ID="txtmodel_code" runat="server" MaxLength="3" Width="90%" AutoPostBack="true" OnTextChanged="txtvehicle_code_TextChanged"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RFVModel_Code" runat="server" ControlToValidate="txtmodel_code" ErrorMessage="Model No. is Required" ForeColor="red" Text="*"></asp:RequiredFieldValidator>
                        </td>
                        <td >Vehicle Description</td>
                        <td>
                            <asp:TextBox ID="txtvehicle_desc" AutoPostBack="true" runat="server" MaxLength="50" Width="90%" OnTextChanged="txtvehicle_desc_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    
                </table>
            </fieldset>

            <fieldset>
                <legend>Vehicle Details</legend>
                <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                    <tr>
                        <td width="13%">Short Description</td>
                        <td >
                            <asp:TextBox ID="txtshort_desc" runat="server" Width="100%" MaxLength="30"></asp:TextBox>
                            
                        </td>
                        <td >Vehicle Type</td>
                        <td >
                            <asp:DropDownList ID="ddlvehicle_type" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlvehicle_type_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td>Vehicle Category</td>
                        <td>
                            <asp:TextBox ID="txtVehCategory" runat="server" ReadOnly="true"></asp:TextBox>
                           <%-- <asp:DropDownList runat="server" ID="ddlVehCategory" width="145px">
									<asp:ListItem Selected="true" Text="Select" Value="Select"></asp:ListItem>
									<asp:ListItem Text="HCV" Value="HCV"></asp:ListItem>
									<asp:ListItem Text="LCV" Value="LCV"></asp:ListItem>
									
								</asp:DropDownList>--%>
                        </td>
                        
                    </tr>
                    <tr>
                        <td>No. of Compartment</td>
                        <td>
                            <asp:TextBox ID="txtNo_compartments" runat="server" MaxLength="2" Width="100%"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtNo_compartments" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                        </td>
                        <td>Weight</td>
                        <td>
                            <asp:TextBox ID="txtWeight" runat="server" MaxLength="5" Width="100%"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtWeight" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                        </td>
                        <td>Engine Capacity (CC)</td>
                        <td>
                            <asp:TextBox ID="txtengine_cap" runat="server" MaxLength="5" Width="100%"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtengine_cap" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>Vehicle Prefix</td>
                        <td>
                            <asp:TextBox ID="txtvehicle_prefix" runat="server" MaxLength="15" Width="100%"></asp:TextBox>
                        </td>
                        <td>Fuel Type</td>
                        <td>
                            <asp:DropDownList ID="ddlfuel_type" runat="server" Width="100%"></asp:DropDownList>
                        </td>
                        <td>Schedule Plan (Days)</td>
                        <td>
                            <asp:TextBox ID="txtsch_plan" runat="server" MaxLength="3" Width="100%"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtsch_plan" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        
                        <td>Free Service Cupon</td>
                        <td>
                            <asp:TextBox ID="txtfree_service" runat="server" MaxLength="2" Width="100%"></asp:TextBox>
                        </td>
                        <td>Transmission Type</td>
                        <td>
                            <asp:DropDownList ID="ddltransmission_type" runat="server" Width="100%"></asp:DropDownList>
                         </td>
                        <td>Seater</td>
                        <td>
                            <asp:TextBox ID="txtseater" runat="server" MaxLength="2" Width="100%"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtseater" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                        </td>
                        
                    </tr>
                    <tr>
                        <td>
                            Body Type
                        </td>
                        <td>
                             <asp:TextBox ID="txtBodyType" runat="server" MaxLength="2" Width="100%"></asp:TextBox>
                        </td>
                         <td>
                            Built Unit
                        </td>
                        <td>
                             <%--<asp:TextBox ID="txtBuiltUnit" runat="server" MaxLength="2" Width="100%"></asp:TextBox>--%>
                              <asp:DropDownList runat="server" ID="ddlBuiltUnit" width="145px">
									<asp:ListItem Selected="true" Text="Select" Value="Select"></asp:ListItem>
									<asp:ListItem Text="CBU" Value="CBU"></asp:ListItem>
									<asp:ListItem Text="CKD" Value="CKD"></asp:ListItem>
									
								</asp:DropDownList>
                        </td>
                          <td>
                             Pay Load
                        </td>
                        <td>
                             <asp:TextBox ID="txtPayLoad" runat="server" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Vehicle Status</td>
                        <td>
                            <asp:DropDownList ID="ddlvehicle_status" runat="server" Width="100%"></asp:DropDownList>
                        </td>
                        <td>Remarks</td>
                        <td>
                            <asp:TextBox ID="txtremarks" runat="server" Width="100%" TextMode="MultiLine" MaxLength="100" Wrap="true" resize="none"></asp:TextBox>
                        </td>
                        <td>Photo</td>
                        <td>
                            <asp:FileUpload ID="PicUpload" runat="server" />
                             <asp:Image ID="vehicle_image" runat="server" Visible="false" Width="50" Height="50" ImageUrl ="~/VehicleMaster.aspx?image={0}"/>
                        </td>                        
                    </tr>
                </table>

            </fieldset>
            <table width="100%">
                    <tr>
                        <td width="100%">
                            <asp:GridView ID="gv_vehicle" runat="server" ShowFooter="true" ShowHeaderWhenEmpty="true" Width="100%"
                                AutoGenerateColumns="False" OnRowDataBound="gv_vehicle_RowDataBound" OnDataBound="gv_vehicle_DataBound" 
                                OnRowEditing="gv_vehicle_RowEditing" OnRowCancelingEdit="gv_vehicle_RowCancelingEdit"
                                 OnRowUpdating="gv_vehicle_RowUpdating"> 
                               
                                 <Columns>
                                    <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image" 
                           UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>
                                </Columns>

                                <HeaderStyle CssClass="GridHeader" />
                                <FooterStyle CssClass="GridFooter" />
                                
                                <Columns>
                                    <asp:TemplateField HeaderText="SNo">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        <asp:ImageButton ID="BtnAdd_Vehicle" runat="server" CausesValidation="false"
                                        ImageUrl="~/Images/Grid_Update.png" OnClick="BtnAdd_Vehicle_Click"/>
                                    </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VehicleCode" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVehicleCode" runat="server" Text='<%# Bind("VehicleCode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Color">
                                        <ItemTemplate>
                                            <asp:Label ID="lblColor" runat="server" Text='<%# Bind("Color") %>' ></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlcolorcode" runat="server" Width="120px" ></asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlcolorcode" runat="server" Width="120px" ></asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sale Price">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSalePrice" runat="server" Text='<%# Bind("SalePrice") %>' ></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtSalePrice" runat="server" Width="80px" placeholder="Sale Price" MaxLength="7"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtSalePrice" runat="server" Width="80px" placeholder="Sale Price" MaxLength="7"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cost Price">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCostPrice" runat="server" Text='<%# Bind("CostPrice") %>' ></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtCostPrice" runat="server" Width="80px" placeholder="Cost Price" MaxLength="7"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtCostPrice" runat="server" Width="80px" placeholder="Cost Price" MaxLength="7"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="WHTax">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWHTax" runat="server" Text='<%# Bind("WHTax") %>' ></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtWHTax" runat="server" Width="80px" placeholder="WithHolding Tax" MaxLength="5"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtWHTax" runat="server" Width="80px" placeholder="WithHolding Tax" MaxLength="5"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fur Tax">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFurTax" runat="server" Text='<%# Bind("FurTax") %>' ></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtFurTax" runat="server" Width="80px" placeholder="Further Tax" MaxLength="5"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFurTax" runat="server" Width="80px" placeholder="Further Tax" MaxLength="5"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Extra Tax">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExtraTax" runat="server" Text='<%# Bind("ExtraTax") %>' ></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtExtraTax" runat="server" Width="80px" placeholder="Extra Tax" MaxLength="5"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtExtraTax" runat="server" Width="80px" placeholder="Extra Tax" MaxLength="5"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reg Type/Tax">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRegType" runat="server" Text='<%# Bind("RegType") %>' ></asp:Label><br/>
                                            <asp:Label ID="lblRegTax" runat="server" Text='<%# Bind("RegTax") %>' ></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlRegType" runat="server" Width="80px" ></asp:DropDownList><br/>
                                            <asp:TextBox ID="txtRegTax" runat="server" Width="80px" placeholder="Register Tax" MaxLength="5"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlRegType" runat="server" Width="80px" ></asp:DropDownList><br/>
                                            <asp:TextBox ID="txtRegTax" runat="server" Width="80px" placeholder="Register Tax" MaxLength="5"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Active">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIsActive" runat="server" Text='<%# Bind("IsActive") %>' ></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="cbIsActive" runat="server" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="cbIsActive" runat="server" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remove" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="BtnRemove_Vehicle" runat="server" CausesValidation="false"
                                            ImageUrl="~/Images/Delete_16x16.png" onclick="BtnRemove_Vehicle_Click"
                                            ToolTip="Remove" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <table width="100%" >
                <tr>
                    <td>
                        <asp:Label ID="lblMsg" runat="server" Text="Item Added Succesfully" ForeColor="Green" Font-Bold="true" ></asp:Label>
                    </td>
                    <td style="text-align:center;">
                        <asp:Button ID="btnSave1" runat="server"  Text="Save" Width="100%" OnClick="btnSave1_Click" Visible="false" />
                        <%--<asp:Button ID="btnDelete0" runat="server" Text="Delete" />--%>
                        <%--<asp:ConfirmButtonExtender ID="CBE" runat="server" ConfirmText="You want to delete?"
                            TargetControlID="btnDelete0">--%>
                        <%--</asp:ConfirmButtonExtender>--%>
                        <asp:Button ID="btnClear0" runat="server" Text="Clear" Width="100%" OnClick="btnClear0_Click" CausesValidation="false" />
                    </td>
                </tr>
            </table>
            </div>

    </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
