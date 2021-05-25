<%@Page Title="Survey Reminders" Language="C#" MasterPageFile="~/Pages/master.Master" AutoEventWireup="true" CodeBehind="SendReminders.aspx.cs" Inherits="SurveyReporting.Pages.SendReminders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<div class="row">
		<div class="col-lg-8 col-lg-offset-2 col-md-10 col-md-offset-1 col-sm-10 col-sm-offset-1 col-xs-12">
			<div class="panel page-heading">
				<h2>Send Reminders</h2>
			</div>
		</div>
	</div>

	<div class="row">
		<div class="col-lg-8 col-lg-offset-2 col-md-10 col-md-offset-1 col-sm-10 col-sm-offset-1 col-xs-12">
			<div class="panel">
				<div class="row">
					<div class="col-xs-12">
						<div class="control-group">
							<label>Open Surveys</label>
							<asp:DropDownList ID="OpenSurvey" runat="server" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="OpenSurvey_SelectedIndexChanged">
								<asp:ListItem Value="" Text="" />
							</asp:DropDownList>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>

	<div class="row">
		<div class="col-lg-8 col-lg-offset-2 col-md-10 col-md-offset-1 col-sm-10 col-sm-offset-1 col-xs-12">
			<div class="panel">
				<div class="panel-table__container">
					<table class="table table--dataview panel-table" data-no-table="true">
						<tr>
							<td>Recipients that have responded</td>
							<td>
								<asp:Literal runat="server" ID="litHaveResponded" />
							</td>
						</tr>
						<tr>
							<td>Recipients that have NOT responsed</td>
							<td>
								<asp:Literal runat="server" ID="litHaveNotResponded" />
							</td>
						</tr>
					</table>
				</div>
			</div>
		</div>
	</div>

	<div class="row">
		<div class="col-lg-8 col-lg-offset-2 col-md-10 col-md-offset-1 col-sm-10 col-sm-offset-1 col-xs-12">
			<div class="panel">
				
				<div class="control-group">
					<label>From</label>
					<asp:TextBox ID="txtFrom" runat="server">noreply@eskom.co.za</asp:TextBox>
				</div>

				<div class="control-group">
					<label>To</label>
					<asp:TextBox ID="TextBox1" runat="server" Enabled="false">Recipients who have not responded yet</asp:TextBox>
				</div>

				<div class="control-group">
					<label>Subject</label>
					<asp:TextBox ID="txtSubject" runat="server">PTM Survey Reminder</asp:TextBox>
				</div>

				<div class="control-group">
					<label>Message</label>
					<asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="18"></asp:TextBox>
				</div>

				<div class="control-group">
					<div class="btn-strip">
						<asp:Literal runat="server" ID="litReminderSend" />

						<asp:Button ID="btnSend" runat="server" Text="Send" OnClick="btnSend_Click" UseSubmitBehavior="False" />
					</div>
				</div>

			</div>
		</div>
	</div>

</asp:Content>
