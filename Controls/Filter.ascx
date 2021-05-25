<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Filter.ascx.cs" Inherits="SurveyReporting.Controls.Filter" %>

<link href="../Scripts/htmlDatePicker.css" rel="stylesheet" />
<script src="../Scripts/htmlDatePicker.js"></script>



<div class="row">
	<div class="col-lg-6 col-md-7 col-sm-8 col-xs-12">
	
		<div id="mFilter" class="panel filter">

			<div class="filter__title">
				Filter
			</div>

			<button type="button" class="filter__btn" title="Collapse Filter">
				<i class="fa fa-chevron-up arrow-up"></i>
                <i class="fa fa-chevron-down arrow-down"></i>
			</button>

			<div class="row filter__content">
				<div class="col-xs-6">
					<div class="control-group">
						<label>Survey From</label>
						<asp:TextBox ID="SurveyFrom" runat="server" OnLoad="SurveyFrom_Load"></asp:TextBox>
					</div>

					<div class="control-group">
						<label>Survey Name</label>
						<asp:TextBox ID="SurveyName" runat="server"></asp:TextBox>
					</div>

					<div class="control-group">
						<label>Survey Type</label>
						<asp:DropDownList ID="SurveyType" runat="server" AppendDataBoundItems="True">
							<asp:ListItem Value="" Text="" />
						</asp:DropDownList>
					</div>

					<div class="control-group">
						<label>Group</label>
						<asp:DropDownList ID="Group" runat="server" AppendDataBoundItems="True">
							<asp:ListItem Value="" Text="" />
						</asp:DropDownList>
					</div>
				</div>
				<div class="col-xs-6">
					<div class="control-group">
						<label>Survey To</label>
						<asp:TextBox ID="SurveyTo" runat="server" OnLoad="SurveyTo_Load"></asp:TextBox>
						<%--<input type="text" name="SurveyTo" id="SurveyTo" style="width: 200px" onClick="GetDate(this);" />--%>
					</div>

					<div class="control-group">
						<label>Survey Status</label>
						<asp:DropDownList ID="SurveyStatus" runat="server" AppendDataBoundItems="True">
							<asp:ListItem Value="" Text="" />
						</asp:DropDownList>
					</div>

					<div class="control-group">
						<label>Project Manager</label>
						<asp:DropDownList ID="ProjectManager" runat="server" AppendDataBoundItems="True">
							<asp:ListItem Value="" Text="" />
						</asp:DropDownList>
					</div>

					<div class="control-group">
						<label>Organization Group</label>
						<asp:DropDownList ID="OrganisationUnit" runat="server" AppendDataBoundItems="True">
							<asp:ListItem Value="" Text="" />
						</asp:DropDownList>

                        <label><input ID="RollupOrgs" type="checkbox" value="" runat="server" style="display:inline;width:10%">Roll up</label>
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
