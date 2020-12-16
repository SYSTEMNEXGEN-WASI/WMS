<%@ Page Title="" Language="C#" Theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="Warranty.aspx.cs" Inherits="DXBMS.Modules.Service.Forms.Warranty" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">
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
               window.location.href = "Warranty.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "Warranty.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "Warranty.aspx";
           }
           function reloads() {
               window.location.href = "Warranty.aspx";
           }

       </script>

	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<div class="container">
				<style>
				    .txt {
				        text-align: right;
				    }
				</style>
                 <div style="font-size: 15px;">
                    <span class="fa-arrow-left fa"></span>
                    <asp:LinkButton ID="lnkBack" ForeColor="Black" runat="server" PostBackUrl="~/Modules/Service/Forms/WRAppr-Reject.aspx" Text="Back"></asp:LinkButton>

                </div>
				<div class="Heading">Warranty/Claim Report</div>
				<fieldset>
					<legend>Warranty/Job Card Reference </legend>
					<table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
						<tr>
						<td width="10%">
							W.R. No&nbsp;
							<asp:Label runat="server" ID="lbl1" ForeColor="Red" Text="*"></asp:Label>
						</td>
						<td  colspan="2">
							<asp:DropDownList ID="ddlPPRNo" runat="server" Width="100%" OnSelectedIndexChanged="ddlPPRNo_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
							<cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" IsSorted="True" QueryPattern="Contains"
											TargetControlID="ddlPPRNo" Enabled="True"></cc1:ListSearchExtender>
							<%--<asp:TextBox ID="txtPPRNo" runat="server" Visible="False"></asp:TextBox>--%>
						</td>
						<td width="15%">
							W.R. Date
						</td>
						<td style="width: 15%">
							<asp:TextBox ID="txtPPRDate" runat="server" CssClass="ROTextBoxes" ReadOnly="True"
								Width="80%"></asp:TextBox>
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
                            <td>DTR  No.<asp:ImageButton runat="server" ID="ImageButtonDTRNO" ImageUrl="~/Images/LookupNew.png" OnClick="ImageButtonDTRNO_Click1" Width="16px" /></td>
							<td>
								<asp:TextBox ID="txtDTRNo" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
							</td> 
							<td>
								JobCard No
								<asp:Label runat="server" ID="lbl2" ForeColor="Red" Text="*"></asp:Label>
								<%--<asp:TextBox ID="txtJobCardNo" runat="server" ReadOnly="True" Visible="False"></asp:TextBox>
								<cc1:MaskedEditExtender ID="MaskedEditExtender3" runat="server" ClearMaskOnLostFocus="False"
									CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
									CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
									CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="99/99999"
									MaskType="None" TargetControlID="txtJobCardNo" />--%>
							</td>							
							
							<td>
								<asp:TextBox ID="txtJobCardNo" runat="server" Width="95%"  ReadOnly="true" AutoPostBack="true"></asp:TextBox>
							</td>
							<td width="10%" align="right">
								JobCard Date
							</td>
							<td>
								<asp:TextBox ID="txtJobCardDate" runat="server" CssClass="ROTextBoxes" ReadOnly="True" Width="115px"
                                Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 100%;"	></asp:TextBox>
								<cc1:CalendarExtender ID="txtRecieptDate_CalendarExtender" runat="server"
                            Enabled="True" TargetControlID="txtJobCardDate" />
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
								<asp:TextBox ID="txtVehModel" runat="server" width="25%" CssClass="ROTextBoxes" ReadOnly="True"></asp:TextBox>
                                <asp:TextBox ID="txtVersionDesc" runat="server" Width="70%" CssClass="ROTextBoxes" ReadOnly="True"></asp:TextBox>
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
								<asp:TextBox ID="txtProdCode" runat="server" width="30%" CssClass="ROTextBoxes" ReadOnly="True"></asp:TextBox>
                                <asp:TextBox ID="txtProdDesc" runat="server" Width="60%" CssClass="ROTextBoxes" ReadOnly="True"></asp:TextBox>
							</td>
							<td>
								Vehicle Color
							</td>
							<td>
								<asp:TextBox ID="txtVehColor" runat="server" CssClass="ROTextBoxes" ReadOnly="True"></asp:TextBox>
							</td>
							<td>Road Condition</td>
							<td>
								<asp:DropDownList ID="ddlRoadCond" runat="server" style=" width: 147px !important;" ></asp:DropDownList>
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
								<asp:TextBox ID="txtCustCode" runat="server" Visible="false" CssClass="ROTextBoxes" ReadOnly="True"
									Width="80px"></asp:TextBox>
								<asp:TextBox ID="txtCustName" runat="server" CssClass="ROTextBoxes" ReadOnly="True"
									Width="145px"></asp:TextBox>
							</td>
							<td>
								User&#39;s Name
							</td>
							<td >
								<asp:TextBox ID="txtUserName" runat="server" Width="145px" MaxLength="50"></asp:TextBox>
							</td>
                            <td>
								Vehicle &#39;s Category
							</td>
							<td >
								<asp:TextBox ID="txtVehicleCategory" ReadOnly="true" runat="server" Width="145px" MaxLength="3"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td>
								User&#39;s Address
							</td>
							<td>
								<asp:TextBox ID="txtUserAddress" runat="server" Width="239px" MaxLength="50"></asp:TextBox>
							</td>
							<td>
								Phone No
							</td>
							<td>
								<asp:TextBox ID="txtPhoneNo" runat="server"  MaxLength="15"></asp:TextBox>
								<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
									TargetControlID="txtPhoneNo" FilterMode="ValidChars">
								</cc1:FilteredTextBoxExtender>
							</td>	
                            <td width="10%" >
								Delivery Date
							</td>
							<td>
								<asp:TextBox ID="txtDelvDate" runat="server" CssClass="ROTextBoxes" ReadOnly="True" Width="115px"
                                Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 100%;"	></asp:TextBox>
								<cc1:CalendarExtender ID="CalendarExtender2" runat="server"
                            Enabled="True" TargetControlID="txtDelvDate" />
							</td>						
						</tr>
						<tr>
							<td >
								Kilometer Reading<asp:Label runat="server" ID="lbl10" ForeColor="Red" Text="*"></asp:Label>
							</td>
							<td>
								<asp:TextBox ID="txtKM" runat="server" Width="147px" MaxLength="5"></asp:TextBox>
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
				<cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="1" 
                    Width="100%" Height="100%">
					<cc1:TabPanel ID="TabPPRCause" runat="server" HeaderText="P.P.R. Cause">
						
						<HeaderTemplate>W.R. Cause</HeaderTemplate>
						
						<ContentTemplate>
							
							<table width="100%" style" border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
								<tr>
									
									<td style="width: 15%">Complaint</td>
									
									<td style="width: 40%" align="left">
										
										<asp:DropDownList ID="ddlComplains" runat="server" Width="70%"></asp:DropDownList>										
										<cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" Enabled="True" IsSorted="True" QueryPattern="Contains" TargetControlID="ddlComplains"></cc1:ListSearchExtender>
                                    </td>
                                    <td style="width: 10%">Defect </td>
                                    <td style="width: 35%">
                                        <asp:DropDownList ID="ddlDefect" runat="server" Width="60%"></asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" IsSorted="True" QueryPattern="Contains"
											TargetControlID="ddlDefect" Enabled="True"></cc1:ListSearchExtender>
									    
									</td>
								    
								</tr>
								
								<tr>
									
									<td>Labor Description</td>
		
									
									<td>
										
										<asp:TextBox ID="txtLaborDesc" runat="server" value="Test" Visible="false" CssClass="ROTextBoxes" MaxLength="50" Width="70%"></asp:TextBox>
									    
									</td>
									
									<td>Amount </td>
									
									<td>
										
										<asp:TextBox ID="txtLaborAmount" runat="server" Visible="false" value="0" CssClass="ROTextBoxes" Width="60%"></asp:TextBox>
									    
									</td>
								    
								</tr>
								
								</table>
								    
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
					<cc1:TabPanel ID="TabPanel2" runat="server" Width="100%" Height="100%" HeaderText="P.P.R. Parts">
						
						<HeaderTemplate>W.R. Parts</HeaderTemplate>
						
						<ContentTemplate>
                            <table>
								
								<tr>                                    
									<td width="1%">Vendor<asp:Label ID="lbl9" runat="server" ForeColor="Red" Text="*"></asp:Label></td>									
									<td width="50%" nowrap="nowrap">
										
										<asp:DropDownList ID="ddlVendor" runat="server"></asp:DropDownList>
										
										<cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" IsSorted="True" QueryPattern="Contains"
											TargetControlID="ddlVendor" Enabled="True"></cc1:ListSearchExtender>
									    
									</td>
								    
								</tr>
							    
							
							<tr>
                            <td  colspan="6">
                            <asp:GridView ID="grdParts" runat="server" AutoGenerateColumns="False" AllowPaging="true" IsPostBack="false"
                                OnRowDataBound="grdParts_RowDataBound"
                                OnRowCancelingEdit="grdParts_RowCancelingEdit"
                                OnRowEditing="grdParts_RowEditing"
                                OnRowUpdating="grdParts_RowUpdating"
							 ShowHeaderWhenEmpty="True"	Width="100%"  Height="100%">
								
								<Columns>
									<asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                        UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>
									<asp:TemplateField HeaderText="S No">
										
										<ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
										
										<ItemStyle Width="5%" HorizontalAlign="Center" />
									    
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Part No">
                                         <ItemTemplate>
                                                            <asp:Label ID="PartNo" runat="server" Text='<%# Bind("PartNo") %>'></asp:Label>
                                                        </ItemTemplate>
									</asp:TemplateField>
									<%--<asp:BoundField DataField="PartNo" HeaderText="Part No" />--%>
									<asp:TemplateField HeaderText="Description">
                                         <ItemTemplate>
                                                            <asp:Label ID="ItemDesc" runat="server" Text='<%# Bind("ItemDesc") %>'></asp:Label>
                                                        </ItemTemplate>
									</asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item No">
                                         <ItemTemplate>
                                                            <asp:Label ID="ItemCode" runat="server" Text='<%# Bind("ItemCode") %>'></asp:Label>
                                                        </ItemTemplate>
									</asp:TemplateField>
									<%--<asp:BoundField DataField="ItemCode" HeaderText="Item"/>--%>
									
									<%--<asp:BoundField DataField="ItemDesc" HeaderText="Description"/>--%>
                                       <asp:TemplateField HeaderText="Part Source">
                                      <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEditType" runat="server"  Font-Names="monospace" Width="100%">
                                                              <asp:ListItem Value="0"> Select</asp:ListItem>
                                                              <asp:ListItem Value="001">AHFM</asp:ListItem>
                                                                <asp:ListItem Value="002">Local Purchase</asp:ListItem>
                                        </asp:DropDownList>
                                          <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" IsSorted="True" QueryPattern="Contains"
											TargetControlID="ddlEditType" Enabled="True"></cc1:ListSearchExtender>
									    
                                      </EditItemTemplate>
                                      <ItemTemplate>
                                      <asp:Label ID="lblPartSource" runat="server" Text='<%# Bind("PartSourceCode") %>'></asp:Label>
                                      </ItemTemplate>
                                      <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Quantity ">
                                         <ItemTemplate>
                                                            <asp:Label ID="Qty" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                                                        </ItemTemplate>
									</asp:TemplateField>
                                                    
                                     <asp:TemplateField HeaderText="Rate">
                                         <ItemTemplate>
                                                            <asp:Label ID="Rate" runat="server" Text='<%# Bind("Price") %>'></asp:Label>
                                                        </ItemTemplate>
									</asp:TemplateField>
                                     <asp:TemplateField HeaderText="Total">
                                         <ItemTemplate>
                                                            <asp:Label ID="Total" runat="server" Text='<%# Bind("GrossAmount") %>'></asp:Label>
                                                        </ItemTemplate>
									</asp:TemplateField>
									<%--<asp:BoundField DataField="Qty" HeaderText="Quantity"></asp:BoundField>
									<asp:BoundField DataField="Price" HeaderText="Rate"></asp:BoundField>
									<asp:BoundField DataField="GrossAmount" HeaderText="Total"></asp:BoundField>--%>
                                    <asp:TemplateField HeaderText="GST %">
                                         <ItemTemplate>
                                                            <asp:Label ID="GSTPer" runat="server" Text='<%# Bind("GSTPer") %>'></asp:Label>
                                                        </ItemTemplate>
									</asp:TemplateField>
                                     <asp:TemplateField HeaderText="GST Amt">
                                         <ItemTemplate>
                                                            <asp:Label ID="GSTAmount" runat="server" Text='<%# Bind("GSTAmount") %>'></asp:Label>
                                                        </ItemTemplate>
									</asp:TemplateField>
                                    <asp:TemplateField HeaderText="Handling Amt">
                                         <ItemTemplate>
                                                            <asp:Label ID="HandlingAmount" runat="server" Text='<%# Bind("HandlingAmount") %>'></asp:Label>
                                                        </ItemTemplate>
									</asp:TemplateField>
                                 <%--   <asp:BoundField DataField="GSTPer" HeaderText="GST %"></asp:BoundField>
                                    <asp:BoundField DataField="GSTAmount" HeaderText="GST Amt"></asp:BoundField>--%>
                                 <%--   <asp:BoundField DataField="HandlingAmount" HeaderText="Handling Amt"></asp:BoundField>--%>
                                  <asp:TemplateField HeaderText="Part Inv">
                                      <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtInvNo"></asp:TextBox>
                                          
                                      </EditItemTemplate>
                                      <ItemTemplate>
                                      <asp:Label ID="tbttrackInv" runat="server" Text='<%# Bind("trackInvNo") %>'></asp:Label>
                                      </ItemTemplate>
                                      <ItemStyle HorizontalAlign="Center" />
                                      </asp:TemplateField>

                                      <%-- Consigment No --%>
                                       <asp:TemplateField HeaderText="Consigment No">
                                      <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="txtConsNo" Text='<%# Bind("ConsNo") %>'></asp:TextBox>
                                          
                                      </EditItemTemplate>
                                      <ItemTemplate>
                                      <asp:Label ID="lblCons" runat="server" Text='<%# Bind("ConsNo") %>'></asp:Label>
                                      </ItemTemplate>
                                      <ItemStyle HorizontalAlign="Center" />
                                      </asp:TemplateField>
								</Columns><AlternatingRowStyle BackColor="#CCCCCC" />
								
								<HeaderStyle CssClass="GridHeader" />
								
								<FooterStyle CssClass="GridFooter" />
							    
							</asp:GridView>
                            </td>
						</tr>
                        <tr>	
						

                     <td>
                      <asp:Label ID="Label7" runat="server" Text="Total Parts Qty" Width="81px"></asp:Label>
                     </td>
                     <td>
                        <asp:Label ID="lblPartCount" runat="server" Text="0" Width="60px"></asp:Label>
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


					    
					</cc1:TabPanel>
					<cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="Part Images" Visible="false">
						<ContentTemplate>
							<table width="100%" style="border-collapse:separate; border-spacing:5px; margin-bottom:0;" class="table table-condensed table-bordered table-hover" >
								<tr>
									<td>Picture 1 
                                        <asp:FileUpload ID="Pic1Upload" runat="server" />
									</td>
							        <td>
                                        <asp:Button ID="btnPic1Upload" runat="server" Text="Upload" OnClick="btnPic1Upload_Click" PostBackUrl="~/Modules/Service/Forms/Warranty.aspx"/>
								        
							        </td>
                                    <td>Picture 2
                                       
                                        <asp:FileUpload ID="Pic2Upload" runat="server" />
									</td>
							        <td>
                                         <asp:Button ID="btnPic2Upload" runat="server" Text="Upload" OnClick="btnPic2Upload_Click"  PostBackUrl="~/Modules/Service/Forms/Warranty.aspx"/>
								        
							        </td>
								</tr>							
								<tr>
                                    <td colspan="2">
                                        <asp:Image runat="server" ID="Image1" Width="150px" Height ="150px"/>
                                    </td>
                                     <td colspan="2">
                                        <asp:Image runat="server" ID="Image2" Width="150px" Height ="150px"/>
                                    </td>
								</tr>
							</table>
						</ContentTemplate>
					</cc1:TabPanel>
                    <cc1:TabPanel ID="TabImages" runat="server" HeaderText="Images">
                         <HeaderTemplate>Images</HeaderTemplate>
                         <ContentTemplate>
                         <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                         
						<tr>
                        
                        <td>
                         <asp:DataList ID="ImgDataList" RepeatDirection="Horizontal" runat="server" Width="1%">
                          <ItemTemplate>
                                                            
                                                            <asp:Image ID="Image1" runat="server" Width="100%" Height="80px" HeaderText="Image" ImageUrl='<%# "~/" + DataBinder.Eval(Container, "DataItem.ImageAddress").ToString()%>' />
                                                           
                                                        </ItemTemplate>
                                                        
                                                        <ItemStyle Width="5%" HorizontalAlign=" center"></ItemStyle>
                                                    

                         </asp:DataList>
                        </td>
						</tr>
                            
                      
                       
                             
					</table>
                             </ContentTemplate>
                     </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel4" runat="server" HeaderText="Warranty Labour">
						
						<HeaderTemplate>Warranty Labour</HeaderTemplate>
						
						<ContentTemplate>
                            <table  width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
										<tr>
									
									<td>Labor Description  <asp:ImageButton runat="server" ID="IMGLabour" ImageUrl="~/Images/LookupNew.png" OnClick="IMGLabour_Click" Width="16px" /></td>
		
									
