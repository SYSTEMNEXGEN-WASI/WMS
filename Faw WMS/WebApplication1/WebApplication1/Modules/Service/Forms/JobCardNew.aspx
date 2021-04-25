<%@ Page Title="" Language="C#" Theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true"
    CodeBehind="JobCardNew.aspx.cs" Inherits="DXBMS.Modules.Service.JobCardNew" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <%--<asp:CalendarExtender ID="txtRecieptDae_CalendarExtender" runat="server" Enabled="True"
                        Format="dd-MM-yyyy" TargetControlID="txtRecieptDae">
                    </asp:CalendarExtender>--%>
    <%--<link rel="stylesheet" href="css/bootstrap.min.css" >

<link href="css/style.css" rel='stylesheet' type='text/css' />
<link href="css/font-awesome.css" rel="stylesheet">
<script src="js/jquery2.0.3.min.js"></script>--%>
  
    <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
    <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="../../../Jquery/quicksearch.js" type="text/javascript"></script>
    <script src="../../../Jquery/jquery.tablesorter-2.0.3.js" type="text/javascript"></script>
    <link href="../../../css/styletable.css" rel="stylesheet" type="text/css" />
    <link href="../../../css/datatables.min.css" rel="stylesheet" type="text/css" />
    <script src="../../../js/datatables.min.js" type="text/javascript"></script>
    <script src="../../../Jquery/jquery-ui-1.7.3.custom.min.js" type="text/javascript"></script>
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
               window.location.href = "JobCardNew.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "JobCardNew.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "JobCardNew.aspx";
           }
           function reloads() {
               window.location.href = "JobCardNew.aspx";
           }

       </script>


    <style>
        /* style sheet for tab*/
        .ClearButton {
            margin-left: 7px;
        }

        .TabStyle .ajax__tab_header {
            cursor: pointer;
            background-color: #f1f1f1;
            font-size: 14px;
            font-weight: bold;
            font-family: Arial, Helvetica, sans-serif;
            height: 36px;
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

            .TabStyle .ajax__tab_active .ajax__tab_tab:hover {
                border: 1px solid;
                border-color: #bebebe #bebebe #e1e1e1 #bebebe;
                background-color: #e1e1e1;
                padding: 10px;
                border-bottom: none;
            }

        .TabStyle .ajax__tab_tab {
            border: 1px solid;
            border-color: #e1e1e1 #e1e1e1 #bebebe #e1e1e1;
            background-color: #f1f1f1;
            color: #777777;
            cursor: pointer;
            text-decoration: none;
            padding: 10px;
        }

            .TabStyle .ajax__tab_tab:hover {
                /*border: 1px solid;*/
                border-color: #bebebe #bebebe #e1e1e1 #bebebe;
                background-color: #e1e1e1;
                color: #777777;
                cursor: pointer;
                text-decoration: none;
                padding: 10px;
                border-bottom: none;
            }
            .ajax__tab_xp .ajax__tab_body {
    border: none !important;
    border-top: 0;
    font-size: 10pt;
    padding: 8px;
}

        .TabStyle .ajax__tab_active .ajax__tab_tab, .TabStyle .ajax__tab_tab, .TabStyle .ajax__tab_header .ajax__tab_tab {
            margin: 0px 0px 0px 0px;
        }

        input.imgBackBtn {
            height: 45px;
            padding-right: 37px;
            margin-right: -35px;
        }

        .TabStyle .ajax__tab_body {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 10pt;
            border-top: 0;
            //border: 1px solid #bebebe;
            border-top: none;
            padding: 5px;
            background-color: #e1e1e1;
            width: 100%;
            padding-top: 15px;
        }

        .chosen-drop {
            width: 100% !important;
        }

        .Refresh {
            margin-left: 7px;
        }
         .textalign {
            text-align:center !important;
             font-size: 10pt !important;
        }

        .FixedHeader {
            position: absolute;
            font-weight: bold;
        }
    </style>
    <link rel="stylesheet" type="text/css" href="../../../css/Tab.css" />


    <script language="javascript" type="text/javascript">
        function Disable(x) {
            x.disabled = true;
            x.Value = "test";
        }

        function Enable(x) {
            x.enabled = true;
            x.Value = "Save";
        }
        function Confrimdelete() {
            var del;
            del = confirm("Are you sure you want to delete ");
            if (del) {
                document.getElementById("lblChk").value = "T";
            }
            else {
                document.getElementById("lblChk").value = "F";
            }

        }
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to save data?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }

        function checkTextAreaMaxLength(textBox, e, length) {

            var mLen = textBox["MaxLength"];
            if (null == mLen)
                mLen = length;

            var maxLength = parseInt(mLen);
            if (!checkSpecialKeys(e)) {
                if (textBox.value.length > maxLength - 1) {
                    if (window.event)//IE
                        e.returnValue = false;
                    else//Firefox
                        e.preventDefault();
                }
            }
        }
        function checkSpecialKeys(e) {
            if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                return false;
            else
                return true;
        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="gv_history" />
        </Triggers>
        <ContentTemplate>
            <div class="container">
                <%--<div class="row">
                    <div class="col-lg-1">
                        <asp:ImageButton runat="server" ImageUrl="~/Images/arrow-left.png" CssClass="imgBackBtn" PostBackUrl="~/Modules/Service/Forms/JC.aspx"/>
                    </div>
                    <div style="padding-top: 10px;">
                        <span style="font-size:large; font-weight:bold; color:black; margin-left: -30px;">Back</span>
                    </div>                    
                </div>--%>
                <div style="font-size: 15px;">
                    <span class="fa-arrow-left fa"></span>
                    <asp:LinkButton ID="lnkBack" ForeColor="Black" runat="server" PostBackUrl="~/Modules/Service/Forms/JC.aspx" Text="Back"></asp:LinkButton>

                </div>
                <div class="Heading">WORK ORDER  /  JOB CARD</div>
                <div class="row">
                <div class="col-md-4 pull-right">
                 <asp:LinkButton runat="server" Visible="false" ID="lknPDI" Text="PDI" onclick="lknPDI_Click" Font-Size="Medium" ></asp:LinkButton>
                </div>
                  
                 </div>
                <fieldset class="field">
                 <div class="row">
                   <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                          <%--Row1--%>
                          <tr>
                        <td colspan="1">
                        Job Card No.<asp:ImageButton runat="server" ID="imgJobCardCode" ImageUrl="~/Images/LookupNew.png" OnClick="imgJobCardCode_Click" AccessKey="j"/>                                        
                         </td>
                        <td colspan="2">
                         <asp:TextBox ID="txtJobCardCode" class="textalign" runat="server" ReadOnly="true" SkinID="ROTextBox" Width="100%"  ></asp:TextBox>
                                            <%--<asp:DropDownList ID="ddlJobCardCode" class="WidthAt764" runat="server" AutoPostBack="True" Font-Names="monospace"
                                        Width="100%" OnSelectedIndexChanged="ddlJobCardCode_SelectedIndexChanged" TabIndex="1">
                                    </asp:DropDownList>
                                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddlJobCardCode"
                                        PromptText="" QueryPattern="Contains">
                                    </ajaxToolkit:ListSearchExtender>--%>
                        </td>
                        <td colspan="1">
                         Estimate No.  <asp:ImageButton runat="server" ID="imgEstNo" ImageUrl="~/Images/LookupNew.png" OnClick="imgEstNo_Click"  AccessKey="q" />
                          
                        </td>
                        <td colspan="2">
                        <asp:TextBox ID="txtExtno" runat="server" ReadOnly="true" SkinID="ROTextBox" class="textalign" Width="100%" ></asp:TextBox>
                                            <%-- <asp:DropDownList ID="ddlEstNo" runat="server" class="WidthAt764" Font-Names="monospace" Width="100%"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlEstNo_SelectedIndexChanged" TabIndex="2">
                                        </asp:DropDownList>
                                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddlEstNo"
                                            PromptText="" QueryPattern="Contains">
                                        </ajaxToolkit:ListSearchExtender>--%>
                        </td>
                        <td colspan="1">
                        <asp:LinkButton ID="LinkButton1" runat="server" style="display:none;"  Height="18px" SkinID="btnLables"></asp:LinkButton>
                        <font color="#000000">Reg No.</font><font color="red"><b>*</b></font>
                       <asp:ImageButton runat="server" ID="imgVehRegNo" ImageUrl="~/Images/LookupNew.png" OnClick="imgVehRegNo_Click"  AccessKey="v" />
                        </td>
                        <td colspan="2">
                        <asp:TextBox runat="server" ID="txtVehRegNo" ReadOnly="true" SkinID="ROTextBox" class="textalign" Width="100%"></asp:TextBox>
                                            <%--<asp:DropDownList ID="ddlVehRegNo" runat="server" AutoPostBack="true" Font-Names="monospace"
                                        Width="100%" OnSelectedIndexChanged="ddlVehRegNo_SelectedIndexChanged" class="WidthAt764" TabIndex="3">
                                    </asp:DropDownList> 
                                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlVehRegNo"
                                            PromptText="" QueryPattern="Contains">
                                        </ajaxToolkit:ListSearchExtender>--%>
                                       
                        </td>
                        </tr>
                          <%--Row2--%>
                          <tr>
                          <td colspan="1">
                           Job Card Date
                         </td>
                          <td colspan="2">
                          <asp:TextBox ID="txtJobCardDate" class="WidthAt764" runat="server" ReadOnly="true"  SkinID="ROTextBox" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                         <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server"
                             ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                             CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                             CultureTimePlaceholder="" Enabled="true" Mask="99-99-9999" TargetControlID="txtJobCardDate"
                             UserDateFormat="DayMonthYear" />
                         <asp:TextBox ID="txtCustomer" class="WidthAt764" runat="server" ReadOnly="true" SkinID="ROTextBox" placeholder="Cust ID"
                             TabIndex="8" Width="100%" Visible="false"></asp:TextBox>
                          </td>
                          <td colspan="1">
                          Job Card Type <font color="red"><b>*</b></font>
                          </td>
                          <td colspan="2">
                          <asp:DropDownList ID="ddlJobCardTypeCode" runat="server" Width="100%" AutoPostBack="true" TabIndex="1"
                                                OnSelectedIndexChanged="ddlJobCardType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <ajaxToolkit:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddlJobCardTypeCode"
                                                PromptText="" QueryPattern="Contains">
                                            </ajaxToolkit:ListSearchExtender>
                          </td>
                          <td colspan="1">
                            Payment Mode <font color="red"><b>*</b></font>
                          </td>
                          <td colspan="2">
                          <asp:DropDownList ID="ddlPayMode" runat="server" Width="100%" TabIndex="2">
                                                <asp:ListItem Selected="true" Text="Select" Value="Select"></asp:ListItem>
                                                <asp:ListItem Text="Cash" Value="Cash"></asp:ListItem>
                                                <asp:ListItem Text="Credit" Value="Credit"></asp:ListItem>
                                                <asp:ListItem Text="Intra" Value="Intra"></asp:ListItem>
                                                <asp:ListItem Text="Internal" Value="Internal"></asp:ListItem>
                                                  <asp:ListItem Text="Free of Cost" Value="FOC"></asp:ListItem>
                                                   <asp:ListItem Text="OEM" Value="OEM"></asp:ListItem>
                                                 <asp:ListItem Text="Good Will Warranty" Value="Good Will Warranty"></asp:ListItem>
                                            </asp:DropDownList>
                                            <ajaxToolkit:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddlPayMode"
                                                PromptText="" QueryPattern="Contains">
                                            </ajaxToolkit:ListSearchExtender>
                          </td>
                          
                          </tr>
                          <%--Row3--%>
                          <tr>
                          <td colspan="1">
                           Customer
                           </td>
                          <td colspan="2">
                            <asp:TextBox ID="txtCustomerDesc" class="WidthAt764" runat="server" ReadOnly="true" SkinID="ROTextBox" Width="100%" placeholder="    Customer Name" ></asp:TextBox>
                           </td>
                          <td colspan="1">
                         <asp:Label ID="Label1" runat="server" Text="Schedule"></asp:Label>
                         </td>
                          <td colspan="2">
                           <asp:DropDownList ID="ddlScheduleJC" runat="server" AutoPostBack="true" TabIndex="3" Width="100%" OnSelectedIndexChanged="ddlScheduleJC_OnSelectedIndexChanged" >
                                            </asp:DropDownList>
                                            <ajaxToolkit:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="ddlScheduleJC"
                                                PromptText="" QueryPattern="Contains">
                                            </ajaxToolkit:ListSearchExtender>
                          </td>
                          <td colspan="1">
                           Tax Policy<font color="red"><b>*</b></font>
                            <asp:ImageButton runat="server" ID="imgTaxDetail" ImageUrl="~/Images/LookupNew.png" OnClick="imgTaxDetail_Click" Visible="false" AccessKey="t"/>
                          </td>
                          <td colspan="2">
                          <asp:DropDownList ID="ddlTaxType" runat="server" Width="100%" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddlTaxType_SelectedIndexChanged"></asp:DropDownList>
                           <asp:TextBox runat="server" ID="txtTaxPolicyCode" ReadOnly="true" SkinID="ROTextBox" Width="35%" Visible="false" ></asp:TextBox>
                                             <asp:TextBox runat="server" ID="txtTaxPolicyDesc" ReadOnly="true" SkinID="ROTextBox" Width="62%" Visible="false" ></asp:TextBox>
                                             <asp:HiddenField runat="server" ID="hdnTaxType" />
                                             <asp:HiddenField runat="server" ID="hdnTaxCode" />
                                            <%--<asp:DropDownList ID="ddlVehRegNo" runat="server" AutoPostBack="true" Font-Names="monospace"
                                        Width="100%" OnSelectedIndexChanged="ddlVehRegNo_SelectedIndexChanged" class="WidthAt764" TabIndex="3">
                                    </asp:DropDownList> 
                                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlVehRegNo"
                                            PromptText="" QueryPattern="Contains">
                                        </ajaxToolkit:ListSearchExtender>--%>
                          </td>
                           <%--Display None Billing Mode --%>
                          <td style="display:none;" colspan="1">
                         Billing: <font color="red"><b>*</b></font>
                          </td>
                          <td style="display:none;" colspan="2">
                           <asp:DropDownList ID="ddlBillingType" runat="server" Width="100%" TabIndex="15">
                                                <asp:ListItem  Text="Select" Value="Select" />
                                                <asp:ListItem Text="Free" Value="Free" />
                                                <asp:ListItem Selected="true" Text="Principal" Value="Principal" />
                                                <asp:ListItem Text="Free Coupon Service" Value="Free Coupon Service" />
                                                <asp:ListItem Text="Customer Paid" Value="Customer Paid" />
                                                <asp:ListItem Text="Insurance" Value="Insurance" />
                                            </asp:DropDownList>
                                            <ajaxToolkit:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddlBillingType"
                                                PromptText="" QueryPattern="Contains">
                                            </ajaxToolkit:ListSearchExtender>
                          </td>
                           </tr>
                          <%--Row4--%>
                          <tr>
                           <td colspan="1">
                           Start Date/Time <font color="red"><b>*</b></font>
                           </td>
                           <td colspan="2">
                           <asp:TextBox ID="txtRecieptDate" class="WidthAt764" runat="server"  ReadOnly="true" SkinID="ROTextBox" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 48%;"></asp:TextBox>
                                           <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender" runat="server"
                             ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                             CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                             CultureTimePlaceholder="" Enabled="true" Mask="99-99-9999" TargetControlID="txtRecieptDate"
                             UserDateFormat="DayMonthYear" />
                                            <%--<ajaxToolkit:CalendarExtender ID="txdiveciepdivate_CalendarExtender" runat="server"
                                                Enabled="true" Format="dd-MM-yyyy" TargetControlID="txtRecieptDate" />--%>
                                                <asp:TextBox ID="txtReciptTime" runat="server" Width="48%" MaxLength="5"  TabIndex="4" Enabled="true"
                                                placeholder="00:23" class="WidthAt764"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txdiveciptTime_MaskedEditExtender" runat="server"
                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="true" Mask="99:99" MaskType="Time" TargetControlID="txtReciptTime"
                                                ClearMaskOnLostFocus="False" />
                           </td>
                           <td colspan="1">
                           Promised Date/Time<font color="red"><b>*</b></font>
                           </td>
                           <td colspan="2">
                           <asp:TextBox ID="txtPromisedDate" TabIndex="5" runat="server" CausesValidation="false" AutoComplete="false" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 48%;"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txtPromisedDate_MaskedEditExtender" runat="server"
                                                ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="true" Mask="99-99-9999" TargetControlID="txtPromisedDate"
                                                UserDateFormat="DayMonthYear" />
                                            <ajaxToolkit:CalendarExtender ID="txtPromisedDate_CalendarExtender" runat="server"
                                                Enabled="true" Format="dd-MM-yyyy" TargetControlID="txtPromisedDate" />
                                                <asp:TextBox ID="txtPromistedTime" runat="server" class="WidthAt764" Width="48%" MaxLength="5" TabIndex="6"
                           placeholder="00:23"></asp:TextBox>
                           <ajaxToolkit:MaskedEditExtender ID="txtPromistedTime_MaskedEditExtender" runat="server"
                           CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                           CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                           CultureTimePlaceholder="" Enabled="true" Mask="99:99" MaskType="Time" TargetControlID="txtPromistedTime" />
                           </td>
                           <td colspan="1">
                            Notification No/Coupon No
                            </td>
                           <td colspan="2">
                             <asp:TextBox ID="txtCouponNo" runat="server" MaxLength="15" TabIndex="7" 
                                                Width="100%"></asp:TextBox>
                            </td>
                          
                           </tr>
                          <%--Row5--%>
                          <tr>
                          <td colspan="1">
                             Category
                          </td>
                          <td colspan="2">
                           <asp:TextBox ID="txtVehicleCategory" runat="server" Width="100%" Enabled="false" SkinID="ROTextBox" ></asp:TextBox>
                          </td>
                         
                          <td colspan="1">
                          Advisor <font color="red"><b>*</b></font>
                          </td>
                          <td colspan="2">
                          <asp:Label ID="lblSIRMaster" runat="server" Visible="False"></asp:Label>
                            <asp:DropDownList ID="ddlAdvisorCode" runat="server" class="WidthAt764" Width="100%" TabIndex="8">
                            </asp:DropDownList>
                            <ajaxToolkit:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddlAdvisorCode"
                                PromptText="" QueryPattern="Contains">
                            </ajaxToolkit:ListSearchExtender>
                          </td>
                          <td colspan="1">
                          Fuel
                          </td>
                         <td colspan="2">
                           <asp:TextBox ID="txtFuel" Width="100%" runat="server" MaxLength="5" placeholder="(%)" Style="padding-right: 5px; text-align: right;" TabIndex="9"></asp:TextBox>
                         <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                         Enabled="true" FilterType="Numbers" TargetControlID="txtFuel" />
                          </td>
                          </tr>
                          <%--Row6--%>
                          <tr>
                         
                         <td colspan="1">
                           Job Card Total
                           </td>
                         <td colspan="2">
                          <asp:TextBox ID="txtJobCardTotal" runat="server" ReadOnly="true" Style="padding-right: 5px; text-align:right;"  SkinID="ROTextBox" Width="100%"></asp:TextBox>
                          </td>
                          <td colspan="1">
                            Valuables
                            </td>
                            <td colspan="2">
                              <asp:TextBox ID="txtvaluables" TabIndex="10" runat="server" MaxLength="100" TextMode="MultiLine" onkeyDown="checkTextAreaMaxLength(this,event,'99');"
                                                Width="100%"></asp:TextBox>
                            </td>
                         <td colspan="1">
                           Comments                                
                          </td>
                         <td colspan="2">
                          <asp:TextBox ID="txtRemarks" runat="server" MaxLength="100" TabIndex="11" TextMode="MultiLine" onkeyDown="checkTextAreaMaxLength(this,event,'99');"
                                                Width="100%"></asp:TextBox>
                          <asp:TextBox ID="txtSIRMasterId" runat="server" Enabled="False" Visible="False" Width="60px"></asp:TextBox>
                                        <asp:TextBox ID="txtDeliveryTime" runat="server" MaxLength="5" placeholder="00:23"
                                            Width="71px" Visible="False"></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="txdiveliveryTime_MaskedEditExtender" runat="server"
                                            ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="" Enabled="true" Mask="99:99" MaskType="Time" TargetControlID="txtDeliveryTime" />
                         </td>
                          </tr>
                          <%--Row7--%>
                          <tr>
                          <td colspan="1">
                          Manual JobCard No
                          </td>
                          <td colspan="2">
                          <asp:TextBox runat="server" ID="txtManJobCardNo" Width="100%" ></asp:TextBox>
                          </td>
                          <td colspan="1">
                          Repair Suggestion 
                          </td>
                          <td colspan="2">
                          <asp:TextBox runat="server" ID="txtRepairSugest" Width="100%" TextMode="MultiLine"></asp:TextBox>
                          </td>
                          <td colspan="1">
                          Customer Remarks
                          </td>
                          <td colspan="2">
                          <asp:TextBox runat="server" ID="txtCusRemarks" Width="100%" TextMode="MultiLine"></asp:TextBox>
                          </td>
                          </tr>
                        </table>
                   </div>
                </fieldset>
                <fieldset class="field">
             <legend>Service & Maintenance</legend>
                    <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0"   
                         Width="100%" Height="100%" >
                         <%-- Customer Vehicle Information  --%>
                        <ajaxToolkit:TabPanel ID="TabPanel6" runat="server">
                            <HeaderTemplate>Customer & Vehicle</HeaderTemplate>
                            <ContentTemplate>
                            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                           <%-- Row---1 --%>
                            <tr>
                            <td>
                            End User/Type
                            </td>
                            <td>
                             <asp:TextBox ID="txtEndUserDesc" runat="server" ReadOnly="true" SkinID="ROTextBox" Width="70%" Placeholder="  User Name"  ></asp:TextBox>
                              <asp:TextBox ID="txtCusType" runat="server" ReadOnly="true" SkinID="ROTextBox" Width="27%" Placeholder=" Type"  ></asp:TextBox>
                              <asp:TextBox ID="txtCusTypeCode" runat="server" ReadOnly="true" SkinID="ROTextBox" Visible="false" Width="27%" Placeholder=" Type" ></asp:TextBox>
                            </td>
                            <td> Cell No. </td>
                            <td >
                             <asp:TextBox ID="txtUserPhNo" runat="server" ReadOnly="true"  Width="100%" SkinID="ROTextBox" Placeholder=" Cell No" ></asp:TextBox>
                             <asp:TextBox ID="txtEndUser" runat="server" MaxLength="8" ReadOnly="true" SkinID="ROTextBox" placeholder="  User ID"   Visible="false"></asp:TextBox>
                            </td>
                            <td >  Brand </td>
                          
                            <td >
                               <asp:TextBox ID="txtBrandDesc"  runat="server" Width="100%" ReadOnly="true" SkinID="ROTextBox" Placeholder="  Brand Name" ></asp:TextBox>
                            </td>
                              </tr>
                           <%-- Row---2 --%>      
                            <tr>
                            
                              <td> Vehicle </td>
                            <td>
                            <asp:TextBox ID="txtProduct"  runat="server" Width="100%" MaxLength="10"
                                                placeholder=" Vehicle ID" ReadOnly="true" SkinID="ROTextBox" ></asp:TextBox>
                            </td>
                             <td> Description</td>
                            <td>
                              <asp:TextBox ID="txtVersionDesc" runat="server" ReadOnly="true" Width="100%" placeholder="  Description" SkinID="ROTextBox" ></asp:TextBox>
                            </td>
                            <td>Color</td>
                            <td> <asp:TextBox ID="txtColor" runat="server" Placeholder="  COLOR"  OnTextChanged="txtColor_TextChanged" SkinID="ROTextBox" ReadOnly="true" Width="100%" ></asp:TextBox>
                            </td>
                            </tr>
                          <%-- Row---3 --%>  
                            <tr>
                            <td>
                           Chassis No.
                           </td>
                          <td >
                           <asp:TextBox ID="txtChassisNo" runat="server" SkinID="ROTextBox"  Enabled="false" Width="100%" ></asp:TextBox>
                           <asp:TextBox ID="txtBrand" runat="server"  MaxLength="30" Visible="false"
                           Placeholder="Brand ID" ReadOnly="true" ></asp:TextBox>
                          </td>
                          <td>
                           Engine No.
                          </td>
                          <td>
                           <asp:TextBox ID="txtEngineNo" runat="server" SkinID="ROTextBox" Width="100%" Enabled="false" ></asp:TextBox>
                          </td>
                           
                           
                            <td>
                            Credit Days
                            </td>
                           
                            <td>
                            <asp:TextBox ID="txtCreditDays"  runat="server" readonly="true" SkinID="ROTextBox" onkeyDown="checkTextAreaMaxLength(this,event,'99');"
                                                Width="100%"></asp:TextBox>
                            </td>
                             </tr>
                          <%-- Row---4 --%>  
                            <tr>
                            <td>
                            Credit Limit
                            </td>
                            <td>
                            <asp:TextBox ID="txtCreditLimit"  runat="server"  readonly="true" SkinID="ROTextBox"   onkeyDown="checkTextAreaMaxLength(this,event,'99');"
                                                Width="100%"></asp:TextBox>
                            </td>
                             <td>
                            Balance
                            </td>
                            <td>
                             <asp:TextBox ID="txtOutStanding"  runat="server"  readonly="true" SkinID="ROTextBox" onkeyDown="checkTextAreaMaxLength(this,event,'99');"
                                                Width="100%"></asp:TextBox> (Consider from last Invoice)
                            </td>
                             <td >
                           Previous KM
                           </td>
                          <td >
                           <asp:TextBox ID="txtPreviousKM"  runat="server" Width="100%" Enabled="false" SkinID="ROTextBox"></asp:TextBox>
                          </td>
                            </tr>
                          <%-- Row---5 --%>  
                            <tr>
                           
                            <td>
                             K.M /  AVG KM Per Day<font color="red"><b>*</b></font>
                            </td>
                            <td>
                             <asp:TextBox ID="txtKM" runat="server"  MaxLength="41" TabIndex="12" OnTextChanged="txtKM_TextChanged"
                                                Style="background-image: url(../../../Images/KmIcon.png); background-repeat: no-repeat; padding-left: 25px; width: 48%;"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"
                                                Enabled="true" FilterType="Numbers" TargetControlID="txtKM" />
                                            <asp:TextBox ID="txtAvgKm" Width="48%" runat="server" MaxLength="4" TabIndex="13"></asp:TextBox>

                            </td>
                            <td>
                            Contact Person<font color="red"><b>*</b></font>
                            </td>
                            <td>
                              <asp:TextBox ID="txtCntPerName"  runat="server" Width="100%" MaxLength="50"
                                                placeholder=" Description" TabIndex="14"></asp:TextBox>
                            </td>
                            <td>
                            Person Cell No.<font color="red"><b>*</b></font>
                            </td>
                            <td>
                              <asp:TextBox ID="txtCntPerCell"  runat="server"  Width="100%" MaxLength="13" placeholder=" Cell No" TabIndex="15"></asp:TextBox>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" FilterType="Numbers"
                                            TargetControlID="txtCntPerCell">
                                          </ajaxToolkit:FilteredTextBoxExtender>
                            </td>
                            </tr>
                            </table>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                         <%-- Jobs Information Tab   --%>
                        <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Sele Person Information">
                            <HeaderTemplate>Labor Task</HeaderTemplate>
                            <ContentTemplate>
                            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                           <%-- Row 1 --%>
                            <tr>
                            <td>
                            Labor <asp:ImageButton runat="server" ID="imgJobLookUp" ImageUrl="~/Images/LookupNew.png" OnClick="imgJobLookUp_Click"  TabIndex="45" AccessKey="k"/>
                            </td>
                            <td>
                            <asp:TextBox ID="txtJobs_Labor" runat="server" ReadOnly="true" Width="25%" TabIndex="46"></asp:TextBox>
                             <asp:TextBox ID="txtJobsDesc" runat="server" ReadOnly="true" Width="70%" TabIndex="47"></asp:TextBox>
                              <%--<asp:DropDownList ID="ddlJobs_Labor" runat="server" Width="100%" Font-Names="monospace" TabIndex="21"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlJobs_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <ajaxToolkit:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="ddlJobs_Labor"
                                                PromptText="" QueryPattern="Contains" BehaviorID="_content_ListSearchExtender9">
                                            </ajaxToolkit:ListSearchExtender>--%>
                            </td>
                            <td>
                              Amount <font color="red"><b>*</b></font>
                            </td>
                            <td>
                             <asp:TextBox ID="txtLabor" runat="server" MaxLength="6" OnTextChanged="txtLabor_TextChanged"
                                                    Style="text-align: right" TabIndex="48" Width="100%">0</asp:TextBox><ajaxToolkit:FilteredTextBoxExtender
                                                        ID="FilteredTextBoxExtender6" runat="server" BehaviorID="_content_txtLaborExtender" FilterType="Numbers"
                                                        TargetControlID="txtLabor" />
                            </td>
                           
                           <td>
                              <asp:ImageButton ID="BtnClearJobs" runat="server" Height="33px" ImageUrl="~/Images/Clear5.png" 
                                        OnClick="BtnClearJobs_Click" Width="33px" TabIndex="51" /> </td>
                            </tr>
                           <%-- Row 2 --%>
                            <tr>
                            <td>Bay</td>
                            <td> <asp:DropDownList ID="ddlBay" runat="server" Width="100%" TabIndex="49">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtJobRemarks" Visible="False" runat="server" Height="19px" MaxLength="200" 
                                                    Width="100%"></asp:TextBox></td>
                            <td>
                            Technician<font color="red"><b>*</b></font>
                            </td>
                            <td>
                             <asp:DropDownList ID="ddlTechnicianEmpCode" runat="server" AutoPostBack="true" Width="100px" TabIndex="50">
                                                </asp:DropDownList>
                                                <ajaxToolkit:ListSearchExtender ID="ListSearchExtender15" runat="server" TargetControlID="ddlTechnicianEmpCode"
                                                    PromptText="" QueryPattern="Contains" BehaviorID="_content_ListSearchExtender15">
                                                </ajaxToolkit:ListSearchExtender>
                            </td>
                           <td><asp:ImageButton ID="BtnAddJobs" runat="server" Height="33px" ImageUrl="~/Images/AddIMG.png"
                                       Width="33px" OnClick="BtnAddJobs_Click"  TabIndex="50"/></td>
                            
                            </tr>
                             <%-- Row 3 --%>
                             <tr>
                               <td>Job Type</td>
                                   <td><asp:DropDownList runat="server" ID="ddlJobType" Visible="false" >
                                   <asp:ListItem Value="J" Selected="True">Normal</asp:ListItem>
                                   <asp:ListItem Value="S">Suplementary</asp:ListItem>
                                       </asp:DropDownList></td>
                             <td style="text-align:right">

                             
                             <asp:Button Width="100%" runat="server" ID="btnUpdJobs" OnClick="btnUpdJobs_Click" Visible="false"  Text="Update Estimate Jobs"/>
                           </td>
                             </tr>
                           <%-- Row 4 --%>
                            <tr>
                            <td colspan="6">
                            <div style="width: 100%; height: 100px; overflow: scroll">
                               <asp:GridView ID="gvJobCard" runat="server" AutoGenerateColumns="False" BackColor="White" HeaderStyle-CssClass="FixedHeader"
                                                    CellPadding="4" ForeColor="Black" OnSelectedIndexChanged="gvJobCard_SelectedIndexChanged"
                                                    ShowHeaderWhenEmpty="True" Width="100%" OnRowDataBound="gvJobCard_RowDataBound">
                                                    <Columns>
                                                        <asp:CommandField ShowSelectButton="True"></asp:CommandField>
                                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                        <asp:BoundField DataField="JobCode" HeaderText="JobCode">
                                                            <ItemStyle Width="60px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DefJobDesc" HeaderText="Job Description(s)">
                                                            <ItemStyle Wrap="True" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="EmpCode" HeaderText="TechCode" Visible="true">
                                                            <ItemStyle Width="60px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="EmpName" HeaderText="Technician">
                                                            <ItemStyle Wrap="True" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Type" HeaderText="Type">
                                                            <ItemStyle Width="60px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Bay" HeaderText="Bay ID" Visible="False">
                                                            <ItemStyle Width="60px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="BayDesc" HeaderText="Bay">
                                                            <ItemStyle Width="200px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Amount" HeaderText="Amount">
                                                            <ItemStyle Width="60px" />
                                                        </asp:BoundField>
                                                        
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <%--<asp:ImageButton ID="BtnRemove_Labor" runat="server" CausesValidation="false" ImageUrl="~/Images/Delete_16x16.png" onclick="BtnRemove_Labor_Click" ToolTip="Remove" />--%>
                                                                <asp:ImageButton ID="lnkRemove" runat="server" CausesValidation="false" ImageUrl="~/Images/Delete_16x16.png" OnClick="lnkRemove_Click1" ToolTip="Remove" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%" HorizontalAlign=" center"></ItemStyle>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <AlternatingRowStyle BackColor="#CCCCCC" />
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <FooterStyle CssClass="GridFooter" />
                                                    <SelectedRowStyle BackColor="WhiteSmoke" Font-Bold="False" ForeColor="Black" />
                                                </asp:GridView>
                            </div>
                          
                            </td>
                            </tr>
                          
                          
                            </table>
                            <table  width="100%" class="table table-condensed table-bordered table-hover">
                              <tr>
                            <td >
                              <asp:Label ID="Label45" runat="server" Text="Total Jobs" Width="100px"></asp:Label>
                            </td>
                            <td >
                             <asp:Label ID="txtJobsTotal" runat="server" Text="0" Width="50px"></asp:Label>
                            </td >
                            <td >
                              <asp:Label ID="Label113" runat="server" Text="Total Labor" Width="100px"></asp:Label>
                            </td>
                            <td >
                            <asp:Label ID="lblJobCardTotal" runat="server" Text="0" Width="50px"></asp:Label>
                            </td>
                            </tr>
                            </table>
                          
                           
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                          <%-- Parts Information Tab   --%>
                        <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Corporate / Institution  ">
                            <HeaderTemplate>Parts</HeaderTemplate>
                            <ContentTemplate>
                             <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                             <%-- Row 1 --%>
                             <tr>
                             <td>
                              Item Code  <asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png" OnClick="btnLookup_Click" AccessKey="i" />
                              <font color="red"><b>*</b></font>
                             </td>
                             <td>
                              <asp:TextBox ID="txtItemCode" runat="server" Width="100%" ReadOnly="true" SkinID="ROTextBox" ></asp:TextBox>
                             </td>
                             <td>
                             Item No
                             </td>
                             <td>
                               <asp:TextBox ID="txtPartItemNo_Parts" runat="server" Width="100%" ReadOnly="true" SkinID="ROTextBox" ></asp:TextBox>
                                                <%--<asp:DropDownList ID="ddlPartCode" runat="server" Font-Names="monospace" Width="400px"
                                            OnSelectedIndexChanged="ddlPartCode_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>--%>
                                                <%--<ajaxToolkit:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="ddlPartCode"
                                            PromptText="" QueryPattern="Contains" BehaviorID="_content_ListSearchExtender10">
                                        </ajaxToolkit:ListSearchExtender>--%>
                             </td>
                             <td>
                             Item Description
                             </td>
                             <td>
                                <asp:TextBox ID="txtItemDesc" runat="server" Width="100%" ReadOnly="true" SkinID="ROTextBox" ></asp:TextBox>
                             </td>
                              <asp:TextBox ID="txtPartsRecQuantity" runat="server" Enabled="False" Visible="False" Style="text-align: right"
                                                Width="60px">0</asp:TextBox>
                             <td>
                              <asp:ImageButton ID="btnClearParts" runat="server" Height="33px" ImageUrl="~/Images/Clear5.png" 
                                        OnClick="btnClearParts_Click" Width="33px" />
                             </td>
                             </tr>
                             <%-- Row 2 --%> 
                             <tr>
                             <td>
                              Quantity<font color="red"><b>*</b></font>
                             </td>
                             <td>
                             <asp:TextBox ID="txtQuantity" runat="server" MaxLength="3" Style="text-align: right" onkeypress="return IsOneDecimalPoint(event);"
                                                    Width="100%"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender
                                                    ID="FilteredTextBoxExtender1" runat="server" BehaviorID="_content_FilteredTextBoxExtender1"
                                                    FilterType="Custom"  ValidChars="01234567890" TargetControlID="txtQuantity" />
                             </td>
                             <td>
                              Price<font color="red"><b>*</b></font>
                             </td>
                             <td>
                             <asp:TextBox ID="txtPartPrice" runat="server" MaxLength="10" Style="text-align: right"
                                                    ValidationGroup="JCParts" Width="100%">0</asp:TextBox>
                             </td>
                             <td>
                               Dep %
                             </td>
                             <td>
                                <asp:TextBox ID="txtDep" runat="server" MaxLength="2" Style="text-align: right" Width="100%"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender
                                                    ID="FilteredTextBoxExtender3" runat="server" BehaviorID="_content_FilteredTextBoxExtender3"
                                                    FilterType="Numbers" TargetControlID="txtDep" />
                             </td>
                             <td>
                             <asp:ImageButton ID="btnAddParts" runat="server" Height="33px" ImageUrl="~/Images/AddIMG.png"
                                       Width="33px" OnClick="btnAddParts_Click" />
                             </td>
                             </tr>
                              <%-- Row 3 Update from Estimate Grid --%>   
                              <tr>
                         
                             
                        
                               <td > 
                                <asp:HiddenField runat="server" ID="lblPartType" ></asp:HiddenField>
                                <asp:Button Width="100%" runat="server" ID="btnUpdParts" OnClick="btnUpdParts_Click1" Visible="false"  Text="Update Estimate Parts"/></td>

                              </tr>
                           <%-- Row 4 Item Grid --%>   
                             <tr>
                           <td colspan="7">
                            <div style="width: 100%; height: 100px; overflow: scroll">
                            <asp:GridView ID="gvJobCardParts" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="gvJobCardParts_SelectedIndexChanged"
                                                ShowHeaderWhenEmpty="True" Width="100%" OnRowDataBound="gvJobCardParts_RowDataBound"
                                                BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                                CellPadding="3" ForeColor="Black" GridLines="Vertical">
                                                <AlternatingRowStyle BackColor="#CCCCCC" />
                                                <Columns>
                                                    <asp:CommandField ShowSelectButton="True" />
                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                    <asp:BoundField DataField="ItemCode" HeaderText="Item Code" />
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No.">
                                                        <ItemStyle Width="150px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PartsDesc" HeaderText="Part Description" />
                                                    <asp:BoundField DataField="Dep" HeaderText="Depriciation" />
                                                    <asp:BoundField DataField="DepAmount" HeaderText="Dep Amount" />
                                                    <asp:BoundField DataField="RecQty" HeaderText="Issued Qty">
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
                                                   

                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lnkRemove" runat="server" ImageUrl="~/Images/Delete_16x16.png" OnClick="lnkRemove_Click"></asp:ImageButton>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="5%" HorizontalAlign=" center"></ItemStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="GridHeader" />
                                                <FooterStyle CssClass="GridFooter" />
                                            </asp:GridView>
                            </div>
                            
                           </td>
                           </tr> 
                             </table>
                             <table  width="100%" class="table table-condensed table-bordered table-hover">
                             <tr>
                              <td>
                                <asp:Label ID="Label56" runat="server" Text="Total Parts" Width="81px"></asp:Label>
                              </td>
                              <td>
                                <asp:Label ID="txtPartTotalQuantity" runat="server" Text="0" Width="60px"></asp:Label>
                              </td>
                              <td>
                                  <asp:Label ID="Label118" runat="server" Text="Total Value" Width="70px"></asp:Label>
                              </td>
                              <td>
                               <asp:Label ID="lblPartsTotal" runat="server" Text="0" Width="60px"></asp:Label>
                              </td>
                             </tr>
                             <tr>
                               
                            <td>
                             <asp:LinkButton runat="server" ID="lknStock" Text="Part Requisition" Visible="false" OnClick="lknStock_Click"></asp:LinkButton>
                            <asp:TextBox
                              ID="txttotPartsRecQty" runat="server" Enabled="False" Width="60px" Visible="False"></asp:TextBox>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                             </tr>

                              </table>
                              
                               
                           
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                         <%-- Lubricants Information Tab   --%>
                        <ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Customer Tag  ">
                            <HeaderTemplate>Lubricants</HeaderTemplate>
                            <ContentTemplate>
                         <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                         <%-- Row1 --%>
                           <tr>
                           <td>
                           Lubricant Code <asp:ImageButton runat="server" ID="imgLubricant" ImageUrl="~/Images/LookupNew.png" OnClick="imgLubricant_Click" AccessKey="o" />
                           </td>
                           <td>
                            <%--<asp:DropDownList ID="ddlLubCode" runat="server" Font-Names="monospace" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlLubCode_SelectedIndexChanged">
                                                </asp:DropDownList><ajaxToolkit:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="ddlLubCode"
                                                    PromptText="" QueryPattern="Contains">
                                                </ajaxToolkit:ListSearchExtender>--%>
                            <asp:TextBox ID="txtLubCode" runat="server" ReadOnly="True" SkinID="ROTextBox"  Width="100%"></asp:TextBox>
                                              
                           </td>
                           <td>
                           Lubricant Desc
                           </td>
                           <td>
                             <asp:TextBox ID="txtLubPartDesc" runat="server" ReadOnly="True" SkinID="ROTextBox" Width="100%"></asp:TextBox>
                                                <asp:TextBox ID="txtPartItemNo_Lubs" runat="server" Visible="False"></asp:TextBox>
                                                <asp:TextBox ID="txtLubRecQuantity" runat="server" Visible="False" Enabled="False">0</asp:TextBox>
                           </td>
                          <td>
                           <asp:ImageButton ID="btnClearLubs" runat="server" Height="33px" ImageUrl="~/Images/Clear5.png" CssClass="ClearButton"
                                        Width="33px" OnClick="btnClearLubs_Click" />
                           </td>
                           </tr>
                         <%-- Row2 --%>  
                           <tr>
                           <td>
                             Quantity<font color="red"><b>*</b></font>
                           </td>
                           <td>
                             <asp:TextBox ID="txtLubQuantity" runat="server" MaxLength="3" Style="text-align: right" onkeypress="return IsOneDecimalPoint(event);"
                                                    Width="100%"></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"
                                                        runat="server" BehaviorID="_content_FilteredTextBoxExtender5" FilterType="Custom"  ValidChars="01234567890"
                                                        TargetControlID="txtLubQuantity" />
                                                          <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                                                            ControlToValidate="txtLubQuantity" runat="server"
                                                            ErrorMessage="Only Numbers allowed"
                                                            ValidationExpression="\d+">
                                                        </asp:RegularExpressionValidator>
                           </td>
                           <td>
                            Price<font color="red"><b>*</b></font>
                           </td>
                           <td>
                            <asp:TextBox ID="txtLubPrice" runat="server" MaxLength="6" Enabled="True" Style="text-align: right"
                                                    Width="100%"></asp:TextBox>
                                               <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                                            ControlToValidate="txtLubPrice" runat="server"
                                                            ErrorMessage="Only Numbers allowed"
                                                            ValidationExpression="\d+">
                                                        </asp:RegularExpressionValidator>--%>
                           </td>
                            <td>
                           <asp:ImageButton ID="btnAddLubs" runat="server" Height="33px" ImageUrl="~/Images/AddIMG.png"
                                       Width="33px" OnClick="btnAddLubs_Click" />
                           </td>
                           
                           </tr>
                            <%-- Row 3 Update from Estimate Grid --%>   
                              <tr>
                              <td>
                              Dep
                              </td>
                              <td>
                              <asp:TextBox ID="txtLubDep" runat="server" Width="100%"></asp:TextBox>
                              </td>
                               <asp:HiddenField runat="server" ID="lblLubType" ></asp:HiddenField>
                               <td>  <asp:Button Width="100%" runat="server" ID="btnUpdLub" OnClick="btnUpdLub_Click" Visible="false"  Text="Update Estimate Lube"/></td>

                              </tr>
                         <%-- Row4 Grid Lubricant  --%>
                         <tr>
                         <td colspan="6">
                           <div style="width: 100%; height: 100px; overflow: scroll">
                          <asp:GridView ID="gvLubParts" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                                CellPadding="4" Style="text-align: left" Width="100%" OnSelectedIndexChanged="gvLubParts_SelectedIndexChanged"
                                                ShowHeaderWhenEmpty="True" OnRowDataBound="gvLubParts_RowDataBound" BackColor="White">
                                                <AlternatingRowStyle BackColor="#CCCCCC" />
                                                <Columns>
                                                    <asp:CommandField ShowSelectButton="True"></asp:CommandField>
                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                    <asp:BoundField DataField="ItemCode" HeaderText="ItemCode">
                                                        <ItemStyle Width="60px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="133px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PartsDesc" HeaderText="Parts Description">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RecQty" HeaderText="Rec Qty">
                                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                    </asp:BoundField>
                                                      <asp:BoundField DataField="Dep" HeaderText="Dep">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                    </asp:BoundField>
                                                      <asp:BoundField DataField="DepAmount" HeaderText="DepAmount">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Price" HeaderText="Price">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Total" HeaderText="Total">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                    </asp:BoundField>
                                                     <asp:BoundField DataField="Type" HeaderText="Type">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lnkRemove" ImageUrl="~/Images/Delete_16x16.png" runat="server" OnClick="lnkRemove_Click2"></asp:ImageButton>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <AlternatingRowStyle BackColor="#CCCCCC" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <FooterStyle CssClass="GridFooter" />
                                            </asp:GridView>
                                            </div>
                         </td>
                         </tr>       
                            </table>
                         <table  width="100%" class="table table-condensed table-bordered table-hover">
                             <tr>
                             <td>
                              <asp:Label ID="Label119" runat="server" Text="Total Lubricants" Width="100px"></asp:Label>
                             </td>
                             <td>
                              <asp:Label ID="lbltotLubQty" runat="server" Text="0" Width="60px" Height="16px"></asp:Label>
                             </td>
                             <td>
                              <asp:Label ID="Label102" runat="server" Text="Total Value" Width="100px"></asp:Label>
                             </td>
                             <td>
                             <asp:Label ID="txtLubTotal" runat="server" Text="0" Width="50px"></asp:Label>
                                                <asp:TextBox ID="txttotLubRecQty" runat="server" Enabled="False" Width="60px" Visible="False" Style="text-align: right">0</asp:TextBox>
                             </td>
                             </tr>
                             </table>
                                
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                          <%-- Sublet Information Tab   --%>
                        <ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="Sublet Job">
                            <HeaderTemplate>
                                Sublet Job
                            </HeaderTemplate>
                            <ContentTemplate>
                            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                           <%-- Row1 --%>
                           <tr>
                           <td>
                            Vendor <font color="red"><b>*</b></font>
                           </td>
                           <td>
                            <asp:DropDownList ID="ddlVendorCode" runat="server">
                                                </asp:DropDownList>
                                                <ajaxToolkit:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="ddlVendorCode"
                                                    PromptText="" QueryPattern="Contains">
                                                </ajaxToolkit:ListSearchExtender>
                           </td>
                           <td>
                             Job <font color="red"><b>*</b></font>
                                                <asp:ImageButton runat="server" ID="imgJobSubLookUp" ImageUrl="~/Images/LookupNew.png" OnClick="imgJobSubLookUp_Click" AccessKey="n" />
                                            
                           </td>
                           <td>
                           <asp:TextBox ID="txtSubletJobCode" runat="server" ReadOnly="true" Width="100%"></asp:TextBox>
                           </td>
                           <td>
                           Job Description
                           </td>
                           <td>
                            <%--<asp:DropDownList ID="ddlJobs_Sublet" runat="server" Font-Names="monospace" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlJobs_Sublet_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <ajaxToolkit:ListSearchExtender ID="ListSearchExtender13" runat="server" TargetControlID="ddlJobs_Sublet"
                                                    PromptText="" QueryPattern="Contains">
                                                </ajaxToolkit:ListSearchExtender>--%>
                           <asp:TextBox ID="txtSubletJobDesc" runat="server" Width="100%"></asp:TextBox>
                           </td>
                         
                            <td>
                          <asp:ImageButton ID="btnClearSublet" runat="server" Height="33px" ImageUrl="~/Images/Clear5.png" 
                                        Width="33px" OnClick="btnClearSublet_Click" />
                          </td>
                           </tr>
                           <%-- Row2 --%>
                           <tr>
                          <td>
                           Date <font color="red"><b>*</b></font>
                          </td>
                          <td>
                           <asp:TextBox ID="txtSubletDate" runat="server" Width="100%"></asp:TextBox><ajaxToolkit:CalendarExtender
                                                    ID="txtSubletDate_CalendarExtender" runat="server" BehaviorID="_content_txtSubletDate_CalendarExtender"
                                                    Format="dd-MM-yyyy" TargetControlID="txtSubletDate" />
                          </td>
                          <td>
                            Payable Amount <font color="red"><b>*</b></font>
                          </td>
                          <td>
                          <asp:TextBox ID="txtSubletPayAmnt" runat="server" MaxLength="6" Style="text-align: right" AutoPostBack="true" CausesValidation="false" OnTextChanged="txtSubletPayAmnt_TextChanged"
                        
                                                    Width="100%"></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8"
                                                        runat="server" BehaviorID="_content_FilteredTextBoxExtender8" FilterType="Numbers"
                                                        TargetControlID="txtSubletPayAmnt" />
                          </td>
                          <td>
                           Invoice Amount <font color="red"><b>*</b></font>
                          </td>
                          <td>
                           <asp:TextBox ID="txtSubletIncAmnt" runat="server" MaxLength="6" Style="text-align: right"
                                                    Width="100%"></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7"
                                                        runat="server" BehaviorID="_content_FilteredTextBoxExtender7" FilterType="Numbers"
                                                        TargetControlID="txtSubletIncAmnt" />
                            <asp:TextBox ID="txtSubletRemarks" runat="server" Width="100%" Visible="false"></asp:TextBox>
                             <%--<asp:DropDownList ID="ddlBaySubletJob" runat="server" Width="100%"></asp:DropDownList>--%>
                          </td>
                           <td>
                            <asp:ImageButton ID="btnAddSublet" runat="server" Height="33px" ImageUrl="~/Images/AddIMG.png"
                                       Width="33px" OnClick="btnAddSublet_Click" />
                           </td>
                           </tr>
                             <%-- Row 3 Update from Estimate Grid --%>   
                              <tr>
                               <td>  <asp:Button Width="100%" runat="server" ID="btnUpdSublet" OnClick="btnUpdSublet_Click" Visible="false"  Text="Update Estimate Sublet"/></td>

                              </tr>
                           <%-- Row4 Sublet Grid --%>
                           <tr>
                           <td colspan="7">
                             <div style="width: 100%; height: 100px; overflow: scroll">
                            <asp:GridView ID="gvSublet" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderStyle="None" CellPadding="4" OnSelectedIndexChanged="gvSublet_SelectedIndexChanged"
                                                ShowHeaderWhenEmpty="True" Style="text-align: left" Width="100%" OnRowDataBound="gvSublet_RowDataBound">
                                                <Columns>
                                                    <asp:CommandField ShowSelectButton="True"></asp:CommandField>
                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                    <asp:BoundField DataField="VendorCode" HeaderText="Vendor Code">
                                                        <ItemStyle Width="60px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Vendor" HeaderText="Vendor" />
                                                    <asp:BoundField DataField="JobCode" HeaderText="JobCode">
                                                        <ItemStyle Width="60px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="JobDescription" HeaderText="Job Description" />
                                                    <%--<asp:BoundField DataField="BayID" HeaderText="Bay" />--%>
                                                    <asp:BoundField DataField="SubletDate" HeaderText="Date">
                                                        <ItemStyle Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SubletAmount" HeaderText="Invoiced Amt"></asp:BoundField>
                                                    <asp:BoundField DataField="PayableAmount" HeaderText="Payable Amt"></asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lnkRemove" ImageUrl="~/Images/Delete_16x16.png" runat="server" OnClick="lnkRemove_Click3"></asp:ImageButton>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                                        <%--<asp:LinkButton ID="lnkRemove" runat="server" OnClick="lnkRemove_Click3">Remove</asp:LinkButton>--%>                                                    
                                                    </asp:TemplateField>
                                                </Columns>

                                                <HeaderStyle CssClass="GridHeader" />
                                                <FooterStyle CssClass="GridFooter" />
                                            </asp:GridView></div>
                           </td>
                           </tr>
                            </table>
                            <table  width="100%" class="table table-condensed table-bordered table-hover">
                               <tr>
                               <td>
                                <asp:Label ID="Label120" runat="server" Height="16px" Text="Total Payable Amount"></asp:Label>
                               </td>
                               <td>
                                  <asp:Label ID="txtSubletPayableTotal" runat="server" Height="16px" Text="0"></asp:Label>
                               </td>
                               <td>
                                 <asp:Label ID="Label77" runat="server" Height="16px" Text="Total Invoice Amount"></asp:Label>
                               </td>
                               <td>
                                <asp:Label ID="txtSubletTotal" runat="server" Height="16px" Text="0"></asp:Label>
                               </td>
                               </tr>
                               </table>   
                              
                                
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                          <%-- Consumable Parts Information Tab   --%>
                        <ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="Consumable Parts">
                            <HeaderTemplate>Consumable Parts</HeaderTemplate>
                            <ContentTemplate>
                              <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                             <%-- Row1 --%>
                              <tr>
                              <td>
                               Part Code  <asp:ImageButton runat="server" ID="ImgConPart" ImageUrl="~/Images/LookupNew.png" OnClick="ImgConPart_Click" AccessKey="m" /><font color="red"><b>*</b></font>
                              </td>
                              <td>
                              <asp:TextBox ID="txtConPartCode" runat="server" Width="100%"></asp:TextBox>
                              </td>
                              <td>
                              Part Description
                              </td>
                              <td>
                               <asp:TextBox ID="txtConPartDesc" Width="100%" runat="server" ReadOnly="True"></asp:TextBox>
                                                <asp:DropDownList ID="ddlConsumableParts" runat="server" Visible="false" Font-Names="monospace" OnSelectedIndexChanged="ddlConsumableParts_SelectedIndexChanged"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                                <ajaxToolkit:ListSearchExtender ID="ListSearchExtender14" runat="server" TargetControlID="ddlConsumableParts"
                                                    PromptText="" QueryPattern="Contains" BehaviorID="_content_ListSearchExtender14">
                                                </ajaxToolkit:ListSearchExtender>
                              </td>
                               <td>
                               <asp:ImageButton ID="btnClearConsumablePart" runat="server" Height="33px" ImageUrl="~/Images/Clear5.png" CssClass="ClearButton"
                                        Width="33px" OnClick="btnClearConsumablePart_Click" />
                              </td>
                              
                              </tr>
                              <%-- Row2 --%>
                              <tr>
                              <td>
                                Price<font color="red"><b>*</b></font>
                              </td>
                              <td>
                              <asp:TextBox ID="txtConPartPrice" runat="server" MaxLength="6" Style="text-align: right"
                                                    ValidationGroup="JCParts" Width="100%">0</asp:TextBox>
                              </td>
                              <td>
                               Quantity<font color="red"><b>*</b></font>
                              </td>
                              <td>
                               <asp:TextBox ID="txtConQuantity" runat="server" MaxLength="3" Style="text-align: right" onkeypress="return IsOneDecimalPoint(event);"
                                                    ValidationGroup="JCParts" Width="100%"></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender
                                                        ID="txtConQuantity_FilteredTextBoxExtender" runat="server" BehaviorID="txtConQuantity_FilteredTextBoxExtender"
                                                        FilterType="Custom" ValidChars="01234567890"  TargetControlID="txtConQuantity" />
                              <asp:TextBox ID="txtConPartsRecQuantity" runat="server" Visible="false" Enabled="False" Style="text-align: right" Width="100%">0</asp:TextBox>
                              <asp:TextBox ID="txtPartItemNoConParts" Visible="false" runat="server" Enabled="False" Width="100%"></asp:TextBox>
                              </td>
                             
                              <td>
                              <asp:ImageButton ID="btnAddConsumablePart" runat="server" Height="33px" ImageUrl="~/Images/AddIMG.png"
                                       Width="33px" OnClick="btnAddConsumablePart_Click" />
                              </td>
                              </tr>
                              <%-- Row3 Consumable Parts Grids --%>
                              <tr>
                              <td colspan="6">
                                <div style="width: 100%; height: 100px; overflow: scroll">
                                <asp:GridView ID="gvJobCardConParts" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                                Width="100%" OnRowDataBound="gvJobCardConParts_RowDataBound" OnSelectedIndexChanged="gvJobCardConParts_SelectedIndexChanged"
                                                BackColor="White" CellPadding="3" PageSize="3">
                                                <AlternatingRowStyle BackColor="#CCCCCC" />
                                                <Columns>
                                                    <asp:CommandField ShowSelectButton="True" />
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
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lnkRemove" runat="server" ImageUrl="~/Images/Delete_16x16.png" OnClick="lnkRemove_Click4"></asp:ImageButton>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="5%" HorizontalAlign=" center"></ItemStyle>
                                                        <%--<ItemTemplate>
                                                        <asp:LinkButton ID="lnkRemove" runat="server" OnClick="lnkRemove_Click4">Remove</asp:LinkButton></ItemTemplate>
                                                    <ItemStyle Width="5%"></ItemStyle>--%>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="GridHeader" />
                                                <FooterStyle CssClass="GridFooter" />
                                            </asp:GridView></div>
                              </td>
                              </tr>
                              </table>
                              <table  width="100%" class="table table-condensed table-bordered table-hover">
                              <tr>
                              <td>
                               <asp:Label ID="Label8" runat="server" Text="Total Parts" Width="81px"></asp:Label>
                              </td>
                              <td>
                                <asp:Label ID="txtConPartTotalQuantity" runat="server" Text="0" Width="60px"></asp:Label>
                              </td>
                              <td>
                                <asp:Label ID="Label10" runat="server" Text="Total Value" Width="70px"></asp:Label>
                              </td>
                              <td>
                               <asp:Label ID="txtConPartsTotal" runat="server" Text="0" Width="60px"></asp:Label>
                                                <asp:TextBox ID="txttotConPartsRecQty" runat="server" Enabled="False" Width="60px" Visible="False">0</asp:TextBox>
                              </td>
                              </tr>
                              </table>
                               
                             
                             

                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                         <%-- BoughtOut Parts Information Tab   --%>
                        <ajaxToolkit:TabPanel ID="TabPanel8" runat="server" HeaderText="Corporate / Institution  ">
                            <HeaderTemplate>Bought Out Part</HeaderTemplate>
                            <ContentTemplate>
                             <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                             <%-- Row1 --%>
                              <tr>
                              <td>
                              Item Code<asp:ImageButton runat="server" ID="ImgBoughtOutPart" ImageUrl="~/Images/LookupNew.png" OnClick="ImgBoughtOutPart_Click" AccessKey="b" />
                              <font color="red"><b>*</b></font>
                              </td>
                              <td>
                               <asp:TextBox ID="txtBItemCode" runat="server" Enabled="false" Width="100%"></asp:TextBox>
                              </td>
                              <td>
                              Item No
                              </td>
                              <td>
                                     <asp:TextBox ID="txtBPartNo" runat="server" Enabled="false" Width="100%"></asp:TextBox>
                                                <%--<asp:DropDownList ID="ddlPartCode" runat="server" Font-Names="monospace" Width="400px"
                                            OnSelectedIndexChanged="ddlPartCode_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>--%>
                                                <%--<ajaxToolkit:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="ddlPartCode"
                                            PromptText="" QueryPattern="Contains" BehaviorID="_content_ListSearchExtender10">
                                        </ajaxToolkit:ListSearchExtender>--%>
                              </td>
                              <td>
                              Item Description
                              </td>
                              <td>
                                    <asp:TextBox ID="txtBItemDesc" runat="server" Enabled="false" Width="100%"></asp:TextBox>
                                     <asp:TextBox ID="TextBox4" runat="server" Enabled="False" Visible="False" Style="text-align: right"
                                                Width="60px">0</asp:TextBox>
                              </td>
                              <td>
                               <asp:ImageButton ID="BClearParts" runat="server" Height="33px" ImageUrl="~/Images/Clear5.png" 
                                        OnClick="BClearParts_Click" Width="33px" />
                              </td>
                             </tr>
                             <%-- Row2 --%>
                              <tr>
                               <td>
                                Quantity<font color="red"><b>*</b></font>
                              </td>
                              <td>
                              <asp:TextBox ID="txtBQty" runat="server" MaxLength="3" AutoPostBack="true" Style="text-align: right" OnTextChanged="txtBQty_TextChanged"
                                                    ValidationGroup="JCParts" Width="100%"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender
                                                    ID="FilteredTextBoxExtender9" runat="server" BehaviorID="_content_FilteredTextBoxExtender1"
                                                    FilterType="Numbers" TargetControlID="txtBQty" />
                              </td>
                              <td>
                               Purchase Rate <font color="red"><b>*</b></font>
                              </td>
                              <td>
                              <asp:TextBox ID="txtBPrice" runat="server" Enabled="false" MaxLength="6" Style="text-align: right"
                                                    ValidationGroup="JCParts" Width="100%">0</asp:TextBox>
                              </td>
                              <td>
                                 Invoice Rate
                              </td>
                              <td>
                                <asp:TextBox ID="txtBAmt" runat="server" Style="text-align: right" Enabled="true" Width="100%" ></asp:TextBox>
                                              <%--  <ajaxToolkit:FilteredTextBoxExtender
                                                    ID="FilteredTextBoxExtender10" runat="server" BehaviorID="_content_FilteredTextBoxExtender3"
                                                    FilterType="Numbers" TargetControlID="txtBAmt" />--%>
                              </td>
                              <td>
                              <asp:ImageButton ID="ImgAddBought" runat="server" Height="33px" ImageUrl="~/Images/AddIMG.png"
                                       Width="33px" OnClick="ImgAddBought_Click" />
                              </td>
                              
                              </tr>
                                <%-- Row 3 Update from Estimate Grid --%>   
                              <tr>
                               <td>  <asp:Button Width="100%" runat="server" ID="btnUpdBout" OnClick="btnUpdBout_Click" Visible="false"  Text="Update Estimate BoughtOut"/></td>

                              </tr>
                             <%-- Row4 --%>
                              <tr>
                              <td colspan="7">
                                <div style="width: 100%; height: 100px; overflow: scroll">
                                <asp:GridView ID="gvJobCardPartsBought" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="gvJobCardPartsBought_SelectedIndexChanged"
                                                ShowHeaderWhenEmpty="True" Width="100%" OnRowDataBound="gvJobCardPartsBought_RowDataBound"
                                                BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                                CellPadding="3" ForeColor="Black" GridLines="Vertical">
                                                <AlternatingRowStyle BackColor="#CCCCCC" />
                                                <Columns>
                                                    <asp:CommandField ShowSelectButton="True" />
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
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lnkBoutRemove" runat="server" ImageUrl="~/Images/Delete_16x16.png" OnClick="lnkBoutRemove_Click"></asp:ImageButton>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="5%" HorizontalAlign=" center"></ItemStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="GridHeader" />
                                                <FooterStyle CssClass="GridFooter" />
                                            </asp:GridView></div>
                              </td>
                              </tr>
                              </table>
                             <table  width="100%" class="table table-condensed table-bordered table-hover">
                             <tr>
                             <td>
                             <asp:Label ID="Label2" runat="server" Text="Total Parts" Width="81px"></asp:Label>
                             </td>
                             <td>
                              <asp:Label ID="lblBParts" runat="server" Text="0" Width="60px"></asp:Label>
                             </td>
                             <td>
                              <asp:Label ID="Label4" runat="server" Text="Total Value" Width="70px"></asp:Label>
                             </td>
                             <td>
                             <asp:Label ID="lblBInvAmt" runat="server" Text="0" Width="60px"></asp:Label><asp:TextBox
                                                    ID="TextBox8" runat="server" Enabled="False" Width="60px" Visible="False">0</asp:TextBox>
                             </td>
                             </tr>
                             </table>
                               
                             
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                         <%-- Bay Management Parts Information Tab   --%>
                        <ajaxToolkit:TabPanel ID="TabPanel7" runat="server">
                            <HeaderTemplate>Bay Management</HeaderTemplate>
                            <ContentTemplate>
                                <div class="col-lg-12">
                                    <div class="row">
                                        <div class="col-lg-11 form-group">
                                            <asp:GridView ID="grdBay" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                                Width="100%" BackColor="White" OnRowDataBound="gv_JobTech_RowDataBound"
                                                OnRowEditing="gv_JobTech_RowEditing" OnRowCancelingEdit="gv_JobTech_RowCancelingEdit"
                                                OnRowUpdating="gv_JobTech_RowUpdating" OnDataBound="gv_JobTech_DataBound">
                                                <AlternatingRowStyle BackColor="#CCCCCC" />
                                                <Columns>
                                                    <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                        UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>
                                                </Columns>
                                                <Columns>
                                                    <%--<asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                    <asp:TemplateField HeaderText="JobCardCode">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblJobCardCode" runat="server" Text='<%# Bind("JobCardCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="EmpCode" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmpCode" runat="server" Text='<%# Bind("EmpCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Technician">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmpName" runat="server" Text='<%# Bind("EmpName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="StdTime">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStdTime" runat="server" Text='<%# Bind("StdTime") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="StartTime">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStartTime" runat="server" Text='<%# Bind("StartTime") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtStartTime" runat="server" Width="80px"></asp:TextBox>
                                                            <ajaxToolkit:MaskedEditExtender ID="txtStartTime_MaskedEditExtender" runat="server"
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                CultureTimePlaceholder="" Enabled="true" Mask="99:99" MaskType="Time" TargetControlID="txtStartTime"
                                                                ClearMaskOnLostFocus="False" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="EndTime">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEndTime" runat="server" Text='<%# Bind("EndTime") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEndTime" runat="server" Width="80px"></asp:TextBox>
                                                            <ajaxToolkit:MaskedEditExtender ID="txtEndTime_MaskedEditExtender" runat="server"
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                CultureTimePlaceholder="" Enabled="true" Mask="99:99" MaskType="Time" TargetControlID="txtEndTime"
                                                                ClearMaskOnLostFocus="False" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Time Taken">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTimeTaken" runat="server" Text='<%# Bind("TimeTaken") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="JobCode" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblJobCode" runat="server" Text='<%# Bind("JobCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Job Description(s)">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDefJobDesc" runat="server" Text='<%# Bind("DefJobDesc") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="BayCode" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBayCode" runat="server" Text='<%# Bind("BayCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Bay">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBayDesc" runat="server" Text='<%# Bind("BayDesc") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="GridHeader" />
                                                <FooterStyle CssClass="GridFooter" />
                                            </asp:GridView>
                                        </div>
                                        <div class="col-lg-1">
                                            <asp:ImageButton ID="imgAddJobTech" runat="server" Height="33px" ImageUrl="~/Images/AddIMG.png" Visible="True"
                                               Width="33px" OnClick="imgAddJobTech_Click" />
                                            <asp:ImageButton ID="imgRefresh" runat="server" Height="33px" CssClass="Refresh" OnClick="btnRefresh_Click" ImageUrl="~/Images/refresh.png" />
                                        </div>
                                    </div>
                                </div>

                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                   
                    </ajaxToolkit:TabContainer>
            
                </fieldset>



                <asp:Panel ID="panel1" class="modal-content modal-body" runat="server" Style="display: none; left: 71.5px;">
                    <div class="modal-header">
                        <h4 class="Heading">Job History</h4>
                    </div>
                    <asp:GridView ID="gv_history" runat="server" ShowFooter="True" ShowHeaderWhenEmpty="True"
                        AutoGenerateColumns="False" Width="100%">

                        <HeaderStyle CssClass="GridHeader" />
                        <FooterStyle CssClass="GridFooter" />

                        <Columns>
                            <asp:TemplateField HeaderText="SNo">
                                <ItemTemplate>
                                    <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex + 1%>"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Job Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblJobCode" runat="server" Text='<%# Bind("JobCardCode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Job Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblJobCode" runat="server" Text='<%# Bind("KM") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="JOB DESCRIPTION">
                                <ItemTemplate>
                                    <asp:Label ID="lblMade" runat="server" Text='<%# Bind("JobDescription") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Lub DESCRIPTION">
                                <ItemTemplate>
                                    <asp:Label ID="lblMade" runat="server" Text='<%# Bind("LubsDescription") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Part DESCRIPTION">
                                <ItemTemplate>
                                    <asp:Label ID="lblMade" runat="server" Text='<%# Bind("PartsDesc") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sublet DESCRIPTION">
                                <ItemTemplate>
                                    <asp:Label ID="lblMade" runat="server" Text='<%# Bind("SubletDesc") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>


                        </Columns>


                    </asp:GridView>


                    <div class="modal-footer">
                        <asp:Button ID="btnOk" runat="server" Text="OK" />
                    </div>

                </asp:Panel>
                <ajaxToolkit:ModalPopupExtender ID="modalPopupExtender1" runat="server" OkControlID="btnOK" PopupControlID="panel1" TargetControlID="btnshowModal"></ajaxToolkit:ModalPopupExtender>

             <table width="100%" style="margin-bottom: 10px;margin-top:10px;">
                    <tr>
                     <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*) Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
                        <td>
                            <asp:Label ID="lblMsg" runat="server" Width="100%"></asp:Label>
                           
                        </td>
                       
                     
                         <td style="text-align: right;">

                            <asp:Button ID="btnSaveJC" runat="server" Width="100px" Text="Save" OnClick="btnSaveJC_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait';"
                                UseSubmitBehavior="false" AccessKey="s" />
                          
                                   <asp:Button ID="btnDeleteJC" runat="server" Width="100px" Text="Delete" OnClientClick=" return confirm('Are you sure to delete?');"
                            OnClick="btnDeleteJC_Click" AccessKey="u" />
                            <asp:Button ID="btnClearJC" runat="server" Width="100px" Text="Clear" OnClick="btnClearJC_Click" AccessKey="w" />
                            <asp:Button ID="btnPostJC" runat="server" Width="100px" Text="Post" Enabled="true" OnClick="btnPostJC_Click" AccessKey="p" />
                            <asp:Button ID="btnPrintJC" runat="server" Width="100px" Text="Print" OnClick="btnPrintJC_Click" AccessKey="l"  />
                            <asp:Button ID="btnLoadData" runat="server" Width="100px" Text="Load Data" OnClick="btnLoadData_Click" Visible="False" />
                            <asp:Button ID="btnshowModal" Text="History" Width="100px" runat="server" AccessKey="h" />
                              <asp:Button ID="btnGatePass" runat="server"  Text="Gate Pass" OnClick="btnGatePass_Click" Visible="false" AccessKey="m"/>
                          

                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <br />
    <asp:HiddenField ID="HFRegNo" runat="server" />
    <asp:DropDownList ID="ddlStatusJC" runat="server" Visible="False">
        <asp:ListItem Selected="True" Text="Select" Value="Select"></asp:ListItem>
        <asp:ListItem Text="Medium" Value="Medium"></asp:ListItem>
        <asp:ListItem Text="Normal" Value="Normal"></asp:ListItem>
        <asp:ListItem Text="Heavy" Value="Heavy"></asp:ListItem>
    </asp:DropDownList>
    <asp:HiddenField ID="HFCustomer" runat="server" />
    <asp:HiddenField ID="HFChassisNo" runat="server" />
    <asp:HiddenField ID="HFEngineNo" runat="server" />




    <%--<script src="../../../js/jquery-3.2.1.min.js"></script>
    <script src="../../../js/bootstrap.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <%--<script src="../../../js/ScrollableGridPlugin_ASP.NetAJAX_2.0.js"></script>
    <link href="../../../css/datatables.min.css" rel="stylesheet" type="text/css" />
    <script src="../../../js/datatables.min.js" type="text/javascript"></script>--%>

    <script language="javascript" type="text/javascript">

