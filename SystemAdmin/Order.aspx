<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Order.aspx.cs"
    Inherits="FinalProject_Chains.SystemAdmin.Order" %>

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
            background-color: yellow;
        }

        #divTop input[type="text"],
        #ddlSales {
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
            color: white;
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
                店家下單
            </h1>
            <asp:Literal runat="server" ID="ltlErrMsg"></asp:Literal>
            <div
                style="display: flex; flex-direction: row; text-align: center; align-items: center;">
                <asp:GridView
                    ID="gvItemList"
                    runat="server"
                    AutoGenerateColumns="False"
                    Style="margin-top: 1rem"
                    OnRowDataBound="gvItemList_RowDataBound"
                    GridLines="Horizontal"
                    CellPadding="10"
                    BackColor="White"
                    BorderColor="#E7E7FF"
                    BorderWidth="1px"
                    class="table table-bordered">
                    <AlternatingRowStyle BackColor="#F7F7F7" />
                    <Columns>
                        <asp:BoundField HeaderText="商品編號" DataField="ItemNo" />
                        <asp:BoundField HeaderText="商品種類" DataField="Category" />
                        <asp:BoundField HeaderText="商品名稱" DataField="ItemName" />
                        <asp:BoundField HeaderText="庫存數量" DataField="StockCount" />
                        <asp:BoundField HeaderText="商品單價" DataField="ItemPrice" />
                        <asp:TemplateField HeaderText="訂購數量">
                            <ItemTemplate>
                                <asp:TextBox
                                    class="gvTxt"
                                    ID="txtOrderCount"
                                    Text="0"
                                    runat="server"
                                    TextMode="Number" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="備註">
                            <ItemTemplate>
                                <asp:TextBox
                                    class="gvTxt"
                                    ID="txtOrderRemark"
                                    Text=""
                                    runat="server" />
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
            <div
                id="divTop"
                style="display: flex; flex-direction: row; margin-top: 2rem; text-align: center;">

                <asp:DropDownList ID="ddlSales" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSales_SelectedIndexChanged"></asp:DropDownList>
                <asp:TextBox
                    ID="txtSalesNo"
                    runat="server"
                    placeholder="中央廚房銷售人員ID" ReadOnly="true" Style="margin-left: 2rem;"></asp:TextBox>
                <br />
            </div>
            <asp:TextBox
                ID="txtRemark"
                runat="server"
                placeholder="請輸入訂單備註"
                Style="width: 500px;" TextMode="MultiLine"></asp:TextBox>
            <div
                style="display: flex; flex-direction: row; margin-top: 2rem; text-align: center;">
                <asp:Button
                    runat="server"
                    ID="btnOrder"
                    Text="送出訂單"
                    OnClick="btnOrder_Click"
                    Style="margin-left: 1.5rem;" />
            </div>
        </div>
    </form>
</body>
</html>
