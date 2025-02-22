using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using ProductCrudForm.DbService;
using ProductCrudForm.Models;

namespace ProductCrudForm.Pages
{
    public partial class ProductsForm : System.Web.UI.Page
    {
        private readonly DbAccess _dbAccess = new DbAccess();

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
                DataSet ds1 = _dbAccess.GetResultSet("sp_product", "getproducts");
                ViewState["dtProducts"] = ds1.Tables[0];

                DataSet ds2 = _dbAccess.GetResultSet("sp_category", "getcategories");
                ViewState["dtCategories"] = ds2.Tables[0];
                ddlCategory.DataSource = ds2.Tables[0];
                ddlCategory.DataBind();

                Data_Bind();
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
                DataTable dt = (DataTable)ViewState["dtProducts"];
                DataView dv = dt.DefaultView;

                string date1 = Date1.Text.Trim();
                string date2 = Date2.Text.Trim();
                string search = txtSearch.Text.Trim();

                if (!string.IsNullOrEmpty(date1) && !string.IsNullOrEmpty(date2))
                {
                    dv.RowFilter = "createdat >= '" + date1 + " 00:00:00" + "' AND createdat <= '" + date2 + " 23:59:59" + "'";
                }
                if (!string.IsNullOrEmpty(search))
                {
                    dv.RowFilter = $"name like '%{search}%'";
                }

                ViewState["dtProducts1"] = null;
                ViewState["dtProducts1"] = dv.ToTable();

                gvProducts.DataSource = dv.ToTable();
                gvProducts.DataBind();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            Data_Bind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "openModal();", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtName.Text;
                string categoryId = ddlCategory.SelectedValue;
                string description = txtDescription.Text;
                string price = string.IsNullOrEmpty(txtPrice.Text) ? "0" : txtPrice.Text;

                string fileName = "";

                if (fileUpload1.HasFile)
                {
                    string fileExtension = Path.GetExtension(fileUpload1.PostedFile.FileName).ToLower();
                    if (fileExtension != ".jpg" && fileExtension != ".png") 
                    {
                        throw new Exception("Only JPG and PNG files are allowed.");
                    }
                    if (fileUpload1.PostedFile.ContentLength > 524288)
                    {
                        throw new Exception("The file size should be less than 512KB.");
                    }

                    fileName = DateTime.Now.ToString("yyMMddHHmmss") + "_" + Path.GetFileName(fileUpload1.PostedFile.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    fileUpload1.SaveAs(filePath);
                }

                bool isInserted = _dbAccess.ExecuteCommand("sp_product", "insertproduct", name, categoryId, description, fileName, price);

                if (isInserted)
                {
                    LoadData();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Notification", "Toast('Success','Record created successfully!', 'success');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', 'Record could not be created!', 'error');", true);
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
                    if (gvProducts.EditIndex == e.Row.RowIndex)
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

        protected void gvProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvProducts.EditIndex = e.NewEditIndex;
            Data_Bind();
        }

        protected void gvProducts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvProducts.Rows[e.RowIndex];

                int id = Convert.ToInt32(((Label)row.FindControl("gvlblId")).Text);
                string name = ((TextBox)row.FindControl("gvtxtName")).Text;
                string categoryId = ((DropDownList)row.FindControl("gvddlCategory")).SelectedValue;
                string description = ((TextBox)row.FindControl("gvtxtDescription")).Text;
                string image = ((Label)row.FindControl("gvlblImage")).Text;
                string price = ((TextBox)row.FindControl("gvtxtPrice")).Text;

                bool isUpdated = _dbAccess.ExecuteCommand("sp_product", "updateproduct", id, name, categoryId, description, image, price);

                if (isUpdated)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Notification", "Toast('Success','Record updated successfully!', 'success');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', 'Record not found or could not be updated!', 'error');", true);
                }

                gvProducts.EditIndex = -1;
                LoadData();
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
                GridViewRow row = gvProducts.Rows[e.RowIndex];
                int id = Convert.ToInt32(((Label)row.FindControl("gvlblId")).Text);

                bool isDeleted = _dbAccess.ExecuteCommand("sp_product", "deleteproduct", id);

