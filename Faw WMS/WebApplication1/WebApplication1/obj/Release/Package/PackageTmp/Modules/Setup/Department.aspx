<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="Department.aspx.cs" Inherits="DXBMS.Modules.Setup.Department" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>    

<asp:Content ID="Content3" ContentPlaceHolderID="Content" Runat="Server">

    
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
    <asp:UpdatePanel runat="server">

        <ContentTemplate>
            <div class="container">
                
                        <fieldset>
                            <legend>Department </legend>

                            <div class="table-responsive">

                                <table width="70%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">
                                    <tr>
                                        
                                        <td class="auto-style1">Department Code</td>
                                        <td class="auto-style5">
                                            <asp:TextBox ID="txtDepartmentCode" runat="server" Height="21px" MaxLength="3" Width="200px" ReadOnly="true"></asp:TextBox><span>   (Auto Number)</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style1">Department Description<span style="color:red;">*</span></td>
                                        <td class="auto-style5">
                                           <asp:TextBox ID="txtDepartmentDesc" runat="server" Height="21px" MaxLength="50" Width="450px"></asp:TextBox>
                                        </td>   
                                    </tr>

                                    <tr>
                                        <td class="auto-style1" >
                                            <asp:Button ID="btnInsert" runat="server"  Text="Insert" OnClick="btnInsert_Click"   />
                                            
                                        </td>
                                        <td class=" auto-style4">
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                        
                                    </tr>

                                </table>

                            </div>

                                        
                                        <asp:GridView runat="server" ShowHeaderWhenEmpty="true" Width="100%" ID="gv_Dept" AutoGenerateColumns="false"
                                            AllowPaging="True"
                                            OnDataBound="gv_Dept_DataBound"
                                            OnRowDataBound="gv_Dept_RowDataBound"
                                            OnRowEditing="gv_Dept_RowEditing"
                                            OnRowCancelingEdit="gv_Dept_RowCancelingEdit"
                                            OnRowUpdating="gv_Dept_RowUpdating"
                                            OnSelectedIndexChanged="gv_Dept_SelectedIndexChanged"
                                            OnPageIndexChanging="gv_Dept_SelectedIndexChanged" 
                                            CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">


                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />

                                            <Columns>
                                                <asp:CommandField ShowSelectButton="True"></asp:CommandField>
                                                <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                    UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>
                                                <asp:TemplateField HeaderText="Department Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDepartmentCode" runat="server" Text='<%# Bind("DeptCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Department Description">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtDeptDesc" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDeptDesc" runat="server" Text='<%# Bind("DeptDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remove">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="BtnRemove_City" runat="server" ImageUrl="~/Images/Delete_16x16.png" OnClientClick="javascript:
                                                            return confirm('Are you sure you want to delete this data?');" OnClick="BtnRemove_City_Click" ToolTip="Remove" />
                                                                                                           
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
