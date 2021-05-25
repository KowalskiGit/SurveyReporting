<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/master.Master" AutoEventWireup="true" CodeBehind="ResultAnalysis.aspx.cs" Inherits="SurveyReporting.Pages.ResultAnalysis" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Src="~/Controls/Filter.ascx" TagPrefix="uc1" TagName="Filter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<uc1:Filter runat="server" id="Filter" />

	<div class="row">
		<div class="col-xs-12">
			<div class="panel page-heading">
				<h2>Results Analysis</h2>
			</div>
		</div>
	</div>

	<div class="row">
		<div class="col-xs-12">
			<div class="panel">
				<div class="panel-table__container">
					
					<asp:GridView ID="gvResultAnalysis" runat="server" class="table panel-table" AutoGenerateColumns="True" OnRowDataBound="gvResultAnalysis_RowDataBound"></asp:GridView>

				</div>
			</div>
		</div>
	</div>

	<div class="row">

		<div class="col-md-6 col-sm-12 col-xs-12 col-spaced">
			
			<asp:Chart ID="PerOUGraph" runat="server" BorderlineColor="Black" Width="500" Height="500">
				<Series>
					<asp:Series Name="GraphSeries" BorderColor="White" Color="DarkSeaGreen" ShadowOffset="5" IsValueShownAsLabel="True"></asp:Series>
				</Series>
				<ChartAreas>
					<asp:ChartArea Name="ChartArea">
						<AxisY IsLabelAutoFit="False" Title="Average Rating">
							<LabelStyle Font="Microsoft Sans Serif, 7pt" />
						</AxisY>
						<AxisX IsLabelAutoFit="False" Title="Organisation Units" Interval="1">
							<MajorGrid Enabled="False" />
							<LabelStyle Font="Microsoft Sans Serif, 8pt" Angle="-60" />
						</AxisX>
					</asp:ChartArea>
				</ChartAreas>
			</asp:Chart>

		</div>

		<div class="col-md-6 col-sm-12 col-xs-12 col-spaced">
			
			<asp:Chart ID="PerCategoryGraph" runat="server" BorderlineColor="Black" Width="500" Height="500">
				<Series>
					<asp:Series Name="GraphSeries" BorderColor="White" Color="DarkSeaGreen" ShadowOffset="5" IsValueShownAsLabel="True"></asp:Series>
				</Series>
				<ChartAreas>
					<asp:ChartArea Name="ChartArea">
						<AxisY IsLabelAutoFit="False" Title="Average Rating">
							<LabelStyle Font="Microsoft Sans Serif, 7pt" />
						</AxisY>
						<AxisX IsLabelAutoFit="False" Title="Category" Interval="1">
							<MajorGrid Enabled="False" />
							<LabelStyle Font="Microsoft Sans Serif, 8pt" Angle="-60" />
						</AxisX>
					</asp:ChartArea>
				</ChartAreas>
			</asp:Chart>
			
		</div>

	</div>

</asp:Content>
