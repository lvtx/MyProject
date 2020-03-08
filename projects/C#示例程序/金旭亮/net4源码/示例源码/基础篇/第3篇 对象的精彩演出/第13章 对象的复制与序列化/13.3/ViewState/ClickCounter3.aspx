<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClickCounter3.aspx.cs" Inherits="ClickCounter3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>按钮计数器</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align:center;">
        <h2>
            按钮计数器</h2>
        <hr />
     <asp:Button ID="btnClickMe" runat="server" Text="Click Me!" 
            onclick="btnClickMe_Click" />
        <br />
        <br />
        <asp:Label ID="lblInfo" runat="server">您单击了0次按钮。</asp:Label>
    </div>
    </div>
    </form>
</body>
</html>
