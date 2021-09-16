using FinalProject_Chains.Logs;
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
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 建立 查詢多筆訂單API之所需資訊
            string apiIP = System.Configuration.ConfigurationManager.AppSettings["ip"];
            string apiPort = System.Configuration.ConfigurationManager.AppSettings["port"];
            string url = $"http://{apiIP}:{apiPort}/Handlers/SearchOrderList.ashx";
            string keyID = HttpContext.Current.Session["KeyID"] as string;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Headers["KeyID"] = keyID;

            try
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write("");
                }
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    // Http Status 200 OK 則顯示名下所有訂單於GridView
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                string jsonStr = reader.ReadToEnd();
                                var table = JsonConvert.DeserializeObject<DataTable>(jsonStr);
                                this.gvOrderInfo.DataSource = table;
                                if (!IsPostBack)
                                    this.gvOrderInfo.DataBind();
                            }
                        }
                    }
                }
                catch (WebException wexp)
                {
                    Logger.WriteLog(wexp);
                    HttpWebResponse response = (HttpWebResponse)wexp.Response;
                    // Http Status 401 Unauthorized 則顯示KeyID 失效，並跳轉至登入頁面
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid KeyID. Please login again.');window.location ='/Default.aspx';", true);
                        return;
                    }
                    // 其他Http Exception 則顯示錯誤，並跳轉至登入頁面
                    else
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                var temp = reader.ReadToEnd();
                                ErrModel errContent = JsonConvert.DeserializeObject<ErrModel>(temp);
                                String errMsg = errContent.ErrorMessage;
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errMsg + "');window.location ='/Default.aspx';", true);
                                return;
                            }
                        }
                    }
                }
            }
            // 發生未預期的Exception則顯示Server Error並跳轉至登入頁面
            catch (Exception err)
            {
                Logger.WriteLog(err);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Server Error. Please contact system contact person.');window.location ='/Default.aspx';", true);
                return;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Check Input
            if (string.IsNullOrWhiteSpace(this.txtOrderNo.Text))
            {
                this.ltlErrMsg.Text = $"<p>訂單編號不得為空</p>";
                return;
            }

            // 建立 查詢單筆訂單API之所需資訊
            string orderNo = this.txtOrderNo.Text;
            string apiIP = System.Configuration.ConfigurationManager.AppSettings["ip"];
            string apiPort = System.Configuration.ConfigurationManager.AppSettings["port"];
            string url = $"http://{apiIP}:{apiPort}/Handlers/SearchOrder.ashx";
            string keyID = HttpContext.Current.Session["KeyID"] as string;
            
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Headers["KeyID"] = keyID;

                string rqString = "{\"OrderNo\":\"" + orderNo + "\"}";
                JObject json = JObject.Parse(rqString);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    // Http Status 200 OK 則顯示名下所有訂單於GridView
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                string jsonStr = reader.ReadToEnd();
                                jsonStr = "[" + jsonStr + "]";
                                var table = JsonConvert.DeserializeObject<DataTable>(jsonStr);
                                gvOrderInfo.DataSource = table;
                                gvOrderInfo.DataBind();
                            }
                        }
                    }
                }
                catch (WebException wexp)
                {
                    Logger.WriteLog(wexp);
                    HttpWebResponse response = (HttpWebResponse)wexp.Response;
                    // Http Status 401 Unauthorized
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                var temp = reader.ReadToEnd();
                                ErrModel errContent = JsonConvert.DeserializeObject<ErrModel>(temp);
                                String errMsg = errContent.ErrorMessage;
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errMsg + "');", true);
                                return;
                            }
                        }
                    }
                    // 其他Http Exception
                    else
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                var temp = reader.ReadToEnd();
                                ErrModel errContent = JsonConvert.DeserializeObject<ErrModel>(temp);
                                String errMsg = errContent.ErrorMessage;
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + errMsg + "');", true);
                                return;
                            }
                        }
                    }
                }
            }
            // 發生未預期的Exception則顯示Server Error
            catch (Exception err)
            {
                Logger.WriteLog(err);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Server Error. Please contact system contact person.');", true);
                return;
            }
        }

        protected void gvOrderInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var row = e.Row;
                e.Row.Cells[5].Text = (e.Row.Cells[5].Text).Replace("/r/n/r/n", "/r/n");
                e.Row.Cells[5].Text = (e.Row.Cells[5].Text).Replace("/r/n", "<br />");
                string orderStatus = e.Row.Cells[6].Text;
                switch (orderStatus)
                {
                    case "1":
                        e.Row.Cells[6].Text = "已完成";
                        break;
                    case "2":
                        e.Row.Cells[6].Text = "已下單、待廠商確認";
                        e.Row.FindControl("btnCancel").Visible = true;
                        break;
                    case "3":
                        e.Row.Cells[6].Text = "廠商已確認、尚未出貨";
                        break;
                    case "4":
                        e.Row.Cells[6].Text = "廠商已確認、運輸途中";
                        e.Row.FindControl("btnComplete").Visible = true;
                        break;
                    case "5":
                        e.Row.Cells[6].Text = "已取消";
                        break;
                    default:
                        e.Row.Cells[6].Text = "狀態未明";
                        break;
                }
            }
        }

        protected void gvOrderInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string errMsg = string.Empty;
            if (e.CommandName == "OrderDetail")
            {
                //取得點擊的索引值
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //取得點擊的該列
                GridViewRow row = gvOrderInfo.Rows[rowIndex];
                
                //取得資訊
                string orderNo = row.Cells[0].Text;
                Response.Redirect($"/SystemAdmin/OrderItem.aspx?OrderNo={orderNo}");
            }

            else if (e.CommandName == "CancelOrder")
            {
                //取得點擊的索引值
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //取得點擊的該列
                GridViewRow row = gvOrderInfo.Rows[rowIndex];

                //取得資訊
                string orderNo = row.Cells[0].Text;
                string keyID = HttpContext.Current.Session["KeyID"] as string;

                if (ModifyOrder(keyID, orderNo, "5", out errMsg))
                {
                    Response.Redirect(Request.Url.ToString());
                }
                else
                {
                    this.ltlErrMsg.Text = $"<p>Exception {errMsg}</p>";
                    return;
                }

            }

            else if (e.CommandName == "CompleteOrder")
            {
                //取得點擊的索引值
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //取得點擊的該列
                GridViewRow row = gvOrderInfo.Rows[rowIndex];

                //取得資訊
                string orderNo = row.Cells[0].Text;
                string keyID = HttpContext.Current.Session["KeyID"] as string;

                if (ModifyOrder(keyID, orderNo, "1", out errMsg))
                {
                    Response.Redirect(Request.Url.ToString());
                }
                else
                {
                    this.ltlErrMsg.Text = $"<p>Exception {errMsg}</p>";
                    return;
                }

            }
        }

        private bool ModifyOrder(string keyID, string orderNo, string modifyCode, out string errMsg)
        {
            // 建立 完成/取消訂單 API之所需資訊
            string apiIP = System.Configuration.ConfigurationManager.AppSettings["ip"];
            string apiPort = System.Configuration.ConfigurationManager.AppSettings["port"];
            string url = $"http://{apiIP}:{apiPort}/Handlers/ModifyOrder.ashx";
            string rqString = "{\"OrderNo\":\"" + orderNo + "\",\"ModifyCode\":\"" + modifyCode + "\"}";
            JObject json = JObject.Parse(rqString);

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "Applicaiton/JSON";
                request.Headers["KeyID"] = keyID;

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            errMsg = string.Empty;
                            return true;
                        }
                    }
                }
                else
                {
                    errMsg = "更新訂單狀態失敗";
                    return false;
                }
            }
            catch (WebException wexp)
            {
                Logger.WriteLog(wexp);
                HttpWebResponse response = (HttpWebResponse)wexp.Response;
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var temp = reader.ReadToEnd();
                            ErrModel errContent = JsonConvert.DeserializeObject<ErrModel>(temp);
                            errMsg = errContent.ErrorMessage;
                            return false;
                        }
                    }
                }
                else
                {
                    errMsg = "更新訂單狀態失敗";
                    return false;
                }
            }
            catch (Exception err)
            {
                Logger.WriteLog(err);
                errMsg = "更新訂單狀態失敗";
                return false;
            }
        }

        public class ErrModel
        {
            public string ErrorMessage { get; set; }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["KeyID"] = null;
            Response.Redirect("/Default.aspx");
        }
    }
}