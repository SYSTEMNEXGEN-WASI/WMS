<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="Complaint.aspx.cs" Inherits="DXBMS.Modules.Setup.Complaint" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 210px;
        }

        .auto-style4 {
            width: 210px;
        }

        .auto-style5 {
            width: 154px;
        }
        .auto-style8 {
            width: 110px;
        }
        .auto-style9 {
            width: 252px;
        }
        .auto-style10 {
            width: 644px;
        }
        .auto-style11 {
            width: 4px;
        }
        .auto-style12 {
            width: 458px;
        }
    </style>
    <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
                <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
                <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
                <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
    <script>
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to delete data?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
     <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div>
                        <fieldset>
                            <legend>Complaint </legend>

                            <div class="table-responsive">
                                
                                       <table  style="border-collapse: separate; float:left; width:60%; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover"  >
                                    <tr>
                                        
                                        <td class="auto-style1">Ticket No</td>
                                        <td class="auto-style4">
                                            <asp:TextBox ID="txtTicketNo" runat="server" Height="21px" MaxLength="5" Width="100%" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    
                                        <td class="auto-style1">Ticket Date<span style="color:red;">*</span></td>
                                        <td class="auto-style4">
                                            <asp:TextBox  ID="txtTicketDate" runat="server" style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;" ></asp:TextBox>
                                        <asp:CalendarExtender runat="server" TargetControlID="txtTicketDate" Format="dd-MM-yyyy"></asp:CalendarExtender>

                                        </td>


                                    </tr>
                                           <tr>
                                               <td class="auto-style1">Subject </td>
                                        <td class="auto-style4">
                                            <asp:TextBox ID="txtSubject" runat="server" Height="21px" MaxLength="50" Width="100%" ></asp:TextBox>
                                        </td>
                                               <td class="auto-style1">Complain By</td>
                                        <td class="auto-style4">
                                            <asp:DropDownList ID="ddlComplaintby" runat="server" Width="100%" AutoPostBack="true"
                                                >
                                                <asp:ListItem value="0" >
                                                       Select
                                                   </asp:ListItem>
                                                 </asp:DropDownList>
 
                                        </td>

                                           </tr>

                                    <tr>
                                        <td class="auto-style8">Type</td>
                                        <td class="auto-style11">
                                            <asp:DropDownList ID="ddlType" runat="server" Width="100%" AutoPostBack="true"
                                                >
                                                <asp:ListItem value="0" >
                                                       Select
                                                   </asp:ListItem>
                                                <asp:ListItem Value="Bug">
                                                    Bug
                                                </asp:ListItem>
                                                <asp:ListItem Value="Understanding">
                                                    Understanding
                                                </asp:ListItem>
                                                <asp:ListItem Value="Training">
                                                    Training
                                                </asp:ListItem>
                                                <asp:ListItem Value="Information">
                                                    Information
                                                </asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="auto-style12">Priority</td>
                                        <td class="auto-style5">
                                            <asp:DropDownList ID="ddlPriority" runat="server" Width="100%" AutoPostBack="true"
                                                >
                                                <asp:ListItem value="0" >
                                                       Select
                                                   </asp:ListItem>
                                                <asp:ListItem Value="High">
                                                    High
                                                </asp:ListItem>
                                                <asp:ListItem Value="Medium">
                                                    Medium
                                                </asp:ListItem>
                                                <asp:ListItem Value="Low">
                                                    Low
                                                </asp:ListItem>
                                             </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style8">Complaint</td>
                                        <td colspan="3"
                                            rowspan="1" class="auto-style10">
                                            <asp:TextBox ID="txtComplaint" runat="server" Height="95%" Width="99%" TextMode="MultiLine" ></asp:TextBox>
                                        </td>

                                    </tr>
                                   
                                    <tr>
                                        
                                        <td class="auto-style8">Status</td>
                                        <td class="auto-style11">
                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="100%" AutoPostBack="true"
                                                >
                                                <asp:ListItem value="0" >
                                                       Select
                                                   </asp:ListItem>
                                                <asp:ListItem Value="Pending">
                                                    Pending
                                                </asp:ListItem>
                                                <asp:ListItem Value="InProcess">
                                                    In Process
                                                </asp:ListItem>
                                                <asp:ListItem Value="Hold">
                                                    Hold
                                                </asp:ListItem>
                                                <asp:ListItem Value="Resolve">
                                                    Resolve
                                                </asp:ListItem>
                                                <asp:ListItem Value="N/A">
                                                    N/A
                                                </asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="auto-style12">Status by OEM</td>
                                        <td  class="auto-style5">
                                            <asp:TextBox ID="txtStatusByOEM" runat="server" Height="21px" MaxLength="30" Width="95%" ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style8">Remarks by OEM</td>
                                        <td colspan="3" class="auto-style10">
                                            <asp:TextBox ID="txtRemarks" runat="server" Height="21px" MaxLength="50" Width="99%" ></asp:TextBox>
                                        </td>
                                    </tr>
                                           <tr>
                                               <td class=" auto-style4" colspan="4">
                                            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label></td>
                                           </tr>
                                    
                                    <tr>
                                        
                                        
                                        <td class="auto-style1" colspan="4">
                                            <asp:Button ID="btnInsert" runat="server" Height="30px" Text="Save" OnClick="btnInsert_Click" Width="200px"  />
                                          
                                             <asp:Button ID="btnClear" runat="server" Height="30px" Text="Clear"  Width="200px" OnClick="btnClear_Click"  />
                                        
                                             <asp:Button ID="btn_Delete" runat="server" Height="30px" Text="Delete"  Width="200px" OnClick="btn_Delete_Click" OnClientClick="return confirm('Are you sure you want to delete ?')"  />
                                            <asp:Button ID="btnShow" runat="server" Height="30px" Width="200px" Text="Show Report" OnClick="btnShow_Click" />
                                        </td>
                                            
                                        
                                    </tr>

                                </table>
                                
                                        <table  style="border-collapse: separate;  float:right; width:40%; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover" >
                                     <tr>
                                        <td >Image</td>
                                        <td>

                                            <asp:FileUpload ID="FileUpload1" runat="server" />
                                            <asp:Button runat="server" ID="btnUpload" Text="Upload" OnClick="btnUpload_Click" />
                                             

                                        
                                       </td>
                                    </tr>
                                        <tr>
                                          
                                            <td colspan="3">
                                                 <asp:Image ID="Image1" Height="35%" Width="100%" style="border-radius: 0%;" runat="server" />
                                            </td>
                                            </tr>
                                </table>
                           </div>
                                     
                                        <asp:GridView runat="server" ShowHeaderWhenEmpty="true" Width="100%" ID="gv_Complaint" AutoGenerateColumns="false"
                                            AllowPaging="True"
                                            OnSelectedIndexChanged="gv_Complaint_SelectedIndexChanged"
                                            OnDataBound="gv_Complaint_DataBound"
                                            OnRowDataBound="gv_Complaint_RowDataBound">
                                           <%-- OnRowEditing="gv_Complaint_RowEditing"
                                            OnRowCancelingEdit="gv_Complaint_RowCancelingEdit"
                                            OnRowUpdating="gv_Complaint_RowUpdating"
                                            --%>
                                           
                                             


                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />

                                            <Columns>
                                                <asp:CommandField ShowSelectButton="True"></asp:CommandField>
<%--                                                <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                    UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>--%>
                                                
                                                <asp:TemplateField HeaderText="Ticket No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTicketNo" runat="server" Text='<%# Bind("TicketNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="TicketDate">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_TicketDate" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTicketDate" runat="server" Text='<%# Bind("TicketDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Type">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="ddl_Type" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblType" runat="server" Text='<%# Bind("ComplaintTypeCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Priority">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="ddlPriority" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPriority" runat="server" Text='<%# Bind("Priority") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Complaint">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtComplaint" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblComplaint" runat="server" Text='<%# Bind("ComplaintDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="ddlStatus" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status by OEM">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="ddlStatusByOEM" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatusByOEM" runat="server" Text='<%# Bind("StatusByOEM") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks by OEM">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtRemarksByOEM" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarksByOEM" runat="server" Text='<%# Bind("StatusRemarks") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                
                                            </Columns>
                                            <%--<PagerSettings FirstPageText="First" LastPageText="Last" />--%>
                                        </asp:GridView>
                                    </td>
                                </tr>

                            </table>

                        </fieldset>

                    </div>
                </div>
            </div>

        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
