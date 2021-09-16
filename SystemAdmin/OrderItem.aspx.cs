using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProject_Chains.SystemAdmin
{
    public partial class OrderItem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string keyID = HttpContext.Current.Session["KeyID"] as string;
            string OrderNo = Request.QueryString["OrderNo"];
            string apiIP = System.Configuration.ConfigurationManager.AppSettings["ip"];
            string apiPort = System.Configuration.ConfigurationManager.AppSettings["port"];
            string url = $"http://{apiIP}:{apiPort}/Handlers/SearchOrderItemList.ashx";

            string rqString = "{\"OrderNo\":\"" + OrderNo + "\"}";
            JObject json = JObject.Parse(rqString);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "Applicaiton/JSON";
            request.Headers["KeyID"] = keyID;

            try
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                string jsonStr = reader.ReadToEnd();
                                //jsonStr = "[" + jsonStr + "]";
                                var table = JsonConvert.DeserializeObject<DataTable>(jsonStr);
                                gvOrderItem.DataSource = table;
                                gvOrderItem.DataBind();
                            }
                        }
                    }
                }
                catch (WebException wexp)
                {
                    HttpWebResponse response = (HttpWebResponse)wexp.Response;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid KeyID. Please login again.');window.location ='/Default.aspx';", true);
                        return;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Server Error. Please contact system contact person.');window.location ='/Default.aspx';", true);
                        return;
                    }
                }
            }
            catch (Exception err)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Server Error. Please contact system contact person.');window.location ='/Default.aspx';", true);
                return;
            }
        }

        protected void gvOrderItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[5].Text = (e.Row.Cells[5].Text).Replace("/r/n", "<br />");

                string orderStatus = e.Row.Cells[6].Text;
                switch (orderStatus)
                {
                    case "Y":
                        e.Row.Cells[6].Text = "有庫存";
                        break;
                    case "N":
                        e.Row.Cells[6].Text = "無庫存";
                        break;
                    default:
                        e.Row.Cells[6].Text = "尚未確認";
                        break;
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["KeyID"] = null;
            Response.Redirect("/Default.aspx");
        }
    }
}