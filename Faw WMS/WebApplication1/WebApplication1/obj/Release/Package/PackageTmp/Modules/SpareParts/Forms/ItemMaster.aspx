<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemMaster.aspx.cs" Inherits="DXBMS.Modules.SpareParts.Forms.ItemMaster" MasterPageFile="~/Root.master" Theme="Theme1"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">

    <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>    
        <div class="container">
    <div class="Heading">
        Item Master
    </div>
    <fieldset>
    <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
        <tr>
            <td style="width:11%;">
                <span>Item Code</span>
                <asp:ImageButton runat="server" CausesValidation="false" ID="imgLookup" ImageUrl="~/Images/LookupNew.png"  OnClick="btnLookup_Click" />
            </td>
            <td >
                <asp:TextBox ID="txtItemCode" runat="server" ReadOnly="true"></asp:TextBox>  
                <%--<asp:DropDownList ID="ddlitemcode" runat="server" width="60%" OnSelectedIndexChanged="ddlitemcode_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlitemcode" QueryPattern="Contains"></asp:ListSearchExtender>--%>
            </td>
            <td width="10%">
                Part Item No.</td>
            <td>
                <asp:TextBox ID="txtpart_itemno" runat="server" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtpart_itemno" ErrorMessage="Item No is Required" 
                    Font-Bold="True" ForeColor="Red" Text="* "></asp:RequiredFieldValidator>
            </td>
            <td>
                Part Description
            </td>
            <td width="29%">
                <asp:TextBox ID="txtpart_desc" runat="server" MaxLength="50" Width="200px" ></asp:TextBox>
                <asp:RequiredFieldValidator ForeColor="Red" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Item Description is Required" Text="* " ControlToValidate="txtpart_desc" Font-Bold="True"></asp:RequiredFieldValidator>
            </td>
            
        </tr>
       
    </table>
        </fieldset>
    <fieldset class="general">
            <legend>General</legend>
            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                <tr>
                    <td width="10%">
                        Item Category
                    </td>
                    <td width="40%">
                        <asp:DropDownList ID="ddlitemcatcode" runat="server" Width="200px"></asp:DropDownList>
                        <asp:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddlitemcatcode" QueryPattern="Contains"></asp:ListSearchExtender>
                    </td>
                    <td width="10%">
                        Product Code</td>
                    <td> 
                        <asp:TextBox ID="txtproduct_code" runat="server" Width="80px" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Unit</td>
                    <td>
                        <asp:DropDownList ID="ddlunitcode" runat="server" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Pack Size</td>
                    <td>
                        <asp:TextBox ID="txtpack_size" runat="server" MaxLength="6" Width="80px" style=" text-align:right;" ></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                            FilterType="Numbers" TargetControlID="txtpack_size" />
                    </td>
                    
                </tr>
                <tr>
                    <td>
                        Part Type</td>
                    <td>
                        <asp:DropDownList ID="ddlpart_type" runat="server" AutoPostBack="true" Width="200px" 
                            OnSelectedIndexChanged="ddlpart_type_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td> 
                        Liter
                    </td>
                    <td>
                        <asp:TextBox ID="txtliter" runat="server" MaxLength="6" Width="80px" style=" text-align:right;"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtliter" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                     </td>
                </tr>
                <tr>
                    <td>
                        Accessories
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlaccess" runat="server" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    
                    <td >
                        Source
                    </td>
                    <td>
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal" Width="100%"  RepeatLayout="Table">
                            <asp:ListItem Text="Local" Value="Local" ></asp:ListItem>
                            <asp:ListItem Text="C.K.D" Value="C.K.D"></asp:ListItem>
                            <asp:ListItem Text="Imported" Value="Imported"></asp:ListItem>
                            <asp:ListItem Text="Market" Value="Market"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td colspan="2">
                        <table>
                            <tbody>
                                <tr>
                                    <td style="padding-left: 15px;" ><asp:CheckBox  ID="cbsaleItem" runat="server" Text="Sale Item"/></td>
                               
                                    <td style="padding-left: 15px;"><asp:CheckBox  ID="cbinventoryItem" runat="server" Text="Inventory Item" /></td>
                                  
                                    <td style="padding-left: 15px;"><asp:CheckBox  ID="cbactive" runat="server" Text="Active" /></td>
                                </tr>
                            </tbody>
                        </table>             
                     </td>
                </tr>
            </table>
        </fieldset>
    <fieldset class="inventory">
        <legend>Inventory</legend>
        <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
            <tr>
                <td width="10%">
                    Min Level
                </td>
                <td>
                    <asp:TextBox ID="txtmin_level" runat="server" MaxLength="3" 
                        ontextchanged="txtmin_level_TextChanged" style=" text-align:right;"></asp:TextBox>
                    <asp:RequiredFieldValidator ForeColor="Red" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Minimum level is Required" Text="* " ControlToValidate="txtmin_level"  Font-Bold="True"></asp:RequiredFieldValidator>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtmin_level" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                </td>
                <td width="12%">
                    Reorder Level
                </td>
                 <td>   
                    <asp:TextBox ID="txtreorder_level" runat="server" MaxLength="3" Width="175px" style=" text-align:right;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Reorder Level is Required" Text="* " ControlToValidate="txtreorder_level" Font-Bold="True" ForeColor="Red"></asp:RequiredFieldValidator>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtreorder_level" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td>
                    WAR
                </td>
                <td>
                    <asp:TextBox ID="txtwar" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td>
                    Default Location
                </td>
                <td>
                    <asp:DropDownList ID="ddllocation" runat="server"></asp:DropDownList>
                    <asp:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddllocation" QueryPattern="Contains"></asp:ListSearchExtender>
                </td>
            </tr>
            <tr>
                <td colspan="4" >
                    <asp:GridView ID="gridItemView" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeaderWhenEmpty="true" ShowFooter="true">
                    <HeaderStyle CssClass="GridHeader" />
                <FooterStyle CssClass="GridFooter" />
                        <Columns>
                            <asp:BoundField DataField="PartsTransLocCode" HeaderText="Location Code" HeaderStyle-Width="130px" />
                            <asp:BoundField DataField="PartsTransLocDesc" HeaderText="Location Name" />
                            <asp:BoundField DataField="ItemQty" HeaderText="Quantity" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </fieldset>
    <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
    <tr>
    <td width="50%">
    <fieldset >
        <legend>Sales</legend>
        <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
            <tr>
                <td width="20%">
                    Sales Amt
                 </td>
                <td width="27%">   
                    <asp:TextBox ID="txtsales_amt" runat="server" Width="80%" MaxLength="8" style=" text-align:right;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Sales Amount is Required" Text="* " ControlToValidate="txtsales_amt"  Font-Bold="True" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <td width="25%">
                    Promotion Prod
                 </td>
                <td >   
                    <asp:TextBox ID="txtpromprod_sales" runat="server" Width="70%" MaxLength="5" style=" text-align:right;"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtpromprod_sales" FilterType="Numbers"  ></asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td>
                    Revenue A/C
                </td>
                <td>    
                    <asp:DropDownList ID="ddlrevenue" runat="server" Width="80%"></asp:DropDownList>
                </td>
                <td>
                    GST %
                </td>
                <td>
                    <asp:TextBox ID="txtgst_sales" runat="server" MaxLength="5" Width="70%" style=" text-align:right;"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtgst_sales" FilterType="Custom, Numbers"  
                                        ValidChars="."></asp:FilteredTextBoxExtender>

                </td>
            </tr>
            <tr>
                <td>
                    Stock Out A/C
                 </td>
                <td>   
                    <asp:DropDownList ID="ddlstockout" runat="server" Width="80%"></asp:DropDownList>
                </td>
                <td>
                    Further Tax %
                 </td>
                <td>   
                    <asp:TextBox ID="txtfurther_sales" runat="server" MaxLength="5" Width="70%" style=" text-align:right;"></asp:TextBox>
                     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtfurther_sales" FilterType="Custom, Numbers"  
                                        ValidChars="."></asp:FilteredTextBoxExtender>

                </td>
            </tr>
            <tr>
                <td>
                    Extra Tax %
                </td>
                <td >
                    <asp:TextBox ID="txtextratax_sales" runat="server" MaxLength="5" Width="80%" style=" text-align:right;"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtextratax_sales" FilterType="Custom, Numbers"  
                                        ValidChars="."></asp:FilteredTextBoxExtender>
                </td>
            </tr>
        </table>
    </fieldset>
    </td>
    
    <td width="50%">
    <fieldset >
        <legend>Purchase</legend>
        <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
            <tr>
                <td width="23%">
                    Purchase Amt
                 </td>
                <td >   
                    <asp:TextBox ID="txtpurchase_amt" runat="server" Width="70%" MaxLength="8" style=" text-align:right;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Purchase Amount is Required" Text="* " ControlToValidate="txtpurchase_amt" Font-Bold="True" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <td width="23%">
                    Promotion Prod
                 </td>
                <td width="25%">   
                    <asp:TextBox ID="txtpromprod_pruchase" runat="server" Width="80%" MaxLength="5" style=" text-align:right;"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtpromprod_pruchase" FilterType="Numbers"  ></asp:FilteredTextBoxExtender>

                </td>
            </tr>
            <tr>
                <td>
                    COGS A/C
                </td>
                <td>    
                    <asp:DropDownList ID="ddlcogs" runat="server" Width="70%"></asp:DropDownList>
                </td>
                <td>
                    GST %
                </td>
                <td>    
                    <asp:TextBox ID="txtgst_purchase" runat="server" MaxLength="5" Width="80%" style=" text-align:right;"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtgst_purchase"  FilterType="Custom, Numbers"  
                                        ValidChars="." ></asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td>
                    Stock in A/C
                 </td>
                <td>   
                    <asp:DropDownList ID="ddlstockin" runat="server" Width="70%"></asp:DropDownList>
                </td>
                <td>
                    Further Tax %
                 </td>
                <td>   
                    <asp:TextBox ID="txtfurther_purchase" runat="server" MaxLength="5" Width="80%"  style=" text-align:right;"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtfurther_purchase" FilterType="Custom, Numbers"  
                                        ValidChars="."></asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td>
                    Extra Tax %
                </td>
                <td > 
                    <asp:TextBox ID="txtextratax_purchase" runat="server" MaxLength="5" Width="70%" style=" text-align:right;"></asp:TextBox>
                   <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtextratax_purchase" FilterType="Custom, Numbers"  
                                        ValidChars="."></asp:FilteredTextBoxExtender>
                </td>
            </tr>
        </table>
    </fieldset>
    </td>
    </tr>
    </table>
    <table width="100%" style="margin-bottom:10px;">
        <tr>
         <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*) Sterik Feilds Are Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
            <td style="padding-left: 25px;">
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" ForeColor="Red" ShowMessageBox ="true" runat="server" /> 
            </td>
            <td style="text-align:center;">
                 
                <asp:Button ID="btn_save" runat="server" Visible="false" Text="Save" Width="100px" CausesValidation="True" OnClick="btn_save_Click" />
                <asp:Button ID="btn_delete" runat="server" Visible="false" Text="Delete" Width="100px"  CausesValidation="False" OnClick="btn_delete_Click" />
                <asp:Button ID="btn_clear" runat="server" Text="Clear" Width="100px" CausesValidation="false" OnClick="btn_clear_Click" />             
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
</html>
--%>