<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs"
    Inherits="FinalProject_Chains.SystemAdmin.Search" %>

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
    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: sans-serif;
            background: linear-gradient(to right, #b92b27, #1565c0);
        }

        h1 {
            color: white;
            text-transform: uppercase;
            font-weight: 500;
        }

        p {
            color: red;
        }

        input[type="text"] {
            border: 0;
            background: none;
            display: block;
            margin: 20px auto;
            text-align: center;
            border: 2px solid #3498db;
            padding: 10px 10px;
            width: 250px;
            outline: none;
            color: white;
            border-radius: 24px;
            transition: 0.25s;
        }

            input[type="text"]:focus {
                width: 300px;
                border-color: #2ecc71;
            }

        input[type="submit"],
        button {
            border: 0;
            background: none;
            display: block;
            margin: 20px auto;
            text-align: center;
            border: 2px solid #2ecc71;
            padding: 14px 40px;
            outline: none;
            color: black;
            border-radius: 24px;
            transition: 0.25s;
            cursor: pointer;
        }

            input[type="submit"]:hover,
            button:hover {
                background: #2ecc71;
                color: white;
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
        <div
            id="header"
            style="display: flex; justify-content: right; align-items: center">
            <asp:Button
                ID="btnLogout"
                runat="server"
                Text="登出"
                OnClick="btnLogout_Click"
                Style="position: absolute; right: 20px; top: 20px" />
        </div>
        <div
            style="display: flex; flex-direction: column; margin-top: 2rem; align-items: center;">
            <h1>
                <a href="/SystemAdmin/MainMenu.aspx" target="_self">
                    <img
                        alt="毛豆"
                        src="/Images/Logo.png"
                        width="80"
                        height="80"
                        style="border-radius: 50%" />
                </a>
                訂單列表
            </h1>
            <asp:Literal runat="server" ID="ltlErrMsg"></asp:Literal>
            <div
                style="display: flex; flex-direction: row; margin-top: 2rem; text-align: center;">
                <asp:TextBox
                    ID="txtOrderNo"
                    runat="server"
                    placeholder="請輸入欲查詢之訂單編號"></asp:TextBox>
                <asp:Button
                    runat="server"
                    ID="btnSearch"
                    Text="查詢"
                    OnClick="btnSearch_Click"
                    Style="color: white; margin-left: 1.5rem;" />
            </div>
            <asp:GridView
                ID="gvOrderInfo"
                runat="server"
                AutoGenerateColumns="False"
                Style="margin-top: 1rem"
                OnRowCommand="gvOrderInfo_RowCommand"
                OnRowDataBound="gvOrderInfo_RowDataBound"
                GridLines="Horizontal"
                CellPadding="1"
                BackColor="White"
                BorderColor="#E7E7FF"
                BorderWidth="1px"
                class="table table-bordered">
                <AlternatingRowStyle BackColor="#F7F7F7" />
                <Columns>
                    <asp:BoundField HeaderText="訂單編號" DataField="OrderNo" />
                    <asp:BoundField HeaderText="下單日期" DataField="OrderDate" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" />
                    <asp:BoundField HeaderText="預計出貨日" DataField="PreShipmentDate" DataFormatString="{0:d}" />
                    <asp:BoundField
                        HeaderText="最新處理時間"
                        DataField="ShipmentDate" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" />
                    <asp:BoundField HeaderText="結單時間" DataField="ArriveDate" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" />
                    <asp:BoundField HeaderText="備註" DataField="Remark" />
                    <asp:BoundField HeaderText="訂單狀態" DataField="OrderStatus" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Button
                                ID="btnOrderDetail"
                                Text="訂購品項"
                                runat="server"
                                CommandName="OrderDetail"
                                CommandArgument="<%# Container.DataItemIndex %>"
                                Style="margin-left: 1.5rem"
                                Visible="true" />
                            <asp:Button
                                ID="btnCancel"
                                Text="取消訂單"
                                runat="server"
                                CommandName="CancelOrder"
                                CommandArgument="<%# Container.DataItemIndex %>"
                                Style="margin-left: 1.5rem"
                                Visible="false" />
                            <asp:Button
                                ID="btnComplete"
                                Text="完成訂單"
                                runat="server"
                                CommandName="CompleteOrder"
                                CommandArgument="<%# Container.DataItemIndex %>"
                                Style="margin-left: 1.5rem"
                                Visible="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                <HeaderStyle
                    BackColor="#4A3C8C"
                    Font-Bold="True"
                    ForeColor="#F7F7F7" />
                <PagerStyle
                    BackColor="#E7E7FF"
                    ForeColor="#4A3C8C"
                    HorizontalAlign="Right" />
                <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                <SelectedRowStyle
                    BackColor="#738A9C"
                    Font-Bold="True"
                    ForeColor="#F7F7F7" />
                <SortedAscendingCellStyle BackColor="#F4F4FD" />
                <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
                <SortedDescendingCellStyle BackColor="#D8D8F0" />
                <SortedDescendingHeaderStyle BackColor="#3E3277" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
