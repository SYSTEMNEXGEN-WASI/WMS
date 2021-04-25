<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="CustomerVehicle.aspx.cs" Inherits="DXBMS.Modules.Service.Forms.CustomerVehicle"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
     <%--   <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>--%>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="container">
               
                    <div style="font-size: 15px;">
                       
                        <table width="100%">
                        
                      <tr>
                      <td align="left">
                           <span class="fa-arrow-left fa"></span>
                        <asp:LinkButton ID="lnkBack" ForeColor="Black" runat="server" PostBackUrl="~/Modules/Service/Forms/CV.aspx" Text="Back"></asp:LinkButton>
                          </td>
                          <td align="center">
                        <asp:Label ID="lblMsg" runat="server" Width="100%"></asp:Label>
					</td>
                         <td align="right">
                   <asp:Label ID="lblImp" runat="server" Text="(*) Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
                </td>
                    
                    </tr>
                        </table>
                    </div>
                    <div class="Heading">&nbsp; VEHICLE ENROLMENT</div>
                    <fieldset>
                        <table   style="border-collapse:separate;width:100%; border-spacing:5px; " class="table table-condensed table-bordered table-hover">
                            <tr>
                            <td >Customer<asp:ImageButton runat="server" ID="imgCustomerCode" CausesValidation="false" ImageUrl="~/Images/LookupNew.png"   OnClick="imgCustomerCode_Click" /><font color="red"><b>*</b></font></td>
                            <td colspan="2"><asp:TextBox runat="server" ID="txtCusCode" MaxLength="8" ReadOnly="true" Width="28%" SkinID="ROTextBox"></asp:TextBox>
                            <asp:TextBox ID="txtCusDesc" style="text-transform: uppercase" runat="server" Width="70%" MaxLength="50" AutoPostBack="true" ReadOnly="true" SkinID="ROTextBox"></asp:TextBox>
                             <asp:DropDownList ID="ddlCustomer"  runat="server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomer_OnSelectedIndexChanged"></asp:DropDownList>
                           <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtCusDesc" runat="server" ErrorMessage="Customer Mandatory" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>--%>
                            </td>

                            
                            <td >Account of <asp:ImageButton runat="server" ID="ImgAccoff" CausesValidation="false" ImageUrl="~/Images/LookupNew.png" OnClick="ImgAccoff_Click" /><font color="red"><b>*</b></font></td>
                             <td colspan="2" ><asp:TextBox runat="server" ID="txtAccCode" ReadOnly="true" Width="28%" MaxLength="8" SkinID="ROTextBox"></asp:TextBox>
                                 <asp:TextBox ID="txtAccDesc" style="text-transform: uppercase" runat="server" Width="70%" MaxLength="50" AutoPostBack="true" ReadOnly="true" SkinID="ROTextBox"></asp:TextBox>
                                   <asp:DropDownList ID="ddlAcctof" runat="server" Visible="false" Enabled="false"></asp:DropDownList>
                                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtAccCode" runat="server" ErrorMessage="Account Customer Mandatory" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>--%>
                            </td>
                            </tr>
                          
                            <tr>
                                <td >Brand</td>
                                <td ><asp:DropDownList ID="ddlBrand" runat="server" Width="100%" AutoPostBack="true" Enabled="false"
                                     OnSelectedIndexChanged="ddlBrand_OnSelectedIndexChanged"></asp:DropDownList>                                    
                                 <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="ddlBrand" InitialValue="none" runat="server" ErrorMessage="Brand  Mandatory" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>--%>
                                </td>
                                <td >Product<%--<asp:ImageButton runat="server" ID="imgProduct" CausesValidation="false" ImageUrl="~/Images/LookupNew.png" OnClick="imgProduct_Click" /><font color="red"><b>*</b></font>--%></td>
                                <td >
                                    <asp:DropDownList ID="ddlProduct" runat="server" Enabled="false" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlProduct_OnSelectedIndexChanged" CausesValidation="false" >
                                    </asp:DropDownList>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="ddlProduct" InitialValue="none" runat="server" ErrorMessage="Product  Mandatory" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>--%>
                                 </td>
                                <td >Model</td>
                                <td>
                                 
                                    <asp:DropDownList ID="ddlVersion" runat="server" Enabled="false" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlVersion_SelectedIndexChanged" CausesValidation="false" >
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                            <td>Vehicle</td>
                            <td>
                            <asp:TextBox runat="server" ID="txtProdDesc" ReadOnly="true" TextMode="MultiLine" Width="100%" SkinID="ROTextBox"></asp:TextBox>
                            </td>
                             <td>Color<font color="red"><b>*</b></font></td>
                                <td>
                                    <%--<asp:TextBox ID="txtColor" runat="server" Width="100%"></asp:TextBox>--%>
                                    <asp:DropDownList ID="ddlColor" runat="server" Width="100%" Enabled="false">
                                    </asp:DropDownList>                                 
                                </td>
                                <td>Reg. No<font color="red"><b>*</b></font></td>
                                <td>
                                    <asp:TextBox ID="txtRegNo" runat="server" AutoPostBack="true" OnTextChanged="txtRegNo_TextChanged" MaxLength="15" Enabled="false" Width="100%"></asp:TextBox>
                              <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtRegNo" runat="server" ErrorMessage="Reg No Mandatory" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>--%>
                                </td>
                                
                                                         
                                
                            </tr>
                            <tr>
                             <td >Reg. Date</td>
                                <td>                                
                                <asp:TextBox ID="txtRegDate" Enabled="false" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png);
                                            background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox> 
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtRegDate"
                                            Format="dd-MM-yyyy"></ajaxToolkit:CalendarExtender>                                                                                                       
                                </td>
                                 <td>Engine No<font color="red"><b>*</b></font></td>
                                <td>
                                    <asp:TextBox ID="txtEngineNo" runat="server" AutoPostBack="true" Enabled="false" MaxLength="30" Width="100%" OnTextChanged="txtEngineNo_TextChanged" CausesValidation="false"></asp:TextBox>
                              <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtEngineNo" runat="server" ErrorMessage="Engine No Mandatory" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>--%>
                                </td>
                                <td>Chassis No<font color="red"><b>*</b></font></td>
                                <td>
                                    <asp:TextBox ID="txtChassisNo" Enabled="false" AutoPostBack="true" runat="server" Width="100%" MaxLength="30" OnTextChanged="txtChassisNo_TextChanged" CausesValidation="false"></asp:TextBox>
                               <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txtChassisNo" runat="server" ErrorMessage="Chassis No Mandatory" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>--%>
                                </td> 
                               
                                                    
                                
                                
                            </tr>                            
                            <tr>
                             <td>Invoice No</td>
                                <td>
                                    <asp:TextBox ID="txtInvoiceNo" runat="server" Enabled="false" Width="100%" MaxLength="25"></asp:TextBox>
                                     <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                       Enabled="true" FilterType="Numbers" TargetControlID="txtInvoiceNo" />
                                </td>
                                <td>Invoice Date</td>
                                <td><asp:TextBox ID="txtInvDate"  runat="server" Enabled="false"  Style="background-image: url(../../../Images/Calendar_16x16.png);
                                           background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox> 
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtInvDate"
                                            Format="dd-MM-yyyy"></ajaxToolkit:CalendarExtender>                                                                                                       
                                </td>        
                                <td>Policy NO.</td>
                                <td>
                                    <asp:TextBox ID="txtPolicyNo" Enabled="false" runat="server" Width="100%" MaxLength="25"></asp:TextBox>
                                     <%--<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                       Enabled="true" FilterType="Custom" TargetControlID="txtPolicyNo" />--%>
                                </td>
                                                            
                            </tr>
                            <tr>
                             <td>Warranty Book No</td>
                                <td>
                                    <asp:TextBox ID="txtWBNo" Width="100%" Enabled="false" runat="server" MaxLength="25" style="text-transform: uppercase"></asp:TextBox>
                                        
                               
                                </td>
                                <td>Insurance Company</td>
                                <td>
                                    <asp:DropDownList ID="ddlInsCo" Enabled="false"  runat="server" Width="100%">
                                    </asp:DropDownList>                                    
                                </td>  
                                
                                <td>Delivery Date</td>
                                <td>
                                    <asp:TextBox ID="txtDeliveryDate"  runat="server" Enabled="false"  Style="background-image: url(../../../Images/Calendar_16x16.png);
                                           background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox> 
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtDeliveryDate"
                                            Format="dd-MM-yyyy"></ajaxToolkit:CalendarExtender> 
                                            <asp:TextBox ID="txtCusVehCode" Visible="false" runat="server" ReadOnly="true" Width="100%"></asp:TextBox>  
                                </td>
                               
                                <tr>
                                <td colspan="6">
                                <div style="width: 100%; height: 200px; overflow: scroll">

                                    <asp:GridView ID="gvCustVeh" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" Width="100%"
                                        CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                        <Columns>
                                            <asp:BoundField DataField="CusDesc" HeaderText="Customer">
                                             
                                               
                                            </asp:BoundField>
                                            
                                            <asp:BoundField DataField="tdDate" HeaderText="Reg Date" DataFormatString="{0:dd-M-yyyy}">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%--<asp:BoundField DataField="RegNo" HeaderText="Reg No">
                                                <HeaderStyle HorizontalAlign="Left" />
                                               
                                            </asp:BoundField>--%>
                                            <asp:TemplateField HeaderText="Reg No.">
                                                    <ItemTemplate>
                                                       <asp:LinkButton ID="lnkRegNo" Text='<%# Bind("RegNo") %>' runat="server" OnClick="lnkSelect_Click1"></asp:LinkButton></ItemTemplate> <%----%>
                                                    <ItemStyle Width="5%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ChassisNo" HeaderText="Chassis No">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                  <ItemStyle  CssClass="text-uppercase"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="EngineNo" HeaderText="Engine No">
                                                <ItemStyle  CssClass="text-uppercase"/> 
                                            </asp:BoundField>
                                            <asp:BoundField DataField="BrandDesc" HeaderText="Brand">
                                                <ItemStyle  CssClass="text-uppercase"/> 
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ProdCode" HeaderText="Product">
                                                 <ItemStyle  CssClass="text-uppercase"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ColorDesc" HeaderText="Color" >
                                                <ItemStyle  CssClass="text-uppercase"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No">
                                                 <ItemStyle  CssClass="text-uppercase"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:dd-M-yyyy}">
                                                <ItemStyle  CssClass="text-uppercase"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="WorkBookNo" HeaderText="Warranty Book No">
                                              <ItemStyle  CssClass="text-uppercase"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AccountOff" HeaderText="Account OFF" >
                                              <ItemStyle  CssClass="text-uppercase"/>
                                            </asp:BoundField>
                                              <asp:BoundField DataField="InsPolicyNo" HeaderText="Policy No" >
                                              <ItemStyle  CssClass="text-uppercase"/>
                                            </asp:BoundField>
                                           
                                        </Columns>
                                        <AlternatingRowStyle BackColor="#CCCCCC" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <FooterStyle CssClass="GridFooter" />
                                    </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            </tr>
                        </table>
                    </fieldset>
                    
                    <table width="100%">
                      
                        <tr>
                    <td align="center">
                        
                        <asp:Button ID="btnSaveInv" runat="server" Width="100px" Text="Save" OnClick="btnSave_Click" OnClientClick="this.disabled = true; this.value = 'Please Wait';"
                            UseSubmitBehavior="false" />  
                        <asp:Button ID="btnDeleteInv" Width="100px" runat="server" Text="Delete" OnClick="btnDelete_Click"                                                     
                            OnClientClick="function(s, e) {
	                                                        var yn = confirm(&quot;Are you sure to delete this Invoice?&quot;);
                                                            if(yn==true)
	                                                        {
		                                                        //alert(&quot;you press ok&quot;);
		                                                        //this.DeletePO();
		                                                        e.processOnServer = true;
	                                                        }
	                                                        else
	                                                        {
		                                                         e.processOnServer = false;
	                                                        }
                                                        }" />
                        <asp:Button ID="btnClear" Width="100px" runat="server" Text="Clear" OnClick="btnClear_Click" />
                    </td>
                </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
