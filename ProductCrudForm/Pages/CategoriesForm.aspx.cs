using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProductCrudForm.DbService;

namespace ProductCrudForm.Pages
{
    public partial class CategoriesForm : System.Web.UI.Page
    {
        private readonly DbAccess _dbAccess = new DbAccess();
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
                DataSet ds1 = _dbAccess.GetResultSet("sp_category", "getcategories");
                ViewState["dtCategories"] = ds1.Tables[0];
                Data_Bind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + HttpUtility.JavaScriptStringEncode(ex.Message) + "', 'error');", true);
            }
        }

        private void Data_Bind()
        {
            if (ViewState["dtCategories"] != null)
            {
                gvCategories.DataSource = (DataTable)ViewState["dtCategories"];
                gvCategories.DataBind();
            }
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
                bool isInserted = _dbAccess.ExecuteCommand("sp_category", "insertcategory", name);

                if (isInserted)
                {
                    LoadData();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Notification", "Toast('Success','Record created successfully!', 'success');", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', 'Record could not be created!', 'error');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + HttpUtility.JavaScriptStringEncode(ex.Message) + "', 'error');", true);
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
                GridViewRow row = gvCategories.Rows[e.RowIndex];
                int id = Convert.ToInt32(((Label)row.FindControl("gvlblId")).Text);
                string name = ((TextBox)row.FindControl("gvtxtName")).Text;

                bool isUpdated = _dbAccess.ExecuteCommand("sp_category", "updatecategory", id, name);

                if (isUpdated)
                {
                    gvCategories.EditIndex = -1;
                    LoadData();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Notification", "Toast('Success','Record updated successfully!', 'success');", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', 'Record not found or could not be updated!', 'error');", true);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + HttpUtility.JavaScriptStringEncode(ex.Message) + "', 'error');", true);
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
                GridViewRow row = gvCategories.Rows[e.RowIndex];
                int id = Convert.ToInt32(((Label)row.FindControl("gvlblId")).Text);

                bool isDeleted = _dbAccess.ExecuteCommand("sp_category", "deletecategory", id);

                if (isDeleted)
                {
                    LoadData();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Notification", "Toast('Success','Record deleted successfully!', 'success');", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', 'Record not found or could not be deleted!', 'error');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Notifications", "Toast('Error', '" + HttpUtility.JavaScriptStringEncode(ex.Message) + "', 'error');", true);
            }
        }

        protected void gvCategories_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCategories.PageIndex = e.NewPageIndex;
            Data_Bind();
        }

    }
}