<td>
										<asp:TextBox ID="txtWLaborCode" runat="server" CssClass="ROTextBoxes" MaxLength="50" Width="30%"></asp:TextBox>
									    
									
									
									
										
										<asp:TextBox ID="txtWLaborDesc" runat="server" CssClass="ROTextBoxes" MaxLength="50" Width="68%"></asp:TextBox>
									    
									</td>
									<td>Hr </td>
									
									<td>
										
										<asp:TextBox ID="txtLaborHr" runat="server" CssClass="ROTextBoxes"  Width="100%"></asp:TextBox>
									    
									</td>
									
									<td>Amount </td>
									
									<td>
										
										<asp:TextBox ID="txtWAmount" runat="server" CssClass="ROTextBoxes" Width="100%"></asp:TextBox>
									    
									</td>
								    	<td>TotalLabor </td>
									
									<td>
										
										<asp:TextBox ID="txtTotalLabor" runat="server" CssClass="ROTextBoxes" Width="100%"></asp:TextBox>
									    
									</td>
								</tr>
                                 <tr>
                                            <td colspan="2">
                                   <asp:ImageButton ID="BtnAdd" runat="server" Height="45px" 
                                       ImageUrl="~/Images/AddIMG.png"  Width="45px" onclick="BtnAdd_Click"  />
                                   <asp:ImageButton ID="BtnClear1" runat="server" Height="40px" 
                                       ImageUrl="~/Images/ClearImg.png" Width="40px" OnClick="BtnClear1_Click" />
                               </td>
                               <td align="right" colspan="4">
                                   &nbsp;</td>
                </tr>
							
							    
							</table>
							
							<asp:GridView ID="gvWLabor" runat="server" AutoGenerateColumns="False" Width="100%"
                OnRowDataBound="gvWLabor_RowDataBound" ShowHeaderWhenEmpty="True" ShowFooter="true">
								<HeaderStyle CssClass="GridHeader" />
                <FooterStyle CssClass="GridFooter" />
								 <Columns>
               
                    <asp:TemplateField HeaderText="Operational Code">
                        <ItemTemplate>
                            <asp:Label ID="LblLaborCode" runat="server" Text='<%# Bind("LaborCode") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGVWLaborCode" runat="server" Text='<%# Bind("LaborCode") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Operational Description">
                        <ItemTemplate>
                            <asp:Label ID="LblLaborDesc" runat="server" Text='<%# Bind("LaborDesc") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGVWLaborDesc" runat="server" Text='<%# Bind("LaborDesc") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Hr">
                        <ItemTemplate>
                            <asp:Label ID="LblHr" runat="server" Text='<%# Bind("Hr") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGVWLaborHr" runat="server" Text='<%# Bind("Hr") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
                            <asp:Label ID="LbPartDesc" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGVWLaborRate" runat="server" Text='<%# Bind("Rate") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Labor Cost ">
                        <ItemTemplate>
                            <asp:Label ID="LblTotalLabor" runat="server" Text='<%# Bind("TotalLabor") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGVWLaborCost" runat="server" Text='<%# Bind("TotalLabor") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="PST %">
                        <ItemTemplate>
                            <asp:Label ID="lblPSTPer" runat="server" Text='<%# Bind("PSTPer") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtPSTPer" runat="server" Text='<%# Bind("PSTPer") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="PST Amount ">
                        <ItemTemplate>
                            <asp:Label ID="LblPSTAmount" runat="server" Text='<%# Bind("PSTAmount") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtPSTAmount" runat="server" Text='<%# Bind("PSTAmount") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remove" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            
                            <asp:ImageButton ID="lnkRemove" OnClick="lnkRemove_Click1" runat="server" 
                                ImageUrl="~/Images/Delete_16x16.png"  
                                ToolTip="Remove" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                                <AlternatingRowStyle BackColor="#CCCCCC" />
								
								<HeaderStyle CssClass="GridHeader" />
								
								<FooterStyle CssClass="GridFooter" />
							    
							</asp:GridView>
								<table>
						    <tr >
                                <td align="right">
                                    Total
                                </td>
                                <td align="left">
                                    <asp:TextBox runat="server" ID="txtTotalLab" ReadOnly="true" ></asp:TextBox>
                                </td>

						    </tr>
							</table>
						    
							
						</ContentTemplate>
					    
					</cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel1" runat="server" Width="100%" Height="100%" HeaderText="P.P.R. Sublet">
						
						<HeaderTemplate>W.R. Sublet</HeaderTemplate>
						
						<ContentTemplate>
                            <table>
								
								<%--<tr>                                    
									<td width="1%">Vendor<asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label></td>									
									<td width="12%" nowrap="nowrap">
										
										<asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
										
										<cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" IsSorted="True" QueryPattern="Contains"
											TargetControlID="ddlVendor" Enabled="True"></cc1:ListSearchExtender>
									    
									</td>
								    
								</tr>--%>
							    
							</table>
							
							<asp:GridView ID="grd_Sublet" runat="server" AutoGenerateColumns="False" 
                                OnRowDataBound="grd_Sublet_RowDataBound"
                                OnRowCancelingEdit="grd_Sublet_RowCancelingEdit"
                                OnRowEditing="grd_Sublet_RowEditing"
                                OnRowUpdating="grd_Sublet_RowUpdating"
							 ShowHeaderWhenEmpty="True"	Width="100%" Height="100%">
								
								<Columns>
									<asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                        UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>
									<asp:TemplateField HeaderText="S No">
										
										<ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
										
										<ItemStyle Width="5%" HorizontalAlign="Center" />
									    
									</asp:TemplateField>
                                    <asp:BoundField DataField="JobCode" HeaderText="Job " />
									<asp:TemplateField HeaderText="Job Description">
                                         <ItemTemplate>
                                                            <asp:Label ID="lblJobDesc" runat="server" Text='<%# Bind("JobDesc") %>'></asp:Label>
                                                        </ItemTemplate>
									</asp:TemplateField>
									
									
									<asp:BoundField DataField="SubletDate" HeaderText="Sublet Date"/>
									
									<asp:BoundField DataField="SubletAmount" HeaderText="Invoice Amount"/>
                                    	<asp:BoundField DataField="PST" HeaderText="PST Rate"/>
                                    <asp:BoundField DataField="PSTAmount" HeaderText="PST Amount"/>
                                      <%-- <asp:TemplateField HeaderText="Part Source">
                                      <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEditType" runat="server" AutoPostBack="True" Font-Names="monospace" Width="95%">
                                                              <%--<asp:ListItem Value="0"> Select</asp:ListItem>--%>
                                                              <%--  <asp:ListItem Value="001">AHFM</asp:ListItem>
                                                                <asp:ListItem Value="002">Local Purchase</asp:ListItem>--
                                        </asp:DropDownList>
                                          <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" IsSorted="True" QueryPattern="Contains"
											TargetControlID="ddlEditType" Enabled="True"></cc1:ListSearchExtender>
									    
                                      </EditItemTemplate>
                                      <ItemTemplate>
                                      <asp:Label ID="lblPartSource" runat="server" Text='<%# Bind("PartSourceCode") '></asp:Label>
                                      </ItemTemplate>
                                      <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>--%>

									<%--<asp:BoundField DataField="Qty" HeaderText="Quantity">
										
										<ItemStyle HorizontalAlign="Right" /></asp:BoundField>
									
									<asp:BoundField DataField="Price" HeaderText="Price">
										
										<ItemStyle HorizontalAlign="Right" /></asp:BoundField>
									
									<asp:BoundField DataField="GrossAmount" HeaderText="Total">
										
										<ItemStyle HorizontalAlign="Right" /></asp:BoundField>--%>
								    
								</Columns><AlternatingRowStyle BackColor="#CCCCCC" />
								
								<HeaderStyle CssClass="GridHeader" />
								
								<FooterStyle CssClass="GridFooter" />
							    
							</asp:GridView>
								<table>
						    <tr >
                                <td align="right">
                                    Total
                                </td>
                                <td align="left">
                                    <asp:TextBox runat="server" ID="txtTotalSublet" ReadOnly="true" ></asp:TextBox>
                                </td>

						    </tr>
							</table>
						    
							
						</ContentTemplate>
					    
					</cc1:TabPanel>
				</cc1:TabContainer>
               <table width="100%" style="margin-bottom: 10px;margin-top:10px;">
					<tr>
						<td>
							&nbsp;
							<asp:Label ID="lblMsg" runat="server"></asp:Label>
						</td>						
						   <td style="text-align: right;">
							<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait';"
								UseSubmitBehavior="false" />
   <asp:Button ID="btnDelete" runat="server" Width="100px" Text="Delete" OnClientClick=" return confirm('Are you sure to delete?');"
                            OnClick="btnDelete_Click" />
							
							<asp:Button ID="btnClear" runat="server" Width="100px" Text="Clear" OnClick="btnClear_Click" />
							<asp:Button ID="btnPrint" runat="server" Width="100px" Text="Print" OnClick="btnPrint_Click" />
							<asp:Button ID="btnPost" runat="server" Width="100px" Text="Post" OnClick="btnPost_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait';"
								UseSubmitBehavior="false" />
						</td>
					</tr>	
										
				</table>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>

    </asp:Content>

