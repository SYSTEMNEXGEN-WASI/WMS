<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LookUP.aspx.cs" Inherits="WebApplication2.LookUP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <link href="Content/Grid.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <table style="width: 100%">
        <tr>
            <td bgcolor="#336699" colspan="5" style="font-weight: bolder; font-size: 8pt; color: white;
                font-family: Verdana; height: 15px; font-variant: normal">
                <asp:Label ID="lblTitle" runat="server" SkinID="LookUpLabel" Text="Look up Control"></asp:Label>
            </td>
        </tr>
       <%-- <tr>
            <td style="width: 83px; height: 24px;">
                &nbsp;</td>
            <td style="height: 24px;" colspan="2">
                &nbsp;</td>
            <td style="width: 100%; height: 24px;">
                &nbsp;</td>
            <td style="width: 100%; height: 24px;">
                &nbsp;</td>
        </tr>--%>
        <tr>
            <td style="width: 83px; height: 24px;">
                <asp:Label ID="Label2" runat="server" Text="Search By:" Width="128px"></asp:Label>
            </td>
            <td style="height: 24px;" colspan="4">
                <asp:RadioButtonList ID="rdblColumns" runat="server" Height="84px" 
                    RepeatDirection="Horizontal" Width="100%">
                </asp:RadioButtonList>
            </td>
            
        </tr>
        <tr>
            <td style="width:10%; height : 26px;">
                <asp:Label ID="Label1" runat="server" Text="Search Text:" Width="128px"></asp:Label>
            </td>
            <td style="height: 26px;">
                <asp:TextBox ID="txtSearch" runat="server" TabIndex="20" width="90%"></asp:TextBox>
            </td>
            <td style="width:10% ; height: 26px;">
                <asp:Button ID="btnSearch" runat="server" Text="Search"  TabIndex="30" Width="90%"
                    OnClick="btnSearch_Click" Height="23px" />
            </td>
            <td style="width: 10%; height: 26px;">
                <asp:Button ID="btnClear" runat="server" Text="Clear"  TabIndex="30" Width="90%"
                    OnClick="btnClear_Click" Height="23px" />
            </td>
            <td style="width: 10%; height: 26px;">
            </td>
        </tr>
        <tr>
            <td colspan="5" >  
               <div style="width: 100%; height: 400px; overflow: scroll">
                    <asp:GridView ID="GDV" runat="server" SkinID="LookUpGridView" CssClass="gvParts" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%"
                    TabIndex="100" OnSelectedIndexChanged="GDV_SelectedIndexChanged" AllowPaging="True" AllowSorting="True" PageSize="15" 
                    Font-Names="Arial" Font-Size="9pt" style="text-align: left" PagerStyle-CssClass="pgr" 
                    ShowHeaderWhenEmpty="True" ShowHeader="true"  OnPageIndexChanging="GDV_PageIndexChanging">
                        <%--style=" text-align: left; overflow-y: visible; overflow-x:hidden; display:block;" --%>
                        <FooterStyle CssClass="GridFooter" />
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" />
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="Label3" runat="server" Style="font-family: Arial; font-size: small;
                            color: #FF3300" Text="No Data Found ... "></asp:Label>
                        &nbsp;
                    </EmptyDataTemplate>
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <EditRowStyle BackColor="#999999" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle  CssClass="mGrid pgr" BackColor="#284775" HorizontalAlign="Center"/> <%--BackColor="#284775" ForeColor="White" HorizontalAlign="Center"--%>
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" 
                        Font-Names="Arial" Font-Size="10pt" HorizontalAlign="Left" 
                        VerticalAlign="Middle" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView>
                </div>             
                
                <br />
                &nbsp;<asp:Label ID="lblMsg" runat="server" Width="739px" Font-Bold="True" ForeColor="#C00000"
                    Visible="False" TabIndex="220" Height="16px"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

<script src="js/jquery-3.2.1.min.js"></script>
    <%--<script src="js/bootstrap.min.js"></script>--%>
<%--<script src="js/ScrollableGridPlugin.js"></script>--%>
<script src="js/ScrollableGridPlugin_ASP.NetAJAX_2.0.js"></script>
<%--<link href="css/datatables.min.css" rel="stylesheet" type="text/css" />
    <script src="js/datatables.min.js" type="text/javascript"></script>--%>
<script type = "text/javascript">
  $(document).ready(function () {
        //$(".gvParts").DataTable({
        //    "bSort": false,
        //    "paging": false
      //});
      
        
  });

    function Confirm() {
          $('#<%=GDV.ClientID %>').Scrollable({
            
        });
      }
</script>