<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerEstimate.aspx.cs"
    Inherits="DXBMS.Modules.Service.CustomerEstimate" MasterPageFile="~/Root.master"
    Theme="Theme1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>--%>
    <%--   <script type="text/javascript">
        function jScript() {
            $(document).ready(function () { $("#<%=ddlCust.ClientID %>").select2(); });
            $(document).ready(function () { $("#<%=ddlchassis.ClientID %>").select2(); }); 
        }
</script>--%>
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
    <script src="../../../Jquery/jquery-ui-1.7.3.custom.min.js"></script>
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
               window.location.href = "CustomerEstimate.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "CustomerEstimate.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "CustomerEstimate.aspx";
           }
           function reloads() {
               window.location.href = "CustomerEstimate.aspx";
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
                border: 1px solid;
                border-color: #bebebe #bebebe #e1e1e1 #bebebe;
                background-color: #e1e1e1;
                color: #777777;
                cursor: pointer;
                text-decoration: none;
                padding: 10px;
                border-bottom: none;
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
            border: 1px solid #bebebe;
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

        .FixedHeader {
            position: absolute;
            font-weight: bold;
        }
    </style>
    <link rel="stylesheet" type="text/css" href="../../../css/Tab.css" />



    <script type="text/javascript" src="../../../Jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../../Jquery/MaxLength.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
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

        function MaxTxtLen() {
            $(function () {
                //Normal Configuration
                $("[id*=txtRemarks]").MaxLength({ MaxLength: 200 });
            });
        }
        function SearchddlRegNo() {
            $("ddlRegNo").searchit({
                textFieldClass: 'searchbox',
                selected: true
            });
        }
        //        function SearchddlRegNo() {
        //            $(document).ready(function () {

        //                $("ddlRegNo").searchable();

        //            });
        //        }

    </script>
    <asp:UpdatePanel ID="UpdPanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript">

                Sys.Application.add_load(MaxTxtLen);
                //Sys.Application.add_load(SearchddlRegNo);
            </script>
            <div class="container">
                <%--<script type="text/javascript" >
        Sys.Application.add_load(jScript);
            </script>--%>
                 <span class="fa-arrow-left fa"></span>
                        <asp:LinkButton ID="lnkBack" ForeColor="Black" runat="server" PostBackUrl="~/" Text="Back"></asp:LinkButton>
                         
                    <div class="Heading">
                        CUSTOMER JOB ESTIMATE FORM
                    </div>
                    <fieldset>
                        <legend>Job Estimate</legend>
                        <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                            <%-- Row 1 --%>
                            <tr>
                                <td >
                                   Estimate Code<asp:ImageButton runat="server" ID="imgEstNo" ImageUrl="~/Images/LookupNew.png" OnClick="imgEstNo_Click" />
                                </td>
                                <td >
                                 <asp:TextBox ID="txtExtno" runat="server" MaxLength="15" ReadOnly="True" SkinID="ROTextBox"
                                        TabIndex="1" Width="100%"></asp:TextBox>
                                        <asp:TextBox ID="txtEstCode" runat="server" Visible="False"></asp:TextBox>
                                    <%-- <asp:TextBox ID="txtJobCardNo" runat="server" CssClass="ROTextBoxes" MaxLength="8"
                                ReadOnly="true" Width="100px"></asp:TextBox>--%>
                                    <%--<asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" CultureAMPMPlaceholder=""
                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                            Enabled="True" ErrorTooltipEnabled="True" Mask="99/99999" MaskType="None" TargetControlID="txtJobCardNo"
                            ClearMaskOnLostFocus="False" />--%>
                                    <asp:DropDownList ID="ddlEstCode" runat="server" Font-Names="monospace" AutoPostBack="True" Visible="false"
                                         OnSelectedIndexChanged="ddlEstCode_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlEstCode"
                                        PromptText="" QueryPattern="Contains">
                                    </ajaxToolkit:ListSearchExtender>
                                </td>
                                <td>
                                  Date
                                </td>
                                <td >
                                    <asp:TextBox ID="txtCreateDate" ReadOnly="true" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png);
                                        background-repeat: no-repeat; padding-left: 25px;" TabIndex="2" Width="100%"></asp:TextBox>
                                </td>
                                <td>
                                  Reg No.<font color="#000000">
                       <asp:ImageButton runat="server" ID="imgVehRegNo" ImageUrl="~/Images/LookupNew.png" OnClick="imgVehRegNo_Click" /><font color="red"><b>*</b></font>
                                </td>
                                <td >
                                <asp:TextBox runat="server" ID="txtVehRegNo" ReadOnly="true" class="textalign" Width="100%" TabIndex="3"></asp:TextBox>
                                    <asp:DropDownList ID="ddlRegNo" runat="server" Visible="false" Font-Names="monospace" Width="330px"
                                        OnSelectedIndexChanged="ddlRegNo_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <ajaxToolkit:ListSearchExtender ID="LstEx" runat="server" TargetControlID="ddlRegNo"
                                        PromptText="" QueryPattern="Contains">
                                    </ajaxToolkit:ListSearchExtender>
                                </td>
                            </tr>
                           <%-- Row 2 --%>
                            <tr>
                               
                                <td>
                                    Brand
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBrand" runat="server" MaxLength="15" ReadOnly="True" SkinID="ROTextBox"
                                        TabIndex="4" Width="30%"></asp:TextBox>
                                        <asp:TextBox ID="txtBrandDesc" runat="server" ReadOnly="True" SkinID="ROTextBox"
                                        Width="68%"></asp:TextBox>
                                </td>
                                <td>
                                    Product
                                </td>
                                <td>
                                    <asp:TextBox ID="txtProduct" runat="server" MaxLength="10" ReadOnly="True" SkinID="ROTextBox"
                                        TabIndex="5" Width="100%"></asp:TextBox>
                                </td>
                                <td>
                                 Version

                                </td>
                                <td>
                                    <asp:TextBox ID="txtVersion" runat="server" MaxLength="3" ReadOnly="True" SkinID="ROTextBox"

                                        TabIndex="6" Width="30%"></asp:TextBox>
                                         <asp:TextBox ID="txtVersionDesc" runat="server" ReadOnly="True" SkinID="ROTextBox"
                                        Width="68%" TabIndex="10"></asp:TextBox>
                                </td>
                            </tr>
                           <%-- Row 3 --%>
                            <tr>
                                <td >
                                    Chassis No.
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChassisNo" runat="server" MaxLength="30" ReadOnly="True" SkinID="ROTextBox"
                                     TabIndex="7" Width="100%"></asp:TextBox>
                                </td>
                                <td>
                                Engine No.
                                </td>
                                <td>
                                 <asp:TextBox ID="txtEngineNo" runat="server" MaxLength="30" ReadOnly="True" SkinID="ROTextBox"
                                        TabIndex="8" Width="100%"></asp:TextBox>
                                </td>
                                <td>
                                   Customer
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCustomer" runat="server" Width="30%" MaxLength="8" TabIndex="9"
                                        ReadOnly="True" SkinID="ROTextBox"></asp:TextBox>
                                        <asp:TextBox ID="txtCustomerDesc" runat="server" Width="68%" SkinID="ROTextBox"
                                        ReadOnly="True" TabIndex="12"></asp:TextBox>
                                </td>
                               
                            </tr>
                          <%-- Row 4 --%>
                            <tr>
                                <td>
                                 End User
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEndUser" runat="server" Width="30%" MaxLength="8" TabIndex="10"
                                        ReadOnly="True" SkinID="ROTextBox"></asp:TextBox>
                                         <asp:TextBox ID="txtEndUserDesc" runat="server" Width="68%" SkinID="ROTextBox"
                                        ReadOnly="True"></asp:TextBox>
                                </td>
                                <td>
                                   Advisor<font color="red"><b>*</b></font>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAdvisor" runat="server" Width="100px" TabIndex="11">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Schedule
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSchedule" runat="server" Width="100px" TabIndex="12">
                                    </asp:DropDownList>
                                </td>
                               
                            </tr>
                          <%-- Row 5 --%>
                            <tr>
                                <td>
                                   Remarks
                                </td>
                                <td >
                                    <asp:TextBox ID="txtRemarks" runat="server" MaxLength="200" TabIndex="13" Width="100%"
                                        TextMode="MultiLine"></asp:TextBox>
                                </td>
                               
                                <td>
                                 Total Estimate Amount :</td>
                               <td>
                            <asp:Label ID="lblTotalAMount" runat="server"></asp:Label>
                        </td>
                            <td>
                                    Type
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlJobCardType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlJobCardType_SelectedIndexChanged"
                                        TabIndex="14" Style="width: 100px !important;">
                                        <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Normal" Value="Normal"></asp:ListItem>
                                        <asp:ListItem Text="Insurance" Value="Insurance"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                             <%-- Row 6 --%>
                             <tr>
                             <td>
                             Manual Estimate No
                             </td>
                             <td>
                             <asp:TextBox runat="server" ID="txtManEstNo" Width="100%" ></asp:TextBox>
                             </td>
                             </tr>
                          <%-- Row 6 --%>
                            <tr>
                              <%-- Insurrance Case --%>
                           
                                <td >
                                    <asp:Label ID="lblInsuranceComp" runat="server" Width="112px">Insurance Company<font 
                                color="red"></font></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlInsuranceComp" runat="server" TabIndex="15" Width="300px" AutoPostBack="true" OnSelectedIndexChanged="ddlInsuranceComp_SelectedIndexChanged" CausesValidation="false">
                                    </asp:DropDownList>
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblBranch" runat="server" Width="40px">Branch<font 
                                color="red"></font></asp:Label>
                                </td>
                                <td>
                                    <%--<asp:CalendarExtender ID="txtRecieptDae_CalendarExtender" runat="server" Enabled="True"
                        Format="dd-MM-yyyy" TargetControlID="txtRecieptDae">
                    </asp:CalendarExtender>--%>
                                    <asp:DropDownList ID="ddlInsBranch" runat="server" TabIndex="16" Width="300px">
                                    </asp:DropDownList>
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblConveyer" runat="server" Width="50px">Surveyor<font 
                                color="red"></font></asp:Label>
                                </td>
                                <td>
                                 
                                    <asp:TextBox ID="txtConvyer" runat="server" MaxLength="300" Width="100%" TabIndex="17"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <fieldset>
                    <legend>Jobs / Parts / Lubricants</legend>
                    <%--<div>--%>
                    <ajaxToolkit:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="1" Width="100%"
                        Style="direction: ltr; text-align: left;">
                        <%-- Labor Tab Panel --%>
                        <ajaxToolkit:TabPanel class="TabPanel" ID="TabPanel1" runat="server" HeaderText="Sele Person Information">
                            <HeaderTemplate>
                                Labor / Jobs
                            </HeaderTemplate>
                            <ContentTemplate>
                                <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                                    <tr>
                                    <td>Job Code<font color="red"><b>*</b></font> <asp:ImageButton runat="server" ID="ImgJobLookUp" ImageUrl="~/Images/LookupNew.png" OnClick="ImgJobLookUp_Click" />
                                        </td>
                                        <td> <asp:TextBox ID="txtJobCode" runat="server" ReadOnly="true" Width="20%" ></asp:TextBox>

                                          <asp:TextBox ID="txtJobDesc" runat="server" Width="70%" ReadOnly="true"></asp:TextBox>
                                       
                                            <asp:DropDownList ID="ddlJobs" runat="server" Visible="false" Style="width: 300px !important;" AutoPostBack="true" OnSelectedIndexChanged="ddlJobs_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        <asp:ImageButton ID="BtnAdd" runat="server" Height="33px" ImageUrl="~/Images/AddIMG.png"
                                                OnClick="BtnAdd_Click1" Width="33px"></asp:ImageButton>
                                        </td>
                                    </tr>
                                        <%--<td>
                                            Job Remarks
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtJobRemarks" runat="server" Height="19px" MaxLength="200" TabIndex="24"
                                                Width="365px"></asp:TextBox>
                                        </td>--%>
                                    <tr>
                                        <td>
                                            Labor
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLabor" runat="server" MaxLength="6" Style="text-align: right"
                                                TabIndex="26" Width="100px"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="txtLabor_FilteredTextBoxExtender" runat="server"
                                                BehaviorID="_content_txtLabor_FilteredTextBoxExtender" FilterType="Numbers" TargetControlID="txtLabor">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                        <asp:ImageButton ID="BtnClear1" runat="server" Height="33px" ImageUrl="~/Images/ClearImg.png"
                                                OnClick="BtnClear1_Click1" Width="33px"></asp:ImageButton>
                                        </td>
                                    </tr>
                                    <tr>
                                      <td>Job Type</td>
                                   <td><asp:DropDownList runat="server" ID="ddlJobType" Visible="false" >
                                   <asp:ListItem Value="J" Selected="True">Normal</asp:ListItem>
                                   <asp:ListItem Value="S">Suplementary</asp:ListItem>
                                       </asp:DropDownList></td>
                                    </tr>
                                      <tr>
                                        <td colspan="4">
                                         <div style="width: 100%; height: 100px; overflow: scroll">
                                            <asp:GridView ID="gvJobCard" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                ShowHeaderWhenEmpty="True" Width="100%" OnRowDataBound="gvJobCard_RowDataBound" OnSelectedIndexChanged="gvJobCard_SelectedIndexChanged">
                                                <Columns>
                                                <asp:CommandField ShowSelectButton="True" HeaderStyle-Width="5%"></asp:CommandField>
                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                    <asp:BoundField DataField="JobCode" HeaderText="Job">
                                                        <ItemStyle Width="60px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DefJobDesc" HeaderText="Job Desc">
                                                        <ItemStyle Width="150px" Wrap="True" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="JobRemarks" HeaderText="Job Remarks" Visible="false">
                                                        <ItemStyle Width="200px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Amount">
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblFooterAmount" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRowAmount" runat="server" Style="text-align: right" Text='<%# Bind("Amount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="60px" />
                                                    </asp:TemplateField>
                                                     <asp:BoundField DataField="Type" HeaderText="Type">
                                                        <ItemStyle Width="150px" Wrap="True" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Remove">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="BtnRemove" runat="server" ImageUrl="~/Images/Delete_16x16.png"
                                                                ToolTip="Remove" OnClick="BtnRemove_Click" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="60px" />
                                                        <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle CssClass="GridFooter" />
                                                <HeaderStyle CssClass="GridHeader" />
                                            </asp:GridView>
                                          </div>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    
                                  
                                </table>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                         <%-- Parts Tab Panel --%>
                        <ajaxToolkit:TabPanel class="TabPanel" ID="TabPanel2" runat="server" HeaderText="Corporate / Institution">
                            <HeaderTemplate>
                                Parts
                            </HeaderTemplate>
                            <ContentTemplate>
                                <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                                    <tr>
                                        <td >
                                            Item Code <font color="red"><b>*</b></font>
                                            <asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png" onclick="btnLookup_Click"/>
                                        </td>
                                        
                                        <td >
                                            <asp:TextBox ID="txtItemCode" runat="server" Width="100%"></asp:TextBox>
                                        </td>
                                        <td >Item No</td>
                                        <td >
                                            <asp:TextBox ID="txtItemNo" runat="server" Width="100%"></asp:TextBox>                                       
                                        </td>  
                                        <td >Item Description</td>
                                        <td>
                                            <asp:TextBox ID="txtItemDesc" runat="server" Width="100%"></asp:TextBox>
                                        </td>
                                        <%--<td colspan="5">
                                            <%--<asp:DropDownList ID="ddlParts" runat="server" AutoPostBack="True" Font-Names="monospace"
                                                OnSelectedIndexChanged="ddlParts_SelectedIndexChanged" Style="width: 650px !important;">
                                            </asp:DropDownList>
                                            <ajaxToolkit:ListSearchExtender ID="ListSearchExtender4" runat="server" BehaviorID="_content_ListSearchExtender4"
                                                QueryPattern="Contains" TargetControlID="ddlParts">
                                            </ajaxToolkit:ListSearchExtender>
                                        </td>--%>
                                        <td>
                                        <asp:ImageButton ID="BtnAdd0" runat="server" Height="33px" ImageUrl="~/Images/AddIMG.png"
                                                Width="33px" OnClick="BtnAdd0_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td >
                                            Quantity<font color="red"><b>*</b></font>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtQuantity" runat="server" MaxLength="3" Style="text-align: right"
                                                ValidationGroup="JCParts" Width="100%"></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender
                                                    ID="FilteredTextBoxExtender1" runat="server" BehaviorID="_content_FilteredTextBoxExtender1"
                                                    FilterType="Numbers" TargetControlID="txtQuantity"></ajaxToolkit:FilteredTextBoxExtender>
                                        </td>
                                        <td >
                                            Price
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPartPrice" runat="server" MaxLength="10"  SkinID="ROTextBox"
                                                Style="text-align: right" ValidationGroup="JCParts" Width="100%">0</asp:TextBox><ajaxToolkit:FilteredTextBoxExtender
                                                    ID="FilteredTextBoxExtender2" runat="server" BehaviorID="_content_FilteredTextBoxExtender2"
                                                    FilterType="Numbers" TargetControlID="txtPartPrice"></ajaxToolkit:FilteredTextBoxExtender>
                                        </td>
                                        <td >
                                            Dep %
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDep" runat="server" MaxLength="2" Style="text-align: right" Width="100%"></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender
                                                ID="FilteredTextBoxExtender3" runat="server" BehaviorID="_content_FilteredTextBoxExtender3"
                                                FilterType="Numbers" TargetControlID="txtDep"></ajaxToolkit:FilteredTextBoxExtender>
                                            <asp:TextBox ID="txtPartsRecQuantity" runat="server" Enabled="False" SkinID="ROTextBox" Style="text-align: right"
                                            Visible="False" Width="60px">0</asp:TextBox>
                                        </td>
                                        <td>
                                        <asp:ImageButton ID="BtnClear2" runat="server"
                                                    Height="33px" ImageUrl="~/Images/ClearImg.png" Width="33px" OnClick="BtnClear2_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                   
                                   <td>Part Type</td>
                                   <td><asp:DropDownList runat="server" ID="ddlPartType" Visible="false" >
                                   <asp:ListItem Value="A" Selected="True">Normal</asp:ListItem>
                                   <asp:ListItem Value="S">Suplementary</asp:ListItem>
                                       </asp:DropDownList></td>
                                        <td>
                                    <asp:Label runat="server" ID="lblIssuedParts"></asp:Label>
                                    </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7">
                                         <div style="width: 100%; height: 100px; overflow: scroll">
                                          <asp:GridView ID="gvJobCardParts" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" 
                                                Width="100%" OnRowDataBound="gvJobCardParts_RowDataBound" ShowFooter="True"
                                                OnSelectedIndexChanged="gvJobCardParts_SelectedIndexChanged">
                                                <HeaderStyle CssClass="GridHeader" />
                                                <FooterStyle CssClass="GridFooter" />
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
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Price" HeaderText="Price">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRowPartAmount" runat="server" Text='<%# Bind("Total") %>'></asp:Label></ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblFooterPartAmount" runat="server"></asp:Label></FooterTemplate>
                                                    </asp:TemplateField>
                                                      <asp:BoundField DataField="Type" HeaderText="Part Type" />
                                                    <asp:TemplateField HeaderText="Remove">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="BtnRemove" runat="server" ImageUrl="~/Images/Delete_16x16.png"
                                                                ToolTip="Remove" OnClick="BtnRemove_Click1" /></ItemTemplate>
                                                        <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    
                                </table>
                            
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                         <%-- Lubricant Tab Panel --%>
                        <ajaxToolkit:TabPanel class="TabPanel" ID="TabPanel3" runat="server" HeaderText="Customer Tag">
                            <HeaderTemplate>
                                Lubricants
                            </HeaderTemplate>
                            <ContentTemplate>
                                <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                                    <tr>
                                        <td>
                                            Part Code <font color="red"><b>*</b></font>
                                        </td>
                                        <td >
                                            <asp:DropDownList ID="ddllLubs" runat="server" Style="width: 100% !important;" Font-Names="monospace"
                                                OnSelectedIndexChanged="ddllLubs_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:DropDownList>
                                            <ajaxToolkit:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddllLubs"
                                                PromptText="" QueryPattern="Contains" BehaviorID="_content_ListSearchExtender3">
                                            </ajaxToolkit:ListSearchExtender>
                                            <asp:TextBox ID="txtLubPartCode" runat="server" Visible="False" Width="100px"></asp:TextBox>
                                            <asp:TextBox ID="txtLubPartDesc" runat="server" SkinID="ROTextBox" Visible="False"
                                                Width="250px"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                         <asp:ImageButton ID="ImageButton1" runat="server" Height="33px" ImageUrl="~/Images/AddIMG.png"
                                                OnClick="ImageButton1_Click" Width="33px"></asp:ImageButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Quantity <font color="red"><b>*</b></font>
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtLubQuantity" runat="server" MaxLength="3" Style="text-align: right"
                                                Width="100%"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server"
                                                FilterType="Numbers" TargetControlID="txtLubQuantity" BehaviorID="_content_FilteredTextBoxExtender5">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            Price <font color="red"><b>*</b></font>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLubPrice" runat="server" MaxLength="6" ReadOnly="True" SkinID="ROTextBox"
                                                Style="text-align: right" Width="100%"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server"
                                                BehaviorID="_content_FilteredTextBoxExtender6" FilterType="Numbers" TargetControlID="txtLubPrice">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                            <asp:TextBox ID="txtItemcodeLub" runat="server" Visible="False" Width="60px"></asp:TextBox>
                                            <asp:TextBox ID="txtLubRecQuantity" runat="server" Enabled="False" SkinID="ROTextBox"
                                                Visible="False" Width="60px"></asp:TextBox>
                                        </td>
                                        <td>
                                         <asp:ImageButton ID="ImageButton2" runat="server" Height="33px" ImageUrl="~/Images/ClearImg.png"
                                                Width="33px"></asp:ImageButton>
                                        </td>
                                        </tr>
                                         <tr>
                                         <td>Dep</td>
                                         <td>
                                         <asp:TextBox runat="server" ID="txtLubDep" MaxLength="2" Style="text-align: right" Width="100%" ></asp:TextBox>
                                         <ajaxToolkit:FilteredTextBoxExtender
                                                ID="FilteredTextBoxExtender4" runat="server" BehaviorID="_content_FilteredTextBoxExtender3"
                                                FilterType="Numbers" TargetControlID="txtLubDep"></ajaxToolkit:FilteredTextBoxExtender>
                                         </td>
                                
                                      <td>Lube Type</td>
                                   <td><asp:DropDownList runat="server" ID="ddlLubType" Visible="false" >
                                   <asp:ListItem Value="A" Selected="True">Normal</asp:ListItem>
                                   <asp:ListItem Value="S">Suplementary</asp:ListItem>
                                       </asp:DropDownList></td>
                                           <td>
                                    <asp:Label runat="server" ID="lblLubIssuedQty"></asp:Label>
                                    </td>
                                   
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                         <div style="width: 100%; height: 100px; overflow: scroll">
                                          <asp:GridView ID="gvLubParts" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                Width="100%" ShowHeaderWhenEmpty="True" OnRowDataBound="gvLubParts_RowDataBound" OnSelectedIndexChanged="gvLubParts_SelectedIndexChanged">
                                                <Columns>
                                                <asp:CommandField ShowSelectButton="True" />
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
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                    </asp:BoundField>
                                                       <asp:BoundField DataField="Dep" HeaderText="Depriciation" />
                                                    <asp:BoundField DataField="DepAmount" HeaderText="Dep Amount" />
                                                    <asp:BoundField DataField="Price" HeaderText="Price">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Amount">
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblFooterLubsAmount" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRowLubsAmount" runat="server" Text='<%# Bind("Total") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="60px" />
                                                    </asp:TemplateField>
                                                     <asp:BoundField DataField="Type" HeaderText="Lube Type" />

                                                    <asp:TemplateField HeaderText="Remove">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="BtnRemove" runat="server" ImageUrl="~/Images/Delete_16x16.png"
                                                                ToolTip="Remove" OnClick="BtnRemove_Click2" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle CssClass="GridFooter" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <PagerStyle CssClass="GridFooter" />
                                            </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    
                                </table>
                                
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <%-- Sublet Tab Panel --%>
                        <ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="Sublet Job">
                            <HeaderTemplate>
                                Sublet Job
                            </HeaderTemplate>
                            <ContentTemplate>
                              <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">

                              <tr>
                             
                                     <td>Job Code<font color="red"><b>*</b></font> <asp:ImageButton runat="server" ID="imgJobSubLookUp" ImageUrl="~/Images/LookupNew.png" OnClick="imgJobSubLookUp_Click" />
                                        </td>
                                        <td> <asp:TextBox ID="txtSubletJobCode" runat="server" ReadOnly="true" Width="100%"></asp:TextBox>
                                          </td>
                                          <td>
                                          Description
                                          </td>
                                          <td>
                                          <asp:TextBox ID="txtSubletJobDesc" runat="server" Width="100%"></asp:TextBox></td>
                                           <td>Sublet Date</td>
                                          <td>
                                           <asp:TextBox ID="txtSubletDate" runat="server" Width="100%"></asp:TextBox><ajaxToolkit:CalendarExtender
                                                    ID="txtSubletDate_CalendarExtender" runat="server" BehaviorID="_content_txtSubletDate_CalendarExtender"
                                                    Format="dd-MM-yyyy" TargetControlID="txtSubletDate" />
                                          </td>
                                          <td>
                                            <asp:ImageButton ID="btnAddSublet" runat="server" Height="33px" ImageUrl="~/Images/AddIMG.png"
                                       Width="33px" OnClick="btnAddSublet_Click" />
                                          </td>
                                               
                              </tr>
                                <tr>
                                 <td>Vendor<font color="red"><b>*</b></font> </td>
                                   <td>
                                       <asp:DropDownList runat="server" Id="ddlVendor" AutoPostBack="true"></asp:DropDownList>
                                                <ajaxToolkit:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="ddlVendor"
                                                    PromptText="" QueryPattern="Contains">
                                                </ajaxToolkit:ListSearchExtender>

                                        </td>
                                  <td>Invoice Amount</td>
                                    <td><asp:TextBox ID="txtSubletIncAmnt" runat="server" MaxLength="6" Style="text-align: right"
                                                    Width="100%"></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7"
                                                        runat="server" BehaviorID="_content_FilteredTextBoxExtender7" FilterType="Numbers"
                                                        TargetControlID="txtSubletIncAmnt" /></td>
                                    <td> Payable Amount</td>
                                     <td>
                                     <asp:TextBox ID="txtSubletPayAmnt" runat="server" MaxLength="6" Style="text-align: right"
                                                    Width="100%"></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8"
                                                        runat="server" BehaviorID="_content_FilteredTextBoxExtender8" FilterType="Numbers"
                                                        TargetControlID="txtSubletPayAmnt" />
                                                        <asp:TextBox ID="txtSubletRemarks" runat="server" Width="100%" Visible="false"></asp:TextBox>
                                     </td>
                                     <td>
                                     <asp:ImageButton ID="btnClearSublet" runat="server" Height="33px" ImageUrl="~/Images/Clear5.png" 
                                        Width="33px" OnClick="btnClearSublet_Click" />
                                     </td>

                                </tr>
                                   <tr>
                              <td colspan="7">
                               <div style="width: 100%; height: 100px; overflow: scroll">
                                 <asp:GridView ID="gvSublet"  BackColor="White"
                                                BorderStyle="None" OnSelectedIndexChanged="gvSublet_SelectedIndexChanged"
                                                runat="server"  AutoGenerateColumns="False" ShowFooter="True"
                                                Width="100%" ShowHeaderWhenEmpty="True"  OnRowDataBound="gvSublet_RowDataBound">
                                                <Columns>
                                                    <asp:CommandField ShowSelectButton="True"></asp:CommandField>
                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" >
                                                    <ItemStyle Width="60px" />
                                                    </asp:BoundField>
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
                                                            <asp:ImageButton ID="lnkRemove" ImageUrl="~/Images/Delete_16x16.png" runat="server" OnClick="lnkRemove_Click"></asp:ImageButton>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                                        <%--<asp:LinkButton ID="lnkRemove" runat="server" OnClick="lnkRemove_Click3">Remove</asp:LinkButton>--%>                                                    
                                                    </asp:TemplateField>
                                                </Columns>

                                                <HeaderStyle CssClass="GridHeader" />
                                                <FooterStyle CssClass="GridFooter" />
                                            </asp:GridView>
                                            </div>
                              </td>
                            
                              </tr>

                              </table>
                              <table width="100%">
                               
                           
                              <tr>
                              <td colspan="2">
                              <asp:Label ID="Label4" runat="server" Height="16px" Text="Total Payable Amount"></asp:Label>
                              </td>
                              
                              <td colspan="2">
                                <asp:Label ID="TotalPaySublet" runat="server" Height="16px" Text="0" style="margin-right:420px;" ></asp:Label>
                              </td>
                              
                              <td colspan="2">
                               <asp:Label ID="lblSubletInvAmount" runat="server" Height="16px" Text="Total Invoice Amount"></asp:Label>
                              </td>
                              
                              <td colspan="2">
                               <asp:Label ID="TotalInvSublet" runat="server" Height="16px" Text="0"></asp:Label>
                              </td>

                              </tr>
                              </table>
                    
                         
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <%-- Bought Out Tab Panel --%>
                        <ajaxToolkit:TabPanel ID="TabPanel8" runat="server" HeaderText="Corporate / Institution  ">
                            <HeaderTemplate>Bought Out Part</HeaderTemplate>
                            <ContentTemplate>
                           <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                           <tr>
                            <td> Item Code<font color="red"><b>*</b></font><asp:ImageButton runat="server" ID="ImgBoughtOutPart" ImageUrl="~/Images/LookupNew.png" OnClick="ImgBoughtOutPart_Click" /> </td>
                            <td>
                            <asp:TextBox ID="txtBItemCode" runat="server" Enabled="false" Width="100%"></asp:TextBox>
                            </td>
                            <td>
                            Part No
                            </td>
                            <td>
                                <asp:TextBox ID="txtBPartNo" runat="server" Enabled="false" Width="100%"></asp:TextBox>
                            </td>
                            <td>
                            Description
                            </td>
                            <td>
                               <asp:TextBox ID="txtBItemDesc" runat="server" Enabled="false" Width="100%"></asp:TextBox>
                            </td>
                            <td>
                            <asp:ImageButton ID="ImgAddBought" runat="server" Height="33px" ImageUrl="~/Images/AddIMG.png"
                                       Width="33px" OnClick="ImgAddBought_Click" />
                            </td>
                           </tr>
                           <tr>
                           <td>
                            Quantity <font color="red"><b>*</b></font>
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
                             Inv Rate
                           </td>
                           <td>
                           <asp:TextBox ID="txtBAmt" runat="server" Style="text-align: right" Enabled="false" Width="100%" ></asp:TextBox>
                           </td>
                           <td>
                             <asp:ImageButton ID="btnClearBParts" runat="server" Height="33px" ImageUrl="~/Images/Clear5.png" 
                                        OnClick="btnClearBParts_Click" Width="33px" />
                           </td>
                           </tr>
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
                                                            <asp:ImageButton ID="lnkBoutRemove" runat="server"  ImageUrl="~/Images/Delete_16x16.png" OnClick="lnkBoutRemove_Click"></asp:ImageButton>
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
                     <table width="100%">
                    
                  
                     <tr>

                     <td>
                      <asp:Label ID="Label7" runat="server" Text="Total Parts Amt" Width="81px"></asp:Label>
                     </td>
                     <td>
                        <asp:Label ID="lblBoutPartCount" runat="server" Text="0" Width="60px"></asp:Label>
                     </td>
                     <td>
                         <asp:Label ID="Label9" runat="server" Text="Total Invoice Amt" Width="70px"></asp:Label>
                     </td>
                     <td>
                     <asp:Label ID="lblTotalInvAmt" runat="server" Text="0" Width="60px"></asp:Label><asp:TextBox
                                                    ID="TextBox8" runat="server" Enabled="False" Width="60px" Visible="False">0</asp:TextBox>

                     </td>
                     </tr>
                      
                     </table>
                        
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                          <ajaxToolkit:TabPanel ID="TabImages" runat="server" HeaderText="Images">
                         <HeaderTemplate>Images</HeaderTemplate>
                         <ContentTemplate>
                         <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                         <tr>
                         <td colspan="1">Sketch</td>
							<td colspan="2">
                            <asp:Label runat="server" ID="lblImgAddress"> </asp:Label>
							</td>
                            <td colspan="1">
								<asp:FileUpload ID="PicUpload" runat="server" Width="100%" />
							</td>
                           
                         </tr>
                         <tr>
                         <td colspan="1">
                         Document Type
                         </td>
                         <td colspan="2">
                         <asp:DropDownList runat="server" ID="ddlDocType" Width="100%"></asp:DropDownList>
                         </td>
                         
                        
                          <td> <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="OnClick_btnUpload" CausesValidation="false" PostBackUrl="~/Modules/Service/Forms/CustomerEstimate.aspx"/></td>
                        
                         </tr>
						<tr>
                        
                        <td colspan="8">
                            <div style="width: 100%; height: 100px; overflow: scroll">
                        <asp:GridView ID="ImgGridView" runat="server" Width="100%"
                        ShowHeaderWhenEmpty="True"  AutoGenerateColumns="false"
                                                BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                                CellPadding="3" ForeColor="Black" GridLines="Vertical">
                                                <AlternatingRowStyle BackColor="#CCCCCC" />
                        
                        
                        
                        <Columns>
                        <asp:TemplateField>
                         <ItemTemplate>
                           <asp:Image ID="Image1" runat="server" Width="100px" Height="80px" HeaderText="Image" ImageUrl='<%# "~/" + DataBinder.Eval(Container, "DataItem.ImageAddress").ToString()%>' />
                           </ItemTemplate>
                           <ItemStyle Width="25%" HorizontalAlign="Left"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField  HeaderText="Document" DataField="DocChkListCode" Visible="false"/>
                        <asp:BoundField  HeaderText="Document" DataField="DocChkListDesc" />
                        <asp:TemplateField>
                          <ItemTemplate>
                              <asp:ImageButton ID="lnkImgRemove" runat="server"  ImageUrl="~/Images/Delete_16x16.png" OnClick="lnkImgRemove_Click"></asp:ImageButton>
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
                  <%-- <tr >
                   <td><asp:Image runat="server" ID="Image1"/></td>
                   </tr>--%>
                            
                      
                       
                             
					</table>
                             </ContentTemplate>
                     </ajaxToolkit:TabPanel>
                    </ajaxToolkit:TabContainer>
                    <%--</div>--%>
                </fieldset>
            <table width="100%" style="margin-bottom: 10px;margin-top:10px;">
                    
                    <tr>
                     <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*)  Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
                        <td>
                            <asp:Label ID="lblMsg" runat="server"></asp:Label>
                        </td>
                       <td style="text-align: right;">
                            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" OnClientClick="this.disabled = true; this.value = 'Please Wait';"
                                UseSubmitBehavior="false" />
                            <%--OnClientClick="Disable(this);"/>--%>
                            <%--<asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID ="btnSave" >
                            </asp:ConfirmButtonExtender>--%>
                            <asp:Button ID="btnClear" runat="server" Width="100px" Text="Clear" OnClick="btnClear_Click" />
                            <asp:Button ID="btnPrint" runat="server" Width="100px" Text="Print" OnClick="btnPrint_Click" />
                              <asp:Button ID="btnDelete" runat="server" Width="100px" Text="Delete" OnClientClick=" return confirm('Are you sure to delete?');"
                            OnClick="btnDelete_Click1" />
                           
                               
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
    <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
    <%--</ContentTemplate> </asp:UpdatePanel>--%>
    <br />
    <asp:HiddenField ID="HFRegNo" runat="server" />
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
</asp:Content>
