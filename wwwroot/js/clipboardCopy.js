window.clipboardCopy = {
	copyText: function (text, successMessage) {
		navigator.clipboard.writeText(text).then(function () {
			alert(successMessage);
			})
			.catch(function (error) {
				alert(error);
			});
	},
	copyTextNoAlert: function (text) {
		navigator.clipboard.writeText(text)
			.catch(function (error) {
				alert(error);
			});
	}
}
