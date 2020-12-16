<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" Theme="Theme1" AutoEventWireup="true"
    CodeBehind="CustomerInvoice.aspx.cs" Inherits="DXBMS.Modules.Service.CustomerInvoice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
    <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
     <script src="../../Data/sweetalert.js" type="text/javascript"></script>  
     <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>
 <script type="text/javascript">
           function Savealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been saved',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "CustomerInvoice.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "CustomerInvoice.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "CustomerInvoice.aspx";
           }
           function reloads() {
               window.location.href = "CustomerInvoice.aspx";
           }

       </script>
    <style>
        /* style sheet for tab*/


        .TabStyle .ajax__tab_header {
            cursor: pointer;
            background-color: #f1f1f1;
            font-size: 14px;
            font-weight: bold;
            font-family: Arial, Helvetica, sans-serif;
            height: 50px;
            border-bottom: 1px solid #bebebe;
            width: 100%;
        }

        .TabStyle .ajax__tab_active .ajax__tab_tab {
            border: 1px solid;
            border-color: #bebebe #bebebe #e1e1e1 #bebebe;
            background-color: #e1e1e1;
            padding: 10px;
            border-bottom: none;
        }

        /*.TabStyle .ajax__tab_active .ajax__tab_tab:hover {
                border: 1px solid;
                border-color: #bebebe #bebebe #e1e1e1 #bebebe;
                background-color: #e1e1e1;
                padding: 10px;
                border-bottom: none;
            }*/

        .TabStyle .ajax__tab_tab {
            border: 1px solid;
            border-color: #e1e1e1 #e1e1e1 #bebebe #e1e1e1;
            background-color: #f1f1f1;
            color: #777777;
            cursor: pointer;
            text-decoration: none;
            padding: 10px;
        }

        /*.TabStyle .ajax__tab_tab:hover {
                border: 1px solid;
                border-color: #bebebe #bebebe #e1e1e1 #bebebe;
                background-color: #e1e1e1;
                color: #777777;
                cursor: pointer;
                text-decoration: none;
                padding: 10px;
                border-bottom: none;
            }*/

        .TabStyle .ajax__tab_active .ajax__tab_tab, .TabStyle .ajax__tab_tab, .TabStyle .ajax__tab_header .ajax__tab_tab {
            margin: 0px 0px 0px 0px;
        }

        .TabStyle .ajax__tab_body {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 10pt;
            border-top: 0;
            border: 1px solid #bebebe;
            border-top: none;
            padding: 5px;
            background-color: #fdfdfd;
            width: 100%;
            padding-top: 15px;
        }
    </style>
    <script type="text/javascript">
        function ConfirmApproval() {



            if (confirm("Are you want to print invoice report") == true)
                return true;
            else
                return false;

        }
        function CalcInvPartsDiscount() {
            debugger;

            var PartDiscAmt = document.getElementById('<%=txtDiscountParts.ClientID%>').value;
            var TotalAmtPartsLubs = document.getElementById('<%=txttotPartsLub.ClientID%>').value;
            var LaborDiscAmt = document.getElementById('<%=txtDiscLabor.ClientID%>').value;
            var TotalAmtJobsSublet = document.getElementById('<%=txttotJobSublet.ClientID%>').value;
            var GST = document.getElementById('<%=txtGSTPercent.ClientID%>').value;
            var PST = document.getElementById('<%=txtPSTPercent.ClientID%>').value;
            var PartsLubGSTAmount = 0, JobsSubletPstAmt = 0;
            if (PartDiscAmt == "") PartDiscAmt = "0.0";
            if (LaborDiscAmt == "") LaborDiscAmt = "0.0";

            if (TotalAmtPartsLubs != "" && TotalAmtPartsLubs != "0") {
                document.getElementById('<%=txthdnParts.ClientID%>').value = PartDiscAmt;
                var PartsLubsDiscountPerc = (parseFloat(PartDiscAmt) / parseFloat(TotalAmtPartsLubs)) * 100;
                PartsLubGSTAmount = ((parseFloat(TotalAmtPartsLubs) - parseFloat(PartDiscAmt)) * parseFloat(GST)) / 100;
                var partdiscount = parseFloat(PartsLubsDiscountPerc).toFixed(2).toString();
                document.getElementById('<%=txtDiscPercentPart.ClientID%>').value = partdiscount;

            }


            if (TotalAmtJobsSublet != "" && TotalAmtJobsSublet != "0") {
                document.getElementById('<%=txthdnLabors.ClientID%>').value = LaborDiscAmt;
                var JobsSubDiscountPerc = (parseFloat(LaborDiscAmt) / parseFloat(TotalAmtJobsSublet)) * 100;
                //JobsSubletPstAmt = ((parseFloat(TotalAmtJobsSublet) - parseFloat(LaborDiscAmt)) * parseFloat(PST)) / 100;
                var labordiscount = parseFloat(JobsSubDiscountPerc).toFixed(2).toString();
                document.getElementById('<%=txtDiscLaborPercent.ClientID%>').value = labordiscount;
            }



            var SubTotalAmount = document.getElementById('<%=txtSubTotal.ClientID%>').value;
            var PstAmt = document.getElementById('<%=txtPSTAmount.ClientID%>').value;
            var GSTAmt = document.getElementById('<%=txtTaxAmount.ClientID%>').value;
            var TotalAmount = (parseFloat(SubTotalAmount) - (parseFloat(PartDiscAmt) + parseFloat(LaborDiscAmt) + parseFloat(PstAmt) + parseFloat(GSTAmt)) + (parseFloat(PartsLubGSTAmount) + parseFloat(JobsSubletPstAmt)));
            document.getElementById('<%=txthdnTotalnet.ClientID%>').value = SubTotalAmount;
            document.getElementById('<%=txtGSTAmount.ClientID%>').value = PartsLubGSTAmount;
            document.getElementById('<%=txtPSTAmount.ClientID%>').value = JobsSubletPstAmt;
            document.getElementById('<%=txtTotalAmtCustomer.ClientID%>').value = TotalAmount;
            document.getElementById('<%=hdnTotalForCustomer.ClientID%>').value = TotalAmount;

        }



    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
             <div class="row">
                <div class="col-lg-12">
               <span class="fa-arrow-left fa"></span>
                        <asp:LinkButton ID="lnkBack" ForeColor="Black" runat="server" PostBackUrl="~/" Text="Back"></asp:LinkButton>
                <div class="col-md-4 pull-right" style="margin-top:10px;">
                <asp:Label runat="server" ID="lblMode" Font-Size="16px" Font-Bold="true" ForeColor="Black"> </asp:Label>
                </div>
                </div>
                </div> 
                <div class="row">
                    <div class="col-lg-12">
                     
                         
                        <div class="Heading">CUSTOMER INVOICE</div>
                        <fieldset style="width: 100%;">
                            <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                                <%-- Row 1 --%>
                                <tr>
                                    <td>Invoice No.<asp:ImageButton runat="server" ID="ImageButtonInvNO" ImageUrl="~/Images/LookupNew.png" OnClick="ImageButtonInvNO_Click" Width="16px" AccessKey="i" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlInvoiceNo" runat="server" AutoPostBack="True" Font-Names="monospace" TabIndex="1"
                                            Width="100%" OnSelectedIndexChanged="ddlInvoiceNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>

                                    <td>Invoice Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInvoiceDate" runat="server"  CausesValidation="false" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="txtInvoiceDate_MaskedEditExtender" runat="server"
                                                ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="true" Mask="99-99-9999" TargetControlID="txtInvoiceDate"
                                                UserDateFormat="DayMonthYear" />
                                            <ajaxToolkit:CalendarExtender ID="txtInvoiceDate_CalendarExtender" runat="server"
                                                Enabled="true" Format="dd-MM-yyyy" TargetControlID="txtInvoiceDate" />
                                    </td>
                                    <td>Invoice Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlInvtype" runat="server" Width="100%" Enabled="True" AutoPostBack="true" OnSelectedIndexChanged="ddlInvtype_OnSelectedIndexChanged" TabIndex="2">
                                            <%-- AutoPostBack="true" OnSelectedIndexChanged="ddlInvtype_OnSelectedIndexChanged"--%>
                                            <%--<asp:ListItem  Text="Select" Value=""></asp:ListItem>--%>
                                            <asp:ListItem Selected="True" Text="Customer" Value="Depriciation"></asp:ListItem>
                                            <asp:ListItem Text="Insurance" Value="Insurance"></asp:ListItem>

                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <%-- Row 2 --%>
                                <tr>


                                    <td>Job Card<asp:ImageButton runat="server" ID="ImageButtonRoNO" AccessKey="j" ImageUrl="~/Images/LookupNew.png" OnClick="ImageButtonRoNO_Click" Width="16px" />
                                        <font color="red"><b>*</b></font>

                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlJobCardCode" runat="server" Visible="false" Font-Names="monospace"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlJobCardCode_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtJobCardCode" runat="server" Width="100%" ReadOnly="true" SkinID="ROTextBox" placeholder="Job Card"></asp:TextBox>

                                    </td>
                                    <td>Tax Type
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ReadOnly="true" Width="100%" ID="txtTaxType" SkinID="ROTextBox"></asp:TextBox>
                                        <asp:DropDownList ID="ddlTaxType" runat="server" Width="100%" Visible="false">
                                            <asp:ListItem Selected="True" Text="Filer" Value="FILER"></asp:ListItem>
                                            <asp:ListItem Text="Non Filer" Value="NON FILER"></asp:ListItem>
                                            <asp:ListItem Text="EXEMPTED" Value="EXEMPTED"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>

                                    <td>Bill Payment Mode</td>
                                    <td>
                                        <asp:TextBox ID="txtBillPayMode" runat="server" Width="100%" ReadOnly="True" SkinID="ROTextBox" placeholder="Payment Mode"></asp:TextBox>
                                    </td>

                                </tr>
                                <%-- Row 3 --%>
                                <tr>
                                    <td>Customer<asp:ImageButton runat="server" ID="imgCustomerCode" CausesValidation="false" ImageUrl="~/Images/LookupNew.png" OnClick="imgCustomerCode_Click" />
                                        <font color="red"><b>*</b></font>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCustCode" runat="server" Font-Names="monospace" Width="100%" TabIndex="3">
                                        </asp:DropDownList>
                                    </td>

                                    <td>Job Card Type
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtJobCardType" runat="server" ReadOnly="true" Style="text-transform: uppercase; background-image: url(../../../Images/ImgJobCardType.png); background-repeat: no-repeat; padding-left: 25px; width: 100%; height: 20pt;"></asp:TextBox>
                                    </td>
                                    <td>Product 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProductCode" runat="server" Width="30%" SkinID="ROTextBox"
                                            ReadOnly="True"></asp:TextBox>
                                        <asp:TextBox ID="txtProductName" runat="server" Width="67%" SkinID="ROTextBox"
                                            Placeholder="Vehicle Description" ReadOnly="True"></asp:TextBox>
                                    </td>

                                </tr>
                                <%-- Row 4 --%>
                                <tr>


                                    <td>Chassis No.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChassisNo" runat="server" ReadOnly="True" SkinID="ROTextBox" Placeholder="Chassis No"
                                            Width="100%"></asp:TextBox>
                                    </td>
                                    <td>Engine No.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEngineNo" runat="server" ReadOnly="True" SkinID="ROTextBox" Width="100%" placeholder="Engine No"></asp:TextBox>
                                    </td>
                                    <td>Registration No.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRegNo" runat="server" Width="100%" ReadOnly="True" SkinID="ROTextBox" placeholder="Reg No"></asp:TextBox>
                                    </td>
                                </tr>
                                <%-- Row 5 --%>
                                <tr>


                                    <td>Color
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtColorName" runat="server" ReadOnly="True" placeholder="Vehicle Color"
                                            SkinID="ROTextBox" Width="100%"></asp:TextBox>
                                    </td>
                                    <td>Policy No.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPolicyNo" runat="server" MaxLength="30" Width="100%" TabIndex="4"></asp:TextBox>
                                    </td>
                                    <td>Insurance Co.
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlInsCo" runat="server" TabIndex="5" AutoPostBack="True" OnSelectedIndexChanged="ddlInsCo_SelectedIndexChanged"
                                            Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <%-- Row 6 --%>
                                <tr>

                                    <td>Ins Branch
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlInsBranch" runat="server" Width="100%" TabIndex="6">
                                        </asp:DropDownList>
                                    </td>

                                    <td style="display: none;">TR Number
                                    </td>
                                    <td style="display: none;">
                                        <asp:TextBox ID="txtTRNumber" runat="server" Width="100%"></asp:TextBox>
                                    </td>
                                    <td style="display: none;">TR Date
                                    </td>
                                    <td style="display: none;">
                                        <asp:TextBox ID="txtTRDate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtTRDate"
                                            Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
                                    </td>
                                    <td>Voucher No</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtVoucherNo" ReadOnly="true" SkinID="ROTextBox" Width="100%"></asp:TextBox>
                                    </td>
                                    <td>Voucher Date
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtVouchDate" Placeholder="dd-MM-yyyy" ReadOnly="true" SkinID="ROTextBox" Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                                <%-- Row 7 --%>
                                <tr>

                                    <td>Remarks For User
                                    </td>
                                    <td colspan="1">
                                        <asp:TextBox ID="txtRemarksUser" runat="server" OnTextChanged="txtRemarksUser_TextChanged"
                                            ReadOnly="True" SkinID="ROTextBox" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                    </td>
                                    <td>User Name
                                    </td>
                                    <td colspan="1">
                                        <asp:TextBox ID="txtUserName" runat="server" Width="100%" ReadOnly="True" TextMode="MultiLine"
                                            SkinID="ROTextBox"></asp:TextBox>
                                    </td>
                                    <td>Receipt Mode
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlReciptMode" runat="server" TabIndex="5"
                                            Width="100%">
                                            <asp:ListItem Selected="True" Text="Manual" Value="Manual"></asp:ListItem>
                                            <asp:ListItem Text="Auto" Value="Auto"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <%-- Row 8 --%>
                                <tr style="display: none;">

                                    <td>
                                        <asp:TextBox ID="txtOutKM" runat="server" MaxLength="6" Visible="false"></asp:TextBox>
                                        <asp:Label ID="Label23" runat="server" Visible="false">Job Type</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtJobTypeDesc" runat="server" Visible="false" ReadOnly="True" SkinID="ROTextBox"
                                            Width="100%"></asp:TextBox>
                                        <asp:TextBox ID="txtJobType" runat="server" ReadOnly="True" SkinID="ROTextBox" Visible="false"
                                            Width="10px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </div>

                <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                    <%-- Row 1 --%>
                    <tr>
                        <td>
                            <asp:Label ID="Label28" runat="server">Jobs</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtJobsTotal" runat="server" Width="60px" SkinID="ROTextBox" ReadOnly="True"
                                Enabled="false"></asp:TextBox>
                        </td>
                        <td style="display: none;">
                            <asp:Label ID="Label101" runat="server">Jobs+Sublet</asp:Label>
                        </td>
                        <td style="display: none;">
                            <asp:TextBox ID="txttotJobSublet" runat="server" SkinID="ROTextBox" Width="60px"
                                Enabled="False"></asp:TextBox>
                        </td>

                        <td>
                            <asp:Label ID="Label6" runat="server">Bought Out</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBoutPercent" runat="server" ReadOnly="True" SkinID="ROTextBox" Width="60px" Placeholder="%" Text="25%"></asp:TextBox>
                            <asp:TextBox ID="txtBoutTotal" runat="server" ReadOnly="True" SkinID="ROTextBox" Width="60px"></asp:TextBox>


                        </td>

                        <td>
                            <asp:Label ID="Label116" runat="server">PST %</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPSTPercent" runat="server" ReadOnly="True" SkinID="ROTextBox"
                                Width="60px" OnTextChanged="txtPSTPercent_TextChanged" Enabled="False" Placeholder="%"></asp:TextBox>
                            <asp:TextBox ID="txtPSTAmount" runat="server" ReadOnly="True" SkinID="ROTextBox"
                                Width="60px" OnTextChanged="txtPSTAmount_TextChanged" Enabled="False"></asp:TextBox>
                        </td>

                        <td>
                            <asp:Label ID="Label105" runat="server" Width="100%">Discount Labor</asp:Label>
                        </td>
                        <td>
                            <%--onblur="CalcInvPartsDiscount()"--%>
                            <asp:TextBox ID="txtDiscLabor" runat="server" MaxLength="4" OnTextChanged="txtDiscLabor_TextChanged" AutoPostBack="true" CausesValidation="false" TabIndex="7"
                                Width="60px"></asp:TextBox>
                            <asp:HiddenField ID="txthdnLabors" runat="server"></asp:HiddenField>

                        </td>


                        <td>
                            <asp:Label ID="Label118" runat="server" Width="150px">Total Amount For Customer</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalAmtCustomer" runat="server" ReadOnly="True" SkinID="ROTextBox" CausesValidation="false"
                                Width="60px"></asp:TextBox>
                            <asp:HiddenField ID="txthdnTotalnet" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="hdnTotalForCustomer" runat="server"></asp:HiddenField>
                        </td>
                    </tr>
                    <%-- Row 2 --%>
                    <tr>
                        <td>
                            <asp:Label ID="Label29" runat="server">Parts</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPartsTotal" runat="server" Width="60px" SkinID="ROTextBox" ReadOnly="True"
                                Enabled="False"></asp:TextBox>
                        </td>
                        <td style="display: none;">
                            <asp:Label ID="Label100" runat="server">Parts+Lube</asp:Label>
                        </td>
                        <td style="display: none;">
                            <asp:TextBox ID="txttotPartsLub" runat="server" SkinID="ROTextBox" Width="60px" Enabled="False"></asp:TextBox>
                        </td>

                        <td>
                            <asp:Label ID="Label103" runat="server">GST %</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtGSTPercent" runat="server" ReadOnly="True" SkinID="ROTextBox" Width="60px" Placeholder="%"></asp:TextBox>
                            <asp:HiddenField ID="hdPrint" runat="server" />
                            <asp:TextBox ID="txtTaxAmount" runat="server" Width="60px" SkinID="ROTextBox" ReadOnly="True"
                                Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblDepAmount" runat="server" Text="Dep. Amount"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDepAmount" runat="server" SkinID="ROTextBox" ReadOnly="true"
                                Width="60px" Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label106" runat="server" Width="80px">Discount Parts</asp:Label>
                        </td>
                        <td>
                        <%--onblur="CalcInvPartsDiscount()"--%>
                            <asp:TextBox ID="txtDiscountParts" runat="server"  TabIndex="8" CausesValidation="false" OnTextChanged="txtDiscountParts_TextChanged" AutoPostBack="true" 
                                Width="60px" MaxLength="4"></asp:TextBox>
                            <asp:HiddenField ID="txthdnParts" runat="server"></asp:HiddenField>

                        </td>

                        <td>
                            <asp:Label ID="Label119" runat="server">Paid By Customer</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPaidCustomer" runat="server" Enabled="False" SkinID="ROTextBox"
                                Width="60px"></asp:TextBox>
                        </td>
                    </tr>
                    <%-- Row 3 --%>
                    <tr>
                        <td>
                            <asp:Label ID="Label30" runat="server">Lubricants</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLubTotal" runat="server" Width="60px" SkinID="ROTextBox" ReadOnly="True"
                                Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server">Further Tax %</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFGSTPerc" runat="server" ReadOnly="True" SkinID="ROTextBox" Placeholder="%"
                                Width="60px" Enabled="False"></asp:TextBox>
                            <asp:TextBox ID="txtFGSTAmount" runat="server" ReadOnly="True" SkinID="ROTextBox"
                                Width="60px" Text="0" Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label114" runat="server">Gross Amount</asp:Label>
                        </td>

                        <td>
                            <asp:TextBox ID="txtGrossAmount" runat="server" ReadOnly="True" SkinID="ROTextBox"
                                Width="60px" Enabled="False"></asp:TextBox>
                        </td>

                        <td>
                            <asp:Label ID="Label107" runat="server" Width="90px">Discount Parts %</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDiscPercentPart" runat="server" Width="60px" Enabled="false" Placeholder="%"></asp:TextBox>
                        </td>

                        <td>
                            <asp:Label ID="lblTotalAmtInsComp" runat="server" Width="100%">Total Amt Ins Co.</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotAmtInsComp" runat="server" ReadOnly="true" SkinID="ROTextBox"
                                Width="60px"></asp:TextBox>
                        </td>
                    </tr>
                    <%-- Row 4 --%>
                    <tr>
                        <td>
                            <asp:Label ID="Label31" runat="server">Sublet</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubletTotal" runat="server" Width="60px" SkinID="ROTextBox" ReadOnly="True"
                                Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label7" runat="server">Extra Tax %</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFTPerc" runat="server" ReadOnly="True" SkinID="ROTextBox" Placeholder="%"
                                Width="60px" Enabled="False"></asp:TextBox>
                            <asp:TextBox ID="txtFTAmount" runat="server" ReadOnly="True" SkinID="ROTextBox"
                                Width="60px" Text="0" Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label115" runat="server">Sub Total</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubTotal" runat="server" ReadOnly="True" SkinID="ROTextBox" Width="60px" CausesValidation="false"
                                OnTextChanged="txtSubTotal_TextChanged" Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label104" runat="server" Width="100%">Discount Labor %</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDiscLaborPercent" runat="server" Width="60px" Enabled="false" Placeholder="%"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblPaidInsComp" runat="server" Text="Paid by Inssurance co."></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPaidInsComp" runat="server" ReadOnly="true" SkinID="ROTextBox"
                                Width="60px"></asp:TextBox>
                        </td>
                    </tr>
                    <%-- Row 5 with property of display none --%>
                    <tr style="display: none;">
                        <td>
                            <asp:TextBox ID="txtDistOtherPercent" runat="server" Enabled="False" Visible="False"
                                Width="60px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDistOtherParts" runat="server" Enabled="False" Visible="False"
                                Width="60px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPaidInsurance" runat="server" SkinID="ROTextBox" Visible="False"
                                Width="60px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalAmntInsurance" runat="server" Enabled="False" Visible="False"
                                Width="60px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDipriAmount" runat="server" Enabled="False" SkinID="ROTextBox"
                                Visible="False" Width="60px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVEODAmount" runat="server" Enabled="False" SkinID="ROTextBox"
                                Visible="False" Width="60px"></asp:TextBox>
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="Label37" runat="server">GST Amount</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtGSTAmount" runat="server" ReadOnly="True" SkinID="ROTextBox" Width="60px"></asp:TextBox>
                        </td>

                    </tr>
                    <%-- Row 6 --%>
                    <td colspan="10" style="text-align: center;">
                        <asp:LinkButton runat="server" ID="lknDetailShow" OnClick="lknDetailShow_Click" TabIndex="9"><font color="blue" >Show Detail</font></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lknDetailHide" Visible="false" OnClick="lknDetailHide_Click"><font color="blue">Hide Detail</font></asp:LinkButton>
                    </td>

                </table>

                <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Visible="false"
                    Width="100%" Style="direction: ltr">
                    <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Sele Person Information">
                        <HeaderTemplate>
                            Jobs
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvJobCard" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                            Width="100%" OnRowDataBound="gvJobCard_RowDataBound" OnSelectedIndexChanged="gvJobCard_SelectedIndexChanged">
                                            <Columns>
                                                <asp:BoundField DataField="JobCode" HeaderText="Job Code">
                                                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DefJobDesc" HeaderText="Job">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="EmpCode" HeaderText="Technician Code" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="EmpName" HeaderText="Technician">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Amount" HeaderText="Labor">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                            </Columns>
                                            <AlternatingRowStyle BackColor="#CCCCCC" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Corporate / Institution">
                        <HeaderTemplate>
                            Parts 
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvJobCardParts" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                            Width="100%" OnRowDataBound="gvJobCardParts_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="PartNo" HeaderText="Part No." />
                                                <asp:BoundField DataField="PartsDesc" HeaderText="Part Description" />
                                                <asp:BoundField DataField="Dep" HeaderText="Depriciation" />
                                                <asp:BoundField DataField="DepAmount" HeaderText="Dep Amount" />
                                                <asp:BoundField DataField="Qty" HeaderText="Quantity" />
                                                <asp:BoundField DataField="Price" HeaderText="Price" />
                                                <asp:BoundField DataField="Total" HeaderText="Total" />
                                                <asp:BoundField DataField="Type" HeaderText="Type" />
                                            </Columns>
                                            <AlternatingRowStyle BackColor="#CCCCCC" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Customer Tag">
                        <HeaderTemplate>
                            Lubricants
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvLubParts" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                            Width="100%" OnRowDataBound="gvLubParts_RowDataBound" OnSelectedIndexChanged="gvLubParts_SelectedIndexChanged">
                                            <Columns>
                                                <asp:BoundField DataField="ItemCode" HeaderText="ItemCode" />
                                                <asp:BoundField DataField="PartNo" HeaderText="Part No">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PartsDesc" HeaderText="Parts Description">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="Dep" HeaderText="Dep%">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Qty" HeaderText="Quantity">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="DepAmount" HeaderText="DepAmount">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Price" HeaderText="Price">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Total" HeaderText="Total">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="GSTPerc" HeaderText="GST % " NullDisplayText="0" ReadOnly="True"
                                                    SortExpression="Item.GSTPerc" />
                                             <asp:BoundField DataField="Type" HeaderText="Type">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                            </Columns>
                                            <AlternatingRowStyle BackColor="#CCCCCC" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="Sublet Job">
                        <HeaderTemplate>
                            Sublet/Repair
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvSublet" runat="server" AutoGenerateColumns="False" Width="100%"
                                            ShowHeaderWhenEmpty="True" OnRowDataBound="gvSublet_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="VendorCode" HeaderText="Vendor Code" />
                                                <asp:BoundField DataField="Vendor" HeaderText="Vendor" />
                                                <asp:BoundField DataField="JobDescription" HeaderText="Job Description" />
                                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                                <asp:BoundField DataField="SubletDate" HeaderText="Date" />
                                                <asp:BoundField DataField="SubletAmount" HeaderText="Amount" />
                                            </Columns>
                                            <AlternatingRowStyle BackColor="#CCCCCC" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="TabPanel5">
                        <HeaderTemplate>
                            Consumeable Parts
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvJobCardConParts" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                            Width="100%">
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                <asp:BoundField DataField="ItemCode" HeaderText="Item Code" />
                                                <asp:BoundField DataField="PartNo" HeaderText="Part No.">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PartsDesc" HeaderText="Part Description" />
                                                <asp:BoundField DataField="RecQty" HeaderText="Rec Qty">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Qty" HeaderText="Qty">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Price" HeaderText="Price">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Total" HeaderText="Total">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Type" HeaderText="Type" />
                                            </Columns>
                                            <AlternatingRowStyle BackColor="#CCCCCC" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>

                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="TabPanel7" runat="server" HeaderText="TabPanel5">
                        <HeaderTemplate>
                            BoughtOut Parts
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvJobCardPartsBought" runat="server" AutoGenerateColumns="False"
                                            ShowHeaderWhenEmpty="True" Width="100%"
                                            BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                            CellPadding="3" ForeColor="Black" GridLines="Vertical">
                                            <AlternatingRowStyle BackColor="#CCCCCC" />
                                            <Columns>

                                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                <asp:BoundField DataField="ItemCode" HeaderText="Item Code" />
                                                <asp:BoundField DataField="PartNo" HeaderText="Part No.">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ItemDesc" HeaderText="Part Description" />
                                                <asp:BoundField DataField="Qty" HeaderText="Qty">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Price" HeaderText="PurchaseRate">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PurchaseTotal" HeaderText="Total Purchase Rate ">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="InvRate" HeaderText="Inv Rate">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="InvoiceTotal" HeaderText="Total Invoice Rate ">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                </asp:BoundField>

                                            </Columns>
                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>

                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="TabPanel6" runat="server" HeaderText="TabPanel6">
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="lblTaxHeader" Text="Taxes Aplicable"></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                                <tr>
                                    <td colspan="6">
                                        <asp:GridView ID="gvTax" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvTax_RowDataBound"
                                            ShowHeaderWhenEmpty="True" Width="100%">
                                            <Columns>
                                                <asp:BoundField DataField="ItemCode" HeaderText="ItemCode" Visible="false" />
                                                <asp:BoundField DataField="PartNo" HeaderText="PartNo" />
                                                <asp:BoundField DataField="ItemDesc" HeaderText="ItemDesc" />
                                                <asp:BoundField DataField="Source" HeaderText="Source" />
                                                <asp:BoundField DataField="Qty" HeaderText="Qty" />
                                                <asp:BoundField DataField="Price" HeaderText="Price" />
                                                <asp:BoundField DataField="GrossAmount" HeaderText="Gross Amount" />
                                                <asp:BoundField DataField="Type" HeaderText="Type" />
                                                <asp:BoundField DataField="Dep" HeaderText="Dep" Visible="True" />
                                                <asp:BoundField DataField="DepAmount" HeaderText="Dep Amount" />
                                                <asp:BoundField DataField="GSTPerc" HeaderText="GS TPerc" Visible="false" />
                                                <asp:BoundField DataField="GSTAmount" HeaderText="GST Amount" />
                                                <asp:BoundField DataField="FGSTPerc" HeaderText="FGST Perc" Visible="false" />
                                                <asp:BoundField DataField="FGSTAmount" HeaderText="FGST Amount" />
                                                <asp:BoundField DataField="ExTaxPerc" HeaderText="Ex Tax Perc" Visible="false" />
                                                <asp:BoundField DataField="ExTaxAmount" HeaderText="Ex Tax Amount" />
                                            </Columns>
                                            <AlternatingRowStyle BackColor="#CCCCCC" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Total Tax Amount
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblTotalTaxAmt" Text="0"></asp:Label>
                                    </td>
                                    <td>Total Amount With Tax
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblTotalWithTax" Text="0"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="TabPanel8" runat="server" HeaderText="TabPanel6" Visible="false">
                        <HeaderTemplate>
                            Insurance Customer Tax
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                                <tr>
                                    <td colspan="6">
                                        <asp:GridView ID="InsCusGVD" runat="server" AutoGenerateColumns="False" OnRowDataBound="InsCusGVD_RowDataBound"
                                            ShowHeaderWhenEmpty="True" Width="100%">
                                            <Columns>
                                                <asp:BoundField DataField="ItemCode" HeaderText="ItemCode" Visible="false" />
                                                <asp:BoundField DataField="PartNo" HeaderText="PartNo" />
                                                <asp:BoundField DataField="ItemDesc" HeaderText="ItemDesc" />
                                                <asp:BoundField DataField="Source" HeaderText="Source" />
                                                <asp:BoundField DataField="Qty" HeaderText="Qty" />
                                                <asp:BoundField DataField="Price" HeaderText="Price" />
                                                <asp:BoundField DataField="GrossAmount" HeaderText="Gross Amount" />
                                                <asp:BoundField DataField="Type" HeaderText="Type" />
                                                <asp:BoundField DataField="Dep" HeaderText="Dep %" Visible="True" />
                                                <asp:BoundField DataField="DepAmount" HeaderText="Tax Payable Amount" />
                                                <asp:BoundField DataField="GSTPerc" HeaderText="GST Perc" Visible="false" />
                                                <asp:BoundField DataField="GSTAmount" HeaderText="GST Amount" />
                                                <asp:BoundField DataField="FGSTPerc" HeaderText="FGST Perc" Visible="false" />
                                                <asp:BoundField DataField="FGSTAmount" HeaderText="FGST Amount" />
                                                <asp:BoundField DataField="ExTaxPerc" HeaderText="Ex Tax Perc" Visible="false" />
                                                <asp:BoundField DataField="ExTaxAmount" HeaderText="Ex Tax Amount" />
                                            </Columns>
                                            <AlternatingRowStyle BackColor="#CCCCCC" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Total Tax Amount
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblCusTaxAmt" Text="0"></asp:Label>
                                    </td>
                                    <td>Total Amount With Tax
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblCusTotalWithTax" Text="0"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                </ajaxToolkit:TabContainer>

              <table width="100%" style="margin-bottom: 10px;margin-top:10px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblMsg" runat="server" Width="100%"></asp:Label>

                            <%--<asp:DropDownList ID="ddlReport" runat="server"></asp:DropDownList>--%>

                            <asp:RadioButton ID="RadioButton1" runat="server" Text="GST" GroupName="Software" TabIndex="10" />
                            <asp:RadioButton ID="RadioButton2" runat="server" Text="PST" GroupName="Software" TabIndex="11" />
                            <asp:RadioButton ID="RadioButton3" runat="server" Text="Combine" GroupName="Software" Checked="true" TabIndex="12" />
                        </td>
                    </tr>
                    <tr>
                      <td style="text-align: right;">
                            <asp:Button ID="btnSaveInv" Width="100px" runat="server" Text="Save" OnClick="btnSaveInv_Click" UseSubmitBehavior="false" AccessKey="s"
                                OnClientClick="this.disabled = true; this.value = 'Please Wait';" />

   <asp:Button ID="btnDeleteInv" runat="server" Width="100px" Text="Delete" OnClientClick=" return confirm('Are you sure to delete?');"
                            OnClick="btnDeleteInv_Click" />
                            <asp:Button ID="btnClearForm" runat="server" Width="100px" Text="Clear" OnClick="btnClearForm_Click" AccessKey="w" />
                            <asp:Button ID="btnPrintInv" runat="server" Width="100px" Text="Print" OnClick="btnPrintInv_Click" AccessKey="l" />
                            <asp:Button ID="btnGL" runat="server" Width="100px" Text="GL" OnClick="btnGL_Click" AccessKey="g" />
                            <asp:Button ID="btnPerInv" runat="server" Text="Proforma Invoice" Width="20%" OnClick="btnPerInv_Click" AccessKey="p" />
                            <asp:Button ID="btnGatePass" runat="server" Text="Gate Pass" OnClick="btnGatePass_Click" AccessKey="m" />
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="HFRegNo" runat="server" />
                <asp:HiddenField ID="HFCustomer" runat="server" />
                <asp:HiddenField ID="HFChassisNo" runat="server" />
                <asp:HiddenField ID="HFEngineNo" runat="server" />
                <asp:HiddenField ID="HFBlank" runat="server" />
                <asp:HiddenField ID="HFJobCard" runat="server" />
                <asp:HiddenField runat="server" ID="hdnTaxType" />
                <asp:HiddenField runat="server" ID="hdnTaxCode" />

                <br />
                <%--<script type="text/javascript" src="../../../Scripts/jquery-1.4.1.js"></script>--%>
                <%--<script type="text/javascript" src="../../../Jquery/jquery-1.4.1.js"></script>
                <script type="text/javascript" src="../../../Jquery/JSInvCalc.js"></script>--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
