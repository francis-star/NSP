<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportHelper.aspx.cs" Inherits="Web.ASPX.ImportHelper" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../JS/lib/ligerUI/skins/aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
</head>
<body style="padding: 20px">
    <form id="form1" runat="server" style="color: Red">
        <div>
            <asp:Button ID="btnxls" runat="server" Text="导出Excel" OnClick="Button1_Click" />
        </div>
        <asp:HiddenField ID="hf" runat="server" />
    </form>
</body>
</html>
