<%@ Page Title="" Language="C#" Theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="employeemaster.aspx.cs" Inherits="DXBMS.Modules.Setup.employeemaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">

<script src="../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../js/chosen.jquery.min.js" type="text/javascript"></script>
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
               window.location.reload();
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.reload();
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
               window.location.reload();
           }
           function reloads() {
               window.location.reload();
           }

       </script>
<style>
        .file-upload 
        {
            margin-top: -100%;
            border-radius: 59%;
            opacity: 0;
            display: inline-block;
            overflow: hidden;    
            text-align: center;
            vertical-align: middle;
            font-family: Arial;
            border: 1px solid #124d77;    
            color: #fff;            
            -moz-border-radius: 6px;
            cursor: pointer;
            text-shadow: #000 1px 1px 2px;
            -webkit-border-radius: 6px;
}

   .file-upload:hover {
      /*  background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #0061a7), color-stop(1, #007dc1));
        background: -moz-linear-gradient(top, #0061a7 5%, #007dc1 100%);
        background: -webkit-linear-gradient(top, #0061a7 5%, #007dc1 100%);
        background: -o-linear-gradient(top, #0061a7 5%, #007dc1 100%);
        background: -ms-linear-gradient(top, #0061a7 5%, #007dc1 100%);
        background: linear-gradient(to bottom, #0061a7 5%, #007dc1 100%);
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#0061a7', endColorstr='#007dc1',GradientType=0); */
        opacity: 1;
   }

