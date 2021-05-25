<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/master.Master" AutoEventWireup="true" CodeBehind="ProjectLeaderSummary.aspx.cs" Inherits="SurveyReporting.Pages.ProjectLeaderSummary" %>

<%@ Register Src="~/Controls/Filter.ascx" TagPrefix="uc1" TagName="Filter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<uc1:Filter runat="server" id="Filter" />

	<div class="row">
		<div class="col-xs-12">
			<div class="panel page-heading">
				<h2>Project Leader Summary</h2>
			</div>
		</div>
	</div>

	<div class="row">
		<div class="col-xs-12">
			<div class="panel">
				<div class="panel-table__container">
					
					<asp:GridView ID="gvProjectLeaderSummary" runat="server" class="table panel-table" AutoGenerateColumns="False">
						<Columns>
							<asp:BoundField DataField="ProjectLeader" HeaderText="Project Leader"></asp:BoundField>
							<asp:BoundField DataField="ProjectLeaderName" HeaderText="Name"></asp:BoundField>
							<asp:BoundField DataField="AvgScore" HeaderText="Avg Score"></asp:BoundField>
						</Columns>
					</asp:GridView>

				</div>
			</div>
		</div>
	</div>

</asp:Content>
