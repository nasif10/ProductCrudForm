function Toast(heading, text, icon) {
	$.toast({
		heading: heading,
		text: text,
		showHideTransition: 'slide',
		icon: icon,
		position: 'top-right',
		stack: false
	});
}
$(function () {
	$(".chosen-select").chosen();
});
