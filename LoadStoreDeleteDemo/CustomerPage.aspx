<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CustomerPage.aspx.vb" Inherits="LoadStoreDeleteDemo.CustomerPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customer</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
            <br />
            <asp:GridView ID="gvwCustomers" runat="server" AutoGenerateSelectButton="True">
            </asp:GridView>
            <br />
            ID<asp:TextBox ID="txtID" runat="server"></asp:TextBox>
            <br />
            Name<asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            <br />
            Address Line 1<asp:TextBox ID="txtAddressLine1" runat="server"></asp:TextBox>
            <br />
            Address Line 2<asp:TextBox ID="txtAddressLine2" runat="server"></asp:TextBox>
            <br />
            City<asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
            <br />
            State<asp:TextBox ID="txtState" runat="server"></asp:TextBox>
            <br />
            Zip<asp:TextBox ID="txtZip" runat="server"></asp:TextBox>
            <br />
            <asp:Button ID="btnSave" runat="server" Text="Save" />
            <br />
            <br />
        </div>
    </form>
</body>
</html>
