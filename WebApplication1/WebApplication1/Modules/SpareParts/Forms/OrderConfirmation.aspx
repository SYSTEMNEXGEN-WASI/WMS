<%@ Page Title="Order Confirmation" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="OrderConfirmation.aspx.cs" Inherits="DXBMS.Modules.SpareParts.Forms.OrderConfirmation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">

        .auto-style1 {
            width: 268435280px;
        }
        .auto-style2 {
            width: 820px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="Server">

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
               window.location.href = "OrderConfirmation.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "OrderConfirmation.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "OrderConfirmation.aspx";
           }
           function reloads() {
               window.location.href = "OrderConfirmation.aspx";
           }

       </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">

        <ContentTemplate>
            <div class="container">
                  <div class="row">
                <div class="col-lg-12">
              
                <div class="col-md-4 pull-right" style="margin-top:10px;">
                <asp:Label runat="server" ID="lblMode" Font-Size="16px" Font-Bold="true" ForeColor="Black"> </asp:Label>
                </div>
                </div>
                </div>
                        <fieldset>
                            <legend>Delivery Challan </legend>

                            <div >


                                <table width="100%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">

                                    <tr>
                                        <td width="8%">D/C No:<asp:ImageButton runat="server" ID="imgDCLookup" ImageUrl="~/Images/LookupNew.png" onclick="imgDCLookup_Click"/> <span style="color:red; font-weight:bold;">*</span></td>
                                        <td width="43%">
                                            <asp:DropDownList ID="ddlDCno" runat="server" AutoPostBack="True" Font-Names="monospace" Width="100%" Enabled="false"
                                            OnSelectedIndexChanged="ddlDCno_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblDCno" runat="server" Text="PONo" Visible="False"></asp:Label>
                                        </td>
                                        <td width="8%">D/C Date:</td>
                                        <td width="15%">
                                            <asp:TextBox ID="txtDCDate" Width="100%" ReadOnly="true" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;" ></asp:TextBox>
                                           
                                        </td>
                                        <td width="11%">Status:</td>
                                        <td width="15%">
                                            <asp:TextBox ID="txtStatus" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td >P/O No: <asp:ImageButton runat="server" ID="imgPOLookup" ImageUrl="~/Images/LookupNew.png" onclick="imgPOLookup_Click"/><span style="color:red; font-weight:bold;">*</span></td>
                                        <td >
                                            <asp:DropDownList ID="ddlPONo" runat="server" AutoPostBack="True" Font-Names="monospace" Width="100%" OnSelectedIndexChanged="ddlPONo_SelectedIndexChanged" Enabled="false">
                                                <%----%>
                                            </asp:DropDownList>
                                             <asp:Label ID="lblPONo" runat="server" Text="PONo" Visible="False"></asp:Label>
                                        </td>
                                        <td >P/O Date:</td>
                                        <td >
                                            <asp:TextBox ID="txtPODate" runat="server" ReadOnly="true" width="100%" Style="background-image: url(../../../Images/Calendar_16x16.png);  background-repeat: no-repeat; padding-left: 25px;" ></asp:TextBox>
                                            
                                        </td>
                                        <td >Vendor D/C No: <span style="color:red; font-weight:bold;">*</span></td>
                                        <td >
                                            <asp:TextBox ID="txtVendorDCn0" runat="server" MaxLength="15"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Vendor</td>
                                        <td>
                                            <asp:TextBox ID="txtVendor" runat="server" Width="100%" Visible="False"></asp:TextBox>                                        
                                            <asp:TextBox ID="txtVendorDesc" ReadOnly="true" runat="server" Width="100%"></asp:TextBox>
                                        </td>
                                        <td >Remarks:</td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtRemarks" runat="server" Width="100%"></asp:TextBox>
                                        </td>
                                        
                                        
                                        
                                    </tr>

                                </table>

                            </div>

                            <!---- FOR GRIDVIEW -->

                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:GridView runat="server" ShowHeaderWhenEmpty="true" Width="100%" ID="gv_DC" AutoGenerateColumns="false"
                                            OnDataBound="gv_DC_DataBound"
                                            OnRowDataBound="gv_DC_RowDataBound"
                                             OnRowEditing="gv_DC_RowEditing"
                                            OnRowCancelingEdit="gv_DC_RowCancelingEdit"
                                            OnRowUpdating="gv_DC_RowUpdating">

                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />

                                            <Columns>

                                                <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                    UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:CommandField>

                                                <asp:templatefield HeaderText="Select">
                                                    <itemtemplate>
                                                        <asp:checkbox ID="cbSelect" runat="server" Checked="true" ></asp:checkbox> <%--OnCheckedChanged="cbCheck_CheckedChanged"--%>
                                                    </itemtemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:templatefield>

                                                <asp:TemplateField HeaderText="Part Item No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPartItemNo" runat="server" Text='<%# Bind("PartItemNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />

                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Part Item Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPartItemName" runat="server" Text='<%# Bind("PartItemName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />

                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Unit">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("unit") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="PO Quantity">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPOQuantity" runat="server" Text='<%# Bind("POQuantity") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Balance Quantity">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBalQuantity" runat="server" Text='<%# Bind("BalanceQuantity") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="DCQuantity">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDCQuantity" runat="server" Text='<%# Bind("DCQuantity") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtDCQuantity" runat="server" MaxLength="3"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Remove">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="BtnRemove" runat="server" OnClick="BtnRemove_Click" ImageUrl="~/Images/Delete_16x16.png" ToolTip="Remove" UseSubmitBehavior="false" />

                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>

                                        </asp:GridView>
                                    </td>
                                </tr>

                            </table>

                            <table width="100%" style="border-collapse: separate; border-spacing: 5px; margin-top: 10px;"
                                class="table table-condensed table-bordered table-hover">
                                <tr>
                                 <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*) Sterik Feilds Are Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
                                    <td >
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>
                                    </td>
                                      <td style="text-align: right;">
                                        <asp:Button ID="btnSave" runat="server" Width="100px" Text="Save"  OnClick="btnSave_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait';"
                                        UseSubmitBehavior="false"/>
                                       
                                           <asp:Button ID="btnDelete" runat="server" Width="100px" Text="Delete" OnClientClick=" return confirm('Are you sure to delete?');"
                            OnClick="btnDelete_Click" />
                                        <asp:Button ID="btnClear" runat="server" Width="100px" Text="Clear" OnClick="btnClear_Click" />                                       
                                        
                                    </td>

                                </tr>
                            </table>


                        </fieldset>
                    
            </div>

            </table>

        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>