/* The button size */
.file-upload {
        height: 70px;
    border-radius: 50%;
    background: #0c0c0c6b;
}

    .file-upload, .file-upload span {
        width: 70px;
    }

        .file-upload input {
            
            top: 0;
            left: 0;
            margin: 0;
            font-size: 11px;
            font-weight: bold;
            /* Loses tab index in webkit if width is set to 0 */
            opacity: 0;
            filter: alpha(opacity=0);
        }

        .file-upload strong {
            font: normal 12px Tahoma,sans-serif;
            text-align: center;
            vertical-align: middle;
        }

        .file-upload span {
            
            top: 0;
            left: 0;
            display: inline-block;
            /* Adjust button text vertical alignment */
            padding-top: 5px;
        }
    </style>
    
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
         <%--   <Triggers>               
                <asp:PostBackTrigger ControlID="btnUpload" />            
            </Triggers>--%>
        <ContentTemplate>
            <div class="container">
    
    <div class="Heading">
        Employee Master
    </div>

    <fieldset>
        <legend>Personal Details</legend>
            <div class="row">
            <div class="row" style=" float:left; width:16%; margin-right: 0; margin-left:18px; margin-top: -5px;">
                        
                        <table width="30%">
                        <tr>
                            <td><asp:Image ID="Image1" Height="130px" Width="145px" ImageUrl="~/Images/BlankProfile.jpg" style="border-radius: 50%;" runat="server" /></td>
                        </tr>
                        <tr>
                            <td style="text-align:center;">
                            <label class="file-upload">
                                    <asp:Image ID="btnImage" runat="server" ImageUrl="~/Images/camera.png" style="margin-top : 15px;" Width="35px" Height="35px"/>                            
                                    <asp:FileUpload ID="FileUpload1" runat="server" ></asp:FileUpload>
                            </label>
                            </td>
                            
                        </tr>
                        <tr>
                            <td style="text-align:center;">
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="OnClick_btnUpload" CausesValidation="false" PostBackUrl="~/Modules/Setup/employeemaster.aspx" />                     
                            </td>
                        </tr>
                    </table>
                        
                    </div>
            <div class="row" style="float:left; width:81%; margin-left: 0px;">
            <table width="100%"  style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
            <tr>
                <td width="12%">Employee Code</td>
                <td><asp:DropDownList ID="ddlEmployee" runat="server" OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged" AutoPostBack="true" Width="85%"></asp:DropDownList>
                <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlEmployee" QueryPattern="Contains"></asp:ListSearchExtender>
                </td>
                <td style="white-space: nowrap;">Manual Employee No.</td>
                <td>
                    <asp:TextBox ID="txtEmpID" runat="server" Width="25%" MaxLength="5" AutoPostBack="true" OnTextChanged="txtEmpID_TextChanged"  ></asp:TextBox>
                    <asp:RegularExpressionValidator ControlToValidate="txtEmpID" ValidationExpression="^[\s\S]{5}$" runat="server" ErrorMessage="ID must be 5 characters long."></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>Employee Name</td>
                <td  width="45%">
                    <asp:TextBox ID="txtEmp_name" runat="server" MaxLength="50" Width="75%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtEmp_name" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>
                </td>
                <td width="10%">Date Of Birth</td>
                <td >
                     <asp:TextBox ID="txtdob" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"
                        Width="60%" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtdob" Format="dd-MM-yyyy" ></asp:CalendarExtender>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtdob" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                    <td>Contact No</td>
                    <td > 
                        <asp:TextBox ID="txtcontact_no" runat="server" MaxLength="15" Width="75%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtcontact_no" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>
                       <%-- <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtcontact_no" runat="server" MaskType="Number" Mask="9999/9999999" InputDirection="LeftToRight" AcceptNegative="None"></asp:MaskedEditExtender>--%>
                    </td>
                    <td>NIC</td>
                    <td >
                        <asp:TextBox ID="txtnic_no" runat="server" MaxLength="15" Width="60%" placeholder="1234-1234567-1" AutoPostBack="true" OnTextChanged="txtnic_no_TextChanged"></asp:TextBox>
                        
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtnic_no" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>
                    </td>
                </tr> 
                <tr>
                    <td>Email</td>
                    <td colspan="3">
                        <asp:TextBox ID="txtEmail" runat="server" Width="37.2%" MaxLength="25" placeholder="abc93@mail.com"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid Email" ForeColor="#FF3300" 
                            ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" ></asp:RegularExpressionValidator>
                    </td>
                </tr>
            </table>
            </div>            
            </div>
    </fieldset>

    <fieldset>
        <legend>Employee Details</legend>      
            
            <table width="100%"  style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                <tr>
                    <td width="12%">Joining Date</td>
                    <td width="45%">
                        <asp:TextBox ID="txtjoining_date" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"
                        Width="125px" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="txtjoining_date" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtjoining_date" Format="dd-MM-yyyy" ></asp:CalendarExtender>
                    </td>
                    <td width="10%">Depart. Code</td>
                    <td>
                        <asp:DropDownList ID="ddldep_code" runat="server" Width="125px"></asp:DropDownList>
                        <asp:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddldep_code" QueryPattern="Contains"></asp:ListSearchExtender>
                    </td>
                </tr>
                <tr>
                    <td>Leaving Date</td>
                    <td>
                        <asp:TextBox ID="txtleaving_date"  runat="server"   Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"
                        Width="125px" placeholder="dd/mm/yyyy"></asp:TextBox>
                       <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtleaving_date" Format="dd-MM-yyyy" ></asp:CalendarExtender>
                    
                    </td>
                    <td>Desig. Code</td>
                    <td>
                        <asp:DropDownList ID="ddldesig_code" runat="server" Width="125px"></asp:DropDownList>
                        <asp:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddldesig_code" QueryPattern="Contains"></asp:ListSearchExtender>
                    </td>
                </tr>
                <tr>
                    <td>Experience</td>
                    <td nowrap="nowrap">
                        <asp:TextBox ID="txtexp" runat="server" Width="125px" MaxLength="2"></asp:TextBox> Years
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txtexp" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtexp" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                    </td>
                    <td>Active</td>
                    <td>
                        <asp:CheckBox ID="cbActive" runat="server" Text="Active" />
                    </td>
                </tr>
                 <tr>
                   <td>Remarks</td>
                    <td colspan="3">
                        <asp:TextBox ID="txtremarks" runat="server" Width="300px" TextMode="MultiLine" MaxLength="100" Wrap="true" ></asp:TextBox>
                    </td>
                </tr>
            </table>                
    </fieldset>
         <table width="100%" style="margin-bottom: 10px;margin-top:10px;">
        <tr>
            <td>
                <asp:Label ID="success_add_item" runat="server" Text="Item Added Succesfully" Font-Bold="True" ForeColor="Red" ></asp:Label>
            </td>
            <td style="text-align:right;">
                <asp:Button ID="btn_save" runat="server" Width="100px" Text="Save"  CausesValidation="True" OnClick="btn_save_Click" />
                <asp:Button ID="btn_delete" runat="server" Width="100px" Text="Delete"  CausesValidation="False" OnClick="btn_delete_Click" />
                <asp:Button ID="btn_clear" runat="server" Width="100px" Text="Clear" CausesValidation="false" OnClick="btn_clear_Click" />          
            </td>
        </tr>
    </table>

          </div>  
    </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


