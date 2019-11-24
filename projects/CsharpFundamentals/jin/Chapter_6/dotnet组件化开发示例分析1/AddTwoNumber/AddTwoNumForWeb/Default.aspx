<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AddTwoNumForWeb.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
                <div>
            <h2>两数相加(借鸡生蛋之Web版)</h2>
            <asp:TextBox ID="txtNumber1" runat="server"></asp:TextBox>
            &nbsp;&nbsp;
            <asp:Label ID="Label2" runat="server" Text="Label" Font-Size="20">+</asp:Label>
            &nbsp;
            <asp:TextBox ID="txtNumber2" runat="server"></asp:TextBox>
            &nbsp;
            <asp:Button ID="btnAdd" runat="server" Text="=" Onclick="btnAdd_Click"/>
            &nbsp;
            <asp:Label ID="lblResult" runat="server" Text="0" Font-Size="20pt"></asp:Label>
            &nbsp;
        </div>
    </form>
</body>
</html>
