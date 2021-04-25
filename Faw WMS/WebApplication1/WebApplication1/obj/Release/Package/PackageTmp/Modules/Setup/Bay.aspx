<%@ Page Title="" Language="C#" Theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="Bay.aspx.cs" Inherits="DXBMS.Modules.Setup.Bay" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

        <script src="../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../js/chosen.jquery.min.js" type="text/javascript"></script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container" >
                <div class="row">
                    <div class="col-lg-12">
                        <div class="Heading">Bay</div>
                        <fieldset>
                            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                                <tr>
                                    <td width="10%">Bays</td>
                                    <td width="40%"><asp:DropDownList ID="ddlBays" runat="server" AutoPostBack="true" width="60%"
                                            onselectedindexchanged="ddlBays_SelectedIndexChanged"></asp:DropDownList></td>                                    
                                </tr>
                                <tr>
                                    <td>Bay ID</td>
                                    <td><asp:TextBox ID="txtBayId" runat="server" ReadOnly="true" width="60%"></asp:TextBox></td>
                                    <td width="10%">Bay Description</td>
                                    <td width="40%">
                                        <asp:TextBox ID="txtBayDesc" runat="server" Width="60%"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" id="reqBayDesc" controltovalidate="txtBayDesc" />
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td>Shop</td>
                                    <td>
                                        <asp:DropDownList ID="ddlShop" runat="server" width="60%"></asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" id="reqShop" controltovalidate="ddlShop" />
                                    </td>
                                    <td>Active</td>
                                    <td>
                                        <asp:CheckBox id="chkActive" runat="server"/>                                        
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12">
                        <table width="100%">
                            <tr>
                                <td width="100%">
                                    <asp:GridView ID="gv_Bays" runat="server"  ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" Width="100%"                                    
                                            CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" AllowPaging="True"
                                            OnPageIndexChanging="gv_Bays_SelectedIndexChanged" >

                                        <HeaderStyle CssClass="GridHeader" />
                                        <FooterStyle CssClass="GridFooter" />

                                        <Columns>                          
                                                   <asp:TemplateField HeaderText="SNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex + 1%>"  ></asp:Label>
                                                    </ItemTemplate>
                                                       <ItemStyle HorizontalAlign="Center" />
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Bay ID" >
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblBayID" runat="server" Text='<%# Bind("BayID") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle HorizontalAlign="Center" />
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Bay Description">                                                       
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblBayDesc" runat="server" Text='<%# Bind("BayDesc") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle HorizontalAlign="Center" />
                                                   </asp:TemplateField>                             
                                                   <asp:TemplateField HeaderText="Shop">                                                       
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblProduct" runat="server" Text='<%# Bind("ShopDesc") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle HorizontalAlign="Center" />
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Active">                                                                                         
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblVersion" runat="server" Text='<%# Bind("Active") %>'></asp:Label>
                                                       </ItemTemplate>
                                                       <ItemStyle HorizontalAlign="Center" />
                                                   </asp:TemplateField>                                                   
                                                   <%--<asp:TemplateField HeaderText="Remove">
                                                       <ItemTemplate>
                                                           <asp:ImageButton ID="BtnRemove_Labor" runat="server" CausesValidation="false" ImageUrl="~/Images/Delete_16x16.png" onclick="BtnRemove_Labor_Click" ToolTip="Remove" />
                                                       </ItemTemplate>
                                                       <ItemStyle HorizontalAlign="Center" />
                                                   </asp:TemplateField>--%>
                                        </Columns>                   
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>   
                <div class="row">
                    <div class="col-lg-12">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblMsg" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#339933"></asp:Label>                                
                                </td>
                                <td align="center">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait';" UseSubmitBehavior="false"/>
                                    <%--OnClick="btnSave_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait';" UseSubmitBehavior="false"--%> 
                   
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" onclick="btnDelete_Click"
                                     OnClientClick="javascript: return confirm('Are you sure you want to Delete?');" /><%--onclick="btnDelete_Click"--%>
                    
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" onclick="btnClear_Click"/>  <%--onclick="btnClear_Click"--%>            
                               
                                </td>
                            </tr>                
                        </table>
                    </div>
                </div>
                          
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
