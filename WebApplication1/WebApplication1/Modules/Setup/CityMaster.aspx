<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="CityMaster.aspx.cs" Inherits="DXBMS.Modules.Setup.CityMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">

     <asp:UpdatePanel ID="UpdatePanel1" runat="server">

        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div class="col-lg-12">
                        <fieldset>
                            <legend> City </legend>
                                <table width="70%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">                                   
                                    <tr>
                                        <td class="auto-style1">State Code</td>
                                        <td class="auto-style5">
                                            <asp:DropDownList ID="ddState" runat="server" Width="100%">

                                            </asp:DropDownList>

                                        </td>
                                        <td class="auto-style1">City Code</td>
                                        <td class="auto-style5">
                                            <asp:TextBox ID="txtCityCode" runat="server" Height="21px" MaxLength="3" Width="300px"></asp:TextBox>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td class="auto-style1">City Description</td>
                                        <td class="auto-style5">
                                            <%--<asp:TextBox ID="txtStateCode" runat="server" Height="21px" MaxLength="50" Width="300px"></asp:TextBox></td>--%>
                                            <asp:TextBox ID="txtCityDescription" runat="server" Height="21px" MaxLength="50" Width="300px"></asp:TextBox>
                                             </td>
                                        <td class="auto-style4">
                                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                        </td>
                                        <td class="auto-style1">
                                            <asp:Button ID="btnInsert" runat="server" Height="30px" Text="Insert" Width="200px" OnClick="btnInsert_Click" />
                                        </td>
                                    </tr>                                    

                                </table>
                            <!---- FOR GRIDVIEW -->
                                <table width="100%">
                                <tr>
                                    <td>
                                        <asp:GridView runat="server" ShowHeaderWhenEmpty="true" Width="100%" ID="gv_city" AutoGenerateColumns="false" 
                                            OnDataBound="gv_city_DataBound"  
                                            OnRowDataBound="gv_city_RowDataBound"
                                            OnRowEditing="gv_city_RowEditing" 
                                            OnRowCancelingEdit="gv_city_RowCancelingEdit"
                                            OnRowUpdating="gv_city_RowUpdating" 
                                            OnPageIndexChanging="gv_city_SelectedIndexChanged" 
                                            CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                AllowPaging="True">

                                            <%--OnPageIndexChanging="gv_city_SelectedIndexChanged" 
                                                AllowPaging="True" --%>
                                            
                                           <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />

                                            <Columns>
                                                <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                    UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>
                                                <asp:TemplateField HeaderText="City Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCityCode" runat="server" Text='<%# Bind("CityCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="City Description">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtCityDescription" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCityDesc" runat="server" Text='<%# Bind("CityDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="State Code">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtStateCode" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStateCode" runat="server" Text='<%# Bind("StateCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remove">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="BtnRemove_City" runat="server" ImageUrl="~/Images/Delete_16x16.png" OnClick="BtnRemove_City_Click" ToolTip="Remove" />
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
