$(document).ready(function () {
	const codeInputContainer = document.getElementById("codeInputContainer");
	const confirmationDialog = document.getElementById("confirmationDialog");
	const confirmEmailLink = document.getElementById("confirmEmailLink");

	confirmEmailLink.addEventListener("click", function (event) {
		event.preventDefault();
		confirmationDialog.style.display = "block";
	});

	$("#confirmButton").click(function () {
		var email = $("#profileValueEmail").text();

		$.ajax({
			url: '/Account/SendCodeToEmail',
			type: 'POST',
			data: { email: email}, // Передаем данные здесь
			success: function (data) {
				if (data.success) {
					alert("Код подтверждения отправлен на ваш email.");
					confirmationDialog.style.display = "none";
					codeInputContainer.style.display = "block";
				} else {
					alert("Ошибка при отправке кода подтверждения: " + data.message);
				}
			},
			error: function (xhr, status, error) {
				console.error("Ошибка при отправке запроса на сервер:", xhr, status, error);
				alert("Произошла ошибка при отправке запроса на сервер: " + error);
			}
		});


		$("#cancelButton").click(function () {
			confirmationDialog.style.display = "none";
		});
	});

});