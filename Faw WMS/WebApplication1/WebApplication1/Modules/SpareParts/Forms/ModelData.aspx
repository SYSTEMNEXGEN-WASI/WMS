<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" Theme="Theme1" CodeBehind="ModelData.aspx.cs" Inherits="DXBMS.Modules.SpareParts.Forms.ModelData" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
  <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
          <style type="text/css">
        body
        {
            font-family: Arial;            
        }
        table
        {
            border: 1px solid #ccc;
        }
        table th
        {
            background-color: #F7F7F7;
            color: #333;
            
        }
        table th, table td
        {
            padding: 5px;
            border-color: #ccc;
        }
        .Pager span
        {
            color: #333;
            background-color: #F7F7F7;
            font-weight: bold;
            text-align: center;
            display: inline-block;
            width: 20px;
            margin-right: 3px;
            line-height: 150%;
            border: 1px solid #ccc;
        }
        .Pager a
        {
            text-align: center;
            display: inline-block;
            width: 20px;
            border: 1px solid #ccc;
            color: #fff;
            color: #333;
            margin-right: 3px;
            line-height: 150%;
            text-decoration: none;
        }
        .highlight
        {
            background-color: #FFFFAF;
        }
        .searchCSS
        {
            height:30px;
            border-radius:5px;
        }
        .dataTables_wrapper
        {
            margin-top:10px;
        }
    </style>
    <asp:UpdatePanel runat="server">
    <ContentTemplate>
     <div class="Heading"><asp:Label runat="server" ID="lblHeading"></asp:Label></div>    
             <div class="row">
                    <div class="col-lg-12 pull-right">
                       
                        <div class="col-lg-2 pull-right" style="margin-top: 10px;" >
                            <asp:Label ID="Label2" class="lead" runat="server">Create New</asp:Label>
                        </div>
                        <div class="col-lg-1 pull-right" style="left: 35px;margin-top: 5px;">
                            <asp:ImageButton ID="btnAdd" runat="server" Height="40px" ImageUrl="~/Images/AddIMG.png"
                              Width="40px" style="cursor:pointer;" onclick="btnAdd_Click" />
                        </div>   
                    </div>
                </div> 
                <fieldset>

            
                  <div class="tab-content">
                   <div class="row">
                    <div class="col-lg-12">
                       
                        <div class="col-lg-2" style="margin-top: 10px;">
                             <asp:Label ID="Label1" runat="server" Text="Search By:" Width="128px" Font-Size="12px"></asp:Label>
                        </div>
                        <div class="col-lg-10" style="margin-top: 5px;">
                            <asp:RadioButtonList ID="rdblColumns" runat="server" 
                    RepeatDirection="Horizontal" Width="100%">
                </asp:RadioButtonList>
                        </div>   
                    </div>
                 
                </div> 
                   <div class="row">
                      <div class="col-lg-12">
                     <div class="col-lg-2" style="margin-top: 10px;">
                        <asp:Label ID="Label4" runat="server" Text="Search Text:" ></asp:Label>
                     </div>
                     <div class="col-lg-2" style="text-align:right;margin-top: 10px;">
                      <asp:TextBox ID="txtSearch" runat="server" TabIndex="20" width="300px"></asp:TextBox>
                     </div>
                     
                     <div class="col-lg-2 pull-right" style="text-align:right;margin-top: 10px;">
                      <asp:Button ID="btnClear" runat="server" Text="Clear"  TabIndex="30" Width="90%"
                    OnClick="btnClear_Click"  />
                     </div>
                     <div class="col-lg-2 pull-right" style="text-align:right;margin-top: 10px;">
                     <asp:Button ID="btnSearch" runat="server" Text="Search"  TabIndex="30" Width="90%"
                  OnClick="btnSearch_Click" />
                     </div>
                     </div>
                    </div>
                  <div role="tabpanel" class="tab-pane active" id="openjobcards">
                      <asp:GridView ID="GDV" runat="server" CssClass="gvParts" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%"
                          TabIndex="100" OnSelectedIndexChanged="GDV_SelectedIndexChanged" AllowPaging="True" AllowSorting="True" PageSize="15"
                          Font-Names="Arial" Font-Size="9pt" Style="text-align: left" PagerStyle-CssClass="pgr" OnPageIndexChanging="GDV_PageIndexChanging"
                          ShowHeaderWhenEmpty="True" >
                          <%--style=" text-align: left; overflow-y: visible; overflow-x:hidden; display:block; " --%>
                          <AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
                          <FooterStyle CssClass="GridFooter" />
                          <Columns>
                              <asp:CommandField ShowSelectButton="True" />
                          </Columns>
                          <EditRowStyle BackColor="#7C6F57"></EditRowStyle>
                          <EmptyDataTemplate>
                              <asp:Label ID="Label3" runat="server" Style="font-family: Arial; font-size: small; color: #FF3300"
                                  Text="No Data Found ... "></asp:Label>
                              &nbsp;
                          </EmptyDataTemplate>
                          <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="blue" />
                          <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" BackColor="#1C5E55" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ForeColor="White"></HeaderStyle>

                          <PagerStyle HorizontalAlign="Center" BackColor="#666666" CssClass="mGrid pgr" ForeColor="blue"></PagerStyle>

                          <RowStyle BackColor="#E3EAEB" />
                          <EditRowStyle BackColor="#999999" />
                          <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        
                       

                         
                          <PagerStyle CssClass="mGrid pgr" BackColor="#284775" HorizontalAlign="Center" />
                          <%--BackColor="#284775" ForeColor="White" HorizontalAlign="Center"--%>
                          <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="black"
                              Font-Names="Arial" Font-Size="10pt" HorizontalAlign="Left"
                              VerticalAlign="Middle" />
                          <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                      </asp:GridView>

                </div>
                <div class="row">
                <asp:Label ID="lblMsg" runat="server" Width="739px" Font-Bold="True" ForeColor="#C00000"
                    Visible="False" TabIndex="220" Height="16px"></asp:Label>

                </div>
                </div>
                    </fieldset>
    </ContentTemplate>

    </asp:UpdatePanel>
     <script type="text/javascript">
         $(document).ready(function () {
             $(".gvParts").DataTable();
                $(".gvParts").DataTable(
                {
                    "order": [[0, "desc"]]
                });
                
              
         });
        

        
    </script>
   <script type="text/javascript">
       function Search_Gridview(strKey, strGV) {
       debugger
        var strData = strKey.value.toLowerCase().split(" ");
        var tblData = document.getElementsByClassName(strGV);
        var rowData;
        for (var i = 1; i < tblData.rows.length; i++) {
            rowData = tblData.rows[i].innerHTML;
            var styleDisplay = 'none';
            for (var j = 0; j < strData.length; j++) {
                if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                    styleDisplay = '';
                else {
                    styleDisplay = 'none';
                    break;
                }
            }
            tblData.rows[i].style.display = styleDisplay;
        }
    }    
</script>


</asp:Content>
