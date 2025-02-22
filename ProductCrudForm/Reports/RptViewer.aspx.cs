using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace ProductCrudForm.Reports
{
    public partial class RptViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Request.QueryString["type"].ToString();
            switch (type)
            {
                case "pdf":
                    PDF();
                    break;
                case "excel":
                    Excel();
                    break;
            }
        }

        protected void PDF()
        {
            LocalReport localReport = new LocalReport();
            localReport = (LocalReport)Session["PrintData"];
            var deviceInfo = @"<DeviceInfo>
                                <EmbedFonts>None</EmbedFonts>
                               </DeviceInfo>";
            Warning[] warnings;
            string[] streamIds;
            string mimeType, encoding, extension;
            byte[] bytes = localReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", $"inline; filename=PCF_{DateTime.Now.ToString("yyMMddHHmmss")}.pdf");
            Response.BinaryWrite(bytes);
            Session.Remove("PrintData");
            Response.Flush();
            Response.End();
        }


        protected void Excel()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", $"attachment;filename=PCF_{DateTime.Now.ToString("yyMMddHHmmss")}.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            GridView gv = (GridView)Session["PrintData"];

            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Session.Remove("PrintData");
            Response.Flush();
            Response.End();
        }
    }
}