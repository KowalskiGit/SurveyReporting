<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/master.Master" AutoEventWireup="true" CodeBehind="Answers.aspx.cs" Inherits="SurveyReporting.Pages.Answers" %>

<%@ Register Src="~/Controls/Filter.ascx" TagPrefix="uc1" TagName="Filter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<uc1:Filter runat="server" id="Filter" />

	<div class="row">
		<div class="col-xs-12">
			<div class="panel page-heading">
				<h2>Survey Answers</h2>
			</div>
		</div>
	</div>

	<div class="row">
		<div class="col-xs-12">
			<div class="panel">
				<div class="panel-table__container">

					<asp:GridView ID="gvAnswers" runat="server" class="table panel-table" AutoGenerateColumns="False">
						<Columns>
							<asp:BoundField DataField="ContactPerson" HeaderText="Contact Person"></asp:BoundField>
							<asp:BoundField DataField="DateResponded" HeaderText="Date Responded" DataFormatString="{0:d}"></asp:BoundField>
							<asp:BoundField DataField="ProjectLeader" HeaderText="Prj Leader / Resp Person"></asp:BoundField>
							<asp:BoundField DataField="Question" HeaderText="Question"></asp:BoundField>
							<asp:BoundField DataField="Rating" HeaderText="Rating"></asp:BoundField>
							<asp:BoundField DataField="Comments" HeaderText="Comments"></asp:BoundField>
						</Columns>
					</asp:GridView>

				</div>
			</div>
		</div>
	</div>

</asp:Content>
