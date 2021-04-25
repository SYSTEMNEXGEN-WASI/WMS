<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockIssueFrm.aspx.cs"
    Inherits="DXBMS.Modules.SpareParts.Forms.StockIssueFrm" MasterPageFile="~/Root.master"
    Theme="Theme1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
<script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
        <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>
 <script src="../../Data/sweetalert.js" type="text/javascript"></script>  
 <script type="text/javascript">
           function Savealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been saved',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "StockIssueFrm.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "StockIssueFrm.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "StockIssueFrm.aspx";
           }
           function reloads() {
               window.location.href = "StockIssueFrm.aspx";
           }

       </script>
    <%--<script type="text/javascript" src="../../../Jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../../Jquery/MaxLength.min.js"></script>
    <script type="text/javascript">
       
        function MaxTxtLen() {
            $(function () {
                //Normal Configuration
                $("[id*=txtRemarks]").MaxLength({ MaxLength: 100 });
            });
        }
    </script>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
            <%--<script type="text/javascript">

                Sys.Application.add_load(MaxTxtLen);
            </script>--%>
            <div class="row">
                <div class="col-lg-12">
              
                <div class="col-md-4 pull-right" style="margin-top:10px;">
                <asp:Label runat="server" ID="lblMode" Font-Size="16px" Font-Bold="true" ForeColor="Black"> </asp:Label>
                </div>
                </div>
                </div>
            <div class="Heading">
                STOCK ISSUE
            </div>
            <fieldset>
            <table style="width: 100%; border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                <tr>
                    <td style="width: 10%">
                        Issue #.<asp:ImageButton runat="server" ID="ImageButtonSI" ImageUrl="~/Images/LookupNew.png" OnClick="ImageButtonSI_Click" Width="16px" AccessKey="i"  />
                    </td>
                    <td width="60%">
                        <asp:DropDownList ID="ddlIssueNo" runat="server" Font-Names="monospace" Width = "63%" Enabled="false"
                            AutoPostBack="True" onselectedindexchanged="ddlIssueNo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 10%">
                    Issue Date
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="txtIssueDate" runat="server"  placeholder="dd-MM-yyyy" Style="background-image: url(../../../Images/Calendar_16x16.png);
                            background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                            
                    </td>
                </tr>
                <tr>
                    <td >
                        SIR #.<asp:ImageButton runat="server" ID="ImageButtonSIR" ImageUrl="~/Images/LookupNew.png" OnClick="ImageButtonSIR_Click" Width="16px" AccessKey="i"  /></td>
                    <td>
                        <asp:DropDownList ID="ddlSirNo" runat="server" AutoPostBack="True" Width ="63%" Font-Names ="monospace" Enabled="false"
                            onselectedindexchanged="ddlSirNo_SelectedIndexChanged">
                        </asp:DropDownList>
                       </td>
                    <td >
                        SIR Date</td>
                    <td>
                        <asp:TextBox ID="txtSIRDate" runat="server" Enabled="false" placeholder="dd-MM-yyyy" Style="background-image: url(../../../Images/Calendar_16x16.png);
                            background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                       </td>
                </tr>
                <tr>
                    <td>
                        Issued SIR #.</td>
                    <td>
                        <asp:TextBox ID="txtIssuedSirNo" runat="server"></asp:TextBox>
                    </td>
                    <td><asp:Label ID="lblSource" runat="server"></asp:Label></td>
                    <td><asp:TextBox ID="txtJC_CS_No" runat="server" Enabled="false" Width="100%" SkinID="ROTextBox"></asp:TextBox>
                        <asp:TextBox ID="txtSource" runat="server" Visible="false"></asp:TextBox>
                    </td>
                </tr>
               
                <tr>
                    <td >

                        Remarks</td>
                    <td>
                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="100" TextMode="MultiLine" Width="63%"></asp:TextBox>
                        </td>
                    <td >
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
            </fieldset>
            <fieldset>
                <legend >PARTS</legend>
                <asp:Panel ID="Panel1" runat="server" >
                
                <asp:GridView ID="GvMain" runat="server" AutoGenerateColumns="False" Width="100%"
                                ShowHeaderWhenEmpty="true" Font-Size="9pt" ShowFooter="true" 
                    onrowdatabound="GvMain_RowDataBound" >
                                <HeaderStyle CssClass="GridHeader" />
                                <FooterStyle CssClass="GridFooter" />
                                <Columns>
                        
                        <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" runat="server" />
                        </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="ItemCode" HeaderText="ItemCode" />
                        <asp:BoundField DataField="PartItemNo" HeaderText="Part Item No" />
                        <asp:BoundField DataField="ItemDesc" HeaderText="Part Description" />

                        <asp:TemplateField HeaderText="Part Location">
                            <ItemTemplate>
                                <asp:Label ID="LbPartsTransLocCode" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                                <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="ddlLocation_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Quantity" HeaderText="Required Qty" />
                        <asp:BoundField DataField="BalQty" HeaderText="Balance Qty" />
                        <%--<asp:BoundField DataField="OHStock" HeaderText="Stock In Hand" />--%>
                        <asp:BoundField DataField="OLStock" HeaderText="Stock In Location" />
                        
                        <asp:TemplateField HeaderText="Issue Qty">
                            <ItemTemplate>
                            <asp:Label ID="lblIssueQty" runat="server" Text='<%# Bind("issueQty") %>'></asp:Label>
                                <asp:TextBox ID="txtIssueQty" runat="server" Placeholder="Issue Qty" AutoPostBack="true"
                                    Width="50px" ontextchanged="txtIssueQty_TextChanged" ></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers"
                            TargetControlID="txtIssueQty">
                        </asp:FilteredTextBoxExtender>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblFooterIssueQty" runat="server" ></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        

                       
                        <asp:TemplateField HeaderText="Avg. Cost">
                            <ItemTemplate>
                                <asp:Label ID="lblAvgCost" runat="server" Text='<%# Bind("AvgCost") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <asp:Label ID="LbAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                            <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        
                        
                    </Columns>
                </asp:GridView>
                </asp:Panel>
                </fieldset>
            <table width="100%" style="margin-bottom: 10px;margin-top:10px;">
                <tr>
                 <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*) Sterik Feilds Are Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
                    <td >
                        <asp:Label ID="LbErr" runat="server" Text=""></asp:Label>
                    </td>
                       <td style="text-align: right;">
                        
                        <asp:Button ID="btnPosting" runat="server" Text="Save"  OnClientClick="this.disabled = true; this.value = 'Please Wait';"
                            UseSubmitBehavior="false" onclick="btnPosting_Click" />
                        &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" onclick="btnClear_Click" />
                        &nbsp;</td>
                </tr>
            </table>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
