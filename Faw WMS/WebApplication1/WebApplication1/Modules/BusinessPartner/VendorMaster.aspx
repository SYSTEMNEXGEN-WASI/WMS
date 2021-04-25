<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" 
CodeFile="VendorMaster.aspx.cs" Inherits="VendorMaster" Theme="Theme1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ></asp:ScriptManager>
    <table width="100%">
        <tr>
            <td>
                <asp:Label ID="lblmsg" runat="server" Text="Vendor Added Succesfully "></asp:Label>
            </td>
            <td style="text-align:right;">
                <asp:Button  ID="btn_save" runat="server" Text="Save" OnClick="btn_save_Click" CausesValidation="True" />
                <asp:Button  ID="btn_delete" runat="server" Text="Delete" OnClick="btn_delete_Click" CausesValidation="False" />
                <asp:Button  ID="btn_clear" runat="server" Text="Clear" OnClick="btn_clear_Click" CausesValidation="false" />                
            </td>
        </tr>

    </table>
    <div class="Heading">
        Vendor Master
    </div>
    <div >
        
            (*) Mandatory">
        
        <fieldset >
            <legend>General Information</legend>
            <table width="100%">
    <tr>
        <td width="12%">
            <asp:Label  ID="Label1" runat="server" Text="Vendor Code"></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="ddlVendor" runat="server" Font-Names="monospace" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged" AutoPostBack="True"> </asp:DropDownList>
            <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlVendor" QueryPattern="Contains"></asp:ListSearchExtender>
        </td>
        
    </tr>
    <tr>
        <td> 
            Vendor Name*
        </td>
        <td>
            <asp:TextBox  ID="txtvendor_name" runat="server" TextMode="SingleLine" MaxLength="50" Width="50%"></asp:TextBox>
            <asp:RequiredFieldValidator  ID="RequiredFieldValidator2" runat="server" ErrorMessage="RequiredFieldValidator" Text="* " ControlToValidate="txtvendor_name" Font-Size="X-Large" Font-Bold="True" ForeColor="Red"></asp:RequiredFieldValidator>
        </td>
    </tr>
                <tr>
                    <td width="12%">

            Contact Person*</td>
        <td nowrap="nowrap">
                 <asp:TextBox ID="txtcontact_person" runat="server" TextMode="SingleLine" MaxLength="50" Width="50%"></asp:TextBox>
                <asp:RequiredFieldValidator  ID="RequiredFieldValidator3" runat="server" ErrorMessage="RequiredFieldValidator" Text="*" ControlToValidate="txtcontact_person" Font-Size="X-Large" Font-Bold="True" ForeColor="Red"></asp:RequiredFieldValidator>
         </td>

                </tr>
    </table>
        </fieldset>

        <fieldset class="contact-info">
            <legend>Contact Information</legend>
            <table width="100%">
                <tr>
                    <td width="10%">Email
                    </td>
                    <td>
                        <asp:TextBox ID="txtemail" runat="server" MaxLength="50" Width="60%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Email must be @ and .com" ForeColor="Red" ControlToValidate="txtemail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                    </td>
                <td width="12%">
                    Phone Number
                    </td>
                    <td>
                        <asp:TextBox  ID="txtphone_1" runat="server" MaxLength="15" Width="180px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtphone_1" FilterType="Numbers" ></asp:FilteredTextBoxExtender>
                          
                        <asp:TextBox  ID="txtphone_2" runat="server" MaxLength="15" Width="180px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txtphone_2" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                 Fax
            </td>
             <td>
                 <asp:TextBox ID="txtfax_number" runat="server" MaxLength="15" Width="180px" ></asp:TextBox>
                 <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtfax_number" FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789-"></asp:FilteredTextBoxExtender>
            </td>
            <td>
                Address 1
            </td>
            <td>
                <asp:TextBox ID="txtaddress_1" runat="server" TextMode="SingleLine" MaxLength="50" Width="90%"></asp:TextBox>
            </td>
            
            </tr>
            <tr>
             <td>
                U.R.L
            </td>
            <td>
                <asp:TextBox ID="txturl" runat="server" MaxLength="50" Width="180px" ></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Email must be www.abc.com" ControlToValidate="txturl" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </td>
            <td>
                Address 2 & Address 3
            </td>
            <td>
                <asp:TextBox ID="txtaddress_2" runat="server" TextMode="SingleLine" MaxLength="50" Width="45%"></asp:TextBox>
                <asp:TextBox ID="txtaddress_3" runat="server" TextMode="SingleLine" MaxLength="50" Width="43%" ></asp:TextBox>
            </td>
            </tr>
       </table>
        </fieldset>

        <fieldset>
            <legend>Other Information</legend>
            <table width="100%">
                <tr>
                    <td width="12%">
                        G.S.T Reg. No.
                    </td>
                    <td>
                        <asp:TextBox  ID="txtgst_number" runat="server" MaxLength="50" Width="35%"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterType="Numbers" TargetControlID="txtgst_number"></asp:FilteredTextBoxExtender>
                    </td>
                    <td width="11%">
                        N.T.N No.
                     </td>  
                     <td>   
                        <asp:TextBox ID="txtntn_number" runat="server" MaxLength="50" Width="35%"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" FilterType="Numbers" TargetControlID="txtntn_number"></asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        Payment Terms
                    </td>
                    <td>
                        <asp:TextBox ID="txtpayment_terms" runat="server" Width="35%" ></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" FilterType="Numbers" TargetControlID="txtpayment_terms"></asp:FilteredTextBoxExtender>
                        Days
                    </td>
                    <td>
                        Account Code
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlacount_code" runat="server" style="width:50%;"></asp:DropDownList>
                        
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>

