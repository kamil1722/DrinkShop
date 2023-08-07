var balance = Math.floor(Math.random() * 51) + 100;
document.getElementById("balance").innerHTML = balance + " руб"

function SetCoin(btnCoin) {
    var selectValueCoin = document.getElementById(btnCoin).value;
    var currentValueCoin = document.getElementById("valueCoin").value;

    document.getElementById("valueCoin").value = +selectValueCoin + +currentValueCoin;
}

function getCode(id) {
    document.getElementById("codeDrink").value = id;
}

$(function () {
    $('#btnSetPay').click(function (e) {
        e.preventDefault();
        var codeDrink = $('#codeDrink').val();
        var currentBalance = $('#balance').text();

        $.ajax({
            type: 'POST',
            url: '/User/UpdateCount',
            data: { codeDrink: codeDrink },
        })
            .then(function (result) {
                console.log(result);

                return $.ajax({
                    url: '/User/GetPrice',
                    type: 'GET',
                    data: { codeDrink: codeDrink },
                });
            })
            .then(function (data) {
                var newBalance = parseInt(currentBalance) - +data;
                document.getElementById("balance").innerHTML = newBalance + " руб";

                return $.ajax({
                    url: '/User/GetCards',
                    type: 'GET',
                });
            })
            .then(function (data) {
                // Обновляем содержимое контейнера cardsContainer
                $('#cardsContainer').html(data);
            })
            .catch(function (error) {
                console.log(error);
            });
    });
});

