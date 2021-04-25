<%@ Page Title="" theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="FeedBackMaster.aspx.cs" Inherits="DXBMS.Modules.Setup.FeedBackMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <link href="../../Content/Style.css" rel="stylesheet" />
    <asp:UpdatePanel runat="server">

        <ContentTemplate>
            <div class="container">
                
                        <fieldset>
                            <legend>Service FeedBack  </legend>

                            <div >

                                <table width="70%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">
                                   <tr>
                                        <td class="auto-style1">Question Description</td>
                                        <td class="auto-style5">
                                            <asp:TextBox ID="txtParentQuestionDesc" runat="server" Height="21px" Width="300px"></asp:TextBox></td>
                                        
                                        <td class="auto-style1">Service Type </td>
                                        <td class="auto-style5">
                                            <asp:DropDownList ID="ddlServiceType" runat="server" Width="234px">
                                                <asp:ListItem value="0" >
                                                       Select
                                                   </asp:ListItem>
                                                <asp:ListItem Value="CRM">
                                                    CRM
                                                </asp:ListItem>
                                                <asp:ListItem Value="Service">
                                                    Service
                                                </asp:ListItem>
                                                <asp:ListItem Value="Compaign">
                                                    Compaign
                                                </asp:ListItem>
                                                <asp:ListItem Value="Event">
                                                    Event
                                                </asp:ListItem>
                                            </asp:DropDownList>
                                        </td>

                                                                     
                                   </tr>
                      
                                    <tr>
                                        <td class="auto-style1">Control Type</td>
                                        <td class="auto-style5">
                                            <asp:DropDownList ID="ddlControlType" runat="server" Width="234px">
                                                   <asp:ListItem value="0" >
                                                       Select
                                                   </asp:ListItem>
                                                   <asp:ListItem value="Option" >
                                                       Option
                                                   </asp:ListItem>
                                                   <asp:ListItem value="Rating" >
                                                       Rating
                                                   </asp:ListItem>
<%--                                                    <asp:ListItem value="Check Box" >
                                                       Check Box
                                                   </asp:ListItem>--%>
                                            </asp:DropDownList>

                                        </td>
                                        <td class="auto-style1">Sequence</td>

                                        <td>
                                            <asp:TextBox ID="txtSequence" runat="server" Width="234px" MaxLength="3"></asp:TextBox>
                                        </td>
                                        

                                        
                                    </tr>
                                    
                                    <%--<tr>
                                         <td>   
                                            <asp:CheckBox runat="server" Checked="true"  id="check" name="Active" Text="Active" > </asp:CheckBox>

                                        </td>
                                    </tr>--%>
                                    <tr>
                                         <td>
                                            <asp:CheckBox runat="server" Checked="false"  id="check" name="InActive" Text="InActive" > </asp:CheckBox>

                                        </td>
                                        <td class="auto-style4" colspan="3">
                                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td class="auto-style1" colspan="4">
                                            <asp:Button ID="btnInsert" runat="server" Height="30px" Text="Insert" Width="200px" OnClick="btnInsert_Click" />
<%--                                            <asp:Button ID="btnPrint" runat="server" Height="30px" Text="Print" Width="200px"  OnClick="btnPrint_Click"/>--%>
                                        </td>
                                    
                                    </tr>

                                </table>


                            </div>

                            <!---- FOR GRIDVIEW -->

                            <table width="100%">
                                <tr>
                                    <td>

                            


                                        <asp:GridView runat="server" ShowHeaderWhenEmpty="true" Width="100%" ID="gv_Feedback" AutoGenerateColumns="false" 
                                            OnRowDataBound="gv_Feedback_RowDataBound" 
                                            OnDataBound="gv_Feedback_DataBound" 
                                            OnRowEditing="gv_Feedback_RowEditing" 
                                            OnRowCancelingEdit="gv_Feedback_RowCancelingEdit" 
                                            OnRowUpdating="gv_Feedback_RowUpdating" 
                                            CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                            <Columns>
                                                <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                    UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>
                                            </Columns>

                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />

                                            <Columns>
                                                   

                                                    <asp:TemplateField HeaderText="Question Code" ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblParentQuestionCode" runat="server" Text='<%# Bind("ParentQuestionCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                   
                                                     <asp:TemplateField HeaderText="Question Description" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblParentQuestionDesc" runat="server" Text='<%# Bind("ParentQuestionDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtParentQuestionDescEdit" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Sequence" ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSequence" runat="server" Text='<%# Bind("QSequence") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtSequenceEdit" maxlength="3" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>

        
                                                    <asp:TemplateField HeaderText="Control Type" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblControlType" runat="server" Text='<%# Bind("ControlType") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlControlTypeEdit">
                                                           <asp:ListItem value="0" >
                                                               Select
                                                           </asp:ListItem>
                                                           <asp:ListItem value="Option" >
                                                               Option
                                                           </asp:ListItem>
                                                           <asp:ListItem value="Raating" >
                                                               Rating
                                                           </asp:ListItem>
<%--                                                            <asp:ListItem value="Check Box" >
                                                               Check Box
                                                           </asp:ListItem>--%>
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>


                                                    
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Service type" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblServiceType" runat="server" Text='<%# Bind("ServiceType") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlServiceTypeEdit">
                                                            <asp:ListItem value="0" >
                                                                   Select
                                                               </asp:ListItem>
                                                            <asp:ListItem Value="CRM">
                                                                CRM
                                                            </asp:ListItem>
                                                            <asp:ListItem Value="Service">
                                                                Service
                                                            </asp:ListItem>
                                                            <asp:ListItem Value="Compaign">
                                                                Compaign
                                                            </asp:ListItem>
                                                            <asp:ListItem Value="Event">
                                                                Event
                                                            </asp:ListItem>
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                                                                            
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                   <asp:TemplateField HeaderText="InActive" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                        <asp:Label ID="lblActive" runat="server" Text='<%# Bind("InActive") %>'></asp:Label>
                                                    </ItemTemplate>
                                                     </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Remove" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="BtnRemove_Feedback" runat="server"
                                                            ImageUrl="~/Images/Delete_16x16.png" OnClick="BtnRemove_Feedback_Click"
                                                            ToolTip="Remove" />
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    

                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>        
                        </fieldset>

                    
            </div>

        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
