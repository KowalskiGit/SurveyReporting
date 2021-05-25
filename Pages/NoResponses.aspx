<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/master.Master" AutoEventWireup="true" CodeBehind="NoResponses.aspx.cs" Inherits="SurveyReporting.Pages.NoResponses" %>

<%@ Register Src="~/Controls/Filter.ascx" TagPrefix="uc1" TagName="Filter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<uc1:Filter runat="server" id="Filter" />

	<div class="row">
		<div class="col-xs-12">
			<div class="panel page-heading">
				<h2>Recipients Not Responded</h2>
			</div>
		</div>
	</div>

	<div class="row">
		<div class="col-xs-12">
			<div class="panel">
				<div class="panel-table__container">
					
					<asp:GridView ID="gvNoResponses" runat="server" class="table panel-table" AutoGenerateColumns="False">
						<Columns>
							<asp:BoundField DataField="FullName" HeaderText="Name"></asp:BoundField>
							<asp:BoundField DataField="SurveyName" HeaderText="Survey"></asp:BoundField>
							<asp:BoundField DataField="Email" HeaderText="Email"></asp:BoundField>
							<asp:BoundField DataField="DatePublished" HeaderText="Date Published" DataFormatString="{0:d}"></asp:BoundField>
							<asp:BoundField DataField="DateReminded" HeaderText="Date Reminded" DataFormatString="{0:d}"></asp:BoundField>
						</Columns>
					</asp:GridView>

				</div>
			</div>
		</div>
	</div>

</asp:Content>
