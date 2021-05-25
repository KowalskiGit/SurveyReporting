<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectFilter.ascx.cs" Inherits="SurveyReporting.Controls.ProjectFilter" %>

<link href="../Scripts/htmlDatePicker.css" rel="stylesheet" />
<script src="../Scripts/htmlDatePicker.js"></script>

<div class="row">
	<div class="col-lg-9 col-md-10 col-sm-12 col-xs-12">
	
		<div id="mFilter" class="panel filter">

			<div class="filter__title">
				Filter
			</div>

			<button type="button" class="filter__btn" title="Collapse Filter">
				<i class="fa fa-chevron-up arrow-up"></i>
                <i class="fa fa-chevron-down arrow-down"></i>
			</button>

			<div class="row filter__content">
				<div class="col-xs-4">
					<div class="control-group">
						<label>Project ID</label>
						<asp:DropDownList ID="ProjectID" runat="server" AppendDataBoundItems="True">
							<asp:ListItem Value="" Text="" />
						</asp:DropDownList>						
					</div>

					<div class="control-group">
						<label>Project Manager</label>
						<asp:TextBox ID="ProjectManager" runat="server"></asp:TextBox>
					</div>

					<div class="control-group">
						<label>Company ID</label>
						<asp:DropDownList ID="CompanyID" runat="server" AppendDataBoundItems="True">
							<asp:ListItem Value="" Text="" />
						</asp:DropDownList>
					</div>

					<div class="control-group">
						<label>Organization Unit</label>
						<asp:DropDownList ID="OrganisationUnit" runat="server" AppendDataBoundItems="True">
							<asp:ListItem Value="" Text="" />
						</asp:DropDownList>

                        <label><input ID="Rollup" type="checkbox" value="" runat="server" style="display:inline;width:10%">Roll up</label>
					</div>
				</div>
				<div class="col-xs-4">
					<div class="control-group">
						<label>Project Name</label>
						<asp:TextBox ID="ProjectName" runat="server" ></asp:TextBox>
					</div>

					<div class="control-group">
						<label>Contact Person</label>
						<asp:TextBox ID="ContactPerson" runat="server"></asp:TextBox>
					</div>

					<div class="control-group">
						<label>Company Name</label>
						<asp:TextBox ID="CompanyName" runat="server"></asp:TextBox>
					</div>

					<div class="control-group">
						<label>Closed Projects</label>
                        <asp:DropDownList ID="Closed" runat="server" AppendDataBoundItems="True">
                            <asp:ListItem Value="True" Text="Yes" />
                            <asp:ListItem Value="False">No</asp:ListItem>
                            <asp:ListItem Selected="True">All</asp:ListItem>
                        </asp:DropDownList>

					</div>

				</div>

                <%--3rd column--%>
                <div class="col-xs-4">
					<div class="control-group">
						<label>Exclude Action Plan Projects</label>
                        <asp:DropDownList ID="ExcludeActionPlan" runat="server" AppendDataBoundItems="True">
                            <asp:ListItem Selected="True" Value="True" Text="Yes" />
                            <asp:ListItem Value="False">No</asp:ListItem>
                            <asp:ListItem>All</asp:ListItem>
                        </asp:DropDownList>
					</div>

					<div class="control-group">
						<label>Tasks Closed From</label>
						<asp:TextBox ID="TasksClosedFrom" runat="server" OnLoad="TasksClosedFrom_Load"></asp:TextBox>
					</div>

					<div class="control-group">
						<label>Tasks Closed To</label>
						<asp:TextBox ID="TasksClosedTo" runat="server" OnLoad="TasksClosedTo_Load"></asp:TextBox>
					</div>

					<div class="control-group" style="visibility:hidden">
						<label>A</label>
						<asp:TextBox runat="server"></asp:TextBox>
					</div>

                	<div class="control-group">
						<asp:Button ID="ApplyFilter" runat="server" CssClass="btn btn--default" Text="Apply Filter" onclick="ApplyFilter_Click" />
					</div>

				</div>

			</div>
            <br />
		</div>
	</div>
</div>

<script>
    $(document).ready(function() {
        $(".arrow-up").hide();
        $('.filter__btn').click(function () {
            $('.filter__content').slideToggle('fast');
            $(this).find(".arrow-up, .arrow-down").toggle();
        });
    });
</script>
