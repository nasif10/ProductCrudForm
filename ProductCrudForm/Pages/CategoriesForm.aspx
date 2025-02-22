<%@ Page Title="Categories" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CategoriesForm.aspx.cs" Inherits="ProductCrudForm.Pages.CategoriesForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function openModal() {
            $('#modalAddCategory').modal('toggle');
        }
        function closeModal() {
            $('#modalAddCategory').modal('hide');
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <section>
                <div class="mb-3">
                    <asp:LinkButton ID="btnAdd" runat="server" Text="Create New" OnClick="btnAdd_Click" /> 
                </div>
                <div class="row mb-2">
                    <asp:GridView runat="server" id="gvCategories"
                        AutoGenerateColumns="False"
                        CssClass="table table-striped table-hover w-auto"
                        Allowpaging="true" Pagesize="8"
                        OnRowEditing="gvCategories_RowEditing"
                        OnRowUpdating="gvCategories_RowUpdating"
                        OnRowCancelingEdit="gvCategories_RowCancelingEdit"
                        OnRowDeleting="gvCategories_RowDeleting"
                        OnPageIndexChanging="gvCategories_PageIndexChanging">
                        
                        <Columns>
                            <asp:TemplateField HeaderText="Sl.">  
                                <ItemTemplate>  
                                    <asp:Label ID="gvlblSl" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>  
                                </ItemTemplate>  
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Id" Visible="false">  
                                <ItemTemplate>  
                                    <asp:Label ID="gvlblId" runat="server" Text='<%#Eval("Id") %>'></asp:Label>  
                                </ItemTemplate>  
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Name">  
                                <ItemTemplate>  
                                    <asp:Label ID="gvlblName" runat="server" Text='<%#Eval("Name") %>'></asp:Label>  
                                </ItemTemplate>  
                                <EditItemTemplate>  
                                    <asp:TextBox ID="gvtxtName" runat="server" Text='<%#Eval("Name") %>' class="form-control form-control-sm"></asp:TextBox>  
                                </EditItemTemplate>  
                            </asp:TemplateField>
                            <asp:TemplateField>  
                                <ItemTemplate>  
                                    <asp:LinkButton ID="btn_Edit" runat="server" Text="" CommandName="Edit" >
                                        <i class="bi bi-pencil-square text-warning"></i>
                                    </asp:LinkButton>  
                                </ItemTemplate>  
                                <EditItemTemplate>  
                                    <asp:LinkButton ID="btn_Update" runat="server" Text="" CommandName="Update" CssClass="text-decoration-none me-1">
                                        <i class="bi bi-check-square text-success"></i>
                                    </asp:LinkButton> 
                                    <asp:LinkButton ID="btn_Cancel" runat="server" Text="" CommandName="Cancel" CssClass="text-decoration-none">
                                        <i class="bi bi-x-lg text-secondary"></i>
                                    </asp:LinkButton>
                                </EditItemTemplate>  
                            </asp:TemplateField>

                            <asp:TemplateField>  
                                <ItemTemplate>  
                                    <asp:LinkButton ID="btn_Delete" runat="server" Text="" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');">
                                        <i class="bi bi-trash text-danger"></i>
                                    </asp:LinkButton> 
                                </ItemTemplate>  
                            </asp:TemplateField>
                        </Columns>
                        <headerstyle backcolor="#e6f3f7"/>
                        <pagerstyle CssClass="pagerStyle" />
                    </asp:GridView>
                </div>
            </section>

            <!-- Modal -->
            <div class="modal fade" id="modalAddCategory" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h1 class="modal-title fs-5" id="exampleModalLabel">Add Category</h1>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <asp:Label ID="Label1" CssClass="col-form-label col-md-3" runat="server" Text="Name"></asp:Label>
                                <div class="col-md-9">
                                    <asp:TextBox CssClass="form-control" ID="txtName" runat="server" ToolTip="Enter category name"></asp:TextBox> 
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
    </asp:UpdatePanel>
</asp:Content>
