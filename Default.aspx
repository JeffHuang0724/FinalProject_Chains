<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="FinalProject_Chains.Default" %>

<!DOCTYPE html>
<html lang="zh-Hant">
<head>
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>分店訂購系統</title>
    <link
        rel="stylesheet"
        href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="./CSS/DefaultStyle.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="col-md-6">
                    <div class="card">
                        <div class="box">
                            <h1>
                                <img
                                    alt="毛豆"
                                    src="/Images/Logo.png"
                                    width="80"
                                    height="80"
                                    style="border-radius: 50%" />
                                毛豆火鍋下單系統
                            </h1>
                            <p class="text-muted">請輸入帳號密碼</p>
                            <asp:TextBox
                                ID="txtAccount"
                                runat="server"
                                placeholder="Account"></asp:TextBox>
                            <asp:TextBox
                                ID="txtPassword"
                                runat="server"
                                placeholder="Password"
                                TextMode="Password"></asp:TextBox>
                            <asp:Button
                                runat="server"
                                ID="btnLogin"
                                Text="登入"
                                OnClick="btnLogin_Click" />
                            <asp:Literal runat="server" ID="ltlErrMsg"></asp:Literal>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
