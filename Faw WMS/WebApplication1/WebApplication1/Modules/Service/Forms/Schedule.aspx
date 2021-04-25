<%@ Page Title="" Language="C#" Theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="Schedule.aspx.cs" Inherits="DXBMS.Modules.Service.Schedule" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
 
    <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
    <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
    <link href="../../../css/TabStyle.css" rel="stylesheet" />
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
               window.location.href = "Schedule.aspx";
           }
           function Updatealert() {
               Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been update',
                   showConfirmButton: false,
                   timer: 6000
               })
               window.location.href = "Schedule.aspx";
           }
           function Deletealert() {
            Swal.fire({
                   position: 'Centered',
                   icon: 'success',
                   title: 'Your record has been delete',
                   showConfirmButton: false,
                   timer: 6000
                    })
            window.location.href = "Schedule.aspx";
           }
           function reloads() {
               window.location.href = "Schedule.aspx";
           }

       </script>


    <style>
        .chosen-drop {
    width: 100% !important;
}

        div#ctl00_Content_TabContainer1_TabPanel3_ddllLubs_chosen {
    width: 85% !important;
}
        div#ctl00_Content_TabContainer1_TabPanel1_ddlJobs_chosen {
    width: 100% !important;
}
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            var selectedTab = $("#<%=hfTab.ClientID%>");
            var tabId = selectedTab.val() != "" ? selectedTab.val() : "tab1";
            $('#dvTab a[href="#' + tabId + '"]').tab('show');
            $("#dvTab a").click(function () {
                selectedTab.val($(this).attr("href").substring(1));
                $(".nav-tabs li").removeClass("active")

            });
        });
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">

        <Triggers>
            <%--<asp:AsyncPostBackTrigger EventName="ddllLubs_SelectedIndexChanged()" ControlID="ddllLubs" />--%>
           <%-- <asp:PostBackTrigger ControlID="imgLookup" />--%>
        </Triggers>
        <ContentTemplate>
            <div class="container">
                <div class="Heading">Schedule Master</div>
                <fieldset>
                    <legend>Schedule</legend>
                    <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                        <tr>
                            <td style="width: 12%" nowrap="nowrap">
                                <asp:Label ID="Label15" runat="server" Text="Schedule"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSchedule" runat="server" Font-Names="monospace" AutoPostBack="True" OnSelectedIndexChanged="ddlEstCode_SelectedIndexChanged"
                                    Width="330px">
                                </asp:DropDownList>
                                <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlSchedule"
                                    PromptText="" QueryPattern="Contains">
                                </ajaxToolkit:ListSearchExtender>
                            </td>
                            <td width="12%">Schedule Code</td>
                            <td width="40%">
                                <asp:TextBox ID="txtSchedule" runat="server" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Brand"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBrand" runat="server" Font-Names="monospace" Width="330px" OnSelectedIndexChanged="ddlRegNo_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                                <%----%>
                                <ajaxToolkit:ListSearchExtender ID="LstEx" runat="server" TargetControlID="ddlBrand" PromptText="" QueryPattern="Contains">
                                </ajaxToolkit:ListSearchExtender>
                            </td>
                            <td>Product</td>
                            <td>
                                <asp:DropDownList ID="ddlVehicleCode" runat="server" Width="147px" AutoPostBack="true" OnSelectedIndexChanged="ddlVehicleCode_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                          <td >Version</td>
                                <td>
                                 
                                    <asp:DropDownList ID="ddlVersion" runat="server" Enabled="false" Width="100%" AutoPostBack="true"  CausesValidation="false" >
                                    </asp:DropDownList>
                                </td>
                            <td>Schedule KM</td>
                            <td>
                                <asp:TextBox ID="txtScheduleKm" runat="server" MaxLength="10"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" BehaviorID="_content_FilteredTextBoxExtender1"
                                    FilterType="Numbers" TargetControlID="txtScheduleKm"></ajaxToolkit:FilteredTextBoxExtender>
                            </td>
                          
                        </tr>
                        <tr>
                          <td>Next Visit</td>
                            <td>
                                <asp:TextBox ID="txtNextVisit" runat="server" MaxLength="3"></asp:TextBox>Days
                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" BehaviorID="_content_FilteredTextBoxExtender2"
                                        FilterType="Numbers" TargetControlID="txtNextVisit"></ajaxToolkit:FilteredTextBoxExtender>
                            </td>
                            <td>Reminder Leter</td>
                            <td>
                                <asp:TextBox ID="txtReminderLeter" runat="server" MaxLength="3"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" BehaviorID="_content_FilteredTextBoxExtender3"
                                    FilterType="Numbers" TargetControlID="txtReminderLeter"></ajaxToolkit:FilteredTextBoxExtender>
                                <asp:Label runat="server" Text="Days"></asp:Label>
                            </td>
                           
                        </tr>
                        <tr>
                         <td>Call Alert</td>
                            <td>
                                <asp:TextBox ID="txtCallAlert" runat="server" MaxLength="3"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" BehaviorID="_content_FilteredTextBoxExtender4"
                                    FilterType="Numbers" TargetControlID="txtCallAlert"></ajaxToolkit:FilteredTextBoxExtender>
                                <asp:Label ID="Label2" runat="server" Text="Days"></asp:Label>
                            </td>
                            <td>SMS Alert</td>
                            <td>
                                <asp:TextBox ID="txtSms" runat="server" MaxLength="3"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" BehaviorID="_content_FilteredTextBoxExtender5"
                                    FilterType="Numbers" TargetControlID="txtSms"></ajaxToolkit:FilteredTextBoxExtender>
                                <asp:Label ID="Label3" runat="server" Text="Days"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>Jobs / Parts / Lubricants</legend>
                    <div class="row">
                        <div class="col-lg-12">
                           <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" CssClass="TabStyle" 
                                    Width="100%" Style="direction: ltr; text-align: left; display: grid;" ActiveTabIndex="1">
                                <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Sele Person Information  ">
                                    <HeaderTemplate>Jobs</HeaderTemplate>
                                        <ContentTemplate >
                                        <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                                            <tr>
                                                <td>Jobs</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlJobs" runat="server" Style="width: 300px !important;"></asp:DropDownList>
                                                </td>
                                                <td>Technician</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlTechnicianEmpCode" runat="server" Width="147px"></asp:DropDownList>
                                                </td>
                                                <td>Labor </td>
                                                <td>
                                                    <asp:TextBox ID="txtLabor" runat="server" MaxLength="6" Style="text-align: right" TabIndex="26" Width="100%"></asp:TextBox>
                                                    <ajaxToolkit:FilteredTextBoxExtender ID="txtLabor_FilteredTextBoxExtender" runat="server" BehaviorID="_content_txtLabor_FilteredTextBoxExtender" FilterType="Numbers"
                                                        TargetControlID="txtLabor"></ajaxToolkit:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:ImageButton ID="BtnAdd" runat="server" Height="45px" ImageUrl="~/Images/AddIMG.png" Width="45px" OnClick="BtnAdd_Click1"></asp:ImageButton>
                                                    <%----%>
                                                    <asp:ImageButton ID="BtnClear1" runat="server" Height="40px" ImageUrl="~/Images/ClearImg.png" Width="40px" OnClick="BtnClear1_Click1"></asp:ImageButton>
                                                    <%----%>
                                                </td>
                                                <td align="right">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="gvJobCard" runat="server" AutoGenerateColumns="False" ShowFooter="True" ShowHeaderWhenEmpty="True"
                                                        Width="100%" OnRowDataBound="gvJobCard_RowDataBound">
                                                        <%----%>
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                            <asp:BoundField DataField="JobCode" HeaderText="Job">
                                                                <ItemStyle Width="60px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DefJobDesc" HeaderText="Job Desc">
                                                                <ItemStyle Width="60%" Wrap="True" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="EmpCode" HeaderText="TechCode">
                                                                <ItemStyle Width="60px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="EmpName" HeaderText="Technician">
                                                                <ItemStyle Width="40%" Wrap="True" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Amount">
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblFooterAmount" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRowAmount" runat="server" Style="text-align: right" Text='<%# Bind("Labor") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="60px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remove">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="BtnRemove" runat="server" ImageUrl="~/Images/Delete_16x16.png" ToolTip="Remove" OnClick="BtnRemove_Click" />
                                                                    <%----%>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="60px" />
                                                                <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <FooterStyle CssClass="GridFooter" />
                                                        <HeaderStyle CssClass="GridHeader" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>                                    
                                        </ContentTemplate>
                                </ajaxToolkit:TabPanel>
                               <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Corporate / Institution  ">
                                    <HeaderTemplate>Parts</HeaderTemplate>
                                    <ContentTemplate>   
                                        <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                                            <tr>
                                                <td nowrap="nowrap">Item Code <font color="red"><b>*</b></font>
                                                    <asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png" OnClick="btnLookup_Click" />
                                                </td>
                                                 <td>
                                                    <asp:TextBox ID="txtItemcodeParts" runat="server" ReadOnly="true" Width="100%"></asp:TextBox>
                                                    <%--<asp:TextBox ID="txtPartItemNo_Parts" runat="server" Width="100%"></asp:TextBox>--%>

                                                </td>
                                                <td>Item No</td>
                                                <td>
                                                    <asp:TextBox ID="txtPartCode" runat="server" ReadOnly="true" Width="100%"></asp:TextBox>
                                                    <%--<asp:TextBox ID="txtItemCode" runat="server" Width="100%"></asp:TextBox>--%>
                                                </td>
                                                <td>Item Description</td>
                                                <td>
                                                    <asp:TextBox ID="txtPartDesc" runat="server" ReadOnly="true" Width="100%"></asp:TextBox>
                                                    
                                                </td>

                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap">Quantity<font color="red"><b>*</b></font></td>
                                                <td>
                                                    <asp:TextBox ID="txtQuantity" runat="server" MaxLength="3" Style="text-align: right" ValidationGroup="JCParts" Width="100%"></asp:TextBox>
                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" BehaviorID="_content_FilteredTextBoxExtender7" FilterType="Numbers"
                                                        TargetControlID="txtQuantity"></ajaxToolkit:FilteredTextBoxExtender>
                                                </td>
                                                <td nowrap="nowrap">Price </td>
                                                <td>
                                                    <asp:TextBox ID="txtPartPrice" runat="server" MaxLength="10" ReadOnly="True" SkinID="ROTextBox" Style="text-align: right"
                                                        ValidationGroup="JCParts" Width="100%">0</asp:TextBox>
                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" BehaviorID="_content_FilteredTextBoxExtender8" FilterType="Numbers"
                                                        TargetControlID="txtPartPrice"></ajaxToolkit:FilteredTextBoxExtender>
                                                </td>
                                                <td colspan="2">                                                   
                                                    <asp:TextBox ID="txtPartsRecQuantity" runat="server" Enabled="False" SkinID="ROTextBox" Style="text-align: right" Visible="False" Width="60px">0</asp:TextBox>
                                                </td>
                                            </tr>                                            
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:ImageButton ID="BtnAdd0" runat="server" Height="45px" ImageUrl="~/Images/AddIMG.png" Width="45px" OnClick="BtnAdd0_Click" />
                                                    <asp:ImageButton ID="BtnClear2" runat="server" Height="40px" ImageUrl="~/Images/ClearImg.png" Width="40px" OnClick="BtnClear2_Click" />
                                                </td>
                                                <td align="right" nowrap="nowrap">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="gvJobCardParts" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" Width="100%"
                                                        ShowFooter="True" OnRowDataBound="gvJobCardParts_RowDataBound">
                                                        <HeaderStyle CssClass="GridHeader" />
                                                        <FooterStyle CssClass="GridFooter" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                            <asp:BoundField DataField="ItemCode" HeaderText="Item Code" />
                                                            <asp:BoundField DataField="PartNo" HeaderText="Part No.">
                                                                <ItemStyle Width="150px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PartsDesc" HeaderText="Part Description" />
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
                                                                    <asp:Label ID="lblRowPartAmount" runat="server" Text='<%# Bind("Total") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblFooterPartAmount" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remove">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="BtnRemove" runat="server" ImageUrl="~/Images/Delete_16x16.png" ToolTip="Remove" OnClick="BtnRemove_Click1" />
                                                                    <%----%>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                               </ajaxToolkit:TabPanel>
                               <ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Customer Tag  ">
                                    <HeaderTemplate>Lubricants</HeaderTemplate>
                                    <ContentTemplate>                                    
                                        <table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                                            <tr>
                                                <td>Part Code <font color="red"><b>*</b></font> </td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="ddllLubs" runat="server" Style="width: 500px !important;" Font-Names="monospace"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddllLubs_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <%----%>
                                                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddllLubs" PromptText="" QueryPattern="Contains"
                                                        BehaviorID="_content_ListSearchExtender3">
                                                    </ajaxToolkit:ListSearchExtender>
                                                    <asp:TextBox ID="txtLubPartCode" runat="server" Visible="False" Width="100%"></asp:TextBox>
                                                    <asp:TextBox ID="txtLubPartDesc" runat="server" SkinID="ROTextBox" Visible="False" Width="250px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Quantity <font color="red"><b>*</b></font></td>
                                                <td width="27%">
                                                    <asp:TextBox ID="txtLubQuantity" runat="server" MaxLength="3" Style="text-align: right" Width="100%"></asp:TextBox>
                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Numbers"
                                                        TargetControlID="txtLubQuantity" BehaviorID="_content_FilteredTextBoxExtender5"></ajaxToolkit:FilteredTextBoxExtender>
                                                </td>
                                                <td>Price<font color="red"><b>*</b></font> </td>
                                                <td>
                                                    <asp:TextBox ID="txtLubPrice" runat="server" MaxLength="6" ReadOnly="True" SkinID="ROTextBox" Style="text-align: right" Width="100%"></asp:TextBox>
                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" BehaviorID="_content_FilteredTextBoxExtender6"
                                                        FilterType="Numbers" TargetControlID="txtLubPrice"></ajaxToolkit:FilteredTextBoxExtender>
                                                    <asp:TextBox ID="txtItemcodeLub" runat="server" Visible="False" Width="60px"></asp:TextBox>
                                                    <asp:TextBox ID="txtLubRecQuantity" runat="server" Enabled="False" SkinID="ROTextBox" Visible="False" Width="60px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" Height="45px" ImageUrl="~/Images/AddIMG.png" Width="45px" OnClick="ImageButton1_Click"></asp:ImageButton>
                                                    <%----%>
                                                    <asp:ImageButton ID="ImageButton2" runat="server" Height="40px" ImageUrl="~/Images/ClearImg.png" Width="40px" OnClick="ImgBtnLubClear_Click"></asp:ImageButton>
                                                </td>
                                                <td align="right">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="gvLubParts" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="100%"
                                                        OnRowDataBound="gvLubParts_RowDataBound" ShowHeaderWhenEmpty="True">
                                                        <%----%>
                                                        <Columns>
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
                                                            <asp:TemplateField HeaderText="Remove">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="BtnRemove" runat="server" ImageUrl="~/Images/Delete_16x16.png"
                                                                        ToolTip="Remove" OnClick="BtnRemove_Click2" />
                                                                    <%----%>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle CssClass="GridFooter" />
                                                        <HeaderStyle CssClass="GridHeader" />
                                                        <PagerStyle CssClass="GridFooter" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                               </ajaxToolkit:TabPanel>
                            </ajaxToolkit:TabContainer>                                
                                <asp:HiddenField ID="hfTab" runat="server" />                            
                        </div>
                    </div>
                </fieldset>
              <table width="100%" style="margin-bottom: 10px;margin-top:10px;">
                    <tr>
                     <td>
                   <asp:Label ID="lblImp" runat="server" Text="(*) Sterik Feilds Are Mandatory" ForeColor="Green" Font-Bold="true" ></asp:Label>
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
                            <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click1" Enabled="false"
                                OnClientClick="javascript: return confirm('Are you sure you want to Delete?');" />
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="TabName" runat="server" />

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
