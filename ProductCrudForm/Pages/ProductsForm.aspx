<%@ Page Title="Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductsForm.aspx.cs" Inherits="ProductCrudForm.Pages.ProductsForm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(PageLoadedEventHandler);
        });
        function PageLoadedEventHandler() {

        }
        function openModal() {
            clearModal();
            $('#modalAddProduct').modal('toggle');
        }
        function closeModal() {
            $('#modalAddProduct').modal('hide');
        }
        function clearModal() {
            $('#<%= txtName.ClientID %>').val('');
            $('#<%= ddlCategory.ClientID %>').prop('selectedIndex', 0);
            $('#<%= txtDescription.ClientID %>').val('');
            $('#<%= txtPrice.ClientID %>').val('');
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <section>
                <div class="row mb-2">
                    <div class="col-lg-2 col-md-2">
                        <div class="input-group input-group-sm mt-2">
                            <asp:Label id="Label1" class="input-group-text" Text="From" runat="server"/>
                            <asp:TextBox id="Date1" runat="server" CssClass="form-control form-control-sm"/>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="yyyy-MM-dd" TargetControlID="Date1"></asp:CalendarExtender>
                        </div>
                    </div>
                    <div class="col-lg-2 col-md-2">
                        <div class="input-group input-group-sm mt-2">
                            <asp:Label id="Label2" class="input-group-text" Text="To" runat="server"/>
                            <asp:TextBox id="Date2" runat="server" CssClass="form-control form-control-sm" />
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="yyyy-MM-dd" TargetControlID="Date2"></asp:CalendarExtender>
                        </div>
                    </div>
                    <div class="col-lg-2 col-md-2 mt-2">
                        <div class="input-group">
                            <asp:TextBox id="txtSearch" runat="server" CssClass="form-control form-control-sm" placeholder="Search" />
                            <asp:LinkButton ID="btnOk" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnOk_Click">Ok</asp:LinkButton>
                        </div>
                    </div>
                </div>

                <div class="row mb-2">
                    <div class="col-md-2 mb-3">
                        <asp:LinkButton ID="btnAdd" runat="server" Text="Create New" OnClick="btnAdd_Click"/> 
                    </div>
                    <asp:GridView runat="server" id="gvProducts"
                        AutoGenerateColumns="False"
                        Allowpaging="true" Pagesize="8"
                        CssClass="table table-striped table-hover w-auto"
                        OnRowEditing="gvProducts_RowEditing"
                        OnRowUpdating="gvProducts_RowUpdating"
                        OnRowCancelingEdit="gvProducts_RowCancelingEdit"
                        OnRowDeleting="gvProducts_RowDeleting"
                        OnRowDataBound="gvProducts_RowDataBound"
                        OnPageIndexChanging="gvProducts_PageIndexChanging">
                        
                        <Columns>
                            <asp:TemplateField HeaderText="Sl.">  
                                <ItemTemplate>  
                                    <asp:Label ID="gvlblSl" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>  
                                </ItemTemplate>  
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Id" Visible="false">  
                                <ItemTemplate>  
                                    <asp:Label ID="gvlblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>  
                                </ItemTemplate>  
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Name">  
                                <ItemTemplate>  
                                    <asp:Label ID="gvlblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>  
                                </ItemTemplate>  
                                <EditItemTemplate>  
                                    <asp:TextBox ID="gvtxtName" runat="server" Text='<%# Eval("Name") %>' class="form-control form-control-sm"></asp:TextBox>  
                                </EditItemTemplate>  
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Category">  
                                <ItemTemplate>  
                                    <asp:Label ID="gvlblCategory" runat="server" Text='<%# Eval("categoryname") %>'></asp:Label>  
                                </ItemTemplate>  
                                <EditItemTemplate>  
                                    <asp:DropDownList ID="gvddlCategory" runat="server" DataTextField="Name" DataValueField="Id" Width="100px" CssClass="form-control form-control-sm"></asp:DropDownList>
                                </EditItemTemplate>  
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Image">  
                                <ItemTemplate>  
                                    <asp:Label ID="gvlblImage" Visible="false" runat="server" Text='<%# Eval("Image") %>'></asp:Label>
                                    <asp:HyperLink ID="gvhlinkImage" runat="server" NavigateUrl='<%# "~/Images/"+Eval("Image") %>' Target="_blank"
                                        Text='<%# Eval("Image").ToString().Length > 13 ? Eval("Image").ToString().Substring(13) : "" %>' ></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Description">  
                                <ItemTemplate>  
                                    <asp:Label ID="gvlblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>  
                                </ItemTemplate>  
                                <EditItemTemplate>  
                                    <asp:TextBox ID="gvtxtDescription" runat="server" Text='<%# Eval("Description") %>' class="form-control form-control-sm"></asp:TextBox>  
                                </EditItemTemplate>  
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Price">  
                                <ItemTemplate>  
                                    <asp:Label ID="gvlblPrice" runat="server" Text='<%# Eval("Price") %>'></asp:Label>  
                                </ItemTemplate>  
                                <EditItemTemplate>  
                                    <asp:TextBox ID="gvtxtPrice" runat="server" Text='<%# Eval("Price") %>' TextMode="Number" class="form-control form-control-sm"></asp:TextBox>  
                                </EditItemTemplate>  
                            </asp:TemplateField>

                            <asp:TemplateField>  
                                <ItemTemplate>  
                                    <asp:LinkButton ID="lbtnEdit" runat="server" Text="" CommandName="Edit" >
                                        <i class="bi bi-pencil-square text-warning"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>  
                                <EditItemTemplate>  
                                    <asp:LinkButton ID="lbtnUpdate" runat="server" Text="" CommandName="Update" CssClass="text-decoration-none me-1">
                                        <i class="bi bi-check-square text-success"></i>
                                    </asp:LinkButton> 
                                    <asp:LinkButton ID="lbtnCancel" runat="server" Text="" CommandName="Cancel" CssClass="text-decoration-none" >
                                        <i class="bi bi-x-lg text-secondary"></i>
                                    </asp:LinkButton>
                                </EditItemTemplate>  
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton id="lbtnProListExcel" Text="Excel" runat="server" OnClick="lbtnProListExcel_Click">
                                        <span class="text-success"><i class="bi bi-file-earmark-excel-fill"></i></span>
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>  
                                    <asp:LinkButton ID="lbtnDelete" runat="server" Text="" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');">
                                        <i class="bi bi-trash text-danger"></i>
                                    </asp:LinkButton>  
                                </ItemTemplate>  
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton id="lbtnProListPrint" Text="PDF" runat="server" OnClick="lbtnProListPrint_Click">
                                        <span class="text-success"><i class="bi bi-printer"></i></span>
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>  
                                    <asp:LinkButton id="gvlbtnProInfoPrint" OnClick="gvlbtnProInfoPrint_Click" Text="Print" runat="server" Font-Underline="False">
                                        <i class="bi bi-file-earmark-arrow-down text-success"></i>
                                    </asp:LinkButton>
                                </ItemTemplate> 
                                <headerstyle backcolor="#e6f3f7"/>
                            </asp:TemplateField>

                        </Columns>
                        <headerstyle backcolor="#e6f3f7"/>
                        <pagerstyle CssClass="pagerStyle" />
                    </asp:GridView>
                </div>
            </section>

            <!-- Modal -->
            <div class="modal fade" id="modalAddProduct" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h1 class="modal-title fs-5" id="exampleModalLabel">Add Product</h1>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <div class="row mb-2">
                                <asp:Label ID="Label3" class="col-form-label col-md-3" runat="server">Name</asp:Label>  
                                <div class="col-md-9">
                                    <asp:TextBox class="form-control form-control-sm" ID="txtName" runat="server" ToolTip="Enter product name"></asp:TextBox> 
                                </div>
                            </div>
                            <div class="row mb-2">
                                <asp:Label ID="label4" class="col-form-label col-md-3" runat="server">Category</asp:Label>  
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlCategory" runat="server" DataTextField="Name" DataValueField="Id" CssClass="form-control form-control-sm"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row mb-2">
                                <asp:Label ID="label5" class="col-form-label col-md-3" runat="server">Description</asp:Label>  
                                <div class="col-md-9">
                                    <asp:TextBox class="form-control form-control-sm" ID="txtDescription" runat="server" ToolTip="Enter description"></asp:TextBox> 
                                </div>
                            </div>
                            <div class="row mb-2">
                                <asp:Label ID="label6" class="col-form-label col-md-3" runat="server">Image</asp:Label>  
                                <div class="col-md-9">
                                    <asp:FileUpload ID="fileUpload1" runat="server" class="form-control form-control-sm" ClientIDMode="Static"/>  
                                </div>
                            </div>
                            <div class="row mb-2">
                                <asp:Label ID="label7" class="col-form-label col-md-3" runat="server">Price</asp:Label>  
                                <div class="col-md-9">
                                    <asp:TextBox class="form-control form-control-sm" ID="txtPrice" runat="server" ToolTip="Enter price"></asp:TextBox> 
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button class="btn btn-sm btn-primary w-auto" ID="btnSave" runat="server" Text="Save"
                                 OnClientClick="closeModal();" OnClick="btnSave_Click" />
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
