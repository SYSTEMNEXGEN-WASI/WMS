<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="PaymentMode.aspx.cs" Inherits="DXBMS.Modules.Setup.PaymentMode" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">

        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div class="col-lg-12">
                        <fieldset>
                            <legend> PaymentMode </legend>

                            <div class="table-responsive">


                                <table width="70%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">                                   
                                    <tr>
                                        <td class="auto-style1">PayMode Code</td>
                                        <td class="auto-style5">
                                            <%--<asp:DropDownList ID="ddState" runat="server" Width="100%">

                                            </asp:DropDownList>--%>
                                            <asp:TextBox ID="txtPayModeCode" runat="server" Height="21px" MaxLength="1" Width="300px"></asp:TextBox>

                                        </td>
                                        <td class="auto-style1">PayMode Description</td>
                                        <td class="auto-style5">
                                            <asp:TextBox ID="txtPayModeDesc" runat="server" Height="21px" MaxLength="50" Width="300px"></asp:TextBox>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td class="auto-style1">Version</td>
                                        <td class="auto-style5">
                                            <%--<asp:TextBox ID="txtStateCode" runat="server" Height="21px" MaxLength="50" Width="300px"></asp:TextBox></td>--%>
                                            
                                            <asp:TextBox ID="txtVersion" runat="server" Height="21px" MaxLength="8" Width="300px"></asp:TextBox>
                                             </td>
                                        <td class="auto-style4">PayMode Code PSMC</td>
                                        <td class="auto-style1">
                                            <asp:TextBox ID="txtPayModeCodePSMC" runat="server" Height="21px" MaxLength="5" Width="300px"></asp:TextBox>
                                        </td>
                                    </tr>  
                                    
                                    <tr>
                                        <td class="auto-style1"><asp:Button ID="btnInsert" runat="server" Height="30px" Text="Insert" Width="200px" OnClick="btnInsert_Click" Visible="false" /></td>
                                        <td class="auto-style5"></td>
                                        <td colspan="2">
                                            <asp:Label ID="lblMessage" runat="server"  ForeColor="Red"></asp:Label>
                                        </td>
                                        
                                    </tr>        

                                </table>


                            </div>

                            <!---- FOR GRIDVIEW -->

                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:GridView runat="server" ShowHeaderWhenEmpty="true" Width="100%" ID="gv_paymode" AutoGenerateColumns="false" 
                                            OnDataBound="gv_paymode_DataBound"  
                                            OnRowDataBound="gv_paymode_RowDataBound"
                                            OnRowEditing="gv_paymode_RowEditing"
                                            OnRowCancelingEdit="gv_paymode_RowCancelingEdit"
                                            OnRowUpdating="gv_paymode_RowUpdating" 
                                            CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                            OnPageIndexChanging="gv_paymode_SelectedIndexChanged" 
                                            AllowPaging="True">
                                           <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />

                                            <Columns>
                                                <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                    UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>
                                                <asp:TemplateField HeaderText="PaymentMode Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPayModeCode" runat="server" Text='<%# Bind("PayModeCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="PaymentMode Description">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_PayModeDesc" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPayModeDesc" runat="server" Text='<%# Bind("PayModeDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                 <asp:TemplateField HeaderText="Row Update">
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblUpdDate" runat="server" Text='<%# Bind("UpdTime") %>'></asp:Label>
                                                    </EditItemTemplate>                    
                                                     <ItemTemplate>
                                                        <asp:Label ID="lblUpdTime" runat="server" Text='<%# Bind("UpdTime") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Remove">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="BtnRemove_PayMode" runat="server" ImageUrl="~/Images/Delete_16x16.png" OnClick="BtnRemove_PayMode_Click" ToolTip="Remove" />
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

