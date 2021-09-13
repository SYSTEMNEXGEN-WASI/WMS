<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" Theme="Theme1" CodeBehind="CategorywiseInvoiceReport.aspx.cs" Inherits="DXBMS.Modules.Service.ServiceReports.Critaria.CategorywiseInvoiceReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="Heading">Job Card Category Invoice wise Detail Report</div>
                <fieldset>
                    <table width="50%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                       
                        <tr>
                         <td align="right">
                          Card Type Category <font color="red"><b>*</b></font>
                          </td>
                          <td align="right">
                          <asp:DropDownList ID="ddlJobCardTypeCategory" runat="server" Width="100%" AutoPostBack="true" TabIndex="1"
                                                OnSelectedIndexChanged="ddlJobCardTypeCategory_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlJobCardTypeCategory"
                                                PromptText="" QueryPattern="Contains">
                                            </ajaxToolkit:ListSearchExtender>
                          </td>
                          </tr>
                          <tr>
                          <td align="right">
                          Job Card Type <font color="red"><b>*</b></font>
                          </td>
                          <td align="right">
                          <asp:DropDownList ID="ddlJobCardTypeCode" runat="server" Width="100%" AutoPostBack="true" TabIndex="1"
                                                OnSelectedIndexChanged="ddlJobCardTypeCode_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <ajaxToolkit:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddlJobCardTypeCode"
                                                PromptText="" QueryPattern="Contains">
                                            </ajaxToolkit:ListSearchExtender>
                          </td>
                          </tr>
                          <tr>
                           <td align="right">
                            Sub Category 
                          </td>
                          <td align="right">
                          <asp:DropDownList ID="ddlSubCategory" runat="server" Width="100%" TabIndex="2">
                                           
                                            </asp:DropDownList>
                                            <ajaxToolkit:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddlSubCategory"
                                                PromptText="" QueryPattern="Contains">
                                            </ajaxToolkit:ListSearchExtender>
                          </td>
                        </tr>
                        <tr>
                            <td align="right">Employee of</td>
                            <td>
                                <asp:DropDownList ID="ddlAdvisorCode" runat="server" class="WidthAt764" Width="100%" TabIndex="8">
                                </asp:DropDownList>
                                <ajaxToolkit:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddlAdvisorCode"
                                    PromptText="" QueryPattern="Contains">
                                </ajaxToolkit:ListSearchExtender>
                            </td>
                        </tr>
                        <tr>
                            <td width="50%" align="right">From Date
                        <%--<CR:CrystalReportViewer ID="crviewer" runat="server" AutoDataBind="true" />--%>
                            </td>
                            <td>
                                <asp:TextBox ID="dtFrom" runat="server" Style="background-image: url(../../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd-MM-yyyy" TargetControlID="dtFrom" />
                            </td>

                        </tr>
                        <tr>
                            <td align="right">To Date
                            </td>
                            <td>
                                <asp:TextBox ID="dtTo" runat="server" Style="background-image: url(../../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd-MM-yyyy" TargetControlID="dtTo" />
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblMSG" runat="server" Text="" ForeColor="Red" Font-Size="Large" Font-Bold="true"></asp:Label></td>
                            <td>
                                <asp:Button ID="btnShowReport" runat="server" Text="Show Report" Width="110px" OnClick="btnShowReport_Click" /></td>
                            <%--OnClientClick="Print()"--%>
                        </tr>
                    </table>
                </fieldset>


            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function loadPDF() {
            window.open('GeneratePDF.aspx', '', 'scrollbars=no,menubar=no,height=600,width=800,resizable=yes,toolbar=no,location=no,status=no');
        }
    </script>

</asp:Content>