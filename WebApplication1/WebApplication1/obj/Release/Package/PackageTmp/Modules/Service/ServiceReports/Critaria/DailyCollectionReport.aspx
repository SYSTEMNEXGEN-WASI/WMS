<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DailyCollectionReport.aspx.cs" Theme="Theme1"
    MasterPageFile="~/Root.master" Inherits="DXBMS.Modules.Service.ServiceReports.Critaria.DailyCollectionReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">

        <script src="../../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../../js/chosen.jquery.min.js" type="text/javascript"></script>
        


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
            <div class="Heading"><asp:Label ID="lblHeading" runat="server"></asp:Label></div>
            <fieldset>
            <table width="50%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                <tr id="CustomerRow">
                    <td align="right">Cutomer 
                        <asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png" OnClick="imgLookup_Click" />
                    </td>
                    <td>
                       <asp:TextBox runat="server" ID="txtCusCode" ReadOnly="true" SkinID="ROTextBox"></asp:TextBox>
                    </td>
                </tr>
                <tr> 
                    <td width="50%" align="right">
                    From Date
                        <%--<CR:CrystalReportViewer ID="crviewer" runat="server" AutoDataBind="true" />--%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" Style="background-image: url(../../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtFromDate"/>
                    </td>
                   
                </tr>
                <tr>
                    <td align="right">
                    To Date
                    </td>
                    <td>
                    <asp:TextBox ID="txtToDate" runat="server" Style="background-image: url(../../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtToDate"/>
                    </td>
                    
                </tr>
                <tr>
                <td></td>
                <td><asp:Button ID="btnPrint" runat="server" Text="Show Report" Width="110px" onclick="btnPrint_Click"/></td> <%----%>
                
                </tr>
            </table>
            </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script>

        $(document).ready(function () {
            debugger
            var qrStr = window.location.search;
            qrStr = qrStr.split("?")[1].split("=")[1];

            if (qrStr == 'DCR') {
                $("#CustomerRow").hide();
            } else {
                $("#CustomerRow").show();
            }
        })

        

    </script>

</asp:Content>
