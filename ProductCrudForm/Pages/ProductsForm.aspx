<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductsForm.aspx.cs" Inherits="ProductCrudForm.Pages.ProductsForm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <main>
        <asp:MultiView id="mvProducts" runat="Server" ActiveViewIndex="0">
            <asp:View id="View1" runat="Server">
                <section class="section1">
                    <div class="row mb-2">
                        <div class="col-md-1">
                            <asp:Label id="Label1" Text="From" runat="server"/>
                            <asp:TextBox id="Date1" runat="server" CssClass="form-control form-control-sm" Width="100px"/>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="yyyy-MM-dd" TargetControlID="Date1"></asp:CalendarExtender>
                        </div>
                        <div class="col-md-1">
                           <asp:Label id="Label2" Text="To" runat="server"/>
                           <asp:TextBox id="Date2" runat="server" CssClass="form-control form-control-sm" Width="100px" />
                           <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="yyyy-MM-dd" TargetControlID="Date2"></asp:CalendarExtender>
                       </div>
                       <div class="col-md-2">
                           <div class="input-group">
                               <asp:TextBox id="SrcTxt" runat="server" CssClass="form-control form-control-sm" placeholder="Search" Style="margin-top: 23px;" />
                               <asp:LinkButton ID="btnOk" runat="server" Style="margin-top: 23px;" CssClass="btn btn-sm btn-primary" OnClick="btnOk_Click" TabIndex="2">Ok</asp:LinkButton>
                           </div>
                       </div>
                    </div>
                   
                </section>

                <section class="section2 pt-2">
                    <div class="col-md-2 mb-3">
                        <asp:LinkButton ID="btnAdd" runat="server" Text="Create New" OnClick="btnAdd_Click" /> 
                    </div>
                    <asp:GridView runat="server" id="gvProducts"
                        AutoGenerateColumns="False"
                        Allowpaging="true"  
                        Pagesize="5"
                        CssClass="table table-striped table-hover w-auto"
                        OnRowEditing="gvProducts_RowEditing"
                        OnRowUpdating="gvProducts_RowUpdating"
                        OnRowCancelingEdit="gvProducts_RowCancelingEdit"
                        OnRowDeleting="gvProducts_RowDeleting"
                        OnRowDataBound="gvProducts_RowDataBound"
                        OnPageIndexChanging="gvProducts_PageIndexChanging">
                        <headerstyle backcolor="#e6f3f7"/>

                        <Columns>
                            <asp:TemplateField HeaderText="Sl.">  
                                <ItemTemplate>  
                                    <asp:Label ID="lbl_sl" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>  
                                </ItemTemplate>  
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Id" Visible="false">  
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

                            <asp:TemplateField HeaderText="Category">  
                                <ItemTemplate>  
                                    <asp:Label ID="lbl_CategoryId" runat="server" Text='<%#Eval("Category") %>'></asp:Label>  
                                </ItemTemplate>  
                                <EditItemTemplate>  
                                    <asp:DropDownList ID="ddlCategoryId" runat="server" DataTextField="Name" DataValueField="Id" Width="100px" CssClass="form-control form-control-sm chosen-select"></asp:DropDownList>
                                </EditItemTemplate>  
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Description">  
                                <ItemTemplate>  
                                    <asp:Label ID="lbl_Description" runat="server" Text='<%#Eval("Description") %>'></asp:Label>  
                                </ItemTemplate>  
                                <EditItemTemplate>  
                                    <asp:TextBox ID="txt_Description" runat="server" Text='<%#Eval("Description") %>' class="form-control form-control-sm"></asp:TextBox>  
                                </EditItemTemplate>  
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Price">  
                                <ItemTemplate>  
                                    <asp:Label ID="lbl_Price" runat="server" Text='<%#Eval("Price") %>'></asp:Label>  
                                </ItemTemplate>  
                                <EditItemTemplate>  
                                    <asp:TextBox ID="txt_Price" runat="server" Text='<%#Eval("Price") %>' TextMode="Number" class="form-control form-control-sm"></asp:TextBox>  
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
                                    <asp:LinkButton ID="btn_Cancel" runat="server" Text="" CommandName="Cancel" CssClass="text-decoration-none" >
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
                                    <asp:LinkButton ID="btn_Delete" runat="server" Text="" CommandName="Delete" >
                                        <i class="bi bi-trash text-danger"></i>
                                    </asp:LinkButton>
                                </EditItemTemplate>  
                            </asp:TemplateField>

                        </Columns>
                        <pagerstyle CssClass="pagerStyle" />
                    </asp:GridView>
                </section>
            </asp:View> 

            <asp:View id="View2" runat="Server">
                <div class="col-md-6 mt-5">
                    <div class="row mb-3">
                        <asp:Label ID="Label3" class="col-form-label col-md-3" runat="server">Name</asp:Label>  
                        <div class="col-md-9">
                            <asp:TextBox class="form-control" ID="tbName" runat="server" ToolTip="Enter product name"></asp:TextBox> 
                        </div>
                    </div>
                    <div class="row mb-3">
                        <asp:Label ID="label4" class="col-form-label col-md-3" runat="server">Category</asp:Label>  
                        <div class="col-md-9">
                            <asp:DropDownList ID="ddlCategory" runat="server" DataTextField="Name" DataValueField="Id" CssClass="form-control chosen-select"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <asp:Label ID="label5" class="col-form-label col-md-3" runat="server">Description</asp:Label>  
                        <div class="col-md-9">
                            <asp:TextBox class="form-control" ID="tbDescription" runat="server" ToolTip="Enter description"></asp:TextBox> 
                        </div>
                    </div>
                    <div class="row mb-3">
                        <asp:Label ID="label6" class="col-form-label col-md-3" runat="server">Price</asp:Label>  
                        <div class="col-md-9">
                            <asp:TextBox class="form-control" ID="tbPrice" runat="server" ToolTip="Enter price"></asp:TextBox> 
                        </div>
                    </div>

                    <div class="row mb-3">  
                        <div class="offset-md-3 col-md-9">
                            <asp:Button class="btn btn-sm btn-primary w-auto" ID="btnSubmit" runat="server" Text="Save" OnClick="btnSubmit_Click" />
                        </div>
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>

    </main>
</asp:Content>
