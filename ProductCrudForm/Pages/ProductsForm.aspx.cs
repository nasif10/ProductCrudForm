using Microsoft.Reporting.WebForms;
using ProductCrudForm.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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

        protected void Page_PreInit(object sender, EventArgs e)
        {
            MasterControl();
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

                    string query1 = "select p.*, c.Name as Category from Products p INNER JOIN Categories c ON p.CategoryId = c.Id";
                    if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                    {
                        query1 += " WHERE p.createdat BETWEEN @Date1 AND @Date2";
                    }
                    if (!string.IsNullOrEmpty(search))
                    {
                        query1 += " AND p.Name LIKE @Search";
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter();

                    SqlCommand command = new SqlCommand(query1, connection);

                    if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                    {
                        command.Parameters.AddWithValue("@Date1", fromDate);
                        command.Parameters.AddWithValue("@Date2", toDate);
                    }

                    if (!string.IsNullOrEmpty(search))
                    {
                        command.Parameters.AddWithValue("@Search", "%" + search + "%");
                    }

                    adapter.SelectCommand = command;
                    DataTable dt1 = new DataTable();
                    adapter.Fill(dt1);

                    if (dt1 != null)
                    {
                        ViewState["dtProducts"] = dt1;
                    }

                    string query2 = "Select * from Categories";

                    SqlCommand command2 = new SqlCommand(query2, connection);
                    adapter.SelectCommand = command2;

                    DataTable dt2 = new DataTable();
                    adapter.Fill(dt2);

                    if (dt2 != null)
                    {
                        ViewState["dtCategories"] = dt2;
                    }

                    Data_Bind();
                }
            }
            catch (Exception ex) 
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + HttpUtility.JavaScriptStringEncode(ex.Message) + "', 'error');", true);
            }
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "openModal();", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var name = txtName.Text;
                var categoryId = ddlCategory.SelectedValue;
                var description = txtDescription.Text;
                var price = Convert.ToDouble(txtPrice.Text);

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Products (name, categoryid, description, price, createdat) VALUES (@Name, @CategoryId, @Description, @Price, @CreatedAt);";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    int affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows > 0)
                    {
                        LoadData();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Notification", "Toast('Success','Record created successfully!', 'success');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', 'Record could not be created!', 'error');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + HttpUtility.JavaScriptStringEncode(ex.Message) + "', 'error');", true);
            }
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
                    int id = Convert.ToInt32(((Label)row.FindControl("gvlblId")).Text);
                    string name = ((TextBox)row.FindControl("gvtxtName")).Text;
                    string categoryId = ((DropDownList)row.FindControl("gvddlCategory")).SelectedValue;
                    string description = ((TextBox)row.FindControl("gvtxtDescription")).Text;
                    string price = ((TextBox)row.FindControl("gvtxtPrice")).Text;

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
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + HttpUtility.JavaScriptStringEncode(ex.Message) + "', 'error');", true);
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
                    int id = Convert.ToInt32(((Label)row.FindControl("gvlblId")).Text);
                    string query = "DELETE FROM Products WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", id);
                    int affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows > 0)
                        ScriptManager.RegisterStartupScript(this, GetType(), "Notification", "Toast('Success','Record deleted successfully!', 'success');", true);
                    else
                        ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', 'Record not found or could not be deleted!', 'error');", true);

                    DataTable dt = (DataTable)ViewState["dtProducts"];
                    DataRow dr;
                    dr = dt.Select($"id = {id}")[0];
                    dr.Delete();
                    dt.AcceptChanges();
                    ViewState["dtProducts"] = dt;
                    Data_Bind();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + HttpUtility.JavaScriptStringEncode(ex.Message) + "', 'error');", true);
            }
        }

        protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if(gvProducts.EditIndex == e.Row.RowIndex)
                    {
                        DropDownList ddlCategoryId = (DropDownList)e.Row.FindControl("gvddlCategory");
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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + HttpUtility.JavaScriptStringEncode(ex.Message) + "', 'error');", true);
            }
        }

        protected void gvProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProducts.PageIndex = e.NewPageIndex;
            Data_Bind();
        }

        protected void MasterControl()
        {
            ((LinkButton)this.Master.FindControl("lbtnPrint")).Click += new EventHandler(lnkPrint_Click);
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["dtProducts"];
                List<Product> list = dt.DtToList<Product>();
                LocalReport localReport = new LocalReport();

                string imageUrl = new Uri(Server.MapPath("~/Images/pcf.png")).AbsoluteUri;
                localReport.EnableExternalImages = true;
                localReport.ReportPath = Server.MapPath("~/Reports/RptProList.rdlc");
                localReport.DataSources.Clear();
                ReportDataSource source = new ReportDataSource("DataSet1", list);
                localReport.DataSources.Add(source);
                localReport.SetParameters(new ReportParameter("footer", "Print Time: " + DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss tt")));
                localReport.SetParameters(new ReportParameter("logourl", imageUrl));

                Session["PrintData"] = localReport;
                Response.Redirect("~/Reports/RptViewer.aspx?type=pdf");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Warning', '" + HttpUtility.JavaScriptStringEncode(ex.Message) + "', 'warning');", true);
            }
        }

        protected void gvlbtnProInfoPrint_Click(object sender, EventArgs e)
        {
            int index = ((GridViewRow)((LinkButton)sender).NamingContainer).RowIndex;
            string id = ((Label)gvProducts.Rows[index].FindControl("gvlblId")).Text.ToString();

            DataTable dt = (DataTable)ViewState["dtProducts"];
            DataRow dr = dt.Select($"Id={id}")[0];
            LocalReport localReport = new LocalReport();

            string imageUrl = new Uri(Server.MapPath("~/Images/pcf.png")).AbsoluteUri;
            localReport.EnableExternalImages = true;
            localReport.ReportPath = Server.MapPath("~/Reports/RptProInfo.rdlc");

            localReport.SetParameters(new ReportParameter("name", dr["Name"].ToString()));
            localReport.SetParameters(new ReportParameter("category", dr["Category"].ToString()));
            localReport.SetParameters(new ReportParameter("description", dr["Description"].ToString()));
            localReport.SetParameters(new ReportParameter("price", dr["Price"].ToString()));
            localReport.SetParameters(new ReportParameter("logourl", imageUrl));
            localReport.SetParameters(new ReportParameter("footer", "Print Time: " + DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss tt")));
            
            Session["PrintData"] = localReport;
            Response.Redirect("~/Reports/RptViewer.aspx?type=pdf");
        }

        protected void hlinkExcel_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 3; i++)
            {
                gvProducts.Columns[gvProducts.Columns.Count - i].Visible = false;
            }

            Session["PrintData"] = gvProducts;
            Response.Redirect("~/Reports/RptViewer.aspx?type=excel");
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

    }
}