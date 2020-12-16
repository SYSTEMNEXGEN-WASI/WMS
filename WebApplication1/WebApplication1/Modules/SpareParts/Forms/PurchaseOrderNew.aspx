<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="PurchaseOrderNew.aspx.cs" Inherits="DXBMS.Modules.SpareParts.Forms.PurchaseOrderNew" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

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
               window.location.href = "PurchaseOrderNew.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "PurchaseOrderNew.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "PurchaseOrderNew.aspx";
           }
           function reloads() {
               window.location.href = "PurchaseOrderNew.aspx";
           }

       </script>
    <script type="text/javascript">
        {
            function calc() {
                debugger
                var Rate = document.getElementById('<%=txtRate.ClientID%>').value;
                var IssueQty = document.getElementById('<%=txtQty.ClientID%>').value;

                if (Rate == "") {
                    Rate = "0.0";
                }
                if (IssueQty == "") {
                    IssueQty = "0.0";
                }

                var total = parseFloat(Rate) * parseFloat(IssueQty);


                document.getElementById('<%=txtAmount.ClientID%>').value = parseFloat(total).toFixed(2).toString();                
                document.getElementById('<%=txtNetAmount.ClientID%>').value = parseFloat(total).toFixed(2).toString();
                document.getElementById('<%=hfAmount.ClientID%>').value = parseFloat(total).toFixed(2).toString();
            }

            function calc1() {
                debugger
                var Rate = document.getElementById('<%=txtRate.ClientID%>').value;
                var IssueQty = document.getElementById('<%=txtQty.ClientID%>').value;

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
            function run(field) {
                setTimeout(function () {
                    var regex = /\d*\.?\d?/g;
                    field.value = regex.exec(field.value);
                }, 0);
            }

            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                if (charCode != 46 && charCode > 31
                  && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }

        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="Server">

    <asp:updatepanel ID="Updatepanel1" runat="server">

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
                            <legend>Purchase Order </legend>
                                <table width="70%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">

                                    <tr>
                                        <td width="13%">P/O No.:<asp:ImageButton runat="server" ID="ImageButtonPO" ImageUrl="~/Images/LookupNew.png" OnClick="ImageButtonPO_Click" Width="16px" AccessKey="i"  /> 
                                           
                                        </td>
                                        <td >
                                            <asp:DropDownList ID="ddlPONo" runat="server" AutoPostBack="True" Font-Names="monospace" Width="100%" OnSelectedIndexChanged="ddlPONo_SelectedIndexChanged" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                        <td width="10%">P/O Date:</td>
                                        <td>
                                          <asp:TextBox ID="txtPODate" runat="server" Style="background-image: url(/Images/Calendar_16x16.png); 
                                                                            background-repeat:no-repeat; padding-left:25px;" width="234px"></asp:TextBox>
                                                 <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtPODate" Format="dd-MM-yyyy"></asp:CalendarExtender>


                                           

                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td >Vendor*</td>
                                        <td >
                                            <asp:DropDownList ID="ddlVendor" runat="server" AutoPostBack="True" Font-Names="monospace" Width="234px" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td >Delivery Type*</td>
                                        <td >
                                            <asp:DropDownList ID="ddlDeliveryType" runat="server" AutoPostBack="True" Font-Names="monospace" Width="234px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style15">Payment Terms </td>
                                        <td class="auto-style14">
                                            <asp:TextBox ID="txtPayment" runat="server" onkeypress="return isNumberKey(event)" Width="234px"></asp:TextBox>
                                            <span>In days e.g 15</span>
                                        </td>
                                        <td class="auto-style15">Parts Source* </td>
                                        <td class="auto-style14">
                                            <asp:DropDownList ID="ddlPartsSource" runat="server" AutoPostBack="True" Font-Names="monospace" Width="234px" OnSelectedIndexChanged="ddlPartsSource_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style8">Remarks:</td>
                                        <td class="auto-style10" colspan="3">
                                            <asp:TextBox ID="txtRemarks" runat="server" Width="98%"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                        </fieldset>
                        <fieldset>
                             <legend>Detail</legend>

                             <table width="60%" style="border-collapse: separate; border-spacing: 5px;"
                                        class="table table-condensed table-bordered table-hover">

                                        <tr>
                                            <td width="10%">
                                                <span style="margin-right: 10px;">Item Code</span>
                                                 <asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png" onclick="btnLookup_Click"/>
                                            </td>                                            
                                            <td  >       
                                                <asp:TextBox ID="txtItemCode" runat="server" Width="100%"></asp:TextBox>                                    
                                                <%--<asp:DropDownList ID="ddlChPartNo" runat="server" AutoPostBack="True" Font-Names="monospace" Width="100%" OnSelectedIndexChanged="ddlChPartNo_SelectedIndexChanged">
                                                </asp:DropDownList>--%>
                                            </td>

                                            <td style="white-space: nowrap;">Item No</td>
                                        <td>
                                            <asp:TextBox ID="txtPartItemNo_Parts" runat="server" Width="100%"></asp:TextBox>                                       
                                        </td>  
                                        <td width="13%">Item Description</td>
                                        <td>
                                            <asp:TextBox ID="txtItemDesc" runat="server" Width="100%"></asp:TextBox>
                                        </td>
                                            <%--<td class="auto-style23">
                                                <asp:TextBox ID="txtItemDesc" runat="server"   ReadOnly="true" ></asp:TextBox>
                                            </td>--%>                                            
                                        </tr>

                                        <tr>
                                            <td width="10%">Packing Qty</td>
                                            <td width="10%">
                                                <asp:TextBox ID="txtPackingQty" runat="server"  ReadOnly="true" Width="100%"></asp:TextBox>
                                            </td>
                                            <td width="10%">Quantity </td>
                                            <td width="10%">
                                                <asp:TextBox ID="txtQty" runat="server"  MaxLength="3"  BackColor="White" AutoPostBack="true" onkeypress="return isNumberKey(event)" OnTextChanged="txtQty_TextChanged" ></asp:TextBox>   
                                                <asp:HiddenField runat="server" ID="hfAmount" />
                                            </td>
                                            <td >Unit </td>
                                            <td >
                                                <asp:TextBox ID="txtUnit" runat="server"   ReadOnly="True" ></asp:TextBox>                                               
                                            </td>                                    
                                             
                                        </tr>  
                                        <tr>
                                            <td >Rate</td>
                                            <td >
                                                <asp:TextBox ID="txtRate" runat="server" onkeypress="return IsOneDecimalPoint(event);" MaxLength="10"  PlaceHolder="0.0" AutoPostBack="true" OnTextChanged="txtQty_TextChanged"></asp:TextBox> 
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender" runat="server" FilterType="Custom"

                                                ValidChars="01234567890." TargetControlID="txtRate"></asp:FilteredTextBoxExtender>                                              
                                            </td>
                                            <td >Discount %</td>
                                            <td >
                                              <asp:TextBox ID="txtDiscountPerc" runat="server"  onkeyup="run(this)" BackColor="White" AutoPostBack="true"  PlaceHolder="0" OnTextChanged="txtDiscountPerc_TextChanged"></asp:TextBox> 
                                            </td>
                                            <td >Discount Amount</td>
                                           <td  >
                                                <asp:TextBox ID="txtDicountAmt" runat="server"   ReadOnly="True" PlaceHolder="0.0"></asp:TextBox>                                               
                                            </td>                                         
                                        </tr>
                                        
                                        <tr>                                      
                                            <td >Amount</td>
                                            <td >
                                                <asp:TextBox ID="txtAmount" runat="server"   ReadOnly="True" ForeColor="Red" PlaceHolder="0.0"></asp:TextBox>                                               
                                            </td>
                                             <td style="white-space:nowrap;">G.S.T %</td>
                                            <td >
                                                <asp:TextBox ID="txtGSTPercent" runat="server"   BackColor="White" AutoPostBack="True" onkeypress="return IsOneDecimalPoint(event);" OnTextChanged="txtGSTPercent_TextChanged"></asp:TextBox>
                                            </td>
                                            <td >G.S.T Amount </td>
                                            <td>
                                                <asp:TextBox ID="txtGSTAmt" runat="server"   ReadOnly="True" PlaceHolder="0.0" ForeColor="Red"></asp:TextBox>                                            
                                            </td>                                           

                                           <%-- <td class="auto-style61">Liter:</td>
                                            <td class="auto-style32">
                                                <asp:TextBox ID="txtLiter" runat="server"   ReadOnly="True" ForeColor="Red"></asp:TextBox>
                                            </td>--%>
                                        </tr>
                                        <tr>                                       
                                            <td style="white-space:nowrap;">Further Tax %:</td>
                                            <td>
                                                <asp:TextBox ID="txtFurtherTaxPerc" runat="server"   BackColor="White" AutoPostBack="True"  onkeypress="return IsOneDecimalPoint(event);" OnTextChanged="txtFurtherTaxPerc_TextChanged"></asp:TextBox>                                                
                                            </td>
                                            <td style="white-space:nowrap;">Further Tax Amount:</td>
                                            <td >
                                                <asp:TextBox ID="txtFurtherTaxAmt" runat="server"   ReadOnly="True" PlaceHolder="0.0"></asp:TextBox>
                                            </td>
                                            <td >Extra Tax % </td>
                                            <td>
                                                <asp:TextBox ID="txtExTaxPerc" runat="server"   AutoPostBack="True" BackColor="White"  onkeypress="return IsOneDecimalPoint(event);" OnTextChanged="txtExTaxPerc_TextChanged"></asp:TextBox>
                                               
                                            </td>                                           

                                           <%-- <td class="auto-style61">Liter:</td>
                                            <td class="auto-style32">
                                                <asp:TextBox ID="txtLiter" runat="server"   ReadOnly="True" ForeColor="Red"></asp:TextBox>
                                            </td>--%>
                                        </tr>

                                        <tr>
                                          <td style="white-space:nowrap;">Extra Tax Amount:</td>
                                            <td>
                                                <asp:TextBox ID="txtExTaxAmt" runat="server" ReadOnly="true"  PlaceHolder="0.0"></asp:TextBox>                                               
                                            </td>               

                                            <td><strong>Net Amount</strong></td>
                                            <td >
                                                
                                                <asp:TextBox ID="txtNetAmount" runat="server" ForeColor="Red" PlaceHolder="0.0" ReadOnly="True"  ></asp:TextBox>
                                                
                                            </td>
  
                                            <td></td>
                                            <td >
                                                <asp:Button ID="btnADD" runat="server" Height="30px" Text="Add" OnClick="btnADD_Click" />  
                                                <asp:Button ID="btnClear2" runat="server" Height="30px" Text="Clear" OnClick="btnClear2_Click"/>                                              
                                            </td>
                                            
                                        </tr>

                                    </table>

                        </fieldset>
                        <fieldset>

                            <!---- FOR GRIDVIEW -->

                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:GridView runat="server" ShowHeaderWhenEmpty="true" Width="100%" ID="gv_purchaseOrder" AutoGenerateColumns="false"
                                           OnDataBound="gv_purchaseOrder_DataBound"
                                            OnRowDataBound="gv_purchaseOrder_RowDataBound"
                                             OnRowEditing="gv_purchaseOrder_RowEditing"
                                            OnRowCancelingEdit="gv_purchaseOrder_RowCancelingEdit"
                                            OnRowUpdating="gv_purchaseOrder_RowUpdating"
                                            AllowPaging="true" PageSize="5">

                                            <%--  AllowPaging="True" PageSize="5" 
                                                OnRowDataBound="gv_purchase_RowDataBound"
                                            OnPageIndexChanging="gv_purchase_SelectedIndexChanged"
                                            --%>

                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />

                                            <Columns>

                                                   <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                    UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>



                                                <%--<asp:TemplateField HeaderText="SA No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblChargeOutNo" runat="server" Text='<%# Bind("ChargeOutNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Part Item Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("ItemCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />

                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Part Item Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemDesc" runat="server" Text='<%# Bind("ItemDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />

                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Unit">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("UnitCode") %>' ></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

<%--                                                <asp:TemplateField HeaderText="Packing Qty">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPackingSize" runat="server" Text='<%# Bind("PackingSize") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>   --%>  

                                                <asp:TemplateField HeaderText="Quantity">
                                                     <EditItemTemplate>
                                                        <asp:TextBox ID="txtQuantityEdit" runat="server" MaxLength="3" ></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("POQty") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>                                          

                                                <asp:TemplateField HeaderText="Rate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRate" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <%--<asp:TemplateField HeaderText="Liter">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLiter" runat="server" Text='<%# Bind("ToltalLiters") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>--%>     
                                                <asp:TemplateField HeaderText="Disc %">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDiscPercent" runat="server" Text='<%# Bind("DiscountPerc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Discount" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDiscount" runat="server" Text='<%# Bind("Discount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="G.S.T %">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGSTPErcent" runat="server" Text='<%# Bind("GSTage") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Fur.Tax%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFurTaxPercent" runat="server" Text='<%# Bind("PFGSTPERC") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Ex.Tax%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblExTaxPercent" runat="server" Text='<%# Bind("ExTaxPerc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="G.S.T Amt">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGSTAmount" runat="server" Text='<%# Bind("GSTAmount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Fur.Tax Amt">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFurTaxAmount" runat="server" Text='<%# Bind("PFGSTAmt") %>' ></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Ex.Tax Amt">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblExTaxAmount" runat="server" Text='<%# Bind("ExTaxAmt") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Net Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNetAmount" runat="server" Text='<%# Bind("NetAmount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>




                                                <asp:TemplateField HeaderText="Remove">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="BtnRemove_PurchaseOrder" runat="server" ImageUrl="~/Images/Delete_16x16.png" ToolTip="Remove" UseSubmitBehavior="false" OnClick="BtnRemove_PurchaseOrder_Click" />
                                                        
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>


                                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="First" NextPageText="Last" />

                                        </asp:GridView>
                                    </td>
                                </tr>

                            </table>

                            <table width="100%" style="border-collapse: separate; border-spacing: 5px;"
                                class="table table-condensed table-bordered table-hover">
                                
                                <tr>
                                    <td width="12%">Total Quantity </td>
                                    <td width="12%">
                                        <asp:TextBox ID="txtTotalQty" runat="server" Width="100%" ReadOnly="True" PlaceHolder="0.0"></asp:TextBox>
                                        <%--OnTextChanged="txtAmount_TextChanged"--%>
                                    </td>
                                    <td width="12%">Total Litres</td>
                                    <td width="12%">
                                        <asp:TextBox ID="txtTotalLitre" runat="server" Width="100%"  ReadOnly="True" PlaceHolder="0.0"></asp:TextBox>
                                        <%--OnTextChanged="txtAmount_TextChanged"--%>
                                    </td>

                                    <td width="12%">G.S.T Amount</td>
                                    <td width="12%">
                                        <asp:TextBox ID="txtTotalGSTAmt" runat="server" Width="100%"  ReadOnly="True" PlaceHolder="0.0"></asp:TextBox>
                                    </td>
                                    <td width="12%">Further Tax Amount:</td>
                                    <td width="12%">
                                        <asp:TextBox ID="txtTotalFurtherTaxAmnt" runat="server" Width="100%"  ReadOnly="True" PlaceHolder="0.0"></asp:TextBox>
                                    </td>
                                </tr>
                              
                                 <tr>
                                    <td >Total Extra Tax </td>
                                    <td >
                                        <asp:TextBox ID="txtTotalExtraTaxAmnt" runat="server" Width="100%"  ReadOnly="True" PlaceHolder="0.0"></asp:TextBox>
                                        <%--OnTextChanged="txtAmount_TextChanged"--%>
                                    </td>
                                    <td >Gross Amount:</td>
                                    <td >
                                        <asp:TextBox ID="txtGrossAmt" runat="server" Width="100%"  ReadOnly="True" PlaceHolder="0.0"></asp:TextBox>
                                        <%--OnTextChanged="txtAmount_TextChanged"--%>
                                    </td>

                                    <td >Other Charges</td>
                                    <td >
                                        <asp:TextBox ID="txtOtherCharges" runat="server" AutoPostBack="true" Width="100%"  BackColor="White" PlaceHolder="0.0" OnTextChanged="txtOtherCharges_TextChanged"></asp:TextBox>
                                    </td>
                                     <td>Total Net Amount</td>
                                    <td >
                                        <asp:TextBox ID="txtTotalNetAmt" runat="server" Width="100%"  ReadOnly="True" PlaceHolder="0.0"></asp:TextBox>
                                    </td>
                                      
                                  
                                </tr>
                                <tr>
                                     <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*) Sterik Feilds Are Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>

                                     <td class="auto-style8" colspan="2">
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                      <td>

                                    </td>
                                    <td class="auto-style13">
                                         <asp:Button ID="btnSubmit" runat="server" Height="30px" Text="Save" Width="100%" OnClick="btnSubmit_Click"  />
                                    </td>
                                    <td class="auto-style45">                                       
                                        <asp:Button ID="btnCLear" runat="server" Height="30px" Text="Clear" Width="100%" OnClick="btnCLear_Click"  />                                       
                                    </td>
                                    <td>
                                        <asp:Button ID="btnPrint" runat="server" Height="30px" Text="Print" Width="100%" OnClick="btnPrint_Click"  />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnDelete" runat="server" Height="30px" Text="Delete" Width="100%" OnClick="btnDelete_Click"                                            
                                            OnClientClick="javascript: return confirm('Are you sure you want to Delete?');" />
                                    </td>
                                  

                                </tr>
                                
                            </table>
                        </fieldset>
                        
                    
            </div>

        </ContentTemplate>

    </asp:updatepanel>

      <script language="javascript" type="text/javascript">

function IsOneDecimalPoint(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode; // restrict user to type only one . point in number
                var parts = evt.srcElement.value.split('.');
                if(parts.length > 1 && charCode==46)
                    return false;
                return true;
            }
</script>
</asp:Content>

