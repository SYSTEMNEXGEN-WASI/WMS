<%@ Page Title="" Language="C#" theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="DealerTechnical.aspx.cs" Inherits="DXBMS.Modules.Service.Forms.DealerTechnical" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

 


<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

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
               window.location.href = "DealerTechnical.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "DealerTechnical.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "DealerTechnical.aspx";
           }
           function reloads() {
               window.location.href = "DealerTechnical.aspx";
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
	<asp:UpdatePanel ID="UpdatePanel" runat="server">
	<%--<Triggers>               
				<asp:PostBackTrigger ControlID="TabContainerDTR" />    --%>        
			<%--</Triggers>--%>
	<ContentTemplate>
			<div class="container">
            <div style="font-size: 15px;">
                    <span class="fa-arrow-left fa"></span>
                    <asp:LinkButton ID="lnkBack" ForeColor="Black" runat="server" PostBackUrl="~/Modules/Service/Forms/DTRAppr-Reject.aspx" Text="Back"></asp:LinkButton>

                </div>
				<div class="Heading">Dealer Technical Form</div>

				<fieldset>
					<table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
						<tr>
							<td width="11%">Dealer Report</td>
							<td >
								<asp:DropDownList ID="ddlDRNo" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlDRNo_OnSelectedIndexChanged" ></asp:DropDownList> <%----%>
							</td>
							<td width="10%">DTR No.</td>
							<td>
								<asp:TextBox ID="txtSRNo" runat="server"></asp:TextBox>
							</td>
                            <td width="10%">Serial No.</td>
							<td>
								<asp:TextBox ID="txtSerial" runat="server" MaxLength="8"></asp:TextBox>
							</td>
							
						</tr>
                        <tr>
                        <td>Date</td>
							<td>
								<asp:TextBox ID="txtDate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png);
									 background-repeat: no-repeat; padding-left: 25px;" Width="100%" ReadOnly="true"></asp:TextBox>
								<ajax:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MM-yyyy" TargetControlID="txtDate"></ajax:CalendarExtender>
                                
							</td>
                        </tr>
					</table>
				</fieldset>
                <ajaxToolkit:TabContainer ID="TabContainerDTR" runat="server" ActiveTabIndex="0"
                    Width="100%" Height="100%">
					 <ajaxToolkit:TabPanel ID="TabVehicle" runat="server" HeaderText="Vehicle">
						
						<HeaderTemplate>Vehicle </HeaderTemplate>
						
						<ContentTemplate>
                            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">                        
						<tr>
                             <td> JobCard No.<asp:ImageButton runat="server" ID="ImageButtonRoNO" ImageUrl="~/Images/LookupNew.png" OnClick="ImageButtonRoNO_Click" Width="16px" /></td>
							<td>
								<asp:TextBox ID="txtRoNo" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
							</td> 
							<td width="11%">VIN<font color="red"><b>*</b></font></td>
							<td >
                                <asp:DropDownList ID="ddlVinNo" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlVinNo_SelectedIndexChanged">
                                </asp:DropDownList>
							</td>
							<td width="10%">Engine No.<font color="red"><b>*</b></font></td>
							<td>
								<asp:TextBox ID="txtEngineNo" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
								
							</td>
							<td>KM<font color="red"><b>*</b></font></td>
							<td>
								<asp:TextBox ID="txtKM" runat="server"></asp:TextBox>
								
							</td>
						</tr>
						<tr>
							<td>Model</td>
							<td>
								<asp:TextBox ID="txtModelCode" runat="server"  ReadOnly="true"></asp:TextBox>
								
							</td>
                            
							<td>Reg No.<font color="red"><b>*</b></font></td>
							<td>
								<asp:TextBox ID="txtRegNo" runat="server" ReadOnly="true"></asp:TextBox>
								
							</td>
                            <td>Vehicle Category .<font color="red"><b>*</b></font></td>
							<td>
								<asp:TextBox ID="txtVehicleCategory" runat="server" ReadOnly="true"></asp:TextBox>
								
							</td>
							<td>Color</td>
							<td>
								<asp:TextBox ID="txtColor" runat="server"  ReadOnly="true"></asp:TextBox>    
							</td>
                           
						</tr>
                        <tr>
                        <td>Type</td>
							<td>
								<asp:RadioButton ID="RadioBtnCBU" runat="server" Text="CBU" GroupName="Dispatched"  />
								<asp:RadioButton ID="RadioBtnCKD" runat="server" Text="CKD" GroupName="Dispatched"  />
							</td>
                            
							<td>Body Type</td>
							<td>
								<asp:TextBox ID="txtBodyType" runat="server" ReadOnly="true"></asp:TextBox>
								
							</td>
							<td>PayLoad</td>
							<td>
								<asp:TextBox ID="txtPayLoad" runat="server"  ReadOnly="true"></asp:TextBox>    
							</td>
                           <td>Date Of Sale</td>
							<td>
								<asp:TextBox ID="txtInvDate" runat="server"  ReadOnly="true"></asp:TextBox>    
							</td>

                        </tr>
                                <tr>
                                     <td>Key No</td>
							<td>
								<asp:TextBox ID="txtKeyNo" runat="server"></asp:TextBox>
								
							</td>
                                
                                    <td>
                                        Break Down Location
                                    </td>
                                    <td>
                                         <asp:DropDownList ID="ddlcity" Width="100%" runat="server"></asp:DropDownList>
                                        <%--<asp:ListSearchExtender ID="ListSearchExtender3" runat="server" PromptPosition="Top" TargetControlID="ddlcity" QueryPattern="Contains" Enabled="True"></asp:ListSearchExtender>--%>
                                    </td>
                                     
                                    <td>
                                      Oprational Route From
                                    </td>
                                    <td>
                                         <asp:DropDownList ID="ddlCityFrom" Width="100%" runat="server"></asp:DropDownList>
                                        <%--<asp:ListSearchExtender ID="ListSearchExtender2" runat="server" PromptPosition="Top" TargetControlID="ddlCityFrom" QueryPattern="Contains" Enabled="True"></asp:ListSearchExtender>--%>
                                    </td>
                                    <td>Oprational Route To</td>
                                    <td>
                                         <asp:DropDownList ID="ddlCityTo" Width="100%" runat="server"></asp:DropDownList>
                                        <%--<asp:ListSearchExtender ID="ListSearchExtender1" runat="server" PromptPosition="Top" TargetControlID="ddlCityTo" QueryPattern="Contains" Enabled="True"></asp:ListSearchExtender>--%>
                                    </td>
                                    
                                </tr>
                                
					</table>
                            </ContentTemplate>
					</ajaxToolkit:TabPanel>
                     <ajaxToolkit:TabPanel ID="TabCustomer" runat="server" HeaderText="Customer">
                        <HeaderTemplate>Customer </HeaderTemplate>
                        <ContentTemplate>
                        <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">                    
						<tr>
							<td width="11%">Customer Name</td>
							<td>
								<asp:TextBox ID="txtCustName" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
							</td>
							<td>Address</td>
							<td colspan="3" >
								<asp:TextBox ID="txtAddress" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
							</td> 
                              <td>Contact Person</td>
							<td>
								<asp:TextBox ID="txtContPerson" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
							</td>                        
						</tr>
						<tr>
							<td>Phone</td>
							<td>
								<asp:TextBox ID="txtPhnNo" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
							</td>
							
							<td>Warranty Book No.</td>
							<td>
								<asp:TextBox ID="txtWarantyBookNo" runat="server" Width="100%" ReadOnly="false"></asp:TextBox>
							</td>
                            <td>Person Cell No.</td>
							<td>
								<asp:TextBox ID="txtContPerCell" runat="server" Width="100%" ReadOnly="false"></asp:TextBox>
							</td>
                            <td>Repair Date</td>
							<td>
								<asp:TextBox ID="txtRepairDate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png);
									 background-repeat: no-repeat; padding-left: 25px;" Width="100%" ReadOnly="true"></asp:TextBox>
								<ajax:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd-MM-yyyy"  TargetControlID="txtRepairDate"></ajax:CalendarExtender>
                               
							</td>
                           
						</tr>
                     

					</table>
                            </ContentTemplate>
						</ajaxToolkit:TabPanel>
                     <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Problem Action">
                         <HeaderTemplate>Problem /Action </HeaderTemplate>
                         <ContentTemplate>
                         <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
						<tr>
							<td width="11%">Delivery Date</td>
							<td>
								<asp:TextBox ID="txtDelDate" runat="server" AutoComplete="off" Style="background-image: url(../../../Images/Calendar_16x16.png);
									 background-repeat: no-repeat; padding-left: 25px;" ReadOnly="False" Width="100%"></asp:TextBox>
								<ajax:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MM-yyyy" TargetControlID="txtDelDate"></ajax:CalendarExtender>
                                           
							</td>
							<td>Inspection Date<font color="red"><b>*</b></font></td>
							<td>
								<asp:TextBox ID="txtInspDate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png);
									 background-repeat: no-repeat; padding-left: 25px;" AutoComplete="off" ReadOnly="False" Width="100%" ></asp:TextBox>
								<ajax:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MM-yyyy" TargetControlID="txtInspDate"></ajax:CalendarExtender>
                                        
								
							</td>
							<td>Failure Date</td>
							<td>
								<asp:TextBox ID="txtFailureDate" runat="server" AutoComplete="off" Style="background-image: url(../../../Images/Calendar_16x16.png);
									 background-repeat: no-repeat; padding-left: 25px;" ReadOnly="true" Width="100%"></asp:TextBox>
								<ajax:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MM-yyyy" TargetControlID="txtFailureDate"></ajax:CalendarExtender>
                                                                      
								
							</td>
                           
						</tr>
						<tr>
							<td style="display:none;">Failed Part #
								<asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png" onclick="btnLookup_Click" Width="16px" />
							</td>
							<td style="display:none;">
								<%--<asp:DropDownList ID="ddlFailedPart" runat="server" Width="100%"></asp:DropDownList>--%>
								<asp:TextBox ID="txtFailedPart" runat="server"></asp:TextBox>
							</td>
							 <td style="display:none;">Failed Part Dispatched Status</td>
							<td style="display:none;">
								<asp:TextBox ID="txtDispStatus" runat="server" Width="100%"></asp:TextBox>
							</td>
							
							<td style="display:none;">Dispatched</td>
							<td style="display:none;">
								<asp:RadioButton ID="RadioButton1" runat="server" Text="Yes" GroupName="Dispatched"  />
								<asp:RadioButton ID="RadioButton2" runat="server" Text="No" GroupName="Dispatched"  />
							</td>
                            <td>Subject<font color="red"><b>*</b></font></td>
							<td>
								<asp:TextBox ID="txtSubject" runat="server" SkinID="ROTextBox" TextMode="MultiLine" Width="100%"></asp:TextBox>
								
							</td>
                              <td width="10%">Defect </td>
                            <td>
                            <ajaxToolkit:ComboBox ID="ddlProblemDefect" runat="server"></ajaxToolkit:ComboBox>
                          
                            </td>
							<td>Corrective Action Taken</td>
							<td >
								<asp:TextBox ID="txtCorAction" runat="server" SkinID="ROTextBox" TextMode="MultiLine" Width="100%"></asp:TextBox>
							</td>
						</tr>
						<tr>
							
                              <td width="10%">Category </td>
                            <td>
                            <ajaxToolkit:ComboBox ID="ddlsubject" runat="server"></ajaxToolkit:ComboBox>
                          
                            </td>
							<td>Customer Complaint</td>
							<td >
								<asp:TextBox ID="txtCustComp" runat="server" SkinID="ROTextBox" TextMode="MultiLine" Width="100%"></asp:TextBox>
							</td>
                            <td>Problem Cause</td>
							<td >
								<asp:TextBox ID="txtPrblmCause" runat="server" SkinID="ROTextBox" TextMode="MultiLine" Width="100%"></asp:TextBox>
							</td>

						</tr>
						<tr>
							
                           
                            
                            <td>Status</td>
							<td>
                                <ajaxToolkit:ComboBox ID="ddlStatusC" runat="server" AutoCompleteMode="Suggest" AutoPostBack="True" DropDownStyle="Simple">
                                  <asp:ListItem Selected="True">Select</asp:ListItem>
                                     <asp:ListItem>Completed</asp:ListItem>
                                  <asp:ListItem>Awaiting Parts</asp:ListItem>
                                  <asp:ListItem>Others</asp:ListItem>
                                  </ajaxToolkit:ComboBox>
                                 
								<%--<asp:DropDownList runat="server" ID="ddlStatus" width="145px">
									<asp:ListItem Selected="true" Text="Select" Value="Select"></asp:ListItem>
									<asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
									<asp:ListItem Text="Awaiting Parts" Value="Awaiting Parts"></asp:ListItem>
									<asp:ListItem Text="Others" Value="Others"></asp:ListItem>
								</asp:DropDownList>--%>
							</td>
                            	<td>Result</td>
							<td>
                                <ajaxToolkit:ComboBox ID="ddlResultC" runat="server" AutoCompleteMode="Suggest" AutoPostBack="True" DropDownStyle="Simple">
                                  <asp:ListItem Selected="True">Select</asp:ListItem>
                                     <asp:ListItem>Satisfactory</asp:ListItem>
                                  <asp:ListItem>Marginally Acceptable</asp:ListItem>
                                  <asp:ListItem>Under Observation</asp:ListItem>
                                     <asp:ListItem>Not Acceptable</asp:ListItem>
                                  </ajaxToolkit:ComboBox>
                           
								<%--<asp:DropDownList runat="server" ID="ddlResult" Width="145px">
									<asp:ListItem Selected="true" Text="Select" Value="Select"></asp:ListItem>
									<asp:ListItem Text="Satisfactory" Value="Satisfactory"></asp:ListItem>
									<asp:ListItem Text="Marginally Acceptable" Value="Marginally Acceptable"></asp:ListItem>
									<asp:ListItem Text="Under Observation" Value="Under Observation"></asp:ListItem>
									<asp:ListItem Text="Not Acceptable" Value="Not Acceptable"></asp:ListItem>
								</asp:DropDownList>--%>
							</td>
						</tr>
						
                             
					</table>
                             </ContentTemplate>
                     </ajaxToolkit:TabPanel>
                     <ajaxToolkit:TabPanel ID="TabPost" runat="server" HeaderText="Post">
                         <HeaderTemplate>Cost Impact </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
						
							
						<tr>
							<td>
								Parts 
							</td>
							<td >
								<asp:TextBox ID="txtPartCost" runat="server" SkinID="ROTextBox"  Width="100%"></asp:TextBox>
								<%--<asp:dropdownlist ID="ddlPart3" runat="server" Width="100%"></asp:dropdownlist>--%>
							</td>
                            <td>
								Lubs 
							</td>
							<td >
								<asp:TextBox ID="txtLubCost" runat="server" SkinID="ROTextBox"  Width="100%"></asp:TextBox>
								<%--<asp:dropdownlist ID="ddlPart3" runat="server" Width="100%"></asp:dropdownlist>--%>
							</td>
                            <td>
								Sublets 
							</td>
                           
							<td >
								<asp:TextBox ID="txtSubletCost" runat="server" SkinID="ROTextBox"  Width="100%"></asp:TextBox>
								<%--<asp:dropdownlist ID="ddlPart3" runat="server" Width="100%"></asp:dropdownlist>--%>
							</td>
                             </tr>
                                <tr>
                            <td>
								Labours 
							</td>
							<td >
								<asp:TextBox ID="txtLabourCost" runat="server" SkinID="ROTextBox"  Width="100%"></asp:TextBox>
								<%--<asp:dropdownlist ID="ddlPart3" runat="server" Width="100%"></asp:dropdownlist>--%>
							</td>
                           <td>
								Others 
							</td>
							<td >
								<asp:TextBox ID="txtOthersCost" runat="server" SkinID="ROTextBox"  Width="100%"></asp:TextBox>
								<%--<asp:dropdownlist ID="ddlPart3" runat="server" Width="100%"></asp:dropdownlist>--%>
							</td>
                            <td>
								Total 
							</td>
							<td >
								<asp:TextBox ID="txtTotalCost" runat="server" SkinID="ROTextBox" TextMode="MultiLine" Width="100%"></asp:TextBox>
								<%--<asp:dropdownlist ID="ddlPart3" runat="server" Width="100%"></asp:dropdownlist>--%>
							</td>
						</tr>
					</table>
                        
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                     <ajaxToolkit:TabPanel ID="TabPart" runat="server" HeaderText="Parts">
                         <HeaderTemplate>Parts</HeaderTemplate>
                         <ContentTemplate>
                                   <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                                    <tr>
                                        <td>
                                            Item Code <font color="red"><b>*</b></font>
                                        </td>
                                     <td>
                                     <asp:DropDownList runat="server" Style="width: 100% !important;" ID="ddlParts" AutoPostBack="true" OnSelectedIndexChanged="ddlParts_SelectedIndexChanged"></asp:DropDownList>
                                     </td>
                                        <td>
                                         Part No <font color="red"><b>*</b></font>
                                        </td>
                                        <td>
                                          <asp:TextBox ID="txtPartCode" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td>
                                         Part Desc <font color="red"><b>*</b></font>
                                        </td>
                                        <td>
                                           <asp:TextBox ID="txtPartDesc" runat="server" SkinID="ROTextBox" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Quantity <font color="red"><b>*</b></font>
                                        </td>
                                        <td width="27%">
                                            <asp:TextBox ID="txtQuantity" runat="server" MaxLength="3" Style="text-align: right"
                                                Width="100%" ReadOnly="true"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server"
                                                FilterType="Numbers" TargetControlID="txtQuantity" BehaviorID="_content_FilteredTextBoxExtender5">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            Price <font color="red"><b>*</b></font>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPrice" runat="server" MaxLength="6" ReadOnly="True" SkinID="ROTextBox"
                                                Style="text-align: right" Width="100%"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server"
                                                BehaviorID="_content_FilteredTextBoxExtender6" FilterType="Numbers" TargetControlID="txtPrice">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                           
                                        </td>
                                          <td>
                                            Dep <font color="red"><b>*</b></font>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDep" runat="server" MaxLength="6" ReadOnly="True"  SkinID="ROTextBox"
                                                Style="text-align: right" Width="100%" ></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                BehaviorID="_content_FilteredTextBoxExtender6" FilterType="Numbers" TargetControlID="txtDep">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                           
                                        </td>
                                    </tr>
                                    <tr>
                                     <td>
                                            Issued Quantity <font color="red"><b>*</b></font>
                                        </td>
                                        <td width="27%">
                                            <asp:TextBox ID="txtIssueQty" runat="server" MaxLength="3" Style="text-align: right"
                                                Width="100%" ReadOnly="true"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                FilterType="Numbers" TargetControlID="txtQuantity" BehaviorID="_content_FilteredTextBoxExtender5">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            Tracking No  <font color="red"><b>*</b></font>
                                        </td>
                                        <td width="27%">
                                            <asp:TextBox ID="txtTrackNo" runat="server"  Style="text-align: right"
                                                Width="100%" ReadOnly="false"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                                FilterType="Numbers" TargetControlID="txtTrackNo" BehaviorID="_content_FilteredTextBoxExtender5">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                        </td>
                                         <td colspan="2">
                                            <asp:ImageButton ID="BtnAdd0" runat="server" Height="45px" ImageUrl="~/Images/AddIMG.png"
                                                Width="45px" OnClick="BtnAdd0_Click" /><asp:ImageButton ID="BtnClear2" runat="server"
                                                    Height="40px" ImageUrl="~/Images/ClearImg.png" Width="40px"  />
                                        </td>
                                    </tr>
                                </table>
                         <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
						<tr>
                        <td colspan="6">
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
                                                     <asp:BoundField DataField="ConsNo" HeaderText="Tracking No" />
                                                    
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
                        </td>
                         
						</tr>
                       
                             
					</table>
                             </ContentTemplate>
                     </ajaxToolkit:TabPanel>
                     <ajaxToolkit:TabPanel ID="TabSublet" runat="server" HeaderText="Sublet">
                         <HeaderTemplate>Sublet</HeaderTemplate>
                         <ContentTemplate>
                         <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
						<tr>
                        <td colspan="6">
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
                                                            <asp:ImageButton ID="lnkRemove" ImageUrl="~/Images/Delete_16x16.png" runat="server" OnClick="lnkRemove_Click"></asp:ImageButton>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                                        <%--<asp:LinkButton ID="lnkRemove" runat="server" OnClick="lnkRemove_Click3">Remove</asp:LinkButton>--%>                                                    
                                                    </asp:TemplateField>
                                                </Columns>

                                                <HeaderStyle CssClass="GridHeader" />
                                                <FooterStyle CssClass="GridFooter" />
                                            </asp:GridView>
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
                         <td>Sketch</td>
							<td>
                            <asp:Label runat="server" ID="lblImgAddress"> </asp:Label>
							</td>
                            <td>
								<asp:FileUpload ID="PicUpload" runat="server" />
							</td>
                            <td> <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="OnClick_btnUpload" CausesValidation="false" PostBackUrl="~/Modules/Service/Forms/DealerTechnical.aspx"/></td>
                         </tr>
						<tr>
                        
                        <td colspan="8">
                         <asp:DataList ID="ImgDataList" RepeatDirection="Horizontal" runat="server" Width="100%">
                                                         <ItemTemplate>
                                                         <asp:Image ID="Image1" runat="server" Width="100px" Height="80px" HeaderText="Image" ImageUrl='<%# "~/" + DataBinder.Eval(Container, "DataItem.ImageAddress").ToString()%>' />
                                                         </ItemTemplate>
                                                         <ItemStyle Width="5%" HorizontalAlign="Left"></ItemStyle>
                                                       <%--  <ItemTemplate>
                                                         <asp:Image runat="server" ID="Images" ImageUrl='<%#"data:Images/png;"+Convert.ToBase64String((byte[])Eval("ImageData")) %>' />
                                                         </ItemTemplate>--%>
                                                    

                         </asp:DataList>
                        </td>
                        

						</tr>
                  <%-- <tr >
                   <td><asp:Image runat="server" ID="Image1"/></td>
                   </tr>--%>
                            
                      
                       
                             
					</table>
                             </ContentTemplate>
                     </ajaxToolkit:TabPanel>
				</ajaxToolkit:TabContainer>
               
				
                <fieldset>
					<legend>Request to OEM</legend>
					<table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
						
							
						<tr>
							<td>
								Request 
							</td>
							<td >
								<asp:TextBox ID="txtRequest" runat="server" SkinID="ROTextBox" TextMode="MultiLine" Width="100%"></asp:TextBox>
								<%--<asp:dropdownlist ID="ddlPart3" runat="server" Width="100%"></asp:dropdownlist>--%>
							</td>
                            <td>Part Attached</td>
							<td>
								<asp:RadioButton ID="RadioButton3" runat="server" Text="Yes" GroupName="Dispatched"  />
								<asp:RadioButton ID="RadioButton4" runat="server" Text="No" GroupName="Dispatched"  />
							</td>
                            </tr>
                        <tr>
                            
							<td>Follow Up Date</td>
							<td>
								<asp:TextBox ID="txtFollowUpDate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png);
									 background-repeat: no-repeat; padding-left: 25px;" Width="100%"></asp:TextBox>
								<ajax:CalendarExtender ID="CalendarExtender6" runat="server" Format="dd-MM-yyyy" TargetControlID="txtFollowUpDate"></ajax:CalendarExtender>
                           
							</td>
						</tr>
					</table>
				</fieldset>
                <fieldset class="hidden">
					<legend>Post</legend>
					<table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
						
							
						<tr>
						
                            <td>Post </td>
							<td>
								<asp:RadioButton ID="RadioButton5" runat="server" Text="Yes" GroupName="Dispatched"  />
								<asp:RadioButton ID="RadioButton6" runat="server" Text="No" GroupName="Dispatched"  />
							</td>
                            </tr>
                        <tr>
                            
							<td>Post Date</td>
							<td>
								<asp:TextBox ID="txtPostDate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png);
									 background-repeat: no-repeat; padding-left: 25px;" ReadOnly="true" Width="100%"></asp:TextBox>
								<ajax:CalendarExtender ID="CalendarExtender7" runat="server" Format="dd-MM-yyyy" TargetControlID="txtPostDate"></ajax:CalendarExtender>
                                     
							</td>
						</tr>
					</table>
				</fieldset>
              
			<table width="100%" style="margin-bottom: 10px;margin-top:10px;">
					<tr>
                     <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*) Feilds Are Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
						<td >
							<asp:Label ID="lblMsg" runat="server" Width="100%"></asp:Label>
						</td>                    
					<td style="text-align: right;">
							<asp:Button ID="btnSaveDTR" Width="100px" runat="server" CausesValidation="false" Text="Save" OnClick="btnSaveJC_Click" />
							<asp:Button ID="btnDeleteDTR" runat="server" Width="100px" Text="Delete" OnClientClick="function(s, e) {
									return confirm('Are you sure you want to Delete?');
								}" OnClick="btnDeleteDTR_Click" />
							<asp:Button ID="btnClearDTR" runat="server" Width="100px" CausesValidation="false" Text="Clear" OnClick="btnClearJC_Click" />                            
							<asp:Button ID="btnPrintJC" runat="server" Width="100px" CausesValidation="false" Text="Print" OnClick="btnPrintJC_Click" />   <%-- --%>
                            <asp:Button ID="btnPost" runat="server" Width="100px" CausesValidation="false" Text="Post" OnClick="btnPost_Click" />   <%-- --%>
						</td>
					</tr>
				</table>
                
                
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
	<asp:HiddenField ID="HFJobCard" runat="server" />
</asp:Content>
