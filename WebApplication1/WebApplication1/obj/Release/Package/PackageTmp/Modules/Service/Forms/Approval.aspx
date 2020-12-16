<%@ Page Title="" Language="C#" Theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="Approval.aspx.cs" Inherits="DXBMS.Modules.Service.Forms.Approval" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
        <asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">
            <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
		        <ContentTemplate>
			        <div class="container">
				        <style>
					        .txt
					        {
						        text-align: right;
					        }
				        </style>				
                        <div class="Heading">
							Warranty/Claim Report
								</div>				
                        <fieldset>
					<legend>Warranty/Job Card Reference </legend>
					<table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
						<tr>
						<td width="10%">
							W.R. No&nbsp;
							<asp:Label runat="server" ID="lbl1" ForeColor="Red" Text="*"></asp:Label>
						</td>
						<td style="width: 30%" colspan="2">
							<%--<asp:DropDownList id="ddlPRNo" runat="server" width="100% "OnSelectedIndexChanged="ddlPPRNo_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>--%>
                            <asp:DropDownList ID="ddlPRNo" runat="server" OnSelectedIndexChanged="ddlPRNo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
							<cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" IsSorted="True" QueryPattern="Contains"
											TargetControlID="ddlPRNo" Enabled="True"></cc1:ListSearchExtender>
							<%--<asp:TextBox ID="txtPPRNo" runat="server" Visible="False"></asp:TextBox>--%>
						</td>
						<td width="15%">
							W.R. Date
						</td>
						<td style="width: 15%">
							<asp:TextBox ID="txtPPRDate" runat="server" CssClass="ROTextBoxes" ReadOnly="True"
								Width="115px"></asp:TextBox>
							<cc1:MaskedEditExtender ID="MaskedEditExtender5" runat="server" ClearMaskOnLostFocus="False"
								CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
								CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
								CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="99/99/9999"
								MaskType="Date" TargetControlID="txtPPRDate" />
						</td>
						<td align="right" width="15%">
							&nbsp;
						</td>
					</tr>
						
						<tr>
							<td>
								JobCard No
								<asp:Label runat="server" ID="lbl2" ForeColor="Red" Text="*"></asp:Label>
								<asp:TextBox ID="txtJobCardNo" runat="server" ReadOnly="True" Visible="False"></asp:TextBox>
								<cc1:MaskedEditExtender ID="MaskedEditExtender3" runat="server" ClearMaskOnLostFocus="False"
									CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
									CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
									CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="99/99999"
									MaskType="None" TargetControlID="txtJobCardNo" />
							</td>							
							
							<td>
                            <asp:TextBox ID="txtJobCode" runat="server" ReadOnly="True" width="250px"></asp:TextBox>
								<%--<asp:DropDownList ID="ddljob_code" runat="server" Width="250px" OnSelectedIndexChanged="ddljob_code_SelectedIndexChanged" AutoPostBack="true" Visible="false"></asp:DropDownList>--%>
							</td>
							<td width="10%" align="right">
								JobCard Date
							</td>
							<td>
								<asp:TextBox ID="txtJobCardDate" runat="server" CssClass="ROTextBoxes" ReadOnly="True"
									Width="115px"></asp:TextBox>
								<cc1:MaskedEditExtender ID="txtJobCardDate_MaskedEditExtender" runat="server" ClearMaskOnLostFocus="False"
									CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
									CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
									CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="99/99/9999"
									MaskType="Date" TargetControlID="txtJobCardDate" />
							</td>
							<td >
								Total Amount
							</td>
							<td>
								<asp:TextBox ID="txtPPRTotalAmount" runat="server" CssClass="ROTextBoxes" ReadOnly="True"></asp:TextBox>
							</td>
						</tr>
					</table>
				</fieldset>
				        <fieldset>
					<legend>Customer/Vehicle Information </legend>
					<table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
						<tr>
							<td width="15%">
								Registration No<asp:Label runat="server" ID="lbl3" ForeColor="Red" Text="*"></asp:Label>
							</td>
							<td width="15%">
								<asp:TextBox ID="txtRegNo" runat="server" CssClass="ROTextBoxes" ReadOnly="True"></asp:TextBox>
							</td>
							<td  width="15%">
								Chassis No
								<asp:Label runat="server" ID="lbl5" ForeColor="Red" Text="*"></asp:Label>								
							</td>
							<td width="15%">
								<asp:TextBox ID="txtChasisNo" runat="server" CssClass="ROTextBoxes" ReadOnly="True"></asp:TextBox>
							</td>
						
							<td width="15%">
								Engine No
								<asp:Label runat="server" ID="lbl4" ForeColor="Red" Text="*"></asp:Label>
							</td>
						   
							<td width="15%">
								<asp:TextBox ID="txtEngNo" runat="server" CssClass="ROTextBoxes" ReadOnly="True"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td>
								Vehicle Model
							</td>
							<td>
								<asp:TextBox ID="txtVehModel" runat="server" CssClass="ROTextBoxes" ReadOnly="True"></asp:TextBox>
							</td>
							<td >
								Invoice No
							</td>
							<td>
								<asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="ROTextBoxes" ReadOnly="True"></asp:TextBox>
							</td>
						
							<td >
								Invoice Date
							</td>
							<td>
								<asp:TextBox ID="txtInvDate" runat="server" CssClass="ROTextBoxes" ReadOnly="True"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td>
								Vehicle
							</td>
							<td>
								<asp:TextBox ID="txtProdCode" runat="server" CssClass="ROTextBoxes" ReadOnly="True"></asp:TextBox>
							</td>
							<td>
								Vehicle Color
							</td>
							<td>
								<asp:TextBox ID="txtVehColor" runat="server" CssClass="ROTextBoxes" ReadOnly="True"></asp:TextBox>
							</td>
							<td>Road Condition</td>
							<td>
								<asp:DropDownList ID="ddlRoadCond" runat="server" width="147px"></asp:DropDownList>
							</td>
							
							
						</tr>
					   <%-- <tr>
							<td>
								Sales Dealer Code
							</td>
							<td>
								<asp:TextBox ID="txtDealerCode" runat="server" CssClass="ROTextBoxes" ReadOnly="True"></asp:TextBox>
							</td>
							<td align="right">
								Sales Dealer Name
							</td>
							<td>
								<asp:TextBox ID="txtDealerName" runat="server" Width="288px" CssClass="ROTextBoxes"
									ReadOnly="True"></asp:TextBox>
							</td>
						</tr>--%>
					
						<tr>
							<td>
								Customer Name
							</td>
							<td>
								<asp:TextBox ID="txtCustCode" runat="server" CssClass="ROTextBoxes" ReadOnly="True" Visible="false"
									Width="80px"></asp:TextBox>
								<asp:TextBox ID="txtCustName" runat="server" CssClass="ROTextBoxes" ReadOnly="True"
									Width="147px"></asp:TextBox>
							</td>
							<td>
								User&#39;s Name
							</td>
							<td colspan="2">
								<asp:TextBox ID="txtUserName" runat="server" Width="288px" MaxLength="50" ReadOnly="true"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td>
								User&#39;s Address
							</td>
							<td>
								<asp:TextBox ID="txtUserAddress" runat="server" Width="239px" MaxLength="50" ReadOnly="true"></asp:TextBox>
							</td>
							<td>
								Phone No
							</td>
							<td>
								<asp:TextBox ID="txtPhoneNo" runat="server"  MaxLength="15" ReadOnly="true"></asp:TextBox>
								<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
									TargetControlID="txtPhoneNo" FilterMode="ValidChars">
								</cc1:FilteredTextBoxExtender>
							</td>							
						</tr>
						<tr>
							<td >
								Kilometer Reading<asp:Label runat="server" ID="lbl10" ForeColor="Red" Text="*"></asp:Label>
							</td>
							<td>
								<asp:TextBox ID="txtKM" runat="server" Width="147px" MaxLength="5" ReadOnly="true"></asp:TextBox>
								<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterMode="ValidChars"
									FilterType="Numbers" TargetControlID="txtKM">
								</cc1:FilteredTextBoxExtender>
							</td>
							<td >
								Problem Occured On
								<asp:Label runat="server" ID="lbl7" ForeColor="Red" Text="*"></asp:Label>
							</td>
							<td>
								<asp:TextBox ID="dtProblemDate" runat="server"></asp:TextBox>
								<cc1:CalendarExtender ID="ExdtInsDate" runat="server" TargetControlID="dtProblemDate"
									Format="dd/MM/yyyy">
								</cc1:CalendarExtender>
								<%--<%@ Register Assembly="DatePickerControl" Namespace="DatePickerControl" TagPrefix="cc1" %>--%>
							</td>
							<td>
								Date of Complition
							</td>
							<td>
								<asp:TextBox ID="dtComplitionDate" runat="server"></asp:TextBox>
								<cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="dtComplitionDate"
									Format="dd/MM/yyyy">
								</cc1:CalendarExtender>
								<%--<%@ Register Assembly="DatePickerControl" Namespace="DatePickerControl" TagPrefix="cc1" %>--%>
							</td>
						</tr>
					</table>
				</fieldset>
				        <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="1" Width="100%" Height="250px">
					<cc1:TabPanel ID="TabPPRCause" TabIndex="0" runat="server" HeaderText="P.P.R. Cause">
						<HeaderTemplate>W.R. Cause</HeaderTemplate>
						<ContentTemplate>
							<table width="100%"  style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
								<tr>
									<td style="width: 15%">Complaint</td>
									<td style="width: 40%" align="left">
										<asp:DropDownList ID="ddlComplains" runat="server" Width="70%"></asp:DropDownList>
										<cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" Enabled="True" IsSorted="True" QueryPattern="Contains" TargetControlID="ddlComplains"></cc1:ListSearchExtender></td>
                                    <td style="width: 10%">Defect </td>
                                    <td style="width: 35%"><asp:DropDownList ID="ddlDefect" runat="server" Width="60%"></asp:DropDownList><cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" IsSorted="True" QueryPattern="Contains"
											TargetControlID="ddlDefect" Enabled="True"></cc1:ListSearchExtender>
									</td>
								</tr>
								<tr>
									<td>Labor Description </td>
									<td>
										<asp:TextBox ID="txtLaborDesc" runat="server" CssClass="ROTextBoxes" MaxLength="50" Width="70%" ReadOnly="true"></asp:TextBox>
									</td>
									<td>Amount </td>
									<td>
										<asp:TextBox ID="txtLaborAmount" runat="server" CssClass="ROTextBoxes" Width="60%" ReadOnly="true"></asp:TextBox>
									</td>
								</tr>
						    <table>
							<table Width="100%">
									<tr>
										<td style="padding-left:10px;">Description of Complain in Detail</td>
										<td style="padding-left:10px;">Root Cause Failure</td>
										<td style="padding-left:10px;">Corrective Action</td>
									</tr>
								    <tr>
									<td width="33%">
										<asp:TextBox ID="txtCompletionDesc" runat="server" Height="100%" width="90%" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
									</td>
									<td width="33%">
										<asp:TextBox ID="txtRootCauseFailure" runat="server" Height="100%" width="90%" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
									</td>
									<td width="33%">
										<asp:TextBox ID="txtCorrectiveAction" runat="server" Height="100%" width="90%" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
									</td>
								</tr>
							</table>
						</ContentTemplate>
					</cc1:TabPanel>
					<cc1:TabPanel ID="TabPanel2" TabIndex="1" runat="server" HeaderText="P.P.R. Parts">
						<HeaderTemplate>W.R. Parts</HeaderTemplate>
						<ContentTemplate>
                            <table>
								<tr>                                    
									<td width="1%">Vendor 
										<asp:Label ID="lbl9" runat="server" ForeColor="Red" Text="*"></asp:Label>
									</td>
									<td width="12%" nowrap="nowrap">
										<asp:DropDownList ID="ddlVendor" runat="server"></asp:DropDownList>
										<cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" IsSorted="True" QueryPattern="Contains"
											TargetControlID="ddlVendor" BehaviorID="_content_ListSearchExtender1"></cc1:ListSearchExtender>
									</td>
								</tr>
							</table>
							<asp:GridView ID="grdParts" runat="server" AutoGenerateColumns="False" OnRowDataBound="grdParts_RowDataBound"
							 ShowHeaderWhenEmpty="True"	Width="100%" >
								<Columns>
									<asp:TemplateField HeaderText="S No">
										<ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
										<ItemStyle Width="5%" HorizontalAlign="Center" />
									</asp:TemplateField>
									<asp:BoundField DataField="PartNo" HeaderText="Part No" />
									<asp:BoundField DataField="ItemCode" HeaderText="Item"/>
									<asp:BoundField DataField="ItemDesc" HeaderText="Description"/>
									<asp:BoundField DataField="Qty" HeaderText="Quantity">
										<ItemStyle HorizontalAlign="Right" /></asp:BoundField>
									<asp:BoundField DataField="Price" HeaderText="Price">
										<ItemStyle HorizontalAlign="Right" /></asp:BoundField>
									<asp:BoundField DataField="GrossAmount" HeaderText="Total">
										<ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                    <asp:TemplateField HeaderText="Remove">
                                        <ItemTemplate >
                                            <asp:ImageButton ID="BtnRemove_Labor" runat="server" CausesValidation="false"
                                                ImageUrl="~/Images/Delete_16x16.png" onclick="BtnRemove_Labor_Click" ToolTip="Remove" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
								</Columns><AlternatingRowStyle BackColor="#CCCCCC" />
								<HeaderStyle CssClass="GridHeader" />
								<FooterStyle CssClass="GridFooter" />
							</asp:GridView>
							
						</ContentTemplate>
					</cc1:TabPanel>
					<cc1:TabPanel ID="TabPanel3" TabIndex="2" runat="server" HeaderText="Approval">
						<ContentTemplate>
							<table width="100%" style="border-collapse:separate; border-spacing:5px; margin-bottom:0;" class="table table-condensed table-bordered table-hover" >
								<tr>
									<td width="15%">Approval No. </td>
									<td width="5%">
										<asp:TextBox ID="txtAprovalNo" runat="server"></asp:TextBox>
									</td>
									<td width="15%">Approval Date </td>
									<td>
										<asp:TextBox ID="txtAprovalDate" runat="server" MaxLength="10"></asp:TextBox>
										<cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtAprovalDate" Format="dd-MM-yyyy"></cc1:CalendarExtender>
									</td>
									<td width="15%">Approval Status</td>
									<td>
										<asp:DropDownList ID="ddlAppSt" runat="server"></asp:DropDownList>
									</td>

								</tr>
								<tr>
									<td width="15%">TR No. </td>
									<td width="5%"><asp:TextBox ID="txtTRNo" runat="server"></asp:TextBox>
									</td>
									<td width="15%">TR Date </td>
									<td>
										<asp:TextBox ID="txtTRDate" runat="server"></asp:TextBox>
										<cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtTRDate" Format="dd-MM-yyyy"></cc1:CalendarExtender>
									</td>
								</tr>
                                </table>
							<table width="100%">
									<tr>
										<td style="padding-left:10px;">Approval/Reject Remarks</td>
										<td align="right">
											<asp:Button ID="btnWarSave" runat="server" Enabled="False" Text="Save" OnClick="btnWarSave_Click" />
											<asp:Button ID="btnWarClear" runat="server" Text="Clear" OnClick="btnWarClear_Click" />
										</td>
									</tr>
									<tr>
										<td>
											<asp:TextBox ID="txtComplainRemarks" runat="server" Height="100%" MaxLength="200" TextMode="MultiLine" Width="400px"></asp:TextBox>
										</td>
									</tr>
								</table>								
						</ContentTemplate>
					</cc1:TabPanel>
				</cc1:TabContainer>
                        <table width="100%">
					<tr>
						<td>
							&nbsp;
							<asp:Label ID="lblMsg" runat="server"></asp:Label>
						</td>						
						<td align="center">
							<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait';"
								UseSubmitBehavior="false" />
							<asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
							<asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
							<asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
							<asp:Button ID="btnPost" runat="server" Text="Post" OnClick="btnPost_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait';"
								UseSubmitBehavior="false" />
						</td>
					</tr>	
											
				</table>
			        </div>
		        </ContentTemplate>
	        </asp:UpdatePanel>
        </asp:Content>

