using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProductCrudForm.Pages
{
    public partial class ProductsForm : System.Web.UI.Page
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Date1.Text = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
                Date2.Text = DateTime.Today.ToString("yyyy-MM-dd");
                LoadData();
            }
        }

        protected void LoadData()
        {
            try
            {
                string fromDate = Date1.Text;
                string toDate = DateTime.Parse(Date2.Text).AddDays(1).ToString();
                string search = SrcTxt.Text;

                ViewState["dtProducts"] = null;
                ViewState["dtCategories"] = null;

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query1 = "Select p.*, c.Name as Category from Products p INNER JOIN Categories c ON p.CategoryId = c.Id";
                    if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                    {
                        query1 += " WHERE p.Created BETWEEN @Date1 AND @Date2";
                    }
                    if (!string.IsNullOrEmpty(search))
                    {
                        query1 += " AND p.Name LIKE @Search";
                    }

                    DataTable dt1 = GetData(query1, connection, fromDate, toDate, search);
                    if (dt1.Rows.Count > 0)
                    {
                        ViewState["dtProducts"] = dt1;
                    }

                    string query2 = "Select * from Categories";
                    DataTable dt2 = GetData(query2, connection, "", "", "");
                    if (dt2.Rows.Count > 0)
                    {
                        ViewState["dtCategories"] = dt2;
                    }
                    Data_Bind();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + ex.Message + "', 'error');", true);
            }
        }

        protected DataTable GetData(string query, SqlConnection connection, string fromDate, string toDate, string search)
        {
            SqlDataAdapter sda = new SqlDataAdapter(query, connection);
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                sda.SelectCommand.Parameters.AddWithValue("@Date1", fromDate);
                sda.SelectCommand.Parameters.AddWithValue("@Date2", toDate);
            }
            if (!string.IsNullOrEmpty(search))
            {
                sda.SelectCommand.Parameters.AddWithValue("@Search", "%" + search + "%");
            }
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        private void Data_Bind()
        {
            if (ViewState["dtProducts"] != null)
            {
                gvProducts.DataSource = (DataTable)ViewState["dtProducts"];
                gvProducts.DataBind();
            }

            if (ViewState["dtCategories"] != null)
            {
                ddlCategory.DataSource = (DataTable)ViewState["dtCategories"];
                ddlCategory.DataBind();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void gvProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvProducts.EditIndex = e.NewEditIndex;
            Data_Bind();
        }

        protected void gvProducts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    GridViewRow row = gvProducts.Rows[e.RowIndex];
                    int id = Convert.ToInt32(((Label)row.FindControl("lbl_Id")).Text);
                    string name = ((TextBox)row.FindControl("txt_Name")).Text;
                    string categoryId = ((DropDownList)row.FindControl("ddlCategoryId")).SelectedValue;
                    string description = ((TextBox)row.FindControl("txt_Description")).Text;
                    string price = ((TextBox)row.FindControl("txt_Price")).Text;

                    string query = "UPDATE Products SET Name = @Name, CategoryId = @CategoryId, Description = @Description, Price = @Price WHERE Id = @Id";
 
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Id", id);
                    int affectedRows = cmd.ExecuteNonQuery();

                    if (affectedRows > 0)
                        ScriptManager.RegisterStartupScript(this, GetType(), "Notification", "Toast('Success','Record updated successfully!', 'success');", true);
                    else
                        ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', 'Record not found or could not be updated!', 'error');", true);

                    gvProducts.EditIndex = -1;
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + ex.Message + "', 'error');", true);
            }
        }

        protected void gvProducts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvProducts.EditIndex = -1;
            Data_Bind();
        }

        protected void gvProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    GridViewRow row = gvProducts.Rows[e.RowIndex];
                    int id = Convert.ToInt32(((Label)row.FindControl("lbl_Id")).Text);
                    string query = "DELETE FROM Products WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", id);
                    int affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows > 0)
                        ScriptManager.RegisterStartupScript(this, GetType(), "Notification", "Toast('Success','Record deleted successfully!', 'success');", true);
                    else
                        ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', 'Record not found or could not be deleted!', 'error');", true);
                    
                    LoadData();
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + ex.Message + "', 'error');", true);
            }
        }

        protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && gvProducts.EditIndex == e.Row.RowIndex)
                    {
                        DropDownList ddlCategoryId = (DropDownList)e.Row.FindControl("ddlCategoryId");
                        if (ddlCategoryId != null)
                        {
                            DataTable dt = (DataTable)ViewState["dtCategories"];

                            if (dt.Rows.Count > 0)
                            {
                                ddlCategoryId.DataSource = dt;
                                ddlCategoryId.DataTextField = "Name";
                                ddlCategoryId.DataValueField = "Id";
                                ddlCategoryId.DataBind();

                                ddlCategoryId.SelectedValue = DataBinder.Eval(e.Row.DataItem, "CategoryId").ToString();
                            }
                        }
                    }
                }
            }
            catch( Exception ex )
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + ex.Message + "', 'error');", true);
            }
        }

        protected void gvProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProducts.PageIndex = e.NewPageIndex;
            Data_Bind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            mvProducts.ActiveViewIndex = 1;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var name = tbName.Text;
                var categoryId = ddlCategory.SelectedValue;
                var description = tbDescription.Text;
                var price = tbPrice.Text;

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Products (Name, CategoryId, Description, Price, Created) VALUES (@Name, @CategoryId, @Description, @Price, @Created);";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Created", DateTime.Now);
                    int affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows > 0)
                    {
                        LoadData();

                        ScriptManager.RegisterStartupScript(this, GetType(), "Notification", "Toast('Success','Record created successfully!', 'success');", true);
                        mvProducts.ActiveViewIndex = 0;
                    }
                    else
                        ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', 'Record could not be created!', 'error');", true);

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + ex.Message + "', 'error');", true);
            }
        }


    }
}