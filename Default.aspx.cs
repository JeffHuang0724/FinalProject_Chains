using FinalProject_Chains.Logs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProject_Chains
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string apiIP = System.Configuration.ConfigurationManager.AppSettings["ip"];
            string apiPort = System.Configuration.ConfigurationManager.AppSettings["port"];
            string url = $"http://{apiIP}:{apiPort}/Handlers/Login.ashx";
            string account = this.txtAccount.Text;
            string password = this.txtPassword.Text;

            string rqString = "{\"Account\":\"" + account + "\",\"Password\":\"" + password + "\"}";
            JObject json = JObject.Parse(rqString);

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "Applicaiton/JSON";
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
                            var temp = reader.ReadToEnd();
                            LoginModel loginContent = JsonConvert.DeserializeObject<LoginModel>(temp);
                            HttpContext.Current.Session["KeyID"] = loginContent.KeyID;
                            Response.Redirect("/SystemAdmin/MainMenu.aspx");
                        }
                    }
                }
                else
                {
                    this.ltlErrMsg.Text = $"<p>登入失敗</p>";
                    return;
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
                            this.ltlErrMsg.Text = $"<p>登入失敗</p>";
                            return;
                        }
                    }
                } else
                {
                    this.ltlErrMsg.Text = $"<p>登入失敗</p>";
                    return;
                }
            }
            catch (Exception err)
            {
                Logger.WriteLog(err);
                this.ltlErrMsg.Text = $"<p>登入失敗</p>";
                return;
            }
        }

        public class LoginModel
        {
            public string KeyID { get; set; }
        }
        public class ErrModel
        {
            public string ErrorMessage { get; set; }
        }
    }
}