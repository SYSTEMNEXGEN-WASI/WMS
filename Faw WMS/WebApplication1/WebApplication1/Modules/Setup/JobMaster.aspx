<%@ Page Title="" Language="C#" theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="JobMaster.aspx.cs" Inherits="DXBMS.Modules.Setup.JobMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">


    <style type="text/css">
        
        .mGrid { 
    width: 100%; 
    background-color: #fff; 
    margin: 5px 0 10px 0; 
    border: solid 1px #525252; 
    border-collapse:collapse; 
}
.mGrid td { 
    padding: 2px; 
    border: solid 1px #c1c1c1; 
    color: #717171; 
}
.mGrid th { 
    padding: 4px 2px; 
    color: #fff; 
    background: #424242 repeat-x top; 
    border-left: solid 1px #525252; 
    font-size: 0.9em; 
}
.mGrid .alt { background: #fcfcfc repeat-x top; }
.mGrid .pgr { background: #424242 repeat-x top; }
.mGrid .pgr table { margin: 5px 0; }
.mGrid .pgr td { 
    border-width: 0; 
    padding: 0 6px; 
    border-left: solid 1px #666; 
    font-weight: bold; 
    color: #fff; 
    line-height: 12px; 
 }   
.mGrid .pgr a { color: #666; text-decoration: none; }
.mGrid .pgr a:hover { color: #000; text-decoration: none; }

</style>

<script src="../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../js/chosen.jquery.min.js" type="text/javascript"></script>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="container">
            

            <%--<div class="Heading">
            Labor Master
            </div>--%>
        
         <fieldset>
             <legend>Job Details</legend>
        <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
            <tr>
                <td width="10%">Job Code</td>
                <td>
                    <asp:DropDownList ID="ddljob_code" runat="server" Width="250px" OnSelectedIndexChanged="ddljob_code_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <asp:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddljob_code" QueryPattern="Contains"></asp:ListSearchExtender>
                </td>
                <td width="10%">Category Code</td>
                <td>
                    <asp:DropDownList ID="ddljob_category" runat="server" Width="250px" OnSelectedIndexChanged="ddljob_category_SelectedIndexChanged" AutoPostBack="True" ></asp:DropDownList>
                   <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddljob_category" QueryPattern="Contains"></asp:ListSearchExtender>
                </td>
            </tr>
            <tr>
                <td>Job Description</td>
                <td>
                    <asp:TextBox ID="txtjob_desc" runat="server" Width="250px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtjob_desc" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                 <td>Labor A/C</td>
                <td>
                    <asp:DropDownList ID="ddlacount_code" runat="server" Width="250px" ></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Standard Time</td>
                <td>
                    <asp:TextBox ID="txtStnTime" runat="server" MaxLength="4" Width="250px" style="text-align:right;"></asp:TextBox>(Avg. Minutes)
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom, Numbers" TargetControlID="txtStnTime">
                    </asp:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="txtStnTime" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <td>Select Bay No.</td>
                <td>
                    <asp:DropDownList ID="ddlBayNo" runat="server" Width="250px" ></asp:DropDownList>
                </td>
            </tr>
        </table>
        </fieldset>
        
        <table width="100%">
            <tr>
                <td width="100%">
                <asp:GridView ID="gv_labor" runat="server" ShowHeaderWhenEmpty="True" 
                    CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" AllowPaging="True"
                    AutoGenerateColumns="False" Width="100%" OnSelectedIndexChanged="gv_labor_SelectedIndexChanged"
                    OnPageIndexChanging="gv_labor_PageIndexChanging" >

                    <HeaderStyle CssClass="GridHeader" />
                    <FooterStyle CssClass="GridFooter" />

                    <Columns>
                        <asp:CommandField ShowSelectButton="True" />
                    </Columns>

                    <Columns>                          
                               <asp:TemplateField HeaderText="SNo">
                                   <ItemTemplate>
                                       <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex + 1%>"  ></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                        <asp:BoundField DataField="DefJobCode" HeaderText="DefJobCode" />
                               <%--<asp:TemplateField HeaderText="DefJobCode" >
                                   <ItemTemplate>
                                       <asp:Label ID="lblJobCode" runat="server" Text='<%# Bind("DefJobCode") %>'></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>--%>
                        <asp:BoundField DataField="DefJobDesc" HeaderText="JOB DESCRIPTION" />
                               <%--<asp:TemplateField HeaderText="JOB DESCRIPTION">
                                   <EditItemTemplate>
                                       <asp:DropDownList ID="ddlbrand_code" runat="server">
                                       </asp:DropDownList>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label ID="lblMade" runat="server" Text='<%# Bind("DefJobDesc") %>'></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>--%>
                         <asp:BoundField DataField="LaborAccount" HeaderText="LABOR ACCOUNT" />
                         <%-- <asp:TemplateField HeaderText="LABOR ACCOUNT">
                                   <EditItemTemplate>
                                       <asp:DropDownList ID="ddlprod_code" runat="server">
                                       </asp:DropDownList>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label ID="lblProduct" runat="server" Text='<%# Bind("LaborAccount") %>'></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>--%>
                          <asp:BoundField DataField="StandardTime" HeaderText="STANDARD TIME" />   
                          <%--<asp:TemplateField HeaderText="STANDARD TIME">
                                   <EditItemTemplate>
                                       <asp:DropDownList ID="ddlVersion_code" runat="server">
                                       </asp:DropDownList>
                                   </EditItemTemplate>                                   
                                   <ItemTemplate>
                                       <asp:Label ID="lblVersion" runat="server" Text='<%# Bind("StandardTime") %>'></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>--%>
                         <asp:BoundField DataField="BayCode" HeaderText="BAY NO." />  
                         <%--<asp:TemplateField HeaderText="BAY NO.">
                                   <EditItemTemplate>
                                       <asp:TextBox ID="txtTaxAmt" runat="server" MaxLength="5" placeholder="Service Tax" Width="100%"></asp:TextBox>
                                   </EditItemTemplate>                                   
                                   <ItemTemplate>
                                       <asp:Label ID="lblTaxAmt" runat="server" Text='<%# Bind("BayCode") %>'></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>--%>

                               <asp:TemplateField HeaderText="Remove">
                                   <ItemTemplate>
                                       <asp:ImageButton ID="BtnRemove_Labor" runat="server" CausesValidation="false" ImageUrl="~/Images/Delete_16x16.png" onclick="BtnRemove_Labor_Click" ToolTip="Remove" />
                                   </ItemTemplate>
                                   <ItemStyle HorizontalAlign="Center" />
                               </asp:TemplateField>
                   </Columns>

                   
                     </asp:GridView>
                </td>
            </tr>
        </table>

        <table width="100%" >
                <tr>
                    <td>
                        <asp:Label ID="lblMsg" runat="server" Text="Item Added Succesfully" ForeColor="Green" Font-Bold="true" ></asp:Label>
                    </td>
                    <td style="text-align:center;">
                        <asp:Button ID="btnSave1" runat="server"  Text="Save" Width="100%" Enabled="True" OnClick="btnSave1_Click" Visible="false"  />
                        <asp:Button ID="btnDelete0" runat="server" Text="Delete" CausesValidation="false" Enabled="True" OnClick="btnDelete0_Click" />
                        <asp:ConfirmButtonExtender ID="CBE" runat="server" ConfirmText="You want to delete?"
                            TargetControlID="btnDelete0">
                        </asp:ConfirmButtonExtender>
                        <asp:Button ID="btnClear0" runat="server" Text="Clear" Width="100%" OnClick="btnClear0_Click" CausesValidation="false" />
                        
                    </td>
                </tr>
            </table>


        </div>
        </ContentTemplate>
   </asp:UpdatePanel> 



</asp:Content>
