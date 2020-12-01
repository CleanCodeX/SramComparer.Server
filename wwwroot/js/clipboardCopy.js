window.clipboardCopy = {
	copyText: function (text, successMessage) {
		navigator.clipboard.writeText(text).then(function () {
			alert(successMessage);
			})
			.catch(function (error) {
				alert(error);
			});
	}
}