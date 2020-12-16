<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyProfile.aspx.cs" Theme="Theme1" Inherits="DXBMS.Modules.BusinessPartner.CompanyProfile" MasterPageFile="~/Root.master"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ MasterType VirtualPath="~/Root.master"  %>


<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <script src="../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="../../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../js/chosen.jquery.js" type="text/javascript"></script>
    <script src="../../js/chosen.jquery.min.js" type="text/javascript"></script>
    <style>
       
        .file-upload 
        {
            margin-top: -151px;
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
        height: 90px;
    border-radius: 50%;
    background: #0c0c0c6b;
}

    .file-upload, .file-upload span {
        width: 90px;
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

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <Triggers>               
                <asp:PostBackTrigger ControlID="btnUpload" />         
                 <asp:AsyncPostBackTrigger ControlID="btnSave" />       
            </Triggers>
        <ContentTemplate>
            <div class="container">   
              <span class="fa-arrow-left fa"></span>
                        <asp:LinkButton ID="lnkBack" ForeColor="Black" runat="server" PostBackUrl="~/" Text="Back"></asp:LinkButton>
                              
                <h3 class="Heading">Dealer Setup Form</h3>
                <fieldset>
                    <legend>Dealer Information</legend>
                    <div class="row">
                    <div class="row" style="float:left; width:70%; margin-left: 10px;">
                        <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">        
                        <tr>
                    <td >
                        Dealer Code:
                        <asp:Label ID="lbl3" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                    <td >
                        <asp:TextBox ID="txtDealerCode" runat="server" MaxLength="5" ReadOnly="True" Width="100%" BackColor="#bacbd8"></asp:TextBox>                        
                    </td>
                    <td>Dealer Ref Code</td>
                  <td>
                   <asp:TextBox ID="txtDealerRefC" runat="server" ReadOnly="true"  Width="100%" BackColor="#bacbd8"></asp:TextBox>
                   </td>
                </tr>
                        <tr>
                    <td>Dealer Name:<asp:Label ID="lbl2" runat="server" ForeColor="#FF3300" Text="*"></asp:Label></td>
                    <td colspan="3">
                        <asp:TextBox ID="txtDealerName" runat="server" Width="100%" MaxLength="50" style="text-transform: uppercase" ></asp:TextBox>
                    </td>
                </tr>
                        <tr>
                    <td >
                        Address:
                    </td>
                    <td colspan="3" style="text-align: left">
                        <asp:TextBox ID="txtAddress1" runat="server" Width="100%" MaxLength="100" style="text-transform: uppercase" ></asp:TextBox>
                    </td>
                </tr>
                        <tr>
                    <td >
                        City
                    </td>
                    <td >
                        <asp:TextBox ID="txtAddress2" runat="server" Width="100%" MaxLength="50" style="text-transform: uppercase" ></asp:TextBox>
                    </td>
                    <td >
                        Country
                    </td>
                    <td >
                        <asp:TextBox ID="txtAddress3" runat="server" Width="100%" MaxLength="50" style="text-transform: uppercase" ></asp:TextBox>
                    </td>
                </tr>
                        <tr>
                    <td >
                        Email:</td>
                    <td>
                        <asp:TextBox ID="txtEmail1" runat="server" MaxLength="50" width="100%"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" 
                            ControlToValidate="txtEmail1" ErrorMessage="*" ForeColor="#FF3300" 
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                    </td>
                    <td style="width: 10%">
                        Fax:</td>
                    <td style="width: 30%">
                        <asp:TextBox ID="txtFax" runat="server" MaxLength="15" Width="100%"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtFax" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                        <tr>
                    <td>
                        Phone1:
                    </td>
                    <td style="width: 30%">
                        <asp:TextBox ID="txtPhone1" runat="server" MaxLength="15" width="100%"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                            FilterMode="ValidChars" FilterType="Numbers" TargetControlID="txtPhone1">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td >
                        Phone2:</td>
                    <td >
                        <asp:TextBox ID="txtPhone2" runat="server" MaxLength="15" Width="100%"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="txtPhone2_FilteredTextBoxExtender" 
                            runat="server" FilterMode="ValidChars" FilterType="Numbers" 
                            TargetControlID="txtPhone2">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    
                </tr>
                        <tr>
                    <td>
                        S. Tax Reg No:</td>
                    <td >
                        <asp:TextBox ID="txtTaxReg" runat="server" MaxLength="20" width="100%"></asp:TextBox>
                    </td>
                     <td >
                        NTN:</td>
                    <td >
                        <asp:TextBox ID="txtNTN" runat="server" MaxLength="50" Width="100%"></asp:TextBox>                       
                    </td>                   
                </tr>
                        <tr>
                    <td>Credit Limit</td>
                    <td>
                    <asp:TextBox ID="txtCreditLimit" runat="server" MaxLength="6" Width="100%"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtCreditLimit" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                    
                    </td>
                    <td>PST %</td>
                    <td>
                        <asp:TextBox ID="txtPSTPerc" runat="server" MaxLength="3" Width="100%"></asp:TextBox>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" Type="Integer" ControlToValidate="txtPSTPerc" MaximumValue="100" MinimumValue="1"
                                        ValidationGroup="form" ForeColor="Red" ErrorMessage="Must be equal or less than 100" />
                    </td>
                             
                </tr>
                <tr>
                 <td>Handling Charges %</td>
                                 <td>
                        <asp:TextBox ID="txtHandling" runat="server" MaxLength="3" Width="100%"></asp:TextBox>
                        <asp:RangeValidator ID="RangeValidator2" runat="server" Type="Integer" ControlToValidate="txtHandling" MaximumValue="100" MinimumValue="1"
                                        ValidationGroup="form" ForeColor="Red" ErrorMessage="Must be equal or less than 100" />
                    </td>
                    <td>
                    Operated As
                    </td>
                    <td>
                    <asp:TextBox runat="server" ReadOnly="true" ID="txtOpAs" Width="100%" BackColor="#bacbd8"></asp:TextBox>
                    </td>
                </tr>
                  <tr>
                 
                    <td>
                    Vehicle Category 
                    </td>
                    <td>
                    <asp:TextBox runat="server" ReadOnly="true" ID="txtVehicleCategory" Width="100%" BackColor="#bacbd8"></asp:TextBox>

                    </td>
                </tr>
                </table>
                    </div>
                    <div class="row" style=" float:left; width:24%; margin-right: 0; margin-left:53px; margin-top: -10px;">
                        <fieldset>
                        <table width="30%">
                        <tr>
                            <td><asp:Image ID="Image1" Height="240px" Width="230px" style="border-radius: 0%;" runat="server" /></td>
                        </tr>
                        <tr>
                            <td style="text-align:center;">
                            <label class="file-upload">
                                    <asp:Image ID="btnImage" runat="server" ImageUrl="~/Images/camera.png" style="margin-top : 25px;" Width="35px" Height="35px"/>                            
                                    <asp:FileUpload ID="FileUpload1" runat="server" ></asp:FileUpload>
                            </label>
                            </td>
                            
                        </tr>
                        <tr>
                            <td style="text-align:center;">
                                <asp:Button ID="btnUpload" runat="server" Text="Upload"  OnClick="OnClick_btnUpload" CausesValidation="false" />                                
                            </td>
                        </tr>
                    </table>
                        </fieldset>
                    </div>
                </div>
                </fieldset>
                <table width="100%">
            <tr>
             <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*) Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
                 <td >
                        <asp:Label ID="lblmsg" style="font-size:14px;" runat="server"></asp:Label>
                 </td>
                 <td style="text-align:center;">
                        <asp:Button ID="btnSave" runat="server" Width="100px"  Text="Save"  OnClick="btnSave_Click" CausesValidation="false" OnClientClick="this.disabled = true; this.value = 'Please Wait';" UseSubmitBehavior="false"/>
                        <%--<asp:Button ID="btncler" runat="server"  Text="Clear" OnClick="btnClear_Click" />--%>
                        
                 </td>
            </tr>
            <%--<tr>
                <td  align="center">
                <asp:Label ID="Label6" runat="server" skinID="Heading" Text="Dealer Setup Form"></asp:Label>
                </td>
           </tr>--%>           
        </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>