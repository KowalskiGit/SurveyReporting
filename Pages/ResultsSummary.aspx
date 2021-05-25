<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/master.Master" AutoEventWireup="true" CodeBehind="ResultsSummary.aspx.cs" Inherits="SurveyReporting.Pages.ResultsSummary" %>

<%@ Register Src="~/Controls/Filter.ascx" TagPrefix="uc1" TagName="Filter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<uc1:Filter runat="server" ID="Filter" />

	<div class="row">
		<div class="col-xs-12">
			<div class="panel page-heading">
				<h2>Result Summary</h2>
			</div>
		</div>
	</div>

	<div class="row">
		<div class="col-md-6 col-sm-12 col-xs-12 col-spaced">
			<div class="panel">
				<div class="panel-table__container">
					<table class="table table--dataview panel-table" data-no-table="true">
						<tr>
							<td>No of Surveys</td>
							<td>
								<asp:Literal runat="server" ID="litNoOfSurveys" />
							</td>
						</tr>
						<tr>
							<td>Responses</td>
							<td>
								<asp:Literal runat="server" ID="litResponses" />
							</td>
						</tr>
						<tr>
							<td>No Responses</td>
							<td>
								<asp:Literal runat="server" ID="litNoResponses" />
							</td>
						</tr>
						<tr>
							<td>% Returns</td>
							<td>
								<asp:Literal runat="server" ID="litPercentageReturns" />
							</td>
						</tr>
					</table>
				</div>
			</div>
		</div>
		
		<div class="col-md-6 col-sm-12 col-xs-12 col-spaced">
			<div class="panel">
				<div class="panel-table__container">
					<table class="table table--dataview panel-table" data-no-table="true">
						<tr>
							<td>Projects Surveyed</td>
							<td>
								<asp:Literal runat="server" ID="litProjectsSurveyed" />
							</td>
						</tr>
						<tr>
							<td>Projects with Responses</td>
							<td>
								<asp:Literal runat="server" ID="litProjectsWithResponses" />
							</td>
						</tr>
						<tr>
							<td>Projects without Reponses</td>
							<td>
								<asp:Literal runat="server" ID="litProjectsWithoutResponses" />
							</td>
						</tr>
						<tr>
							<td>Projects Return Rate</td>
							<td>
								<asp:Literal runat="server" ID="litProjectsReturnRate" />
							</td>
						</tr>
					</table>
				</div>
			</div>
		</div>
	</div>

</asp:Content>
