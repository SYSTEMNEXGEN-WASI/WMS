<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Vendor.aspx.cs" Inherits="DXBMS.Modules.BusinessPartner.Vendor" Theme="Theme1" MasterPageFile="~/Root.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="Server">

    <script src="../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="../../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../js/chosen.jquery.js" type="text/javascript"></script>
    <script src="../../js/chosen.jquery.min.js" type="text/javascript"></script>
     <script src="../../Data/sweetalert.js" type="text/javascript"></script>  
     <script type="text/javascript">
           function Savealert(code) {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been saved'+code,
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "Vendor.aspx";
           }
           function Updatealert(code) {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update'+code,
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "Vendor.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "Vendor.aspx";
           }
           function reloads() {
               window.location.href = "Vendor.aspx";
           }

       </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
              <span class="fa-arrow-left fa"></span>
                        <asp:LinkButton ID="lnkBack" ForeColor="Black" runat="server" PostBackUrl="~/" Text="Back"></asp:LinkButton>
                         
                <div class="Heading">Vendor Master</div>
                <div>
                    <fieldset>
                        <legend>General Information</legend>
                        <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                            <tr>
                                <td >
                                  Vendor Code<asp:ImageButton runat="server" ID="imgVendorCode" CausesValidation="false" ImageUrl="~/Images/LookupNew.png" OnClick="imgVendorCode_Click" />
                                </td>
                                <td>
                                 <asp:TextBox runat="server" ID="txtVendorCode" ReadOnly="true" Width="100%"></asp:TextBox>
                                    <asp:DropDownList ID="ddlVendor" runat="server" Visible="false" Font-Names="monospace"  OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                    <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlVendor" QueryPattern="Contains"></asp:ListSearchExtender>
                                </td>

                      
                          
                                <td >Vendor Name<span style="color: red; font-weight: bold; padding-left: 5px;">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtvendor_name" runat="server" MaxLength="50" Width="100%"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtvendor_name" runat="server" ErrorMessage="Vendor Name Mandatory" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>
                                </td>
                                 <td>Credit Limit</td>
                                <td>
                                    <asp:TextBox ID="txtCreditLimit" runat="server" MaxLength="6"></asp:TextBox>
                                </td>
                                      </tr>
                                  <tr>
                               
                           
                                <td >Contact Person <span style="color: red; font-weight: bold; padding-left: 5px;">*</span></td>
                                <td >
                                    <asp:TextBox ID="txtcontact_person" runat="server" TextMode="SingleLine" MaxLength="50" Width="100%"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtcontact_person" runat="server" ErrorMessage="Contact Person Mandatory" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>
                                </td>
                               
                                <td>Vendor /  Supplier</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTitle" Width="150px">
                                <asp:ListItem Value="Vendor">Vendor</asp:ListItem>
                                <asp:ListItem Value="Supplier">Supplier</asp:ListItem>
                            </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                         <ajaxToolkit:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" Width="100%"
                        Style="direction: ltr; text-align: left;">
                          <ajaxToolkit:TabPanel class="TabPanel" ID="TabPanel2" runat="server">
                            <HeaderTemplate >
                               <asp:Label runat="server" Text="Contact Information" style="font-size:12px;font-family:'Times New Roman';"></asp:Label>
                            </HeaderTemplate>
                            <ContentTemplate>
                             <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                            <tr>
                                <td width="12%">Email
                                </td>
                                <td>
                                    <asp:TextBox ID="txtemail" runat="server" MaxLength="50" Width="100%"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Incorrect Email" ForeColor="Red" ControlToValidate="txtemail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                </td>
                                <td width="12%">Phone Number<span style="color: red; font-weight: bold; padding-left: 5px;">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtphone_1" runat="server" MaxLength="15" Width="48%"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtphone_1" FilterType="Numbers"></asp:FilteredTextBoxExtender>

                                    <asp:TextBox ID="txtphone_2" runat="server" MaxLength="15" Width="48%"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txtphone_2" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtphone_1" runat="server" ErrorMessage="Phone Number Mandatory" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Fax
                                </td>
                                <td>
                                    <asp:TextBox ID="txtfax_number" runat="server" MaxLength="15" Width="100%"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtfax_number" FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789-"></asp:FilteredTextBoxExtender>
                                </td>
                                <td>Address 1
                                </td>
                                <td>
                                    <asp:TextBox ID="txtaddress_1" runat="server" TextMode="SingleLine" MaxLength="50" Width="100%"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td>U.R.L
                                </td>
                                <td>
                                    <asp:TextBox ID="txturl" runat="server" MaxLength="50" Width="100%"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Incorrect URL" ControlToValidate="txturl" ForeColor="Red" ValidationExpression="(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"></asp:RegularExpressionValidator>
                                </td>
                                <td style="display:none;"></td>
                                <td  style="display:none;">
                                    <asp:TextBox ID="txtaddress_2" runat="server" TextMode="SingleLine" MaxLength="50" Width="180px" Visible="False"></asp:TextBox>
                                    <asp:TextBox ID="txtaddress_3" runat="server" TextMode="SingleLine" MaxLength="50" Width="180px" Visible="False"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                            </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                             <ajaxToolkit:TabPanel class="TabPanel" ID="TabPanel1" runat="server">
                            <HeaderTemplate >
                               <asp:Label runat="server" Text="Other Information" style="font-size:12px;font-family:'Times New Roman';"></asp:Label>
                            </HeaderTemplate>
                            <ContentTemplate>
                             <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                            <tr>
                                <td width="12%">G.S.T Reg. No.
                                </td>
                                <td width="37%">
                                    <asp:TextBox ID="txtgst_number" runat="server" MaxLength="13" Width="100%" ></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterType="Numbers" TargetControlID="txtgst_number"></asp:FilteredTextBoxExtender>
                                </td>
                                <td width="12%">N.T.N No.
                                </td>
                                <td>
                                    <asp:TextBox ID="txtntn_number" runat="server" MaxLength="8" Width="100%"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" FilterType="Numbers" TargetControlID="txtntn_number"></asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>Payment Terms
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpayment_terms" runat="server" Width="100%" palceholder="Days"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" FilterType="Numbers" TargetControlID="txtpayment_terms"></asp:FilteredTextBoxExtender>

                                </td>
                                <td>Account Code
                                    <asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png" onclick="imgLookup_Click"  CausesValidation="false"/>
                                </td>
                                <td>
                                    <%--<asp:DropDownList ID="ddlacount_code" runat="server" Style="width: 94%;"></asp:DropDownList>--%>
                                    <asp:TextBox ID="txtAccountCode" runat="server" Width="40%" ReadOnly="true"></asp:TextBox>
                                     <asp:TextBox ID="txtAccountDesc" runat="server" Width="55%" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                            </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                    </ajaxToolkit:TabContainer>
                    <table width="100%" style="margin-bottom: 10px;margin-top:10px;">
                        <tr>
                         <td>
                         <asp:Label ID="lblImp" runat="server" Text="(*) Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                         </td>
                            <td>
                                <asp:Label ID="lblmsg" runat="server"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btn_save" runat="server" Width="100px" Text="Save" OnClick="btn_save_Click" CausesValidation="True" OnClientClick="this.disabled = true; this.value = 'Please Wait';" UseSubmitBehavior="false" />
                               
                                <asp:Button ID="btn_delete" runat="server" Width="100px" Text="Delete" OnClientClick=" return confirm('Are you sure to delete?');"
                            OnClick="btn_delete_Click" />
                                <asp:Button ID="btn_clear" runat="server" Width="100px" Text="Clear" OnClick="btn_clear_Click" CausesValidation="false" />
                            </td>
                        </tr>

                    </table>
                    </fieldset>

                  

                   
                
                   
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
