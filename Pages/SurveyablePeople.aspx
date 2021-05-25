<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/master.Master" AutoEventWireup="true" CodeBehind="SurveyablePeople.aspx.cs" Inherits="SurveyReporting.Pages.SurveyablePeople" %>

<%@ Register Src="~/Controls/ProjectFilter.ascx" TagPrefix="uc1" TagName="ProjectFilter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<uc1:ProjectFilter runat="server" id="ProjectFilter" />

	<div class="row">
		<div class="col-xs-12">
			<div class="panel page-heading">
				<h2>Surveyable People</h2>
			</div>
		</div>
	</div>

	<div class="row">
		<div class="col-xs-12">
			<div class="panel">
				<div class="panel-table__container">
					
					<asp:GridView ID="gvSurveyablePeople" runat="server" class="table panel-table" AutoGenerateColumns="False" AllowSorting="True" OnSorting="gvSurveyablePeople_Sorting">
						<Columns>
							<asp:BoundField DataField="ProjectID" HeaderText="Project ID" SortExpression="ProjectID"></asp:BoundField>
							<asp:BoundField DataField="ProjectName" HeaderText="Project Name" SortExpression="ProjectName"></asp:BoundField>
							<asp:BoundField DataField="ProjectLeader" HeaderText="Project Leader" SortExpression="ProjectLeader"></asp:BoundField>
							<asp:BoundField DataField="ProjectLeaderName" HeaderText="Project Leader Name" SortExpression="PersonnelView.FirstName + ' ' + PersonnelView.Surname"></asp:BoundField>
							<asp:BoundField DataField="CompanyID" HeaderText="Company ID" SortExpression="P.CompanyID"></asp:BoundField>
							<asp:BoundField DataField="CompanyName" HeaderText="Company Name" SortExpression="CompanyName"></asp:BoundField>
							<asp:BoundField DataField="ContactPersonID" HeaderText="Contact Person" SortExpression="ContactPersonID"></asp:BoundField>
							<asp:BoundField DataField="Email" HeaderText="EMail" SortExpression="C.Email "></asp:BoundField>
						</Columns>
					</asp:GridView>

				</div>
			</div>
		</div>
	</div>

</asp:Content>
