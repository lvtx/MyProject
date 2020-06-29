<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>在线计算器</title>
</head>
<body>
    <center>
        <form id="form1" runat="server">
        <h2>在线四则运算计算器</h2>
        <div>
            输入表达式：<asp:TextBox ID="txtExpression" runat="server"></asp:TextBox><br />
            <br />
            <asp:Button ID="btnCalculator" runat="server" OnClick="btnCalculator_Click" Text="计算"
                Width="96px" /><br />
            <br />
            <asp:Label ID="lblResult" runat="server"></asp:Label></div>
        </form>
    </center>
</body>
</html>
