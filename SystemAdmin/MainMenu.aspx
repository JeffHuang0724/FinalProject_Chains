<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainMenu.aspx.cs" Inherits="FinalProject_Chains.SystemAdmin.MainMenu" %>

<!DOCTYPE html>
<html lang="zh-Hant">
<head>
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>分店訂購系統</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: sans-serif;
            background: linear-gradient(to right, #b92b27, #1565c0);
        }

        .card {
            margin-bottom: 20px;
            border: none;
        }

        .box {
            width: 600px;
            padding: 40px;
            position: absolute;
            top: 50%;
            left: 50%;
            background: #191919;
            text-align: center;
            transition: 0.25s;
            margin-top: 100px;
        }

            .box h1 {
                color: white;
                text-transform: uppercase;
                font-weight: 500;
            }

            .box p {
                color: red;
            }

        a {
            border: 0;
            background: none;
            display: block;
            margin: 20px auto;
            text-align: center;
            border: 2px solid #2ecc71;
            padding: 14px 40px;
            outline: none;
            color: white;
            border-radius: 24px;
            transition: 0.25s;
            cursor: pointer;
        }

            a:hover {
                background: #2ecc71;
            }

        #header input[type="submit"],
        #header button {
            border: 0;
            background: none;
            display: block;
            margin: 20px auto;
            text-align: right;
            border: 2px solid #2ecc71;
            padding: 14px 40px;
            outline: none;
            color: white;
            border-radius: 24px;
            transition: 0.25s;
            cursor: pointer;
        }

            #header input[type="submit"]:hover,
            #header button:hover {
                background: #2ecc71;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="header" style="display: flex; justify-content: right; align-items: center">
            <asp:Button ID="btnLogout" runat="server" Text="登出" OnClick="btnLogout_Click" Style="position: absolute; right: 20px; top: 20px" />
        </div>
        <div class="container">
            <div class="row">
                <div class="col-md-6">
                    <div class="card">
                        <div class="box">
                            <h1>
                                <img alt="毛豆" src="/Images/Logo.png" width="80" height="80" style="border-radius: 50%;" />
                                毛豆端火鍋下單系統
                            </h1>
                            <div style="display: flex; flex-direction: column; margin-top: 2rem;">
                                <a href="/SystemAdmin/Order.aspx">訂購下單</a>
                                <a href="/SystemAdmin/Search.aspx">查詢訂單</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
