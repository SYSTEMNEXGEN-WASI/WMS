<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true"
    Theme="Theme1" CodeBehind="NDM.aspx.cs" Inherits="DXBMS.Modules.CustomerExperience.NDM" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

        <script src="../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../js/chosen.jquery.min.js" type="text/javascript"></script>

    <style>
        .Grid
        {
            background-color: #fff;
            margin: 5px 0 10px 0;
            border: solid 1px #525252;
            border-collapse: collapse;
            font-family: Calibri;
            color: #474747;
        }
        
        .Grid td
        {
            padding: 2px;
            border: solid 1px #c1c1c1;
        }
        
        .Grid th
        {
            padding: 4px 2px;
            color: #fff;
            border-left: solid 1px #525252;
            font-size: 11px;
            text-align: center;
        }
        
        .Grid .alt
        {
            background: #fcfcfc url(Images/grid-alt.png) repeat-x top;
        }
        
        .Grid .pgr
        {
            background: #363670 url(Images/grid-pgr.png) repeat-x top;
        }
        
        .Grid .pgr table
        {
            margin: 3px 0;
        }
        
        .Grid .pgr td
        {
            border-width: 0;
            padding: 0 6px;
            border-left: solid 1px #666;
            font-weight: bold;
            color: #fff;
            line-height: 12px;
        }
        
        .Grid .pgr a
        {
            color: Gray;
            text-decoration: none;
        }
        
        .Grid .pgr a:hover
        {
            color: #000;
            text-decoration: none;
        }
        
        .calTodayDay
        {
            color: black;
            background-color: #cccccc;
        }
        .calSelector
        {
            background-color: #cccccc;
        }
        
        .calNextPrev
        {
            vertical-align: sub;
        }
        
        .calDayHeader
        {
            font-size: 7pt;
            background-color: #cccccc;
            font-weight: bold;
            text-align: center;
        }
        
        .calSelectedDay
        {
            font-weight: bold;
            color: white;
            background-color: #666666;
        }
        
        .calTitle
        {
            font-weight: bold;
            border-left-color: black;
            border-bottom-color: black;
            border-top-color: black;
            background-color: #999999;
            border-right-color: black;
        }
        
        .calWeekendDay
        {
            background-color: "#FFFFCC";
        }
        
        .calOtherMonthDay
        {
            color: gray;
        }
        
        
    </style>

    <link href="../../css/TabStyle.css" rel="stylesheet" type="text/css" />
    <asp:UpdatePanel runat="server" ID="UpdatePanel">
        <ContentTemplate>
            <div class="container">
                <div class="Heading" style="margin-top: 5px;">
                    Next Due Maintenance</div>
                <fieldset style="padding: 0;">
                    <table style="width: 100%; border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                             
                                <asp:Calendar ID="Calendar1" runat="server" Width="100%" 
                                    BackColor="White" DayNameFormat="FirstLetter" Font-Size="8pt" Font-Names="Arial"
                                     DayHeaderStyle-CssClass="calDayHeader"
                                     OnSelectionChanged="Calendar1_SelectionChanged">
                                 
                                </asp:Calendar>
                             
                            </td>
                        </tr>
                    </table>
                    <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="1" CssClass="TabStyle"
                        Width="100%" Style="direction: ltr; text-align: left; display: grid;">
                        <asp:TabPanel ID="TabPanel1" runat="server">
                            <HeaderTemplate>Letter Send Date</HeaderTemplate>
                            <ContentTemplate>
                                
                                            <asp:GridView ID="grdNextDueMaintenance" runat="server" Width="100%" ShowFooter="True"
                                                CssClass="Grid" OnRowCancelingEdit="gv_PSF_RowCancelingEdit" ShowHeaderWhenEmpty="True"
                                                AutoGenerateColumns="False" OnRowDataBound="gv_PSF_RowDataBound" OnRowEditing="gv_PSF_RowEditing"
                                                OnRowUpdating="gv_PSF_RowUpdating" OnDataBound="gv_PSF_DataBound">
                                                <HeaderStyle CssClass="GridHeader" />
                                                <FooterStyle CssClass="GridFooter" />
                                                <Columns>
                                                    <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png"
                                                        ButtonType="Image" UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png"
                                                        CausesValidation="false"></asp:CommandField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="DefJobCode">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblJobCode" runat="server" Text='<%# Bind("JobCardCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Customer Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="Customer" HeaderText="Customer Name"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Reg No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRegNo" runat="server" Text='<%# Bind("RegNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="RegNo" HeaderText="Reg No"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Last Maint. Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLastMaintenanceDate" runat="server" Text='<%# Bind("LastMaintenanceDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="LastMaintenanceDate" HeaderText="Last Maint. Date"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Last KM">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLastMaintenanceKM" runat="server" Text='<%# Bind("LastMaintenanceKM") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="LastMaintenanceKM" HeaderText="Last KM"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Schedule KM">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblScheduleKM" runat="server" Text='<%# Bind("ScheduleKM") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="ScheduleKM" HeaderText="Schedule KM"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Next Maint. Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNextMaintenanceDate" runat="server" Text='<%# Bind("NextMaintenanceDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="NextMaintenanceDate" HeaderText="Next Maint. Date"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Next Maint. KM">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNextMaintenanceKM" runat="server" Text='<%# Bind("NextMaintenanceKM") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="NextMaintenanceKM" HeaderText="Next Maint. KM"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Letter Send Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLetrDate" runat="server" Text='<%# Bind("LetterSentDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtLetterSentDate" runat="server" placeholder="dd-MM-yyyy" Style="background-image: url(../../../Images/Calendar_16x16.png);
                                                                background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtLetterSentDate"
                                                                Format="dd-MM-yyyy" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Call Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCallDate" runat="server" Text='<%# Bind("CalledDate10Day") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtCallDate" runat="server" placeholder="dd-MM-yyyy" Style="background-image: url(../../../Images/Calendar_16x16.png);
                                                                background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtCallDate"
                                                                Format="dd-MM-yyyy" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SMS Send Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSMSSendDate" runat="server" Text='<%# Bind("SMSSendDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtSmsDate" runat="server" placeholder="dd-MM-yyyy" Style="background-image: url(../../../Images/Calendar_16x16.png);
                                                                background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtSmsDate"
                                                                Format="dd-MM-yyyy" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks2Day") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="50" placeholder="Remarks"></asp:TextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel ID="TabPanel2" runat="server">
                            <HeaderTemplate>Call Send date</HeaderTemplate>
                            <ContentTemplate>
                                
                                            <asp:GridView ID="gvCallSend" runat="server" Width="100%" ShowFooter="True" CssClass="Grid"
                                                OnRowCancelingEdit="gvCallSend_RowCancelingEdit" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False"
                                                OnRowDataBound="gvCallSend_RowDataBound" OnRowEditing="gvCallSend_RowEditing" OnRowUpdating="gvCallSend_RowUpdating"
                                                OnDataBound="gvCallSend_DataBound">
                                                <HeaderStyle CssClass="GridHeader" />
                                                <FooterStyle CssClass="GridFooter" />
                                                <Columns>
                                                    <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png"
                                                        ButtonType="Image" UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png"
                                                        CausesValidation="false"></asp:CommandField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="DefJobCode">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblJobCode" runat="server" Text='<%# Bind("JobCardCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Customer Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="Customer" HeaderText="Customer Name"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Reg No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRegNo" runat="server" Text='<%# Bind("RegNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="RegNo" HeaderText="Reg No"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Last Maint. Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLastMaintenanceDate" runat="server" Text='<%# Bind("LastMaintenanceDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="LastMaintenanceDate" HeaderText="Last Maint. Date"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Last KM">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLastMaintenanceKM" runat="server" Text='<%# Bind("LastMaintenanceKM") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="LastMaintenanceKM" HeaderText="Last KM"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Schedule KM">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblScheduleKM" runat="server" Text='<%# Bind("ScheduleKM") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="ScheduleKM" HeaderText="Schedule KM"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Next Maint. Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNextMaintenanceDate" runat="server" Text='<%# Bind("NextMaintenanceDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="NextMaintenanceDate" HeaderText="Next Maint. Date"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Next Maint. KM">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNextMaintenanceKM" runat="server" Text='<%# Bind("NextMaintenanceKM") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="NextMaintenanceKM" HeaderText="Next Maint. KM"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Letter Send Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLetrDate" runat="server" Text='<%# Bind("LetterSentDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtLetterSentDate" runat="server" placeholder="dd-MM-yyyy" Style="background-image: url(../../../Images/Calendar_16x16.png);
                                                                background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtLetterSentDate"
                                                                Format="dd-MM-yyyy" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Call Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCallDate" runat="server" Text='<%# Bind("CalledDate10Day") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtCallDate" runat="server" placeholder="dd-MM-yyyy" Style="background-image: url(../../../Images/Calendar_16x16.png);
                                                                background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtCallDate"
                                                                Format="dd-MM-yyyy" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SMS Send Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSMSSendDate" runat="server" Text='<%# Bind("SMSSendDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtSmsDate" runat="server" placeholder="dd-MM-yyyy" Style="background-image: url(../../../Images/Calendar_16x16.png);
                                                                background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtSmsDate"
                                                                Format="dd-MM-yyyy" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks2Day") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="50" placeholder="Remarks"></asp:TextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                     
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel ID="TabPanel3" runat="server">
                            <HeaderTemplate>SMS Send Date</HeaderTemplate>
                            <ContentTemplate>
                               
                                            <asp:GridView ID="gvSMSSend" runat="server" Width="100%" ShowFooter="True" CssClass="Grid"
                                                OnRowCancelingEdit="gvSMSSend_RowCancelingEdit" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False"
                                                OnRowDataBound="gvSMSSend_RowDataBound" OnRowEditing="gvSMSSend_RowEditing" OnRowUpdating="gvSMSSend_RowUpdating"
                                                OnDataBound="gvSMSSend_DataBound">
                                                <HeaderStyle CssClass="GridHeader" />
                                                <FooterStyle CssClass="GridFooter" />
                                                <Columns>
                                                    <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png"
                                                        ButtonType="Image" UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png"
                                                        CausesValidation="false"></asp:CommandField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="DefJobCode">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblJobCode" runat="server" Text='<%# Bind("JobCardCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Customer Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="Customer" HeaderText="Customer Name"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Reg No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRegNo" runat="server" Text='<%# Bind("RegNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="RegNo" HeaderText="Reg No"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Last Maint. Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLastMaintenanceDate" runat="server" Text='<%# Bind("LastMaintenanceDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="LastMaintenanceDate" HeaderText="Last Maint. Date"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Last KM">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLastMaintenanceKM" runat="server" Text='<%# Bind("LastMaintenanceKM") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="LastMaintenanceKM" HeaderText="Last KM"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Schedule KM">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblScheduleKM" runat="server" Text='<%# Bind("ScheduleKM") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="ScheduleKM" HeaderText="Schedule KM"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Next Maint. Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNextMaintenanceDate" runat="server" Text='<%# Bind("NextMaintenanceDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="NextMaintenanceDate" HeaderText="Next Maint. Date"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Next Maint. KM">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNextMaintenanceKM" runat="server" Text='<%# Bind("NextMaintenanceKM") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="NextMaintenanceKM" HeaderText="Next Maint. KM"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Letter Send Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLetrDate" runat="server" Text='<%# Bind("LetterSentDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtLetterSentDate" runat="server" placeholder="dd-MM-yyyy" Style="background-image: url(../../../Images/Calendar_16x16.png);
                                                                background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtLetterSentDate"
                                                                Format="dd-MM-yyyy" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Call Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCallDate" runat="server" Text='<%# Bind("CalledDate10Day") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtCallDate" runat="server" placeholder="dd-MM-yyyy" Style="background-image: url(../../../Images/Calendar_16x16.png);
                                                                background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtCallDate"
                                                                Format="dd-MM-yyyy" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SMS Send Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSMSSendDate" runat="server" Text='<%# Bind("SMSSendDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtSmsDate" runat="server" placeholder="dd-MM-yyyy" Style="background-image: url(../../../Images/Calendar_16x16.png);
                                                                background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtSmsDate"
                                                                Format="dd-MM-yyyy" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks2Day") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="50" placeholder="Remarks"></asp:TextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>                                       
                            </ContentTemplate>
                        </asp:TabPanel>
                    </asp:TabContainer>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
