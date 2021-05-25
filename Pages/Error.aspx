<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/master.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="SurveyReporting.Pages.Error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Error occurred</title>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <br />
    <table><tr><td>
        <strong>An error has occurred.
        <br /><br />
        We apologise for the inconvenience.  Please report this error to info@smms.co.za.<br /><br />
        </strong>
    </td></tr></table>
</asp:Content>
