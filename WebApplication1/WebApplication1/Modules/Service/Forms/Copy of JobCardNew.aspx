<%@ Page Title="" Language="C#" Theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true"
     Inherits="DXBMS.Modules.Service.JobCardNew" %> <%--CodeBehind="JobCardNew.aspx.cs"--%>

    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <%--<asp:CalendarExtender ID="txtRecieptDae_CalendarExtender" runat="server" Enabled="True"
                        Format="dd-MM-yyyy" TargetControlID="txtRecieptDae">
                    </asp:CalendarExtender>--%>
                    <%--<link rel="stylesheet" href="css/bootstrap.min.css" >

<link href="css/style.css" rel='stylesheet' type='text/css' />
<link href="css/font-awesome.css" rel="stylesheet">
<script src="js/jquery2.0.3.min.js"></script>--%>

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
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="Heading">
                JOB CARD
            </div>
            
  
            <div class="col-lg-12 well">
                <div class="row">
                    <form>
                        <div class="col-sm-12">                                                           
                            <div class="row">
                                <div class="col-sm-6 form-group">
                        Job Card No.                    
                        <asp:DropDownList ID="ddlJobCardCode" runat="server" AutoPostBack="True" Font-Names="monospace"
                            Width="350px" OnSelectedIndexChanged="ddlJobCardCode_SelectedIndexChanged">
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddlJobCardCode"
                            PromptText="" QueryPattern="Contains">
                        </ajaxToolkit:ListSearchExtender>
                    </div>
                                <div class="col-sm-6 form-group">
                        Estimate No.                    
                        <asp:DropDownList ID="ddlEstNo" runat="server" Font-Names="monospace" Width="386px"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlEstNo_SelectedIndexChanged">
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddlEstNo"
                            PromptText="" QueryPattern="Contains">
                        </ajaxToolkit:ListSearchExtender>
                    </div>
                            </div>
                <tr>
                    <td style="width: 68px">
                        <asp:LinkButton ID="LinkButton3" runat="server" Height="18px" SkinID="btnLables"><font 
                    color="#000000">Reg No.</font>  <font color="red"><b>*</b></font></asp:LinkButton>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlVehRegNo" runat="server" AutoPostBack="True" Font-Names="monospace"
                            OnSelectedIndexChanged="ddlVehRegNo_SelectedIndexChanged" Width="350px">
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlVehRegNo"
                            PromptText="" QueryPattern="Contains">
                        </ajaxToolkit:ListSearchExtender>
                    </td>
                    <td>
                        Job Card Type <font color="red"><b>*</b></font>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlJobCardTypeCode" runat="server" Width="100px">
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddlJobCardTypeCode"
                            PromptText="" QueryPattern="Contains">
                        </ajaxToolkit:ListSearchExtender>
                    </td>
                    <td>
                        Payment Mode <font color="red"><b>*</b></font>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPayMode" runat="server" Width="100px">
                            <asp:ListItem Selected="True" Text="Select" Value="Select"></asp:ListItem>
                            <asp:ListItem Text="Cash" Value="Cash"></asp:ListItem>
                            <asp:ListItem Text="Credit" Value="Credit"></asp:ListItem>
                            <asp:ListItem Text="Insurance" Value="Insurance"></asp:ListItem>
                            <asp:ListItem Text="Warranty" Value="Warranty"></asp:ListItem>
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddlPayMode"
                            PromptText="" QueryPattern="Contains">
                        </ajaxToolkit:ListSearchExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        Customer
                    </td>
                    <td>
                        <asp:TextBox ID="txtCustomer" runat="server" MaxLength="8" ReadOnly="True" placeholder="Cust ID"
                            TabIndex="6" Width="100px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCustomerDesc" runat="server" ReadOnly="True" Width="217px" placeholder="Customer Name"></asp:TextBox>
                    </td>
                     <td>
                        <asp:Label ID="Label1" runat="server" Text="Schedule"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlScheduleJC" runat="server" width="100px">
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="ddlScheduleJC"
                            PromptText="" QueryPattern="Contains">
                        </ajaxToolkit:ListSearchExtender>
                    </td>
                    <td >
                        Billing: <font color="red"><b>*</b></font>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBillingType" runat="server" Width="100px">
                            <asp:ListItem Selected="True" Text="Select" Value="Select" />
                            <asp:ListItem Text="Free" Value="Free" />
                            <asp:ListItem Text="Principal" Value="Principal" />
                            <asp:ListItem Text="Free Coupon Service" Value="Free Coupon Service" />
                            <asp:ListItem Text="Customer Paid" Value="Customer Paid" />
                            <asp:ListItem Text="Insurance" Value="Insurance" />
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddlBillingType"
                            PromptText="" QueryPattern="Contains">
                        </ajaxToolkit:ListSearchExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        End User
                    </td>
                    <td>
                        <asp:TextBox ID="txtEndUser" runat="server" MaxLength="8" ReadOnly="True" placeholder="User ID"
                            TabIndex="7" Width="100px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEndUserDesc" runat="server" ReadOnly="True" Width="217px" Placeholder="User Name"></asp:TextBox>
                    </td>
                    <td>
                        Start Date <font color="red"><b>*</b></font>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRecieptDate" runat="server" TabIndex="17" ReadOnly="True" Style="background-image: url(../../../Images/Calendar_16x16.png);
                            background-repeat: no-repeat; padding-left: 25px; width: 100px;"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="txtRecieptDate_MaskedEditExtender" runat="server"
                            ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                            CultureTimePlaceholder="" Enabled="True" Mask="99-99-9999" TargetControlID="txtRecieptDate"
                            UserDateFormat="DayMonthYear" />
                        <ajaxToolkit:CalendarExtender ID="txtRecieptDate_CalendarExtender" runat="server"
                            Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtRecieptDate" />
                        <%--<asp:CalendarExtender ID="txtRecieptDae_CalendarExtender" runat="server" Enabled="True"
                        Format="dd-MM-yyyy" TargetControlID="txtRecieptDae">
                    </asp:CalendarExtender>--%>
                    </td>
                    <td>
                        Start Time <font color="red"><b>*</b></font>
                    </td>
                    <td>
                        <asp:TextBox ID="txtReciptTime" runat="server" Width="100px" MaxLength="5" TabIndex="12"
                            placeholder="00:23" ReadOnly="True" Enabled="False"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="txtReciptTime_MaskedEditExtender" runat="server"
                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                            CultureTimePlaceholder="" Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtReciptTime"
                            ClearMaskOnLostFocus="False" />
                    </td>
                </tr>
                <tr>
                     <td>
                        Brand
                    </td>
                    <td>
                        <asp:TextBox ID="txtBrand" runat="server" Width="100px" MaxLength="15" TabIndex="8"
                            Placeholder="Brand ID" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtBrandDesc" runat="server" Width="217px" ReadOnly="True" Placeholder="Brand Name"></asp:TextBox>
                    </td>
                    <td>
                        Promised Date
                    </td>
                    <td>
                        <asp:TextBox ID="txtPromisedDate" runat="server" ReadOnly="True" Style="background-image: url(../../../Images/Calendar_16x16.png);
                            background-repeat: no-repeat; padding-left: 25px; width: 100px;"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="txtPromisedDate_MaskedEditExtender" runat="server"
                            ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                            CultureTimePlaceholder="" Enabled="True" Mask="99-99-9999" TargetControlID="txtPromisedDate"
                            UserDateFormat="DayMonthYear" />
                        <ajaxToolkit:CalendarExtender ID="txtPromisedDate_CalendarExtender" runat="server"
                            Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtPromisedDate" />
                    </td>
                    <td>
                        Promised Time
                    </td>
                    <td>
                        <asp:TextBox ID="txtPromistedTime" runat="server" Width="100px" MaxLength="5" TabIndex="13"
                            placeholder="00:23"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="txtPromistedTime_MaskedEditExtender" runat="server"
                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                            CultureTimePlaceholder="" Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtPromistedTime" />
                    </td>
                </tr>
                <tr>
                   <td>
                        Vehicle
                    </td>
                    <td>
                        <asp:TextBox ID="txtProduct" runat="server" Width="100px" MaxLength="10" TabIndex="9"
                            placeholder="Vehicle ID" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtVersionDesc" runat="server" ReadOnly="True" Width="217px" placeholder="Description"></asp:TextBox>
                    </td>
                    <td>
                        Advisor <font color="red"><b>*</b></font>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAdvisorCode" runat="server" Width="100px">
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddlAdvisorCode"
                            PromptText="" QueryPattern="Contains">
                        </ajaxToolkit:ListSearchExtender>
                    </td>
                    <td>
                        K.M <font color="red"><b>*</b></font>
                    </td>
                    <td>
                        <asp:TextBox ID="txtKM" runat="server" MaxLength="8" TabIndex="15" OnTextChanged="txtKM_TextChanged"
                            Style="background-image: url(../../../Images/KmIcon.png); background-repeat: no-repeat;
                            padding-left: 25px; width: 100px;"></asp:TextBox>
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"
                            Enabled="True" FilterType="Numbers" TargetControlID="txtKM" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Chassis No.
                    </td>
                    <td>
                        <asp:TextBox ID="txtChassisNo" runat="server" Enabled="false" Width="100px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtColor" runat="server" OnTextChanged="txtColor_TextChanged" Placeholder="COLOR"
                            ReadOnly="True" Width="217px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblChk" runat="server" Visible="False"></asp:Label>
                        Engine No.
                    </td>
                    <td>
                        <asp:Label ID="lblSIRMaster" runat="server" Visible="False"></asp:Label>
                        <asp:TextBox ID="txtEngineNo" runat="server" Width="100px" Enabled ="false"></asp:TextBox>
                    </td>                   
                    <td>
                        Coupon No.
                    </td>
                    <td>
                        <asp:TextBox ID="txtCouponNo" runat="server" MaxLength="10"  TabIndex="10"
                            Width="100px"></asp:TextBox>
                    </td>
                </tr>                      
                <tr>
                    <td>
                        Comments
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="100" TextMode="MultiLine"
                            Width="350px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSIRMasterId" runat="server" Enabled="False" Visible="False" Width="60px"></asp:TextBox>
                    </td>
                    <td>
                        
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtDeliveryTime" runat="server" MaxLength="5" placeholder="00:23"
                            TabIndex="14" Width="71px" Visible="False"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="txtDeliveryTime_MaskedEditExtender" runat="server"
                            ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                            CultureTimePlaceholder="" Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtDeliveryTime" />
                    </td>
                    
                </tr>              

                        </div>
                    </form>
                </div>
            </div>
            <fieldset>
                <legend>Service & Maintenance</legend>                
                <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="4"
                Width="100%" Style="direction: ltr; text-align: left;">
                <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Sele Person Information  ">                    
                    <HeaderTemplate>Labor Task</HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                                <tr>
                                    <td>
                                        Labor <font color="red"><b>*</b></font>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlJobs_Labor" runat="server" Width="400px" Font-Names="monospace" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlJobs_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="ddlJobs_Labor" PromptText="" QueryPattern="Contains">
                                        </ajaxToolkit:ListSearchExtender>
                                    </td>
                                    <td>Technician <font color="red"><b>*</b></font></td>
                                    <td>
                                        <asp:DropDownList ID="ddlTechnicianEmpCode" runat="server" Width="100px">
                                        </asp:DropDownList>
                                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender15" runat="server" TargetControlID="ddlTechnicianEmpCode" PromptText="" QueryPattern="Contains">
                                        </ajaxToolkit:ListSearchExtender>
                                    </td>
                                    </tr>
                                <tr>
                                        <td>Labor Remarks</td>
                                    <td>
                                        <asp:TextBox ID="txtJobRemarks" runat="server" Height="19px" MaxLength="200" TabIndex="24" Width="400px"></asp:TextBox>
                                    </td>
                                    <td>Labor<font color="red"><b>*</b></font></td>
                                    <td>
                                        <asp:TextBox ID="txtLabor" runat="server" MaxLength="6" OnTextChanged="txtLabor_TextChanged"
                                        Style="text-align: right" TabIndex="26" Width="100px">0</asp:TextBox>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="txtLaborExtender" runat="server" BehaviorID="_content_txtLaborExtender"
                                        FilterType="Numbers" TargetControlID="txtLabor" />
                                    </td>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr>
                                    <td style="width: 260px">
                                        <asp:ImageButton ID="BtnAddJobs" runat="server" Height="45px" ImageUrl="~/Images/AddIMG.png"
                                        Width="45px" OnClick="BtnAddJobs_Click" />
                                    </td>
                                    <td align="right">
                                        <asp:ImageButton ID="BtnClearJobs" runat="server" Height="40px" ImageUrl="~/Images/ClearImg.png"
                                        OnClick="BtnClearJobs_Click" Width="40px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="gvJobCard" runat="server" AutoGenerateColumns="False" BackColor="White"
                                        CellPadding="4" ForeColor="Black" OnSelectedIndexChanged="gvJobCard_SelectedIndexChanged"
                                        ShowHeaderWhenEmpty="True" Width="100%" OnRowDataBound="gvJobCard_RowDataBound">
                                        <Columns>
                                            <asp:CommandField ShowSelectButton="True"></asp:CommandField>
                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                            <asp:BoundField DataField="JobCode" HeaderText="Jobc">
                                                <ItemStyle Width="60px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DefJobDesc" HeaderText="Job Desc">
                                                <ItemStyle Wrap="True" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="EmpCode" HeaderText="TechCode">
                                                <ItemStyle Width="60px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="EmpName" HeaderText="Technician">
                                                <ItemStyle Width="200px" Wrap="True" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Type" HeaderText="Type">
                                                <ItemStyle Width="60px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="JobRemarks" HeaderText="Job Remarks">
                                                <ItemStyle Width="200px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Amount" HeaderText="Amount">
                                                <ItemStyle Width="60px" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkRemove" runat="server" OnClick="lnkRemove_Click1">Remove</asp:LinkButton></ItemTemplate>
                                                <ItemStyle Width="5%"></ItemStyle>
                                            </asp:TemplateField>
                                         </Columns>
                                        <AlternatingRowStyle BackColor="#CCCCCC" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <FooterStyle CssClass="GridFooter" />
                                        <SelectedRowStyle BackColor="WhiteSmoke" Font-Bold="False" ForeColor="Black" />
                                    </asp:GridView>                                
                               </td>                               
                           </tr>                      
                                <tr>
                               <td align="center" style="width: 260px">
                                   <asp:Label ID="Label45" runat="server" Text="Total Jobs" Width="100px"></asp:Label><asp:Label
                                        ID="txtJobsTotal" runat="server" Text="0" Width="50px"></asp:Label>
                               </td>
                               <td align="center">
                                   <asp:Label ID="Label113" runat="server" Text="Total Labor" Width="100px"></asp:Label><asp:Label
                                        ID="lblJobCardTotal" runat="server" Text="0" Width="50px"></asp:Label>
                               </td>
                           </tr>
                            </table>                       
                        </ContentTemplate>                    
               
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Corporate / Institution  ">                   
                   
                    <HeaderTemplate>Material</HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                                <tr>
                                    <td style="width: 10%">Part Code<font color="red"><b>*</b></font></td>
                                    <td colspan="4">
                                        <asp:DropDownList ID="ddlPartCode" runat="server" Font-Names="monospace" Width="400px" OnSelectedIndexChanged="ddlPartCode_SelectedIndexChanged"
                                        AutoPostBack="True"></asp:DropDownList>
                                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="ddlPartCode"
                                        PromptText="" QueryPattern="Contains" BehaviorID="_content_ListSearchExtender10"></ajaxToolkit:ListSearchExtender>
                                    
                               
                                </td>
                                
                               
                                <td>    
                                        
                                       
                                    <asp:TextBox ID="txtItemDesc" runat="server" Visible="False"></asp:TextBox>
                                    
                                   
                                    <asp:TextBox ID="txtPartItemNo_Parts" runat="server" Visible="False"></asp:TextBox>
                                    
                                   
                                    <asp:TextBox ID="txtPartsRecQuantity" runat="server" Enabled="False" Visible="False" Style="text-align: right" Width="60px">0</asp:TextBox>
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                            
                            
                            
                           
                            
                            <tr>
                                
                               
                                <td style="width: 80px">
                                    
                                   
                                    Quantity 
                                    
                                    
                                    <font color="red"><b>*</b></font>
                               
                                </td>
                                
                                
                                
                                
                               
                                
                                <td>
                                    
                                   
                                    <asp:TextBox ID="txtQuantity" runat="server" MaxLength="3" Style="text-align: right"
                                        ValidationGroup="JCParts" Width="100px"></asp:TextBox>
                                    
                                    
                                    
                                    
                                   
                                    
                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                        BehaviorID="_content_FilteredTextBoxExtender1" FilterType="Numbers" TargetControlID="txtQuantity" />
                                    
                               
                                </td>
                                
                                
                                
                                
                               
                                
                                <td style="width: 80px">
                                    
                                   
                                    Price 
                                    
                                    
                                    <font color="red"><b>*</b></font>
                               
                                </td>
                                
                                
                                
                                
                               
                                
                                <td>
                                    
                                   
                                    <asp:TextBox ID="txtPartPrice" runat="server" MaxLength="6" Enabled="False" Style="text-align: right"
                                        ValidationGroup="JCParts" Width="100px">0</asp:TextBox>
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                               
                                    
                                    
                                </td>
                                
                                
                                
                                
                               
                                
                                <td style="width: 80px">
                                    
                                   
                                    Dep %
                               
                                </td>
                                
                                
                                
                                
                               
                                
                                <td>
                                    
                                   
                                    <asp:TextBox ID="txtDep" runat="server" MaxLength="2" Style="text-align: right" Width="60px"></asp:TextBox>
                                    
                                    
                                    
                                    
                                   
                                    
                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                        BehaviorID="_content_FilteredTextBoxExtender3" FilterType="Numbers" TargetControlID="txtDep" />
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                       
                        </table>
                        
                        
                        
                        
                       
                        
                        <table width="100%">
                            
                           
                            <tr>
                                
                                
                                
                                
                               
                                
                                <td>
                                    
                                   
                                    <asp:ImageButton ID="btnAddParts" runat="server" Height="45px" ImageUrl="~/Images/AddIMG.png"
                                        Width="45px" OnClick="btnAddParts_Click" />
                                    
                               
                                </td>
                                
                                
                                
                                
                               
                                
                                <td align="right" nowrap="nowrap">
                                    
                                    
                                    
                                    
                                   
                                    
                                    <asp:ImageButton ID="btnClearParts" runat="server" Height="40px" ImageUrl="~/Images/ClearImg.png"
                                        OnClick="btnClearParts_Click" Width="40px" />
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                           
                            <tr>
                                
                                
                                
                                
                               
                                
                                <td colspan="2">
                                    
                                   
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
                                                    
                                                    
                                                    
                                                    
                                                   
                                                    
                                                    <asp:LinkButton ID="lnkRemove" runat="server" OnClick="lnkRemove_Click">Remove</asp:LinkButton></ItemTemplate>
                                                
                                                
                                                
                                                
                                               
                                                
                                                <ItemStyle Width="5%"></ItemStyle>
                                                
                                           
                                            </asp:TemplateField>
                                            
                                       
                                        </Columns>
                                        
                                        
                                        
                                        
                                       
                                        
                                        <HeaderStyle CssClass="GridHeader" />
                                        
                                        
                                        
                                        
                                       
                                        
                                        <FooterStyle CssClass="GridFooter" />
                                        
                                   
                                    </asp:GridView>
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                            
                            
                            
                           
                            
                            <tr>
                                
                               
                                <td>
                                    
                                   
                                    <asp:Label ID="Label56" runat="server" Text="Total Parts" Width="81px"></asp:Label><asp:Label
                                        ID="txtPartTotalQuantity" runat="server" Text="0" Width="60px"></asp:Label>
                                    
                               
                                </td>
                                
                               
                                <td>
                                    
                                    
                                    
                                    
                                   
                                    
                                    <asp:Label ID="Label118" runat="server" Text="Total Value" Width="70px"></asp:Label><asp:Label
                                        ID="lblPartsTotal" runat="server" Text="0" Width="60px"></asp:Label><asp:TextBox
                                            ID="txttotPartsRecQty" runat="server" Enabled="False" Width="60px" Visible="False">0</asp:TextBox>
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                       
                        </table>
                        
                   
                    </ContentTemplate>
                    
               
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Customer Tag  ">
                    
                   
                    <HeaderTemplate>
                        
                       
                        Lubricants</HeaderTemplate>
                    
                    
                    
                    
                   
                    
                    <ContentTemplate>
                        
                       
                        <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                            
                           
                            <tr>
                                
                               
                                <td style="width: 10%">
                                    
                                   
                                    Lubs Code
                               
                                </td>
                                
                               
                                <td colspan="3">
                                    
                                   
                                    <asp:DropDownList ID="ddlLubCode" runat="server" Font-Names="monospace" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlLubCode_SelectedIndexChanged">
                                        
                                   
                                    </asp:DropDownList>
                                    
                                   
                                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="ddlLubCode"
                            PromptText="" QueryPattern="Contains">
                                        
                       
                        </ajaxToolkit:ListSearchExtender>
                                    
                                   
                                    <asp:TextBox ID="txtLubPartDesc" runat="server" ReadOnly="True" Visible="False"></asp:TextBox>
                                    
                                    
                                    
                                    
                                   
                                    
                                    <asp:TextBox ID="txtPartItemNo_Lubs" runat="server" Visible="False"></asp:TextBox>
                                    
                                   
                                    <asp:TextBox ID="txtLubRecQuantity" runat="server" Visible="False" Enabled="False" Width="60px">0</asp:TextBox>
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                            
                            
                            
                           
                            
                            <tr>
                                
                               
                                <td style="width: 80px">
                                    
                                   
                                    Quantity 
                                    
                                    
                                    <font color="red"><b>*</b></font>
                               
                                </td>
                                
                                
                                
                                
                               
                                
                                <td width="26%">
                                    
                                   
                                    <asp:TextBox ID="txtLubQuantity" runat="server" MaxLength="3" Style="text-align: right"
                                        Width="100px"></asp:TextBox>
                                    
                                    
                                    
                                    
                                   
                                    
                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server"
                                        BehaviorID="_content_FilteredTextBoxExtender5" FilterType="Numbers" TargetControlID="txtLubQuantity" />
                                    
                               
                                </td>
                                
                                
                                
                                
                               
                                
                                <td style="width: 80px">
                                    
                                   
                                    Price 
                                    
                                    
                                    <font color="red"><b>*</b></font>
                               
                                </td>
                                
                                
                                
                                
                               
                                
                                <td>
                                    
                                   
                                    <asp:TextBox ID="txtLubPrice" runat="server" MaxLength="6" Enabled="False" Style="text-align: right"
                                        Width="100px"></asp:TextBox>
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                               
                                    
                                    
                                </td>
                                
                           
                            </tr>
                            
                       
                        </table>
                        
                        
                        
                        
                       
                        
                        <table width="100%">
                            
                           
                            <tr>
                                
                                
                                
                                
                               
                                
                                <td>
                                    
                                   
                                    <asp:ImageButton ID="btnAddLubs" runat="server" Height="45px" ImageUrl="~/Images/AddIMG.png"
                                        Width="45px" OnClick="btnAddLubs_Click" />
                                    
                               
                                </td>
                                
                                
                                
                                
                               
                                
                                <td align="right">
                                    
                                   
                                    <asp:ImageButton ID="btnClearLubs" runat="server" Height="40px" ImageUrl="~/Images/ClearImg.png"
                                        Width="40px" OnClick="btnClearLubs_Click" />
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                            
                            
                            
                           
                            
                            <tr>
                                
                               
                                <td colspan="2">
                                    
                                    
                                    
                                    
                                   
                                    
                                    <asp:GridView ID="gvLubParts" runat="server" AutoGenerateColumns="False" BorderStyle="None"
                                        CellPadding="4" Style="text-align: left" Width="100%" OnSelectedIndexChanged="gvLubParts_SelectedIndexChanged"
                                        ShowHeaderWhenEmpty="True" OnRowDataBound="gvLubParts_RowDataBound">
                                        
                                       
                                        <Columns>
                                            
                                            
                                            
                                            
                                           
                                            
                                            <asp:CommandField ShowSelectButton="True"></asp:CommandField>
                                            
                                            
                                            
                                            
                                           
                                            
                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                            
                                           
                                            <asp:BoundField DataField="ItemCode" HeaderText="ItemCode">
                                                
                                               
                                                <ItemStyle Width="60px" />
                                            
                                           
                                            </asp:BoundField>
                                            
                                            
                                            
                                            
                                           
                                            
                                            <asp:BoundField DataField="PartNo" HeaderText="Part No">
                                                
                                               
                                                <HeaderStyle HorizontalAlign="Left" />
                                                
                                               
                                                <ItemStyle Width="140px" />
                                            
                                           
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
                                            
                                            
                                            
                                            
                                           
                                            
                                            <asp:BoundField DataField="Price" HeaderText="Price">
                                                
                                               
                                                <HeaderStyle HorizontalAlign="Center" />
                                                
                                               
                                                <ItemStyle HorizontalAlign="Right" Width="60px" />
                                            
                                           
                                            </asp:BoundField>
                                            
                                            
                                            
                                            
                                           
                                            
                                            <asp:BoundField DataField="Total" HeaderText="Total">
                                                
                                               
                                                <HeaderStyle HorizontalAlign="Center" />
                                                
                                               
                                                <ItemStyle HorizontalAlign="Right" Width="60px" />
                                            
                                           
                                            </asp:BoundField>
                                            
                                            
                                            
                                            
                                           
                                            
                                            <asp:TemplateField>
                                                
                                               
                                                <ItemTemplate>
                                                    
                                                    
                                                    
                                                    
                                                   
                                                    
                                                    <asp:LinkButton ID="lnkRemove" runat="server" OnClick="lnkRemove_Click2">Remove</asp:LinkButton></ItemTemplate>
                                                
                                                
                                                
                                                
                                               
                                                
                                                <ItemStyle Width="5%"></ItemStyle>
                                                
                                           
                                            </asp:TemplateField>
                                            
                                       
                                        </Columns>
                                        
                                        
                                        
                                        
                                       
                                        
                                        <AlternatingRowStyle BackColor="#CCCCCC" />
                                        
                                        
                                        
                                        
                                       
                                        
                                        <HeaderStyle CssClass="GridHeader" />
                                        
                                        
                                        
                                        
                                       
                                        
                                        <FooterStyle CssClass="GridFooter" />
                                        
                                   
                                    </asp:GridView>
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                            
                            
                            
                           
                            
                            <tr>
                                
                               
                                <td>
                                    
                                   
                                    <asp:Label ID="Label119" runat="server" Text="Total Lubricants" Width="100px"></asp:Label><asp:Label
                                        ID="lbltotLubQty" runat="server" Text="0" Width="60px" Height="16px"></asp:Label>
                                    
                               
                                </td>
                                
                               
                                <td>
                                    
                                    
                                    
                                    
                                   
                                    
                                    <asp:Label ID="Label102" runat="server" Text="Total Value" Width="100px"></asp:Label><asp:Label
                                        ID="txtLubTotal" runat="server" Text="0" Width="50px"></asp:Label><asp:TextBox ID="txttotLubRecQty"
                                            runat="server" Enabled="False" Width="60px" Visible="False" Style="text-align: right">0</asp:TextBox>
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                       
                        </table>
                        
                   
                    </ContentTemplate>
                    
               
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="Sublet Job">
                    
                   
                    <HeaderTemplate>
                        
                       
                        Sublet Job</HeaderTemplate>
                    
                    
                    
                    
                   
                    
                    <ContentTemplate>
                        
                       
                        <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                            
                           
                            <tr>
                                
                               
                                <td style="width: 10%">
                                    
                                   
                                    Vendor <font color="red">
                                        
                                       
                                        <b>*</b></font>
                               
                                </td>
                                
                               
                                <td>
                                    
                                    
                                    
                                    
                                   
                                    
                                    <asp:DropDownList ID="ddlVendorCode" runat="server">
                                        
                                   
                                    </asp:DropDownList>
                                    
                                   
                                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="ddlVendorCode"
                            PromptText="" QueryPattern="Contains">
                                        
                       
                        </ajaxToolkit:ListSearchExtender>
                                    
                               
                                </td>
                                
                                
                                
                                
                               
                                
                                <td style="width: 14%">
                                    
                                   
                                    Date <font color="red"><b>*</b></font>
                               
                                </td>
                                
                                
                                
                                
                               
                                
                                <td>
                                    
                                   
                                    <asp:TextBox ID="txtSubletDate" runat="server" Width="100px"></asp:TextBox>                                    
                                    
                                                                        
                                    
                                    
                                    
                                                                       
                                    
                                    <ajaxToolkit:CalendarExtender ID="txtSubletDate_CalendarExtender" runat="server"
                                        BehaviorID="_content_txtSubletDate_CalendarExtender" Format="dd-MM-yyyy" TargetControlID="txtSubletDate" />
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                            
                            
                            
                           
                            
                            <tr>
                                
                               
                                <td>
                                    
                                   
                                    Job <font color="red"><b>*</b></font>
                               
                                </td>
                                
                                
                                
                                
                               
                                
                                <td>
                                    
                                   
                                    <asp:DropDownList ID="ddlJobs_Sublet" runat="server" Font-Names="monospace" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlJobs_Sublet_SelectedIndexChanged">
                                        
                                   
                                    </asp:DropDownList>
                                    
                                   
                                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender13" runat="server" TargetControlID="ddlJobs_Sublet"
                            PromptText="" QueryPattern="Contains">
                                        
                       
                        </ajaxToolkit:ListSearchExtender>
                                    
                                   
                                    <asp:TextBox ID="txtSubletJobDesc" runat="server" Visible="False"></asp:TextBox>
                                    
                               
                                </td>
                                
                               
                                <td>
                                    
                                   
                                    Invoice Amount<font color="red"><b>*</b></font>
                               
                                </td>
                                
                                
                                
                                
                               
                                
                                <td>
                                    
                                   
                                    <asp:TextBox ID="txtSubletIncAmnt" runat="server" MaxLength="6" Style="text-align: right"
                                        Width="100px"></asp:TextBox>
                                    
                                    
                                    
                                    
                                   
                                    
                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server"
                                        BehaviorID="_content_FilteredTextBoxExtender7" FilterType="Numbers" TargetControlID="txtSubletIncAmnt" />
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                           
                            <tr>
                                
                                
                                
                                
                               
                                
                                <td>
                                    
                                   
                                    Remarks
                               
                                </td>
                                
                               
                                <td>
                                    
                                    
                                    
                                    
                                   
                                    
                                    <asp:TextBox ID="txtSubletRemarks" runat="server" Width="80%"></asp:TextBox>
                                    
                               
                                </td>
                                
                               
                                <td>
                                    
                                   
                                    Payable Amount<font color="red"><b>*</b></font>
                               
                                </td>
                                
                                
                                
                                
                               
                                
                                <td>
                                    
                                   
                                    <asp:TextBox ID="txtSubletPayAmnt" runat="server" MaxLength="6" Style="text-align: right"
                                        Width="100px"></asp:TextBox>
                                    
                                    
                                    
                                    
                                   
                                    
                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server"
                                        BehaviorID="_content_FilteredTextBoxExtender8" FilterType="Numbers" TargetControlID="txtSubletPayAmnt" />
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                       
                        </table>
                        
                       
                        <table width="100%">
                            
                           
                            <tr>
                                
                               
                                <td>
                                    
                                    
                                    
                                    
                                   
                                    
                                    <asp:ImageButton ID="btnAddSublet" runat="server" Height="45px" ImageUrl="~/Images/AddIMG.png"
                                        Width="45px" OnClick="btnAddSublet_Click" />
                                    
                               
                                </td>
                                
                               
                                <td align="right">
                                    
                                    
                                    
                                    
                                   
                                    
                                    <asp:ImageButton ID="btnClearSublet" runat="server" Height="40px" ImageUrl="~/Images/ClearImg.png"
                                        Width="40px" OnClick="btnClearSublet_Click" />
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                            
                            
                            
                           
                            
                            <tr>
                                
                               
                                <td colspan="2">
                                    
                                    
                                    
                                    
                                   
                                    
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
                                            
                                           
                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                            
                                            
                                            
                                            
                                           
                                            
                                            <asp:BoundField DataField="SubletDate" HeaderText="Date">
                                                
                                               
                                                <ItemStyle Width="80px" />
                                            
                                           
                                            </asp:BoundField>
                                            
                                            
                                            
                                            
                                           
                                            
                                            <asp:BoundField DataField="SubletAmount" HeaderText="Invoiced Amt">
                                                
                                               
                                                <ItemStyle Width="60px" />
                                            
                                           
                                            </asp:BoundField>
                                            
                                            
                                            
                                            
                                           
                                            
                                            <asp:BoundField DataField="PayableAmount" HeaderText="Payable Amt">
                                                
                                               
                                                <ItemStyle Width="60px" />
                                            
                                           
                                            </asp:BoundField>
                                            
                                            
                                            
                                            
                                           
                                            
                                            <asp:TemplateField>
                                                
                                               
                                                <ItemTemplate>
                                                    
                                                    
                                                    
                                                    
                                                   
                                                    
                                                    <asp:LinkButton ID="lnkRemove" runat="server" OnClick="lnkRemove_Click3">Remove</asp:LinkButton></ItemTemplate>
                                                
                                                
                                                
                                                
                                               
                                                
                                                <ItemStyle Width="5%"></ItemStyle>
                                                
                                           
                                            </asp:TemplateField>
                                            
                                       
                                        </Columns>
                                        
                                        
                                        
                                        
                                       
                                        
                                        <AlternatingRowStyle BackColor="#CCCCCC" />
                                        
                                        
                                        
                                        
                                       
                                        
                                        <HeaderStyle CssClass="GridHeader" />
                                        
                                        
                                        
                                        
                                       
                                        
                                        <FooterStyle CssClass="GridFooter" />
                                        
                                   
                                    </asp:GridView>
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                            
                            
                            
                           
                            
                            <tr>
                                
                               
                                <td>
                                    
                                   
                                    <asp:Label ID="Label77" runat="server" Height="16px" Text="Total Invoice Amount"></asp:Label><asp:Label
                                        ID="txtSubletTotal" runat="server" Height="16px" Text="0"></asp:Label>
                                    
                               
                                </td>
                                
                               
                                <td>
                                    
                                    
                                    
                                    
                                   
                                    
                                    <asp:Label ID="Label120" runat="server" Height="16px" Text="Total Payable Amount"></asp:Label><asp:Label
                                        ID="txtSubletPayableTotal" runat="server" Height="16px" Text="0"></asp:Label>
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                       
                        </table>
                        
                   
                    </ContentTemplate>
                    
               
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="Consumeable Material">
                    
                    
                    
                    
                   
                    
                    <HeaderTemplate>
                        
                       
                        Consumeable Material
                   
                    </HeaderTemplate>
                    
                    
                    
                    
                   
                    
                    <ContentTemplate>
                        
                       
                        <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                            
                           
                            <tr>
                                
                               
                                <td style="width: 10%">
                                    
                                   
                                    Part Code<font color="red"><b>*</b></font>
                               
                                </td>
                                
                               
                                <td colspan="3">
                                    
                                   
                                    <asp:DropDownList ID="ddlConsumableParts" runat="server" Font-Names="monospace" OnSelectedIndexChanged="ddlConsumableParts_SelectedIndexChanged"
                                        AutoPostBack="True">
                                        
                                   
                                    </asp:DropDownList>
                                    
                                   
                                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender14" runat="server" TargetControlID="ddlConsumableParts"
                            PromptText="" QueryPattern="Contains" BehaviorID="_content_ListSearchExtender14">
                                        
                       
                        </ajaxToolkit:ListSearchExtender>
                                    
                                   
                                    <asp:TextBox ID="txtConPartDesc" runat="server" ReadOnly="True"></asp:TextBox>
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                            
                            
                            
                           
                            
                            <tr>
                                
                               
                                <td>
                                    
                                   
                                    Quantity<font color="red"><b>*</b></font>
                               
                                </td>
                                
                                
                                
                                
                               
                                
                                <td>
                                    
                                   
                                    <asp:TextBox ID="txtConQuantity" runat="server" MaxLength="3" Style="text-align: right"
                                        ValidationGroup="JCParts" Width="100px"></asp:TextBox>
                                    
                                    
                                    
                                    
                                   
                                    
                                    <ajaxToolkit:FilteredTextBoxExtender ID="txtConQuantity_FilteredTextBoxExtender"
                                        runat="server" BehaviorID="txtConQuantity_FilteredTextBoxExtender" FilterType="Numbers"
                                        TargetControlID="txtConQuantity" />
                                    
                               
                                </td>
                                
                                
                                
                                
                               
                                
                                <td style="width: 80px">
                                    
                                   
                                    Price<font color="red"><b>*</b></font>
                               
                                </td>
                                
                                
                                
                                
                               
                                
                                <td>
                                    
                                   
                                    <asp:TextBox ID="txtConPartPrice" runat="server" MaxLength="6" Style="text-align: right"
                                        ValidationGroup="JCParts" Width="100px">0</asp:TextBox>
                                    
                                    
                                    
                                    
                                   
                                    
                                    <asp:TextBox ID="txtConPartsRecQuantity" runat="server" Enabled="False" Style="text-align: right"
                                        Width="60px">0</asp:TextBox>
                                    
                                    
                                    
                                    
                                   
                                    
                                    <asp:TextBox ID="txtPartItemNoConParts" runat="server" Enabled="False" Width="60px"></asp:TextBox>
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                       
                        </table>
                        
                        
                        
                        
                       
                        
                        <table width="100%">
                            
                           
                            <tr>
                                
                                
                                
                                
                               
                                
                                <td>
                                    
                                   
                                    <asp:ImageButton ID="btnAddConsumablePart" runat="server" Height="45px" ImageUrl="~/Images/AddIMG.png"
                                        Width="45px" OnClick="btnAddConsumablePart_Click" />
                                    
                               
                                </td>
                                
                               
                                <td align="right" nowrap="nowrap">
                                    
                                   
                                    <asp:ImageButton ID="btnClearConsumablePart" runat="server" Height="40px" ImageUrl="~/Images/ClearImg.png"
                                        Width="40px" OnClick="btnClearConsumablePart_Click" />
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                           
                            <tr>
                                
                                
                                
                                
                               
                                
                                <td colspan="2">
                                    
                                   
                                    <asp:GridView ID="gvJobCardConParts" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                        Width="100%" OnRowDataBound="gvJobCardConParts_RowDataBound" OnSelectedIndexChanged="gvJobCardConParts_SelectedIndexChanged"
                                        BackColor="White" CellPadding="3">
                                        
                                        
                                        
                                        
                                       
                                        
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
                                                    
                                                    
                                                    
                                                    
                                                   
                                                    
                                                    <asp:LinkButton ID="lnkRemove" runat="server" OnClick="lnkRemove_Click4">Remove</asp:LinkButton></ItemTemplate>
                                                
                                                
                                                
                                                
                                               
                                                
                                                <ItemStyle Width="5%"></ItemStyle>
                                                
                                           
                                            </asp:TemplateField>
                                            
                                       
                                        </Columns>
                                        
                                        
                                        
                                        
                                       
                                        
                                        <HeaderStyle CssClass="GridHeader" />
                                        
                                        
                                        
                                        
                                       
                                        
                                        <FooterStyle CssClass="GridFooter" />
                                        
                                   
                                    </asp:GridView>
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                            
                            
                            
                           
                            
                            <tr>
                                
                               
                                <td>
                                    
                                   
                                    <asp:Label ID="Label8" runat="server" Text="Total Parts" Width="81px"></asp:Label>
                                    
                                    
                                    
                                    
                                   
                                    
                                    <asp:Label ID="txtConPartTotalQuantity" runat="server" Text="0" Width="60px"></asp:Label>
                                    
                               
                                </td>
                                
                               
                                <td>
                                    
                                    
                                    
                                    
                                   
                                    
                                    <asp:Label ID="Label10" runat="server" Text="Total Value" Width="70px"></asp:Label><asp:Label
                                        ID="txtConPartsTotal" runat="server" Text="0" Width="60px"></asp:Label>
                                        
                                       
                                        <asp:TextBox ID="txttotConPartsRecQty" runat="server" Enabled="False" Width="60px" Visible="False">0</asp:TextBox>
                                    
                               
                                </td>
                                
                           
                            </tr>
                            
                       
                        </table>
                        
                   
                    </ContentTemplate>
                    
               
                </ajaxToolkit:TabPanel>
            </ajaxToolkit:TabContainer>
            </fieldset>
                <table width="100%" >
                <tr>
                    <td >
                        <asp:Label ID="lblMsg" runat="server" Width="100%"></asp:Label>
                    </td>
                    
                    <td align="center">
                        <asp:Button ID="btnSaveJC" runat="server" Text="Save" OnClick="btnSaveJC_Click" />
                        <asp:Button ID="btnDeleteJC" runat="server" Text="Delete" OnClientClick="function(s, e) {
	return confirm('Are you sure you want to Delete?');
}" OnClick="btnDeleteJC_Click" />
                        <asp:Button ID="btnClearJC" runat="server" Text="Clear" OnClick="btnClearJC_Click" />
                        <asp:Button ID="btnPostJC" runat="server" Text="Post" Enabled="false" OnClick="btnPostJC_Click" />
                        <asp:Button ID="btnPrintJC" runat="server" Text="Print" OnClick="btnPrintJC_Click" />
                        <asp:Button ID="btnLoadData" runat="server" Text="Load Data" OnClick="btnLoadData_Click"
                            Visible="False" />
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
    <script language="javascript" type="text/javascript">

        function isNumeric(keyCode) {

            return ((keyCode >= 48 && keyCode <= 57) || keyCode == 8 || keyCode == 189 || keyCode == 32 || (keyCode >= 96 && keyCode <= 105))
        }

        function isAlpha(keyCode) {

            return ((keyCode >= 65 && keyCode <= 90) || keyCode == 8 || keyCode == 32 || keyCode == 190)

        }

    </script>
    <asp:HiddenField ID="HFBlank" runat="server" />
    <asp:HiddenField ID="HFJobCard" runat="server" />
    <asp:DropDownList ID="ddlJobTypeCode" runat="server" Visible="False">
    </asp:DropDownList>
    <asp:TextBox ID="txtDeliveryDate" runat="server" Width="100px" MaxLength="10" TabIndex="16"
        ReadOnly="True" Enabled="False" Visible="False"></asp:TextBox>
    <%--<script type="text/javascript" src="../../../Scripts/jquery-1.4.1.js"></script>--%>
    <%--<script type="text/javascript" src="../../../Scripts/JSvalidJobCard.js"></script>--%>
    <asp:TextBox ID="txtGatePass" runat="server" Enabled="False" Width="60px" Visible="False"></asp:TextBox>
    <asp:TextBox ID="txtVersion" runat="server" Width="100px" MaxLength="3" TabIndex="10"
        ReadOnly="True"></asp:TextBox>
</asp:Content>
