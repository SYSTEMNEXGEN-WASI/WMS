<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="DXBMS.Main.login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
  <meta charset="UTF-8">
  <title>Login</title>
  
  
  <%--<link rel='stylesheet prefetch' href='http://fonts.googleapis.com/css?family=Open+Sans:600'>--%>

	  <link rel="stylesheet" href="Content/LoginStyle.css">
	  <%--<link rel="stylesheet" href="Style/Login.css">--%>

  
</head>

<body>
    <%--<a href="login.aspx">login.aspx</a>--%>
 <form id="form1" runat="server">
  <div class="login-wrap">
	<div class="login-html">
		<input id="tab-1" type="radio" name="tab" class="sign-in" checked="checked"/><label for="tab-1" class="tab">Sign In</label>
		<input id="tab-2" type="radio" name="tab" class="sign-up"/><label for="tab-2" class="tab">Sign Up</label>
		<div class="login-form">
			<div class="sign-in-htm">
				<div class="group">
					<label for="user" class="label">Username</label>
	<asp:TextBox ID="txtUserID" runat="server" CssClass ="input"></asp:TextBox><%--<input id="user" type="text" class="input">--%>
				</div>
				<div class="group">
					<label for="pass" class="label">Password</label>
					<asp:TextBox ID="txtPassword" runat="server" CssClass="input" TextMode="Password"></asp:TextBox><%--<input id="pass" type="password" class="input" data-type="password">--%>
				</div>
				
				<div class="group">
				
		<asp:Button ID="btnSignIn" runat="server" Text="Sign In" class="button" onclick="btnSignIn_Click" 
						/><%--<input type="submit" class="button" value="Sign In">--%>
				</div>
				<div >
					<asp:Label ID="lblMsg" runat="server"  CssClass="Login_Btn" ></asp:Label>
				</div>
				<div class="hr"></div>
				<div class="foot-lnk">
					<a href="#forgot">Forgot Password?</a>
				</div>
			</div>
			<div class="sign-up-htm">
			<div class="group">
					<label for="pass" class="label">Employee Code</label>
				<asp:TextBox ID="txtEmpCode" runat="server" CssClass ="input"></asp:TextBox>
				</div>
				<div class="group">
					<label for="user" class="label">Username</label>
					<input id="user" type="text" class="input">
				</div>
				<div class="group">
					<label for="pass" class="label">Password</label>
					<input id="pass" type="password" class="input" data-type="password">
				</div>
				<div class="group">
					<label for="pass" class="label">Repeat Password</label>
					<input id="pass" type="password" class="input" data-type="password">
				</div>
				<div class="group">
					<input type="submit" class="button" value="Sign Up">
				</div>
				<div class="hr"></div>
				<div class="foot-lnk">
					<label for="tab-1">Already Member?</a>
				</div>
			</div>
		</div>
	</div>
</div>
  
  </form>
</body>
</html>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
<center>
	<h1>Login to system</h1>
	<table>
		<tr>
			<td>
				<asp:Label ID="Label1" runat="server" Text="User ID: "></asp:Label>
			</td>
			<td>
				<asp:TextBox ID="txtUserID" runat="server"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Label ID="Label2" runat="server" Text="Password: "></asp:Label>
			</td>
			<td>
				<asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
			</td>
		</tr>
		<caption>
			<br />
			<asp:Label ID="lblError" runat="server" ForeColor="#CC0000"></asp:Label>
		</caption>
		</tr>
		<tr >
		<td colspan ="2" align="center">Version : 18th Feb 2017</td>
		</tr>
	</table>    
		
		<dx:ASPxButton ID="btnLogin" runat="server" Text="Login" 
			onclick="btnLogin_Click">
			<Image Url="~/Images/Employee_16x16.png">
			</Image>
		</dx:ASPxButton>
	<dx:ASPxButton ID="btnClear" runat="server" Text="Clear" 
		onclick="btnClear_Click">
		<Image Url="~/Images/Clear_16x16.png">
		</Image>
	</dx:ASPxButton>
	</asp:Panel>
</asp:Content>
--%>