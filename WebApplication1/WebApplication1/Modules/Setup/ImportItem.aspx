<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="ImportItem.aspx.cs" Inherits="DXBMS.Modules.Setup.ImportItem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <script src="../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="../../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../js/chosen.jquery.js" type="text/javascript"></script>
    <script src="../../js/chosen.jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="../../Jquery/quicksearch.js" type="text/javascript"></script>
    <script src="../../../Jquery/jquery.tablesorter-2.0.3.js" type="text/javascript"></script>
    <link href="../../../css/styletable.css" rel="stylesheet" type="text/css" />
    <link href="../../../css/datatables.min.css" rel="stylesheet" type="text/css" />
    <script src="../../../js/datatables.min.js" type="text/javascript"></script>
    <style type="text/css">
    .makespace {
        margin-bottom: 0.5rem;
    }

    .icheckbox_minimal-blue {
        top: 10px;
    }

        body {
            font-family: Arial;
        }
   
        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: black;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }

        .loading {
            font-family: Arial;
            font-size: 10pt;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 120px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>

        <ContentTemplate>
            <div class="container">


                <div class="row">
                    <div class="col-lg-12">
                        <fieldset>
                        <div class="row">
                        <div class="col-md-4">
                         <asp:Label ID="lblDemo" runat="server">Demo</asp:Label>
                        <div class="input-group controls">
                           <%-- <span class="input-group-addon"><i class="fa fa-calendar"></i></span>--%>
                            <asp:Button  runat="server" ID="Download" Text="Download" OnClick="Download_Click"/>
                        </div>
                        <div class="makespace"></div>
                    </div>
                        </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="col-lg-5">
                                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-lg-3" style="text-align: right; margin-top: 5px;">
                                        <asp:Label ID="Label2" Class="lead" runat="server">Import Excel File</asp:Label>
                                    </div>
                                    <div class="col-lg-2" style="text-align: right; margin-top: 5px;">
                                        <asp:FileUpload ID="FileUpload1" CssClass="custom-file-input" runat="server" />
                                        <label class="custom-file-label"></label>
                                    </div>
                                    <div class="col-lg-2 pull-right" style="text-align: right;">
                                        <asp:Button ID="btnUpload" runat="server" Text="Import Excel" ty OnClick="btnUpload_Click" OnClientClick="ShowProgress(); this.disabled = true; this.value = 'Please Wait';" UseSubmitBehavior="false" ></asp:Button>
                                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="ShowProgress(); this.disabled = true; this.value = 'Please Wait';" UseSubmitBehavior="false"/>
                                    </div>
                                </div>
                            </div>
                            <div style="width: 100%; height: 500px; overflow: scroll">

                            <asp:GridView ID="grdItem" runat="server" AutoGenerateColumns="true" CssClass="GridItem table table-bordered " Width="100%">
                                <EmptyDataTemplate>
                                    <div class="text-center">No record found</div>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:BoundField HeaderText="Code" DataField="ItemCode" />
                                    <asp:BoundField HeaderText="Description" DataField="ItemDesc" />
                                    <asp:BoundField HeaderText="Part" DataField="PartItemNo" />
                                    <asp:BoundField HeaderText="Purchase" DataField="PurchaseRate" />
                                    <asp:BoundField HeaderText="Sale" DataField="SaleRate" />
                                    <asp:BoundField HeaderText="Source" DataField="Source" />
                                </Columns>
                            </asp:GridView>
                            </div>
                            <div class="loading" align="center">
                                Loading. Please wait.<br />
                                <br />
                                <img src="../../Images/loader.gif" alt="" />
                            </div>
                            <%--<img src="../../Images/loader.gif" alt="" />--%>
                        </fieldset>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
             

        $(document).ready(function () {
          
            $(".GridItem").DataTable();
        });

        function ShowProgress() {
            $("#btnSave").value = "Please Wait";

            setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
    </script>

</asp:Content>
