<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" Theme="Theme1" AutoEventWireup="true" CodeBehind="UpdateLossNo.aspx.cs" Inherits="DXBMS.Modules.Service.Forms.UpdateLossNo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
               window.location.href = "UpdateLossNo.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "UpdateLossNo.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "UpdateLossNo.aspx";
           }
           function reloads() {
               window.location.href = "UpdateLossNo.aspx";
           }

       </script>
<asp:UpdatePanel runat="server">
     <%--<script type="text/javascript" >
        Sys.Application.add_load(jScript);
            </script>--%>
               <ContentTemplate>
               <div class="container">
           
                    <div class="Heading">
                        UPDATE LOSS NO  
                    </div>
                    <fieldset>
                        <legend>Loss No</legend>
                        <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                            <%-- Row 1 --%>
                            <tr>
                                <td >
                                   Estimate Code<asp:ImageButton runat="server" ID="imgEstNo" ImageUrl="~/Images/LookupNew.png" OnClick="imgEstNo_Click" />
                                </td>
                                <td >
                                 <asp:TextBox ID="txtExtno" runat="server" MaxLength="15" ReadOnly="True" SkinID="ROTextBox"
                                        TabIndex="1" Width="100%"></asp:TextBox>
                                  </td>
                                <td>
                                  Date
                                </td>
                                <td >
                                    <asp:TextBox ID="txtCreateDate" ReadOnly="true" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png);
                                        background-repeat: no-repeat; padding-left: 25px;" TabIndex="2" Width="100%"></asp:TextBox>
                                </td>

                                    <td>
                                    Loss No
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLossNo" runat="server" MaxLength="50" ReadOnly="FALSE" SkinID="ROTextBox"
                                        TabIndex="3" Width="100%"></asp:TextBox>
                                </td>
                              
                              
                            </tr>
                          
                      
                        </table>
                    </fieldset>
                 <table width="100%" style="margin-bottom: 10px;margin-top:10px;">
                    
                    <tr>
                     <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*) Feilds Are Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
                        <td>
                            <asp:Label ID="lblMsg" runat="server"></asp:Label>
                        </td>
                      <td style="text-align: right;">
                            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Update" OnClientClick="this.disabled = true; this.value = 'Please Wait';"
                                UseSubmitBehavior="false" />
                            <%--OnClientClick="Disable(this);"/>--%>
                            <%--<asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID ="btnSave" >
                            </asp:ConfirmButtonExtender>--%>
                            <asp:Button ID="btnClear" runat="server" Width="100px" Text="Clear" OnClick="btnClear_Click" />
                            
                            
                        </td>
                    </tr>
                </table>
                </div>

               </ContentTemplate>


</asp:UpdatePanel>


</asp:Content>
