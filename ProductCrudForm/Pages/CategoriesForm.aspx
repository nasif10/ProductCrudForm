<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CategoriesForm.aspx.cs" Inherits="ProductCrudForm.Pages.CategoriesForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <asp:MultiView id="mvCategories" runat="Server" ActiveViewIndex="0">
            <asp:View id="View3" runat="Server">

                <section class="section1 pt-2">
                    <div class="mb-3">
                        <asp:LinkButton ID="btnAdd" runat="server" Text="Create New" OnClick="btnAdd_Click" /> 
                    </div>
                    <asp:GridView runat="server" id="gvCategories"
                        AutoGenerateColumns="False"
                        allowpaging="true"  
                        pagesize="7"
                        CssClass="table table-striped table-hover w-auto"
                        OnRowEditing="gvCategories_RowEditing"
                        OnRowUpdating="gvCategories_RowUpdating"
                        OnRowCancelingEdit="gvCategories_RowCancelingEdit"
                        OnRowDeleting="gvCategories_RowDeleting"
                        OnPageIndexChanging="gvCategories_PageIndexChanging">
                        <headerstyle backcolor="#e6f3f7"/>

                        <Columns>
                            <asp:TemplateField HeaderText="Id">  
                                <ItemTemplate>  
                                    <asp:Label ID="lbl_Id" runat="server" Text='<%#Eval("Id") %>'></asp:Label>  
                                </ItemTemplate>  
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Name">  
                                <ItemTemplate>  
                                    <asp:Label ID="lbl_Name" runat="server" Text='<%#Eval("Name") %>'></asp:Label>  
                                </ItemTemplate>  
                                <EditItemTemplate>  
                                    <asp:TextBox ID="txt_Name" runat="server" Text='<%#Eval("Name") %>' class="form-control form-control-sm"></asp:TextBox>  
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
                                    <asp:LinkButton ID="btn_Delete" runat="server" Text="" CommandName="Delete" >
                                        <i class="bi bi-trash text-danger"></i>
                                    </asp:LinkButton> 
                                </ItemTemplate>  
                                <EditItemTemplate>  
                                    <asp:LinkButton ID="btn_Delete" runat="server" Text="" CommandName="Delete">
                                        <i class="bi bi-trash text-danger"></i>
                                    </asp:LinkButton>  
                                </EditItemTemplate>  
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle CssClass="pagerStyle" />
                    </asp:GridView>
                </section>
            </asp:View> 

            <asp:View id="View4" runat="Server">
                <div class="col-md-4 mt-4">
                    <div class="mb-3">
                        <asp:Label ID="Label3" CssClass="form-label" runat="server" Text="Name"></asp:Label>  
                        <asp:TextBox CssClass="form-control" ID="txtName" runat="server" ToolTip="Enter category name"></asp:TextBox> 
                    </div>
                    <div class="mb-3">  
                        <asp:Button class="btn btn-sm btn-primary w-auto" ID="btnSubmit" runat="server" Text="Save" OnClick="btnSubmit_Click" />
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>

    </main>
</asp:Content>
