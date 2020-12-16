<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="Designation.aspx.cs" Inherits="DXBMS.Modules.Setup.Designation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <link href="../../Content/Style.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            width: 282px;
        }

        .auto-style4 {
            width: 336px;
        }

        .auto-style5 {
            width: 154px;
        }
    </style>

        <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
                <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
                <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
                <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
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

    <asp:UpdatePanel runat="server">

        <ContentTemplate>
            <div class="container">
                    
                        <fieldset>
                            <legend>Designation </legend>

                            <div>

                                <table width="70%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">
                                    <tr>
                                       
                                        <td width="15%">Designation Code</td>
                                        <td >
                                            <asp:TextBox ID="txtDesigCode" runat="server" Height="21px" MaxLength="3" Width="70%" ReadOnly="true"></asp:TextBox><span>   (Auto Number)</span>
                                        </td> 
                                          <td width="15%">Designation Description <span style="color:red;">*</span></td>
                                        <td width="35%">
                                            <%--<asp:TextBox ID="txtStateCode" runat="server" Height="21px" MaxLength="50" Width="300px"></asp:TextBox></td>--%>
                                            <asp:TextBox ID="txtDesigDesc" runat="server" Height="21px" MaxLength="50" Width="100%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>                                        
                                        <td >Department <span style="color:red;">*</span></td>
                                        <td class="auto-style9">
                                            <asp:DropDownList ID="ddlDeptCode" runat="server" AutoPostBack="True" Font-Names="monospace" Width="100%" >
                                            </asp:DropDownList>
<%--                                            OnSelectedIndexChanged="ddlDeptCode_SelectedIndexChanged"--%>
                                        </td>

                                        <td class=" auto-style4" colspan="2">
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td class="auto-style1" colspan="4">
                                            <asp:Button ID="btnInsert" runat="server"  Text="Insert" OnClick="btnInsert_Click"   />
                                            
                                        </td>
                                    </tr>

                                </table>

                            </div>

                            <!---- FOR GRIDVIEW -->

                            <table width="100%">
                                <tr>
                                    <td>
                                        
                                        <asp:GridView runat="server" ShowHeaderWhenEmpty="true" Width="100%" ID="gv_Desig" AutoGenerateColumns="false"
                                            AllowPaging="True" 
                                            OnDataBound="gv_Desig_DataBound"
                                            OnRowDataBound="gv_Desig_RowDataBound"
                                            OnRowEditing="gv_Desig_RowEditing"
                                            OnRowCancelingEdit="gv_Desig_RowCancelingEdit"
                                            OnRowUpdating="gv_Desig_RowUpdating"
                                            OnSelectedIndexChanged="gv_Desig_SelectedIndexChanged"
                                            OnPageIndexChanging="gv_Desig_SelectedIndexChanged"
                                            CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" >


                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />

                                            <Columns>
                                                <asp:CommandField ShowSelectButton="True"></asp:CommandField>
                                                <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                    UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>
                                                <asp:TemplateField HeaderText="Designation Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDesigCode" runat="server" Text='<%# Bind("DesigCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Department Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDeptCode" runat="server" Text='<%# Bind("DeptCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Department Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDeptDesc" runat="server" Text='<%# Bind("DeptDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Designation Description">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtDesigDesc" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDesigDesc" runat="server" Text='<%# Bind("DesigDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remove">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="BtnRemove_City" runat="server" ImageUrl="~/Images/Delete_16x16.png" OnClientClick="javascript:
                                                            return confirm('Are you sure you want to delete this data?');"  OnClick="BtnRemove_City_Click" ToolTip="Remove" />
                                                                                                           
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

        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>
