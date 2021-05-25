<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/master.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SurveyReporting.Pages.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<div class="row">
		<div class="col-md-8 col-md-offset-2 col-sm-10 col-sm-offset-1 col-xs-12">
			<div class="panel">
				<div class="row">
					<div class="col-xs-6 col-spaced">
						<asp:Button class="btn btn--default btn-large btn-block" ID="btnResultSummary" runat="server" Text="Result Summary" OnClick="btnResultSummary_Click" />
					</div>
					<div class="col-xs-6 col-spaced">
						<asp:Button class="btn btn--default btn-large btn-block" ID="btnResultAnalysis" runat="server" Text="Result Analysis" OnClick="btnResultAnalysis_Click" /></td>
					</div>
					<div class="col-xs-6 col-spaced">
						<asp:Button class="btn btn--default btn-large btn-block" ID="btnProjectLeaderSummary" runat="server" Text="Project Leader Summary" OnClick="btnProjectLeaderSummary_Click" />
					</div>
					<div class="col-xs-6 col-spaced">
						<asp:Button class="btn btn--default btn-large btn-block" ID="btnAnswers" runat="server" Text="Answers" OnClick="btnAnswers_Click" />
					</div>
					<div class="col-xs-6 col-spaced">
						<asp:Button class="btn btn--default btn-large btn-block" ID="btnNoResponses" runat="server" Text="No Responses" OnClick="btnNoResponses_Click" />
					</div>
					<div class="col-xs-6 col-spaced">
						<asp:Button class="btn btn--default btn-large btn-block" ID="btnSurveyablePeople" runat="server" Text="Who will be Surveyed" OnClick="btnSurveyablePeople_Click"/>
					</div>
					<div class="col-xs-6 col-spaced">
						<asp:Button class="btn btn--default btn-large btn-block" ID="btnSenReminders" runat="server" Text="Send Reminders" OnClick="btnSendReminders_Click" />
					</div>
				</div>
			</div>
			
		</div>
	</div>
	
</asp:Content>
