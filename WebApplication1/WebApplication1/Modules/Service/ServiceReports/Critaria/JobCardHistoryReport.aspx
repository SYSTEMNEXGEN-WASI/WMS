<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="JobCardHistoryReport.aspx.cs" Inherits="DXBMS.Modules.Reports.JobCardHistoryReport" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
     <script src="../../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../../js/chosen.jquery.min.js" type="text/javascript"></script>
   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="Heading">JOB CARD HISTORY</div>
                <fieldset>                
                    <table style="width: 100%; border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                                                
                        <tr>
                            <td style="width: 10%" align="right">
                                RegNo:
                                <asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png" OnClick="imgLookup_Click" />
                            </td>
                            <td style="width: 20%" align="left">
                                <asp:DropDownList ID="ddlRegNo" runat="server" Width="195px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlRegNo_SelectedIndexChanged">
                                </asp:DropDownList>
                                <ajaxToolkit:ListSearchExtender ID="ddlRegNo_ListSearchExtender" runat="server" BehaviorID="ddlRegNo_ListSearchExtender"
                                    TargetControlID="ddlRegNo" QueryPattern="Contains">
                                </ajaxToolkit:ListSearchExtender>
                            </td>
                            <td style="width: 8%" align="right">
                                EngineNo:
                            </td>
                            <td style="width: 20%" align="left">
                                <asp:DropDownList ID="ddlEngineNo" runat="server" Width="195px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlEngineNo_SelectedIndexChanged">
                                </asp:DropDownList>
                                <ajaxToolkit:ListSearchExtender ID="ddlEngineNo_ListSearchExtender" runat="server"
                                    BehaviorID="ddlEngineNo_ListSearchExtender" TargetControlID="ddlEngineNo" QueryPattern="Contains">
                                </ajaxToolkit:ListSearchExtender>
                            </td>
                            <td style="width: 6%">
                                ChassisNo
                            </td>
                            <td style="width: 25%" align="left">
                                <asp:DropDownList ID="ddlChassisNo" runat="server" Width="195px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlChassisNo_SelectedIndexChanged">
                                </asp:DropDownList>
                                <ajaxToolkit:ListSearchExtender ID="ddlChassisNo_ListSearchExtender" runat="server"
                                    BehaviorID="ddlChassisNo_ListSearchExtender" TargetControlID="ddlChassisNo" QueryPattern="Contains">
                                </ajaxToolkit:ListSearchExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Customer:
                            </td>
                            <td  align="left">
                                <asp:TextBox ID="txtCusCode" runat="server" ReadOnly="True" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtCustomer" runat="server" Width="195px" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td align="right">
                                Product:
                            </td>
                            <td>
                                <asp:TextBox ID="txtProduct" runat="server" ReadOnly="True" Width="195px"></asp:TextBox>
                            </td>
                            <td align="right">
                                VersionCode:
                            </td>
                            <td >
                                <asp:TextBox ID="txtVersionCode" runat="server" ReadOnly="True" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtVerDesc" runat="server" Width="195px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>                     
                        
                        <tr>
                            <td>
                                <asp:Label ID="lblMSG" runat="server"></asp:Label>
                            </td>
                            <td colspan="5">                                
                                <asp:Button ID="BtnHistory" runat="server" OnClick="BtnHistory_Click" Text="View"
                                    Width="120px" />
                            </td>                           
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
   </asp:UpdatePanel>
</asp:Content>
