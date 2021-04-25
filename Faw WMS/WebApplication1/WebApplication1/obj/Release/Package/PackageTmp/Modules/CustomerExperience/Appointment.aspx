<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" Theme="Theme1" CodeBehind="Appointment.aspx.cs" Inherits="DXBMS.Modules.CustomerExperience.Appointment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

<script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
    <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="container">
            <div class="Heading">Appointment</div>       
                    <fieldset>
               
                        <legend>Visit/Appointment</legend>
                        <asp:Calendar ID="Calendar1" runat="server" Width="100%" OnSelectionChanged="Calendar1_SelectionChanged" Format="dd-MM-yyyy"></asp:Calendar>
                            <div  >
                                 <table  width="70%" style="border-collapse:separate; border-spacing:5px;" 
                                 class="table table-condensed table-bordered table-hover"  > 
                                <tr width="100%" style="border-spacing:5px;">

                                    <td width="15%" style="padding-left:10px;">Visit Date   <font color="red"><b>*</b></font></td>
                                    <td >
                                         <asp:TextBox ID="txtVisitDate"  runat="server" Style="background-image: url(/Images/Calendar_16x16.png);
                                                   background-repeat:no-repeat; padding-left:20px;" MaxLength="10"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtVisitDate" Format="dd-MM-yyyy"></asp:CalendarExtender>
                                       <%-- <asp:Button ID="btnView" runat="server" Text="View" OnClick="btnView_Click" width=" 66px"/>--%>
                                        <%--<asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" width="66px"/>--%>
                                    </td>
                                    <td style="padding-left:10px;">Date</td>
                                    <td>
                                        <asp:TextBox ID="txtDate" runat="server" Width="145px" Style="margin-top: 6px; background-image: url(/Images/Calendar_16x16.png);
                                                   background-repeat:no-repeat; padding-left:20px;" MaxLength="10"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDate" Format="dd-MM-yyyy"></asp:CalendarExtender>
                                    </td>
                                    <%--<td style="padding-left:10px;">ID</td>
                                    <td>
                                        <asp:DropDownList ID="ddlRegNo" runat="server"  Width="167px" OnSelectedIndexChanged="ddlRegNo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList >
                        
                                    </td>--%>
                                </tr>
                                <tr>
                                    <td width="15%" style="padding-left:10px;">Reg. No.  <font color="red"><b>*</b></font></td>
                                    <td width="35%">
                                        <asp:TextBox ID="txtRegNo" runat="server" width="168px" MaxLength="10"></asp:TextBox>                          
                                    </td>
                                  <td width="15%" style="padding-left:10px; " >Vehicle Model <font color="red"><b>*</b></font></td>
                                    <td >
                                        <asp:DropDownList ID="ddlVehicleModel" runat="server" Width="147px"></asp:DropDownList>
                                    </td>
                   
                                </tr>
                                <tr>
                                     <td width="11%" style="padding-left:10px;">Customer Name   <font color="red"><b>*</b></font>  </td>
                                    <td>
                                       <asp:TextBox ID ="txtCustName" runat="server" width="170px"></asp:TextBox>
                       
                                    </td>
                                    <td style="padding-left:10px;">Contact No.  <font color="red"><b>*</b></font></td>
                                    <td>
                                        <asp:TextBox ID ="txtContactNo" runat="server" placeholder="0300-1234567" MaxLength="12" style="text-align:right;" ></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                   runat="server" FilterType="Custom, Numbers" TargetControlID="txtContactNo" ValidChars="-">
                               </asp:FilteredTextBoxExtender>
                                    </td>
                    

                                </tr>
                                <tr>
                    
                                    <td width="8%" style="padding-left:10px;  padding-top:10px;" >Time Slot<font color="red"><b>*</b></font></td>
                                    <td >
                                        <asp:DropDownList ID="ddlTimeSlot" runat="server" Width="147px"></asp:DropDownList>
                                    </td>
                                   <td width="10%" style="padding-left:10px;">Priority</td>
                                   <td>
                                      <asp:DropDownList ID="ddlPriority" runat="server" Width="147px"></asp:DropDownList>

                                   </td>
                   
                                </tr>
                                <tr>
                                   <td style="padding-left:10px;" >Purpose</td>
                                    <td >
                                        <asp:DropDownList ID="ddlpurpose" runat="server" Width="147px"></asp:DropDownList>
                      
                                    </td>
                                                    
                               </tr>             
                             </table>
                            </div>
                    </fieldset>
                    <fieldset>
                        <table width="100%">
                <tr>
                    <td >
                        <asp:GridView runat="server" ShowFooter="true" ShowHeaderWhenEmpty="true" width="100%" ID="gv_visit" AutoGenerateColumns="false" >

                            <HeaderStyle CssClass="GridHeader" />
                            <FooterStyle CssClass="GridFooter" />
                                                                                  

                              <Columns>

                                     <asp:TemplateField HeaderText="SNo" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                  <asp:TemplateField HeaderText="ID" ItemStyle-HorizontalAlign="left">
                                      <ItemTemplate>
                                          <asp:Label ID="Label1" runat="server" Text='<%# Bind("App_Id") %>'></asp:Label>
                                      </ItemTemplate>
                                      <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1"  runat="server"></asp:TextBox>
                                      </EditItemTemplate>
                                     <ItemStyle HorizontalAlign="Center" />
                                  </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Date" ItemStyle-HorizontalAlign="left">
                                      <ItemTemplate>
                                          <asp:Label ID="lblId" runat="server" Text='<%# Bind("VisitDate") %>'></asp:Label>
                                      </ItemTemplate>
                                      <EditItemTemplate>
                                            <asp:TextBox ID="txt_Visitdate"  runat="server"></asp:TextBox>
                                      </EditItemTemplate>
                                     <ItemStyle HorizontalAlign="Center" />
                                  </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Reg No." ItemStyle-HorizontalAlign="left">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkRegNo" runat="server" Text='<%# Bind("RegNo") %>' OnClick="lnkRegNo_Click"></asp:LinkButton>
                                           <%--<asp:Label ID="lblRegNo" runat="server" Text='<%# Bind("RegNo") %>'></asp:Label> --%>
                                        </ItemTemplate>
                                      <EditItemTemplate>
                                            <asp:DropDownList ID="ddl_RegNo" runat="server" width="80px"></asp:DropDownList>
                                      </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Vehicle Model" ItemStyle-HorizontalAlign="left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVM" runat="server" Text='<%# Bind("VehicleModel") %>'></asp:Label> 
                                        </ItemTemplate>
                                      <EditItemTemplate>
                                            <asp:DropDownList ID="DropDownList1" runat="server" width="80px"></asp:DropDownList>
                                      </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Cust Name" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                           <asp:Label ID="lblCustName" runat="server" Text='<%# Bind("CustName") %>'></asp:Label> 
                                        </ItemTemplate>
                                      <EditItemTemplate>
                                            <asp:TextBox ID="txt_CustName"  runat="server"></asp:TextBox>
                                      </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Contact Number" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                           <asp:Label ID="lblContact" runat="server" Text='<%# Bind("Contact") %>'></asp:Label> 
                                        </ItemTemplate>
                                      <EditItemTemplate>
                                            <asp:TextBox ID="txt_Cont"  runat="server"></asp:TextBox>
                                      </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Time" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                           <asp:Label ID="lblTime" runat="server" Text='<%# Bind("TimeSlot") %>'></asp:Label> 
                                        </ItemTemplate>
                                      <EditItemTemplate>
                                            <asp:DropDownList ID="ddl_TimeSlot" runat="server"></asp:DropDownList>
                                      </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Purpose" ItemStyle-HorizontalAlign="center">
                                      <ItemTemplate>
                                          <asp:Label ID="lblChassesNo" runat="server" Text='<%# Bind("Purpose") %>'></asp:Label>
                                      </ItemTemplate>
                                      <EditItemTemplate>
                                          <asp:DropDownList ID="ddl_purpose" runat="server"></asp:DropDownList>
                                      </EditItemTemplate>
                                  </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Priority" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                           <asp:Label ID="lblPriority" runat="server" Text='<%# Bind("Priority") %>'></asp:Label> 
                                        </ItemTemplate>
                                      <EditItemTemplate>
                                            <asp:DropDownList ID="ddl_Priority" runat="server"></asp:DropDownList>
                                      </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>


                              
                              </Columns>

                        </asp:GridView>
                    </td>
                </tr>
            </table>
                    </fieldset>    

                <div class="col-lg-12">
                    <table style="margin-bottom: 5px; width:100%" >
                        <tr>
                            
                            <td >
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </td>
                            <td style="text-align:center;padding-top:5px;">
                            <asp:Button  CssClass="btn btn-primary" ID="btnAdd"  runat="server" Text="Book Appointment" OnClick="addVisitor" Width="140px"></asp:Button>
                            <asp:Button ID="btnClear" CssClass="btn btn-default" runat="server" Text="Clear" OnClick="clearFields"></asp:Button>
                            <%--<asp:Button ID="test"  runat="server" Text="Test" OnClick="Test"></asp:Button>--%>
                            </td>
                        </tr>
                   </table>
                  </div>

        </div>
    </ContentTemplate>
</asp:UpdatePanel>

</asp:Content>
