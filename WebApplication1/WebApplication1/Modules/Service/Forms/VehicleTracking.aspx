<%@ Page Title="" Language="C#" Theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="VehicleTracking.aspx.cs" Inherits="DXBMS.Modules.Service.Forms.VehicleTracking" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<style>
    
    .Grid {background-color: #fff; margin: 5px 0 10px 0; border: solid 1px #525252; border-collapse:collapse; font-family:Calibri; color: #474747;}

.Grid td {

      padding: 2px;

      border: solid 1px #c1c1c1; }

.Grid th  {

      padding : 4px 2px;

      color: #fff;     

      border-left: solid 1px #525252;

      font-size: 14px;
      text-align: center; }

.Grid .alt {

      background: #fcfcfc url(Images/grid-alt.png) repeat-x top; }

.Grid .pgr {background: #363670 url(Images/grid-pgr.png) repeat-x top; }

.Grid .pgr table { margin: 3px 0; }

.Grid .pgr td { border-width: 0; padding: 0 6px; border-left: solid 1px #666; font-weight: bold; color: #fff; line-height: 12px; }  

.Grid .pgr a { color: Gray; text-decoration: none; }

.Grid .pgr a:hover { color: #000; text-decoration: none; }
    
</style>
   <asp:UpdatePanel runat="server" ID="UpdatePanel">
        <ContentTemplate>
            <div class="container">
                <fieldset>
                    <legend>Vehicle Tracking Board</legend>
                    <table  style="border-collapse:separate; border-spacing:5px; width:100%;" class="table table-condensed table-bordered table-hover">
                    <tr>
                        <td align="right" style="width: 16%">
                            From Date :
                        </td>                        
                        <td style="width: 16%">
                            <asp:TextBox ID="txtFromDate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png);
                                            background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                            <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" Enabled="True"
                                Format="dd-MM-yyyy" TargetControlID="txtFromDate">
                            </asp:CalendarExtender>
                        </td>
                        <td align="right" style="width: 16%">
                            To Date:
                        </td>
                        <td style="width: 16%">
                            <asp:TextBox ID="txtToDate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png);
                                            background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>                            
                            <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Enabled="True"
                                Format="dd-MM-yyyy" TargetControlID="txtToDate">
                            </asp:CalendarExtender>
                        </td>                     
                        <td style="width: 16% " align="right">Refresh Time</td>
                        <td style="width: 16%">
                            <asp:DropDownList ID="DropDownList1" runat="server" Width="80%">
                            <asp:ListItem Value="60000">1 min</asp:ListItem>
                            <asp:ListItem Value="180000" Selected="True">3 min</asp:ListItem>
                            <asp:ListItem Value="300000">5 min</asp:ListItem>                            
                            </asp:DropDownList>
                        </td>
                    </tr>                    
                    <tr>                       
                        <td colspan="6">
                            <asp:Timer ID="Timer1" runat="server" Enabled="false" OnTick="OnClick_btnCheck">
                            </asp:Timer>
                            <asp:GridView ID="gvVehicleTrack"  runat="server" AutoGenerateColumns="False" Width="100%" CssClass="Grid input-lg" AllowPaging="true">
                                <Columns>
                                    
                                    <asp:BoundField HeaderText="JobCard" DataField="JobCardCode" />
                                    <asp:BoundField HeaderText="RegNo" DataField="RegNo"/>
                                    <asp:BoundField HeaderText="ProdTitle" DataField="ProdTitle" />
                                    <asp:BoundField HeaderText="Job Card Date" DataField="jobCardDate" />
                                    <asp:BoundField HeaderText="Promise" DataField="PromiseDateTime"/>
                                    <asp:BoundField HeaderText="AdvisorName" DataField="AdvisorName" />                                
                                    <asp:BoundField HeaderText="WorkinProgress" DataField="WorkinProgress">
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </asp:BoundField>
                                    <%--<asp:BoundField HeaderText="FinalInspection" DataField="FinalInspection" />--%>
                                    <asp:BoundField HeaderText="ReadyForDelivery" DataField="ReadyForDelivery" >
                                    <ItemStyle HorizontalAlign="Center"/>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Delayed" DataField="Delayed" />
                                    <%--<asp:BoundField HeaderText="NewDate" DataField="NewDate" />--%>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <FooterStyle CssClass="GridFooter" />
                            </asp:GridView>
                        </td>                       
                    </tr>  
                    <tr>
                        <td align="center" colspan="6">
                            <asp:Button ID="Button1" runat="server" Text="Start" OnClick="OnClick_btnCheck" />
                            <asp:Button ID="Button2" runat="server" Text="Stop" OnClick="Button2_OnClick"/>
                        </td>
                    </tr>                  
                </table>
                </fieldset>
            </div>
        </ContentTemplate>
   </asp:UpdatePanel>
<%--   <script type="text/javascript" language="javascript">
       Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(delayUpdateClick);
       function delayUpdateClick(sender, args) {
           button = $get("<%= btnCheck.ClientID %>");
           setTimeout(function () { button.click(); }, 10000);
       }
</script>--%>
</asp:Content>
