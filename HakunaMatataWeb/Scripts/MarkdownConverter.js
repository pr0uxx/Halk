$.valHooks.textarea = {
	get: function (elem) {
		return elem.value.replace(/\r?\n/g, "\r\n");
	}
};

$(document).ready(function () {
	convertMarkdown($('#Content').html());
	console.log($('#Content').text());

	$('#Content').on('change', function (e) {
		convertMarkdown();
	});
})

function convertMarkdown() {
	$.ajax({
		url: '../../Utilities/ConvertMarkdown',
		data: { markdown: $('#Content').val() },
		type: 'POST',

	})
		.done(function (data) {
			$('#preview-pane').html(data);
		});
}