function IsOneDecimalPoint(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode; // restrict user to type only one . point in number
                var parts = evt.srcElement.value.split('.');
                if(parts.length > 1 && charCode==46)
                    return false;
                return true;
            }
</script>
    <script language="javascript" type="text/javascript">

        //function isNumeric(keyCode) {

        //    return ((keyCode >= 48 && keyCode <= 57) || keyCode == 8 || keyCode == 189 || keyCode == 32 || (keyCode >= 96 && keyCode <= 105))
        //}

        function isAlpha(keyCode) {

            return ((keyCode >= 65 && keyCode <= 90) || keyCode == 8 || keyCode == 32 || keyCode == 190)

        }



        $(document).ready(function () {
            $(".gvParts").DataTable();
            <%--$('#<%=gvParts.ClientID %>').Scrollable({
            
        });--%>
        });


    </script>
    <asp:HiddenField ID="HFBlank" runat="server" />
    <asp:HiddenField ID="HFJobCard" runat="server" />
    <asp:DropDownList ID="ddlJobTypeCode" runat="server" Visible="False">
    </asp:DropDownList>
    <asp:TextBox ID="txtDeliveryDate" runat="server" Width="100px" MaxLength="10"
        ReadOnly="True" Enabled="False" Visible="False"></asp:TextBox>
    <%--<script type="text/javascript" src="../../../Scripts/jquery-1.4.1.js"></script>--%>
    <%--<script type="text/javascript" src="../../../Scripts/JSvalidJobCard.js"></script>--%>
    <asp:TextBox ID="txtGatePass" runat="server" Enabled="False" Width="60px" Visible="False"></asp:TextBox>
    <asp:TextBox ID="txtVersion" runat="server" Width="100px" MaxLength="3" Visible="False"
        ReadOnly="True"></asp:TextBox>
</asp:Content>
