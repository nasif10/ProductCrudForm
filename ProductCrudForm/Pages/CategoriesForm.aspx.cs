using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProductCrudForm.Pages
{
    public partial class CategoriesForm : System.Web.UI.Page
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        protected void LoadData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "Select * from Categories";
                    DataTable dt = GetData(query, connection);

                    if (dt.Rows.Count > 0)
                    {
                        ViewState["dtCategories"] = dt;
                    }
                    Data_Bind();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + ex.Message + "', 'error');", true);
            }
        }

        protected DataTable GetData(string query, SqlConnection connection)
        {
            SqlDataAdapter sda = new SqlDataAdapter(query, connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        private void Data_Bind()
        {
            if (ViewState["dtCategories"] != null)
            {
                gvCategories.DataSource = (DataTable)ViewState["dtCategories"];
                gvCategories.DataBind();
            }
        }

        protected void gvCategories_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCategories.EditIndex = e.NewEditIndex;
            Data_Bind();
        }

        protected void gvCategories_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    GridViewRow row = gvCategories.Rows[e.RowIndex];
                    int id = Convert.ToInt32(((Label)row.FindControl("lbl_Id")).Text);
                    string name = ((TextBox)row.FindControl("txt_Name")).Text;

                    string query = "UPDATE Categories SET Name = @Name WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Id", id);
                    int affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows > 0)
                        ScriptManager.RegisterStartupScript(this, GetType(), "Notification", "Toast('Success','Record updated successfully!', 'success');", true);
                    else
                        ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', 'Record not found or could not be updated!', 'error');", true);

                    gvCategories.EditIndex = -1;
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + ex.Message + "', 'error');", true);
            }
        }

        protected void gvCategories_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCategories.EditIndex = -1;
            Data_Bind();
        }

        protected void gvCategories_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    GridViewRow row = gvCategories.Rows[e.RowIndex];
                    int id = Convert.ToInt32(((Label)row.FindControl("lbl_Id")).Text);
                    string query = "DELETE FROM Categories WHERE Id = @Id";
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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + ex.Message + "', 'error');", true);
            }
        }

        protected void gvCategories_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCategories.PageIndex = e.NewPageIndex;
            Data_Bind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            mvCategories.ActiveViewIndex = 1;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var name = txtName.Text;
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Categories (Name) VALUES (@Name);";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Name", name);
                    int affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows > 0)
                    {
                        LoadData();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Notification", "Toast('Success','Record created successfully!', 'success');", true);
                        mvCategories.ActiveViewIndex = 0;
                    }
                    else
                        ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', 'Record could not be created!', 'error');", true);
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + ex.Message.Replace("'", "\\'").Replace(Environment.NewLine, "\\n") + "', 'error');", true);
            }
        }
    }
}