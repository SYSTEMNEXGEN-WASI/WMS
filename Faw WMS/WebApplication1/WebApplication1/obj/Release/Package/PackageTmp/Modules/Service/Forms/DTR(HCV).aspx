<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" Theme="Theme1" AutoEventWireup="true" CodeBehind="DTR(HCV).aspx.cs" Inherits="DXBMS.Modules.Service.Forms.DTR_HCV_" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

 

		<script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
		<script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
		<script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
		<script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>

	<asp:UpdatePanel ID="UpdatePanel" runat="server">
<%--	<Triggers>               
				<asp:PostBackTrigger ControlID="btnUpload" />            
			</Triggers>--%>

		<ContentTemplate>
			<div class="container">
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
							<td>Date</td>
							<td>
								<asp:TextBox ID="txtDate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png);
									 background-repeat: no-repeat; padding-left: 25px;" Width="100%" ReadOnly="true"></asp:TextBox>
								<ajax:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MM-yyyy" TargetControlID="txtDate"></ajax:CalendarExtender>
                                
							</td>
						</tr>
					</table>
				</fieldset>
                <cc1:TabContainer ID="TabContainerDTR" runat="server" ActiveTabIndex="1" 
                    Width="100%" Height="300px">
					<cc1:TabPanel ID="TabVehicle" runat="server" HeaderText="Vehicle">
						
						<HeaderTemplate>Vehicle </HeaderTemplate>
						
						<ContentTemplate>
                            <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">                        
						<tr>
                             <td>Repair Order No.<asp:ImageButton runat="server" ID="ImageButtonRoNO" ImageUrl="~/Images/LookupNew.png" OnClick="ImageButtonRoNO_Click" Width="16px" /></td>
							<td>
								<asp:TextBox ID="txtRoNo" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
							</td> 
							<td width="11%">VIN<font color="red"><b>*</b></font></td>
							<td >
								<asp:DropDownList ID="ddlVinNo" runat="server" AutoPostBack="true" Width="" OnSelectedIndexChanged="ddlVinNo_SelectedIndexChanged">
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
							<td>Color</td>
							<td>
								<asp:TextBox ID="txtColor" runat="server"  ReadOnly="true"></asp:TextBox>    
							</td>
                            <td>Key No<font color="red"><b>*</b></font></td>
							<td>
								<asp:TextBox ID="txtKeyNo" runat="server"></asp:TextBox>
								
							</td>
						</tr>
                        <tr>
                        <td>Type</td>
							<td>
								<asp:RadioButton ID="RadioBtnCBU" runat="server" Text="CBU" GroupName="Dispatched"  />
								<asp:RadioButton ID="RadioBtnCKD" runat="server" Text="CKD" GroupName="Dispatched"  />
							</td>
                            
							<td>Body Type<font color="red"><b>*</b></font></td>
							<td>
								<asp:TextBox ID="txtBodyType" runat="server" ReadOnly="true"></asp:TextBox>
								
							</td>
							<td>PayLoad</td>
							<td>
								<asp:TextBox ID="txtPayLoad" runat="server"  ReadOnly="true"></asp:TextBox>    
							</td>
                            <td> No<font color="red"><b>*</b></font></td>
							<td>
								<asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
								
							</td>

                        </tr>
					</table>
                            </ContentTemplate>
					</cc1:TabPanel>
                    <cc1:TabPanel ID="TabCustomer" runat="server" HeaderText="Customer">
                        <HeaderTemplate>Customer </HeaderTemplate>
                        <ContentTemplate>
                        <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">                    
						<tr>
							<td width="11%">Customer Name</td>
							<td>
								<asp:TextBox ID="txtCustName" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
							</td>
							<td>Address</td>
							<td >
								<asp:TextBox ID="txtAddress" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
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
                            <td>Repair Date</td>
							<td>
								<asp:TextBox ID="txtRepairDate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png);
									 background-repeat: no-repeat; padding-left: 25px;" Width="100%" ReadOnly="true"></asp:TextBox>
								<ajax:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd-MM-yyyy"  TargetControlID="txtRepairDate"></ajax:CalendarExtender>
                               
							</td>
                           
						</tr>
					</table>
                            </ContentTemplate>
						</cc1:TabPanel>
                     <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Problem Action">
                         <HeaderTemplate>Problem /Action </HeaderTemplate>
                         <ContentTemplate>
                         <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
						<tr>
							<td width="11%">Delivery Date</td>
							<td>
								<asp:TextBox ID="txtDelDate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png);
									 background-repeat: no-repeat; padding-left: 25px;" ReadOnly="False" Width="100%"></asp:TextBox>
								<ajax:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MM-yyyy" TargetControlID="txtDelDate"></ajax:CalendarExtender>
                                           
							</td>
							<td>Inspection Date<font color="red"><b>*</b></font></td>
							<td>
								<asp:TextBox ID="txtInspDate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png);
									 background-repeat: no-repeat; padding-left: 25px;" ReadOnly="False" Width="100%"></asp:TextBox>
								<ajax:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MM-yyyy" TargetControlID="txtInspDate"></ajax:CalendarExtender>
                                        
								
							</td>
							<td>Failure Date</td>
							<td>
								<asp:TextBox ID="txtFailureDate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png);
									 background-repeat: no-repeat; padding-left: 25px;" ReadOnly="true" Width="100%"></asp:TextBox>
								<ajax:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MM-yyyy" TargetControlID="txtFailureDate"></ajax:CalendarExtender>
                                                                      
								
							</td>
                            
						</tr>
						<tr>
							<td>Failed Part #
								<asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png" onclick="btnLookup_Click" Width="16px" />
							</td>
							<td>
								<%--<asp:DropDownList ID="ddlFailedPart" runat="server" Width="100%"></asp:DropDownList>--%>
								<asp:TextBox ID="txtFailedPart" runat="server"></asp:TextBox>
							</td>
							<td>Failed Part Dispatched Status</td>
							<td>
								<asp:TextBox ID="txtDispStatus" runat="server" Width="100%"></asp:TextBox>
							</td>
							
							<td>Dispatched</td>
							<td>
								<asp:RadioButton ID="RadioButton1" runat="server" Text="Yes" GroupName="Dispatched"  />
								<asp:RadioButton ID="RadioButton2" runat="server" Text="No" GroupName="Dispatched"  />
							</td>
						</tr>
						<tr>
							<td>Subject<font color="red"><b>*</b></font></td>
							<td colspan="2">
								<asp:TextBox ID="txtSubject" runat="server" SkinID="ROTextBox" TextMode="MultiLine" Width="100%"></asp:TextBox>
								
							</td>
							<td>Customer Complaint</td>
							<td colspan="2">
								<asp:TextBox ID="txtCustComp" runat="server" SkinID="ROTextBox" TextMode="MultiLine" Width="100%"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td>Problem Cause</td>
							<td colspan="2">
								<asp:TextBox ID="txtPrblmCause" runat="server" SkinID="ROTextBox" TextMode="MultiLine" Width="100%"></asp:TextBox>
							</td>
							<td>Corrective Action Taken</td>
							<td colspan="2">
								<asp:TextBox ID="txtCorAction" runat="server" SkinID="ROTextBox" TextMode="MultiLine" Width="100%"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td>Status</td>
							<td>
								<asp:DropDownList runat="server" ID="ddlStatus" width="145px">
									<asp:ListItem Selected="true" Text="Select" Value="Select"></asp:ListItem>
									<asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
									<asp:ListItem Text="Awaiting Parts" Value="Awaiting Parts"></asp:ListItem>
									<asp:ListItem Text="Others" Value="Others"></asp:ListItem>
								</asp:DropDownList>
							</td>
							<td>Result</td>
							<td>
								<asp:DropDownList runat="server" ID="ddlResult" Width="145px">
									<asp:ListItem Selected="true" Text="Select" Value="Select"></asp:ListItem>
									<asp:ListItem Text="Satisfactory" Value="Satisfactory"></asp:ListItem>
									<asp:ListItem Text="Marginally Acceptable" Value="Marginally Acceptable"></asp:ListItem>
									<asp:ListItem Text="Under Observation" Value="Under Observation"></asp:ListItem>
									<asp:ListItem Text="Not Acceptable" Value="Not Acceptable"></asp:ListItem>
								</asp:DropDownList>
							</td>
							<td>Sketch <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" CausesValidation="false" /></td>
							<td>
								<asp:FileUpload ID="PicUpload" runat="server" />
							</td>
						</tr>
					</table>
                             </ContentTemplate>
                     </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPost" runat="server" HeaderText="Post">
                         <HeaderTemplate>Post </HeaderTemplate>
                        <ContentTemplate>
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
                            </ContentTemplate>
                        </cc1:TabPanel>
				</cc1:TabContainer>
				<fieldset>
					<legend>Replace / Required</legend>
					<table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
						<tr>
							<td width="11%">Part 1
								<asp:ImageButton runat="server" ID="ImageButton1" ImageUrl="~/Images/LookupNew.png" onclick="imgLookup1_Click" Width="16px" />
							</td>
							<td Width="40%">
								<asp:TextBox ID="txtPart1" Width="35%" runat="server"></asp:TextBox>
                                <asp:TextBox ID="txtPart1Desc"  Width="60%" runat="server"></asp:TextBox>
								<%--<asp:dropdownlist ID="ddlPart1" runat="server" Width="100%"></asp:dropdownlist>--%>
							</td>
							<td width="10%">Part 2
								<asp:ImageButton runat="server" ID="ImageButton2" ImageUrl="~/Images/LookupNew.png" onclick="imgLookup2_Click" Width="16px" />
							</td>
							<td>
								<asp:TextBox ID="txtPart2"  Width="35%" runat="server"></asp:TextBox>
                                <asp:TextBox ID="txtPart2Desc"  Width="60%" runat="server"></asp:TextBox>
								<%--<asp:dropdownlist ID="ddlPart2" runat="server" Width="100%"></asp:dropdownlist>--%>
							</td>
						</tr>
						<tr>
							<td>Part 3
								<asp:ImageButton runat="server" ID="ImageButton3" ImageUrl="~/Images/LookupNew.png" onclick="imgLookup3_Click" Width="16px" />
							</td>
							<td>
								<asp:TextBox ID="txtPart3"  Width="35%" runat="server"></asp:TextBox>
                                 <asp:TextBox ID="txtPart3Desc"  Width="60%" runat="server"></asp:TextBox>
								<%--<asp:dropdownlist ID="ddlPart3" runat="server" Width="100%"></asp:dropdownlist>--%>
							</td>
							<td>Part 4
								<asp:ImageButton runat="server" ID="ImageButton4" ImageUrl="~/Images/LookupNew.png" onclick="imgLookup4_Click" Width="16px" />
							</td>
							<td>
								<asp:TextBox ID="txtPart4"  Width="35%" runat="server"></asp:TextBox>
                                 <asp:TextBox ID="txtPart4Desc"  Width="60%" runat="server"></asp:TextBox>
								<%--<asp:dropdownlist ID="ddlPart4" runat="server" Width="100%"></asp:dropdownlist>--%>
							</td>
						</tr>
					</table>
				</fieldset>
                <fieldset>
					<legend>Request to AHFM</legend>
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
              
				<table width="100%" >
					<tr>
						<td >
							<asp:Label ID="lblMsg" runat="server" Width="100%"></asp:Label>
						</td>                    
						<td align="center">
							<asp:Button ID="btnSaveDTR" runat="server" CausesValidation="false" Text="Save" OnClick="btnSaveJC_Click" />
							<%--<asp:Button ID="btnDeleteDTR" runat="server" Text="Delete" OnClientClick="function(s, e) {
									return confirm('Are you sure you want to Delete?');
								}" OnClick="btnDeleteJC_Click" />--%>
							<asp:Button ID="btnClearDTR" runat="server" CausesValidation="false" Text="Clear" OnClick="btnClearJC_Click" />                            
							<asp:Button ID="btnPrintJC" runat="server" CausesValidation="false" Text="Print" OnClick="btnPrintJC_Click" />   <%-- --%>
                            <asp:Button ID="btnPost" runat="server" CausesValidation="false" Text="Post" OnClick="btnPost_Click" />   <%-- --%>
						</td>
					</tr>
				</table>
                
                
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
	<asp:HiddenField ID="HFJobCard" runat="server" />
</asp:Content>

