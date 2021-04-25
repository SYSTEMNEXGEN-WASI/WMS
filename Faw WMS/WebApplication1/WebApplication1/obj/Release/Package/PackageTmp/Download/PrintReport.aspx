<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintReport.aspx.cs" Inherits="DXBMS.Download.PrintReport" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
   <%-- <input id="btnPrint" type="button" value="Print" onclick="Print()" />  --%>

   <div id="dvReport">
        <%--<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />--%>
          <asp:Literal runat="server" ID="ltEmbed" ></asp:Literal>
    </div>
    </form>
</body>
</html>
<script type="text/javascript">  
   
     
</script>  