$.valHooks.textarea = {
	get: function (elem) {
		return elem.value.replace(/\r?\n/g, "\r\n");
	}
};

$(document).ready(function () {
	convertMarkdown($('#Content').html());
	//console.log($('#Content').text());

	var _changeInterval = null;
	$("#Content").on('keyup', function () {
		console.log("wait I am still typing, clear my previous Interval, If any exists")
		// wait untill user type in something
		// Don't let call setInterval - clear it, user is still typing
		clearInterval(_changeInterval)
		_changeInterval = setInterval(function () {
			console.log("User finished typing, clear interval again. We don;t want to repeat our task for indefinitely")
			// Typing finished, now you can Do whatever after 2 sec
			clearInterval(_changeInterval);
			convertMarkdown();
		}, 1000);

	});

	//$('#Content').on('change', function (e) {
	//	convertMarkdown();
	//});
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