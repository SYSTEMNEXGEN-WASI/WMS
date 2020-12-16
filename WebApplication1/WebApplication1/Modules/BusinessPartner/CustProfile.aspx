<%@ Page Title="" Language="C#" Theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="CustProfile.aspx.cs" Inherits="DXBMS.Modules.SpareParts.Forms.CustProfile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<script src="../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
         <script src="../../Data/sweetalert.js" type="text/javascript"></script>
        <script src="../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../js/chosen.jquery.min.js" type="text/javascript"></script>
        
     
       <script type="text/javascript">
           function Savealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been saved',
                   showConfirmButton: false,
                   timer: 10000
               })
               window.location.href = "CustProfile.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 10000
               })
               window.location.href = "CustProfile.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 10000
                    })
            window.location.href = "CustProfile.aspx";
           }
           function reloads() {
               window.location.href = "CustProfile.aspx";
           }

       </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">

            <div style="font-size: 15px;">
                <span class="fa-arrow-left fa"></span>
                <asp:LinkButton ID="lblBack" ForeColor="Black" runat="server" CausesValidation="false" PostBackUrl="~/Modules/BusinessPartner/Customer.aspx" Text="Back"></asp:LinkButton>
            </div>
            
        <div class="Heading">
        Customer Master
        </div>

            <fieldset>
                <legend>Customer Information</legend>
                <table width="100%"  style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                    <tr>
                        <td width="13%">Customer Code<asp:ImageButton runat="server" ID="imgCustomerCode" CausesValidation="false" ImageUrl="~/Images/LookupNew.png" OnClick="imgCustomerCode_Click" /></td>
                        <td width="39%">
                        <asp:TextBox runat="server" ID="txtCusCode" ReadOnly="true" Width="100%" TabIndex="1" SkinID="ROTextBox"></asp:TextBox>
                            <asp:DropDownList ID="ddlcust_code" runat="server" Width="300px" AutoPostBack="true" Font-Names="monospace" Visible="false" OnSelectedIndexChanged="ddlcust_code_SelectedIndexChanged"></asp:DropDownList>
                            <asp:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddlcust_code" QueryPattern="Contains"></asp:ListSearchExtender>
                        </td>
                        <td width="10%">Customer Type<span  style="font-size:large; color:red;" >*</span></td>
                        <td>
                        <asp:DropDownList ID="ddlType" runat="server" Width="147px" AutoPostBack="True" TabIndex="2">
                        
                        </asp:DropDownList>
                          
                    </td>
                    </tr>
                    <tr>
                        <td>Title</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlTitle" Width="100%" TabIndex="3">
                                <asp:ListItem Value="Mr.">Mr.</asp:ListItem>
                                <asp:ListItem Value="Ms.">Ms.</asp:ListItem>
                                <asp:ListItem Value="Mis">Miss</asp:ListItem>
                                <asp:ListItem Value="D/O">D/O</asp:ListItem>
                                <asp:ListItem Value="Mrs">Mrs</asp:ListItem>                                
                                 <asp:ListItem Value="M/S">M/S</asp:ListItem>                                
                            </asp:DropDownList>
                        </td>
                        <td>Customer/MS<span  style="font-size:large; color:red;" >*</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustName" style="text-transform: uppercase" runat="server" Width="100%" MaxLength="50" TabIndex="4" AutoPostBack="true" CausesValidation="false" OnTextChanged="txtCustName_TextChanged"></asp:TextBox>
                          <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtCustName" runat="server" ErrorMessage="Customer Mandatory" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>--%>
                        </td>                        
                    </tr>
                    <tr>
                    <td>NIC</td>
                        <td>
                            <asp:TextBox ID="txtNIC" runat="server" MaxLength="13" Width="147px" AutoPostBack="true" TabIndex="5" OnTextChanged="txtNIC_TextChanged" placeholder="123412345671"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterType="Numbers" ValidChars="-"
                                TargetControlID="txtNIC" >
                            </asp:FilteredTextBoxExtender>
                            <asp:TextBox ID="txtBirthDT" runat="server" Visible="False" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"
                            Width="100%" placeholder="dd/mm/yyyy"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="txtBirthDT" ></asp:CalendarExtender>
                        </td>
                        <td width="13%">NTN No.</td>
                        <td width="39%">
                            <asp:TextBox ID="txtNTN" runat="server" MaxLength="8" AutoPostBack="true" TabIndex="6" OnTextChanged="txtNTN_TextChanged"></asp:TextBox>
                            
                        </td>
                        
                    </tr>
                </table>
                 <ajaxToolkit:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" Width="100%"
                        Style="direction: ltr; text-align: left;">
                           <ajaxToolkit:TabPanel class="TabPanel" ID="TabPanel2" runat="server">
                            <HeaderTemplate >
                               <asp:Label runat="server" Text="Contact Information" style="font-size:12px;font-family:Arial;"></asp:Label>
                            </HeaderTemplate>
                            <ContentTemplate>
                              <table width="100%"  style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                    <tr>
                        <td width="13%">Address</td>
                        <td width="39%">
                            <asp:TextBox ID="txtAddress1" style="text-transform: uppercase" runat="server" Width="100%" TabIndex="7" MaxLength="100" 
                                TextMode="SingleLine"  Wrap="true" ></asp:TextBox>
                        </td>
                        <td width="10%">Country</td>
                        <td>
                            <asp:DropDownList ID="ddlcountry" runat="server" Width="147px" OnSelectedIndexChanged="ddlcountry_SelectedIndexChanged" TabIndex="8" AutoPostBack="true"></asp:DropDownList>
                            <asp:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddlcountry" QueryPattern="Contains"></asp:ListSearchExtender>
                        </td>
                                              
                    </tr>
                    <tr>
                        <td>State</td>
                        <td>
                            <asp:DropDownList ID="ddlstates" runat="server" AutoPostBack="true" Width="147px" TabIndex="9" OnSelectedIndexChanged="ddlstates_SelectedIndexChanged"></asp:DropDownList>
                            <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlstates" QueryPattern="Contains"></asp:ListSearchExtender>
                        </td>                        
                        <td>City</td>
                        <td>
                            <asp:DropDownList ID="ddlcity" Width="147px" runat="server" TabIndex="10"></asp:DropDownList>
                            <asp:ListSearchExtender ID="ListSearchExtender3"  runat="server" PromptPosition="Top"  TargetControlID="ddlcity" QueryPattern="Contains" Enabled="True"></asp:ListSearchExtender>
                        </td>  
                    </tr>
                    <tr>
                    <td>Phone No.</td>
                        <td>
                            <asp:TextBox ID="txtPhone1" runat="server" MaxLength="12" TabIndex="11"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                TargetControlID="txtPhone1" >
                            </asp:FilteredTextBoxExtender>
                        
                        </td>
                        <td>Phone 2</td>
                        <td>
                                <asp:TextBox ID="txtPhone2" runat="server" MaxLength="12" TabIndex="12" ></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
                                TargetControlID="txtPhone2">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        
                    </tr>
                        <tr>
                    <td>Mobile No<span  style="font-size:large; color:red;" >*</span></td>
                    <td>
                    <asp:TextBox ID="txtCellNo" runat="server" MaxLength="12" TabIndex="13" Placeholder="92123456789" OnTextChanged="txtCellNo_TextChanged" AutoPostBack="true" CausesValidation="false"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="Numbers"
                                TargetControlID="txtCellNo"></asp:FilteredTextBoxExtender>
                    </td>
                        <td>FAX</td>
                        <td>
                            <asp:TextBox ID="txtFaxNo" runat="server" MaxLength="12" TabIndex="14"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                                TargetControlID="txtFaxNo">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        
                    </tr>
                    <tr>
                        <td>Email</td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" placeholder="customeremail@abc.com" TabIndex="15" ></asp:TextBox>
                            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                            ControlToValidate="txtEmail" ErrorMessage="Invalid email address" ForeColor="#FF3300"  ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>--%>
                        </td>
                        <td>U.R.L</td>
                        <td>
                            <asp:TextBox ID="txtURL" runat="server" MaxLength="50" placeholder="www.CustomerSite.com" TabIndex="16"></asp:TextBox>
                        </td>
                    </tr>
                
                </table>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel class="TabPanel" ID="TabPanel1" runat="server" >
                            <HeaderTemplate>
                             <asp:Label runat="server" Text=" Other Information" style="font-size:12px;font-family:Arial;"></asp:Label>
                                 
                            </HeaderTemplate>
                            <ContentTemplate>
                            
                <table width="100%"  style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                         <tr>
                        <td >Contact Person</td>
                        <td >
                        <asp:TextBox ID="txtContPerson" style="text-transform: uppercase" runat="server" MaxLength="50" Width="100%" TabIndex="17"></asp:TextBox>
                            <asp:TextBox ID="txtFName" style="text-transform: uppercase" runat="server" MaxLength="50" Width="300px" Visible="False"></asp:TextBox>
                        </td>
                   
                        <td width="10%">Sales Tax No.</td>
                        <td>
                            <asp:TextBox ID="txtSalesTax" runat="server" MaxLength="13" Width="100%" TabIndex="18"></asp:TextBox>                            
                        </td>
                        
                    </tr>
                    <tr>
                        <td >Payment Term</td>
                        <td>
                            <asp:TextBox ID="txtCreditTerm" runat="server" Width="100%" MaxLength="3" TabIndex="19"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers"
                                TargetControlID="txtCreditTerm">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>Payment Limit</td>
                        <td>
                            <asp:TextBox ID="txtCreditLimt" runat="server" MaxLength="7" TabIndex="20" Width="100%"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Numbers"
                                TargetControlID="txtCreditLimt">
                            </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>Customer Distance</td>
                        <td>
                            <asp:TextBox ID="txtDist" runat="server" MaxLength="7" Width="100%" placeholder="Kilometer" TabIndex="21"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtDist_FilteredTextBoxExtender" runat="server" 
                                FilterType="Numbers" TargetControlID="txtDist">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>Account Code
                             <asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png" TabIndex="22" onclick="imgLookup_Click"  CausesValidation="false"/>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccountCode" runat="server" ReadOnly="true" TabIndex="23" Width="45%"></asp:TextBox>
                          <asp:TextBox ID="txtAccountDesc" runat="server" ReadOnly="true" TabIndex="24" Width="52%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                    </ajaxToolkit:TabContainer>

                      
                      <table width="100%" style="margin-bottom: 10px;margin-top:10px;">
               
                <tr>
                <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*)  Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
                    <td>
                        <asp:Label ID="lblMsg" runat="server" Text="Record Added Succesfully" ForeColor="Green" Font-Bold="true" ></asp:Label>
                    </td>
                    
                    <td style="text-align:right;">
                        
                        <asp:Button ID="btnSave1" runat="server" OnClick="btnSave_Click" Text="Save" Width="100px" OnClientClick="this.disabled = true; this.value = 'Please Wait';" UseSubmitBehavior="false"  />
                        
                           <asp:Button ID="btnDelete0" runat="server" Width="100px" Text="Delete" OnClientClick=" return confirm('Are you sure to delete?');"
                            OnClick="btnDelete0_Click" />
                        <asp:Button ID="btnClear0" runat="server" CausesValidation="false" OnClick="btnClear_Click" Text="Clear" Width="100px" />
                    </td>
                </tr>
            </table>
            </fieldset>

           
                     
             
           
            
          
           






 
             </div> 
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
