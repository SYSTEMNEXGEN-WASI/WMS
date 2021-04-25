<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="WorkFlowPolicy.aspx.cs" Inherits="DXBMS.Modules.WorkFlow.WorkFlowPolicy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

    
    <asp:updatepanel runat="server">

        <ContentTemplate>
            <div class="container">
              
                    <div>
                        <fieldset>
                            <legend>WorkFlow Creation </legend>

                            <div class="table-responsive">


                                <table width="70%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">

                                    <tr>
                                        <td class="auto-style10">Policy Code</td>
                                        <td class="auto-style9">
                                            <asp:DropDownList ID="ddlWFPolicyMasterCode" runat="server" AutoPostBack="True" Font-Names="monospace" Width="234px" OnSelectedIndexChanged="ddlWFPolicyMasterCode_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>Policy Description</td>
                                        <td>
                                             <asp:TextBox ID="txtPolicyDesc" runat="server" Width="234px"></asp:TextBox>
                                        </td>
                                       
                                    </tr>
                                    <tr>
                                        <td class="auto-style10">Policy Type</td>
                                        <td class="auto-style9">
                                            <asp:DropDownList ID="ddlPolicyType" runat="server" AutoPostBack="True" Font-Names="monospace" Width="234px">
                                            </asp:DropDownList>
                                        </td>
                                         <td class="auto-style8">Solution</td>
                                        <td class="auto-style10">
                                            <asp:DropDownList ID="ddlSolution" runat="server" AutoPostBack="True" Font-Names="monospace" Width="234px">
                                            </asp:DropDownList>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td class="auto-style15">Start Date </td>
                                        <td class="auto-style14">
                                            <asp:TextBox ID="txtStartDate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;" Width="234px"></asp:TextBox>
                                            <asp:CalendarExtender runat="server" TargetControlID="txtStartDate" Format="dd-MM-yyyy"></asp:CalendarExtender>
                                        </td>
                                        <td class="auto-style15">End Date </td>
                                        <td class="auto-style14">
                                            <asp:TextBox ID="txtEndDate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;" Width="234px"></asp:TextBox>
                                            <asp:CalendarExtender runat="server" TargetControlID="txtEndDate" Format="dd-MM-yyyy"></asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style8">Status</td>
                                        <td class="auto-style10">
                                            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" Font-Names="monospace" Width="234px">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="auto-style15">Comment </td>
                                        <td class="auto-style14">
                                            <asp:TextBox ID="txtComment" runat="server" Width="234px"></asp:TextBox>
                                        </td>
                                    </tr>



                                    <tr>
                                        <td class="auto-style8" colspan="2">
                                            <%--<asp:Button ID="btnInsert" runat="server" Height="30px" Text="Insert" Width="200px" OnClick="btnInsert_Click" />--%>
                                        </td>
                                        <td class="auto-style10" colspan="2">
                                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                        </td>
                                    </tr>

                                </table>



                            </div>

                            <!---- FOR GRIDVIEW -->

                            <table width="100%">
                                <tr>
                                    <td width="100%">
                                        <asp:GridView ID="gv_WFPolicy" runat="server" ShowFooter="true" ShowHeaderWhenEmpty="true" Width="100%"
                                            OnDataBound="gv_WFPolicy_DataBound"
                                            OnRowDataBound="gv_WFPolicy_RowDataBound"
                                            OnRowEditing="gv_WFPolicy_RowEditing"
                                            OnRowCancelingEdit="gv_WFPolicy_RowCancelingEdit"
                                            OnRowUpdating="gv_WFPolicy_RowUpdating"

                                            AutoGenerateColumns="False">

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
                                                        <asp:ImageButton ID="BtnAdd_WFPolicy" runat="server" CausesValidation="false"
                                                            ImageUrl="~/Images/Grid_Update.png" OnClick="BtnAdd_WFPolicy_Click" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>                         

                                                <asp:TemplateField HeaderText="Approval" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblApprover" runat="server" Text='<%# Bind("ApprovalCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%--<EditItemTemplate>
                                                        <asp:TextBox ID="txtApproverEdit" runat="server" Width="150px" placeholder="Approver"></asp:TextBox>
                                                    </EditItemTemplate>--%>
                                                    <FooterTemplate>
                                                        <%--<asp:TextBox ID="txtApprover" runat="server" Width="150px" ForeColor="Black" Font-Bold="true" placeholder="Approver"></asp:TextBox>--%>
                                                    <asp:DropDownList ID="ddlApprover" runat="server"  Width="150px" ForeColor="Black" ></asp:DropDownList>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                         

                                                <asp:TemplateField HeaderText="Priority">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPriority" runat="server" Text='<%# Bind("Priority") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <%--<asp:TextBox ID="txtPriorityEdit" runat="server" Width="150px" placeholder="Priority"></asp:TextBox>--%>
                                                        <asp:DropDownList ID="ddlPriorityEdit" runat="server"  Width="150px" ForeColor="Black" ></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <%--<asp:TextBox ID="txtPriority" runat="server" Width="150px" ForeColor="Black" Font-Bold="true" placeholder="Priority"></asp:TextBox>--%>
                                                    <asp:DropDownList ID="ddlPriority" runat="server"  Width="150px" ForeColor="Black" ></asp:DropDownList>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Role">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRole" runat="server" Text='<%# Bind("ApprovalRoll") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <%--<asp:TextBox ID="txtRoleEdit" runat="server" Width="150px" placeholder="Role"></asp:TextBox>--%>
                                                        <asp:DropDownList ID="ddlRoleEdit" runat="server"  Width="150px" ></asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <%--<asp:TextBox ID="txtRole" runat="server" Width="150px" ForeColor="Black" Font-Bold="true" placeholder="Role"></asp:TextBox>--%>
                                                    <asp:DropDownList ID="ddlRole" runat="server"  Width="150px" ForeColor="Black" ></asp:DropDownList>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Employee">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmployee" runat="server" Text='<%# Bind("BatchNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%--<EditItemTemplate>
                                                        <asp:TextBox ID="txtEmployeeEdit" runat="server" Width="150px" placeholder="Employee"></asp:TextBox>
                                                    </EditItemTemplate>--%>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtEmployee" runat="server" Width="150px" ForeColor="Black" placeholder="Employee"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtStatusEdit" runat="server" Width="150px" placeholder="Status" ></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtStatus" runat="server" Width="150px"  ForeColor="Black" placeholder="Status" ></asp:TextBox>
                                                    <%--<asp:DropDownList ID="ddlStatus" runat="server" Width="80px" ></asp:DropDownList>--%><br/>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Remove" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="BtnRemove_WFPolicy" runat="server" CausesValidation="false"
                                                            ImageUrl="~/Images/Delete_16x16.png" onclick="BtnRemove_WFPolicy_Click"
                                                            ToolTip="Remove" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                 <%--OnClick="BtnRemove_WFPolicy_Click"--%>

                                            </Columns>

                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>

                            <table width="70%" style="border-collapse: separate; border-spacing: 5px;"
                                class="table table-condensed table-bordered table-hover">
                                <tr>
                                    <td class="auto-style8" colspan="3">
                                        <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td class="auto-style13">
                                         <asp:Button ID="btnSave1" runat="server"  Text="Save" Width="100%" OnClick="btnSave1_Click" />
                                        <asp:Button ID="btnCLear0" runat="server" Height="30px" Text="Clear" Width="100%" OnClick="btnClear0_Click" />
                                        <%--<asp:Button ID="btnPrint" runat="server" Height="30px" Text="Print" Width="100%" OnClick="btnPrint_Click" />--%>

                                    </td>
                                </tr>
                            </table>
                        </fieldset>

                    </div>
                
            </div>

        </ContentTemplate>

    </asp:updatepanel>

</asp:Content>