                if (isDeleted)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Notification", "Toast('Success','Record deleted successfully!', 'success');", true);
                    
                    DataTable dt = (DataTable)ViewState["dtProducts"];
                    DataRow dr;
                    dr = dt.Select($"id = {id}")[0];
                    dr.Delete();
                    dt.AcceptChanges();
                    ViewState["dtProducts"] = dt;
                    Data_Bind();
                }
                else
                    ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', 'Record not found or could not be deleted!', 'error');", true);
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

        protected void gvlbtnProInfoPrint_Click(object sender, EventArgs e)
        {
            try
            {
                int index = ((GridViewRow)((LinkButton)sender).NamingContainer).RowIndex;
                string id = ((Label)gvProducts.Rows[index].FindControl("gvlblId")).Text.ToString();

                DataTable dt = (DataTable)ViewState["dtProducts"];
                DataRow dr = dt.Select($"Id={id}")[0];
                LocalReport localReport = new LocalReport();

                string logoUrl = new Uri(Server.MapPath("~/Images/pcf.png")).AbsoluteUri;
                string imageUrl = string.IsNullOrEmpty(dr["Image"].ToString()) ? "" : new Uri(Server.MapPath("~/Images/"+ dr["Image"].ToString())).AbsoluteUri;
                localReport.EnableExternalImages = true;
                localReport.ReportPath = Server.MapPath("~/Reports/RptProInfo.rdlc");

                localReport.SetParameters(new ReportParameter("name", dr["Name"].ToString()));
                localReport.SetParameters(new ReportParameter("category", dr["CategoryName"].ToString()));
                localReport.SetParameters(new ReportParameter("description", dr["Description"].ToString()));
                localReport.SetParameters(new ReportParameter("image", imageUrl));
                localReport.SetParameters(new ReportParameter("price", dr["Price"].ToString()));
                localReport.SetParameters(new ReportParameter("logourl", logoUrl));
                localReport.SetParameters(new ReportParameter("footer", "Print Time: " + DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss tt")));

                Session["PrintData"] = localReport;
                ScriptManager.RegisterStartupScript(this, GetType(), "OpenReport", "window.open('/Reports/RptViewer.aspx?type=pdf', '_blank');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + HttpUtility.JavaScriptStringEncode(ex.Message) + "', 'error');", true);
            }
        }

        protected void lbtnProListPrint_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["dtProducts1"];
                List<Product> list = dt.DataTableToList<Product>();
                LocalReport localReport = new LocalReport();
                string date1 = Convert.ToDateTime(Date1.Text).ToString("dd-MMM-yyyy");
                string date2 = Convert.ToDateTime(Date2.Text).ToString("dd-MMM-yyyy");

                string imageUrl = new Uri(Server.MapPath("~/Images/pcf.png")).AbsoluteUri;
                localReport.EnableExternalImages = true;
                localReport.ReportPath = Server.MapPath("~/Reports/RptProList.rdlc");
                localReport.DataSources.Clear();
                ReportDataSource source = new ReportDataSource("DataSet1", list);
                localReport.DataSources.Add(source);
                localReport.SetParameters(new ReportParameter("fromdate", date1));
                localReport.SetParameters(new ReportParameter("todate", date2));
                localReport.SetParameters(new ReportParameter("logourl", imageUrl));
                localReport.SetParameters(new ReportParameter("footer", "Print Time: " + DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss tt")));
                
                Session["PrintData"] = localReport;
                ScriptManager.RegisterStartupScript(this, GetType(), "OpenReport", "window.open('/Reports/RptViewer.aspx?type=pdf', '_blank');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + HttpUtility.JavaScriptStringEncode(ex.Message) + "', 'error');", true);
            }
        }

        protected void lbtnProListExcel_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 1; i <= 3; i++)
                {
                    gvProducts.Columns[gvProducts.Columns.Count - i].Visible = false;
                }

                Session["PrintData"] = gvProducts;
                Response.Redirect("~/Reports/RptViewer.aspx?type=excel");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + HttpUtility.JavaScriptStringEncode(ex.Message) + "', 'error');", true);
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}