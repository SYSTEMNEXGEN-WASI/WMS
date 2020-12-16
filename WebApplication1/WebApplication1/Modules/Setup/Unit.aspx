<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="Unit.aspx.cs" Inherits="DXBMS.Modules.Setup.Unit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">

    <asp:UpdatePanel runat="Server">
        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div>
                        <fieldset>
                            <legend>Units</legend>
                            <div class="table-reponsive">
                               <table>
                                   <div class="table-responsive">


                                <table Width="70px" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">
                                    <tr>
                                        <td class="auto-style1">Unit  Description</td>
                                        <td class="auto-style6">
                                           <asp:TextBox ID="txtunitdec" runat="server" Height="21px" MaxLength="50" Width="400px"></asp:TextBox>
                                        </td>
                                        </tr>
                                    <tr>

                                        <td class="auto-style4">
                                               <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                              
                                        </td>

                                         <td class="auto-style2">
                                            
                                              <asp:Button ID="btnInsert" runat="server" Height="30px"  Text="Save" OnClick="btnInsert_Click" />                                            
                                             <asp:Button ID="btnClear" runat="server" Height="30px"  Text="Clear" OnClick="btnClear_Click" />
                                            <asp:Button ID="btnShow" runat="server" Height="30px"  Text="Print" OnClick="btnShow_Click" />
                                         </td>
                                        



                                    </tr>
                                      </table>

                            </div>
                                     <!---- FOR GRIDVIEW -->
                                   <table width="100%" >
                                       <td>
                                          <asp:GridView runat="server" ShowHeaderWhenEmpty="true" width="100%" ID="gv_Unit" AutoGenerateColumns="false"
                                            OnDataBound="gv_Unit_DataBound"
                                              OnRowDataBound="gv_Unit_RowDataBound"
                                            OnRowEditing="gv_Unit_RowEditing"
                                           OnRowCancelingEdit="gv_Unit_RowCancelingEdit"
                                            OnRowUpdating="gv_Unit_RowUpdating" 
                                           OnPageIndexChanging="gv_Unit_SelectedIndexChanged"
                                            AllowPaging="True">
                                              
                                              
                                              <PagerStyle CssClass="gridview"></PagerStyle>


                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />
                                             <Columns>
                                                <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                    UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false" ></asp:CommandField>
                                                 <asp:TemplateField HeaderText="Unit Code">
                                                     <ItemTemplate>
                                                        <asp:Label ID="lblUnitCode" runat="server" Text='<%# Bind("UnitCode") %>'></asp:Label>

                                                     </ItemTemplate>
                                                     <asp:ItemStyle HorizontalAlign="Center" />
                                                 </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Unit Description">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtUnitDesc" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitDesc" runat="server" Text='<%# Bind("UnitDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Remove">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="BtnRemove_Unit" runat="server" ImageUrl="~/Images/Delete_16x16.png" OnClick="BtnRemove_Unit_Click" OnClientClick="return confirm('Are you sure you want to delete ?')" ToolTip="Remove" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                             </Columns>

                                          </asp:GridView>

                                       </td>
                                       

                                   </table>
                              
                        </fieldset>

                    </div>

                </div>

            </div>

        </ContentTemplate>

    </asp:UpdatePanel>
        
</asp:Content>

