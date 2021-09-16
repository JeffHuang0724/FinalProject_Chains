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
    public partial class Order : System.Web.UI.Page
    {
        public string orderItem = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 建立 取得商品列表API之所需資訊
                string apiIP = System.Configuration.ConfigurationManager.AppSettings["ip"];
                string apiPort = System.Configuration.ConfigurationManager.AppSettings["port"];
                string url = $"http://{apiIP}:{apiPort}/Handlers/ItemsList.ashx";
                string keyID = HttpContext.Current.Session["KeyID"] as string;
                
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "POST";
                    request.Headers["KeyID"] = keyID;
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write("");
                    }
                    try
                    {
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        // Http Status 200 OK 則將回傳內容呈現於GridView
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (var stream = response.GetResponseStream())
                            {
                                using (var reader = new StreamReader(stream))
                                {
                                    string jsonStr = reader.ReadToEnd();
                                    var table = JsonConvert.DeserializeObject<DataTable>(jsonStr);
                                    this.gvItemList.DataSource = table;
                                    this.gvItemList.DataBind();
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
                        // 其餘之Http Exception則顯示Server error，並跳轉至登入頁面
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Server Error. Please contact system contact person.');window.location ='/Default.aspx';", true);
                            return;
                        }
                    }
                }
                // 發生未預期的Exception則顯示Server error，並跳轉至登入頁面
                catch (Exception err)
                {
                    Logger.WriteLog(err);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Server Error. Please contact system contact person.');window.location ='/Default.aspx';", true);
                    return;
                }

                // 建立 取得銷售人員列表API之所需資訊
                string salesUrl = $"http://{apiIP}:{apiPort}/Handlers/SearchSalesList.ashx";
                

                try
                {
                    HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(salesUrl);
                    request2.Method = "POST";
                    request2.Headers["KeyID"] = keyID;
                    using (var streamWriter = new StreamWriter(request2.GetRequestStream()))
                    {
                        streamWriter.Write("");
                    }
                    try
                    {
                        HttpWebResponse response = (HttpWebResponse)request2.GetResponse();
                        // Http Status 200 OK 則將回傳內容呈現於DropdownList
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (var stream = response.GetResponseStream())
                            {
                                using (var reader = new StreamReader(stream))
                                {
                                    string jsonStr = reader.ReadToEnd();
                                    var table = JsonConvert.DeserializeObject<DataTable>(jsonStr);
                                    this.ddlSales.DataSource = table;
                                    this.ddlSales.DataTextField = "EmployeeName";
                                    this.ddlSales.DataValueField = "EmployeeID";
                                    this.ddlSales.DataBind();
                                    this.ddlSales.Items.Insert(0, "請選擇銷售人員");
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
                        // 其餘之Http Exception則顯示Server error，並跳轉至登入頁面
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Server Error. Please contact system contact person.');window.location ='/Default.aspx';", true);
                            return;
                        }
                    }
                }
                // 發生未預期的Exception則顯示Server error，並跳轉至登入頁面
                catch (Exception err)
                {
                    Logger.WriteLog(err);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Server Error. Please contact system contact person.');window.location ='/Default.aspx';", true);
                    return;
                }
            }
        }

        protected void gvItemList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string errMsg = string.Empty;
            if (e.CommandName == "AddCart")
            {
                //取得點擊的索引值
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //取得點擊的該列
                GridViewRow row = this.gvItemList.Rows[rowIndex];

                //取得資訊
                TextBox txtOrderCount = (TextBox)row.Cells[5].FindControl("txtOrderCount");
                TextBox txtOrderRemark = (TextBox)row.Cells[6].FindControl("txtOrderRemark");
                Button btnAddCart = (Button)row.Cells[7].FindControl("btnAddCart");
                Button btnEditCart = (Button)row.Cells[7].FindControl("btnEditCart");

                txtOrderCount.Enabled = false;
                txtOrderRemark.Enabled = false;
                btnAddCart.Visible = false;
                btnEditCart.Visible = true;
            }
            else if (e.CommandName == "EditCart")
            {

                //取得點擊的索引值
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //取得點擊的該列
                GridViewRow row = this.gvItemList.Rows[rowIndex];

                //取得資訊
                TextBox txtOrderCount = (TextBox)row.Cells[5].FindControl("txtOrderCount");
                TextBox txtOrderRemark = (TextBox)row.Cells[6].FindControl("txtOrderRemark");
                Button btnAddCart = (Button)row.Cells[7].FindControl("btnAddCart");
                Button btnEditCart = (Button)row.Cells[7].FindControl("btnEditCart");

                txtOrderCount.Enabled = true;
                txtOrderRemark.Enabled = true;
                btnAddCart.Visible = true;
                btnEditCart.Visible = false;
            }
        }

        protected void gvItemList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var row = e.Row;
                string orderStatus = e.Row.Cells[1].Text;
                switch (orderStatus)
                {
                    case "0":
                        e.Row.Cells[1].Text = "肉類";
                        break;
                    case "1":
                        e.Row.Cells[1].Text = "海鮮類";
                        break;
                    case "2":
                        e.Row.Cells[1].Text = "蔬果類";
                        break;
                    case "3":
                        e.Row.Cells[1].Text = "火鍋料類";
                        break;
                    default:
                        e.Row.Cells[1].Text = "尚未定義";
                        break;
                }

                int stockCount = Convert.ToInt32(e.Row.Cells[3].Text);
                if(stockCount == 0)
                {
                    e.Row.Cells[3].Text = "現無庫存";
                    TextBox txtOrderCount = (TextBox)e.Row.FindControl("txtOrderCount");
                    txtOrderCount.Enabled = false;
                    TextBox txtOrderRemark = (TextBox)e.Row.FindControl("txtOrderRemark");
                    txtOrderRemark.Enabled = false;
                }

            }
        }
        protected void btnOrder_Click(object sender, EventArgs e)
        {
            if (this.ddlSales.SelectedIndex == 0)
            {
                this.ltlErrMsg.Text = "<p>請選擇銷售人員</p>";
                return;
            }

            // 組 OrderDetail 內容 
            string orderDetail = string.Empty;
            for (int i = 0; i < this.gvItemList.Rows.Count; i++)
            {
                string orderNo = gvItemList.Rows[i].Cells[0].Text;
                TextBox txtOrderCount = (TextBox)gvItemList.Rows[i].Cells[5].FindControl("txtOrderCount");
                int orderCount = 0;
                if (!Int32.TryParse(txtOrderCount.Text, out orderCount)){
                    this.ltlErrMsg.Text = "<p>訂購數量不得為空，請確認後再下單</p>";
                    return;
                }
                if (orderCount < 0)
                {
                    this.ltlErrMsg.Text = "<p>訂購數量不得為負數，請確認後再下單</p>";
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtOrderCount.Text) || orderCount == 0)
                {
                    continue;
                }
                TextBox txtOrderRemark = (TextBox)gvItemList.Rows[i].Cells[6].FindControl("txtOrderRemark");
                string orderRemark = txtOrderRemark.Text;

                orderDetail += "{\"ItemNo\":\"" + orderNo + "\",\"ItemCount\":" + orderCount + ",\"Remark\":\"" + orderRemark + "\"}";
            }
            // 如果完全沒有下單任何品項，則顯示錯誤訊息
            if (string.IsNullOrWhiteSpace(orderDetail))
            {
                this.ltlErrMsg.Text = "<p>訂購數量不得為空，請確認後再下單</p>";
                return;
            }
            orderDetail = orderDetail.Replace("}{", "},{");
            string orderDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string salesNo = this.txtSalesNo.Text;
            orderDetail = "[" + orderDetail + "]";
            string remark = this.txtRemark.Text;

            // 建立 下單API之所需資訊
            string apiIP = System.Configuration.ConfigurationManager.AppSettings["ip"];
            string apiPort = System.Configuration.ConfigurationManager.AppSettings["port"];
            string url = $"http://{apiIP}:{apiPort}/Handlers/Order.ashx";
            string keyID = HttpContext.Current.Session["KeyID"] as string;
            string rqString = "{\"OrderDate\":\"" + orderDate + "\",\"SalesNo\":\"" + salesNo + "\",\"OrderDetail\":" + orderDetail + ",\"Remark\":\"" + remark + "\"}";
            JObject json = JObject.Parse(rqString);

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Headers["KeyID"] = keyID;
                
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    // Http Status 200 OK 則顯示訂單編號，並跳轉至主功能頁
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                var temp = reader.ReadToEnd();
                                SuccessModel successContent = JsonConvert.DeserializeObject<SuccessModel>(temp);
                                String orderNo = successContent.OrderNo;
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('OrderNo: " + orderNo + "');window.location ='/SystemAdmin/MainMenu.aspx';", true);
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
            // 發生未預期的Exception則顯示下單失敗
            catch (Exception err)
            {
                Logger.WriteLog(err);
                this.ltlErrMsg.Text = $"<p>下單失敗，請洽系統服務員</p>";
                return;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["KeyID"] = null;
            Response.Redirect("/Default.aspx");
        }

        protected void ddlSales_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtSalesNo.Text = this.ddlSales.SelectedValue;
        }
    }

    public class SuccessModel
    {
        public string OrderNo { get; set; }
    }
    public class ErrModel
    {
        public string ErrorMessage { get; set; }
    }
}