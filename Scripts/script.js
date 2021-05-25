function toggleFilterCollapse(filterId) {
	var filter = $("#" + filterId);

	filter.toggleClass("filter--collapsed");

	if(filter.hasClass("filter--collapsed"))
		filter.find(".filter__btn").attr("title", "Expande Filter");
	else
		filter.find(".filter__btn").attr("title", "Collapse Filter");
}