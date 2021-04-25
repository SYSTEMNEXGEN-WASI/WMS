<%@ Page Title="" Language="C#" Theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="StockAdjustment.aspx.cs" Inherits="DXBMS.Modules.SpareParts.Forms.StockAdjustment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
               window.location.href = "StockAdjustment.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "StockAdjustment.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "StockAdjustment.aspx";
           }
           function reloads() {
               window.location.href = "StockAdjustment.aspx";
           }

       </script>
     <style type="text/css">
        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: black;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }
    .loading {
             font-family: Arial;
             font-size: 10pt;
             border: 5px solid #67CFF5;
             width: 200px;
             height: 120px;
             display: none;
             position: fixed;
             background-color: White;
             z-index: 999;
         }
    </style>

    <script type="text/javascript">
        {
            function calc() {
                debugger
                var Rate = document.getElementById('<%=txtRate.ClientID%>').value;
                var IssueQty = document.getElementById('<%=txtChargeOutQty.ClientID%>').value;

                if (Rate == "") {
                    Rate = "0.0";
                }
                if (IssueQty == "") {
                    IssueQty = "0.0";
                }

                var total = parseFloat(Rate) * parseFloat(IssueQty);


                document.getElementById('<%=txtAmount.ClientID%>').value = parseFloat(total).toFixed(2).toString();
                document.getElementById('<%=hfAmount.ClientID%>').value = parseFloat(total).toFixed(2).toString();
            }

            function calc1() {
                debugger
                var Rate = document.getElementById('<%=txtRate.ClientID%>').value;
                var IssueQty = document.getElementById('<%=txtChargeOutQty.ClientID%>').value;

                if (Rate == "") {
                    Rate = "0.0";
                }
                if (IssueQty == "") {
                    IssueQty = "0.0";
                }

                var total = parseFloat(IssueQty) * parseFloat(Rate);


                document.getElementById('<%=txtAmount.ClientID%>').value = parseFloat(total).toFixed(2).toString();
                document.getElementById('<%=hfAmount.ClientID%>').value = parseFloat(total).toFixed(2).toString();
            }


            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode
                if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;
                return true;
            }

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
       <%-- <Triggers>               
                <asp:PostBackTrigger ControlID="btnUpload" />            
            </Triggers>--%>
        
        <ContentTemplate>
            <div class="container">
                <div class="row">
                <div class="col-lg-12">
              
                <div class="col-md-4 pull-right" style="margin-top:10px;">
                <asp:Label runat="server" ID="lblMode" Font-Size="16px" Font-Bold="true" ForeColor="Black"> </asp:Label>
                </div>
                </div>
                </div>
                        <fieldset>
                            <legend>Stock Adjustment </legend>

                            <table width="100%" style="border-collapse: separate; border-spacing: 5px;"
                                class="table table-condensed table-bordered table-hover">
                                <tr>
                                    <td>SA No<asp:ImageButton runat="server" ID="imgSALookup" ImageUrl="~/Images/LookupNew.png" onclick="imgSALookup_Click"/></td>
                                    <td>
                                        <%--<asp:DropDownList ID="ddState" runat="server" Width="100%">

                                            </asp:DropDownList>--%>
                                        <%-- <asp:TextBox ID="txtChargeOutNo" runat="server" Height="21px" MaxLength="1" Width="234px"></asp:TextBox>--%>

                                        <asp:DropDownList ID="ddlChargeOutNo" runat="server" Enabled="false" AutoPostBack="True" Font-Names="monospace" Width="234px" OnSelectedIndexChanged="ddlChargeOutNo_SelectedIndexChanged">
                                        </asp:DropDownList>

                                    </td>
                                    <td>SA Date</td>
                                    <td>

                                        <asp:TextBox ID="txtChargeOutDate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;" Width="234px" ReadOnly="true" OnTextChanged="txtChargeOutDate_TextChanged"></asp:TextBox>

                                    </td>
                                    <td>Stock Type </td>
                                    <td>
                                        <asp:RadioButton ID="RadioIN" Text="IN" runat="server" GroupName="RadioINOUTg" Checked="true" AutoPostBack="true" OnCheckedChanged="RadioIN_CheckedChanged" />
                                        <asp:RadioButton ID="RadioOUT" Text="OUT" runat="server" GroupName="RadioINOUTg" AutoPostBack="true" OnCheckedChanged="RadioOUT_CheckedChanged" />
                                    </td>
                                </tr>

                                <tr>
                                    
                                    <td>Reason </td>
                                    <td>
                                        <asp:DropDownList ID="ddlReason" runat="server" Width="234px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>Vender</td>

                                    <td>
                                        <asp:DropDownList ID="ddlVendor" runat="server" Width="234px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="auto-style10">Reference Document No</td>
                                    <td class="auto-style9">
                                        <asp:TextBox ID="txtRefDocno" runat="server" MaxLength="50" Width="234px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Remarks</td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="50" Width="80%" Height="19px"></asp:TextBox>
                                    </td>
                                    <td class="auto-style15">Total Amount</td>
                                    <td class="auto-style14">
                                        <asp:Label ID="lblTotalAMount" runat="server"></asp:Label>
                                    </td>

                                </tr>
                            </table>
                        </fieldset>
                        <fieldset>
                            <legend>Detail</legend>

                            <table width="100%" style="border-collapse: separate; border-spacing: 5px;"
                                class="table table-condensed table-bordered table-hover">

                                <tr>
                                    <td width="10%">
                                        <span style="margin-right: 10px;">Item Code</span>
                                        <asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png" OnClick="btnLookup_Click" />
                                    </td>
                                    <td >
                                        <asp:TextBox ID="txtItemCode" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>

                                        <%--<asp:DropDownList ID="ddlChPartNo" runat="server" AutoPostBack="True" Font-Names="monospace" Width="100%" OnSelectedIndexChanged="ddlChPartNo_SelectedIndexChanged">
                                                </asp:DropDownList>--%>
                                    </td>
                                    <td style="white-space: nowrap; width:10%;">Item No</td>
                                    <td width="10%">
                                        <asp:TextBox ID="txtPartItemNo_Parts" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td width="10%" style="white-space: nowrap;">Item Description</td>
                                    <td width="20%">
                                        <asp:TextBox ID="txtItemDesc" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>Location</td>
                                    <td>

                                        <%--<asp:TextBox ID="txtLocation" runat="server" Width="234px"></asp:TextBox>--%>
                                        <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="True" Font-Names="monospace" Width="100%" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                        </asp:DropDownList>

                                    </td>
                                </tr>

                                <tr>
                                    
                                    <td>Quantity </td>
                                    <td>
                                        <asp:TextBox ID="txtChargeOutQty" runat="server" MaxLength="4" Width="100%" onblur="calc()" onkeypress="return isNumberKey(event)" OnTextChanged="txtChargeOutQty_TextChanged"></asp:TextBox>
                                        <%--OnTextChanged="txtQty_TextChanged"--%>

                                        <asp:HiddenField runat="server" ID="hfAmount" />
                                    </td>
                                    <td>Stock Quantity </td>
                                    <td>
                                        <asp:TextBox ID="txtStockQty" runat="server" Width="100%" ReadOnly="True" OnTextChanged="txtStockQty_TextChanged1"></asp:TextBox>

                                    </td>
                                    <td class="auto-style20">Rate</td>

                                    <td class="auto-style24">
                                        <asp:TextBox ID="txtRate" runat="server" Width="100%" onblur="calc1()" MaxLength="6" onkeypress="return isNumberKey(event)" OnTextChanged="txtRate_TextChanged"></asp:TextBox>
                                    </td>

                                    <td >Weighted Average Rate </td>
                                    <td class="auto-style24">
                                        <asp:TextBox ID="txtWAR" runat="server" Width="100%" OnTextChanged="txtAmount_TextChanged" ReadOnly="True"></asp:TextBox>

                                    </td>
                                </tr>

                                <tr>
                                    
                                    <td class="auto-style25">Amount</td>
                                    <td class="auto-style23">
                                        <asp:TextBox ID="txtAmount" runat="server" Width="100%" OnTextChanged="txtAmount_TextChanged" ReadOnly="True"></asp:TextBox>

                                    </td>
                                    <td colspan="5">
                                        <label>Import Excel File:  </label>
                                        <asp:FileUpload ID="FileUpload1" runat="server" style="display: inline"/>
                                        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" PostBackUrl="~/Modules/SpareParts/Forms/StockAdjustment.aspx" OnClientClick="this.disabled = true; this.value = 'Please Wait'; ShowProgress()" UseSubmitBehavior="false" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnADD" runat="server" Text="Add" Width="100%" OnClick="btnAdd_Click" />
                                    </td>
                                </tr>
                                <tr>

                                    
                                </tr>




                            </table>
                        </fieldset>
                        <!---- FOR GRIDVIEW -->

                        <table width="100%">
                            <tr>
                                <td>
                                 <div style="width: 100%; height: 200px; overflow: scroll">
                                    <asp:GridView runat="server" ShowHeaderWhenEmpty="true" Width="100%" ID="gv_purchase" AutoGenerateColumns="false"
                                        OnRowDataBound="gv_purchase_RowDataBound"
                                        OnDataBound="gv_purchase_DataBound"
                                        OnPageIndexChanging="gv_purchase_SelectedIndexChanged"
                                        AllowPaging="true" PageSize="10"  Font-Names="Arial" Font-Size="9pt" style="text-align: left" PagerStyle-CssClass="pgr"
                                        >

                                        <%--  AllowPaging="True" PageSize="5" --%>

                                        <HeaderStyle CssClass="GridHeader" />
                                        <FooterStyle CssClass="GridFooter" />

                                        <Columns>

                                            
                                            <asp:TemplateField HeaderText="Item Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("ItemCode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />                                             

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblChargeOutNo" runat="server" Text='<%# Bind("ItemNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Item Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemDesc" runat="server" Text='<%# Bind("ItemDesc") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Location">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("PartsTransLocCode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="IN Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblINQty" runat="server" Text='<%# Bind("INQty") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Out Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChargOutQty" runat="server" Text='<%# Bind("ChargeOutQty") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Current Stock">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStockQty" runat="server" Text='<%# Bind("StockQty") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRate" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="WAR">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWAR" runat="server" Text='<%# Bind("WAR") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <%--  <asp:TemplateField HeaderText="Reason">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReason" runat="server" Text='<%# Bind("Reason") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>--%>




                                            <asp:TemplateField HeaderText="Remove">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="BtnRemove_PurchaseReturn" runat="server" ImageUrl="~/Images/Delete_16x16.png" OnClick="BtnRemove_PurchaseReturn_Click" ToolTip="Remove" UseSubmitBehavior="false" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                         <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <EditRowStyle BackColor="#999999" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle  CssClass="mGrid pgr" BackColor="White" HorizontalAlign="Center"/> <%--BackColor="#284775" ForeColor="White" HorizontalAlign="Center"--%>
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" 
                        Font-Names="Arial" Font-Size="10pt" HorizontalAlign="Left" 
                        VerticalAlign="Middle" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />

                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="First"  NextPageText="Last" />

                                    </asp:GridView>
                                    </div>
                                </td>
                            </tr>

                        </table>

                <div class="loading" align="center">
                                Loading. Please wait.<br />
                                <br />
                                <img src="../../../Images/loader.gif" alt="" />
                            </div>
                        <table width="70%" style="border-collapse: separate; border-spacing: 5px;"
                            class="table table-condensed table-bordered table-hover">

                            <tr>
                             <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*)Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
                                <td class="auto-style8" colspan="3">
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                                <td class="auto-style13" style="text-align: right;">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Save"  OnClick="btnSubmit_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait'; ShowProgress()" UseSubmitBehavior="false" />
                                    <asp:Button ID="btnCLear" runat="server" Text="Clear"  OnClick="btnClear_Click" />
                                    <asp:Button ID="btnPrint" runat="server" Text="Print"  OnClick="btnPrint_Click" />
                                     <asp:Button ID="btnGL" runat="server" Text="GL"  OnClick="btnGL_Click" />
                                    <asp:Button ID="btnGatePass" runat="server" Text="Gate Pass"  OnClick="btnGatePass_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait'; ShowProgress()" UseSubmitBehavior="false" />
                                </td>
                            </tr>




                        </table>
                    
            </div>
             
        </ContentTemplate>

    </asp:UpdatePanel>

     <script type="text/javascript">             

        function ShowProgress() {
            $("#btnSave").value = "Please Wait";

            setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
    </script>

</asp:Content>
