<%@ Page Title="" Language="C#" Theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="OnlinePartsEnquiry.aspx.cs" Inherits="DXBMS.Modules.SpareParts.Forms.OnlinePartsEnquiry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">

        <ContentTemplate>
            <div class="container">
                
                        <fieldset>
                            <legend>Delivery Challan </legend>

                            <div >


                                <table width="100%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">

                                    <tr>
                                        <td >Part No <span style="color:red; font-weight:bold;">*</span></td>
                                        <td >
                                            <asp:TextBox ID="txtPartNo" runat="server" />
                                        </td>
                                        <td >Description</td>
                                        <td >
                                            <asp:TextBox ID="txtPartDesc" runat="server" ></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td width="11%">Source</td>
                                        <td>
                                            <asp:TextBox ID="txtSource" runat="server"></asp:TextBox>
                                        </td>
                                        <td >Quantity<span style="color:red; font-weight:bold;">*</span></td>
                                        <td >
                                            <asp:TextBox ID="txtQty" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    
                                </table>

                            </div>

                            <!---- FOR GRIDVIEW -->

                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:GridView runat="server" ShowHeaderWhenEmpty="true" Width="100%" ID="gv_DC" AutoGenerateColumns="false"
                                            CssClass="mGrid">
                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />

                                            <Columns>                                               

                                                <asp:TemplateField HeaderText="Dealer">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPartItemNo" runat="server" Text='<%# Bind("Dealer") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Part No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPartItemName" runat="server" Text='<%# Bind("PartNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />

                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Part Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("PartDescription") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Source">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPOQuantity" runat="server" Text='<%# Bind("Source") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Quantity">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBalQuantity" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>

                                        </asp:GridView>
                                    </td>
                                </tr>

                            </table>

                            <table width="100%" style="border-collapse: separate; border-spacing: 5px; margin-top: 10px;"
                                class="table table-condensed table-bordered table-hover">
                                <tr>
                                    <td width="60%">
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>
                                    </td>
                                    <td >
                                        <asp:Button ID="btnCheck" runat="server" Text="Check"  OnClick="btnCheck_Click" Width="100px"
                                             OnClientClick="this.disabled = true; this.value = 'Please Wait';"
                                        UseSubmitBehavior="false"/>
                                        <%--<asp:Button ID="btnDelete" runat="server" Text="Delete"  OnClick="btnDelete_Click"/>
                                        <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />  --%>                                     
                                        
                                    </td>

                                </tr>
                            </table>


                        </fieldset>
                    
            </div>

            </table>

        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>
