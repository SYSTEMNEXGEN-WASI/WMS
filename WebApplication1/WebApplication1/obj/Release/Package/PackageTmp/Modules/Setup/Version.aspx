<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" Theme="Theme1" AutoEventWireup="true" CodeBehind="Version.aspx.cs" Inherits="DXBMS.Modules.Setup.Version" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        .required:before{
  content:"*";
  font-weight:bold;
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

        <ContentTemplate>
            <div class="container">
                
                        <fieldset>
                            <legend>Version </legend>

                            <div class="table-responsive">

                                <table width="70%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">
                                    <tr>
                                        
                                        <td class="auto-style1" >Version Code<asp:ImageButton runat="server" ID="ImageButtonVersion" ImageUrl="~/Images/LookupNew.png" OnClick="ImageButtonVersion_Click" Width="16px" /></td>
                                        <td class="auto-style5">
                                             <asp:TextBox ID="txtVersion" runat="server"  Width="100%"  MaxLength="3" Height="21px"  ></asp:TextBox>
                                            <%--<asp:DropDownList ID="ddlBrandCode" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlBrandCode_SelectedIndexChanged" ></asp:DropDownList> <%----%>
                                            <%--<asp:TextBox ID="txtBrandCode" runat="server" Height="21px" MaxLength="3" Width="200px"></asp:TextBox>--%>
                                        </td>
                                        

                                    </tr>
                                    <tr>
                                        <td class="auto-style1">Version Description <span style="color:red;">*</span></td>
                                        <td class="auto-style5">
                                           
                                            <asp:TextBox ID="txtBrandDesc" runat="server"  Width="100%"  MaxLength="50" Height="21px" ></asp:TextBox>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td >
                                            <asp:Button ID="btnInsert" runat="server"  Text="Insert" OnClick="btnInsert_Click"  />
                                            
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>

                                </table>

                            </div>

                            <!---- FOR GRIDVIEW -->

                            <table width="100%">
                                <tr>
                                    <td>
                                        
                                        <asp:GridView runat="server" ShowHeaderWhenEmpty="true" Width="100%" ID="gv_Brand" AutoGenerateColumns="false"
                                            AllowPaging="True"
                                            OnDataBound="gv_Brand_DataBound"
                                            OnRowDataBound="gv_Brand_RowDataBound"                                           
                                             OnRowEditing="gv_Brand_RowEditing"
                                            OnRowCancelingEdit="gv_Brand_RowCancelingEdit"
                                            OnRowUpdating="gv_Brand_RowUpdating"
                                            OnSelectedIndexChanged="gv_Brand_SelectedIndexChanged"
                                            OnPageIndexChanging="gv_Brand_SelectedIndexChanged"
                                            CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">


                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />

                                            <Columns>
                                                <asp:CommandField ShowSelectButton="True"></asp:CommandField>
                                                <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                    UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>
                                                <asp:TemplateField HeaderText="Version Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBrandCode" runat="server" Text='<%# Bind("VersionCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Version Description">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtBrandDesc" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBrandDesc" runat="server" Text='<%# Bind("VersionDesc") %>'></asp:Label>
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



