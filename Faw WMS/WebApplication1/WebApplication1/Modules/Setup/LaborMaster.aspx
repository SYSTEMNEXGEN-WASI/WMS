<%@ Page Title="" Language="C#" Theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="LaborMaster.aspx.cs" Inherits="DXBMS.Modules.Setup.LaborMaster" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<script src="../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../js/chosen.jquery.min.js" type="text/javascript"></script>
    <link href="../../Content/Style.css" rel="stylesheet" />
 <style>
     .foot{
    text-align: center;
    background: #424242 repeat-x top;
}
 </style>
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="container">
            

            <%--<div class="Heading">
            Labor Master
            </div>--%>
        
         <fieldset>
             <legend>Labour Detail</legend>
        <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
            <tr>
                <td width="10%">Job Code</td>
                <td>
                    <asp:DropDownList ID="ddljob_code" runat="server" Width="250px" OnSelectedIndexChanged="ddljob_code_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <asp:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddljob_code" QueryPattern="Contains"></asp:ListSearchExtender>
                </td>
                <td width="10%">Category Code</td>
                <td>
                    <asp:TextBox ID="txtJobCategory" runat="server" ReadOnly="True"></asp:TextBox>
                    <asp:TextBox ID="txtJobCatDesc" runat="server" ReadOnly="true"></asp:TextBox>
                    <%--<asp:DropDownList ID="ddljob_category" runat="server" Width="250px" ></asp:DropDownList>--%>
                    <%--<asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddljob_category" QueryPattern="Contains"></asp:ListSearchExtender>--%>
                </td>
            </tr>
            <tr>
                <td>Job Description</td>
                <td>
                    <asp:TextBox ID="txtjob_desc" runat="server" Width="250px" ReadOnly="True"></asp:TextBox>
                    
                </td>
                 <td>Labor A/C</td>
                <td>
                     <asp:TextBox ID="txtLaborAC" runat="server" ReadOnly="True"></asp:TextBox>
                  
                </td>
            </tr>
        </table>
        </fieldset>
        
        <table width="100%">
            <tr>
                <td width="100%">
                <asp:GridView ID="gv_labor" runat="server" ShowFooter="true" ShowHeaderWhenEmpty="true" 
                    AutoGenerateColumns="false" Width="100%" OnRowDataBound="gv_labor_RowDataBound" OnDataBound="gv_labor_DataBound"
                    OnRowEditing="gv_labor_RowEditing" OnRowCancelingEdit="gv_labor_RowCancelingEdit" OnRowUpdating="gv_labor_RowUpdating" 
                    CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">

                    <Columns>
                               <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image" 
                           UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>
                   </Columns>

                    <HeaderStyle CssClass="GridHeader" />
                    <FooterStyle CssClass="foot" />
                                
                    <Columns>
                        
                        <asp:TemplateField HeaderText="SNo">
                            <ItemTemplate>
                                <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex +1 %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:ImageButton ID="BtnAdd_Labor" runat="server" CausesValidation="false"
                                ImageUrl="~/Images/Grid_Update.png" OnClick="BtnAdd_Labor_Click" ToolTip="Add" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DefJobCode" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblJobCode" runat="server" Text='<%# Bind("DefJobCode") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Made">
                            <ItemTemplate>
                                <asp:Label ID="lblMade" runat="server" Text='<%# Bind("Made") %>' ></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlbrand_code" runat="server" Width="90%" OnSelectedIndexChanged="ddlbrand_code_Edit" AutoPostBack="true" ></asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddlbrand_code" runat="server" Width="90%" OnSelectedIndexChanged="ddlbrand_code_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product">
                            <ItemTemplate>
                                <asp:Label ID="lblProduct" runat="server" Text='<%# Bind("Product") %>' ></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                               <%-- <asp:DropDownList ID="ddlprod_code" runat="server"></asp:DropDownList>--%>
                                <asp:DropDownList id="ddlprod_code" runat="server" Width="90%" OnSelectedIndexChanged="ddlprod_code_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddlprod_code" runat="server" Width="90%" OnSelectedIndexChanged="ddlprod_code_Footer" AutoPostBack="true"></asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Version">
                            <ItemTemplate>
                                <asp:Label ID="lblVersion" runat="server" Text='<%# Bind("Version") %>' ></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                               <%-- <asp:DropDownList ID="ddlprod_code" runat="server"></asp:DropDownList>--%>
                                <asp:DropDownList id="ddlVersion_code" runat="server" Width="90%"></asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddlVersion_code" runat="server" Width="90%"></asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Service Tax">
                            <ItemStyle Width="10%"/>
                            <ItemTemplate>
                                <asp:Label ID="lblTaxAmt" runat="server" Text='<%# Bind("ServiceTax") %>' ></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtTaxAmt" runat="server" Width="90%" MaxLength="5" placeholder="Service Tax"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtTaxAmt" runat="server" MaxLength="5" Width="90%" placeholder="Enter Tax here.."></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemStyle Width="10%" />
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount") %>' ></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAmount" runat="server" Width="90%" MaxLength="5" placeholder="Amount"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtAmount" runat="server" MaxLength="5" Width="90%" placeholder="Enter Amount here.."></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Standard Time">
                            <ItemStyle Width="10%"/>
                         <ItemTemplate>
                                <asp:Label ID="lblStnTime" runat="server" Text='<%# Bind("StandardTime") %>' ></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtStnTime" runat="server" Width="90%" MaxLength="4" placeholder="StandardTime"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtStnTime" runat="server" MaxLength="4" Width="90%" placeholder="Enter Minutes here.."></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Remove" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate >
                                <asp:ImageButton ID="BtnRemove_Labor" runat="server" CausesValidation="false"
                                ImageUrl="~/Images/Delete_16x16.png" onclick="BtnRemove_Labor_Click"
                                ToolTip="Remove" />
                            </ItemTemplate>
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
                        <asp:Button ID="btnSave1" runat="server"  Text="Save" Width="100%" OnClick="btnSave1_Click" Visible="false"  />
                        <asp:Button ID="btnDelete0" runat="server" Text="Delete" CausesValidation="false" OnClick="btnDelete0_Click" />
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
