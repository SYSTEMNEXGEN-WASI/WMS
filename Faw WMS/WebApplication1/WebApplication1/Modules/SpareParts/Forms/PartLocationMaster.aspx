<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartLocationMaster.aspx.cs" Inherits="DXBMS.Modules.SpareParts.Forms.PartLocationMaster" 
MasterPageFile="~/Root.master" Theme="Theme1" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">


<script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
<asp:UpdatePanel ID="UpdPanel1" runat="server">
        <ContentTemplate>
        <div class="container">
    
    <%--<div class="Heading">
    Part Locations Category
    </div>--%>
            <fieldset >
                <legend>Part Locations</legend>
                <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                    <tr>
                        <td style="width: 14%">
                            <asp:Label ID="Label3" runat="server" Text="Part Location Code*"></asp:Label>
                        </td>
                        <td style="width: 25%">
                            <asp:DropDownList ID="ddlLocCode" runat="server" AutoPostBack="True" Width="100%" 
                                onselectedindexchanged="ddlLocCode_SelectedIndexChanged">
                            </asp:DropDownList>
                            
                        </td>  
                            
                        <td style="width: 14%">
                            <asp:Label ID="Label4" runat="server" Text="Part Location Desc.*"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="txtPartLocationDesc" runat="server" 
                                ontextchanged="txtPartLocationDesc_TextChanged" Width="200px"></asp:TextBox>
                        </td>
            
                    
                        <td align="right" >
                            <asp:TextBox ID="txtPartLocationCode" runat="server" MaxLength="2" 
                                Visible="false" Width="40px"></asp:TextBox>
                            <asp:Button ID="BtnSavePartLocation" runat="server" 
                                OnClick="BtnSavePartLocation_Click" 
                                OnClientClick="this.disabled = true; this.value = 'Please Wait';" Text="Save" 
                                UseSubmitBehavior="false" />
                            <asp:Button ID="BtnDeletePartLocation" runat="server" 
                                onclick="BtnDeletePartLocation_Click" Text="Delete" />
                        </td>
                    </tr>
    </table>
            </fieldset>
    
    <%-- </td>
        </tr>
        <tr>
            <td>--%>
    <%--<table width="100%">
        <tr>
            <td colspan="5">
                <asp:Label ID="Label6" runat="server" Text="Part Sub Locations" SkinID="Heading"></asp:Label>
            </td>
        </tr>
    </table>--%>
    <fieldset >
                    <legend >Part Sub Locations</legend>
    <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
       
        <tr>
            <td style="width:17%">Sub Location</td>
            <td style="width: 30%">
                <asp:DropDownList ID="ddlSubLocCode" runat="server" AutoPostBack="True" width="100%"
                    onselectedindexchanged="ddlSubLocCode_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
           
            <td style="width:17%">
                <asp:Label ID="Label7" runat="server" Text="Sub Location Code*"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSubLocationCode" runat="server" MaxLength="2" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td >
                <asp:Label ID="Label8" runat="server" Text="Sub Location Desc.*"></asp:Label>
            </td>
            <td >
                <asp:TextBox ID="txtSubLocationDesc" runat="server" Width="100%"></asp:TextBox>
            </td>
           
            <td colspan="2" align="right">
                <asp:Button ID="BtnPartSubLocations" runat="server" Text="Save" OnClick="BtnPartSubLocations_Click" />
                &nbsp;<asp:Button ID="BtnDeletePartSubLocations" runat="server" Text="Delete" 
                    onclick="BtnDeletePartSubLocations_Click" />
            </td>
        </tr>
       </table>
                <asp:GridView ID="dtg_PartLocation" runat="server" AutoGenerateColumns="False" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <asp:LinkButton ID="BtnSelect" runat="server" OnClick="BtnSelect_Click">Select</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rack Code">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("PartsSubLocCode") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="LbPartsSubLocCode" runat="server" Text='<%# Bind("PartsSubLocCode") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("PartsSubLocDesc") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="LbPartsSubLocDesc" runat="server" Text='<%# Bind("PartsSubLocDesc") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="300px" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle HorizontalAlign="Left" />
                    <RowStyle HorizontalAlign="Left" />
                </asp:GridView>
           </fieldset>
    <%--</td> </tr>
    <tr>
        <td>--%>
    <fieldset style="width:100%; float: left;">
                    <legend>Part Transition Locations</legend>
            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
               <%-- <tr>
                    <td colspan="5">
                        <asp:Label ID="Label5" runat="server" Text="Part Transition Locations" SkinID="Heading"></asp:Label>
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="1" style="width: 17%" >
                        <asp:Label ID="Label9" runat="server" Text="Transition Location Code*" 
                            Width="140px"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAutoPartLocCode" runat="server" MaxLength="2" 
                            ReadOnly="True" Width="66px"></asp:TextBox>
                        <asp:TextBox ID="txtAutoPartsSubLocCode" runat="server" MaxLength="2" 
                            ReadOnly="True" Width="66px"></asp:TextBox>
                        <asp:TextBox ID="txtPartsTransLocCode" runat="server" MaxLength="3" 
                            ReadOnly="True" Width="66px"></asp:TextBox>
                    </td>                
                    <td>
                        <asp:Label ID="Label10" runat="server" Text="Sub Location Desc.*"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPartsTransLocDesc" runat="server" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                   <td align="right" colspan="4">
                        <asp:Button ID="BtnTransLca" runat="server" OnClick="BtnTransLca_Click" 
                            Text="Save" />
                        <asp:Button ID="BtnDelete" runat="server" OnClick="BtnDelete_Click" 
                            Text="Delete" />
                        <asp:Button ID="BtnClear" runat="server" onclick="BtnClear_Click" 
                            Text="Clear" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <%--<td colspan="2" align="right">
                        <asp:Button ID="BtnTransLca" runat="server" OnClick="BtnTransLca_Click" 
                            Text="Save" />
                        <asp:Button ID="BtnDelete" runat="server" OnClick="BtnDelete_Click" 
                            Text="Delete" />
                        <asp:Button ID="BtnClear" runat="server" onclick="BtnClear_Click" 
                            Text="Clear" />
                    </td>--%>
                    <td colspan="4">
                        <asp:GridView ID="GVTransLocation" runat="server" AutoGenerateColumns="False" 
                            Width="100%" ShowFooter="true">
                            <HeaderStyle CssClass="GridHeader" />
                <FooterStyle CssClass="GridFooter" />
                            <Columns>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnTransSelect" runat="server" 
                                            onclick="BtnTransSelect_Click">Select</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PartsTransLocCode" HeaderText="Shelf Code" HeaderStyle-Width="100%" />
                                <asp:BoundField DataField="PartsTransLocDesc" HeaderText="Description" />
                                                                
                            </Columns>
                            
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            </fieldset>
            <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
         <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*) Sterik Feilds Are Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
            <td colspan="3" style=" width:50%">
                <asp:Label ID="LbErr" runat="server"  Text=""></asp:Label>
            </td>
            <td style="text-align:center;">
                <asp:Button ID="Button1" runat="server" Width="100px" onclick="Button1_Click" Text="Clear All" />
            </td>
        </tr>
        
    </table>
        </div>
            </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>--%>