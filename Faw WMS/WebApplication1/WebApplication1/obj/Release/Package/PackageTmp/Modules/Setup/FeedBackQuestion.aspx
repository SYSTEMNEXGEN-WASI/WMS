<%@ Page Title="" theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="FeedBackQuestion.aspx.cs" Inherits="DXBMS.Modules.Setup.FeedBackQuestion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

	 <asp:UpdatePanel runat="server">
		<Triggers>
	<asp:PostBackTrigger ControlID="gv_FeedbackQuestions" />  
	</Triggers>
		<ContentTemplate>
			<div class="container">
				
					<div class="field">
						<fieldset>
							<legend>
								Service Feedback Card
							</legend>
					   
			<div class="row">
					<form>
						<div class="col-sm-12">                                                           
		   <%--Row1--%>     <div class="row">
								<div class="col-sm-12 form-group">  
									<div class="col-sm-1" style="white-space:nowrap;">
									FeedBack No.
									</div>
									<div class="col-sm-5">                    
									<asp:DropDownList ID="ddlfeedbackNo" class="WidthAt764" runat="server" AutoPostBack="True" Font-Names="monospace"
										Width="100%" OnSelectedIndexChanged="ddlFeedBackCode_SelectedIndexChanged">
									</asp:DropDownList>
									</div>                               
									<div class="col-sm-1" style="white-space:nowrap;">
									FeedBack Date 
									</div>
									<div class="col-sm-5">                   
										<asp:TextBox  ID="txtfeedbackDate" runat="server" style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;" ></asp:TextBox>
										<asp:CalendarExtender runat="server" TargetControlID="txtfeedbackDate" Format="dd-MM-yyyy"></asp:CalendarExtender>
										


									</div>
								</div>  
							</div>
		   <%--Row2--%>     <div class="row">                        
								<div class="col-sm-12 form-group">
									<div class="col-sm-1" style="white-space: nowrap;">
										Job Card Code:
									</div>
									<div class="col-sm-5">                        
										<asp:DropDownList ID="ddlJobCardCode" runat="server" AutoPostBack="true" Font-Names="monospace"
										Width="100%" OnSelectedIndexChanged="ddlJobCardCode_SelectedIndexChanged" class="WidthAt764">
									</asp:DropDownList>  <%----%>
									</div>
									<div class="col-sm-1" style="white-space:nowrap">
										Job Card Type: 
									</div>
									<div class="col-sm-5">                    
										<asp:TextBox ID="txtJobCardType" runat="server" Height="21px" MaxLength="50" width="100%"></asp:TextBox>

									</div>
								 </div>                        
							</div>
		   <%--Row3--%>     <div class="row">                
								<div class="col-sm-12 form-group">
									<div class="col-sm-1" style="white-space:nowrap;">
									Customer Name
									</div >
									<div class="col-sm-2" style="float:left;">                    
									<asp:TextBox ID="txtCusCode" runat="server" ReadOnly="true" Width="100%" ></asp:TextBox>
                                        </div>
                                    <div class="col-sm-3"> 
									<asp:TextBox ID="txtCustomerName" class="WidthAt764"  runat="server" ReadOnly="true" Width="100%" ></asp:TextBox>
									</div>
									<div class="col-sm-1" style="white-space:nowrap;">
										Product
									</div>
									<div class="col-sm-5">                    
										<asp:TextBox ID="txtProduct" runat="server" Height="21px" MaxLength="50" width="100%"></asp:TextBox>
									</div>
								  </div>
								</div>
		   <%--Row3--%>     <div class="row">
								<div class="col-sm-12 form-group">
									<div class="col-sm-1" style="white-space:nowrap;">
									Chassis No.                                    </div>
									<div class="col-sm-5">
										 <asp:TextBox ID="txtCustomerChassisNo" runat="server" Height="21px" MaxLength="50" width="100%"></asp:TextBox>
									</div>
									<div class="col-sm-1" style="white-space:nowrap;">
									Reg No. 
									</div>
									<div class="col-sm-5">                    
										<asp:TextBox ID="txtRegNo" runat="server" Height="21px" MaxLength="50" width="100%"></asp:TextBox>
									</div>
								</div>                
							</div>
		   <%--Row4--%>     <div class="row">                
								<div class="col-sm-12 form-group">
									<div class="col-sm-1" style="white-space:nowrap;">
										Service Type
									</div>
									<div class="col-sm-5">                          
										<asp:DropDownList ID="ddlServiceType" runat="server" Width="100%" AutoPostBack="true"
												OnSelectedIndexChanged="ddlServiceType_SelectedIndexChanged">
												<asp:ListItem value="0" >
													   Select
												   </asp:ListItem>
												<asp:ListItem Value="CRM">
													CRM
												</asp:ListItem>
												<asp:ListItem Value="Service">
													Service
												</asp:ListItem>
												<asp:ListItem Value="Compaign">
													Compaign
												</asp:ListItem>
												<asp:ListItem Value="Event">
													Event
												</asp:ListItem>
											</asp:DropDownList>
									</div>
									<div class="col-sm-6" style="white-space:nowrap;">
										  <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
									</div>
								</div>                
							</div>
							   </form>
			   

										<asp:GridView runat="server" ShowHeaderWhenEmpty="true" Width="100%" ID="gv_FeedbackQuestions" AutoGenerateColumns="false" 
                                            OnRowDataBound="gv_FeedbackQuestions_RowDataBound"
											OnRowCreated="gv_FeedbackQuestions_DataBound"  >
											
											
											<HeaderStyle CssClass="GridHeader" />
											<FooterStyle CssClass="GridFooter" />

											<Columns>
													

													<asp:TemplateField HeaderText="Sequence" ItemStyle-HorizontalAlign="left">
													<ItemTemplate>
														<asp:Label ID="lblSequence" runat="server" Text='<%# Bind("QSequence") %>'></asp:Label>
													</ItemTemplate>
													<EditItemTemplate>
														<asp:TextBox ID="txtSequenceEdit" maxlength="3" runat="server"></asp:TextBox>
													</EditItemTemplate>
													
													<ItemStyle HorizontalAlign="Center" />
													</asp:TemplateField>   

													 <asp:TemplateField HeaderText="Question Description" ItemStyle-HorizontalAlign="Left">
													<ItemTemplate>
														<asp:Label ID="lblParentQuestionDesc" runat="server" Text='<%# Bind("ParentQuestionDesc") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Center" />
													</asp:TemplateField>


													<asp:TemplateField HeaderText="ParentQuestionCode" ItemStyle-HorizontalAlign="Left" visible="false">
													<ItemTemplate>
														<asp:Label ID="lblParentQuestionCode" runat="server" Text='<%# Bind("ParentQuestionCode") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Center" />
													</asp:TemplateField>



													 <asp:templatefield HeaderText="Rating" ItemStyle-HorizontalAlign="Center">
														   <ItemTemplate>
																<asp:RadioButtonList id="rblYesOrNo" runat="server" AutoPostBack="false" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblYesOrNo_SelectedIndexChanged" >
																</asp:RadioButtonList>
														  </ItemTemplate>
													</asp:templatefield>
												
											</Columns>
										</asp:GridView>
								<div class="col-sm-12 auto-style4"  align="middle">
									<asp:Button ID="BtnSubmit" runat="server" Text="Save" OnClick="BtnSubmit_Click"  />
									<asp:Button ID="BtnClear" runat="server" Text="Clear" OnClick="BtnClear_Click" />
									<asp:Button ID="btnDeleteJC" runat="server" Text="Delete" OnClientClick="function(s, e) {
											return confirm('Are you sure you want to Delete?');
										}" OnClick="BtnClear_Delete" />
									<asp:Button ID="BtnPrint" runat="server" Text="Print" Visible="false" OnClick="BtnPrint_Click" />
									
								</div>

                            </div>                                    
						</fieldset>
						  
					</div>
				
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>


</asp:Content>
