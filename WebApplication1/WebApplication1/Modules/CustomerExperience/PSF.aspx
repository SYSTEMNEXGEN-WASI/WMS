<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" Theme="Theme1" CodeBehind="PSF.aspx.cs" Inherits="DXBMS.Modules.CustomerExperience.PSF" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">

	<script src="../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
	<script src="../../js/bootstrap.min.js" type="text/javascript"></script>
	<script src="../../js/chosen.jquery.js" type="text/javascript"></script>
	<script src="../../js/chosen.jquery.min.js" type="text/javascript"></script>
   
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			 <div class="container" >
				
			<div class="Heading">
				 Post Service Follow Up
			</div>           
			<table width="100%">
				<tr>
					<td width="100%">
						<asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
					</td>    
				</tr>
				 <tr>
					 <td>
                    
                     <asp:Calendar ID="Calendar1" runat="server" Width="100%" OnSelectionChanged="Calendar1_SelectionChanged" Format="dd-MM-yyyy"></asp:Calendar>
                   
						 
					 </td>				
				</tr>
			</table>
				  
						<table width="100%">
							<tr>
								<td width="100%">
							<asp:GridView ID="grdPostServiceFollowup" runat="server" Width="100%" ShowFooter="True" OnPageIndexChanging="grdPostServiceFollowup_PageIndexChanging"
								ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" OnRowDataBound="gv_PSF_RowDataBound" OnRowEditing="gv_PSF_RowEditing" OnRowCancelingEdit="gv_PSF_RowCancelingEdit"
								 OnRowUpdating="gv_PSF_RowUpdating" OnDataBound="gv_PSF_DataBound">		
								
							   <Columns>
								   <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image" 
									UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>
							   </Columns>						
								
					<HeaderStyle CssClass="GridHeader" />
					<FooterStyle CssClass="GridFooter" />

								<Columns>
									<asp:TemplateField HeaderText="SNo">
										 <ItemTemplate>
											 <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex + 1 %>'></asp:Label>
										 </ItemTemplate>
									</asp:TemplateField>

									<asp:TemplateField HeaderText="Invoice No">
										<ItemTemplate>
											<asp:Label ID="lblJobCode" runat="server" Text='<%# Bind("JobCardCode") %>'></asp:Label>
										</ItemTemplate>
									</asp:TemplateField>

									<asp:TemplateField HeaderText="End User">
										<ItemTemplate>
											<asp:Label ID="lblEndUser" runat="server" Text='<%# Bind("EndUserDesc") %>' ></asp:Label>
										</ItemTemplate>                                                                              
								   </asp:TemplateField>

									<asp:TemplateField HeaderText="Reg No.">
										<ItemTemplate>
											<asp:Label ID="lblRegNo" runat="server" Text='<%# Bind("RegNo") %>' ></asp:Label>
										</ItemTemplate>                                        
									</asp:TemplateField>

									<asp:TemplateField HeaderText="Schedule">
										<ItemTemplate>
											<asp:Label ID="Label1" runat="server" Text='<%# Bind("Schedule") %>' ></asp:Label>
										</ItemTemplate>                                        
									</asp:TemplateField>

									<asp:TemplateField HeaderText="KM">
										<ItemTemplate>
											<asp:Label ID="Label3" runat="server" Text='<%# Bind("KM") %>' ></asp:Label>
										</ItemTemplate>                                        
									</asp:TemplateField>

									<asp:TemplateField HeaderText="Delivery Date">
										<ItemTemplate>
											<asp:Label ID="Label4" runat="server" MaxLength="10" Text='<%# Bind("DelvDate") %>'></asp:Label>
										</ItemTemplate>                                        
									</asp:TemplateField>

									<asp:TemplateField HeaderText="Customer Cell #">
										<ItemTemplate>
											<asp:Label ID="Label5" runat="server" Text='<%# Bind("EndUserCellNo") %>' ></asp:Label>
										</ItemTemplate>                                        
									</asp:TemplateField>

									<asp:TemplateField HeaderText="FollowUp Status">
										<ItemTemplate>
											<asp:Label ID="lblSatisfied" runat="server" Text='<%# Bind("Satisfied") %>' ></asp:Label>
										</ItemTemplate>
										<EditItemTemplate>
											<asp:DropDownList ID="ddl_Satisfied" runat="server" ></asp:DropDownList>
										</EditItemTemplate>                                        
									</asp:TemplateField>

									<asp:TemplateField HeaderText="Complain">
										<ItemTemplate>
											<asp:Label ID="lblComplain" runat="server" Text='<%# Bind("Complain") %>' ></asp:Label>
										</ItemTemplate>
										<EditItemTemplate>
											<asp:TextBox ID="txtComplain" runat="server" MaxLength="50"></asp:TextBox>
										</EditItemTemplate>                                          
									</asp:TemplateField>

									<asp:TemplateField HeaderText="Remarks">
										<ItemTemplate>
											<asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks") %>' ></asp:Label>
										</ItemTemplate>    
										<EditItemTemplate>
											<asp:TextBox ID="txtRemarks" runat="server" MaxLength="50" placeholder="Remarks"></asp:TextBox>
										</EditItemTemplate>                                    
									</asp:TemplateField>

									<asp:TemplateField HeaderText="FollowUp Date">
										<ItemTemplate>
											<asp:Label ID="Label10" runat="server" MaxLength="10" Text='<%# Bind("FollowUpDate") %>' ></asp:Label>
										</ItemTemplate>										                                        
									</asp:TemplateField>
																		
								</Columns>

								<%--<PagerStyle HorizontalAlign="Left" ForeColor="#4A3C8C"></PagerStyle>
								<SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7"></SelectedRowStyle>--%>

							</asp:GridView>
								</td>
							</tr>
						</table>						
						<table width="100%" style="border-collapse:separate; border-spacing:5px;" class="tabe table-condensed table-hover table-bordered">
							<tbody>
								<tr>
									<td align="left" style="width: 9%">
										<asp:Label ID="Label2" runat="server" Width="117px" Font-Names="Verdana" Font-Size="X-Small"
											ForeColor="Blue" Text="Total Customers :"></asp:Label>
									</td>
									<td style="width: 62px; height: 19px">
										<asp:Label ID="lbltotalCustomer" runat="server" Width="140px" Font-Names="Verdana"
											Font-Size="X-Small" ForeColor="Blue" Font-Bold="True"></asp:Label>
									</td>
									<td style="width: 50px; height: 19px">
										<asp:Label Style="text-align: right" ID="Label17" runat="server" Width="156px" Font-Names="Verdana"
											Font-Size="X-Small" ForeColor="#400000" Font-Bold="True" Text="Last Follow up :"></asp:Label>
									</td>
									<td>
										<asp:Label ID="lblLastFollowup" runat="server" Width="104px" Font-Names="Verdana"
											Font-Size="10px" Height="15px" ForeColor="#C04000" Font-Bold="True" Font-Underline="True"></asp:Label>
									</td>
								</tr>
								<tr>
									<td align="left">
										<asp:Label ID="Label9" runat="server" Width="111px" Font-Names="Verdana" Font-Size="X-Small"
											ForeColor="Green" Text="Follow up required :"></asp:Label>
									</td>
									<td style="width: 62px; height: 16px">
										<asp:Label ID="lblFollowupRequired" runat="server" Width="140px" Font-Names="Verdana"
											Font-Size="X-Small" ForeColor="Green" Font-Bold="True"></asp:Label>
									</td>
									<td>
										<asp:Label ID="lblRegNo" runat="server" Width="104px" Font-Names="Verdana" Font-Size="10px"
											Height="15px" ForeColor="#C04000" Font-Bold="True" Font-Underline="True"></asp:Label>
									</td>
								</tr>
								<tr>
									<td align="left">
										<asp:Label ID="Label11" runat="server" Width="128px" Font-Names="Verdana" Font-Size="X-Small"
											Text="Follow up completed :"></asp:Label>
									</td>
									<td style="width: 62px; height: 12px">
										<asp:Label ID="lblFollowupCompleted" runat="server" Width="140px" Font-Names="Verdana"
											Font-Size="X-Small" ForeColor="Black" Font-Bold="True"></asp:Label>
									</td>
								</tr>
							</tbody>
						</table>			
				
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
   
  
 
</asp:Content>

