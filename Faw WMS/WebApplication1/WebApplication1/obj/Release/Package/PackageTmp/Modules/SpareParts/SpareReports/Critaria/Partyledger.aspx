<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="Partyledger.aspx.cs" Inherits="DXBMS.Modules.SpareParts.SpareReports.Critaria.Partyledger" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

<script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container" >
              <fieldset>
       <legend>Party Ledger</legend>
    <table width="100%" style="border-collapse:separate; border-spacing:5px;" id="tbl" class="table table-condensed table-bordered table"/>
        <tr>
            <td style="width:50%; text-align:right">
            <asp:Label runat="server" ID="lblType">Type  <font color="red"><b>*</b></font></asp:Label>   
            </td>
            <td>
               <asp:DropDownList runat="server" ID="ddlType" CausesValidation="false" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                 <asp:ListItem Value="Cus" Selected="True">Customer</asp:ListItem>
               <asp:ListItem Value="Ins">Insurance</asp:ListItem>

               </asp:DropDownList>
            </td>
        </tr>
          <tr id="CustomerRow">
                    <td style="text-align:right"">  <asp:Label runat="server" Visible="false" ID="lblInsComp">Ins Comp 
                     <font color="red"><b>*</b></font>
                      <asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png" OnClick="imgLookup_Click" />
                          </asp:Label>   
                       
                    </td>
                    <td>
                       <asp:TextBox runat="server" ID="txtInsCompCode" ReadOnly="true" Visible="false" SkinID="ROTextBox" Width="25%"></asp:TextBox>
                       <asp:TextBox runat="server" ID="txtInsCompDesc" ReadOnly="true" Visible="false" SkinID="ROTextBox" Width="65%"></asp:TextBox>
                    </td>
                </tr>
                <tr id="InsRow">
                  
                   <td style="text-align:right"">  <asp:Label runat="server" ID="lblInsBrh" Visible="false">Ins Branch 
                     <font color="red"><b>*</b></font><asp:ImageButton runat="server" ID="imgInsBr" ImageUrl="~/Images/LookupNew.png" OnClick="imgInsBr_Click" />
                   </asp:Label>
                    </td>
                    <td>
                       <asp:TextBox runat="server" ID="txtInsBrCode" ReadOnly="true" Visible="false" SkinID="ROTextBox" Width="25%"></asp:TextBox>
                       <asp:TextBox runat="server" ID="txtInsBrDesc" ReadOnly="true" Visible="false" SkinID="ROTextBox" Width="65%"></asp:TextBox>
                    </td>
                </tr>
        <tr>
             <td style="width:50%; text-align:right">Customer Code<asp:ImageButton runat="server" ID="imgCustomerCode" CausesValidation="false" ImageUrl="~/Images/LookupNew.png" OnClick="imgCustomerCode_Click" /></td>
                        <td width="39%">
                        <asp:TextBox runat="server" ID="txtCusCode" ReadOnly="true" Width="30%"   TabIndex="1" SkinID="ROTextBox"></asp:TextBox>
                         <asp:TextBox runat="server" ID="txtCusDesc" ReadOnly="true" Width="67%"  TabIndex="2" SkinID="ROTextBox"></asp:TextBox>
                            
                        </td>
        </tr>
        <tr>
            <td style="width:50%; text-align:right">
                From Date  <font color="red"><b>*</b></font>
            </td>
            <td>
                <asp:TextBox ID="txtFromDate" runat="server" Style="background-image: url(/Images/Calendar_16x16.png); 
                                   background-repeat:no-repeat; padding-left:20px;" width="180px"></asp:TextBox>
                 <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFromDate" Format="dd-MM-yyyy"></asp:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">
                To Date  <font color="red"><b>*</b></font>
            </td>
            <td>
                <asp:TextBox ID="txtToDate" runat="server" Style="background-image: url(/Images/Calendar_16x16.png);
                                   background-repeat:no-repeat; padding-left:20px;" width="180px"></asp:TextBox>
                 <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate" Format="dd-MM-yyyy"></asp:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td >
               <asp:Label ID="lblMsg" runat="server" ></asp:Label> 
            </td>
            <td>
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="Print"/>
            </td>
        </tr>
    </table>

  </fieldset>
               
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
